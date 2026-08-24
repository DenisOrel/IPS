// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareRuleComboItem
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class CompareRuleComboItem
{
  public Guid RuleID { get; private set; }

  public string Name { get; set; }

  public bool IsVirtual { get; private set; }

  public CompareRuleComboItem(Guid ruleID, string name)
  {
    this.RuleID = ruleID;
    this.Name = name;
    this.IsVirtual = VirtualCompoitionSettings.VirtualSchemes.ContainsKey(ruleID);
  }

  public override string ToString() => this.Name;
}
