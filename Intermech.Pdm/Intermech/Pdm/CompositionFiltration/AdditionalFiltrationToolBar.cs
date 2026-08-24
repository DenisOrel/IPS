// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.CompositionFiltration.AdditionalFiltrationToolBar
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace Intermech.Pdm.CompositionFiltration;

internal sealed class AdditionalFiltrationToolBar : IClientPluginsDataTransfer
{
  private List<ICompositionFiltrationCommand> _filtrationCommand;

  public AdditionalFiltrationToolBar(
    List<ICompositionFiltrationCommand> filtrationCommand,
    Guid registerGuid)
  {
    this._filtrationCommand = filtrationCommand;
    this.PluginGuid = registerGuid != Guid.Empty ? registerGuid : Guid.NewGuid();
    this.Registered = registerGuid != Guid.Empty;
  }

  public bool Registered { get; }

  public Guid PluginGuid { get; }

  public void PutPluginData(HybridDictionary tag)
  {
    foreach (ICompositionFiltrationCommand filtrationCommand in this._filtrationCommand)
      filtrationCommand.OnPutPluginData(tag);
  }

  public void GetPluginData(HybridDictionary tag)
  {
    foreach (ICompositionFiltrationCommand filtrationCommand in this._filtrationCommand)
      filtrationCommand.OnGetPluginData(tag);
  }
}
