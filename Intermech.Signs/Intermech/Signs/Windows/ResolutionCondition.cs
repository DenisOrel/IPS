// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Windows.ResolutionCondition
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Signs.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Windows;

internal class ResolutionCondition : AttributeCondition
{
  private TextBox _tbResolution;

  public ResolutionCondition(TextBox tbResolution)
    : base(SignsHolder.ResolutionAttrTypeID)
  {
    this._tbResolution = tbResolution;
  }

  public override ConditionStructure GetConditionStricture()
  {
    return this._tbResolution.Text == string.Empty ? ConditionStructure.Empty : new ConditionStructure(this.attributeID, RelationalOperators.Substring, (object) this._tbResolution.Text, LogicalOperators.AND, 0, false);
  }

  public static bool IsOwnCondition(ConditionStructure cs)
  {
    return (cs.Attribute is int attribute1 && attribute1 == SignsHolder.ResolutionAttrTypeID || cs.Attribute is Guid attribute2 && attribute2.Equals(SignsHolder.ResolutionAttrTypeGuid)) && cs.RelationalOperator == RelationalOperators.Substring;
  }

  public override void Clear() => this._tbResolution.Text = string.Empty;

  protected override void SetConditionStructure(
    IUserSession session,
    ConditionStructure cs,
    ref bool signed)
  {
    if (cs.Attribute != null && cs.Value != null && cs.Value is string)
    {
      this._tbResolution.Text = (string) cs.Value;
      if (cs.RelationalOperator != RelationalOperators.NotSubstring)
        return;
      signed = false;
    }
    else
      this.Clear();
  }
}
