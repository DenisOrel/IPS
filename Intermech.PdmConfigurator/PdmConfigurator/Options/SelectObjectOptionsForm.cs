// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.Options.SelectObjectOptionsForm
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator.Options;

public class SelectObjectOptionsForm : Form
{
  private IContainer components;
  private Panel panel1;
  private Button btnCancel;
  private Button btnOk;
  private ObjectContextEditor objectContextEditor;

  public SelectObjectOptionsForm() => this.InitializeComponent();

  public static DialogResult Execute(IDBObject obj, ref string selectedOptions)
  {
    SelectObjectOptionsForm objectOptionsForm = new SelectObjectOptionsForm();
    RelationPair key = PdmConfiguratorHelper.CreateKey(obj.ObjectID, obj.ObjectType, 0L, -1, obj.ObjectID, obj.ObjectType);
    objectOptionsForm.objectContextEditor.LoadInfo((IServiceProvider) null, key, (RelationPair) null, obj, (IDBRelation) null);
    PdmConfiguratorContext context1 = objectOptionsForm.objectContextEditor.Context;
    context1.Assign((object) selectedOptions);
    objectOptionsForm.objectContextEditor.Context = context1;
    int num = (int) objectOptionsForm.ShowDialog();
    if (num != 1)
      return (DialogResult) num;
    PdmConfiguratorContext context2 = objectOptionsForm.objectContextEditor.Context;
    selectedOptions = context2.ToString();
    return (DialogResult) num;
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
    this.btnCancel = new Button();
    this.btnOk = new Button();
    this.objectContextEditor = new ObjectContextEditor();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this.panel1.BorderStyle = BorderStyle.Fixed3D;
    this.panel1.Controls.Add((Control) this.btnCancel);
    this.panel1.Controls.Add((Control) this.btnOk);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 411);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(712, 48 /*0x30*/);
    this.panel1.TabIndex = 1;
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.ImeMode = ImeMode.NoControl;
    this.btnCancel.Location = new Point(582, 11);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(121, 27);
    this.btnCancel.TabIndex = 2;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOk.DialogResult = DialogResult.OK;
    this.btnOk.ImeMode = ImeMode.NoControl;
    this.btnOk.Location = new Point(454, 11);
    this.btnOk.Name = "btnOk";
    this.btnOk.Size = new Size(121, 27);
    this.btnOk.TabIndex = 1;
    this.btnOk.Text = "ОК";
    this.btnOk.UseVisualStyleBackColor = true;
    this.objectContextEditor.Dock = DockStyle.Fill;
    this.objectContextEditor.Font = new Font("Tahoma", 8.25f);
    this.objectContextEditor.IsChanged = false;
    this.objectContextEditor.IsOptionValueStatus = false;
    this.objectContextEditor.Location = new Point(0, 0);
    this.objectContextEditor.MinimumSize = new Size(220, 100);
    this.objectContextEditor.Name = "objectContextEditor1";
    this.objectContextEditor.Size = new Size(712, 411);
    this.objectContextEditor.TabIndex = 2;
    this.AcceptButton = (IButtonControl) this.btnOk;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(712, 459);
    this.Controls.Add((Control) this.objectContextEditor);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (SelectObjectOptionsForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Выберите опции объекта";
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
