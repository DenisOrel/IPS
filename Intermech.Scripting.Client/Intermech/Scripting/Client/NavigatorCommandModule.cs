// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Client.NavigatorCommandModule
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.ApplicationModel;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Data.Metadata;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Client;

internal sealed class NavigatorCommandModule : InitializerModule
{
  private MetadataResolverFactory metadataResolvers;
  private IFactory navigatorFactory;
  private IDefaultCommands4ObjTypes navigatorDefaults;
  private Func<NavigatorCommandProvider> commandProviderFactory;

  public NavigatorCommandModule(
    MetadataResolverFactory metadataResolvers,
    IFactory navigatorFactory,
    IDefaultCommands4ObjTypes navigatorDefaults,
    Func<NavigatorCommandProvider> commandProviderFactory)
  {
    if (metadataResolvers == null)
      throw new ArgumentNullException(nameof (metadataResolvers));
    if (navigatorFactory == null)
      throw new ArgumentNullException(nameof (navigatorFactory));
    if (navigatorDefaults == null)
      throw new ArgumentNullException(nameof (navigatorDefaults));
    if (commandProviderFactory == null)
      throw new ArgumentNullException(nameof (commandProviderFactory));
    this.metadataResolvers = metadataResolvers;
    this.navigatorFactory = navigatorFactory;
    this.navigatorDefaults = navigatorDefaults;
    this.commandProviderFactory = commandProviderFactory;
  }

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this.RegisterNavigatorCommandsProvider();
  }

  protected override void DoShutdown() => base.DoShutdown();

  private void RegisterNavigatorCommandsProvider()
  {
    List<GlobalId<int>> scriptTypes = this.GetScriptTypes();
    if (scriptTypes.Count == 0)
      return;
    NavigatorCommandProvider provider = this.commandProviderFactory();
    foreach (GlobalId<int> globalId in scriptTypes)
    {
      this.navigatorFactory.AddCommandsProvider(1, globalId.Id, (ICommandsProvider) provider);
      this.navigatorDefaults.AddDefaultCommand(globalId.Id, "EditDocument", DefaultCommandHandler.ContectMenu);
    }
  }

  private List<GlobalId<int>> GetScriptTypes()
  {
    return new List<GlobalId<int>>()
    {
      this.metadataResolvers.ObjectTypeResolver(new Guid("CAD0036A-306C-11D8-B4E9-00304F19F545")).GID,
      this.metadataResolvers.ObjectTypeResolver(new Guid("CADD939B-306C-11D8-B4E9-00304F19F545")).GID
    };
  }
}
