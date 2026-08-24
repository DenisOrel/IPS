// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHViewCtrlBase
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.DataFormats;
using Intermech.Imbase;
using Intermech.Imbase.Views;
using Intermech.Interfaces;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

[ToolboxItem(false)]
public class IMHViewCtrlBase : UserControl
{
  protected IServiceProvider _services;
  protected NavigatorTreeView _treeView;
  protected NodeIDPath _parentNodePath;
  protected long _mTableRefID;
  protected long _mRecID = -1;
  protected string _mCaption = string.Empty;
  protected long _aTableRefID;
  protected long _aRecID = -1;
  protected string _aCaption = string.Empty;
  protected Guid _categoryNodeGuid = Guid.Empty;
  private IViewsManager _pageViewsManager;
  private long _tempMTableRefID;
  private long _tempMRecID = -1;
  private long _tempATableRefID;
  private long _tempARecID = -1;
  private IContainer components;
  protected ToolStripButton _tsBtnMaterialSubstitutes;
  protected ToolStripSeparator _tsSeparator2;
  protected Panel _pnlFormula;
  protected SplitContainer _splt;
  protected ToolStripButton _tsBtnMaterialProperties;
  protected ToolStrip _tsMaterial;
  protected ToolStripSeparator _tsSeparator1;
  protected ToolStripButton _tsBtnSearch;
  protected ToolStripButton _tsBtnFavourites;
  protected ToolStripButton _tsBtnAddFavourite;
  protected ToolStripButton _tsBtnGluedMaterials;
  protected ToolStripButton _tsBtnCoatingProperties;
  protected ToolStripButton _tsBtnSort;
  protected ToolStripButton _tsBtnApplicabilityFilter;
  protected ToolStripSeparator _cmSeparator1;
  protected ToolStripMenuItem _cmFavourites;
  protected ToolStripMenuItem _cmAddFavourite;
  protected ToolStripSeparator _cmSeparator2;
  protected ToolStripMenuItem _cmSearch;
  protected ToolStripMenuItem _cmCoatingProperties;
  protected ToolStripMenuItem _cmMaterialProperties;
  protected ToolStripMenuItem _cmMaterialSubstitutes;
  protected ToolStripMenuItem _cmSort;
  protected ContextMenuStrip _contextMenuBase;
  private ImageList _il;
  protected ToolStripMenuItem _cmGluedMaterials;
  protected ToolStripMenuItem _cmApplicabilityFilter;
  protected ToolStripButton _tsShowInImbase;
  protected ToolStripMenuItem _cmShowInImbase;
  private ToolStripSeparator toolStripSeparator1;
  private ToolStripSeparator toolStripSeparator2;

  public IMHViewCtrlBase()
  {
    this.InitializeComponent();
    this._tsMaterial.ImageList = this._contextMenuBase.ImageList = this._il;
    this._tsBtnCoatingProperties.ImageIndex = this._cmCoatingProperties.ImageIndex = this._il.Images.IndexOfKey("Coating.png");
    this._tsBtnGluedMaterials.ImageIndex = this._cmGluedMaterials.ImageIndex = this._il.Images.IndexOfKey("GluedMaterials.png");
    this._tsBtnMaterialProperties.ImageIndex = this._cmMaterialProperties.ImageIndex = this._il.Images.IndexOfKey("MaterialProperties.png");
    this._tsBtnMaterialSubstitutes.ImageIndex = this._cmMaterialSubstitutes.ImageIndex = this._il.Images.IndexOfKey("MaterialSubstitutes.png");
    this._tsBtnFavourites.ImageIndex = this._cmFavourites.ImageIndex = this._il.Images.IndexOfKey("Favorites.png");
    this._tsBtnAddFavourite.ImageIndex = this._cmAddFavourite.ImageIndex = this._il.Images.IndexOfKey("AddFavorites.png");
    this._tsBtnSearch.ImageIndex = this._cmSearch.ImageIndex = this._il.Images.IndexOfKey("Find.png");
    this._tsBtnSort.ImageIndex = this._cmSort.ImageIndex = this._il.Images.IndexOfKey("SortAlphabetDesc.png");
    this._tsBtnApplicabilityFilter.ImageIndex = this._cmApplicabilityFilter.ImageIndex = this._il.Images.IndexOfKey("Filter.png");
    this._tsShowInImbase.ImageIndex = this._cmShowInImbase.ImageIndex = this._il.Images.IndexOfKey("ImbaseTable.png");
  }

  public event EventHandler<IMHMaterialChangedEventArgs> IMHMaterialChanged;

  private void On_pnlFormula_Paint(object sender, PaintEventArgs e)
  {
    this.FormulaPaint(e, string.Empty);
  }

  private void OnCoatingProperties_Click(object sender, EventArgs e)
  {
    this.CoatingPropertiesClick(sender, e);
  }

  private void OnGluedMaterials_Click(object sender, EventArgs e) => this.MaterialsClick(sender, e);

  private void OnMaterialProperties_Click(object sender, EventArgs e)
  {
    this.PropertiesClick(sender, e);
  }

  private void OnMaterialSubstitutes_Click(object sender, EventArgs e)
  {
    this.SubstitutesClick(sender, e);
  }

  private void OnFavourites_Click(object sender, EventArgs e) => this.FavouritesClick(sender, e);

  private void OnAddFavourite_Click(object sender, EventArgs e)
  {
    this.AddFavouriteClick(sender, e);
  }

  private void OnSearch_Click(object sender, EventArgs e) => this.SearchClick(sender, e);

  private void OnSort_Click(object sender, EventArgs e) => this.SortClick(sender, e);

  private void On_tsBtnApplicabilityFilter_Click(object sender, EventArgs e)
  {
    this.ApplicabilityFilterClick(sender, e);
  }

  private void _pageViewsManager_ViewsUpdated(object sender, EventArgs e)
  {
    if (!(this._pageViewsManager?.ActiveViewPage.View is IMHView view))
      return;
    view.iMHViewCtrlBase?.RestoreSelection(this._tempMTableRefID, this._tempMRecID, this._tempATableRefID, this._tempARecID);
    this._tempMTableRefID = this._tempATableRefID = 0L;
    this._tempMRecID = this._tempARecID = -1L;
    this._pageViewsManager.ViewsUpdated -= new EventHandler(this._pageViewsManager_ViewsUpdated);
  }

  public virtual void Initialize(
    ISelectedItems items,
    IServiceProvider provider,
    NavigatorTreeNode parentINode)
  {
    this._services = provider;
    this._treeView = ServiceUtils.GetService<NavigatorTreeView>((object) this._services, true);
    this._pageViewsManager = ServiceUtils.GetService<IViewsManager>((object) this._services, true);
    this._parentNodePath = this._treeView?.GetNodeIDPath(parentINode);
  }

  public virtual void Activate(IView previousView)
  {
    this.CustomizeSortButton(SortOrder.Descending);
    this._pnlFormula.Invalidate();
  }

  public virtual void Deactivate(IView nextView) => this.ClearData();

  protected virtual void CoatingPropertiesClick(object sender, EventArgs e)
  {
  }

  protected virtual void MaterialsClick(object sender, EventArgs e)
  {
  }

  protected virtual void PropertiesClick(object sender, EventArgs e)
  {
    if (this._mTableRefID == 0L || this._mRecID <= -1L)
      return;
    using (MaterialProperties materialProperties = new MaterialProperties(ImbaseHelper.MakeInternalImbaseKey(this._mTableRefID, this._mRecID), this._mCaption))
    {
      int num = (int) materialProperties.ShowDialog();
    }
  }

  protected virtual void SubstitutesClick(object sender, EventArgs e)
  {
  }

  protected virtual void FavouritesClick(object sender, EventArgs e)
  {
  }

  protected virtual void AddFavouriteClick(object sender, EventArgs e)
  {
  }

  protected virtual void SearchClick(object sender, EventArgs e)
  {
    using (BaseSearchForm baseSearchForm = new BaseSearchForm())
    {
      int num = (int) baseSearchForm.ShowDialog();
    }
  }

  protected virtual void SortClick(object sender, EventArgs e)
  {
    this.CustomizeSortButton((SortOrder) this._tsBtnSort.Tag == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending);
  }

  protected virtual void ApplicabilityFilterClick(object sender, EventArgs e)
  {
  }

  protected virtual void FormulaPaint(PaintEventArgs e, string text)
  {
    SizeF sizeF = e.Graphics.MeasureString(text, this._pnlFormula.Font);
    int width = (int) sizeF.Width;
    int height = (int) sizeF.Height;
    if (width <= 0 || height <= 0)
      return;
    int x = this._pnlFormula.Width > width ? this._pnlFormula.Width / 2 - width / 2 : 1;
    int y = this._pnlFormula.Height > height ? this._pnlFormula.Height / 2 - height / 2 : 1;
    using (SolidBrush solidBrush = new SolidBrush(SystemColors.WindowText))
      e.Graphics.DrawString(text, this._pnlFormula.Font, (Brush) solidBrush, (float) x, (float) y);
  }

  protected virtual void RestoreSelection(
    long mTableRefID,
    long mRecID,
    long aTableRefID,
    long aRecID)
  {
    this._mTableRefID = mTableRefID;
    this._mRecID = mRecID;
    this._aTableRefID = aTableRefID;
    this._aRecID = aRecID;
  }

  protected void ClearData()
  {
    this._mTableRefID = this._aTableRefID = 0L;
    this._mRecID = this._aRecID = -1L;
    this._mCaption = this._aCaption = string.Empty;
  }

  protected virtual void GoToNode(
    long nodeFolderID,
    long mTblRefID,
    long mRecID,
    long aTblRefID = 0,
    long aRecID = -1)
  {
    if (this._treeView == null)
      return;
    DataTable imbaseTableTree = IMHHelper.GetImbaseTableTree(nodeFolderID);
    if (imbaseTableTree == null)
      return;
    long folderID = nodeFolderID;
    QuickObjectInfo objectInfo;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      objectInfo = sessionKeeper.Session.GetObjectInfo(nodeFolderID);
    if (objectInfo.ObjectTypeID == Intermech.Imbase.Consts.ImbaseTableRefTypeID)
      folderID = IMHHelper.GetParentID(imbaseTableTree, nodeFolderID);
    if (folderID == 0L)
      return;
    if (this.IsSameNode(folderID))
    {
      this.RestoreSelection(mTblRefID, mRecID, aTblRefID, aRecID);
    }
    else
    {
      NavigatorTreeNode lastNode;
      if (!this._treeView.TryFind(this._parentNodePath, out lastNode) || lastNode == null)
        return;
      this.SaveIdsForRestore(mTblRefID, mRecID, aTblRefID, aRecID);
      this._treeView.FocusedNode = lastNode;
      this._treeView.SetNodeExpanded(lastNode, true);
      while (!lastNode.Full)
        Thread.Sleep(50);
      this.BrowseNode(this.SearchFolderNode(lastNode, folderID, imbaseTableTree));
    }
  }

  protected void SaveIdsForRestore(long mTblRefID, long mRecID, long aTblRefID, long aRecID)
  {
    this._tempMTableRefID = mTblRefID;
    this._tempMRecID = mRecID;
    this._tempATableRefID = aTblRefID;
    this._tempARecID = aRecID;
    this._pageViewsManager.ViewsUpdated += new EventHandler(this._pageViewsManager_ViewsUpdated);
  }

  protected void BrowseNode(NavigatorTreeNode folderNode)
  {
    if (folderNode == null || this._treeView == null || folderNode.HasFocus)
      return;
    this._treeView.TryBrowse(this._treeView.GetNodeIDPath(folderNode));
  }

  private bool IsSameNode(long folderID)
  {
    NavigatorTreeNode focusedNode = this._treeView.FocusedNode;
    return focusedNode?.Handler?.GetData(focusedNode.NodeID, typeof (IDBObjectID)) is IDBObjectID data && data.Value == folderID;
  }

  private void CustomizeSortButton(SortOrder order)
  {
    int num;
    string str;
    if (order == SortOrder.Ascending)
    {
      num = this._il.Images.IndexOfKey("SortAlphabetAsc.png");
      str = LocalizationHolder.rm.GetString("IMH_Sort_Ascending");
    }
    else
    {
      num = this._il.Images.IndexOfKey("SortAlphabetDesc.png");
      str = LocalizationHolder.rm.GetString("IMH_Sort_Descending");
    }
    this._tsBtnSort.ImageIndex = this._cmSort.ImageIndex = num;
    this._tsBtnSort.Text = this._cmSort.Text = str;
    this._tsBtnSort.Tag = (object) order;
  }

  private NavigatorTreeNode SearchFolderNode(
    NavigatorTreeNode parentNode,
    long folderID,
    DataTable dt)
  {
    NavigatorTreeNode navigatorTreeNode1 = (NavigatorTreeNode) null;
    NavigatorTreeNode navigatorTreeNode2 = parentNode;
    if (navigatorTreeNode2 != null)
    {
      INode handler = navigatorTreeNode2.Handler;
      NavigatorTreeNodes children = parentNode.Children;
      if (children != null && handler != null)
      {
        foreach (NavigatorTreeNode navigatorTreeNode3 in (List<NavigatorTreeNode>) children)
        {
          NavigatorTreeNode navigatorTreeNode4 = navigatorTreeNode3;
          if (navigatorTreeNode4 != null && handler.GetData(navigatorTreeNode4.NodeID, typeof (IDBObjectID)) is IDBObjectID data && data.Value != 0L)
          {
            long id = data.Value;
            if (id == folderID)
            {
              navigatorTreeNode4.Tree.FocusedNode = navigatorTreeNode4;
              navigatorTreeNode4.Tree.PopulateNode(navigatorTreeNode4);
              navigatorTreeNode4.Tree.SetNodeExpanded(navigatorTreeNode4, true);
              navigatorTreeNode1 = navigatorTreeNode4;
              break;
            }
            if (dt.Columns.Contains("F_OBJECT_ID") && dt.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToInt64(x["F_OBJECT_ID"]) == id)) != null)
            {
              navigatorTreeNode4.Tree.FocusedNode = navigatorTreeNode4;
              navigatorTreeNode4.Tree.PopulateNode(navigatorTreeNode4);
              navigatorTreeNode4.Tree.SetNodeExpanded(navigatorTreeNode4, true);
              navigatorTreeNode1 = this.SearchFolderNode(navigatorTreeNode4, folderID, dt);
              if (navigatorTreeNode1 != null)
                break;
            }
          }
        }
      }
    }
    return navigatorTreeNode1;
  }

  protected void OnIMHMaterialChanged(
    long tableRefID,
    long recID,
    bool selectable = true,
    string designation = "")
  {
    EventHandler<IMHMaterialChangedEventArgs> imhMaterialChanged = this.IMHMaterialChanged;
    if (imhMaterialChanged == null)
      return;
    imhMaterialChanged((object) this, new IMHMaterialChangedEventArgs(tableRefID, recID, selectable, designation));
  }

  internal static bool ExtractCanEdit(NavigatorTreeNode parentINode)
  {
    for (; parentINode != null; parentINode = parentINode.Parent)
    {
      if (parentINode.Handler is VirtualNode handler)
        return handler.CanEdit;
    }
    return false;
  }

  protected void GotoImbase(long tableRefId, long recId)
  {
    if (recId == -1L)
      return;
    if (tableRefId == 0L)
      return;
    try
    {
      NodeIDPath pathToImbaseObject;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        pathToImbaseObject = ImbaseClientHelper.CreatePathToImbaseObject(sessionKeeper.Session, tableRefId);
      SelectedRecords.Add(tableRefId, new long[1]{ recId });
      SelectedRecords.Add(-tableRefId, new long[1]{ recId });
      Utils.OpenNewWindow(pathToImbaseObject.RootDescriptor, (IServiceProvider) null, new GetSupportedColumnsEventHandler(Utils.DefaultSupportedColumnsObjects), pathToImbaseObject);
    }
    catch (ApplicationException ex)
    {
      ExceptionHelper.ExceptionService.ShowException((Exception) ex);
    }
  }

  protected void OnGotoImbase_Click(object sender, EventArgs e)
  {
    this.GotoImbase(this._mTableRefID, this._mRecID);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IMHViewCtrlBase));
    this._tsMaterial = new ToolStrip();
    this._tsBtnCoatingProperties = new ToolStripButton();
    this._tsBtnGluedMaterials = new ToolStripButton();
    this._tsBtnMaterialProperties = new ToolStripButton();
    this._tsBtnMaterialSubstitutes = new ToolStripButton();
    this._tsSeparator1 = new ToolStripSeparator();
    this._tsBtnFavourites = new ToolStripButton();
    this._tsBtnAddFavourite = new ToolStripButton();
    this._tsSeparator2 = new ToolStripSeparator();
    this._tsShowInImbase = new ToolStripButton();
    this.toolStripSeparator1 = new ToolStripSeparator();
    this._tsBtnSearch = new ToolStripButton();
    this._tsBtnSort = new ToolStripButton();
    this._tsBtnApplicabilityFilter = new ToolStripButton();
    this._pnlFormula = new Panel();
    this._splt = new SplitContainer();
    this._contextMenuBase = new ContextMenuStrip(this.components);
    this._cmCoatingProperties = new ToolStripMenuItem();
    this._cmGluedMaterials = new ToolStripMenuItem();
    this._cmMaterialProperties = new ToolStripMenuItem();
    this._cmMaterialSubstitutes = new ToolStripMenuItem();
    this._cmSeparator1 = new ToolStripSeparator();
    this._cmFavourites = new ToolStripMenuItem();
    this._cmAddFavourite = new ToolStripMenuItem();
    this.toolStripSeparator2 = new ToolStripSeparator();
    this._cmShowInImbase = new ToolStripMenuItem();
    this._cmSeparator2 = new ToolStripSeparator();
    this._cmSearch = new ToolStripMenuItem();
    this._cmSort = new ToolStripMenuItem();
    this._cmApplicabilityFilter = new ToolStripMenuItem();
    this._il = new ImageList(this.components);
    this._tsMaterial.SuspendLayout();
    this._splt.BeginInit();
    this._splt.SuspendLayout();
    this._contextMenuBase.SuspendLayout();
    this.SuspendLayout();
    this._tsMaterial.GripStyle = ToolStripGripStyle.Hidden;
    this._tsMaterial.Items.AddRange(new ToolStripItem[13]
    {
      (ToolStripItem) this._tsBtnCoatingProperties,
      (ToolStripItem) this._tsBtnGluedMaterials,
      (ToolStripItem) this._tsBtnMaterialProperties,
      (ToolStripItem) this._tsBtnMaterialSubstitutes,
      (ToolStripItem) this._tsSeparator1,
      (ToolStripItem) this._tsBtnFavourites,
      (ToolStripItem) this._tsBtnAddFavourite,
      (ToolStripItem) this._tsSeparator2,
      (ToolStripItem) this._tsShowInImbase,
      (ToolStripItem) this.toolStripSeparator1,
      (ToolStripItem) this._tsBtnSearch,
      (ToolStripItem) this._tsBtnSort,
      (ToolStripItem) this._tsBtnApplicabilityFilter
    });
    componentResourceManager.ApplyResources((object) this._tsMaterial, "_tsMaterial");
    this._tsMaterial.Name = "_tsMaterial";
    this._tsBtnCoatingProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._tsBtnCoatingProperties.Name = "_tsBtnCoatingProperties";
    componentResourceManager.ApplyResources((object) this._tsBtnCoatingProperties, "_tsBtnCoatingProperties");
    this._tsBtnCoatingProperties.Click += new EventHandler(this.OnCoatingProperties_Click);
    this._tsBtnGluedMaterials.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this._tsBtnGluedMaterials.Name = "_tsBtnGluedMaterials";
    componentResourceManager.ApplyResources((object) this._tsBtnGluedMaterials, "_tsBtnGluedMaterials");
    this._tsBtnGluedMaterials.Click += new EventHandler(this.OnGluedMaterials_Click);
    this._tsBtnMaterialProperties.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnMaterialProperties, "_tsBtnMaterialProperties");
    this._tsBtnMaterialProperties.Name = "_tsBtnMaterialProperties";
    this._tsBtnMaterialProperties.Click += new EventHandler(this.OnMaterialProperties_Click);
    this._tsBtnMaterialSubstitutes.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnMaterialSubstitutes, "_tsBtnMaterialSubstitutes");
    this._tsBtnMaterialSubstitutes.Name = "_tsBtnMaterialSubstitutes";
    this._tsBtnMaterialSubstitutes.Click += new EventHandler(this.OnMaterialSubstitutes_Click);
    this._tsSeparator1.Name = "_tsSeparator1";
    componentResourceManager.ApplyResources((object) this._tsSeparator1, "_tsSeparator1");
    this._tsBtnFavourites.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnFavourites, "_tsBtnFavourites");
    this._tsBtnFavourites.Name = "_tsBtnFavourites";
    this._tsBtnFavourites.Click += new EventHandler(this.OnFavourites_Click);
    this._tsBtnAddFavourite.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnAddFavourite, "_tsBtnAddFavourite");
    this._tsBtnAddFavourite.Name = "_tsBtnAddFavourite";
    this._tsBtnAddFavourite.Click += new EventHandler(this.OnAddFavourite_Click);
    this._tsSeparator2.Name = "_tsSeparator2";
    componentResourceManager.ApplyResources((object) this._tsSeparator2, "_tsSeparator2");
    this._tsShowInImbase.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsShowInImbase, "_tsShowInImbase");
    this._tsShowInImbase.Name = "_tsShowInImbase";
    this._tsShowInImbase.Click += new EventHandler(this.OnGotoImbase_Click);
    this.toolStripSeparator1.Name = "toolStripSeparator1";
    componentResourceManager.ApplyResources((object) this.toolStripSeparator1, "toolStripSeparator1");
    this._tsBtnSearch.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnSearch, "_tsBtnSearch");
    this._tsBtnSearch.Name = "_tsBtnSearch";
    this._tsBtnSearch.Click += new EventHandler(this.OnSearch_Click);
    this._tsBtnSort.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnSort, "_tsBtnSort");
    this._tsBtnSort.Name = "_tsBtnSort";
    this._tsBtnSort.Click += new EventHandler(this.OnSort_Click);
    this._tsBtnApplicabilityFilter.DisplayStyle = ToolStripItemDisplayStyle.Image;
    componentResourceManager.ApplyResources((object) this._tsBtnApplicabilityFilter, "_tsBtnApplicabilityFilter");
    this._tsBtnApplicabilityFilter.Name = "_tsBtnApplicabilityFilter";
    this._tsBtnApplicabilityFilter.Click += new EventHandler(this.On_tsBtnApplicabilityFilter_Click);
    componentResourceManager.ApplyResources((object) this._pnlFormula, "_pnlFormula");
    this._pnlFormula.Name = "_pnlFormula";
    this._pnlFormula.Paint += new PaintEventHandler(this.On_pnlFormula_Paint);
    componentResourceManager.ApplyResources((object) this._splt, "_splt");
    this._splt.Name = "_splt";
    this._contextMenuBase.Items.AddRange(new ToolStripItem[13]
    {
      (ToolStripItem) this._cmCoatingProperties,
      (ToolStripItem) this._cmGluedMaterials,
      (ToolStripItem) this._cmMaterialProperties,
      (ToolStripItem) this._cmMaterialSubstitutes,
      (ToolStripItem) this._cmSeparator1,
      (ToolStripItem) this._cmFavourites,
      (ToolStripItem) this._cmAddFavourite,
      (ToolStripItem) this.toolStripSeparator2,
      (ToolStripItem) this._cmShowInImbase,
      (ToolStripItem) this._cmSeparator2,
      (ToolStripItem) this._cmSearch,
      (ToolStripItem) this._cmSort,
      (ToolStripItem) this._cmApplicabilityFilter
    });
    this._contextMenuBase.Name = "_contextMenuGlues";
    componentResourceManager.ApplyResources((object) this._contextMenuBase, "_contextMenuBase");
    this._cmCoatingProperties.Name = "_cmCoatingProperties";
    componentResourceManager.ApplyResources((object) this._cmCoatingProperties, "_cmCoatingProperties");
    this._cmCoatingProperties.Click += new EventHandler(this.OnCoatingProperties_Click);
    this._cmGluedMaterials.Name = "_cmGluedMaterials";
    componentResourceManager.ApplyResources((object) this._cmGluedMaterials, "_cmGluedMaterials");
    this._cmGluedMaterials.Click += new EventHandler(this.OnGluedMaterials_Click);
    this._cmMaterialProperties.Name = "_cmMaterialProperties";
    componentResourceManager.ApplyResources((object) this._cmMaterialProperties, "_cmMaterialProperties");
    this._cmMaterialProperties.Click += new EventHandler(this.OnMaterialProperties_Click);
    this._cmMaterialSubstitutes.Name = "_cmMaterialSubstitutes";
    componentResourceManager.ApplyResources((object) this._cmMaterialSubstitutes, "_cmMaterialSubstitutes");
    this._cmMaterialSubstitutes.Click += new EventHandler(this.OnMaterialSubstitutes_Click);
    this._cmSeparator1.Name = "_cmSeparator1";
    componentResourceManager.ApplyResources((object) this._cmSeparator1, "_cmSeparator1");
    this._cmFavourites.Name = "_cmFavourites";
    componentResourceManager.ApplyResources((object) this._cmFavourites, "_cmFavourites");
    this._cmFavourites.Click += new EventHandler(this.OnFavourites_Click);
    this._cmAddFavourite.Name = "_cmAddFavourite";
    componentResourceManager.ApplyResources((object) this._cmAddFavourite, "_cmAddFavourite");
    this._cmAddFavourite.Click += new EventHandler(this.OnAddFavourite_Click);
    this.toolStripSeparator2.Name = "toolStripSeparator2";
    componentResourceManager.ApplyResources((object) this.toolStripSeparator2, "toolStripSeparator2");
    this._cmShowInImbase.Name = "_cmShowInImbase";
    componentResourceManager.ApplyResources((object) this._cmShowInImbase, "_cmShowInImbase");
    this._cmShowInImbase.Click += new EventHandler(this.OnGotoImbase_Click);
    this._cmSeparator2.Name = "_cmSeparator2";
    componentResourceManager.ApplyResources((object) this._cmSeparator2, "_cmSeparator2");
    this._cmSearch.Name = "_cmSearch";
    componentResourceManager.ApplyResources((object) this._cmSearch, "_cmSearch");
    this._cmSearch.Click += new EventHandler(this.OnSearch_Click);
    this._cmSort.Name = "_cmSort";
    componentResourceManager.ApplyResources((object) this._cmSort, "_cmSort");
    this._cmSort.Click += new EventHandler(this.OnSort_Click);
    this._cmApplicabilityFilter.Name = "_cmApplicabilityFilter";
    componentResourceManager.ApplyResources((object) this._cmApplicabilityFilter, "_cmApplicabilityFilter");
    this._il.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("_il.ImageStream");
    this._il.TransparentColor = Color.Transparent;
    this._il.Images.SetKeyName(0, "Coating.png");
    this._il.Images.SetKeyName(1, "GluedMaterials.png");
    this._il.Images.SetKeyName(2, "MaterialProperties.png");
    this._il.Images.SetKeyName(3, "MaterialSubstitutes.png");
    this._il.Images.SetKeyName(4, "Favorites.png");
    this._il.Images.SetKeyName(5, "AddFavorites.png");
    this._il.Images.SetKeyName(6, "Find.png");
    this._il.Images.SetKeyName(7, "SortAlphabetAsc.png");
    this._il.Images.SetKeyName(8, "SortAlphabetDesc.png");
    this._il.Images.SetKeyName(9, "Filter.png");
    this._il.Images.SetKeyName(10, "Filter_On.png");
    this._il.Images.SetKeyName(11, "ImbaseTable.png");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._splt);
    this.Controls.Add((Control) this._pnlFormula);
    this.Controls.Add((Control) this._tsMaterial);
    this.DoubleBuffered = true;
    this.Name = nameof (IMHViewCtrlBase);
    this._tsMaterial.ResumeLayout(false);
    this._tsMaterial.PerformLayout();
    this._splt.EndInit();
    this._splt.ResumeLayout(false);
    this._contextMenuBase.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
