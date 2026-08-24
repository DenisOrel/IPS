// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Security.BOType
// Assembly: Intermech.ImpExp.Security, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B4185E78-CFCB-46F6-B1BC-486522A5A9AE
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Security.dll

#nullable disable
namespace Intermech.ImpExp.Security;

public enum BOType
{
  All = 0,
  Document = 2,
  Articles = 3,
  Archive = 4,
  UserGroup = 5,
  ThemeParamsGroup = 8,
  ThemeParameter = 9,
  User = 10, // 0x0000000A
  ArchiveParameter = 11, // 0x0000000B
  Scheme = 13, // 0x0000000D
  SchemesRoot = 14, // 0x0000000E
  ArticleType = 23, // 0x00000017
  Object = 37, // 0x00000025
  ClassificatorsList = 38, // 0x00000026
  Classificator = 39, // 0x00000027
}
