// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.MbomClientPackage
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Intermech.Interfaces.Pdm;
using Intermech.Interfaces.Plugins;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Search.Mbom;

internal sealed class MbomClientPackage : IPackage
{
  private MbomCommandsProvider _mbomCommandsProvider;
  private MenuTemplateNode _createMbomMenuTemplateNode;

  public void Load(IServiceProvider serviceProvider)
  {
    IFactory factory = ServiceLocator.Get<IFactory>();
    PDMPluginConsts.DisableCreateTauCommand = true;
    MenuTemplateNode menuTemplateNode = factory.ContextMenuTemplate["Create"];
    if (menuTemplateNode != null)
    {
      this._createMbomMenuTemplateNode = new MenuTemplateNode("CreateMbom", "ТЭСИ", -1, 10, int.MaxValue);
      menuTemplateNode.Nodes.Add(this._createMbomMenuTemplateNode);
    }
    this._mbomCommandsProvider = new MbomCommandsProvider((IMbomClientService) new MbomClientService());
    factory.AddCommandsProvider((ICommandsProvider) this._mbomCommandsProvider);
    factory.AddCommandsProvider(1, MbomConstants.MbomObjectTypeID, (ICommandsProvider) this._mbomCommandsProvider);
  }

  public void Unload()
  {
    IFactory factory = ServiceLocator.Get<IFactory>();
    factory.ContextMenuTemplate["Create"]?.Nodes.Remove(this._createMbomMenuTemplateNode);
    factory.RemoveCommandsProvider((ICommandsProvider) this._mbomCommandsProvider);
  }

  public string Name => "Редактор технологической ЭСИ";
}
