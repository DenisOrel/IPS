// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ArtSelectForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.NavBars;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Pdm;

public class ArtSelectForm : Form
{
  private int _substGroupNo = -1;
  private int _substInGroup = -1;
  private int _design = -1;
  private int _name = -1;
  private long _currentArticle = -1;
  private IDescriptor _rootDescriptor;
  private IServiceProvider _viewServices;
  private INavGraphicsCache _navGraphicsCache;
  private Dictionary<long, RelationAttributesPackage> _relationsPackages;
  private Dictionary<long, MyElementEx> _articlesToChange = new Dictionary<long, MyElementEx>();
  private ICategoryTypeIconService _objTypesIcons;
  private INamedImageList _namedImageList;
  private string _filtrationOwnerID;
  private int _relationTypeID = -1;
  private List<long> _contexts;
  private List<NodeColumnID> _advAttributes;
  private long _mainArticle;
  private List<long> _articles = new List<long>();
  private PDMSubstitutesEditorOptionsHolder options;
  protected Font boldFont;
  private IContainer components;
  private Panel panelBottom;
  private Button btnCancel;
  private Button btnOK;
  private ImageList imagesState;
  private ToolTip toolTip;
  private ImageList imagesMenus;
  private MenuBar menuComposition;
  private ContextMenuBarItem contextMenuComposition;
  private MenuButtonItem mnpColumnsSetup;
  private SplitContainer splitContainer;
  private iGrid gridArticles;
  private SubstitutesView articleComposition;
  private StatusStrip articleCompositionHeader;
  private ToolStripStatusLabel articleCaption;
  private HeaderControl headerControl;
  private PictureBox pictureAdded;
  private Label labelAdded;
  private Label labelRemoved;
  private PictureBox pictureRemoved;
  private Label labelChanged;
  private PictureBox pictureChanged;
  private Intermech.Bars.ToolBar tbComposition;
  private ButtonItem btnSelectAll;
  private ButtonItem btnDeselectAll;
  private ButtonItem btnToActual;
  private ButtonItem btnToSubstitute;
  private ButtonItem btnAddToSubstitute2;
  private ButtonItem btnAddToNewActualSubstitute;
  private ButtonItem btnAddToNewSubstitute;
  private ButtonItem btnProperties;
  private ButtonItem btnVirtualComposition;
  private ButtonItem btnRefresh;
  private iGCellStyle gridArticlesCol4CellStyle;
  private iGColHdrStyle gridArticlesCol4ColHdrStyle;

  public ArtSelectForm() => this.InitializeComponent();

  public ArtSelectForm(
    IServiceProvider viewServices,
    ref Dictionary<long, RelationAttributesPackage> relationsPackages,
    string filtrationOwnerID,
    int relationTypeID,
    List<long> contexts,
    List<NodeColumnID> advAttributes,
    long mainArticle)
  {
    this.InitializeComponent();
    this.btnSelectAll.ShowText = true;
    this.btnDeselectAll.ShowText = true;
    this.InitForm(viewServices, ref relationsPackages, filtrationOwnerID, relationTypeID, contexts, advAttributes, mainArticle);
  }

  protected virtual bool InitForm(
    IServiceProvider viewServices,
    ref Dictionary<long, RelationAttributesPackage> relationsPackages,
    string filtrationOwnerID,
    int relationTypeID,
    List<long> contexts,
    List<NodeColumnID> advAttributes,
    long mainArticle)
  {
    if (relationsPackages == null)
      relationsPackages = new Dictionary<long, RelationAttributesPackage>();
    this._viewServices = viewServices;
    this._relationsPackages = relationsPackages;
    this._filtrationOwnerID = filtrationOwnerID;
    this._relationTypeID = relationTypeID;
    this._contexts = contexts;
    this._advAttributes = advAttributes;
    this._mainArticle = mainArticle;
    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
    this.Size = new Size(workingArea.Width / 100 * 60, workingArea.Height / 100 * 50);
    int width1 = workingArea.Width;
    Size size = this.Size;
    int width2 = size.Width;
    int x = (width1 - width2) / 2;
    int height1 = workingArea.Height;
    size = this.Size;
    int height2 = size.Height;
    int y = (height1 - height2) / 2;
    this.Location = new Point(x, y);
    SubstitutesView articleComposition = this.articleComposition;
    size = this.ClientSize;
    int num = size.Height / 3;
    articleComposition.Height = num;
    FormStorage.LoadLayout((Control) this);
    this.options = viewServices.GetService(typeof (PDMSubstitutesEditorOptionsHolder)) as PDMSubstitutesEditorOptionsHolder;
    if (this.options == null)
      this.options = new PDMSubstitutesEditorOptionsHolder(PDMSubstitutesEditorMode.Default, AVSSpecificationForm.Single, (List<long>) null);
    this._objTypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._navGraphicsCache = ServicesManager.GetService(typeof (INavGraphicsCache)) as INavGraphicsCache;
    this._namedImageList = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this.gridArticles.Cols[0].CellStyle.ImageList = this._objTypesIcons.ImageList;
    this.gridArticles.Cols[1].CellStyle.ImageList = this.imagesState;
    this.mnpColumnsSetup.Image = this._namedImageList != null ? this._namedImageList.ImageList.Images[this._namedImageList.ImageIndex("imgViewSettings")] : this.mnpColumnsSetup.Image;
    this.PrepareLegend();
    this.LoadArticles();
    this.FillArticlesGrid();
    this.UpdateControls();
    return true;
  }

  protected virtual void LoadArticles()
  {
    this._articlesToChange.Clear();
    this._articles.Clear();
    if (this._relationsPackages == null || this._relationsPackages.Count == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._substGroupNo = sessionKeeper.Session.IdentHelper.SubstitutesGroupNoID;
      this._substInGroup = sessionKeeper.Session.IdentHelper.SubstituteInGroup;
      this._design = sessionKeeper.Session.IdentHelper.DesignationID;
      this._name = sessionKeeper.Session.IdentHelper.NameID;
      long articleID = 0;
      foreach (KeyValuePair<long, RelationAttributesPackage> relationsPackage in this._relationsPackages)
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(relationsPackage.Key, false);
        if (dbObject != null && (this.options.Articles == null || this.options.Articles.Count <= 0 || this.options.Articles.IndexOf(dbObject.ObjectID) >= 0 || this._mainArticle != 0L && dbObject.ObjectID == this._mainArticle))
        {
          string stringValue1 = DataSetProcessor.GetStringValue(dbObject.GetAttributeByID(this._design)?.Value, string.Empty);
          string stringValue2 = DataSetProcessor.GetStringValue(dbObject.GetAttributeByID(this._name)?.Value, string.Empty);
          MyElementEx myElementEx = new MyElementEx((object) null, dbObject.Caption, (relationsPackage.Key == this._mainArticle ? 1 : 0) != 0, (relationsPackage.Key == this._mainArticle ? 1 : 0) != 0, false, relationsPackage.Key, dbObject.ObjectType, Guid.Empty, new object[2]
          {
            (object) stringValue1,
            (object) stringValue2
          });
          if (this.options.Articles != null && this.options.Articles.Count > 0 && this.options.Articles.IndexOf(myElementEx.ElementID64) >= 0)
            myElementEx.ElementBool = true;
          this._articlesToChange.Add(relationsPackage.Key, myElementEx);
          if (articleID == 0L)
            articleID = relationsPackage.Key;
        }
      }
      if (articleID == 0L)
        return;
      this._articles.AddRange((IEnumerable<long>) (sessionKeeper.Session.GetCustomService(typeof (IArticleService)) as IArticleService).GetListInstances(articleID, (object) sessionKeeper.Session.SessionGUID));
      for (int index = this._articles.Count - 1; index >= 0; --index)
      {
        if (!this._articlesToChange.ContainsKey(this._articles[index]))
          this._articles.RemoveAt(index);
      }
      foreach (KeyValuePair<long, MyElementEx> keyValuePair in this._articlesToChange)
      {
        if (this._articles.IndexOf(keyValuePair.Key) < 0)
          this._articles.Add(keyValuePair.Key);
      }
    }
  }

  protected virtual iGRow AddArticle(long articleID)
  {
    if (this.boldFont == null)
      this.boldFont = new Font(this.gridArticles.Font, FontStyle.Bold);
    if (articleID == 0L || !this._articlesToChange.ContainsKey(articleID))
      return (iGRow) null;
    MyElementEx myElementEx = this._articlesToChange[articleID];
    int num = this._objTypesIcons.IndexOf(4, myElementEx.ElementID32);
    iGRow iGrow = this.gridArticles.Rows.Add();
    iGrow.Key = articleID.ToString();
    if (myElementEx.ElementBool2)
    {
      for (int colIndex = 0; colIndex < iGrow.Cells.Count; ++colIndex)
        iGrow.Cells[colIndex].Font = this.boldFont;
    }
    iGrow.Cells[0].ImageIndex = num;
    iGrow.Cells[1].ImageIndex = myElementEx.ElementBool ? 1 : 0;
    iGrow.Cells[2].Value = myElementEx.Tags[0];
    iGrow.Cells[3].Value = myElementEx.Tags[1];
    iGrow.Cells[4].Value = (object) articleID;
    iGrow.Tag = (object) myElementEx;
    return iGrow;
  }

  protected virtual void FillArticlesGrid()
  {
    try
    {
      this.gridArticles.BeginUpdate();
      this.gridArticles.Redraw = false;
      this.gridArticles.Rows.Clear();
      this.gridArticles.SortObject.Add(2, iGSortOrder.Ascending);
      for (int index = 0; index < this._articles.Count; ++index)
        this.AddArticle(this._articles[index]);
      this.gridArticles.Sort();
    }
    finally
    {
      this.gridArticles.Redraw = true;
      this.gridArticles.EndUpdate();
      this.gridArticles_CurRowChanged((object) this, (EventArgs) null);
    }
  }

  internal static Bitmap PrepareBitmap(
    Color start,
    Color end,
    LinearGradientMode mode,
    Rectangle rectangle)
  {
    Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height);
    Graphics graphics = Graphics.FromImage((Image) bitmap);
    Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
    NavGradientBrush navGradientBrush = (ServicesManager.GetService(typeof (INavGraphicsCache)) as INavGraphicsCache).GetNavGradientBrush(start, end, mode, rect);
    if (navGradientBrush != null)
    {
      try
      {
        graphics.FillRectangle(navGradientBrush.Brush, rect);
      }
      finally
      {
        navGradientBrush.Dispose();
        graphics.Dispose();
      }
    }
    return bitmap;
  }

  protected virtual void PrepareLegend()
  {
    this.pictureAdded.Image = (Image) ArtSelectForm.PrepareBitmap(this._navGraphicsCache.CurrentColorsScheme.HintCellBkStartColor, this._navGraphicsCache.CurrentColorsScheme.HintCellBkEndColor, this._navGraphicsCache.CurrentColorsScheme.HintCellGradientMode, this.pictureAdded.ClientRectangle);
    this.pictureChanged.Image = (Image) ArtSelectForm.PrepareBitmap(this._navGraphicsCache.CurrentColorsScheme.InformationCellBkStartColor, this._navGraphicsCache.CurrentColorsScheme.InformationCellBkEndColor, this._navGraphicsCache.CurrentColorsScheme.InformationCellGradientMode, this.pictureChanged.ClientRectangle);
    this.pictureRemoved.Image = (Image) ArtSelectForm.PrepareBitmap(this._navGraphicsCache.CurrentColorsScheme.WarningCellBkStartColor, this._navGraphicsCache.CurrentColorsScheme.WarningCellBkEndColor, this._navGraphicsCache.CurrentColorsScheme.WarningCellGradientMode, this.pictureRemoved.ClientRectangle);
  }

  private void ArtSelectForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  protected virtual void UpdateControls()
  {
    this.mnpColumnsSetup.Enabled = this.articleComposition.SelectedItems != null && this.articleComposition.SelectedItems.Count > 0 && this._currentArticle != -1L;
    this.btnSelectAll.Enabled = this.gridArticles.Rows.Count > 0;
    this.btnDeselectAll.Enabled = this.btnSelectAll.Enabled;
  }

  public static DialogResult Execute(
    IServiceProvider viewServices,
    ref Dictionary<long, RelationAttributesPackage> relationsPackages,
    string filtrationOwnerID,
    int relationTypeID,
    List<long> contexts,
    List<NodeColumnID> advAttributes,
    long mainArticle)
  {
    using (ArtSelectForm artSelectForm = new ArtSelectForm(viewServices, ref relationsPackages, filtrationOwnerID, relationTypeID, contexts, advAttributes, mainArticle))
      return artSelectForm.ShowDialog();
  }

  private void btnOK_Click(object sender, EventArgs e)
  {
    this.UpdateControls();
    foreach (KeyValuePair<long, MyElementEx> keyValuePair in this._articlesToChange)
    {
      if (!keyValuePair.Value.ElementBool)
        this._relationsPackages.Remove(keyValuePair.Key);
    }
    this.DialogResult = DialogResult.OK;
  }

  protected virtual void LoadArticle(long newArticle)
  {
    this._currentArticle = newArticle;
    if (newArticle == -1L)
    {
      this.articleComposition.Deactivate((IView) null);
    }
    else
    {
      this._rootDescriptor = (IDescriptor) new SubstitutesDescriptor(PDMPluginConsts.CategorySubstitutes, 0, this._viewServices, this._filtrationOwnerID, this._contexts, newArticle, -1, this._relationTypeID, string.Empty, 0L, 0L, 0L, this._advAttributes);
      this.articleComposition.Initialize(this._rootDescriptor, this._viewServices);
      this.articleComposition.Activate((IView) null);
    }
  }

  private void DoCellMouseUp(object sender, iGCellMouseUpEventArgs e)
  {
    if (e.RowIndex >= this.gridArticles.Rows.Count || e.ColIndex != 1 || e.Button != MouseButtons.Left)
      return;
    iGRow row = this.gridArticles.Rows[e.RowIndex];
    MyElementEx tag = row.Tag as MyElementEx;
    if (!e.Bounds.Contains(e.MousePos) || tag.ElementBool2)
      return;
    tag.ElementBool = !tag.ElementBool;
    row.Cells[1].ImageIndex = tag.ElementBool ? 1 : 0;
  }

  private void DoShowContextMenu(object sender, ContextMenuEventArgs e)
  {
    this.contextMenuComposition.Show(e.Control, e.Location);
  }

  private void DoColumnsSetup(object sender, EventArgs e)
  {
    if (this._currentArticle == -1L || this.articleComposition.SelectedItems == null || this.articleComposition.SelectedItems.Count == 0)
      return;
    this.articleComposition.SetColumnsCommand(this.articleComposition.SelectedItems, this._viewServices, (object) null);
    this.UpdateControls();
  }

  private void contextMenuComposition_BeforePopup(object sender, MenuPopupEventArgs e)
  {
    this.UpdateControls();
  }

  private void gridArticles_CurRowChanged(object sender, EventArgs e)
  {
    iGRow curRow = this.gridArticles.CurRow;
    long newArticle = -1;
    MyElementEx tag = curRow != null ? curRow.Tag as MyElementEx : (MyElementEx) null;
    this.articleCaption.Text = tag != null ? string.Format(LocalizationHolder.rm.GetString("Pdm_246"), (object) tag.Caption) : LocalizationHolder.rm.GetString("Pdm_247");
    if (tag != null)
      newArticle = tag.ElementID64;
    if (this._currentArticle != newArticle)
      this.LoadArticle(newArticle);
    this.UpdateControls();
  }

  private void articleComposition_ShowCellCustomBackground(
    object sender,
    CustomCellBackgroundEventArgs e)
  {
    if (e.NodeID == null || !(e.NodeID is SubstitutesNodeID nodeId) || !this._relationsPackages.ContainsKey(this._currentArticle))
      return;
    RelationAttributesPackage relationsPackage = this._relationsPackages[this._currentArticle];
    if (!relationsPackage.Values.ContainsKey(nodeId.PrjLinkID))
      return;
    Color cellBkStartColor = this._navGraphicsCache.CurrentColorsScheme.InformationCellBkStartColor;
    Color endColor = this._navGraphicsCache.CurrentColorsScheme.InformationCellBkEndColor;
    LinearGradientMode cellGradientMode = this._navGraphicsCache.CurrentColorsScheme.InformationCellGradientMode;
    if (nodeId.SubstitutesGroupNoID > 0L && relationsPackage.Values[nodeId.PrjLinkID] == null)
    {
      cellBkStartColor = this._navGraphicsCache.CurrentColorsScheme.WarningCellBkStartColor;
      endColor = this._navGraphicsCache.CurrentColorsScheme.WarningCellBkEndColor;
      cellGradientMode = this._navGraphicsCache.CurrentColorsScheme.WarningCellGradientMode;
    }
    else if (nodeId.SubstitutesGroupNoID == 0L && Convert.ToInt64(relationsPackage[nodeId.PrjLinkID, this._substGroupNo]) > 0L)
    {
      cellBkStartColor = this._navGraphicsCache.CurrentColorsScheme.HintCellBkStartColor;
      endColor = this._navGraphicsCache.CurrentColorsScheme.HintCellBkEndColor;
      cellGradientMode = this._navGraphicsCache.CurrentColorsScheme.HintCellGradientMode;
    }
    Rectangle bounds = e.Cell.Bounds;
    NavGradientBrush navGradientBrush = this._navGraphicsCache.GetNavGradientBrush(cellBkStartColor, endColor, cellGradientMode, bounds);
    if (navGradientBrush == null)
      return;
    try
    {
      e.DrawArgs.Graphics.FillRectangle(navGradientBrush.Brush, bounds);
    }
    finally
    {
      navGradientBrush.Dispose();
    }
  }

  private void btnSelectAll_Click(object sender, EventArgs e)
  {
    for (int index = 0; index < this.gridArticles.Rows.Count; ++index)
    {
      iGRow row = this.gridArticles.Rows[index];
      MyElementEx tag = row.Tag as MyElementEx;
      tag.ElementBool = true;
      row.Cells[1].ImageIndex = tag.ElementBool ? 1 : 0;
    }
    this.UpdateControls();
  }

  private void btnDeselectAll_Click(object sender, EventArgs e)
  {
    for (int index = 0; index < this.gridArticles.Rows.Count; ++index)
    {
      iGRow row = this.gridArticles.Rows[index];
      MyElementEx tag = row.Tag as MyElementEx;
      if (!tag.ElementBool2)
      {
        tag.ElementBool = false;
        row.Cells[1].ImageIndex = tag.ElementBool ? 1 : 0;
      }
    }
    this.UpdateControls();
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ArtSelectForm));
    iGColPattern iGcolPattern1 = new iGColPattern();
    iGColPattern iGcolPattern2 = new iGColPattern();
    iGColPattern iGcolPattern3 = new iGColPattern();
    iGColPattern iGcolPattern4 = new iGColPattern();
    iGColPattern iGcolPattern5 = new iGColPattern();
    this.gridArticlesCol4CellStyle = new iGCellStyle(true);
    this.gridArticlesCol4ColHdrStyle = new iGColHdrStyle(true);
    this.splitContainer = new SplitContainer();
    this.gridArticles = new iGrid();
    this.tbComposition = new Intermech.Bars.ToolBar();
    this.imagesMenus = new ImageList(this.components);
    this.btnSelectAll = new ButtonItem();
    this.btnDeselectAll = new ButtonItem();
    this.headerControl = new HeaderControl();
    this.articleComposition = new SubstitutesView();
    this.menuComposition = new MenuBar();
    this.contextMenuComposition = new ContextMenuBarItem();
    this.mnpColumnsSetup = new MenuButtonItem();
    this.articleCompositionHeader = new StatusStrip();
    this.articleCaption = new ToolStripStatusLabel();
    this.panelBottom = new Panel();
    this.labelRemoved = new Label();
    this.pictureRemoved = new PictureBox();
    this.labelChanged = new Label();
    this.pictureChanged = new PictureBox();
    this.labelAdded = new Label();
    this.pictureAdded = new PictureBox();
    this.btnCancel = new Button();
    this.btnOK = new Button();
    this.imagesState = new ImageList(this.components);
    this.toolTip = new ToolTip(this.components);
    this.btnToActual = new ButtonItem();
    this.btnToSubstitute = new ButtonItem();
    this.btnAddToSubstitute2 = new ButtonItem();
    this.btnAddToNewActualSubstitute = new ButtonItem();
    this.btnAddToNewSubstitute = new ButtonItem();
    this.btnProperties = new ButtonItem();
    this.btnVirtualComposition = new ButtonItem();
    this.btnRefresh = new ButtonItem();
    this.splitContainer.BeginInit();
    this.splitContainer.Panel1.SuspendLayout();
    this.splitContainer.Panel2.SuspendLayout();
    this.splitContainer.SuspendLayout();
    ((ISupportInitialize) this.gridArticles).BeginInit();
    this.articleCompositionHeader.SuspendLayout();
    this.panelBottom.SuspendLayout();
    ((ISupportInitialize) this.pictureRemoved).BeginInit();
    ((ISupportInitialize) this.pictureChanged).BeginInit();
    ((ISupportInitialize) this.pictureAdded).BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.splitContainer, "splitContainer");
    this.splitContainer.Name = "splitContainer";
    componentResourceManager.ApplyResources((object) this.splitContainer.Panel1, "splitContainer.Panel1");
    this.splitContainer.Panel1.Controls.Add((Control) this.gridArticles);
    this.splitContainer.Panel1.Controls.Add((Control) this.tbComposition);
    this.splitContainer.Panel1.Controls.Add((Control) this.headerControl);
    this.toolTip.SetToolTip((Control) this.splitContainer.Panel1, componentResourceManager.GetString("splitContainer.Panel1.ToolTip"));
    componentResourceManager.ApplyResources((object) this.splitContainer.Panel2, "splitContainer.Panel2");
    this.splitContainer.Panel2.Controls.Add((Control) this.articleComposition);
    this.splitContainer.Panel2.Controls.Add((Control) this.menuComposition);
    this.splitContainer.Panel2.Controls.Add((Control) this.articleCompositionHeader);
    this.toolTip.SetToolTip((Control) this.splitContainer.Panel2, componentResourceManager.GetString("splitContainer.Panel2.ToolTip"));
    this.toolTip.SetToolTip((Control) this.splitContainer, componentResourceManager.GetString("splitContainer.ToolTip"));
    componentResourceManager.ApplyResources((object) this.gridArticles, "gridArticles");
    this.gridArticles.AutoResizeCols = true;
    this.gridArticles.BackColorEvenRows = Color.White;
    iGcolPattern1.AllowGrouping = false;
    iGcolPattern1.AllowMoving = false;
    iGcolPattern1.AllowSizing = false;
    componentResourceManager.ApplyResources((object) iGcolPattern1, "iGColPattern1");
    iGcolPattern1.SortOrder = iGSortOrder.None;
    iGcolPattern2.AllowGrouping = false;
    iGcolPattern2.AllowMoving = false;
    iGcolPattern2.AllowSizing = false;
    componentResourceManager.ApplyResources((object) iGcolPattern2, "iGColPattern2");
    iGcolPattern2.SortOrder = iGSortOrder.None;
    iGcolPattern3.AllowGrouping = false;
    iGcolPattern3.AllowMoving = false;
    componentResourceManager.ApplyResources((object) iGcolPattern3, "iGColPattern3");
    iGcolPattern4.AllowGrouping = false;
    iGcolPattern4.AllowMoving = false;
    iGcolPattern4.CellStyle = this.gridArticlesCol4CellStyle;
    iGcolPattern4.ColHdrStyle = this.gridArticlesCol4ColHdrStyle;
    componentResourceManager.ApplyResources((object) iGcolPattern4, "iGColPattern4");
    iGcolPattern5.AllowGrouping = false;
    iGcolPattern5.AllowMoving = false;
    componentResourceManager.ApplyResources((object) iGcolPattern5, "iGColPattern5");
    this.gridArticles.Cols.AddRange(new iGColPattern[5]
    {
      iGcolPattern1,
      iGcolPattern2,
      iGcolPattern3,
      iGcolPattern4,
      iGcolPattern5
    });
    this.gridArticles.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this.gridArticles.DefaultRow.Key = componentResourceManager.GetString("resource.Key");
    this.gridArticles.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    this.gridArticles.GroupBox.Text = componentResourceManager.GetString("gridArticles.GroupBox.Text");
    this.gridArticles.Header.Height = (int) componentResourceManager.GetObject("gridArticles.Header.Height");
    this.gridArticles.Name = "gridArticles";
    this.gridArticles.ReadOnly = true;
    this.gridArticles.RowMode = true;
    this.toolTip.SetToolTip((Control) this.gridArticles, componentResourceManager.GetString("gridArticles.ToolTip"));
    this.gridArticles.CellMouseUp += new iGCellMouseUpEventHandler(this.DoCellMouseUp);
    this.gridArticles.CurRowChanged += new EventHandler(this.gridArticles_CurRowChanged);
    componentResourceManager.ApplyResources((object) this.tbComposition, "tbComposition");
    this.tbComposition.AllowVerticalDock = false;
    this.tbComposition.Closable = false;
    this.tbComposition.DockLine = 3;
    this.tbComposition.FullMenus = true;
    this.tbComposition.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.tbComposition.Hidden = false;
    this.tbComposition.ImageList = this.imagesMenus;
    this.tbComposition.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnSelectAll,
      (ToolbarItemBase) this.btnDeselectAll
    });
    this.tbComposition.MinimumFloatingSize = new Size(250, 30);
    this.tbComposition.Movable = false;
    this.tbComposition.Name = "tbComposition";
    this.tbComposition.Overflow = ToolBarOverflow.Wrap;
    this.tbComposition.Stretch = true;
    this.toolTip.SetToolTip((Control) this.tbComposition, componentResourceManager.GetString("tbComposition.ToolTip"));
    this.imagesMenus.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesMenus.ImageStream");
    this.imagesMenus.TransparentColor = Color.Transparent;
    this.imagesMenus.Images.SetKeyName(0, "Настройка отображения.ico");
    this.imagesMenus.Images.SetKeyName(1, "uncheck_all.ico");
    this.imagesMenus.Images.SetKeyName(2, "check_all.ico");
    componentResourceManager.ApplyResources((object) this.btnSelectAll, "btnSelectAll");
    this.btnSelectAll.ImageIndex = 2;
    this.btnSelectAll.Locked = true;
    this.btnSelectAll.Click += new EventHandler(this.btnSelectAll_Click);
    componentResourceManager.ApplyResources((object) this.btnDeselectAll, "btnDeselectAll");
    this.btnDeselectAll.ImageIndex = 1;
    this.btnDeselectAll.Locked = true;
    this.btnDeselectAll.Click += new EventHandler(this.btnDeselectAll_Click);
    componentResourceManager.ApplyResources((object) this.headerControl, "headerControl");
    this.headerControl.BackColor = SystemColors.Control;
    this.headerControl.ForeColor = SystemColors.ControlText;
    this.headerControl.HeaderFont = new Font("Tahoma", 10f, FontStyle.Bold);
    this.headerControl.Name = "headerControl";
    this.toolTip.SetToolTip((Control) this.headerControl, componentResourceManager.GetString("headerControl.ToolTip"));
    componentResourceManager.ApplyResources((object) this.articleComposition, "articleComposition");
    this.articleComposition.AllowCustomGroupValues = true;
    this.articleComposition.BackColor = SystemColors.Control;
    this.articleComposition.Control = (object) this.articleComposition;
    this.articleComposition.DisableDelayedUpdates = true;
    this.articleComposition.DisableFiltration = true;
    this.articleComposition.DisableGroupBox = true;
    this.articleComposition.DisableHeaderContextMenu = true;
    this.articleComposition.DisableIMContextMenu = true;
    this.articleComposition.DisableKeyDownEvents = false;
    this.articleComposition.DisableStatusBar = true;
    this.articleComposition.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.articleComposition.Name = "articleComposition";
    this.articleComposition.Remarks = (RelationAttributesPackage) null;
    this.articleComposition.SubstitutesVirtual = (SubstituteObjects) null;
    this.toolTip.SetToolTip((Control) this.articleComposition, componentResourceManager.GetString("articleComposition.ToolTip"));
    this.articleComposition.ShowCellCustomBackground += new CustomCellBackgroundEventHandler(this.articleComposition_ShowCellCustomBackground);
    this.articleComposition.ShowCustomContextMenu += new EventHandler<ContextMenuEventArgs>(this.DoShowContextMenu);
    componentResourceManager.ApplyResources((object) this.menuComposition, "menuComposition");
    this.menuComposition.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuComposition.Hidden = false;
    this.menuComposition.ImageList = this.imagesMenus;
    this.menuComposition.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuComposition
    });
    this.menuComposition.Name = "menuComposition";
    this.menuComposition.OwnerForm = (Form) this;
    this.toolTip.SetToolTip((Control) this.menuComposition, componentResourceManager.GetString("menuComposition.ToolTip"));
    componentResourceManager.ApplyResources((object) this.contextMenuComposition, "contextMenuComposition");
    this.contextMenuComposition.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.mnpColumnsSetup
    });
    this.contextMenuComposition.ShowText = true;
    this.contextMenuComposition.BeforePopup += new MenuItemBase.BeforePopupEventHandler(this.contextMenuComposition_BeforePopup);
    componentResourceManager.ApplyResources((object) this.mnpColumnsSetup, "mnpColumnsSetup");
    this.mnpColumnsSetup.ImageIndex = 0;
    this.mnpColumnsSetup.ShowText = true;
    this.mnpColumnsSetup.Click += new EventHandler(this.DoColumnsSetup);
    componentResourceManager.ApplyResources((object) this.articleCompositionHeader, "articleCompositionHeader");
    this.articleCompositionHeader.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.articleCaption
    });
    this.articleCompositionHeader.Name = "articleCompositionHeader";
    this.articleCompositionHeader.SizingGrip = false;
    this.toolTip.SetToolTip((Control) this.articleCompositionHeader, componentResourceManager.GetString("articleCompositionHeader.ToolTip"));
    componentResourceManager.ApplyResources((object) this.articleCaption, "articleCaption");
    this.articleCaption.Name = "articleCaption";
    this.articleCaption.Spring = true;
    componentResourceManager.ApplyResources((object) this.panelBottom, "panelBottom");
    this.panelBottom.BorderStyle = BorderStyle.Fixed3D;
    this.panelBottom.Controls.Add((Control) this.labelRemoved);
    this.panelBottom.Controls.Add((Control) this.pictureRemoved);
    this.panelBottom.Controls.Add((Control) this.labelChanged);
    this.panelBottom.Controls.Add((Control) this.pictureChanged);
    this.panelBottom.Controls.Add((Control) this.labelAdded);
    this.panelBottom.Controls.Add((Control) this.pictureAdded);
    this.panelBottom.Controls.Add((Control) this.btnCancel);
    this.panelBottom.Controls.Add((Control) this.btnOK);
    this.panelBottom.Name = "panelBottom";
    this.toolTip.SetToolTip((Control) this.panelBottom, componentResourceManager.GetString("panelBottom.ToolTip"));
    componentResourceManager.ApplyResources((object) this.labelRemoved, "labelRemoved");
    this.labelRemoved.BackColor = SystemColors.Control;
    this.labelRemoved.Name = "labelRemoved";
    this.toolTip.SetToolTip((Control) this.labelRemoved, componentResourceManager.GetString("labelRemoved.ToolTip"));
    componentResourceManager.ApplyResources((object) this.pictureRemoved, "pictureRemoved");
    this.pictureRemoved.BorderStyle = BorderStyle.FixedSingle;
    this.pictureRemoved.Name = "pictureRemoved";
    this.pictureRemoved.TabStop = false;
    this.toolTip.SetToolTip((Control) this.pictureRemoved, componentResourceManager.GetString("pictureRemoved.ToolTip"));
    componentResourceManager.ApplyResources((object) this.labelChanged, "labelChanged");
    this.labelChanged.BackColor = SystemColors.Control;
    this.labelChanged.Name = "labelChanged";
    this.toolTip.SetToolTip((Control) this.labelChanged, componentResourceManager.GetString("labelChanged.ToolTip"));
    componentResourceManager.ApplyResources((object) this.pictureChanged, "pictureChanged");
    this.pictureChanged.BorderStyle = BorderStyle.FixedSingle;
    this.pictureChanged.Name = "pictureChanged";
    this.pictureChanged.TabStop = false;
    this.toolTip.SetToolTip((Control) this.pictureChanged, componentResourceManager.GetString("pictureChanged.ToolTip"));
    componentResourceManager.ApplyResources((object) this.labelAdded, "labelAdded");
    this.labelAdded.BackColor = SystemColors.Control;
    this.labelAdded.Name = "labelAdded";
    this.toolTip.SetToolTip((Control) this.labelAdded, componentResourceManager.GetString("labelAdded.ToolTip"));
    componentResourceManager.ApplyResources((object) this.pictureAdded, "pictureAdded");
    this.pictureAdded.BorderStyle = BorderStyle.FixedSingle;
    this.pictureAdded.Name = "pictureAdded";
    this.pictureAdded.TabStop = false;
    this.toolTip.SetToolTip((Control) this.pictureAdded, componentResourceManager.GetString("pictureAdded.ToolTip"));
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Cursor = Cursors.Hand;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    this.toolTip.SetToolTip((Control) this.btnCancel, componentResourceManager.GetString("btnCancel.ToolTip"));
    componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
    this.btnOK.Cursor = Cursors.Hand;
    this.btnOK.Name = "btnOK";
    this.toolTip.SetToolTip((Control) this.btnOK, componentResourceManager.GetString("btnOK.ToolTip"));
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.imagesState.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesState.ImageStream");
    this.imagesState.TransparentColor = Color.Transparent;
    this.imagesState.Images.SetKeyName(0, "unchecked.ico");
    this.imagesState.Images.SetKeyName(1, "checked.ico");
    this.imagesState.Images.SetKeyName(2, "grayed.ico");
    componentResourceManager.ApplyResources((object) this.btnToActual, "btnToActual");
    this.btnToActual.ImageIndex = 0;
    this.btnToActual.Locked = true;
    componentResourceManager.ApplyResources((object) this.btnToSubstitute, "btnToSubstitute");
    this.btnToSubstitute.ImageIndex = 1;
    this.btnToSubstitute.Locked = true;
    this.btnAddToSubstitute2.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnAddToSubstitute2, "btnAddToSubstitute2");
    this.btnAddToSubstitute2.Locked = true;
    this.btnAddToSubstitute2.Visible = false;
    this.btnAddToNewActualSubstitute.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnAddToNewActualSubstitute, "btnAddToNewActualSubstitute");
    this.btnAddToNewActualSubstitute.Locked = true;
    this.btnAddToNewActualSubstitute.Visible = false;
    componentResourceManager.ApplyResources((object) this.btnAddToNewSubstitute, "btnAddToNewSubstitute");
    this.btnAddToNewSubstitute.Locked = true;
    this.btnAddToNewSubstitute.Visible = false;
    this.btnProperties.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnProperties, "btnProperties");
    this.btnProperties.ImageIndex = 2;
    this.btnProperties.Locked = true;
    this.btnVirtualComposition.AutoToggle = AutoToggleType.Single;
    this.btnVirtualComposition.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnVirtualComposition, "btnVirtualComposition");
    this.btnVirtualComposition.ImageIndex = 3;
    this.btnVirtualComposition.Locked = true;
    this.btnRefresh.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnRefresh, "btnRefresh");
    this.btnRefresh.Enabled = false;
    this.btnRefresh.ImageIndex = 4;
    this.btnRefresh.Locked = true;
    this.btnRefresh.Visible = false;
    this.AcceptButton = (IButtonControl) this.btnOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.Controls.Add((Control) this.splitContainer);
    this.Controls.Add((Control) this.panelBottom);
    this.Name = nameof (ArtSelectForm);
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.Tag = (object) " ";
    this.toolTip.SetToolTip((Control) this, componentResourceManager.GetString("$this.ToolTip"));
    this.FormClosed += new FormClosedEventHandler(this.ArtSelectForm_FormClosed);
    this.splitContainer.Panel1.ResumeLayout(false);
    this.splitContainer.Panel2.ResumeLayout(false);
    this.splitContainer.Panel2.PerformLayout();
    this.splitContainer.EndInit();
    this.splitContainer.ResumeLayout(false);
    ((ISupportInitialize) this.gridArticles).EndInit();
    this.articleCompositionHeader.ResumeLayout(false);
    this.articleCompositionHeader.PerformLayout();
    this.panelBottom.ResumeLayout(false);
    this.panelBottom.PerformLayout();
    ((ISupportInitialize) this.pictureRemoved).EndInit();
    ((ISupportInitialize) this.pictureChanged).EndInit();
    ((ISupportInitialize) this.pictureAdded).EndInit();
    this.ResumeLayout(false);
  }
}
