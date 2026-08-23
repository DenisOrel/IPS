// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Windows.DateOfSignatureCondition
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using DevExpress.IM.XtraEditors;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Signs.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Windows;

internal sealed class DateOfSignatureCondition : AttributeCondition
{
  private DateEdit _deStart;
  private DateEdit _deEnd;
  private CheckBox _cbStart;
  private CheckBox _cbEnd;

  public DateOfSignatureCondition(
    DateEdit deStart,
    CheckBox cbStart,
    DateEdit deEnd,
    CheckBox cbEnd)
    : base(SignsHolder.DateOfSignatureID)
  {
    this._deStart = deStart;
    this._deEnd = deEnd;
    this._cbStart = cbStart;
    this._cbEnd = cbEnd;
  }

  public override ConditionStructure GetConditionStricture()
  {
    bool flag1 = this._deStart.EditValue != null || this._cbStart.Checked;
    object conditionValue = (object) null;
    if (flag1)
      conditionValue = this._cbStart.Checked ? (object) Consts.CurrentDateFunction : this._deStart.EditValue;
    bool flag2 = this._deEnd.EditValue != null || this._cbEnd.Checked;
    object obj = (object) null;
    if (flag2)
      obj = this._cbEnd.Checked ? (object) Consts.CurrentDateFunction : this._deEnd.EditValue;
    if (flag1 & flag2)
      return new ConditionStructure(this.attributeID, RelationalOperators.Between, conditionValue, obj, LogicalOperators.AND, 0, false);
    if (flag1)
      return new ConditionStructure(this.attributeID, RelationalOperators.GreaterOrEqual, conditionValue, LogicalOperators.AND, 0, false);
    return flag2 ? new ConditionStructure(this.attributeID, RelationalOperators.LessOrEqual, obj, LogicalOperators.AND, 0, false) : ConditionStructure.Empty;
  }

  public static bool IsOwnCondition(ConditionStructure cs)
  {
    if ((!(cs.Attribute is int attribute1) || attribute1 != SignsHolder.DateOfSignatureID) && (!(cs.Attribute is Guid attribute2) || !attribute2.Equals(SignsHolder.DateOfSignatureGuid)))
      return false;
    return cs.RelationalOperator == RelationalOperators.Between || cs.RelationalOperator == RelationalOperators.GreaterOrEqual || cs.RelationalOperator == RelationalOperators.LessOrEqual;
  }

  public override void Clear()
  {
    this._deStart.EditValue = (object) null;
    this._deEnd.EditValue = (object) null;
    this._cbStart.Checked = false;
    this._cbEnd.Checked = false;
  }

  protected override void SetConditionStructure(
    IUserSession session,
    ConditionStructure cs,
    ref bool signed)
  {
    this.Clear();
    if (cs.Attribute == null)
      return;
    if (cs.RelationalOperator == RelationalOperators.Between || cs.RelationalOperator == RelationalOperators.NotBetween)
    {
      if (cs.Value.Equals((object) Consts.CurrentDateFunction))
        this._cbStart.Checked = true;
      else
        this._deStart.EditValue = (object) (DateTime) cs.Value;
      if (cs.Value2.Equals((object) Consts.CurrentDateFunction))
        this._cbEnd.Checked = true;
      else
        this._deEnd.EditValue = (object) (DateTime) cs.Value2;
    }
    else if (cs.RelationalOperator == RelationalOperators.GreaterOrEqual || cs.RelationalOperator == RelationalOperators.Less)
    {
      if (cs.Value.Equals((object) Consts.CurrentDateFunction))
        this._cbStart.Checked = true;
      else
        this._deStart.EditValue = (object) (DateTime) cs.Value;
    }
    else if (cs.RelationalOperator == RelationalOperators.Greater || cs.RelationalOperator == RelationalOperators.LessOrEqual)
    {
      if (cs.Value.Equals((object) Consts.CurrentDateFunction))
        this._cbEnd.Checked = true;
      else
        this._deEnd.EditValue = (object) (DateTime) cs.Value;
    }
    if (cs.RelationalOperator != RelationalOperators.NotBetween && cs.RelationalOperator != RelationalOperators.Less && cs.RelationalOperator != RelationalOperators.Greater)
      return;
    signed = false;
  }
}
