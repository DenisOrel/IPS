// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.ListTablesForm
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

public class ListTablesForm : Form
{
  private IContainer components;
  private Panel panel1;
  private Button button1;
  private Panel panel2;
  private ListBox listBox1;

  public ListTablesForm() => this.InitializeComponent();

  public void SetValues(List<string> values)
  {
    this.listBox1.Items.Clear();
    for (int index = 0; index < values.Count; ++index)
      this.listBox1.Items.Add((object) values[index]);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this.button1 = new Button();
    this.listBox1 = new ListBox();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.button1);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 186);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(282, 37);
    this.panel1.TabIndex = 0;
    this.panel2.Controls.Add((Control) this.listBox1);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(282, 186);
    this.panel2.TabIndex = 1;
    this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.button1.DialogResult = DialogResult.Cancel;
    this.button1.Location = new Point(195, 6);
    this.button1.Name = "button1";
    this.button1.Size = new Size(75, 23);
    this.button1.TabIndex = 0;
    this.button1.Text = "Отмена";
    this.button1.UseVisualStyleBackColor = true;
    this.listBox1.Dock = DockStyle.Fill;
    this.listBox1.FormattingEnabled = true;
    this.listBox1.Items.AddRange(new object[4]
    {
      (object) "еншлнгл",
      (object) "нглшл",
      (object) "гшдгшдгшдгш",
      (object) "гшдгшдгшд"
    });
    this.listBox1.Location = new Point(0, 0);
    this.listBox1.Name = "listBox1";
    this.listBox1.Size = new Size(282, 186);
    this.listBox1.TabIndex = 0;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.button1;
    this.ClientSize = new Size(282, 223);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ListTablesForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Таблицы Imbase";
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
