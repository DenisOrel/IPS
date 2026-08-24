// Decompiled with JetBrains decompiler
// Type: Intermech.NormaCSIntegrator.Client.FindInNormaCSForm
// Assembly: Intermech.NormaCSIntegrator.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: BC215C8E-677A-43E5-99F7-5ED2ECAA0726
// Assembly location: D:\IPS\Client\Intermech.NormaCSIntegrator.Client.dll

using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.NormaCSIntegrator.Client;

public class FindInNormaCSForm : Form
{
  private IContainer components;
  private Button btnFind;
  private Button btnCancel;
  private TextBox tbSearchText;
  private Label label1;

  public string SearchText { get; private set; }

  public sealed override string Text
  {
    get => base.Text;
    set => base.Text = value;
  }

  public FindInNormaCSForm() => this.InitializeComponent();

  public FindInNormaCSForm(string formTitle, string searchText)
  {
    this.InitializeComponent();
    this.Text = formTitle;
    this.tbSearchText.Text = searchText;
    this.SearchText = searchText;
  }

  private void btnFind_Click(object sender, EventArgs e)
  {
    if (this.tbSearchText.Text.Equals(string.Empty))
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("NormaCSIntegrator_18"), LocalizationHolder.rm.GetString("NormaCSIntegrator_9"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
    else
    {
      this.SearchText = this.tbSearchText.Text;
      this.Close();
    }
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    this.SearchText = string.Empty;
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
    this.btnFind = new Button();
    this.btnCancel = new Button();
    this.tbSearchText = new TextBox();
    this.label1 = new Label();
    this.SuspendLayout();
    this.btnFind.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnFind.Location = new Point(116, 51);
    this.btnFind.Name = "btnFind";
    this.btnFind.Size = new Size(75, 23);
    this.btnFind.TabIndex = 0;
    this.btnFind.Text = "Найти";
    this.btnFind.UseVisualStyleBackColor = true;
    this.btnFind.Click += new EventHandler(this.btnFind_Click);
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(197, 51);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(75, 23);
    this.btnCancel.TabIndex = 1;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.tbSearchText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbSearchText.Location = new Point(12, 25);
    this.tbSearchText.Name = "tbSearchText";
    this.tbSearchText.Size = new Size(260, 20);
    this.tbSearchText.TabIndex = 2;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 9);
    this.label1.Name = "label1";
    this.label1.Size = new Size(97, 13);
    this.label1.TabIndex = 4;
    this.label1.Text = "Текст для поиска";
    this.AcceptButton = (IButtonControl) this.btnFind;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(284, 86);
    this.ControlBox = false;
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.tbSearchText);
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnFind);
    this.MaximumSize = new Size(500, 125);
    this.MinimumSize = new Size(0, 125);
    this.Name = nameof (FindInNormaCSForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = nameof (FindInNormaCSForm);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
