// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsStartup
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using ImSSP;
using Intermech.Bars;
using Intermech.DatabaseConfigurator;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Interfaces.Plugins;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Protection;
using Intermech.Search;
using Intermech.Signs.Classes;
using Intermech.Signs.Interfaces;
using Intermech.Signs.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.Signs.Client;

public class SignsStartup : IPackage, IConfigurable
{
  private IServiceProvider _serviceProvider;
  private CertSheetSaveToDiskPageProvider certSheetSaveToDiskPageProvider;

  public void Unload()
  {
    (ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService).BeforeDraftCreateEvent -= new BeforeDraftCreateEventHandler(this.creatorService_ObjectCreatorBeforeDraftCreateEvent);
    (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).Unsubscribe("ObjectsChanged", new NotificationEventHandler(this.NotifyEvent));
    if (!(ServicesManager.GetService(typeof (ISaveToDiskService)) is ISaveToDiskService service))
      return;
    service.UnregisterProvider((ISaveToDiskPageProvider) this.certSheetSaveToDiskPageProvider);
  }

  public string Name => LocalizationHolder.rm.GetString("Signs_92");

  public void Load(IServiceProvider serviceProvider)
  {
    this._serviceProvider = serviceProvider;
    IProtectionKey service1 = ServicesManager.GetService(typeof (IProtectionKey)) as IProtectionKey;
    if (!(serviceProvider.GetService(typeof (ILicenser)) is ILicenser service2))
      throw new ProtectionException(LocalizationHolder.rm.GetString("Signs_93"));
    service2.AllocateLicense(SignsCache.appId);
    if (service1 != null)
    {
      int index1 = (Environment.TickCount & 15) * 2;
      byte[] queryData = SignsCache.b[index1];
      byte[] numArray = SignsCache.b[index1 + 1];
      byte[] response = new byte[numArray.Length];
      service1.Query(true, SignsCache.appId, queryData, response);
      int length = queryData.Length;
      for (int index2 = 0; index2 < length; ++index2)
      {
        if ((int) numArray[index2] != (int) response[index2])
          return;
      }
      (serviceProvider.GetService(typeof (IPluginManager)) as IPluginManager).LoadComplete += new EventHandler(this.manager_LoadComplete);
      ServicesManager.AddService(typeof (ISignsClientService), (object) new SignsClientService());
      SignsPropertiesPage signsPropertiesPage = new SignsPropertiesPage(serviceProvider);
      SignsOutputPropertyPage outputPropertyPage = new SignsOutputPropertyPage(serviceProvider);
      SignsUserPropertiesPage userPropertiesPage = new SignsUserPropertiesPage(serviceProvider);
      ((ISelectionFormCustomCommandsService) ServicesManager.GetService(typeof (ISelectionFormCustomCommandsService)))?.Register((ISelectionFormCustomCommandsSubscriber) new SignConditionCommandSubscriber());
      ServicesManager.AddService(typeof (ICertSheetClientService), (object) new CertSheetClientService());
      CertSheetPropertiesPage sheetPropertiesPage = new CertSheetPropertiesPage((IServiceProvider) ServicesManager.ServiceContainer);
      if (ServicesManager.GetService(typeof (ISaveToDiskService)) is ISaveToDiskService service3)
      {
        this.certSheetSaveToDiskPageProvider = new CertSheetSaveToDiskPageProvider();
        service3.RegisterProvider((ISaveToDiskPageProvider) this.certSheetSaveToDiskPageProvider);
      }
    }
    if (!(ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole).IsAdmin || !(ServicesManager.GetService(typeof (IMainMenuService)) is IMainMenuService service4))
      return;
    MenuButtonItem menuButtonItem1 = new MenuButtonItem(LocalizationHolder.rm.GetString("Signs_ConvertInnerSignsToLastVersion"), new EventHandler(this.ConvertSignVersionsClick));
    menuButtonItem1.CommandName = "AdminUtils.ConvertSigns";
    service4.RegisterMenuItemsGroup(MainMenuItemSite.AdministratorUtilities, MainMenuItemPosition.Default, false, menuButtonItem1);
    MenuButtonItem menuButtonItem2 = new MenuButtonItem("Исправить значения граф для подписей", new EventHandler(this.ConvertSignGraphsClick));
    menuButtonItem2.CommandName = "AdminUtils.ConvertSignGraphs";
    service4.RegisterMenuItems(MainMenuItemSite.AdministratorUtilities, MainMenuItemPosition.Default, menuButtonItem2);
  }

  internal void ConvertSignGraphsClick(object sender, EventArgs e)
  {
    long num1 = 0;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      num1 = sessionKeeper.Session.Configurations.ReadInteger("KERNEL", "SIGNS", "ConvertSignGraphs", 0L, DBConfigMode.GlobalOnly);
    if (num1 >= 2L)
    {
      int num2 = (int) MessageBox.Show("Конвертация граф для подписей уже проведена.", MessageDialogs.msgInformation, MessageBoxButtons.OK);
    }
    else
    {
      if (MessageBox.Show("Данная команда преобразует цифровые значения граф для подписи в строковые, что позволит обмениваться подписями с другими базами данных IPS. Команда может выполняться долго и перед выполнением команды требуется завершить работу с базой всех пользователей системы. Продолжить?", MessageDialogs.msgQuery, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      int num3 = 0;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        sessionKeeper.Session.AddToTrace("Выполняется преобразование цифровых значений граф для подписей...", Consts.traceAlways, "ConvertSignGraphs.log");
        num3 = Convert.ToInt32(sessionKeeper.Session.GetObjectCollection(SignConsts.CryptoSignObjectTypeGuid).Select(new DBRecordSetParams()).Rows[0][0]);
      }
      if (num3 > 0 && MessageBox.Show("В базе данных найдены усиленные электронные подписи с криптозащитой, которые станут невалидными после выполения данной команды. Продолжить?", MessageDialogs.msgQuery, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        DataRow[] possibleValuesRows = sessionKeeper.Session.GetAttributeType(SignConsts.GraphAttrTypeGuid).GetPossibleValuesRows();
        List<string> stringList = new List<string>(possibleValuesRows.Length);
        for (int index = 0; index < possibleValuesRows.Length; ++index)
        {
          string str1 = possibleValuesRows[index]["F_DESCRIPTION"].ToString().Trim();
          if (stringList.IndexOf(str1) >= 0)
          {
            string str2 = "Выполнение команды прервано. В списке граф найдено неуникальное описание значения: " + str1;
            sessionKeeper.Session.AddToTrace(str2, Consts.traceAlways, "ConvertSignGraphs.log");
            throw new KernelException(str2);
          }
          stringList.Add(str1);
        }
      }
      using (FixGraphs fixGraphs = new FixGraphs())
      {
        int num4 = (int) fixGraphs.ShowDialog();
      }
    }
  }

  public void LoadConfiguration(IConfigurationManager configurationManager) => this.LoadColumns();

  public void SaveConfiguration(IConfigurationManager configurationManager) => this.SaveColumns();

  private void LoadColumns()
  {
    byte[] config_file;
    (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).LoadConfigData("SignsViewColumns", out BlobInformation _, out config_file);
    if (config_file.Length == 0)
      return;
    MemoryStream inStream = new MemoryStream(config_file);
    inStream.Position = 0L;
    try
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load((Stream) inStream);
      foreach (XmlNode childNode in xmlDocument.ChildNodes)
        this.ParseXml(childNode);
    }
    catch
    {
    }
  }

  private void ParseXml(XmlNode node)
  {
    if (node.Name.Equals("SignsViewColumns"))
    {
      foreach (XmlNode childNode in node.ChildNodes)
        this.ParseXml(childNode);
    }
    if (!node.Name.Equals("Column"))
      return;
    string key = node.Attributes["Name"].Value;
    int int32 = Convert.ToInt32(node.InnerText);
    SignsCache.SignsViewColumns[(object) key] = (object) int32;
  }

  private void SaveColumns()
  {
    XmlDocument xmlDocument = new XmlDocument();
    XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
    XmlElement element1 = xmlDocument.CreateElement("SignsViewColumns");
    foreach (string key in (IEnumerable) SignsCache.SignsViewColumns.Keys)
    {
      int int32 = Convert.ToInt32(SignsCache.SignsViewColumns[(object) key]);
      XmlNode element2 = (XmlNode) xmlDocument.CreateElement("Column");
      XmlAttribute attribute = xmlDocument.CreateAttribute("Name");
      attribute.Value = key;
      element2.Attributes.Append(attribute);
      element2.InnerText = int32.ToString();
      element1.AppendChild(element2);
    }
    xmlDocument.AppendChild((XmlNode) xmlDeclaration);
    xmlDocument.AppendChild((XmlNode) element1);
    MemoryStream outStream = new MemoryStream();
    xmlDocument.Save((Stream) outStream);
    outStream.Position = 0L;
    (ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations).WriteConfigData(new BlobInformation(outStream.Length, outStream.Length, DateTime.Now, "SignsViewColumns", ArcMethods.NotPacked, string.Empty), outStream.ToArray());
  }

  private void manager_LoadComplete(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      ISignsService signsService;
      try
      {
        signsService = session.GetCustomService(typeof (ISignsService)) as ISignsService;
      }
      catch
      {
        signsService = (ISignsService) null;
      }
      if (signsService == null)
      {
        int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString(sc_18398.ssp_signs_18399()), LocalizationHolder.rm.GetString("Signs_96"));
      }
      else
      {
        SignsHolder.Factory = ServicesManager.GetService(typeof (IFactory));
        SignsHolder.Bar = ServicesManager.GetService(typeof (BarManager));
        SignsHolder.Init(session, (IServiceProvider) ServicesManager.ServiceContainer);
        IDBAttributeType attributeType = session.GetAttributeType(SignsHolder.GraphAttrTypeID);
        if (attributeType.MultipleValued.Equals((object) MultiValueModes.SingleValueFromList))
          SignsCache.PossibleGraphs = SignsCache.ParsePossibleGraphs(attributeType.GetPossibleValues());
        SignsCache.UserSignsCard = SignsCache.LoadUserGraphInfo(session, session.UserID);
        this.RegisterNodes();
        this.RegisterViews(session);
        this.RegisterContextMenus();
        this.RegisterLevel();
        this.RegisterCreators();
        (ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService).BeforeDraftCreateEvent += new BeforeDraftCreateEventHandler(this.creatorService_ObjectCreatorBeforeDraftCreateEvent);
        (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).Subscribe("ObjectsChanged", new NotificationEventHandler(this.NotifyEvent));
        if (!(ServicesManager.GetService(typeof (IFormDesignerActionManager)) is IFormDesignerActionManager service))
          return;
        service.RegisterAction(SignActionInfo.SignUpExecute, (IFormDesignerActionHandler) new SignUpActionHandler());
      }
    }
  }

  private void creatorService_ObjectCreatorBeforeDraftCreateEvent(
    object sender,
    BeforeDraftCreateEventArgs e)
  {
    if (e.ObjectTypeID == SignsHolder.SignObjectTypeID || MetaDataHelper.IsObjectTypeChildOf(e.ObjectTypeID, SignsHolder.SignObjectTypeID))
      throw new Exception(string.Format(LocalizationHolder.rm.GetString(sc_18398.ssp_signs_18400()), (object) MetaDataHelper.GetObjectTypeName(e.ObjectTypeID)));
  }

  private void NotifyEvent(object sender, NotificationEventArgs e)
  {
    if (e == null)
      return;
    DBObjectsExtendedEventArgs extendedEventArgs = e as DBObjectsExtendedEventArgs;
    if (!(e.EventName == "ObjectsChanged") || extendedEventArgs == null || extendedEventArgs.ObjectType != MetaDataHelper.GetObjectTypeID("cad00002-306c-11d8-b4e9-00304f19f545"))
      return;
    foreach (AttributeValues attributeValues in extendedEventArgs.AttributeValuesArray)
    {
      if (attributeValues.AttributeID == SignsHolder.RankAttrTypeID)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          SignsCard signsCard = SignsCache.LoadUserGraphInfo(sessionKeeper.Session, extendedEventArgs.ObjectIDs[0], false);
          if (extendedEventArgs.ObjectIDs[0] != sessionKeeper.Session.UserID)
            break;
          SignsCache.UserSignsCard = signsCard;
          break;
        }
      }
    }
  }

  internal void UpdateSignsHashesClick(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (ISignsService)) is ISignsService customService))
        return;
      string message;
      customService.UpdateSignsHashes(sessionKeeper.Session.SessionGUID, out message);
      int num = (int) MessageBox.Show(message, MessageDialogs.msgInformation, MessageBoxButtons.OK);
    }
  }

  internal void ConvertSignVersionsClick(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (ISignsService)) is ISignsService customService))
        return;
      customService.ConvertSignsToLastVersion(sessionKeeper.Session.SessionGUID);
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("Signs_SignsConvertEnds"), MessageDialogs.msgInformation, MessageBoxButtons.OK);
    }
  }

  internal void RegisterNodes()
  {
  }

  internal void RegisterViews(IUserSession session)
  {
    if (!(SignsHolder.Factory is IFactory factory))
      return;
    factory.AddViewsProvider(1, SignsHolder.RankTypeID, (IViewsProvider) new GraphsProvider());
    factory.AddViewsProvider(1, SignsHolder.ArchTypeID, (IViewsProvider) new ArchiveSignsProvider());
    factory.AddViewsProvider(1, (IViewsProvider) new SignsViewProvider());
    factory.AddViewsProvider(1, SignsHolder.SignObjectTypeID, (IViewsProvider) new EDSProvider());
    factory.AddViewsProvider(1, session.IdentHelper.UsersTypeID, (IViewsProvider) new OpenKeysProvider());
  }

  internal void RegisterContextMenus()
  {
    if (!(SignsHolder.Factory is IFactory factory))
      return;
    MenuTemplate contextMenuTemplate = factory.ContextMenuTemplate;
    contextMenuTemplate.BeginUpdate();
    try
    {
      INamedImageList service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      MenuTemplateNode node = new MenuTemplateNode("Signs", LocalizationHolder.rm.GetString("Signs_54"), service.ImageIndex("imgSign"), 15, 27);
      contextMenuTemplate.Nodes.Add(node);
      node.Nodes.Add(new MenuTemplateNode("SignView", LocalizationHolder.rm.GetString("SignsView"), -1, 0, 0, Keys.E | Keys.Alt));
      node.Nodes.Add(new MenuTemplateNode("SignUp", LocalizationHolder.rm.GetString("Signs_97"), service.ImageIndex("imgSign"), 1, 0, Keys.Q | Keys.Alt));
      node.Nodes.Add(new MenuTemplateNode("SignAs", LocalizationHolder.rm.GetString("SignAs"), -1, 1, 1, Keys.W | Keys.Alt));
      node.Nodes.Add(new MenuTemplateNode("CryptoSignUp", LocalizationHolder.rm.GetString("CryptoSign"), service.ImageIndex("imgCryptoSignUp"), 1, 2, Keys.S | Keys.Alt));
      node.Nodes.Add(new MenuTemplateNode("CreateCertSheet", LocalizationHolder.rm.GetString("CreateCertSheet"), -1, 2, 0));
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
    factory.AddCommandsProvider(1, (ICommandsProvider) new SignUpContextMenuProvider());
    factory.AddCommandsProvider(1, SignsHolder.SignObjectTypeID, (ICommandsProvider) new EDSContextMenuProvider());
    factory.AddCommandsProvider(4, SignsHolder.SignObjectTypeID, (ICommandsProvider) new EDSTypeContextMenuProvider());
    factory.AddCommandsProvider(1, (ICommandsProvider) new CertSheetContextMenuProvider());
  }

  internal void RegisterLevel()
  {
    if (!SignsHolder.isDatabaseConfiguratorLoaded || !(ServicesManager.GetService(typeof (IDatabaseConfiguratorService)) is IDatabaseConfiguratorService service))
      return;
    service.RegisterCategoryProps(7, (ICategoryProps) new SignControl_LCStepProperty());
    service.RegisterCategoryProps(8, (ICategoryProps) new SignControl_LCLevelProperty());
  }

  internal void RegisterCreators()
  {
  }
}
