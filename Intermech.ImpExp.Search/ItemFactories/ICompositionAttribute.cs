// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.ICompositionAttribute
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal interface ICompositionAttribute
{
  int ParamID { get; set; }

  string Name { get; set; }

  string DBField { get; set; }

  bool IsImbaseLink { get; set; }

  int Size { get; set; }

  int IsInherited { get; set; }
}
