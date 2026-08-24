// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.CompositionFiltration.ContextCommand
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Search;
using Intermech.Search.CompositionContexts;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.CompositionFiltration;

internal sealed class ContextCommand(IFiltrationService filtration) : CompositionFiltrationCommand(filtration, (IMainMenuService) null)
{
  private ButtonItem _buttonContext;

  public override object Value { get; }

  public override void CreateCommand(INamedImageList namedImageList)
  {
    this._buttonContext = this.filtration.AddNewButton();
    this._buttonContext.BeginGroup = true;
    this._buttonContext.ShowText = false;
    this._buttonContext.ImageIndex = namedImageList.ImageIndex("imgContextComposition.PDM");
    this._buttonContext.AutoToggle = AutoToggleType.None;
    this._buttonContext.Text = PDMPluginConsts.buttonContextCompositionText;
    this._buttonContext.ToolTipText = PDMPluginConsts.buttonContextCompositionHint;
    this._buttonContext.Click += new EventHandler(this.ContextClick);
  }

  private void ContextClick(object sender, EventArgs e)
  {
    if (!(this.filtration.Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] is List<long> selectedContexts))
    {
      selectedContexts = ((IEnumerable<CompositionContext>) CompositionContextClientHelper.GetDefaultCompositionContexts().CompositionContexts).Select<CompositionContext, long>((Func<CompositionContext, long>) (o => o.Value)).ToList<long>();
      this.filtration.Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) selectedContexts;
    }
    Point screen = this._buttonContext.ToolBar.PointToScreen(new Point(this._buttonContext.ButtonBounds.X, this._buttonContext.ButtonBounds.Y + this._buttonContext.ToolBar.Height + 5));
    int num1 = screen.X + (int) byte.MaxValue;
    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
    int x1 = workingArea.X;
    workingArea = Screen.PrimaryScreen.WorkingArea;
    int width1 = workingArea.Width;
    int num2 = x1 + width1;
    if (num1 > num2)
    {
      ref Point local = ref screen;
      workingArea = Screen.PrimaryScreen.WorkingArea;
      int x2 = workingArea.X;
      workingArea = Screen.PrimaryScreen.WorkingArea;
      int width2 = workingArea.Width;
      int num3 = x2 + width2 - (int) byte.MaxValue;
      local.X = num3;
    }
    int x3 = screen.X;
    workingArea = Screen.PrimaryScreen.WorkingArea;
    int x4 = workingArea.X;
    if (x3 < x4)
    {
      ref Point local = ref screen;
      workingArea = Screen.PrimaryScreen.WorkingArea;
      int x5 = workingArea.X;
      local.X = x5;
    }
    int num4 = screen.Y + 205;
    workingArea = Screen.PrimaryScreen.WorkingArea;
    int y1 = workingArea.Y;
    workingArea = Screen.PrimaryScreen.WorkingArea;
    int height1 = workingArea.Height;
    int num5 = y1 + height1;
    if (num4 > num5)
    {
      ref Point local = ref screen;
      workingArea = Screen.PrimaryScreen.WorkingArea;
      int y2 = workingArea.Y;
      workingArea = Screen.PrimaryScreen.WorkingArea;
      int height2 = workingArea.Height;
      int num6 = y2 + height2 - 205;
      local.Y = num6;
    }
    int y3 = screen.Y;
    workingArea = Screen.PrimaryScreen.WorkingArea;
    int y4 = workingArea.Y;
    if (y3 < y4)
    {
      ref Point local = ref screen;
      workingArea = Screen.PrimaryScreen.WorkingArea;
      int y5 = workingArea.Y;
      local.Y = y5;
    }
    Rectangle formBounds = new Rectangle(screen.X, screen.Y, 250, 200);
    if (ContextSelectionForm.Execute(ref selectedContexts, formBounds) != DialogResult.OK)
      return;
    this.filtration.Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) selectedContexts;
    this.filtration.FiltrationApplyUpdates(true);
  }

  public override void OnGetPluginData(HybridDictionary tag)
  {
    List<long> tag1 = this.filtration.Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] as List<long>;
    tag[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) tag1;
  }
}
