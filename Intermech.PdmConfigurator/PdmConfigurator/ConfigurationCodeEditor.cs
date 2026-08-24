// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.ConfigurationCodeEditor
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class ConfigurationCodeEditor : UserControl
{
  private bool _isChanged;
  private long _objectID;
  private int _objectTypeID = -1;
  private ObjectOptionsHolder _objectOptionsHolder = new ObjectOptionsHolder();
  private ConfigurationCode _configurationCode = new ConfigurationCode();
  private SortedList<string, string> _sortOptionList = new SortedList<string, string>();
  private SortedList<string, string> _sortAttrList = new SortedList<string, string>();
  private OptionAccessRights _optionAccessRights;
  private iGCellStyle _cellStringStyle = new iGCellStyle(true);
  private iGCellStyle _cellReadOnlyStyle = new iGCellStyle(true);
  private iGCellStyle _cellCodeTypeComboStyle = new iGCellStyle(true);
  private iGDropDownList _ddlCodeType;
  private IContainer components;
  private Intermech.Bars.ToolBar tbCodeWork;
  private ButtonItem _addButtonItem;
  private ButtonItem _deleteButtonItem;
  private ButtonItem _clearButtonItem;
  private ImageList ilLinked;
  private iGrid _grid;
  private Panel panel1;
  private Label lbCaptionSample;
  private Label lbCaptionStructure;
  private MenuBar cmsCodeWork;
  private ContextMenuBarItem contextMenuBarItem;
  private MenuButtonItem tsmAddCodePart;
  private MenuButtonItem tsmDeleteCodePart;
  private MenuButtonItem tsmClear;
  private ButtonItem _moteToTopButtonItem;
  private ButtonItem _moveToLeftButtonItem;
  private ButtonItem _moveToRightButtonitem;
  private ButtonItem _moveToEndButtonItem;
  private Timer _refreshTimer;
  private TextBox lbStructure;
  private Panel errorPanel;
  private Label lbErrorState;
  private PictureBox pbError;
  private TextBox lbSample;
  private Label panelHint;

  public ConfigurationCodeEditor()
  {
    this.InitializeComponent();
    if (!(ServicesManager.GetService(typeof (BarManager)) is BarManager service))
      return;
    this.tbCodeWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
    this.cmsCodeWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
    service.RendererChanged += new EventHandler(this.BarManager_RendererChanged);
    this.BarManager_RendererChanged((object) service, EventArgs.Empty);
  }

  public event ConfigurationCodeEditor.ObjectOptionsChangedEventHandler OnChanged;

  public bool IsChanged
  {
    get => this._isChanged;
    set
    {
      this._isChanged = value;
      this.RaiseOnChanged();
    }
  }

  public void LoadConfigurationCode(
    ObjectOptionsHolder options,
    int objectTypeID,
    OptionAccessRights accessRights)
  {
    this.IsChanged = false;
    this._optionAccessRights = accessRights;
    this._objectOptionsHolder = options;
    this._objectID = options.ObjectID;
    this._objectTypeID = objectTypeID;
    this._grid.Cols.Clear();
    this.InitCodeTypesCombo();
    this.InitOptionsList();
    this.InitAttributeTypesList();
    this.CellStylesCreate();
    this.LoadCode();
    this.UpdateControls();
  }

  public void Save()
  {
    this._configurationCode.Clear();
    for (int colOrder = 0; colOrder < this._grid.Cols.Count; ++colOrder)
    {
      iGCol iGcol = this._grid.Cols.FromOrder(colOrder);
      CodePartType type = iGcol.Cells[0].Value == null ? CodePartType.Undefined : (CodePartType) iGcol.Cells[0].Value;
      object obj = type == CodePartType.FixedText || iGcol.Cells[1].Value == null ? iGcol.Cells[1].Value : (!(iGcol.Cells[1].AuxValue is iGDropDownListItem auxValue) ? (iGcol.Cells[1].Value as MyElement).Value : auxValue.Value);
      this._configurationCode.AddCodePart(type, obj);
    }
    this._objectOptionsHolder.Incompatibilities.ConfigurationCode = this._configurationCode;
    this.IsChanged = false;
  }

  private void BarManager_RendererChanged(object sender, EventArgs e)
  {
    this.tbCodeWork.Renderer = this.cmsCodeWork.Renderer = (sender as BarManager).Renderer;
  }

  private void AddButtonItem_Click(object sender, EventArgs e)
  {
    if (this._grid.IsEditing)
      this._grid.CommitEditCurCell();
    this.AddColumn();
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void DeleteButtonItem_Click(object sender, EventArgs e)
  {
    this._grid.Cols.RemoveAt(this._grid.CurCell.ColIndex);
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void ClearButtonItem_Click(object sender, EventArgs e)
  {
    if (IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), LocalizationHolder.rm.GetString("PdmConfigurator_25"), MessageBoxButtons.YesNo, IMMessageBoxImage.Question) != DialogResult.Yes)
      return;
    this._grid.Cols.Clear();
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void MoveToTopButtonItem_Click(object sender, EventArgs e)
  {
    this._grid.Cols[this._grid.CurCell.ColIndex].Move(0);
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void MoveToLeftButtonItem_Click(object sender, EventArgs e)
  {
    iGCol col = this._grid.Cols[this._grid.CurCell.ColIndex];
    col.Move(col.Order - 1);
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void MoveToRightButtonItem_Click(object sender, EventArgs e)
  {
    iGCol col = this._grid.Cols[this._grid.CurCell.ColIndex];
    col.Move(col.Order + 1);
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void MoveToEndButtonItem_Click(object sender, EventArgs e)
  {
    this._grid.Cols[this._grid.CurCell.ColIndex].Move(this._grid.Cols.Count - 1);
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void Grid_AfterCommitEdit(object sender, iGAfterCommitEditEventArgs e)
  {
    this._grid.Cells[2, e.ColIndex].Value = (object) ErrorState.None;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void Grid_BeforeCommitEdit(object sender, iGBeforeCommitEditEventArgs e)
  {
    if (e.RowIndex == 1)
    {
      switch ((CodePartType) this._grid.Cells[0, e.ColIndex].Value)
      {
        case CodePartType.ObjectAttribute:
          if (Convert.ToInt32((e.NewDropDownControlItem as iGDropDownListItem).Value) != 0)
            break;
          e.Result = iGEditResult.Cancel;
          this._refreshTimer.Enabled = true;
          break;
        case CodePartType.FixedText:
          e.NewValue = (object) e.NewText;
          break;
      }
    }
    else
    {
      object obj = this._grid.Cells[e.RowIndex, e.ColIndex].Value;
      CodePartType newValue = (CodePartType) e.NewValue;
      CodePartType codePartType = obj == null ? CodePartType.Undefined : (CodePartType) obj;
      if (newValue == codePartType)
        return;
      this._grid.Cells[1, e.ColIndex].Value = (object) null;
      this._grid.Cells[1, e.ColIndex].AuxValue = (object) null;
      this._grid.Cells[1, e.ColIndex].Style = this.GetCellStyle(newValue);
    }
  }

  private void Grid_CurCellChanged(object sender, EventArgs e) => this.UpdateControls();

  private void Grid_ColHdrEndDrag(object sender, iGColHdrEndDragEventArgs e)
  {
    this.UpdateControls();
    this.IsChanged = true;
  }

  private void RefreshTimer_Tick(object sender, EventArgs e)
  {
    this._refreshTimer.Enabled = false;
    iGCell curCell = this._grid.CurCell;
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false))
    {
      if (attributesSelectDlg.ShowDialog() != DialogResult.OK || attributesSelectDlg.SelectedAttributesID.Count <= 0)
        return;
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributesSelectDlg.SelectedAttributesID[0]);
      if (!this._sortAttrList.ContainsValue(attributeType.AttributeID.ToString()))
        this._sortAttrList.Add(MetaDataHelper.GetAttributeTypeName(attributeType.AttributeID), attributeType.AttributeID.ToString());
      int index = this._sortAttrList.IndexOfValue(attributeType.AttributeID.ToString());
      this._grid.Cells[1, curCell.ColIndex].Style = this.GetCellStyle(CodePartType.ObjectAttribute);
      iGDropDownListItem gdropDownListItem = (this._grid.Cells[1, curCell.ColIndex].DropDownControl as iGDropDownList).Items[index];
      this._grid.Cells[1, curCell.ColIndex].Value = (object) attributeType.AttributeID;
      this._grid.Cells[1, curCell.ColIndex].AuxValue = (object) gdropDownListItem;
      this.UpdateControls();
      this.IsChanged = true;
    }
  }

  private void RaiseOnChanged()
  {
    if (this.OnChanged == null)
      return;
    this.OnChanged((object) this, new EventArgs());
  }

  private void UpdateControls()
  {
    this.panelHint.Visible = this._grid.ReadOnly = this._optionAccessRights != OptionAccessRights.FullAccess;
    this.cmsCodeWork.Enabled = this.tbCodeWork.Enabled = this._optionAccessRights == OptionAccessRights.FullAccess;
    this.CreateCodeStructure();
    if (this._optionAccessRights != OptionAccessRights.FullAccess)
      return;
    this._clearButtonItem.Enabled = this._moveToLeftButtonItem.Enabled = this._moveToRightButtonitem.Enabled = this._moveToEndButtonItem.Enabled = this._moteToTopButtonItem.Enabled = this._grid.Cols.Count > 0;
    this._deleteButtonItem.Enabled = this._grid.CurCell != null;
    if (this._grid.CurCell != null)
    {
      iGCol col = this._grid.Cols[this._grid.CurCell.ColIndex];
      this._moveToLeftButtonItem.Enabled = this._moteToTopButtonItem.Enabled = col.Order != 0;
      this._moveToRightButtonitem.Enabled = this._moveToEndButtonItem.Enabled = col.Order != this._grid.Cols.Count - 1;
      ErrorState errorState = (ErrorState) this._grid.Cells[2, col.Index].Value;
      this.errorPanel.Visible = errorState != 0;
      this._grid.Cells[1, col.Index].ImageIndex = errorState != ErrorState.None ? 0 : -1;
    }
    else
      this.errorPanel.Visible = false;
  }

  private void LoadCode()
  {
    foreach (CodePartProperties codePart in this._objectOptionsHolder.Incompatibilities.ConfigurationCode.CodeParts)
    {
      iGCol iGcol = this.AddColumn();
      iGcol.Cells[0].Value = codePart.codePartType != CodePartType.Undefined ? (object) codePart.codePartType : (object) null;
      iGcol.Cells[1].Style = this.GetCellStyle(codePart.codePartType);
      if (codePart.codePartValue != null)
      {
        if (codePart.codePartType == CodePartType.OptionValueCode || codePart.codePartType == CodePartType.OptionCode)
        {
          long int64 = Convert.ToInt64(codePart.codePartValue);
          if (!this._objectOptionsHolder.Options.Contains(int64))
          {
            iGcol.Cells[2].Value = (object) ErrorState.Option;
            iGcol.Cells[1].ImageIndex = 0;
            OptionHolder option = PdmConfiguratorCache.CacheFindOption(int64);
            if (option == null)
            {
              using (SessionKeeper sessionKeeper = new SessionKeeper())
              {
                PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, int64);
                option = PdmConfiguratorCache.CacheFindOption(int64);
              }
            }
            string empty = string.Empty;
            string caption = codePart.codePartType != CodePartType.OptionValueCode ? (option == null ? string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_24"), (object) int64) : option.OptionCaption) : (option == null || option.OptionValues.Count == 0 ? string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_23"), (object) int64) : option.OptionValues[0].Value);
            iGcol.Cells[1].Value = (object) new MyElement((object) int64, caption, (object) null);
            continue;
          }
        }
        iGcol.Cells[1].Value = codePart.codePartValue;
        iGcol.Cells[2].Value = (object) ErrorState.None;
      }
    }
  }

  private void CellStylesCreate()
  {
    this._cellStringStyle.TextAlign = iGContentAlignment.TopLeft;
    this._cellStringStyle.ReadOnly = iGBool.False;
    this._cellStringStyle.EmptyStringAs = iGEmptyStringAs.EmptyString;
    this._cellReadOnlyStyle.TextAlign = iGContentAlignment.TopLeft;
    this._cellReadOnlyStyle.ReadOnly = iGBool.True;
    this._cellReadOnlyStyle.Type = iGCellType.Combo;
    this._cellReadOnlyStyle.EmptyStringAs = iGEmptyStringAs.EmptyString;
    this._cellCodeTypeComboStyle.TextAlign = iGContentAlignment.TopLeft;
    this._cellCodeTypeComboStyle.ReadOnly = iGBool.False;
    this._cellCodeTypeComboStyle.Type = iGCellType.Combo;
    this._cellCodeTypeComboStyle.EmptyStringAs = iGEmptyStringAs.EmptyString;
    this._cellCodeTypeComboStyle.DropDownControl = (IiGDropDownControl) this._ddlCodeType;
    this._cellCodeTypeComboStyle.ImageList = this.ilLinked;
  }

  private void InitCodeTypesCombo()
  {
    this._ddlCodeType = new iGDropDownList();
    this._ddlCodeType.AutoWidth = true;
    this._ddlCodeType.MaxVisibleRowCount = 15;
    foreach (int num in Enum.GetValues(typeof (CodePartType)))
    {
      if (num != -1)
      {
        CodePartType codePartType = (CodePartType) num;
        string enumDescription = EnumDescConverter.GetEnumDescription((Enum) codePartType);
        this._ddlCodeType.Items.Add((object) codePartType, enumDescription);
      }
    }
  }

  private void InitOptionsList()
  {
    this._sortOptionList.Clear();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (long option1 in this._objectOptionsHolder.Options)
      {
        OptionHolder option2 = PdmConfiguratorCache.CacheFindOption(option1);
        if (option2 == null)
        {
          PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, option1);
          option2 = PdmConfiguratorCache.CacheFindOption(option1);
        }
        if (option2 != null)
          this._sortOptionList.Add(option2.OptionCaption, option2.OptionObjectID.ToString());
      }
    }
  }

  private void InitAttributeTypesList()
  {
    this._sortAttrList.Clear();
    foreach (IMSAttribute4ObjectType attribute4ObjectType in MetaDataHelper.GetAttribute4ObjectTypeList(this._objectTypeID))
      this._sortAttrList.Add(MetaDataHelper.GetAttributeTypeName(attribute4ObjectType.AttributeID), attribute4ObjectType.AttributeID.ToString());
    foreach (CodePartProperties codePart in this._objectOptionsHolder.Incompatibilities.ConfigurationCode.CodeParts)
    {
      if (codePart.codePartType == CodePartType.ObjectAttribute && codePart.codePartValue != null)
      {
        string str = codePart.codePartValue.ToString();
        if (!this._sortAttrList.ContainsValue(str))
          this._sortAttrList.Add(MetaDataHelper.GetAttributeTypeName(Convert.ToInt32(str)), str);
      }
    }
  }

  private iGCol AddColumn()
  {
    iGCol iGcol = this._grid.CurCell != null ? this._grid.Cols.Insert(this._grid.CurCell.ColIndex + 1) : this._grid.Cols.Add();
    iGcol.AllowMoving = this._optionAccessRights == OptionAccessRights.FullAccess;
    iGcol.MaxWidth = iGcol.MinWidth = iGcol.Width = 200;
    iGcol.AllowSizing = iGcol.AllowGrouping = false;
    iGcol.SortType = iGSortType.None;
    if (this._grid.Rows.Count == 0)
    {
      this._grid.Rows.AddRange(3);
      this._grid.Rows[2].Visible = false;
    }
    this._grid.Cells[0, iGcol.Index].Style = this._cellCodeTypeComboStyle;
    this._grid.Cells[1, iGcol.Index].Style = this._cellReadOnlyStyle;
    this._grid.Cells[2, iGcol.Index].Value = (object) ErrorState.None;
    this._grid.CurCell = this._grid.Cells[0, iGcol.Index];
    return iGcol;
  }

  private iGCellStyle GetCellStyle(CodePartType ct)
  {
    if (ct == CodePartType.FixedText)
      return this._cellStringStyle;
    iGDropDownList iGdropDownList = new iGDropDownList();
    if (ct == CodePartType.Undefined)
      return this._cellReadOnlyStyle;
    if (ct == CodePartType.ObjectAttribute)
    {
      foreach (string key in (IEnumerable<string>) this._sortAttrList.Keys)
        iGdropDownList.Items.Add((object) this._sortAttrList[key], key);
      iGdropDownList.Items.Add((object) 0, LocalizationHolder.rm.GetString("PdmConfigurator_26"));
    }
    else
    {
      foreach (string key in (IEnumerable<string>) this._sortOptionList.Keys)
        iGdropDownList.Items.Add((object) this._sortOptionList[key], key);
    }
    iGCellStyle cellStyle = new iGCellStyle(true);
    cellStyle.TextAlign = iGContentAlignment.TopLeft;
    cellStyle.ReadOnly = iGBool.False;
    cellStyle.Type = iGCellType.Combo;
    cellStyle.EmptyStringAs = iGEmptyStringAs.EmptyString;
    cellStyle.ImageList = this.ilLinked;
    iGdropDownList.AutoWidth = true;
    iGdropDownList.MaxVisibleRowCount = 15;
    cellStyle.DropDownControl = (IiGDropDownControl) iGdropDownList;
    return cellStyle;
  }

  private void CreateCodeStructure()
  {
    List<string> stringList1 = new List<string>();
    List<string> stringList2 = new List<string>();
    for (int colOrder = 0; colOrder < this._grid.Cols.Count; ++colOrder)
    {
      iGCol iGcol = this._grid.Cols.FromOrder(colOrder);
      if (iGcol.Cells[0].Value != null && iGcol.Cells[1].Value != null)
      {
        CodePartType type = (CodePartType) iGcol.Cells[0].Value;
        string format = this.StringFormat(type);
        if (type == CodePartType.FixedText)
        {
          stringList1.Add(string.Format(format, iGcol.Cells[1].Value));
          stringList2.Add(iGcol.Cells[1].Value.ToString());
        }
        else if ((ErrorState) iGcol.Cells[2].Value == ErrorState.None)
        {
          iGDropDownListItem auxValue = iGcol.Cells[1].AuxValue as iGDropDownListItem;
          object obj = auxValue.Value;
          if (type == CodePartType.ObjectAttribute)
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              IDBObject dbObject = sessionKeeper.Session.GetObject(this._objectID, false);
              if (dbObject != null)
              {
                int int32 = Convert.ToInt32(obj);
                IDBAttribute attributeById = dbObject.GetAttributeByID(int32);
                if (attributeById != null)
                {
                  if (!string.IsNullOrEmpty(attributeById.AsString))
                    stringList2.Add(attributeById.AsString);
                }
              }
            }
            stringList1.Add(string.Format(format, (object) auxValue.Text));
          }
          else
          {
            long int64 = Convert.ToInt64(obj);
            OptionHolder option = PdmConfiguratorCache.CacheFindOption(int64);
            if (option == null)
            {
              using (SessionKeeper sessionKeeper = new SessionKeeper())
              {
                PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, int64);
                option = PdmConfiguratorCache.CacheFindOption(int64);
              }
            }
            if (option != null)
            {
              if (type == CodePartType.OptionCode)
              {
                stringList2.Add(option.OptionCode);
              }
              else
              {
                List<string> stringList3 = this._objectOptionsHolder.VisibleOptionValues.Items[option.OptionGuid];
                string code = (stringList3.Count == 0 ? option.OptionValues[0] : option.OptionValues.FindValue(stringList3[0])).Code;
                stringList2.Add(code);
              }
            }
            stringList1.Add(string.Format(format, (object) auxValue.Text));
          }
        }
      }
    }
    this.lbStructure.Text = string.Join("", stringList1.ToArray());
    this.lbSample.Text = string.Join("", stringList2.ToArray());
  }

  private string StringFormat(CodePartType type)
  {
    if (type == CodePartType.FixedText)
      return "{0}";
    return type == CodePartType.OptionValueCode ? "[{0}]" : "<{0}>";
  }

  protected override void Dispose(bool disposing)
  {
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.tbCodeWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.cmsCodeWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged -= new EventHandler(this.BarManager_RendererChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConfigurationCodeEditor));
    this.ilLinked = new ImageList(this.components);
    this.panel1 = new Panel();
    this.lbSample = new TextBox();
    this.lbStructure = new TextBox();
    this.lbCaptionSample = new Label();
    this.lbCaptionStructure = new Label();
    this.tbCodeWork = new Intermech.Bars.ToolBar();
    this._addButtonItem = new ButtonItem();
    this._deleteButtonItem = new ButtonItem();
    this._clearButtonItem = new ButtonItem();
    this._moteToTopButtonItem = new ButtonItem();
    this._moveToLeftButtonItem = new ButtonItem();
    this._moveToRightButtonitem = new ButtonItem();
    this._moveToEndButtonItem = new ButtonItem();
    this._grid = new iGrid();
    this.cmsCodeWork = new MenuBar();
    this.contextMenuBarItem = new ContextMenuBarItem();
    this.tsmAddCodePart = new MenuButtonItem();
    this.tsmDeleteCodePart = new MenuButtonItem();
    this.tsmClear = new MenuButtonItem();
    this._refreshTimer = new Timer(this.components);
    this.errorPanel = new Panel();
    this.lbErrorState = new Label();
    this.pbError = new PictureBox();
    this.panelHint = new Label();
    this.panel1.SuspendLayout();
    ((ISupportInitialize) this._grid).BeginInit();
    this.errorPanel.SuspendLayout();
    ((ISupportInitialize) this.pbError).BeginInit();
    this.SuspendLayout();
    this.ilLinked.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilLinked.ImageStream");
    this.ilLinked.TransparentColor = Color.Transparent;
    this.ilLinked.Images.SetKeyName(0, "error.gif");
    this.ilLinked.Images.SetKeyName(1, "add.png");
    this.ilLinked.Images.SetKeyName(2, "delete.png");
    this.ilLinked.Images.SetKeyName(3, "document.png");
    this.ilLinked.Images.SetKeyName(4, "arrow_right_blue.ico");
    this.ilLinked.Images.SetKeyName(5, "arrow_all_left_blue.ico");
    this.ilLinked.Images.SetKeyName(6, "arrow_all_right_blue.ico");
    this.ilLinked.Images.SetKeyName(7, "arrow_left_blue.ico");
    this.panel1.BorderStyle = BorderStyle.Fixed3D;
    this.panel1.Controls.Add((Control) this.lbSample);
    this.panel1.Controls.Add((Control) this.lbStructure);
    this.panel1.Controls.Add((Control) this.lbCaptionSample);
    this.panel1.Controls.Add((Control) this.lbCaptionStructure);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.lbSample, "lbSample");
    this.lbSample.BorderStyle = BorderStyle.None;
    this.lbSample.Name = "lbSample";
    this.lbSample.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this.lbStructure, "lbStructure");
    this.lbStructure.BorderStyle = BorderStyle.None;
    this.lbStructure.Name = "lbStructure";
    this.lbStructure.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this.lbCaptionSample, "lbCaptionSample");
    this.lbCaptionSample.Name = "lbCaptionSample";
    componentResourceManager.ApplyResources((object) this.lbCaptionStructure, "lbCaptionStructure");
    this.lbCaptionStructure.Name = "lbCaptionStructure";
    this.tbCodeWork.FullMenus = true;
    this.tbCodeWork.Guid = new Guid("37056402-c6d1-47d4-be0f-e941c1a06e55");
    this.tbCodeWork.Hidden = false;
    this.tbCodeWork.ImageList = this.ilLinked;
    this.tbCodeWork.Items.AddRange(new ToolbarItemBase[7]
    {
      (ToolbarItemBase) this._addButtonItem,
      (ToolbarItemBase) this._deleteButtonItem,
      (ToolbarItemBase) this._clearButtonItem,
      (ToolbarItemBase) this._moteToTopButtonItem,
      (ToolbarItemBase) this._moveToLeftButtonItem,
      (ToolbarItemBase) this._moveToRightButtonitem,
      (ToolbarItemBase) this._moveToEndButtonItem
    });
    componentResourceManager.ApplyResources((object) this.tbCodeWork, "tbCodeWork");
    this.tbCodeWork.Name = "tbCodeWork";
    this.tbCodeWork.Tag = (object) "";
    this._addButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._addButtonItem, "_addButtonItem");
    this._addButtonItem.ImageIndex = 1;
    this._addButtonItem.Click += new EventHandler(this.AddButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._deleteButtonItem, "_deleteButtonItem");
    this._deleteButtonItem.Enabled = false;
    this._deleteButtonItem.ImageIndex = 2;
    this._deleteButtonItem.Click += new EventHandler(this.DeleteButtonItem_Click);
    this._clearButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._clearButtonItem, "_clearButtonItem");
    this._clearButtonItem.Enabled = false;
    this._clearButtonItem.ImageIndex = 3;
    this._clearButtonItem.Click += new EventHandler(this.ClearButtonItem_Click);
    this._moteToTopButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._moteToTopButtonItem, "_moteToTopButtonItem");
    this._moteToTopButtonItem.Enabled = false;
    this._moteToTopButtonItem.ImageIndex = 5;
    this._moteToTopButtonItem.Click += new EventHandler(this.MoveToTopButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._moveToLeftButtonItem, "_moveToLeftButtonItem");
    this._moveToLeftButtonItem.Enabled = false;
    this._moveToLeftButtonItem.ImageIndex = 7;
    this._moveToLeftButtonItem.Click += new EventHandler(this.MoveToLeftButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._moveToRightButtonitem, "_moveToRightButtonitem");
    this._moveToRightButtonitem.Enabled = false;
    this._moveToRightButtonitem.ImageIndex = 4;
    this._moveToRightButtonitem.Click += new EventHandler(this.MoveToRightButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._moveToEndButtonItem, "_moveToEndButtonItem");
    this._moveToEndButtonItem.Enabled = false;
    this._moveToEndButtonItem.ImageIndex = 6;
    this._moveToEndButtonItem.Click += new EventHandler(this.MoveToEndButtonItem_Click);
    this._grid.BackColorEvenRows = Color.WhiteSmoke;
    this._grid.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this._grid.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this._grid, "_grid");
    this._grid.Header.Height = (int) componentResourceManager.GetObject("_grid.Header.Height");
    this._grid.HighlightBackColorNoFocus = SystemColors.ControlLight;
    this._grid.HotTracking = false;
    this._grid.Name = "_grid";
    this._grid.ProcessTab = false;
    this._grid.SilentValidation = true;
    this._grid.ColHdrEndDrag += new iGColHdrEndDragEventHandler(this.Grid_ColHdrEndDrag);
    this._grid.CurCellChanged += new EventHandler(this.Grid_CurCellChanged);
    this._grid.BeforeCommitEdit += new iGBeforeCommitEditEventHandler(this.Grid_BeforeCommitEdit);
    this._grid.AfterCommitEdit += new iGAfterCommitEditEventHandler(this.Grid_AfterCommitEdit);
    componentResourceManager.ApplyResources((object) this.cmsCodeWork, "cmsCodeWork");
    this.cmsCodeWork.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.cmsCodeWork.Hidden = false;
    this.cmsCodeWork.ImageList = this.ilLinked;
    this.cmsCodeWork.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuBarItem
    });
    this.cmsCodeWork.Name = "cmsCodeWork";
    this.cmsCodeWork.OwnerForm = (Form) null;
    componentResourceManager.ApplyResources((object) this.contextMenuBarItem, "contextMenuBarItem");
    this.contextMenuBarItem.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.tsmAddCodePart,
      (ToolbarItemBase) this.tsmDeleteCodePart,
      (ToolbarItemBase) this.tsmClear
    });
    this.contextMenuBarItem.ShowText = true;
    componentResourceManager.ApplyResources((object) this.tsmAddCodePart, "tsmAddCodePart");
    this.tsmAddCodePart.ImageIndex = 1;
    this.tsmAddCodePart.ShowText = true;
    this.tsmAddCodePart.Click += new EventHandler(this.AddButtonItem_Click);
    componentResourceManager.ApplyResources((object) this.tsmDeleteCodePart, "tsmDeleteCodePart");
    this.tsmDeleteCodePart.ImageIndex = 2;
    this.tsmDeleteCodePart.ShowText = true;
    this.tsmDeleteCodePart.Click += new EventHandler(this.DeleteButtonItem_Click);
    this.tsmClear.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.tsmClear, "tsmClear");
    this.tsmClear.ImageIndex = 3;
    this.tsmClear.ShowText = true;
    this.tsmClear.Click += new EventHandler(this.ClearButtonItem_Click);
    this._refreshTimer.Tick += new EventHandler(this.RefreshTimer_Tick);
    this.errorPanel.Controls.Add((Control) this.lbErrorState);
    this.errorPanel.Controls.Add((Control) this.pbError);
    componentResourceManager.ApplyResources((object) this.errorPanel, "errorPanel");
    this.errorPanel.Name = "errorPanel";
    componentResourceManager.ApplyResources((object) this.lbErrorState, "lbErrorState");
    this.lbErrorState.Name = "lbErrorState";
    componentResourceManager.ApplyResources((object) this.pbError, "pbError");
    this.pbError.Name = "pbError";
    this.pbError.TabStop = false;
    componentResourceManager.ApplyResources((object) this.panelHint, "panelHint");
    this.panelHint.Name = "panelHint";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._grid);
    this.Controls.Add((Control) this.tbCodeWork);
    this.Controls.Add((Control) this.errorPanel);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.panelHint);
    this.Controls.Add((Control) this.cmsCodeWork);
    this.Name = nameof (ConfigurationCodeEditor);
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    ((ISupportInitialize) this._grid).EndInit();
    this.errorPanel.ResumeLayout(false);
    ((ISupportInitialize) this.pbError).EndInit();
    this.ResumeLayout(false);
  }

  public delegate void ObjectOptionsChangedEventHandler(object sender, EventArgs e);
}
