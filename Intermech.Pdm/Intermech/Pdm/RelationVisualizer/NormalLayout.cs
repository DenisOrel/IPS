// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.NormalLayout
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Localization;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class NormalLayout : HierarchicalLayout
{
  public NormalLayout() => this.Normalize = true;

  public override string GetAlgorithmName() => NormalLayout.AlgorithmName();

  public new static string AlgorithmName() => LocalizationHolder.rm.GetString("Pdm_rv_12");
}
