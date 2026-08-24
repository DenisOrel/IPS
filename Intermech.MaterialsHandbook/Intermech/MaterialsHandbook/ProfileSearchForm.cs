// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ProfileSearchForm
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class ProfileSearchForm : BaseSearchForm
{
  private string _sourceCassifKey = string.Empty;
  private IContainer components;

  public long FolderID { get; private set; }

  public ProfileSearchForm()
  {
    this.InitializeComponent();
    this.FolderID = 0L;
    this._cmbSearchIn.Items.Add((object) LocalizationHolder.rm.GetString("IMH_Search_Profiles"));
    this._cmbSearchIn.SelectedIndex = 0;
  }

  protected override void On_btnSearch_Click(object sender, EventArgs e)
  {
    string text = this._txtSearch.Text;
    this._lvResult.SuspendLayout();
    try
    {
      this._lvResult.Items.Clear();
      if (string.IsNullOrEmpty(text))
        return;
      RelationalOperators relOperator = this.GetRelOperator();
      List<ProfileSearchForm.LvItem> profiles = this.GetProfiles(text, relOperator);
      if (profiles == null)
        return;
      foreach (ProfileSearchForm.LvItem lvItem in profiles)
        this._lvResult.Items.Add(new ListViewItem(new string[2]
        {
          lvItem.Caption,
          lvItem.Path
        })
        {
          Name = lvItem.ObjID.ToString()
        });
    }
    finally
    {
      this._lvResult.ResumeLayout();
    }
  }

  protected override void On_lvResult_DoubleClick(object sender, EventArgs e)
  {
    if (this._lvResult.SelectedItems.Count <= 0)
      return;
    this.FolderID = Convert.ToInt64(this._lvResult.SelectedItems[0].Name);
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  private RelationalOperators GetRelOperator()
  {
    return !this._rbExactly.Checked ? (!this._rbBeg.Checked ? (!this._rbEntry.Checked ? RelationalOperators.EndString : RelationalOperators.Substring) : RelationalOperators.StartString) : RelationalOperators.Equal;
  }

  private List<ProfileSearchForm.LvItem> GetProfiles(string text, RelationalOperators relOperator)
  {
    List<ProfileSearchForm.LvItem> profiles = (List<ProfileSearchForm.LvItem>) null;
    if (relOperator != RelationalOperators.Empty)
    {
      DataTable dataTable = this.GetFolderIDs(text, relOperator);
      if (dataTable != null)
      {
        profiles = new List<ProfileSearchForm.LvItem>(dataTable.Rows.Count);
        List<long> longList = new List<long>(dataTable.Rows.Count);
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          longList.Add(Convert.ToInt64(row[-2.ToString()]));
        if (longList.Count > 0)
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService)
            {
              dataTable = customService.GetFoldersForObjects(sessionKeeper.Session.SessionGUID, longList.ToArray(), (long[]) null);
              dataTable.DefaultView.Sort = "F_PATH ASC";
              dataTable = dataTable.DefaultView.ToTable();
            }
          }
        }
        Dictionary<string, string> dictionary = new Dictionary<string, string>(dataTable.Rows.Count);
        dictionary.Add(this._sourceCassifKey, LocalizationHolder.rm.GetString("IMH_Search_Profiles_RootFolder"));
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        string empty4 = string.Empty;
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          string key1 = Convert.ToString(row["F_PATH"]);
          if (key1.Length > this._sourceCassifKey.Length)
          {
            string key2 = key1.Substring(0, key1.Length - 2);
            string caption = Convert.ToString(row["CAPTION"]);
            long int64 = Convert.ToInt64(row["F_OBJECT_ID"]);
            if (longList.Contains(int64))
              profiles.Add(new ProfileSearchForm.LvItem(int64, caption, $"{dictionary[key2]}\\"));
            string str = $"{dictionary[key2]}\\{caption}";
            dictionary.Add(key1, str);
          }
        }
      }
    }
    return profiles;
  }

  private DataTable GetFolderIDs(string text, RelationalOperators relOperator)
  {
    DataTable folderIds = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseFolderTypeGUID);
      if (objectCollection != null)
      {
        this._sourceCassifKey = IMHHelper.GetClassifFolderKey("ADDITION_MATERIALS_CTL");
        if (!string.IsNullOrEmpty(this._sourceCassifKey))
        {
          int attributeTypeId = MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545");
          DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
          {
            new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) this._sourceCassifKey, LogicalOperators.AND, 0, false),
            new ConditionStructure(attributeTypeId, relOperator, (object) text, LogicalOperators.NONE, 0, false)
          }, new List<ColumnDescriptor>()
          {
            new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
          }.ToArray());
          folderIds = objectCollection.Select(paramSet);
        }
      }
    }
    return folderIds;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProfileSearchForm));
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this._pnlMaterialSearch.SuspendLayout();
    this._pnl.SuspendLayout();
    this.SuspendLayout();
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(804, 482);
    this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
    this.Name = nameof (ProfileSearchForm);
    this.Text = "Поиск профиля";
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this._pnlMaterialSearch.ResumeLayout(false);
    this._pnlMaterialSearch.PerformLayout();
    this._pnl.ResumeLayout(false);
    this._pnl.PerformLayout();
    this.ResumeLayout(false);
  }

  private class LvItem
  {
    internal long ObjID;
    internal string Caption = string.Empty;
    internal string Path = string.Empty;

    internal LvItem(long objID, string caption, string path)
    {
      this.ObjID = objID;
      this.Caption = caption;
      this.Path = path;
    }
  }
}
