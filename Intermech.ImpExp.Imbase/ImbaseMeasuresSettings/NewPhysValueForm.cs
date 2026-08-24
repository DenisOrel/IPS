// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseMeasuresSettings.NewPhysValueForm
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Imbase.ImbaseMeasuresSettings;

public class NewPhysValueForm : Form
{
  public long newPhysicalValueID;
  private IContainer components;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;
  private Panel panel2;
  private Label label1;
  private TextBox textBox1;
  private GroupBox groupBox1;
  private MeasureControl measureControl1;

  public NewPhysValueForm()
  {
    this.InitializeComponent();
    this.measureControl1.ShortName = string.Empty;
    this.measureControl1.MeasureName = string.Empty;
    this.measureControl1.Koef = 1.0;
    this.measureControl1.ReadOnlyKoef = true;
  }

  private void bOK_Click(object sender, EventArgs e)
  {
    string str = this.textBox1.Text.Trim();
    try
    {
      if (str == string.Empty)
      {
        int num1 = (int) MessageBox.Show("Наименование физической длины не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.measureControl1.Name == string.Empty)
      {
        int num2 = (int) MessageBox.Show("Наименование базовой единицы измерения не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.measureControl1.ShortName == string.Empty)
      {
        int num3 = (int) MessageBox.Show("Краткое наименование базовой единицы измерения не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        IPhysicalValues service1 = ServicesManager.GetService(typeof (IPhysicalValues)) as IPhysicalValues;
        if (service1.GetPhysicalValue(str) != null)
        {
          int num4 = (int) MessageBox.Show("Физическая величина с таким наименование уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          if (!MeasureControlHelper.CheckMeasureShortName(this.measureControl1.ShortName))
            return;
          IDataWriter service2 = ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter;
          IImportedObjectList importedObjectList = service2.CreateImportedObjectList(0);
          IMetadataInfo service3 = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
          int id1 = service3.ObjectTypes.GetByGuid(new Guid("cad00048-306c-11d8-b4e9-00304f19f545")).ID;
          int id2 = service3.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID;
          importedObjectList.AddObject(id1, 0, str);
          importedObjectList.AddAttributeStr(id2, str);
          AttributesHelper.AddObligatoryObjectAttributes(service2.GetUserSession(), importedObjectList);
          importedObjectList.Import();
          long objectId = importedObjectList.Items[0].Object.Object_id;
          if (objectId != 0L)
          {
            service1.AddPhysicalValue(importedObjectList.Items[0].Object.Object_id, str, (Guid) importedObjectList.Items[0].Object.ObjectGuid);
            importedObjectList.Items.Clear();
            (ServicesManager.GetService(typeof (IMeasures)) as IMeasures).AddMeasure(this.measureControl1.ShortName, this.measureControl1.MeasureName, this.measureControl1.Koef, objectId);
            this.DialogResult = DialogResult.OK;
            this.Close();
          }
          else
          {
            int num5 = (int) MessageBox.Show($"Физическая величина \"{str}\" не импортирована", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
    this.panel2 = new Panel();
    this.textBox1 = new TextBox();
    this.label1 = new Label();
    this.groupBox1 = new GroupBox();
    this.measureControl1 = new MeasureControl();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 181);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(529, 35);
    this.panel1.TabIndex = 0;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(443, 6);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(75, 23);
    this.bCancel.TabIndex = 1;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.Location = new Point(362, 6);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(75, 23);
    this.bOK.TabIndex = 0;
    this.bOK.Text = "ОК";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    this.panel2.Controls.Add((Control) this.groupBox1);
    this.panel2.Controls.Add((Control) this.textBox1);
    this.panel2.Controls.Add((Control) this.label1);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(529, 181);
    this.panel2.TabIndex = 1;
    this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.textBox1.Location = new Point(15, 25);
    this.textBox1.Name = "textBox1";
    this.textBox1.Size = new Size(503, 20);
    this.textBox1.TabIndex = 1;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 9);
    this.label1.Name = "label1";
    this.label1.Size = new Size(83, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Наименование";
    this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox1.Controls.Add((Control) this.measureControl1);
    this.groupBox1.Location = new Point(15, 51);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(502, 122);
    this.groupBox1.TabIndex = 2;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Базовая единица измерения";
    this.measureControl1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.measureControl1.Location = new Point(6, 19);
    this.measureControl1.Name = "measureControl1";
    this.measureControl1.Size = new Size(488, 90);
    this.measureControl1.TabIndex = 0;
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(529, 216);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.MinimumSize = new Size(545, (int) byte.MaxValue);
    this.Name = nameof (NewPhysValueForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Новая физическая величина";
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.groupBox1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
