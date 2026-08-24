// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.IncludesForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Client.Core;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class IncludesForm : Form
{
  private IContainer components;
  private Button bClose;
  private RichTextBox textBox1;

  public IncludesForm(string text)
  {
    this.InitializeComponent();
    FormStorage.LoadLayout((Control) this);
    this.Text = SiteClientConsts.CommandTaskIncludesCaption;
    this.textBox1.Text = text;
  }

  private void IncludesForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.bClose = new Button();
    this.textBox1 = new RichTextBox();
    this.SuspendLayout();
    this.bClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bClose.DialogResult = DialogResult.Cancel;
    this.bClose.Location = new Point(398, 328);
    this.bClose.Name = "bClose";
    this.bClose.Size = new Size(121, 27);
    this.bClose.TabIndex = 1;
    this.bClose.Text = "Закрыть";
    this.bClose.UseVisualStyleBackColor = true;
    this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.textBox1.Location = new Point(0, 0);
    this.textBox1.Name = "textBox1";
    this.textBox1.ReadOnly = true;
    this.textBox1.Size = new Size(530, 322);
    this.textBox1.TabIndex = 2;
    this.textBox1.Text = "";
    this.AcceptButton = (IButtonControl) this.bClose;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bClose;
    this.ClientSize = new Size(531, 367);
    this.Controls.Add((Control) this.textBox1);
    this.Controls.Add((Control) this.bClose);
    this.MinimumSize = new Size(300, 200);
    this.Name = nameof (IncludesForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = nameof (IncludesForm);
    this.FormClosing += new FormClosingEventHandler(this.IncludesForm_FormClosing);
    this.ResumeLayout(false);
  }
}
