// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.SettingsItems.SettingsAttributeGroupItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.SettingsItems;

/// <summary>Настройки импортируемой группы атрибутов</summary>
public class SettingsAttributeGroupItem(string longName) : 
  SettingsItem(longName),
  ISettingsAttributeGroupItem,
  ISettingsItem,
  ISettingsGroupItem
{
  protected List<ISettingsItem> settingsItems = new List<ISettingsItem>();

  public virtual string Caption => this.LongName;

  public List<ISettingsItem> SettingsItems => this.settingsItems;

  public object Tag { get; set; }

  public SettingsGroupType GroupType => SettingsGroupType.AttributeGroups;

  public void Sort()
  {
    if (this.settingsItems == null || this.settingsItems.Count <= 0)
      return;
    this.settingsItems.Sort((IComparer<ISettingsItem>) new SettingsItemComparer());
  }
}
