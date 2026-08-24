// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.SettingsItems.SettingsAttributeTypeItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.SettingsItems;

/// <summary>Настройки импортируемого типа атрибута</summary>
public class SettingsAttributeTypeItem : 
  SettingsItem,
  ISettingsAttributeTypeItem,
  ISettingsItem,
  ISettingsGroupItem
{
  protected string shortName = "";
  protected string alias = "";
  protected FieldTypes fieldType;
  protected int valueMaxLength;
  protected List<ISettingsItem> settingsItems = new List<ISettingsItem>();
  protected bool existsInBase = true;

  public SettingsAttributeTypeItem(
    string longName,
    string shortName,
    string alias,
    FieldTypes fieldType)
    : base(longName)
  {
    this.shortName = shortName;
    this.fieldType = fieldType;
    this.alias = alias;
  }

  public string ShortName
  {
    get => this.shortName;
    set
    {
      if (!(this.shortName != value))
        return;
      this.shortName = value;
    }
  }

  public string Alias
  {
    get => this.alias;
    set
    {
      if (!(this.alias != value))
        return;
      this.alias = value;
    }
  }

  public FieldTypes FieldType
  {
    get => this.fieldType;
    set
    {
      if (this.fieldType == value)
        return;
      this.fieldType = value;
    }
  }

  public int ValueMaxLength
  {
    get => this.valueMaxLength;
    set
    {
      if (this.valueMaxLength == value)
        return;
      this.valueMaxLength = value;
    }
  }

  public bool ExistsInBase
  {
    get => this.existsInBase;
    set
    {
      if (this.existsInBase == value)
        return;
      this.existsInBase = value;
    }
  }

  public string Caption => this.LongName;

  public List<ISettingsItem> SettingsItems => this.settingsItems;

  public object Tag { get; set; }

  public SettingsGroupType GroupType => SettingsGroupType.AttributeTypes;

  public void Sort()
  {
    if (this.settingsItems == null || this.settingsItems.Count <= 0)
      return;
    this.settingsItems.Sort((IComparer<ISettingsItem>) new SettingsItemComparer());
  }
}
