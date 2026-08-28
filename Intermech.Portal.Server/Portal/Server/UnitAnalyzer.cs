// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.UnitAnalyzer
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server.WebPortal;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal abstract class UnitAnalyzer : IUnitAnalyzer
{
  protected TransferedObject unit;
  protected XmlNode rootNode;
  protected ISitesCacheService cacheService;
  protected SiteInfo info;
  protected IUserSession session;

  public UnitAnalyzer(
    IUserSession session,
    ISitesCacheService cacheService,
    SiteInfo info,
    TransferedObject unit,
    XmlNode rootNode)
    : this(session, cacheService, info, unit, rootNode, false)
  {
  }

  public UnitAnalyzer(
    IUserSession session,
    ISitesCacheService cacheService,
    SiteInfo info,
    TransferedObject unit,
    XmlNode rootNode,
    bool autoTransfer)
  {
    this.unit = unit;
    this.rootNode = rootNode;
    this.AutoTransfer = autoTransfer;
    this.SiteForUpdate = string.Empty;
    this.cacheService = cacheService;
    this.info = info;
    this.session = session;
  }

  public static IUnitAnalyzer GetAnalyzer(
    IUserSession session,
    string unitFilePath,
    string enabledSites,
    ISitesCacheService cacheService,
    SiteInfo info)
  {
    TransferedObject unit;
    XmlNode info1 = UnitXmlFile.GetInfo(session, out unit, unitFilePath, new XmlDocument());
    IUnitAnalyzer analyzer = (IUnitAnalyzer) null;
    switch (unit.Category)
    {
      case TransferedObjectCategory.Object:
      case TransferedObjectCategory.ObjectLink:
      case TransferedObjectCategory.AttributesContainer:
        analyzer = (IUnitAnalyzer) new ObjectAnalyzer(session, cacheService, info, unit, info1);
        break;
      case TransferedObjectCategory.Relation:
        analyzer = (IUnitAnalyzer) new RelationAnalyzer(session, cacheService, info, unit, info1);
        break;
      case TransferedObjectCategory.AutoTransfer:
        analyzer = (IUnitAnalyzer) new AutoTransferAnalyzer(session, cacheService, info, unit, info1, enabledSites);
        break;
      case TransferedObjectCategory.IncompleteRelation:
        analyzer = (IUnitAnalyzer) new IncompleteRelationAnalyzer(session, cacheService, info, unit);
        break;
    }
    return analyzer;
  }

  protected AnalyzObjectInfo GetObjectAnalyzInfo(
    IDBObjectCollection publishObjects,
    Guid objectGuid)
  {
    AnalyzObjectInfo objectAnalyzInfo = new AnalyzObjectInfo(0L, Guid.Empty, true);
    object[] columns = new object[5]
    {
      (object) -2,
      (object) IDHelper.AttributeOwner,
      (object) IDHelper.AttributeParentSitesID,
      (object) -9,
      (object) -12
    };
    ConditionStructure conditionStructure = new ConditionStructure(PortalConsts.attributePublishObjectGUID, RelationalOperators.Equal, (object) objectGuid, LogicalOperators.AND, 0);
    DataTable dataTable = publishObjects.Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      conditionStructure
    }, columns));
    if (dataTable.Rows.Count == 0)
      dataTable = publishObjects.Select(new DBRecordSetParams(new ConditionStructure[2]
      {
        conditionStructure,
        new ConditionStructure(-9, RelationalOperators.Equal, (object) this.session.IdentHelper.DeletedID, LogicalOperators.AND, 0, false)
      }, columns));
    if (dataTable.Rows.Count > 0)
    {
      objectAnalyzInfo.ID = Convert.ToInt64(dataTable.Rows[0][0]);
      objectAnalyzInfo.GUID = new Guid(Convert.ToString(dataTable.Rows[0][4]));
      string str = Convert.ToString(dataTable.Rows[0][1]);
      if (Convert.ToString(dataTable.Rows[0][2]).IndexOf(this.info.Code) < 0 || str != string.Empty && (int) str[0] != (int) this.info.Code)
        objectAnalyzInfo.PublishEnable = false;
      objectAnalyzInfo.Deleted = Convert.ToInt32(dataTable.Rows[0][3]) == this.session.IdentHelper.DeletedID;
    }
    return objectAnalyzInfo;
  }

  protected void GetObjectInfo(
    XmlNode rootNode,
    SiteInfo info,
    IDBObjectCollection publishObjects,
    List<Guid> importedObjects,
    out ObjectInfo oi,
    out AnalyzInfo aInfo)
  {
    oi = AttributesFile.GetObjectAttributes(rootNode);
    if (oi.Guid == Guid.Empty || oi.ObjectGuid == Guid.Empty)
      throw new Exception(LocalizationHolder.rm.GetString("PortalServer_24"));
    if (this.unit.Category == TransferedObjectCategory.AttributesContainer)
    {
      if (!importedObjects.Contains(oi.ObjectGuid))
        importedObjects.Add(oi.ObjectGuid);
    }
    else
    {
      if (importedObjects.Contains(oi.ObjectGuid))
        throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_25"), (object) oi.ObjectGuid));
      importedObjects.Add(oi.ObjectGuid);
    }
    aInfo = (AnalyzInfo) this.GetObjectAnalyzInfo(publishObjects, oi.ObjectGuid);
  }

  public abstract void Analysis(
    IDBObjectCollection publishObjects,
    List<Guid> importedObjects,
    PackAnalyzInfo packAnalyzInfo,
    Dictionary<Guid, int> partCounter);

  public static Tuple<long, Guid> GetPublishObjectID(
    IDBObjectCollection publishObjects,
    Guid objectGuid)
  {
    DataTable dataTable = publishObjects.Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(PortalConsts.attributePublishObjectGUID, RelationalOperators.Equal, (object) objectGuid, LogicalOperators.AND, 0)
    }, new object[2]{ (object) -2, (object) -12 }));
    return dataTable.Rows.Count != 0 ? new Tuple<long, Guid>(Convert.ToInt64(dataTable.Rows[0][0]), new Guid(Convert.ToString(dataTable.Rows[0][1]))) : (Tuple<long, Guid>) null;
  }

  public string SiteForUpdate { get; protected set; }

  public bool AutoTransfer { get; private set; }

  public XmlNode RootNode => this.rootNode;
}
