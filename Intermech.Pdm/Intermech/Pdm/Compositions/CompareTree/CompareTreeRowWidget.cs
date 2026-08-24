// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareTreeRowWidget
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

public class CompareTreeRowWidget(PanelWidget panelWidget, Row row) : RowWidget(panelWidget, row)
{
  protected override void CollapseRow()
  {
    if ((this.Tree as CompareTreeView).DisableExpandCollapse)
      return;
    base.CollapseRow();
  }

  protected override void ExpandRow()
  {
    if ((this.Tree as CompareTreeView).DisableExpandCollapse)
      return;
    base.ExpandRow();
  }
}
