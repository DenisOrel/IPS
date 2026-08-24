// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.Menu.FiltrationCompositionCommandProvider
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.MRP2.Commands;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MRP2.Menu;

/// <summary>
/// Команды организации фильтрации связей по сроку действия
/// </summary>
internal class FiltrationCompositionCommandProvider : ICommandsProvider
{
  private static void OnNavigatorNewWindowOpening(object sender, NotificationEventArgs e)
  {
    if (!(e is NavigatorWindowOpeningEventArgs openingEventArgs))
      return;
    INamedImageList service = ApplicationServices.Container.GetService<INamedImageList>();
    FilterDateAttributesCommand.ReadFilterSettingsFromServerFilterService();
    DropDownMenuItem dropDownMenuItem = new DropDownMenuItem();
    dropDownMenuItem.ToolTipText = LocalizationHolder.rm.GetString("msgFilterCompositionByLinkDate");
    dropDownMenuItem.AutoToggle = AutoToggleType.Single;
    dropDownMenuItem.MenuImageList = service.ImageList;
    dropDownMenuItem.ImageIndex = service.ImageIndex("imgFunnel");
    dropDownMenuItem.ShowText = false;
    dropDownMenuItem.Checked = FilterDateAttributesCommand.FilterByDateInCompositionEnabled;
    dropDownMenuItem.ToolTipText = FilterDateAttributesCommand.FilterByDateInComposition.ToShortDateString();
    dropDownMenuItem.Click += new EventHandler(FilterDateAttributesCommand.ApplyFilterDateCommandHandler);
    MenuButtonItem menuButtonItem = new MenuButtonItem(LocalizationHolder.rm.GetString("msgChooseDate"), new EventHandler(FilterDateAttributesCommand.ChooseFilterDateCommandHandler), -1);
    menuButtonItem.AutoToggle = AutoToggleType.None;
    menuButtonItem.ImageIndex = service.ImageIndex("imgFunnelSetup");
    dropDownMenuItem.Items.Add((ToolbarItemBase) menuButtonItem);
    NavWindowBase navigatorWindow = openingEventArgs.NavigatorWindow;
    int index = navigatorWindow.TreeViewControl.TreeToolbar.Items.IndexOf((ToolbarItemBase) navigatorWindow.TreeViewControl.LabelSpace);
    if (index < 0)
      index = navigatorWindow.TreeViewControl.TreeToolbar.Items.Count;
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) dropDownMenuItem);
  }

  public FiltrationCompositionCommandProvider()
  {
    INamedImageList service1 = ApplicationServices.Container.GetService<INamedImageList>();
    IFactory service2 = ApplicationServices.Container.GetService<IFactory>();
    service2.ContextMenuTemplate.BeginUpdate();
    MenuTemplateNode menuTemplateNode = service2.ContextMenuTemplate["ObjectComposition"];
    MenuTemplateNode node = new MenuTemplateNode("MRP2.FilterByDateMenu", LocalizationHolder.rm.GetString("msgFilterCompositionByLinkDate"), service1.ImageIndex("imgFunnel"), 10, 31 /*0x1F*/);
    menuTemplateNode?.Nodes.Add(node);
    node.Nodes.Add(new MenuTemplateNode("MRP2.AddLinkDateAttributes", LocalizationHolder.rm.GetString("msgAddDateFiltrationAttribute"), -1, 0, 0));
    node.Nodes.Add(new MenuTemplateNode("MRP2.RemoveLinkDateAttributes", LocalizationHolder.rm.GetString("msgRemoveDateFiltrationAttribute"), -1, 0, 1));
    service2.ContextMenuTemplate.EndUpdate();
    ApplicationServices.Container.GetService<INotificationService>()?.Subscribe("NavigatorWindowOpening", new NotificationEventHandler(FiltrationCompositionCommandProvider.OnNavigatorNewWindowOpening));
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add("MRP2.AddLinkDateAttributes", new CommandInfo(0, new ClickEventHandler(FilterDateAttributesCommand.AddDateAttributesCommandHandler)));
    mergedCommands.Add("MRP2.RemoveLinkDateAttributes", new CommandInfo(0, new ClickEventHandler(FilterDateAttributesCommand.RemoveDateAttributesCommandHandler)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }
}
