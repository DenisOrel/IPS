// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.ToSAP.IMXML.IMXMLFormat
// Assembly: Intermech.XmlExchange.IpsXml.Converter.ToSAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946972C6-4ABC-4C4A-94A5-3ADC51FD9A58
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.ToSAP.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.ToSAP.IMXML;

public class IMXMLFormat
{
  public enum NodeType
  {
    ntUnknown = 0,
    [Description("main")] ntMain = 1,
    [Description("tp")] ntTP = 2,
    [Description("art")] ntArt = 3,
    [Description("tpdoc")] ntTPDoc = 4,
    [Description("tool")] ntTool = 5,
    [Description("work")] ntWork = 6,
    [Description("zag")] ntWorkpiece = 7,
    [Description("mat")] ntMat = 8,
    [Description("oper")] ntOper = 9,
    [Description("stepex")] ntStepEx = 10, // 0x0000000A
    [Description("eq")] ntEquipment = 11, // 0x0000000B
    [Description("step")] ntStep = 12, // 0x0000000C
    [Description("rez")] ntRez = 13, // 0x0000000D
    [Description("route")] ntRoute = 14, // 0x0000000E
    [Description("template")] ntTemplate = 15, // 0x0000000F
    [Description("stringelement")] ntStringElement = 16, // 0x00000010
    [Description("spec")] ntSpec = 17, // 0x00000011
    [Description("doc")] ntDoc = 18, // 0x00000012
    [Description("catalog")] ntCatalog = 19, // 0x00000013
    [Description("field")] ntField = 20, // 0x00000014
    [Description("folder")] ntFolder = 21, // 0x00000015
    [Description("record")] ntRecord = 22, // 0x00000016
    [Description("table")] ntTable = 23, // 0x00000017
    [Description("row")] ntRow = 24, // 0x00000018
    [Description("attrgr")] ntAttGr = 25, // 0x00000019
    [Description("att")] ntAttr = 26, // 0x0000001A
    [Description("matset")] ntMatSet = 27, // 0x0000001B
    [Description("inv_num")] ntInvNum = 28, // 0x0000001C
    [Description("ole_obj")] ntOleObj = 29, // 0x0000001D
    [Description("rtr_link")] ntRTRLink = 30, // 0x0000001E
    [Description("rte_link")] ntRTELink = 31, // 0x0000001F
    [Description("art_analog")] ntArtAnalog = 32, // 0x00000020
    [Description("analog_replacement")] ntAnalogReplacement = 33, // 0x00000021
    [Description("art_effectivity")] ntArtEffectivity = 34, // 0x00000022
    [Description("occurrence")] ntOccurrence = 200, // 0x000000C8
    [Description("relation")] ntRelation = 201, // 0x000000C9
    [Description("form")] ntForm = 202, // 0x000000CA
    [Description("formattribute")] ntFormAttribute = 203, // 0x000000CB
    [Description("mbom")] ntMBOM = 204, // 0x000000CC
    [Description("proc")] ntPROC = 205, // 0x000000CD
    [Description("pf")] ntWorkShowEnter = 206, // 0x000000CE
    [Description("intermech")] ntIntermech = 207, // 0x000000CF
  }

  public enum ParmType
  {
    [Description("unknown")] ptUnknown,
    [Description("search")] ptSearch,
    [Description("techcard")] ptTechcard,
  }

  public enum Attr
  {
    [Description("unknown")] atUnknown,
    [Description("id")] atId,
    [Description("name")] atName,
    [Description("ref")] atReference,
    [Description("parmtype")] atParmType,
    [Description("value")] atValue,
    [Description("elementtype")] atElementType,
    [Description("exp_user")] atExportUser,
  }

  public enum FixedParam
  {
    [Description("N_ОП_pf")] fpOperNumberInWorkshop,
  }
}
