// Decompiled with JetBrains decompiler
// Type: Intermech.SearchAPI.SearchAPIPlugin
// Assembly: Intermech.SearchAPI, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D1D502F5-7810-48B3-B639-4FF6D7A8DD6F
// Assembly location: D:\IPS\Client\Intermech.SearchAPI.dll

using Intermech.Interfaces.Plugins;
using Intermech.Runtime.ComInterop.LocalServer;
using System;

#nullable disable
namespace Intermech.SearchAPI;

internal sealed class SearchAPIPlugin : IPackage
{
  internal static IServiceProvider _serviceProvider;

  public void Load(IServiceProvider serviceProvider)
  {
    SearchAPIPlugin._serviceProvider = serviceProvider;
    if (!ComHost.Configuration.ComSupportActive)
      return;
    ComHost.ActivateClassFactory(typeof (Intermech.SearchAPI.SearchAPI));
  }

  public void Unload()
  {
    if (!ComHost.Configuration.ComSupportActive)
      return;
    ComHost.DeactivateClassFactory(typeof (Intermech.SearchAPI.SearchAPI));
  }

  public string Name => "IPS Search API Emulator v0.3";
}
