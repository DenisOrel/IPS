// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.SelectAttributeAndSourceForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal class SelectAttributeAndSourceForm : Form
{
  private IContainer components;
  private Button bAttrSelect;
  private RadioButton rbRelation;
  private RadioButton rbObject;
  private TextBox tbAttributeNames;
  private Label label2;
  private Button bCancel;
  private Button bOK;

  public SelectAttributeAndSourceForm() => this.InitializeComponent();

  private void bAttrSelect_Click(object sender, EventArgs e)
  {
    AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(true);
    attributesSelectDlg.AllowedAttributesSourceTypes = this.rbObject.Checked ? AllowedAttrsSourceTypesEnum.Objects : AllowedAttrsSourceTypesEnum.Relations;
    if (attributesSelectDlg.ShowDialog() != DialogResult.OK || attributesSelectDlg.SelectedAttributesID.Count <= 0)
      return;
    this.SelectedAttributes.Clear();
    this.tbAttributeNames.Text = string.Empty;
    foreach (int attrTypeID in attributesSelectDlg.SelectedAttributesID)
    {
      if (this.tbAttributeNames.Text.Length > 0)
        this.tbAttributeNames.Text += ", ";
      this.tbAttributeNames.Text += MetaDataHelper.GetAttributeTypeName(attrTypeID);
      this.SelectedAttributes.Add(new Tuple<int, AttributeSourceTypes>(attrTypeID, this.rbObject.Checked ? AttributeSourceTypes.Object : AttributeSourceTypes.Relation));
    }
    this.bOK.Enabled = true;
  }

  public List<Tuple<int, AttributeSourceTypes>> SelectedAttributes { get; private set; } = new List<Tuple<int, AttributeSourceTypes>>();

  private void rbRelation_CheckedChanged(object sender, EventArgs e)
  {
    this.SelectedAttributes.Clear();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.bAttrSelect = new Button();
    this.rbRelation = new RadioButton();
    this.rbObject = new RadioButton();
    this.tbAttributeNames = new TextBox();
    this.label2 = new Label();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.SuspendLayout();
    this.bAttrSelect.Location = new Point(392, 110);
    this.bAttrSelect.Name = "bAttrSelect";
    this.bAttrSelect.Size = new Size(26, 24);
    this.bAttrSelect.TabIndex = 19;
    this.bAttrSelect.Text = "...";
    this.bAttrSelect.UseVisualStyleBackColor = true;
    this.bAttrSelect.Click += new EventHandler(this.bAttrSelect_Click);
    this.rbRelation.AutoSize = true;
    this.rbRelation.Location = new Point(43, 72);
    this.rbRelation.Name = "rbRelation";
    this.rbRelation.Size = new Size(98, 17);
    this.rbRelation.TabIndex = 17;
    this.rbRelation.Text = "Атрибут связи";
    this.rbRelation.UseVisualStyleBackColor = true;
    this.rbRelation.CheckedChanged += new EventHandler(this.rbRelation_CheckedChanged);
    this.rbObject.AutoSize = true;
    this.rbObject.Checked = true;
    this.rbObject.Location = new Point(43, 49);
    this.rbObject.Name = "rbObject";
    this.rbObject.Size = new Size(110, 17);
    this.rbObject.TabIndex = 16 /*0x10*/;
    this.rbObject.TabStop = true;
    this.rbObject.Text = "Атрибут объекта";
    this.rbObject.UseVisualStyleBackColor = true;
    this.tbAttributeNames.BackColor = SystemColors.Window;
    this.tbAttributeNames.Location = new Point(30, 110);
    this.tbAttributeNames.Multiline = true;
    this.tbAttributeNames.Name = "tbAttributeNames";
    this.tbAttributeNames.ReadOnly = true;
    this.tbAttributeNames.Size = new Size(356, 23);
    this.tbAttributeNames.TabIndex = 20;
    this.label2.AutoSize = true;
    this.label2.Location = new Point(27, 24);
    this.label2.Name = "label2";
    this.label2.Size = new Size(97, 13);
    this.label2.TabIndex = 18;
    this.label2.Text = "Принадлежность:";
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(297, 152);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(121, 27);
    this.bCancel.TabIndex = 22;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Enabled = false;
    this.bOK.Location = new Point(170, 152);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(121, 27);
    this.bOK.TabIndex = 21;
    this.bOK.Text = "OK";
    this.bOK.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(439, 191);
    this.Controls.Add((Control) this.bCancel);
    this.Controls.Add((Control) this.bOK);
    this.Controls.Add((Control) this.bAttrSelect);
    this.Controls.Add((Control) this.rbRelation);
    this.Controls.Add((Control) this.rbObject);
    this.Controls.Add((Control) this.tbAttributeNames);
    this.Controls.Add((Control) this.label2);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SelectAttributeAndSourceForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Добавить атрибуты сортировки";
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
