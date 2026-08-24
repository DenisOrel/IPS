// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.ItemBindChangedEventArgs
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.CommonData.SettingsItems;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData;

/// <summary>Аргументы события об изменении привязки</summary>
public class ItemBindChangedEventArgs
{
  /// <summary>Группа настроек</summary>
  public ISettingsGroup Group { get; private set; }

  /// <summary>Настройки метаданного</summary>
  public ISettingsItem Item { get; private set; }

  public ItemBindChangedEventArgs(ISettingsGroup group, ISettingsItem item)
  {
    this.Group = group;
    this.Item = item;
  }
}
