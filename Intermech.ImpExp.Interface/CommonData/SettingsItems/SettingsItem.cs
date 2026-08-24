// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.SettingsItems.SettingsItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.SettingsItems;

/// <summary>
/// Базовый класс для реализации элемента настройки на атрибут
/// </summary>
public class SettingsItem : ISettingsItem
{
  protected string longName = "";
  protected Guid attrGuid = Guid.Empty;
  protected int attrSystemId;

  public SettingsItem(string longName) => this.longName = longName;

  public string LongName
  {
    get => this.longName;
    set
    {
      if (!(this.longName != value))
        return;
      this.longName = value;
    }
  }

  public Guid AttrGuid
  {
    get => this.attrGuid;
    set
    {
      if (!(this.attrGuid != value))
        return;
      this.attrGuid = value;
    }
  }

  public int AttrSystemId
  {
    get => this.attrSystemId;
    set
    {
      if (this.attrSystemId == value)
        return;
      this.attrSystemId = value;
    }
  }

  public ItemError Error { get; set; }
}
