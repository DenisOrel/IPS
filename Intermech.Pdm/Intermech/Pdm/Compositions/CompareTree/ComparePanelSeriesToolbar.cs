// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.ComparePanelSeriesToolbar
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Interfaces.Client;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class ComparePanelSeriesToolbar : SeriesToolbar
{
  private readonly ToolBarContainer _toolBarContainer;

  public ComparePanelSeriesToolbar(
    IFiltrationService filtrationService,
    ToolBarContainer toolBarContainer)
    : base(filtrationService)
  {
    this._toolBarContainer = toolBarContainer;
  }

  protected override Guid guid => Guid.NewGuid();

  protected override void AddToolbar()
  {
    this._toolBarContainer.Controls.Add((Control) this.toolbar);
    this.toolbar.DockLine = 3;
    this.toolbar.DockOffset = 0;
    this.toolbar.Location = new Point(0, 0);
    this.toolbar.Hidden = false;
    this.toolbar.Visible = true;
  }
}
