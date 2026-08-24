// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.TreeStateSyncronyzer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class TreeStateSyncronyzer
{
  private bool _selfScroll;
  private Intermech.VirtualTreeView.VirtualTreeView _virtualTreeView1;
  private Intermech.VirtualTreeView.VirtualTreeView _virtualTreeView2;
  private bool _selfExpand;
  private bool _selfSelected;

  public event TreeRowExpanded TreeRowExpandedEvent;

  public TreeStateSyncronyzer(
    Intermech.VirtualTreeView.VirtualTreeView virtualTreeView1,
    Intermech.VirtualTreeView.VirtualTreeView virtualTreeView2,
    VScrollBar scrollBar1,
    VScrollBar scrollBar2)
  {
    this._virtualTreeView1 = virtualTreeView1;
    this._virtualTreeView2 = virtualTreeView2;
    virtualTreeView1.RowExpand += new RowEventHandler(this.VirtualTreeView1_RowExpand);
    virtualTreeView1.RowCollapse += new RowEventHandler(this.VirtualTreeView1_RowCollapse1);
    virtualTreeView1.SelectionChanged += new EventHandler(this.VirtualTreeView1_SelectionChanged);
    virtualTreeView2.RowExpand += new RowEventHandler(this.VirtualTreeView2_RowExpand);
    virtualTreeView2.RowCollapse += new RowEventHandler(this.VirtualTreeView2_RowCollapse);
    virtualTreeView2.SelectionChanged += new EventHandler(this.VirtualTreeView2_SelectionChanged);
    scrollBar1.ValueChanged += new EventHandler(this.ScrollBar1_ValueChanged);
    scrollBar2.ValueChanged += new EventHandler(this.ScrollBar2_ValueChanged);
  }

  private void ScrollBar2_ValueChanged(object sender, EventArgs e)
  {
    if (this._selfScroll)
      return;
    this._selfScroll = true;
    try
    {
      this._virtualTreeView1.TopRowIndex = this._virtualTreeView2.TopRowIndex;
    }
    finally
    {
      this._selfScroll = false;
    }
  }

  private void ScrollBar1_ValueChanged(object sender, EventArgs e)
  {
    if (this._selfScroll)
      return;
    this._selfScroll = true;
    try
    {
      this._virtualTreeView2.TopRowIndex = this._virtualTreeView1.TopRowIndex;
    }
    finally
    {
      this._selfScroll = false;
    }
  }

  private void VirtualTreeView2_SelectionChanged(object sender, EventArgs e)
  {
    this.SyncSelected(this._virtualTreeView2, this._virtualTreeView1);
  }

  private void VirtualTreeView1_SelectionChanged(object sender, EventArgs e)
  {
    this.SyncSelected(this._virtualTreeView1, this._virtualTreeView2);
  }

  private void SyncSelected(Intermech.VirtualTreeView.VirtualTreeView selectedView, Intermech.VirtualTreeView.VirtualTreeView targetView)
  {
    if (this._selfSelected || selectedView.SelectedRow == null)
      return;
    int rowIndex = selectedView.SelectedRow.RowIndex;
    this._selfSelected = true;
    try
    {
      targetView.SelectedRow = targetView.GetRow(rowIndex);
    }
    finally
    {
      this._selfSelected = false;
    }
  }

  private void VirtualTreeView2_RowCollapse(object sender, RowEventArgs e)
  {
    this.SyncExpanded(this._virtualTreeView1, e);
  }

  private void VirtualTreeView2_RowExpand(object sender, RowEventArgs e)
  {
    this.SyncExpanded(this._virtualTreeView1, e);
  }

  private void VirtualTreeView1_RowCollapse1(object sender, RowEventArgs e)
  {
    this.SyncExpanded(this._virtualTreeView2, e);
  }

  private void VirtualTreeView1_RowExpand(object sender, RowEventArgs e)
  {
    Row row = this._virtualTreeView2.GetRow(e.Row.RowIndex);
    TreeRowExpanded rowExpandedEvent = this.TreeRowExpandedEvent;
    if (rowExpandedEvent != null)
      rowExpandedEvent((object) this, new TreeRowExpandedEventArgs(e.Row, row));
    this.SyncExpanded(this._virtualTreeView2, e);
  }

  private void SyncExpanded(Intermech.VirtualTreeView.VirtualTreeView targetView, RowEventArgs e)
  {
    if (this._selfExpand)
      return;
    int rowIndex = e.Row.RowIndex;
    Row row = targetView.GetRow(rowIndex);
    if (row == null)
      return;
    this._selfExpand = true;
    try
    {
      row.Expanded = e.Row.Expanded;
    }
    finally
    {
      this._selfExpand = false;
    }
  }
}
