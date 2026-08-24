// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseFieldsSettings.ImbaseTableRecordTypeItem
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Imbase.ImbaseFieldsSettings;

internal class ImbaseTableRecordTypeItem
{
  protected Guid guid = Guid.Empty;
  protected string name = "";
  protected string objectsName = "";
  protected string shortName = "";
  protected bool isNew = true;
  public IImTablesItem tableItem;

  public ImbaseTableRecordTypeItem(
    string name,
    string objectsName,
    string shortName,
    IImTablesItem tabItem,
    Guid guid)
  {
    this.guid = guid;
    this.name = name;
    this.objectsName = objectsName;
    this.shortName = shortName;
    this.isNew = true;
    this.tableItem = tabItem;
    this.tableItem.RecordsTypeGuid = this.guid;
  }

  public void CorrectInternalFields(IObjectTypeItem objectTypeItem)
  {
    this.guid = objectTypeItem.GUID;
    this.name = objectTypeItem.Name;
    this.objectsName = objectTypeItem.ObjectName;
    this.shortName = objectTypeItem.ShortName;
    this.isNew = true;
    this.tableItem.RecordsTypeGuid = this.guid;
  }

  [DisplayName("Глобальный идентификатор")]
  public Guid GUID => this.guid;

  [DisplayName("Наименование")]
  public string Name => this.name;

  [DisplayName("Наименование созданных объектов данного типа")]
  public string ObjectsName => this.objectsName;

  [DisplayName("Краткое наименование")]
  public string ShortName => this.shortName;

  [DisplayName("Новый тип")]
  public bool IsNew => this.isNew;
}
