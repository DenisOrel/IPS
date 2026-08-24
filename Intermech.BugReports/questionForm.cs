// Decompiled with JetBrains decompiler
// Type: Intermech.BugReports.questionForm
// Assembly: Intermech.BugReports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 16F80F46-2B9D-4747-9BFD-4CC209192F4E
// Assembly location: D:\IPS\Client\Intermech.BugReports.dll

using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.BugReports;

public class questionForm : Form
{
  public bool replace;
  private IContainer components;
  private Label label2;
  private RadioButton replace_rb;
  private RadioButton rename_rb;
  private Button ok_b;
  private Button cancel_b;

  public questionForm() => this.InitializeComponent();

  private void questionForm_Load(object sender, EventArgs e)
  {
  }

  private void ok_b_Click(object sender, EventArgs e)
  {
    if (this.replace_rb.Checked)
      this.replace = true;
    else
      this.replace = false;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (questionForm));
    this.label2 = new Label();
    this.replace_rb = new RadioButton();
    this.rename_rb = new RadioButton();
    this.ok_b = new Button();
    this.cancel_b = new Button();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.Name = "label2";
    componentResourceManager.ApplyResources((object) this.replace_rb, "replace_rb");
    this.replace_rb.Checked = true;
    this.replace_rb.Name = "replace_rb";
    this.replace_rb.TabStop = true;
    this.replace_rb.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.rename_rb, "rename_rb");
    this.rename_rb.Name = "rename_rb";
    this.rename_rb.UseVisualStyleBackColor = true;
    this.ok_b.DialogResult = DialogResult.OK;
    componentResourceManager.ApplyResources((object) this.ok_b, "ok_b");
    this.ok_b.Name = "ok_b";
    this.ok_b.UseVisualStyleBackColor = true;
    this.ok_b.Click += new EventHandler(this.ok_b_Click);
    this.cancel_b.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.cancel_b, "cancel_b");
    this.cancel_b.Name = "cancel_b";
    this.cancel_b.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.cancel_b);
    this.Controls.Add((Control) this.ok_b);
    this.Controls.Add((Control) this.rename_rb);
    this.Controls.Add((Control) this.replace_rb);
    this.Controls.Add((Control) this.label2);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.Name = nameof (questionForm);
    this.ShowInTaskbar = false;
    this.Load += new EventHandler(this.questionForm_Load);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
