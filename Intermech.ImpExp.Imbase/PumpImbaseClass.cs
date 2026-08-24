// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpImbaseClass
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal abstract class PumpImbaseClass : PumpClass
{
  protected ImbasePlugin plugin;

  public PumpImbaseClass(ImbasePlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }
}
