// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.SettingsItems.SettingsGroupItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.SettingsItems;

/// <summary>
/// Базовый класс для реализации элемента группы настройки атрибутов
/// </summary>
public class SettingsGroupItem : ISettingsGroupItem
{
  protected string caption = string.Empty;
  protected List<ISettingsItem> settingsItems = new List<ISettingsItem>();

  public SettingsGroupItem(string caption) => this.caption = caption;

  public virtual string Caption
  {
    get => this.caption;
    set
    {
      if (!(this.caption != value))
        return;
      this.caption = value;
    }
  }

  public List<ISettingsItem> SettingsItems => this.settingsItems;

  public void Sort()
  {
  }

  public object Tag { get; set; }
}
