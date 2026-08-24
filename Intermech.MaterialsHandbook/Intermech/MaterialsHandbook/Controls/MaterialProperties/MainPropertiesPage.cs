// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.MainPropertiesPage
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

public class MainPropertiesPage : UserControl
{
  private bool _propertiesChanged;
  private DataProvider _dataProvider;
  private IContainer components;
  private Ribbon ribbon1;
  private RibbonTab ribbonTab;
  private RibbonPanel ribbonPanelMain;
  private RibbonButton rbtnAdd;
  private RibbonButton rbtnRemove;
  private RibbonButton rbtnEdit;
  private RibbonPanel ribbonPanelAdditional;
  private RibbonButton rbtnSave;
  private RibbonButton rbtnCancel;
  private MainPropertiesCtrl mainPropertiesCtrl;

  public MainPropertiesPage() => this.InitializeComponent();

  public void Initialize(bool canEdit)
  {
    this.mainPropertiesCtrl.ReadOnly = this.ribbon1.Visible = !canEdit;
  }

  public DataProvider DataProvider
  {
    get => this._dataProvider;
    set
    {
      this._dataProvider = value;
      this.mainPropertiesCtrl.SettingsDriver = this._dataProvider;
    }
  }

  public string ImbaseKey
  {
    get => this.mainPropertiesCtrl.ImbaseKey;
    set
    {
      if (this._propertiesChanged)
      {
        string caption = LocalizationHolder.rm.GetString("IMH_MainPropertiesChanged_Caption");
        string text = LocalizationHolder.rm.GetString("IMH_MainPropertiesChanged_Msg");
        if (MessageBox.Show((IWin32Window) this.FindForm(), text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          this.SaveChanged();
        this._propertiesChanged = false;
      }
      this.mainPropertiesCtrl.ImbaseKey = value;
    }
  }

  public void SaveChanged()
  {
    if (!this._propertiesChanged)
      return;
    this.mainPropertiesCtrl.SaveSettings();
  }

  private void mainPropertiesCtrl_SelectedElementChanged(
    object senedr,
    SelectedRibbonElementEventArgs e)
  {
    switch (e.Element)
    {
      case SelectedElement.None:
        this.rbtnAdd.Enabled = this.rbtnRemove.Enabled = this.rbtnEdit.Enabled = false;
        break;
      case SelectedElement.Page:
        this.rbtnAdd.Enabled = this.rbtnEdit.Enabled = this.rbtnRemove.Enabled = false;
        break;
      case SelectedElement.Table:
        this.rbtnAdd.Enabled = this.rbtnEdit.Enabled = this.rbtnRemove.Enabled = false;
        break;
      case SelectedElement.Column:
        this.rbtnAdd.Enabled = false;
        this.rbtnEdit.Enabled = false;
        this.rbtnRemove.Enabled = false;
        break;
      case SelectedElement.Row:
        this.rbtnAdd.Enabled = true;
        this.rbtnEdit.Enabled = e.IsUnitedRow || e.ColumnCount == 1;
        this.rbtnRemove.Enabled = e.ElementsCount > 1;
        break;
      case SelectedElement.Cell:
        this.rbtnAdd.Enabled = e.ColumnCount == 1;
        this.rbtnEdit.Enabled = true;
        if (e.ColumnCount == 1 && e.RowCount > 1)
        {
          this.rbtnRemove.Enabled = true;
          break;
        }
        this.rbtnRemove.Enabled = false;
        break;
    }
    this.rbtnSave.Enabled = this.rbtnCancel.Enabled = this._propertiesChanged;
  }

  private void rbtnAdd_Click(object sender, EventArgs e) => this.mainPropertiesCtrl.AddAction();

  private void rbtnRemove_Click(object sender, EventArgs e)
  {
    this.mainPropertiesCtrl.RemoveAction();
  }

  private void rbtnEdit_Click(object sender, EventArgs e) => this.mainPropertiesCtrl.EditAction();

  private void rbtnSave_Click(object sender, EventArgs e)
  {
    try
    {
      this.rbtnCancel.Enabled = this.rbtnSave.Enabled = this._propertiesChanged = false;
      this.mainPropertiesCtrl.SaveSettings();
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
    }
  }

  private void rbtnCancel_Click(object sender, EventArgs e)
  {
    if (!this.rbtnSave.Enabled)
      return;
    string caption = LocalizationHolder.rm.GetString("IMH_PropertiesChanged_Caption");
    string text = LocalizationHolder.rm.GetString("IMH_RevertChanges_Msg");
    if (MessageBox.Show((IWin32Window) this.FindForm(), text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    this._propertiesChanged = false;
    this.mainPropertiesCtrl.ReloadSettingsData();
  }

  private void mainPropertiesCtrl_EditorEnter(object sender, EventArgs e)
  {
    this.rbtnAdd.Enabled = this.rbtnCancel.Enabled = this.rbtnEdit.Enabled = this.rbtnSave.Enabled = this.rbtnRemove.Enabled = false;
  }

  private void mainPropertiesCtrl_DataChanged(object sender, EventArgs e)
  {
    this._propertiesChanged = true;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainPropertiesPage));
    this.ribbon1 = new Ribbon();
    this.ribbonTab = new RibbonTab();
    this.ribbonPanelMain = new RibbonPanel();
    this.rbtnAdd = new RibbonButton();
    this.rbtnRemove = new RibbonButton();
    this.rbtnEdit = new RibbonButton();
    this.ribbonPanelAdditional = new RibbonPanel();
    this.rbtnSave = new RibbonButton();
    this.rbtnCancel = new RibbonButton();
    this.mainPropertiesCtrl = new MainPropertiesCtrl();
    this.SuspendLayout();
    this.ribbon1.ActualBorderMode = RibbonWindowMode.NonClientAreaGlass;
    this.ribbon1.Font = new Font("Segoe UI", 9f);
    this.ribbon1.Location = new Point(0, 0);
    this.ribbon1.Name = "ribbon1";
    this.ribbon1.Size = new Size(576, 80 /*0x50*/);
    this.ribbon1.TabIndex = 0;
    this.ribbon1.Tabs.Add(this.ribbonTab);
    this.ribbon1.Text = "ribbon1";
    this.ribbonTab.Panels.Add(this.ribbonPanelMain);
    this.ribbonTab.Panels.Add(this.ribbonPanelAdditional);
    this.ribbonTab.Tag = (object) null;
    this.ribbonTab.Text = "";
    this.ribbonPanelMain.ContentBounds = new Rectangle(7, 7, 229, 52);
    this.ribbonPanelMain.Items.Add((RibbonItem) this.rbtnAdd);
    this.ribbonPanelMain.Items.Add((RibbonItem) this.rbtnRemove);
    this.ribbonPanelMain.Items.Add((RibbonItem) this.rbtnEdit);
    this.ribbonPanelMain.SizeMode = RibbonElementSizeMode.Large;
    this.ribbonPanelMain.Tag = (object) null;
    this.ribbonPanelMain.Text = "";
    this.rbtnAdd.DropDownArrowSize = new Size(5, 3);
    this.rbtnAdd.Enabled = false;
    this.rbtnAdd.Image = (Image) componentResourceManager.GetObject("rbtnAdd.Image");
    this.rbtnAdd.MaxSizeMode = RibbonElementSizeMode.Large;
    this.rbtnAdd.MinSizeMode = RibbonElementSizeMode.Compact;
    this.rbtnAdd.SmallImage = (Image) componentResourceManager.GetObject("rbtnAdd.SmallImage");
    this.rbtnAdd.Style = RibbonButtonStyle.Normal;
    this.rbtnAdd.Tag = (object) null;
    this.rbtnAdd.Text = "Добавить";
    this.rbtnAdd.ToolTip = (string) null;
    this.rbtnAdd.Click += new EventHandler(this.rbtnAdd_Click);
    this.rbtnRemove.DropDownArrowSize = new Size(5, 3);
    this.rbtnRemove.Enabled = false;
    this.rbtnRemove.Image = (Image) componentResourceManager.GetObject("rbtnRemove.Image");
    this.rbtnRemove.MaxSizeMode = RibbonElementSizeMode.Large;
    this.rbtnRemove.MinSizeMode = RibbonElementSizeMode.Compact;
    this.rbtnRemove.SmallImage = (Image) componentResourceManager.GetObject("rbtnRemove.SmallImage");
    this.rbtnRemove.Style = RibbonButtonStyle.Normal;
    this.rbtnRemove.Tag = (object) null;
    this.rbtnRemove.Text = "Удалить";
    this.rbtnRemove.ToolTip = (string) null;
    this.rbtnRemove.Click += new EventHandler(this.rbtnRemove_Click);
    this.rbtnEdit.DropDownArrowSize = new Size(5, 3);
    this.rbtnEdit.Enabled = false;
    this.rbtnEdit.Image = (Image) componentResourceManager.GetObject("rbtnEdit.Image");
    this.rbtnEdit.MaxSizeMode = RibbonElementSizeMode.Large;
    this.rbtnEdit.MinSizeMode = RibbonElementSizeMode.Compact;
    this.rbtnEdit.SmallImage = (Image) componentResourceManager.GetObject("rbtnEdit.SmallImage");
    this.rbtnEdit.Style = RibbonButtonStyle.Normal;
    this.rbtnEdit.Tag = (object) null;
    this.rbtnEdit.Text = "Редактировать";
    this.rbtnEdit.ToolTip = (string) null;
    this.rbtnEdit.Click += new EventHandler(this.rbtnEdit_Click);
    this.ribbonPanelAdditional.ContentBounds = new Rectangle(246, 7, 146, 52);
    this.ribbonPanelAdditional.Items.Add((RibbonItem) this.rbtnSave);
    this.ribbonPanelAdditional.Items.Add((RibbonItem) this.rbtnCancel);
    this.ribbonPanelAdditional.SizeMode = RibbonElementSizeMode.Large;
    this.ribbonPanelAdditional.Tag = (object) null;
    this.ribbonPanelAdditional.Text = "";
    this.rbtnSave.DropDownArrowSize = new Size(5, 3);
    this.rbtnSave.Enabled = false;
    this.rbtnSave.Image = (Image) componentResourceManager.GetObject("rbtnSave.Image");
    this.rbtnSave.MaxSizeMode = RibbonElementSizeMode.Large;
    this.rbtnSave.MinSizeMode = RibbonElementSizeMode.Compact;
    this.rbtnSave.SmallImage = (Image) componentResourceManager.GetObject("rbtnSave.SmallImage");
    this.rbtnSave.Style = RibbonButtonStyle.Normal;
    this.rbtnSave.Tag = (object) null;
    this.rbtnSave.Text = "Сохранить";
    this.rbtnSave.ToolTip = (string) null;
    this.rbtnSave.Click += new EventHandler(this.rbtnSave_Click);
    this.rbtnCancel.DropDownArrowSize = new Size(5, 3);
    this.rbtnCancel.Enabled = false;
    this.rbtnCancel.Image = (Image) componentResourceManager.GetObject("rbtnCancel.Image");
    this.rbtnCancel.MaxSizeMode = RibbonElementSizeMode.Large;
    this.rbtnCancel.MinSizeMode = RibbonElementSizeMode.Compact;
    this.rbtnCancel.SmallImage = (Image) componentResourceManager.GetObject("rbtnCancel.SmallImage");
    this.rbtnCancel.Style = RibbonButtonStyle.Normal;
    this.rbtnCancel.Tag = (object) null;
    this.rbtnCancel.Text = "Отменить";
    this.rbtnCancel.ToolTip = (string) null;
    this.rbtnCancel.Click += new EventHandler(this.rbtnCancel_Click);
    this.mainPropertiesCtrl.AutoScroll = true;
    this.mainPropertiesCtrl.BackColor = SystemColors.Window;
    this.mainPropertiesCtrl.BetweenPagesDistance = 10;
    this.mainPropertiesCtrl.BorderStyle = BorderStyle.FixedSingle;
    this.mainPropertiesCtrl.Dock = DockStyle.Fill;
    this.mainPropertiesCtrl.ImbaseKey = "";
    this.mainPropertiesCtrl.Location = new Point(0, 80 /*0x50*/);
    this.mainPropertiesCtrl.Name = "mainPropertiesCtrl";
    this.mainPropertiesCtrl.Padding = new Padding(10);
    this.mainPropertiesCtrl.SettingsDriver = (DataProvider) null;
    this.mainPropertiesCtrl.Size = new Size(576, 369);
    this.mainPropertiesCtrl.TabIndex = 1;
    this.mainPropertiesCtrl.SelectedElementChanged += new SelectedRibbonElementEventHandler(this.mainPropertiesCtrl_SelectedElementChanged);
    this.mainPropertiesCtrl.DataChanged += new EventHandler(this.mainPropertiesCtrl_DataChanged);
    this.mainPropertiesCtrl.EditorEnter += new EventHandler(this.mainPropertiesCtrl_EditorEnter);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.mainPropertiesCtrl);
    this.Controls.Add((Control) this.ribbon1);
    this.Name = nameof (MainPropertiesPage);
    this.Size = new Size(576, 449);
    this.ResumeLayout(false);
  }
}
