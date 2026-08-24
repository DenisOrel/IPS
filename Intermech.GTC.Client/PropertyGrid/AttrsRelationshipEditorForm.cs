// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.AttrsRelationshipEditorForm
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

public class AttrsRelationshipEditorForm : Form
{
  private int _relatingAttrId;
  private int _relatedAttrId;
  private long _objectId;
  private IContainer components;
  private ButtonedEdit edtRelatedAttr;
  private ButtonedEdit edtRelatingAttr;
  private Panel pnlBottom;
  private Button btnCancel;
  private Button btnOK;

  public AttrsRelationshipEditorForm() => this.InitializeComponent();

  public AttrsRelationshipEditorForm(
    AttrsRelationshipPropertyClass attrsRelationshipPropClass)
    : this()
  {
    this._relatingAttrId = attrsRelationshipPropClass.RelatingAttrId;
    this._relatedAttrId = attrsRelationshipPropClass.RelatedAttrId;
    this._objectId = attrsRelationshipPropClass.ObjectId;
    this.FillAttrs();
  }

  private void FillAttrs()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this._relatingAttrId != 0)
        this.edtRelatingAttr.Value = sessionKeeper.Session.GetAttributeType(this._relatingAttrId).Name;
      if (this._relatedAttrId == 0)
        return;
      this.edtRelatedAttr.Value = sessionKeeper.Session.GetAttributeType(this._relatedAttrId).Name;
    }
  }

  private bool ChangeAttribute(out int newAttrId, out string newAttrName)
  {
    newAttrId = 0;
    newAttrName = string.Empty;
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false))
    {
      attributesSelectDlg.LoadAttrDialogForObject(this._objectId, false);
      if (attributesSelectDlg.ShowDialog().Equals((object) DialogResult.OK))
      {
        if (attributesSelectDlg.SelectedAttributesID.Count > 0)
        {
          newAttrId = attributesSelectDlg.SelectedAttributesID[0];
          using (SessionKeeper sessionKeeper = new SessionKeeper())
            newAttrName = sessionKeeper.Session.GetAttributeType(newAttrId).Name;
          return true;
        }
      }
    }
    return false;
  }

  private void edtRelatingAttr_ButtonClick(object sender, EventArgs e)
  {
    int newAttrId;
    string newAttrName;
    if (!this.ChangeAttribute(out newAttrId, out newAttrName))
      return;
    this._relatingAttrId = newAttrId;
    this.edtRelatingAttr.Value = newAttrName;
  }

  private void edtRelatedAttr_ButtonClick(object sender, EventArgs e)
  {
    int newAttrId;
    string newAttrName;
    if (!this.ChangeAttribute(out newAttrId, out newAttrName))
      return;
    this._relatedAttrId = newAttrId;
    this.edtRelatedAttr.Value = newAttrName;
  }

  public int RelatingAttrId => this._relatingAttrId;

  public int RelatedAttrId => this._relatedAttrId;

  public long ObjectId => this._objectId;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.pnlBottom = new Panel();
    this.btnCancel = new Button();
    this.btnOK = new Button();
    this.edtRelatingAttr = new ButtonedEdit();
    this.edtRelatedAttr = new ButtonedEdit();
    this.pnlBottom.SuspendLayout();
    this.SuspendLayout();
    this.pnlBottom.BorderStyle = BorderStyle.FixedSingle;
    this.pnlBottom.Controls.Add((Control) this.btnCancel);
    this.pnlBottom.Controls.Add((Control) this.btnOK);
    this.pnlBottom.Dock = DockStyle.Bottom;
    this.pnlBottom.Location = new Point(0, 66);
    this.pnlBottom.Name = "pnlBottom";
    this.pnlBottom.Size = new Size(530, 39);
    this.pnlBottom.TabIndex = 2;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(442, 6);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(75, 23);
    this.btnCancel.TabIndex = 1;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Location = new Point(364, 6);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(75, 23);
    this.btnOK.TabIndex = 0;
    this.btnOK.Text = "OK";
    this.btnOK.UseVisualStyleBackColor = true;
    this.edtRelatingAttr.ButtonImage = (Image) null;
    this.edtRelatingAttr.ButtonText = "...";
    this.edtRelatingAttr.Caption = "Основной атрибут";
    this.edtRelatingAttr.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.edtRelatingAttr.Image = (Image) null;
    this.edtRelatingAttr.Location = new Point(12, 12);
    this.edtRelatingAttr.MinimumSize = new Size(40, 20);
    this.edtRelatingAttr.Name = "edtRelatingAttr";
    this.edtRelatingAttr.ReadOnly = true;
    this.edtRelatingAttr.Size = new Size(250, 38);
    this.edtRelatingAttr.TabIndex = 1;
    this.edtRelatingAttr.ButtonClick += new EventHandler(this.edtRelatingAttr_ButtonClick);
    this.edtRelatedAttr.ButtonImage = (Image) null;
    this.edtRelatedAttr.ButtonText = "...";
    this.edtRelatedAttr.Caption = "Зависимый атрибут";
    this.edtRelatedAttr.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.edtRelatedAttr.Image = (Image) null;
    this.edtRelatedAttr.Location = new Point(268, 12);
    this.edtRelatedAttr.MinimumSize = new Size(40, 20);
    this.edtRelatedAttr.Name = "edtRelatedAttr";
    this.edtRelatedAttr.ReadOnly = true;
    this.edtRelatedAttr.Size = new Size(250, 38);
    this.edtRelatedAttr.TabIndex = 0;
    this.edtRelatedAttr.ButtonClick += new EventHandler(this.edtRelatedAttr_ButtonClick);
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(530, 105);
    this.Controls.Add((Control) this.pnlBottom);
    this.Controls.Add((Control) this.edtRelatingAttr);
    this.Controls.Add((Control) this.edtRelatedAttr);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (AttrsRelationshipEditorForm);
    this.ShowIcon = false;
    this.ShowInTaskbar = false;
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Соотношение значений атрибутов";
    this.pnlBottom.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
