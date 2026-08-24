// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Controls.ExcludeValuesForTasksCntrl
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics.Controls;

public class ExcludeValuesForTasksCntrl : UserControl
{
  private IContainer components;
  private GroupBox groupBox2;
  private TextBox tbPercent;
  private Label label4;
  private CheckBox cbExcludeAbnormalValues;

  public ExcludeValuesForTasksCntrl() => this.InitializeComponent();

  public string Percent
  {
    get => this.tbPercent.Text;
    set => this.tbPercent.Text = value;
  }

  public bool NeedExcludeAbnormalValues
  {
    get => this.cbExcludeAbnormalValues.Checked;
    set => this.cbExcludeAbnormalValues.Checked = value;
  }

  private void Modify()
  {
    EventHandler onModified = this.OnModified;
    if (onModified == null)
      return;
    onModified((object) this, EventArgs.Empty);
  }

  public event EventHandler OnModified;

  private bool IsPercentValid(string tbPercentText)
  {
    tbPercentText = tbPercentText.Trim();
    if (tbPercentText == string.Empty || tbPercentText == "0")
    {
      int num = (int) MessageBox.Show("Значение процента может быть выражено только положительным целым числом.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }
    foreach (char c in tbPercentText.ToCharArray())
    {
      if (!char.IsDigit(c))
      {
        int num = (int) MessageBox.Show("Значение процента может быть выражено только положительным целым числом.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }
    return true;
  }

  private void RemoveZerosFromTheStringBeginning()
  {
    char[] charArray = this.tbPercent.Text.ToCharArray();
    int count = 0;
    if (charArray[0] == '0')
    {
      for (int index = 0; index < charArray.Length && charArray[index] == '0'; ++index)
        ++count;
    }
    if (count == this.tbPercent.Text.Length)
      this.tbPercent.Text = "0";
    else
      this.tbPercent.Text = this.tbPercent.Text.Remove(0, count);
  }

  private void tbPercent_Validating(object sender, CancelEventArgs e)
  {
    if (this.IsPercentValid(this.tbPercent.Text))
      return;
    e.Cancel = true;
  }

  private void cbExcludeAbnormalValues_CheckedChanged(object sender, EventArgs e)
  {
    this.tbPercent.Enabled = this.cbExcludeAbnormalValues.Checked;
    this.Modify();
  }

  private void tbPercent_Validated(object sender, EventArgs e)
  {
    this.RemoveZerosFromTheStringBeginning();
    this.Modify();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.groupBox2 = new GroupBox();
    this.tbPercent = new TextBox();
    this.label4 = new Label();
    this.cbExcludeAbnormalValues = new CheckBox();
    this.groupBox2.SuspendLayout();
    this.SuspendLayout();
    this.groupBox2.Controls.Add((Control) this.tbPercent);
    this.groupBox2.Controls.Add((Control) this.label4);
    this.groupBox2.Controls.Add((Control) this.cbExcludeAbnormalValues);
    this.groupBox2.Dock = DockStyle.Fill;
    this.groupBox2.Location = new Point(0, 0);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(597, 49);
    this.groupBox2.TabIndex = 12;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Исключение  результатов";
    this.tbPercent.Location = new Point(359, 17);
    this.tbPercent.Name = "tbPercent";
    this.tbPercent.Size = new Size(30, 20);
    this.tbPercent.TabIndex = 2;
    this.tbPercent.Text = "200";
    this.tbPercent.Validating += new CancelEventHandler(this.tbPercent_Validating);
    this.tbPercent.Validated += new EventHandler(this.tbPercent_Validated);
    this.label4.AutoSize = true;
    this.label4.Location = new Point(395, 20);
    this.label4.Name = "label4";
    this.label4.Size = new Size(191, 13);
    this.label4.TabIndex = 1;
    this.label4.Text = "% среднеквадратичного отклонения";
    this.cbExcludeAbnormalValues.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.cbExcludeAbnormalValues.AutoSize = true;
    this.cbExcludeAbnormalValues.Checked = true;
    this.cbExcludeAbnormalValues.CheckState = CheckState.Checked;
    this.cbExcludeAbnormalValues.Location = new Point(6, 20);
    this.cbExcludeAbnormalValues.Name = "cbExcludeAbnormalValues";
    this.cbExcludeAbnormalValues.Size = new Size(347, 17);
    this.cbExcludeAbnormalValues.TabIndex = 0;
    this.cbExcludeAbnormalValues.Text = "Исключать значения, отличающиеся от среднего более чем на";
    this.cbExcludeAbnormalValues.UseVisualStyleBackColor = true;
    this.cbExcludeAbnormalValues.CheckedChanged += new EventHandler(this.cbExcludeAbnormalValues_CheckedChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.groupBox2);
    this.Name = nameof (ExcludeValuesForTasksCntrl);
    this.Size = new Size(597, 49);
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.ResumeLayout(false);
  }
}
