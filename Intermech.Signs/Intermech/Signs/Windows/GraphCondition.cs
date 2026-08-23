// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Windows.GraphCondition
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

internal sealed class GraphCondition : AttributeCondition
{
  private readonly ComboBox _cbGraph;

  public GraphCondition(ComboBox cbGraph)
    : base(SignsHolder.GraphAttrTypeID)
  {
    this._cbGraph = cbGraph;
  }

  public override ConditionStructure GetConditionStricture()
  {
    string id = (string) ((SignConditionsEditor.ComboBoxValue) this._cbGraph.SelectedItem).ID;
    return !(id != string.Empty) ? ConditionStructure.Empty : new ConditionStructure(this.attributeID, RelationalOperators.Equal, (object) id, LogicalOperators.AND, 0, false);
  }

  public override void Clear()
  {
    if (this._cbGraph.Items.Count <= 0)
      return;
    this._cbGraph.SelectedIndex = this._cbGraph.Items.Count - 1;
  }

  public static bool IsOwnCondition(ConditionStructure cs)
  {
    return (cs.Attribute is int attribute1 && attribute1 == SignsHolder.GraphAttrTypeID || cs.Attribute is Guid attribute2 && attribute2.Equals(SignsHolder.GraphAttrTypeGuid)) && cs.RelationalOperator == RelationalOperators.Equal;
  }

  protected override void SetConditionStructure(
    IUserSession session,
    ConditionStructure cs,
    ref bool signed)
  {
    string str = Convert.ToString(cs.Value);
    if (cs.Attribute != null && !string.IsNullOrEmpty(str))
    {
      for (int index = 0; index < this._cbGraph.Items.Count; ++index)
      {
        SignConditionsEditor.ComboBoxValue comboBoxValue = (SignConditionsEditor.ComboBoxValue) this._cbGraph.Items[index];
        if (str.Equals((string) comboBoxValue.ID))
        {
          this._cbGraph.SelectedIndex = index;
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
