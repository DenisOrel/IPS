// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.AttributeComprasionList
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Actions;
using Intermech.Bars;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class AttributeComprasionList : UserControl
{
  private readonly char[] charSeparators = new char[1]
  {
    '='
  };
  private int _SourceObjTypeID;
  private int _DestinationObjTypeID;
  private IContainer components;
  private Intermech.Bars.ToolBar toolBar;
  private ButtonItem btnAdd;
  private ButtonItem btnEdit;
  private ButtonItem btnDelete;
  private ListView listView;
  private ActionList actList;
  private Intermech.Actions.Action actAdd;
  private Intermech.Actions.Action actEdit;
  private Intermech.Actions.Action actDelete;
  private ColumnHeader colSource;
  private ColumnHeader colDestination;
  private Label label;
  private ColumnHeader colImage;

  public string Caption
  {
    get => this.label.Text;
    set
    {
      if (value.Length > 0)
      {
        this.label.Visible = true;
        this.label.Height = 15;
        this.label.Text = value;
      }
      else
      {
        this.label.Visible = true;
        this.label.Height = 0;
        this.label.Text = "";
      }
    }
  }

  public string[] AttributeComprasion => this.GetAttributeComprasion();

  public event EventHandler ListChanged;

  public AttributeComprasionList() => this.InitializeComponent();

  private string[] GetAttributeComprasion()
  {
    List<string> stringList = new List<string>();
    foreach (ListViewItem listViewItem in this.listView.Items)
    {
      if (listViewItem.Tag != null)
      {
        string tag = (string) listViewItem.Tag;
        if (!string.IsNullOrEmpty(tag))
          stringList.Add(tag);
      }
    }
    return stringList.ToArray();
  }

  private void SetAttributeComprasion(string[] value)
  {
    this.listView.Items.Clear();
    foreach (string str in value)
    {
      if (!string.IsNullOrEmpty(str))
      {
        string[] strArray = str.Split(this.charSeparators, StringSplitOptions.RemoveEmptyEntries);
        int result1;
        int result2;
        if (strArray.Length == 2 && int.TryParse(strArray[0], out result1) && int.TryParse(strArray[1], out result2))
          this.AddListViewItem(result1, result2);
      }
    }
  }

  private void AddListViewItem(int sourceAttrID, int destinationAttrID)
  {
    if (sourceAttrID == 0 || destinationAttrID == 0)
      return;
    IMSAttributeType attributeType1 = MetaDataHelper.GetAttributeType(sourceAttrID);
    IMSAttributeType attributeType2 = MetaDataHelper.GetAttributeType(destinationAttrID);
    if (attributeType1 == null || attributeType2 == null)
      return;
    ListViewItem listViewItem = this.listView.Items.Add("");
    listViewItem.ImageIndex = ServiceHolder.CategoryTypeIconService.IndexOf(3, -1, (object) attributeType1.FieldType);
    listViewItem.SubItems.Add(attributeType1.Name);
    listViewItem.SubItems.Add(attributeType2.Name);
    listViewItem.Tag = (object) string.Join(new string(this.charSeparators), attributeType1.AttributeID.ToString(), attributeType2.AttributeID.ToString());
  }

  public void Activate(
    int ASourceObjTypeID,
    int ADestinationObjTypeID,
    string[] _AttributeComprasion)
  {
    this.listView.SmallImageList = ServiceHolder.CategoryTypeIconService.ImageList;
    this._SourceObjTypeID = ASourceObjTypeID;
    this._DestinationObjTypeID = ADestinationObjTypeID;
    string objectTypeName1 = MetaDataHelper.GetObjectTypeName(this._SourceObjTypeID);
    string objectTypeName2 = MetaDataHelper.GetObjectTypeName(this._DestinationObjTypeID);
    this.colSource.Text = objectTypeName1;
    this.colDestination.Text = objectTypeName2;
    this.SetAttributeComprasion(_AttributeComprasion);
  }

  private void actAdd_Execute(object sender, EventArgs e)
  {
    AttributeComprasionForm attributeComprasionForm = new AttributeComprasionForm(this._SourceObjTypeID, this._DestinationObjTypeID);
    if (!attributeComprasionForm.ShowDialog().Equals((object) DialogResult.OK))
      return;
    this.AddListViewItem(attributeComprasionForm.SourceAttrID, attributeComprasionForm.DestinationAttrID);
    EventHandler listChanged = this.ListChanged;
    if (listChanged == null)
      return;
    listChanged((object) this, new EventArgs());
  }

  private void actDelete_Execute(object sender, EventArgs e)
  {
    this.listView.SelectedItems[0].Remove();
    EventHandler listChanged = this.ListChanged;
    if (listChanged == null)
      return;
    listChanged((object) this, new EventArgs());
  }

  private void actEdit_Execute(object sender, EventArgs e)
  {
    ListViewItem selectedItem = this.listView.SelectedItems[0];
    if (selectedItem == null || selectedItem.Tag == null)
      return;
    string tag = (string) selectedItem.Tag;
    if (string.IsNullOrEmpty(tag))
      return;
    string[] strArray = tag.Split(this.charSeparators, StringSplitOptions.RemoveEmptyEntries);
    int result1;
    int result2;
    if (strArray.Length != 2 || !int.TryParse(strArray[0], out result1) || !int.TryParse(strArray[1], out result2))
      return;
    AttributeComprasionForm attributeComprasionForm = new AttributeComprasionForm(this._SourceObjTypeID, this._DestinationObjTypeID, result1, result2);
    if (!attributeComprasionForm.ShowDialog().Equals((object) DialogResult.OK))
      return;
    IMSAttributeType attributeType1 = MetaDataHelper.GetAttributeType(attributeComprasionForm.SourceAttrID);
    IMSAttributeType attributeType2 = MetaDataHelper.GetAttributeType(attributeComprasionForm.DestinationAttrID);
    selectedItem.ImageIndex = ServiceHolder.CategoryTypeIconService.IndexOf(3, -1, (object) attributeType1.FieldType);
    selectedItem.SubItems.Clear();
    selectedItem.SubItems.Add(attributeType1.Name);
    selectedItem.SubItems.Add(attributeType2.Name);
    selectedItem.Tag = (object) string.Join(new string(this.charSeparators), attributeComprasionForm.SourceAttrID.ToString(), attributeComprasionForm.DestinationAttrID.ToString());
    EventHandler listChanged = this.ListChanged;
    if (listChanged == null)
      return;
    listChanged((object) this, new EventArgs());
  }

  private void actDelete_Update(object sender, EventArgs e)
  {
    if (this.DesignMode)
      return;
    this.actDelete.Enabled = this.listView.SelectedItems.Count > 0;
  }

  private void actEdit_Update(object sender, EventArgs e)
  {
    if (this.DesignMode)
      return;
    this.actEdit.Enabled = this.listView.SelectedItems.Count > 0;
  }

  private void listView_Resize(object sender, EventArgs e)
  {
    this.colDestination.Width = this.colSource.Width = (this.Width - this.colImage.Width - 10) / 2;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AttributeComprasionList));
    this.toolBar = new Intermech.Bars.ToolBar();
    this.btnAdd = new ButtonItem();
    this.btnEdit = new ButtonItem();
    this.btnDelete = new ButtonItem();
    this.listView = new ListView();
    this.colImage = new ColumnHeader();
    this.colSource = new ColumnHeader();
    this.colDestination = new ColumnHeader();
    this.actList = new ActionList(this.components);
    this.actAdd = new Intermech.Actions.Action(this.components);
    this.actEdit = new Intermech.Actions.Action(this.components);
    this.actDelete = new Intermech.Actions.Action(this.components);
    this.label = new Label();
    this.SuspendLayout();
    this.toolBar.FullMenus = true;
    this.toolBar.Guid = new Guid("5d760550-8ad3-4586-9eb4-742a7b97c189");
    this.toolBar.Hidden = false;
    this.toolBar.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.btnAdd,
      (ToolbarItemBase) this.btnEdit,
      (ToolbarItemBase) this.btnDelete
    });
    this.toolBar.Location = new Point(0, 0);
    this.toolBar.Name = "toolBar";
    this.toolBar.Size = new Size(540, 24);
    this.toolBar.TabIndex = 0;
    this.toolBar.Text = "toolBar1";
    this.actList.SetAction((Component) this.btnAdd, this.actAdd);
    this.btnAdd.CommandName = "btnAdd";
    this.btnAdd.Icon = (Icon) componentResourceManager.GetObject("btnAdd.Icon");
    this.btnAdd.Text = "Добавить";
    this.btnAdd.ToolTipText = "Добавить";
    this.actList.SetAction((Component) this.btnEdit, this.actEdit);
    this.btnEdit.CommandName = "btnEdit";
    this.btnEdit.Icon = (Icon) componentResourceManager.GetObject("btnEdit.Icon");
    this.btnEdit.Text = "Редактировать";
    this.btnEdit.ToolTipText = "Редактировать";
    this.actList.SetAction((Component) this.btnDelete, this.actDelete);
    this.btnDelete.CommandName = "btnDelete";
    this.btnDelete.Icon = (Icon) componentResourceManager.GetObject("btnDelete.Icon");
    this.btnDelete.Text = "Удалить";
    this.btnDelete.ToolTipText = "Удалить";
    this.listView.Columns.AddRange(new ColumnHeader[3]
    {
      this.colImage,
      this.colSource,
      this.colDestination
    });
    this.listView.Dock = DockStyle.Fill;
    this.listView.FullRowSelect = true;
    this.listView.GridLines = true;
    this.listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    this.listView.Location = new Point(0, 24);
    this.listView.MultiSelect = false;
    this.listView.Name = "listView";
    this.listView.Size = new Size(540, 235);
    this.listView.TabIndex = 1;
    this.listView.UseCompatibleStateImageBehavior = false;
    this.listView.View = View.Details;
    this.listView.Resize += new EventHandler(this.listView_Resize);
    this.colImage.Text = "";
    this.colImage.Width = 32 /*0x20*/;
    this.colSource.Text = "colSource";
    this.colSource.Width = 250;
    this.colDestination.Text = "colDestination";
    this.colDestination.Width = 250;
    this.actList.Actions.AddRange(new Intermech.Actions.Action[3]
    {
      this.actAdd,
      this.actEdit,
      this.actDelete
    });
    this.actList.ImageList = (ImageList) null;
    this.actList.ShowTextOnToolBar = false;
    this.actList.Tag = (object) null;
    this.actAdd.Hint = (string) null;
    this.actAdd.Text = "Добавить";
    this.actAdd.Execute += new EventHandler(this.actAdd_Execute);
    this.actEdit.Hint = (string) null;
    this.actEdit.Text = "Редактировать";
    this.actEdit.Execute += new EventHandler(this.actEdit_Execute);
    this.actEdit.Update += new EventHandler(this.actEdit_Update);
    this.actDelete.Hint = (string) null;
    this.actDelete.Text = "Удалить";
    this.actDelete.Execute += new EventHandler(this.actDelete_Execute);
    this.actDelete.Update += new EventHandler(this.actDelete_Update);
    this.label.Dock = DockStyle.Top;
    this.label.Location = new Point(0, 0);
    this.label.Name = "label";
    this.label.Size = new Size(540, 0);
    this.label.TabIndex = 2;
    this.label.Visible = false;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.listView);
    this.Controls.Add((Control) this.toolBar);
    this.Controls.Add((Control) this.label);
    this.Name = nameof (AttributeComprasionList);
    this.Size = new Size(540, 259);
    this.ResumeLayout(false);
  }
}
