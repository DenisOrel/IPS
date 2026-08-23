// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.Graphs
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Bars;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Закладка "Графы для подписей".</summary>
[ViewDescriptionProvider(typeof (Graphs.GraphsViewDescriptionProvider))]
internal class Graphs : UserControl, IView
{
  private TreeView _InfoBox;
  private Button _bAdd;
  private Panel _buttonsPanel;
  private Button _bCancel;
  private Button _bApply;
  private Panel _bottomPanel;
  private Button _bDelete;
  private Graphs4Type _graphTypeInfo = new Graphs4Type(SignsCache.PossibleGraphs);
  private bool _modified;
  private MenuBarItem _menu;
  private MenuButtonItem _miAdd;
  private MenuButtonItem _miDelete;
  private MenuButtonItem _miClear;
  private long _objID;
  private bool _firstRun;
  private SortedList images = new SortedList();
  private ImageList imageList = new ImageList();
  private ICategoryTypeIconService objTypesIcons;

  /// <summary>Конструктор.</summary>
  public Graphs()
  {
    this.InitializeComponent();
    this.CreateMenu();
  }

  /// <summary>
  /// 
  /// </summary>
  public bool Modified
  {
    get => this._modified;
    set
    {
      this._modified = value;
      this._buttonsPanel.Enabled = value;
    }
  }

  /// <summary>
  /// 
  /// </summary>
  private void CreateMenu()
  {
    this._menu = (SignsHolder.Bar as BarManager).MenuBar.AddMenuBar(LocalizationHolder.rm.GetString("Signs_41"));
    this._menu.Visible = false;
    this._menu.BeforePopup += new MenuItemBase.BeforePopupEventHandler(this._menu_BeforePopup);
    this._miAdd = new MenuButtonItem(LocalizationHolder.rm.GetString("Signs_42"), new EventHandler(this._menu_Click));
    this._miAdd.CommandName = "Add";
    this._miDelete = new MenuButtonItem(LocalizationHolder.rm.GetString("Signs_43"), new EventHandler(this._menu_Click));
    this._miDelete.CommandName = "Delete";
    this._miClear = new MenuButtonItem(LocalizationHolder.rm.GetString("Signs_44"), new EventHandler(this._menu_Click));
    this._miClear.CommandName = "Clear";
    this._menu.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this._miAdd,
      (ToolbarItemBase) this._miDelete,
      (ToolbarItemBase) this._miClear
    });
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _menu_Click(object sender, EventArgs e)
  {
    if (!(sender is ButtonItemBase buttonItemBase))
      return;
    if (buttonItemBase.CommandName.Equals("Add"))
    {
      using (SelectGraphs selectGraphs = new SelectGraphs())
      {
        int int32 = Convert.ToInt32(this._InfoBox.SelectedNode.Tag);
        if (!selectGraphs.ShowDialog().Equals((object) DialogResult.OK) || selectGraphs.SelectedList.Count <= 0)
          return;
        this._graphTypeInfo.Add(int32, selectGraphs.SelectedList);
        this.PopulateInfo();
        this.Modified = true;
      }
    }
    else if (buttonItemBase.CommandName.Equals("Delete"))
    {
      this._bDelete_Click((object) null, (EventArgs) null);
    }
    else
    {
      if (!buttonItemBase.CommandName.Equals("Clear"))
        return;
      this._graphTypeInfo.Clear();
      this.PopulateInfo();
      this.Modified = true;
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _InfoBox_MouseUp(object sender, MouseEventArgs e)
  {
    if (!e.Button.Equals((object) MouseButtons.Right))
      return;
    TreeNode nodeAt = this._InfoBox.GetNodeAt(e.X, e.Y);
    if (nodeAt != null)
      this._InfoBox.SelectedNode = nodeAt;
    this._menu.Show(sender as Control, new Point(e.X, e.Y));
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _menu_BeforePopup(object sender, MenuPopupEventArgs e)
  {
    TreeNode selectedNode = this._InfoBox.SelectedNode;
    if (selectedNode != null && selectedNode.Level == 0)
      this._miAdd.Enabled = this._miAdd.Visible = true;
    else
      this._miAdd.Enabled = this._miAdd.Visible = false;
    this._miDelete.Enabled = selectedNode != null;
    this._bDelete.Enabled = selectedNode != null;
    this._miClear.Enabled = this._InfoBox.GetNodeCount(true) > 0;
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Graphs));
    this._InfoBox = new TreeView();
    this._bAdd = new Button();
    this._buttonsPanel = new Panel();
    this._bCancel = new Button();
    this._bApply = new Button();
    this._bottomPanel = new Panel();
    this._bDelete = new Button();
    this._buttonsPanel.SuspendLayout();
    this._bottomPanel.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this._InfoBox, "_InfoBox");
    this._InfoBox.Name = "_InfoBox";
    this._InfoBox.MouseUp += new MouseEventHandler(this._InfoBox_MouseUp);
    this._InfoBox.AfterSelect += new TreeViewEventHandler(this._InfoBox_AfterSelect);
    componentResourceManager.ApplyResources((object) this._bAdd, "_bAdd");
    this._bAdd.Name = "_bAdd";
    this._bAdd.Click += new EventHandler(this._bAdd_Click);
    this._buttonsPanel.Controls.Add((Control) this._bCancel);
    this._buttonsPanel.Controls.Add((Control) this._bApply);
    componentResourceManager.ApplyResources((object) this._buttonsPanel, "_buttonsPanel");
    this._buttonsPanel.Name = "_buttonsPanel";
    componentResourceManager.ApplyResources((object) this._bCancel, "_bCancel");
    this._bCancel.Name = "_bCancel";
    this._bCancel.Click += new EventHandler(this._bCancel_Click);
    componentResourceManager.ApplyResources((object) this._bApply, "_bApply");
    this._bApply.Name = "_bApply";
    this._bApply.Click += new EventHandler(this._bApply_Click);
    this._bottomPanel.Controls.Add((Control) this._bDelete);
    this._bottomPanel.Controls.Add((Control) this._bAdd);
    this._bottomPanel.Controls.Add((Control) this._buttonsPanel);
    componentResourceManager.ApplyResources((object) this._bottomPanel, "_bottomPanel");
    this._bottomPanel.Name = "_bottomPanel";
    componentResourceManager.ApplyResources((object) this._bDelete, "_bDelete");
    this._bDelete.Name = "_bDelete";
    this._bDelete.Click += new EventHandler(this._bDelete_Click);
    this.Controls.Add((Control) this._InfoBox);
    this.Controls.Add((Control) this._bottomPanel);
    this.Name = nameof (Graphs);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Tag = (object) " ";
    this._buttonsPanel.ResumeLayout(false);
    this._bottomPanel.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  /// <summary>
  /// 
  /// </summary>
  public int ImageIndex => -1;

  /// <summary>
  /// 
  /// </summary>
  public int OrderID => 21;

  /// <summary>
  /// 
  /// </summary>
  public string Caption => LocalizationHolder.rm.GetString("Signs_45");

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="services"></param>
  public void Initialize(ISelectedItems items, IServiceProvider services)
  {
    this._objID = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
    this._firstRun = true;
    this.objTypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._InfoBox.ImageList = this.objTypesIcons.ImageList;
    this._InfoBox.ImageList.Images.Add("empty", (Image) new Bitmap(1, 1));
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="nextView"></param>
  public void Deactivate(IView nextView)
  {
    if (!this.Modified)
      return;
    if (MessageBox.Show(LocalizationHolder.rm.GetString("Signs_46"), LocalizationHolder.rm.GetString("Signs_47"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals((object) DialogResult.Yes))
      this._bApply_Click((object) null, (EventArgs) null);
    else
      this._bCancel_Click((object) null, (EventArgs) null);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="previousView"></param>
  public void Activate(IView previousView)
  {
    if (!this._firstRun)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      IDBAttribute objectAttributeById = session.GetObjectAttributeByID(this._objID, SignsHolder.SignsSetupAttrTypeID);
      IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(SignsHolder.GraphAttrTypeID);
      if (attributeType != null)
        SignsCache.PossibleGraphs = SignsCache.ParsePossibleGraphs(attributeType.GetPossibleValues());
      if (objectAttributeById != null)
      {
        MemoryStream memoryStream = new MemoryStream();
        new BlobProcReader(objectAttributeById, 0, (Stream) memoryStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData();
        this._graphTypeInfo = memoryStream.Length <= 0L ? new Graphs4Type(SignsCache.PossibleGraphs) : new Graphs4Type((Stream) memoryStream, SignsCache.PossibleGraphs);
        this.PopulateInfo(session);
      }
      else
      {
        this._graphTypeInfo.Clear();
        this._InfoBox.Nodes.Clear();
      }
    }
    this.Modified = false;
  }

  /// <summary>
  /// 
  /// </summary>
  private void PopulateInfo()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.PopulateInfo(sessionKeeper.Session);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="session"></param>
  private void PopulateInfo(IUserSession session)
  {
    this._InfoBox.BeginUpdate();
    try
    {
      this._InfoBox.Nodes.Clear();
      foreach (int num1 in this._graphTypeInfo)
      {
        IMSObjectType objectType = MetaDataHelper.GetObjectType(num1);
        int num2 = this.objTypesIcons.IndexOf(4, objectType.ObjectTypeID);
        Graphs4TypeStruct graphs4ObjectType = this._graphTypeInfo.GetGraphs4ObjectType(session, num1, false);
        if (graphs4ObjectType != null)
        {
          TreeNode node1 = new TreeNode(objectType.ObjectTypeName)
          {
            Tag = (object) num1
          };
          node1.ImageIndex = node1.SelectedImageIndex = num2;
          foreach (string graph in graphs4ObjectType.Graphs)
          {
            string text;
            if (SignsCache.PossibleGraphs.TryGetValue(graph, out text))
            {
              TreeNode node2 = new TreeNode(text);
              node2.ImageKey = node2.SelectedImageKey = "empty";
              node1.Nodes.Add(node2);
            }
          }
          this._InfoBox.Nodes.Add(node1);
        }
      }
    }
    finally
    {
      this._InfoBox.EndUpdate();
    }
    this._menu_BeforePopup((object) null, (MenuPopupEventArgs) null);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _InfoBox_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this._menu_BeforePopup((object) null, (MenuPopupEventArgs) null);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _bAdd_Click(object sender, EventArgs e)
  {
    using (AddGraphs addGraphs = new AddGraphs())
    {
      addGraphs.LoadForm();
      if (!addGraphs.ShowDialog().Equals((object) DialogResult.OK) || addGraphs.SelectedList.Count <= 0)
        return;
      foreach (int id in addGraphs.IDList)
        this._graphTypeInfo.Add(id, addGraphs.SelectedList);
      this.PopulateInfo();
      this.Modified = true;
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _bDelete_Click(object sender, EventArgs e)
  {
    TreeNode selectedNode = this._InfoBox.SelectedNode;
    string str = "";
    if (selectedNode.Text.Equals(""))
      return;
    if (selectedNode.Parent == null)
    {
      this._graphTypeInfo.Remove(Convert.ToInt32(selectedNode.Tag));
      this.Modified = true;
    }
    else
    {
      str = selectedNode.Parent.Text;
      int int32 = Convert.ToInt32(selectedNode.Parent.Tag);
      string graph = string.Empty;
      foreach (KeyValuePair<string, string> possibleGraph in SignsCache.PossibleGraphs)
      {
        if (possibleGraph.Value.Equals(selectedNode.Text))
        {
          graph = possibleGraph.Key;
          break;
        }
      }
      if (!graph.Equals(string.Empty))
        this._graphTypeInfo.Remove(int32, graph);
      this.Modified = true;
    }
    this.PopulateInfo();
    if (str.Equals(""))
      return;
    foreach (TreeNode node in this._InfoBox.Nodes)
    {
      if (node.Text.Equals(str))
      {
        node.Expand();
        break;
      }
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _bApply_Click(object sender, EventArgs e)
  {
    using (MemoryStream destStream = new MemoryStream())
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IUserSession session = sessionKeeper.Session;
        this._graphTypeInfo.Save((Stream) destStream, session);
        IDBAttribute aIDBAttribute = session.GetObject(this._objID).Attributes.AddAttribute(SignsHolder.SignsSetupAttrTypeID, false);
        destStream.Position = 0L;
        BlobInformation aBlobInformation = new BlobInformation(destStream.Length, 0L, DateTime.Now, "signs.xml", ArcMethods.ZLibPacked, "");
        MemoryStream aSourceStream = destStream;
        new BlobProcWriter(aIDBAttribute, 0, aBlobInformation, (Stream) aSourceStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
        if (new ArrayList((ICollection) session.GetObjectAttributeByID(session.UserID, SignsHolder.RankAttrTypeID).Values).Contains((object) this._objID))
          SignsCache.UserSignsCard = SignsCache.LoadUserGraphInfo(session, session.UserID, false);
        SignsCache.ClearCache(session);
        SignsCache.LoadUserGraphInfo(session, session.UserID, false);
      }
    }
    this.Modified = false;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void _bCancel_Click(object sender, EventArgs e)
  {
    this._firstRun = true;
    this.Activate((IView) null);
  }

  private sealed class GraphsViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      return new ViewDescription()
      {
        Caption = LocalizationHolder.rm.GetString("Signs_45"),
        ImageIndex = -1,
        OrderID = 21
      };
    }
  }
}
