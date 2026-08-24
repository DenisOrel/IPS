// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.MaterialPropertiesFileNameDlg
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class MaterialPropertiesFileNameDlg : Form
{
  private IContainer components;
  private Panel _pnl;
  private Button _btnCancel;
  private Button _btnApply;
  private Label _lblMsg;
  private TextBox _txt;

  public string FileName => this._txt.Text;

  public MaterialPropertiesFileNameDlg() => this.InitializeComponent();

  private void On_txt_TextChanged(object sender, EventArgs e)
  {
    this._btnApply.Enabled = !string.IsNullOrEmpty(this._txt.Text);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MaterialPropertiesFileNameDlg));
    this._pnl = new Panel();
    this._btnCancel = new Button();
    this._btnApply = new Button();
    this._lblMsg = new Label();
    this._txt = new TextBox();
    this._pnl.SuspendLayout();
    this.SuspendLayout();
    this._pnl.Controls.Add((Control) this._btnCancel);
    this._pnl.Controls.Add((Control) this._btnApply);
    componentResourceManager.ApplyResources((object) this._pnl, "_pnl");
    this._pnl.Name = "_pnl";
    componentResourceManager.ApplyResources((object) this._btnCancel, "_btnCancel");
    this._btnCancel.DialogResult = DialogResult.Cancel;
    this._btnCancel.Name = "_btnCancel";
    this._btnCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._btnApply, "_btnApply");
    this._btnApply.DialogResult = DialogResult.OK;
    this._btnApply.Name = "_btnApply";
    this._btnApply.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._lblMsg, "_lblMsg");
    this._lblMsg.Name = "_lblMsg";
    componentResourceManager.ApplyResources((object) this._txt, "_txt");
    this._txt.Name = "_txt";
    this._txt.TextChanged += new EventHandler(this.On_txt_TextChanged);
    this.AcceptButton = (IButtonControl) this._btnApply;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnCancel;
    this.Controls.Add((Control) this._txt);
    this.Controls.Add((Control) this._lblMsg);
    this.Controls.Add((Control) this._pnl);
    this.DoubleBuffered = true;
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.Name = nameof (MaterialPropertiesFileNameDlg);
    this.ShowInTaskbar = false;
    this._pnl.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
