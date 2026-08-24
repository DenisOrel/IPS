// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.CoatingFavouritesForm
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

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

public class CoatingFavouritesForm : Form
{
  private Guid _userGuid = Guid.Empty;
  private Guid _categoryGuid = Guid.Empty;
  private string _coatingsKey = string.Empty;
  private string _materialsKey = string.Empty;
  private string _caption = string.Empty;
  private List<object> _params;
  private bool _modified;
  private IContainer components;
  private Panel _pnlBottom;
  private Button _btnCancel;
  private Button _btnDel;
  private Button _btnApply;
  private ListBox _lbFavourites;
  private Button _btnGoTo;
  private Button _btnClose;

  public CoatingsFavouriteData Data => this._lbFavourites.SelectedItem as CoatingsFavouriteData;

  public CoatingFavouritesForm(
    Guid categoryGuid,
    string coatingsKey,
    string materialsKey,
    List<object> parameters,
    string caption)
  {
    this.InitializeComponent();
    this._categoryGuid = categoryGuid;
    this._coatingsKey = coatingsKey;
    this._materialsKey = materialsKey;
    this._params = parameters;
    this._caption = caption;
    this._btnApply.Enabled = false;
    this.LoadData();
    this.CheckEnableButtons();
  }

  private void On_btnDel_Click(object sender, EventArgs e)
  {
    int selectedIndex = this._lbFavourites.SelectedIndex;
    this._lbFavourites.BeginUpdate();
    try
    {
      this._lbFavourites.Items.Remove(this._lbFavourites.SelectedItem);
    }
    finally
    {
      this._lbFavourites.EndUpdate();
    }
    this._modified = true;
    if (this._lbFavourites.Items.Count > selectedIndex)
      this._lbFavourites.SelectedIndex = selectedIndex;
    else if (this._lbFavourites.Items.Count > 0)
      this._lbFavourites.SelectedIndex = this._lbFavourites.Items.Count - 1;
    this.CheckEnableButtons();
  }

  private void On_btnGoTo_Click(object sender, EventArgs e)
  {
    if (this._lbFavourites.SelectedItem == null)
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

  private void On_lbFavourites_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.CheckEnableButtons();
  }

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    FormStorage.LoadLayout((Control) this);
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    if (!this._modified || MessageBox.Show(LocalizationHolder.rm.GetString("IMH_MaterialFavourites_Changed_Msg"), LocalizationHolder.rm.GetString("IMH_DataChanged"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
    this._lbFavourites.BeginUpdate();
    try
    {
      this._lbFavourites.Items.Clear();
      List<CoatingsFavouriteData> coatingsFavouriteDataList = (List<CoatingsFavouriteData>) null;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IIMHUserSettingsService)) is IIMHUserSettingsService customService)
        {
          QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(sessionKeeper.Session.UserID);
          if (!objectInfo.Empty)
          {
            this._userGuid = objectInfo.VersionGuid;
            coatingsFavouriteDataList = customService.GetCoatingFavourites(this._userGuid, this._categoryGuid);
            coatingsFavouriteDataList.ForEach((Action<CoatingsFavouriteData>) (x => this._lbFavourites.Items.Add((object) x)));
          }
        }
      }
      if (string.IsNullOrEmpty(this._coatingsKey) || string.IsNullOrEmpty(this._materialsKey) || this._params == null || this._params.Count <= 0)
        return;
      CoatingsFavouriteData coatingsFavouriteData = new CoatingsFavouriteData(this._coatingsKey, this._materialsKey, this._params, this._caption);
      if (!coatingsFavouriteDataList.Contains(coatingsFavouriteData))
      {
        this._lbFavourites.SelectedIndex = this._lbFavourites.Items.Add((object) coatingsFavouriteData);
        this._modified = true;
      }
      else
        this._lbFavourites.SelectedItem = (object) coatingsFavouriteData;
    }
    finally
    {
      this._lbFavourites.EndUpdate();
    }
  }

  private void Save()
  {
    if ((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IIMHUserSettingsService)) is IIMHUserSettingsService customService)
    {
      List<CoatingsFavouriteData> coatingFavourites = new List<CoatingsFavouriteData>(this._lbFavourites.Items.Count);
      foreach (object obj in this._lbFavourites.Items)
        coatingFavourites.Add(obj as CoatingsFavouriteData);
      customService.SaveCoatingFavourites(this._userGuid, this._categoryGuid, coatingFavourites);
      customService.SaveUserSettings();
    }
    this._modified = false;
  }

  private void Cancel()
  {
    this._modified = false;
    this._coatingsKey = this._materialsKey = string.Empty;
    this._params = (List<object>) null;
    this.LoadData();
  }

  private void CheckEnableButtons()
  {
    this._btnDel.Enabled = this._btnGoTo.Enabled = this._lbFavourites.SelectedItems.Count > 0;
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CoatingFavouritesForm));
    this._pnlBottom = new Panel();
    this._btnClose = new Button();
    this._btnGoTo = new Button();
    this._btnApply = new Button();
    this._btnDel = new Button();
    this._btnCancel = new Button();
    this._lbFavourites = new ListBox();
    this._pnlBottom.SuspendLayout();
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
    componentResourceManager.ApplyResources((object) this._lbFavourites, "_lbFavourites");
    this._lbFavourites.FormattingEnabled = true;
    this._lbFavourites.Name = "_lbFavourites";
    this._lbFavourites.Sorted = true;
    this._lbFavourites.SelectedIndexChanged += new EventHandler(this.On_lbFavourites_SelectedIndexChanged);
    this._lbFavourites.DoubleClick += new EventHandler(this.On_btnGoTo_Click);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnClose;
    this.Controls.Add((Control) this._lbFavourites);
    this.Controls.Add((Control) this._pnlBottom);
    this.DoubleBuffered = true;
    this.Name = nameof (CoatingFavouritesForm);
    this.ShowInTaskbar = false;
    this._pnlBottom.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
