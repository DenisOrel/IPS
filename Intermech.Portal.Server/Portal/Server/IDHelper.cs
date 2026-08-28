// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.IDHelper
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal static class IDHelper
{
  public static int AttributePublishInCompositionID;
  public static int AttributeObjectTypeGuidID;
  public static int AttributeObjTypeNameID;
  public static int AttributeLinkedGuidID;
  public static int AttributePublishObjectGuidID;
  public static int AttributePublishGuidID;
  public static int AttributeRootTypePublishObjectID;
  public static int AttributeSitesForUpdateID;
  public static int AttributeCopyKeepersID;
  public static int AttributeRelationTypeGuidID;
  public static int AttributeRelTypeNameID;
  public static int AttributeVersionInRelationID;
  public static int AttributeOwner;
  public static int AttributeParentSitesID;
  public static int ObjtypePublishID;
  public static int ReltypePublishID;
  public static int AttributePublishLinksID;
  public static int AttributePublishGuidLinksID;
  public static int AttributeEnableSitesID;
  public static int AttributeVerCodeID;
  public static int AttributeErrorTextID;

  public static void Initialize(IUserSession session)
  {
    IDHelper.AttributeErrorTextID = session.GetAttributeType(new Guid("cadd95b8-306c-11d8-b4e9-00304f19f545")).AttributeID;
    IDHelper.AttributePublishInCompositionID = session.GetAttributeType(PortalConsts.attributePublishInComposition, true).AttributeID;
    IDHelper.AttributeObjectTypeGuidID = session.GetAttributeType(new Guid("cad001a0-306c-11d8-b4e9-00304f19f545"), true).AttributeID;
    IDHelper.AttributeObjTypeNameID = session.GetAttributeType(PortalServerConsts.attributeObjTypeName, true).AttributeID;
    IDHelper.AttributeLinkedGuidID = session.GetAttributeType(PortalConsts.attributeLinkedGuid, true).AttributeID;
    IDHelper.AttributePublishObjectGuidID = session.GetAttributeType(PortalConsts.attributePublishObjectGUID, true).AttributeID;
    IDHelper.AttributePublishGuidID = session.GetAttributeType(PortalServerConsts.attributePublishGUID, true).AttributeID;
    IDHelper.AttributeRootTypePublishObjectID = session.GetAttributeType(PortalConsts.attributeRootTypePublishObject, true).AttributeID;
    IDHelper.AttributeSitesForUpdateID = session.GetAttributeType(PortalConsts.attributeSitesForUpdate, true).AttributeID;
    IDHelper.AttributeCopyKeepersID = session.GetAttributeType(PortalConsts.attributeCopyKeepers, true).AttributeID;
    IDHelper.AttributeRelationTypeGuidID = session.GetAttributeType(new Guid("cad001a9-306c-11d8-b4e9-00304f19f545"), true).AttributeID;
    IDHelper.AttributeRelTypeNameID = session.GetAttributeType(PortalConsts.attributeRelTypeName, true).AttributeID;
    IDHelper.AttributeVersionInRelationID = session.GetAttributeType(new Guid("cad001c2-306c-11d8-b4e9-00304f19f545"), true).AttributeID;
    IDHelper.AttributeOwner = session.GetAttributeType(PortalConsts.attributeOwner, true).AttributeID;
    IDHelper.AttributeParentSitesID = session.GetAttributeType(PortalConsts.attributeParentSites, true).AttributeID;
    IDHelper.ObjtypePublishID = session.GetObjectType(PortalConsts.objtypePublishObjects, true).ObjectType;
    IDHelper.ReltypePublishID = session.GetRelationType(PortalConsts.reltypePublish, true).RelationType;
    IDHelper.AttributePublishLinksID = session.GetAttributeType(PortalConsts.attributePublishLinksGuid, true).AttributeID;
    IDHelper.AttributePublishGuidLinksID = session.GetAttributeType(PortalServerConsts.attributePublishGuidLinksGuid, true).AttributeID;
    IDHelper.AttributeVerCodeID = session.GetAttributeType(PortalConsts.attributeVerCode, true).AttributeID;
    IDHelper.AttributeEnableSitesID = session.GetAttributeType(PortalConsts.attributeEnabledSites, true).AttributeID;
  }
}
