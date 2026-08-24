// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Controls.FiltersControl
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Bars;
using Intermech.Collections;
using Intermech.DataFormats;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.DBObjectTypes;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Statistics.Interfaces;
using Intermech.Statistics.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Statistics.Controls;

public class FiltersControl : UserControl
{
  private List<long> _currentSelections = new List<long>();
  private Dictionary<long, List<long>> _currentSearchSchemes = new Dictionary<long, List<long>>();
  private CommandStatisticsTypesEnum _statisticsCommandType;
  private IContainer components;
  private SplitContainer splitContainer1;
  private ChildrenView childrenViewFilters;
  private Label label3;
  private ChildrenView childrenViewRootObjects;
  private Intermech.Bars.ToolBar toolBar1;
  private ButtonItem btnAddFilter;
  private ButtonItem btnDeleteFilter;
  private Intermech.Bars.ToolBar toolBar2;
  private ButtonItem btnDeleteRootObject;
  private Panel panel1;
  private ButtonItem btnAddRootObject;
  private ToolTip toolTip1;
  private ButtonItem btnCreateSelection;
  private ButtonItem btnRefresh;

  public Filters Filters
  {
    get
    {
      return new Filters(this._currentSelections, new XmlSerializableDictionary<long, List<long>>((IDictionary<long, List<long>>) this._currentSearchSchemes));
    }
  }

  private void Modify()
  {
    EventHandler onModified = this.OnModified;
    if (onModified == null)
      return;
    onModified((object) this, EventArgs.Empty);
  }

  public event EventHandler OnModified;

  public FiltersControl() => this.InitializeComponent();

  public void Init(CommandStatisticsTypesEnum statisticsCommandType)
  {
    ServiceUtils.GetService<INotificationService>((object) ServicesManager.ServiceContainer, true)?.Subscribe("ObjectsChanged", new NotificationEventHandler(this.OnObjectChanged));
    this._statisticsCommandType = statisticsCommandType;
    this.UpdateControl();
  }

  public void Init(Filters commandSettingsFilters, CommandStatisticsTypesEnum statisticsCommandType)
  {
    ServiceUtils.GetService<INotificationService>((object) ServicesManager.ServiceContainer, true)?.Subscribe("ObjectsChanged", new NotificationEventHandler(this.OnObjectChanged));
    this._statisticsCommandType = statisticsCommandType;
    this._currentSelections = new List<long>((IEnumerable<long>) commandSettingsFilters.Selections);
    this._currentSearchSchemes = new Dictionary<long, List<long>>((IDictionary<long, List<long>>) commandSettingsFilters.SearchSchemes);
    this.UpdateControl();
  }

  private void UpdateControl() => this.InitFiltersChildrenView();

  private void InitFiltersChildrenView()
  {
    List<long> objectIds = new List<long>((IEnumerable<long>) this._currentSelections);
    objectIds.AddRange((IEnumerable<long>) this._currentSearchSchemes.Keys.ToList<long>());
    Dictionary<int, List<long>> typedDictionary = FiltersControl.CreateTypedDictionary(objectIds);
    DictDescriptor rootDescriptor = new DictDescriptor(Intermech.Navigator.Consts.CategoryAllObjectTypes, 0, string.Empty, typedDictionary);
    this.childrenViewFilters.Deactivate((IView) null);
    this.childrenViewFilters.Initialize((IDescriptor) rootDescriptor, (IServiceProvider) this.childrenViewFilters.Services);
    this.childrenViewFilters.Activate((IView) null);
  }

  private void AddNewFiltersToData(object[] newFilterObjects)
  {
    foreach (object newFilterObject in newFilterObjects)
    {
      if (newFilterObject is IDBTypedObjectID dbTypedObjectId)
      {
        if (StatisticsConst.AllSelectionsTypes.Contains(dbTypedObjectId.ObjectType))
          this._currentSelections.SafeAdd<long>(dbTypedObjectId.ObjectID);
        if (StatisticsConst.AllSchemeTypes.Contains(dbTypedObjectId.ObjectType) && !this._currentSearchSchemes.Keys.Contains<long>((long) dbTypedObjectId.ObjectType))
          this._currentSearchSchemes.Add(dbTypedObjectId.ObjectID, new List<long>());
      }
    }
  }

  private object[] GetNewFilterObjects()
  {
    DescriptorCollection descriptors;
    switch (this._statisticsCommandType)
    {
      case CommandStatisticsTypesEnum.CreatedDate:
      case CommandStatisticsTypesEnum.DateAttrValue:
        descriptors = new DescriptorCollection()
        {
          (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cad00156-306c-11d8-b4e9-00304f19f545")),
          (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cad00129-306c-11d8-b4e9-00304f19f545"))
        };
        break;
      default:
        descriptors = new DescriptorCollection()
        {
          (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cad00156-306c-11d8-b4e9-00304f19f545"))
        };
        break;
    }
    return Intermech.Navigator.SelectionWindow.Select("Выберите фильтрующие объекты", "", (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor("Фильтрующие объекты", descriptors), typeof (IDBTypedObjectID), SelectionOptions.SelectObjects);
  }

  private void RemoveSelectedFiltersFromData()
  {
    for (int index = 0; index < this.childrenViewFilters.SelectedItems.Count; ++index)
    {
      if (this.childrenViewFilters.SelectedItems.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData)
      {
        if (StatisticsConst.AllSelectionsTypes.Contains(itemData.ObjectType))
          this._currentSelections.Remove(itemData.ObjectID);
        if (StatisticsConst.AllSchemeTypes.Contains(itemData.ObjectType) && !this._currentSearchSchemes.Keys.Contains<long>((long) itemData.ObjectType))
          this._currentSearchSchemes.Remove(itemData.ObjectID);
      }
    }
  }

  private void childrenView1_SelectedItemsChanged(object sender, EventArgs e)
  {
    if (this.childrenViewFilters.SelectedItems.Count == 0)
    {
      this.btnDeleteFilter.Enabled = false;
      this.splitContainer1.Panel2Collapsed = true;
    }
    else if (this.childrenViewFilters.SelectedItems.Count != 1)
    {
      this.splitContainer1.Panel2Collapsed = true;
    }
    else
    {
      this.btnDeleteFilter.Enabled = true;
      if (!(this.childrenViewFilters.SelectedItems.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
        return;
      if (StatisticsConst.AllSchemeTypes.Contains(itemData.ObjectType))
      {
        this.splitContainer1.Panel2Collapsed = false;
        List<long> objectIds;
        this._currentSearchSchemes.TryGetValue(itemData.ObjectID, out objectIds);
        if (objectIds != null && objectIds.Count > 0)
        {
          Dictionary<int, List<long>> typedDictionary = FiltersControl.CreateTypedDictionary(objectIds);
          DictDescriptor rootDescriptor = new DictDescriptor(Intermech.Navigator.Consts.CategoryAllObjectTypes, 0, string.Empty, typedDictionary);
          this.childrenViewRootObjects.Deactivate((IView) null);
          this.childrenViewRootObjects.Initialize((IDescriptor) rootDescriptor, (IServiceProvider) this.childrenViewRootObjects.Services);
          this.childrenViewRootObjects.Activate((IView) null);
          this.btnDeleteRootObject.Enabled = true;
        }
        else
        {
          DictDescriptor rootDescriptor = new DictDescriptor(Intermech.Navigator.Consts.CategoryAllObjectTypes, 0, string.Empty, new Dictionary<int, List<long>>());
          this.childrenViewRootObjects.Deactivate((IView) null);
          this.childrenViewRootObjects.Initialize((IDescriptor) rootDescriptor, (IServiceProvider) this.childrenViewRootObjects.Services);
          this.childrenViewRootObjects.Activate((IView) null);
          this.btnDeleteRootObject.Enabled = false;
        }
      }
      else
        this.splitContainer1.Panel2Collapsed = true;
    }
  }

  private static Dictionary<int, List<long>> CreateTypedDictionary(List<long> objectIds)
  {
    Dictionary<int, List<long>> typedDictionary = new Dictionary<int, List<long>>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (long objectId in objectIds)
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(objectId);
        if (!objectInfo.Empty)
        {
          List<long> longList;
          if (typedDictionary.TryGetValue(objectInfo.ObjectTypeID, out longList))
            longList.Add(objectId);
          else
            typedDictionary.Add(objectInfo.ObjectTypeID, new List<long>()
            {
              objectId
            });
        }
      }
    }
    return typedDictionary;
  }

  private void childrenViewRootObjects_SelectedItemsChanged(object sender, EventArgs e)
  {
    if (this.childrenViewRootObjects.SelectedItems.Count != 1)
      return;
    this.btnDeleteRootObject.Enabled = true;
  }

  private void btnAddFilter_Click_1(object sender, EventArgs e)
  {
    object[] newFilterObjects = this.GetNewFilterObjects();
    if (newFilterObjects == null || newFilterObjects.Length < 1)
      return;
    this.AddNewFiltersToData(newFilterObjects);
    this.UpdateControl();
    this.Modify();
  }

  private void btnDeleteFilter_Click(object sender, EventArgs e)
  {
    if (MessageBox.Show("Удалить из списка выделенные объекты?", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK || this.childrenViewFilters.SelectedItems.Count <= 0)
      return;
    this.RemoveSelectedFiltersFromData();
    this.UpdateControl();
    this.childrenView1_SelectedItemsChanged((object) null, EventArgs.Empty);
    this.Modify();
  }

  private void btnAddRootObject_Click_1(object sender, EventArgs e)
  {
    if (!(this.childrenViewFilters.SelectedItems.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    object[] objArray = Intermech.Navigator.SelectionWindow.Select("Выберите корневые объекты", "", (IDescriptor) new AllObjectTypesDescriptor(), typeof (IDBTypedObjectID), SelectionOptions.SelectObjects);
    if (objArray == null || objArray.Length == 0)
      return;
    foreach (object obj in objArray)
    {
      if (!(obj is IDBTypedObjectID dbTypedObjectId))
        return;
      List<long> collection;
      if (this._currentSearchSchemes.TryGetValue(itemData.ObjectID, out collection))
        collection.SafeAdd<long>(dbTypedObjectId.ObjectID);
    }
    this.childrenView1_SelectedItemsChanged((object) null, EventArgs.Empty);
    this.Modify();
  }

  private void btnDeleteRootObject_Click(object sender, EventArgs e)
  {
    if (MessageBox.Show("Удалить из списка выделенные объекты?", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK || !(this.childrenViewFilters.SelectedItems.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData1))
      return;
    for (int index = 0; index < this.childrenViewRootObjects.SelectedItems.Count; ++index)
    {
      if (!(this.childrenViewRootObjects.SelectedItems.GetItemData(index, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData2))
        return;
      List<long> longList;
      if (this._currentSearchSchemes.TryGetValue(itemData1.ObjectID, out longList))
        longList.Remove(itemData2.ObjectID);
    }
    this.childrenView1_SelectedItemsChanged((object) null, EventArgs.Empty);
    this.Modify();
  }

  private void btnCreateSelection_Click(object sender, EventArgs e)
  {
    List<int> childrenIdRecursive = MetaDataHelper.GetObjectTypeChildrenIDRecursive(MetaDataHelper.GetObjectTypeID("cad00156-306c-11d8-b4e9-00304f19f545"));
    if (this._statisticsCommandType != CommandStatisticsTypesEnum.CreatedDate)
    {
      int statisticsCommandType = (int) this._statisticsCommandType;
    }
    childrenIdRecursive.AddRange((IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(MetaDataHelper.GetObjectTypeID(new Guid("cad00129-306c-11d8-b4e9-00304f19f545"))));
    IObjectCreatorService service = ServicesManager.GetService<IObjectCreatorService>();
    service.AfterObjectCreatedEvent += new AfterObjectCreatedEventHandler(this.OnCreateNewObject);
    try
    {
      int objectTypeID;
      long objectByTypeDialog = service.CreateObjectByTypeDialog(childrenIdRecursive.ToArray(), out objectTypeID);
      if (objectByTypeDialog == -1L)
        return;
      DBObjectsEventArgs e1 = new DBObjectsEventArgs("ObjectsCreated", objectByTypeDialog, objectTypeID, true);
      ApplicationServices.Container.GetService<INotificationService>().FireEvent((object) this, (NotificationEventArgs) e1);
    }
    finally
    {
      service.AfterObjectCreatedEvent -= new AfterObjectCreatedEventHandler(this.OnCreateNewObject);
    }
  }

  private void OnCreateNewObject(object sender, AfterObjectCreatedEventArgs e)
  {
    if (StatisticsConst.AllSelectionsTypes.Contains(e.ObjectTypeID))
      this._currentSelections.Add(e.ObjectID);
    else if (StatisticsConst.AllSchemeTypes.Contains(e.ObjectTypeID))
      this._currentSearchSchemes.Add(e.ObjectID, new List<long>());
    this.UpdateControl();
    this.Modify();
    this.SetSelectedObjectVersionIds(new long[1]
    {
      e.ObjectID
    });
  }

  private void SetSelectedObjectVersionIds(long[] objectVersionIds)
  {
    this.childrenViewFilters.SelectNodes(((IEnumerable<long>) objectVersionIds).Select<long, INodeID>((Func<long, INodeID>) (o => this.FindNodeIDForObjectVersionID(o))).Where<INodeID>((Func<INodeID, bool>) (o => o != null)).ToList<INodeID>());
  }

  private INodeID FindNodeIDForObjectVersionID(long objectVersionID)
  {
    return (INodeID) this.GetAllObjectNodeIds().FirstOrDefault<NodeID>((Func<NodeID, bool>) (o => o.ObjectID == objectVersionID));
  }

  private IEnumerable<NodeID> GetAllObjectNodeIds()
  {
    foreach (iGRow row in (IEnumerable) this.childrenViewFilters.Grid.Rows)
    {
      INodeID nodeIdForRow = this.childrenViewFilters.GetNodeIDForRow(row);
      if (nodeIdForRow is NodeID)
        yield return (NodeID) nodeIdForRow;
    }
  }

  private void btnRefresh_Click(object sender, EventArgs e) => this.UpdateControl();

  private void OnObjectChanged(object sender, NotificationEventArgs ne)
  {
    if (!(ne is DBObjectsEventArgs objectsEventArgs) || objectsEventArgs.EventName != "ObjectsChanged" || objectsEventArgs.ObjectIDs.Intersect<long>((IEnumerable<long>) this.Filters.Selections).ToList<long>().Count <= 0 && objectsEventArgs.ObjectIDs.Intersect<long>((IEnumerable<long>) this.Filters.SearchSchemes.Keys).ToList<long>().Count <= 0)
      return;
    this.UpdateControl();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FiltersControl));
    this.splitContainer1 = new SplitContainer();
    this.toolBar1 = new Intermech.Bars.ToolBar();
    this.btnAddFilter = new ButtonItem();
    this.btnDeleteFilter = new ButtonItem();
    this.btnCreateSelection = new ButtonItem();
    this.btnRefresh = new ButtonItem();
    this.panel1 = new Panel();
    this.toolBar2 = new Intermech.Bars.ToolBar();
    this.btnAddRootObject = new ButtonItem();
    this.btnDeleteRootObject = new ButtonItem();
    this.label3 = new Label();
    this.toolTip1 = new ToolTip();
    this.childrenViewFilters = new ChildrenView();
    this.childrenViewRootObjects = new ChildrenView();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this.splitContainer1.BackColor = SystemColors.ControlLight;
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(0, 0);
    this.splitContainer1.Margin = new Padding(2);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Orientation = Orientation.Horizontal;
    this.splitContainer1.Panel1.BackColor = SystemColors.Control;
    this.splitContainer1.Panel1.Controls.Add((Control) this.toolBar1);
    this.splitContainer1.Panel1.Controls.Add((Control) this.childrenViewFilters);
    this.splitContainer1.Panel2.AutoScroll = true;
    this.splitContainer1.Panel2.BackColor = SystemColors.Control;
    this.splitContainer1.Panel2.Controls.Add((Control) this.panel1);
    this.splitContainer1.Panel2.Controls.Add((Control) this.label3);
    this.splitContainer1.Panel2MinSize = 250;
    this.splitContainer1.Size = new Size(681, 519);
    this.splitContainer1.SplitterDistance = 230;
    this.splitContainer1.SplitterWidth = 3;
    this.splitContainer1.TabIndex = 0;
    this.toolBar1.FullMenus = true;
    this.toolBar1.Guid = new Guid("cbfea772-9072-4b58-813c-6de857da40d2");
    this.toolBar1.Hidden = false;
    this.toolBar1.Items.AddRange(new ToolbarItemBase[4]
    {
      (ToolbarItemBase) this.btnAddFilter,
      (ToolbarItemBase) this.btnDeleteFilter,
      (ToolbarItemBase) this.btnCreateSelection,
      (ToolbarItemBase) this.btnRefresh
    });
    this.toolBar1.Location = new Point(0, 0);
    this.toolBar1.Name = "toolBar1";
    this.toolBar1.Size = new Size(681, 24);
    this.toolBar1.TabIndex = 9;
    this.toolBar1.Text = "toolBar1";
    this.btnAddFilter.CommandName = "buttonItem1";
    this.btnAddFilter.Image = (Image) Resources.add;
    this.btnAddFilter.ToolTipText = "Добавить фильтрующие объекты";
    this.btnAddFilter.Click += new EventHandler(this.btnAddFilter_Click_1);
    this.btnDeleteFilter.CommandName = "btnDeleteFilter";
    this.btnDeleteFilter.Image = (Image) Resources.minus;
    this.btnDeleteFilter.ToolTipText = "Удалить фильтрующие объекты";
    this.btnDeleteFilter.Click += new EventHandler(this.btnDeleteFilter_Click);
    this.btnCreateSelection.CommandName = "btnCreateSelection";
    this.btnCreateSelection.Icon = (Icon) componentResourceManager.GetObject("btnCreateSelection.Icon");
    this.btnCreateSelection.Image = (Image) Resources.NewFilter;
    this.btnCreateSelection.ToolTipText = "Создать  фильтрующий объект";
    this.btnCreateSelection.Click += new EventHandler(this.btnCreateSelection_Click);
    this.btnRefresh.CommandName = "buttonItem1";
    this.btnRefresh.Image = (Image) Resources.refresh;
    this.btnRefresh.ToolTipText = "Обновить";
    this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
    this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.panel1.AutoScroll = true;
    this.panel1.Controls.Add((Control) this.toolBar2);
    this.panel1.Controls.Add((Control) this.childrenViewRootObjects);
    this.panel1.Location = new Point(3, 26);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(675, 377);
    this.panel1.TabIndex = 14;
    this.toolBar2.FullMenus = true;
    this.toolBar2.Guid = new Guid("cbfea772-9072-4b58-813c-6de857da40d2");
    this.toolBar2.Hidden = false;
    this.toolBar2.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnAddRootObject,
      (ToolbarItemBase) this.btnDeleteRootObject
    });
    this.toolBar2.Location = new Point(0, 0);
    this.toolBar2.Name = "toolBar2";
    this.toolBar2.Size = new Size(675, 24);
    this.toolBar2.TabIndex = 15;
    this.toolBar2.Text = "toolBar2";
    this.btnAddRootObject.CommandName = "buttonItem1";
    this.btnAddRootObject.Image = (Image) Resources.add;
    this.btnAddRootObject.ToolTipText = "Добавить корневые объекты";
    this.btnAddRootObject.Click += new EventHandler(this.btnAddRootObject_Click_1);
    this.btnDeleteRootObject.CommandName = "btnDelete";
    this.btnDeleteRootObject.Image = (Image) Resources.minus;
    this.btnDeleteRootObject.ToolTipText = "Удалить корневые объекты";
    this.btnDeleteRootObject.Click += new EventHandler(this.btnDeleteRootObject_Click);
    this.label3.AutoSize = true;
    this.label3.Location = new Point(2, 10);
    this.label3.Margin = new Padding(2, 0, 2, 0);
    this.label3.Name = "label3";
    this.label3.Size = new Size(204, 13);
    this.label3.TabIndex = 13;
    this.label3.Text = "Корневые  объекты для схемы поиска";
    this.childrenViewFilters.AllowCustomGroupValues = true;
    this.childrenViewFilters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.childrenViewFilters.AutoScroll = true;
    this.childrenViewFilters.Control = (object) this.childrenViewFilters;
    this.childrenViewFilters.DisableColumnsGrouping = true;
    this.childrenViewFilters.DisableContextSearch = true;
    this.childrenViewFilters.DisableFiltration = true;
    this.childrenViewFilters.DisableGroupBox = true;
    this.childrenViewFilters.DisableIMContextMenu = true;
    this.childrenViewFilters.DisableKeyDownEvents = false;
    this.childrenViewFilters.DisableManualSortingSetup = true;
    this.childrenViewFilters.DisableStatusBar = true;
    this.childrenViewFilters.DisableToolBar = true;
    this.childrenViewFilters.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.childrenViewFilters.Font = new Font("Tahoma", 8.25f);
    this.childrenViewFilters.Location = new Point(2, 24);
    this.childrenViewFilters.Margin = new Padding(2);
    this.childrenViewFilters.Name = "childrenViewFilters";
    this.childrenViewFilters.Size = new Size(677, 193);
    this.childrenViewFilters.TabIndex = 0;
    this.childrenViewFilters.ViewContentType = ContentType.NonFolders;
    this.childrenViewFilters.SelectedItemsChanged += new EventHandler(this.childrenView1_SelectedItemsChanged);
    this.childrenViewRootObjects.AllowCustomGroupValues = true;
    this.childrenViewRootObjects.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.childrenViewRootObjects.AutoScroll = true;
    this.childrenViewRootObjects.Control = (object) this.childrenViewRootObjects;
    this.childrenViewRootObjects.DisableColumnsGrouping = true;
    this.childrenViewRootObjects.DisableContextSearch = true;
    this.childrenViewRootObjects.DisableFiltration = true;
    this.childrenViewRootObjects.DisableGroupBox = true;
    this.childrenViewRootObjects.DisableIMContextMenu = true;
    this.childrenViewRootObjects.DisableKeyDownEvents = false;
    this.childrenViewRootObjects.DisableManualSortingSetup = true;
    this.childrenViewRootObjects.DisableStatusBar = true;
    this.childrenViewRootObjects.DisableToolBar = true;
    this.childrenViewRootObjects.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.childrenViewRootObjects.Font = new Font("Tahoma", 8.25f);
    this.childrenViewRootObjects.Location = new Point(2, 28);
    this.childrenViewRootObjects.Margin = new Padding(2);
    this.childrenViewRootObjects.Name = "childrenViewRootObjects";
    this.childrenViewRootObjects.Size = new Size(671, 347);
    this.childrenViewRootObjects.TabIndex = 14;
    this.childrenViewRootObjects.ViewContentType = ContentType.NonFolders;
    this.childrenViewRootObjects.SelectedItemsChanged += new EventHandler(this.childrenViewRootObjects_SelectedItemsChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.AutoScroll = true;
    this.Controls.Add((Control) this.splitContainer1);
    this.Margin = new Padding(2);
    this.Name = nameof (FiltersControl);
    this.Size = new Size(681, 519);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.Panel2.PerformLayout();
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
