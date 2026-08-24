// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.AttributeComprasionForm
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Actions;
using Intermech.Client.Core;
using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class AttributeComprasionForm : Form
{
  private int _SourceObjTypeID;
  private int _DestinationObjTypeID;
  private IContainer components;
  private ButtonedEdit edSourceAttr;
  private ButtonedEdit edDestinationAttr;
  private Panel pnlBottom;
  private Button btnCancel;
  private Button btnOK;
  private ActionList actList;
  private Intermech.Actions.Action actSelSouceAttr;
  private Intermech.Actions.Action actSelDestAttr;

  public int SourceAttrID
  {
    get => this.GetSourceAttrID();
    private set => this.SetSourceAttrID(value);
  }

  public int DestinationAttrID
  {
    get => this.GetDestinationAttrID();
    private set => this.SetDestinationAttrID(value);
  }

  private AttributeComprasionForm() => this.InitializeComponent();

  public AttributeComprasionForm(int _ASourceObjTypeID, int _ADestinationObjTypeID)
    : this()
  {
    this._SourceObjTypeID = _ASourceObjTypeID;
    this._DestinationObjTypeID = _ADestinationObjTypeID;
  }

  public AttributeComprasionForm(
    int _ASourceObjTypeID,
    int _ADestinationObjTypeID,
    int ASourceAttributeID,
    int ADestinationAttributeID)
    : this(_ASourceObjTypeID, _ADestinationObjTypeID)
  {
    this.SourceAttrID = ASourceAttributeID;
    this.DestinationAttrID = ADestinationAttributeID;
  }

  private void SetSourceAttrID(int value)
  {
    AttributeComprasionForm.SetEditValue(value, this.edSourceAttr);
  }

  private int GetSourceAttrID() => this.GetAttributeID(this.edSourceAttr);

  private void SetDestinationAttrID(int value)
  {
    AttributeComprasionForm.SetEditValue(value, this.edDestinationAttr);
  }

  private int GetDestinationAttrID() => this.GetAttributeID(this.edDestinationAttr);

  private int GetAttributeID(ButtonedEdit AEdit)
  {
    int attributeId = 0;
    if (AEdit.Tag != null)
      attributeId = (int) AEdit.Tag;
    return attributeId;
  }

  private static void SetEditValue(int AValue, ButtonedEdit AEdit)
  {
    if (AValue == 0)
      return;
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(AValue);
    if (attributeType == null)
      return;
    AEdit.Value = attributeType.Name;
    AEdit.Image = (Image) ServiceHolder.CategoryTypeIconService.GetIcon(3, -1, (object) attributeType.FieldType).ToBitmap();
    AEdit.Tag = (object) AValue;
  }

  private int SelectAttributeTypeID(int AObjectTypeID)
  {
    int num = 0;
    try
    {
      Cursor.Current = Cursors.WaitCursor;
      using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false, new int[1]))
      {
        attributesSelectDlg.LoadAttrDialogForObjectsTypes(MetaDataHelper.GetObjectTypeGuid(AObjectTypeID));
        if (attributesSelectDlg.ShowDialog().Equals((object) DialogResult.OK))
        {
          if (attributesSelectDlg.SelectedAttributesID.Count > 0)
            num = attributesSelectDlg.SelectedAttributesID[0];
        }
      }
    }
    finally
    {
      Cursor.Current = Cursors.Default;
    }
    return num;
  }

  private void actSelSouceAttr_Execute(object sender, EventArgs e)
  {
    AttributeComprasionForm.SetEditValue(this.SelectAttributeTypeID(this._SourceObjTypeID), this.edSourceAttr);
  }

  private void actSelDestAttr_Execute(object sender, EventArgs e)
  {
    AttributeComprasionForm.SetEditValue(this.SelectAttributeTypeID(this._DestinationObjTypeID), this.edDestinationAttr);
  }

  private void actSelectAttribute_Update(object sender, EventArgs e)
  {
    this.actSelDestAttr.Enabled = this.edSourceAttr.Value.Length > 0;
  }

  private void btnOK_Click(object sender, EventArgs e)
  {
    if (this.SourceAttrID != 0 && this.DestinationAttrID != 0)
    {
      if (MetaDataHelper.GetAttributeType(this.SourceAttrID).FieldType != MetaDataHelper.GetAttributeType(this.DestinationAttrID).FieldType)
      {
        int num1 = (int) MessageBox.Show("Не соответствует типы атрибутов");
      }
      else
        this.DialogResult = DialogResult.OK;
    }
    else
    {
      int num2 = (int) MessageBox.Show("Не указан один из атрибутов");
    }
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    this.DialogResult = DialogResult.Cancel;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.edSourceAttr = new ButtonedEdit();
    this.edDestinationAttr = new ButtonedEdit();
    this.pnlBottom = new Panel();
    this.btnCancel = new Button();
    this.btnOK = new Button();
    this.actList = new ActionList();
    this.actSelSouceAttr = new Intermech.Actions.Action();
    this.actSelDestAttr = new Intermech.Actions.Action();
    this.pnlBottom.SuspendLayout();
    this.SuspendLayout();
    this.actList.SetAction((Component) this.edSourceAttr, this.actSelSouceAttr);
    this.edSourceAttr.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.edSourceAttr.ButtonImage = (Image) null;
    this.edSourceAttr.ButtonText = "...";
    this.edSourceAttr.Caption = "Атрибут источник";
    this.edSourceAttr.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.edSourceAttr.Image = (Image) null;
    this.edSourceAttr.Location = new Point(12, 12);
    this.edSourceAttr.MinimumSize = new Size(40, 20);
    this.edSourceAttr.Name = "edSourceAttr";
    this.edSourceAttr.ReadOnly = true;
    this.edSourceAttr.Size = new Size(332, 38);
    this.edSourceAttr.TabIndex = 0;
    this.edSourceAttr.ButtonClick += new EventHandler(this.actSelSouceAttr_Execute);
    this.actList.SetAction((Component) this.edDestinationAttr, this.actSelDestAttr);
    this.edDestinationAttr.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.edDestinationAttr.ButtonImage = (Image) null;
    this.edDestinationAttr.ButtonText = "...";
    this.edDestinationAttr.Caption = "Атрибут назначение";
    this.edDestinationAttr.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.edDestinationAttr.Image = (Image) null;
    this.edDestinationAttr.Location = new Point(12, 56);
    this.edDestinationAttr.MinimumSize = new Size(40, 20);
    this.edDestinationAttr.Name = "edDestinationAttr";
    this.edDestinationAttr.ReadOnly = true;
    this.edDestinationAttr.Size = new Size(332, 38);
    this.edDestinationAttr.TabIndex = 1;
    this.edDestinationAttr.ButtonClick += new EventHandler(this.actSelDestAttr_Execute);
    this.pnlBottom.Controls.Add((Control) this.btnCancel);
    this.pnlBottom.Controls.Add((Control) this.btnOK);
    this.pnlBottom.Dock = DockStyle.Bottom;
    this.pnlBottom.Location = new Point(0, 102);
    this.pnlBottom.Name = "pnlBottom";
    this.pnlBottom.Size = new Size(357, 46);
    this.pnlBottom.TabIndex = 2;
    this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(269, 11);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(75, 23);
    this.btnCancel.TabIndex = 1;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.btnOK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.btnOK.Location = new Point(188, 11);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(75, 23);
    this.btnOK.TabIndex = 0;
    this.btnOK.Text = "ОК";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.actList.Actions.AddRange(new Intermech.Actions.Action[2]
    {
      this.actSelSouceAttr,
      this.actSelDestAttr
    });
    this.actList.ImageList = (ImageList) null;
    this.actList.ShowTextOnToolBar = false;
    this.actList.Tag = (object) null;
    this.actSelSouceAttr.Hint = (string) null;
    this.actSelSouceAttr.Text = "";
    this.actSelSouceAttr.Execute += new EventHandler(this.actSelSouceAttr_Execute);
    this.actSelSouceAttr.Update += new EventHandler(this.actSelectAttribute_Update);
    this.actSelDestAttr.Hint = (string) null;
    this.actSelDestAttr.Text = "action1";
    this.actSelDestAttr.Execute += new EventHandler(this.actSelDestAttr_Execute);
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(357, 148);
    this.Controls.Add((Control) this.pnlBottom);
    this.Controls.Add((Control) this.edDestinationAttr);
    this.Controls.Add((Control) this.edSourceAttr);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.MinimumSize = new Size(256 /*0x0100*/, 192 /*0xC0*/);
    this.Name = nameof (AttributeComprasionForm);
    this.ShowIcon = false;
    this.ShowInTaskbar = false;
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Значение атрибута";
    this.pnlBottom.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
