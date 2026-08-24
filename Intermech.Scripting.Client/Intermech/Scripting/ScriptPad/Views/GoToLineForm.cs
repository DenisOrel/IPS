// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.GoToLineForm
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views;

internal class GoToLineForm : Form
{
  private IContainer components;
  private FlowLayoutPanel flpBottomButtons;
  private Button btOK;
  private Button btCancel;
  private TableLayoutPanel tlpMain;
  private Label lbEnterText;
  private NumericUpDown udLineNumber;

  public GoToLineForm() => this.InitializeComponent();

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int MaxLineNumber
  {
    get => (int) this.udLineNumber.Maximum;
    set
    {
      this.udLineNumber.Maximum = value >= 1 ? (Decimal) value : throw new ArgumentOutOfRangeException(nameof (MaxLineNumber));
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public int LineNumber
  {
    get => (int) this.udLineNumber.Value;
    set => this.udLineNumber.Value = (Decimal) value;
  }

  private void SearchTextForm_Shown(object sender, EventArgs e)
  {
    this.lbEnterText.Text = $"Номер строки (1-{this.udLineNumber.Maximum})";
    this.udLineNumber.Select(0, 100);
    this.udLineNumber.Focus();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.flpBottomButtons = new FlowLayoutPanel();
    this.btCancel = new Button();
    this.btOK = new Button();
    this.tlpMain = new TableLayoutPanel();
    this.lbEnterText = new Label();
    this.udLineNumber = new NumericUpDown();
    this.flpBottomButtons.SuspendLayout();
    this.tlpMain.SuspendLayout();
    this.udLineNumber.BeginInit();
    this.SuspendLayout();
    this.flpBottomButtons.AutoSize = true;
    this.flpBottomButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.flpBottomButtons.Controls.Add((Control) this.btCancel);
    this.flpBottomButtons.Controls.Add((Control) this.btOK);
    this.flpBottomButtons.Dock = DockStyle.Bottom;
    this.flpBottomButtons.FlowDirection = FlowDirection.RightToLeft;
    this.flpBottomButtons.Location = new Point(0, 75);
    this.flpBottomButtons.Name = "flpBottomButtons";
    this.flpBottomButtons.Padding = new Padding(4);
    this.flpBottomButtons.Size = new Size(304, 37);
    this.flpBottomButtons.TabIndex = 1;
    this.flpBottomButtons.WrapContents = false;
    this.btCancel.DialogResult = DialogResult.Cancel;
    this.btCancel.Location = new Point(218, 7);
    this.btCancel.Name = "btCancel";
    this.btCancel.Size = new Size(75, 23);
    this.btCancel.TabIndex = 1;
    this.btCancel.Text = "Отмена";
    this.btCancel.UseVisualStyleBackColor = true;
    this.btOK.DialogResult = DialogResult.OK;
    this.btOK.Location = new Point(137, 7);
    this.btOK.Name = "btOK";
    this.btOK.Size = new Size(75, 23);
    this.btOK.TabIndex = 0;
    this.btOK.Text = "OK";
    this.btOK.UseVisualStyleBackColor = true;
    this.tlpMain.ColumnCount = 1;
    this.tlpMain.ColumnStyles.Add(new ColumnStyle());
    this.tlpMain.Controls.Add((Control) this.lbEnterText, 0, 0);
    this.tlpMain.Controls.Add((Control) this.udLineNumber, 0, 1);
    this.tlpMain.Dock = DockStyle.Fill;
    this.tlpMain.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
    this.tlpMain.Location = new Point(0, 0);
    this.tlpMain.Name = "tlpMain";
    this.tlpMain.Padding = new Padding(4, 8, 4, 4);
    this.tlpMain.RowCount = 2;
    this.tlpMain.RowStyles.Add(new RowStyle());
    this.tlpMain.RowStyles.Add(new RowStyle());
    this.tlpMain.Size = new Size(304, 75);
    this.tlpMain.TabIndex = 0;
    this.lbEnterText.AutoSize = true;
    this.lbEnterText.Dock = DockStyle.Fill;
    this.lbEnterText.Location = new Point(8, 11);
    this.lbEnterText.Margin = new Padding(4, 3, 7, 3);
    this.lbEnterText.Name = "lbEnterText";
    this.lbEnterText.Size = new Size(285, 13);
    this.lbEnterText.TabIndex = 0;
    this.lbEnterText.Text = "Номер строки";
    this.lbEnterText.TextAlign = ContentAlignment.BottomLeft;
    this.udLineNumber.Dock = DockStyle.Top;
    this.udLineNumber.Location = new Point(11, 30);
    this.udLineNumber.Margin = new Padding(7, 3, 7, 3);
    this.udLineNumber.Maximum = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this.udLineNumber.Minimum = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this.udLineNumber.MinimumSize = new Size(250, 0);
    this.udLineNumber.Name = "tbText";
    this.udLineNumber.Size = new Size(282, 20);
    this.udLineNumber.TabIndex = 1;
    this.udLineNumber.Value = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this.AcceptButton = (IButtonControl) this.btOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btCancel;
    this.ClientSize = new Size(304, 112 /*0x70*/);
    this.Controls.Add((Control) this.tlpMain);
    this.Controls.Add((Control) this.flpBottomButtons);
    this.MaximizeBox = false;
    this.MaximumSize = new Size(1000, 150);
    this.MinimizeBox = false;
    this.MinimumSize = new Size(320, 150);
    this.Name = nameof (GoToLineForm);
    this.ShowIcon = false;
    this.SizeGripStyle = SizeGripStyle.Show;
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Переход к строке";
    this.Shown += new EventHandler(this.SearchTextForm_Shown);
    this.flpBottomButtons.ResumeLayout(false);
    this.tlpMain.ResumeLayout(false);
    this.tlpMain.PerformLayout();
    this.udLineNumber.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
