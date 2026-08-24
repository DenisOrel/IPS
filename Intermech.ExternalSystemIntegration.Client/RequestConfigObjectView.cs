// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.RequestConfigObjectView
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

[ViewDescriptionProvider(typeof (RequestConfigObjectView.RequestConfigObjectViewDescriptionProvider))]
public class RequestConfigObjectView : NavBaseView
{
  internal ICommandManager _commandManager;
  private INamedImageList _namedImageList;
  private IContainer components;
  private AttrTextBtn attrTextBtnTransfScheme;
  private AttributeComprasionList AttributeComprasionList;
  private CheckBox chbShowCard;
  private ButtonedEdit tbRequestFileName;
  private Label lblSchemeName;

  public RequestConfigObjectView() => this.InitializeComponent();

  public override string Caption => Const.RequestConfigTabName;

  public override int ImageIndex
  {
    get
    {
      INamedImageList namedImageList = this._namedImageList;
      return namedImageList == null ? -1 : namedImageList.ImageIndex(Const.RequestConfigIconName);
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
      this.chbShowCard.CheckedChanged -= new EventHandler(this.chbShowCard_CheckedChanged);
      this.tbRequestFileName.EditTextChanged -= new EventHandler(this.tbRequestFileName_EditTextChanged);
      if (!(sessionKeeper.Session.GetObject(this._objID, true) is IRequestConfigObject requestConfigObject))
        return;
      this.chbShowCard.Checked = requestConfigObject.ShowCard;
      this.tbRequestFileName.Value = requestConfigObject.FileName;
      IDBAttribute byId = sessionKeeper.Session.GetObject(this._objID).Attributes.FindByID(Const.RequestSchemeLinkAttrTypeID);
      if (byId != null)
      {
        this.attrTextBtnTransfScheme.AttributeInfo = new AttributeInfo(Const.RequestSchemeLinkAttrTypeGUID, Const.RequestConfigObjTypeGUID);
        this.attrTextBtnTransfScheme.Values = new AttributeValues(Const.RequestSchemeLinkAttrTypeID, byId.Value);
      }
      long settingItemObjectId = requestConfigObject.ObjTypeSettingItemObjectID;
      if (!(sessionKeeper.Session.GetObject(settingItemObjectId, true) is IObjTypeSettingItemObject settingItemObject))
        return;
      this.AttributeComprasionList.Activate(MetaDataHelper.GetObjectType(new Guid(settingItemObject.ObjTypeGUID)).ObjectTypeID, Const.RequestObjTypeID, requestConfigObject.AttributeComprasion);
      this.AttributeComprasionList.Enabled = this.attrTextBtnTransfScheme.Values.Values[0] != DBNull.Value;
      this.attrTextBtnTransfScheme.ValueChanged += new EventHandler(this.attrTextBtnTransfScheme_ValueChanged);
      this.AttributeComprasionList.ListChanged += new EventHandler(this.AttributeComprasionList_ListChanged);
      this.chbShowCard.CheckedChanged += new EventHandler(this.chbShowCard_CheckedChanged);
      this.tbRequestFileName.EditTextChanged += new EventHandler(this.tbRequestFileName_EditTextChanged);
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
    else
    {
      using (SessionKeeper sk = new SessionKeeper())
      {
        long schemeLinkID = (long) this.attrTextBtnTransfScheme.Values.Values[0];
        IRequestConfigObject requestConfigObject;
        if ((requestConfigObject = sk.Session.GetObject(this._objID, true) as IRequestConfigObject) == null)
          return;
        long settingItemObjectId = requestConfigObject.ObjTypeSettingItemObjectID;
        if (!(sk.Session.GetObject(settingItemObjectId, true) is IObjTypeSettingItemObject settingItemObject))
          return;
        if (((IEnumerable<long>) ((IEnumerable<long>) settingItemObject.RequestConfigs).Select<long, IRequestConfigObject>((Func<long, IRequestConfigObject>) (x => sk.Session.GetObject(x) as IRequestConfigObject)).Where<IRequestConfigObject>((Func<IRequestConfigObject, bool>) (x => x != null)).Select<IRequestConfigObject, long>((Func<IRequestConfigObject, long>) (x => x.SchemeTransfLink)).ToArray<long>()).Any<long>((Func<long, bool>) (x => x == schemeLinkID && x != requestConfigObject.SchemeTransfLink)))
        {
          int num2 = (int) MessageBox.Show($"Указанная схема трансформации уже используется{Environment.NewLine} Изменения не будут сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          requestConfigObject.SchemeTransfLink = schemeLinkID;
          requestConfigObject.AttributeComprasion = this.AttributeComprasionList.AttributeComprasion;
          requestConfigObject.ShowCard = this.chbShowCard.Checked;
          requestConfigObject.FileName = this.tbRequestFileName.Value;
          requestConfigObject.ConfigName = $"{sk.Session.GetObjectType(new Guid(settingItemObject.ObjTypeGUID)).ObjectTypeName} ({sk.Session.GetObject(schemeLinkID).Caption})";
          base.SaveData(sendNotifications);
          if (!sendNotifications)
            return;
          ServiceHolder.NotificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", requestConfigObject.ObjectID));
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

  private void chbShowCard_CheckedChanged(object sender, EventArgs e) => this.Modified = true;

  private void tbRequestFileName_EditTextChanged(object sender, EventArgs e)
  {
    this.Modified = true;
  }

  private void tbRequestFileName_ButtonClick(object sender, EventArgs e)
  {
    int attrTypeID = -1;
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false))
    {
      attributesSelectDlg.LoadAttrDialogForObjectsTypes(MetaDataHelper.GetObjectTypeGuid(Const.RequestObjTypeID));
      if (attributesSelectDlg.ShowDialog().Equals((object) DialogResult.OK))
      {
        if (attributesSelectDlg.SelectedAttributesID.Count > 0)
          attrTypeID = attributesSelectDlg.SelectedAttributesID[0];
      }
    }
    if (attrTypeID == 0)
      return;
    this.tbRequestFileName.Value = this.tbRequestFileName.Value.Insert(this.tbRequestFileName.CaretPosition, $"[{MetaDataHelper.GetAttributeTypeName(attrTypeID)}]");
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.attrTextBtnTransfScheme = new AttrTextBtn();
    this.AttributeComprasionList = new AttributeComprasionList();
    this.chbShowCard = new CheckBox();
    this.tbRequestFileName = new ButtonedEdit();
    this.lblSchemeName = new Label();
    this.pnButtons.SuspendLayout();
    this.SuspendLayout();
    this.pnButtons.Location = new Point(2, 358);
    this.pnButtons.Size = new Size(596, 40);
    this.btApply.Location = new Point(343, 7);
    this.btCancel.Location = new Point(470, 7);
    this.attrTextBtnTransfScheme.BackColor = SystemColors.Window;
    this.attrTextBtnTransfScheme.DataSourceName = (string) null;
    this.attrTextBtnTransfScheme.Location = new Point(16 /*0x10*/, 21);
    this.attrTextBtnTransfScheme.Name = "attrTextBtnTransfScheme";
    this.attrTextBtnTransfScheme.Size = new Size(250, 22);
    this.attrTextBtnTransfScheme.TabIndex = 0;
    this.AttributeComprasionList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.AttributeComprasionList.Caption = "Соответствие атрибутов";
    this.AttributeComprasionList.Location = new Point(16 /*0x10*/, 49);
    this.AttributeComprasionList.Name = "AttributeComprasionList";
    this.AttributeComprasionList.Padding = new Padding(0, 5, 0, 5);
    this.AttributeComprasionList.Size = new Size(566, 275);
    this.AttributeComprasionList.TabIndex = 2;
    this.chbShowCard.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.chbShowCard.AutoSize = true;
    this.chbShowCard.Location = new Point(16 /*0x10*/, 330);
    this.chbShowCard.Name = "chbShowCard";
    this.chbShowCard.Size = new Size(123, 17);
    this.chbShowCard.TabIndex = 8;
    this.chbShowCard.Text = "Показать карточку";
    this.chbShowCard.UseVisualStyleBackColor = true;
    this.tbRequestFileName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbRequestFileName.ButtonImage = (Image) null;
    this.tbRequestFileName.ButtonText = "...";
    this.tbRequestFileName.Caption = "Имя файла";
    this.tbRequestFileName.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.tbRequestFileName.Image = (Image) null;
    this.tbRequestFileName.Location = new Point(282, 5);
    this.tbRequestFileName.MinimumSize = new Size(40, 20);
    this.tbRequestFileName.Name = "tbRequestFileName";
    this.tbRequestFileName.Size = new Size(300, 38);
    this.tbRequestFileName.TabIndex = 9;
    this.tbRequestFileName.ButtonClick += new EventHandler(this.tbRequestFileName_ButtonClick);
    this.lblSchemeName.AutoSize = true;
    this.lblSchemeName.Location = new Point(16 /*0x10*/, 5);
    this.lblSchemeName.Name = "lblSchemeName";
    this.lblSchemeName.Size = new Size(123, 13);
    this.lblSchemeName.TabIndex = 10;
    this.lblSchemeName.Text = "Схема трансформации";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.lblSchemeName);
    this.Controls.Add((Control) this.tbRequestFileName);
    this.Controls.Add((Control) this.chbShowCard);
    this.Controls.Add((Control) this.AttributeComprasionList);
    this.Controls.Add((Control) this.attrTextBtnTransfScheme);
    this.Name = nameof (RequestConfigObjectView);
    this.Size = new Size(600, 400);
    this.Controls.SetChildIndex((Control) this.attrTextBtnTransfScheme, 0);
    this.Controls.SetChildIndex((Control) this.AttributeComprasionList, 0);
    this.Controls.SetChildIndex((Control) this.pnButtons, 0);
    this.Controls.SetChildIndex((Control) this.chbShowCard, 0);
    this.Controls.SetChildIndex((Control) this.tbRequestFileName, 0);
    this.Controls.SetChildIndex((Control) this.lblSchemeName, 0);
    this.pnButtons.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private sealed class RequestConfigObjectViewDescriptionProvider : BaseViewDescriptionProvider
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
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex(Const.RequestConfigIconName) : -1,
        OrderID = 1
      };
    }
  }
}
