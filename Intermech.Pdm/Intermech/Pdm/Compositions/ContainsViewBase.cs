// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ContainsViewBase
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Navigator.VirtualNodes;
using Intermech.Pdm.Compositions.ContainsBase;
using Intermech.Pdm.SearchScheme;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class ContainsViewBase : ChildrenView
{
  private static readonly string _statusText = LocalizationHolder.rm.GetString("Pdm_50");
  private const string _stateStreamName = "ContainsView_{0}_{1}_{2}";
  private int _objectType = -1;
  private bool _firstEnter = true;
  public static VirtualSchemesMode VirtualSchemesMode = VirtualSchemesMode.GroupByText;
  private int _iconStart = -1;
  private int _iconStop = -1;
  private bool _queryBegining;
  private Thread _circleThread;
  private Thread _queryThread;
  private BackgroundReader _reader;
  private ContainsMode _mode;
  private bool _reloadFromSelf;
  private bool _activated;
  private ContainsViewBase.Types4Atributes _types4attributes;
  private static ContainsViewBase.SearchSchemeChoiceCache _searchSchemeChoiceCache;
  private IContainer components;
  private ButtonItem biStartQuery;
  private ComboBoxItem cbiScheme;
  private ButtonItem biDefaultColumns;
  private ButtonItem biNewScheme;
  private ToolStripStatusLabel tsslSelectState;
  private ButtonItem biEditScheme;
  private ButtonItem biRefreshSchemes;
  private ToolStripStatusLabel ttslCircle;
  private BarManager barManager1;
  private ToolBarContainer topBarDock;

  public long ObjectID { get; private set; } = -1;

  public long ID { get; private set; } = -1;

  private long CurrentSchemeID
  {
    get
    {
      SearchSchemeID currentScheme = this.CurrentScheme;
      return currentScheme == null ? 0L : currentScheme.SchemeID;
    }
  }

  public SearchSchemeID CurrentScheme
  {
    get
    {
      return this.cbiScheme?.ComboBox == null ? (SearchSchemeID) null : this.cbiScheme.ComboBox.SelectedItem as SearchSchemeID;
    }
  }

  public ContainsViewBase()
  {
  }

  public ContainsViewBase(ContainsMode mode)
  {
    this._readDataOnActivate = false;
    this._mode = mode;
    this._useInheritedNavViews = false;
    if (ContainsViewBase._searchSchemeChoiceCache == null)
      ContainsViewBase._searchSchemeChoiceCache = new ContainsViewBase.SearchSchemeChoiceCache();
    this.Options |= ChildrenViewOptions.DisablePathProcessing;
    this.InitializeComponent();
    this._filtersComboBoxItem.Stretch = false;
    this.cbiScheme.ComboBox.SelectedValueChanged += new EventHandler(this.ComboBox_SelectedValueChanged);
    this.SetControlIcons();
    this._reader = new BackgroundReader((IServiceProvider) this._services);
    BackgroundReader reader = this._reader;
    reader.StateChangedEvent = reader.StateChangedEvent + new StateChanged(this.ReaderStateChanged);
    this._readNextToolStripDropDownButton.Enabled = false;
    this._readNextToolStripDropDownButton.Visible = false;
    this._readAllToolStripDropDownButton.Enabled = false;
    this._readAllToolStripDropDownButton.Visible = false;
    this._refreshButtonItem.Visible = false;
    this._manualSortingSetupButtonItem.Visible = false;
    this._toggleManualSortingButtonItem.Visible = false;
    this._statusStrip.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.tsslSelectState,
      (ToolStripItem) this.ttslCircle
    });
    this.buttonHeightSet.Index = this.cbiScheme.Index + 1;
  }

  protected override void NotificationEventFired(object sender, NotificationEventArgs e)
  {
    base.NotificationEventFired(sender, e);
    if (!(e.EventName == "ObjectsCheckedIn") && !(e.EventName == "ObjectsCheckedOut") && !(e.EventName == "ObjectsChangesCancelled") || !(e is DBObjectsEventArgs objectsEventArgs) || !objectsEventArgs.ObjectIDs.Contains(this.ObjectID))
      return;
    this.ObjectID = e.EventName == "ObjectsCheckedOut" ? -1L * this.ObjectID : Math.Abs(this.ObjectID);
  }

  public override void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    if (this._queryBegining)
    {
      if (this._queryThread != null)
        this._queryThread.Abort();
      if (this._circleThread != null)
        this._circleThread.Abort();
    }
    if (this._reloadFromSelf)
    {
      this._reloadFromSelf = false;
    }
    else
    {
      IDBTypedObjectID itemData = (IDBTypedObjectID) items.GetItemData(0, typeof (IDBTypedObjectID));
      this.ObjectID = itemData.ObjectID;
      this.ID = itemData.ID;
      this._objectType = itemData.ObjectType;
      base.Initialize(items, provider);
      this.tsslSelectState.Text = string.Empty;
      this._reader.State = BackgroundState.Empty;
      this._activated = false;
      this._firstEnter = true;
      base.Initialize(items, provider);
    }
  }

  public override void Activate(IView previousView)
  {
    if (!this._activated && previousView != PageViewsManager.BlackHoleView)
    {
      if (this._firstEnter)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          this._firstEnter = false;
          this.RefreshComboBoxWithSchemes(sessionKeeper.Session, this._objectType);
        }
      }
      this._activated = true;
      this.SetColumns(true);
    }
    if (previousView == PageViewsManager.BlackHoleView)
      return;
    base.Activate(previousView);
  }

  protected override void AfterContentsSorted()
  {
    bool flag = false;
    BackgroundState state = this._reader.State;
    if (this._reader.QueryResult != null && this._reader.State != BackgroundState.Fill)
    {
      state = this._reader.State;
      this._reader.State = BackgroundState.Fill;
      flag = true;
    }
    base.AfterContentsSorted();
    if (!flag)
      return;
    this._reader.State = state;
  }

  protected override bool reloadGridOnDisableColumnsSortingOrGrouping
  {
    get => this._reader.QueryResult != null && this._reader.QueryResult.Rows.Count > 0;
  }

  private void RefreshComboBoxWithSchemes(IUserSession session, int objectType)
  {
    List<Guid> possibleRelationTypes = this.GetPossibleRelationTypes(session, objectType);
    this.AddShemes(session, possibleRelationTypes, objectType);
    if (this.cbiScheme.Items.Count > 0)
    {
      try
      {
        this.cbiScheme.ComboBox.SelectedValueChanged -= new EventHandler(this.ComboBox_SelectedValueChanged);
        long schemeId = ContainsViewBase._searchSchemeChoiceCache.GetSchemeID(this._mode, objectType);
        if (schemeId != 0L)
        {
          bool flag = false;
          for (int index = 0; index < this.cbiScheme.Items.Count; ++index)
          {
            if (this.cbiScheme.Items[index] is SearchSchemeID && (this.cbiScheme.Items[index] as SearchSchemeID).SchemeID == schemeId)
            {
              this.cbiScheme.ComboBox.SelectedIndex = index;
              flag = true;
              break;
            }
          }
          if (flag)
            return;
          this.cbiScheme.ComboBox.SelectedIndex = 0;
        }
        else
          this.cbiScheme.ComboBox.SelectedIndex = 0;
      }
      finally
      {
        this.cbiScheme.ComboBox.SelectedValueChanged += new EventHandler(this.ComboBox_SelectedValueChanged);
        this.SetTypes4Columns();
        this.UpdateEnabled();
      }
    }
    else
      ((ContainsNode) this.Node).Columns = this.GetDefaultColumns();
  }

  protected override INode GetNode()
  {
    ContainsNode node = new ContainsNode(this._objectType, this.ObjectID);
    if (this.CurrentScheme != null)
    {
      node.Scheme = this.CurrentScheme;
      node.Columns = this.GetColumnsFromScheme(this.CurrentScheme);
    }
    else
      node.Columns = this.GetDefaultColumns();
    node.Reader = this._reader;
    AdvancedServiceContainer serviceContainer = new AdvancedServiceContainer((IServiceProvider) this._services);
    RelationPair rootObjectKey = this.GetRootObjectKey();
    if (rootObjectKey != null && !rootObjectKey.Empty && rootObjectKey.TOP_OBJECT_ID != 0L)
      serviceContainer.AddService(typeof (RelationPair), (object) rootObjectKey);
    if (this._parentNode is IContextAware parentNode)
      serviceContainer.AdvancedProvider = parentNode.Services;
    node.Services = (IServiceProvider) serviceContainer;
    if (this._reader != null)
      this._reader.Services = (IServiceProvider) serviceContainer;
    return (INode) node;
  }

  private void SetControlIcons()
  {
    if (!(ServicesManager.GetService(typeof (INamedImageList)) is INamedImageList service))
      return;
    this._iconStart = service.ImageIndex("imgStart");
    this._iconStop = service.ImageIndex("imgStop2");
    this._toolBar.ImageList = service.ImageList;
    this.biDefaultColumns.ImageIndex = service.ImageIndex("imgDefaultColumns");
    this.biNewScheme.ImageIndex = service.ImageIndex("imgNewScheme");
    this.biEditScheme.ImageIndex = service.ImageIndex("imgEditScheme");
    this.biRefreshSchemes.ImageIndex = service.ImageIndex("imgRefresh");
    this.biStartQuery.ImageIndex = this._iconStart;
  }

  protected virtual List<Guid> GetPossibleRelationTypes(IUserSession session, int objType)
  {
    return (List<Guid>) null;
  }

  protected void FillApplicability(
    IUserSession session,
    DataTable appTable,
    ref List<Guid> relationTypes)
  {
    foreach (DataRow row in (InternalDataCollectionBase) appTable.Rows)
    {
      if (Convert.ToInt32(row["F_MIN_LINKS"]) != -1)
      {
        Guid guid = (session.GetRelationType(Convert.ToInt32(row["F_RELATION_TYPE"])) as IDBGuid).GUID;
        if (!relationTypes.Contains(guid))
          relationTypes.Add(guid);
      }
    }
  }

  private void ReaderStateChanged(object sender, StateChangedEventArgs arg)
  {
    try
    {
      switch (arg.State)
      {
        case BackgroundState.Empty:
          this._statusStrip.Invoke((Delegate) new ContainsViewBase.SetPersentHandler(this.SetPersent), (object) string.Empty, (object) 0);
          this._dataLoaded = true;
          break;
        case BackgroundState.Error:
          this._statusStrip.Invoke((Delegate) new ContainsViewBase.SetPersentHandler(this.SetPersent), (object) string.Empty, (object) 0);
          this.SetEnabledControls(true);
          this._toolBar.Invoke((Delegate) new ContainsViewBase.SetButtonItemHandler(this.SetButtonItem), (object) true);
          if (this._circleThread != null)
          {
            this._circleThread.Abort();
            this._circleThread.Join();
            this.SetCircle(string.Empty);
            this._circleThread = (Thread) null;
          }
          this._reader.State = BackgroundState.Empty;
          break;
        case BackgroundState.Reading:
          this.SetEnabledControls(false);
          this._toolBar.Invoke((Delegate) new ContainsViewBase.SetButtonItemHandler(this.SetButtonItem), (object) false);
          this._queryBegining = true;
          this._statusStrip.Invoke((Delegate) new ContainsViewBase.SetPersentHandler(this.SetPersent), (object) ContainsViewBase._statusText, (object) 0);
          break;
        case BackgroundState.Fill:
          this._statusStrip.Invoke((Delegate) new ContainsViewBase.SetPersentHandler(this.SetPersent), (object) string.Empty, (object) 0);
          this.SetEnabledControls(true);
          this._toolBar.Invoke((Delegate) new ContainsViewBase.SetButtonItemHandler(this.SetButtonItem), (object) true);
          if (this._circleThread != null)
          {
            this._circleThread.Abort();
            this._circleThread.Join();
            this.SetCircle(string.Empty);
            this._circleThread = (Thread) null;
          }
          this._dataLoaded = true;
          break;
        case BackgroundState.SetPersent:
          this._statusStrip.Invoke((Delegate) new ContainsViewBase.SetPersentHandler(this.SetPersent), (object) ContainsViewBase._statusText, (object) arg.Percent);
          break;
      }
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
  }

  private void SetEnabledControls(bool enable)
  {
    this._grid.Invoke((Delegate) new ContainsViewBase.SetEnabledHandler(this.SetEnabled), (object) this._grid, (object) enable);
    this._toolBar.Invoke((Delegate) new ContainsViewBase.SetEnabledHandler(this.SetEnabled), (object) this._toolBar, (object) enable);
    this.cbiScheme.ComboBox.Invoke((Delegate) new ContainsViewBase.SetEnabledHandler(this.SetEnabled), (object) this.cbiScheme.ComboBox, (object) enable);
  }

  private void SetCircle(string text) => this.ttslCircle.Text = text;

  private void SetButtonItem(bool start)
  {
    this.biStartQuery.ImageIndex = start ? this._iconStart : this._iconStop;
    this.biStartQuery.ToolTipText = start ? LocalizationHolder.rm.GetString("Pdm_52") : LocalizationHolder.rm.GetString("Pdm_53");
    if (!start)
      return;
    this._queryBegining = false;
    this.ReloadItems();
  }

  private void SetPersent(string text, int persent) => this.tsslSelectState.Text = text;

  private void SetEnabled(System.Windows.Forms.Control sender, bool enable)
  {
    if (sender == this._toolBar)
    {
      this._embeddedViewsDropDownMenuItem.Enabled = enable;
      this.biNewScheme.Enabled = enable;
      this.biDefaultColumns.Enabled = enable;
      this.biRefreshSchemes.Enabled = enable;
      this.biEditScheme.Enabled = enable && this.CurrentSchemeID > 0L;
    }
    else
      sender.Enabled = enable;
  }

  private string ComboSeparatorString
  {
    get
    {
      return "———————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————————";
    }
  }

  private bool Check4Role(IDBObject scheme, IUserSession session)
  {
    IDBAttribute attributeByGuid = scheme.GetAttributeByGuid(new Guid("cad00d18-306c-11d8-b4e9-00304f19f545"));
    bool flag = true;
    if (attributeByGuid != null && CompareValuesHelper.NormalizedValue(attributeByGuid.Value) != null)
    {
      flag = false;
      foreach (object obj in attributeByGuid.Values)
      {
        SearchSchemeRole searchSchemeRole = new SearchSchemeRole(obj.ToString(), session);
        if (searchSchemeRole.ValidRole && searchSchemeRole.RoleID == session.RoleID)
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  private bool Check4Type(IDBObject scheme, int objectType)
  {
    IDBAttribute attributeByGuid = scheme.GetAttributeByGuid(new Guid(SearchConsts.attributeScheme4Types));
    if (attributeByGuid == null || attributeByGuid.IsNull)
      return true;
    bool flag = false;
    for (int index = 0; index < attributeByGuid.ValuesCount; ++index)
    {
      attributeByGuid.Index = index;
      if (!attributeByGuid.IsNull)
      {
        int objectTypeId = MetaDataHelper.GetObjectTypeID(new Guid(attributeByGuid.AsString));
        if (objectTypeId != -1 && MetaDataHelper.GetObjectTypeChildrenIDRecursive(objectTypeId).Contains(objectType))
          return true;
      }
    }
    return flag;
  }

  private void AddShemes(IUserSession session, List<Guid> relTypes, int objectType)
  {
    List<SearchSchemeID> searchSchemeIdList = new List<SearchSchemeID>();
    if (relTypes != null && relTypes.Count > 0)
    {
      List<SearchSchemaInfo> forRelationTypesEx = ((ICompositionService) session.GetCustomService(typeof (ICompositionService))).GetSchemesForRelationTypesEx(session.SessionGUID, relTypes, this._mode, true);
      if (forRelationTypesEx != null && forRelationTypesEx.Count > 0)
        searchSchemeIdList.AddRange((IEnumerable<SearchSchemeID>) forRelationTypesEx.ConvertAll<SearchSchemeID>((Converter<SearchSchemaInfo, SearchSchemeID>) (_ => new SearchSchemeID(_.Name, _.SchemeID))));
    }
    this.cbiScheme.Items.Clear();
    searchSchemeIdList.Sort((Comparison<SearchSchemeID>) ((x, y) =>
    {
      if (x.Name == null && y.Name == null)
        return 0;
      if (x.Name == null)
        return -1;
      return y.Name == null ? 1 : x.Name.CompareTo(y.Name);
    }));
    foreach (object obj in searchSchemeIdList)
      this.cbiScheme.Items.Add(obj);
    if (ContainsViewBase.VirtualSchemesMode == VirtualSchemesMode.None)
      return;
    IDBRelationsApplicabilityCollection applicabilityCollection = session.GetRelationsApplicabilityCollection();
    DataTable applicabilitiesList;
    string columnName;
    if (this._mode == ContainsMode.Contains)
    {
      applicabilitiesList = applicabilityCollection.GetApplicabilitiesList(-1, -1, this._objectType);
      columnName = "F_OBJECT_TYPE";
    }
    else
    {
      applicabilitiesList = applicabilityCollection.GetApplicabilitiesList(-1, this._objectType, -1);
      columnName = "F_INOBJECT_TYPE";
    }
    if (applicabilitiesList != null)
    {
      if (ContainsViewBase.VirtualSchemesMode == VirtualSchemesMode.GroupByRelation)
      {
        List<Tuple<int, string, List<int>>> tupleList = new List<Tuple<int, string, List<int>>>();
        foreach (DataRow row in (InternalDataCollectionBase) applicabilitiesList.Rows)
        {
          int relTypeID = Convert.ToInt32(row["F_RELATION_TYPE"]);
          Tuple<int, string, List<int>> tuple = tupleList.Find((Predicate<Tuple<int, string, List<int>>>) (x => x.Item1 == relTypeID));
          List<int> intList = (List<int>) null;
          if (tuple == null)
          {
            intList = new List<int>();
            tupleList.Add(new Tuple<int, string, List<int>>(relTypeID, this.GetRelationTypeName(relTypeID), intList));
          }
          intList.Add(Convert.ToInt32(row[columnName]));
        }
        bool flag = this.cbiScheme.Items.Count == 0;
        tupleList.Sort((Comparison<Tuple<int, string, List<int>>>) ((x, y) =>
        {
          if (x.Item2 == null && y.Item2 == null)
            return 0;
          if (x.Item2 == null)
            return -1;
          return y.Item2 == null ? 1 : x.Item2.CompareTo(y.Item2);
        }));
        foreach (Tuple<int, string, List<int>> tuple in tupleList)
        {
          if (!flag)
          {
            this.cbiScheme.Items.Add((object) this.ComboSeparatorString);
            flag = true;
          }
          this.cbiScheme.Items.Add((object) new VirtualSearchSchemeID(tuple.Item2, this._mode, tuple.Item1, tuple.Item3));
        }
      }
      else
      {
        SortedDictionary<string, Tuple<List<int>, List<int>>> sortedDictionary = new SortedDictionary<string, Tuple<List<int>, List<int>>>();
        foreach (DataRow row in (InternalDataCollectionBase) applicabilitiesList.Rows)
        {
          int int32_1 = Convert.ToInt32(row["F_RELATION_TYPE"]);
          IMSRelationType relationType = MetaDataHelper.GetRelationType(int32_1);
          string key = this._mode == ContainsMode.Contains ? relationType.TypeName : relationType.ReverseName;
          Tuple<List<int>, List<int>> tuple;
          if (!sortedDictionary.TryGetValue(key, out tuple))
          {
            tuple = new Tuple<List<int>, List<int>>(new List<int>(), new List<int>());
            sortedDictionary.Add(key, tuple);
          }
          if (!tuple.Item1.Contains(int32_1))
            tuple.Item1.Add(int32_1);
          int int32_2 = Convert.ToInt32(row[columnName]);
          if (!tuple.Item2.Contains(int32_2))
            tuple.Item2.Add(int32_2);
        }
        bool flag = this.cbiScheme.Items.Count == 0;
        foreach (KeyValuePair<string, Tuple<List<int>, List<int>>> keyValuePair in sortedDictionary)
        {
          if (!flag)
          {
            this.cbiScheme.Items.Add((object) this.ComboSeparatorString);
            flag = true;
          }
          this.cbiScheme.Items.Add((object) new VirtualSearchSchemeID(keyValuePair.Key, this._mode, keyValuePair.Value.Item1, new List<int>()));
        }
      }
    }
    this.UpdateEnabled();
  }

  private string GetRelationTypeName(int relTypeID)
  {
    IMSRelationType relationType = MetaDataHelper.GetRelationType(relTypeID);
    string relationTypeName = this._mode == ContainsMode.Contains ? relationType.TypeName : relationType.ReverseName;
    if (relationTypeName != relationType.Text)
      relationTypeName = $"{relationTypeName} ({relationType.Text})";
    return relationTypeName;
  }

  private NodeColumnCollection GetColumnsFromScheme(SearchSchemeID scheme)
  {
    NodeColumnCollection columnsFromScheme = new NodeColumnCollection();
    columnsFromScheme.AddRange((IEnumerable<NodeColumn>) this.GetDefaultColumns());
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      object[] attributeValuesByGuid = sessionKeeper.Session.GetObjectAttributeValuesByGuid(scheme.SchemeID, new Guid("cad00620-306c-11d8-b4e9-00304f19f545"));
      if (attributeValuesByGuid != null)
      {
        foreach (object obj in attributeValuesByGuid)
        {
          if (obj != null && obj.ToString() != string.Empty)
          {
            ColumnSchemeAttProxy columnSchemeAttProxy = new ColumnSchemeAttProxy(obj.ToString());
            Guid attributeGuid = columnSchemeAttProxy.AttributeGuid;
            if (attributeGuid != Guid.Empty)
            {
              NodeColumn column = this.CreateColumn(sessionKeeper.Session, service, attributeGuid, columnSchemeAttProxy.AttributeSource, columnSchemeAttProxy.ColumnWidth);
              if (column != null)
                columnsFromScheme.Add(column);
            }
          }
        }
      }
    }
    return columnsFromScheme;
  }

  private NodeColumn CreateColumn(
    IUserSession session,
    IColumnSchemes schemes,
    Guid aGuid,
    AttributeSourceTypes aType,
    int aColumnWidth)
  {
    IDBAttributeType attributeType = session.GetAttributeType(aGuid, false);
    return attributeType != null ? this.CreateColumn(session, schemes, attributeType, aType, aColumnWidth) : (NodeColumn) null;
  }

  private NodeColumn CreateColumn(
    IUserSession session,
    IColumnSchemes schemes,
    IDBAttributeType val,
    AttributeSourceTypes aType,
    int aColumnWidth)
  {
    if (val != null && val.IsGridable && !this.ColumnIsDefault(val.AttributeID))
    {
      Guid schemeGuid = Guid.Empty;
      switch (aType)
      {
        case AttributeSourceTypes.Object:
          schemeGuid = val.AttributeID < 0 ? Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid : Intermech.Navigator.Consts.ObjectColumnSchemeGuid;
          break;
        case AttributeSourceTypes.Relation:
          schemeGuid = val.AttributeID < 0 ? Intermech.Navigator.Consts.RelationObligatoryColumnSchemeGuid : Intermech.Navigator.Consts.RelationColumnSchemeGuid;
          break;
      }
      object columnID = val.AttributeID >= 0 ? (object) val.AttributeID : (object) (ObligatoryObjectAttributes) val.AttributeID;
      if (schemeGuid != Guid.Empty && columnID != null)
      {
        NodeColumn column = schemes.CreateColumn(schemeGuid, columnID);
        column.Width = aColumnWidth;
        return column;
      }
    }
    return (NodeColumn) null;
  }

  private NodeColumnCollection GetDefaultColumns()
  {
    NodeColumnCollection defaultColumns = new NodeColumnCollection();
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_TYPE));
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_ID));
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_ID));
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.CAPTION));
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_CHKOUT_BY));
    return defaultColumns;
  }

  private bool ColumnIsDefault(int AttributeID)
  {
    return AttributeID == -7 || AttributeID == -2 || AttributeID == -3 || AttributeID == -50 || AttributeID == -6;
  }

  private void SetColumns(bool fromConfig)
  {
    ContainsNode node = (ContainsNode) this.Node;
    if (this.CurrentScheme != null)
    {
      node.Scheme = this.CurrentScheme;
      node.Columns = this.GetColumnsFromScheme(node.Scheme);
    }
    else
      node.Columns = this.GetDefaultColumns();
    if (node.Reader != null && node.Reader.QueryResult != null)
      node.Reader.QueryResult = (DataTable) null;
    this.ClearData();
    this.SetPersent(string.Empty, 0);
    this._reloadFromSelf = true;
    try
    {
      NodeColumnCollection columns = (NodeColumnCollection) null;
      if (fromConfig)
      {
        NavigatorColumns navigatorColumns = this._navigatorColumnsService.GetNavigatorColumns(this.StateStreamCategoryID, this.StateStreamCategoryType, this.StateStreamPrefix, this.UseInheritedNavViews);
        if (navigatorColumns != null && !navigatorColumns.Empty)
          columns = navigatorColumns.Columns.Clone() as NodeColumnCollection;
      }
      if (columns == null)
        columns = node.Columns;
      this.SetColumns(columns, false);
    }
    finally
    {
      this._reloadFromSelf = false;
    }
  }

  private void RefreshSchemes(long positionScheme)
  {
    this.cbiScheme.ComboBox.SelectedValueChanged -= new EventHandler(this.ComboBox_SelectedValueChanged);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      List<Guid> possibleRelationTypes = this.GetPossibleRelationTypes(sessionKeeper.Session, this._objectType);
      this.AddShemes(sessionKeeper.Session, possibleRelationTypes, this._objectType);
    }
    try
    {
      if (this.cbiScheme.Items.Count > 0)
      {
        bool flag = false;
        for (int index = 0; index < this.cbiScheme.Items.Count; ++index)
        {
          if (this.cbiScheme.Items[index] is SearchSchemeID && (this.cbiScheme.Items[index] as SearchSchemeID).SchemeID == positionScheme)
          {
            flag = true;
            this.cbiScheme.ComboBox.SelectedIndex = index;
            ((ContainsNode) this.Node).Scheme = this.cbiScheme.Items[index] as SearchSchemeID;
            break;
          }
        }
        if (flag)
          return;
        this.cbiScheme.ComboBox.SelectedIndex = 0;
      }
      else
        ((ContainsNode) this.Node).Columns = this.GetDefaultColumns();
    }
    finally
    {
      this.cbiScheme.ComboBox.SelectedValueChanged += new EventHandler(this.ComboBox_SelectedValueChanged);
    }
  }

  private void CircleMethod()
  {
    try
    {
      string[] strArray = new string[4]
      {
        "|",
        "/",
        "-",
        "\\"
      };
      int index = 0;
      while (true)
      {
        this._statusStrip.Invoke((Delegate) new ContainsViewBase.SetCircleHandler(this.SetCircle), (object) strArray[index]);
        if (index == 3)
          index = 0;
        else
          ++index;
        Thread.Sleep(500);
      }
    }
    catch
    {
    }
  }

  private void StartQuery_Click(object sender, EventArgs e)
  {
    ContainsNode node = (ContainsNode) this.Node;
    if (!this._queryBegining)
    {
      if (this.CurrentScheme != null)
        node.Scheme = this.CurrentScheme;
      node.RealQuery = true;
      node.Reader = this._reader;
      using (FixEditingContext fixEditingContext = new FixEditingContext())
      {
        this._circleThread = new Thread(fixEditingContext.SendEditingContextToThread(new ThreadStart(this.CircleMethod)))
        {
          Name = $"ContainsCircleThread_{Guid.NewGuid()}",
          IsBackground = true
        };
        this._circleThread.Start();
        this.ReloadItems();
      }
    }
    else
    {
      if (this._reader != null)
        this._reader.Cancel();
      if (this._circleThread != null)
      {
        this._circleThread.Abort();
        this._circleThread.Join();
        this.SetCircle(string.Empty);
        this._circleThread = (Thread) null;
      }
      this._reader.State = BackgroundState.Empty;
    }
  }

  private void DefaultColumns_Click(object sender, EventArgs e)
  {
    bool flag = this._reader.QueryResult != null && this._reader.QueryResult.Rows.Count > 0;
    if (flag && MessageBox.Show("Внимание! Будут перечитаны результаты поиска! Продолжить?", "Восстановление схемы колонок по умолчанию", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
      return;
    this.SetColumns(false);
    this.SaveColumnsToConfig();
    if (!flag)
      return;
    this.StartQuery_Click(sender, e);
  }

  private void NewScheme_Click(object sender, EventArgs e)
  {
    SearchSchemeEditor.EditorParams = new SchemeEditorParams(this._mode);
    ContainsNode node = (ContainsNode) this.Node;
    node.RealQuery = false;
    try
    {
      long objectByTypeDialog = (ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService).CreateObjectByTypeDialog(new Guid[2]
      {
        new Guid("cad0012b-306c-11d8-b4e9-00304f19f545"),
        new Guid("cad0012a-306c-11d8-b4e9-00304f19f545")
      });
      switch (objectByTypeDialog)
      {
        case -1:
          break;
        case 0:
          break;
        default:
          this.RefreshSchemes(objectByTypeDialog);
          this.SetTypes4Columns();
          ContainsViewBase._searchSchemeChoiceCache.SetSchemeID(this._mode, this._objectType, objectByTypeDialog);
          this.SetColumns(false);
          this.UpdateEnabled();
          break;
      }
    }
    finally
    {
      SearchSchemeEditor.EditorParams = (SchemeEditorParams) null;
      node.RealQuery = true;
    }
  }

  private void RefreshSchemes_Click(object sender, EventArgs e)
  {
    this.RefreshSchemes(this.CurrentSchemeID);
    this.UpdateEnabled();
  }

  private void EditScheme_Click(object sender, EventArgs e)
  {
    SearchSchemeEditor.EditorParams = new SchemeEditorParams(this._mode);
    try
    {
      SearchSchemeEditor searchSchemeEditor = new SearchSchemeEditor()
      {
        ParentMode = 0,
        SchemeID = this.CurrentSchemeID
      };
      searchSchemeEditor.LoadObjectData(0);
      if (searchSchemeEditor.ShowDialog() != DialogResult.OK)
        return;
      this.RefreshSchemes(this.CurrentSchemeID);
      this.SetTypes4Columns();
      if (searchSchemeEditor.IsChangedColumns)
      {
        this.SetColumns(false);
        this.SaveColumnsToConfig();
      }
      this.UpdateEnabled();
    }
    finally
    {
      SearchSchemeEditor.EditorParams = (SchemeEditorParams) null;
    }
  }

  private void SaveColumnsToConfig()
  {
    this.GridSaveState((Stream) null, ((ContainsNode) this.Node).Columns);
  }

  private void UpdateEnabled()
  {
    this.biEditScheme.Enabled = this.CurrentSchemeID > 0L;
    this.biStartQuery.Enabled = this.CurrentSchemeID != 0L;
  }

  private void ComboBox_SelectedValueChanged(object sender, EventArgs e)
  {
    this.SetTypes4Columns();
    this.SetColumns(true);
    ContainsViewBase._searchSchemeChoiceCache.SetSchemeID(this._mode, this._objectType, this.CurrentSchemeID);
    this.UpdateEnabled();
  }

  private void SetTypes4Columns()
  {
    SearchSchemeID currentScheme = this.CurrentScheme;
    if (currentScheme == null)
      return;
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    if (currentScheme is VirtualSearchSchemeID)
    {
      VirtualSearchSchemeID virtualSearchSchemeId = currentScheme as VirtualSearchSchemeID;
      intList1 = virtualSearchSchemeId.RelTypes;
      intList2 = virtualSearchSchemeId.Types;
    }
    else
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(currentScheme.SchemeID, false);
        if (dbObject != null)
        {
          SearchDirection int32_1 = (SearchDirection) Convert.ToInt32(dbObject.GetAttributeByGuid(new Guid("cad00131-306c-11d8-b4e9-00304f19f545")).Value);
          IDBAttribute attributeByGuid1 = dbObject.GetAttributeByGuid(new Guid("cad0014a-306c-11d8-b4e9-00304f19f545"));
          if (attributeByGuid1 != null && attributeByGuid1.ValuesCount > 0)
          {
            for (int index = 0; index < attributeByGuid1.ValuesCount; ++index)
            {
              if (CompareValuesHelper.NormalizedValue(attributeByGuid1.Values[index]) != null && GuidHelper.IsGuid(attributeByGuid1.Values[index].ToString()))
              {
                int relationTypeId = MetaDataHelper.GetRelationTypeID(new Guid(attributeByGuid1.Values[index].ToString()));
                if (relationTypeId != -1)
                  intList1.Add(relationTypeId);
              }
            }
          }
          IDBAttribute attributeByGuid2 = dbObject.GetAttributeByGuid(new Guid("cad00149-306c-11d8-b4e9-00304f19f545"));
          if (attributeByGuid2 != null && attributeByGuid2.ValuesCount > 0)
          {
            for (int index = 0; index < attributeByGuid2.ValuesCount; ++index)
            {
              if (CompareValuesHelper.NormalizedValue(attributeByGuid2.Values[index]) != null && GuidHelper.IsGuid(attributeByGuid2.Values[index].ToString()))
              {
                int objectTypeId = MetaDataHelper.GetObjectTypeID(new Guid(attributeByGuid2.Values[index].ToString()));
                if (objectTypeId != -1)
                  intList2.Add(objectTypeId);
              }
            }
          }
          if (int32_1 == SearchDirection.Contains && this._objectType != -1)
          {
            DataTable applicabilitiesList = sessionKeeper.Session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(-1, -1, this._objectType);
            if (applicabilitiesList.Rows.Count > 0)
            {
              for (int index = 0; index < applicabilitiesList.Rows.Count; ++index)
              {
                int int32_2 = Convert.ToInt32(applicabilitiesList.Rows[index]["F_RELATION_TYPE"]);
                if (intList1.IndexOf(int32_2) < 0)
                  intList1.Add(int32_2);
              }
            }
          }
          if (int32_1 == SearchDirection.EntersTo)
          {
            DataTable applicabilitiesList = sessionKeeper.Session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(-1, this._objectType, -1);
            if (applicabilitiesList.Rows.Count > 0)
            {
              for (int index = 0; index < applicabilitiesList.Rows.Count; ++index)
              {
                int int32_3 = Convert.ToInt32(applicabilitiesList.Rows[index]["F_INOBJECT_TYPE"]);
                int int32_4 = Convert.ToInt32(applicabilitiesList.Rows[index]["F_RELATION_TYPE"]);
                if (intList2.Count > 0)
                {
                  if (intList2.IndexOf(int32_3) >= 0)
                    intList1.Add(int32_4);
                }
                else if (intList1.IndexOf(int32_4) < 0)
                  intList1.Add(int32_4);
              }
            }
          }
        }
      }
    }
    this._types4attributes = new ContainsViewBase.Types4Atributes(intList2.ToArray(), intList1.ToArray());
    ContainsNode node = (ContainsNode) this.Node;
    if (node == null)
      return;
    if (intList2.Count > 0)
      node.SchemeObjectTypes = intList2.ToArray();
    if (intList1.Count <= 0)
      return;
    node.SchemeRelationTypes = intList1.ToArray();
  }

  protected override DialogResult ExecuteAppearanceTuningForm(
    INode node,
    ContentType content,
    NodeColumnCollection supportedColumns,
    NodeColumnCollection columns,
    params object[] nodeIDs)
  {
    ContainsAppearanceTuningForm appearanceTuningForm = new ContainsAppearanceTuningForm(node, content, supportedColumns, columns, true, this._types4attributes.ObjectTypeIDs, this._types4attributes.RelationTypeIDs, nodeIDs);
    try
    {
      return appearanceTuningForm.ShowDialog();
    }
    finally
    {
      NodeColumnCollection.CorrectSortIndex(columns);
    }
  }

  protected override IDescriptor GetEmptyPathDescriptor()
  {
    return (IDescriptor) new HiveDescriptor(PDMPluginConsts.CategoryContains, 0, "ContainsNodeDescriptor");
  }

  public override void SetColumnsCommand(
    ISelectedItems selectedItems,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ContainsNode node = (ContainsNode) this.Node;
    base.SetColumnsCommand(selectedItems, viewServices, additionalInfo);
  }

  protected override void RefreshViewCommand(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    ContainsNode node = (ContainsNode) this.Node;
    base.RefreshViewCommand(items, viewServices, additionalInfo);
  }

  public override INodeQuery NodeQuery => ((ContainsNode) this.Node).GetReportQuery();

  protected override bool Eof => true;

  public override void ResetColumnsCommand(
    ISelectedItems selectedItems,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (MessageBox.Show(LocalizationHolder.rm.GetString("Pdm_525"), LocalizationHolder.rm.GetString("Pdm_526"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
      return;
    this.SetColumns(false);
    this.SaveColumnsToConfig();
  }

  public override string StateStreamPrefix => $"{this._mode}_{this.CurrentSchemeID}";

  protected override void Dispose(bool disposing)
  {
    if (this._queryBegining)
    {
      if (this._reader != null)
        this._reader.Cancel();
      if (this._queryThread != null)
        this._queryThread.Abort();
      if (this._circleThread != null)
        this._circleThread.Abort();
    }
    if (this.cbiScheme != null)
      this.cbiScheme.ComboBox.SelectedValueChanged -= new EventHandler(this.ComboBox_SelectedValueChanged);
    if (this._reader != null)
    {
      BackgroundReader reader = this._reader;
      reader.StateChangedEvent = reader.StateChangedEvent - new StateChanged(this.ReaderStateChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ContainsViewBase));
    this.biStartQuery = new ButtonItem();
    this.tsslSelectState = new ToolStripStatusLabel();
    this.ttslCircle = new ToolStripStatusLabel();
    this.cbiScheme = new ComboBoxItem();
    this.biDefaultColumns = new ButtonItem();
    this.biNewScheme = new ButtonItem();
    this.biEditScheme = new ButtonItem();
    this.biRefreshSchemes = new ButtonItem();
    this.barManager1 = new BarManager();
    this.topBarDock = new ToolBarContainer();
    ((ISupportInitialize) this._grid).BeginInit();
    ((ISupportInitialize) this._pictureBox).BeginInit();
    this.SuspendLayout();
    this._toolBar.FlipLastItem = true;
    this._toolBar.Guid = new Guid("d17b853b-f24b-4626-a798-5910dfa2afc4");
    this._toolBar.Items.AddRange(new ToolbarItemBase[6]
    {
      (ToolbarItemBase) this.biDefaultColumns,
      (ToolbarItemBase) this.biNewScheme,
      (ToolbarItemBase) this.biEditScheme,
      (ToolbarItemBase) this.biStartQuery,
      (ToolbarItemBase) this.cbiScheme,
      (ToolbarItemBase) this.biRefreshSchemes
    });
    componentResourceManager.ApplyResources((object) this._toolBar, "_toolBar");
    this._grid.DefaultAutoGroupRow.Height = 21;
    this._grid.FrozenArea.ColCount = 1;
    this._grid.FrozenArea.SortFrozenRows = true;
    this._grid.GroupBox.BackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintForeColor = SystemColors.ControlText;
    this._grid.GroupBox.Text = componentResourceManager.GetString("_grid.GroupBox.Text");
    this._grid.GroupBox.Visible = true;
    this._grid.Header.AutoHeightFlags = iGHdrAutoHeightFlags.OnAddCol | iGHdrAutoHeightFlags.OnRemoveCol | iGHdrAutoHeightFlags.OnShowCol | iGHdrAutoHeightFlags.OnContentsChange | iGHdrAutoHeightFlags.OnThemeChange | iGHdrAutoHeightFlags.OnResizeCol;
    this._grid.Header.Height = (int) componentResourceManager.GetObject("_grid.Header.Height");
    this._grid.LayoutObject.Flags = iGLayoutFlags.Grouping | iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    componentResourceManager.ApplyResources((object) this._grid, "_grid");
    componentResourceManager.ApplyResources((object) this._pageViewsManager, "_pageViewsManager");
    this._filtersComboBoxItem.Padding.Bottom = 0;
    this._filtersComboBoxItem.Padding.Left = 1;
    this._filtersComboBoxItem.Padding.Right = 1;
    this._filtersComboBoxItem.Padding.Top = 0;
    this.buttonHeightSet.Padding.Bottom = 0;
    this.buttonHeightSet.Padding.Left = 0;
    this.buttonHeightSet.Padding.Right = 0;
    this.buttonHeightSet.Padding.Top = 0;
    this.biStartQuery.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.biStartQuery, "biStartQuery");
    this.biStartQuery.Click += new EventHandler(this.StartQuery_Click);
    componentResourceManager.ApplyResources((object) this.tsslSelectState, "tsslSelectState");
    this.tsslSelectState.Name = "tsslSelectState";
    this.ttslCircle.Name = "ttslCircle";
    componentResourceManager.ApplyResources((object) this.ttslCircle, "ttslCircle");
    this.cbiScheme.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.cbiScheme, "cbiScheme");
    this.cbiScheme.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbiScheme.MinimumControlWidth = 50;
    this.cbiScheme.Padding.Bottom = 0;
    this.cbiScheme.Padding.Left = 1;
    this.cbiScheme.Padding.Right = 1;
    this.cbiScheme.Padding.Top = 0;
    this.cbiScheme.Stretch = true;
    this.biDefaultColumns.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.biDefaultColumns, "biDefaultColumns");
    this.biDefaultColumns.Click += new EventHandler(this.DefaultColumns_Click);
    this.biNewScheme.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.biNewScheme, "biNewScheme");
    this.biNewScheme.Click += new EventHandler(this.NewScheme_Click);
    componentResourceManager.ApplyResources((object) this.biEditScheme, "biEditScheme");
    this.biEditScheme.Click += new EventHandler(this.EditScheme_Click);
    componentResourceManager.ApplyResources((object) this.biRefreshSchemes, "biRefreshSchemes");
    this.biRefreshSchemes.Click += new EventHandler(this.RefreshSchemes_Click);
    this.barManager1.OwnerForm = (Form) null;
    componentResourceManager.ApplyResources((object) this.topBarDock, "topBarDock");
    this.topBarDock.Guid = new Guid("34b9835d-4a21-4b29-a375-29b538e6b0fc");
    this.topBarDock.Manager = this.barManager1;
    this.topBarDock.Name = "topBarDock";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this.topBarDock);
    this.Name = nameof (ContainsViewBase);
    this.Tag = (object) " ";
    this.Controls.SetChildIndex((System.Windows.Forms.Control) this._gridHeaderMenuBar, 0);
    this.Controls.SetChildIndex((System.Windows.Forms.Control) this._pictureBox, 0);
    this.Controls.SetChildIndex((System.Windows.Forms.Control) this.topBarDock, 0);
    this.Controls.SetChildIndex((System.Windows.Forms.Control) this._toolBar, 0);
    this.Controls.SetChildIndex((System.Windows.Forms.Control) this._grid, 0);
    ((ISupportInitialize) this._grid).EndInit();
    ((ISupportInitialize) this._pictureBox).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private class SearchSchemeChoiceCache
  {
    private Dictionary<ContainsMode, Dictionary<int, long>> _cache;

    public SearchSchemeChoiceCache()
    {
      this._cache = new Dictionary<ContainsMode, Dictionary<int, long>>(2)
      {
        {
          ContainsMode.Applicability,
          (Dictionary<int, long>) null
        },
        {
          ContainsMode.Contains,
          (Dictionary<int, long>) null
        }
      };
    }

    public long GetSchemeID(ContainsMode mode, int objectTypeID)
    {
      if (this._cache[mode] == null)
        return 0;
      long schemeId = 0;
      this._cache[mode].TryGetValue(objectTypeID, out schemeId);
      return schemeId;
    }

    public void SetSchemeID(ContainsMode mode, int objectTypeID, long schemeID)
    {
      if (this._cache[mode] == null)
        this._cache[mode] = new Dictionary<int, long>(10);
      if (!this._cache[mode].ContainsKey(objectTypeID))
        this._cache[mode].Add(objectTypeID, schemeID);
      else
        this._cache[mode][objectTypeID] = schemeID;
    }
  }

  private class Types4Atributes
  {
    public Types4Atributes(int[] objectTypeIDs, int[] relationTypeIDs)
    {
      this.ObjectTypeIDs = objectTypeIDs;
      this.RelationTypeIDs = relationTypeIDs;
    }

    public int[] ObjectTypeIDs { get; }

    public int[] RelationTypeIDs { get; }
  }

  private delegate void SetEnabledHandler(System.Windows.Forms.Control sender, bool enable);

  private delegate void SetButtonItemHandler(bool start);

  private delegate void SetPersentHandler(string text, int persent);

  private delegate void SetCircleHandler(string text);
}
