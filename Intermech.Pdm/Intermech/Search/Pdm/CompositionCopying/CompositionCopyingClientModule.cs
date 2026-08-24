// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionCopying.CompositionCopyingClientModule
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using System;

#nullable disable
namespace Intermech.Search.Pdm.CompositionCopying;

internal sealed class CompositionCopyingClientModule
{
  private ICompositionCopyingDispatcherService _compositionCopyingDispatcherService;
  private CompositionCopyingDispatcherHandler _compositionCopyingDispatcherHandler;

  public CompositionCopyingClientModule(
    ICompositionCopyingDispatcherService compositionCopyingDispatcherService,
    CompositionCopyingDispatcherHandler compositionCopyingDispatcherHandler)
  {
    if (compositionCopyingDispatcherService == null)
      throw new ArgumentNullException(nameof (compositionCopyingDispatcherService));
    if (compositionCopyingDispatcherHandler == null)
      throw new ArgumentNullException(nameof (compositionCopyingDispatcherHandler));
    this._compositionCopyingDispatcherService = compositionCopyingDispatcherService;
    this._compositionCopyingDispatcherHandler = compositionCopyingDispatcherHandler;
  }

  public void Load()
  {
    this._compositionCopyingDispatcherService.FindBySelectedItems += new EventHandler<FindCompositionCopyingHandlerEventArgs>(this._compositionCopyingDispatcherHandler.FindHandlerBySelectedItems);
  }

  public void Unload()
  {
    this._compositionCopyingDispatcherService.FindBySelectedItems -= new EventHandler<FindCompositionCopyingHandlerEventArgs>(this._compositionCopyingDispatcherHandler.FindHandlerBySelectedItems);
  }
}
