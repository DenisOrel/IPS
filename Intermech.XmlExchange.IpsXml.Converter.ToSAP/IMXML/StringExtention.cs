// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.ToSAP.IMXML.StringExtention
// Assembly: Intermech.XmlExchange.IpsXml.Converter.ToSAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946972C6-4ABC-4C4A-94A5-3ADC51FD9A58
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.ToSAP.dll

using Intermech.IpsXmlViewer.Interfaces;
using Intermech.XmlExchange.IpsXml.Provider.Ips.Utils;
using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.ToSAP.IMXML;

public static class StringExtention
{
  private static T ParseEnum<T>(string sourceString) where T : struct, IConvertible
  {
    return (T) EnumDescConverter.GetEnumValue(typeof (T), sourceString);
  }

  public static IMXMLFormat.Attr ParseAttr(this string xmlTag)
  {
    return StringExtention.ParseEnum<IMXMLFormat.Attr>(xmlTag);
  }

  public static IMXMLFormat.NodeType ParseNodeType(this string xmlTag)
  {
    return StringExtention.ParseEnum<IMXMLFormat.NodeType>(xmlTag);
  }

  public static IMXMLFormat.ParmType ParseParmType(this string xmlTag)
  {
    return StringExtention.ParseEnum<IMXMLFormat.ParmType>(xmlTag);
  }

  public static IMXMLFormat.FixedParam ParseFixedParam(this string xmlTag)
  {
    return StringExtention.ParseEnum<IMXMLFormat.FixedParam>(xmlTag);
  }

  public static IMXMLFormat.NodeType ToIMXMLNodeType(this IImObjectType targetType)
  {
    if (targetType.IsArticleObjType())
      return IMXMLFormat.NodeType.ntArt;
    if (targetType.IsMaterialObjType())
      return IMXMLFormat.NodeType.ntMat;
    if (targetType.IsOperObjType())
      return IMXMLFormat.NodeType.ntOper;
    if (targetType.IsProcessRouteObjType() || targetType.IsRouteElemObjType())
      return IMXMLFormat.NodeType.ntUnknown;
    if (targetType.IsRouteObjType())
      return IMXMLFormat.NodeType.ntRoute;
    if (targetType.IsTechProccessObjType())
      return IMXMLFormat.NodeType.ntTP;
    if (targetType.IsTemplateObjType())
      return IMXMLFormat.NodeType.ntTemplate;
    if (targetType.IsWorkpieceObjType())
      return IMXMLFormat.NodeType.ntWorkpiece;
    return targetType.IsWorkShopEnterObjType() ? IMXMLFormat.NodeType.ntWorkShowEnter : IMXMLFormat.NodeType.ntUnknown;
  }

  public static IMXMLFormat.NodeType ToIMXMLNodeType(this IImObject targetObj)
  {
    if (targetObj.IsArticle())
      return IMXMLFormat.NodeType.ntArt;
    if (targetObj.IsMaterial())
      return IMXMLFormat.NodeType.ntMat;
    if (targetObj.IsOper())
      return IMXMLFormat.NodeType.ntOper;
    if (targetObj.IsProcessRoute() || targetObj.IsRouteElem())
      return IMXMLFormat.NodeType.ntUnknown;
    if (targetObj.IsRoute())
      return IMXMLFormat.NodeType.ntRoute;
    if (targetObj.IsTechProccess())
      return IMXMLFormat.NodeType.ntTP;
    if (targetObj.IsTemplate())
      return IMXMLFormat.NodeType.ntTemplate;
    if (targetObj.IsWorkpiece())
      return IMXMLFormat.NodeType.ntWorkpiece;
    return targetObj.IsWorkShopEnter() ? IMXMLFormat.NodeType.ntWorkShowEnter : IMXMLFormat.NodeType.ntUnknown;
  }
}
