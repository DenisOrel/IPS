// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Classes.SignConditionCommandSubscriber
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Signs.Interfaces;
using Intermech.Signs.Properties;
using Intermech.Signs.Windows;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Classes;

internal class SignConditionCommandSubscriber : ISelectionFormCustomCommandsSubscriber
{
  private readonly string _buttonName = "Signs.AddSignsCondition";

  public List<SelectionFormCommand> Buttons
  {
    get
    {
      return new List<SelectionFormCommand>(1)
      {
        new SelectionFormCommand(this._buttonName, "Поиск по атрибутам подписей", 1, (Image) Resources.SignConditions, new SelectionFormCommandExecHandler(this.OnClick))
      };
    }
  }

  public bool EnableButton(
    ConditionStructure[] allConditions,
    ConditionStructure current,
    string name,
    ref bool handled)
  {
    if (!name.Equals(this._buttonName))
      return false;
    handled = true;
    return true;
  }

  private void OnClick(object sender, SelectionFormCommandExecEventArgs e)
  {
    SignConditionsEditor conditionsEditor = new SignConditionsEditor();
    bool flag = false;
    if (this.IsSignCondition(e.Condition))
    {
      conditionsEditor.Condition = e.Condition;
      flag = true;
    }
    if (conditionsEditor.ShowDialog() != DialogResult.OK)
      return;
    if (flag)
      e.SelectionForm.Replace(conditionsEditor.Condition);
    else
      e.SelectionForm.Add(conditionsEditor.Condition);
  }

  private bool IsSignCondition(ConditionStructure cs)
  {
    bool flag = cs.RelationalOperator == RelationalOperators.ConsistFromType && cs.Value != null && cs.Value is int && ((int) cs.Value == SignsHolder.SignObjectTypeID || (int) cs.Value == SignsHolder.CryptoSignObjectTypeID);
    if (flag && cs.NestedConditions != null)
    {
      foreach (ConditionStructure nestedCondition in cs.NestedConditions)
      {
        if (!SignConditionsEditor.IsOwnCondition(nestedCondition))
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  public ConditionStructure Edit(ConditionStructure current, ref bool handled)
  {
    if (!this.IsSignCondition(current))
      return ConditionStructure.Empty;
    handled = true;
    SignConditionsEditor conditionsEditor = new SignConditionsEditor()
    {
      Condition = current
    };
    return conditionsEditor.ShowDialog() != DialogResult.OK ? ConditionStructure.Empty : conditionsEditor.Condition;
  }
}
