// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseMeasuresSettings.NewMeasureDialog
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using DevExpress.IM.XtraEditors;
using DevExpress.IM.XtraEditors.Controls;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.Interfaces.Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase.ImbaseMeasuresSettings;

public class NewMeasureDialog : Form
{
  public long NewMeasureID;
  private IContainer components;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;
  private TextBox textBox1;
  private Label label1;
  private TextBox textBox2;
  private Label label2;
  private Label label3;
  private CalcEdit calcEdit1;
  private System.Windows.Forms.ComboBox comboBox1;
  private Label label4;
  private Panel panel2;
  private Button buttonCancel;
  private Button buttonOK;
  private Label label8;
  private System.Windows.Forms.ComboBox cbPhysValues;
  private MeasureControl measureControl;

  public NewMeasureDialog() => this.InitializeComponent();

  public void LoadData(long currentValue)
  {
    this.measureControl.ShortName = string.Empty;
    this.measureControl.MeasureName = string.Empty;
    this.measureControl.Koef = 1.0;
    this.cbPhysValues.Items.Clear();
    IPhysicalValues service = ServicesManager.GetService(typeof (IPhysicalValues)) as IPhysicalValues;
    int num1 = 0;
    foreach (IPhysicalValueItem allPhysicalValue in service.GetAllPhysicalValues())
    {
      int num2 = this.cbPhysValues.Items.Add((object) allPhysicalValue.Name);
      if (allPhysicalValue.Id == currentValue)
        num1 = num2;
    }
    if (this.cbPhysValues.Items.Count <= 0)
      return;
    this.cbPhysValues.SelectedIndex = num1;
  }

  private void buttonOK_Click(object sender, EventArgs e)
  {
    try
    {
      if (!MeasureControlHelper.CheckMeasureShortName(this.measureControl.ShortName))
        return;
      IMeasures service = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
      IPhysicalValueItem physicalValue = (ServicesManager.GetService(typeof (IPhysicalValues)) as IPhysicalValues).GetPhysicalValue(this.cbPhysValues.Text);
      if ((physicalValue.Measures == null || physicalValue.Measures.Count == 0) && this.measureControl.Koef != 1.0)
        throw new Exception("Первая создаваемая единица измерения для физической величины должна быть базовой (с коэффициентом 1)");
      this.NewMeasureID = service.AddMeasure(this.measureControl.ShortName, this.measureControl.MeasureName, this.measureControl.Koef, physicalValue.Id);
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.NewMeasureID = 0L;
    }
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
    this.bCancel = new Button();
    this.bOK = new Button();
    this.textBox1 = new TextBox();
    this.label1 = new Label();
    this.textBox2 = new TextBox();
    this.label2 = new Label();
    this.label3 = new Label();
    this.calcEdit1 = new CalcEdit();
    this.comboBox1 = new System.Windows.Forms.ComboBox();
    this.label4 = new Label();
    this.panel2 = new Panel();
    this.buttonCancel = new Button();
    this.buttonOK = new Button();
    this.label8 = new Label();
    this.cbPhysValues = new System.Windows.Forms.ComboBox();
    this.measureControl = new MeasureControl();
    this.panel1.SuspendLayout();
    this.calcEdit1.Properties.BeginInit();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 111);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(463, 34);
    this.panel1.TabIndex = 0;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(376, 5);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(75, 23);
    this.bCancel.TabIndex = 3;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.Location = new Point(295, 5);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(75, 23);
    this.bOK.TabIndex = 2;
    this.bOK.Text = "ОК";
    this.bOK.UseVisualStyleBackColor = true;
    this.textBox1.Location = new Point(98, 10);
    this.textBox1.Name = "textBox1";
    this.textBox1.Size = new Size(353, 20);
    this.textBox1.TabIndex = 3;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(9, 14);
    this.label1.Name = "label1";
    this.label1.Size = new Size(83, 13);
    this.label1.TabIndex = 2;
    this.label1.Text = "Наименование";
    this.textBox2.Location = new Point(98, 42);
    this.textBox2.Name = "textBox2";
    this.textBox2.Size = new Size(70, 20);
    this.textBox2.TabIndex = 5;
    this.label2.AutoSize = true;
    this.label2.Location = new Point(49, 46);
    this.label2.Name = "label2";
    this.label2.Size = new Size(43, 13);
    this.label2.TabIndex = 4;
    this.label2.Text = "Кратко";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(174, 46);
    this.label3.Name = "label3";
    this.label3.Size = new Size(196, 13);
    this.label3.TabIndex = 6;
    this.label3.Text = "Коэф.приведения к базовой единице";
    this.calcEdit1.Location = new Point(376, 42);
    this.calcEdit1.Name = "calcEdit1";
    this.calcEdit1.Properties.Buttons.AddRange(new EditorButton[1]
    {
      new EditorButton(ButtonPredefines.Combo)
    });
    this.calcEdit1.Size = new Size(75, 23);
    this.calcEdit1.TabIndex = 7;
    this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
    this.comboBox1.FormattingEnabled = true;
    this.comboBox1.Location = new Point(139, 73);
    this.comboBox1.Name = "comboBox1";
    this.comboBox1.Size = new Size(312, 21);
    this.comboBox1.TabIndex = 8;
    this.label4.AutoSize = true;
    this.label4.Location = new Point(12, 77);
    this.label4.Name = "label4";
    this.label4.Size = new Size(121, 13);
    this.label4.TabIndex = 9;
    this.label4.Text = "Физическая величина";
    this.panel2.Controls.Add((Control) this.buttonCancel);
    this.panel2.Controls.Add((Control) this.buttonOK);
    this.panel2.Dock = DockStyle.Bottom;
    this.panel2.Location = new Point(0, 122);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(506, 35);
    this.panel2.TabIndex = 3;
    this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.buttonCancel.DialogResult = DialogResult.Cancel;
    this.buttonCancel.Location = new Point(420, 6);
    this.buttonCancel.Name = "buttonCancel";
    this.buttonCancel.Size = new Size(75, 23);
    this.buttonCancel.TabIndex = 1;
    this.buttonCancel.Text = "Отмена";
    this.buttonCancel.UseVisualStyleBackColor = true;
    this.buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.buttonOK.Location = new Point(339, 6);
    this.buttonOK.Name = "buttonOK";
    this.buttonOK.Size = new Size(75, 23);
    this.buttonOK.TabIndex = 0;
    this.buttonOK.Text = "ОК";
    this.buttonOK.UseVisualStyleBackColor = true;
    this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
    this.label8.AutoSize = true;
    this.label8.Location = new Point(22, 90);
    this.label8.Name = "label8";
    this.label8.Size = new Size(121, 13);
    this.label8.TabIndex = 10;
    this.label8.Text = "Физическая величина";
    this.cbPhysValues.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.cbPhysValues.FormattingEnabled = true;
    this.cbPhysValues.Location = new Point(149, 86);
    this.cbPhysValues.Name = "cbPhysValues";
    this.cbPhysValues.Size = new Size(320, 21);
    this.cbPhysValues.TabIndex = 11;
    this.measureControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.measureControl.Location = new Point(0, 0);
    this.measureControl.Name = "measureControl";
    this.measureControl.Size = new Size(489, 90);
    this.measureControl.TabIndex = 12;
    this.AcceptButton = (IButtonControl) this.bOK;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(506, 157);
    this.Controls.Add((Control) this.cbPhysValues);
    this.Controls.Add((Control) this.label8);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.measureControl);
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.MinimumSize = new Size(500, 0);
    this.Name = nameof (NewMeasureDialog);
    this.Text = "Новая единица измерения";
    this.panel1.ResumeLayout(false);
    this.calcEdit1.Properties.EndInit();
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
