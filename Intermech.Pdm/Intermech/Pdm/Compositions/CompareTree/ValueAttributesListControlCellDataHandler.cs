// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ValueAttributesListControlCellDataHandler
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.Interfaces.Pdm;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class ValueAttributesListControlCellDataHandler : 
  AttributesListControlCellDataHandler
{
  private CompositionItemAttributeValue item;

  public ValueAttributesListControlCellDataHandler(CompositionItemAttributeValue item)
  {
    this.item = item;
  }

  public override void SetBackColor(GetCellDataEventArgs e)
  {
  }

  protected override object Value => this.item.Value;

  protected override string Description
  {
    get => this.item.Description;
    set => this.item.Description = value;
  }

  protected override int AttributeID => this.item.Parent.AttributeID;

  protected override string AttributeColumnValue => $"[{this.item.Index.ToString("00")}]";

  protected override bool IsDummyItem => this.item.IsDummy;
}
