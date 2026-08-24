// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Ips.Utils.IPSObjectsTypesExtention
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Ips, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B2EE3099-B947-440E-865D-611E406056AB
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Ips.dll

using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using Intermech.IpsXmlViewer.Interfaces;
using System;
using XmlReaderAPI.MetaData;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Ips.Utils;

public static class IPSObjectsTypesExtention
{
  private const string AT_ISHEAD_OBJECT = "-510";
  private static string AT_PROC_ROUTE_DEFAULT = ImAttributeType.GetDictAttrKey(MetaDataHelper.GetAttributeType(TechCardConsts.AttributeTypes.ProcRouteDefaultAttrGuid).AttributeID.ToString());
  public static string ATDK_GROUP_INSTANCE = ImAttributeType.GetDictAttrKey(MetaDataHelper.GetAttributeType(new Guid("cad001f9-306c-11d8-b4e9-00304f19f545")).AttributeID.ToString());

  private static bool TargetIsObjType(IImObjectType targetType, int typeID)
  {
    return targetType != null && MetaDataHelper.IsObjectTypeChildOf(targetType.F_OBJ_TYPE, typeID);
  }

  private static bool TargetIsObjType(IImObject targetObj, int typeID)
  {
    return targetObj != null && MetaDataHelper.IsObjectTypeChildOf(targetObj.GetAsInt32("F_OBJECT_TYPE", 0), typeID);
  }

  public static bool IsArticleObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.ArticleBaseID);
  }

  public static bool IsArticle(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.ArticleBaseID);
  }

  public static bool IsSBArticle(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, MetaDataHelper.GetObjectTypeID("cad00132-306c-11d8-b4e9-00304f19f545"));
  }

  public static bool IsSpecification(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, MetaDataHelper.GetObjectTypeID("cad00133-306c-11d8-b4e9-00304f19f545"));
  }

  public static bool IsPartsDrawings(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, MetaDataHelper.GetObjectTypeID("cad00261-306c-11d8-b4e9-00304f19f545"));
  }

  public static bool IsModel(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, MetaDataHelper.GetObjectTypeID(TechCardConsts.ObjectTypes.ExternalCADModelTypeGuid));
  }

  public static bool IsPart(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, MetaDataHelper.GetObjectTypeID("cad00250-306c-11d8-b4e9-00304f19f545"));
  }

  public static bool IsPartWithoutDrawing(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, MetaDataHelper.GetObjectTypeID("cad00861-306c-11d8-b4e9-00304f19f545"));
  }

  public static bool IsProcessRouteObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.ProcRoutingID);
  }

  public static bool IsProcessRoute(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.ProcRoutingID);
  }

  public static bool IsDefaultProcRoute(this IImObject targetObj)
  {
    return targetObj.IsProcessRoute() && targetObj.Attributes.ContainsKey(IPSObjectsTypesExtention.AT_PROC_ROUTE_DEFAULT) && !string.IsNullOrEmpty((targetObj.Attributes[IPSObjectsTypesExtention.AT_PROC_ROUTE_DEFAULT] as IImAttribute).GetAsString("F_VALUE", string.Empty));
  }

  public static bool IsProcessRouteEntryObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.ProcRoutingEntryID);
  }

  public static bool IsProcessRouteEntry(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.ProcRoutingEntryID);
  }

  public static bool IsRouteObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.CehRouteID);
  }

  public static bool IsRoute(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.CehRouteID);
  }

  public static bool IsTemplateObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.TemplRouteBaseID);
  }

  public static bool IsTemplate(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.TemplRouteBaseID);
  }

  public static bool IsRouteElemObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.ElemRouteID);
  }

  public static bool IsRouteElem(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.ElemRouteID);
  }

  public static bool IsTemplateRouteBaseObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.TemplRouteBaseID);
  }

  public static bool IsIsTemplateRouteBase(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.TemplRouteBaseID);
  }

  public static bool IsTechProccessObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.TechProcBaseID);
  }

  public static bool IsTechProccess(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.TechProcBaseID);
  }

  public static bool IsWorkShopEnterObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.CehZahodObjectID);
  }

  public static bool IsWorkShopEnter(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.CehZahodObjectID);
  }

  public static bool IsOperObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.OperaciyaID);
  }

  public static bool IsOper(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.OperaciyaID);
  }

  public static bool IsMaterialObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.MaterialBaseID);
  }

  public static bool IsMaterial(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.MaterialBaseID);
  }

  public static bool IsToolObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, MetaDataHelper.GetObjectTypeID(TechCardConsts.ObjectTypes.OsnastBaseGUID));
  }

  public static bool IsTool(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, MetaDataHelper.GetObjectTypeID(TechCardConsts.ObjectTypes.OsnastBaseGUID));
  }

  public static bool IsWorkpieceObjType(this IImObjectType targetType)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetType, TechCardConsts.ObjectTypes.ZagotID);
  }

  public static bool IsWorkpiece(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.ZagotID);
  }

  public static bool IsDocument(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.DocumentBaseID);
  }

  public static bool IsDocumentObjType(this IImObjectType targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.DocumentBaseID);
  }

  public static bool IsIsp(this IImObject targetObj)
  {
    return targetObj.Attributes.ContainsKey(IPSObjectsTypesExtention.ATDK_GROUP_INSTANCE);
  }

  public static bool IsEcoII(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, MetaDataHelper.GetObjectTypeID("cad00349-306c-11d8-b4e9-00304f19f545"));
  }

  public static bool IsEcoII(this IImObjectType targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, MetaDataHelper.GetObjectTypeID("cad00349-306c-11d8-b4e9-00304f19f545"));
  }

  public static bool IsPersonal(this IImObject targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.PersonalBaseID);
  }

  public static bool IsPersonal(this IImObjectType targetObj)
  {
    return IPSObjectsTypesExtention.TargetIsObjType(targetObj, TechCardConsts.ObjectTypes.PersonalBaseID);
  }

  public static bool IsHead(this IImObject targetObj)
  {
    string dictAttrKey = ImAttributeType.GetDictAttrKey("-510");
    return targetObj.Attributes.ContainsKey(dictAttrKey) && (targetObj.Attributes[dictAttrKey] as IImAttribute).Text == "1";
  }
}
