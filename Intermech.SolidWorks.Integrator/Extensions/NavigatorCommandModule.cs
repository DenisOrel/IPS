// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.Extensions.NavigatorCommandModule
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Bars;
using Intermech.Interfaces.Data.Metadata;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.SolidWorks.Integrator.Extensions;

internal sealed class NavigatorCommandModule : InitializerModule
{
  private MetadataResolverFactory metadataResolvers;
  private SWIntegratorModule integratorModule;
  private IFactory navigatorFactory;
  private Func<NavigatorCommandProvider> commandProviderFactory;
  private List<MenuButtonItem> mainMenuItems;

  public NavigatorCommandModule(
    MetadataResolverFactory metadataResolvers,
    SWIntegratorModule integratorModule,
    IFactory navigatorFactory,
    Func<NavigatorCommandProvider> commandProviderFactory)
  {
    if (metadataResolvers == null)
      throw new ArgumentNullException(nameof (metadataResolvers));
    if (integratorModule == null)
      throw new ArgumentNullException(nameof (integratorModule));
    if (navigatorFactory == null)
      throw new ArgumentNullException(nameof (navigatorFactory));
    if (commandProviderFactory == null)
      throw new ArgumentNullException(nameof (commandProviderFactory));
    this.metadataResolvers = metadataResolvers;
    this.integratorModule = integratorModule;
    this.navigatorFactory = navigatorFactory;
    this.commandProviderFactory = commandProviderFactory;
    this.mainMenuItems = new List<MenuButtonItem>();
  }

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this.AddNavigatorCommandsToMenuTemplate();
    this.RegisterNavigatorCommandsProvider();
  }

  protected override void DoShutdown() => base.DoShutdown();

  private void AddNavigatorCommandsToMenuTemplate()
  {
    MenuTemplate contextMenuTemplate = this.navigatorFactory.ContextMenuTemplate;
    contextMenuTemplate.BeginUpdate();
    try
    {
      MenuTemplateNode node = contextMenuTemplate[NavigatorCommandConsts.IntegratorsMenuName];
      if (node == null)
      {
        node = new MenuTemplateNode(NavigatorCommandConsts.IntegratorsMenuName, NavigatorCommandConsts.IntegratorsMenuDisplayName, -1, 24, 30);
        contextMenuTemplate.Nodes.Add(node);
      }
      node.Nodes.Add(new MenuTemplateNode(NavigatorCommandConsts.RepairFileReferencesCommandName, NavigatorCommandConsts.RepairFileReferencesDisplayName, -1, 1, 1));
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
  }

  private void RegisterNavigatorCommandsProvider()
  {
    this.navigatorFactory.AddCommandsProvider(1, (ICommandsProvider) this.commandProviderFactory());
  }
}
