// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.AddresseeEditor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Bars;
using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Office.Client.Editors;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class AddresseeEditor : Form
{
  private readonly long _officeDocID;
  [NotNull]
  private readonly DesForm _dForm;
  private OfficeDocumentTypes _type;
  private bool _changed;
  private IContainer components;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;
  private ListView listView1;
  private Intermech.Bars.ToolBar toolBar1;
  private Panel panel2;
  private ImageList imageList1;
  private ButtonItem biAdd;
  private ButtonItem biEdit;
  private ButtonItem biDelete;

  public AddresseeEditor(long officeDocID, [NotNull] DesForm dForm)
  {
    this.InitializeComponent();
    this._officeDocID = officeDocID;
    this._dForm = dForm;
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 2586);
  }

  private void AddColumns(bool internalAddressee)
  {
    this.listView1.Columns.Clear();
    ColumnHeader columnHeader = new ColumnHeader();
    columnHeader.Text = Localization.GetString("Office.Client_5");
    if (internalAddressee)
    {
      columnHeader.Width = this.listView1.Width;
      this.listView1.Columns.Add(columnHeader);
    }
    else
    {
      columnHeader.Width = 214;
      this.listView1.Columns.AddRange(new ColumnHeader[4]
      {
        columnHeader,
        new ColumnHeader()
        {
          Text = Localization.GetString("Office.Client_6"),
          Width = 117
        },
        new ColumnHeader()
        {
          Text = Localization.GetString("Office.Client_7"),
          Width = 139
        },
        new ColumnHeader()
        {
          Text = Localization.GetString("Office.Client_8"),
          Width = 183
        }
      });
    }
  }

  [NotNull]
  private ListViewItem AddItem([NotNull] string caption, long addresseeID, int imageIndex)
  {
    ListViewItem listViewItem = new ListViewItem(caption);
    listViewItem.Tag = (object) addresseeID;
    listViewItem.ImageIndex = imageIndex;
    this.listView1.Items.Add(listViewItem);
    return listViewItem;
  }

  private void AddAddressee(
    long addresseeID,
    [NotNull] IUserSession session,
    [NotNull] List<AttributeValues> attributes,
    int index,
    bool multi)
  {
    IDBObject dbObject = session.GetObject(addresseeID);
    if (this._type != OfficeDocumentTypes.Outgoing)
    {
      this.AddItem(dbObject.Caption, dbObject.ObjectID, 3);
    }
    else
    {
      ListViewItem listViewItem = this.AddItem(dbObject.Caption, dbObject.ObjectID, 4);
      int recipient = multi ? OfficeConsts.AttrDocRecipientsID : OfficeConsts.AttrDocRecipientID;
      AttributeValues attributeValues1 = attributes.Find((Predicate<AttributeValues>) (x => x.AttributeID == recipient));
      listViewItem.SubItems.Add(attributeValues1?.Values == null || attributeValues1.Values.Length == 0 ? string.Empty : Convert.ToString(attributeValues1.Values[index]));
      int regNum = multi ? OfficeConsts.AttrInputRegNumsID : OfficeConsts.AttrInputRegNumID;
      AttributeValues attributeValues2 = attributes.Find((Predicate<AttributeValues>) (x => x.AttributeID == regNum));
      listViewItem.SubItems.Add(attributeValues2?.Values == null || attributeValues2.Values.Length == 0 ? string.Empty : Convert.ToString(attributeValues2.Values[index]));
      int regDate = multi ? OfficeConsts.AttrAddresseeRegDatesID : OfficeConsts.AttrAddresseeRegDateID;
      AttributeValues attributeValues3 = attributes.Find((Predicate<AttributeValues>) (x => x.AttributeID == regDate));
      listViewItem.SubItems.Add(attributeValues3?.Values == null || attributeValues3.Values.Length == 0 ? string.Empty : Convert.ToString(attributeValues3.Values[index], (IFormatProvider) CultureInfo.CurrentCulture));
    }
  }

  internal void Init()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject iDbAttributable = sessionKeeper.Session.GetObject(this._officeDocID);
      this._type = (OfficeDocumentTypes) iDbAttributable.AttributeByID(OfficeConsts.AttrOfficeDocumentTypeID).AsInteger;
      this.AddColumns(this._type != OfficeDocumentTypes.Outgoing);
      List<AttributeValues> additionalValues = this._dForm.GetAdditionalValues(this._officeDocID);
      bool flag = false;
      AttributeValues attributeValues = additionalValues.Find((Predicate<AttributeValues>) (x => x.AttributeID == OfficeConsts.AttrAddresseesID));
      if (attributeValues?.Values != null && attributeValues.Values.Length != 0)
      {
        for (int index = 0; index < attributeValues.Values.Length; ++index)
        {
          long int64 = Convert.ToInt64(attributeValues.Values[index]);
          if (int64 != 0L)
            this.AddAddressee(int64, sessionKeeper.Session, additionalValues, index, true);
        }
        flag = true;
      }
      if (flag)
        return;
      IDBAttribute attributeById = iDbAttributable.GetAttributeByID(OfficeConsts.AttrAddresseesID);
      if (attributeById == null || attributeById.IsNull || attributeById.ValuesCount <= 0)
        return;
      for (int index = 0; index < attributeById.ValuesCount; ++index)
      {
        attributeById.Index = index;
        IDBObject dbObject = ((IDBObjectLinkAttribute) attributeById).DBObject;
        if (this._type != OfficeDocumentTypes.Outgoing)
        {
          this.AddItem(dbObject.Caption, dbObject.ObjectID, 3);
        }
        else
        {
          ListViewItem listViewItem = this.AddItem(dbObject.Caption, dbObject.ObjectID, 4);
          IDBAttribute dbAttribute1 = iDbAttributable.AttributeByID(OfficeConsts.AttrDocRecipientsID);
          dbAttribute1.Index = index;
          listViewItem.SubItems.Add(dbAttribute1.AsString);
          IDBAttribute dbAttribute2 = iDbAttributable.AttributeByID(OfficeConsts.AttrInputRegNumsID);
          dbAttribute2.Index = index;
          listViewItem.SubItems.Add(dbAttribute2.AsString);
          IDBAttribute dbAttribute3 = iDbAttributable.AttributeByID(OfficeConsts.AttrAddresseeRegDatesID);
          dbAttribute3.Index = index;
          listViewItem.SubItems.Add(Convert.ToString(dbAttribute3.AsDateTime, (IFormatProvider) CultureInfo.CurrentCulture));
        }
      }
    }
  }

  private void RefreshButtons()
  {
    this.biEdit.Enabled = this.biDelete.Enabled = this.listView1.SelectedItems.Count > 0;
  }

  private void biAdd_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    ListViewItem listViewItem = (ListViewItem) null;
    if (this._type != OfficeDocumentTypes.Outgoing)
    {
      object[] objArray = SelectionWindow.Select(Localization.GetString("Office.Client_9"), OfficeClientHelper.GetAddresseesDescriptor(), typeof (IDBTypedObjectID), SelectionOptions.SelectObjects, OfficeClientHelper.AddresseeTypes);
      if (objArray != null && objArray.Length != 0)
      {
        foreach (object obj in objArray)
        {
          long objectId = ((IDBTypedObjectID) obj).ObjectID;
          if (!this.ItemExist(objectId))
          {
            if (OfficeClientHelper.CheckDirector((IDBTypedObjectID) obj))
            {
              listViewItem = this.AddItem(((IDBObjectID) obj).Caption, objectId, 3);
              this._changed = true;
            }
            else
              break;
          }
        }
      }
    }
    else
    {
      using (ExternalAddresseeForm externalAddresseeForm = new ExternalAddresseeForm(Localization.GetString("Office.Client_10"), this._officeDocID, this.listView1.Items.Count >= 1))
      {
        if (externalAddresseeForm.ShowDialog() == DialogResult.OK)
        {
          if (externalAddresseeForm.AddresseeID != 0L)
          {
            if (!this.ItemExist(externalAddresseeForm.AddresseeID))
            {
              listViewItem = this.AddItem(externalAddresseeForm.AddresseeCaption, externalAddresseeForm.AddresseeID, 4);
              listViewItem.SubItems.Add(externalAddresseeForm.DocRecipients);
              listViewItem.SubItems.Add(externalAddresseeForm.RegNum);
              listViewItem.SubItems.Add(Convert.ToString(externalAddresseeForm.RegDate, (IFormatProvider) CultureInfo.CurrentCulture));
              this._changed = true;
            }
          }
        }
      }
    }
    if (listViewItem == null)
      return;
    listViewItem.Focused = true;
    listViewItem.Selected = true;
    this.RefreshButtons();
  }

  private bool ItemExist(long objectID)
  {
    for (int index = 0; index < this.listView1.Items.Count; ++index)
    {
      if ((long) this.listView1.Items[index].Tag == objectID)
        return true;
    }
    return false;
  }

  private void biEdit_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this.listView1.FocusedItem == null)
      return;
    if (this._type != OfficeDocumentTypes.Outgoing)
    {
      object[] objArray = SelectionWindow.Select(Localization.GetString("Office.Client_11"), OfficeClientHelper.GetAddresseesDescriptor(), typeof (IDBTypedObjectID), SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect, OfficeClientHelper.AddresseeTypes);
      if (objArray == null || objArray.Length == 0)
        return;
      long objectId = ((IDBTypedObjectID) objArray[0]).ObjectID;
      if (this.ItemExist(objectId) || !OfficeClientHelper.CheckDirector((IDBTypedObjectID) objArray[0]))
        return;
      this.listView1.FocusedItem.Tag = (object) objectId;
      this.listView1.FocusedItem.Text = ((IDBObjectID) objArray[0]).Caption;
      this._changed = true;
    }
    else
    {
      using (ExternalAddresseeForm externalAddresseeForm = new ExternalAddresseeForm(Localization.GetString("Office.Client_12"), this._officeDocID, this.listView1.Items.Count >= 1))
      {
        externalAddresseeForm.Init((long) this.listView1.FocusedItem.Tag, this.listView1.FocusedItem.Text, this.listView1.FocusedItem.SubItems[1].Text, this.listView1.FocusedItem.SubItems[2].Text, this.listView1.FocusedItem.SubItems[3].Text != string.Empty ? Convert.ToDateTime(this.listView1.FocusedItem.SubItems[3].Text, (IFormatProvider) CultureInfo.CurrentCulture) : DateTime.Now);
        if (externalAddresseeForm.ShowDialog() != DialogResult.OK || this.ItemExist(externalAddresseeForm.AddresseeID))
          return;
        this.listView1.FocusedItem.Tag = (object) externalAddresseeForm.AddresseeID;
        this.listView1.FocusedItem.Text = externalAddresseeForm.AddresseeCaption;
        this.listView1.FocusedItem.SubItems[1].Text = externalAddresseeForm.DocRecipients;
        this.listView1.FocusedItem.SubItems[2].Text = externalAddresseeForm.RegNum;
        this.listView1.FocusedItem.SubItems[3].Text = Convert.ToString(externalAddresseeForm.RegDate, (IFormatProvider) CultureInfo.CurrentCulture);
        this._changed = true;
      }
    }
  }

  private void biDelete_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this.listView1.FocusedItem != null)
      this.listView1.Items.Remove(this.listView1.FocusedItem);
    if (this.listView1.Items.Count > 0)
    {
      this.listView1.Items[this.listView1.Items.Count - 1].Focused = true;
      this.listView1.Items[this.listView1.Items.Count - 1].Selected = true;
    }
    this._changed = true;
    this.RefreshButtons();
  }

  private void AddresseeEditor_Load([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    Intermech.Client.Core.FormStorage.LoadLayout((Control) this);
  }

  private void AddresseeEditor_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
  {
    Intermech.Client.Core.FormStorage.SaveLayout((Control) this);
  }

  private void bOK_Click([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (!this._changed)
    {
      this.Close();
    }
    else
    {
      List<AttributeValues> values = new List<AttributeValues>();
      if (this.listView1.Items.Count == 0)
      {
        values.Add(new AttributeValues(OfficeConsts.AttrAddresseesID, (object) DeleteModesEnum.None));
        values.Add(new AttributeValues(OfficeConsts.AttrDocRecipientID, (object) DeleteModesEnum.None));
        values.Add(new AttributeValues(OfficeConsts.AttrDocRecipientsID, (object) DeleteModesEnum.None));
        values.Add(new AttributeValues(OfficeConsts.AttrInputRegNumID, (object) DeleteModesEnum.None));
        values.Add(new AttributeValues(OfficeConsts.AttrInputRegNumsID, (object) DeleteModesEnum.None));
        values.Add(new AttributeValues(OfficeConsts.AttrAddresseeRegDateID, (object) DeleteModesEnum.None));
        values.Add(new AttributeValues(OfficeConsts.AttrAddresseeRegDatesID, (object) DeleteModesEnum.None));
        this._dForm.SetAdditionalValues(this._officeDocID, values, true);
      }
      else
      {
        values.Add(new AttributeValues(OfficeConsts.AttrDocRecipientID, (object) DeleteModesEnum.None));
        values.Add(new AttributeValues(OfficeConsts.AttrInputRegNumID, (object) DeleteModesEnum.None));
        values.Add(new AttributeValues(OfficeConsts.AttrAddresseeRegDateID, (object) DeleteModesEnum.None));
        List<object> enumerable = new List<object>();
        for (int index = 0; index < this.listView1.Items.Count; ++index)
          enumerable.Add((object) (long) this.listView1.Items[index].Tag);
        values.Add(new AttributeValues(OfficeConsts.AttrAddresseesID, (object) enumerable.AsArray<object>()));
        if (this._type == OfficeDocumentTypes.Outgoing)
        {
          List<object> objectList1 = new List<object>();
          List<object> objectList2 = new List<object>();
          List<object> objectList3 = new List<object>();
          for (int index = 0; index < this.listView1.Items.Count; ++index)
          {
            objectList1.Add((object) this.listView1.Items[index].SubItems[1].Text);
            objectList2.Add((object) this.listView1.Items[index].SubItems[2].Text);
            objectList3.Add((object) Convert.ToDateTime(this.listView1.Items[index].SubItems[3].Text, (IFormatProvider) CultureInfo.CurrentCulture));
          }
          values.Add(new AttributeValues(OfficeConsts.AttrDocRecipientsID, (object) objectList1.ToArray()));
          values.Add(new AttributeValues(OfficeConsts.AttrInputRegNumsID, (object) objectList2.ToArray()));
          values.Add(new AttributeValues(OfficeConsts.AttrAddresseeRegDatesID, (object) objectList3.ToArray()));
        }
        else
        {
          values.Add(new AttributeValues(OfficeConsts.AttrDocRecipientsID, (object) DeleteModesEnum.None));
          values.Add(new AttributeValues(OfficeConsts.AttrInputRegNumsID, (object) DeleteModesEnum.None));
          values.Add(new AttributeValues(OfficeConsts.AttrAddresseeRegDatesID, (object) DeleteModesEnum.None));
        }
      }
      this._dForm.SetAdditionalValues(this._officeDocID, values, false);
    }
  }

  private void listView1_SelectedIndexChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    this.RefreshButtons();
  }

  private void AddresseeEditor_Shown([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this.listView1.Items.Count > 0)
    {
      this.listView1.Items[0].Focused = true;
      this.listView1.Items[0].Selected = true;
    }
    this.RefreshButtons();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddresseeEditor));
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.listView1 = new ListView();
    this.toolBar1 = new Intermech.Bars.ToolBar();
    this.imageList1 = new ImageList();
    this.biAdd = new ButtonItem();
    this.biEdit = new ButtonItem();
    this.biDelete = new ButtonItem();
    this.panel2 = new Panel();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    componentResourceManager.ApplyResources((object) this.listView1, "listView1");
    this.listView1.GridLines = true;
    this.listView1.MultiSelect = false;
    this.listView1.Name = "listView1";
    this.listView1.UseCompatibleStateImageBehavior = false;
    this.listView1.View = View.Details;
    this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
    this.toolBar1.FullMenus = true;
    this.toolBar1.Guid = new Guid("9f3c8354-f4db-4d7a-bbb1-3b4ad674492c");
    this.toolBar1.Hidden = false;
    this.toolBar1.ImageList = this.imageList1;
    this.toolBar1.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.biAdd,
      (ToolbarItemBase) this.biEdit,
      (ToolbarItemBase) this.biDelete
    });
    componentResourceManager.ApplyResources((object) this.toolBar1, "toolBar1");
    this.toolBar1.Name = "toolBar1";
    this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
    this.imageList1.TransparentColor = Color.Transparent;
    this.imageList1.Images.SetKeyName(0, "добавить.png");
    this.imageList1.Images.SetKeyName(1, "удалить.png");
    this.imageList1.Images.SetKeyName(2, "редактировать.png");
    this.imageList1.Images.SetKeyName(3, "user1.png");
    this.imageList1.Images.SetKeyName(4, "factory.png");
    componentResourceManager.ApplyResources((object) this.biAdd, "biAdd");
    this.biAdd.ImageIndex = 0;
    this.biAdd.Click += new EventHandler(this.biAdd_Click);
    this.biEdit.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.biEdit, "biEdit");
    this.biEdit.ImageIndex = 2;
    this.biEdit.Click += new EventHandler(this.biEdit_Click);
    componentResourceManager.ApplyResources((object) this.biDelete, "biDelete");
    this.biDelete.ImageIndex = 1;
    this.biDelete.Click += new EventHandler(this.biDelete_Click);
    this.panel2.Controls.Add((Control) this.listView1);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.toolBar1);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (AddresseeEditor);
    this.FormClosing += new FormClosingEventHandler(this.AddresseeEditor_FormClosing);
    this.Load += new EventHandler(this.AddresseeEditor_Load);
    this.Shown += new EventHandler(this.AddresseeEditor_Shown);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
