// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpSearchClass
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;

#nullable disable
namespace Intermech.ImpExp.Search;

public abstract class PumpSearchClass : PumpClass
{
  protected SearchPlugin plugin;
  protected string SettingsName = string.Empty;

  public PumpSearchClass(SearchPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  public PumpSearchClass(SearchPlugin plugin, string settingsName)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
    this.SettingsName = settingsName;
  }
}
