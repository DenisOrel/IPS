// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ArticlesCreatorForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using ImSSP;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.NavBars;
using Intermech.Navigator;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Pdm;

public class ArticlesCreatorForm : Form
{
  protected long _prototypeID;
  protected string _defMainDesign = string.Empty;
  protected string _artName = string.Empty;
  protected List<ArticlesPair> _articles = new List<ArticlesPair>();
  protected List<long> _newObjects = new List<long>();
  protected INavGraphicsCache _navGraphicsCache;
  protected ICategoryTypeIconService _objTypesIcons;
  protected IPDMSpecificationsService _specService;
  private IContainer components;
  private Panel panelBottom;
  private Label labelCheckedOutOther;
  private PictureBox pictureCheckedOutOther;
  private Label labelCheckedOut;
  private PictureBox pictureCheckedOut;
  private Button btnCancel;
  private Button btnOK;
  private iGrid gridArticles;
  private HeaderControl headerControl;
  private ImageList imagesState;
  private iGCellStyleDesign iGCellStyleDesign1;
  private iGCellStyleDesign iGCellStyleDesign2;
  private iGCellStyleDesign iGCellStyleDesign3;
  private iGCellStyleDesign iGCellStyleDesign4;

  public ArticlesCreatorForm()
    : this(0L, string.Empty, string.Empty)
  {
  }

  public ArticlesCreatorForm(long prototypeID, string defMainDesign, string artName)
  {
    this.InitializeComponent();
    this.Init(prototypeID, defMainDesign, artName);
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1505);
  }

  public void Init(long prototypeID, string defMainDesign, string artName)
  {
    this._prototypeID = prototypeID;
    this._defMainDesign = defMainDesign;
    this._artName = artName;
    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
    this.Location = new Point((workingArea.Width - this.Size.Width) / 2, (workingArea.Height - this.Size.Height) / 2);
    FormStorage.LoadLayout((Control) this);
    this._objTypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._navGraphicsCache = ServicesManager.GetService(typeof (INavGraphicsCache)) as INavGraphicsCache;
    this._specService = ServicesManager.GetService(typeof (IPDMSpecificationsService)) as IPDMSpecificationsService;
    this.gridArticles.Cols[0].CellStyle.ImageList = this._objTypesIcons.ImageList;
    this.gridArticles.Cols[1].CellStyle.ImageList = this.imagesState;
    this.PrepareLegend();
    if (!this.DesignMode)
      this._articles = this.ReadArticlePairs(prototypeID);
    this._articles.Sort();
    this.FillArticlesGrid(this._articles);
  }

  public static DialogResult Execute(
    long prototypeID,
    List<long> newObjects,
    string defMainDesign,
    string articlesName)
  {
    using (ArticlesCreatorForm articlesCreatorForm = new ArticlesCreatorForm(prototypeID, defMainDesign, articlesName))
    {
      if (articlesCreatorForm._articles.Count == 0)
        return DialogResult.Abort;
      DialogResult dialogResult = articlesCreatorForm.ShowDialog();
      if (newObjects != null)
      {
        for (int index = 0; index < articlesCreatorForm._newObjects.Count; ++index)
        {
          if (!newObjects.Contains(articlesCreatorForm._newObjects[index]))
            newObjects.Add(articlesCreatorForm._newObjects[index]);
        }
      }
      if (articlesCreatorForm._newObjects.Count > 0)
        (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", (IList<long>) articlesCreatorForm._newObjects));
      return dialogResult;
    }
  }

  protected virtual void UpdateControls()
  {
    bool flag = true;
    for (int index = 0; index < this._articles.Count; ++index)
      flag &= !this._articles[index].NewTemplateEnabled;
    this.btnOK.Enabled = this._articles.Count > 0 && !flag;
    this.btnCancel.Enabled = true;
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
    this.pictureCheckedOut.Image = (Image) ArticlesCreatorForm.PrepareBitmap(this._navGraphicsCache.CurrentColorsScheme.CheckedOutBkStartColor, (this._navGraphicsCache.CurrentColorsScheme.Gradient & GradientUsing.CheckOut) == GradientUsing.CheckOut ? this._navGraphicsCache.CurrentColorsScheme.CheckedOutBkEndColor : this._navGraphicsCache.CurrentColorsScheme.CheckedOutBkStartColor, this._navGraphicsCache.CurrentColorsScheme.CheckedOutGradientMode, this.pictureCheckedOut.ClientRectangle);
    this.pictureCheckedOutOther.Image = (Image) ArticlesCreatorForm.PrepareBitmap(this._navGraphicsCache.CurrentColorsScheme.CheckedOutOtherBkStartColor, (this._navGraphicsCache.CurrentColorsScheme.Gradient & GradientUsing.CheckedOutOther) == GradientUsing.CheckedOutOther ? this._navGraphicsCache.CurrentColorsScheme.CheckedOutOtherBkEndColor : this._navGraphicsCache.CurrentColorsScheme.CheckedOutOtherBkStartColor, this._navGraphicsCache.CurrentColorsScheme.CheckedOutOtherGradientMode, this.pictureCheckedOutOther.ClientRectangle);
  }

  private void ArticlesCreatorForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  protected virtual List<ArticlesPair> ReadArticlePairs(long articleID)
  {
    List<ArticlesPair> articlesPairList = new List<ArticlesPair>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IArticleService)) is IArticleService customService))
        return articlesPairList;
      IDBObject dbObject = sessionKeeper.Session.GetObject(articleID, false);
      if (dbObject == null)
        return articlesPairList;
      object obj = dbObject.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"))?.Value;
      string oldValue = obj == null || obj == DBNull.Value ? string.Empty : Convert.ToString(obj);
      SortedDictionary<string, long> sortedDictionary = new SortedDictionary<string, long>();
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      long[] articlesByGroupId = customService.FindArticlesByGroupID(articleID, (object) sessionKeeper.Session.SessionGUID);
      if (articlesByGroupId != null)
      {
        for (int index = 0; index < articlesByGroupId.Length; ++index)
        {
          IDBObject artObject = sessionKeeper.Session.GetObject(articlesByGroupId[index], false);
          if (artObject != null)
          {
            IDBAttribute attributeById = artObject.GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545"));
            if (attributeById != null)
            {
              string asString = attributeById.AsString;
              sortedDictionary[asString] = articlesByGroupId[index];
              dictionary[asString] = asString;
            }
            ArticlesPair articlesPair = new ArticlesPair(artObject, this._defMainDesign);
            articlesPairList.Add(articlesPair);
          }
        }
      }
      int num1 = 0;
      using (SortedDictionary<string, long>.Enumerator enumerator = sortedDictionary.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          KeyValuePair<string, long> current = enumerator.Current;
          if (num1 == 0)
            oldValue = current.Key;
        }
      }
      List<string> stringList = new List<string>(articlesPairList.Count * 2);
      for (int index = 0; index < articlesPairList.Count; ++index)
        stringList.Add(articlesPairList[index].TemplateArticleDesignation);
      for (int index = 0; index < articlesPairList.Count; ++index)
      {
        if (!string.IsNullOrEmpty(this._defMainDesign) && !string.IsNullOrEmpty(oldValue))
        {
          string articleDesignation = articlesPairList[index].TemplateArticleDesignation;
          if (articleDesignation.IndexOf(oldValue) == 0)
            articlesPairList[index].NewTemplateDesignation = articleDesignation.Replace(oldValue, this._defMainDesign);
          else if (articlesPairList[index].TemplateArticleDesignation == oldValue)
            articlesPairList[index].NewTemplateDesignation = this._defMainDesign;
          string str = articlesPairList[index].NewTemplateDesignation;
          int num2 = 1;
          while (stringList.IndexOf(str) >= 0)
          {
            str = $"{articlesPairList[index].NewTemplateDesignation}.{num2}";
            ++num2;
          }
          articlesPairList[index].NewTemplateDesignation = str;
          stringList.Add(str);
        }
      }
    }
    return articlesPairList;
  }

  protected virtual void FillArticlesGrid(List<ArticlesPair> articles)
  {
    try
    {
      this.gridArticles.BeginUpdate();
      this.gridArticles.Redraw = false;
      this.gridArticles.Rows.Clear();
      if (articles == null)
        return;
      for (int index = 0; index < articles.Count; ++index)
      {
        ArticlesPair article = articles[index];
        int num = this._objTypesIcons.IndexOf(4, article.TemplateArticleTypeID);
        iGRow iGrow = this.gridArticles.Rows.Add();
        iGrow.Key = article.TemplateArticleID.ToString();
        iGrow.Cells[0].ImageIndex = num;
        iGrow.Cells[1].ImageIndex = article.NewTemplateEnabled ? 1 : 0;
        iGrow.Cells[2].Value = (object) article.TemplateArticleDesignation;
        iGrow.Cells[3].Value = (object) article.NewTemplateDesignation;
        iGrow.Tag = (object) article;
      }
    }
    finally
    {
      this.gridArticles.Redraw = true;
      this.gridArticles.EndUpdate();
    }
  }

  private void gridArticles_DynamicForeColor(object sender, iGDynamicColorEventArgs e)
  {
    iGRow row = e.RowIndex >= 0 ? this.gridArticles.Rows[e.RowIndex] : (iGRow) null;
    if (e.ColIndex >= 0 && row != null)
    {
      iGCell cell = row.Cells[e.ColIndex];
    }
    ArticlesPair tag = row != null ? row.Tag as ArticlesPair : (ArticlesPair) null;
    Color color = e.Color;
    if (tag != null && !tag.NewTemplateEnabled)
      color = SystemColors.GrayText;
    if (tag.NewTemplateEnabled)
      return;
    e.Color = color;
  }

  private void gridArticles_DynamicFont(object sender, iGDynamicFontEventArgs e)
  {
    iGRow row = e.RowIndex >= 0 ? this.gridArticles.Rows[e.RowIndex] : (iGRow) null;
    iGCell cell = e.ColIndex < 0 || row == null ? (iGCell) null : row.Cells[e.ColIndex];
    ArticlesPair tag = row != null ? row.Tag as ArticlesPair : (ArticlesPair) null;
    if (tag == null || !tag.TemplateArticleIsMain || cell == null || cell.ColIndex != 2)
      return;
    e.Font = new Font(e.Font ?? cell.EffectiveFont, FontStyle.Bold);
  }

  private void gridArticles_CustomDrawCellBackground(object sender, iGCustomDrawCellEventArgs e)
  {
    iGRow row = e.RowIndex >= 0 ? this.gridArticles.Rows[e.RowIndex] : (iGRow) null;
    if (e.ColIndex >= 0 && row != null)
    {
      iGCell cell = row.Cells[e.ColIndex];
    }
    ArticlesPair tag = row != null ? row.Tag as ArticlesPair : (ArticlesPair) null;
    if (tag == null || tag.TemplateArticleCheckedOutBy == 0L)
      return;
    NavGradientBrush navGradientBrush = (NavGradientBrush) null;
    Rectangle bounds = e.Bounds;
    if (ServicesManager.GetService(typeof (ICurrentUserAndRole)) is ICurrentUserAndRole service)
    {
      if (tag.TemplateArticleCheckedOutBy != service.UserID && tag.TemplateArticleCheckedOutBy != 0L)
      {
        bool useGradient = (this._navGraphicsCache.CurrentColorsScheme.Gradient & GradientUsing.CheckedOutOther) == GradientUsing.CheckedOutOther;
        navGradientBrush = this._navGraphicsCache.GetNavGradientBrush(this._navGraphicsCache.CurrentColorsScheme.CheckedOutOtherBkStartColor, this._navGraphicsCache.CurrentColorsScheme.CheckedOutOtherBkEndColor, this._navGraphicsCache.CurrentColorsScheme.CheckedOutOtherGradientMode, bounds, useGradient);
      }
      if (tag.TemplateArticleID < 0L && tag.TemplateArticleCheckedOutBy == service.UserID)
      {
        bool useGradient = (this._navGraphicsCache.CurrentColorsScheme.Gradient & GradientUsing.CheckOut) == GradientUsing.CheckOut;
        navGradientBrush = this._navGraphicsCache.GetNavGradientBrush(this._navGraphicsCache.CurrentColorsScheme.CheckedOutBkStartColor, this._navGraphicsCache.CurrentColorsScheme.CheckedOutBkEndColor, this._navGraphicsCache.CurrentColorsScheme.CheckedOutGradientMode, bounds, useGradient);
      }
    }
    if (navGradientBrush == null)
      return;
    try
    {
      e.Graphics.FillRectangle(navGradientBrush.Brush, bounds);
    }
    finally
    {
      navGradientBrush.Dispose();
    }
  }

  private void gridArticles_CustomDrawCellForeground(object sender, iGCustomDrawCellEventArgs e)
  {
    Region clip = e.Graphics.Clip;
    try
    {
      e.Graphics.SetClip(e.Bounds);
      iGRow row = e.RowIndex >= 0 ? this.gridArticles.Rows[e.RowIndex] : (iGRow) null;
      if (row != null)
      {
        iGCol col = this.gridArticles.Cols[e.ColIndex];
      }
      ArticlesPair tag = row != null ? row.Tag as ArticlesPair : (ArticlesPair) null;
      if (tag == null)
        return;
      int colIndex = e.ColIndex;
      if (!(this.gridArticles.Cols[e.ColIndex].Key == "IMAGE") || tag == null)
        return;
      e.Graphics.DrawIcon(tag.TemplateArticleTypeIcon, e.Bounds.Left + 3, e.Bounds.Top + 1);
    }
    finally
    {
      e.Graphics.ResetClip();
      e.Graphics.Clip = clip;
    }
  }

  private void gridArticles_CellMouseUp(object sender, iGCellMouseUpEventArgs e)
  {
    if (this.gridArticles.IsEditing || e.RowIndex >= this.gridArticles.Rows.Count || e.ColIndex != 1 || e.Button != MouseButtons.Left)
      return;
    iGRow row = this.gridArticles.Rows[e.RowIndex];
    iGCell cell = e.ColIndex < 0 || row == null ? (iGCell) null : row.Cells[e.ColIndex];
    ArticlesPair tag = row != null ? row.Tag as ArticlesPair : (ArticlesPair) null;
    if (tag == null || cell == null)
      return;
    Rectangle bounds = e.Bounds;
    int left = bounds.Left;
    bounds = e.Bounds;
    int num1 = (bounds.Width - this.imagesState.ImageSize.Width) / 2;
    int num2 = left + num1;
    bounds = e.Bounds;
    int top = bounds.Top;
    bounds = e.Bounds;
    int height1 = bounds.Height;
    Size imageSize = this.imagesState.ImageSize;
    int height2 = imageSize.Height;
    int num3 = (height1 - height2) / 2;
    int num4 = top + num3;
    Rectangle rectangle;
    ref Rectangle local = ref rectangle;
    int x = num2;
    int y = num4;
    imageSize = this.imagesState.ImageSize;
    int width = imageSize.Width;
    imageSize = this.imagesState.ImageSize;
    int height3 = imageSize.Height;
    local = new Rectangle(x, y, width, height3);
    if (!rectangle.Contains(e.MousePos))
      return;
    tag.NewTemplateEnabled = !tag.NewTemplateEnabled;
    cell.ImageIndex = tag.NewTemplateEnabled ? 1 : 0;
    row.Cells[3].ReadOnly = tag.NewTemplateEnabled ? iGBool.False : iGBool.True;
    this.gridArticles.Invalidate(e.Bounds);
    this.UpdateControls();
  }

  private void gridArticles_BeforeCommitEdit(object sender, iGBeforeCommitEditEventArgs e)
  {
    if (e.ColIndex != 3)
      return;
    iGRow row = this.gridArticles.Rows[e.RowIndex];
    ArticlesPair tag = row != null ? row.Tag as ArticlesPair : (ArticlesPair) null;
    if (tag == null)
      return;
    string designation = e.NewValue != null ? e.NewValue.ToString() : string.Empty;
    if (designation.Trim().ToUpperInvariant() == string.Empty)
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString(sc_16489.ssp_pdm_16490()), LocalizationHolder.rm.GetString("Pdm_508"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      e.Result = iGEditResult.Proceed;
    }
    else if (designation.Trim().ToUpperInvariant() == tag.TemplateArticleDesignation.Trim().ToUpperInvariant())
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString(sc_16489.ssp_pdm_16491()), LocalizationHolder.rm.GetString("Pdm_508"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      e.Result = iGEditResult.Proceed;
    }
    else if (this._specService.GetObjectWithDesignation(tag.TemplateArticleTypeID, designation) != 0L)
    {
      int num = (int) MessageBox.Show(string.Format(LocalizationHolder.rm.GetString(sc_16489.ssp_pdm_16492()), (object) MetaDataHelper.GetObjectTypeName(tag.TemplateArticleTypeID)), LocalizationHolder.rm.GetString("Pdm_508"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      e.Result = iGEditResult.Proceed;
    }
    else
    {
      tag.NewTemplateDesignation = designation;
      this.UpdateControls();
    }
  }

  private void gridArticles_AfterCommitEdit(object sender, iGAfterCommitEditEventArgs e)
  {
  }

  private void gridArticles_CurCellChanged(object sender, EventArgs e) => this.UpdateControls();

  private void btnOK_Click(object sender, EventArgs e)
  {
    if (!this.TryCreateArticles())
      return;
    this.DialogResult = DialogResult.OK;
  }

  protected virtual bool TryCreateArticles()
  {
    bool articles = false;
    Exception exception = (Exception) null;
    ProgressForm progressForm = ProgressForm.Execute(LocalizationHolder.rm.GetString("Pdm_512"), LocalizationHolder.rm.GetString("Pdm_513"), 0, this._articles.Count - 1, false, string.Empty, (EventHandler) null);
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBTransactions customService = sessionKeeper.Session.GetCustomService(typeof (IDBTransactions)) as IDBTransactions;
        for (int index = 0; index < this._articles.Count; ++index)
          this._articles[index].NewTemplateID = 0L;
        this._newObjects.Clear();
        int attributeTypeId1 = MetaDataHelper.GetAttributeTypeID("cad001f9-306c-11d8-b4e9-00304f19f545");
        int attributeTypeId2 = MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545");
        int attributeTypeId3 = MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545");
        try
        {
          customService?.StartTransaction();
          try
          {
            List<ArticlesPair> articlesPairList = new List<ArticlesPair>(this._articles.Count);
            Guid initValue = Guid.NewGuid();
            for (int index = 0; index < this._articles.Count; ++index)
            {
              if (this._articles[index].NewTemplateEnabled)
              {
                if (this._articles[index].TemplateArticleIsMain)
                  articlesPairList.Insert(0, this._articles[index]);
                else
                  articlesPairList.Add(this._articles[index]);
              }
            }
            for (int index = 0; index < articlesPairList.Count; ++index)
            {
              progressForm.SetProgressValue(index);
              if (articlesPairList[index].NewTemplateEnabled)
              {
                IDBObject dbObject = sessionKeeper.Session.GetObjectCollection(articlesPairList[index].TemplateArticleTypeID).Create(articlesPairList[index].TemplateArticleID);
                if (dbObject.ObjectModifyMode == ObjectModifyModes.Checkout && dbObject.CheckoutBy == 0L)
                  dbObject = dbObject.CheckOut();
                dbObject.SetAttributesValues(new AttributeValues[3]
                {
                  new AttributeValues(attributeTypeId1, (object) initValue),
                  new AttributeValues(attributeTypeId2, (object) articlesPairList[index].NewTemplateDesignation),
                  new AttributeValues(attributeTypeId3, this._artName == null || !(this._artName != "") ? (object) articlesPairList[index].TemplateArticleName : (object) this._artName)
                });
                if (dbObject.IsCreationMode)
                  dbObject.CommitCreation(true, true);
                articlesPairList[index].NewTemplateID = dbObject.ObjectID;
                this._newObjects.Add(articlesPairList[index].NewTemplateID);
              }
            }
            articles = true;
          }
          catch (Exception ex)
          {
            exception = ex;
            for (int index = 0; index < this._articles.Count; ++index)
              this._articles[index].NewTemplateID = 0L;
            this._newObjects.Clear();
          }
        }
        finally
        {
          if (customService != null)
          {
            if (articles)
              customService.Commit();
            else
              customService.Rollback();
          }
        }
      }
    }
    finally
    {
      progressForm.CanCloseForm = true;
      progressForm.Close();
      progressForm.Dispose();
    }
    if (exception == null)
      return articles;
    ExceptionHelper.ExceptionService.ShowException(exception);
    return false;
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ArticlesCreatorForm));
    iGColPattern iGcolPattern1 = new iGColPattern();
    iGColPattern iGcolPattern2 = new iGColPattern();
    iGColPattern iGcolPattern3 = new iGColPattern();
    iGColPattern iGcolPattern4 = new iGColPattern();
    this.iGCellStyleDesign1 = new iGCellStyleDesign();
    this.iGCellStyleDesign2 = new iGCellStyleDesign();
    this.iGCellStyleDesign3 = new iGCellStyleDesign();
    this.iGCellStyleDesign4 = new iGCellStyleDesign();
    this.panelBottom = new Panel();
    this.labelCheckedOutOther = new Label();
    this.pictureCheckedOutOther = new PictureBox();
    this.labelCheckedOut = new Label();
    this.pictureCheckedOut = new PictureBox();
    this.btnCancel = new Button();
    this.btnOK = new Button();
    this.gridArticles = new iGrid();
    this.headerControl = new HeaderControl();
    this.imagesState = new ImageList(this.components);
    this.panelBottom.SuspendLayout();
    ((ISupportInitialize) this.pictureCheckedOutOther).BeginInit();
    ((ISupportInitialize) this.pictureCheckedOut).BeginInit();
    ((ISupportInitialize) this.gridArticles).BeginInit();
    this.SuspendLayout();
    this.iGCellStyleDesign1.CustomDrawFlags = iGCustomDrawFlags.Foreground | iGCustomDrawFlags.Background;
    this.iGCellStyleDesign1.ImageAlign = iGContentAlignment.MiddleCenter;
    this.iGCellStyleDesign1.ReadOnly = iGBool.True;
    this.iGCellStyleDesign2.CustomDrawFlags = iGCustomDrawFlags.Background;
    this.iGCellStyleDesign2.ImageAlign = iGContentAlignment.MiddleCenter;
    this.iGCellStyleDesign2.ReadOnly = iGBool.True;
    this.iGCellStyleDesign3.CustomDrawFlags = iGCustomDrawFlags.Background;
    this.iGCellStyleDesign3.ReadOnly = iGBool.True;
    this.iGCellStyleDesign4.ReadOnly = iGBool.False;
    this.iGCellStyleDesign4.Type = iGCellType.Text;
    this.iGCellStyleDesign4.ValueType = typeof (string);
    this.panelBottom.BorderStyle = BorderStyle.Fixed3D;
    this.panelBottom.Controls.Add((Control) this.labelCheckedOutOther);
    this.panelBottom.Controls.Add((Control) this.pictureCheckedOutOther);
    this.panelBottom.Controls.Add((Control) this.labelCheckedOut);
    this.panelBottom.Controls.Add((Control) this.pictureCheckedOut);
    this.panelBottom.Controls.Add((Control) this.btnCancel);
    this.panelBottom.Controls.Add((Control) this.btnOK);
    componentResourceManager.ApplyResources((object) this.panelBottom, "panelBottom");
    this.panelBottom.Name = "panelBottom";
    componentResourceManager.ApplyResources((object) this.labelCheckedOutOther, "labelCheckedOutOther");
    this.labelCheckedOutOther.Name = "labelCheckedOutOther";
    this.pictureCheckedOutOther.BorderStyle = BorderStyle.FixedSingle;
    componentResourceManager.ApplyResources((object) this.pictureCheckedOutOther, "pictureCheckedOutOther");
    this.pictureCheckedOutOther.Name = "pictureCheckedOutOther";
    this.pictureCheckedOutOther.TabStop = false;
    componentResourceManager.ApplyResources((object) this.labelCheckedOut, "labelCheckedOut");
    this.labelCheckedOut.Name = "labelCheckedOut";
    this.pictureCheckedOut.BorderStyle = BorderStyle.FixedSingle;
    componentResourceManager.ApplyResources((object) this.pictureCheckedOut, "pictureCheckedOut");
    this.pictureCheckedOut.Name = "pictureCheckedOut";
    this.pictureCheckedOut.TabStop = false;
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Cursor = Cursors.Hand;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
    this.btnOK.Cursor = Cursors.Hand;
    this.btnOK.Name = "btnOK";
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.gridArticles.BackColorEvenRows = Color.White;
    iGcolPattern1.AllowGrouping = false;
    iGcolPattern1.AllowMoving = false;
    iGcolPattern1.AllowSizing = false;
    iGcolPattern1.CellStyle = (iGCellStyle) this.iGCellStyleDesign1;
    iGcolPattern1.IncludeInSelect = false;
    componentResourceManager.ApplyResources((object) iGcolPattern1, "iGColPattern5");
    iGcolPattern1.SortOrder = iGSortOrder.None;
    iGcolPattern2.AllowGrouping = false;
    iGcolPattern2.AllowMoving = false;
    iGcolPattern2.AllowSizing = false;
    iGcolPattern2.CellStyle = (iGCellStyle) this.iGCellStyleDesign2;
    iGcolPattern2.IncludeInSelect = false;
    componentResourceManager.ApplyResources((object) iGcolPattern2, "iGColPattern6");
    iGcolPattern2.SortOrder = iGSortOrder.None;
    iGcolPattern3.AllowGrouping = false;
    iGcolPattern3.AllowMoving = false;
    iGcolPattern3.CellStyle = (iGCellStyle) this.iGCellStyleDesign3;
    componentResourceManager.ApplyResources((object) iGcolPattern3, "iGColPattern7");
    iGcolPattern4.AllowGrouping = false;
    iGcolPattern4.AllowMoving = false;
    iGcolPattern4.CellStyle = (iGCellStyle) this.iGCellStyleDesign4;
    componentResourceManager.ApplyResources((object) iGcolPattern4, "iGColPattern8");
    this.gridArticles.Cols.AddRange(new iGColPattern[4]
    {
      iGcolPattern1,
      iGcolPattern2,
      iGcolPattern3,
      iGcolPattern4
    });
    this.gridArticles.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this.gridArticles.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this.gridArticles, "gridArticles");
    this.gridArticles.GroupBox.Text = componentResourceManager.GetString("gridArticles.GroupBox.Text");
    this.gridArticles.Header.Height = (int) componentResourceManager.GetObject("gridArticles.Header.Height");
    this.gridArticles.Name = "gridArticles";
    this.gridArticles.RowMode = true;
    this.gridArticles.RowModeHasCurCell = true;
    this.gridArticles.SingleClickEdit = true;
    this.gridArticles.DynamicForeColor += new iGDynamicColorEventHandler(this.gridArticles_DynamicForeColor);
    this.gridArticles.CurCellChanged += new EventHandler(this.gridArticles_CurCellChanged);
    this.gridArticles.BeforeCommitEdit += new iGBeforeCommitEditEventHandler(this.gridArticles_BeforeCommitEdit);
    this.gridArticles.CellMouseUp += new iGCellMouseUpEventHandler(this.gridArticles_CellMouseUp);
    this.gridArticles.CustomDrawCellBackground += new iGCustomDrawCellEventHandler(this.gridArticles_CustomDrawCellBackground);
    this.gridArticles.DynamicFont += new iGDynamicFontEventHandler(this.gridArticles_DynamicFont);
    this.gridArticles.AfterCommitEdit += new iGAfterCommitEditEventHandler(this.gridArticles_AfterCommitEdit);
    this.gridArticles.CustomDrawCellForeground += new iGCustomDrawCellEventHandler(this.gridArticles_CustomDrawCellForeground);
    this.headerControl.BackColor = SystemColors.Control;
    componentResourceManager.ApplyResources((object) this.headerControl, "headerControl");
    this.headerControl.ForeColor = SystemColors.ControlText;
    this.headerControl.HeaderFont = new Font("Tahoma", 10f, FontStyle.Bold);
    this.headerControl.Name = "headerControl";
    this.imagesState.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesState.ImageStream");
    this.imagesState.TransparentColor = Color.Transparent;
    this.imagesState.Images.SetKeyName(0, "unchecked.ico");
    this.imagesState.Images.SetKeyName(1, "checked.ico");
    this.imagesState.Images.SetKeyName(2, "grayed.ico");
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.CancelButton = (IButtonControl) this.btnCancel;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this.gridArticles);
    this.Controls.Add((Control) this.headerControl);
    this.Controls.Add((Control) this.panelBottom);
    this.HelpButton = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ArticlesCreatorForm);
    this.ShowInTaskbar = false;
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.FormClosed += new FormClosedEventHandler(this.ArticlesCreatorForm_FormClosed);
    this.panelBottom.ResumeLayout(false);
    this.panelBottom.PerformLayout();
    ((ISupportInitialize) this.pictureCheckedOutOther).EndInit();
    ((ISupportInitialize) this.pictureCheckedOut).EndInit();
    ((ISupportInitialize) this.gridArticles).EndInit();
    this.ResumeLayout(false);
  }
}
