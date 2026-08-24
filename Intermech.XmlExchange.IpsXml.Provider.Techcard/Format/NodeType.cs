// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.Format.NodeType
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;

public enum NodeType
{
  Unknown = 0,
  [Description("main")] Main = 1,
  [Description("tp")] TP = 2,
  [Description("art")] Art = 3,
  [Description("tpdoc")] TPDoc = 4,
  [Description("tool")] Tool = 5,
  [Description("work")] Work = 6,
  [Description("zag")] Workpiece = 7,
  [Description("mat")] Mat = 8,
  [Description("oper")] Oper = 9,
  [Description("stepex")] StepEx = 10, // 0x0000000A
  [Description("eq")] Equipment = 11, // 0x0000000B
  [Description("step")] Step = 12, // 0x0000000C
  [Description("rez")] Rez = 13, // 0x0000000D
  [Description("route")] Route = 14, // 0x0000000E
  [Description("template")] Template = 15, // 0x0000000F
  [Description("stringelement")] StringElement = 16, // 0x00000010
  [Description("spec")] Spec = 17, // 0x00000011
  [Description("doc")] Doc = 18, // 0x00000012
  [Description("catalog")] Catalog = 19, // 0x00000013
  [Description("field")] Field = 20, // 0x00000014
  [Description("folder")] Folder = 21, // 0x00000015
  [Description("record")] Record = 22, // 0x00000016
  [Description("table")] Table = 23, // 0x00000017
  [Description("row")] Row = 24, // 0x00000018
  [Description("attrgr")] AttGr = 25, // 0x00000019
  [Description("att")] Attr = 26, // 0x0000001A
  [Description("matset")] MatSet = 27, // 0x0000001B
  [Description("inv_num")] InvNum = 28, // 0x0000001C
  [Description("ole_obj")] OleObj = 29, // 0x0000001D
  [Description("rtr_link")] RTRLink = 30, // 0x0000001E
  [Description("rte_link")] RTELink = 31, // 0x0000001F
  [Description("art_analog")] ArtAnalog = 32, // 0x00000020
  [Description("analog_replacement")] AnalogReplacement = 33, // 0x00000021
  [Description("art_effectivity")] ArtEffectivity = 34, // 0x00000022
  [Description("control_param")] ControlParam = 35, // 0x00000023
  [Description("ref_doc")] RefDoc = 36, // 0x00000024
  [Description("izw")] EcoDoc = 37, // 0x00000025
  [Description("comment")] OldComment = 38, // 0x00000026
  [Description("sketch")] Sketch = 39, // 0x00000027
  [Description("occurrence")] Occurrence = 200, // 0x000000C8
  [Description("relation")] Relation = 201, // 0x000000C9
  [Description("form")] Form = 202, // 0x000000CA
  [Description("formattribute")] FormAttribute = 203, // 0x000000CB
  [Description("mbom")] MBOM = 204, // 0x000000CC
  [Description("proc")] PROC = 205, // 0x000000CD
  [Description("pf")] WorkShowEnter = 206, // 0x000000CE
  [Description("intermech")] Intermech = 207, // 0x000000CF
}
