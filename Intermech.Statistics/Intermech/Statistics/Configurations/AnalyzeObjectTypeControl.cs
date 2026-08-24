// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Configurations.AnalyzeObjectTypeControl
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using Intermech.Signs.Client;
using Intermech.Statistics.Interfaces;
using Intermech.Statistics.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics.Configurations;

public class AnalyzeObjectTypeControl : UserControl
{
  private List<ObjectTypesListItem> _typesObjectTypesListItems = new List<ObjectTypesListItem>();
  private int _attrId;
  private CommandStatisticsTypesEnum _statisticsCommandType;
  private IContainer components;
  private ToolTip toolTip1;
  private ListView listView1;
  private ColumnHeader typeName;
  private Intermech.Bars.ToolBar toolBar1;
  private ButtonItem btnAdd;
  private ButtonItem btnDelete;
  private ButtonItem btnRemoveChildTypes;

  public event AnalyzeObjectTypeControl.ModifyItems ItemsChanged;

  public List<ObjectTypesListItem> TypesListItems
  {
    get => this._typesObjectTypesListItems;
    private set => this._typesObjectTypesListItems = value;
  }

  public AnalyzeObjectTypeControl()
  {
    this.InitializeComponent();
    this.listView1.SmallImageList = Statics.IconSrv == null ? (ImageList) null : Statics.IconSrv.ImageList;
    this.btnDelete.Enabled = false;
  }

  public void Init(CommandStatisticsTypesEnum statisticsCommandType)
  {
    this._statisticsCommandType = statisticsCommandType;
  }

  public void Init(
    List<ObjectTypesListItem> objectTypes,
    CommandStatisticsTypesEnum statisticsCommandType)
  {
    this.listView1.BeginUpdate();
    this.listView1.Items.Clear();
    foreach (ObjectTypesListItem objectType in objectTypes)
    {
      if (MetaDataHelper.GetObjectType(objectType.ObjectTypeID) != null)
      {
        ListViewItem listViewItem = new ListViewItem(objectType.ToString());
        listViewItem.Tag = (object) objectType.ObjectTypeID;
        if (Statics.IconSrv != null)
        {
          int num = Statics.IconSrv.IndexOf(4, objectType.ObjectTypeID);
          listViewItem.ImageIndex = num;
        }
        this.listView1.Items.Add(listViewItem);
      }
    }
    this.listView1.EndUpdate();
    this.TypesListItems = objectTypes;
    this._statisticsCommandType = statisticsCommandType;
  }

  public void ClearTypes()
  {
    this.listView1.Items.Clear();
    this._typesObjectTypesListItems.Clear();
  }

  public void SetAttrForFilter(int attrId) => this._attrId = attrId;

  private void SetSelectorFormFilters(SelectorForm objTypeSelectorForm)
  {
    switch (this._statisticsCommandType)
    {
      case CommandStatisticsTypesEnum.SignDate:
        objTypeSelectorForm.SelectorFilter = (ISelectorFilter) new FilterObjectType();
        break;
      case CommandStatisticsTypesEnum.DateAttrValue:
        List<int> typesWithDateAttr = this.FindAvailableTypesWithDateAttr();
        List<int> parentTypes = AnalyzeObjectTypeControl.FindParentTypes(typesWithDateAttr);
        objTypeSelectorForm.SelectorFilter = (ISelectorFilter) new CustomSelectorFilter(typesWithDateAttr, parentTypes);
        objTypeSelectorForm.NodeSelectorFilter = (INodeSelectorFilter) new CustomNodeFilter(typesWithDateAttr);
        break;
    }
  }

  private static List<int> FindParentTypes(List<int> availableTypes)
  {
    List<int> collection = new List<int>();
    foreach (int availableType in availableTypes)
      collection.SafeAddRange<int>((IEnumerable<int>) MetaDataHelper.GetObjectTypeParentsID(availableType));
    return collection;
  }

  private List<int> FindAvailableTypesWithDateAttr()
  {
    List<IMSAttribute4ObjectType> attributes4ObjectTypeList = MetaDataHelper.GetAllAttributes4ObjectTypeList(this._attrId);
    List<int> typesWithDateAttr = new List<int>();
    foreach (IMSAttribute4ObjectType attribute4ObjectType in attributes4ObjectTypeList)
      typesWithDateAttr.Add(attribute4ObjectType.ObjectTypeID);
    return typesWithDateAttr;
  }

  private void AddTypeToItemsAndListView(int typeId, string name)
  {
    IMSObjectType objectType = MetaDataHelper.GetObjectType(typeId);
    if (objectType == null || this.listView1.Items.IndexOfFirst((Predicate<object>) (x => (int) ((ListViewItem) x).Tag == typeId)) != -1)
      return;
    this._typesObjectTypesListItems.Add(new ObjectTypesListItem(typeId, objectType.Guid.ToString(), name));
    ListViewItem listViewItem = new ListViewItem(name);
    listViewItem.Tag = (object) typeId;
    if (Statics.IconSrv != null)
    {
      int num = Statics.IconSrv.IndexOf(4, typeId);
      listViewItem.ImageIndex = num;
    }
    this.listView1.Items.Add(listViewItem);
  }

  private void listView1_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this.listView1.SelectedItems.Count == 0)
      this.btnDelete.Enabled = false;
    else
      this.btnDelete.Enabled = true;
  }

  private void btnAdd_Click(object sender, EventArgs e)
  {
    SelectorForm selectorForm = new SelectorForm(typeof (ObjectTypesFolder), "Все типы объектов", typeof (ObjectTypeFolder), true);
    selectorForm.Text = "Выберите типы объектов";
    selectorForm.OnCheckActions = SelectorForm.CheckActions.None;
    selectorForm.OnUncheckActions = SelectorForm.CheckActions.None;
    selectorForm.SelectFocusedWhenNothingMultiselected = false;
    SelectorForm objTypeSelectorForm = selectorForm;
    this.SetSelectorFormFilters(objTypeSelectorForm);
    if (this._statisticsCommandType == CommandStatisticsTypesEnum.DateAttrValue && this._attrId == 0)
    {
      int num = (int) MessageBox.Show("Для настройки типов объектов укажите сначала атрибут для подсчета статистики.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    else
    {
      if (objTypeSelectorForm.ShowDialog() != DialogResult.OK)
        return;
      ArrayList idList = objTypeSelectorForm.IDList;
      if (idList == null || idList.Count < 1)
        return;
      for (int index = 0; index < idList.Count; ++index)
      {
        int int32 = Convert.ToInt32(idList[index]);
        string name = objTypeSelectorForm.NameList[index].ToString();
        if (!this._typesObjectTypesListItems.ContainsObjectType(int32))
          this.AddTypeToItemsAndListView(int32, name);
      }
      if (this.listView1.Items.Count == 0)
        this._typesObjectTypesListItems.Clear();
      AnalyzeObjectTypeControl.ModifyItems itemsChanged = this.ItemsChanged;
      if (itemsChanged == null)
        return;
      itemsChanged(true);
    }
  }

  private void btnDelete_Click(object sender, EventArgs e)
  {
    if (!ControlHelper.CanRemoveItems(this.listView1.SelectedItems.Count, "тип", "типы"))
      return;
    bool flag = false;
    for (int i = 0; i < this.listView1.SelectedItems.Count; i++)
    {
      this._typesObjectTypesListItems.RemoveAll((Predicate<ObjectTypesListItem>) (item => item.ObjectTypeID == Convert.ToInt32(this.listView1.SelectedItems[i].Tag)));
      this.listView1.Items.Remove(this.listView1.SelectedItems[i]);
      i--;
      flag = true;
    }
    if (flag)
    {
      AnalyzeObjectTypeControl.ModifyItems itemsChanged = this.ItemsChanged;
      if (itemsChanged != null)
        itemsChanged(true);
    }
    if (this.listView1.Items.Count != 0)
      return;
    this._typesObjectTypesListItems.Clear();
  }

  private void btnRemoveChildTypes_Click(object sender, EventArgs e)
  {
    bool flag = false;
    int count = this.listView1.Items.Count;
    if (count == 1)
      return;
    List<int> collection = new List<int>();
    for (int index1 = 0; index1 < count; ++index1)
    {
      for (int index2 = 1; index2 < count; ++index2)
      {
        if (index1 != index2)
        {
          int int32_1 = Convert.ToInt32(this.listView1.Items[index1].Tag);
          int int32_2 = Convert.ToInt32(this.listView1.Items[index2].Tag);
          if (MetaDataHelper.IsObjectTypeChildOf(int32_1, int32_2))
            collection.SafeAdd<int>(int32_1);
          if (MetaDataHelper.IsObjectTypeChildOf(int32_2, int32_1))
            collection.SafeAdd<int>(int32_2);
        }
      }
    }
    foreach (int num in collection)
    {
      int id = num;
      this._typesObjectTypesListItems.First<ObjectTypesListItem>((Func<ObjectTypesListItem, bool>) (item => item.ObjectTypeID == id));
      this._typesObjectTypesListItems.RemoveAll((Predicate<ObjectTypesListItem>) (item => item.ObjectTypeID == id));
      this.listView1.Items.RemoveAt(this.listView1.Items.IndexOfFirst((Predicate<object>) (x => (int) ((ListViewItem) x).Tag == id)));
      flag = true;
    }
    if (!flag)
      return;
    AnalyzeObjectTypeControl.ModifyItems itemsChanged = this.ItemsChanged;
    if (itemsChanged == null)
      return;
    itemsChanged(true);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.toolTip1 = new ToolTip();
    this.listView1 = new ListView();
    this.typeName = new ColumnHeader();
    this.toolBar1 = new Intermech.Bars.ToolBar();
    this.btnAdd = new ButtonItem();
    this.btnDelete = new ButtonItem();
    this.btnRemoveChildTypes = new ButtonItem();
    this.SuspendLayout();
    this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.listView1.Columns.AddRange(new ColumnHeader[1]
    {
      this.typeName
    });
    this.listView1.HideSelection = false;
    this.listView1.Location = new Point(0, 27);
    this.listView1.Name = "listView1";
    this.listView1.Size = new Size(495, 379);
    this.listView1.TabIndex = 8;
    this.listView1.UseCompatibleStateImageBehavior = false;
    this.listView1.View = View.Details;
    this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
    this.typeName.Text = "Наименование";
    this.typeName.Width = 252;
    this.toolBar1.FullMenus = true;
    this.toolBar1.Guid = new Guid("dfde644e-a287-4cd4-9395-6ac08018a98f");
    this.toolBar1.Hidden = false;
    this.toolBar1.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.btnAdd,
      (ToolbarItemBase) this.btnDelete,
      (ToolbarItemBase) this.btnRemoveChildTypes
    });
    this.toolBar1.Location = new Point(0, 0);
    this.toolBar1.Name = "toolBar1";
    this.toolBar1.Size = new Size(495, 24);
    this.toolBar1.TabIndex = 9;
    this.toolBar1.Text = "toolBar1";
    this.btnAdd.CommandName = "btnAdd";
    this.btnAdd.Image = (Image) Resources.add;
    this.btnAdd.ToolTipText = "Добавить тип";
    this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
    this.btnDelete.CommandName = "btnDelete";
    this.btnDelete.Image = (Image) Resources.minus;
    this.btnDelete.Text = "Удалить тип";
    this.btnDelete.ToolTipText = "Удалить тип";
    this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
    this.btnRemoveChildTypes.CommandName = "btnRemoveChildTypes";
    this.btnRemoveChildTypes.Image = (Image) Resources.RemoveChildTypes;
    this.btnRemoveChildTypes.ToolTipText = "Удалить дочерние типы";
    this.btnRemoveChildTypes.Click += new EventHandler(this.btnRemoveChildTypes_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.AutoScroll = true;
    this.Controls.Add((Control) this.toolBar1);
    this.Controls.Add((Control) this.listView1);
    this.Name = nameof (AnalyzeObjectTypeControl);
    this.Size = new Size(495, 438);
    this.ResumeLayout(false);
  }

  public delegate void ModifyItems(bool message);
}
