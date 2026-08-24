// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ResponceConfigObjectView
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

[ViewDescriptionProvider(typeof (ResponceConfigObjectView.ResponceConfigObjectViewDescriptionProvider))]
public class ResponceConfigObjectView : NavBaseView
{
  internal ICommandManager _commandManager;
  private INamedImageList _namedImageList;
  private IContainer components;
  private AttributeComprasionList AttributeComprasionList;
  private AttrTextBtn attrTextBtnTransfScheme;
  private ButtonedEdit tbFindAttr;
  private Label lblSchemeName;

  public ResponceConfigObjectView() => this.InitializeComponent();

  public override string Caption => Const.RequestConfigTabName;

  public override int ImageIndex
  {
    get
    {
      INamedImageList namedImageList = this._namedImageList;
      return namedImageList == null ? -1 : namedImageList.ImageIndex(Const.ResponceConfigIconName);
    }
  }

  public override int OrderID => 1;

  protected override void LoadData()
  {
    base.LoadData();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.attrTextBtnTransfScheme.ValueChanged -= new EventHandler(this.attrTextBtnTransfScheme_ValueChanged);
      this.AttributeComprasionList.ListChanged -= new EventHandler(this.AttributeComprasionList_ListChanged);
      this.tbFindAttr.EditTextChanged -= new EventHandler(this.tbFindAttr_EditTextChanged);
      if (!(sessionKeeper.Session.GetObject(this._objID, true) is IResponceConfigObject responceConfigObject))
        return;
      IDBAttribute byId = sessionKeeper.Session.GetObject(this._objID).Attributes.FindByID(Const.ResponceSchemeLinkAttrTypeID);
      if (byId != null)
      {
        this.attrTextBtnTransfScheme.AttributeInfo = new AttributeInfo(Const.ResponceSchemeLinkAttrTypeGUID, Const.ResponceConfigObjTypeGUID);
        this.attrTextBtnTransfScheme.Values = new AttributeValues(Const.ResponceSchemeLinkAttrTypeID, byId.Value);
      }
      long settingItemObjectId = responceConfigObject.ObjTypeSettingItemObjectID;
      int responceObjTypeId = Const.ResponceObjTypeID;
      if (!(sessionKeeper.Session.GetObject(settingItemObjectId) is IObjTypeSettingItemObject settingItemObject))
        return;
      int objectTypeId = MetaDataHelper.GetObjectType(new Guid(settingItemObject.ObjTypeGUID)).ObjectTypeID;
      this.AttributeComprasionList.Activate(responceObjTypeId, objectTypeId, responceConfigObject.AttributeComprasion);
      this.AttributeComprasionList.Enabled = this.attrTextBtnTransfScheme.Values.Values[0] != DBNull.Value;
      if (responceConfigObject.FinderID != 0)
      {
        IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(responceConfigObject.FinderID, false);
        if (attributeType != null)
        {
          this.tbFindAttr.Value = attributeType.Name;
          this.tbFindAttr.Image = (Image) ServiceHolder.CategoryTypeIconService.GetIcon(3, -1, (object) attributeType.AttributeType).ToBitmap();
          this.tbFindAttr.Tag = (object) attributeType.AttributeID;
        }
      }
      else
      {
        this.tbFindAttr.Value = string.Empty;
        this.tbFindAttr.Image = (Image) null;
        this.tbFindAttr.Tag = (object) 0;
      }
      this.attrTextBtnTransfScheme.ValueChanged += new EventHandler(this.attrTextBtnTransfScheme_ValueChanged);
      this.AttributeComprasionList.ListChanged += new EventHandler(this.AttributeComprasionList_ListChanged);
      this.tbFindAttr.EditTextChanged += new EventHandler(this.tbFindAttr_EditTextChanged);
    }
  }

  protected override void InitServices(IServiceProvider services)
  {
    base.InitServices(services);
    this._namedImageList = ServiceHolder.NamedImageList;
    this._commandManager = ServicesManager.GetService(typeof (ICommandManager)) as ICommandManager;
  }

  protected override void SaveData(bool sendNotifications = true)
  {
    if (this.attrTextBtnTransfScheme.Values.Values[0] == DBNull.Value)
    {
      int num1 = (int) MessageBox.Show("Не указана схема трансформации. Изменения не будут сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    else if ((int) this.tbFindAttr.Tag == 0)
    {
      int num2 = (int) MessageBox.Show("Не указан поисковый атрибут. Изменения не будут сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    else
    {
      using (SessionKeeper sk = new SessionKeeper())
      {
        long schemeLinkID = (long) this.attrTextBtnTransfScheme.Values.Values[0];
        IResponceConfigObject responceConfigObject;
        if ((responceConfigObject = sk.Session.GetObject(this._objID, true) as IResponceConfigObject) == null)
          return;
        long settingItemObjectId = responceConfigObject.ObjTypeSettingItemObjectID;
        if (!(sk.Session.GetObject(settingItemObjectId, true) is IObjTypeSettingItemObject settingItemObject))
          return;
        if (((IEnumerable<long>) ((IEnumerable<long>) settingItemObject.ResponceConfigs).Select<long, IResponceConfigObject>((Func<long, IResponceConfigObject>) (x => sk.Session.GetObject(x) as IResponceConfigObject)).Where<IResponceConfigObject>((Func<IResponceConfigObject, bool>) (x => x != null)).Select<IResponceConfigObject, long>((Func<IResponceConfigObject, long>) (x => x.SchemeTransfLink)).ToArray<long>()).Any<long>((Func<long, bool>) (x => x == schemeLinkID && x != responceConfigObject.SchemeTransfLink)))
        {
          int num3 = (int) MessageBox.Show($"Указанная схема трансформации уже используется{Environment.NewLine} Изменения не будут сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if ((int) this.tbFindAttr.Tag != 0)
            responceConfigObject.FinderID = (int) this.tbFindAttr.Tag;
          responceConfigObject.SchemeTransfLink = schemeLinkID;
          responceConfigObject.AttributeComprasion = this.AttributeComprasionList.AttributeComprasion;
          responceConfigObject.ConfigName = $"{sk.Session.GetObjectType(new Guid(settingItemObject.ObjTypeGUID)).ObjectTypeName} ({sk.Session.GetObject(schemeLinkID).Caption})";
          base.SaveData(sendNotifications);
          if (!sendNotifications)
            return;
          ServiceHolder.NotificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", responceConfigObject.ObjectID));
        }
      }
    }
  }

  private void attrTextBtnTransfScheme_ValueChanged(object sender, EventArgs e)
  {
    this.Modified = true;
    this.AttributeComprasionList.Enabled = this.attrTextBtnTransfScheme.Values.Values[0] != DBNull.Value;
  }

  private void AttributeComprasionList_ListChanged(object sender, EventArgs e)
  {
    this.Modified = true;
  }

  private void tbFindAttr_EditTextChanged(object sender, EventArgs e) => this.Modified = true;

  private void tbFindAttr_ButtonClick(object sender, EventArgs e)
  {
    int attrTypeID = 0;
    try
    {
      Cursor.Current = Cursors.WaitCursor;
      using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false, new int[1]))
      {
        List<int> objType = new List<int>()
        {
          Const.ResponceObjTypeID
        };
        attributesSelectDlg.LoadAttrDialogForObjectsTypes(objType);
        if (attributesSelectDlg.ShowDialog().Equals((object) DialogResult.OK))
        {
          if (attributesSelectDlg.SelectedAttributesID.Count > 0)
            attrTypeID = attributesSelectDlg.SelectedAttributesID[0];
        }
      }
      if (attrTypeID == 0)
        return;
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attrTypeID);
      if (attributeType == null || !(sender is ButtonedEdit buttonedEdit))
        return;
      buttonedEdit.Value = attributeType.Name;
      buttonedEdit.Image = (Image) ServiceHolder.CategoryTypeIconService.GetIcon(3, -1, (object) attributeType.FieldType).ToBitmap();
      buttonedEdit.Tag = (object) attrTypeID;
    }
    finally
    {
      Cursor.Current = Cursors.Default;
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
    this.AttributeComprasionList = new AttributeComprasionList();
    this.attrTextBtnTransfScheme = new AttrTextBtn();
    this.tbFindAttr = new ButtonedEdit();
    this.lblSchemeName = new Label();
    this.pnButtons.SuspendLayout();
    this.SuspendLayout();
    this.AttributeComprasionList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.AttributeComprasionList.Caption = "Соответствие атрибутов";
    this.AttributeComprasionList.Location = new Point(16 /*0x10*/, 49);
    this.AttributeComprasionList.Name = "AttributeComprasionList";
    this.AttributeComprasionList.Padding = new Padding(0, 5, 0, 5);
    this.AttributeComprasionList.Size = new Size(566, 300);
    this.AttributeComprasionList.TabIndex = 4;
    this.attrTextBtnTransfScheme.AttributeInfo = (AttributeInfo) null;
    this.attrTextBtnTransfScheme.BackColor = SystemColors.Window;
    this.attrTextBtnTransfScheme.DataSourceName = (string) null;
    this.attrTextBtnTransfScheme.Location = new Point(16 /*0x10*/, 21);
    this.attrTextBtnTransfScheme.Name = "attrTextBtnTransfScheme";
    this.attrTextBtnTransfScheme.Size = new Size(250, 22);
    this.attrTextBtnTransfScheme.TabIndex = 3;
    this.tbFindAttr.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbFindAttr.ButtonImage = (Image) null;
    this.tbFindAttr.ButtonText = "...";
    this.tbFindAttr.Caption = "Поисковый атрибут";
    this.tbFindAttr.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.tbFindAttr.Image = (Image) null;
    this.tbFindAttr.Location = new Point(282, 5);
    this.tbFindAttr.MinimumSize = new Size(40, 20);
    this.tbFindAttr.Name = "tbFindAttr";
    this.tbFindAttr.Size = new Size(300, 38);
    this.tbFindAttr.TabIndex = 5;
    this.tbFindAttr.ButtonClick += new EventHandler(this.tbFindAttr_ButtonClick);
    this.lblSchemeName.AutoSize = true;
    this.lblSchemeName.Location = new Point(16 /*0x10*/, 5);
    this.lblSchemeName.Name = "lblSchemeName";
    this.lblSchemeName.Size = new Size(123, 13);
    this.lblSchemeName.TabIndex = 6;
    this.lblSchemeName.Text = "Схема трансформации";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.lblSchemeName);
    this.Controls.Add((Control) this.tbFindAttr);
    this.Controls.Add((Control) this.AttributeComprasionList);
    this.Controls.Add((Control) this.attrTextBtnTransfScheme);
    this.Name = nameof (ResponceConfigObjectView);
    this.Size = new Size(600, 400);
    this.Controls.SetChildIndex((Control) this.pnButtons, 0);
    this.Controls.SetChildIndex((Control) this.attrTextBtnTransfScheme, 0);
    this.Controls.SetChildIndex((Control) this.AttributeComprasionList, 0);
    this.Controls.SetChildIndex((Control) this.tbFindAttr, 0);
    this.Controls.SetChildIndex((Control) this.lblSchemeName, 0);
    this.pnButtons.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private sealed class ResponceConfigObjectViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList service))
        service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      INamedImageList namedImageList = service;
      return new ViewDescription()
      {
        Caption = Const.RequestConfigTabName,
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex(Const.ResponceConfigIconName) : -1,
        OrderID = 1
      };
    }
  }
}
