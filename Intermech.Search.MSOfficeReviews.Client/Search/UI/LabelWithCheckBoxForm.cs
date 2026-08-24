// Decompiled with JetBrains decompiler
// Type: Intermech.Search.UI.LabelWithCheckBoxForm
// Assembly: Intermech.Search.MSOfficeReviews.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 4AB1E446-C278-4B7C-8A5E-DB94EF37D83B
// Assembly location: D:\IPS\Client\Intermech.Search.MSOfficeReviews.Client.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.UI;

public class LabelWithCheckBoxForm : Form
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private FlowLayoutPanel _flowLayoutPanel;
  private CheckBox _checkBox;
  private Label _label;
  private Button _noButton;
  private Button _yesButton;

  public LabelWithCheckBoxForm() => this.InitializeComponent();

  public string LabelText
  {
    get => this._label.Text;
    set => this._label.Text = value;
  }

  public string CheckBoxText
  {
    get => this._checkBox.Text;
    set => this._checkBox.Text = value;
  }

  public bool Checked
  {
    get => this._checkBox.Checked;
    set => this._checkBox.Checked = value;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this._flowLayoutPanel = new FlowLayoutPanel();
    this._noButton = new Button();
    this._yesButton = new Button();
    this._checkBox = new CheckBox();
    this._label = new Label();
    this.tableLayoutPanel1.SuspendLayout();
    this._flowLayoutPanel.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this._flowLayoutPanel, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this._checkBox, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this._label, 0, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 3;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40f));
    this.tableLayoutPanel1.Size = new Size(355, 132);
    this.tableLayoutPanel1.TabIndex = 0;
    this._flowLayoutPanel.Controls.Add((Control) this._noButton);
    this._flowLayoutPanel.Controls.Add((Control) this._yesButton);
    this._flowLayoutPanel.Dock = DockStyle.Fill;
    this._flowLayoutPanel.FlowDirection = FlowDirection.RightToLeft;
    this._flowLayoutPanel.Location = new Point(3, 95);
    this._flowLayoutPanel.Name = "_flowLayoutPanel";
    this._flowLayoutPanel.Size = new Size(349, 34);
    this._flowLayoutPanel.TabIndex = 0;
    this._noButton.DialogResult = DialogResult.No;
    this._noButton.Location = new Point(271, 3);
    this._noButton.Name = "_noButton";
    this._noButton.Size = new Size(75, 23);
    this._noButton.TabIndex = 0;
    this._noButton.Text = "Нет";
    this._noButton.UseVisualStyleBackColor = true;
    this._yesButton.DialogResult = DialogResult.Yes;
    this._yesButton.Location = new Point(190, 3);
    this._yesButton.Name = "_yesButton";
    this._yesButton.Size = new Size(75, 23);
    this._yesButton.TabIndex = 0;
    this._yesButton.Text = "Да";
    this._yesButton.UseVisualStyleBackColor = true;
    this._checkBox.AutoSize = true;
    this._checkBox.CheckAlign = ContentAlignment.TopLeft;
    this._checkBox.Dock = DockStyle.Fill;
    this._checkBox.Location = new Point(13, 56);
    this._checkBox.Margin = new Padding(13, 10, 10, 10);
    this._checkBox.Name = "_checkBox";
    this._checkBox.Size = new Size(332, 26);
    this._checkBox.TabIndex = 1;
    this._checkBox.Text = "CheckBox";
    this._checkBox.TextAlign = ContentAlignment.TopLeft;
    this._checkBox.UseVisualStyleBackColor = true;
    this._label.AutoSize = true;
    this._label.Dock = DockStyle.Fill;
    this._label.Location = new Point(10, 10);
    this._label.Margin = new Padding(10);
    this._label.Name = "_label";
    this._label.Size = new Size(335, 26);
    this._label.TabIndex = 2;
    this._label.Text = "Label";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(355, 132);
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.MaximizeBox = false;
    this.Name = nameof (LabelWithCheckBoxForm);
    this.ShowIcon = false;
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Intermech Professional Solution";
    this.TopMost = true;
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this._flowLayoutPanel.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
