// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.BackgroundReader
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Contexts;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator.Queries;
using Intermech.Pdm.Compositions.ContainsBase;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class BackgroundReader : BaseBackgroundReader
{
  private bool _inProduct;
  private long _objectID;
  private long _schemeID;
  private string _filtrationOwnerID;
  private CurrentEditingContext _editingContext;

  public BackgroundReader(IServiceProvider services)
    : base(services)
  {
    this._editingContext = CurrentEditingContext.Dummy;
  }

  public CurrentEditingContext EditingContext
  {
    [DebuggerStepThrough] get => this._editingContext;
    set
    {
      this._editingContext = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public void Execute(
    RecordMapping mapping,
    long objectID,
    long schemeID,
    string filtrationOwnerID)
  {
    this.Execute(mapping, objectID, schemeID, false, filtrationOwnerID);
  }

  public void Execute(
    RecordMapping mapping,
    long objectID,
    long schemeID,
    bool inProduct,
    string filtrationOwnerID)
  {
    this.Mapping = mapping;
    this._objectID = objectID;
    this._schemeID = schemeID;
    this.scheme = (RuntimeSearchScheme) null;
    this._inProduct = inProduct;
    this._filtrationOwnerID = filtrationOwnerID;
    Random random = new Random();
    this.thread = new Thread(this._editingContext.SendToThread(new ThreadStart(this.StartThread)));
    this.thread.Name = $"Contains_{random.Next(10000)}";
    this.thread.IsBackground = true;
    this.thread.Start();
    this.ChangeState(BackgroundState.Reading);
  }

  public void Execute(
    RecordMapping mapping,
    long objectID,
    RuntimeSearchScheme scheme,
    string filtrationOwnerID)
  {
    this.Mapping = mapping;
    this._objectID = objectID;
    this._schemeID = -1L;
    this.scheme = scheme;
    this._filtrationOwnerID = filtrationOwnerID;
    Random random = new Random();
    this.thread = new Thread(this._editingContext.SendToThread(new ThreadStart(this.StartThread)))
    {
      Name = $"Contains_{Guid.NewGuid()}",
      IsBackground = true
    };
    this.thread.Start();
    this.ChangeState(BackgroundState.Reading);
  }

  public DataTable CorrectDataTableFromMapping(RecordMapping mapping)
  {
    bool flag = false;
    if (mapping.Fields.Length != this.Mapping.Fields.Length)
      throw new Exception("Не совпадают количество новых и старых колонок!");
    for (int index = 0; index < mapping.Fields.Length; ++index)
    {
      if (((NodeColumnID) mapping.Fields[index]).AttributeID != ((NodeColumnID) this.Mapping.Fields[index]).AttributeID)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return this.queryResult;
    DataTable dataTable = new DataTable(this.QueryResult.TableName);
    int length = mapping.Fields.Length;
    int[] numArray = new int[length];
    for (int index1 = 0; index1 < length; ++index1)
    {
      NodeColumnID field = (NodeColumnID) mapping.Fields[index1];
      DataColumn dataColumn = (DataColumn) null;
      for (int index2 = 0; index2 < length; ++index2)
      {
        if (field.AttributeID.Equals(((NodeColumnID) this.Mapping.Fields[index2]).AttributeID))
        {
          numArray[index1] = index2;
          dataColumn = this.queryResult.Columns[index2];
          break;
        }
      }
      DataColumn column = new DataColumn(index1.ToString(), dataColumn.DataType);
      dataTable.Columns.Add(column);
    }
    foreach (DataRow row1 in (InternalDataCollectionBase) this.queryResult.Rows)
    {
      DataRow row2 = dataTable.NewRow();
      for (int columnIndex = 0; columnIndex < length; ++columnIndex)
        row2[columnIndex] = row1[numArray[columnIndex]];
      dataTable.Rows.Add(row2);
    }
    this.Mapping = mapping;
    this.queryResult = dataTable;
    return dataTable;
  }

  private void StartThread()
  {
    try
    {
      HybridDictionary PluginsData = new HybridDictionary();
      if (ServicesManager.GetService(typeof (IClientPluginsService)) is IClientPluginsService service1)
        service1.GetClientPluginsData(ref PluginsData);
      RelationPair service2 = this.Services != null ? this.Services.GetService(typeof (RelationPair)) as RelationPair : (RelationPair) null;
      if (service2 != null && !service2.Empty)
        PluginsData[(object) "{78D53C74-3CF7-4F48-94FC-80C4FCB0BA77}"] = (object) service2;
      PluginsData[(object) "{7FB30639-2F65-4407-B78E-523547B1B133}"] = (object) true;
      List<ColumnDescriptor> columns = Helper.FormingColumns(this.Mapping);
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        ICompositionService customService = (ICompositionService) sessionKeeper.Session.GetCustomService(typeof (ICompositionService));
        List<ConditionStructure> filterConditions = (List<ConditionStructure>) null;
        if (this._schemeID != -1L)
        {
          IDBAttribute attributeByGuid = sessionKeeper.Session.GetObject(this._schemeID).GetAttributeByGuid(new Guid("cad00621-306c-11d8-b4e9-00304f19f545"));
          if (attributeByGuid.AsInteger != 0L)
            filterConditions = new List<ConditionStructure>((IEnumerable<ConditionStructure>) (ServicesManager.GetService(typeof (ISelectionsService)) as ISelectionsService).GetConditionStructures((object) sessionKeeper.Session, attributeByGuid.AsInteger));
        }
        if (this.scheme == null)
          this.queryResult = customService.Select(sessionKeeper.Session.SessionGUID, this._objectID, this._schemeID, filterConditions, columns, this.selectGuid, this._filtrationOwnerID, PluginsData);
        else
          this.queryResult = customService.Select(sessionKeeper.Session.SessionGUID, this._objectID, this.scheme, columns, this.selectGuid, this._filtrationOwnerID, PluginsData);
        CompositionInfo info;
        while (true)
        {
          info = customService.GetInfo(this.selectGuid);
          if (info != null)
          {
            if (!info.ErrorPresent)
            {
              if (info.Percent != 100)
              {
                this.ChangeState(BackgroundState.SetPersent, info.Percent);
                Thread.Sleep(1000);
              }
              else
                goto label_15;
            }
            else
              break;
          }
          else
            goto label_17;
        }
        this.ChangeState(BackgroundState.Error);
        ExceptionHelper.ExceptionService.ShowException(info.ErrorException);
        goto label_17;
label_15:
        this.queryResult = info.Result as DataTable;
        this.ChangeState(BackgroundState.Fill);
label_17:
        this.state = BackgroundState.Empty;
      }
    }
    catch (Exception ex)
    {
      if (ex is ThreadAbortException)
        return;
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
  }
}
