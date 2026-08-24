// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.FileRenameForm
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class FileRenameForm : Form
{
  [NotNull]
  private readonly string _oldName;
  private IContainer components;
  private TextBox tbFileName;
  private Button bOK;
  private Button bCancel;
  private TextBox tbNote;
  private Label label1;
  private Label label2;

  public FileRenameForm([NotNull] string oldName, [NotNull] string oldNote)
  {
    this.InitializeComponent();
    this.tbFileName.Text = oldName;
    this.tbNote.Text = oldNote;
    this._oldName = oldName.ToUpper();
  }

  [NotNull]
  public string NewFileName => this.tbFileName.Text;

  [NotNull]
  public string NewNote => this.tbNote.Text;

  private void bOK_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this.tbFileName.Text.ToUpper() == this._oldName)
    {
      int num = (int) MessageBox.Show(Localization.GetString("Office.Client_51"), Localization.GetString("Office.Client_52"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
    else
      this.Close();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FileRenameForm));
    this.tbFileName = new TextBox();
    this.bOK = new Button();
    this.bCancel = new Button();
    this.tbNote = new TextBox();
    this.label1 = new Label();
    this.label2 = new Label();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.tbFileName, "tbFileName");
    this.tbFileName.Name = "tbFileName";
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.tbNote, "tbNote");
    this.tbNote.Name = "tbNote";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.Name = "label2";
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.label2);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.tbNote);
    this.Controls.Add((Control) this.bCancel);
    this.Controls.Add((Control) this.bOK);
    this.Controls.Add((Control) this.tbFileName);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (FileRenameForm);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
