// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextComposition.NotAddObject
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.ContextComposition;

public class NotAddObject : Form
{
  private List<string> _objCaptions;
  private IContainer components;
  private ListBox listBox1;
  private Button button1;
  private Label label1;

  public NotAddObject(List<string> objectCaptions)
  {
    this.InitializeComponent();
    this._objCaptions = objectCaptions;
  }

  private void NotAddObject_Load(object sender, EventArgs e)
  {
    foreach (object objCaption in this._objCaptions)
      this.listBox1.Items.Add(objCaption);
  }

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
    this.label1 = new Label();
    this.SuspendLayout();
    this.listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.listBox1.FormattingEnabled = true;
    this.listBox1.Location = new Point(12, 25);
    this.listBox1.Name = "listBox1";
    this.listBox1.Size = new Size(410, 212);
    this.listBox1.TabIndex = 0;
    this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.button1.Location = new Point(347, 247);
    this.button1.Name = "button1";
    this.button1.Size = new Size(75, 23);
    this.button1.TabIndex = 1;
    this.button1.Text = "ОК";
    this.button1.UseVisualStyleBackColor = true;
    this.button1.Click += new EventHandler(this.button1_Click);
    this.label1.AutoSize = true;
    this.label1.Location = new Point(9, 9);
    this.label1.Name = "label1";
    this.label1.Size = new Size(398, 13);
    this.label1.TabIndex = 2;
    this.label1.Text = "Объекты не были добавлены в состав технологической сборочной единицы: ";
    this.AcceptButton = (IButtonControl) this.button1;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(434, 282);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.button1);
    this.Controls.Add((Control) this.listBox1);
    this.MinimumSize = new Size(450, 320);
    this.Name = nameof (NotAddObject);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Внимание";
    this.Load += new EventHandler(this.NotAddObject_Load);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
