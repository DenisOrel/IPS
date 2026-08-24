// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ImportingRelationEx
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[Serializable]
internal class ImportingRelationEx(RelationRecord rec) : ImportingRelation(rec)
{
  [NonSerialized]
  public CacheCategory Cache;
  [NonSerialized]
  public ITagImportObject Tag;
  [NonSerialized]
  public object OldKey;
  [NonSerialized]
  public string DocLinkKey = "";
}
