// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ImportMetadataThread
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Briefcase;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal class ImportMetadataThread
{
  private Guid _numOfBriefcase;
  private DataSet _metadata;
  private DataSet _importingList;
  private IgnoringErrors _ignoringErrors;
  private IDBImporter _dbImporter;
  private ImportMetadataForm _form;
  public bool Result = true;
  public bool EndImport;

  public ImportMetadataThread(
    IDBImporter dbImporter,
    Guid NumOfBriefcase,
    DataSet Metadata,
    DataSet ImportingList,
    IgnoringErrors ignoringErrors)
  {
    this._dbImporter = dbImporter;
    this._numOfBriefcase = NumOfBriefcase;
    this._metadata = Metadata;
    this._importingList = ImportingList;
    this._ignoringErrors = ignoringErrors;
  }

  public bool Import()
  {
    Thread thread = this.StartImportThread();
    this._form = new ImportMetadataForm();
    this.StartFormThread();
    try
    {
      BriefcaseImportProgress progress;
      do
      {
        do
        {
          progress = this._dbImporter.GetProgress(this._numOfBriefcase);
        }
        while (progress == null);
        this._form.SetPercent(progress.Percent);
        if (progress.Operation == OperationType.TerminateCurrent)
          goto label_5;
      }
      while (progress.Operation != OperationType.Error);
      throw progress.ErrorException;
label_5:
      thread.Join();
      IDataWriter service = ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter;
      List<AttributeTypePossibleValues> valuesAttributeType = this._dbImporter.PossibleValuesAttributeType;
      Hashtable importingObjectIDs = new Hashtable();
      foreach (AttributeTypePossibleValues typePossibleValues in valuesAttributeType)
      {
        foreach (KeyValuePair<int, object[]> keyValuePair in typePossibleValues.Values)
        {
          if (!importingObjectIDs.ContainsKey(keyValuePair.Value[0]))
            importingObjectIDs.Add(keyValuePair.Value[0], keyValuePair.Value[0]);
        }
      }
      if (importingObjectIDs.Count > 0 && !this._dbImporter.SetPossibleValues(valuesAttributeType, importingObjectIDs))
        service.AppManager.AddWarningMessage("Ошибка при восстановлении допустимых значений для ссылочных типов атрибутов. См. серверный лог.");
      List<SaveImportValues> measureValueObjectLink = this._dbImporter.MeasureValueObjectLink;
      importingObjectIDs.Clear();
      for (int index = 0; index < measureValueObjectLink.Count; ++index)
      {
        if (measureValueObjectLink[index].Value != null && !importingObjectIDs.ContainsKey(measureValueObjectLink[index].Value))
          importingObjectIDs.Add(measureValueObjectLink[index].Value, measureValueObjectLink[index].Value);
      }
      if (importingObjectIDs.Count > 0 && !this._dbImporter.SetMeasureValues(measureValueObjectLink, importingObjectIDs))
        service.AppManager.AddWarningMessage("Ошибка при восстановлении значений SizeType для типов атрибутов, выраженных в ед.измерения. См. серверный лог.");
      List<SaveImportValues> defaultValueObjectLink = this._dbImporter.DefaultValueObjectLink;
      importingObjectIDs.Clear();
      for (int index = 0; index < defaultValueObjectLink.Count; ++index)
      {
        if (defaultValueObjectLink[index].Value != null && !importingObjectIDs.ContainsKey(defaultValueObjectLink[index].Value))
          importingObjectIDs.Add(defaultValueObjectLink[index].Value, defaultValueObjectLink[index].Value);
      }
      if (importingObjectIDs.Count > 0)
      {
        if (!this._dbImporter.SetDefaultValues(defaultValueObjectLink, importingObjectIDs))
          service.AppManager.AddWarningMessage("Ошибка при восстановлении значений по-умолчанию для ссылочных типов атрибутов. См. серверный лог.");
      }
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
    finally
    {
      this._form.CloseForm();
      this._form = (ImportMetadataForm) null;
    }
    return this.Result;
  }

  private Thread StartFormThread()
  {
    Thread thread = new Thread(new ThreadStart(this.threadFormMethod));
    thread.IsBackground = true;
    thread.Name = $"CheckState_{this._numOfBriefcase}";
    thread.Start();
    return thread;
  }

  private void threadFormMethod()
  {
    int num = (int) this._form.ShowDialog();
  }

  private Thread StartImportThread()
  {
    Thread thread = new Thread(new ThreadStart(this.threadMethod));
    thread.IsBackground = true;
    thread.Name = $"Import_{this._numOfBriefcase}";
    thread.Start();
    return thread;
  }

  private void threadMethod()
  {
    try
    {
      this.Result = this._dbImporter.ImportMetadata(this._numOfBriefcase, this._metadata, this._importingList, this._ignoringErrors);
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
  }
}
