// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.SchDocument
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Intermech.AltiumDesigner.Interfaces;
using Intermech.Data;
using SCH;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace CSharpPlugin;

internal sealed class SchDocument : 
  Parametrable<ISch_Document>,
  ISchDocument,
  IParametrable,
  IValueBagContainer,
  IIdentification,
  IFileDocument,
  IDisposable
{
  private readonly string[] _obligatoryParameters = new string[2]
  {
    "DocumentNumber",
    "SheetNumber"
  };
  private ComponentIterator _componentsIterator;
  private SheetSymbolIterator _sheetSymbolIterator;
  private readonly IPSAddInProxy _proxy;
  private readonly string _fileName;
  private IADProject _project;

  public SchDocument(IPSAddInProxy proxy, ISch_Document document, string fileName)
    : base(document)
  {
    this._fileName = fileName;
    this._proxy = proxy;
  }

  public override void Dispose()
  {
    this._componentsIterator?.Dispose();
    this._sheetSymbolIterator?.Dispose();
    base.Dispose();
  }

  public ISchSheetSymbol GetNextSheetSymbol()
  {
    if (this._sheetSymbolIterator == null)
      this._sheetSymbolIterator = new SheetSymbolIterator(this.parametrableObject);
    return this._sheetSymbolIterator.GetNextComponent();
  }

  public ISchComponent GetNextComponent()
  {
    if (this._componentsIterator == null)
      this._componentsIterator = new ComponentIterator(this.parametrableObject, this._fileName);
    return this._componentsIterator.GetNextComponent();
  }

  public Parameter[] ObligatoryParameters
  {
    get
    {
      return SchParametersHelper.ReadParameters((ISch_BasicContainer) this.parametrableObject, this._obligatoryParameters);
    }
  }

  public override string InternalId => this._fileName;

  public string FilePath => this._fileName;

  public List<IPhysicalDocument> PhysicalDocuments()
  {
    return this.Project is CSharpPlugin.Project project ? project.GetPhysicalDocuments(this._fileName) : (List<IPhysicalDocument>) null;
  }

  public override void SetModified() => Helper.SetModified(this._fileName);

  private string ProjectFileName
  {
    get
    {
      string str = Helper.SearchFile("*.PrjPcb", Path.GetDirectoryName(this._fileName), SearchOption.TopDirectoryOnly);
      return !string.IsNullOrEmpty(str) ? str : throw new FileNotFoundException("Не найден файл проекта.", this._fileName);
    }
  }

  public IADProject Project
  {
    get
    {
      if (this._project == null)
        this._project = this._proxy.GetProject(this.ProjectFileName);
      return this._project;
    }
  }

  protected override Parameter[] GetParameters()
  {
    return SchParametersHelper.ReadParameters((ISch_BasicContainer) this.parametrableObject);
  }

  protected override void WriteNewParameter(Parameter parameter)
  {
    SchParametersHelper.WriteNewParameter((ISch_BasicContainer) this.parametrableObject, parameter);
  }

  protected override void WriteParameterValue(Parameter parameter)
  {
    SchParametersHelper.WriteParameterValue((ISch_BasicContainer) this.parametrableObject, parameter);
  }
}
