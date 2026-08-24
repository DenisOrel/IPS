// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.StatisticsMainForm
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Docking;
using Intermech.Extensions;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Statistics.Configurations;
using Intermech.Statistics.Controls;
using Intermech.Statistics.Interfaces;
using Intermech.Statistics.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml.Serialization;

#nullable disable
namespace Intermech.Statistics;

public class StatisticsMainForm : DockControl, IGuid
{
  private IDefaultCommands4ObjTypes _defaultCommands4ObjTypes;
  private ServiceContainer _services;
  private IIODispatcher _ioDispatcher = (IIODispatcher) new IODispatcher();
  private IStatisticsService _statisticsService;
  private IStatisticsClientService _statisticsClientService;
  private bool _canContinueBuildChart;
  private BarManager _barManager;
  private readonly MenuButtonItem _createStatObjectStrip = new MenuButtonItem();
  private readonly MenuButtonItem _deleteStatObjectStrip = new MenuButtonItem();
  private readonly MenuButtonItem _printStatObjectStrip = new MenuButtonItem();
  private readonly MenuButtonItem _createReportStatObjectStrip = new MenuButtonItem();
  private readonly MenuButtonItem _showChartStrip = new MenuButtonItem();
  private readonly MenuButtonItem _enableStatisticsStrip = new MenuButtonItem();
  private readonly MenuButtonItem _pauseStatisticsStrip = new MenuButtonItem();
  private readonly MenuButtonItem _refreshStrip = new MenuButtonItem();
  private readonly MenuBar _bar = new MenuBar();
  private readonly ContextMenuBarItem _allMenu = new ContextMenuBarItem();
  private readonly MenuButtonItem _contextMenuStrip = new MenuButtonItem("Команды навигатора");
  private INotificationService _notificationService;
  private bool _modify;
  private readonly List<StatisticNodeItem> _newStatisticsItems = new List<StatisticNodeItem>();
  private bool _canShowWaitingPage;
  private string filtrationOwnerID = string.Empty;
  private bool _suspend;
  private StatisticNodeItem _oldSelStatisticNode;
  private IContainer components;
  private SplitContainer splitContainer1;
  private ToolStrip toolStrip1;
  private ToolStripButton btnCreateStatObject;
  private ToolStripButton btnDelete;
  private ToolStripButton btnShowChart;
  private ToolStripButton btnPrint;
  private ToolStripButton btnCreateReport;
  private ToolStripButton btnTaskStart;
  private ToolStripButton btnTaskStop;
  private ToolStripButton toolStripButton1;
  private TreeView treeView1;
  private ToolTip toolTip1;
  private ToolTip toolTip2;
  private System.Windows.Forms.TabControl tabControl;
  private System.Windows.Forms.TabPage configuratePage;
  private System.Windows.Forms.TabPage chartPage;
  private System.Windows.Forms.TabPage propertyPage;
  private PropertiesView propertiesView1;
  private ImageList objectsIcons;
  private Panel panelObjects;
  private ListView lvObjects;
  private ColumnHeader columnHeader1;
  private ToolStripButton btnRefresh;
  private ChartDisplayControl _chartDisplayControl1;

  private bool Modify
  {
    get => this._modify;
    set => this._modify = value;
  }

  public StatisticsMainForm()
  {
    this.InitializeComponent();
    this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
    Bitmap statistics = Resources._statistics;
    if (statistics != null)
      this.TabImage = (Image) statistics;
    this.treeView1.HideSelection = false;
    this.treeView1.ImageList = this.objectsIcons;
    this.lvObjects.LargeImageList = this.lvObjects.SmallImageList = this.objectsIcons;
    this.Guid = StatisticsConst.StatisticsDockControlGuid;
    this.InitServices();
    if (ServicesManager.GetService(typeof (DockManager)) is DockManager service)
    {
      service.DockControlActivated += new DockControlEventHandler(this.dm_DockControlActivated);
      service.DockControlDeactivated += new DockControlEventHandler(this.dm_DockControlDeactivated);
    }
    this._refreshStrip.Image = (Image) new Bitmap((Image) Resources.refresh, new Size(16 /*0x10*/, 16 /*0x10*/));
    this._refreshStrip.CommandName = "refreshStrip";
    this._refreshStrip.Text = "Обновить";
    this._refreshStrip.Click += new EventHandler(this.btnRefresh_Click);
    this._createStatObjectStrip.Image = (Image) new Bitmap((Image) Resources.Add16, new Size(16 /*0x10*/, 16 /*0x10*/));
    this._createStatObjectStrip.CommandName = "createStatObjectStrip";
    this._createStatObjectStrip.Text = "Создать объект сбора статистики";
    this._createStatObjectStrip.Click += new EventHandler(this.createStatObject_Click);
    this._deleteStatObjectStrip.Enabled = false;
    this._deleteStatObjectStrip.Image = (Image) new Bitmap((Image) Resources.del, new Size(16 /*0x10*/, 16 /*0x10*/));
    this._deleteStatObjectStrip.CommandName = "deleteStatObjectStrip";
    this._deleteStatObjectStrip.Text = "Удалить";
    this._deleteStatObjectStrip.Click += new EventHandler(this.del_Click);
    this._printStatObjectStrip.Enabled = false;
    this._printStatObjectStrip.Image = (Image) new Bitmap((Image) Resources.print, new Size(16 /*0x10*/, 16 /*0x10*/));
    this._printStatObjectStrip.CommandName = "printStatObjectStrip";
    this._printStatObjectStrip.Text = "Распечатать";
    this._printStatObjectStrip.Click += new EventHandler(this.print_Click);
    this._createReportStatObjectStrip.Visible = true;
    this._createReportStatObjectStrip.Enabled = false;
    this._createReportStatObjectStrip.Image = (Image) new Bitmap((Image) Resources.table_report, new Size(16 /*0x10*/, 16 /*0x10*/));
    this._createReportStatObjectStrip.CommandName = "createReportStatObjectStrip";
    this._createReportStatObjectStrip.Text = "Сформировать отчет";
    this._createReportStatObjectStrip.Click += new EventHandler(this.СreateReport_Click);
    this._showChartStrip.Enabled = false;
    this._showChartStrip.Image = (Image) new Bitmap((Image) Resources.chart, new Size(16 /*0x10*/, 16 /*0x10*/));
    this._showChartStrip.CommandName = "showChartStrip";
    this._showChartStrip.Text = "Показать график";
    this._showChartStrip.Click += new EventHandler(this.showChart_Click);
    this._enableStatisticsStrip.Enabled = false;
    this._enableStatisticsStrip.Image = (Image) new Bitmap((Image) Resources.play, new Size(16 /*0x10*/, 16 /*0x10*/));
    this._enableStatisticsStrip.CommandName = "enableStatisticsStrip";
    this._enableStatisticsStrip.Text = "Включить сбор статистики";
    this._enableStatisticsStrip.Click += new EventHandler(this.taskStart_Click);
    this._pauseStatisticsStrip.Enabled = false;
    this._pauseStatisticsStrip.Image = (Image) new Bitmap((Image) Resources.pause, new Size(16 /*0x10*/, 16 /*0x10*/));
    this._pauseStatisticsStrip.CommandName = "pauseStatisticsStrip";
    this._pauseStatisticsStrip.Text = "Приостановить сбор статистики";
    this._pauseStatisticsStrip.Click += new EventHandler(this.taskStop_Click);
    this._allMenu.Text = "Команды статистики";
    this._allMenu.Items.Add((ToolbarItemBase) this._refreshStrip);
    this._allMenu.Items.Add((ToolbarItemBase) this._createStatObjectStrip);
    this._allMenu.Items.Add((ToolbarItemBase) this._deleteStatObjectStrip);
    this._allMenu.Items.Add((ToolbarItemBase) this._printStatObjectStrip);
    this._allMenu.Items.Add((ToolbarItemBase) this._showChartStrip);
    this._allMenu.Items.Add((ToolbarItemBase) this._createReportStatObjectStrip);
    if (this._barManager != null && this._barManager.MenuBar != null)
      this._bar.ImageList = this._barManager.MenuBar.ImageList;
    this._bar.Items.Add((ToolbarItemBase) this._allMenu);
  }

  private void dm_DockControlDeactivated(object sender, DockControlEventArgs e)
  {
    if (!(sender is DockManager dockManager) || dockManager.ActiveDockControl != this)
      return;
    this._contextMenuStrip.Items.Clear();
  }

  private void dm_DockControlActivated(object sender, DockControlEventArgs e)
  {
    if (!(sender is DockManager dockManager) || dockManager.ActiveDockControl != this)
      return;
    IFiltrationService service = ServiceUtils.GetService<IFiltrationService>((object) ServicesManager.ServiceContainer, true);
    if (service == null)
      return;
    service.FiltrationServiceOwnerID = this.Get_FiltrationOwnerID();
  }

  private void InitServices()
  {
    this._notificationService = ApplicationServices.Container.GetService(typeof (INotificationService)) as INotificationService;
    this._services = new ServiceContainer();
    this._defaultCommands4ObjTypes = ApplicationServices.Container.GetService(typeof (IDefaultCommands4ObjTypes)) as IDefaultCommands4ObjTypes;
    this._services.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.NodeInTree));
    this._services.AddService(typeof (IDefaultCommands4ObjTypes), (object) this._defaultCommands4ObjTypes);
    this._services.AddService(typeof (IIODispatcher), (object) this._ioDispatcher);
    IFiltrationService service = ServiceUtils.GetService<IFiltrationService>((object) ApplicationServices.Container, true);
    if (service != null)
    {
      service.FiltrationServiceOwnerID = this.Get_FiltrationOwnerID();
      this._services.AddService(typeof (IFiltrationService), (object) service);
    }
    this._services.AddService(typeof (ICurrentUserAndRole), ApplicationServices.Container.GetService(typeof (ICurrentUserAndRole)));
    this._services.AddService(typeof (INotificationService), (object) this._notificationService);
    this.EnableNotifications(this._notificationService, true);
    this._barManager = (BarManager) ApplicationServices.Container.GetService(typeof (BarManager));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._statisticsService = (IStatisticsService) sessionKeeper.Session.GetCustomService(typeof (IStatisticsService));
    this._statisticsClientService = ApplicationServices.Container.GetService(typeof (IStatisticsClientService)) as IStatisticsClientService;
  }

  protected virtual void EnableNotifications(INotificationService notification, bool enabled)
  {
    if (!(notification is SwitchedNotificationService notificationService))
      return;
    notificationService.Enabled = enabled;
  }

  private void StatisticsObjectForm_Load(object sender, EventArgs e)
  {
    this.LoadAllStatisticObjects();
    this.LoadOftenUsedStatisticObjects();
    this.ReloadOftenUsedStatisticObjects += new StatisticsMainForm.ChangedDelegat(this.LoadOftenUsedStatisticObjects);
  }

  private void StatisticsObjectForm_Closing(object sender, CancelEventArgs e)
  {
    this.ReloadOftenUsedStatisticObjects -= new StatisticsMainForm.ChangedDelegat(this.LoadOftenUsedStatisticObjects);
    this._bar.Dispose();
    this._allMenu.Dispose();
    this._contextMenuStrip.Dispose();
    this._createStatObjectStrip.Dispose();
    this._deleteStatObjectStrip.Dispose();
    this._printStatObjectStrip.Dispose();
    this._createReportStatObjectStrip.Dispose();
    this._showChartStrip.Dispose();
    this._enableStatisticsStrip.Dispose();
    this._pauseStatisticsStrip.Dispose();
    this._refreshStrip.Dispose();
    this._services.RemoveService(typeof (IDefaultCommands4ObjTypes));
    this._services.RemoveService(typeof (IViewState));
    this._services.RemoveService(typeof (IIODispatcher));
    this._services.RemoveService(typeof (IFiltrationService));
    this._services.RemoveService(typeof (ICurrentUserAndRole));
    this._services.RemoveService(typeof (INotificationService));
    this._notificationService = (INotificationService) null;
    this._barManager = (BarManager) null;
    this._ioDispatcher = (IIODispatcher) null;
    this._defaultCommands4ObjTypes = (IDefaultCommands4ObjTypes) null;
    if (!(ServicesManager.GetService(typeof (DockManager)) is DockManager service))
      return;
    service.DockControlActivated -= new DockControlEventHandler(this.dm_DockControlActivated);
    service.DockControlDeactivated -= new DockControlEventHandler(this.dm_DockControlDeactivated);
  }

  private event StatisticsMainForm.ChangedDelegat ReloadOftenUsedStatisticObjects;

  private void LoadOftenUsedStatisticObjects()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      string currentUserName = sessionKeeper.Session.UserName;
      Dictionary<long, int> source = new Dictionary<long, int>();
      foreach (StatisticNodeItem newStatisticsItem in this._newStatisticsItems)
      {
        IDBAttribute objectAttribute = sessionKeeper.Session.GetObjectAttribute(newStatisticsItem.ObjectID, (object) new Guid(StatisticsConst.OftenUsed), false, false);
        MemoryStream serializationStream = new MemoryStream();
        MemoryStream aDestStream = serializationStream;
        new BlobProcReader(objectAttribute, 0, (Stream) aDestStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData(sessionKeeper.Session);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (serializationStream.Length > 0L)
        {
          serializationStream.Position = 0L;
          if (binaryFormatter.Deserialize((Stream) serializationStream) is UsersLaunchesCount usersLaunchesCount)
          {
            UsersInfo usersInfo = usersLaunchesCount.UsersInfo.Find((Predicate<UsersInfo>) (x => x.UserName == currentUserName));
            if (usersInfo != null)
              source.Add(newStatisticsItem.ObjectID, usersInfo.LaunchCount);
          }
        }
      }
      List<long> list = (source.Count > 5 ? source.OrderByDescending<KeyValuePair<long, int>, int>((System.Func<KeyValuePair<long, int>, int>) (x => x.Value)).Take<KeyValuePair<long, int>>(5).ToDictionary<KeyValuePair<long, int>, long, int>((System.Func<KeyValuePair<long, int>, long>) (x => x.Key), (System.Func<KeyValuePair<long, int>, int>) (y => y.Value)) : source.OrderByDescending<KeyValuePair<long, int>, int>((System.Func<KeyValuePair<long, int>, int>) (x => x.Value)).ToDictionary<KeyValuePair<long, int>, long, int>((System.Func<KeyValuePair<long, int>, long>) (x => x.Key), (System.Func<KeyValuePair<long, int>, int>) (y => y.Value))).Keys.ToList<long>();
      Dictionary<long, CommandStatisticsTypesEnum> dictionary = new Dictionary<long, CommandStatisticsTypesEnum>();
      foreach (long num in list)
      {
        foreach (StatisticNodeItem newStatisticsItem in this._newStatisticsItems)
        {
          if (newStatisticsItem.ObjectID == num)
            dictionary.Add(newStatisticsItem.ObjectID, newStatisticsItem.CommandType);
        }
      }
      this.treeView1.SuspendDrawing();
      this.treeView1.Nodes[0].Nodes.Clear();
      foreach (KeyValuePair<long, CommandStatisticsTypesEnum> keyValuePair in dictionary)
      {
        int iconIndex = this.GetIconIndex(keyValuePair.Value);
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(keyValuePair.Key);
        StatisticNodeItem statisticNodeItem = new StatisticNodeItem(objectInfo.Caption, objectInfo.ObjectTypeID, objectInfo.ObjectID, objectInfo.ID);
        this.treeView1.Nodes[0].Nodes.Add(objectInfo.VersionGuid.ToString(), objectInfo.Caption, iconIndex, iconIndex).Tag = (object) statisticNodeItem;
      }
      this.treeView1.Nodes[0].Expand();
      this.treeView1.ResumeDrawing();
    }
  }

  private void LoadAllStatisticObjects()
  {
    this.treeView1.Nodes[1].Nodes.Clear();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(StatisticsConst.StatisticsObjectsTypeID);
      List<ConditionStructure> conditionStructureList = new List<ConditionStructure>()
      {
        new ConditionStructure(-7, RelationalOperators.Equal, (object) StatisticsConst.StatisticsCommandTypeID, LogicalOperators.NONE, 0, false)
      };
      if (!sessionKeeper.Session.Configurations.ReadBool(StatisticsConst.ModuleName, StatisticsConst.SETTINGS, StatisticsConst.CANSHOWALLOBJECTS, false, DBConfigMode.UserOnly))
        conditionStructureList.Add(new ConditionStructure(-81, RelationalOperators.Equal, (object) sessionKeeper.Session.UserID, LogicalOperators.NONE, 0, false));
      ColumnDescriptor[] columns = new ColumnDescriptor[6]
      {
        new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -3, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -50, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.ASC, 0),
        new ColumnDescriptor((object) -7, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) MetaDataHelper.GetAttributeID((object) new Guid(StatisticsConst.CollectMethod)), AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -12, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0)
      };
      DBRecordSetParams paramSet = new DBRecordSetParams(conditionStructureList.ToArray(), columns);
      DataTable dataTable = objectCollection.Select(paramSet);
      this._newStatisticsItems.Clear();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        this._newStatisticsItems.Add(new StatisticNodeItem((long) row.ItemArray[0], (long) row.ItemArray[1], (string) row.ItemArray[2], (int) row.ItemArray[3], row.ItemArray[4].ToString().ToEnum<CommandStatisticsTypesEnum>(CommandStatisticsTypesEnum.None), ((Guid) row.ItemArray[5]).ToString()));
      this.treeView1.SuspendLayout();
      this.treeView1.Nodes[1].Nodes.Clear();
      foreach (StatisticNodeItem newStatisticsItem in this._newStatisticsItems)
      {
        int iconIndex = this.GetIconIndex(newStatisticsItem.CommandType);
        this.treeView1.Nodes[1].Nodes.Add(new TreeNode(newStatisticsItem.Caption, iconIndex, iconIndex)
        {
          Tag = (object) newStatisticsItem,
          Name = newStatisticsItem.StatObjectGuid
        });
      }
      this.treeView1.ResumeLayout();
    }
  }

  private int GetIconIndex(CommandStatisticsTypesEnum commandType)
  {
    int iconIndex;
    switch (commandType)
    {
      case CommandStatisticsTypesEnum.CreatedDate:
        iconIndex = this.objectsIcons.Images.IndexOfKey("create_object");
        break;
      case CommandStatisticsTypesEnum.SignDate:
        iconIndex = this.objectsIcons.Images.IndexOfKey("sign_object");
        break;
      case CommandStatisticsTypesEnum.LCStepDate:
        iconIndex = this.objectsIcons.Images.IndexOfKey("lc_step");
        break;
      case CommandStatisticsTypesEnum.LCLevelDate:
        iconIndex = this.objectsIcons.Images.IndexOfKey("lc_level");
        break;
      case CommandStatisticsTypesEnum.DateAttrValue:
        iconIndex = this.objectsIcons.Images.IndexOfKey("date");
        break;
      case CommandStatisticsTypesEnum.ProcessTemplate:
        iconIndex = this.objectsIcons.Images.IndexOfKey("process_template");
        break;
      case CommandStatisticsTypesEnum.TimeInTask:
        iconIndex = this.objectsIcons.Images.IndexOfKey("time_in_task");
        break;
      case CommandStatisticsTypesEnum.TimeOneTaskFormUsers:
        iconIndex = this.objectsIcons.Images.IndexOfKey("one_task_user");
        break;
      case CommandStatisticsTypesEnum.RevertCountTask:
        iconIndex = this.objectsIcons.Images.IndexOfKey("revert_count");
        break;
      default:
        iconIndex = -1;
        break;
    }
    return iconIndex;
  }

  private void createStatObject_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(ServicesManager.GetService(typeof (IObjectCreatorService)) is IObjectCreatorService service))
        return;
      long objectByTypeDialog = service.CreateObjectByTypeDialog(StatisticsConst.StatisticsCommandTypeID);
      if (objectByTypeDialog == -1L)
        return;
      CommandStatisticsTypesEnum commandType = this._statisticsClientService.ReadStatisticsCommandType(sessionKeeper.Session, objectByTypeDialog);
      int iconIndex = this.GetIconIndex(commandType);
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(objectByTypeDialog);
      StatisticNodeItem statisticNodeItem = new StatisticNodeItem(objectInfo.ObjectID, objectInfo.ID, objectInfo.Caption, objectInfo.ObjectTypeID, commandType, objectInfo.VersionGuid.ToString());
      this._newStatisticsItems.Add(statisticNodeItem);
      this._newStatisticsItems.Sort();
      TreeNode treeNode = this.treeView1.Nodes[1].Nodes.Insert(this._newStatisticsItems.IndexOf(statisticNodeItem) - 1, statisticNodeItem.StatObjectGuid, statisticNodeItem.Caption, iconIndex, iconIndex);
      treeNode.Tag = (object) statisticNodeItem;
      this.treeView1.SelectedNode = treeNode;
      if (this._notificationService == null)
        return;
      this._notificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", objectByTypeDialog, false));
    }
  }

  private void del_Click(object sender, EventArgs e)
  {
    if (MessageBox.Show((IWin32Window) this, "Вы действительно хотите удалить выбранный объект?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    List<StatisticNodeItem> collection1 = new List<StatisticNodeItem>();
    List<string> collection2 = new List<string>();
    if (this.lvObjects.Visible)
    {
      foreach (object selectedItem in this.lvObjects.SelectedItems)
      {
        if (((ListViewItem) selectedItem).Tag is StatisticNodeItem tag)
        {
          collection1.SafeAdd<StatisticNodeItem>(tag);
          collection2.SafeAdd<string>(((ListViewItem) selectedItem).Name);
        }
      }
    }
    else
    {
      TreeNode selectedNode = this.treeView1.SelectedNode;
      if (selectedNode.Tag is StatisticNodeItem tag)
      {
        collection2.Add(selectedNode.Name);
        collection1.Add(tag);
      }
      this.treeView1.SelectedNode = this.treeView1.Nodes[1];
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (IObjectsDeleteService)) is IObjectsDeleteService customService)
      {
        foreach (StatisticNodeItem statisticNodeItem in collection1)
        {
          StatisticNodeItem obj = statisticNodeItem;
          DeletingObjects deletingObjects = new DeletingObjects()
          {
            {
              0L,
              obj.ID,
              obj.ObjectID,
              true
            }
          };
          customService.Delete(sessionKeeper.Session.SessionGUID, deletingObjects, DeleteObjectsJobMode.AscOnError);
          this._newStatisticsItems.Remove(this._newStatisticsItems.Where<StatisticNodeItem>((System.Func<StatisticNodeItem, bool>) (x => x.ObjectID == obj.ObjectID)).First<StatisticNodeItem>());
        }
        foreach (string key in collection2)
        {
          this.treeView1.Nodes[1].Nodes.RemoveByKey(key);
          this.lvObjects.Items.RemoveByKey(key);
        }
      }
    }
    StatisticsMainForm.ChangedDelegat statisticObjects = this.ReloadOftenUsedStatisticObjects;
    if (statisticObjects == null)
      return;
    statisticObjects();
  }

  private void taskStart_Click(object sender, EventArgs e)
  {
    TreeNode selectedNode = this.treeView1.SelectedNode;
    if (selectedNode == null || !(selectedNode.Tag is StatisticNodeItem tag))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long[] objectIDs = new long[1]{ tag.ObjectID };
      IDBLifecycleStepCollection lifecycleStepCollection = sessionKeeper.Session.GetLifecycleStepCollection(StatisticsConst.StatisticsTasksObjectsTypeID);
      IMSLifeCycleStep lcStep = MetaDataHelper.GetLCStep(new Guid(StatisticsConst.StatisticsObjectsInProcess));
      if (lcStep != null)
        lifecycleStepCollection.SetObjectsLCStep(objectIDs, lcStep.LCStepID);
    }
    this.btnTaskStart.Enabled = this._enableStatisticsStrip.Enabled = false;
    this.btnTaskStop.Enabled = this._pauseStatisticsStrip.Enabled = true;
  }

  private void taskStop_Click(object sender, EventArgs e)
  {
    TreeNode selectedNode = this.treeView1.SelectedNode;
    if (selectedNode == null || !(selectedNode.Tag is StatisticNodeItem tag))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long[] objectIDs = new long[1]{ tag.ObjectID };
      IDBLifecycleStepCollection lifecycleStepCollection = sessionKeeper.Session.GetLifecycleStepCollection(StatisticsConst.StatisticsTasksObjectsTypeID);
      IMSLifeCycleStep lcStep = MetaDataHelper.GetLCStep(new Guid(StatisticsConst.StatisticsObjectsCreated));
      if (lcStep != null)
        lifecycleStepCollection.SetObjectsLCStep(objectIDs, lcStep.LCStepID);
    }
    this.btnTaskStart.Enabled = this._enableStatisticsStrip.Enabled = true;
    this.btnTaskStop.Enabled = this._pauseStatisticsStrip.Enabled = false;
  }

  private void showChart_Click(object sender, EventArgs e)
  {
    this.tabControl.SelectedTab = this.chartPage;
    try
    {
      if (this.lvObjects.Visible)
      {
        if (this.lvObjects.SelectedItems.Count != 1)
          return;
        string name = this.lvObjects.SelectedItems[0].Name;
        foreach (TreeNode node in this.treeView1.Nodes[1].Nodes)
        {
          if (node.Name == name)
          {
            this.treeView1.SelectedNode = node;
            break;
          }
        }
      }
      this._suspend = true;
      TreeNode selectedNode = this.treeView1.SelectedNode;
      if (selectedNode == null)
        return;
      if (!(selectedNode.Tag is StatisticNodeItem tag))
        throw new KernelException("График не построен. Не найден объект статистики.");
      if (tag.ObjectTypeID == StatisticsConst.StatisticsTasksObjectsTypeID)
      {
        this.tabControl.SelectedTab = this.chartPage;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(tag.ObjectID);
          IDBAttribute objectAttributeByGuid = sessionKeeper.Session.GetObjectAttributeByGuid(tag.ObjectID, new Guid(StatisticsConst.OftenUsed));
          IDBAttribute[] attributesByType = dbObject.Attributes.GetAttributesByType(FieldTypes.ftFile);
          if (attributesByType.Length == 0)
            return;
          using (MemoryStream aDestStream = new MemoryStream())
          {
            BlobProcReader blobProcReader = new BlobProcReader(attributesByType[0], 0, (Stream) aDestStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null);
            blobProcReader.ReadData(sessionKeeper.Session);
            long blobId = blobProcReader.BlobInformation.BlobID;
            PublishedFile publishedFile = ClientContext.FileVault.ViewArea.Publish((IList<DBObjectState>) ClientContext.FileVault.DBObjectsInfo.CreateStateListForObjectTree(dbObject.ObjectID, VersionsRuleSources.GetEditorRule())).ObjectFiles.Find((Predicate<PublishedFile>) (viewFile => viewFile.BlobId == blobId));
            if (publishedFile == null)
              throw new KernelException("Файл со статистикой отсутствует, дождитесь когда задача сбора проведет операцию.");
            string name = selectedNode.Parent.Name;
            this.RewriteOftenUsedAttr(objectAttributeByGuid, sessionKeeper.Session.UserName);
            StatisticsMainForm.ChangedDelegat statisticObjects = this.ReloadOftenUsedStatisticObjects;
            if (statisticObjects != null)
              statisticObjects();
            if (name == "mostUsed")
              this.SetSelectedNodeInOftenUsedNode(tag.ObjectID);
            Objects objects = (Objects) new XmlSerializer(typeof (Objects)).Deserialize((TextReader) new StringReader(File.ReadAllText(publishedFile.FullName)));
            if (objects.Records.Count <= 0)
              return;
            CommandSettings commandSettings = this._statisticsService.ReadStatisticObjectsCommandSettings(sessionKeeper.Session.SessionGUID, dbObject.ObjectID);
            if (commandSettings == null)
              throw new KernelException("Задача не сконфигурирована, построение графика невозможно.");
            try
            {
              if (this.GetChartsForTaskObject(objects.Records, commandSettings.CollectPeriod))
                this.btnPrint.Enabled = this._printStatObjectStrip.Enabled = true;
              else
                this._chartDisplayControl1.SetEmptyPage(StatisticsConst.CHART_ABSENCE_MESSAGE);
            }
            catch
            {
              this._chartDisplayControl1.SetEmptyPage(StatisticsConst.CHART_ABSENCE_MESSAGE);
              throw;
            }
          }
        }
      }
      else
      {
        if (tag.ObjectTypeID != StatisticsConst.StatisticsCommandTypeID)
          return;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBAttribute objectAttributeByGuid = sessionKeeper.Session.GetObjectAttributeByGuid(tag.ObjectID, new Guid(StatisticsConst.OftenUsed));
          string name = selectedNode.Parent.Name;
          this.RewriteOftenUsedAttr(objectAttributeByGuid, sessionKeeper.Session.UserName);
          StatisticsMainForm.ChangedDelegat statisticObjects = this.ReloadOftenUsedStatisticObjects;
          if (statisticObjects != null)
            statisticObjects();
          if (name == "mostUsed")
            this.SetSelectedNodeInOftenUsedNode(tag.ObjectID);
        }
        if (this.tabControl.SelectedTab != this.chartPage)
          this.tabControl.SelectedTab = this.chartPage;
        this.CollectAndShowCommandStatistic((object) tag.ObjectID);
        Thread.Sleep(200);
        if (!this._canShowWaitingPage)
          return;
        this._chartDisplayControl1.SetWaitingPage();
      }
    }
    finally
    {
      this._suspend = false;
    }
  }

  private async void CollectAndShowCommandStatistic(object ID)
  {
    long objectID = (long) ID;
    try
    {
      this._canShowWaitingPage = true;
      try
      {
        this._canContinueBuildChart = true;
        CollectedStatistics collectedStatistics = await Task.Run<CollectedStatistics>((Func<Task<CollectedStatistics>>) (async () => await this.CollectStatistics(objectID)));
        if (!this._canContinueBuildChart)
          return;
        this._chartDisplayControl1.OpenChart(collectedStatistics);
      }
      catch
      {
        this._chartDisplayControl1.SetEmptyPage(StatisticsConst.CHART_ABSENCE_MESSAGE);
        throw;
      }
      if (this._chartDisplayControl1.ChartIsDrown)
        this.btnPrint.Enabled = this._printStatObjectStrip.Enabled = true;
      else
        this.btnPrint.Enabled = this._printStatObjectStrip.Enabled = false;
    }
    catch
    {
      this.btnPrint.Enabled = this._printStatObjectStrip.Enabled = false;
      throw;
    }
    finally
    {
      this._canShowWaitingPage = false;
      this._canContinueBuildChart = false;
    }
  }

  private Task<CollectedStatistics> CollectStatistics(long objectID)
  {
    CollectedStatistics result;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      result = this._statisticsService.CollectStatistics(sessionKeeper.Session.SessionGUID, this._statisticsService.ReadStatisticObjectsCommandSettings(sessionKeeper.Session.SessionGUID, objectID) ?? throw new KernelException("Команда не сконфигурирована. Продолжение невозможно."));
    return Task.FromResult<CollectedStatistics>(result);
  }

  private bool GetChartsForTaskObject(
    List<Records> records,
    CollectPeriodsEnum taskSettingCollectPeriod)
  {
    return false;
  }

  private void RewriteOftenUsedAttr(IDBAttribute oftenUsedAttr, string userName)
  {
    try
    {
      MemoryStream memoryStream1 = new MemoryStream();
      BlobProcReader blobProcReader = new BlobProcReader(oftenUsedAttr, 0, (Stream) memoryStream1, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null);
      blobProcReader.ReadData(oftenUsedAttr.Session);
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      if (memoryStream1.Length > 0L)
      {
        memoryStream1.Position = 0L;
        UsersLaunchesCount graph = binaryFormatter.Deserialize((Stream) memoryStream1) as UsersLaunchesCount;
        UsersInfo usersInfo = graph.UsersInfo.FirstOrDefault<UsersInfo>((System.Func<UsersInfo, bool>) (x => x.UserName.StartsWith(userName)));
        int index = graph.UsersInfo.IndexOf(usersInfo);
        if (usersInfo == null)
        {
          using (MemoryStream memoryStream2 = new MemoryStream())
          {
            graph.UsersInfo.Add(new UsersInfo()
            {
              UserName = userName,
              LaunchCount = 1
            });
            binaryFormatter.Serialize((Stream) memoryStream2, (object) graph);
            BlobInformation aBlobInformation = new BlobInformation(memoryStream2.Length, 0L, DateTime.Now, blobProcReader.BlobInformation.FileName, blobProcReader.BlobInformation.ArcMethod, blobProcReader.BlobInformation.Note);
            new BlobProcWriter(oftenUsedAttr, 0, aBlobInformation, (Stream) memoryStream2, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
          }
        }
        else
        {
          using (MemoryStream memoryStream3 = new MemoryStream())
          {
            ++graph.UsersInfo[index].LaunchCount;
            binaryFormatter.Serialize((Stream) memoryStream3, (object) graph);
            BlobInformation aBlobInformation = new BlobInformation(memoryStream3.Length, 0L, DateTime.Now, blobProcReader.BlobInformation.FileName, blobProcReader.BlobInformation.ArcMethod, blobProcReader.BlobInformation.Note);
            new BlobProcWriter(oftenUsedAttr, 0, aBlobInformation, (Stream) memoryStream3, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
          }
        }
      }
      else
      {
        using (MemoryStream memoryStream4 = new MemoryStream())
        {
          UsersLaunchesCount graph = new UsersLaunchesCount()
          {
            UsersInfo = new List<UsersInfo>()
            {
              new UsersInfo() { LaunchCount = 1, UserName = userName }
            }
          };
          binaryFormatter.Serialize((Stream) memoryStream4, (object) graph);
          BlobInformation aBlobInformation = new BlobInformation(memoryStream4.Length, 0L, DateTime.Now, "oftenUsed.txt", ArcMethods.ZLibPacked, string.Empty);
          new BlobProcWriter(oftenUsedAttr, 0, aBlobInformation, (Stream) memoryStream4, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
        }
      }
      memoryStream1.Close();
    }
    catch (Exception ex)
    {
      throw new KernelException(ex.Message, ex.InnerException);
    }
  }

  private void SetSelectedNodeInOftenUsedNode(long objectID)
  {
    foreach (TreeNode node in this.treeView1.Nodes[0].Nodes)
    {
      if (node.Tag is StatisticNodeItem tag && tag.ObjectID == objectID)
      {
        this.treeView1.SelectedNode = node;
        break;
      }
    }
  }

  private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
  {
    TreeNode selectedNode = this.treeView1.SelectedNode;
    if (selectedNode == null || !(selectedNode.Tag is StatisticNodeItem tag) || tag.ObjectTypeID == StatisticsConst.StatisticsObjectsTypeID || !(sender is System.Windows.Forms.TabControl tabControl))
      return;
    this.btnPrint.Enabled = false;
    string name = tabControl.SelectedTab.Name;
    if (name.Equals(this.propertyPage.Name))
    {
      this.propertiesView1.Initialize(Intermech.Navigator.ContextMenu.Services.GetItems(tag.ObjectID), (IServiceProvider) this._services);
      this.propertiesView1.Activate((IView) null);
    }
    else
    {
      if (!name.Equals(this.chartPage.Name))
        return;
      this.showChart_Click(sender, e);
    }
  }

  private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
  {
    if (!e.TabPage.Name.Equals(this.chartPage.Name) || !this.Modify)
      return;
    switch (MessageBox.Show("Сохранить внесённые изменения?", "Внимание", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
    {
      case DialogResult.Cancel:
        e.Cancel = true;
        break;
      case DialogResult.Yes:
        IEnumerator enumerator = this.configuratePage.Controls.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            if (enumerator.Current is IStatisticSettingsForm current)
              current.Save(sender, (EventArgs) e);
          }
          break;
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      case DialogResult.No:
        foreach (object control in (ArrangedElementCollection) this.configuratePage.Controls)
        {
          if (control is IStatisticSettingsForm statisticSettingsForm)
            statisticSettingsForm.InitForm(statisticSettingsForm.Settings);
        }
        this.Modify = false;
        break;
    }
  }

  private void print_Click(object sender, EventArgs e)
  {
    this._chartDisplayControl1.PrintCurrentChart();
  }

  private void СreateReport_Click(object sender, EventArgs e)
  {
    StatisticNodeItem tag;
    if (this.lvObjects.Visible)
    {
      if (this.lvObjects.SelectedItems.Count != 1)
        return;
      tag = this.lvObjects.SelectedItems[0].Tag as StatisticNodeItem;
    }
    else
      tag = this.treeView1.SelectedNode.Tag as StatisticNodeItem;
    if (tag == null)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute objectAttributeByGuid = sessionKeeper.Session.GetObjectAttributeByGuid(tag.ObjectID, new Guid(StatisticsConst.OftenUsed));
      if (this.treeView1.SelectedNode.Parent != null)
      {
        string name = this.treeView1.SelectedNode.Parent.Name;
        this.RewriteOftenUsedAttr(objectAttributeByGuid, sessionKeeper.Session.UserName);
        StatisticsMainForm.ChangedDelegat statisticObjects = this.ReloadOftenUsedStatisticObjects;
        if (statisticObjects != null)
          statisticObjects();
        if (name == "mostUsed")
          this.SetSelectedNodeInOftenUsedNode(tag.ObjectID);
      }
      else
      {
        this.RewriteOftenUsedAttr(objectAttributeByGuid, sessionKeeper.Session.UserName);
        StatisticsMainForm.ChangedDelegat statisticObjects = this.ReloadOftenUsedStatisticObjects;
        if (statisticObjects != null)
          statisticObjects();
      }
    }
    this.CreateReport(tag);
  }

  private void CreateReport(StatisticNodeItem statisticNodeItem)
  {
    CreateReportForm createReportForm = new CreateReportForm();
    createReportForm.Build(statisticNodeItem);
    int num = (int) createReportForm.ShowDialog();
  }

  public Guid GUID => StatisticsConst.StatisticsDockControlGuid;

  protected string Get_FiltrationOwnerID()
  {
    if (this.filtrationOwnerID.Length <= 0)
      this.filtrationOwnerID = Convert.ToString((object) Guid.NewGuid());
    return this.filtrationOwnerID;
  }

  private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
  {
    if (e.Button != MouseButtons.Right)
      return;
    Point mousePosition;
    if (this.Modify)
    {
      this.treeView1.SelectedNode = e.Node;
      mousePosition = Control.MousePosition;
      int x = mousePosition.X;
      mousePosition = Control.MousePosition;
      int y = mousePosition.Y;
      Cursor.Position = new Point(x, y);
    }
    this.treeView1.SelectedNode = e.Node;
    this.SetContextMenuForObject(e.Node.Tag as StatisticNodeItem);
    ContextMenuBarItem allMenu = this._allMenu;
    mousePosition = Control.MousePosition;
    int x1 = mousePosition.X;
    mousePosition = Control.MousePosition;
    int y1 = mousePosition.Y;
    Point position = new Point(x1, y1);
    allMenu.ShowIndependent(position);
  }

  private void SetContextMenuForObject(StatisticNodeItem statisticNodeItem)
  {
    this._contextMenuStrip.Items.Clear();
    if (statisticNodeItem == null)
      return;
    MenuBarItem menu = Intermech.Navigator.ContextMenu.Services.GetMenu(Intermech.Navigator.ContextMenu.Services.GetItems(statisticNodeItem.ObjectID), (IServiceProvider) this._services);
    for (int index = 0; index < menu.Items.Count; ++index)
      this._contextMenuStrip.Items.Add(menu.Items[index].CloneItem());
  }

  private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this._canContinueBuildChart = false;
    if (this._suspend)
      return;
    this.RemovePreviousDataFromTabControlPages();
    if (e.Node == this.treeView1.Nodes[0])
    {
      this.ShowOftenUsedStatisticObjects();
      this.SetMenuButtonsAvailabilityForCommonNodes();
    }
    else if (e.Node == this.treeView1.Nodes[1])
    {
      this.SetMenuButtonsAvailabilityForCommonNodes();
      this.ShowAllStatisticObjects();
    }
    else if (e.Node.Tag is StatisticNodeItem tag)
    {
      this.SetMenuButtonsAvailabilityForStatisticNodeItem(tag.ObjectTypeID);
      this.ShowStatisticNodeItem(tag);
    }
    else
    {
      this.tabControl.Visible = false;
      this.panelObjects.Visible = false;
    }
  }

  private void SetMenuButtonsAvailabilityForCommonNodes()
  {
    this.btnShowChart.Enabled = false;
    this.btnCreateReport.Enabled = false;
    this.btnDelete.Enabled = false;
    this.btnPrint.Enabled = false;
    this._showChartStrip.Enabled = false;
    this._createReportStatObjectStrip.Enabled = false;
    this._deleteStatObjectStrip.Enabled = false;
    this._printStatObjectStrip.Enabled = false;
    this._contextMenuStrip.Enabled = false;
  }

  private void SetMenuButtonsAvailabilityForStatisticNodeItem(int statisticItemObjectTypeId)
  {
    this.btnDelete.Enabled = this._deleteStatObjectStrip.Enabled = true;
    this.btnPrint.Enabled = this._printStatObjectStrip.Enabled = false;
    this.btnShowChart.Enabled = this._showChartStrip.Enabled = true;
    this.btnCreateReport.Enabled = this._createReportStatObjectStrip.Enabled = true;
    this._contextMenuStrip.Enabled = true;
    if (statisticItemObjectTypeId != StatisticsConst.StatisticsCommandTypeID)
      return;
    this.btnTaskStart.Enabled = this._enableStatisticsStrip.Enabled = false;
    this.btnTaskStop.Enabled = this._pauseStatisticsStrip.Enabled = false;
  }

  private void ShowAllStatisticObjects()
  {
    this.panelObjects.Visible = true;
    this.tabControl.Visible = false;
    this.LoadObjectsListViewItems(this.treeView1.Nodes[1]);
  }

  private void ShowOftenUsedStatisticObjects()
  {
    this.panelObjects.Visible = true;
    this.tabControl.Visible = false;
    this.LoadObjectsListViewItems(this.treeView1.Nodes[0]);
  }

  private void LoadObjectsListViewItems(TreeNode currentTreeNode)
  {
    this.lvObjects.BeginUpdate();
    this.lvObjects.Items.Clear();
    foreach (TreeNode node in currentTreeNode.Nodes)
      this.lvObjects.Items.Add(new ListViewItem(node.Text)
      {
        Tag = node.Tag,
        ImageIndex = node.ImageIndex,
        Name = node.Name
      });
    this.lvObjects.EndUpdate();
    this.lvObjects.Refresh();
  }

  private void ShowStatisticNodeItem(StatisticNodeItem statisticNodeItem)
  {
    this.panelObjects.Visible = false;
    this.tabControl.Visible = true;
    int objectTypeId = statisticNodeItem.ObjectTypeID;
    if (objectTypeId == StatisticsConst.StatisticsTasksObjectsTypeID)
    {
      IStatisticSettingsForm taskSettingsForm = this.GetTaskSettingsForm(this.GetCommandSettingsFromObject(statisticNodeItem.ObjectID));
      taskSettingsForm.SetAsControl((Control) this.configuratePage);
      taskSettingsForm.OnApplied += new EventHandler(this.SaveSettingsFromForm);
      taskSettingsForm.OnModified += new EventHandler(this.SetMainFormModify);
      taskSettingsForm.OnCancelModify += new EventHandler(this.CancelModify);
      if (taskSettingsForm is Control control)
      {
        control.Show();
        this.configuratePage.Controls.Add(control);
      }
      this.Modify = false;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(statisticNodeItem.ObjectID);
        int lcStepId1 = MetaDataHelper.GetLCStepID(new Guid(StatisticsConst.StatisticsObjectsCreated));
        int lcStepId2 = MetaDataHelper.GetLCStepID(new Guid(StatisticsConst.StatisticsObjectsInProcess));
        if (dbObject.LCStep == lcStepId1)
        {
          this.btnTaskStart.Enabled = this._enableStatisticsStrip.Enabled = true;
          this.btnTaskStop.Enabled = this._pauseStatisticsStrip.Enabled = false;
        }
        else if (dbObject.LCStep == lcStepId2)
        {
          this.btnTaskStart.Enabled = this._enableStatisticsStrip.Enabled = false;
          this.btnTaskStop.Enabled = this._pauseStatisticsStrip.Enabled = true;
        }
      }
      this.propertiesView1.Show();
      if (!this.tabControl.SelectedTab.Name.Equals(this.propertyPage.Name))
        return;
      this.propertiesView1.Initialize(Intermech.Navigator.ContextMenu.Services.GetItems(statisticNodeItem.ObjectID), (IServiceProvider) this._services);
      this.propertiesView1.Activate((IView) null);
    }
    else if (objectTypeId == StatisticsConst.StatisticsCommandTypeID)
    {
      this.propertiesView1.Show();
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IStatisticSettingsForm commandSettingsForm = this.GetCommandSettingsForm(sessionKeeper.Session.GetObjectAttributeByGuid(statisticNodeItem.ObjectID, new Guid(StatisticsConst.CollectMethod)).Value.ToString().ToEnum<CommandStatisticsTypesEnum>(CommandStatisticsTypesEnum.None), this.GetCommandSettingsFromObject(statisticNodeItem.ObjectID));
        if (commandSettingsForm == null)
          throw new KernelException("Попытка создать форму настройки статистики для несуществующей команды сбора.");
        commandSettingsForm.SetAsControl((Control) this.configuratePage);
        commandSettingsForm.OnApplied += new EventHandler(this.SaveSettingsFromForm);
        commandSettingsForm.OnModified += new EventHandler(this.SetMainFormModify);
        commandSettingsForm.OnCancelModify += new EventHandler(this.CancelModify);
        if (commandSettingsForm is Control control)
        {
          control.Show();
          this.configuratePage.Controls.Add(control);
        }
        this.Modify = false;
      }
      string name = this.tabControl.SelectedTab.Name;
      if (name.Equals(this.propertyPage.Name))
      {
        this.propertiesView1.Initialize(Intermech.Navigator.ContextMenu.Services.GetItems(statisticNodeItem.ObjectID), (IServiceProvider) this._services);
        this.propertiesView1.Activate((IView) null);
      }
      else
      {
        if (!name.Equals(this.chartPage.Name))
          return;
        this.showChart_Click((object) this, EventArgs.Empty);
      }
    }
    else
      this.SetElementsAvailabilityForEmptyPage();
  }

  private void RemovePreviousDataFromTabControlPages()
  {
    foreach (Control control in (ArrangedElementCollection) this.configuratePage.Controls)
      this.configuratePage.Controls.Remove(control);
    this._chartDisplayControl1.SetEmptyPage(StatisticsConst.CHART_ABSENCE_MESSAGE);
  }

  private CommandSettings GetCommandSettingsFromObject(long statisticsObjectId)
  {
    CommandSettings settingsFromObject;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      settingsFromObject = this._statisticsService.ReadStatisticObjectsCommandSettings(sessionKeeper.Session.SessionGUID, statisticsObjectId);
    if (settingsFromObject == null)
      settingsFromObject = new CommandSettings();
    return settingsFromObject;
  }

  private void SetElementsAvailabilityForEmptyPage()
  {
    this.btnDelete.Enabled = this._deleteStatObjectStrip.Enabled = false;
    this.btnPrint.Enabled = this._printStatObjectStrip.Enabled = false;
    this.btnShowChart.Enabled = this._showChartStrip.Enabled = false;
    this.btnCreateReport.Enabled = this._createReportStatObjectStrip.Enabled = false;
    this.btnTaskStart.Enabled = this._enableStatisticsStrip.Enabled = false;
    this.btnTaskStop.Enabled = this._pauseStatisticsStrip.Enabled = false;
    this._contextMenuStrip.Enabled = false;
    this.tabControl.Visible = false;
    this.propertiesView1.Hide();
  }

  private IStatisticSettingsForm GetTaskSettingsForm(CommandSettings commandSettings)
  {
    return (IStatisticSettingsForm) new TaskConfigsForm(commandSettings);
  }

  private IStatisticSettingsForm GetCommandSettingsForm(
    CommandStatisticsTypesEnum commandStatisticsType,
    CommandSettings commandSettings)
  {
    IStatisticSettingsForm commandSettingsForm = (IStatisticSettingsForm) null;
    switch (commandStatisticsType)
    {
      case CommandStatisticsTypesEnum.CreatedDate:
        commandSettingsForm = (IStatisticSettingsForm) new CreatedDateConfigsForm(commandSettings);
        break;
      case CommandStatisticsTypesEnum.SignDate:
        commandSettingsForm = (IStatisticSettingsForm) new SignDateConfigsForm(commandSettings);
        break;
      case CommandStatisticsTypesEnum.LCStepDate:
        commandSettingsForm = (IStatisticSettingsForm) new LCStepDateConfigsForm(commandSettings);
        break;
      case CommandStatisticsTypesEnum.LCLevelDate:
        commandSettingsForm = (IStatisticSettingsForm) new LCLevelDateConfigsForm(commandSettings);
        break;
      case CommandStatisticsTypesEnum.DateAttrValue:
        commandSettingsForm = (IStatisticSettingsForm) new DateAttrValueConfigsForm(commandSettings);
        break;
      case CommandStatisticsTypesEnum.ProcessTemplate:
        commandSettingsForm = (IStatisticSettingsForm) new ProcessTemplateConfigsForm(commandSettings);
        break;
      case CommandStatisticsTypesEnum.TimeInTask:
        commandSettingsForm = (IStatisticSettingsForm) new TimeInTaskConfigsForm(commandSettings);
        break;
      case CommandStatisticsTypesEnum.TimeOneTaskFormUsers:
        commandSettingsForm = (IStatisticSettingsForm) new TimeOneTaskFormUsersConfigsForm(commandSettings);
        break;
      case CommandStatisticsTypesEnum.RevertCountTask:
        commandSettingsForm = (IStatisticSettingsForm) new RevertCountTaskConfigsForm(commandSettings);
        break;
    }
    return commandSettingsForm;
  }

  private void btnRefresh_Click(object sender, EventArgs e)
  {
    this.treeView1.SuspendLayout();
    TreeNode selectedNode = this.treeView1.SelectedNode;
    string str = string.Empty;
    if (selectedNode.Parent != null)
      str = selectedNode.Parent.Name;
    StatisticNodeItem tag1 = this.treeView1.SelectedNode.Tag as StatisticNodeItem;
    this.LoadAllStatisticObjects();
    this.LoadOftenUsedStatisticObjects();
    if (selectedNode == this.treeView1.Nodes[0] || selectedNode == this.treeView1.Nodes[1])
      this.treeView1.SelectedNode = selectedNode;
    else if (str == this.treeView1.Nodes[0].Name)
    {
      foreach (TreeNode node in this.treeView1.Nodes[0].Nodes)
      {
        if (node.Tag is StatisticNodeItem tag2 && tag2.ObjectID == tag1.ObjectID)
        {
          this.treeView1.SelectedNode = node;
          break;
        }
      }
    }
    else if (str == this.treeView1.Nodes[1].Name)
    {
      foreach (TreeNode node in this.treeView1.Nodes[1].Nodes)
      {
        if (node.Tag is StatisticNodeItem tag3 && tag3.ObjectID == tag1.ObjectID)
        {
          this.treeView1.SelectedNode = node;
          break;
        }
      }
    }
    this.treeView1.ResumeLayout();
  }

  private void SaveSettings(CommandSettings commandSettings)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject;
      if (this._oldSelStatisticNode == null)
      {
        StatisticNodeItem tag = this.treeView1.SelectedNode.Tag as StatisticNodeItem;
        dbObject = sessionKeeper.Session.GetObject(tag.ObjectID);
      }
      else
        dbObject = sessionKeeper.Session.GetObject(this._oldSelStatisticNode.ObjectID);
      this._statisticsClientService.WriteStatisticObjectsCommandSettings(sessionKeeper.Session, dbObject.ObjectID, commandSettings);
    }
    this._oldSelStatisticNode = (StatisticNodeItem) null;
    this.Modify = false;
  }

  private void SaveSettingsFromForm(object sender, EventArgs e)
  {
    if (!(sender is IStatisticSettingsForm statisticSettingsForm))
      return;
    this.SaveSettings(statisticSettingsForm.Settings);
  }

  private void SetMainFormModify(object sender, EventArgs e)
  {
    if (this.Modify)
      return;
    this.Modify = true;
  }

  private void CancelModify(object sender, EventArgs e) => this.Modify = false;

  private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
  {
    if (this.treeView1.SelectedNode == null)
      return;
    this._oldSelStatisticNode = this.treeView1.SelectedNode.Tag as StatisticNodeItem;
  }

  private void configuratePage_ControlRemoved(object sender, ControlEventArgs e)
  {
    if (this.Modify)
    {
      if (MessageBox.Show("Сохранить внесённые изменения?", "Внимание", MessageBoxButtons.YesNo) == DialogResult.Yes && e.Control is IStatisticSettingsForm)
        ((IStatisticSettingsForm) e.Control).Save(sender, (EventArgs) e);
      this.Modify = false;
    }
    if (e.Control is IStatisticSettingsForm)
    {
      ((IStatisticSettingsForm) e.Control).OnApplied -= new EventHandler(this.SaveSettingsFromForm);
      ((IStatisticSettingsForm) e.Control).OnModified -= new EventHandler(this.SetMainFormModify);
      ((IStatisticSettingsForm) e.Control).OnCancelModify -= new EventHandler(this.CancelModify);
      e.Control.Dispose();
    }
    this._oldSelStatisticNode = (StatisticNodeItem) null;
  }

  private void lvObjects_DoubleClick(object sender, EventArgs e)
  {
    if (!(sender is ListView listView) || listView.SelectedItems.Count != 1)
      return;
    string name = listView.SelectedItems[0].Name;
    foreach (TreeNode node in this.treeView1.Nodes[1].Nodes)
    {
      if (node.Name == name)
      {
        this.treeView1.SelectedNode = node;
        break;
      }
    }
  }

  private void lvObjects_MouseClick(object sender, MouseEventArgs e)
  {
    if (e.Button != MouseButtons.Right || !(this.lvObjects.SelectedItems[0].Tag is StatisticNodeItem tag))
      return;
    this.SetMenuButtonsAvailabilityForStatisticNodeItem(tag.ObjectTypeID);
    this.SetContextMenuForObject(tag);
    this._allMenu.ShowIndependent(new Point(Control.MousePosition.X, Control.MousePosition.Y));
  }

  private void lvObjects_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this.lvObjects.SelectedItems.Count == 0 || !(this.lvObjects.SelectedItems[0].Tag is StatisticNodeItem tag))
      return;
    this.SetMenuButtonsAvailabilityForStatisticNodeItem(tag.ObjectTypeID);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (StatisticsMainForm));
    TreeNode treeNode1 = new TreeNode("Часто используемые");
    TreeNode treeNode2 = new TreeNode("Объекты статистики");
    this.toolStrip1 = new ToolStrip();
    this.btnRefresh = new ToolStripButton();
    this.btnCreateStatObject = new ToolStripButton();
    this.btnDelete = new ToolStripButton();
    this.btnCreateReport = new ToolStripButton();
    this.btnPrint = new ToolStripButton();
    this.btnShowChart = new ToolStripButton();
    this.btnTaskStart = new ToolStripButton();
    this.btnTaskStop = new ToolStripButton();
    this.toolStripButton1 = new ToolStripButton();
    this.toolTip1 = new ToolTip(this.components);
    this.toolTip2 = new ToolTip(this.components);
    this.splitContainer1 = new SplitContainer();
    this.treeView1 = new TreeView();
    this.panelObjects = new Panel();
    this.lvObjects = new ListView();
    this.columnHeader1 = new ColumnHeader();
    this.tabControl = new System.Windows.Forms.TabControl();
    this.configuratePage = new System.Windows.Forms.TabPage();
    this.chartPage = new System.Windows.Forms.TabPage();
    this._chartDisplayControl1 = new ChartDisplayControl();
    this.propertyPage = new System.Windows.Forms.TabPage();
    this.propertiesView1 = new PropertiesView();
    this.objectsIcons = new ImageList(this.components);
    this.toolStrip1.SuspendLayout();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.panelObjects.SuspendLayout();
    this.tabControl.SuspendLayout();
    this.chartPage.SuspendLayout();
    this.propertyPage.SuspendLayout();
    this.SuspendLayout();
    this.toolStrip1.Items.AddRange(new ToolStripItem[8]
    {
      (ToolStripItem) this.btnRefresh,
      (ToolStripItem) this.btnCreateStatObject,
      (ToolStripItem) this.btnDelete,
      (ToolStripItem) this.btnCreateReport,
      (ToolStripItem) this.btnPrint,
      (ToolStripItem) this.btnShowChart,
      (ToolStripItem) this.btnTaskStart,
      (ToolStripItem) this.btnTaskStop
    });
    this.toolStrip1.Location = new Point(0, 0);
    this.toolStrip1.Name = "toolStrip1";
    this.toolStrip1.Size = new Size(758, 25);
    this.toolStrip1.TabIndex = 1;
    this.toolStrip1.Text = "toolStrip1";
    this.btnRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.btnRefresh.Image = (Image) componentResourceManager.GetObject("btnRefresh.Image");
    this.btnRefresh.ImageTransparentColor = Color.Magenta;
    this.btnRefresh.Name = "btnRefresh";
    this.btnRefresh.Size = new Size(23, 22);
    this.btnRefresh.Text = "Обновить";
    this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
    this.btnCreateStatObject.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.btnCreateStatObject.Image = (Image) componentResourceManager.GetObject("btnCreateStatObject.Image");
    this.btnCreateStatObject.ImageTransparentColor = Color.Magenta;
    this.btnCreateStatObject.Name = "btnCreateStatObject";
    this.btnCreateStatObject.Size = new Size(23, 22);
    this.btnCreateStatObject.Text = "Создать объект сбора статистики";
    this.btnCreateStatObject.Click += new EventHandler(this.createStatObject_Click);
    this.btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.btnDelete.Enabled = false;
    this.btnDelete.Image = (Image) componentResourceManager.GetObject("btnDelete.Image");
    this.btnDelete.ImageTransparentColor = Color.Magenta;
    this.btnDelete.Name = "btnDelete";
    this.btnDelete.Size = new Size(23, 22);
    this.btnDelete.Text = "Удалить";
    this.btnDelete.Click += new EventHandler(this.del_Click);
    this.btnCreateReport.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.btnCreateReport.Enabled = false;
    this.btnCreateReport.Image = (Image) componentResourceManager.GetObject("btnCreateReport.Image");
    this.btnCreateReport.ImageTransparentColor = Color.Magenta;
    this.btnCreateReport.Name = "btnCreateReport";
    this.btnCreateReport.Size = new Size(23, 22);
    this.btnCreateReport.Text = "Сформировать отчет";
    this.btnCreateReport.Click += new EventHandler(this.СreateReport_Click);
    this.btnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.btnPrint.Enabled = false;
    this.btnPrint.Image = (Image) componentResourceManager.GetObject("btnPrint.Image");
    this.btnPrint.ImageTransparentColor = Color.Magenta;
    this.btnPrint.Name = "btnPrint";
    this.btnPrint.Size = new Size(23, 22);
    this.btnPrint.Text = "Распечатать";
    this.btnPrint.Click += new EventHandler(this.print_Click);
    this.btnShowChart.Enabled = false;
    this.btnShowChart.Image = (Image) componentResourceManager.GetObject("btnShowChart.Image");
    this.btnShowChart.ImageTransparentColor = Color.Magenta;
    this.btnShowChart.Name = "btnShowChart";
    this.btnShowChart.Size = new Size(120, 22);
    this.btnShowChart.Text = "Показать график";
    this.btnShowChart.Click += new EventHandler(this.showChart_Click);
    this.btnTaskStart.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.btnTaskStart.Enabled = false;
    this.btnTaskStart.Image = (Image) componentResourceManager.GetObject("btnTaskStart.Image");
    this.btnTaskStart.ImageTransparentColor = Color.Magenta;
    this.btnTaskStart.Name = "btnTaskStart";
    this.btnTaskStart.Size = new Size(23, 22);
    this.btnTaskStart.Text = "Включить сбор статистики";
    this.btnTaskStart.Visible = false;
    this.btnTaskStart.Click += new EventHandler(this.taskStart_Click);
    this.btnTaskStop.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.btnTaskStop.Enabled = false;
    this.btnTaskStop.Image = (Image) Resources.pause;
    this.btnTaskStop.ImageTransparentColor = Color.Magenta;
    this.btnTaskStop.Name = "btnTaskStop";
    this.btnTaskStop.Size = new Size(23, 22);
    this.btnTaskStop.Text = "Приостановить сбор статистики";
    this.btnTaskStop.Visible = false;
    this.btnTaskStop.Click += new EventHandler(this.taskStop_Click);
    this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.toolStripButton1.ImageTransparentColor = Color.Magenta;
    this.toolStripButton1.Name = "toolStripButton1";
    this.toolStripButton1.Size = new Size(23, 22);
    this.toolStripButton1.Text = "toolStripButton1";
    this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.splitContainer1.BackColor = SystemColors.ControlLight;
    this.splitContainer1.Location = new Point(0, 28);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.BackColor = Color.White;
    this.splitContainer1.Panel1.Controls.Add((Control) this.treeView1);
    this.splitContainer1.Panel2.AutoScroll = true;
    this.splitContainer1.Panel2.BackColor = SystemColors.Control;
    this.splitContainer1.Panel2.Controls.Add((Control) this.panelObjects);
    this.splitContainer1.Panel2.Controls.Add((Control) this.tabControl);
    this.splitContainer1.Size = new Size(758, 500);
    this.splitContainer1.SplitterDistance = 215;
    this.splitContainer1.SplitterWidth = 6;
    this.splitContainer1.TabIndex = 0;
    this.treeView1.Dock = DockStyle.Fill;
    this.treeView1.ItemHeight = 18;
    this.treeView1.Location = new Point(0, 0);
    this.treeView1.Name = "treeView1";
    treeNode1.Name = "mostUsed";
    treeNode1.Text = "Часто используемые";
    treeNode2.Name = "statObjects";
    treeNode2.Text = "Объекты статистики";
    this.treeView1.Nodes.AddRange(new TreeNode[2]
    {
      treeNode1,
      treeNode2
    });
    this.treeView1.Size = new Size(215, 500);
    this.treeView1.TabIndex = 2;
    this.treeView1.BeforeSelect += new TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
    this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
    this.treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
    this.panelObjects.AutoScroll = true;
    this.panelObjects.Controls.Add((Control) this.lvObjects);
    this.panelObjects.Dock = DockStyle.Fill;
    this.panelObjects.Location = new Point(0, 0);
    this.panelObjects.Name = "panelObjects";
    this.panelObjects.Size = new Size(537, 500);
    this.panelObjects.TabIndex = 0;
    this.panelObjects.Visible = false;
    this.lvObjects.Columns.AddRange(new ColumnHeader[1]
    {
      this.columnHeader1
    });
    this.lvObjects.Dock = DockStyle.Fill;
    this.lvObjects.HideSelection = false;
    this.lvObjects.Location = new Point(0, 0);
    this.lvObjects.Name = "lvObjects";
    this.lvObjects.Size = new Size(537, 500);
    this.lvObjects.TabIndex = 0;
    this.lvObjects.UseCompatibleStateImageBehavior = false;
    this.lvObjects.View = View.Details;
    this.lvObjects.SelectedIndexChanged += new EventHandler(this.lvObjects_SelectedIndexChanged);
    this.lvObjects.DoubleClick += new EventHandler(this.lvObjects_DoubleClick);
    this.lvObjects.MouseClick += new MouseEventHandler(this.lvObjects_MouseClick);
    this.columnHeader1.Text = "Наименование объекта статистики";
    this.columnHeader1.Width = 232;
    this.tabControl.Controls.Add((Control) this.configuratePage);
    this.tabControl.Controls.Add((Control) this.chartPage);
    this.tabControl.Controls.Add((Control) this.propertyPage);
    this.tabControl.Dock = DockStyle.Fill;
    this.tabControl.Location = new Point(0, 0);
    this.tabControl.Name = "tabControl";
    this.tabControl.SelectedIndex = 0;
    this.tabControl.Size = new Size(537, 500);
    this.tabControl.TabIndex = 0;
    this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
    this.tabControl.Selecting += new TabControlCancelEventHandler(this.tabControl_Selecting);
    this.configuratePage.AutoScroll = true;
    this.configuratePage.AutoScrollMinSize = new Size(800, 400);
    this.configuratePage.BackColor = SystemColors.Control;
    this.configuratePage.Location = new Point(4, 22);
    this.configuratePage.Name = "configuratePage";
    this.configuratePage.Size = new Size(529, 474);
    this.configuratePage.TabIndex = 4;
    this.configuratePage.Text = "Настройки";
    this.configuratePage.ControlRemoved += new ControlEventHandler(this.configuratePage_ControlRemoved);
    this.chartPage.Controls.Add((Control) this._chartDisplayControl1);
    this.chartPage.ImeMode = ImeMode.NoControl;
    this.chartPage.Location = new Point(4, 22);
    this.chartPage.Name = "chartPage";
    this.chartPage.Padding = new Padding(3);
    this.chartPage.Size = new Size(529, 474);
    this.chartPage.TabIndex = 3;
    this.chartPage.Text = "Графики";
    this.chartPage.UseVisualStyleBackColor = true;
    this._chartDisplayControl1.AutoScroll = true;
    this._chartDisplayControl1.AutoSize = true;
    this._chartDisplayControl1.Dock = DockStyle.Fill;
    this._chartDisplayControl1.Location = new Point(3, 3);
    this._chartDisplayControl1.Name = "_chartDisplayControl1";
    this._chartDisplayControl1.Size = new Size(523, 468);
    this._chartDisplayControl1.TabIndex = 1;
    this.propertyPage.Controls.Add((Control) this.propertiesView1);
    this.propertyPage.Location = new Point(4, 22);
    this.propertyPage.Name = "propertyPage";
    this.propertyPage.Size = new Size(529, 474);
    this.propertyPage.TabIndex = 2;
    this.propertyPage.Text = "Свойства";
    this.propertyPage.UseVisualStyleBackColor = true;
    this.propertiesView1.Dock = DockStyle.Fill;
    this.propertiesView1.Font = new Font("Tahoma", 8.25f);
    this.propertiesView1.Location = new Point(0, 0);
    this.propertiesView1.Name = "propertiesView1";
    this.propertiesView1.Padding = new Padding(2);
    this.propertiesView1.Size = new Size(529, 474);
    this.propertiesView1.TabIndex = 2;
    this.propertiesView1.Tag = (object) "    ";
    this.objectsIcons.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("objectsIcons.ImageStream");
    this.objectsIcons.TransparentColor = Color.Transparent;
    this.objectsIcons.Images.SetKeyName(0, "collect_statistic");
    this.objectsIcons.Images.SetKeyName(1, "create_object");
    this.objectsIcons.Images.SetKeyName(2, "date");
    this.objectsIcons.Images.SetKeyName(3, "lc_level");
    this.objectsIcons.Images.SetKeyName(4, "lc_step");
    this.objectsIcons.Images.SetKeyName(5, "one_task_user");
    this.objectsIcons.Images.SetKeyName(6, "process_template");
    this.objectsIcons.Images.SetKeyName(7, "revert_count");
    this.objectsIcons.Images.SetKeyName(8, "sign_object");
    this.objectsIcons.Images.SetKeyName(9, "time_in_task");
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.toolStrip1);
    this.Controls.Add((Control) this.splitContainer1);
    this.DoubleBuffered = true;
    this.MinimumSize = new Size(261, 100);
    this.Name = nameof (StatisticsMainForm);
    this.ShowImageInDocumentTab = true;
    this.Size = new Size(758, 528);
    this.Text = "Статистика";
    this.Closing += new CancelEventHandler(this.StatisticsObjectForm_Closing);
    this.Load += new EventHandler(this.StatisticsObjectForm_Load);
    this.toolStrip1.ResumeLayout(false);
    this.toolStrip1.PerformLayout();
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this.panelObjects.ResumeLayout(false);
    this.tabControl.ResumeLayout(false);
    this.chartPage.ResumeLayout(false);
    this.chartPage.PerformLayout();
    this.propertyPage.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private delegate void ChangedDelegat();
}
