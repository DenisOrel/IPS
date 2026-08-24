// Decompiled with JetBrains decompiler
// Type: Intermech.NormaCSIntegrator.Client.NormaCSIntegratorPlugin
// Assembly: Intermech.NormaCSIntegrator.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: BC215C8E-677A-43E5-99F7-5ED2ECAA0726
// Assembly location: D:\IPS\Client\Intermech.NormaCSIntegrator.Client.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.Plugins;
using Intermech.Localization;
using Intermech.Navigator.ContextCommands;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.NormaCSIntegrator.Client;

public class NormaCSIntegratorPlugin : IPackage, ICommandsProvider
{
  public void Load(IServiceProvider serviceProvider)
  {
    if (ServicesManager.GetService(typeof (IPluginManager)) is IPluginManager service)
      service.LoadComplete += new EventHandler(this.PluginManager_LoadComplete);
    ServicesManager.AddService(typeof (INormaCSService), (object) new NormaCSService());
  }

  public void Unload() => ServicesManager.RemoveService(typeof (INormaCSService));

  public string Name => LocalizationHolder.rm.GetString("NormaCSIntegrator_1");

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    if (items.Count != 1)
      return CommandsInfo.Empty;
    CommandsInfo groupCommands = new CommandsInfo();
    if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || MetaDataHelper.GetAttribute4ObjectType(itemData.ObjectType, ConstsHolder.AttrImbaseLinkID) == null)
      return groupCommands;
    groupCommands.Add("StartNormaCS", new CommandInfo(0, new ClickEventHandler(this.StartNormaCS)));
    groupCommands.Add("FindByNumberNormaCS", new CommandInfo(0, new ClickEventHandler(this.FindByNumberNormaCS)));
    groupCommands.Add("FindByNameNormaCS", new CommandInfo(0, new ClickEventHandler(NormaCSIntegratorPlugin.FindByNameNormaCS)));
    groupCommands.Add("FindByTextNormaCS", new CommandInfo(0, new ClickEventHandler(NormaCSIntegratorPlugin.FindByTextNormaCS)));
    return groupCommands;
  }

  private void StartNormaCS(
    ISelectedItems items,
    IServiceProvider viewservices,
    object additionalinfo)
  {
    ServiceUtils.GetService<INormaCSService>((object) ServicesManager.ServiceContainer, true).Start();
  }

  private static void FindByTextNormaCS(
    ISelectedItems items,
    IServiceProvider viewservices,
    object additionalinfo)
  {
    ServiceUtils.GetService<INormaCSService>((object) ServicesManager.ServiceContainer, true).FindByText(NormaCSIntegratorPlugin.GetTextForSearching(items, viewservices, additionalinfo));
  }

  private static void FindByNameNormaCS(
    ISelectedItems items,
    IServiceProvider viewservices,
    object additionalinfo)
  {
    ServiceUtils.GetService<INormaCSService>((object) ServicesManager.ServiceContainer, true).FindByName(NormaCSIntegratorPlugin.GetTextForSearching(items, viewservices, additionalinfo));
  }

  private static string GetTextForSearching(
    ISelectedItems items,
    IServiceProvider viewservices,
    object additionalinfo)
  {
    string textForSearching = string.Empty;
    if (viewservices.GetService(typeof (ChildrenView)) is ChildrenView)
    {
      ObjectCommands.CopyTextCommand(items, viewservices, additionalinfo);
      try
      {
        textForSearching = Clipboard.GetText(TextDataFormat.UnicodeText);
      }
      catch (Exception ex)
      {
      }
    }
    else
    {
      IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (itemData != null)
        {
          textForSearching = sessionKeeper.Session.GetObject(itemData.ObjectID).GetAttributeByID(ConstsHolder.AttrNameID).AsString;
          if (textForSearching == string.Empty)
            textForSearching = itemData.Caption;
        }
      }
    }
    return textForSearching;
  }

  private void FindByNumberNormaCS(
    ISelectedItems items,
    IServiceProvider viewservices,
    object additionalinfo)
  {
    if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    INormaCSService service = ServiceUtils.GetService<INormaCSService>((object) ServicesManager.ServiceContainer, true);
    string objectGostNumber;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(itemData.ObjectID);
      objectGostNumber = NormaCSIntegratorPlugin.GetObjectGostNumber(dbObject);
      if (objectGostNumber.Equals(string.Empty))
      {
        IDBAttribute attributeById = dbObject.GetAttributeByID(ConstsHolder.AttrImbaseLinkID);
        if (attributeById != null)
        {
          if (!attributeById.IsNull)
          {
            long asInteger = attributeById.AsInteger;
            objectGostNumber = NormaCSIntegratorPlugin.GetObjectGostNumber(sessionKeeper.Session.GetObject(asInteger));
          }
        }
      }
    }
    service.FindByNumber(objectGostNumber);
  }

  private static string GetObjectGostNumber(IDBObject obj)
  {
    string objectGostNumber = string.Empty;
    IDBAttribute attributeById = obj.GetAttributeByID(ConstsHolder.AttrGostID);
    if (attributeById != null)
      objectGostNumber = attributeById.AsString;
    return objectGostNumber;
  }

  private void PluginManager_LoadComplete(object sender, EventArgs e)
  {
    if (!(ServicesManager.GetService(typeof (IImbaseSelector)) is IImbaseSelector))
      throw new Exception(LocalizationHolder.rm.GetString("NormaCSIntegrator_7"));
    if (!(ServicesManager.GetService(typeof (IFactory)) is IFactory service1))
      return;
    service1.AddCommandsProvider(1, (ICommandsProvider) this);
    MenuTemplate contextMenuTemplate = service1.ContextMenuTemplate;
    if (contextMenuTemplate == null)
      return;
    contextMenuTemplate.BeginUpdate();
    try
    {
      INamedImageList service2 = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
      int imageIndex = service2 == null ? -1 : service2.ImageIndex("imgNormaCS");
      MenuTemplateNode node = new MenuTemplateNode("NormaCS", LocalizationHolder.rm.GetString("NormaCSIntegrator_2"), imageIndex, 65, 10);
      contextMenuTemplate.Nodes.Add(node);
      node.Nodes.Add(new MenuTemplateNode("StartNormaCS", LocalizationHolder.rm.GetString("NormaCSIntegrator_19"), imageIndex, 66, 10));
      node.Nodes.Add(new MenuTemplateNode("FindByNumberNormaCS", LocalizationHolder.rm.GetString("NormaCSIntegrator_4"), -1, 66, 11));
      node.Nodes.Add(new MenuTemplateNode("FindByNameNormaCS", LocalizationHolder.rm.GetString("NormaCSIntegrator_5"), -1, 66, 12));
      node.Nodes.Add(new MenuTemplateNode("FindByTextNormaCS", LocalizationHolder.rm.GetString("NormaCSIntegrator_6"), -1, 66, 13));
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
  }
}
