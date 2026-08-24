// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Security.ParamInfo
// Assembly: Intermech.ImpExp.Security, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B4185E78-CFCB-46F6-B1BC-486522A5A9AE
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Security.dll

#nullable disable
namespace Intermech.ImpExp.Security;

internal class ParamInfo
{
  public readonly int ArchiveID;
  public readonly string FieldName;

  public ParamInfo(int archiveID, string fieldName)
  {
    this.ArchiveID = archiveID;
    this.FieldName = fieldName;
  }
}
