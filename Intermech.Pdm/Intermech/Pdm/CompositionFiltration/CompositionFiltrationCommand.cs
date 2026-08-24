// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.CompositionFiltration.CompositionFiltrationCommand
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Search;
using System.Collections.Specialized;

#nullable disable
namespace Intermech.Pdm.CompositionFiltration;

internal abstract class CompositionFiltrationCommand : ICompositionFiltrationCommand
{
  protected IFiltrationService filtration;
  protected IMainMenuService mainMenuService;

  public CompositionFiltrationCommand(
    IFiltrationService filtration,
    IMainMenuService mainMenuService)
  {
    this.filtration = filtration;
    this.mainMenuService = mainMenuService;
  }

  public abstract object Value { get; }

  public abstract void CreateCommand(INamedImageList namedImageList);

  public virtual void OnPutPluginData(HybridDictionary tag)
  {
  }

  public virtual void OnGetPluginData(HybridDictionary tag)
  {
  }
}
