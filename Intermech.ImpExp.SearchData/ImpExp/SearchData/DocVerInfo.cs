// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.DocVerInfo
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class DocVerInfo : ObjVerInfo
{
  public long AdvanFilesDate;
  public DateTime FileDate;
  public int FileSize;
  public DateTime ContentModifiedDate;
  public int LCStep;
  public DocumentFlag Flags;
  public Dictionary<long, BlobInformation4Import> Blobs;

  public string Designation { get; private set; }

  public DocVerInfo(int id, int verID, string designation = "", int actualVerID = -1)
    : base(id, verID, actualVerID)
  {
    this.Designation = designation;
  }
}
