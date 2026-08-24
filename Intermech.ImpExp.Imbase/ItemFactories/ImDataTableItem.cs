// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImDataTableItem
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal class ImDataTableItem
{
  public int RecKey;
  public long ObjectID;
  public bool IsTableLink;
  public bool IsMixTableLink;
  public Dictionary<Guid, object> Data = new Dictionary<Guid, object>();
  public Dictionary<string, object> FieldsValues = new Dictionary<string, object>();
}
