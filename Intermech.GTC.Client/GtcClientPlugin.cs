// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.GtcClientPlugin
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Bars;
using Intermech.GTC.Client.ImportWizard;
using Intermech.GTC.Client.ItemView;
using Intermech.GTC.Client.PropertyGrid;
using Intermech.GTC.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Views;
using Intermech.PropertyEditors;
using Intermech.Search;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client;

public class GtcClientPlugin : IPackage
{
  private void ImportStepFiles(object sender, EventArgs e)
  {
    ImportMaster importMaster = new ImportMaster();
    if (importMaster.ShowDialog() != DialogResult.OK)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(ServicesManager.GetService(typeof (IBackgroundTaskView)) is IBackgroundTaskView service) || !(sessionKeeper.Session.GetCustomService(typeof (IServiceForBackgroundTask)) is IServiceForBackgroundTask customService))
        return;
      BackgroundTask task = new BackgroundTask(customService);
      service.AddTask((IBackgroundTask) task);
      task.StartTask((object) importMaster.ImportSettings);
    }
  }

  public void Load(IServiceProvider serviceProvider)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      ServiceHolder.Initialize(serviceProvider);
      Icon icon = (Icon) ServiceHolder.Rm.GetObject(Const.IconName);
      if (icon != null)
        ServiceHolder.NamedImageList.Add(icon, Const.IconName);
      if (ServiceUtils.GetService<IServiceForBackgroundTask>((object) sessionKeeper.Session, false) == null)
      {
        ServiceHolder.OutputView.WriteString(Const.PluginName, "Работа плагина невозможна - не загружена серверная часть!");
      }
      else
      {
        if (ServicesManager.GetService(typeof (IMainMenuService)) is IMainMenuService service1 && session.IsAdmin)
        {
          MenuButtonItem[] menuButtonItemArray = new MenuButtonItem[1]
          {
            new MenuButtonItem(ServiceHolder.Rm.GetString("GTC_1"), new EventHandler(this.ImportStepFiles), ServiceHolder.NamedImageList.ImageIndex(Const.IconName))
          };
          service1.RegisterMenuItemsGroup(MainMenuItemSite.ExportImport, MainMenuItemPosition.Last, false, menuButtonItemArray);
        }
        ServiceHolder.Factory.AddViewsProvider(1, Const.BaseItemObjectTypeId, (IViewsProvider) new GtcViewProvider());
        if (ServicesManager.GetService(typeof (IAttributePropertyDescriberService)) is IAttributePropertyDescriberService service2 && service2.GetDescriber(Const.AttrsRelationshipTypeAttributeTypeId) == null)
          service2.RegisterDescriber(Const.AttrsRelationshipTypeAttributeTypeId, (IAttributePropertyDescriber) new AttrsRelationshipDescriber());
        GtcCommandsProvider provider = new GtcCommandsProvider();
        ServiceHolder.Factory.AddCommandsProvider(1, Const.ImbaseCatalogObjectTypeId, (ICommandsProvider) provider);
        ServiceHolder.Factory.AddCommandsProvider(1, Const.ImbaseFolderObjectTypeId, (ICommandsProvider) provider);
        foreach (int typeID in MetaDataHelper.GetObjectTypeChildrenID(Const.BaseItemObjectTypeId))
          ServiceHolder.Factory.AddCommandsProvider(1, typeID, (ICommandsProvider) provider);
        MenuTemplate contextMenuTemplate = ServiceHolder.Factory.ContextMenuTemplate;
        MenuTemplateNode menuTemplateNode = contextMenuTemplate["Create"];
        contextMenuTemplate.BeginUpdate();
        try
        {
          if (menuTemplateNode == null)
            return;
          MenuTemplateNode node1 = new MenuTemplateNode("Создать запись каталога GTC", -1, 10, 4);
          menuTemplateNode.Nodes.Add(node1);
          MenuTemplateNode node2 = new MenuTemplateNode("CreateAdaptiveItem", "Адаптивный элемент", -1, 0, 0);
          node1.Nodes.Add(node2);
          MenuTemplateNode node3 = new MenuTemplateNode("CreateInstrumentalItem", "Инструментальный элемент", -1, 0, 1);
          node1.Nodes.Add(node3);
          MenuTemplateNode node4 = new MenuTemplateNode("CreateCuttingItem", "Режущий элемент", -1, 0, 2);
          node1.Nodes.Add(node4);
        }
        finally
        {
          contextMenuTemplate.EndUpdate();
        }
      }
    }
  }

  public void Unload()
  {
  }

  public string Name => Const.PluginName;
}
