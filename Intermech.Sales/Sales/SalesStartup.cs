// Decompiled with JetBrains decompiler
// Type: Intermech.Sales.SalesStartup
// Assembly: Intermech.Sales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D9A9043-6548-439B-99F7-AF22F44A5D2B
// Assembly location: D:\IPS\Client\Intermech.Sales.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Plugins;
using Intermech.Interfaces.Sales;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;

#nullable disable
namespace Intermech.Sales;

public class SalesStartup : IPackage
{
  private KeyProcessor keyProcessor = new KeyProcessor();

  public void Load(IServiceProvider serviceProvider)
  {
    IFactory service1 = (IFactory) serviceProvider.GetService(typeof (IFactory));
    service1.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID("cad0150f-306c-11d8-b4e9-00304f19f545"), (IViewsProvider) new KeyComposition4RequestViewProvider());
    service1.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID("cad01510-306c-11d8-b4e9-00304f19f545"), (IViewsProvider) new KeyCompositionViewProvider());
    service1.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID("cad01510-306c-11d8-b4e9-00304f19f545"), (IViewsProvider) new ProgrammatorViewProvider());
    service1.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID("cad01510-306c-11d8-b4e9-00304f19f545"), (IViewsProvider) new CollectorViewProvider());
    service1.AddCommandsProvider(1, MetaDataHelper.GetObjectTypeID(new Guid("cad0150f-306c-11d8-b4e9-00304f19f545")), (ICommandsProvider) this.keyProcessor);
    MenuTemplateNode menuTemplateNode = service1.ContextMenuTemplate["Create"];
    INamedImageList service2 = serviceProvider.GetService(typeof (INamedImageList)) as INamedImageList;
    menuTemplateNode?.Nodes.Add(new MenuTemplateNode(SalesClientConsts.mnuCreateKey4Request, LocalizationHolder.rm.GetString("Sales_9"), service2.ImageIndex("imgCreate"), 20, 200));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      ProductsProcessor.LoadInfo(sessionKeeper.Session);
  }

  public void Unload()
  {
  }

  public string Name => LocalizationHolder.rm.GetString("Sales_10");
}
