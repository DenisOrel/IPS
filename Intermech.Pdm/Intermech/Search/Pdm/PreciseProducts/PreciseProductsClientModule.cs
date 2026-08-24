// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.PreciseProducts.PreciseProductsClientModule
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.Search.Pdm.PreciseProducts;

internal sealed class PreciseProductsClientModule
{
  private MenuTemplateNode _createPreciseProductMenuTemplateNode = new MenuTemplateNode("CreatePreciseProduct", "Точное изделие", -1, 10, 140);
  private PreciseProductsCommandsProvider _preciseProductsCommandsProvider = new PreciseProductsCommandsProvider();

  public void Load()
  {
    ServicesManager.AddService(typeof (IPreciseProductsClientService), (object) new PreciseProductsClientService());
    if (!(ServicesManager.GetService(typeof (IFactory)) is IFactory service))
      return;
    service.ContextMenuTemplate["Create"]?.Nodes.Add(this._createPreciseProductMenuTemplateNode);
    service.AddCommandsProvider((ICommandsProvider) this._preciseProductsCommandsProvider);
  }

  public void Unload()
  {
    ServicesManager.RemoveService(typeof (IPreciseProductsClientService));
    if (!(ServicesManager.GetService(typeof (IFactory)) is IFactory service))
      return;
    service.ContextMenuTemplate["Create"]?.Nodes.Remove(this._createPreciseProductMenuTemplateNode);
    service.RemoveCommandsProvider((ICommandsProvider) this._preciseProductsCommandsProvider);
  }
}
