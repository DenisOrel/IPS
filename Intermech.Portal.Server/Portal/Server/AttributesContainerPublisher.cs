// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.AttributesContainerPublisher
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server.WebPortal;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.Portal.Server.Classes.Publishers;
using System;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal class AttributesContainerPublisher : UnitPublisher
{
  public AttributesContainerPublisher(
    IUserSession session,
    TransferedObject unit,
    XmlDocument xmlDocument,
    XmlNode rootNode,
    SiteInfo info)
    : base(session, unit, xmlDocument, rootNode, info, 1)
  {
    if (rootNode == null)
      throw new ArgumentNullException(nameof (rootNode));
  }

  public override Guid Publish(
    IDBObjectCollection publishObjects,
    string enabledSites,
    GroupPublishItem item,
    PackAnalyzInfo packAnalyzInfo,
    PublishCaches caches,
    IDBRelationCollection relCollection,
    IDBRelationType relTypePublish)
  {
    ObjectInfo objectAttributes = AttributesFile.GetObjectAttributes(this.rootNode);
    AnalyzObjectInfo analyzObjectInfo = packAnalyzInfo.AnalyzObjectInfo[objectAttributes.ObjectGuid];
    if (analyzObjectInfo.ID == 0L)
      throw new Exception(string.Format(LocalizationHolder.rm.GetString("PortalServer_34"), (object) objectAttributes.ObjectGuid));
    if (TraceLog.Enabled)
    {
      TraceLog.Write($"...AttributesContainerPublisher: ObjectGuid={objectAttributes.ObjectGuid} Guid={objectAttributes.Guid} Caption={objectAttributes.Caption} LinkedGuid={objectAttributes.LinkedGuid} ParentGuid={objectAttributes.ParentGuid} ObjTypeName={objectAttributes.ObjTypeName} ObjectTypeGuid={objectAttributes.ObjectTypeGuid}");
      TraceLog.Write($"...AnalyzObjectInfo: ID={analyzObjectInfo.ID} PublishEnable={analyzObjectInfo.PublishEnable} Deleted={analyzObjectInfo.Deleted}");
    }
    IDBObject dbObject = this.session.GetObject(analyzObjectInfo.ID, true);
    if (TraceLog.Enabled)
      TraceLog.Write("...start AddRemarks");
    new Remarks(this.session, dbObject, this.info.Code).Add(this.directoryName, this.xmlDocument, this.rootNode, enabledSites);
    if (TraceLog.Enabled)
      TraceLog.Write("...end AddRemarks");
    this.AddEvent(analyzObjectInfo.ID, this.LogName(objectAttributes), "Опубликованы замечания");
    IDBAttribute attributeByGuid1 = dbObject.GetAttributeByGuid(PortalConsts.attributeOwner);
    IDBAttribute attributeByGuid2 = dbObject.GetAttributeByGuid(PortalConsts.attributeCompositionOwner);
    if (attributeByGuid1.AsString.Equals(this.info.Code.ToString()))
      PublishHelper.SetSiteCodes(dbObject, this.unit.Tag as ObjectTag, TransferedObjectCategory.Object, enabledSites, this.info.Code.ToString(), packAnalyzInfo.SiteForUpdate, packAnalyzInfo.IsAutoTransfer, false, analyzObjectInfo.InComposition);
    else if (attributeByGuid2.AsString.Equals(this.info.Code.ToString()))
      PublishHelper.SetCompositionOwnerCodes(dbObject, packAnalyzInfo.IsAutoTransfer, this.unit.Tag as ObjectTag, this.info.Code.ToString());
    if (objectAttributes.LinkedGuid != Guid.Empty)
    {
      PublishHelper.AddAtribute((IDBAttributable) dbObject, IDHelper.AttributeLinkedGuidID, (object) objectAttributes.LinkedGuid);
      this.AddEvent(ActionType.EditProperties, analyzObjectInfo.ID, this.LogName(objectAttributes), $"Установлен GUID связанного объекта {objectAttributes.LinkedGuid}");
      if (TraceLog.Enabled)
        TraceLog.Write($"...add AttributeLinkedGuid={objectAttributes.LinkedGuid}");
    }
    return dbObject.ObjectGUID;
  }

  private string LogName(ObjectInfo objectInfo)
  {
    return $"{objectInfo.Caption} ({objectInfo.ObjectGuid})";
  }
}
