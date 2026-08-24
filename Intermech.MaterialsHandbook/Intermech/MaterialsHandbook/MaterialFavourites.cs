// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.MaterialFavourites
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using ImSSP;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class MaterialFavourites : Form
{
  private Guid _userGuid = Guid.Empty;
  private Guid _categoryGuid = Guid.Empty;
  private long _folderID;
  private long _tableRefID;
  private long _recID = -1;
  private string _caption = string.Empty;
  private bool _modified;
  private ListBox _currentLB;
  private IContainer components;
  private Panel _pnlBottom;
  private Button _btnCancel;
  private TabControl _tc;
  private TabPage _tpMaterials;
  private TabPage _tpDimensionTypes;
  private ListBox _lbMaterials;
  private Button _btnDel;
  private Button _btnApply;
  private ListBox _lbAssortment;
  private Button _btnGoTo;
  private Button _btnClose;

  public FavouriteData Data => this._currentLB.SelectedItem as FavouriteData;

  public bool IsMaterial { get; private set; }

  public MaterialFavourites(
    Guid categoryGuid,
    long tableRefID,
    long recID,
    string caption,
    bool isMaterial)
  {
    this.InitializeComponent();
    this._categoryGuid = categoryGuid;
    this._tableRefID = tableRefID;
    this._recID = recID;
    this._caption = caption;
    this.IsMaterial = isMaterial;
    if (this.IsMaterial)
    {
      this._currentLB = this._lbMaterials;
    }
    else
    {
      this._currentLB = this._lbAssortment;
      this._tc.SelectedIndex = sc_14480.ssp_imbase_14481(2033695064);
    }
    this.LoadData();
    this.CheckEnableButtons();
  }

  public MaterialFavourites(
    Guid categoryGuid,
    long folderID,
    long tableRefID,
    long recID,
    string caption)
  {
    this.InitializeComponent();
    this._categoryGuid = categoryGuid;
    this._folderID = folderID;
    this._tableRefID = tableRefID;
    this._recID = recID;
    this._caption = caption;
    this._currentLB = this._lbAssortment;
    this._lbAssortment.Parent = (Control) null;
    this.Controls.Remove((Control) this._tc);
    this.Controls.Add((Control) this._lbAssortment);
    this._lbAssortment.BringToFront();
    this.LoadProfilesData();
    this.CheckEnableButtons();
  }

  private void On_btnDel_Click(object sender, EventArgs e)
  {
    int selectedIndex = this._currentLB.SelectedIndex;
    this._currentLB.BeginUpdate();
    try
    {
      this._currentLB.Items.Remove(this._currentLB.SelectedItem);
    }
    finally
    {
      this._currentLB.EndUpdate();
    }
    this._modified = true;
    if (this._currentLB.Items.Count > selectedIndex)
      this._currentLB.SelectedIndex = selectedIndex;
    else if (this._currentLB.Items.Count > sc_14480.ssp_imbase_14482(1720752687))
      this._currentLB.SelectedIndex = this._currentLB.Items.Count - 1;
    this.CheckEnableButtons();
  }

  private void On_btnGoTo_Click(object sender, EventArgs e)
  {
    if (!(sender is ListBox listBox) || listBox.SelectedItem == null)
      return;
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  private void On_btnApply_Click(object sender, EventArgs e)
  {
    this.Save();
    this.CheckEnableButtons();
  }

  private void On_btnCancel_Click(object sender, EventArgs e)
  {
    this.Cancel();
    this.CheckEnableButtons();
  }

  private void On_lb_SelectedIndexChanged(object sender, EventArgs e) => this.CheckEnableButtons();

  private void On_tc_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this._tc.SelectedIndex == 0)
    {
      this._currentLB = this._lbMaterials;
      this.IsMaterial = true;
    }
    else
    {
      this._currentLB = this._lbAssortment;
      this.IsMaterial = false;
    }
    this.CheckEnableButtons();
  }

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    FormStorage.LoadLayout((Control) this);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    if (!this._modified || MessageBox.Show(LocalizationHolder.rm.GetString(sc_14480.ssp_imbase_14483()), LocalizationHolder.rm.GetString("IMH_DataChanged"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    this.Save();
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    FormStorage.SaveLayout((Control) this);
  }

  private void LoadData()
  {
    this._lbMaterials.BeginUpdate();
    this._lbAssortment.BeginUpdate();
    try
    {
      this._lbMaterials.Items.Clear();
      this._lbAssortment.Items.Clear();
      List<FavouriteData> favouriteDataList1 = (List<FavouriteData>) null;
      List<FavouriteData> favouriteDataList2 = (List<FavouriteData>) null;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IIMHUserSettingsService)) is IIMHUserSettingsService customService)
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(sessionKeeper.Session.UserID);
          if (!objectInfo.Empty)
          {
            this._userGuid = objectInfo.VersionGuid;
            favouriteDataList1 = customService.GetMaterialFavourites(this._userGuid, this._categoryGuid);
            favouriteDataList1.ForEach((Action<FavouriteData>) (x => this._lbMaterials.Items.Add((object) x)));
            favouriteDataList2 = customService.GetAssortmentFavourites(this._userGuid, this._categoryGuid);
            favouriteDataList2.ForEach((Action<FavouriteData>) (x => this._lbAssortment.Items.Add((object) x)));
          }
        }
      }
      if (this._tableRefID == 0L)
        return;
      FavouriteData favouriteData = new FavouriteData(this._tableRefID, this._recID, this._caption);
      if (!(this.IsMaterial ? favouriteDataList1 : favouriteDataList2).Contains(favouriteData))
      {
        this._currentLB.SelectedIndex = this._currentLB.Items.Add((object) favouriteData);
        this._modified = true;
      }
      else
        this._currentLB.SelectedItem = (object) favouriteData;
    }
    finally
    {
      this._lbMaterials.EndUpdate();
      this._lbAssortment.EndUpdate();
    }
  }

  private void LoadProfilesData()
  {
    this._currentLB.BeginUpdate();
    try
    {
      this._currentLB.Items.Clear();
      List<FavouriteData> favouriteDataList = (List<FavouriteData>) null;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IIMHUserSettingsService)) is IIMHUserSettingsService customService)
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(sessionKeeper.Session.UserID);
          if (!objectInfo.Empty)
          {
            this._userGuid = objectInfo.VersionGuid;
            favouriteDataList = customService.GetAssortmentFavourites(this._userGuid, this._categoryGuid);
            favouriteDataList.ForEach((Action<FavouriteData>) (x => this._currentLB.Items.Add((object) x)));
          }
        }
      }
      if (this._tableRefID != 0L)
      {
        FavouriteData favouriteData = new FavouriteData(this._folderID, this._tableRefID, this._recID, this._caption);
        if (!favouriteDataList.Contains(favouriteData))
        {
          this._currentLB.SelectedIndex = this._currentLB.Items.Add((object) favouriteData);
          this._modified = true;
        }
        else
          this._currentLB.SelectedItem = (object) favouriteData;
      }
      if (this._currentLB.Items.Count <= 0 || this._currentLB.SelectedItem != null)
        return;
      this._currentLB.SelectedItem = this._currentLB.Items[0];
    }
    finally
    {
      this._currentLB.EndUpdate();
    }
  }

  private void Save()
  {
    if ((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IIMHUserSettingsService)) is IIMHUserSettingsService customService)
    {
      if (this._lbAssortment.Parent != this)
      {
        List<FavouriteData> materialFavourites = new List<FavouriteData>(this._lbMaterials.Items.Count);
        foreach (object obj in this._lbMaterials.Items)
          materialFavourites.Add(obj as FavouriteData);
        customService.SaveMaterialFavourites(this._userGuid, this._categoryGuid, materialFavourites);
      }
      List<FavouriteData> assortmentFavourites = new List<FavouriteData>(this._lbAssortment.Items.Count);
      foreach (object obj in this._lbAssortment.Items)
        assortmentFavourites.Add(obj as FavouriteData);
      customService.SaveAssortmentFavourites(this._userGuid, this._categoryGuid, assortmentFavourites);
      customService.SaveUserSettings();
    }
    this._modified = false;
  }

  private void Cancel()
  {
    this._modified = false;
    this._tableRefID = 0L;
    this._recID = -1L;
    this.LoadData();
  }

  private void CheckEnableButtons()
  {
    this._btnDel.Enabled = this._btnGoTo.Enabled = this._currentLB.SelectedItems.Count > 0;
    this._btnApply.Enabled = this._btnCancel.Enabled = this._modified;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MaterialFavourites));
    this._pnlBottom = new Panel();
    this._btnClose = new Button();
    this._btnGoTo = new Button();
    this._btnApply = new Button();
    this._btnDel = new Button();
    this._btnCancel = new Button();
    this._tc = new TabControl();
    this._tpMaterials = new TabPage();
    this._lbMaterials = new ListBox();
    this._tpDimensionTypes = new TabPage();
    this._lbAssortment = new ListBox();
    this._pnlBottom.SuspendLayout();
    this._tc.SuspendLayout();
    this._tpMaterials.SuspendLayout();
    this._tpDimensionTypes.SuspendLayout();
    this.SuspendLayout();
    this._pnlBottom.Controls.Add((Control) this._btnClose);
    this._pnlBottom.Controls.Add((Control) this._btnGoTo);
    this._pnlBottom.Controls.Add((Control) this._btnApply);
    this._pnlBottom.Controls.Add((Control) this._btnDel);
    this._pnlBottom.Controls.Add((Control) this._btnCancel);
    componentResourceManager.ApplyResources((object) this._pnlBottom, "_pnlBottom");
    this._pnlBottom.Name = "_pnlBottom";
    componentResourceManager.ApplyResources((object) this._btnClose, "_btnClose");
    this._btnClose.DialogResult = DialogResult.Cancel;
    this._btnClose.Name = "_btnClose";
    this._btnClose.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._btnGoTo, "_btnGoTo");
    this._btnGoTo.DialogResult = DialogResult.OK;
    this._btnGoTo.Name = "_btnGoTo";
    this._btnGoTo.UseVisualStyleBackColor = true;
    this._btnGoTo.Click += new EventHandler(this.On_btnGoTo_Click);
    componentResourceManager.ApplyResources((object) this._btnApply, "_btnApply");
    this._btnApply.Name = "_btnApply";
    this._btnApply.UseVisualStyleBackColor = true;
    this._btnApply.Click += new EventHandler(this.On_btnApply_Click);
    componentResourceManager.ApplyResources((object) this._btnDel, "_btnDel");
    this._btnDel.Name = "_btnDel";
    this._btnDel.UseVisualStyleBackColor = true;
    this._btnDel.Click += new EventHandler(this.On_btnDel_Click);
    componentResourceManager.ApplyResources((object) this._btnCancel, "_btnCancel");
    this._btnCancel.Name = "_btnCancel";
    this._btnCancel.UseVisualStyleBackColor = true;
    this._btnCancel.Click += new EventHandler(this.On_btnCancel_Click);
    this._tc.Controls.Add((Control) this._tpMaterials);
    this._tc.Controls.Add((Control) this._tpDimensionTypes);
    componentResourceManager.ApplyResources((object) this._tc, "_tc");
    this._tc.Name = "_tc";
    this._tc.SelectedIndex = 0;
    this._tc.SelectedIndexChanged += new EventHandler(this.On_tc_SelectedIndexChanged);
    this._tpMaterials.Controls.Add((Control) this._lbMaterials);
    componentResourceManager.ApplyResources((object) this._tpMaterials, "_tpMaterials");
    this._tpMaterials.Name = "_tpMaterials";
    this._tpMaterials.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._lbMaterials, "_lbMaterials");
    this._lbMaterials.FormattingEnabled = true;
    this._lbMaterials.Name = "_lbMaterials";
    this._lbMaterials.Sorted = true;
    this._lbMaterials.SelectedIndexChanged += new EventHandler(this.On_lb_SelectedIndexChanged);
    this._lbMaterials.DoubleClick += new EventHandler(this.On_btnGoTo_Click);
    this._tpDimensionTypes.Controls.Add((Control) this._lbAssortment);
    componentResourceManager.ApplyResources((object) this._tpDimensionTypes, "_tpDimensionTypes");
    this._tpDimensionTypes.Name = "_tpDimensionTypes";
    this._tpDimensionTypes.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._lbAssortment, "_lbAssortment");
    this._lbAssortment.FormattingEnabled = true;
    this._lbAssortment.Name = "_lbAssortment";
    this._lbAssortment.Sorted = true;
    this._lbAssortment.SelectedIndexChanged += new EventHandler(this.On_lb_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnClose;
    this.Controls.Add((Control) this._tc);
    this.Controls.Add((Control) this._pnlBottom);
    this.DoubleBuffered = true;
    this.Name = nameof (MaterialFavourites);
    this.ShowInTaskbar = false;
    this._pnlBottom.ResumeLayout(false);
    this._tc.ResumeLayout(false);
    this._tpMaterials.ResumeLayout(false);
    this._tpDimensionTypes.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
