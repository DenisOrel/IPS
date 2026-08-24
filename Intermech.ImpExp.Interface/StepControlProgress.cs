// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.StepControlProgress
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class StepControlProgress : Form
{
  private string _caption = string.Empty;
  public bool DrawProgressInCaption;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Label label1;
  private ProgressBar pb1;

  public StepControlProgress()
  {
    this.InitializeComponent();
    this.Size = new Size(352, 90);
    this.pb1.Minimum = 0;
    this.pb1.Maximum = 100;
  }

  public override string Text
  {
    get => this._caption;
    set
    {
      base.Text = value;
      this._caption = value;
    }
  }

  public void SetProgress(string progressInformation, int progressValue)
  {
    this.label1.Text = progressInformation;
    this.pb1.Value = progressValue;
    if (this.DrawProgressInCaption)
      base.Text = $"{this._caption} ({progressValue}%)";
    this.Refresh();
  }

  private int CalcProgress(int startValue, int endValue, int posValue, int maxValue)
  {
    return (endValue - startValue) * posValue / maxValue + startValue;
  }

  public void SetProgress(
    string progressInformation,
    int startValue,
    int endValue,
    int posValue,
    int maxValue)
  {
    this.SetProgress(progressInformation, this.CalcProgress(startValue, endValue, posValue, maxValue));
  }

  public void SetCenterParentLocation(Control parent)
  {
    this.StartPosition = FormStartPosition.Manual;
    this.Location = new Point((parent.Width - this.Width) / 2 + parent.Location.X, (parent.Height - this.Height) / 2 + parent.Location.Y);
  }

  private void StepControlProgress_Deactivate(object sender, EventArgs e) => this.Activate();

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.label1 = new Label();
    this.pb1 = new ProgressBar();
    this.SuspendLayout();
    this.label1.Dock = DockStyle.Top;
    this.label1.Location = new Point(0, 0);
    this.label1.Name = "label1";
    this.label1.Size = new Size(346, 32 /*0x20*/);
    this.label1.TabIndex = 0;
    this.label1.Text = "label1";
    this.label1.TextAlign = ContentAlignment.MiddleCenter;
    this.pb1.Dock = DockStyle.Top;
    this.pb1.Location = new Point(0, 32 /*0x20*/);
    this.pb1.Name = "pb1";
    this.pb1.Size = new Size(346, 23);
    this.pb1.TabIndex = 1;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(346, 58);
    this.ControlBox = false;
    this.Controls.Add((Control) this.pb1);
    this.Controls.Add((Control) this.label1);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.Name = nameof (StepControlProgress);
    this.Text = nameof (StepControlProgress);
    this.Deactivate += new EventHandler(this.StepControlProgress_Deactivate);
    this.ResumeLayout(false);
  }
}
