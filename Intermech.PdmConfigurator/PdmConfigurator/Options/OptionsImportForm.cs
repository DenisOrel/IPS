// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.Options.OptionsImportForm
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
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.PdmConfigurator.Options;

public sealed class OptionsImportForm : Form
{
  private const string IMPORTED = "IMPORTED";
  private const string OPTION = "OPTION";
  private const string OPTION_CAPTION = "OPTION_CAPTION";
  private const string RADIO = "RADIO";
  private const string OBJECT_CAPTION = "OBJECT_CAPTION";
  private const string OBJECT_ID = "OBJECT_ID";
  private const string OBJECT_TYPE = "OBJECT_TYPE";
  private const string INCOMP = "INCOMP";
  private const string LINKED = "LINKED";
  private const string OBJECT_HOLDER = "OBJECT_HOLDER";
  private const string OPTIONS_LIST = "OPTIONS_LIST";
  private const string LINKED_IDS_LIST = "LINKED_IDS_LIST";
  private const string INCOMP_IDS_LIST = "INCOMP_IDS_LIST";
  private const string DESCRIPTION = "DESCRIPTION";
  private const string TYPE_ICON = "TYPE_ICON";
  private const string OPTION_TYPE_ICON = "OPTION_TYPE_ICON";
  private iGCellStyle cellString = new iGCellStyle(true);
  private iGCellStyle cellCheckBoxEdit = new iGCellStyle(true);
  private iGCellStyle boldStyle = new iGCellStyle(true);
  private List<PdmAnalyzedOptionObject> analyzedObjects;
  private IList<long> excludedOptions;
  private static Dictionary<string, int> _colWidths = new Dictionary<string, int>();
  private List<ObjectOptionsHolder> objectsOptions = new List<ObjectOptionsHolder>();
  private Dictionary<long, List<long>> dict = new Dictionary<long, List<long>>();
  private Dictionary<long, Dictionary<long, OptionsImportForm.DependentType>> recoveryDictionary = new Dictionary<long, Dictionary<long, OptionsImportForm.DependentType>>();
  private ImportOptions importOptions = new ImportOptions();
  private SortedDictionary<OptionObjectDescription, List<OptionHolder>> categories = new SortedDictionary<OptionObjectDescription, List<OptionHolder>>();
  private ICategoryTypeIconService objtypesIcons;
  private INamedImageList images;
  private IContainer components;
  private Intermech.Bars.ToolBar toolBarTop;
  private iGrid igOptions;
  private iGrid igDependent;
  private iGrid igObjects;
  private Panel panel3;
  private Button btnApply;
  private Button btnCancel;
  private ImageList ilGrids;
  private SplitContainer splitContainer1;
  private SplitContainer splitContainer2;
  private ImageList ilGridImage;
  private Intermech.Bars.ToolBar tbOptionWork;
  private ButtonItem btnCard;
  private ButtonItem btnObjectCard;
  private ButtonItem btnCheck;
  private ButtonItem btnUncheck;
  private MenuBar menuBarOptions;
  private ContextMenuBarItem contextMenuBarOptions;
  private MenuButtonItem mnpCheck;
  private MenuButtonItem mnpUncheck;
  private MenuButtonItem mnpCard;
  private MenuBar menuBarObjects;
  private ContextMenuBarItem contextMenuBarObjects;
  private MenuButtonItem mnpObjectCard;

  public OptionsImportForm() => this.InitializeComponent();

  public OptionsImportForm(
    List<PdmAnalyzedOptionObject> analyzedObjects,
    IList<long> excludedOptions)
  {
    this.InitializeComponent();
    this.InitServices();
    if (ServicesManager.GetService(typeof (IGuidMapper)) is IGuidMapper)
      this.Init(analyzedObjects, excludedOptions);
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1831);
  }

  public static ImportOptions Execute(
    List<PdmAnalyzedOptionObject> analyzedObjects,
    IList<long> excludedOptions)
  {
    using (OptionsImportForm optionsImportForm = new OptionsImportForm(analyzedObjects, excludedOptions))
      return optionsImportForm.ShowDialog() != DialogResult.OK ? (ImportOptions) null : optionsImportForm.importOptions;
  }

  private void OnFormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void ToolbarRendererChanged(object sender, EventArgs e)
  {
    IToolBarRenderer renderer = (sender as BarManager).Renderer;
    this.toolBarTop.Renderer = renderer;
    this.tbOptionWork.Renderer = renderer;
    this.menuBarOptions.Renderer = renderer;
    this.menuBarObjects.Renderer = renderer;
  }

  private void igOptions_SelectionChanged(object sender, EventArgs e)
  {
    this.FillObjectsGrid();
    this.UpdateControls();
  }

  private void igObjects_SelectionChanged(object sender, EventArgs e)
  {
    iGRow selectedObject = this.GetSelectedObject();
    if (selectedObject == null)
      return;
    this.FillDependentGrid(selectedObject);
  }

  private void igOptions_AfterCommitEdit(object sender, iGAfterCommitEditEventArgs e)
  {
    iGRow row = this.igOptions.Rows[e.RowIndex];
    if (e.ColIndex == this.igOptions.Cols["IMPORTED"].Index)
    {
      int num = Convert.ToBoolean(row.Cells["IMPORTED"].Value) ? 1 : 0;
      OptionHolder optionHolder = row.Cells["OPTION"].Value as OptionHolder;
      if (num != 0)
        this.AddOption(row);
      else
        this.importOptions.RemoveImportOption(optionHolder.OptionObjectID);
    }
    this.UpdateControls();
  }

  private void igObjects_BeforeCommitEdit(object sender, iGBeforeCommitEditEventArgs e)
  {
    if (!Convert.ToBoolean(e.NewValue))
      return;
    iGRow curRow = this.igOptions.CurRow;
    long option1;
    if (e.ColIndex == this.igObjects.Cols["LINKED"].Index)
      option1 = this.IsExcludeOptionExists(curRow.Cells["LINKED_IDS_LIST"].Value.ToString().Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.RemoveEmptyEntries));
    else
      option1 = this.IsExcludeOptionExists(curRow.Cells["INCOMP_IDS_LIST"].Value.ToString().Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.RemoveEmptyEntries));
    if (option1 == 0L)
      return;
    OptionHolder option2 = PdmConfiguratorCache.CacheFindOption(option1);
    string str = option2 != null ? string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_88"), (object) option2.OptionCaption) : string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_89"), (object) option1);
    int num = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_64"), string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_90"), (object) str), MessageBoxButtons.OK, IMMessageBoxImage.Information);
    e.Result = iGEditResult.Cancel;
  }

  private void igObjects_AfterCommitEdit(object sender, iGAfterCommitEditEventArgs e)
  {
    iGRow row1 = this.igObjects.Rows[e.RowIndex];
    if (row1.Index == this.GetCheckedObject().Index)
    {
      iGRow row2 = this.igOptions.SelectedCells[0].Row;
      if (!Convert.ToBoolean(row2.Cells["IMPORTED"].Value))
      {
        this.AddOption(row2);
        row2.Cells["IMPORTED"].Value = (object) true;
      }
      else
      {
        bool boolean = Convert.ToBoolean(row1.Cells[e.ColIndex].Value);
        OptionsImportForm.DependentType type = this.igObjects.Cols["LINKED"].Index == e.ColIndex ? OptionsImportForm.DependentType.Linked : OptionsImportForm.DependentType.Incomp;
        ObjectOptionsHolder objectHolder = row1.Cells["OBJECT_HOLDER"].Value as ObjectOptionsHolder;
        this.ChangeDependent(boolean, type, row2, objectHolder);
      }
    }
    this.UpdateControls();
  }

  private void igObjects_CellClick(object sender, iGCellClickEventArgs e)
  {
    if (e.ColIndex != this.igObjects.Cols["RADIO"].Index)
      return;
    this.CheckObject(this.igObjects.Rows[e.RowIndex]);
  }

  private void igObjects_CellDoubleClick(object sender, iGCellDoubleClickEventArgs e)
  {
    if (e.ColIndex == this.igObjects.Cols["OBJECT_CAPTION"].Index)
    {
      this.CheckObject(this.igObjects.Rows[e.RowIndex]);
      iGRow row = this.igOptions.SelectedCells[0].Row;
      this.AddOption(row);
      row.Cells["IMPORTED"].Value = (object) true;
    }
    this.UpdateControls();
  }

  private void btnApply_Click(object sender, EventArgs e)
  {
    try
    {
      this.importOptions.CheckErrorExists();
    }
    catch (Exception ex)
    {
      if (ex is PdmConfiguratorExeption || ex.InnerException is PdmConfiguratorExeption)
      {
        int num = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), ex.Message, MessageBoxButtons.OK, IMMessageBoxImage.Information);
        return;
      }
      throw;
    }
    this.DialogResult = DialogResult.OK;
  }

  private void igOptions_Resize(object sender, EventArgs e) => this.CorrectOptionsColsWidth();

  private void igObjects_Resize(object sender, EventArgs e) => this.CorrectObjectsColsWidth();

  private void igDependent_Resize(object sender, EventArgs e) => this.CorrectDependentColsWidth();

  private void igOptions_ColWidthEndChange(object sender, iGColWidthEventArgs e)
  {
    OptionsImportForm._colWidths[this.igOptions.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectOptionsColsWidth();
  }

  private void igObjects_ColWidthEndChange(object sender, iGColWidthEventArgs e)
  {
    OptionsImportForm._colWidths[this.igObjects.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectObjectsColsWidth();
  }

  private void igDependent_ColWidthEndChange(object sender, iGColWidthEventArgs e)
  {
    OptionsImportForm._colWidths[this.igDependent.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectDependentColsWidth();
  }

  private void igOptions_ColWidthChanging(object sender, iGColWidthEventArgs e)
  {
    OptionsImportForm._colWidths[this.igOptions.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectDependentColsWidth();
  }

  private void igObjects_ColWidthChanging(object sender, iGColWidthEventArgs e)
  {
    OptionsImportForm._colWidths[this.igObjects.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectObjectsColsWidth();
  }

  private void igDependent_ColWidthChanging(object sender, iGColWidthEventArgs e)
  {
    OptionsImportForm._colWidths[this.igDependent.Cols[e.ColIndex].Key] = e.Width;
    this.CorrectDependentColsWidth();
  }

  private void btnCard_Click(object sender, EventArgs e)
  {
    iGRow curRow = this.igOptions.CurRow;
    object obj = curRow.Cells["OPTION"].Value;
    long ObjectID = curRow.Level != 0 ? (obj as OptionHolder).OptionObjectID : (obj as OptionObjectDescription).F_OBJECT_ID;
    if (ObjectID == 0L)
      return;
    int num = (int) PropertiesWindow.Execute(string.Empty, string.Empty, ObjectID, false);
  }

  private void btnObjectCard_Click(object sender, EventArgs e)
  {
    iGRow selectedObject = this.GetSelectedObject();
    if (selectedObject == null)
      return;
    long int64 = Convert.ToInt64(selectedObject.Cells["OBJECT_ID"].Value);
    int num = (int) PropertiesWindow.Execute(string.Empty, string.Empty, int64, false);
  }

  private void btnCheck_Click(object sender, EventArgs e)
  {
    foreach (iGRow row in (IEnumerable) this.igOptions.Rows)
    {
      if (row.Level == 1 && !Convert.ToBoolean(row.Cells["IMPORTED"].Value))
      {
        row.Cells["IMPORTED"].Value = (object) true;
        this.AddOption(row);
      }
    }
    this.UpdateControls();
  }

  private void btnUncheck_Click(object sender, EventArgs e)
  {
    foreach (iGRow row in (IEnumerable) this.igOptions.Rows)
    {
      if (row.Level == 1 && Convert.ToBoolean(row.Cells["IMPORTED"].Value))
      {
        row.Cells["IMPORTED"].Value = (object) false;
        this.importOptions.RemoveImportOption((row.Cells["OPTION"].Value as OptionHolder).OptionObjectID);
      }
    }
    this.UpdateControls();
  }

  private void DoCellMouseDown(object sender, iGCellMouseDownEventArgs e)
  {
    if (!(sender is iGrid iGrid) || e.Button != MouseButtons.Right)
      return;
    iGRow row = iGrid.Rows[e.RowIndex];
    iGrid.PerformAction(iGActions.DeselectAll);
    this.iGridSelectRowCells(row, true);
    iGrid.CurRow = row;
  }

  private void InitServices()
  {
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      service.RendererChanged += new EventHandler(this.ToolbarRendererChanged);
      this.ToolbarRendererChanged((object) service, EventArgs.Empty);
    }
    this.objtypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this.images = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
  }

  private void Init(List<PdmAnalyzedOptionObject> analyzedObjects, IList<long> excludedOptions)
  {
    Rectangle workingArea = Screen.GetWorkingArea((Control) this);
    this.Size = new Size(workingArea.Width / 100 * 80 /*0x50*/, workingArea.Height / 100 * 80 /*0x50*/);
    this.Location = new Point((workingArea.Width - this.Size.Width) / 2, (workingArea.Height - this.Size.Height) / 2);
    this.btnCard.Image = this.btnObjectCard.Image = this.images.ImageList.Images[this.images.ImageIndex("imgCard")];
    this.mnpCard.Image = this.mnpObjectCard.Image = this.btnCard.Image;
    this.analyzedObjects = analyzedObjects;
    this.excludedOptions = excludedOptions;
    this.PrepareGridsColumns();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService)
      {
        this.objectsOptions = customService.LoadObjectsOptions(sessionKeeper.Session.SessionGUID, analyzedObjects);
        this.dict = ObjectOptionsHolder.ExtractOptionsInObjects(this.objectsOptions, excludedOptions);
      }
    }
    this.FillOptionsGrid();
    FormStorage.LoadLayout((Control) this);
    this.UpdateControls();
  }

  private void UpdateControls()
  {
    this.btnApply.Enabled = this.btnUncheck.Enabled = this.mnpUncheck.Enabled = this.IsImportOptionExists();
    this.btnObjectCard.Enabled = this.mnpObjectCard.Enabled = this.GetSelectedObject() != null;
    this.btnCheck.Enabled = this.mnpCheck.Enabled = this.IsNotImportOptionExists();
  }

  private void FillOptionsGrid()
  {
    this.CollectCategories();
    foreach (OptionObjectDescription key in this.categories.Keys)
    {
      iGRow iGrow1 = this.igOptions.Rows.Add();
      iGrow1.Level = 0;
      iGrow1.Cells["OPTION_CAPTION"].Value = (object) key.CAPTION;
      iGrow1.Cells["IMPORTED"].Style = this.cellString;
      iGrow1.Cells["OPTION_TYPE_ICON"].ImageIndex = this.objtypesIcons.IndexOf(4, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionsGroupID);
      iGrow1.Cells["OPTION"].Value = (object) key;
      iGrow1.Cells["OPTION_CAPTION"].Style = this.boldStyle;
      iGrow1.Expanded = true;
      iGrow1.TreeButton = iGTreeButtonState.Visible;
      List<OptionHolder> category = this.categories[key];
      category.Sort();
      foreach (OptionHolder optionHolder in category)
      {
        iGRow iGrow2 = this.igOptions.Rows.Add();
        iGrow2.Level = 1;
        iGrow2.Cells["IMPORTED"].Value = (object) 0;
        iGrow2.Cells["OPTION_CAPTION"].Value = (object) optionHolder.OptionCaption;
        iGrow2.Cells["OPTION"].Value = (object) optionHolder;
        iGrow2.Cells["LINKED_IDS_LIST"].Value = (object) string.Empty;
        iGrow2.Cells["INCOMP_IDS_LIST"].Value = (object) string.Empty;
        iGrow2.Cells["OPTION_TYPE_ICON"].ImageIndex = this.objtypesIcons.IndexOf(4, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID);
      }
    }
    if (this.igOptions.Rows.Count > 0)
      this.igOptions.SetCurRow(0);
    this.CorrectOptionsColsWidth();
  }

  private void FillObjectsGrid()
  {
    this.igObjects.Rows.Clear();
    OptionHolder selectedOptionHolder = this.GetSelectedOptionHolder();
    if (selectedOptionHolder == null || !this.dict.ContainsKey(selectedOptionHolder.OptionObjectID))
    {
      this.igDependent.Rows.Clear();
    }
    else
    {
      List<long> longList = this.dict[selectedOptionHolder.OptionObjectID];
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        bool flag = false;
        foreach (long objectID in longList)
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(objectID, false);
          if (dbObject != null)
          {
            iGRow iGrow = this.igObjects.Rows.Add();
            iGrow.Cells["OBJECT_CAPTION"].Value = (object) dbObject.NameInMessages;
            iGrow.Cells["OBJECT_ID"].Value = (object) dbObject.ObjectID;
            iGrow.Cells["OBJECT_TYPE"].Value = (object) dbObject.ObjectType;
            iGrow.Cells["OBJECT_HOLDER"].Value = (object) this.FindObjectOptionsHolder(objectID);
            iGrow.Cells["TYPE_ICON"].ImageIndex = this.objtypesIcons.IndexOf(4, dbObject.ObjectType);
            if (this.recoveryDictionary.ContainsKey(dbObject.ObjectID))
            {
              Dictionary<long, OptionsImportForm.DependentType> recovery = this.recoveryDictionary[dbObject.ObjectID];
              if (recovery.ContainsKey(selectedOptionHolder.OptionObjectID))
              {
                OptionsImportForm.DependentType dependentType = recovery[selectedOptionHolder.OptionObjectID];
                iGrow.Cells["RADIO"].ImageIndex = 1;
                iGrow.Cells["INCOMP"].Style = iGrow.Cells["LINKED"].Style = this.cellCheckBoxEdit;
                iGrow.Cells["INCOMP"].Value = (object) ((dependentType & OptionsImportForm.DependentType.Incomp) == OptionsImportForm.DependentType.Incomp);
                iGrow.Cells["LINKED"].Value = (object) ((dependentType & OptionsImportForm.DependentType.Linked) == OptionsImportForm.DependentType.Linked);
                this.igObjects.SetCurRow(iGrow.Index);
                flag = true;
                continue;
              }
            }
            iGrow.Cells["INCOMP"].Value = (object) string.Empty;
            iGrow.Cells["LINKED"].Value = (object) string.Empty;
            iGrow.Cells["RADIO"].ImageIndex = 0;
          }
        }
        if (!flag)
        {
          this.igObjects.Rows[0].Cells["INCOMP"].Style = this.igObjects.Rows[0].Cells["LINKED"].Style = this.cellCheckBoxEdit;
          this.igObjects.Rows[0].Cells["RADIO"].ImageIndex = 1;
          this.igObjects.Rows[0].Cells["INCOMP"].Value = this.igObjects.Rows[0].Cells["LINKED"].Value = (object) false;
          this.igObjects.SetCurRow(0);
        }
      }
      this.CorrectObjectsColsWidth();
    }
  }

  private void FillDependentGrid(iGRow objRow)
  {
    this.igDependent.Rows.Clear();
    OptionHolder selectedOptionHolder = this.GetSelectedOptionHolder();
    if (selectedOptionHolder == null || !this.dict.ContainsKey(selectedOptionHolder.OptionObjectID))
      return;
    Convert.ToInt64(objRow.Cells["OBJECT_ID"].Value);
    ObjectOptionsHolder objectHolder = objRow.Cells["OBJECT_HOLDER"].Value as ObjectOptionsHolder;
    List<string> stringList1 = new List<string>();
    List<string> stringList2 = new List<string>();
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    this.igOptions.SelectedCells[0].Row.Cells["LINKED_IDS_LIST"].Value = (object) string.Empty;
    this.igOptions.SelectedCells[0].Row.Cells["INCOMP_IDS_LIST"].Value = (object) string.Empty;
    Dictionary<string, string> linkedOptions = this.FindLinkedOptions(objectHolder, selectedOptionHolder.OptionGuid);
    if (linkedOptions.Count > 0)
    {
      iGRow iGrow = this.igDependent.Rows.Add();
      iGrow.Cells["OPTIONS_LIST"].Value = (object) string.Join(Environment.NewLine, new List<string>((IEnumerable<string>) linkedOptions.Values).ToArray());
      iGrow.Cells["DESCRIPTION"].Value = (object) LocalizationHolder.rm.GetString("PdmConfigurator_86");
      iGrow.Cells["DESCRIPTION"].ImageIndex = 3;
      this.igOptions.SelectedCells[0].Row.Cells["LINKED_IDS_LIST"].Value = (object) string.Join(Environment.NewLine, new List<string>((IEnumerable<string>) linkedOptions.Keys).ToArray());
    }
    Dictionary<string, string> incompOptions = this.FindIncompOptions(objectHolder, selectedOptionHolder.OptionGuid);
    if (incompOptions.Count > 0)
    {
      iGRow iGrow = this.igDependent.Rows.Add();
      iGrow.Cells["OPTIONS_LIST"].Value = (object) string.Join(Environment.NewLine, new List<string>((IEnumerable<string>) incompOptions.Values).ToArray());
      iGrow.Cells["DESCRIPTION"].Value = (object) LocalizationHolder.rm.GetString("PdmConfigurator_87");
      iGrow.Cells["DESCRIPTION"].ImageIndex = 2;
      this.igOptions.SelectedCells[0].Row.Cells["INCOMP_IDS_LIST"].Value = (object) string.Join(Environment.NewLine, new List<string>((IEnumerable<string>) incompOptions.Keys).ToArray());
    }
    this.CorrectDependentColsWidth();
  }

  private Dictionary<string, string> FindIncompOptions(
    ObjectOptionsHolder objectHolder,
    Guid optionGuid)
  {
    Dictionary<string, string> incompOptions = new Dictionary<string, string>();
    List<IPdmCriterion> criterionEx = objectHolder.Incompatibilities.FindCriterionEx(optionGuid);
    if (criterionEx != null && criterionEx.Count > 0)
    {
      foreach (IPdmCriterion pdmCriterion in criterionEx)
      {
        if (pdmCriterion is ObjectIncompatibilityCriterion incompatibilityCriterion)
        {
          Guid optionConflict = incompatibilityCriterion.OptionConflict;
          OptionHolder option = PdmConfiguratorCache.CacheFindOption(optionConflict);
          if (option == null)
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, optionConflict);
              option = PdmConfiguratorCache.CacheFindOption(optionConflict);
              if (option == null)
                continue;
            }
          }
          if (!incompOptions.ContainsKey(option.OptionObjectID.ToString()))
            incompOptions.Add(option.OptionObjectID.ToString(), option.OptionCaption);
        }
      }
    }
    return incompOptions;
  }

  private Dictionary<string, string> FindLinkedOptions(
    ObjectOptionsHolder objectHolder,
    Guid optionGuid)
  {
    Dictionary<string, string> linkedOptions1 = new Dictionary<string, string>();
    List<Guid> linkedOptions2 = objectHolder.Incompatibilities.LinkedOptions.FindLinkedOptions(optionGuid);
    if (linkedOptions2.Count > 0)
    {
      foreach (Guid guid in linkedOptions2)
      {
        OptionHolder option = PdmConfiguratorCache.CacheFindOption(guid);
        if (option == null)
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, guid);
            option = PdmConfiguratorCache.CacheFindOption(guid);
            if (option == null)
              continue;
          }
        }
        if (!linkedOptions1.ContainsKey(option.OptionObjectID.ToString()))
          linkedOptions1.Add(option.OptionObjectID.ToString(), option.OptionCaption);
      }
    }
    return linkedOptions1;
  }

  private void CheckObject(iGRow objRow)
  {
    if (Convert.ToBoolean(objRow.Cells["RADIO"].ImageIndex))
      return;
    long int64_1 = Convert.ToInt64(objRow.Cells["OBJECT_ID"].Value);
    ObjectOptionsHolder objectOptionsHolder = objRow.Cells["OBJECT_HOLDER"].Value as ObjectOptionsHolder;
    iGRow row = this.igOptions.SelectedCells[0].Row;
    OptionHolder optionHolder = row.Cells["OPTION"].Value as OptionHolder;
    iGRow checkedObject = this.GetCheckedObject();
    if (checkedObject != null)
    {
      long int64_2 = Convert.ToInt64(checkedObject.Cells["OBJECT_ID"].Value);
      checkedObject.Cells["RADIO"].ImageIndex = 0;
      checkedObject.Cells["LINKED"].Value = checkedObject.Cells["INCOMP"].Value = (object) string.Empty;
      checkedObject.Cells["LINKED"].Style = checkedObject.Cells["INCOMP"].Style = this.cellString;
      this.RemoveFromRecovery(int64_2, optionHolder.OptionObjectID);
    }
    objRow.Cells["RADIO"].ImageIndex = 1;
    objRow.Cells["LINKED"].Style = objRow.Cells["INCOMP"].Style = this.cellCheckBoxEdit;
    objRow.Cells["LINKED"].Value = objRow.Cells["INCOMP"].Value = (object) false;
    List<string> visibleValues = objectOptionsHolder.VisibleOptionValues.Items[optionHolder.OptionGuid];
    this.importOptions.AddImportOption(optionHolder.OptionObjectID, objectOptionsHolder.ObjectID, visibleValues);
    row.Cells["IMPORTED"].Value = (object) true;
    this.AddToRecovery(int64_1, optionHolder.OptionObjectID);
    this.UpdateControls();
  }

  private void AddToRecovery(long objectID, long optionID)
  {
    if (this.recoveryDictionary.ContainsKey(objectID))
    {
      Dictionary<long, OptionsImportForm.DependentType> recovery = this.recoveryDictionary[objectID];
      if (recovery.ContainsKey(optionID))
        recovery[optionID] = OptionsImportForm.DependentType.None;
      else
        recovery.Add(optionID, OptionsImportForm.DependentType.None);
    }
    else
      this.recoveryDictionary.Add(objectID, new Dictionary<long, OptionsImportForm.DependentType>()
      {
        {
          optionID,
          OptionsImportForm.DependentType.None
        }
      });
  }

  private void RemoveFromRecovery(long objectID, long optionID)
  {
    if (!this.recoveryDictionary.ContainsKey(objectID))
      return;
    Dictionary<long, OptionsImportForm.DependentType> recovery = this.recoveryDictionary[objectID];
    if (!recovery.ContainsKey(optionID))
      return;
    recovery.Remove(optionID);
  }

  private void ChangeDependence(
    long objectID,
    long optionID,
    OptionsImportForm.DependentType type,
    bool add)
  {
    if (this.recoveryDictionary.ContainsKey(objectID))
    {
      Dictionary<long, OptionsImportForm.DependentType> recovery = this.recoveryDictionary[objectID];
      if (!recovery.ContainsKey(optionID))
        return;
      if (add)
        recovery[optionID] |= type;
      else
        recovery[optionID] &= ~type;
    }
    else
    {
      if (!add)
        return;
      this.recoveryDictionary.Add(objectID, new Dictionary<long, OptionsImportForm.DependentType>()
      {
        {
          optionID,
          type
        }
      });
    }
  }

  private void RemoveFromRecovery(long optionID)
  {
    foreach (Dictionary<long, OptionsImportForm.DependentType> dictionary in this.recoveryDictionary.Values)
    {
      foreach (long key in new Dictionary<long, OptionsImportForm.DependentType>((IDictionary<long, OptionsImportForm.DependentType>) dictionary).Keys)
      {
        if (key == optionID)
        {
          dictionary.Remove(key);
          return;
        }
      }
    }
  }

  private long StateForObjectID(long optionID)
  {
    foreach (long key1 in this.recoveryDictionary.Keys)
    {
      foreach (long key2 in this.recoveryDictionary[key1].Keys)
      {
        if (key2 == optionID)
          return key1;
      }
    }
    return 0;
  }

  private void PrepareGridsColumns()
  {
    iGCellStyle iGcellStyle1 = new iGCellStyle(true);
    iGcellStyle1.ImageAlign = iGContentAlignment.TopLeft;
    iGcellStyle1.TextAlign = iGContentAlignment.TopLeft;
    iGcellStyle1.ReadOnly = iGBool.True;
    iGcellStyle1.TextFormatFlags = iGStringFormatFlags.WordWrap;
    iGcellStyle1.ImageList = this.ilGrids;
    this.boldStyle = new iGCellStyle(true);
    this.boldStyle.TextAlign = iGContentAlignment.TopLeft;
    this.boldStyle.ReadOnly = iGBool.True;
    this.boldStyle.ImageList = this.ilGrids;
    this.boldStyle.Font = new Font(this.igOptions.Font, FontStyle.Bold);
    this.cellCheckBoxEdit.ImageAlign = iGContentAlignment.TopCenter;
    this.cellCheckBoxEdit.TextAlign = iGContentAlignment.TopCenter;
    this.cellCheckBoxEdit.Type = iGCellType.Check;
    this.cellCheckBoxEdit.ValueType = typeof (bool);
    this.cellCheckBoxEdit.SingleClickEdit = iGBool.True;
    this.cellCheckBoxEdit.ReadOnly = iGBool.False;
    this.cellCheckBoxEdit.EmptyStringAs = iGEmptyStringAs.EmptyString;
    iGCellStyle iGcellStyle2 = new iGCellStyle(true);
    iGcellStyle2.ImageAlign = iGContentAlignment.TopCenter;
    iGcellStyle2.ValueType = typeof (string);
    iGcellStyle2.SingleClickEdit = iGBool.True;
    iGcellStyle2.ReadOnly = iGBool.True;
    iGcellStyle2.EmptyStringAs = iGEmptyStringAs.EmptyString;
    iGcellStyle2.ImageList = this.ilGridImage;
    this.cellString.TextAlign = iGContentAlignment.TopLeft;
    this.cellString.ValueType = typeof (string);
    this.cellString.SingleClickEdit = iGBool.False;
    this.cellString.ReadOnly = iGBool.True;
    this.cellString.EmptyStringAs = iGEmptyStringAs.EmptyString;
    this.cellString.TextFormatFlags = iGStringFormatFlags.WordWrap;
    this.cellString.Type = iGCellType.Text;
    iGCellStyle iGcellStyle3 = new iGCellStyle(true);
    iGcellStyle3.ImageAlign = iGContentAlignment.MiddleLeft;
    iGcellStyle3.ReadOnly = iGBool.True;
    iGcellStyle3.ImageList = this.objtypesIcons.ImageList;
    if (OptionsImportForm._colWidths.Count == 0)
      OptionsImportForm._colWidths = new Dictionary<string, int>()
      {
        {
          "OPTION_TYPE_ICON",
          48 /*0x30*/
        },
        {
          "IMPORTED",
          18
        },
        {
          "OPTION",
          0
        },
        {
          "LINKED_IDS_LIST",
          0
        },
        {
          "INCOMP_IDS_LIST",
          0
        },
        {
          "OPTION_CAPTION",
          200
        },
        {
          "RADIO",
          36
        },
        {
          "TYPE_ICON",
          32 /*0x20*/
        },
        {
          "OBJECT_CAPTION",
          200
        },
        {
          "OBJECT_ID",
          0
        },
        {
          "OBJECT_TYPE",
          0
        },
        {
          "OBJECT_HOLDER",
          0
        },
        {
          "INCOMP",
          180
        },
        {
          "LINKED",
          180
        },
        {
          "OPTIONS_LIST",
          200
        },
        {
          "DESCRIPTION",
          280
        }
      };
    iGCol col1 = this.igOptions.Cols["OPTION_TYPE_ICON"];
    iGCol iGcol1 = this.igOptions.Cols["OPTION_TYPE_ICON"] ?? this.igOptions.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["OPTION_TYPE_ICON"], true, true, 48 /*0x30*/, 48 /*0x30*/, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "OPTION_TYPE_ICON", -1, (object) null, (object) null, -1));
    iGcol1.Width = OptionsImportForm._colWidths["OPTION_TYPE_ICON"];
    iGcol1.CellStyle = iGcellStyle3;
    iGCol col2 = this.igOptions.Cols["IMPORTED"];
    iGCol iGcol2 = this.igOptions.Cols["IMPORTED"] ?? this.igOptions.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["IMPORTED"], true, true, 18, 18, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "IMPORTED", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol2.CellStyle = this.cellCheckBoxEdit;
    iGcol2.Width = OptionsImportForm._colWidths["IMPORTED"];
    iGCol col3 = this.igOptions.Cols["OPTION_CAPTION"];
    iGCol iGcol3 = this.igOptions.Cols["OPTION_CAPTION"] ?? this.igOptions.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["OPTION_CAPTION"], true, true, 200, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_55"), "OPTION_CAPTION", -1, (object) null, (object) null, -1));
    iGcol3.Width = OptionsImportForm._colWidths["OPTION_CAPTION"];
    iGcol3.CellStyle = this.cellString;
    iGCol col4 = this.igOptions.Cols["OPTION"];
    (this.igOptions.Cols["OPTION"] ?? this.igOptions.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["OPTION"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "OPTION", -1, (object) null, (object) null, -1))).Width = OptionsImportForm._colWidths["OPTION"];
    iGCol col5 = this.igOptions.Cols["LINKED_IDS_LIST"];
    iGCol iGcol4 = this.igOptions.Cols["LINKED_IDS_LIST"] ?? this.igOptions.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["LINKED_IDS_LIST"], false, false, 0, 0, true, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "LINKED_IDS_LIST", -1, (object) null, (object) null, -1));
    iGcol4.Width = OptionsImportForm._colWidths["LINKED_IDS_LIST"];
    iGcol4.CellStyle = this.cellString;
    iGCol col6 = this.igOptions.Cols["INCOMP_IDS_LIST"];
    iGCol iGcol5 = this.igOptions.Cols["INCOMP_IDS_LIST"] ?? this.igOptions.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["INCOMP_IDS_LIST"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "INCOMP_IDS_LIST", -1, (object) null, (object) null, -1));
    iGcol5.Width = OptionsImportForm._colWidths["INCOMP_IDS_LIST"];
    iGcol5.CellStyle = this.cellString;
    iGCol col7 = this.igOptions.Cols["RADIO"];
    iGCol iGcol6 = this.igObjects.Cols["RADIO"] ?? this.igObjects.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["RADIO"], true, true, 36, 36, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "RADIO", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol6.CellStyle = iGcellStyle2;
    iGcol6.Width = OptionsImportForm._colWidths["RADIO"];
    iGCol col8 = this.igObjects.Cols["TYPE_ICON"];
    iGCol iGcol7 = this.igObjects.Cols["TYPE_ICON"] ?? this.igObjects.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["TYPE_ICON"], true, true, 32 /*0x20*/, 32 /*0x20*/, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "TYPE_ICON", -1, (object) null, (object) null, -1));
    iGcol7.Width = OptionsImportForm._colWidths["TYPE_ICON"];
    iGcol7.CellStyle = iGcellStyle3;
    iGCol col9 = this.igObjects.Cols["OBJECT_CAPTION"];
    iGCol iGcol8 = this.igObjects.Cols["OBJECT_CAPTION"] ?? this.igObjects.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["OBJECT_CAPTION"], true, true, 200, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_3"), "OBJECT_CAPTION", -1, (object) null, (object) null, -1));
    iGcol8.Width = OptionsImportForm._colWidths["OBJECT_CAPTION"];
    iGcol8.CellStyle = this.cellString;
    iGCol col10 = this.igObjects.Cols["OBJECT_ID"];
    iGCol iGcol9 = this.igObjects.Cols["OBJECT_ID"] ?? this.igObjects.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["OBJECT_ID"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "OBJECT_ID", -1, (object) null, (object) null, -1));
    iGcol9.Width = OptionsImportForm._colWidths["OBJECT_ID"];
    iGcol9.CellStyle = this.cellString;
    iGCol col11 = this.igObjects.Cols["OBJECT_TYPE"];
    iGCol iGcol10 = this.igObjects.Cols["OBJECT_TYPE"] ?? this.igObjects.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["OBJECT_TYPE"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "OBJECT_TYPE", -1, (object) null, (object) null, -1));
    iGcol10.Width = OptionsImportForm._colWidths["OBJECT_TYPE"];
    iGcol10.CellStyle = this.cellString;
    iGCol col12 = this.igObjects.Cols["OBJECT_HOLDER"];
    (this.igObjects.Cols["OBJECT_HOLDER"] ?? this.igObjects.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["OBJECT_HOLDER"], false, false, 0, 0, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) string.Empty, "OBJECT_HOLDER", -1, (object) null, (object) null, -1))).Width = OptionsImportForm._colWidths["OBJECT_HOLDER"];
    iGCol col13 = this.igObjects.Cols["INCOMP"];
    iGCol iGcol11 = this.igObjects.Cols["INCOMP"] ?? this.igObjects.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["INCOMP"], true, true, 180, 180, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_91"), "INCOMP", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol11.CellStyle = this.cellString;
    iGcol11.Width = OptionsImportForm._colWidths["INCOMP"];
    iGCol col14 = this.igObjects.Cols["LINKED"];
    iGCol iGcol12 = this.igObjects.Cols["LINKED"] ?? this.igObjects.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["LINKED"], true, true, 180, 180, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_92"), "LINKED", -1, (object) string.Empty, (object) string.Empty, -1));
    iGcol12.CellStyle = this.cellString;
    iGcol12.Width = OptionsImportForm._colWidths["LINKED"];
    iGCol col15 = this.igDependent.Cols["OPTIONS_LIST"];
    iGCol iGcol13 = this.igDependent.Cols["OPTIONS_LIST"] ?? this.igDependent.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["OPTIONS_LIST"], true, true, 200, -1, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_93"), "OPTIONS_LIST", -1, (object) null, (object) null, -1));
    iGcol13.Width = OptionsImportForm._colWidths["OPTIONS_LIST"];
    iGcol13.CellStyle = this.cellString;
    iGCol col16 = this.igDependent.Cols["DESCRIPTION"];
    iGCol iGcol14 = this.igDependent.Cols["DESCRIPTION"] ?? this.igDependent.Cols.Add(new iGColPattern(OptionsImportForm._colWidths["DESCRIPTION"], true, true, 280, 280, false, false, false, iGSortType.None, iGSortOrder.None, false, (object) null, (object) LocalizationHolder.rm.GetString("PdmConfigurator_94"), "DESCRIPTION", -1, (object) null, (object) null, -1));
    iGcol14.Width = OptionsImportForm._colWidths["DESCRIPTION"];
    iGcol14.CellStyle = this.cellString;
    this.CorrectOptionsColsWidth();
    this.CorrectObjectsColsWidth();
    this.CorrectDependentColsWidth();
  }

  private void CorrectOptionsColsWidth()
  {
    if (this.igOptions.AutoResizeCols || OptionsImportForm._colWidths.Count == 0)
      return;
    int num = this.igOptions.ClientRectangle.Width - 30 - OptionsImportForm._colWidths["IMPORTED"] - OptionsImportForm._colWidths["OPTION_TYPE_ICON"];
    if (this.igOptions.Cols.Count == 0)
      return;
    this.igOptions.Cols["IMPORTED"].Width = OptionsImportForm._colWidths["IMPORTED"];
    this.igOptions.Cols["OPTION_TYPE_ICON"].Width = OptionsImportForm._colWidths["OPTION_TYPE_ICON"];
    this.igOptions.Cols["OPTION_CAPTION"].Width = num <= 200 ? OptionsImportForm._colWidths["OPTION_CAPTION"] : (OptionsImportForm._colWidths["OPTION_CAPTION"] = num);
    this.igOptions.Rows.AutoHeight();
  }

  private void CorrectObjectsColsWidth()
  {
    if (this.igObjects.AutoResizeCols || OptionsImportForm._colWidths.Count == 0)
      return;
    int num = this.igObjects.ClientRectangle.Width - 30 - OptionsImportForm._colWidths["RADIO"] - OptionsImportForm._colWidths["TYPE_ICON"] - OptionsImportForm._colWidths["INCOMP"] - OptionsImportForm._colWidths["LINKED"];
    if (this.igObjects.Cols.Count == 0)
      return;
    this.igObjects.Cols["RADIO"].Width = OptionsImportForm._colWidths["RADIO"];
    this.igObjects.Cols["TYPE_ICON"].Width = OptionsImportForm._colWidths["TYPE_ICON"];
    this.igObjects.Cols["INCOMP"].Width = OptionsImportForm._colWidths["INCOMP"];
    this.igObjects.Cols["LINKED"].Width = OptionsImportForm._colWidths["LINKED"];
    this.igObjects.Cols["OBJECT_CAPTION"].Width = num <= 200 ? OptionsImportForm._colWidths["OBJECT_CAPTION"] : (OptionsImportForm._colWidths["OBJECT_CAPTION"] = num);
    this.igObjects.Rows.AutoHeight();
  }

  private void CorrectDependentColsWidth()
  {
    if (this.igDependent.AutoResizeCols || OptionsImportForm._colWidths.Count == 0)
      return;
    int num = this.igDependent.ClientRectangle.Width - 30 - OptionsImportForm._colWidths["DESCRIPTION"];
    if (this.igDependent.Cols.Count == 0)
      return;
    this.igDependent.Cols["DESCRIPTION"].Width = OptionsImportForm._colWidths["DESCRIPTION"];
    this.igDependent.Cols["OPTIONS_LIST"].Width = num <= 200 ? OptionsImportForm._colWidths["OPTIONS_LIST"] : (OptionsImportForm._colWidths["OPTIONS_LIST"] = num);
    this.igDependent.Rows.AutoHeight();
  }

  private void ChangeDependent(
    bool value,
    OptionsImportForm.DependentType type,
    iGRow optRow,
    ObjectOptionsHolder objectHolder)
  {
    OptionHolder optionHolder = optRow.Cells["OPTION"].Value as OptionHolder;
    if (type == OptionsImportForm.DependentType.Linked)
    {
      LinkedOptions linked = new LinkedOptions();
      string[] strArray;
      if (optRow.Index == this.igOptions.SelectedCells[0].RowIndex)
        strArray = optRow.Cells["LINKED_IDS_LIST"].Value.ToString().Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.RemoveEmptyEntries);
      else
        strArray = new List<string>((IEnumerable<string>) this.FindLinkedOptions(objectHolder, optionHolder.OptionGuid).Values).ToArray();
      if (value)
      {
        this.CheckLinkedOptions(objectHolder, strArray);
        LinkedOptions linkedOptions = objectHolder.Incompatibilities.LinkedOptions.Clone() as LinkedOptions;
        linked.Items = linkedOptions.SelectLinkedOptions(optionHolder.OptionGuid);
      }
      this.importOptions.ChangeLinkedOptions(optionHolder.OptionObjectID, strArray, linked, !value);
    }
    else
    {
      IPdmCriterion criterion = objectHolder.Incompatibilities.FindCriterion(optionHolder.OptionGuid);
      string[] strArray;
      if (optRow.Index == this.igOptions.SelectedCells[0].RowIndex)
      {
        strArray = optRow.Cells["INCOMP_IDS_LIST"].Value.ToString().Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.RemoveEmptyEntries);
        if (value)
          this.CheckLinkedOptions(objectHolder, strArray);
      }
      else
        strArray = new List<string>((IEnumerable<string>) this.FindIncompOptions(objectHolder, optionHolder.OptionGuid).Values).ToArray();
      this.importOptions.ChangeIncompOptions(optionHolder.OptionObjectID, strArray, criterion != null ? criterion.Clone() as IPdmCriterion : (IPdmCriterion) null, !value);
    }
    this.ChangeDependence(objectHolder.ObjectID, optionHolder.OptionObjectID, type, value);
  }

  private OptionHolder GetSelectedOptionHolder()
  {
    iGRow row = this.igOptions.SelectedCells.Count <= 0 || !this.igOptions.SelectedCells[0].Selected ? (iGRow) null : this.igOptions.SelectedCells[0].Row;
    return row == null ? (OptionHolder) null : row.Cells["OPTION"].Value as OptionHolder;
  }

  private iGRow GetSelectedObject()
  {
    return this.igObjects.SelectedCells.Count <= 0 || !this.igObjects.SelectedCells[0].Selected ? (iGRow) null : this.igObjects.SelectedCells[0].Row;
  }

  private bool IsImportOptionExists()
  {
    foreach (iGRow row in (IEnumerable) this.igOptions.Rows)
    {
      if (row.Level == 1 && Convert.ToBoolean(row.Cells["IMPORTED"].Value))
        return true;
    }
    return false;
  }

  private bool IsNotImportOptionExists()
  {
    foreach (iGRow row in (IEnumerable) this.igOptions.Rows)
    {
      if (row.Level == 1 && !Convert.ToBoolean(row.Cells["IMPORTED"].Value))
        return true;
    }
    return false;
  }

  private bool IsOptionSelect() => this.igOptions.SelectedCells[0].Row.Level == 1;

  private iGRow GetCheckedObject()
  {
    foreach (iGRow row in (IEnumerable) this.igObjects.Rows)
    {
      if (Convert.ToBoolean(row.Cells["RADIO"].ImageIndex))
        return row;
    }
    return (iGRow) null;
  }

  private void CheckLinkedOptions(ObjectOptionsHolder objectHolder, string[] linked)
  {
    for (int index = 0; index < linked.Length; ++index)
    {
      long int64 = Convert.ToInt64(linked[index]);
      foreach (iGRow row in (IEnumerable) this.igOptions.Rows)
      {
        if (row.Level == 1)
        {
          OptionHolder optionHolder = row.Cells["OPTION"].Value as OptionHolder;
          if (optionHolder.OptionObjectID == int64)
          {
            row.Cells["IMPORTED"].Value = (object) true;
            List<string> visibleValues = objectHolder.VisibleOptionValues.Items[optionHolder.OptionGuid];
            if (this.StateForObjectID(int64) != objectHolder.ObjectID)
            {
              this.RemoveFromRecovery(int64);
              this.AddToRecovery(objectHolder.ObjectID, int64);
              this.importOptions.AddImportOption(int64, objectHolder.ObjectID, visibleValues);
            }
            else if (this.importOptions.IsOptionExists(int64) == null)
              this.AddOption(row);
          }
        }
      }
    }
  }

  private ObjectOptionsHolder FindObjectOptionsHolder(long objectID)
  {
    foreach (ObjectOptionsHolder objectsOption in this.objectsOptions)
    {
      if (objectsOption.ObjectID == objectID)
        return objectsOption;
    }
    return (ObjectOptionsHolder) null;
  }

  private void AddOption(iGRow optRow)
  {
    OptionHolder optionHolder = optRow.Cells["OPTION"].Value as OptionHolder;
    bool incom = false;
    bool linked = false;
    ObjectOptionsHolder objectOptionsHolder;
    if (optRow.Index == this.igOptions.SelectedCells[0].RowIndex)
    {
      iGRow checkedObject = this.GetCheckedObject();
      objectOptionsHolder = checkedObject.Cells["OBJECT_HOLDER"].Value as ObjectOptionsHolder;
      incom = Convert.ToBoolean(checkedObject.Cells["INCOMP"].Value);
      linked = Convert.ToBoolean(checkedObject.Cells["LINKED"].Value);
    }
    else
      objectOptionsHolder = this.FindObjectOptionsHolder(optionHolder.OptionObjectID, incom, linked);
    List<string> visibleValues = objectOptionsHolder.VisibleOptionValues.Items[optionHolder.OptionGuid];
    this.importOptions.AddImportOption(optionHolder.OptionObjectID, objectOptionsHolder.ObjectID, visibleValues);
    this.AddToRecovery(objectOptionsHolder.ObjectID, optionHolder.OptionObjectID);
    if (incom)
      this.ChangeDependent(incom, OptionsImportForm.DependentType.Incomp, optRow, objectOptionsHolder);
    if (!linked)
      return;
    this.ChangeDependent(linked, OptionsImportForm.DependentType.Linked, optRow, objectOptionsHolder);
  }

  private ObjectOptionsHolder FindObjectOptionsHolder(long optionID, bool incom, bool linked)
  {
    ObjectOptionsHolder objectOptionsHolder1 = new ObjectOptionsHolder();
    foreach (long key1 in this.recoveryDictionary.Keys)
    {
      Dictionary<long, OptionsImportForm.DependentType> recovery = this.recoveryDictionary[key1];
      foreach (long key2 in recovery.Keys)
      {
        if (key2 == optionID)
        {
          ObjectOptionsHolder objectOptionsHolder2 = this.FindObjectOptionsHolder(key1);
          int num = (int) recovery[key2];
          incom = (num & 1) == 1;
          linked = (num & 2) == 2;
          return objectOptionsHolder2;
        }
      }
    }
    long objectID = this.dict[optionID][0];
    incom = linked = false;
    return this.FindObjectOptionsHolder(objectID);
  }

  private void CollectCategories()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (PdmConfiguratorCache.CategoriesCache.Count == 0)
        PdmConfiguratorCache.CacheLoadCategories(sessionKeeper.Session);
    }
    foreach (long key in this.dict.Keys)
    {
      OptionHolder option = PdmConfiguratorCache.CacheFindOption(key);
      if (option == null)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, key);
          option = PdmConfiguratorCache.CacheFindOption(key);
          if (option == null)
            continue;
        }
      }
      OptionObjectDescription category = PdmConfiguratorCache.CacheFindCategory(option.OptionCategory);
      if (this.categories.ContainsKey(category))
      {
        this.categories[category].Add(option);
      }
      else
      {
        this.categories.Add(category, new List<OptionHolder>());
        this.categories[category].Add(option);
      }
    }
  }

  private long IsExcludeOptionExists(string[] linkedOptionsID)
  {
    foreach (string str in linkedOptionsID)
    {
      long int64 = Convert.ToInt64(str);
      if (this.excludedOptions.Contains(int64))
        return int64;
    }
    return 0;
  }

  private void iGridSelectRowCells(iGRow row, bool select)
  {
    if (row == null)
      return;
    for (int colIndex = 0; colIndex < row.Cells.Count; ++colIndex)
      row.Cells[colIndex].Selected = select;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.toolBarTop.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.tbOptionWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.menuBarOptions.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.menuBarObjects.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged -= new EventHandler(this.ToolbarRendererChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OptionsImportForm));
    this.splitContainer1 = new SplitContainer();
    this.igOptions = new iGrid();
    this.tbOptionWork = new Intermech.Bars.ToolBar();
    this.ilGrids = new ImageList();
    this.btnCard = new ButtonItem();
    this.btnCheck = new ButtonItem();
    this.btnUncheck = new ButtonItem();
    this.splitContainer2 = new SplitContainer();
    this.igObjects = new iGrid();
    this.toolBarTop = new Intermech.Bars.ToolBar();
    this.btnObjectCard = new ButtonItem();
    this.menuBarObjects = new MenuBar();
    this.contextMenuBarObjects = new ContextMenuBarItem();
    this.mnpObjectCard = new MenuButtonItem();
    this.igDependent = new iGrid();
    this.ilGridImage = new ImageList();
    this.panel3 = new Panel();
    this.btnCancel = new Button();
    this.btnApply = new Button();
    this.menuBarOptions = new MenuBar();
    this.contextMenuBarOptions = new ContextMenuBarItem();
    this.mnpCard = new MenuButtonItem();
    this.mnpCheck = new MenuButtonItem();
    this.mnpUncheck = new MenuButtonItem();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    ((ISupportInitialize) this.igOptions).BeginInit();
    this.splitContainer2.BeginInit();
    this.splitContainer2.Panel1.SuspendLayout();
    this.splitContainer2.Panel2.SuspendLayout();
    this.splitContainer2.SuspendLayout();
    ((ISupportInitialize) this.igObjects).BeginInit();
    ((ISupportInitialize) this.igDependent).BeginInit();
    this.panel3.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.FixedPanel = FixedPanel.Panel1;
    this.splitContainer1.Name = "splitContainer1";
    componentResourceManager.ApplyResources((object) this.splitContainer1.Panel1, "splitContainer1.Panel1");
    this.splitContainer1.Panel1.Controls.Add((Control) this.igOptions);
    this.splitContainer1.Panel1.Controls.Add((Control) this.tbOptionWork);
    componentResourceManager.ApplyResources((object) this.splitContainer1.Panel2, "splitContainer1.Panel2");
    this.splitContainer1.Panel2.Controls.Add((Control) this.splitContainer2);
    componentResourceManager.ApplyResources((object) this.igOptions, "igOptions");
    this.igOptions.BackColorEvenRows = Color.WhiteSmoke;
    this.igOptions.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this.igOptions.DefaultRow.Key = componentResourceManager.GetString("resource.Key");
    this.igOptions.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    this.igOptions.GridLines.GroupRows = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igOptions.GridLines.Horizontal = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igOptions.GridLines.HorizontalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igOptions.GridLines.HorizontalLastRow = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igOptions.GridLines.Vertical = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igOptions.GridLines.VerticalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igOptions.GridLines.VerticalLastCol = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igOptions.Header.Height = (int) componentResourceManager.GetObject("igOptions.Header.Height");
    this.igOptions.HighlightBackColorNoFocus = SystemColors.ControlLight;
    this.igOptions.HotTracking = false;
    this.igOptions.LayoutObject.Flags = iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this.igOptions.Name = "igOptions";
    this.menuBarOptions.SetPopupMenu((Control) this.igOptions, (MenuBarItem) this.contextMenuBarOptions);
    this.igOptions.ProcessTab = false;
    this.igOptions.RowMode = true;
    this.igOptions.SilentValidation = true;
    this.igOptions.SingleClickEdit = true;
    this.igOptions.SortByLevels = true;
    this.igOptions.CellMouseDown += new iGCellMouseDownEventHandler(this.DoCellMouseDown);
    this.igOptions.ColWidthEndChange += new iGColWidthEventHandler(this.igOptions_ColWidthEndChange);
    this.igOptions.ColWidthChanging += new iGColWidthEventHandler(this.igOptions_ColWidthChanging);
    this.igOptions.SelectionChanged += new EventHandler(this.igOptions_SelectionChanged);
    this.igOptions.AfterCommitEdit += new iGAfterCommitEditEventHandler(this.igOptions_AfterCommitEdit);
    this.igOptions.Resize += new EventHandler(this.igOptions_Resize);
    componentResourceManager.ApplyResources((object) this.tbOptionWork, "tbOptionWork");
    this.tbOptionWork.FullMenus = true;
    this.tbOptionWork.Guid = new Guid("37056402-c6d1-47d4-be0f-e941c1a06e55");
    this.tbOptionWork.Hidden = false;
    this.tbOptionWork.ImageList = this.ilGrids;
    this.tbOptionWork.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.btnCard,
      (ToolbarItemBase) this.btnCheck,
      (ToolbarItemBase) this.btnUncheck
    });
    this.tbOptionWork.Name = "tbOptionWork";
    this.tbOptionWork.Tag = (object) "";
    this.ilGrids.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilGrids.ImageStream");
    this.ilGrids.TransparentColor = Color.Transparent;
    this.ilGrids.Images.SetKeyName(0, "cb_unckecked.ico");
    this.ilGrids.Images.SetKeyName(1, "cb_checked.ico");
    this.ilGrids.Images.SetKeyName(2, "ball_green_plus.ico");
    this.ilGrids.Images.SetKeyName(3, "delete.ico");
    this.ilGrids.Images.SetKeyName(4, "down_plus.png");
    this.btnCard.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnCard, "btnCard");
    this.btnCard.Click += new EventHandler(this.btnCard_Click);
    this.btnCheck.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnCheck, "btnCheck");
    this.btnCheck.ImageIndex = 1;
    this.btnCheck.Click += new EventHandler(this.btnCheck_Click);
    componentResourceManager.ApplyResources((object) this.btnUncheck, "btnUncheck");
    this.btnUncheck.ImageIndex = 0;
    this.btnUncheck.Click += new EventHandler(this.btnUncheck_Click);
    componentResourceManager.ApplyResources((object) this.splitContainer2, "splitContainer2");
    this.splitContainer2.FixedPanel = FixedPanel.Panel2;
    this.splitContainer2.Name = "splitContainer2";
    componentResourceManager.ApplyResources((object) this.splitContainer2.Panel1, "splitContainer2.Panel1");
    this.splitContainer2.Panel1.Controls.Add((Control) this.igObjects);
    this.splitContainer2.Panel1.Controls.Add((Control) this.toolBarTop);
    this.splitContainer2.Panel1.Controls.Add((Control) this.menuBarObjects);
    componentResourceManager.ApplyResources((object) this.splitContainer2.Panel2, "splitContainer2.Panel2");
    this.splitContainer2.Panel2.Controls.Add((Control) this.igDependent);
    componentResourceManager.ApplyResources((object) this.igObjects, "igObjects");
    this.igObjects.BackColorEvenRows = Color.WhiteSmoke;
    this.igObjects.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height1");
    this.igObjects.DefaultRow.Key = componentResourceManager.GetString("resource.Key1");
    this.igObjects.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight1");
    this.igObjects.GridLines.GroupRows = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igObjects.GridLines.Horizontal = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igObjects.GridLines.HorizontalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igObjects.GridLines.HorizontalLastRow = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igObjects.GridLines.Vertical = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igObjects.GridLines.VerticalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igObjects.GridLines.VerticalLastCol = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igObjects.Header.Height = (int) componentResourceManager.GetObject("igObjects.Header.Height");
    this.igObjects.HighlightBackColorNoFocus = SystemColors.ControlLight;
    this.igObjects.HotTracking = false;
    this.igObjects.LayoutObject.Flags = iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this.igObjects.Name = "igObjects";
    this.menuBarObjects.SetPopupMenu((Control) this.igObjects, (MenuBarItem) this.contextMenuBarObjects);
    this.igObjects.ProcessTab = false;
    this.igObjects.RowMode = true;
    this.igObjects.RowModeHasCurCell = true;
    this.igObjects.SilentValidation = true;
    this.igObjects.SingleClickEdit = true;
    this.igObjects.CellMouseDown += new iGCellMouseDownEventHandler(this.DoCellMouseDown);
    this.igObjects.CellDoubleClick += new iGCellDoubleClickEventHandler(this.igObjects_CellDoubleClick);
    this.igObjects.CellClick += new iGCellClickEventHandler(this.igObjects_CellClick);
    this.igObjects.ColWidthEndChange += new iGColWidthEventHandler(this.igObjects_ColWidthEndChange);
    this.igObjects.ColWidthChanging += new iGColWidthEventHandler(this.igObjects_ColWidthChanging);
    this.igObjects.SelectionChanged += new EventHandler(this.igObjects_SelectionChanged);
    this.igObjects.BeforeCommitEdit += new iGBeforeCommitEditEventHandler(this.igObjects_BeforeCommitEdit);
    this.igObjects.AfterCommitEdit += new iGAfterCommitEditEventHandler(this.igObjects_AfterCommitEdit);
    this.igObjects.Resize += new EventHandler(this.igObjects_Resize);
    componentResourceManager.ApplyResources((object) this.toolBarTop, "toolBarTop");
    this.toolBarTop.AddRemoveButtonsVisible = false;
    this.toolBarTop.AllowHorizontalDock = false;
    this.toolBarTop.DockLine = 3;
    this.toolBarTop.DrawActionsButton = false;
    this.toolBarTop.FullMenus = true;
    this.toolBarTop.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarTop.Hidden = false;
    this.toolBarTop.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.btnObjectCard
    });
    this.toolBarTop.MinimumFloatingSize = new Size(250, 30);
    this.toolBarTop.Name = "toolBarTop";
    this.toolBarTop.Overflow = ToolBarOverflow.Wrap;
    this.toolBarTop.Stretch = true;
    this.toolBarTop.Tearable = false;
    componentResourceManager.ApplyResources((object) this.btnObjectCard, "btnObjectCard");
    this.btnObjectCard.Enabled = false;
    this.btnObjectCard.Click += new EventHandler(this.btnObjectCard_Click);
    componentResourceManager.ApplyResources((object) this.menuBarObjects, "menuBarObjects");
    this.menuBarObjects.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuBarObjects.Hidden = false;
    this.menuBarObjects.ImageList = this.ilGrids;
    this.menuBarObjects.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuBarObjects
    });
    this.menuBarObjects.Name = "menuBarObjects";
    this.menuBarObjects.OwnerForm = (Form) null;
    componentResourceManager.ApplyResources((object) this.contextMenuBarObjects, "contextMenuBarObjects");
    this.contextMenuBarObjects.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.mnpObjectCard
    });
    this.contextMenuBarObjects.ShowText = true;
    this.mnpObjectCard.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpObjectCard, "mnpObjectCard");
    this.mnpObjectCard.ShowText = true;
    this.mnpObjectCard.Click += new EventHandler(this.btnObjectCard_Click);
    componentResourceManager.ApplyResources((object) this.igDependent, "igDependent");
    this.igDependent.BackColorEvenRows = Color.WhiteSmoke;
    this.igDependent.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height2");
    this.igDependent.DefaultRow.Key = componentResourceManager.GetString("resource.Key2");
    this.igDependent.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight2");
    this.igDependent.GridLines.GroupRows = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igDependent.GridLines.Horizontal = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igDependent.GridLines.HorizontalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igDependent.GridLines.HorizontalLastRow = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igDependent.GridLines.Vertical = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igDependent.GridLines.VerticalExtended = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igDependent.GridLines.VerticalLastCol = new iGPenStyle(SystemColors.ControlLight, 1, DashStyle.Dot);
    this.igDependent.Header.Height = (int) componentResourceManager.GetObject("igDependent.Header.Height");
    this.igDependent.HighlightBackColorNoFocus = SystemColors.ControlLight;
    this.igDependent.HotTracking = false;
    this.igDependent.ImageList = this.ilGridImage;
    this.igDependent.LayoutObject.Flags = iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this.igDependent.Name = "igDependent";
    this.igDependent.ProcessTab = false;
    this.igDependent.ReadOnly = true;
    this.igDependent.RowMode = true;
    this.igDependent.SilentValidation = true;
    this.igDependent.ColWidthEndChange += new iGColWidthEventHandler(this.igDependent_ColWidthEndChange);
    this.igDependent.ColWidthChanging += new iGColWidthEventHandler(this.igDependent_ColWidthChanging);
    this.igDependent.Resize += new EventHandler(this.igDependent_Resize);
    this.ilGridImage.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilGridImage.ImageStream");
    this.ilGridImage.TransparentColor = Color.Transparent;
    this.ilGridImage.Images.SetKeyName(0, "rb_unchecked.ico");
    this.ilGridImage.Images.SetKeyName(1, "rb_checked.ico");
    this.ilGridImage.Images.SetKeyName(2, "gears_stop.png");
    this.ilGridImage.Images.SetKeyName(3, "gears.png");
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Controls.Add((Control) this.btnCancel);
    this.panel3.Controls.Add((Control) this.btnApply);
    this.panel3.Name = "panel3";
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.btnApply, "btnApply");
    this.btnApply.Name = "btnApply";
    this.btnApply.UseVisualStyleBackColor = true;
    this.btnApply.Click += new EventHandler(this.btnApply_Click);
    componentResourceManager.ApplyResources((object) this.menuBarOptions, "menuBarOptions");
    this.menuBarOptions.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuBarOptions.Hidden = false;
    this.menuBarOptions.ImageList = this.ilGrids;
    this.menuBarOptions.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuBarOptions
    });
    this.menuBarOptions.Name = "menuBarOptions";
    this.menuBarOptions.OwnerForm = (Form) null;
    componentResourceManager.ApplyResources((object) this.contextMenuBarOptions, "contextMenuBarOptions");
    this.contextMenuBarOptions.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.mnpCard,
      (ToolbarItemBase) this.mnpCheck,
      (ToolbarItemBase) this.mnpUncheck
    });
    this.contextMenuBarOptions.ShowText = true;
    this.mnpCard.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpCard, "mnpCard");
    this.mnpCard.ShowText = true;
    this.mnpCard.Click += new EventHandler(this.btnCard_Click);
    this.mnpCheck.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpCheck, "mnpCheck");
    this.mnpCheck.ImageIndex = 1;
    this.mnpCheck.ShowText = true;
    this.mnpCheck.Click += new EventHandler(this.btnCheck_Click);
    componentResourceManager.ApplyResources((object) this.mnpUncheck, "mnpUncheck");
    this.mnpUncheck.ImageIndex = 0;
    this.mnpUncheck.ShowText = true;
    this.mnpUncheck.Click += new EventHandler(this.btnUncheck_Click);
    this.AcceptButton = (IButtonControl) this.btnApply;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.Controls.Add((Control) this.splitContainer1);
    this.Controls.Add((Control) this.panel3);
    this.Controls.Add((Control) this.menuBarOptions);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (OptionsImportForm);
    this.ShowInTaskbar = false;
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.FormClosed += new FormClosedEventHandler(this.OnFormClosed);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    ((ISupportInitialize) this.igOptions).EndInit();
    this.splitContainer2.Panel1.ResumeLayout(false);
    this.splitContainer2.Panel2.ResumeLayout(false);
    this.splitContainer2.EndInit();
    this.splitContainer2.ResumeLayout(false);
    ((ISupportInitialize) this.igObjects).EndInit();
    ((ISupportInitialize) this.igDependent).EndInit();
    this.panel3.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  [Flags]
  private enum DependentType
  {
    None = 0,
    Incomp = 1,
    Linked = 2,
  }
}
