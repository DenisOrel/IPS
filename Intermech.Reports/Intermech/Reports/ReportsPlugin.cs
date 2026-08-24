// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.ReportsPlugin
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using Intermech.Interfaces.Reports;
using Intermech.Localization;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Protection;
using Intermech.Reports.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Intermech.Reports;

/// <summary>Класс идентификации плагина</summary>
public class ReportsPlugin : IPackage
{
  /// <summary>Сервис плагинов</summary>
  private IPluginManager _manager;
  /// <summary>Служба генерации комплектов документов</summary>
  private ReportsService _reportService;
  /// <summary>Провайдер контекстного меню</summary>
  private ICommandsProvider _commandsProvider;

  /// <summary>Инициализация данных класса</summary>
  private void InitData() => this.Name = LocalizationHolder.rm.GetString("Reports_23");

  /// <summary>Constructor</summary>
  public ReportsPlugin() => this.InitData();

  /// <summary>Выгрузка плагинов</summary>
  public void Unload()
  {
    IProtectionKey service = ServiceUtils.GetService<IProtectionKey>((object) ApplicationServices.Container, true);
    int index = (Environment.TickCount & 15) * 2;
    byte[] numArray = ReportsProtection.Key[index];
    byte[] inArray = new byte[numArray.Length];
    int appId = ReportsProtection.appId;
    byte[] queryData = numArray;
    byte[] response = inArray;
    int num = service.Query(true, appId, queryData, response);
    if (!num.Equals(0) || !Convert.ToBase64String(inArray).Equals(Convert.ToBase64String(ReportsProtection.Key[index + 1])))
      throw new ProtectionException(string.Format(LocalizationHolder.rm.GetString("Reports_44"), (object) LocalizationHolder.rm.GetString("Reports_23"), (object) num));
    ApplicationServices.Container.RemoveService(typeof (IReportsService));
    this._reportService = (ReportsService) null;
    if (this._manager != null)
      this._manager.LoadComplete -= new EventHandler(this._manager_LoadComplete);
    ReportsClientCache.Services.Factory?.RemoveCommandsProvider(1, this._commandsProvider);
    ReportsClientCache.Services.Factory?.RemoveCommandsProvider(1, ReportsConsts.DocPackageBaseTypeID, this._commandsProvider);
    CategoryHelper.Uninitialize(ReportsClientCache.Services.Factory);
  }

  /// <summary>Заголовок плагина</summary>
  public string Name { get; private set; } = string.Empty;

  /// <summary>Загрузка плагина</summary>
  /// <param name="serviceProvider">Провайдер сервисов</param>
  public void Load(IServiceProvider serviceProvider)
  {
    IProtectionKey service1 = ServiceUtils.GetService<IProtectionKey>((object) ApplicationServices.Container, true);
    int index = (Environment.TickCount & 15) * 2;
    byte[] numArray = ReportsProtection.Key[index];
    byte[] inArray = new byte[numArray.Length];
    int appId = ReportsProtection.appId;
    byte[] queryData = numArray;
    byte[] response = inArray;
    int num = service1.Query(true, appId, queryData, response);
    if (!num.Equals(0) || !Convert.ToBase64String(inArray).Equals(Convert.ToBase64String(ReportsProtection.Key[index + 1])))
      throw new ProtectionException(string.Format(LocalizationHolder.rm.GetString("Reports_44"), (object) LocalizationHolder.rm.GetString("Reports_23"), (object) num));
    this.LoadResources();
    this._reportService = new ReportsService();
    ApplicationServices.Container.AddService(typeof (IReportsService), (object) this._reportService);
    ServicesManager.AddService(typeof (IReportUtils), (object) ReportUtils.Instance);
    ComplectAuthFileGenerator.Register(serviceProvider);
    this._manager = serviceProvider.GetService(typeof (IPluginManager)) as IPluginManager;
    if (this._manager != null)
      this._manager.LoadComplete += new EventHandler(this._manager_LoadComplete);
    ReportsClientCache.Services.Factory = serviceProvider.GetService(typeof (IFactory)) as IFactory;
    ReportsClientCache.Services.BackgroundTaskView = serviceProvider.GetService(typeof (IBackgroundTaskView)) as IBackgroundTaskView;
    if (ReportsClientCache.Services.Factory != null)
      ReportsClientCache.Services.Factory.AddViewsProvider(1, ReportsConsts.ScriptPackageTypeID, (IViewsProvider) new DocComplectScriptViewProvider());
    this._commandsProvider = (ICommandsProvider) new ReportCommandProvider(ReportsClientCache.Services.Factory);
    ReportsClientCache.Services.Factory?.AddCommandsProvider(1, this._commandsProvider);
    ReportsClientCache.Services.Factory?.AddCommandsProvider(1, ReportsConsts.DocPackageBaseTypeID, this._commandsProvider);
    CategoryHelper.Initialize(ReportsClientCache.Services.Factory);
    IFactory service2 = ServiceUtils.GetService<IFactory>((object) ApplicationServices.Container, true);
    service2.AddNodeType(CategoryHelper.ReportCategoryID, typeof (ObjectsListNode));
    service2.AddViewsProvider(CategoryHelper.ReportCategoryID, (IViewsProvider) new AdvObjectsPropertiesProvider());
    IPreviewExtender service3 = (IPreviewExtender) serviceProvider.GetService(typeof (IPreviewExtender));
    if (service3 == null)
      return;
    service3.Extend += new ExtendEventHandler(this.previewExtender_Extend);
  }

  private void LoadResources()
  {
    INamedImageList service = ServiceUtils.GetService<INamedImageList>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    Stream manifestResourceStream = this.GetType().Assembly.GetManifestResourceStream("Intermech.Reports.Resources.ReportAndDocs.png");
    if (manifestResourceStream == null)
      return;
    using (Bitmap bitmap = new Bitmap(manifestResourceStream))
      service.Add((Image) bitmap, "imgReport");
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _manager_LoadComplete(object sender, EventArgs e)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="eventArgs"></param>
  private void previewExtender_Extend(ExtendEventArgs eventArgs)
  {
    if (eventArgs == null || eventArgs.ObjectID == -1L || ReportsConsts.FileAttributeTypeID == 0 || !MetaDataHelper.IsObjectTypeChildOf(eventArgs.ObjectType, ReportsConsts.DocPackageBaseTypeID))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IReportsServerService service = ServiceUtils.GetService<IReportsServerService>((object) sessionKeeper.Session, false);
      ReportsDocComplect complect;
      if (service == null || !service.LoadComplectData(eventArgs.ObjectID, out complect, sessionKeeper.Session.SessionGUID, ReportsDocModes.None) || complect == null || complect.Items.Count == 0)
        return;
      List<ReportsBaseDoc> docList = new List<ReportsBaseDoc>();
      complect.CollectDocItem(docList, typeof (ReportsDoc));
      bool flag = true;
      foreach (ReportsBaseDoc reportsBaseDoc in docList)
      {
        if (reportsBaseDoc != null && reportsBaseDoc.ObjectID != 0L)
        {
          IDBAttribute attributeById = sessionKeeper.Session.GetObject(reportsBaseDoc.ObjectID, false)?.GetAttributeByID(ReportsConsts.FileAttributeTypeID);
          if (attributeById != null)
          {
            BlobInformation blobInformation = attributeById is IBlobReader blobReader ? blobReader.OpenBlob(-1) : BlobInformation.EmptyBlobInformation();
            if (!(blobInformation.FileName == "") && blobInformation.RealFileSize != 0L)
            {
              if (flag)
              {
                eventArgs.PreferedBlobID = attributeById.AsInteger;
                flag = false;
              }
              FileBlobItem fileBlobItem = new FileBlobItem(reportsBaseDoc.ObjectID, ReportsConsts.FileAttributeTypeID, 0);
              eventArgs.Items.Add(fileBlobItem);
            }
          }
        }
      }
    }
  }
}
