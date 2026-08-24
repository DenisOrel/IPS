// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Security.RightType
// Assembly: Intermech.ImpExp.Security, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B4185E78-CFCB-46F6-B1BC-486522A5A9AE
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Security.dll

#nullable disable
namespace Intermech.ImpExp.Security;

public enum RightType
{
  arOpenObject = 1,
  arReadObject = 2,
  arPrintObject = 3,
  arCopyObject = 4,
  arListObject = 5,
  arLaunchProcess = 10, // 0x0000000A
  arAddObject = 101, // 0x00000065
  arEditObject = 102, // 0x00000066
  arDeleteObject = 103, // 0x00000067
  arEditParameters = 104, // 0x00000068
  arRemoveObject = 105, // 0x00000069
  arSetupEditors = 106, // 0x0000006A
  arEditCommonClassif = 107, // 0x0000006B
  arEditStructure = 108, // 0x0000006C
  arEditTechStructure = 109, // 0x0000006D
  arCreateCommonSample = 110, // 0x0000006E
  arModifyObjOptions = 111, // 0x0000006F
  arEditParametersNoDocEdit = 112, // 0x00000070
  arAddClassif = 113, // 0x00000071
  arEditClassif = 114, // 0x00000072
  arDeleteClassif = 115, // 0x00000073
  arAddObjectVersion = 116, // 0x00000074
  arDeleteObjectVersion = 117, // 0x00000075
  arAddObjectCopy = 118, // 0x00000076
  arEditPurchased = 119, // 0x00000077
  arReadRights = 201, // 0x000000C9
  arChangeRights = 202, // 0x000000CA
  arAddChildObject = 203, // 0x000000CB
  arEditChildObject = 204, // 0x000000CC
  arDeleteChildObject = 205, // 0x000000CD
  arChangeStructure = 206, // 0x000000CE
  arDeleteThisObject = 207, // 0x000000CF
  arChangeObjectsRights = 208, // 0x000000D0
  arExtendedAPICall = 209, // 0x000000D1
  arArchiveManagement = 210, // 0x000000D2
  arEditProcess = 211, // 0x000000D3
  arAdminProcess = 212, // 0x000000D4
  arEditScripts = 213, // 0x000000D5
  arControlChangeDates = 214, // 0x000000D6
  arScheduleLRI = 215, // 0x000000D7
  arBCExport = 216, // 0x000000D8
  arBCImport = 217, // 0x000000D9
  arAddSubChildObject = 218, // 0x000000DA
  arDeleteSubChildObject = 219, // 0x000000DB
  arDocRegistry = 220, // 0x000000DC
  Default = 99999, // 0x0001869F
}
