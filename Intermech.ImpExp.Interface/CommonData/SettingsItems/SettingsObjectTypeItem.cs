// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.SettingsItems.SettingsObjectTypeItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.SettingsItems;

public class SettingsObjectTypeItem : 
  SettingsItem,
  ISettingsObjectTypeItem,
  ISettingsItem,
  ISettingsGroupItem
{
  protected List<ISettingsItem> settingsItems = new List<ISettingsItem>();
  public SettingsObjectTypeItem ParentItem;
  protected string shortName = "";
  protected int docType;

  public SettingsObjectTypeItem(string longName, string shortName, int docType)
    : base(longName)
  {
    this.shortName = shortName;
    this.docType = docType;
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

  public string Caption
  {
    get => this.LongName + (this.shortName.Equals("") ? "" : $" [{this.shortName}]");
  }

  public List<ISettingsItem> SettingsItems => this.settingsItems;

  public object Tag { get; set; }

  public int ID => this.docType;

  public void Sort()
  {
    if (this.settingsItems == null || this.settingsItems.Count <= 0)
      return;
    this.settingsItems.Sort((IComparer<ISettingsItem>) new SettingsItemComparer());
  }
}
