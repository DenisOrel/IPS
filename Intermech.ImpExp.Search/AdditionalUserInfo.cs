// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.AdditionalUserInfo
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;

#nullable disable
namespace Intermech.ImpExp.Search;

internal struct AdditionalUserInfo(ImportingCategory category, long userID)
{
  public ImportingCategory Category = category;
  public long UserID = userID;
  public long ProjectID = 0;
  public long PartID = 0;

  public AdditionalUserInfo(ImportingCategory category, long userID, long projectID, long partID)
    : this(category, userID)
  {
    this.ProjectID = projectID;
    this.PartID = partID;
  }
}
