// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetTemplate
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Document.Client;
using Intermech.Document.Model;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Search.Interfaces;
using System;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Класс обслуживания бланка Удостоверяющего листа</summary>
public class CertSheetTemplate
{
  /// <summary>Id объекта бланка</summary>
  private long certSheetBlankObjectId = CertSheetTemplate.GetCertSheetBlankId();
  /// <summary>Выполнена ли проверка правильности бланка</summary>
  private bool fieldsChecked;
  private ImDocument template;
  private TableElement template_certSheet_Top_Table;
  private Page template_page;
  private TableElement template_docHeader;
  private TableElement template_docBody;
  private TableElement template_empty1;
  private TableElement template_fileHeader;
  private TableElement template_empty2;
  private TableElement template_fileNotes;
  private TableElement template_empty3;
  private TableElement template_signsBody;
  private TableElement template_empty4;

  /// <summary>Бланк</summary>
  private ImDocument Template => this.template;

  public TableElement Template_certSheet_Top_Table => this.template_certSheet_Top_Table;

  public Page Template_page => this.template_page;

  public TableElement Template_docHeader => this.template_docHeader;

  public TableElement Template_docBody => this.template_docBody;

  public TableElement Template_empty1 => this.template_empty1;

  public TableElement Template_fileHeader => this.template_fileHeader;

  public TableElement Template_empty2 => this.template_empty2;

  public TableElement Template_fileNotes => this.template_fileNotes;

  public TableElement Template_empty3 => this.template_empty3;

  public TableElement Template_signsBody => this.template_signsBody;

  public TableElement Template_empty4 => this.template_empty4;

  /// <summary>Вернуть id бланка УЛ</summary>
  /// <returns>-1, если не назначен</returns>
  private static long GetCertSheetBlankId()
  {
    long certSheetBlankId = -1;
    string g = (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).ReadString("CLIENT", "CERTSHEETS", "BLANKGUID", CertSheetHolder.DefaultParamBlankGuid, DBConfigMode.GlobalOnly);
    if (g.Trim() != string.Empty)
    {
      QuickObjectInfo objectInfo = ApplicationServices.Container.GetService<IObjectsInfoCache>().GetObjectInfo(new Guid(g));
      if (!objectInfo.Empty)
        certSheetBlankId = objectInfo.ObjectID;
    }
    return certSheetBlankId;
  }

  /// <summary>Загрузка темплета</summary>
  /// <returns></returns>
  public bool LoadTemplate()
  {
    this.fieldsChecked = false;
    try
    {
      this.template = DocumentEditorPlugin.LoadDocumentFromDBObject(this.certSheetBlankObjectId);
    }
    catch
    {
      this.template = (ImDocument) null;
    }
    return this.template != null;
  }

  /// <summary>Анализ наличия полей бланка</summary>
  /// <param name="errorField"></param>
  /// <returns></returns>
  public bool CheckFields(out string errorField)
  {
    this.fieldsChecked = false;
    errorField = string.Empty;
    this.template_certSheet_Top_Table = this.template.FindNode(CertSheetConsts.CertSheet_Top_Table) as TableElement;
    if (this.template_certSheet_Top_Table == null)
    {
      errorField = CertSheetConsts.CertSheet_Top_Table;
      return false;
    }
    this.template_page = this.template_certSheet_Top_Table.Page as Page;
    if (this.template_page == null)
    {
      errorField = CertSheetConsts.Page;
      return false;
    }
    this.template_docHeader = this.template.FindNode(CertSheetConsts.Doc_Header) as TableElement;
    if (this.template_docHeader == null)
    {
      errorField = CertSheetConsts.Doc_Header;
      return false;
    }
    this.template_docBody = this.template.FindNode(CertSheetConsts.Doc_Body) as TableElement;
    if (this.template_docBody == null)
    {
      errorField = CertSheetConsts.Doc_Body;
      return false;
    }
    this.template_empty1 = this.template.FindNode(CertSheetConsts.Empty1) as TableElement;
    if (this.template_empty1 == null)
    {
      errorField = CertSheetConsts.Empty1;
      return false;
    }
    this.template_fileHeader = this.template.FindNode(CertSheetConsts.File_Header) as TableElement;
    if (this.template_fileHeader == null)
    {
      errorField = CertSheetConsts.File_Header;
      return false;
    }
    this.template_empty2 = this.template.FindNode(CertSheetConsts.Empty2) as TableElement;
    if (this.template_empty2 == null)
    {
      errorField = CertSheetConsts.Empty2;
      return false;
    }
    this.template_fileNotes = this.template.FindNode(CertSheetConsts.File_Notes) as TableElement;
    if (this.template_fileNotes == null)
    {
      errorField = CertSheetConsts.File_Notes;
      return false;
    }
    this.template_empty3 = this.template.FindNode(CertSheetConsts.Empty3) as TableElement;
    if (this.template_empty3 == null)
    {
      errorField = CertSheetConsts.Empty3;
      return false;
    }
    this.template_signsBody = this.template.FindNode(CertSheetConsts.Signs_Body) as TableElement;
    if (this.template_signsBody == null)
    {
      errorField = CertSheetConsts.Signs_Body;
      return false;
    }
    this.template_empty4 = this.template.FindNode(CertSheetConsts.Empty4) as TableElement;
    if (this.template_empty4 == null)
    {
      errorField = CertSheetConsts.Empty4;
      return false;
    }
    this.fieldsChecked = true;
    return true;
  }

  public ImDocument CreateDocument() => new ImDocument(this.template, true, true);

  internal TableElement Get_Doc_Header()
  {
    return (TableElement) this.Template_docHeader.CloneFromTemplate(true, true);
  }

  internal TableElement Get_Doc_Body()
  {
    return (TableElement) this.Template_docBody.CloneFromTemplate(true, true);
  }

  internal TableElement Get_Empty(string blockName)
  {
    TableElement tableElement = (TableElement) null;
    if (blockName == CertSheetConsts.Empty1)
      tableElement = this.Template_empty1;
    if (blockName == CertSheetConsts.Empty2)
      tableElement = this.Template_empty2;
    if (blockName == CertSheetConsts.Empty3)
      tableElement = this.Template_empty3;
    if (blockName == CertSheetConsts.Empty4)
      tableElement = this.Template_empty4;
    return tableElement == null ? (TableElement) null : (TableElement) tableElement.CloneFromTemplate(true, true);
  }

  internal TableElement Get_File_Header()
  {
    return (TableElement) this.Template_fileHeader.CloneFromTemplate(true, true);
  }

  internal TableElement Get_File_Notes()
  {
    return (TableElement) this.Template_fileNotes.CloneFromTemplate(true, true);
  }

  internal TableElement Get_Signs_Body()
  {
    return (TableElement) this.Template_signsBody.CloneFromTemplate(true, true);
  }

  internal Page Get_New_Page() => this.Template_page.CloneFromTemplate() as Page;
}
