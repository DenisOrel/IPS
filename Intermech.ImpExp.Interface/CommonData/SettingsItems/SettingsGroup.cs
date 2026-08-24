// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.SettingsItems.SettingsGroup
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.SettingsItems;

/// <summary>
/// Базовый класс для реализации группы настройки атрибутов
/// </summary>
public class SettingsGroup : ISettingsGroup
{
  protected string caption = string.Empty;
  protected bool visible = true;
  protected SettingsGroupType groupType;
  protected List<ISettingsGroupItem> groupItems = new List<ISettingsGroupItem>();

  public SettingsGroup(string caption, SettingsGroupType groupType)
  {
    this.caption = caption;
    this.groupType = groupType;
  }

  public string Caption
  {
    get => this.caption;
    set
    {
      if (!(this.caption != value))
        return;
      this.caption = value;
    }
  }

  public event ObjectCreatedEventHandler ObjectCreated;

  public List<ISettingsGroupItem> GroupItems => this.groupItems;

  public bool Visible
  {
    get => this.visible;
    set => this.visible = value;
  }

  public SettingsGroupType GroupType => this.groupType;

  public void DoObjectCreated()
  {
    ObjectCreatedEventHandler objectCreated = this.ObjectCreated;
    if (objectCreated == null)
      return;
    objectCreated();
  }

  public void Sort()
  {
    if (this.groupItems == null || this.groupItems.Count <= 0)
      return;
    this.groupItems.Sort((IComparer<ISettingsGroupItem>) new SettingsGroupItemComparer());
  }
}
