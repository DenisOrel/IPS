// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VirtualExemplars.VirtualRelation
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.Pdm.VirtualExemplars;

internal class VirtualRelation
{
  public Guid ParentExemplar;
  public Guid ChildExemplar;
  public MeasuredValue Quantity;

  public VirtualRelation(Guid parentExemplar, Guid childExemplar, MeasuredValue quantity)
  {
    this.ParentExemplar = parentExemplar;
    this.ChildExemplar = childExemplar;
    this.Quantity = quantity;
  }
}
