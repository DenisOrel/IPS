// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.TestPublishForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class TestPublishForm : Form
{
  private IContainer components;
  private TextBox textBox1;
  private Label label1;
  private RadioButton rbCreatePacket;
  private RadioButton radioButton2;
  private Button button1;

  public long PacketID
  {
    get => !string.IsNullOrEmpty(this.textBox1.Text) ? Convert.ToInt64(this.textBox1.Text) : 0L;
  }

  public bool CreatePacket => this.rbCreatePacket.Checked;

  public bool UsePacket => this.radioButton2.Checked;

  public TestPublishForm() => this.InitializeComponent();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.textBox1 = new TextBox();
    this.label1 = new Label();
    this.rbCreatePacket = new RadioButton();
    this.radioButton2 = new RadioButton();
    this.button1 = new Button();
    this.SuspendLayout();
    this.textBox1.Location = new Point(144 /*0x90*/, 39);
    this.textBox1.Name = "textBox1";
    this.textBox1.Size = new Size(124, 20);
    this.textBox1.TabIndex = 0;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(141, 23);
    this.label1.Name = "label1";
    this.label1.Size = new Size((int) sbyte.MaxValue, 13);
    this.label1.TabIndex = 2;
    this.label1.Text = "Идентификатор Пакета";
    this.rbCreatePacket.AutoSize = true;
    this.rbCreatePacket.Checked = true;
    this.rbCreatePacket.Location = new Point(12, 19);
    this.rbCreatePacket.Name = "rbCreatePacket";
    this.rbCreatePacket.Size = new Size(99, 17);
    this.rbCreatePacket.TabIndex = 3;
    this.rbCreatePacket.TabStop = true;
    this.rbCreatePacket.Text = "Создать пакет";
    this.rbCreatePacket.UseVisualStyleBackColor = true;
    this.radioButton2.AutoSize = true;
    this.radioButton2.Location = new Point(12, 42);
    this.radioButton2.Name = "radioButton2";
    this.radioButton2.Size = new Size(106, 17);
    this.radioButton2.TabIndex = 4;
    this.radioButton2.Text = "Включить пакет";
    this.radioButton2.UseVisualStyleBackColor = true;
    this.button1.DialogResult = DialogResult.OK;
    this.button1.Location = new Point(122, 104);
    this.button1.Name = "button1";
    this.button1.Size = new Size(75, 23);
    this.button1.TabIndex = 5;
    this.button1.Text = "button1";
    this.button1.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(297, 155);
    this.Controls.Add((Control) this.button1);
    this.Controls.Add((Control) this.radioButton2);
    this.Controls.Add((Control) this.rbCreatePacket);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.textBox1);
    this.Name = nameof (TestPublishForm);
    this.Text = nameof (TestPublishForm);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
