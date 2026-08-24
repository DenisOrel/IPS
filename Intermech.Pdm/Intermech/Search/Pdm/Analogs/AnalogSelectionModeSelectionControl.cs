// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Analogs.AnalogSelectionModeSelectionControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.Analogs;

public sealed class AnalogSelectionModeSelectionControl : UserControl
{
  private AnalogDropDownMenuItem _analogDropDownMenuItem = new AnalogDropDownMenuItem();
  private IContainer components;
  private Intermech.Bars.ToolBar _toolBar;

  public AnalogSelectionModeSelectionControl()
  {
    this.InitializeComponent();
    this._analogDropDownMenuItem.AnalogSelectionModeChanged += new EventHandler(this.AnalogDropDownMenuItem_AnalogSelectionModeChanged);
    this._toolBar.Items.Add((ToolbarItemBase) this._analogDropDownMenuItem);
  }

  public event EventHandler AnalogSelectionModeChanged;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public AnalogSelectionMode AnalogSelectionMode
  {
    get => this._analogDropDownMenuItem.GetCurrentAnalogSelectionMode();
  }

  private void AnalogDropDownMenuItem_AnalogSelectionModeChanged(object sender, EventArgs e)
  {
    EventHandler selectionModeChanged = this.AnalogSelectionModeChanged;
    if (selectionModeChanged == null)
      return;
    selectionModeChanged((object) this, EventArgs.Empty);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._toolBar = new Intermech.Bars.ToolBar();
    this.SuspendLayout();
    this._toolBar.FullMenus = true;
    this._toolBar.Guid = new Guid("c17ada03-8d71-420b-b2a3-698fbf9aa251");
    this._toolBar.Hidden = false;
    this._toolBar.Location = new Point(0, 0);
    this._toolBar.Name = "_toolBar";
    this._toolBar.Size = new Size(526, 18);
    this._toolBar.TabIndex = 0;
    this._toolBar.Text = "toolBar1";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._toolBar);
    this.Name = nameof (AnalogSelectionModeSelectionControl);
    this.Size = new Size(526, 115);
    this.ResumeLayout(false);
  }
}
