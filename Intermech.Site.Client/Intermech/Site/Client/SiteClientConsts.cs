// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.SiteClientConsts
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using System;

#nullable disable
namespace Intermech.Site.Client;

public class SiteClientConsts
{
  public static string CommandAutoImportComplete = "Intermech.Site.Client.AutoImportComplete";
  public static string CommandAutoImportCompleteCaption = "Завершить автоимпорт";
  public static string CommandEndAutoPublish = "Intermech.Site.Client.EndAutoPublish";
  public static string CommandEndAutoPublishCaption = "Отменить автопубликацию";
  public static string CommandPublishTableLinks = "Intermech.Site.Client.PublishTableLinks";
  public static string CommandPublishTableLinksCaption = "Опубликовать ярлыки на портале";
  public static string CommandSetEnterPoint = "Intermech.Site.Client.SetEnterPoint";
  public static string CommandSetEnterPointCaption = "Назначить точку ввода";
  public static string CommandToPublishName = "Intermech.Site.Client.ToPublish";
  public static string CommandToPublishCaption = LocalizationHolder.rm.GetString("Site.Client_77");
  public static string CommandImport = "Intermech.Site.Client.Import";
  public static string CommandOfflineImport = "Intermech.Site.Client.OfflineImport";
  public static string CommandImportCaption = LocalizationHolder.rm.GetString("Site.Client_78");
  public static string CommandDelete = "Intermech.Site.Client.Delete";
  public static string CommandDeleteCaption = LocalizationHolder.rm.GetString("Site.Client_79");
  public static string CommandOwnComplete = "Intermech.Site.Client.OwnComplete";
  public static string CommandOwnCompleteCaption = LocalizationHolder.rm.GetString("Site.Client_80");
  public static string CommandOpenInNewWindow = "Intermech.Site.Client.OpenInNewWindow";
  public const string SectionPublishSettings = "PUBLISH_SETTINGS";
  public const string SectionImportSettings = "IMPORT_SETTINGS";
  public const string SectionImportSettingsUncheckedTypes = "IMPORT_SETTINGS_UNCH_TYPES";
  public static string CommandTaskIncludes = "Intermech.Site.Client.TaskIncludes";
  public static string CommandTaskIncludesCaption = "Состав задачи";
  public static string CommandStartTask = "Intermech.Site.Client.StartTask";
  public static string CommandStartTaskCaption = LocalizationHolder.rm.GetString("Site.Client_81");
  public static string CommandStopTask = "Intermech.Site.Client.StopTask";
  public static string CommandStopTaskCaption = LocalizationHolder.rm.GetString("Site.Client_82");
  public static string PortalCaption = LocalizationHolder.rm.GetString("Site.Client_83");
  public static string CommandShowPortalName = "Intermech.Site.Client.ShowPortal";
  public static string PublishTypesSettingsCaption = LocalizationHolder.rm.GetString("Site.Client_84");
  public static string CommandPublishTypesSettings = "Intermech.Site.Client.PublishTypesSettings";
  public static string AutoPublishOblectsListCaption = LocalizationHolder.rm.GetString("Site.Client_108");
  public static string AutoPublishOblectsListCommand = "Intermech.Site.Client.AutoPublishOblectsList";
  public static string PluginName = LocalizationHolder.rm.GetString("Site.Client_85");
  public static Guid attributeDataBaseGuid = new Guid("cad0148a-306c-11d8-b4e9-00304f19f545");
  public static Guid objtypeToPublishGuid = new Guid("cad01489-306c-11d8-b4e9-00304f19f545");
  public static int CategoryPortal = -1;
  public static int CategoryListSites = -1;
  public static int CategorySiteNode = -1;
  public static int CategoryRootPublishType = -1;
  public static int CategoryRootListPublishObjects = -1;
  public static int CategoryRootPacketType = -1;
  public static int CategoryPublishObject = -1;
  public static int CategoryPublishPacket = -1;
  public static int CategoryPortalSelection = -1;
  public static int CategoryUserNode = -1;
  public static readonly Guid CategoryRootListPublishObjectsGuid = new Guid("{286C5DC4-BF22-470E-9AE7-2B3C8ACF5719}");
  public static readonly Guid CategoryUserNodeGuid = new Guid("{FFAFC3F8-13C1-48d9-A4DF-2B3CC824251F}");
  public static readonly Guid CategorySiteNodeGuid = new Guid("{398BA2CA-E275-47f7-9371-EA16B629490D}");
  public static readonly Guid CategoryRootPublishTypeGuid = new Guid("{E25D7BE6-5574-442a-8AFF-2E3D2AEA38F0}");
  public static readonly Guid CategoryRootPacketTypeGuid = new Guid("{985DE2D2-B527-4CB1-886D-0FD13E005305}");
  public static readonly Guid CategoryListSitesGuid = new Guid("{FC72696E-83B6-486e-B287-972E36A1F87E}");
  public static readonly Guid CategoryPortalGuid = new Guid("{C922D2FD-DEF1-4ffe-B581-812E23F3E304}");
  public static readonly Guid CategoryPublishObjectGuid = new Guid("{3AADF267-9590-4d9a-989B-28693A73C3FA}");
  public static readonly Guid CategoryPublishPacketGuid = new Guid("{F140AD7F-F8DC-4FCD-A7F6-EFA1137E3509}");
  public static readonly Guid CategoryPortalSelectionGuid = new Guid("{3B2917E9-590D-44ac-AD74-C47091B31409}");
  public static readonly Guid PublishRelationColumnSchemeGuid = new Guid("{36A32A72-E6A8-4af2-B44C-5ADED2222574}");
  public static readonly Guid PublishObjectTypeColumnSchemeGuid = new Guid("{3DA91D17-953C-4b3c-BEFB-FF0A92E1C9E7}");
  public static readonly Guid PublishObjectObligatoryColumnSchemeGuid = new Guid("{380FF576-FBBC-4a94-955A-837F36785CA8}");
  public static readonly Guid PublishUserObligatoryColumnSchemeGuid = new Guid("{7AFAB4CE-CBB8-416b-9EFF-40980E17E5F7}");
  public static readonly Guid PublishPacketsObligatoryColumnSchemeGuid = new Guid("{971377CA-9447-406E-A824-25638CC6689D}");
  public static int CategoryContains = -1;
  public static readonly Guid CategoryContainsGuid = new Guid("{83D7CA79-63A1-4365-8317-40A7B3E7D0D1}");
  public static int CategoryPublishType = -1;
  public static Guid CategoryPublishTypeGuid = new Guid("C61E0131-7074-4190-9114-BA8F4A30E5E6");
  public static readonly string PublishObjectAttributesCategoryName = LocalizationHolder.rm.GetString("Site.Client_103");
  public static readonly string ErrorInitializeHelper = LocalizationHolder.rm.GetString("Site.Client_86");
  internal static string CfgUserEnableSites = "EnableSites";
  internal static string CfgUserAutoUpdate = "AutoUpdate";
  internal static string CfgUserAutoUpdateComposition = "AutoUpdateComposition";
  internal static string CfgUserPriority = "Priority";
  internal static string CfgOwner = "Owner";
  internal static string CfgUserCompositionType = "CompositionType";
  internal static readonly string ImageExportName = "imgExportToPortal";
  internal static readonly string ImageImportName = "imgImportFromPortal";
  internal static string CfgStartImmediately = "StartImmediately";
  internal static string CfgAutoPublish = "AutoPublish";

  public static long CountRecordsInPackage(IUserSession session)
  {
    return session.Configurations.ReadInteger(PortalConsts.PortalClientModuleName, "GENERAL_SETTINGS", "RECORD_COUNT", PortalConsts.DefaultCountRecordsInPackage, DBConfigMode.UserAndGlobal);
  }
}
