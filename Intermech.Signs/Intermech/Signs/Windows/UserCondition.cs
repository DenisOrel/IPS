// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Windows.UserCondition
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

internal class UserCondition : AttributeCondition
{
  private readonly TextBox _tbUser;
  private readonly CheckBox _currentUser;

  public UserCondition(TextBox tbUser, CheckBox currentUser)
    : base(SignsHolder.SignUpAttrTypeID)
  {
    this._tbUser = tbUser;
    this._currentUser = currentUser;
  }

  public override ConditionStructure GetConditionStricture()
  {
    object conditionValue = this._currentUser == null || !this._currentUser.Checked ? (this._tbUser.Tag == null || !(this._tbUser.Tag is long) || (long) this._tbUser.Tag == 0L ? (object) null : this._tbUser.Tag) : (object) Consts.CurrentUserFunction;
    return conditionValue == null ? ConditionStructure.Empty : new ConditionStructure(this.attributeID, RelationalOperators.Equal, conditionValue, LogicalOperators.AND, 0, false);
  }

  public static bool IsOwnCondition(ConditionStructure cs)
  {
    return (cs.Attribute is int attribute1 && attribute1 == SignsHolder.SignUpAttrTypeID || cs.Attribute is Guid attribute2 && attribute2.Equals(SignsHolder.SignUpAttrTypeGuid)) && cs.RelationalOperator == RelationalOperators.Equal;
  }

  public override void Clear()
  {
    this._tbUser.Text = string.Empty;
    this._tbUser.Tag = (object) 0L;
    if (this._currentUser == null)
      return;
    this._currentUser.Checked = false;
  }

  protected override void SetConditionStructure(
    IUserSession session,
    ConditionStructure cs,
    ref bool signed)
  {
    this.Clear();
    if (cs.Attribute == null || cs.Value == null)
      return;
    if (cs.Value is long && (long) cs.Value != 0L)
    {
      this._tbUser.Text = ApplicationServices.Container.GetService<IObjectsInfoCache>().GetObjectInfo((long) cs.Value).Caption;
      this._tbUser.Tag = (object) (long) cs.Value;
      if (this._currentUser != null)
        this._currentUser.Checked = false;
    }
    else if (this._currentUser != null && Convert.ToString(cs.Value) == Consts.CurrentUserFunction)
      this._currentUser.Checked = true;
    if (cs.RelationalOperator != RelationalOperators.NotEqual)
      return;
    signed = false;
  }
}
