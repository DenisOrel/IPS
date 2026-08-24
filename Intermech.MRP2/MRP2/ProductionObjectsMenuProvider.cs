// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionObjectsMenuProvider
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Bars;
using Intermech.Client.Core.Navigator.Classes.Providers;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;

#nullable disable
namespace Intermech.MRP2;

internal class ProductionObjectsMenuProvider : ICommandsProvider
{
  private static FontStyledNodeService _fonsStyleService;
  private readonly IFactory factory;
  private CheckInOutCommandsProvider _checkInOutProvider;
  private readonly string[] _allowedCommands = new string[31 /*0x1F*/]
  {
    "Add",
    "Exclude",
    "CancelChanges",
    "CheckIn",
    "CheckOut",
    "EditDocument",
    "Refresh",
    "ParametersCard",
    "SeekInTree",
    "ListVersions",
    "SetupColumns",
    "Delete",
    "ObjectComposition",
    "Signs",
    "Lifecycle",
    "PDM.EditSubstitutesGroup",
    "PDM.MakeActualSubstitute",
    "PDM.DeleteSubstitutesGroup",
    "Reports",
    "RunScenarioDoc",
    "GenerateDoc",
    "CreateVersion",
    "Create",
    "OpenInNewWindow",
    "ExpandNodeRecursive",
    "CollapseNode",
    "RunScenarioDocComplect",
    "GenerateAdditionalComplect",
    "GenerateReport",
    "techElemAddNode",
    "CreateByProductionAnalogObject"
  };
  private readonly string[] _allowedDocCommands = new string[1]
  {
    "ViewDocument"
  };

  public ProductionObjectsMenuProvider(IFactory factory, INamedImageList _images)
  {
    this.factory = factory;
    MenuTemplate contextMenuTemplate = factory.ContextMenuTemplate;
    contextMenuTemplate.BeginUpdate();
    contextMenuTemplate.Nodes.Add(new MenuTemplateNode("MRP2.SeparateDelivery", "Изменить способ поставки", -1, 30, 40));
    contextMenuTemplate.Nodes.Add(new MenuTemplateNode("MRP2.LaunchProcess", "Запустить процесс для ПВ", _images.ImageIndex("wfLaunch"), 41, 20));
    contextMenuTemplate.Nodes.Add(new MenuTemplateNode("MRP2.StartPLCheck", "Запустить проверку ЭС ПВ", -1, 41, 21));
    contextMenuTemplate.Nodes.Add(new MenuTemplateNode("MRP2.RecalcCounts", "Пересчитать количества", -1, 41, 22));
    contextMenuTemplate.Nodes.Add(new MenuTemplateNode(MRP2Consts.cmdApplyChangesInPL, "Применить изменения в ПВ по извещениям", -1, 41, 23));
    MenuTemplateNode menuTemplateNode = factory.ContextMenuTemplate["ObjectComposition"];
    if (menuTemplateNode != null)
    {
      menuTemplateNode.Nodes.Add(new MenuTemplateNode(MRP2Consts.cmdAddMRP2, "Добавить в состав объекты ПВ", -1, 10, 21));
      menuTemplateNode.Nodes.Add(new MenuTemplateNode("MRP2.AddFromPL", "Добавить из состава другой ПВ", -1, 10, 40));
      menuTemplateNode.Nodes.Add(new MenuTemplateNode("MRP2.ReplacePart", "Заменить изделие", -1, 10, 50));
      menuTemplateNode.Nodes.Add(new MenuTemplateNode("MRP2.ReplacePartZ", "Заменить на конструкторский заменитель", -1, 10, 50));
      menuTemplateNode.Nodes.Add(new MenuTemplateNode("MRP2.HideDeleted", "Скрыть удаленные позиции", _images.ImageIndex("MRP2.HideDeleted"), 10, 53));
      menuTemplateNode.Nodes.Add(new MenuTemplateNode("MRP2.SelectPL", "Указать производственную ведомость", -1, 10, 54));
      menuTemplateNode.Nodes.Add(new MenuTemplateNode("MRP2.ReplaceVersion", "Заменить версию объекта", -1, 10, 51));
      menuTemplateNode.Nodes.Add(new MenuTemplateNode("MRP2.ExcludeAllDeleted", "Удалить из состава все исключенные объекты", -1, 10, 55));
    }
    contextMenuTemplate.EndUpdate();
    Intermech.Navigator.ContextMenu.Services.AfterCreateMenu += new AfterCreateMenuHandler(this.AfterCreateMenu);
    this._checkInOutProvider = new CheckInOutCommandsProvider();
    this._checkInOutProvider.IgnoreLCStep4CheckOut = true;
  }

  private void AfterCreateMenu(Component contextMenu, IServiceProvider viewServices)
  {
    INavigatorTreeViewContextMenuHelper service = (INavigatorTreeViewContextMenuHelper) viewServices.GetService(typeof (INavigatorTreeViewContextMenuHelper));
    if (service == null || service.Tree == null || service.Tree.FocusedItem == null || service.Tree.FocusedItem.ItemID == null)
      return;
    if (!(service.Tree.FocusedItem.GetItemData(typeof (IDBRelationID)) is IDBRelationID itemData) || itemData.RelationType == -1)
    {
      INodeID itemId = service.Tree.FocusedItem.ItemID;
      if (itemId.CategoryID != 1 || !MetaDataHelper.IsObjectTypeChildOf(itemId.TypeID, MRP2Consts.objtypeIdProductionObjects))
        return;
    }
    else if (itemData.RelationType != MRP2Consts.reltypeIdProductComposition && itemData.RelationType != MRP2Consts.reltypeIdDocumentComposition)
      return;
    ContextMenuBarItem contextMenuBarItem = contextMenu as ContextMenuBarItem;
    if (itemData == null || itemData.RelationType != MRP2Consts.reltypeIdDocumentComposition)
      contextMenuBarItem.BeforePopup += new MenuItemBase.BeforePopupEventHandler(this.ProductionListContextMenuProvider_BeforePopup);
    else
      contextMenuBarItem.BeforePopup += new MenuItemBase.BeforePopupEventHandler(this.ProductionListContextMenuProvider_BeforePopupDoc);
  }

  private void ProductionListContextMenuProvider_BeforePopupDoc(object sender, MenuPopupEventArgs e)
  {
    ContextMenuBarItem cm = sender as ContextMenuBarItem;
    for (int i = cm.Items.Count - 1; i >= 0; i--)
    {
      if (!cm.Items[i].CommandName.StartsWith("MRP2.") && !Array.Exists<string>(this._allowedCommands, (Predicate<string>) (s => s == cm.Items[i].CommandName)) && !Array.Exists<string>(this._allowedDocCommands, (Predicate<string>) (s => s == cm.Items[i].CommandName)))
        cm.Items.RemoveAt(i);
      else if (cm.Items[i].CommandName == "Reports")
      {
        for (int j = cm.Items[i].Items.Count - 1; j >= 0; j--)
        {
          if (!cm.Items[i].Items[j].CommandName.StartsWith("MRP2.") && !Array.Exists<string>(this._allowedCommands, (Predicate<string>) (s => s == cm.Items[i].Items[j].CommandName)))
            cm.Items[i].Items.RemoveAt(j);
        }
      }
      else if (cm.Items[i].CommandName == "ObjectComposition")
      {
        for (int j = cm.Items[i].Items.Count - 1; j >= 0; j--)
        {
          if (!cm.Items[i].Items[j].CommandName.StartsWith("MRP2.") && !Array.Exists<string>(this._allowedCommands, (Predicate<string>) (s => s == cm.Items[i].Items[j].CommandName)))
            cm.Items[i].Items.RemoveAt(j);
        }
      }
    }
  }

  private void ProductionListContextMenuProvider_BeforePopup(object sender, MenuPopupEventArgs e)
  {
    ContextMenuBarItem cm = sender as ContextMenuBarItem;
    for (int i = cm.Items.Count - 1; i >= 0; i--)
    {
      if (!cm.Items[i].CommandName.StartsWith("MRP2.") && !Array.Exists<string>(this._allowedCommands, (Predicate<string>) (s => s == cm.Items[i].CommandName)))
        cm.Items.RemoveAt(i);
      else if (cm.Items[i].CommandName == "Reports")
      {
        for (int j = cm.Items[i].Items.Count - 1; j >= 0; j--)
        {
          if (!cm.Items[i].Items[j].CommandName.StartsWith("MRP2.") && !Array.Exists<string>(this._allowedCommands, (Predicate<string>) (s => s == cm.Items[i].Items[j].CommandName)))
            cm.Items[i].Items.RemoveAt(j);
        }
      }
      else if (cm.Items[i].CommandName == "ObjectComposition")
      {
        for (int j = cm.Items[i].Items.Count - 1; j >= 0; j--)
        {
          if (!cm.Items[i].Items[j].CommandName.StartsWith("MRP2.") && !Array.Exists<string>(this._allowedCommands, (Predicate<string>) (s => s == cm.Items[i].Items[j].CommandName)))
            cm.Items[i].Items.RemoveAt(j);
        }
      }
    }
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    ViewStateFlags viewStateFlags = viewServices.GetService(typeof (IViewState)) is IViewState service ? service.ViewState : ViewStateFlags.None;
    CommandsInfo mergedCommands = new CommandsInfo();
    ContextMenuItemState state = !HideDeletePositionsCommand.checkedState ? new ContextMenuItemState(ContextMenuCheckState.Unchecked) : new ContextMenuItemState(ContextMenuCheckState.Checked);
    mergedCommands.Add("MRP2.HideDeleted", new CommandInfo(0, new ClickEventHandler(HideDeletePositionsCommand.Handler), state));
    int num = 0;
    if (items.Count > 0)
    {
      INodeID itemId = items.GetItemID(0);
      num = MetaDataHelper.IsObjectTypeChildOf(MRP2Consts.objtypeIdProductionLists, itemId.TypeID) ? 1 : 2;
    }
    mergedCommands.Add("MRP2.LaunchProcess", new CommandInfo(0, new ClickEventHandler(LaunchProcessPLCommand.Handler)));
    mergedCommands.Add("MRP2.StartPLCheck", new CommandInfo(0, new ClickEventHandler(StartPLCheckCommand.Handler)));
    if (num == 1)
      mergedCommands.Add("MRP2.RecalcCounts", new CommandInfo(0, new ClickEventHandler(RecalcCountsCommand.Handler)));
    if (!viewStateFlags.HasFlag((Enum) ViewStateFlags.ReadOnly) && items.Count > 0)
    {
      this.validateCheckInOut(items, viewServices);
      mergedCommands.Add(MRP2Consts.cmdAddMRP2, new CommandInfo(0, new ClickEventHandler(AddSostavCommand.Handler)));
      if (num == 2)
        mergedCommands.Add("Exclude", new CommandInfo(0, new ClickEventHandler(ExcludeSostavCommand.Handler)));
      mergedCommands.Add("CheckOut", new CommandInfo(4, new ClickEventHandler(CheckOutCommand.Handler)));
      if (this._checkInOutProvider.AllowCheckIn)
        mergedCommands.Add("CheckIn", new CommandInfo(4, new ClickEventHandler(CheckInCommand.Handler)));
      if (this._checkInOutProvider.AllowCancel)
        mergedCommands.Add("CancelChanges", new CommandInfo(4, new ClickEventHandler(CancelChangesCommand.Handler)));
      if (viewStateFlags.HasFlag((Enum) ViewStateFlags.NodeInTree))
      {
        if (num == 1)
          mergedCommands.Add("EditDocument", new CommandInfo(4, new ClickEventHandler(EditDocumentCommand.EditDocumentInTreeHandler)));
        mergedCommands.Add("MRP2.ExcludeAllDeleted", new CommandInfo(4, new ClickEventHandler(ExcludeAllDeletedCommand.Handler)));
      }
      else if (num == 1)
        mergedCommands.Add("EditDocument", new CommandInfo(4, new ClickEventHandler(EditDocumentCommand.EditDocumentHandler)));
      mergedCommands.Add("MRP2.AddFromPL", new CommandInfo(0, new ClickEventHandler(AddFromPLCommand.Handler)));
      if (num == 1)
        mergedCommands.Add(MRP2Consts.cmdApplyChangesInPL, new CommandInfo(0, new ClickEventHandler(ApplyChangesInPLbyECO.Handler)));
      if (num == 2)
      {
        mergedCommands.Add("MRP2.ReplacePart", new CommandInfo(0, new ClickEventHandler(ReplacePartCommand.Handler)));
        mergedCommands.Add("MRP2.ReplacePartZ", new CommandInfo(0, new ClickEventHandler(ReplacePartFromSubstitutesCommand.Handler)));
        mergedCommands.Add("MRP2.SelectPL", new CommandInfo(0, new ClickEventHandler(SelectPLCommand.Handler)));
        mergedCommands.Add("MRP2.ReplaceVersion", new CommandInfo(0, new ClickEventHandler(ReplaceVersionCommand.Handler)));
        mergedCommands.Add("MRP2.SeparateDelivery", new CommandInfo(0, new ClickEventHandler(SeparateDeliveryCommand.Handler)));
      }
    }
    else
    {
      mergedCommands.Suppress("Add", 0);
      mergedCommands.Suppress("Exclude", 0);
      mergedCommands.Suppress("CheckIn", 0);
      mergedCommands.Suppress("CheckOut", 0);
      mergedCommands.Suppress("CancelChanges", 0);
      mergedCommands.Suppress("EditDocument", 0);
    }
    return mergedCommands;
  }

  private void validateCheckInOut(ISelectedItems items, IServiceProvider viewServices)
  {
    this._checkInOutProvider.Preprocess(items, viewServices);
    for (int index = 0; index < items.Count && this._checkInOutProvider.CanContinue; ++index)
      this._checkInOutProvider.Process(items, index);
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  /// <summary>тестовая команда заглушка</summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  private static void DummyCommandHandler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    throw new NotImplementedException();
  }

  internal static FontStyledNodeService FontStyleService
  {
    get
    {
      if (ProductionObjectsMenuProvider._fonsStyleService == null)
        ProductionObjectsMenuProvider._fonsStyleService = new FontStyledNodeService();
      return ProductionObjectsMenuProvider._fonsStyleService;
    }
  }

  internal void OnNavigatorNewWindowOpening(object sender, NotificationEventArgs e)
  {
    if (!(e is NavigatorWindowOpeningEventArgs openingEventArgs))
      return;
    NavWindowBase navigatorWindow = openingEventArgs.NavigatorWindow;
    INodeID nodeId = (INodeID) null;
    if (openingEventArgs.Descriptor != null)
      nodeId = openingEventArgs.Descriptor.GetRecordNodeID();
    else if (openingEventArgs.Path != null)
      nodeId = openingEventArgs.Path.FirstID;
    if ((nodeId == null || nodeId.CategoryID != 1 ? 0 : (MetaDataHelper.IsObjectTypeChildOf(nodeId.TypeID, MRP2Consts.objtypeIdProductionLists) ? 1 : 0)) == 0)
      return;
    IServiceContainer services = navigatorWindow.Services;
    services.AddService<IFontStyledNode>((IFontStyledNode) ProductionObjectsMenuProvider.FontStyleService);
    TechRouteFilter service1 = new TechRouteFilter(navigatorWindow.TreeView);
    services.AddService<INavigatorVirtualColumnProvider>((INavigatorVirtualColumnProvider) service1);
    INamedImageList service2 = ServiceUtils.GetService<INamedImageList>((object) ServicesManager.ServiceContainer, false);
    ICommandManager service3 = ServicesManager.GetService<ICommandManager>();
    ButtonItem navigatorTreeButton1 = ProductionObjectsMenuProvider.CreateNavigatorTreeButton("", "Скрыть/показать закладки с параметрами", service2.ImageIndex("MRP2.HideViews"), (ICommandManager) null);
    navigatorTreeButton1.AutoToggle = AutoToggleType.Single;
    navigatorTreeButton1.Checked = HideViewsCommand.Hide;
    navigatorTreeButton1.Click += new EventHandler(new HideViewsCommand(navigatorWindow).ClickHandler);
    if (HideViewsCommand.Hide)
      navigatorWindow.ToggleViewsManager(HideViewsCommand.Hide);
    ButtonItem navigatorTreeButton2 = ProductionObjectsMenuProvider.CreateNavigatorTreeButton("MRP2.HideDeleted", "Скрыть удаленные позиции в составе производственной ведомости", service2.ImageIndex("MRP2.HideDeleted"), service3, true);
    navigatorTreeButton2.AutoToggle = AutoToggleType.Single;
    navigatorTreeButton2.Checked = false;
    DropDownMenuItem dropDownMenuItem = new DropDownMenuItem();
    dropDownMenuItem.ToolTipText = "Фильтровать маршруты обработки по входимости";
    dropDownMenuItem.AutoToggle = AutoToggleType.Single;
    dropDownMenuItem.MenuImageList = service2.ImageList;
    MenuButtonItem menuButtonItem1 = new MenuButtonItem("Не фильтровать", new EventHandler(service1.ClickHandler), -1);
    menuButtonItem1.AutoToggle = AutoToggleType.Radio;
    menuButtonItem1.Tag = (object) TechRouteFilterState.trfDisabled;
    menuButtonItem1.ImageIndex = service2.ImageIndex("MRP2.TechFilterDisabled");
    menuButtonItem1.Checked = service1.FilterState == TechRouteFilterState.trfDisabled;
    MenuButtonItem menuButtonItem2 = new MenuButtonItem("Фильтровать", new EventHandler(service1.ClickHandler), -2);
    menuButtonItem2.AutoToggle = AutoToggleType.Radio;
    menuButtonItem2.Tag = (object) TechRouteFilterState.trfEnabled;
    menuButtonItem2.ImageIndex = service2.ImageIndex("MRP2.TechFilterEnabled");
    menuButtonItem2.Checked = service1.FilterState == TechRouteFilterState.trfEnabled;
    MenuButtonItem menuButtonItem3 = new MenuButtonItem("Фильтровать или по умолчанию", new EventHandler(service1.ClickHandler), -3);
    menuButtonItem3.AutoToggle = AutoToggleType.Radio;
    menuButtonItem3.Tag = (object) TechRouteFilterState.trfWithDefault;
    menuButtonItem3.ImageIndex = service2.ImageIndex("MRP2.TechFilterDefault");
    menuButtonItem3.Checked = service1.FilterState == TechRouteFilterState.trfWithDefault;
    dropDownMenuItem.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) menuButtonItem1,
      (ToolbarItemBase) menuButtonItem2,
      (ToolbarItemBase) menuButtonItem3
    });
    dropDownMenuItem.Checked = service1.FilterState != 0;
    dropDownMenuItem.Click += new EventHandler(service1.ClickHandler);
    if (menuButtonItem1.Checked)
      dropDownMenuItem.ImageIndex = menuButtonItem1.ImageIndex;
    if (menuButtonItem2.Checked)
      dropDownMenuItem.ImageIndex = menuButtonItem2.ImageIndex;
    if (menuButtonItem3.Checked)
      dropDownMenuItem.ImageIndex = menuButtonItem3.ImageIndex;
    int index = navigatorWindow.TreeViewControl.TreeToolbar.Items.IndexOf((ToolbarItemBase) navigatorWindow.TreeViewControl.LabelSpace);
    if (index < 0)
      index = navigatorWindow.TreeViewControl.TreeToolbar.Items.Count;
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) navigatorTreeButton1);
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) dropDownMenuItem);
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) navigatorTreeButton2);
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("MRP2.LaunchProcess", "Запустить процесс для производственных ведомостей", service2.ImageIndex("wfLaunch"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("MRP2.StartPLCheck", "Запустить проверку ЭС ПВ", service2.ImageIndex("MRP2.StartPLCheck"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("MRP2.RecalcCounts", "Пересчитать количества", service2.ImageIndex("MRP2.RecalcCounts"), service3, true));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("ListVersions", "Версии объекта", service2.ImageIndex("imgVersionsTree"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("SignUp", "Подписать", service2.ImageIndex("imgSign"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("CancelChanges", "Отменить изменения", service2.ImageIndex("imgCancelChanges"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("CheckIn", "Завершить редактирование", service2.ImageIndex("imgCheckIn"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("CheckOut", "Взять на редактирование", service2.ImageIndex("imgCheckOut"), service3, true));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("CreateProto", "Создать по прототипу", service2.ImageIndex("MRP2.CreateProto"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("MRP2.SelectPL", "Указать производственную ведомость", service2.ImageIndex("MRP2.SelectPL"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("Exclude", "Исключить из состава", service2.ImageIndex("imgExclude"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("MRP2.ReplacePart", "Заменить объект", service2.ImageIndex("MRP2.ReplacePart"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("MRP2.AddFromPL", "Добавить объект из состава другой ПВ", service2.ImageIndex("MRP2.AddFromPL"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton(MRP2Consts.cmdAddMRP2, "Добавить в состав объекты ПВ", service2.ImageIndex("MRP2.Add"), service3, true));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("SeekInTree", "Найти в дереве", service2.ImageIndex("imgSearchTree"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("CollapseNode", "Свернуть", service2.ImageIndex("MRP2.Collapse"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("ExpandNodeRecursive", "Развернуть всё", service2.ImageIndex("MRP2.ExpandAll"), service3));
    navigatorWindow.TreeViewControl.TreeToolbar.Items.Insert(index, (ToolbarItemBase) ProductionObjectsMenuProvider.CreateNavigatorTreeButton("ParametersCard", "Свойства(Карточка)", service2.ImageIndex("imgCard"), service3, true));
    navigatorWindow.TreeViewControl.TreeToolbar.Overflow = ToolBarOverflow.Wrap;
  }

  private static ButtonItem CreateNavigatorTreeButton(
    string commandName,
    string toolTip,
    int imageIndex,
    ICommandManager _commandManager,
    bool beginGroup = false)
  {
    ButtonItem buttonItem = new ButtonItem();
    buttonItem.CommandName = commandName;
    buttonItem.ShowText = false;
    buttonItem.ToolTipText = toolTip;
    buttonItem.ImageIndex = imageIndex;
    buttonItem.BeginGroup = beginGroup;
    ButtonItem navigatorTreeButton = buttonItem;
    _commandManager?.Add((ButtonItemBase) navigatorTreeButton);
    return navigatorTreeButton;
  }
}
