// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.S4Table
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp;

public class S4Table : Dictionary<string, object>
{
  public int AsInteger(string key, int defValue = -1)
  {
    object obj = (object) null;
    return this.TryGetValue(key, out obj) && !DBNull.Value.Equals(obj) ? Convert.ToInt32(obj) : defValue;
  }
}
