// Decompiled with JetBrains decompiler
// Type: Intermech.Sales.KeyCompositionView
// Assembly: Intermech.Sales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D9A9043-6548-439B-99F7-AF22F44A5D2B
// Assembly location: D:\IPS\Client\Intermech.Sales.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Sales;

public class KeyCompositionView : UserControl, IView
{
  protected int imageIndex = -1;
  private IContainer components;
  private TabControl tabControl;
  private TabPage tcIntermech;
  private TabPage tcIPS;

  public KeyCompositionView() => this.InitializeComponent();

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
  }

  public void Activate(IView previousView)
  {
  }

  public void Deactivate(IView nextView)
  {
  }

  public string Caption
  {
    [DebuggerStepThrough] get => LocalizationHolder.rm.GetString("Sales_7");
  }

  public int ImageIndex
  {
    [DebuggerStepThrough] get
    {
      if (this.imageIndex < 0)
        this.imageIndex = (ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList).ImageIndex("");
      return this.imageIndex;
    }
  }

  public int OrderID
  {
    [DebuggerStepThrough] get => 20;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.tabControl = new TabControl();
    this.tcIntermech = new TabPage();
    this.tcIPS = new TabPage();
    this.tabControl.SuspendLayout();
    this.SuspendLayout();
    this.tabControl.Controls.Add((Control) this.tcIntermech);
    this.tabControl.Controls.Add((Control) this.tcIPS);
    this.tabControl.Dock = DockStyle.Fill;
    this.tabControl.Location = new Point(0, 0);
    this.tabControl.Name = "tabControl";
    this.tabControl.SelectedIndex = 0;
    this.tabControl.Size = new Size(689, 421);
    this.tabControl.TabIndex = 0;
    this.tcIntermech.Location = new Point(4, 22);
    this.tcIntermech.Name = "tcIntermech";
    this.tcIntermech.Padding = new Padding(3);
    this.tcIntermech.Size = new Size(681, 395);
    this.tcIntermech.TabIndex = 0;
    this.tcIntermech.Text = "Intermech";
    this.tcIntermech.UseVisualStyleBackColor = true;
    this.tcIPS.Location = new Point(4, 22);
    this.tcIPS.Name = "tcIPS";
    this.tcIPS.Padding = new Padding(3);
    this.tcIPS.Size = new Size(681, 395);
    this.tcIPS.TabIndex = 1;
    this.tcIPS.Text = "IPS";
    this.tcIPS.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tabControl);
    this.Name = nameof (KeyCompositionView);
    this.Size = new Size(689, 421);
    this.tabControl.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
