// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Windows.RankCondition
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

internal sealed class RankCondition : AttributeCondition
{
  private readonly ComboBox _cbRanks;

  public RankCondition(ComboBox cbRanks)
    : base(SignsHolder.RankAttrTypeID)
  {
    this._cbRanks = cbRanks;
  }

  public override ConditionStructure GetConditionStricture()
  {
    long id = (long) ((SignConditionsEditor.ComboBoxValue) this._cbRanks.SelectedItem).ID;
    return id == 0L ? ConditionStructure.Empty : new ConditionStructure(this.attributeID, RelationalOperators.Equal, (object) id, LogicalOperators.AND, 0, false);
  }

  public override void Clear()
  {
    if (this._cbRanks.Items.Count <= 0)
      return;
    this._cbRanks.SelectedIndex = this._cbRanks.Items.Count - 1;
  }

  public static bool IsOwnCondition(ConditionStructure cs)
  {
    return (cs.Attribute is int attribute1 && attribute1 == SignsHolder.RankAttrTypeID || cs.Attribute is Guid attribute2 && attribute2.Equals(SignsHolder.RankAttrTypeGuid)) && cs.RelationalOperator == RelationalOperators.Equal;
  }

  protected override void SetConditionStructure(
    IUserSession session,
    ConditionStructure cs,
    ref bool signed)
  {
    if (cs.Attribute != null && cs.Value != null && cs.Value is long)
    {
      for (int index = 0; index < this._cbRanks.Items.Count; ++index)
      {
        if ((long) ((SignConditionsEditor.ComboBoxValue) this._cbRanks.Items[index]).ID == (long) cs.Value)
        {
          this._cbRanks.SelectedIndex = index;
          break;
        }
      }
      if (cs.RelationalOperator != RelationalOperators.NotEqual)
        return;
      signed = false;
    }
    else
      this.Clear();
  }
}
