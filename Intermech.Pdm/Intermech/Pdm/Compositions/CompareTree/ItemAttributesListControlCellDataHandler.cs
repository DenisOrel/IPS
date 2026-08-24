// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ItemAttributesListControlCellDataHandler
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Interfaces.Pdm;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class ItemAttributesListControlCellDataHandler : AttributesListControlCellDataHandler
{
  private CompositionItemAttribute item;

  public ItemAttributesListControlCellDataHandler(CompositionItemAttribute item)
  {
    this.item = item;
  }

  public override void SetBackColor(GetCellDataEventArgs e)
  {
    CompositionAttributeState compositionAttributeState = CompositionAttributeState.Equal;
    if ((this.item.State & CompositionAttributeState.Dummy) != CompositionAttributeState.Dummy)
      compositionAttributeState = this.item.State;
    if ((compositionAttributeState & CompositionAttributeState.Equal) != CompositionAttributeState.None)
      return;
    StyleDelta styleDelta = new StyleDelta()
    {
      HorzAlignment = e.Column.CellStyle.HorzAlignment
    };
    if ((compositionAttributeState & CompositionAttributeState.Removed) == CompositionAttributeState.Removed)
      styleDelta.BackColor = ControlsHelper.RemovedColor;
    else if ((compositionAttributeState & CompositionAttributeState.Added) == CompositionAttributeState.Added)
      styleDelta.BackColor = ControlsHelper.AddedColor;
    else if ((compositionAttributeState & CompositionAttributeState.Changed) == CompositionAttributeState.Changed)
      styleDelta.BackColor = ControlsHelper.ChangedColor;
    StyleDelta delta1 = new StyleDelta()
    {
      BackColor = styleDelta.BackColor,
      HorzAlignment = styleDelta.HorzAlignment
    };
    e.CellData.OddStyle = new Style(e.Row.Tree.RowOddStyle, delta1);
    StyleDelta delta2 = new StyleDelta()
    {
      BackColor = styleDelta.BackColor,
      HorzAlignment = styleDelta.HorzAlignment
    };
    e.CellData.EvenStyle = new Style(e.Row.Tree.RowEvenStyle, delta2);
  }

  protected override object Value => this.item.Count != 0 ? (object) null : this.item.Value;

  protected override string Description
  {
    get => this.item.Count != 0 ? string.Empty : this.item.Description;
    set => this.item.Description = value;
  }

  protected override int AttributeID => this.item.AttributeID;

  protected override string AttributeColumnValue => this.item.AttributeName;

  protected override bool IsDummyItem
  {
    get => (this.item.State & CompositionAttributeState.Dummy) == CompositionAttributeState.Dummy;
  }
}
