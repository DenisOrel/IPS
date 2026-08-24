// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.ImportWizard.StartPage
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.GTC.Interfaces;
using Intermech.UI.Winforms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client.ImportWizard;

public class StartPage : UserControl, IWizardPage
{
  private IImportConfig _importSettings;
  private IContainer components;
  private Panel pnlRadioButton;
  private RadioButton rbGTC1Adveon;
  private RadioButton rbGTC1;
  private RadioButton rbGTC2;

  public StartPage() => this.InitializeComponent();

  public StartPage(IImportConfig importSettings)
    : this()
  {
    this._importSettings = importSettings;
  }

  public void Activate(IWizardPage prevPage, bool rollback)
  {
    switch (this._importSettings.Version)
    {
      case GtcVersion.Second:
        this.rbGTC2.Checked = true;
        break;
      case GtcVersion.First:
        this.rbGTC1.Checked = true;
        break;
      case GtcVersion.FirstForAdveon:
        this.rbGTC1Adveon.Checked = true;
        break;
    }
    this.PageComplete((object) this, new PageCompleteEventArgs(true));
  }

  public void Deactivate(IWizardPage nextPage, bool rollback)
  {
  }

  public bool ReallyComplete => true;

  public void DoMagic()
  {
    this._importSettings.Version = this.rbGTC2.Checked ? GtcVersion.Second : (this.rbGTC1.Checked ? GtcVersion.First : (this.rbGTC1Adveon.Checked ? GtcVersion.FirstForAdveon : GtcVersion.Second));
  }

  Control IWizardPage.Control => (Control) this;

  public IWizard Wizard { get; set; }

  string IWizardPage.Name => Intermech.GTC.Client.Const.StartPageName;

  public string Caption => ServiceHolder.Rm.GetString("GTC_7");

  public string Description => ServiceHolder.Rm.GetString("GTC_8");

  public Image Image => (Image) null;

  public event EventHandler<PageCompleteEventArgs> PageComplete;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.pnlRadioButton = new Panel();
    this.rbGTC1Adveon = new RadioButton();
    this.rbGTC1 = new RadioButton();
    this.rbGTC2 = new RadioButton();
    this.pnlRadioButton.SuspendLayout();
    this.SuspendLayout();
    this.pnlRadioButton.Controls.Add((Control) this.rbGTC1Adveon);
    this.pnlRadioButton.Controls.Add((Control) this.rbGTC1);
    this.pnlRadioButton.Controls.Add((Control) this.rbGTC2);
    this.pnlRadioButton.Dock = DockStyle.Fill;
    this.pnlRadioButton.Location = new Point(0, 0);
    this.pnlRadioButton.Name = "pnlRadioButton";
    this.pnlRadioButton.Size = new Size(600, 350);
    this.pnlRadioButton.TabIndex = 1;
    this.rbGTC1Adveon.AutoSize = true;
    this.rbGTC1Adveon.Location = new Point(40, 141);
    this.rbGTC1Adveon.Name = "rbGTC1Adveon";
    this.rbGTC1Adveon.Size = new Size(120, 17);
    this.rbGTC1Adveon.TabIndex = 2;
    this.rbGTC1Adveon.TabStop = true;
    this.rbGTC1Adveon.Text = "GTC 1.0 for Adveon";
    this.rbGTC1Adveon.UseVisualStyleBackColor = true;
    this.rbGTC1.AutoSize = true;
    this.rbGTC1.Location = new Point(40, 87);
    this.rbGTC1.Name = "rbGTC1";
    this.rbGTC1.Size = new Size(65, 17);
    this.rbGTC1.TabIndex = 1;
    this.rbGTC1.TabStop = true;
    this.rbGTC1.Text = "GTC 1.0";
    this.rbGTC1.UseVisualStyleBackColor = true;
    this.rbGTC2.AutoSize = true;
    this.rbGTC2.Location = new Point(40, 32 /*0x20*/);
    this.rbGTC2.Name = "rbGTC2";
    this.rbGTC2.Size = new Size(65, 17);
    this.rbGTC2.TabIndex = 0;
    this.rbGTC2.TabStop = true;
    this.rbGTC2.Text = "GTC 2.0";
    this.rbGTC2.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.pnlRadioButton);
    this.Name = nameof (StartPage);
    this.Size = new Size(600, 350);
    this.pnlRadioButton.ResumeLayout(false);
    this.pnlRadioButton.PerformLayout();
    this.ResumeLayout(false);
  }
}
