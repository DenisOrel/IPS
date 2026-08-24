// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.SeriesDates.SeriesDatesClientModule
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Search.Pdm.SeriesDates;

public sealed class SeriesDatesClientModule
{
  private IFactory _factory;

  public SeriesDatesClientModule(IFactory factory)
  {
    this._factory = factory != null ? factory : throw new ArgumentNullException(nameof (factory));
  }

  public void Load()
  {
    AdjustableViewsHelper.RegisterView("VersionsApplicabilitiesView", LocalizationHolder.rm.GetString("Pdm_702"), LocalizationHolder.rm.GetString("Pdm_703"), "Intermech.Pdm", "imgObjectsFilter", true, 27);
    this._factory.AddViewsProvider(1, (IViewsProvider) new SeriesDatesViewsProvider());
  }

  public void Unload()
  {
  }
}
