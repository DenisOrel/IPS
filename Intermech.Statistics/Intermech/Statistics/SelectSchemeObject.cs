// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.SelectSchemeObject
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public class SelectSchemeObject : Form
{
  private IContainer components;
  private Button button1;
  public ListBox listBox1;

  public SelectSchemeObject() => this.InitializeComponent();

  private void button1_Click(object sender, EventArgs e) => this.Close();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.listBox1 = new ListBox();
    this.button1 = new Button();
    this.SuspendLayout();
    this.listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.listBox1.FormattingEnabled = true;
    this.listBox1.Location = new Point(13, 13);
    this.listBox1.Name = "listBox1";
    this.listBox1.Size = new Size(409, 238);
    this.listBox1.TabIndex = 0;
    this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.button1.Location = new Point(347, 262);
    this.button1.Name = "button1";
    this.button1.Size = new Size(75, 23);
    this.button1.TabIndex = 1;
    this.button1.Text = "ОК";
    this.button1.UseVisualStyleBackColor = true;
    this.button1.Click += new EventHandler(this.button1_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(434, 293);
    this.Controls.Add((Control) this.button1);
    this.Controls.Add((Control) this.listBox1);
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.MinimumSize = new Size(450, 330);
    this.Name = nameof (SelectSchemeObject);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Выберите фильтрующий объект для выбранных корневых объектов";
    this.ResumeLayout(false);
  }
}
