// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.MeasureControl
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using DevExpress.IM.XtraEditors;
using DevExpress.IM.XtraEditors.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase;

public class MeasureControl : UserControl
{
  private IContainer components;
  private CalcEdit ceKoef;
  private Label label7;
  private TextBox tbShortName;
  private Label label6;
  private TextBox tbName;
  private Label label5;

  public MeasureControl() => this.InitializeComponent();

  public string ShortName
  {
    get => this.tbShortName.Text;
    set => this.tbShortName.Text = value;
  }

  public string MeasureName
  {
    get => this.tbName.Text;
    set => this.tbName.Text = value;
  }

  public double Koef
  {
    get => Convert.ToDouble(this.ceKoef.Value);
    set => this.ceKoef.Value = Convert.ToDecimal(value);
  }

  public bool ReadOnlyKoef
  {
    get => !this.ceKoef.Enabled;
    set => this.ceKoef.Enabled = !value;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.ceKoef = new CalcEdit();
    this.label7 = new Label();
    this.tbShortName = new TextBox();
    this.label6 = new Label();
    this.tbName = new TextBox();
    this.label5 = new Label();
    this.ceKoef.Properties.BeginInit();
    this.SuspendLayout();
    this.ceKoef.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.ceKoef.Location = new Point(384, 53);
    this.ceKoef.Name = "ceKoef";
    this.ceKoef.Properties.Buttons.AddRange(new EditorButton[1]
    {
      new EditorButton(ButtonPredefines.Combo)
    });
    this.ceKoef.Size = new Size(108, 23);
    this.ceKoef.TabIndex = 17;
    this.label7.AutoSize = true;
    this.label7.Location = new Point(179, 57);
    this.label7.Name = "label7";
    this.label7.Size = new Size(199, 13);
    this.label7.TabIndex = 16 /*0x10*/;
    this.label7.Text = "Коэф. приведения к базовой единице";
    this.tbShortName.Location = new Point(114, 53);
    this.tbShortName.Name = "tbShortName";
    this.tbShortName.Size = new Size(61, 20);
    this.tbShortName.TabIndex = 15;
    this.label6.AutoSize = true;
    this.label6.Location = new Point(65, 57);
    this.label6.Name = "label6";
    this.label6.Size = new Size(43, 13);
    this.label6.TabIndex = 14;
    this.label6.Text = "Кратко";
    this.tbName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbName.Location = new Point(114, 22);
    this.tbName.Name = "tbName";
    this.tbName.Size = new Size(378, 20);
    this.tbName.TabIndex = 13;
    this.label5.AutoSize = true;
    this.label5.Location = new Point(25, 26);
    this.label5.Name = "label5";
    this.label5.Size = new Size(83, 13);
    this.label5.TabIndex = 12;
    this.label5.Text = "Наименование";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.ceKoef);
    this.Controls.Add((Control) this.label7);
    this.Controls.Add((Control) this.tbShortName);
    this.Controls.Add((Control) this.label6);
    this.Controls.Add((Control) this.tbName);
    this.Controls.Add((Control) this.label5);
    this.Name = nameof (MeasureControl);
    this.Size = new Size(512 /*0x0200*/, 90);
    this.ceKoef.Properties.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
