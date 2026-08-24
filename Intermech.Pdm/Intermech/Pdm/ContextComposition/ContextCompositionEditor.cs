// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextComposition.ContextCompositionEditor
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.Bars;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Compositions.CompositionService;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator.ContextCommands;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Descriptos;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using Intermech.PropertyEditors;
using Intermech.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

#nullable disable
namespace Intermech.Pdm.ContextComposition;

public class ContextCompositionEditor : FIltratedDocControl, IGuid
{
  public static Guid ContextCompositionControlGuid = new Guid("B33C2DC7-DE69-47C3-A69F-D659C0C6830C");
  private RelationPropertiesView _relationPropertiesView = new RelationPropertiesView();
  private NavigatorTreeViewWithObjectTypeFiltration _prototypeCompositionTreeView = new NavigatorTreeViewWithObjectTypeFiltration();
  private long _prototypeObject;
  private long _contextCompositionObject;
  private INavigatorColumnsService _navigatorColumnsService;
  private INotificationService _notificationService;
  private string _treeNameContextPrototype = "TreeViewContextPrototypeTCE";
  private string _treeNameContextComposition = "TreeViewContextCompositionTCE";
  private long _selectedContext = -1;
  private long _relationID = -1;
  private string _contexName = "Общий контекст";
  private bool _activated;
  private bool _isInitializeComponent;
  private IContainer components;
  private SplitContainer mainSplitContainer;
  private Intermech.Bars.ToolBar additionalMenu;
  private ButtonItem addObjectToTSE;
  private ButtonItem quantityAnalize;
  private LabelItem contextName;
  private ImageList menuImageList;
  private NavigatorTreeView compositionTreeView;
  private Intermech.Bars.ToolBar addDelObjectTCE;
  private ImageList imagesToolbars;
  private ButtonItem btnAdd;
  private ButtonItem btnDelete;
  private LabelItem emptyLabel;
  private RadSplitContainer objectSplitContainer;
  private SplitPanel splitPanel1;
  private SplitPanel splitPanel2;
  private FluentTheme fluentTheme1;

  public ContextCompositionEditor(
    long prototypeObject,
    long contextCompositionObject,
    long selectedContext,
    string contextNameString,
    long relationID)
  {
    this.Guid = ContextCompositionEditor.ContextCompositionControlGuid;
    this._notificationService = ApplicationServices.Container.GetService(typeof (INotificationService)) as INotificationService;
    this.InitializeComponent();
    this._prototypeObject = prototypeObject;
    this._contextCompositionObject = contextCompositionObject;
    this._selectedContext = selectedContext;
    this._relationID = relationID;
    this.Initialize();
    this.contextName.Text = contextNameString;
    this.contextName.ToolTipText = contextNameString;
    this._contexName = contextNameString;
  }

  public ContextCompositionEditor()
  {
    this.InitializeComponent();
    this._isInitializeComponent = true;
  }

  public void Init(
    long prototypeObject,
    long contextCompositionObject,
    long selectedContext,
    string contextNameString,
    long relationID)
  {
    if (!this._isInitializeComponent)
      return;
    this.Guid = ContextCompositionEditor.ContextCompositionControlGuid;
    this._notificationService = ApplicationServices.Container.GetService(typeof (INotificationService)) as INotificationService;
    this._prototypeObject = prototypeObject;
    this._contextCompositionObject = contextCompositionObject;
    this._selectedContext = selectedContext;
    this._relationID = relationID;
    this.Initialize();
    this.contextName.Text = contextNameString;
    this.contextName.ToolTipText = contextNameString;
    this._contexName = contextNameString;
  }

  private void Initialize()
  {
    if (this._prototypeObject == -1L && this._contextCompositionObject == -1L && this._selectedContext == -1L && this._relationID == -1L)
      return;
    AdvancedServiceContainer ownerServices = new AdvancedServiceContainer();
    ownerServices.AddService(typeof (INotificationService), (object) this._notificationService);
    this.compositionTreeView.Services = (IServiceProvider) ownerServices;
    this._relationPropertiesView.Dock = DockStyle.Fill;
    this._prototypeCompositionTreeView.Dock = DockStyle.Fill;
    this.compositionTreeView.Dock = DockStyle.Fill;
    this.splitPanel2.Controls.Add((Control) this._relationPropertiesView);
    this.mainSplitContainer.Panel1.Controls.Add((Control) this._prototypeCompositionTreeView);
    this._prototypeCompositionTreeView.BringToFront();
    this._prototypeCompositionTreeView.InitializeServices((IServiceProvider) ownerServices);
    this._prototypeCompositionTreeView.TreeView.MultiSelect = true;
    this.compositionTreeView.MultiSelect = true;
  }

  private void FiltrationService_OnFiltrationChanged(
    IFiltrationSettings newFiltration,
    bool filtrationValid)
  {
    this._prototypeCompositionTreeView.FullWindowRefreshImplementation();
  }

  public override void Activated()
  {
    base.Activated();
    if (this._activated)
      return;
    QueryEvents.BeforeClientRecordsSelectEvent += new BeforeClientRecordsSelectHandler(this.QueryEvents_BeforeClientRecordsSelectEvent);
    this.compositionTreeView.AfterFocusNode += new EventHandler<NavigatorTreeNodeEventArgs>(this.CompositionTreeView_AfterFocusNode);
    this._notificationService.Subscribe("ObjectsCheckedIn", new NotificationEventHandler(this.CheckedInEvents));
    this._notificationService.Subscribe("ObjectsCheckedOut", new NotificationEventHandler(this.CheckedOutEvents));
    this.FiltrationService.OnFiltrationChanged += new Intermech.Interfaces.Client.FiltrationChanged(this.FiltrationService_OnFiltrationChanged);
    try
    {
      Intermech.Navigator.DBObjects.Descriptor rootDescriptor1 = new Intermech.Navigator.DBObjects.Descriptor(this._prototypeObject);
      if (rootDescriptor1.InvalidDescriptor)
      {
        rootDescriptor1 = new Intermech.Navigator.DBObjects.Descriptor(-this._prototypeObject);
        if (!rootDescriptor1.InvalidDescriptor)
          this._prototypeObject = -this._prototypeObject;
      }
      this._prototypeCompositionTreeView.TreeView.SupportedColumns = Intermech.Navigator.Utils.GetNavigatorColumns();
      this._prototypeCompositionTreeView.TreeView.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.GetNavigatorColumns);
      this._prototypeCompositionTreeView.TreeView.SetColumns(Intermech.Navigator.Utils.CaptionColumnOnly(NodeColumnSortOrder.Ascending));
      this._prototypeCompositionTreeView.TreeView.Build((IDescriptor) rootDescriptor1);
      this._navigatorColumnsService = ApplicationServices.Container.GetService(typeof (INavigatorColumnsService)) as INavigatorColumnsService;
      INodeID rootNodeId1 = this._prototypeCompositionTreeView.TreeView.RootNodeID;
      if (rootNodeId1 != null)
      {
        NavigatorColumns navigatorColumns = this._navigatorColumnsService?.GetNavigatorColumns(rootNodeId1.CategoryID, rootNodeId1.TypeID, this._treeNameContextPrototype, false);
        if (navigatorColumns != null)
          this._prototypeCompositionTreeView.TreeView.SetColumns(navigatorColumns.Columns);
        else if (this._prototypeCompositionTreeView.TreeView.RootNode?.Handler != null)
          this._prototypeCompositionTreeView.TreeView.SetColumns(Intermech.Navigator.Utils.DefaultColumnsObjects());
      }
      ContextCompositionDescriptorDisableFiltration rootDescriptor2 = new ContextCompositionDescriptorDisableFiltration(this._contextCompositionObject);
      if (rootDescriptor2.InvalidDescriptor)
      {
        rootDescriptor2 = new ContextCompositionDescriptorDisableFiltration(-this._contextCompositionObject);
        if (!rootDescriptor2.InvalidDescriptor)
          this._contextCompositionObject = -this._contextCompositionObject;
      }
      this.compositionTreeView.OnGetSupportedColumnsEventHandler += new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.GetNavigatorColumns);
      this.compositionTreeView.SetColumns(Intermech.Navigator.Utils.CaptionColumnOnly(NodeColumnSortOrder.Ascending));
      this.compositionTreeView.Build((IDescriptor) rootDescriptor2);
      INodeID rootNodeId2 = this.compositionTreeView.RootNodeID;
      if (rootNodeId2 == null)
        return;
      NavigatorColumns navigatorColumns1 = this._navigatorColumnsService?.GetNavigatorColumns(rootNodeId2.CategoryID, rootNodeId2.TypeID, this._treeNameContextComposition, false);
      if (navigatorColumns1 != null)
      {
        this.compositionTreeView.SetColumns(navigatorColumns1.Columns);
      }
      else
      {
        if (this.compositionTreeView.RootNode?.Handler == null)
          return;
        this.compositionTreeView.SetColumns(Intermech.Navigator.Utils.DefaultColumnsObjects());
      }
    }
    finally
    {
      this._activated = true;
    }
  }

  public override void Deactivated()
  {
    base.Deactivated();
    if (!this._activated)
      return;
    try
    {
      try
      {
        INodeID rootNodeId1 = this._prototypeCompositionTreeView.TreeView.RootNodeID;
        if (rootNodeId1 != null)
        {
          NavigatorColumns columns = this._navigatorColumnsService.GetNavigatorColumns(rootNodeId1.CategoryID, rootNodeId1.TypeID, this._treeNameContextPrototype, false) ?? this._navigatorColumnsService.CreateNavigatorColumns(rootNodeId1.CategoryID, rootNodeId1.TypeID, this._treeNameContextPrototype);
          columns.Columns = this._prototypeCompositionTreeView.TreeView.ReflectTreeColumsChanges();
          this._navigatorColumnsService.CreateNavigatorColumns(columns);
        }
        INodeID rootNodeId2 = this.compositionTreeView.RootNodeID;
        if (rootNodeId2 != null)
        {
          NavigatorColumns columns = this._navigatorColumnsService.GetNavigatorColumns(rootNodeId2.CategoryID, rootNodeId2.TypeID, this._treeNameContextComposition, false) ?? this._navigatorColumnsService.CreateNavigatorColumns(rootNodeId2.CategoryID, rootNodeId2.TypeID, this._treeNameContextComposition);
          columns.Columns = this.compositionTreeView.ReflectTreeColumsChanges();
          this._navigatorColumnsService.CreateNavigatorColumns(columns);
        }
      }
      catch
      {
      }
      QueryEvents.BeforeClientRecordsSelectEvent -= new BeforeClientRecordsSelectHandler(this.QueryEvents_BeforeClientRecordsSelectEvent);
      this.compositionTreeView.OnGetSupportedColumnsEventHandler -= new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.GetObjectsColumns);
      this._prototypeCompositionTreeView.TreeView.OnGetSupportedColumnsEventHandler -= new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.GetObjectsColumns);
      this.compositionTreeView.AfterFocusNode -= new EventHandler<NavigatorTreeNodeEventArgs>(this.CompositionTreeView_AfterFocusNode);
      this._notificationService.Unsubscribe("ObjectsCheckedIn", new NotificationEventHandler(this.CheckedInEvents));
      this._notificationService.Unsubscribe("ObjectsCheckedOut", new NotificationEventHandler(this.CheckedOutEvents));
      this.FiltrationService.OnFiltrationChanged -= new Intermech.Interfaces.Client.FiltrationChanged(this.FiltrationService_OnFiltrationChanged);
    }
    finally
    {
      this._activated = false;
    }
  }

  public Guid GUID => ContextCompositionEditor.ContextCompositionControlGuid;

  private void QueryEvents_BeforeClientRecordsSelectEvent(
    object sender,
    BeforeClientRecordsSelectEventArgs args)
  {
    if (!(sender is ObjectContextCompositionNodeQuery))
      return;
    args.NewParameters = new DBRecordSetParams?(args.OldParameters);
    args.NewParameters.Value.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) new List<long>()
    {
      this._selectedContext
    };
  }

  private void CheckedInEvents(object sender, NotificationEventArgs e)
  {
    if (e.ItemsCount <= 0 || !(e is DBObjectsEventArgs objectsEventArgs) || !objectsEventArgs.ObjectIDs.Contains(this._contextCompositionObject))
      return;
    this._contextCompositionObject = -this._contextCompositionObject;
  }

  private void CheckedOutEvents(object sender, NotificationEventArgs e)
  {
    if (e.ItemsCount <= 0 || !(e is DBObjectsCheckOutEventArgs checkOutEventArgs) || !checkOutEventArgs.ObjectIDs.Contains(this._contextCompositionObject))
      return;
    this._contextCompositionObject = -this._contextCompositionObject;
  }

  private void CompositionTreeView_AfterFocusNode(object sender, NavigatorTreeNodeEventArgs e)
  {
    if (e.Node == null)
      return;
    if (object.Equals((object) e.Node, (object) this.compositionTreeView.RootNode))
    {
      this._relationPropertiesView.Initialize(SelectedItemsHelper.CreateSelectedItemsForCompositionPart(this._relationID, this._contextCompositionObject), this.compositionTreeView.Services);
      this._relationPropertiesView.Activate((Intermech.Navigator.Views.IView) null);
    }
    else
    {
      this._relationPropertiesView.Initialize(e.Node.NodeAsSelectedItem, this.compositionTreeView.Services);
      this._relationPropertiesView.Activate((Intermech.Navigator.Views.IView) null);
    }
  }

  private void addObjectToTSE_Click(object sender, EventArgs e)
  {
    List<int> childObjectTypesId = MetaDataHelper.GetApplicabilityChildObjectTypesID(MetaDataHelper.GetObjectTypeID("cad00650-306c-11d8-b4e9-00304f19f545"), MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545"));
    int attributeId = MetaDataHelper.GetAttributeID((object) "cad00267-306c-11d8-b4e9-00304f19f545");
    List<MeasureDescriptor> measureDescriptorList = new List<MeasureDescriptor>(0);
    MeasureForm measureForm = new MeasureForm();
    Keys modifierKeys = Control.ModifierKeys;
    IDBTypedObjectID[] dbTypedObjectIdArray = (IDBTypedObjectID[]) Intermech.Navigator.SelectionWindow.Select("Выберите объект для добавления в состав ТСЕ", "", (IDescriptor) new ObjectTypesDescriptor(childObjectTypesId.ToArray(), "Допустимые типы объектов"), typeof (IDBTypedObjectID), SelectionOptions.SelectObjects);
    if (dbTypedObjectIdArray == null || dbTypedObjectIdArray.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._contextCompositionObject);
      if (dbObject.CheckoutBy == 0L)
        ObjectCommands.CheckoutCommand(SelectedItemsHelper.CreateSelectedItemsForObject(dbObject.ObjectID), (IServiceProvider) ApplicationServices.Container, (object) null);
      else if (dbObject.CheckoutBy != sessionKeeper.Session.UserID)
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(dbObject.CheckoutBy);
        throw new KernelException($"Технологическая сборочная единица [{dbObject.ObjectID}]'{dbObject.Caption}' взята на редактирование пользователем '{objectInfo.Caption}' редактирование невозможно.");
      }
      IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MetaDataHelper.GetRelationTypeID("cad00023-306c-11d8-b4e9-00304f19f545"));
      List<long> relationIDs = new List<long>();
      foreach (IDBTypedObjectID dbTypedObjectId in dbTypedObjectIdArray)
      {
        IDBRelation dbRelation = relationCollection.Create(this._contextCompositionObject, dbTypedObjectId.ObjectID);
        dbRelation.Attributes.AddAttribute(MetaDataHelper.GetAttributeID((object) "cad00651-306c-11d8-b4e9-00304f19f545"), false, new object[1]
        {
          (object) this._selectedContext
        });
        if (modifierKeys != Keys.Control)
        {
          IDBAttribute byId = dbRelation.Attributes.FindByID(attributeId);
          if (byId != null)
          {
            MeasuredValue aMeasureValue = byId.Value as MeasuredValue;
            ArrayList listByAttributeId = MeasureEditor.GetMeasureDescriptorListByAttributeId(attributeId);
            if (listByAttributeId != null && listByAttributeId.Count > 0)
            {
              measureForm.Text = "Задайте количество для " + dbTypedObjectId.Caption;
              if (measureForm.ExecuteDialog(ref aMeasureValue, listByAttributeId.ToArray(typeof (MeasureDescriptor)) as MeasureDescriptor[]) == DialogResult.OK)
                byId.Value = (object) aMeasureValue;
            }
          }
        }
        relationIDs.Add(dbRelation.RelationID);
      }
      this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", (IList<long>) relationIDs));
    }
  }

  private void btnAdd_Click(object sender, EventArgs e)
  {
    ISelectedItems selectedItems = this._prototypeCompositionTreeView.TreeView.SelectedItems;
    if (selectedItems == null || selectedItems.Count == 0)
      return;
    List<long> relationIDs1 = new List<long>();
    List<long> relationIDs2 = new List<long>();
    List<string> objectCaptions = new List<string>();
    int attributeId = MetaDataHelper.GetAttributeID((object) "cad00267-306c-11d8-b4e9-00304f19f545");
    MeasureForm measureForm = new MeasureForm();
    List<MeasureDescriptor> measureDescriptorList = new List<MeasureDescriptor>(0);
    Keys modifierKeys = Control.ModifierKeys;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (selectedItems.Count > 0)
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._contextCompositionObject);
        if (dbObject.CheckoutBy == 0L)
          ObjectCommands.CheckoutCommand(SelectedItemsHelper.CreateSelectedItemsForObject(dbObject.ObjectID), (IServiceProvider) ApplicationServices.Container, (object) null);
        else if (dbObject.CheckoutBy != sessionKeeper.Session.UserID)
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(dbObject.CheckoutBy);
          throw new KernelException($"Технологическая сборочная единица [{dbObject.ObjectID}]'{dbObject.Caption}' взята на редактирование пользователем '{objectInfo.Caption}' редактирование невозможно.");
        }
      }
      for (int index = 0; index < selectedItems.Count; ++index)
      {
        IDBObjectID itemData1 = selectedItems.GetItemData(index, typeof (IDBObjectID)) as IDBObjectID;
        IDBRelationID itemData2 = selectedItems.GetItemData(index, typeof (IDBRelationID)) as IDBRelationID;
        if (itemData1 != null && itemData2 != null)
        {
          if (itemData1.Value == this._prototypeObject)
          {
            int num1 = (int) MessageBox.Show("Родительская сборочная единица не может быть добавлена в состав технологической сборочной единицы.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else if (MetaDataHelper.IsObjectTypeChildOf(sessionKeeper.Session.GetObjectInfo(itemData1.Value).ObjectTypeID, MetaDataHelper.GetObjectTypeID("cad00650-306c-11d8-b4e9-00304f19f545")))
          {
            int num2 = (int) MessageBox.Show("Технологическая сборочная единица не может быть добавлена в состав другой технологической сборочной единицы.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(itemData2.RelationType);
            IDBRelation relation = sessionKeeper.Session.GetRelation(itemData2.Value);
            IDBAttribute byId1 = relation.Attributes.FindByID(MetaDataHelper.GetAttributeTypeID("cad00651-306c-11d8-b4e9-00304f19f545"));
            if (byId1 == null || (long) byId1.Value != 0L && (long) byId1.Value != 1L)
            {
              objectCaptions.Add($"\"{itemData1.Caption}\"");
            }
            else
            {
              NewRelationProperties properties = new NewRelationProperties(itemData2.Value, this._contextCompositionObject, itemData1.ID)
              {
                PartObjectID = itemData1.Value,
                ValuesList = new AttributeValues[1]
                {
                  new AttributeValues(MetaDataHelper.GetAttributeTypeID("cad00651-306c-11d8-b4e9-00304f19f545"), FieldTypes.ftInteger, MultiValueModes.MultiValuesFromList, new object[1]
                  {
                    (object) this._selectedContext
                  })
                }
              };
              IDBAttribute byId2 = relation.Attributes.FindByID(attributeId);
              object obj = (object) null;
              if (byId2 != null)
              {
                if (modifierKeys == Keys.Control)
                {
                  obj = byId2.Value;
                }
                else
                {
                  if (byId2.Value is MeasuredValue measuredValue)
                  {
                    try
                    {
                      measuredValue = MeasureHelper.ConvertToMeasuredValue(measuredValue.Caption);
                    }
                    catch
                    {
                      measuredValue = byId2.Value as MeasuredValue;
                    }
                  }
                  if (measuredValue != null)
                  {
                    measureDescriptorList.Clear();
                    MeasureDescriptor descriptor = MeasureHelper.FindDescriptor(measuredValue.MeasureID);
                    foreach (MeasureDescriptor measure in MeasureHelper.Measures)
                    {
                      if (measure.PhysicalQuantityID == descriptor.PhysicalQuantityID)
                        measureDescriptorList.Add(measure);
                    }
                    measureForm.Text = "Задайте количество для " + itemData1.Caption;
                    if (measureForm.ExecuteDialog(ref measuredValue, measureDescriptorList.ToArray()) == DialogResult.OK)
                      obj = (object) measuredValue;
                  }
                }
              }
              if (obj != null)
              {
                IDBRelation dbRelation = relationCollection.Create(properties);
                dbRelation.Attributes.FindByID(attributeId).Value = obj;
                relationIDs1.Add(dbRelation.RelationID);
                if ((long) byId1.Value == 0L)
                {
                  byId1.Value = (object) 1L;
                  relationIDs2.Add(relation.RelationID);
                }
              }
            }
          }
        }
      }
    }
    if (relationIDs1.Count > 0)
      this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", (IList<long>) relationIDs1));
    if (relationIDs2.Count > 0)
      this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsChanged", (IList<long>) relationIDs2));
    if (objectCaptions.Count <= 0)
      return;
    int num = (int) new NotAddObject(objectCaptions).ShowDialog();
  }

  private void btnDelete_Click(object sender, EventArgs e)
  {
    ISelectedItems selectedItems = this.compositionTreeView.SelectedItems;
    if (selectedItems == null || selectedItems.Count == 0)
      return;
    List<long> relationIDs = new List<long>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      for (int index = 0; index < selectedItems.Count; ++index)
      {
        IDBObjectID itemData1 = selectedItems.GetItemData(index, typeof (IDBObjectID)) as IDBObjectID;
        IDBRelationID itemData2 = selectedItems.GetItemData(index, typeof (IDBRelationID)) as IDBRelationID;
        if (itemData1 != null && itemData2 != null)
        {
          if (itemData1.Value == this._contextCompositionObject)
          {
            int num = (int) MessageBox.Show("Технологическая сборочная единица не может быть исключена сама из себя.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            sessionKeeper.Session.GetRelation(itemData2.Value, false)?.Delete(0L);
            relationIDs.Add(itemData2.Value);
          }
        }
      }
    }
    if (relationIDs.Count <= 0)
      return;
    this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsRemoved", (IList<long>) relationIDs));
  }

  private void quantityAnalize_Click(object sender, EventArgs e) => this.FillAnalyzeTree();

  private int FillAnalyzeTree()
  {
    DataTable tableDesign = (DataTable) null;
    DataTable tableTech = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ICompositionLoadService customService = sessionKeeper.Session.GetCustomService(typeof (ICompositionLoadService)) as ICompositionLoadService;
      List<ColumnDescriptor> schemeDescriptors = RuntimeSearchScheme.GetCompositionQuantitySchemeDescriptors(sessionKeeper.Session);
      CompositionLoadingParams loadingParams1 = new CompositionLoadingParams((IEnumerable<ObjInfoItem>) new List<ObjInfoItem>()
      {
        new ObjInfoItem(this._prototypeObject, sessionKeeper.Session.GetObjectInfo(this._prototypeObject).ObjectTypeID)
      }, (IEnumerable<int>) null, (IEnumerable<int>) null, (IEnumerable<int>) new List<int>()
      {
        sessionKeeper.Session.IdentHelper.SPRelationTypeID
      }, (IEnumerable<ColumnDescriptor>) schemeDescriptors, (IEnumerable<ConditionStructure>) null, true, false, -1, (VersionsRule) null, string.Empty, (IDictionary<long, HybridDictionary>) new Dictionary<long, HybridDictionary>()
      {
        {
          this._prototypeObject,
          new HybridDictionary(0, true)
          {
            [(object) "{82E381A1-8952-416A-B303-F81BA2945F8F}"] = (object) true,
            [(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) new List<long>()
            {
              0L,
              1L
            }
          }
        }
      });
      tableDesign = customService.LoadComplexCompositions((object) sessionKeeper.Session.SessionGUID, loadingParams1);
      HybridDictionary hybridDictionary = new HybridDictionary(0, true);
      hybridDictionary[(object) "{82E381A1-8952-416A-B303-F81BA2945F8F}"] = (object) true;
      List<long> longList = new List<long>() { 0L };
      if (!longList.Contains(this._selectedContext))
        longList.Add(this._selectedContext);
      hybridDictionary[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) longList;
      CompositionLoadingParams loadingParams2 = new CompositionLoadingParams((IEnumerable<ObjInfoItem>) new List<ObjInfoItem>()
      {
        new ObjInfoItem(this._contextCompositionObject, sessionKeeper.Session.GetObjectInfo(this._contextCompositionObject).ObjectTypeID)
      }, (IEnumerable<int>) null, (IEnumerable<int>) null, (IEnumerable<int>) new List<int>()
      {
        sessionKeeper.Session.IdentHelper.SPRelationTypeID
      }, (IEnumerable<ColumnDescriptor>) schemeDescriptors, (IEnumerable<ConditionStructure>) null, true, false, -1, (VersionsRule) null, string.Empty, (IDictionary<long, HybridDictionary>) new Dictionary<long, HybridDictionary>()
      {
        {
          this._contextCompositionObject,
          hybridDictionary
        }
      });
      tableTech = customService.LoadComplexCompositions((object) sessionKeeper.Session.SessionGUID, loadingParams2);
    }
    AnalyzeQuantity analyzeQuantity = new AnalyzeQuantity(tableDesign, tableTech);
    int num1 = analyzeQuantity.Analyze();
    int num2 = (int) analyzeQuantity.ShowDialog();
    return num1;
  }

  protected override string GetPersistString()
  {
    try
    {
      XmlDocument state = this.GetState();
      using (TextWriter w1 = (TextWriter) new StringWriter())
      {
        XmlWriter w2 = (XmlWriter) new XmlTextWriter(w1);
        state.WriteTo(w2);
        w2.Flush();
        w2.Close();
        return w1.ToString();
      }
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message, "Невозможно сохранить состояние окна!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return (string) null;
    }
  }

  private XmlDocument GetState()
  {
    XmlDocument xmlDoc = new XmlDocument();
    XmlNode element = (XmlNode) xmlDoc.CreateElement("Settings");
    element.AppendChild(this.GetPropertiesNode(xmlDoc));
    xmlDoc.AppendChild((XmlNode) xmlDoc.CreateXmlDeclaration("1.0", (string) null, (string) null));
    xmlDoc.AppendChild(element);
    return xmlDoc;
  }

  private XmlNode GetPropertiesNode(XmlDocument xmlDoc)
  {
    XmlElement element1 = xmlDoc.CreateElement("StartUPProperties");
    XmlNode element2 = (XmlNode) xmlDoc.CreateElement("PrototypeObject");
    element2.AppendChild((XmlNode) xmlDoc.CreateTextNode(XmlConvert.ToString(this._prototypeObject)));
    element1.AppendChild(element2);
    XmlNode element3 = (XmlNode) xmlDoc.CreateElement("ContextCompositionObject");
    element3.AppendChild((XmlNode) xmlDoc.CreateTextNode(XmlConvert.ToString(this._contextCompositionObject)));
    element1.AppendChild(element3);
    XmlNode element4 = (XmlNode) xmlDoc.CreateElement("SelectedContext");
    element4.AppendChild((XmlNode) xmlDoc.CreateTextNode(XmlConvert.ToString(this._selectedContext)));
    element1.AppendChild(element4);
    XmlNode element5 = (XmlNode) xmlDoc.CreateElement("ContextName");
    element5.AppendChild((XmlNode) xmlDoc.CreateTextNode(this._contexName));
    element1.AppendChild(element5);
    XmlNode element6 = (XmlNode) xmlDoc.CreateElement("RelationID");
    element6.AppendChild((XmlNode) xmlDoc.CreateTextNode(XmlConvert.ToString(this._relationID)));
    element1.AppendChild(element6);
    XmlNode element7 = (XmlNode) xmlDoc.CreateElement("FiltrationOwnerID");
    element7.AppendChild((XmlNode) xmlDoc.CreateTextNode(this.Get_FiltrationOwnerID()));
    element1.AppendChild(element7);
    return (XmlNode) element1;
  }

  public (long prototypeObject, long contextCompositionObject, long selectedContext, string contextName, long relationID)? RestoreState(
    string persistString)
  {
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(persistString);
    return this.RestoreProperties(xmlDocument.SelectSingleNode("/Settings"));
  }

  private (long prototypeObject, long contextCompositionObject, long selectedContext, string contextName, long relationID)? RestoreProperties(
    XmlNode settingsNode)
  {
    XmlNode xmlNode1 = settingsNode.SelectSingleNode("StartUPProperties/PrototypeObject");
    if (xmlNode1 == null)
      return new (long, long, long, string, long)?();
    long int64_1 = XmlConvert.ToInt64(xmlNode1.InnerText);
    XmlNode xmlNode2 = settingsNode.SelectSingleNode("StartUPProperties/ContextCompositionObject");
    if (xmlNode2 == null)
      return new (long, long, long, string, long)?();
    long int64_2 = XmlConvert.ToInt64(xmlNode2.InnerText);
    XmlNode xmlNode3 = settingsNode.SelectSingleNode("StartUPProperties/SelectedContext");
    if (xmlNode3 == null)
      return new (long, long, long, string, long)?();
    long int64_3 = XmlConvert.ToInt64(xmlNode3.InnerText);
    XmlNode xmlNode4 = settingsNode.SelectSingleNode("StartUPProperties/ContextName");
    if (xmlNode4 == null)
      return new (long, long, long, string, long)?();
    string innerText1 = xmlNode4.InnerText;
    XmlNode xmlNode5 = settingsNode.SelectSingleNode("StartUPProperties/RelationID");
    if (xmlNode5 == null)
      return new (long, long, long, string, long)?();
    long int64_4 = XmlConvert.ToInt64(xmlNode5.InnerText);
    XmlNode xmlNode6 = settingsNode.SelectSingleNode("StartUPProperties/FiltrationOwnerID");
    if (xmlNode6 != null)
    {
      string innerText2 = xmlNode6.InnerText;
      if (this.filtrationOwnerID.Length > 0 && this.filtrationOwnerID != innerText2)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          if (sessionKeeper.Session.GetCustomService(typeof (IVersionRulesCacheService)) is IVersionRulesCacheService customService)
            customService.SetFiltrationSettings((object) sessionKeeper.Session.SessionGUID, this.filtrationOwnerID, (FiltrationSettings) null);
        }
      }
      this.filtrationOwnerID = innerText2;
      this.filtrationsApplyed = false;
    }
    return new (long, long, long, string, long)?((int64_1, int64_2, int64_3, innerText1, int64_4));
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ContextCompositionEditor));
    this.mainSplitContainer = new SplitContainer();
    this.addDelObjectTCE = new Intermech.Bars.ToolBar();
    this.imagesToolbars = new ImageList(this.components);
    this.emptyLabel = new LabelItem();
    this.btnAdd = new ButtonItem();
    this.btnDelete = new ButtonItem();
    this.objectSplitContainer = new RadSplitContainer();
    this.splitPanel1 = new SplitPanel();
    this.compositionTreeView = new NavigatorTreeView();
    this.additionalMenu = new Intermech.Bars.ToolBar();
    this.menuImageList = new ImageList(this.components);
    this.addObjectToTSE = new ButtonItem();
    this.quantityAnalize = new ButtonItem();
    this.contextName = new LabelItem();
    this.splitPanel2 = new SplitPanel();
    this.fluentTheme1 = new FluentTheme();
    this.mainSplitContainer.BeginInit();
    this.mainSplitContainer.Panel1.SuspendLayout();
    this.mainSplitContainer.Panel2.SuspendLayout();
    this.mainSplitContainer.SuspendLayout();
    this.objectSplitContainer.BeginInit();
    this.objectSplitContainer.SuspendLayout();
    this.splitPanel1.BeginInit();
    this.splitPanel1.SuspendLayout();
    this.compositionTreeView.BeginInit();
    this.splitPanel2.BeginInit();
    this.SuspendLayout();
    this.mainSplitContainer.Dock = DockStyle.Fill;
    this.mainSplitContainer.Location = new Point(0, 0);
    this.mainSplitContainer.Margin = new Padding(4);
    this.mainSplitContainer.Name = "mainSplitContainer";
    this.mainSplitContainer.Panel1.Controls.Add((Control) this.addDelObjectTCE);
    this.mainSplitContainer.Panel2.Controls.Add((Control) this.objectSplitContainer);
    this.mainSplitContainer.Size = new Size(920, 543);
    this.mainSplitContainer.SplitterDistance = 306;
    this.mainSplitContainer.SplitterWidth = 5;
    this.mainSplitContainer.TabIndex = 0;
    this.addDelObjectTCE.AddRemoveButtonsVisible = false;
    this.addDelObjectTCE.AllowHorizontalDock = false;
    this.addDelObjectTCE.Dock = DockStyle.Right;
    this.addDelObjectTCE.DockLine = 3;
    this.addDelObjectTCE.DrawActionsButton = false;
    this.addDelObjectTCE.Flow = ToolBarLayout.Vertical;
    this.addDelObjectTCE.FullMenus = true;
    this.addDelObjectTCE.Guid = new Guid("58836909-e764-4f11-a441-6f5c2fc5bb64");
    this.addDelObjectTCE.Hidden = false;
    this.addDelObjectTCE.ImageList = this.imagesToolbars;
    this.addDelObjectTCE.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.emptyLabel,
      (ToolbarItemBase) this.btnAdd,
      (ToolbarItemBase) this.btnDelete
    });
    this.addDelObjectTCE.Location = new Point(281, 0);
    this.addDelObjectTCE.Margin = new Padding(4);
    this.addDelObjectTCE.Name = "addDelObjectTCE";
    this.addDelObjectTCE.Size = new Size(25, 543);
    this.addDelObjectTCE.TabIndex = 1;
    this.addDelObjectTCE.Text = "Добавление и удаление в состав ТСЕ";
    this.imagesToolbars.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolbars.ImageStream");
    this.imagesToolbars.TransparentColor = Color.Transparent;
    this.imagesToolbars.Images.SetKeyName(0, "arrow_right_blue.ico");
    this.imagesToolbars.Images.SetKeyName(1, "arrow_left_blue.ico");
    this.imagesToolbars.Images.SetKeyName(2, "refresh.ico");
    this.emptyLabel.CommandName = "emptyLabel";
    this.emptyLabel.Text = " ";
    this.emptyLabel.ToolTipText = " ";
    this.btnAdd.CommandName = "btnAdd";
    this.btnAdd.ImageIndex = 0;
    this.btnAdd.ToolTipText = "Добавить указанные элементы в состав технологической сборочной единицы\n\rС нажатым Ctrl использовать все конструкторское количество";
    this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
    this.btnDelete.CommandName = "btnDelete";
    this.btnDelete.ImageIndex = 1;
    this.btnDelete.ToolTipText = "Удалить указанные элементы из технологической сборочной единицы";
    this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
    this.objectSplitContainer.Controls.Add((Control) this.splitPanel1);
    this.objectSplitContainer.Controls.Add((Control) this.splitPanel2);
    this.objectSplitContainer.Dock = DockStyle.Fill;
    this.objectSplitContainer.Location = new Point(0, 0);
    this.objectSplitContainer.Margin = new Padding(4);
    this.objectSplitContainer.Name = "objectSplitContainer";
    this.objectSplitContainer.Orientation = Orientation.Horizontal;
    this.objectSplitContainer.RootElement.MinSize = new Size(25, 25);
    this.objectSplitContainer.Size = new Size(609, 543);
    this.objectSplitContainer.SplitterWidth = 10;
    this.objectSplitContainer.TabIndex = 0;
    this.objectSplitContainer.TabStop = false;
    this.objectSplitContainer.ThemeName = "Fluent";
    this.splitPanel1.Controls.Add((Control) this.compositionTreeView);
    this.splitPanel1.Controls.Add((Control) this.additionalMenu);
    this.splitPanel1.Location = new Point(0, 0);
    this.splitPanel1.Name = "splitPanel1";
    this.splitPanel1.RootElement.MinSize = new Size(25, 25);
    this.splitPanel1.Size = new Size(609, 300);
    this.splitPanel1.SizeInfo.AutoSizeScale = new SizeF(0.0f, 0.2211896f);
    this.splitPanel1.SizeInfo.MinimumSize = new Size(0, 300);
    this.splitPanel1.TabIndex = 0;
    this.splitPanel1.TabStop = false;
    this.splitPanel1.ThemeName = "Fluent";
    this.compositionTreeView.AllowDrop = true;
    this.compositionTreeView.AllowMultiSelect = false;
    this.compositionTreeView.AllowUserPinnedColumns = false;
    this.compositionTreeView.DisableCheckedOutColumn = true;
    this.compositionTreeView.Dock = DockStyle.Fill;
    this.compositionTreeView.HeaderStyle.HorzAlignment = StringAlignment.Near;
    this.compositionTreeView.ImageList = (ImageList) null;
    this.compositionTreeView.LineStyle = LineStyle.Dot;
    this.compositionTreeView.Location = new Point(0, 29);
    this.compositionTreeView.Margin = new Padding(4);
    this.compositionTreeView.Name = "compositionTreeView";
    this.compositionTreeView.RowEvenStyle.WordWrap = false;
    this.compositionTreeView.RowOddStyle.WordWrap = false;
    this.compositionTreeView.RowSelectedStyle.WordWrap = false;
    this.compositionTreeView.RowStyle.BorderColor = SystemColors.Control;
    this.compositionTreeView.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this.compositionTreeView.RowStyle.BorderWidth = 1;
    this.compositionTreeView.RowStyle.WordWrap = false;
    this.compositionTreeView.SelectBeforeEdit = true;
    this.compositionTreeView.ShowRootRow = false;
    this.compositionTreeView.Size = new Size(609, 271);
    this.compositionTreeView.SuppressErrorMessages = true;
    this.compositionTreeView.TabIndex = 2;
    this.additionalMenu.FlipLastItem = true;
    this.additionalMenu.FullMenus = true;
    this.additionalMenu.Guid = new Guid("fc565529-0213-4973-a847-6e6fe1f690db");
    this.additionalMenu.Hidden = false;
    this.additionalMenu.ImageList = this.menuImageList;
    this.additionalMenu.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.addObjectToTSE,
      (ToolbarItemBase) this.quantityAnalize,
      (ToolbarItemBase) this.contextName
    });
    this.additionalMenu.Location = new Point(0, 0);
    this.additionalMenu.Margin = new Padding(4);
    this.additionalMenu.Name = "additionalMenu";
    this.additionalMenu.Size = new Size(609, 29);
    this.additionalMenu.TabIndex = 1;
    this.additionalMenu.Text = "";
    this.menuImageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("menuImageList.ImageStream");
    this.menuImageList.TransparentColor = Color.Transparent;
    this.menuImageList.Images.SetKeyName(0, "Все типы объектов.ico");
    this.menuImageList.Images.SetKeyName(1, "Q.png");
    this.addObjectToTSE.CommandName = "addObjectToTSE";
    this.addObjectToTSE.ImageIndex = 0;
    this.addObjectToTSE.Text = "Добавить произвольный объект";
    this.addObjectToTSE.ToolTipText = "Добавить произвольный объект\n\rС нажатым Ctrl без ввода количества";
    this.addObjectToTSE.Click += new EventHandler(this.addObjectToTSE_Click);
    this.quantityAnalize.BeginGroup = true;
    this.quantityAnalize.CommandName = "quantityAnalize";
    this.quantityAnalize.ImageIndex = 1;
    this.quantityAnalize.Text = "Сравнительный анализ количества";
    this.quantityAnalize.ToolTipText = "Сравнительный анализ количества";
    this.quantityAnalize.Click += new EventHandler(this.quantityAnalize_Click);
    this.contextName.CommandName = "contextName";
    this.contextName.Locked = true;
    this.contextName.Text = "Технологический контекст";
    this.contextName.ToolTipText = "ContextName";
    this.splitPanel2.Location = new Point(0, 310);
    this.splitPanel2.Name = "splitPanel2";
    this.splitPanel2.RootElement.MinSize = new Size(25, 25);
    this.splitPanel2.Size = new Size(609, 250);
    this.splitPanel2.SizeInfo.AutoSizeScale = new SizeF(0.0f, -0.2211896f);
    this.splitPanel2.SizeInfo.MinimumSize = new Size(0, 250);
    this.splitPanel2.TabIndex = 1;
    this.splitPanel2.TabStop = false;
    this.splitPanel2.Text = "splitPanel2";
    this.splitPanel2.ThemeName = "Fluent";
    this.AutoScaleDimensions = new SizeF(8f, 16f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.mainSplitContainer);
    this.Margin = new Padding(4);
    this.Name = nameof (ContextCompositionEditor);
    this.Size = new Size(920, 543);
    this.Text = "Редактор ТСЕ";
    this.mainSplitContainer.Panel1.ResumeLayout(false);
    this.mainSplitContainer.Panel2.ResumeLayout(false);
    this.mainSplitContainer.EndInit();
    this.mainSplitContainer.ResumeLayout(false);
    this.objectSplitContainer.EndInit();
    this.objectSplitContainer.ResumeLayout(false);
    this.splitPanel1.EndInit();
    this.splitPanel1.ResumeLayout(false);
    this.compositionTreeView.EndInit();
    this.splitPanel2.EndInit();
    this.ResumeLayout(false);
  }
}
