// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.TechcardDraftInfo
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class TechcardDraftInfo
{
  public readonly int DocID;
  public readonly int VersionID;
  public readonly string FileName = "";
  public Dictionary<long, BlobInformation4Import> Blobs;

  public TechcardDraftInfo(int docID, int versionID, string fileName)
  {
    this.DocID = docID;
    this.VersionID = versionID;
    this.FileName = fileName;
  }
}
