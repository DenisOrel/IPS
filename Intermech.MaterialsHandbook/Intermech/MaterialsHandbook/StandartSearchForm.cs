// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.StandartSearchForm
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
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

public class StandartSearchForm : BaseSearchForm
{
  private long _tableRefID;
  private long _recID = -1;
  private Guid _baseMaterialAttr = Guid.Empty;
  private IContainer components;

  public bool IsMaterial { get; private set; }

  public long RecID => this._recID;

  public string StandartText { get; private set; }

  public long TableRefID => this._tableRefID;

  public long aTableRefID { get; private set; }

  public StandartSearchForm()
  {
    this.InitializeComponent();
    this.IsMaterial = true;
    this.aTableRefID = 0L;
    this._cmbSearchIn.Items.Add((object) LocalizationHolder.rm.GetString("IMH_Search_Standart"));
    this._cmbSearchIn.SelectedIndex = 0;
    if (!((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IIMHSystemSettingsService)) is IIMHSystemSettingsService customService))
      return;
    this._baseMaterialAttr = customService.GetObjectGuidByName("BASE_MATERIAL_ATTR");
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
      List<string> stringList = new List<string>();
      Dictionary<string, string> materialsData = this.GetMaterialsData(text, relOperator, stringList);
      Dictionary<string, Dictionary<long, string>> assortmentData = this.GetAssortmentData(text, relOperator, stringList);
      if (stringList.Count <= 0)
        return;
      Dictionary<string, string> dictKeys = (Dictionary<string, string>) null;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) is IImbaseServer customService)
          dictKeys = customService.NameRecordReferences(sessionKeeper.Session.SessionGUID, stringList);
      }
      if (dictKeys == null)
        return;
      this.CreateMaterialItems(dictKeys, materialsData);
      this.CreateAssortmentItems(dictKeys, assortmentData);
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
    ListViewItem selectedItem = this._lvResult.SelectedItems[0];
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, selectedItem.Name, out this._tableRefID, out this._recID))
        return;
      StandartSearchForm.LvItem tag = selectedItem.Tag as StandartSearchForm.LvItem;
      this.IsMaterial = tag.IsMaterial;
      this.StandartText = tag.StandartText;
      this.aTableRefID = tag.aTableRefID;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
  }

  private void CreateAssortmentItems(
    Dictionary<string, string> dictKeys,
    Dictionary<string, Dictionary<long, string>> dictStandarts)
  {
    if (dictStandarts.Count <= 0)
      return;
    foreach (KeyValuePair<string, string> dictKey in dictKeys)
    {
      if (dictStandarts.ContainsKey(dictKey.Key))
      {
        foreach (KeyValuePair<long, string> keyValuePair in dictStandarts[dictKey.Key])
          this._lvResult.Items.Add(new ListViewItem(new string[2]
          {
            dictKey.Value,
            string.Format(LocalizationHolder.rm.GetString("IMH_Search_Standart_Asortment"), (object) keyValuePair.Value)
          })
          {
            Name = dictKey.Key,
            Tag = (object) new StandartSearchForm.LvItem(false, keyValuePair.Value, keyValuePair.Key)
          });
      }
    }
  }

  private void CreateMaterialItems(
    Dictionary<string, string> dictKeys,
    Dictionary<string, string> dictStandarts)
  {
    if (dictStandarts.Count <= 0)
      return;
    string empty = string.Empty;
    foreach (KeyValuePair<string, string> dictKey in dictKeys)
    {
      if (dictStandarts.ContainsKey(dictKey.Key))
      {
        string dictStandart = dictStandarts[dictKey.Key];
        this._lvResult.Items.Add(new ListViewItem(new string[2]
        {
          dictKey.Value,
          string.Format(LocalizationHolder.rm.GetString("IMH_Search_Standart_Materials"), (object) dictStandart)
        })
        {
          Name = dictKey.Key,
          Tag = (object) new StandartSearchForm.LvItem(true, dictStandart, 0L)
        });
      }
    }
  }

  private Dictionary<string, Dictionary<long, string>> GetAssortmentData(
    string text,
    RelationalOperators relOperator,
    List<string> imbaseKeys)
  {
    Dictionary<string, Dictionary<long, string>> assortmentData = new Dictionary<string, Dictionary<long, string>>();
    if (relOperator != RelationalOperators.Empty)
    {
      DataTable assortmentTableRefId = this.GetAssortmentTableRefID(text, relOperator);
      if (assortmentTableRefId != null && this._baseMaterialAttr != Guid.Empty)
      {
        int attributeTypeId = MetaDataHelper.GetAttributeTypeID(this._baseMaterialAttr);
        string columnName1 = Intermech.Imbase.Consts.StandartAssortmentAttrID.ToString();
        string columnName2 = attributeTypeId.ToString();
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        foreach (DataRow row in (InternalDataCollectionBase) assortmentTableRefId.Rows)
        {
          long int64 = Convert.ToInt64(row[-2.ToString()]);
          string str = Convert.ToString(row[columnName1]);
          string key = Convert.ToString(row[columnName2]);
          if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(key))
          {
            if (!imbaseKeys.Contains(key))
              imbaseKeys.Add(key);
            if (assortmentData.ContainsKey(key))
              assortmentData[key][int64] = str;
            else
              assortmentData.Add(key, new Dictionary<long, string>(0)
              {
                [int64] = str
              });
          }
        }
      }
    }
    return assortmentData;
  }

  private DataTable GetAssortmentTableRefID(string text, RelationalOperators relOperator)
  {
    DataTable assortmentTableRefId = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeGUID);
      if (objectCollection != null)
      {
        string classifFolderKey = IMHHelper.GetClassifFolderKey("ASSORTMENT_FOLDER_NAME");
        if (!string.IsNullOrEmpty(classifFolderKey))
        {
          if (this._baseMaterialAttr != Guid.Empty)
          {
            int attributeTypeId = MetaDataHelper.GetAttributeTypeID(this._baseMaterialAttr);
            DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[3]
            {
              new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) classifFolderKey, LogicalOperators.AND, 0, false),
              new ConditionStructure(Intermech.Imbase.Consts.StandartAssortmentAttrID, relOperator, (object) text, LogicalOperators.AND, 0, false),
              new ConditionStructure(attributeTypeId, RelationalOperators.NotEmpty, (object) null, LogicalOperators.NONE, 0, false)
            }, new List<ColumnDescriptor>()
            {
              new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
              new ColumnDescriptor((object) Intermech.Imbase.Consts.StandartAssortmentAttrID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
              new ColumnDescriptor((object) attributeTypeId, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
            }.ToArray());
            assortmentTableRefId = objectCollection.Select(paramSet);
          }
        }
      }
    }
    return assortmentTableRefId;
  }

  private Dictionary<string, string> GetMaterialsData(
    string text,
    RelationalOperators relOperator,
    List<string> imbaseKeys)
  {
    Dictionary<string, string> materialsData = new Dictionary<string, string>();
    if (relOperator != RelationalOperators.Empty)
    {
      DataTable materialsTableRefId = this.GetMaterialsTableRefID(text, relOperator);
      if (materialsTableRefId != null)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          string columnName1 = Intermech.Imbase.Consts.StandartAttrID.ToString();
          string columnName2 = Intermech.Imbase.Consts.ImbaseTableRefAttID.ToString();
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          foreach (DataRow row1 in (InternalDataCollectionBase) materialsTableRefId.Rows)
          {
            long int64 = Convert.ToInt64(row1[-2.ToString()]);
            string str = Convert.ToString(row1[columnName1]);
            long result = 0;
            if (long.TryParse(Convert.ToString(row1[columnName2]), out result) && result != 0L)
            {
              DataSet tables = TableLoadHelper.GetTables(sessionKeeper.Session, result, true);
              if (tables != null && tables.Tables.Contains("IMS_DATA"))
              {
                foreach (DataRow row2 in (InternalDataCollectionBase) tables.Tables["IMS_DATA"].Rows)
                {
                  object obj = row2["F_KEY"];
                  string key = ImbaseHelper.MakeInternalImbaseKey(int64, Convert.ToInt64(obj));
                  imbaseKeys.Add(key);
                  materialsData.Add(key, str);
                }
              }
            }
          }
        }
      }
    }
    return materialsData;
  }

  private DataTable GetMaterialsTableRefID(string text, RelationalOperators relOperator)
  {
    DataTable materialsTableRefId = (DataTable) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectCollection objectCollection = sessionKeeper.Session.GetObjectCollection(Intermech.Imbase.Consts.ImbaseTableRefTypeGUID);
      if (objectCollection != null)
      {
        string classifFolderKey = IMHHelper.GetClassifFolderKey("BASE_MATERIALS_CTL");
        if (!string.IsNullOrEmpty(classifFolderKey))
        {
          DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
          {
            new ConditionStructure(Intermech.Imbase.Consts.ClassifFolderKeyAttId, RelationalOperators.StartString, (object) classifFolderKey, LogicalOperators.AND, 0, false),
            new ConditionStructure(Intermech.Imbase.Consts.StandartAttrID, relOperator, (object) text, LogicalOperators.NONE, 0, false)
          }, new List<ColumnDescriptor>()
          {
            new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
            new ColumnDescriptor((object) Intermech.Imbase.Consts.StandartAttrID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
            new ColumnDescriptor((object) Intermech.Imbase.Consts.ImbaseTableRefAttID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0)
          }.ToArray());
          materialsTableRefId = objectCollection.Select(paramSet);
        }
      }
    }
    return materialsTableRefId;
  }

  private RelationalOperators GetRelOperator()
  {
    return !this._rbExactly.Checked ? (!this._rbBeg.Checked ? (!this._rbEntry.Checked ? RelationalOperators.EndString : RelationalOperators.Substring) : RelationalOperators.StartString) : RelationalOperators.Equal;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (StandartSearchForm));
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
    this.Name = nameof (StandartSearchForm);
    this.Text = "Поиск нормативного документа";
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
    internal bool IsMaterial = true;
    internal string StandartText = string.Empty;
    internal long aTableRefID;

    internal LvItem(bool isMaterial, string standartText, long tableRefID)
    {
      this.IsMaterial = isMaterial;
      this.StandartText = standartText;
      this.aTableRefID = tableRefID;
    }
  }
}
