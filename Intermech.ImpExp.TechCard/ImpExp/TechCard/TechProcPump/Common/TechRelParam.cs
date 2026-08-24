// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechRelParam
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Interfaces;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

internal class TechRelParam
{
  private RelationRecord _relRec;
  private int _relType;
  private long _ipsObjectAid;
  private int _ipsObjTypeA;
  private long _ipsObjectBid;
  private int _ipsObjTypeB;
  private int _sort;

  public TechRelParam(
    long ipsObjectB,
    long ipsObjectA,
    int ipsRelType,
    int ipsObjTypeB,
    int ipsObjTypeA)
  {
    this._ipsObjectBid = ipsObjectB;
    this._ipsObjectAid = ipsObjectA;
    this._relType = ipsRelType;
    this._ipsObjTypeB = ipsObjTypeB;
    this._ipsObjTypeA = ipsObjTypeA;
  }

  public RelationRecord RelRec
  {
    get => this._relRec;
    set => this._relRec = value;
  }

  public int RelType
  {
    get => this._relType;
    set => this._relType = value;
  }

  public long IpsObjectAid
  {
    get => this._ipsObjectAid;
    set => this._ipsObjectAid = value;
  }

  public long IpsObjectBid
  {
    get => this._ipsObjectBid;
    set => this._ipsObjectBid = value;
  }

  public int IpsObjTypeA
  {
    get => this._ipsObjTypeA;
    set => this._ipsObjTypeA = value;
  }

  public int IpsObjTypeB
  {
    get => this._ipsObjTypeB;
    set => this._ipsObjTypeB = value;
  }

  public int Sort
  {
    get => this._sort;
    set => this._sort = value;
  }
}
