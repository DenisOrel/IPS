// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.MaterialPropertiesPage
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class MaterialPropertiesPage : UserControl
{
  private IContainer components;
  private PropertiesCtrl _propsCtrl;
  private Ribbon _ribbon;
  private RibbonTab _ribbonTab;
  private RibbonPanel _ribbonPanel;
  private RibbonButton _ribbonBtnAdd;
  private RibbonButton _ribbonBtnDel;
  private RibbonButton _ribbonBtnMove;
  private RibbonButton _ribbonBtnEdit;
  private RibbonButton _ribbonBtnBeg;
  private RibbonButton _ribbonBtnUp;
  private RibbonButton _ribbonBtnDown;
  private RibbonButton _ribbonBtnEnd;
  private RibbonButton _ribbonBtnCombine;
  private RibbonPanel _SaveCancelPanel;
  private RibbonButton _rBtnSave;
  private RibbonButton _rBtnCancel;
  private ImageList _imgList;

  public string ColMaterial => this._propsCtrl.ColMaterial;

  public string ColObject => this._propsCtrl.ColObject;

  public string ImbaseKey
  {
    get => this._propsCtrl.ImbaseKey;
    set
    {
      if (this.PropertiesChanged)
      {
        string caption = LocalizationHolder.rm.GetString("IMH_PropertiesChanged_Caption");
        string text = LocalizationHolder.rm.GetString("IMH_PropertiesChanged_Msg");
        if (MessageBox.Show((IWin32Window) this.FindForm(), text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          this.SaveChanged();
        else
          this._rBtnSave.Enabled = this._rBtnCancel.Enabled = false;
      }
      this.SwitchCombineBtn(1);
      this._propsCtrl.ImbaseKey = value;
      if (string.IsNullOrEmpty(value))
        this._ribbonBtnAdd.Enabled = this._ribbonBtnDel.Enabled = this._ribbonBtnMove.Enabled = this._ribbonBtnEdit.Enabled = this._ribbonBtnCombine.Enabled = this._rBtnSave.Enabled = this._rBtnCancel.Enabled = this._rBtnCancel.Enabled = false;
      else
        this._ribbonBtnAdd.Enabled = true;
    }
  }

  public bool IsSettingsLoaded => this._propsCtrl.IsSettingsLoaded;

  public bool PropertiesChanged => this._rBtnSave.Enabled;

  public DataTable SettingsTable => this._propsCtrl.SettingsTable;

  public MaterialPropertiesPage()
  {
    this.InitializeComponent();
    this.SwitchCombineBtn(1);
  }

  public event EventHandler ReloadAdditionalPage;

  private void On_rBtnCancel_Click(object sender, EventArgs e)
  {
    if (!this._rBtnSave.Enabled)
      return;
    this._propsCtrl.LeaveProperties();
    string caption = LocalizationHolder.rm.GetString("IMH_PropertiesChanged_Caption");
    string text = LocalizationHolder.rm.GetString("IMH_RevertChanges_Msg");
    if (MessageBox.Show((IWin32Window) this.FindForm(), text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    this.ReloadSettingsData();
    this.OnReloadAdditionalPage();
    this.SwitchCombineBtn(1);
    this._rBtnSave.Enabled = this._rBtnCancel.Enabled = false;
  }

  private void On_rBtnSave_Click(object sender, EventArgs e)
  {
    try
    {
      this._propsCtrl.LeaveProperties();
      this._propsCtrl.SaveProperties();
      this._rBtnSave.Enabled = this._rBtnCancel.Enabled = false;
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
  }

  private void On_ribbonBtnAdd_Click(object sender, EventArgs e)
  {
    this._propsCtrl.OnBtnAddClick();
    this._rBtnSave.Enabled = this._rBtnCancel.Enabled = true;
  }

  private void On_ribbonBtnBeg_Click(object sender, EventArgs e)
  {
    this._propsCtrl.OnBtnMoveBegClick();
    this._rBtnSave.Enabled = this._rBtnCancel.Enabled = true;
  }

  private void On_ribbonBtnCombine_Click(object sender, EventArgs e)
  {
    short int16 = Convert.ToInt16(this._ribbonBtnCombine.Tag);
    this._propsCtrl.OnBtnUnionClick(int16 > (short) 0);
    this.SwitchCombineBtn((int) int16 * -1);
    this._rBtnSave.Enabled = this._rBtnCancel.Enabled = true;
  }

  private void On_ribbonBtnDel_Click(object sender, EventArgs e)
  {
    this._propsCtrl.OnBtnRemoveClick();
    this._rBtnSave.Enabled = this._rBtnCancel.Enabled = true;
  }

  private void On_ribbonBtnDown_Click(object sender, EventArgs e)
  {
    if (this._ribbonBtnDown.Tag == null)
      this._propsCtrl.OnBtnMoveDownClick();
    else
      this._propsCtrl.OnBtnMoveRightClick();
    this._rBtnSave.Enabled = this._rBtnCancel.Enabled = true;
  }

  private void On_ribbonBtnEdit_Click(object sender, EventArgs e)
  {
    this._propsCtrl.OnBtnEditClick();
    this._ribbonBtnAdd.Enabled = this._ribbonBtnDel.Enabled = this._ribbonBtnMove.Enabled = this._ribbonBtnEdit.Enabled = this._ribbonBtnCombine.Enabled = false;
    this._rBtnSave.Enabled = this._rBtnCancel.Enabled = true;
  }

  private void On_ribbonBtnEnd_Click(object sender, EventArgs e)
  {
    this._propsCtrl.OnBtnMoveEndClick();
    this._rBtnSave.Enabled = this._rBtnCancel.Enabled = true;
  }

  private void On_ribbonBtnUp_Click(object sender, EventArgs e)
  {
    if (this._ribbonBtnDown.Tag == null)
      this._propsCtrl.OnBtnMoveUpClick();
    else
      this._propsCtrl.OnBtnMoveLeftClick();
    this._rBtnSave.Enabled = this._rBtnCancel.Enabled = true;
  }

  private void On_propsCtrl_SelectedElementChanged(object senedr, SelectedRibbonElementEventArgs e)
  {
    SelectedElement element = e.Element;
    switch (element)
    {
      case SelectedElement.None:
        this._ribbonBtnDel.Enabled = this._ribbonBtnMove.Enabled = this._ribbonBtnEdit.Enabled = this._ribbonBtnCombine.Enabled = false;
        break;
      case SelectedElement.Page:
      case SelectedElement.Table:
        this._ribbonBtnAdd.Enabled = this._ribbonBtnEdit.Enabled = true;
        this._ribbonBtnCombine.Enabled = false;
        this._ribbonBtnDel.Enabled = element == SelectedElement.Page || e.ElementsCount > 1;
        if (!(this._ribbonBtnMove.Enabled = e.ElementsCount > 1))
          break;
        this._ribbonBtnUp.Text = LocalizationHolder.rm.GetString("IMH_Move_Up");
        this._ribbonBtnDown.Text = LocalizationHolder.rm.GetString("IMH_Move_Down");
        this._ribbonBtnUp.Tag = this._ribbonBtnDown.Tag = (object) null;
        this._ribbonBtnBeg.Enabled = this._ribbonBtnUp.Enabled = e.Index > 0;
        this._ribbonBtnDown.Enabled = this._ribbonBtnEnd.Enabled = e.Index < e.ElementsCount - 1;
        break;
      case SelectedElement.Column:
        this._ribbonBtnAdd.Enabled = true;
        this._ribbonBtnEdit.Enabled = true;
        this._ribbonBtnCombine.Enabled = false;
        if (!(this._ribbonBtnDel.Enabled = this._ribbonBtnMove.Enabled = e.ElementsCount > 1))
          break;
        this._ribbonBtnUp.Text = LocalizationHolder.rm.GetString("IMH_Move_Left");
        this._ribbonBtnDown.Text = LocalizationHolder.rm.GetString("IMH_Move_Right");
        this._ribbonBtnUp.Tag = this._ribbonBtnDown.Tag = (object) true;
        this._ribbonBtnBeg.Enabled = this._ribbonBtnUp.Enabled = e.Index > 0;
        this._ribbonBtnDown.Enabled = this._ribbonBtnEnd.Enabled = e.Index < e.ElementsCount - 1;
        break;
      case SelectedElement.Row:
        this._ribbonBtnAdd.Enabled = true;
        this._ribbonBtnCombine.Enabled = e.ColumnCount > 1;
        this._ribbonBtnEdit.Enabled = e.IsUnitedRow || e.ColumnCount == 1;
        if (this._ribbonBtnDel.Enabled = this._ribbonBtnMove.Enabled = e.ElementsCount > 1)
        {
          this._ribbonBtnUp.Text = LocalizationHolder.rm.GetString("IMH_Move_Up");
          this._ribbonBtnDown.Text = LocalizationHolder.rm.GetString("IMH_Move_Down");
          this._ribbonBtnUp.Tag = this._ribbonBtnDown.Tag = (object) null;
          this._ribbonBtnBeg.Enabled = this._ribbonBtnUp.Enabled = e.Index > 0;
          this._ribbonBtnDown.Enabled = this._ribbonBtnEnd.Enabled = e.Index < e.ElementsCount - 1;
        }
        this.SwitchCombineBtn(e.IsUnitedRow ? -1 : 1);
        break;
      case SelectedElement.Cell:
        this._ribbonBtnAdd.Enabled = e.ColumnCount == 1;
        this._ribbonBtnEdit.Enabled = true;
        this._ribbonBtnCombine.Enabled = false;
        if (e.ColumnCount == 1 && e.RowCount > 1)
        {
          this._ribbonBtnDel.Enabled = this._ribbonBtnMove.Enabled = true;
          this._ribbonBtnUp.Text = LocalizationHolder.rm.GetString("IMH_Move_Up");
          this._ribbonBtnDown.Text = LocalizationHolder.rm.GetString("IMH_Move_Down");
          this._ribbonBtnUp.Tag = this._ribbonBtnDown.Tag = (object) null;
          this._ribbonBtnBeg.Enabled = this._ribbonBtnUp.Enabled = e.Index > 0;
          this._ribbonBtnDown.Enabled = this._ribbonBtnEnd.Enabled = e.Index < e.RowCount - 1;
          break;
        }
        this._ribbonBtnDel.Enabled = this._ribbonBtnMove.Enabled = false;
        break;
    }
  }

  private void SwitchCombineBtn(int n)
  {
    if (n < 0)
    {
      this._ribbonBtnCombine.Text = LocalizationHolder.rm.GetString("IMH_Row_Break");
      this._ribbonBtnCombine.Tag = (object) -1;
      this._ribbonBtnCombine.Image = this._imgList.Images["SplitRow.ico"];
      this._ribbonBtnCombine.SmallImage = this._imgList.Images["SplitRow.ico"];
    }
    else
    {
      this._ribbonBtnCombine.Text = LocalizationHolder.rm.GetString("IMH_Row_Combine");
      this._ribbonBtnCombine.Tag = (object) 1;
      this._ribbonBtnCombine.Image = this._imgList.Images["CombineRow.ico"];
      this._ribbonBtnCombine.SmallImage = this._imgList.Images["CombineRow.ico"];
    }
  }

  private void OnReloadAdditionalPage()
  {
    EventHandler reloadAdditionalPage = this.ReloadAdditionalPage;
    if (reloadAdditionalPage == null)
      return;
    reloadAdditionalPage((object) this, new EventArgs());
  }

  public void Initialize(bool canEdit)
  {
    if (!canEdit)
      this.SetRibbonInvisible();
    this._propsCtrl.ReadOnly = !canEdit;
  }

  public void AddPage(string caption, List<DataTable> tables)
  {
    this._propsCtrl.AddPage(caption, (IEnumerable<DataTable>) tables);
  }

  public void AddPage(
    string caption,
    List<DataTable> tables,
    bool drawLines,
    bool drawTablesHeader,
    bool forboddenColumnsAdd = false)
  {
    this._propsCtrl.AddPage(caption, (IEnumerable<DataTable>) tables, drawLines, drawTablesHeader, forboddenColumnsAdd);
  }

  public void Clear(bool bInvalidate) => this._propsCtrl.Clear(bInvalidate);

  public void ExpandAll(bool isExpand) => this._propsCtrl.ExpandAll(isExpand);

  public void ReloadSettingsData() => this._propsCtrl.ReloadSettingsData();

  public void SetRibbonInvisible() => this._ribbon.Visible = false;

  public void SaveChanged()
  {
    if (!this.PropertiesChanged)
      return;
    this.On_rBtnSave_Click((object) this._rBtnSave, new EventArgs());
  }

  private void _propsCtrl_DataChanged(object sender, EventArgs e)
  {
    this._rBtnSave.Enabled = this._rBtnCancel.Enabled = true;
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MaterialPropertiesPage));
    this._propsCtrl = new PropertiesCtrl();
    this._ribbon = new Ribbon();
    this._ribbonTab = new RibbonTab();
    this._ribbonPanel = new RibbonPanel();
    this._ribbonBtnAdd = new RibbonButton();
    this._ribbonBtnDel = new RibbonButton();
    this._ribbonBtnMove = new RibbonButton();
    this._ribbonBtnBeg = new RibbonButton();
    this._ribbonBtnUp = new RibbonButton();
    this._ribbonBtnDown = new RibbonButton();
    this._ribbonBtnEnd = new RibbonButton();
    this._ribbonBtnEdit = new RibbonButton();
    this._ribbonBtnCombine = new RibbonButton();
    this._SaveCancelPanel = new RibbonPanel();
    this._rBtnSave = new RibbonButton();
    this._rBtnCancel = new RibbonButton();
    this._imgList = new ImageList(this.components);
    this.SuspendLayout();
    this._propsCtrl.AutoScroll = true;
    this._propsCtrl.BackColor = SystemColors.Window;
    this._propsCtrl.BetweenPagesDistance = 10;
    this._propsCtrl.BorderStyle = BorderStyle.FixedSingle;
    this._propsCtrl.Dock = DockStyle.Fill;
    this._propsCtrl.ImbaseKey = "";
    this._propsCtrl.Location = new Point(0, 80 /*0x50*/);
    this._propsCtrl.Name = "_propsCtrl";
    this._propsCtrl.Padding = new Padding(10);
    this._propsCtrl.Size = new Size(582, 445);
    this._propsCtrl.TabIndex = 0;
    this._propsCtrl.SelectedElementChanged += new SelectedRibbonElementEventHandler(this.On_propsCtrl_SelectedElementChanged);
    this._propsCtrl.DataChanged += new EventHandler(this._propsCtrl_DataChanged);
    this._ribbon.ActualBorderMode = RibbonWindowMode.NonClientAreaGlass;
    this._ribbon.BackColor = SystemColors.Control;
    this._ribbon.Font = new Font("Segoe UI", 9f);
    this._ribbon.Location = new Point(0, 0);
    this._ribbon.Name = "_ribbon";
    this._ribbon.Size = new Size(582, 80 /*0x50*/);
    this._ribbon.TabIndex = 1;
    this._ribbon.Tabs.Add(this._ribbonTab);
    this._ribbon.Text = "ribbon1";
    this._ribbonTab.Panels.Add(this._ribbonPanel);
    this._ribbonTab.Panels.Add(this._SaveCancelPanel);
    this._ribbonTab.Tag = (object) null;
    this._ribbonTab.Text = "";
    this._ribbonPanel.ContentBounds = new Rectangle(7, 7, 410, 52);
    this._ribbonPanel.Items.Add((RibbonItem) this._ribbonBtnAdd);
    this._ribbonPanel.Items.Add((RibbonItem) this._ribbonBtnDel);
    this._ribbonPanel.Items.Add((RibbonItem) this._ribbonBtnMove);
    this._ribbonPanel.Items.Add((RibbonItem) this._ribbonBtnEdit);
    this._ribbonPanel.Items.Add((RibbonItem) this._ribbonBtnCombine);
    this._ribbonPanel.SizeMode = RibbonElementSizeMode.Large;
    this._ribbonPanel.Tag = (object) null;
    this._ribbonPanel.Text = "";
    this._ribbonBtnAdd.DropDownArrowSize = new Size(5, 3);
    this._ribbonBtnAdd.Image = (Image) componentResourceManager.GetObject("_ribbonBtnAdd.Image");
    this._ribbonBtnAdd.MaxSizeMode = RibbonElementSizeMode.Large;
    this._ribbonBtnAdd.MinSizeMode = RibbonElementSizeMode.Compact;
    this._ribbonBtnAdd.SmallImage = (Image) componentResourceManager.GetObject("_ribbonBtnAdd.SmallImage");
    this._ribbonBtnAdd.Style = RibbonButtonStyle.Normal;
    this._ribbonBtnAdd.Tag = (object) null;
    this._ribbonBtnAdd.Text = "Добавить";
    this._ribbonBtnAdd.ToolTip = (string) null;
    this._ribbonBtnAdd.Click += new EventHandler(this.On_ribbonBtnAdd_Click);
    this._ribbonBtnDel.DropDownArrowSize = new Size(5, 3);
    this._ribbonBtnDel.Image = (Image) componentResourceManager.GetObject("_ribbonBtnDel.Image");
    this._ribbonBtnDel.MaxSizeMode = RibbonElementSizeMode.Large;
    this._ribbonBtnDel.MinSizeMode = RibbonElementSizeMode.Compact;
    this._ribbonBtnDel.SmallImage = (Image) componentResourceManager.GetObject("_ribbonBtnDel.SmallImage");
    this._ribbonBtnDel.Style = RibbonButtonStyle.Normal;
    this._ribbonBtnDel.Tag = (object) null;
    this._ribbonBtnDel.Text = "Удалить";
    this._ribbonBtnDel.ToolTip = (string) null;
    this._ribbonBtnDel.Click += new EventHandler(this.On_ribbonBtnDel_Click);
    this._ribbonBtnMove.DropDownArrowSize = new Size(5, 3);
    this._ribbonBtnMove.DropDownItems.Add((RibbonItem) this._ribbonBtnBeg);
    this._ribbonBtnMove.DropDownItems.Add((RibbonItem) this._ribbonBtnUp);
    this._ribbonBtnMove.DropDownItems.Add((RibbonItem) this._ribbonBtnDown);
    this._ribbonBtnMove.DropDownItems.Add((RibbonItem) this._ribbonBtnEnd);
    this._ribbonBtnMove.Image = (Image) componentResourceManager.GetObject("_ribbonBtnMove.Image");
    this._ribbonBtnMove.MaxSizeMode = RibbonElementSizeMode.Large;
    this._ribbonBtnMove.MinSizeMode = RibbonElementSizeMode.Compact;
    this._ribbonBtnMove.SmallImage = (Image) componentResourceManager.GetObject("_ribbonBtnMove.SmallImage");
    this._ribbonBtnMove.Style = RibbonButtonStyle.DropDown;
    this._ribbonBtnMove.Tag = (object) null;
    this._ribbonBtnMove.Text = "Переместить";
    this._ribbonBtnMove.ToolTip = (string) null;
    this._ribbonBtnBeg.DropDownArrowSize = new Size(5, 3);
    this._ribbonBtnBeg.Image = (Image) null;
    this._ribbonBtnBeg.MaxSizeMode = RibbonElementSizeMode.Medium;
    this._ribbonBtnBeg.MinSizeMode = RibbonElementSizeMode.Compact;
    this._ribbonBtnBeg.SmallImage = (Image) componentResourceManager.GetObject("_ribbonBtnBeg.SmallImage");
    this._ribbonBtnBeg.Style = RibbonButtonStyle.Normal;
    this._ribbonBtnBeg.Tag = (object) null;
    this._ribbonBtnBeg.Text = "В начало";
    this._ribbonBtnBeg.ToolTip = (string) null;
    this._ribbonBtnBeg.Click += new EventHandler(this.On_ribbonBtnBeg_Click);
    this._ribbonBtnUp.DropDownArrowSize = new Size(5, 3);
    this._ribbonBtnUp.Image = (Image) componentResourceManager.GetObject("_ribbonBtnUp.Image");
    this._ribbonBtnUp.MaxSizeMode = RibbonElementSizeMode.Medium;
    this._ribbonBtnUp.MinSizeMode = RibbonElementSizeMode.Compact;
    this._ribbonBtnUp.SmallImage = (Image) componentResourceManager.GetObject("_ribbonBtnUp.SmallImage");
    this._ribbonBtnUp.Style = RibbonButtonStyle.Normal;
    this._ribbonBtnUp.Tag = (object) null;
    this._ribbonBtnUp.Text = "Вверх";
    this._ribbonBtnUp.ToolTip = (string) null;
    this._ribbonBtnUp.Click += new EventHandler(this.On_ribbonBtnUp_Click);
    this._ribbonBtnDown.DropDownArrowSize = new Size(5, 3);
    this._ribbonBtnDown.Image = (Image) componentResourceManager.GetObject("_ribbonBtnDown.Image");
    this._ribbonBtnDown.MaxSizeMode = RibbonElementSizeMode.Medium;
    this._ribbonBtnDown.MinSizeMode = RibbonElementSizeMode.Compact;
    this._ribbonBtnDown.SmallImage = (Image) componentResourceManager.GetObject("_ribbonBtnDown.SmallImage");
    this._ribbonBtnDown.Style = RibbonButtonStyle.Normal;
    this._ribbonBtnDown.Tag = (object) null;
    this._ribbonBtnDown.Text = "Вниз";
    this._ribbonBtnDown.ToolTip = (string) null;
    this._ribbonBtnDown.Click += new EventHandler(this.On_ribbonBtnDown_Click);
    this._ribbonBtnEnd.DropDownArrowSize = new Size(5, 3);
    this._ribbonBtnEnd.Image = (Image) componentResourceManager.GetObject("_ribbonBtnEnd.Image");
    this._ribbonBtnEnd.MaxSizeMode = RibbonElementSizeMode.Medium;
    this._ribbonBtnEnd.MinSizeMode = RibbonElementSizeMode.Compact;
    this._ribbonBtnEnd.SmallImage = (Image) componentResourceManager.GetObject("_ribbonBtnEnd.SmallImage");
    this._ribbonBtnEnd.Style = RibbonButtonStyle.Normal;
    this._ribbonBtnEnd.Tag = (object) null;
    this._ribbonBtnEnd.Text = "В конец";
    this._ribbonBtnEnd.ToolTip = (string) null;
    this._ribbonBtnEnd.Click += new EventHandler(this.On_ribbonBtnEnd_Click);
    this._ribbonBtnEdit.DropDownArrowSize = new Size(5, 3);
    this._ribbonBtnEdit.Image = (Image) componentResourceManager.GetObject("_ribbonBtnEdit.Image");
    this._ribbonBtnEdit.MaxSizeMode = RibbonElementSizeMode.Large;
    this._ribbonBtnEdit.MinSizeMode = RibbonElementSizeMode.Compact;
    this._ribbonBtnEdit.SmallImage = (Image) componentResourceManager.GetObject("_ribbonBtnEdit.SmallImage");
    this._ribbonBtnEdit.Style = RibbonButtonStyle.Normal;
    this._ribbonBtnEdit.Tag = (object) null;
    this._ribbonBtnEdit.Text = "Редактировать";
    this._ribbonBtnEdit.ToolTip = (string) null;
    this._ribbonBtnEdit.Click += new EventHandler(this.On_ribbonBtnEdit_Click);
    this._ribbonBtnCombine.DropDownArrowSize = new Size(5, 3);
    this._ribbonBtnCombine.Image = (Image) null;
    this._ribbonBtnCombine.MaxSizeMode = RibbonElementSizeMode.Large;
    this._ribbonBtnCombine.MinSizeMode = RibbonElementSizeMode.Compact;
    this._ribbonBtnCombine.SmallImage = (Image) componentResourceManager.GetObject("_ribbonBtnCombine.SmallImage");
    this._ribbonBtnCombine.Style = RibbonButtonStyle.Normal;
    this._ribbonBtnCombine.Tag = (object) null;
    this._ribbonBtnCombine.Text = "Объединить";
    this._ribbonBtnCombine.ToolTip = (string) null;
    this._ribbonBtnCombine.Click += new EventHandler(this.On_ribbonBtnCombine_Click);
    this._SaveCancelPanel.ContentBounds = new Rectangle(427, 7, 146, 52);
    this._SaveCancelPanel.Items.Add((RibbonItem) this._rBtnSave);
    this._SaveCancelPanel.Items.Add((RibbonItem) this._rBtnCancel);
    this._SaveCancelPanel.SizeMode = RibbonElementSizeMode.Large;
    this._SaveCancelPanel.Tag = (object) null;
    this._SaveCancelPanel.Text = "";
    this._rBtnSave.DropDownArrowSize = new Size(5, 3);
    this._rBtnSave.Enabled = false;
    this._rBtnSave.Image = (Image) componentResourceManager.GetObject("_rBtnSave.Image");
    this._rBtnSave.MaxSizeMode = RibbonElementSizeMode.Large;
    this._rBtnSave.MinSizeMode = RibbonElementSizeMode.Compact;
    this._rBtnSave.SmallImage = (Image) componentResourceManager.GetObject("_rBtnSave.SmallImage");
    this._rBtnSave.Style = RibbonButtonStyle.Normal;
    this._rBtnSave.Tag = (object) null;
    this._rBtnSave.Text = "Сохранить";
    this._rBtnSave.ToolTip = (string) null;
    this._rBtnSave.Click += new EventHandler(this.On_rBtnSave_Click);
    this._rBtnCancel.DropDownArrowSize = new Size(5, 3);
    this._rBtnCancel.Enabled = false;
    this._rBtnCancel.Image = (Image) componentResourceManager.GetObject("_rBtnCancel.Image");
    this._rBtnCancel.MaxSizeMode = RibbonElementSizeMode.Large;
    this._rBtnCancel.MinSizeMode = RibbonElementSizeMode.Compact;
    this._rBtnCancel.SmallImage = (Image) componentResourceManager.GetObject("_rBtnCancel.SmallImage");
    this._rBtnCancel.Style = RibbonButtonStyle.Normal;
    this._rBtnCancel.Tag = (object) null;
    this._rBtnCancel.Text = "Отменить";
    this._rBtnCancel.ToolTip = (string) null;
    this._rBtnCancel.Click += new EventHandler(this.On_rBtnCancel_Click);
    this._imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("_imgList.ImageStream");
    this._imgList.TransparentColor = Color.Transparent;
    this._imgList.Images.SetKeyName(0, "CombineRow.ico");
    this._imgList.Images.SetKeyName(1, "SplitRow.ico");
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._propsCtrl);
    this.Controls.Add((Control) this._ribbon);
    this.Name = nameof (MaterialPropertiesPage);
    this.Size = new Size(582, 525);
    this.ResumeLayout(false);
  }
}
