// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.ItemsToCreate.NewAttributeTypeForm
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using DevExpress.IM.XtraEditors;
using DevExpress.IM.XtraEditors.Controls;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData.ItemsToCreate;

public class NewAttributeTypeForm : Form
{
  private IAttributeTypeToCreateList _attrService;
  private IContainer components;
  private Panel panel1;
  private Panel panel2;
  private Button bCancel;
  private Button bOK;
  private TextBox tbShortName;
  private Label label2;
  private TextBox tbName;
  private Label label1;
  private Label label4;
  private Label label3;
  private System.Windows.Forms.ComboBox cbType;
  private CalcEdit ceSize;
  private TextBox tbDefault;
  private Label label6;
  private System.Windows.Forms.ComboBox cbMultiply;
  private Label label5;

  public NewAttributeTypeForm(IAttributeTypeToCreateList attrService, string name)
  {
    this.InitializeComponent();
    this._attrService = attrService;
    this.tbName.Text = name;
    this.cbType.Items.Clear();
    foreach (FieldTypes fieldTypes in Enum.GetValues(typeof (FieldTypes)))
      this.cbType.Items.Add((object) EnumDescConverter.GetEnumDescription((Enum) fieldTypes));
    foreach (MultiValueModes multiValueModes in Enum.GetValues(typeof (MultiValueModes)))
      this.cbMultiply.Items.Add((object) EnumDescConverter.GetEnumDescription((Enum) multiValueModes));
    this.cbType.SelectedIndex = 1;
    this.cbMultiply.SelectedIndex = 0;
  }

  private void bOK_Click(object sender, EventArgs e)
  {
    if (this._attrService.GetByName(this.tbName.Text) != null)
    {
      int num = (int) MessageBox.Show("Атрибут с таким наименованием уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
    else
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
  }

  internal string LongName => this.tbName.Text;

  internal string ShortName => this.tbShortName.Text;

  internal FieldTypes Type => (FieldTypes) this.cbType.SelectedIndex;

  internal int Size => Convert.ToInt32(this.ceSize.Value);

  internal MultiValueModes MultiValueMode => (MultiValueModes) this.cbMultiply.SelectedIndex;

  internal string DefaultValue => this.tbDefault.Text;

  private void cbType_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.ceSize.Enabled = this.cbType.SelectedIndex == 1;
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
    this.ceSize = new CalcEdit();
    this.cbType = new System.Windows.Forms.ComboBox();
    this.tbShortName = new TextBox();
    this.tbName = new TextBox();
    this.label4 = new Label();
    this.label3 = new Label();
    this.label2 = new Label();
    this.label1 = new Label();
    this.panel2 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.cbMultiply = new System.Windows.Forms.ComboBox();
    this.label5 = new Label();
    this.tbDefault = new TextBox();
    this.label6 = new Label();
    this.panel1.SuspendLayout();
    this.ceSize.Properties.BeginInit();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.tbDefault);
    this.panel1.Controls.Add((Control) this.label6);
    this.panel1.Controls.Add((Control) this.cbMultiply);
    this.panel1.Controls.Add((Control) this.label5);
    this.panel1.Controls.Add((Control) this.ceSize);
    this.panel1.Controls.Add((Control) this.cbType);
    this.panel1.Controls.Add((Control) this.tbShortName);
    this.panel1.Controls.Add((Control) this.tbName);
    this.panel1.Controls.Add((Control) this.label4);
    this.panel1.Controls.Add((Control) this.label3);
    this.panel1.Controls.Add((Control) this.label2);
    this.panel1.Controls.Add((Control) this.label1);
    this.panel1.Dock = DockStyle.Fill;
    this.panel1.Location = new Point(0, 0);
    this.panel1.Name = "panel1";
    this.panel1.Size = new System.Drawing.Size(374, 254);
    this.panel1.TabIndex = 0;
    this.ceSize.EditValue = (object) new Decimal(new int[4]
    {
      10,
      0,
      0,
      0
    });
    this.ceSize.Location = new Point(16 /*0x10*/, 136);
    this.ceSize.Name = "ceSize";
    this.ceSize.Properties.Buttons.AddRange(new EditorButton[1]
    {
      new EditorButton(ButtonPredefines.Combo)
    });
    this.ceSize.RightToLeft = RightToLeft.Yes;
    this.ceSize.Size = new System.Drawing.Size(108, 23);
    this.ceSize.TabIndex = 7;
    this.cbType.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbType.FormattingEnabled = true;
    this.cbType.Location = new Point(15, 95);
    this.cbType.Name = "cbType";
    this.cbType.Size = new System.Drawing.Size(335, 21);
    this.cbType.TabIndex = 4;
    this.cbType.SelectedIndexChanged += new EventHandler(this.cbType_SelectedIndexChanged);
    this.tbShortName.Location = new Point(15, 57);
    this.tbShortName.Name = "tbShortName";
    this.tbShortName.Size = new System.Drawing.Size(123, 20);
    this.tbShortName.TabIndex = 3;
    this.tbName.Location = new Point(15, 18);
    this.tbName.Name = "tbName";
    this.tbName.Size = new System.Drawing.Size(335, 20);
    this.tbName.TabIndex = 1;
    this.label4.AutoSize = true;
    this.label4.Location = new Point(13, 120);
    this.label4.Name = "label4";
    this.label4.Size = new System.Drawing.Size(46, 13);
    this.label4.TabIndex = 6;
    this.label4.Text = "Размер";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(12, 80 /*0x50*/);
    this.label3.Name = "label3";
    this.label3.Size = new System.Drawing.Size(66, 13);
    this.label3.TabIndex = 5;
    this.label3.Text = "Тип данных";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(12, 41);
    this.label2.Name = "label2";
    this.label2.Size = new System.Drawing.Size(126, 13);
    this.label2.TabIndex = 2;
    this.label2.Text = "Краткое наименование";
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 4);
    this.label1.Name = "label1";
    this.label1.Size = new System.Drawing.Size(83, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Наименование";
    this.panel2.Controls.Add((Control) this.bCancel);
    this.panel2.Controls.Add((Control) this.bOK);
    this.panel2.Dock = DockStyle.Bottom;
    this.panel2.Location = new Point(0, 254);
    this.panel2.Name = "panel2";
    this.panel2.Size = new System.Drawing.Size(374, 38);
    this.panel2.TabIndex = 1;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(287, 6);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new System.Drawing.Size(75, 23);
    this.bCancel.TabIndex = 1;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.Location = new Point(206, 6);
    this.bOK.Name = "bOK";
    this.bOK.Size = new System.Drawing.Size(75, 23);
    this.bOK.TabIndex = 0;
    this.bOK.Text = "Применить";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    this.cbMultiply.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbMultiply.FormattingEnabled = true;
    this.cbMultiply.Location = new Point(16 /*0x10*/, 183);
    this.cbMultiply.Name = "cbMultiply";
    this.cbMultiply.Size = new System.Drawing.Size(334, 21);
    this.cbMultiply.TabIndex = 8;
    this.label5.AutoSize = true;
    this.label5.Location = new Point(13, 168);
    this.label5.Name = "label5";
    this.label5.Size = new System.Drawing.Size(0, 13);
    this.label5.TabIndex = 9;
    this.tbDefault.Location = new Point(16 /*0x10*/, 228);
    this.tbDefault.Name = "tbDefault";
    this.tbDefault.Size = new System.Drawing.Size(334, 20);
    this.tbDefault.TabIndex = 11;
    this.label6.AutoSize = true;
    this.label6.Location = new Point(13, 212);
    this.label6.Name = "label6";
    this.label6.Size = new System.Drawing.Size(129, 13);
    this.label6.TabIndex = 10;
    this.label6.Text = "Значение по умолчанию";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new System.Drawing.Size(374, 292);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.panel2);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (NewAttributeTypeForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Новый тип атрибута";
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.ceSize.Properties.EndInit();
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
