// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.CatalogPres
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using System;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal class CatalogPres
{
  public Guid ID;
  public string Name;
  public ImTablesType Type;

  public CatalogPres(Guid id, string name, ImTablesType type)
  {
    this.ID = id;
    this.Name = name;
    this.Type = type;
  }
}
