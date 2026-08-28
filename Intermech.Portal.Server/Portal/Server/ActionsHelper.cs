// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.ActionsHelper
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using ImSSP;
using Intermech.Interfaces;
using Intermech.Interfaces.Portal;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.Portal.Server;

internal static class ActionsHelper
{
  public static string SiteInfoKeyForSession = "{0FB68BC2-C834-4C64-9805-7354DB7E550B}";
  public static string TaskDataFileName = sc_17179.ssp_webportal_server_17180();
  public static string TransferedUnitFileName = sc_17179.ssp_webportal_server_17181();

  public static int CalculatePercent(int count, int index, int startPercent, int endPercent)
  {
    if (endPercent > 100)
      endPercent = 100;
    else if (endPercent < 0)
      endPercent = 0;
    if (startPercent > endPercent)
      startPercent = endPercent;
    if (startPercent < 0)
      startPercent = 0;
    return startPercent + (endPercent - startPercent) * (index - 1) / count;
  }

  public static void ValuePresentInEnum(Type enumType, int searchValue, string paramName)
  {
    bool flag = false;
    foreach (int num in Enum.GetValues(enumType))
    {
      if (num == searchValue)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      throw new ArgumentException(string.Format(LocalizationHolder.rm.GetString("PortalServer_7"), (object) paramName));
  }

  public static string GetString(int length, BinaryReader br)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(br.ReadChars(length));
    return stringBuilder.ToString();
  }

  public static int GetAttributeTypeID(IUserSession session, Guid attributeGuid)
  {
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID(attributeGuid);
    return attributeTypeId != -10000 ? attributeTypeId : throw new Exception($"Атрибут с глобальным идентификатором {attributeGuid} в базе не найден!");
  }

  public static int CountLinks(
    IUserSession session,
    IDBObjectCollection objColl,
    IDBRelationCollection relCollection,
    long linkID,
    long masterObjectID)
  {
    IDbManager dataManager = (session as UserSession).DataManager;
    List<IDbDataParameter> dbDataParameterList = new List<IDbDataParameter>()
    {
      dataManager.Parameter(nameof (linkID), (object) Math.Abs(linkID))
    };
    string commandText = "SELECT COUNT(F_OBJECT_ID) FROM IMS_OBJECT_LINKS WHERE F_TOOBJECT_ID=:linkID";
    if (masterObjectID != 0L)
    {
      commandText += " AND F_OBJECT_ID<>:objID";
      dbDataParameterList.Add(dataManager.Parameter("objID", (object) masterObjectID));
    }
    return Convert.ToInt32(dataManager.ExecuteScalar(commandText, dbDataParameterList.ToArray()));
  }

  public static bool SetOwner(
    SiteInfo info,
    IUserSession session,
    IDBObject obj,
    bool throwException)
  {
    if (!OwnChecks.CheckPossibilityOwn(session, info, obj, throwException))
      return false;
    obj.GetAttributeByGuid(PortalConsts.attributeOwner).Value = (object) info.Code;
    obj.GetAttributeByGuid(PortalConsts.attributeCompositionOwner).Value = (object) info.Code;
    IDBAttribute attributeByGuid = obj.GetAttributeByGuid(PortalConsts.attributeCopyKeepers);
    if (attributeByGuid.AsString.IndexOf(info.Code) >= 0)
      attributeByGuid.AsString = attributeByGuid.AsString.Replace(info.Code.ToString(), string.Empty);
    return true;
  }

  public static void AddSiteCode(SiteInfo info, IDBObject obj)
  {
    IDBAttribute attributeByGuid = obj.GetAttributeByGuid(PortalConsts.attributeCopyKeepers);
    string asString = attributeByGuid.AsString;
    if (asString.IndexOf(info.Code) >= 0)
      return;
    attributeByGuid.AsString = asString + info.Code.ToString();
  }

  public static ConditionStructure[] GetConditionOnEnabledObjects(IUserSession session)
  {
    SiteInfo sessionPluginsData = (SiteInfo) session.GetSessionPluginsData((object) ActionsHelper.SiteInfoKeyForSession);
    SystemTypes filterType = ((PortalSettings) ServerServices.GetService(typeof (PortalSettings))).SitesSystemTypesIgnore ? SystemTypes.Unknown : sessionPluginsData.SystemType;
    ConditionStructure[] existingConditions = new ConditionStructure[2]
    {
      new ConditionStructure(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeEnabledSites), RelationalOperators.Empty, (object) null, (object) null, LogicalOperators.OR, 1, false, AttributeSourceTypes.Object),
      new ConditionStructure(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeEnabledSites), RelationalOperators.Substring, (object) Convert.ToString(sessionPluginsData.Code), (object) null, LogicalOperators.AND, -1, false, AttributeSourceTypes.Object)
    };
    if (filterType != SystemTypes.Unknown)
    {
      SiteInfo[] sitesFromDb = SiteInfoHelper.GetSitesFromDB(session, filterType);
      List<string> stringList = new List<string>(sitesFromDb.Length);
      foreach (SiteInfo siteInfo in sitesFromDb)
        stringList.Add(siteInfo.Code.ToString());
      existingConditions = ConditionStructure.Join(new ConditionStructure(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeFirstPublishSite), RelationalOperators.In, (object) stringList.ToArray(), (object) null, LogicalOperators.AND, 0, false, AttributeSourceTypes.Object), existingConditions);
    }
    return existingConditions;
  }

  public static bool IsObjectOwner(SiteInfo info, IDBObject dBObject)
  {
    IDBAttribute attributeByGuid = dBObject.GetAttributeByGuid(PortalConsts.attributeOwner);
    return info.Code.ToString().Equals(attributeByGuid.AsString);
  }
}
