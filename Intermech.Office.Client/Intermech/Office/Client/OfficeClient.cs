// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeClient
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using ImSSP;
using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.DatabaseConfigurator;
using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Office.Client.Properties;
using Intermech.Office.Interfaces;
using Intermech.PropertyEditors;
using Intermech.Protection;
using Intermech.Search.MSOfficeAddins;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class OfficeClient : IPackage
{
  [CanBeNull]
  private IAdditionalView _docAdditionalView;
  [CanBeNull]
  private IServiceProvider _serviceProvider;
  private MSOfficeAddinsClientModule _msOfficeAddinsClientModule = new MSOfficeAddinsClientModule();

  public void Load([NotNull] IServiceProvider serviceProvider)
  {
    int appId = 349;
    Holder.Init((IPackage) this, serviceProvider);
    IProtectionKey service1 = serviceProvider.GetService<IProtectionKey>(false);
    ServicesManager.GetService<ILicenser>().AllocateLicense(appId);
    if (service1 == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      OfficeConsts.Init(sessionKeeper.Session);
      IOfficeDocumentTypeService documentTypeService;
      try
      {
        documentTypeService = sessionKeeper.Session.GetCustomService<IOfficeDocumentTypeService>(false);
      }
      catch
      {
        documentTypeService = (IOfficeDocumentTypeService) null;
      }
      if (documentTypeService == null)
      {
        int num = (int) MessageBox.Show(Localization.GetString("Office.Client_53"), Localization.GetString("Office.Client_52"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }
      OfficeClientConsts.IsPrivateOffice = sessionKeeper.Session.GetCustomService<IOfficeGeneralSettingsService>().Settings.PrivateOffice;
      IAttributePropertyDescriberService service2 = ServicesManager.GetService<IAttributePropertyDescriberService>(false);
      if (service2 != null)
      {
        SubordinateDescriber subordinateDescriber = new SubordinateDescriber();
        if (service2.GetDescriber(OfficeConsts.AttrControllerID) == null)
          service2.RegisterDescriber(OfficeConsts.AttrControllerID, (IAttributePropertyDescriber) subordinateDescriber);
        if (service2.GetDescriber(OfficeConsts.AttrExecutorsID) == null)
          service2.RegisterDescriber(OfficeConsts.AttrExecutorsID, (IAttributePropertyDescriber) subordinateDescriber);
        AddresseeDescriber addresseeDescriber = new AddresseeDescriber();
        if (service2.GetDescriber(OfficeConsts.AttrAddresseesID) == null)
          service2.RegisterDescriber(OfficeConsts.AttrAddresseesID, (IAttributePropertyDescriber) addresseeDescriber);
      }
    }
    MenuTemplate contextMenuTemplate = Holder.Factory.ContextMenuTemplate;
    this._serviceProvider = serviceProvider;
    IPluginManager service3 = serviceProvider.GetService<IPluginManager>();
    service3.LoadComplete += new EventHandler(this.pluginManager_LoadComplete);
    OfficeClientConsts.CategorySubordinateRoot = Holder.GuidMapper.Register(OfficeClientConsts.CategorySubordinateRootGuid);
    OfficeClientConsts.CategoryResolutionsRoot = Holder.GuidMapper.Register(OfficeClientConsts.CategoryResolutionsRootGuid);
    Holder.Factory.AddNodeType(OfficeClientConsts.CategoryResolutionsRoot, typeof (ResolutionsRootNode));
    Holder.IconService.AddIcon(Resources.resolutions_tree, OfficeClientConsts.CategoryResolutionsRoot);
    Holder.Factory.AddViewsProvider(OfficeClientConsts.CategoryResolutionsRoot, (IViewsProvider) new ResolutionsRootNodeViewsProvider());
    TaskViewProvider provider1 = new TaskViewProvider();
    Holder.Factory.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID(wfConsts.TaskGuid), (IViewsProvider) provider1);
    Holder.Factory.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID(wfConsts.StartGuid), (IViewsProvider) provider1);
    Holder.NamedList.Add(Resources.list_resolutions, sc_15053.ssp_office_15054());
    MenuTemplateNode node = new MenuTemplateNode(OfficeClientConsts.CmdOfficeMainNode, OfficeClientConsts.MenuOfficeMainNode, Holder.NamedList.Add(Resources.office, "Office.Office"), 999, 10);
    node.Nodes.Add(new MenuTemplateNode(OfficeClientConsts.CmdRegisterDocument, OfficeClientConsts.MenuRegisterDocument, Holder.NamedList.Add(Resources.reg_to_office, "Office.RegToOffice"), 10, 10));
    node.Nodes.Add(new MenuTemplateNode(OfficeClientConsts.CmdPrivateRegister, OfficeClientConsts.MenuPrivateRegister, Holder.NamedList.Add(Resources.reg_to_private_office, "Office.RegToPrivateOffice"), 10, 20));
    node.Nodes.Add(new MenuTemplateNode(OfficeClientConsts.CmdCreateResolution, OfficeClientConsts.MenuCreateResolution, Holder.NamedList.Add(Resources.create_resolution, "Office.CreateResolution"), 20, 10));
    node.Nodes.Add(new MenuTemplateNode(OfficeClientConsts.CmdCreateConfidentialResolution, OfficeClientConsts.MenuCreateConfidentialResolution, Holder.NamedList.Add(Resources.create_conf_resolution, "Office.CreateConfidentialResolution"), 20, 15));
    node.Nodes.Add(new MenuTemplateNode(OfficeClientConsts.CmdAnswer, OfficeClientConsts.MenuAnswer, Holder.NamedList.Add(Resources.answer, "Office.Answer"), 20, 20));
    node.Nodes.Add(new MenuTemplateNode(OfficeClientConsts.CmdSendEmail, OfficeClientConsts.MenuSendEmail, Holder.NamedList.Add(Resources.send_e_mail, "Office.SendToEmail"), 20, 30));
    node.Nodes.Add(new MenuTemplateNode(OfficeClientConsts.CmdSendEmailProcess, OfficeClientConsts.MenuSendEmailProcess, Holder.NamedList.Add(Resources.process_send_e_mail, "Office.ProcessSendEmail"), 20, 40));
    node.Nodes.Add(new MenuTemplateNode(OfficeClientConsts.CmdRegisterAttachments, OfficeClientConsts.MenuRegisterAttachments, -1, 30, 10));
    node.Nodes.Add(new MenuTemplateNode(OfficeClientConsts.CmdConvertToInternalMessage, OfficeClientConsts.MenuConvertToInternalMessage, -1, 30, 20));
    node.Nodes.Add(new MenuTemplateNode(OfficeClientConsts.CmdGenerateRegNumber, OfficeClientConsts.MenuGenerateRegNumber, -1, 40, 10));
    try
    {
      contextMenuTemplate.BeginUpdate();
      contextMenuTemplate.Nodes.Add(node);
    }
    finally
    {
      contextMenuTemplate.EndUpdate();
    }
    Holder.Factory.AddCommandsProvider(1, OfficeConsts.ObjtypeEmailMessagesID, (ICommandsProvider) new EmailMessagesCommands());
    DocumentCommands provider2 = new DocumentCommands();
    provider2.ServiceProvider = serviceProvider;
    Holder.Factory.AddCommandsProvider(1, OfficeConsts.ObjtypeDocumentsID, (ICommandsProvider) provider2);
    Holder.Factory.AddCommandsProvider(1, OfficeConsts.ObjtypeResolutionsID, (ICommandsProvider) provider2);
    Holder.Factory.AddViewsProvider(1, MetaDataHelper.GetObjectTypeID("cadd9235-306c-11d8-b4e9-00304f19f545"), (IViewsProvider) new OfficeSettingsViewsProvider());
    OfficeDocViewsProvider provider3 = new OfficeDocViewsProvider();
    Holder.Factory.AddViewsProvider(1, OfficeConsts.ObjtypeOfficeDocumentsID, (IViewsProvider) provider3);
    Holder.Factory.AddViewsProvider(1, OfficeConsts.ObjtypeResolutionsID, (IViewsProvider) provider3);
    Holder.Factory.AddNodeType(4, OfficeConsts.ObjtypeOfficeDocumentsID, typeof (OfficeTypeNode));
    Holder.Factory.AddNodeType(1, OfficeConsts.ObjtypeOfficeDocumentsID, typeof (OfficeDocNode));
    Holder.Factory.AddNodeType(1, OfficeConsts.ObjtypeResolutionsID, typeof (OfficeDocNode));
    if (!service3.AutoLoad)
      this.pluginManager_LoadComplete((object) this, new EventArgs());
    Holder.NotificationService.Subscribe("ObjectsChanged", new NotificationEventHandler(this.ObjectsChangedHandler));
    Holder.NotificationService.Subscribe("ObjectsCreated", new NotificationEventHandler(this.ObjectsCreatedHandler));
    Holder.ObjectCreatorService.AfterObjectCreatedEvent += new AfterObjectCreatedEventHandler(OfficeClient.ObjCreator_OnObjectCreatorCompletedEvent);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IOfficeRegistrationService customService1 = sessionKeeper.Session.GetCustomService<IOfficeRegistrationService>();
      IOfficeGeneralSettingsService customService2 = sessionKeeper.Session.GetCustomService<IOfficeGeneralSettingsService>();
      long userId = sessionKeeper.Session.UserID;
      long userUnit = customService1.GetUserUnit(userId);
      if (userUnit != 0L)
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(userUnit);
        OfficeClientConsts.CategoryIncomingDocuments = Holder.GuidMapper.Register(OfficeClientConsts.CategoryIncomingDocumentsGuid);
        Holder.IconService.AddIcon(Resources.IncomingDocuments, OfficeClientConsts.CategoryIncomingDocuments, 0);
        Holder.Factory.AddViewsProvider(OfficeClientConsts.CategoryIncomingDocuments, (IViewsProvider) new IncomingDocumentsViewProvider());
        if (customService2.Settings.IncomingPrivateFolderEnable)
          Holder.Factory.AddGlobalNode(Guid.NewGuid(), (IDescriptor) new IncomingDocumentsDescriptor(userUnit, objectInfo.Caption), 22);
      }
    }
    this._msOfficeAddinsClientModule.Load();
  }

  private static void ObjCreator_OnObjectCreatorCompletedEvent(
    [CanBeNull] object sender,
    [NotNull] AfterObjectCreatedEventArgs ea)
  {
    if (!MetaDataHelper.IsObjectTypeChildOf(ea.ObjectTypeID, OfficeConsts.ObjtypeResolutionsID))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      sessionKeeper.Session.GetCustomService<IResolutionAccessService>().SetAccess(ea.ObjectID);
  }

  public void ObjectsCreatedHandler([CanBeNull] object sender, [NotNull] NotificationEventArgs e)
  {
    if (!(e is DBObjectsEventArgs ea))
      return;
    using (LazySession sk = new LazySession())
    {
      foreach ((long ObjectID, int ObjectTypeID) tuple in (IEnumerable<(long ObjectID, int ObjectTypeID)>) ea.GetObjectsInfo(sk, OfficeConsts.ObjtypeResolutionsID))
        ResolutionCreator.OnCreatedNotificationFired(sk, tuple.ObjectID, tuple.ObjectTypeID);
    }
  }

  public void ObjectsChangedHandler([CanBeNull] object sender, [NotNull] NotificationEventArgs e)
  {
    if (this.Equals(sender) || !(e is DBObjectsExtendedEventArgs extendedEventArgs) || extendedEventArgs.OrigAttributeValuesArray == null)
      return;
    using (SessionKeeper keeper = new SessionKeeper())
    {
      if (keeper.Session.GetObject(extendedEventArgs.ObjectIDs[0]).ObjectVerType == -1 || !MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeDocumentsID).Contains(extendedEventArgs.ObjectType) || !((IEnumerable<AttributeValues>) extendedEventArgs.OrigAttributeValuesArray).Any<AttributeValues>((Func<AttributeValues, bool>) (t => t.AttributeID.Equals(OfficeConsts.AttrOfficeDocumentTypeID) && Convert.ToInt32(t.Values[0]) != 1)))
        return;
      List<long> origAddressees = new List<long>();
      foreach (object obj in ((IEnumerable<AttributeValues>) extendedEventArgs.OrigAttributeValuesArray).Where<AttributeValues>((Func<AttributeValues, bool>) (attributeValue => attributeValue.AttributeID == OfficeConsts.AttrAddresseesID)).SelectMany<AttributeValues, object>((Func<AttributeValues, IEnumerable<object>>) (attributeValue => (IEnumerable<object>) attributeValue.Values)))
        this.AddUserToList(keeper.Session, origAddressees, Convert.ToInt64(obj));
      List<long> longList = new List<long>(((IEnumerable<AttributeValues>) extendedEventArgs.AttributeValuesArray).Where<AttributeValues>((Func<AttributeValues, bool>) (attributeValue => attributeValue.AttributeID == OfficeConsts.AttrAddresseesID)).SelectMany<AttributeValues, long>((Func<AttributeValues, IEnumerable<long>>) (attributeValue => attributeValue.Values.OfType<long>().SelectNotNull<long, List<long>>((Func<long, List<long>>) (longValue => OfficeClientHelper.GetUserAddresseeList(keeper.Session, longValue))).SelectMany<List<long>, long>((Func<List<long>, IEnumerable<long>>) (userIDs => (IEnumerable<long>) userIDs)).Where<long>((Func<long, bool>) (userID => !origAddressees.Contains(userID))).Distinct<long>())));
      if (longList.Count <= 0)
        return;
      foreach (long objectId in (IEnumerable<long>) extendedEventArgs.ObjectIDs)
      {
        IDBObject document = keeper.Session.GetObject(objectId);
        if (document.ObjectVerType == 0)
          OfficeClientHelper.CreateAddresseesMessage(keeper.Session, longList.ToArray(), document);
      }
    }
  }

  private void AddUserToList([NotNull] IUserSession session, [NotNull] List<long> result, long addresseeID)
  {
    List<long> userAddresseeList = OfficeClientHelper.GetUserAddresseeList(session, addresseeID);
    if (userAddresseeList == null)
      return;
    foreach (long num in userAddresseeList)
    {
      if (!result.Contains(num))
        result.Add(num);
    }
  }

  private void pluginManager_LoadComplete([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    IDatabaseConfiguratorService service1 = Intermech.Diagnostics.Check.NotNull<IServiceProvider>(this._serviceProvider, "_serviceProvider").GetService<IDatabaseConfiguratorService>(false);
    if (service1 != null)
    {
      this._docAdditionalView = (IAdditionalView) new OfficeDocSettingsView();
      service1.RegisterDocumentAdditionalView(this._docAdditionalView);
    }
    Holder.ObjectCreatorService.RegisterCreatorCustomService(OfficeConsts.ObjtypeOfficeDocumentsID, typeof (OfficeDocumentCreator));
    Holder.ObjectCreatorService.RegisterCreatorCustomService(OfficeConsts.ObjtypeResolutionsID, typeof (ResolutionCreator));
    IPropertyPagesService service2 = this._serviceProvider.GetService<IPropertyPagesService>(false);
    if (service2 != null)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.IsAdmin)
        {
          service2.AddPage($"{OfficeClientConsts.OfficePropertyPageName}\\{OfficeClientConsts.GeneralPropertyPageName}", (IPropertyPage) new GeneralSettings(sessionKeeper.Session));
          service2.AddPage($"{OfficeClientConsts.OfficePropertyPageName}\\{OfficeClientConsts.OfficeSupervisorsPageName}", (IPropertyPage) new OfficeSupervisorsListControl(sessionKeeper.Session));
        }
      }
    }
    IFormDesignerActionManager service3 = ServicesManager.GetService<IFormDesignerActionManager>(false);
    if (service3 != null)
    {
      service3.RegisterAction(ReportsInfo.ReportsExecute, (IFormDesignerActionHandler) new ReportsEditorActionHandler());
      service3.RegisterAction(AddresseeInfo.AddresseeEditorExecute, (IFormDesignerActionHandler) new AddresseeEditorActionHandler());
      service3.RegisterAction(ResolutionTextInfo.ResolutionTextEditorExecute, (IFormDesignerActionHandler) new ResolutionTextEditorActionHandler());
      service3.RegisterAction(ExecutionOrderInfo.ExecutionOrderEditorAction, (IFormDesignerActionHandler) new ExecutionOrderEditorActionHandler());
    }
    IPreviewExtender service4 = this._serviceProvider.GetService<IPreviewExtender>(false);
    if (service4 != null)
      service4.Extend += new ExtendEventHandler(ResolutionPreview.OnExtend);
    this._serviceProvider.GetService<IFormDesignerEventsManager>().DataLoadCompleted += new FormDesignerEventHandler(OfficeClient.EventsManager_DataLoadCompleted);
    this.RegisterViews();
  }

  private void RegisterViews()
  {
    AdjustableViewsHelper.RegisterView("Office.TreeResolutionsView", "Поручения", "", "", "Office.ResolutionsList", true, 0);
  }

  private static void EventsManager_DataLoadCompleted([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (!(sender is DesForm currentForm) || currentForm.Info == null || currentForm.Info.ElementKind != AttributableElements.Object || !MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeResolutionsID).Contains(currentForm.ElementTypeID))
      return;
    System.Timers.Timer timer = new System.Timers.Timer(3000.0);
    timer.AutoReset = false;
    timer.Elapsed += new ElapsedEventHandler(new OfficeClient.ElapsedEvent(currentForm).OnElapsed);
    timer.Start();
  }

  public void Unload() => this._msOfficeAddinsClientModule.Unload();

  [NotNull]
  public string Name => Localization.GetString("Office.Client_3");

  private class ElapsedEvent
  {
    [NotNull]
    private readonly DesForm _currentForm;

    public ElapsedEvent([NotNull] DesForm currentForm) => this._currentForm = currentForm;

    public void OnElapsed([CanBeNull] object sender, [NotNull] ElapsedEventArgs e)
    {
      if (!this._currentForm.IsFormActivated)
        return;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject iDbAttributable = sessionKeeper.Session.GetObject(this._currentForm.Info.ElementIdentifier);
        bool flag = false;
        IDBAttribute attributeById = iDbAttributable.GetAttributeByID(OfficeConsts.AttrExecutorsID);
        if (attributeById != null && !attributeById.IsNull)
        {
          for (int index = 0; index < attributeById.ValuesCount; ++index)
          {
            attributeById.Index = index;
            if (attributeById.AsInteger == sessionKeeper.Session.UserID)
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          return;
        bool result;
        if (!iDbAttributable.TryGetAttrBoolValue(OfficeConsts.AttrReadID, out result))
        {
          iDbAttributable.SetAttrBoolValue(OfficeConsts.AttrReadID, false);
        }
        else
        {
          if (result)
            return;
          iDbAttributable.SetAttrBoolValue(OfficeConsts.AttrReadID, true);
          Holder.NotificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", this._currentForm.Info.ElementIdentifier));
        }
      }
    }
  }
}
