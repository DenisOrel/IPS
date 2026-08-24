// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImportedObjectClass
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

internal enum ImportedObjectClass : short
{
  DocTypesSettings = 1,
  ImbaseGroup = 2,
  ImbaseTableAttributes = 3,
  ImbaseGroupAttributes = 4,
  AttributeType = 5,
  Archive = 6,
  ObjectType = 7,
  Article = 8,
  Document = 9,
  Composition = 10, // 0x000A
  TechDiffTag = 11, // 0x000B
  [Obsolete] TechMOKeyTag = 12, // 0x000C
  TechObjectTag = 13, // 0x000D
  LCSteps4Archives = 14, // 0x000E
  ObjectInfo = 15, // 0x000F
  VComposition = 16, // 0x0010
  Sign = 17, // 0x0011
  Material = 18, // 0x0012
  TechDraft = 19, // 0x0013
  ImbaseTableLinks = 20, // 0x0014
  User = 21, // 0x0015
  TechRecObjectTag = 22, // 0x0016
  ArticleOptions = 23, // 0x0017
  ListIntTag = 24, // 0x0018
  Blob = 25, // 0x0019
  ImbaseLinks = 26, // 0x001A
  TableAttributePV = 27, // 0x001B
  ImportingObject = 28, // 0x001C
  ImportingRelation = 29, // 0x001D
  SearchArticleID = 30, // 0x001E
  ProcRouteEntry = 31, // 0x001F
  ProcRoutes = 32, // 0x0020
  ProductionCopyInfo = 33, // 0x0021
  ObjectInfoEx = 34, // 0x0022
}
