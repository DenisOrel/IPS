// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.IVariantsItem
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal interface IVariantsItem
{
  int PrjLinkID { get; }

  int ProjAID { get; }

  int PartAID { get; }

  double CountPC { get; }

  int MuID { get; }

  int Razdel { get; }

  string Positio { get; }

  string Note { get; }

  string VarMode { get; }

  int VarNo { get; }

  string Format { get; }

  int PrID { get; }

  int CtxID { get; }

  int CtxFL { get; }

  ArtIDInfo ArtInfo { get; }
}
