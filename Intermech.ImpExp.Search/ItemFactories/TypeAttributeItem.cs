// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.TypeAttributeItem
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class TypeAttributeItem : ITypeAttributeItem
{
  private int typeID;
  private string dbFieldName;
  private string attributeName;
  private FieldTypes attributeType;
  private int attributeSize;
  private object defaultValue;
  private Guid guid;
  private Guid createObjTypeGUID = Guid.Empty;

  public TypeAttributeItem(
    int typeID,
    string dbFieldName,
    string attributeName,
    FieldTypes attributeType)
    : this(typeID, dbFieldName, attributeName, attributeType, 0, (object) null)
  {
  }

  public TypeAttributeItem(
    int typeID,
    string dbFieldName,
    string attributeName,
    FieldTypes attributeType,
    int attributeSize)
    : this(typeID, dbFieldName, attributeName, attributeType, attributeSize, (object) null)
  {
  }

  public TypeAttributeItem()
  {
  }

  public TypeAttributeItem(
    int typeID,
    string dbFieldName,
    string attributeName,
    FieldTypes attributeType,
    int attributeSize,
    object defaultValue)
  {
    this.TypeID = typeID;
    this.DBFieldName = dbFieldName;
    this.AttributeName = attributeName;
    this.AttributeType = attributeType;
    this.AttributeSize = attributeSize;
    this.DefaultValue = defaultValue;
    this.guid = Guid.NewGuid();
  }

  public int TypeID
  {
    get => this.typeID;
    set => this.typeID = value;
  }

  public string DBFieldName
  {
    get => this.dbFieldName;
    set => this.dbFieldName = value;
  }

  public string AttributeName
  {
    get => this.attributeName;
    set => this.attributeName = value;
  }

  public FieldTypes AttributeType
  {
    get => this.attributeType;
    set => this.attributeType = value;
  }

  public int AttributeSize
  {
    get => this.attributeSize;
    set => this.attributeSize = value;
  }

  public object DefaultValue
  {
    get => this.defaultValue;
    set => this.defaultValue = value;
  }

  public Guid GUID
  {
    get => this.guid;
    set => this.guid = value;
  }

  public Guid CreateObjTypeGUID
  {
    get => this.createObjTypeGUID;
    set => this.createObjTypeGUID = value;
  }
}
