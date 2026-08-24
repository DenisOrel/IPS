// Decompiled with JetBrains decompiler
// Type: Intermech.BugReports.BugReportsStartup
// Assembly: Intermech.BugReports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 16F80F46-2B9D-4747-9BFD-4CC209192F4E
// Assembly location: D:\IPS\Client\Intermech.BugReports.dll

using Intermech.Bars;
using Intermech.Document.Model;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.IO;

#nullable disable
namespace Intermech.BugReports;

public class BugReportsStartup : IPackage, ICommandTarget
{
  private static ICommandManager _commandManager;
  private readonly long whatsNewID = -1074025;
  private readonly string whatsNewPath = "\\\\market\\S4Install\\Site Upgrades\\whatsnew.txt";

  public void Load(IServiceProvider serviceProvider)
  {
    (serviceProvider.GetService(typeof (IPluginManager)) as IPluginManager).LoadComplete += new EventHandler(this.pluginManager_LoadComplete);
    BugReportsStartup.Holder.GuidMapper = (IGuidMapper) serviceProvider.GetService(typeof (IGuidMapper));
    BugReportsStartup.Holder.Factory = (IFactory) serviceProvider.GetService(typeof (IFactory));
    BugReportsStartup.Holder.IconService = (ICategoryTypeIconService) serviceProvider.GetService(typeof (ICategoryTypeIconService));
    BugReportsStartup.Holder.NotificationService = (INotificationService) serviceProvider.GetService(typeof (INotificationService));
    BarManager service1 = (BarManager) serviceProvider.GetService(typeof (BarManager));
    MenuBar menuBar = (MenuBar) null;
    if (service1 != null)
      menuBar = service1.MenuBar;
    if (menuBar != null)
    {
      if (BugReportsStartup._commandManager == null)
        BugReportsStartup._commandManager = (ICommandManager) ServicesManager.GetService(typeof (ICommandManager));
      DocumentMenuHelper.CreateMenuCommands(BugReportsStartup._commandManager);
      MenuItemBase menuItem = menuBar.FindMenuItem("File.New");
      MenuButtonItem menuButtonItem = new MenuButtonItem("Ошибка по Helpdesk");
      menuButtonItem.CommandName = "New.BugFromHelpDesk";
      if (ServicesManager.GetService(typeof (ICategoryTypeIconService)) is ICategoryTypeIconService service2 && service2.IndexOf(4, MetaDataHelper.GetObjectTypeID(BugReportsHolder.OT.BugObjectType)) >= 0)
        menuButtonItem.Icon = service2.GetIcon(4, MetaDataHelper.GetObjectTypeID(BugReportsHolder.OT.BugObjectType));
      menuButtonItem.Click += new EventHandler(BugReportsStartup.BugFromHelpDeskEditor);
      menuItem.Items.Add((ToolbarItemBase) menuButtonItem);
      HelpDeskSetting.Default.Password = "";
      HelpDeskSetting.Default.UserName = "";
      HelpDeskSetting.Default.Save();
    }
    MenuTemplate contextMenuTemplate = BugReportsStartup.Holder.Factory.ContextMenuTemplate;
    contextMenuTemplate.BeginUpdate();
    try
    {
      contextMenuTemplate.Nodes.Add(new MenuTemplateNode("PasteFromClipboard", "Вставить из буфера", -1, 100, 35));
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      BugReportsStartup.Holder.Factory.AddCommandsProvider(1, sessionKeeper.Session.GetObjectType(new Guid(BugReportsHolder.OT.BugObjectType.ToString())).ObjectType, (ICommandsProvider) new PasteFromCipboardProvider());
  }

  private void pluginManager_LoadComplete(object sender, EventArgs e)
  {
    IFormDesignerActionManager service1 = ServicesManager.GetService(typeof (IFormDesignerActionManager)) as IFormDesignerActionManager;
    ServicesManager.GetService(typeof (IObjectCreatorService));
    if (service1 != null)
    {
      service1.RegisterAction(ActionsInfo.FixBugAction.ActionGuid, ActionsInfo.FixBugAction.ActionName, (IFormDesignerActionHandler) new FixBugAction());
      service1.RegisterAction(ActionsInfo.RejectBugAction.ActionGuid, ActionsInfo.RejectBugAction.ActionName, (IFormDesignerActionHandler) new RejectBugAction());
      service1.RegisterAction(ActionsInfo.CheckBugAction.ActionGuid, ActionsInfo.CheckBugAction.ActionName, (IFormDesignerActionHandler) new CheckBugAction());
    }
    if (!(ServicesManager.GetService(typeof (INotificationService)) is INotificationService service2))
      return;
    service2.Subscribe("ObjectsCheckedIn", new NotificationEventHandler(this.PlaceIntoDirectory));
  }

  public void PlaceIntoDirectory(object sender, EventArgs e)
  {
    if (!(e is DBObjectsEventArgs objectsEventArgs) || !objectsEventArgs.ObjectIDs.Contains(this.whatsNewID))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(-this.whatsNewID, false);
      if (dbObject == null)
        return;
      IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"), false);
      if (attributeByGuid == null || !(attributeByGuid is IBlobReader blobReader))
        return;
      BlobInformation blobInformation = blobReader.OpenBlob(0);
      try
      {
        if (blobInformation.RealFileSize <= 0L)
          return;
        byte[] buffer = blobReader.ReadDataBlock((int) blobInformation.RealFileSize);
        if (buffer == null || buffer.Length == 0)
          return;
        using (MemoryStream inStream = new MemoryStream(buffer))
        {
          MemoryStream outStream = new MemoryStream();
          int arcMethod = (int) blobInformation.ArcMethod;
          if (blobInformation.ArcMethod == ArcMethods.ZLibPacked)
            ZLibStreamHelper.UnpackStream((Stream) inStream, (Stream) outStream);
          else
            outStream = inStream;
          outStream.Seek(0L, SeekOrigin.Begin);
          FileInfo fileInfo = new FileInfo(this.whatsNewPath);
          if ((fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            fileInfo.Attributes &= ~FileAttributes.ReadOnly;
          using (FileStream fileStream = File.Open(this.whatsNewPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
          {
            fileStream.SetLength(0L);
            byte[] array = outStream.ToArray();
            fileStream.Write(array, 0, array.Length);
          }
          outStream.Close();
        }
      }
      catch (Exception ex)
      {
        throw new Exception($"Ошибка обновления {this.whatsNewPath}.\r\n{ex.Message}", ex);
      }
      finally
      {
        blobReader.CloseBlob();
      }
    }
  }

  public void Unload()
  {
    if (ServicesManager.GetService(typeof (IFormDesignerActionManager)) is IFormDesignerActionManager service1)
    {
      service1.UnregisterAction(ActionsInfo.FixBugAction.ActionGuid);
      service1.UnregisterAction(ActionsInfo.RejectBugAction.ActionGuid);
      service1.UnregisterAction(ActionsInfo.CheckBugAction.ActionGuid);
    }
    if (!(ServicesManager.GetService(typeof (INotificationService)) is INotificationService service2))
      return;
    service2.Unsubscribe(new NotificationEventHandler(this.PlaceIntoDirectory));
  }

  public string Name => "Клиентская часть модуля \"Ошибки и предложения\"";

  public bool Execute(ICommandState commandState)
  {
    return commandState != null && commandState.CommandName == "New.BugFromHelpDesk";
  }

  public bool QueryStatus(ICommandState commandState) => false;

  private static void BugFromHelpDeskEditor(object sender, EventArgs eventArgs)
  {
    BugFromHelpDeskForm.Execute();
  }

  internal sealed class Holder
  {
    public static IGuidMapper GuidMapper;
    public static IFactory Factory;
    public static ICategoryTypeIconService IconService;
    public static INotificationService NotificationService;
    public static INamedImageList NamedImageList;
  }
}
