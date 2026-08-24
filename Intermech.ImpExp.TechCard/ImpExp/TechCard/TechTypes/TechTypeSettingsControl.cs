// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechTypes.TechTypeSettingsControl
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechTypes.Settings;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechTypes;

public class TechTypeSettingsControl : StepControl, IComparer
{
  private bool _readOnly;
  private int _columnIdx;
  private int _columnAsc;
  private Image _image;
  protected TechTypeList _techTypeList;
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  protected Button btnSettingsSave;
  private Button btnApply;
  private Button btnCancel;
  private Label lblCaption;
  private Panel pnlClient;
  private PropertyGrid pgTypeSett;
  protected ListView lvTypes;
  private ColumnHeader ch_RecID;
  private ColumnHeader ch_Type;
  private ColumnHeader ch_Name;
  private ColumnHeader ch_Status;
  private ColumnHeader ch_ObjType;
  private Splitter splCRight;
  protected Button btnSettingsLoad;

  private TechTypeSett GetDefaultTechTypeSett(TechTypeInfo techInfo, IMetadataInfo metaInfo)
  {
    if (techInfo == null)
      return (TechTypeSett) null;
    TechTypeSett defaultTechTypeSett = new TechTypeSett();
    TechTypeConvertionRule typeConvertionRule;
    if (TechTypeConversionPredefinedRules.ObjectTypeImportRules.TryGetValue(techInfo.RecordID, out typeConvertionRule))
    {
      defaultTechTypeSett.ObjType = typeConvertionRule.PumpTo;
      defaultTechTypeSett.Mode = !typeConvertionRule.Mode.HasFlag((Enum) TechTypeConvertionRuleMode.Hide) ? TechTypePumpMode.ExistObjType : TechTypePumpMode.LockedType;
      defaultTechTypeSett.readOnly = typeConvertionRule.Mode.HasFlag((Enum) TechTypeConvertionRuleMode.ReadOnly);
    }
    else
    {
      IObjectTypeItem byName = metaInfo.ObjectTypes.GetByName(techInfo.Name);
      if (byName != null)
      {
        defaultTechTypeSett.Mode = TechTypePumpMode.ExistObjType;
        defaultTechTypeSett.ObjType = byName.GUID;
      }
    }
    return defaultTechTypeSett;
  }

  protected virtual void InitializeData()
  {
  }

  private void InitializeTypeSettList()
  {
    if (this._techTypeList == null || this._techTypeList.Count == 0 || !(ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service))
      return;
    foreach (TechTypeInfo techInfo in this._techTypeList.Values)
    {
      if (techInfo != null && techInfo.TypeSett == null)
      {
        TechTypeSett defaultTechTypeSett = this.GetDefaultTechTypeSett(techInfo, service);
        if (defaultTechTypeSett != null)
          techInfo.TypeSett = defaultTechTypeSett;
      }
    }
  }

  private void UpdateControls()
  {
    this.pgTypeSett.Enabled = this.btnSettingsSave.Enabled = !this.ReadOnly;
  }

  protected virtual void NormalizeTypeSettings(TechTypeList typeList)
  {
    if (typeList == null || typeList.Count == 0 || this._techTypeList == null || this._techTypeList.Count == 0 || !(ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service))
      return;
    IObjectTypeItemList objectTypes = service.ObjectTypes;
    foreach (KeyValuePair<int, TechTypeInfo> techType in (Dictionary<int, TechTypeInfo>) this._techTypeList)
    {
      TechTypeConvertionRule typeConvertionRule;
      TechTypeInfo techTypeInfo;
      if (techType.Value != null && (!TechTypeConversionPredefinedRules.ObjectTypeImportRules.TryGetValue(techType.Key, out typeConvertionRule) || !typeConvertionRule.Mode.HasFlag((Enum) TechTypeConvertionRuleMode.ReadOnly)) && typeList.TryGetValue(techType.Key, out techTypeInfo) && techTypeInfo?.TypeSett != null)
      {
        if (techTypeInfo.TypeSett.ObjType != Guid.Empty && !objectTypes.ExistsByGuid(techTypeInfo.TypeSett.ObjType))
        {
          techTypeInfo.TypeSett.ObjType = Guid.Empty;
          switch (techTypeInfo.TypeSett.Mode)
          {
            case TechTypePumpMode.ExistObjType:
            case TechTypePumpMode.LockedType:
              techTypeInfo.TypeSett.Mode = TechTypePumpMode.NewObjType;
              break;
          }
        }
        if (techTypeInfo.TypeSett.OwnerType != Guid.Empty && !objectTypes.ExistsByGuid(techTypeInfo.TypeSett.OwnerType))
          techTypeInfo.TypeSett.OwnerType = Guid.Empty;
        techType.Value.TypeSett = techTypeInfo.TypeSett;
      }
    }
  }

  protected void LvFillData()
  {
    this.lvTypes.BeginUpdate();
    this.lvTypes.ListViewItemSorter = (IComparer) null;
    try
    {
      this.lvTypes.Items.Clear();
      if (this._techTypeList == null)
        return;
      foreach (TechTypeInfo techTypeInfo in this._techTypeList.Values)
      {
        TechTypeConvertionRule typeConvertionRule;
        if (!TechTypeConversionPredefinedRules.ObjectTypeImportRules.TryGetValue(techTypeInfo.RecordID, out typeConvertionRule) || !typeConvertionRule.Mode.HasFlag((Enum) TechTypeConvertionRuleMode.Hide))
        {
          ListViewItem listItem = this.lvTypes.Items.Add(string.Empty);
          listItem.Tag = (object) techTypeInfo.RecordID;
          this.LvFillItemData(listItem);
        }
      }
    }
    finally
    {
      this.lvTypes.EndUpdate();
      this.lvTypes.ListViewItemSorter = (IComparer) this;
    }
    this.lvTypes.Sort();
  }

  private void LvFillItemData(ListViewItem listItem)
  {
    if (listItem == null)
      return;
    int tag = (int) listItem.Tag;
    TechTypeConvertionRule typeConvertionRule;
    if (TechTypeConversionPredefinedRules.ObjectTypeImportRules.TryGetValue(tag, out typeConvertionRule) && typeConvertionRule.Mode.HasFlag((Enum) TechTypeConvertionRuleMode.Hide))
      return;
    for (int count = listItem.SubItems.Count; count < 5; ++count)
      listItem.SubItems.Add(string.Empty);
    TechTypeSett techTypeSett = (TechTypeSett) null;
    TechTypeInfo techTypeInfo;
    if (this._techTypeList.TryGetValue(tag, out techTypeInfo))
    {
      techTypeSett = techTypeInfo.TypeSett;
      listItem.SubItems[0].Text = techTypeInfo.RecordID.ToString();
      listItem.SubItems[1].Text = techTypeInfo.Type;
      listItem.SubItems[2].Text = techTypeInfo.Name;
    }
    else
    {
      listItem.SubItems[0].Text = string.Empty;
      listItem.SubItems[1].Text = string.Empty;
      listItem.SubItems[2].Text = string.Empty;
    }
    if (techTypeSett != null)
    {
      listItem.SubItems[3].Text = EnumTypeHelper.GetCaption((Enum) techTypeSett.Mode);
      if (techTypeSett.ObjType != Guid.Empty)
      {
        if (TechcardConsts.Plugin != null && TechcardConsts.Plugin.Imdi != null)
        {
          IObjectTypeItem byGuid = TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(techTypeSett.ObjType);
          listItem.SubItems[4].Text = byGuid != null ? byGuid.Name : string.Empty;
        }
        else
        {
          IMSObjectType objectType = MetaDataHelper.GetObjectType(techTypeSett.ObjType);
          listItem.SubItems[4].Text = objectType != null ? objectType.ObjectTypeName : string.Empty;
        }
      }
    }
    else
    {
      listItem.SubItems[3].Text = string.Empty;
      listItem.SubItems[4].Text = string.Empty;
    }
    if (typeConvertionRule == null || !typeConvertionRule.Mode.HasFlag((Enum) TechTypeConvertionRuleMode.ReadOnly))
      return;
    listItem.ForeColor = Color.Gray;
  }

  protected virtual void LoadTypeSettings()
  {
    using (OpenFileDialog openFileDialog = new OpenFileDialog())
    {
      openFileDialog.Filter = "TechTypes File (*.ttc)|*.ttc";
      openFileDialog.FilterIndex = 0;
      openFileDialog.AddExtension = true;
      openFileDialog.CheckPathExists = true;
      openFileDialog.CheckFileExists = true;
      openFileDialog.RestoreDirectory = true;
      TechTypeList techTypeList;
      if (!openFileDialog.ShowDialog().Equals((object) DialogResult.OK) || !TechTypeListHelper.LoadFromFile(openFileDialog.FileName, out techTypeList))
        return;
      this.NormalizeTypeSettings(techTypeList);
      this.LvFillData();
    }
  }

  private void SaveTypeSettings()
  {
    if (this.ReadOnly)
      return;
    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
    {
      saveFileDialog.Filter = "TechTypes File (*.ttc)|*.ttc";
      saveFileDialog.FilterIndex = 0;
      saveFileDialog.AddExtension = true;
      saveFileDialog.CheckPathExists = true;
      saveFileDialog.RestoreDirectory = true;
      if (!saveFileDialog.ShowDialog().Equals((object) DialogResult.OK))
        return;
      TechTypeListHelper.SaveToFile(saveFileDialog.FileName, this._techTypeList);
    }
  }

  private bool CheckSettingsData()
  {
    bool flag = true;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<int, TechTypeInfo> techType in (Dictionary<int, TechTypeInfo>) this._techTypeList)
    {
      TechTypeSett typeSett = techType.Value.TypeSett;
      if (typeSett == null)
      {
        flag = false;
        stringBuilder.Append(techType.Value.Name + " ");
      }
      else if (typeSett.Mode == TechTypePumpMode.ExistObjType && typeSett.ObjType == Guid.Empty || typeSett.Mode == TechTypePumpMode.NewObjType && typeSett.RelType == Guid.Empty)
      {
        flag = false;
        stringBuilder.Append(techType.Value.Name + " ");
      }
    }
    if (!flag)
    {
      int num = (int) MessageBox.Show("Обнаружены типы записей, для которых не указан тип объекта/связи: " + stringBuilder.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
    return flag;
  }

  private bool PumpData()
  {
    if (!this.CheckSettingsData())
      return false;
    TechPumpData.TechType._techTypeList = this._techTypeList;
    TechCache.WriteOneList(TechCache.CategoryList.TechTypeList, (object) TechPumpData.TechType.TechTypeList);
    return true;
  }

  internal void SetTechTypeList(TechTypeList techTypeList) => this.TechTypeList = techTypeList;

  public TechTypeSettingsControl(object owner)
    : base(owner)
  {
    this.stepPrevAllowed = true;
    this.stepRepumpble = true;
    this.InitializeComponent();
    this.lvTypes.ListViewItemSorter = (IComparer) this;
    this.InitializeData();
  }

  public int Compare(object x, object y)
  {
    if (x == y)
      return 0;
    ListViewItem listViewItem1 = x as ListViewItem;
    ListViewItem listViewItem2 = y as ListViewItem;
    if (listViewItem1 == null)
      return -1;
    if (listViewItem2 == null)
      return 1;
    int num = -1;
    if (this._columnIdx == 0)
    {
      try
      {
        num = Convert.ToInt32(listViewItem1.SubItems[this._columnIdx].Text) - Convert.ToInt32(listViewItem2.SubItems[this._columnIdx].Text);
      }
      catch (FormatException ex)
      {
      }
    }
    else
      num = string.CompareOrdinal(listViewItem1.SubItems[this._columnIdx].Text, listViewItem2.SubItems[this._columnIdx].Text);
    return this._columnAsc == 1 ? -num : num;
  }

  protected override string getCaption() => "Настройка перекачки технологических типов объектов";

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
    {
      int index = service.ImageIndex("imgTechObjTypes");
      if (index != -1)
        this._image = service.ImageList.Images[index];
    }
    return this._image;
  }

  public override SaveSettingsResult SaveSettings() => SaveSettingsResult.ssrOk;

  public override bool LeaveControl()
  {
    int num = this.PumpData() ? 1 : 0;
    if (num == 0)
      return num != 0;
    TechTypeListHelper.SaveToSettings(TechPumpData.TechType.TechTypeList);
    return num != 0;
  }

  public bool ReadOnly
  {
    get => this._readOnly;
    set
    {
      this._readOnly = value;
      this.UpdateControls();
    }
  }

  private TechTypeList TechTypeList
  {
    get => this._techTypeList;
    set
    {
      this._techTypeList = value;
      this.InitializeTypeSettList();
      this.LvFillData();
    }
  }

  private void lvTypes_ColumnClick(object sender, ColumnClickEventArgs e)
  {
    if (e == null)
      return;
    if (this._columnIdx == e.Column)
    {
      this._columnAsc = 1 - this._columnAsc;
    }
    else
    {
      this._columnIdx = e.Column;
      this._columnAsc = 0;
    }
    this.lvTypes.Sort();
  }

  private void lvTypes_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.btnApply.Enabled = this.btnCancel.Enabled = false;
    if (this.lvTypes.SelectedItems.Count == 0)
      return;
    TechTypeInfo techTypeInfo;
    this._techTypeList.TryGetValue((int) this.lvTypes.SelectedItems[0].Tag, out techTypeInfo);
    TechTypeSett typeSett = techTypeInfo?.TypeSett;
    this.pgTypeSett.SelectedObject = (object) typeSett;
    this.pgTypeSett.Enabled = !this._readOnly && typeSett != null && typeSett.Mode != TechTypePumpMode.LockedType;
  }

  private void pgTypeSett_Click(object sender, EventArgs e)
  {
  }

  private void pgTypeSett_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
  {
    if (this.lvTypes.SelectedItems.Count == 0)
      return;
    this.btnApply.Enabled = this.btnCancel.Enabled = true;
    this.LvFillItemData(this.lvTypes.SelectedItems[0]);
  }

  private void btnSettingsLoad_Click(object sender, EventArgs e) => this.LoadTypeSettings();

  private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
  {
  }

  private void btnSettingsSave_Click(object sender, EventArgs e) => this.SaveTypeSettings();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TechTypeSettingsControl));
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.btnSettingsLoad = new Button();
    this.btnSettingsSave = new Button();
    this.btnCancel = new Button();
    this.lblCaption = new Label();
    this.btnApply = new Button();
    this.pnlClient = new Panel();
    this.splCRight = new Splitter();
    this.pgTypeSett = new PropertyGrid();
    this.lvTypes = new ListView();
    this.ch_RecID = new ColumnHeader();
    this.ch_Type = new ColumnHeader();
    this.ch_Name = new ColumnHeader();
    this.ch_Status = new ColumnHeader();
    this.ch_ObjType = new ColumnHeader();
    this.tableLayoutPanel1.SuspendLayout();
    this.pnlClient.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
    this.tableLayoutPanel1.Controls.Add((Control) this.btnSettingsLoad, 1, 7);
    this.tableLayoutPanel1.Controls.Add((Control) this.btnSettingsSave, 0, 7);
    this.tableLayoutPanel1.Controls.Add((Control) this.btnCancel, 7, 7);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblCaption, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.btnApply, 6, 7);
    this.tableLayoutPanel1.Controls.Add((Control) this.pnlClient, 0, 1);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.Paint += new PaintEventHandler(this.tableLayoutPanel1_Paint);
    componentResourceManager.ApplyResources((object) this.btnSettingsLoad, "btnSettingsLoad");
    this.btnSettingsLoad.Name = "btnSettingsLoad";
    this.btnSettingsLoad.UseVisualStyleBackColor = true;
    this.btnSettingsLoad.Click += new EventHandler(this.btnSettingsLoad_Click);
    componentResourceManager.ApplyResources((object) this.btnSettingsSave, "btnSettingsSave");
    this.btnSettingsSave.Name = "btnSettingsSave";
    this.btnSettingsSave.UseVisualStyleBackColor = true;
    this.btnSettingsSave.Click += new EventHandler(this.btnSettingsSave_Click);
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.lblCaption, "lblCaption");
    this.tableLayoutPanel1.SetColumnSpan((Control) this.lblCaption, 8);
    this.lblCaption.Name = "lblCaption";
    componentResourceManager.ApplyResources((object) this.btnApply, "btnApply");
    this.btnApply.Name = "btnApply";
    this.btnApply.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.pnlClient, "pnlClient");
    this.tableLayoutPanel1.SetColumnSpan((Control) this.pnlClient, 8);
    this.pnlClient.Controls.Add((Control) this.splCRight);
    this.pnlClient.Controls.Add((Control) this.pgTypeSett);
    this.pnlClient.Controls.Add((Control) this.lvTypes);
    this.pnlClient.Name = "pnlClient";
    this.tableLayoutPanel1.SetRowSpan((Control) this.pnlClient, 5);
    componentResourceManager.ApplyResources((object) this.splCRight, "splCRight");
    this.splCRight.Name = "splCRight";
    this.splCRight.TabStop = false;
    componentResourceManager.ApplyResources((object) this.pgTypeSett, "pgTypeSett");
    this.pgTypeSett.Name = "pgTypeSett";
    this.pgTypeSett.PropertyValueChanged += new PropertyValueChangedEventHandler(this.pgTypeSett_PropertyValueChanged);
    this.pgTypeSett.Click += new EventHandler(this.pgTypeSett_Click);
    componentResourceManager.ApplyResources((object) this.lvTypes, "lvTypes");
    this.lvTypes.Columns.AddRange(new ColumnHeader[5]
    {
      this.ch_RecID,
      this.ch_Type,
      this.ch_Name,
      this.ch_Status,
      this.ch_ObjType
    });
    this.lvTypes.FullRowSelect = true;
    this.lvTypes.GridLines = true;
    this.lvTypes.HideSelection = false;
    this.lvTypes.MultiSelect = false;
    this.lvTypes.Name = "lvTypes";
    this.lvTypes.UseCompatibleStateImageBehavior = false;
    this.lvTypes.View = View.Details;
    this.lvTypes.ColumnClick += new ColumnClickEventHandler(this.lvTypes_ColumnClick);
    this.lvTypes.SelectedIndexChanged += new EventHandler(this.lvTypes_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this.ch_RecID, "ch_RecID");
    componentResourceManager.ApplyResources((object) this.ch_Type, "ch_Type");
    componentResourceManager.ApplyResources((object) this.ch_Name, "ch_Name");
    componentResourceManager.ApplyResources((object) this.ch_Status, "ch_Status");
    componentResourceManager.ApplyResources((object) this.ch_ObjType, "ch_ObjType");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (TechTypeSettingsControl);
    this.Tag = (object) " ";
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.pnlClient.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
