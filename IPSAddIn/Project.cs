// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Project
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using EDP;
using Intermech.AltiumDesigner.Interfaces;
using Intermech.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

#nullable disable
namespace CSharpPlugin;

internal class Project : 
  Parametrable<IProject>,
  IADProject,
  IADBaseProject,
  IParametrable,
  IValueBagContainer,
  IIdentification,
  IFileDocument,
  IDisposable
{
  private List<DocumentInfo> _documents;
  protected readonly IPSAddInProxy proxy;

  public Project(IPSAddInProxy owner, IProject project)
    : base(project)
  {
    this.proxy = owner;
  }

  public List<IPhysicalDocument> GetPhysicalDocuments(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      throw new ArgumentNullException(nameof (fileName));
    List<IPhysicalDocument> physicalDocuments = new List<IPhysicalDocument>();
    string fileName1 = Path.GetFileName(fileName);
    for (int argIndex = 0; argIndex < this.parametrableObject.DM_PhysicalDocumentCount(); ++argIndex)
    {
      IDocument document = this.parametrableObject.DM_PhysicalDocuments(argIndex);
      if (document.DM_DocumentKind() == "SCH" && fileName1.Equals(document.DM_FileName(), StringComparison.OrdinalIgnoreCase))
      {
        SchemaDocumentInfo schDoc = this.GetDocuments(true).FirstOrDefault<DocumentInfo>((Func<DocumentInfo, bool>) (f => f.FullPath == document.DM_FullPath() && f is SchemaDocumentInfo)) as SchemaDocumentInfo;
        physicalDocuments.Add((IPhysicalDocument) new PhysicalDocument(document, this.proxy, schDoc));
      }
    }
    return physicalDocuments;
  }

  public List<DocumentInfo> GetDocuments(bool leaveDocsOpen)
  {
    if (this._documents == null)
    {
      string str1 = this.parametrableObject.DM_ProjectFileName();
      int capacity = this.parametrableObject.DM_LogicalDocumentCount();
      this._documents = new List<DocumentInfo>(capacity);
      for (int argIndex = 0; argIndex < capacity; ++argIndex)
      {
        IDocument o = this.parametrableObject.DM_LogicalDocuments(argIndex);
        try
        {
          if (!(o.DM_FileName() == str1))
          {
            string kind = o.DM_DocumentKind();
            if (kind == "SCH")
            {
              string str2 = o.DM_FullPath();
              bool flag = o.DM_DocumentIsLoaded();
              using (ISchDocument schDocument = this.proxy.GetSchDocument(str2, !flag))
                this._documents.Add((DocumentInfo) new SchemaDocumentInfo(str2, schDocument.ObligatoryParameters));
              if (!flag)
              {
                if (!leaveDocsOpen)
                  this.proxy.CloseObject(str2);
              }
            }
            else
              this._documents.Add(new DocumentInfo(o.DM_FullPath(), kind));
          }
        }
        finally
        {
          Marshal.FinalReleaseComObject((object) o);
        }
      }
    }
    return this._documents;
  }

  public List<DocumentInfo> GeneratedDocuments
  {
    get
    {
      int capacity = this.parametrableObject.DM_GeneratedDocumentCount();
      List<DocumentInfo> generatedDocuments = new List<DocumentInfo>(capacity);
      for (int argIndex = 0; argIndex < capacity; ++argIndex)
      {
        IDocument o = this.parametrableObject.DM_GeneratedDocuments(argIndex);
        try
        {
          generatedDocuments.Add(new DocumentInfo(o.DM_FullPath(), o.DM_DocumentKind()));
        }
        finally
        {
          Marshal.FinalReleaseComObject((object) o);
        }
      }
      return generatedDocuments;
    }
  }

  protected override Parameter[] GetParameters()
  {
    List<Parameter> parameterList = new List<Parameter>();
    int num = this.parametrableObject.DM_ParameterCount();
    for (int argIndex = 0; argIndex < num; ++argIndex)
    {
      IParameter parameter = this.parametrableObject.DM_Parameters(argIndex);
      parameterList.Add(ProjectParametersHelper.GetParameter(parameter));
    }
    return parameterList.ToArray();
  }

  protected override void WriteNewParameter(Parameter parameter)
  {
    this.parametrableObject.DM_AddParameter(parameter.Name, Convert.ToString(parameter.Value));
  }

  protected override void WriteParameterValue(Parameter parameter)
  {
    for (int argIndex = 0; argIndex < this.parametrableObject.DM_ParameterCount(); ++argIndex)
    {
      IParameter parameter1 = this.parametrableObject.DM_Parameters(argIndex);
      if (parameter1.DM_Name().Equals(parameter.Name))
      {
        parameter1.DM_SetValue(Convert.ToString(parameter.Value));
        break;
      }
    }
  }

  public string FilePath => this.parametrableObject.DM_ProjectFullPath();

  public int VariantsCount => this.parametrableObject.DM_ProjectVariantCount();

  public IVariant GetVariant(int index)
  {
    return (IVariant) new Variant(this.parametrableObject.DM_ProjectVariants(index));
  }

  public override string InternalId => this.parametrableObject.DM_ProjectFileName();

  public override void Dispose()
  {
    if (this.parametrableObject == null)
      return;
    Marshal.FinalReleaseComObject((object) this.parametrableObject);
  }
}
