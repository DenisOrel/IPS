// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.Document
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class Document : S4DBItem
{
  private int _typeID = -1;
  public Dictionary<int, S4Table> RC = new Dictionary<int, S4Table>();

  public int ID
  {
    get
    {
      if (this._id == 0 && this.Data.ContainsKey("doc_id"))
        this._id = Convert.ToInt32(this.Data["doc_id"]);
      return this._id;
    }
  }

  public int TypeID
  {
    get
    {
      if (this._typeID == -1 && this.Data.ContainsKey("doc_type"))
        this._typeID = Convert.ToInt32(this.Data["doc_type"]);
      return this._typeID;
    }
  }

  public string ArchiveTableName => $"SECT_{0}";

  internal override void Clear()
  {
    base.Clear();
    this._typeID = -1;
    this.RC.Clear();
  }
}
