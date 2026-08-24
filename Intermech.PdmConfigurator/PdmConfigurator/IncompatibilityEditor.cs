// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.IncompatibilityEditor
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using DevExpress.IM.Utils;
using DevExpress.IM.XtraEditors.Controls;
using DevExpress.IM.XtraEditors.Repository;
using DevExpress.IM.XtraTreeList;
using DevExpress.IM.XtraTreeList.Columns;
using DevExpress.IM.XtraTreeList.Nodes;
using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.Search.Pdm.CompositionsConfigurator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator;

public class IncompatibilityEditor : UserControl
{
  private IncompatibilityEditor.PathPart[] _focusPath = new IncompatibilityEditor.PathPart[0];
  private const string NodeTypeColumnKey = "CATEGORY";
  public const string NotColumnKey = "NotColumnKey";
  private const string OptionColumnKey = "CONFLICT_OPTION";
  private const string RelationOperatorColumnKey = "OPERATION";
  private const string OptionValueColumnKey = "CONFLICT_VALUE";
  public const string NotString = "НЕ";
  private OptionAccessRights _optionAccessRights;
  protected ObjectOptionsHolder _objectOptionsHolder;
  protected IPdmCriterion _pdmCriterionsCollection;
  protected PdmCriterionsCollection resultCollection = (PdmCriterionsCollection) new ObjectIncompatibilitiesCollection();
  private OptionHolder _selectedOptionHolder;
  private OptionValue _selectedOptionValue;
  private bool _isChanged;
  private bool localChange;
  protected RepositoryItemComboBox cbVisibleValue = new RepositoryItemComboBox();
  protected RepositoryItemComboBox cbAvailableOptions = new RepositoryItemComboBox();
  protected RepositoryItemComboBox cbOperators = new RepositoryItemComboBox();
  protected RepositoryItemTextEdit teReadOnly = new RepositoryItemTextEdit();
  private RepositoryItemComboBox _booleanOperatorsRepositoryItemComboBox = new RepositoryItemComboBox();
  private RepositoryItemComboBox _stringOperatorsRepositoryItemComboBox = new RepositoryItemComboBox();
  private TreeListColumn _notTreeListColumn;
  private IContainer components;
  private TreeList _treeList;
  private Intermech.Bars.ToolBar tbOptionWork;
  private ButtonItem _deleteButtonItem;
  private ButtonItem _addCriterionButtonItem;
  private ButtonItem _addCriterionGroupButtonItem;
  private TreeListColumn colConflictOption;
  private TreeListColumn colOperation;
  private TreeListColumn colConflictValue;
  private ImageList ilOptions;
  private RepositoryItemImageComboBox repositoryItemImageComboBox2;
  private RepositoryItemComboBox repositoryItemComboBox1;
  private ButtonItem _addChildCriterionGroupButtonItem;
  private ButtonItem _addChildCriterionButtonItem;
  private ButtonItem _changeLogicalOperatorButtonItem;
  private ToolTip toolTip1;
  private TreeListColumn colCategory;
  private TreeListColumn colOptionError;
  private RepositoryItemTextEdit cellEditor;
  private MenuBar cmsOptionWork;
  private ContextMenuBarItem contextMenuBarItem;
  private MenuButtonItem tsmAddOption;
  private MenuButtonItem tsmDeleteOption;
  private MenuButtonItem tsmClear;
  private PictureBox pbError;
  private Panel errorPanel;
  private Label lbErrorState;
  private MenuBarItem menuBarItem1;
  private MenuButtonItem _deleteMenuButtonItem;
  private MenuButtonItem _addCriterionGroupMenuButtonItem;
  private MenuButtonItem _addCriterionMenuButtonItem;
  private MenuButtonItem _addChildCriterionGroupMenuButtonItem;
  private MenuButtonItem _addChildCriterionMenuButtonItem;
  private MenuButtonItem _changeLogicalOperatorMenuButtonItem;
  internal Panel panelHint;
  internal Label labelWarning;
  internal PictureBox pictureHint;
  private TextBox _hintTextBox;
  private ImageList ilError;
  private ButtonItem _notButtonItem;

  public IncompatibilityEditor()
  {
    this.InitializeComponent();
    this._notTreeListColumn = this._treeList.Columns.Add();
    this._notTreeListColumn.Caption = "Отрицание";
    this._notTreeListColumn.FieldName = nameof (NotColumnKey);
    this._notTreeListColumn.Options = ColumnOptions.CanResized | ColumnOptions.ReadOnly;
    this._notTreeListColumn.VisibleIndex = 1;
    this._notTreeListColumn.Width = 25;
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.tbOptionWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.cmsOptionWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.cmsOptionWork.Visible = false;
      service.RendererChanged += new EventHandler(this.BarManager_RendererChanged);
      this.BarManager_RendererChanged((object) service, EventArgs.Empty);
    }
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1832);
  }

  public event EventHandler Changed;

  public bool IsChanged
  {
    get => this._isChanged;
    set
    {
      this._isChanged = this.localChange = value;
      this.OnChanged();
      this.UpdateControls();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public IncompatibilityEditor.PathPart[] FocusPath
  {
    get => this._focusPath;
    set
    {
      if (this._focusPath == value)
        return;
      this._focusPath = value ?? new IncompatibilityEditor.PathPart[0];
      if (this._focusPath.Length != 0)
        this._treeList.FocusedNode = this.FindNode(this._treeList.Nodes, value);
      else
        this._treeList.FocusedNode = (TreeListNode) null;
    }
  }

  public IncompatibilityEditor.PathPart[] GetLiveFocusPath()
  {
    List<IncompatibilityEditor.PathPart> source = new List<IncompatibilityEditor.PathPart>();
    for (TreeListNode node = this._treeList.FocusedNode; node != null; node = node.ParentNode)
      source.Add(this.GetPathPart(node));
    return source.Reverse<IncompatibilityEditor.PathPart>().ToArray<IncompatibilityEditor.PathPart>();
  }

  private IncompatibilityEditor.PathPart GetPathPart(TreeListNode node)
  {
    return new IncompatibilityEditor.PathPart(node[(object) "CONFLICT_OPTION"], node[(object) "CONFLICT_VALUE"]);
  }

  private TreeListNode FindNode(TreeListNodes nodes, IncompatibilityEditor.PathPart[] path)
  {
    IncompatibilityEditor.PathPart objA = ((IEnumerable<IncompatibilityEditor.PathPart>) path).FirstOrDefault<IncompatibilityEditor.PathPart>();
    IncompatibilityEditor.PathPart[] array = ((IEnumerable<IncompatibilityEditor.PathPart>) path).Skip<IncompatibilityEditor.PathPart>(1).ToArray<IncompatibilityEditor.PathPart>();
    foreach (TreeListNode node1 in nodes)
    {
      IncompatibilityEditor.PathPart pathPart = this.GetPathPart(node1);
      if (object.Equals((object) objA, (object) pathPart))
      {
        if (array.Length == 0)
          return node1;
        TreeListNode node2 = this.FindNode(node1.Nodes, array);
        if (node2 != null)
          return node2;
      }
    }
    return (TreeListNode) null;
  }

  public void LoadOptions(
    ObjectOptionsHolder options,
    OptionHolder selectedOption,
    OptionValue selectedValue,
    OptionAccessRights accessRights)
  {
    this.Save();
    this._optionAccessRights = accessRights;
    this._treeList.ClearNodes();
    this._objectOptionsHolder = options;
    this._selectedOptionHolder = selectedOption;
    this._selectedOptionValue = selectedValue;
    if (!this.DisableEdit())
    {
      this.FindCriterionCollection();
      this._treeList.BeginUpdate();
      try
      {
        this._treeList.ClearNodes();
        this.FillCriterionsTree((TreeListNode) null, this._pdmCriterionsCollection);
      }
      finally
      {
        this._treeList.EndUpdate();
      }
      this.InitRepositories();
    }
    this.UpdateControls();
  }

  protected virtual void FindCriterionCollection()
  {
    this._pdmCriterionsCollection = this._objectOptionsHolder.Incompatibilities.FindCriterion(this._selectedOptionHolder.OptionGuid);
    if (this._pdmCriterionsCollection == null)
    {
      PdmCriterion pdmCriterion1 = this._objectOptionsHolder.Incompatibilities.AddStubCriterion() as PdmCriterion;
      pdmCriterion1.Function = LogicalFunction.Or;
      pdmCriterion1.Option = this._selectedOptionHolder.OptionGuid;
      PdmCriterion pdmCriterion2 = pdmCriterion1.AddStubCriterion() as PdmCriterion;
      pdmCriterion2.Function = LogicalFunction.Or;
      pdmCriterion2.Option = this._selectedOptionHolder.OptionGuid;
      pdmCriterion2.Value = this._selectedOptionValue.ID;
      this._pdmCriterionsCollection = (IPdmCriterion) pdmCriterion2.Items;
    }
    else if (!(this._pdmCriterionsCollection.FindCriterion(this._selectedOptionHolder.OptionGuid, this._selectedOptionValue.ID) is PdmCriterion criterion))
    {
      PdmCriterion pdmCriterion = this._pdmCriterionsCollection.AddStubCriterion() as PdmCriterion;
      pdmCriterion.Function = LogicalFunction.Or;
      pdmCriterion.Option = this._selectedOptionHolder.OptionGuid;
      pdmCriterion.Value = this._selectedOptionValue.ID;
      this._pdmCriterionsCollection = (IPdmCriterion) pdmCriterion.Items;
    }
    else
      this._pdmCriterionsCollection = (IPdmCriterion) criterion.Items;
  }

  protected virtual bool DisableEdit()
  {
    return this._objectOptionsHolder == null || this._selectedOptionHolder == null || this._selectedOptionValue == null;
  }

  protected virtual void UpdateControls()
  {
    this.panelHint.Visible = this._optionAccessRights != OptionAccessRights.FullAccess;
    if (this.DisableEdit())
    {
      this.cmsOptionWork.Enabled = this.tbOptionWork.Enabled = this._treeList.Enabled = false;
      this.cmsOptionWork.Visible = false;
      this.errorPanel.Visible = false;
    }
    else
    {
      TreeListNode focusedNode = this._treeList.FocusedNode;
      if (this._optionAccessRights == OptionAccessRights.FullAccess)
      {
        this.cmsOptionWork.Enabled = this.tbOptionWork.Enabled = this._treeList.Enabled = true;
        this.cmsOptionWork.Visible = false;
        this._addChildCriterionGroupButtonItem.Enabled = this._addChildCriterionButtonItem.Enabled = this._addChildCriterionMenuButtonItem.Enabled = this._addChildCriterionGroupMenuButtonItem.Enabled = this.EnabledChildAddition(focusedNode);
        this._changeLogicalOperatorButtonItem.Enabled = this._changeLogicalOperatorMenuButtonItem.Enabled = focusedNode != null && !this.IsFirstNode(focusedNode);
        this._deleteMenuButtonItem.Enabled = this._deleteButtonItem.Enabled = focusedNode != null;
        this._addCriterionGroupButtonItem.Enabled = this._addCriterionButtonItem.Enabled = this._addCriterionGroupMenuButtonItem.Enabled = this._addCriterionMenuButtonItem.Enabled = this.EnabledAddition();
      }
      else
      {
        this.cmsOptionWork.Enabled = this.tbOptionWork.Enabled = false;
        this.cmsOptionWork.Visible = false;
        this._treeList.Enabled = true;
      }
      if (focusedNode != null)
      {
        if (focusedNode[(object) "OPTION_ERROR"] != null)
        {
          ErrorState errorState = (ErrorState) focusedNode[(object) "OPTION_ERROR"];
          this.lbErrorState.Text = EnumDescConverter.GetEnumDescription((Enum) errorState);
          switch (errorState)
          {
            case ErrorState.Value:
            case ErrorState.ObsoleteOption:
              this.pbError.Image = this.ilError.Images[0];
              break;
            case ErrorState.ObsoleteOptionValue:
              this.pbError.Image = this.ilError.Images[2];
              break;
            default:
              this.pbError.Image = this.ilError.Images[1];
              break;
          }
          this.errorPanel.Visible = errorState != ErrorState.None;
        }
      }
      else
        this.errorPanel.Visible = false;
      this._hintTextBox.Text = this.CreateHint();
      this._notButtonItem.Enabled = this._optionAccessRights == OptionAccessRights.FullAccess && focusedNode != null;
    }
  }

  protected virtual bool EnabledAddition() => this._objectOptionsHolder.Options.Count > 1;

  protected virtual void InitAvailableOptions()
  {
    this.cbAvailableOptions.TextEditStyle = TextEditStyles.DisableTextEditor;
    this.cbAvailableOptions.Buttons[0].Width = 14;
    this.cbAvailableOptions.DropDownRows = 15;
    this.cbAvailableOptions.BeginUpdate();
    try
    {
      this.cbAvailableOptions.Items.Clear();
      foreach (long option1 in this._objectOptionsHolder.Options)
      {
        OptionHolder option2 = PdmConfiguratorCache.CacheFindOption(option1);
        if (option2 == null)
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, option1);
            option2 = PdmConfiguratorCache.CacheFindOption(option1);
          }
        }
        if (option2 != null && option2.OptionGuid != this._selectedOptionHolder.OptionGuid)
          this.cbAvailableOptions.Items.Add((object) new MyElement((object) option2.OptionGuid, option2.OptionCaption, (object) option2));
      }
    }
    finally
    {
      this.cbAvailableOptions.EndUpdate();
    }
  }

  public void Save()
  {
    if (this.localChange)
    {
      if (!this.DisableEdit())
      {
        this.resultCollection.Clear();
        this.SaveIncompatibilityCollection(this._treeList.Nodes, this.resultCollection);
        this._pdmCriterionsCollection.Assign((object) this.resultCollection);
      }
      this.resultCollection.Clear();
      this.localChange = false;
    }
    this.UpdateControls();
  }

  public virtual void Undo()
  {
    this.IsChanged = false;
    this._treeList.ClearNodes();
    if (this._pdmCriterionsCollection != null)
      this._pdmCriterionsCollection.Clear();
    this.resultCollection.Clear();
  }

  protected virtual PdmCriterion SaveCriterion(TreeListNode currentNode)
  {
    ObjectIncompatibilityCriterion incompatibilityCriterion = new ObjectIncompatibilityCriterion();
    incompatibilityCriterion.Option = this._selectedOptionHolder.OptionGuid;
    incompatibilityCriterion.Value = this._selectedOptionValue.ID;
    incompatibilityCriterion.OptionConflict = !(currentNode[(object) "CONFLICT_OPTION"] is MyElement myElement1) ? Guid.Empty : (Guid) myElement1.Value;
    incompatibilityCriterion.ValueConflict = !(currentNode[(object) "CONFLICT_VALUE"] is MyElement myElement2) ? string.Empty : myElement2.Value.ToString();
    incompatibilityCriterion.Operator = !(currentNode[(object) "OPERATION"] is MyElement myElement3) ? Operator.Undefined : (Operator) myElement3.Value;
    LogicalFunction logicalFunction = LogicalFunction.And;
    if (!this.IsLastNode(currentNode))
      logicalFunction = (LogicalFunction) this.NextNode(currentNode).ImageIndex;
    incompatibilityCriterion.Function = logicalFunction;
    incompatibilityCriterion.Not = currentNode[(object) "NotColumnKey"] == (object) "НЕ";
    return (PdmCriterion) incompatibilityCriterion;
  }

  protected bool IsLastNode(TreeListNode node)
  {
    return node.Level == 0 ? this._treeList.Nodes.LastNode.Equals((object) node) : node.ParentNode.Nodes.LastNode.Equals((object) node);
  }

  protected TreeListNode NextNode(TreeListNode node)
  {
    if (this.IsLastNode(node))
      return (TreeListNode) null;
    if (node.Level == 0)
    {
      int num1 = this._treeList.Nodes.IndexOf(node);
      int num2;
      return this._treeList.Nodes[num2 = num1 + 1];
    }
    int num3 = node.ParentNode.Nodes.IndexOf(node);
    int num4;
    return node.ParentNode.Nodes[num4 = num3 + 1];
  }

  private void BarManager_RendererChanged(object sender, EventArgs e)
  {
    IToolBarRenderer renderer = (sender as BarManager).Renderer;
    this.tbOptionWork.Renderer = renderer;
    this.cmsOptionWork.Renderer = renderer;
  }

  private void AddCriterionGroupButtonItem_Click(object sender, EventArgs e)
  {
    this.AddCriterionGroup();
  }

  private void AddCriterionButtonItem_Click(object sender, EventArgs e) => this.AddCriterion();

  private void AddChildCriterionGroupButtonItem_Click(object sender, EventArgs e)
  {
    this.AddChildCriterionGroup();
  }

  private void AddChildCriterionButtonItem_Click(object sender, EventArgs e)
  {
    this.AddChildCriterion();
  }

  private void DeleteButtonItem_Click(object sender, EventArgs e) => this.Delete();

  private void ChangeLogicalOperatorButtonItem_Click(object sender, EventArgs e)
  {
    this.ChangeLogicalOperator();
  }

  private void NotButtonItem_Click(object sender, EventArgs e) => this.Not();

  private void TreeList_AfterDragNode(object sender, NodeEventArgs e)
  {
    if (e.Node == null)
      return;
    if (this.IsFirstNode(e.Node))
      e.Node.ImageIndex = e.Node.SelectImageIndex = 0;
    this._treeList.FocusedNode = e.Node;
    this._treeList.Focus();
    this.IsChanged = true;
  }

  private void TreeList_BeforeFocusNode(object sender, BeforeFocusNodeEventArgs e)
  {
    TreeListNode node = e.Node;
  }

  private void TreeList_CellValueChanging(object sender, CellValueChangedEventArgs e)
  {
    TreeListNode node = e.Node;
    ErrorState errorState = (ErrorState) e.Node.GetValue((object) "OPTION_ERROR");
    if (errorState != ErrorState.None)
    {
      if (e.Column.FieldName == "CONFLICT_OPTION" && errorState == ErrorState.Option)
      {
        node[(object) "OPTION_ERROR"] = (object) ErrorState.None;
        node.StateImageIndex = -1;
      }
      else if (e.Column.FieldName == "CONFLICT_VALUE" && errorState == ErrorState.Value)
      {
        node[(object) "OPTION_ERROR"] = (object) ErrorState.None;
        node.StateImageIndex = -1;
      }
    }
    if (e.Column.FieldName == "CONFLICT_OPTION" && (node[(object) "CONFLICT_OPTION"] == null || !node[(object) "CONFLICT_OPTION"].Equals(e.Value)))
    {
      node[(object) "CONFLICT_VALUE"] = (object) LocalizationHolder.rm.GetString("PdmConfigurator_30");
      this.cbVisibleValue.Items.Clear();
      OptionHolder tag1 = (node[(object) "CONFLICT_OPTION"] is MyElement myElement1 ? myElement1.Tag : (object) null) as OptionHolder;
      OptionHolder tag2 = (e.Value is MyElement myElement2 ? myElement2.Tag : (object) null) as OptionHolder;
      FieldTypes? optionDataType1 = tag1?.OptionDataType;
      FieldTypes? optionDataType2 = tag2?.OptionDataType;
      if (!(optionDataType1.GetValueOrDefault() == optionDataType2.GetValueOrDefault() & optionDataType1.HasValue == optionDataType2.HasValue))
        this.ClearOperation(node);
    }
    this.IsChanged = true;
  }

  private void TreeList_DragDrop(object sender, DragEventArgs e)
  {
    if (this._optionAccessRights != OptionAccessRights.FullAccess)
    {
      e.Effect = DragDropEffects.None;
    }
    else
    {
      if (!(e.Data.GetData(typeof (TreeListNode)) is TreeListNode data))
        return;
      if (this.IsFirstNode(data))
      {
        TreeListNode treeListNode = this.NextNode(data);
        if (treeListNode != null)
          treeListNode.ImageIndex = treeListNode.SelectImageIndex = 0;
      }
      TreeListHitInfo hitInfo = this._treeList.GetHitInfo(this._treeList.PointToClient(new Point(e.X, e.Y)));
      if (hitInfo.Node != null && hitInfo.HitInfoType != HitInfoType.Empty)
        return;
      int num = this._treeList.MoveNode(data, (TreeListNode) null, false) ? 1 : 0;
      e.Effect = DragDropEffects.None;
      if (num == 0)
        return;
      if (this.IsFirstNode(data))
        data.ImageIndex = data.SelectImageIndex = 0;
      this._treeList.FocusedNode = data;
      this.IsChanged = true;
    }
  }

  private void TreeList_DragOver(object sender, DragEventArgs e)
  {
    if (this._optionAccessRights != OptionAccessRights.FullAccess)
    {
      e.Effect = DragDropEffects.None;
    }
    else
    {
      TreeListHitInfo hitInfo = this._treeList.GetHitInfo(this._treeList.PointToClient(new Point(e.X, e.Y)));
      if (hitInfo.Node == null || hitInfo.HitInfoType == HitInfoType.Empty)
        e.Effect = DragDropEffects.Move;
      else if ((PdmCriterionType) hitInfo.Node[(object) "CATEGORY"] == PdmCriterionType.Criterion)
        e.Effect = DragDropEffects.None;
      else
        e.Effect = DragDropEffects.Move;
    }
  }

  private void TreeList_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
  {
    this.UpdateControls();
  }

  private void TreeList_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
  {
    this._focusPath = this.GetLiveFocusPath();
    this.UpdateControls();
    this._treeList.ShowEditor();
  }

  private void TreeList_GetCustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
  {
    if (e.Node == null || e.Node[(object) "CATEGORY"] == null)
      return;
    if ((PdmCriterionType) e.Node[(object) "CATEGORY"] == PdmCriterionType.Criterion)
    {
      if (e.Column.FieldName == "CONFLICT_VALUE")
      {
        e.RepositoryItem = (RepositoryItem) this.cbVisibleValue;
      }
      else
      {
        if (!(e.Column.FieldName == "CONFLICT_OPTION"))
          return;
        e.RepositoryItem = (RepositoryItem) this.cbAvailableOptions;
      }
    }
    else
      e.RepositoryItem = (RepositoryItem) this.teReadOnly;
  }

  private void TreeList_GetCustomNodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
  {
    TreeListNode node = e.Node;
    if (node == null || e.Node[(object) "CATEGORY"] == null || (PdmCriterionType) node[(object) "CATEGORY"] != PdmCriterionType.Collection)
      return;
    if (node.Equals((object) this._treeList.FocusedNode))
      e.Style = this._treeList.Styles["FocusedRow"];
    else
      e.Style = this._treeList.Styles["CollectionStyle"];
  }

  private void TreeList_ShowingEditor(object sender, CancelEventArgs e)
  {
    if (this._optionAccessRights != OptionAccessRights.FullAccess)
    {
      e.Cancel = true;
    }
    else
    {
      TreeListNode focusedNode = this._treeList.FocusedNode;
      if (focusedNode != null && focusedNode[(object) "CATEGORY"] != null)
      {
        PdmCriterionType pdmCriterionType = (PdmCriterionType) focusedNode[(object) "CATEGORY"];
        if (pdmCriterionType != PdmCriterionType.Criterion)
        {
          e.Cancel = true;
          return;
        }
        if (pdmCriterionType == PdmCriterionType.Criterion && this._treeList.FocusedColumn.FieldName == "CONFLICT_VALUE")
        {
          object obj = focusedNode.GetValue((object) "CONFLICT_OPTION");
          this.cbVisibleValue.BeginUpdate();
          try
          {
            if (obj != null)
            {
              if (obj is MyElement myElement && !myElement.Value.Equals((object) Guid.Empty))
              {
                Guid guid = (Guid) myElement.Value;
                OptionHolder option = PdmConfiguratorCache.CacheFindOption(guid);
                if ((ErrorState) focusedNode.GetValue((object) "OPTION_ERROR") == ErrorState.Option)
                {
                  e.Cancel = true;
                  return;
                }
                this.cbVisibleValue.Items.Clear();
                OptionValuesCollection optionValues = option.OptionValues;
                List<string> list = (this._objectOptionsHolder.VisibleOptionValues.Items.ContainsKey(guid) ? (IEnumerable<string>) this._objectOptionsHolder.VisibleOptionValues.Items[guid] : (IEnumerable<string>) new List<string>(0)).OrderBy<string, int>((Func<string, int>) (o => optionValues.IndexOf(optionValues.FindValue(o)))).ToList<string>();
                if (list.Count == 0)
                {
                  foreach (OptionValue optionValue in optionValues.ToArray())
                    this.cbVisibleValue.Items.Add((object) new MyElement()
                    {
                      Caption = this.GetOptionValueCaption(optionValue, option),
                      Tag = (object) optionValue,
                      Value = (object) optionValue.ID
                    });
                }
                else
                {
                  foreach (string str in list)
                  {
                    OptionValue optionValue = optionValues.FindValue(str);
                    this.cbVisibleValue.Items.Add((object) new MyElement()
                    {
                      Caption = this.GetOptionValueCaption(optionValue, option),
                      Tag = (object) optionValue,
                      Value = (object) optionValue.ID
                    });
                  }
                }
              }
              else
                e.Cancel = true;
            }
            else
              e.Cancel = true;
          }
          finally
          {
            this.cbVisibleValue.EndUpdate();
          }
        }
      }
      if (!(focusedNode[(object) "CONFLICT_OPTION"] is MyElement myElement1) || !(myElement1.Tag is OptionHolder))
        return;
      OptionHolder tag = (OptionHolder) myElement1.Tag;
      if (tag.OptionDataType == FieldTypes.ftBoolean)
        this._treeList.Columns["OPERATION"].ColumnEdit = (RepositoryItem) this._booleanOperatorsRepositoryItemComboBox;
      else if (tag.OptionDataType == FieldTypes.ftString)
        this._treeList.Columns["OPERATION"].ColumnEdit = (RepositoryItem) this._stringOperatorsRepositoryItemComboBox;
      else
        this._treeList.Columns["OPERATION"].ColumnEdit = (RepositoryItem) this.cbOperators;
    }
  }

  private void AddCriterionGroup() => this.AddCriterionNode(PdmCriterionType.Collection, false);

  private void AddCriterion() => this.AddCriterionNode(PdmCriterionType.Criterion, false);

  private void AddChildCriterionGroup() => this.AddCriterionNode(PdmCriterionType.Collection, true);

  private void AddChildCriterion() => this.AddCriterionNode(PdmCriterionType.Criterion, true);

  private void Delete()
  {
    this.tbOptionWork.Focus();
    TreeListNode focusedNode = this._treeList.FocusedNode;
    if (focusedNode == null)
      return;
    if (this.IsFirstNode(focusedNode))
    {
      TreeListNode treeListNode = this.NextNode(focusedNode);
      if (treeListNode != null)
        treeListNode.ImageIndex = treeListNode.SelectImageIndex = 0;
    }
    this._treeList.Nodes.Remove(focusedNode);
    this._treeList.Focus();
    this.IsChanged = true;
  }

  private void ChangeLogicalOperator()
  {
    this.tbOptionWork.Focus();
    TreeListNode focusedNode = this._treeList.FocusedNode;
    if (focusedNode == null)
      return;
    focusedNode.SelectImageIndex = focusedNode.ImageIndex = focusedNode.SelectImageIndex == 0 ? 1 : 0;
    this._treeList.Focus();
    this.IsChanged = true;
  }

  private void Not()
  {
    this._treeList.FocusedNode[(object) "NotColumnKey"] = this._treeList.FocusedNode[(object) "NotColumnKey"] == (object) "НЕ" ? (object) string.Empty : (object) "НЕ";
    this.IsChanged = true;
  }

  private void OnChanged()
  {
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, EventArgs.Empty);
  }

  private string GetOptionValueCaption(OptionValue optionValue, OptionHolder optionHolder)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (!string.IsNullOrEmpty(optionValue.Code))
      stringBuilder.AppendFormat("[{0}] ", (object) optionValue.Code);
    stringBuilder.Append(optionValue.GetDisplayValue(optionHolder));
    if (!string.IsNullOrEmpty(optionValue.Description))
      stringBuilder.AppendFormat(" ({0})", (object) optionValue.Description);
    return stringBuilder.ToString();
  }

  private bool EnabledChildAddition(TreeListNode selNode)
  {
    return selNode != null && selNode[(object) "CATEGORY"] != null && (PdmCriterionType) selNode[(object) "CATEGORY"] == PdmCriterionType.Collection;
  }

  private void InitRepositories()
  {
    this.InitAvailableOptions();
    this._treeList.Columns["CONFLICT_OPTION"].ColumnEdit = (RepositoryItem) this.cbAvailableOptions;
    this.InitOperators();
    this._treeList.Columns["OPERATION"].ColumnEdit = (RepositoryItem) this.cbOperators;
    this.cbVisibleValue.TextEditStyle = TextEditStyles.DisableTextEditor;
    if (this.cbVisibleValue.Buttons.Count > 0)
      this.cbVisibleValue.Buttons[0].Width = 14;
    this.cbVisibleValue.DropDownRows = 15;
    this._treeList.Columns["CONFLICT_VALUE"].ColumnEdit = (RepositoryItem) this.cbVisibleValue;
    this.teReadOnly = new RepositoryItemTextEdit();
    this.teReadOnly.ReadOnly = true;
  }

  private void InitOperators()
  {
    this.cbOperators.TextEditStyle = this._booleanOperatorsRepositoryItemComboBox.TextEditStyle = this._stringOperatorsRepositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
    if (this.cbAvailableOptions.Buttons.Count > 0)
      this.cbOperators.Buttons[0].Width = this._booleanOperatorsRepositoryItemComboBox.Buttons[0].Width = this._stringOperatorsRepositoryItemComboBox.Buttons[0].Width = 14;
    this.cbOperators.BeginUpdate();
    try
    {
      this.cbOperators.Items.Clear();
      foreach (int num in Enum.GetValues(typeof (Operator)))
      {
        Operator @operator = (Operator) num;
        if (@operator != Operator.Undefined)
        {
          string enumDescription = EnumDescConverter.GetEnumDescription((Enum) @operator);
          this.cbOperators.Items.Add((object) new MyElement((object) @operator, enumDescription, (object) null));
        }
      }
    }
    finally
    {
      this.cbOperators.EndUpdate();
    }
    this._booleanOperatorsRepositoryItemComboBox.BeginUpdate();
    try
    {
      this._booleanOperatorsRepositoryItemComboBox.MaskData.BeepOnError = this.cbOperators.MaskData.BeepOnError;
      this._booleanOperatorsRepositoryItemComboBox.MaskData.Blank = this.cbOperators.MaskData.Blank;
      this._booleanOperatorsRepositoryItemComboBox.MaskData.EditMask = this.cbOperators.MaskData.EditMask;
      this._booleanOperatorsRepositoryItemComboBox.MaskData.IgnoreMaskBlank = this.cbOperators.MaskData.IgnoreMaskBlank;
      this._booleanOperatorsRepositoryItemComboBox.MaskData.MaskType = this.cbOperators.MaskData.MaskType;
      this._booleanOperatorsRepositoryItemComboBox.MaskData.SaveLiteral = this.cbOperators.MaskData.SaveLiteral;
      this._booleanOperatorsRepositoryItemComboBox.Items.Clear();
      this._booleanOperatorsRepositoryItemComboBox.Items.AddRange(new object[2]
      {
        (object) new MyElement((object) Operator.Equals, EnumDescConverter.GetEnumDescription((Enum) Operator.Equals), (object) null),
        (object) new MyElement((object) Operator.NotEquals, EnumDescConverter.GetEnumDescription((Enum) Operator.NotEquals), (object) null)
      });
    }
    finally
    {
      this._booleanOperatorsRepositoryItemComboBox.EndUpdate();
    }
    this._stringOperatorsRepositoryItemComboBox.BeginUpdate();
    try
    {
      this._stringOperatorsRepositoryItemComboBox.MaskData.BeepOnError = this.cbOperators.MaskData.BeepOnError;
      this._stringOperatorsRepositoryItemComboBox.MaskData.Blank = this.cbOperators.MaskData.Blank;
      this._stringOperatorsRepositoryItemComboBox.MaskData.EditMask = this.cbOperators.MaskData.EditMask;
      this._stringOperatorsRepositoryItemComboBox.MaskData.IgnoreMaskBlank = this.cbOperators.MaskData.IgnoreMaskBlank;
      this._stringOperatorsRepositoryItemComboBox.MaskData.MaskType = this.cbOperators.MaskData.MaskType;
      this._stringOperatorsRepositoryItemComboBox.MaskData.SaveLiteral = this.cbOperators.MaskData.SaveLiteral;
      this._stringOperatorsRepositoryItemComboBox.Items.Clear();
      this._stringOperatorsRepositoryItemComboBox.Items.AddRange(new object[2]
      {
        (object) new MyElement((object) Operator.Equals, EnumDescConverter.GetEnumDescription((Enum) Operator.Equals), (object) null),
        (object) new MyElement((object) Operator.NotEquals, EnumDescConverter.GetEnumDescription((Enum) Operator.NotEquals), (object) null)
      });
    }
    finally
    {
      this._stringOperatorsRepositoryItemComboBox.EndUpdate();
    }
  }

  private void FillCriterionsTree(TreeListNode parentNode, IPdmCriterion parent)
  {
    if (!(parent is PdmCriterionsCollection criterionsCollection))
      return;
    for (int index = 0; index < criterionsCollection.Count; ++index)
    {
      IPdmCriterion pdmCriterion1 = criterionsCollection[index];
      TreeListNode parentNode1;
      PdmCriterionsCollection parent1;
      if (pdmCriterion1.CriterionType == PdmCriterionType.Criterion)
      {
        ObjectIncompatibilityCriterion incompatibilityCriterion = pdmCriterion1 as ObjectIncompatibilityCriterion;
        PdmCriterion pdmCriterion2 = pdmCriterion1 as PdmCriterion;
        MyElement myElement1 = new MyElement();
        myElement1.Caption = EnumDescConverter.GetEnumDescription((Enum) pdmCriterion2.Operator);
        myElement1.Value = (object) pdmCriterion2.Operator;
        MyElement myElement2 = new MyElement();
        Guid guid = incompatibilityCriterion != null ? incompatibilityCriterion.OptionConflict : pdmCriterion2.Option;
        OptionHolder option = PdmConfiguratorCache.CacheFindOption(guid);
        if (option == null)
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, guid);
            option = PdmConfiguratorCache.CacheFindOption(guid);
          }
        }
        myElement2.Caption = option != null ? option.OptionCaption : LocalizationHolder.rm.GetString("PdmConfigurator_29");
        myElement2.Value = (object) (option != null ? option.OptionGuid : Guid.Empty);
        myElement2.Tag = (object) option;
        MyElement myElement3 = new MyElement();
        string str = incompatibilityCriterion != null ? incompatibilityCriterion.ValueConflict : pdmCriterion2.Value;
        OptionValue optionValue = option?.OptionValues.FindValue(str);
        myElement3.Caption = optionValue == null ? LocalizationHolder.rm.GetString("PdmConfigurator_30") : this.GetOptionValueCaption(optionValue, option);
        myElement3.Value = optionValue != null ? (object) optionValue.ID : (object) string.Empty;
        myElement3.Tag = (object) optionValue;
        LogicalFunction logicalFunction = LogicalFunction.And;
        if (index > 0)
          logicalFunction = criterionsCollection[index - 1].Function;
        parentNode1 = this._treeList.AppendNode((object) new object[6]
        {
          null,
          null,
          null,
          (object) PdmCriterionType.Criterion,
          (object) ErrorState.None,
          (object) string.Empty
        }, parentNode);
        parentNode1[(object) "CONFLICT_OPTION"] = (object) myElement2;
        parentNode1[(object) "OPERATION"] = (object) myElement1;
        parentNode1[(object) "CONFLICT_VALUE"] = (object) myElement3;
        ErrorState errorState = this.CheckError(option, optionValue);
        switch (errorState)
        {
          case ErrorState.None:
label_16:
            parentNode1.ImageIndex = parentNode1.SelectImageIndex = (int) logicalFunction;
            parentNode1[(object) "NotColumnKey"] = pdmCriterion1.Not ? (object) "НЕ" : (object) string.Empty;
            parent1 = pdmCriterion2.Items;
            goto label_22;
          case ErrorState.Value:
          case ErrorState.ObsoleteOption:
            parentNode1.StateImageIndex = 9;
            break;
          case ErrorState.ObsoleteOptionValue:
            parentNode1.StateImageIndex = 11;
            break;
          default:
            parentNode1.StateImageIndex = 10;
            break;
        }
        parentNode1[(object) "OPTION_ERROR"] = (object) errorState;
        goto label_16;
      }
      parent1 = pdmCriterion1 as PdmCriterionsCollection;
      LogicalFunction logicalFunction1 = LogicalFunction.And;
      if (index > 0)
        logicalFunction1 = criterionsCollection[index - 1].Function;
      parentNode1 = this._treeList.AppendNode((object) new object[6]
      {
        (object) LocalizationHolder.rm.GetString("PdmConfigurator_31"),
        null,
        null,
        (object) PdmCriterionType.Collection,
        (object) ErrorState.None,
        (object) string.Empty
      }, parentNode);
      if (index > 0)
        parentNode1.ImageIndex = parentNode1.SelectImageIndex = (int) logicalFunction1;
      parentNode1[(object) "NotColumnKey"] = parent1.Not ? (object) "НЕ" : (object) string.Empty;
label_22:
      this.FillCriterionsTree(parentNode1, (IPdmCriterion) parent1);
      parentNode1.Expanded = true;
    }
  }

  private ErrorState CheckError(OptionHolder conflictOption, OptionValue conflictValue)
  {
    if (conflictOption == null)
      return ErrorState.None;
    if (!this._objectOptionsHolder.Options.Contains(conflictOption.OptionObjectID))
      return ErrorState.Option;
    if (conflictValue == null)
      return ErrorState.None;
    if ((conflictOption.OptionFlags & OptionFlags.Obsolete) == OptionFlags.Obsolete)
      return ErrorState.ObsoleteOption;
    if ((conflictValue.Flags & OptionValueFlags.Obsolete) == OptionValueFlags.Obsolete)
      return ErrorState.ObsoleteOptionValue;
    List<string> stringList = this._objectOptionsHolder.VisibleOptionValues.Items[conflictOption.OptionGuid];
    return stringList.Count != 0 && !stringList.Contains(conflictValue.ID) ? ErrorState.Value : ErrorState.None;
  }

  private void ClearOperation(TreeListNode criterionNode)
  {
    criterionNode[(object) "OPERATION"] = (object) LocalizationHolder.rm.GetString("PdmConfigurator_32");
  }

  private void AddCriterionNode(PdmCriterionType criterionType, bool isChild)
  {
    this.tbOptionWork.Focus();
    TreeListNode parentNode = this._treeList.FocusedNode;
    if (!isChild)
      parentNode = parentNode == null ? (TreeListNode) null : parentNode.ParentNode;
    TreeListNode criterionNode = this._treeList.AppendNode((object) new object[6]
    {
      null,
      null,
      null,
      (object) criterionType,
      (object) ErrorState.None,
      (object) string.Empty
    }, parentNode);
    if (criterionNode == null)
      return;
    if (criterionType == PdmCriterionType.Collection)
    {
      criterionNode[(object) "CONFLICT_OPTION"] = (object) LocalizationHolder.rm.GetString("PdmConfigurator_31");
    }
    else
    {
      criterionNode[(object) "CONFLICT_OPTION"] = (object) LocalizationHolder.rm.GetString("PdmConfigurator_29");
      this.ClearOperation(criterionNode);
      criterionNode[(object) "CONFLICT_VALUE"] = (object) LocalizationHolder.rm.GetString("PdmConfigurator_30");
    }
    criterionNode.ImageIndex = criterionNode.SelectImageIndex = 0;
    this._treeList.FocusedNode = criterionNode;
    this._treeList.Focus();
    this.IsChanged = true;
  }

  private void SaveIncompatibilityCollection(
    TreeListNodes nodes,
    PdmCriterionsCollection parentCollection)
  {
    for (int index = 0; index < nodes.Count; ++index)
    {
      TreeListNode node = nodes[index];
      if ((PdmCriterionType) node[(object) "CATEGORY"] == PdmCriterionType.Collection)
      {
        IPdmCriterion parentCollection1 = (IPdmCriterion) new PdmCriterionsCollection();
        LogicalFunction logicalFunction = LogicalFunction.And;
        if (index < nodes.Count - 1)
          logicalFunction = (LogicalFunction) nodes[index + 1].ImageIndex;
        parentCollection1.Function = logicalFunction;
        parentCollection1.Not = nodes[index][(object) "NotColumnKey"] == (object) "НЕ";
        parentCollection.Add(parentCollection1);
        this.SaveIncompatibilityCollection(node.Nodes, parentCollection1 as PdmCriterionsCollection);
      }
      else
      {
        PdmCriterion pdmCriterion = this.SaveCriterion(node);
        parentCollection.Add((IPdmCriterion) pdmCriterion);
        this.SaveIncompatibilityCollection(node.Nodes, pdmCriterion.Items);
      }
    }
  }

  private string CreateHint()
  {
    string empty = string.Empty;
    foreach (TreeListNode node in this._treeList.Nodes)
      empty += this.CreateHintForNode(node, this._treeList.Nodes.Count == 1);
    return empty;
  }

  private string CreateHintForNode(TreeListNode treeListNode, bool isSingleNode)
  {
    string hintForNode = string.Empty;
    ApplicationConditionsDisplaySettings settings = CompositionsConfiguratorConfigurationOptions.ApplicationConditionsDisplaySettings ?? new ApplicationConditionsDisplaySettings();
    if (treeListNode != null && treeListNode[(object) "CATEGORY"] != null)
    {
      string str1 = string.Empty;
      if (!this.IsFirstNode(treeListNode))
        str1 = treeListNode.ImageIndex != 0 ? LocalizationHolder.rm.GetString("PdmConfigurator_34") : LocalizationHolder.rm.GetString("PdmConfigurator_33");
      if ((PdmCriterionType) treeListNode[(object) "CATEGORY"] == PdmCriterionType.Collection)
      {
        if (treeListNode.Nodes.Count > 0)
        {
          string empty = string.Empty;
          foreach (TreeListNode node in treeListNode.Nodes)
            empty += this.CreateHintForNode(node, treeListNode.Nodes.Count == 1);
          string str2 = !(empty != string.Empty) || isSingleNode ? empty : $"({empty})";
          hintForNode = !object.Equals(treeListNode[(object) "NotColumnKey"], (object) "НЕ") ? hintForNode + str1 + str2 : (!(str2 != string.Empty) || isSingleNode ? $"{str1}НЕ({str2})" : $"{str1}НЕ{str2}");
          treeListNode[(object) "CONFLICT_VALUE"] = (object) hintForNode;
        }
        else
          treeListNode[(object) "CONFLICT_VALUE"] = (object) string.Empty;
      }
      else
      {
        MyElement myElement1 = treeListNode[(object) "CONFLICT_OPTION"] as MyElement;
        string str3 = string.Empty;
        OptionHolder optionHolder = (OptionHolder) null;
        if (myElement1 != null)
        {
          Guid option = (Guid) myElement1.Value;
          optionHolder = PdmConfiguratorCache.CacheFindOption(option);
          str3 = optionHolder == null ? string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_35"), (object) option) : CompositionsConfiguratorHelper.GetOptionNameReplacemenetForDisplayApplicationConditions(optionHolder, settings);
        }
        MyElement myElement2 = treeListNode[(object) "CONFLICT_VALUE"] as MyElement;
        string str4 = string.Empty;
        if (myElement2 != null)
        {
          OptionValue tag = myElement2.Tag as OptionValue;
          string str5 = myElement2.Value.ToString();
          str4 = optionHolder == null || tag == null ? string.Format(LocalizationHolder.rm.GetString("PdmConfigurator_36"), (object) str5) : CompositionsConfiguratorHelper.GetOptionValueReplacementForDisplayApplicationConditions(optionHolder, tag, settings);
        }
        string str6 = !(treeListNode[(object) "OPERATION"] is MyElement myElement3) ? string.Empty : CompositionsConfiguratorHelper.GetOperatorForDisplayApplicationConditions((Operator) myElement3.Value, settings);
        string str7 = $"{str3} {str6} {str4}";
        string str8 = isSingleNode ? str7 : $"({str7})";
        if (object.Equals(treeListNode[(object) "NotColumnKey"], (object) "НЕ"))
          str8 = isSingleNode ? $"НЕ({str8})" : "НЕ" + str8;
        hintForNode = str1 + str8;
      }
    }
    return hintForNode;
  }

  private bool IsFirstNode(TreeListNode node)
  {
    return node.Level == 0 ? this._treeList.Nodes != null && this._treeList.Nodes.FirstNode.Equals((object) node) : node.ParentNode.Nodes.FirstNode != null && node.ParentNode.Nodes.FirstNode.Equals((object) node);
  }

  private void UpdateColumnOptionsForNode(TreeListNode node)
  {
    if (node[(object) "CATEGORY"] == null)
      return;
    if ((PdmCriterionType) node[(object) "CATEGORY"] == PdmCriterionType.Collection)
    {
      foreach (TreeListColumn column in (CollectionBase) this._treeList.Columns)
        column.Options &= ~ColumnOptions.CanFocused;
    }
    else
    {
      foreach (TreeListColumn column in (CollectionBase) this._treeList.Columns)
      {
        if (column.FieldName != "NotColumnKey")
          column.Options |= ColumnOptions.CanFocused;
      }
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.tbOptionWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.cmsOptionWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged -= new EventHandler(this.BarManager_RendererChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IncompatibilityEditor));
    ImageComboBoxItem imageComboBoxItem1 = new ImageComboBoxItem();
    ImageComboBoxItem imageComboBoxItem2 = new ImageComboBoxItem();
    this._treeList = new TreeList();
    this.colConflictOption = new TreeListColumn();
    this.colOperation = new TreeListColumn();
    this.colConflictValue = new TreeListColumn();
    this.colCategory = new TreeListColumn();
    this.colOptionError = new TreeListColumn();
    this.cellEditor = new RepositoryItemTextEdit();
    this.ilOptions = new ImageList(this.components);
    this.repositoryItemImageComboBox2 = new RepositoryItemImageComboBox();
    this.repositoryItemComboBox1 = new RepositoryItemComboBox();
    this.tbOptionWork = new Intermech.Bars.ToolBar();
    this._addCriterionGroupButtonItem = new ButtonItem();
    this._addCriterionButtonItem = new ButtonItem();
    this._deleteButtonItem = new ButtonItem();
    this._addChildCriterionGroupButtonItem = new ButtonItem();
    this._addChildCriterionButtonItem = new ButtonItem();
    this._changeLogicalOperatorButtonItem = new ButtonItem();
    this._notButtonItem = new ButtonItem();
    this.toolTip1 = new ToolTip(this.components);
    this.cmsOptionWork = new MenuBar();
    this.menuBarItem1 = new MenuBarItem();
    this._deleteMenuButtonItem = new MenuButtonItem();
    this._addCriterionGroupMenuButtonItem = new MenuButtonItem();
    this._addCriterionMenuButtonItem = new MenuButtonItem();
    this._addChildCriterionGroupMenuButtonItem = new MenuButtonItem();
    this._addChildCriterionMenuButtonItem = new MenuButtonItem();
    this._changeLogicalOperatorMenuButtonItem = new MenuButtonItem();
    this.contextMenuBarItem = new ContextMenuBarItem();
    this.tsmAddOption = new MenuButtonItem();
    this.tsmDeleteOption = new MenuButtonItem();
    this.tsmClear = new MenuButtonItem();
    this.pbError = new PictureBox();
    this.errorPanel = new Panel();
    this.lbErrorState = new Label();
    this.panelHint = new Panel();
    this.labelWarning = new Label();
    this.pictureHint = new PictureBox();
    this._hintTextBox = new TextBox();
    this.ilError = new ImageList(this.components);
    this._treeList.BeginInit();
    this.cellEditor.BeginInit();
    this.repositoryItemImageComboBox2.BeginInit();
    this.repositoryItemComboBox1.BeginInit();
    ((ISupportInitialize) this.pbError).BeginInit();
    this.errorPanel.SuspendLayout();
    this.panelHint.SuspendLayout();
    ((ISupportInitialize) this.pictureHint).BeginInit();
    this.SuspendLayout();
    this._treeList.AllowDrop = true;
    componentResourceManager.ApplyResources((object) this._treeList, "_treeList");
    this._treeList.Columns.AddRange(new TreeListColumn[5]
    {
      this.colConflictOption,
      this.colOperation,
      this.colConflictValue,
      this.colCategory,
      this.colOptionError
    });
    this._treeList.IndicatorWidth = 4;
    this._treeList.Name = "tlOptionsEditor";
    this.cmsOptionWork.SetPopupMenu((Control) this._treeList, this.menuBarItem1);
    this._treeList.RepositoryItems.AddRange(new RepositoryItem[1]
    {
      (RepositoryItem) this.cellEditor
    });
    this._treeList.SelectImageList = this.ilOptions;
    this._treeList.StateImageList = this.ilOptions;
    this._treeList.Styles.AddReplace("EvenRow", (object) new ViewStyle("EvenRow", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.None, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, Color.LightSkyBlue, SystemColors.WindowText));
    this._treeList.Styles.AddReplace("Preview", (object) new ViewStyle("Preview", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, true, false, HorzAlignment.Near, VertAlignment.Center, (Image) null, SystemColors.Window, Color.Blue));
    this._treeList.Styles.AddReplace("HideSelectionRow", (object) new ViewStyle("HideSelectionRow", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.InactiveCaption, SystemColors.InactiveCaptionText));
    this._treeList.Styles.AddReplace("PressedColumn", (object) new ViewStyle("PressedColumn", "TreeList", new Font("Tahoma", 8.25f), "HeaderPanel", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseForeColor, true, false, false, HorzAlignment.Near, VertAlignment.Center, (Image) null, SystemColors.ControlDark, SystemColors.ControlLightLight));
    this._treeList.Styles.AddReplace("OddRow", (object) new ViewStyle("OddRow", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.None, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, Color.LightSalmon, SystemColors.WindowText));
    this._treeList.Styles.AddReplace("Row", (object) new ViewStyle("Row", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.WindowText));
    this._treeList.Styles.AddReplace("Empty", (object) new ViewStyle("Empty", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.Window));
    this._treeList.Styles.AddReplace("GroupFooter", (object) new ViewStyle("GroupFooter", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Far, VertAlignment.Center, (Image) null, SystemColors.Control, SystemColors.ControlText));
    this._treeList.Styles.AddReplace("HorzLine", (object) new ViewStyle("HorzLine", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Control, SystemColors.Control));
    this._treeList.Styles.AddReplace("FocusedRow", (object) new ViewStyle("FocusedRow", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Highlight, SystemColors.HighlightText));
    this._treeList.Styles.AddReplace("TreeLine", (object) new ViewStyle("TreeLine", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.ControlDark));
    this._treeList.Styles.AddReplace("GroupButton", (object) new ViewStyle("GroupButton", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Control, SystemColors.ControlText));
    this._treeList.Styles.AddReplace("FocusedCell", (object) new ViewStyle("FocusedCell", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.WindowText));
    this._treeList.Styles.AddReplace("VertLine", (object) new ViewStyle("VertLine", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Near, VertAlignment.Center, (Image) null, SystemColors.Control, SystemColors.Control));
    this._treeList.Styles.AddReplace("CollectionStyle", (object) new ViewStyle("CollectionStyle", "", new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "Row", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawEndEllipsis | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseHorzAlignment | StyleOptions.UseImage | StyleOptions.UseWordWrap | StyleOptions.UseVertAlignment, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, Color.Azure, SystemColors.ControlText));
    this._treeList.Styles.AddReplace("VisibleError", (object) new ViewStyle("VisibleError", "", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawEndEllipsis | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseHorzAlignment | StyleOptions.UseImage | StyleOptions.UseWordWrap | StyleOptions.UseVertAlignment, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, Color.SeaShell, Color.SeaShell));
    this._treeList.Styles.AddReplace("SelectedRow", (object) new ViewStyle("SelectedRow", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled | StyleOptions.UseBackColor | StyleOptions.UseDrawFocusRect | StyleOptions.UseFont | StyleOptions.UseForeColor | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Highlight, SystemColors.HighlightText));
    this._treeList.Styles.AddReplace("HeaderPanel", (object) new ViewStyle("HeaderPanel", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Near, VertAlignment.Center, (Image) null, SystemColors.Control, SystemColors.ControlText));
    this._treeList.Styles.AddReplace("FooterPanel", (object) new ViewStyle("FooterPanel", "TreeList", new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Far, VertAlignment.Center, (Image) null, SystemColors.Control, SystemColors.ControlText));
    this._treeList.TreeLineStyle = LineStyle.None;
    this._treeList.GetCustomNodeCellEdit += new GetCustomNodeCellEditEventHandler(this.TreeList_GetCustomNodeCellEdit);
    this._treeList.GetCustomNodeCellStyle += new GetCustomNodeCellStyleEventHandler(this.TreeList_GetCustomNodeCellStyle);
    this._treeList.BeforeFocusNode += new BeforeFocusNodeEventHandler(this.TreeList_BeforeFocusNode);
    this._treeList.AfterDragNode += new NodeEventHandler(this.TreeList_AfterDragNode);
    this._treeList.FocusedNodeChanged += new FocusedNodeChangedEventHandler(this.TreeList_FocusedNodeChanged);
    this._treeList.FocusedColumnChanged += new FocusedColumnChangedEventHandler(this.TreeList_FocusedColumnChanged);
    this._treeList.CellValueChanging += new CellValueChangedEventHandler(this.TreeList_CellValueChanging);
    this._treeList.ShowingEditor += new CancelEventHandler(this.TreeList_ShowingEditor);
    this._treeList.DragDrop += new DragEventHandler(this.TreeList_DragDrop);
    this._treeList.DragOver += new DragEventHandler(this.TreeList_DragOver);
    componentResourceManager.ApplyResources((object) this.colConflictOption, "colConflictOption");
    this.colConflictOption.Name = "colConflictOption";
    componentResourceManager.ApplyResources((object) this.colOperation, "colOperation");
    this.colOperation.Name = "colOperation";
    componentResourceManager.ApplyResources((object) this.colConflictValue, "colConflictValue");
    this.colConflictValue.Name = "colConflictValue";
    componentResourceManager.ApplyResources((object) this.colCategory, "colCategory");
    this.colCategory.Name = "colCategory";
    componentResourceManager.ApplyResources((object) this.colOptionError, "colOptionError");
    this.colOptionError.Name = "colOptionError";
    this.cellEditor.AutoHeight = false;
    this.cellEditor.Name = "cellEditor";
    this.cellEditor.Style = new ViewStyle("ControlStyle", (string) null, new Font("Tahoma", 8.25f), "", StyleOptions.StyleEnabled, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.WindowText);
    this.ilOptions.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilOptions.ImageStream");
    this.ilOptions.TransparentColor = Color.Transparent;
    this.ilOptions.Images.SetKeyName(0, "and.ico");
    this.ilOptions.Images.SetKeyName(1, "Or.ico");
    this.ilOptions.Images.SetKeyName(2, "delete2.png");
    this.ilOptions.Images.SetKeyName(3, "branch_add.png");
    this.ilOptions.Images.SetKeyName(4, "document_add.png");
    this.ilOptions.Images.SetKeyName(5, "revert.ico");
    this.ilOptions.Images.SetKeyName(6, "Add.ico");
    this.ilOptions.Images.SetKeyName(7, "add_group.ico");
    this.ilOptions.Images.SetKeyName(8, "add_task.png");
    this.ilOptions.Images.SetKeyName(9, "gear_warning.png");
    this.ilOptions.Images.SetKeyName(10, "error.gif");
    this.ilOptions.Images.SetKeyName(11, "garbage.png");
    this.repositoryItemImageComboBox2.AutoHeight = false;
    imageComboBoxItem1.ImageIndex = 0;
    imageComboBoxItem1.Value = (object) 0;
    imageComboBoxItem2.ImageIndex = 1;
    imageComboBoxItem2.Value = (object) 1;
    this.repositoryItemImageComboBox2.Items.AddRange(new ImageComboBoxItem[2]
    {
      imageComboBoxItem1,
      imageComboBoxItem2
    });
    this.repositoryItemImageComboBox2.LargeImages = this.ilOptions;
    this.repositoryItemImageComboBox2.Name = "repositoryItemImageComboBox2";
    this.repositoryItemComboBox1.AutoHeight = false;
    this.repositoryItemComboBox1.Items.AddRange(new object[6]
    {
      (object) componentResourceManager.GetString("repositoryItemComboBox1.Items"),
      (object) componentResourceManager.GetString("repositoryItemComboBox1.Items1"),
      (object) componentResourceManager.GetString("repositoryItemComboBox1.Items2"),
      (object) componentResourceManager.GetString("repositoryItemComboBox1.Items3"),
      (object) componentResourceManager.GetString("repositoryItemComboBox1.Items4"),
      (object) componentResourceManager.GetString("repositoryItemComboBox1.Items5")
    });
    this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
    this.repositoryItemComboBox1.TextEditStyle = TextEditStyles.DisableTextEditor;
    this.tbOptionWork.FullMenus = true;
    this.tbOptionWork.Guid = new Guid("37056402-c6d1-47d4-be0f-e941c1a06e55");
    this.tbOptionWork.Hidden = false;
    this.tbOptionWork.ImageList = this.ilOptions;
    this.tbOptionWork.Items.AddRange(new ToolbarItemBase[7]
    {
      (ToolbarItemBase) this._addCriterionGroupButtonItem,
      (ToolbarItemBase) this._addCriterionButtonItem,
      (ToolbarItemBase) this._deleteButtonItem,
      (ToolbarItemBase) this._addChildCriterionGroupButtonItem,
      (ToolbarItemBase) this._addChildCriterionButtonItem,
      (ToolbarItemBase) this._changeLogicalOperatorButtonItem,
      (ToolbarItemBase) this._notButtonItem
    });
    componentResourceManager.ApplyResources((object) this.tbOptionWork, "tbOptionWork");
    this.tbOptionWork.Name = "tbOptionWork";
    this.tbOptionWork.Tag = (object) "";
    this._addCriterionGroupButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._addCriterionGroupButtonItem, "_addCriterionGroupButtonItem");
    this._addCriterionGroupButtonItem.ImageIndex = 3;
    this._addCriterionGroupButtonItem.Click += new EventHandler(this.AddCriterionGroupButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._addCriterionButtonItem, "_addCriterionButtonItem");
    this._addCriterionButtonItem.ImageIndex = 4;
    this._addCriterionButtonItem.Click += new EventHandler(this.AddCriterionButtonItem_Click);
    this._deleteButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._deleteButtonItem, "_deleteButtonItem");
    this._deleteButtonItem.Enabled = false;
    this._deleteButtonItem.ImageIndex = 2;
    this._deleteButtonItem.Click += new EventHandler(this.DeleteButtonItem_Click);
    this._addChildCriterionGroupButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._addChildCriterionGroupButtonItem, "_addChildCriterionGroupButtonItem");
    this._addChildCriterionGroupButtonItem.Enabled = false;
    this._addChildCriterionGroupButtonItem.ImageIndex = 7;
    this._addChildCriterionGroupButtonItem.Click += new EventHandler(this.AddChildCriterionGroupButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._addChildCriterionButtonItem, "_addChildCriterionButtonItem");
    this._addChildCriterionButtonItem.Enabled = false;
    this._addChildCriterionButtonItem.ImageIndex = 8;
    this._addChildCriterionButtonItem.Click += new EventHandler(this.AddChildCriterionButtonItem_Click);
    this._changeLogicalOperatorButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._changeLogicalOperatorButtonItem, "_changeLogicalOperatorButtonItem");
    this._changeLogicalOperatorButtonItem.ImageIndex = 5;
    this._changeLogicalOperatorButtonItem.Click += new EventHandler(this.ChangeLogicalOperatorButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._notButtonItem, "_notButtonItem");
    this._notButtonItem.IconSize = new Size(8, 8);
    this._notButtonItem.Image = (Image) componentResourceManager.GetObject("_notButtonItem.Image");
    this._notButtonItem.Click += new EventHandler(this.NotButtonItem_Click);
    componentResourceManager.ApplyResources((object) this.cmsOptionWork, "cmsOptionWork");
    this.cmsOptionWork.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.cmsOptionWork.Hidden = false;
    this.cmsOptionWork.ImageList = this.ilOptions;
    this.cmsOptionWork.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.contextMenuBarItem,
      (ToolbarItemBase) this.menuBarItem1
    });
    this.cmsOptionWork.Name = "cmsOptionWork";
    this.cmsOptionWork.OwnerForm = (Form) null;
    componentResourceManager.ApplyResources((object) this.menuBarItem1, "menuBarItem1");
    this.menuBarItem1.Items.AddRange(new ToolbarItemBase[6]
    {
      (ToolbarItemBase) this._deleteMenuButtonItem,
      (ToolbarItemBase) this._addCriterionGroupMenuButtonItem,
      (ToolbarItemBase) this._addCriterionMenuButtonItem,
      (ToolbarItemBase) this._addChildCriterionGroupMenuButtonItem,
      (ToolbarItemBase) this._addChildCriterionMenuButtonItem,
      (ToolbarItemBase) this._changeLogicalOperatorMenuButtonItem
    });
    this.menuBarItem1.ShowText = true;
    componentResourceManager.ApplyResources((object) this._deleteMenuButtonItem, "_deleteMenuButtonItem");
    this._deleteMenuButtonItem.ImageIndex = 2;
    this._deleteMenuButtonItem.ShowText = true;
    this._deleteMenuButtonItem.Click += new EventHandler(this.DeleteButtonItem_Click);
    this._addCriterionGroupMenuButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._addCriterionGroupMenuButtonItem, "_addCriterionGroupMenuButtonItem");
    this._addCriterionGroupMenuButtonItem.ImageIndex = 3;
    this._addCriterionGroupMenuButtonItem.ShowText = true;
    this._addCriterionGroupMenuButtonItem.Click += new EventHandler(this.AddCriterionGroupButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._addCriterionMenuButtonItem, "_addCriterionMenuButtonItem");
    this._addCriterionMenuButtonItem.ImageIndex = 4;
    this._addCriterionMenuButtonItem.ShowText = true;
    this._addCriterionMenuButtonItem.Click += new EventHandler(this.AddCriterionButtonItem_Click);
    this._addChildCriterionGroupMenuButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._addChildCriterionGroupMenuButtonItem, "_addChildCriterionGroupMenuButtonItem");
    this._addChildCriterionGroupMenuButtonItem.ImageIndex = 7;
    this._addChildCriterionGroupMenuButtonItem.ShowText = true;
    this._addChildCriterionGroupMenuButtonItem.Click += new EventHandler(this.AddChildCriterionGroupButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._addChildCriterionMenuButtonItem, "_addChildCriterionMenuButtonItem");
    this._addChildCriterionMenuButtonItem.ImageIndex = 8;
    this._addChildCriterionMenuButtonItem.ShowText = true;
    this._addChildCriterionMenuButtonItem.Click += new EventHandler(this.AddChildCriterionButtonItem_Click);
    this._changeLogicalOperatorMenuButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._changeLogicalOperatorMenuButtonItem, "_changeLogicalOperatorMenuButtonItem");
    this._changeLogicalOperatorMenuButtonItem.ImageIndex = 5;
    this._changeLogicalOperatorMenuButtonItem.ShowText = true;
    this._changeLogicalOperatorMenuButtonItem.Click += new EventHandler(this.ChangeLogicalOperatorButtonItem_Click);
    componentResourceManager.ApplyResources((object) this.contextMenuBarItem, "contextMenuBarItem");
    this.contextMenuBarItem.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.tsmAddOption,
      (ToolbarItemBase) this.tsmDeleteOption,
      (ToolbarItemBase) this.tsmClear
    });
    this.contextMenuBarItem.ShowText = true;
    componentResourceManager.ApplyResources((object) this.tsmAddOption, "tsmAddOption");
    this.tsmAddOption.ImageIndex = 0;
    this.tsmAddOption.ShowText = true;
    componentResourceManager.ApplyResources((object) this.tsmDeleteOption, "tsmDeleteOption");
    this.tsmDeleteOption.ImageIndex = 1;
    this.tsmDeleteOption.ShowText = true;
    this.tsmClear.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.tsmClear, "tsmClear");
    this.tsmClear.ShowText = true;
    componentResourceManager.ApplyResources((object) this.pbError, "pbError");
    this.pbError.Name = "pbError";
    this.pbError.TabStop = false;
    this.errorPanel.Controls.Add((Control) this.lbErrorState);
    this.errorPanel.Controls.Add((Control) this.pbError);
    componentResourceManager.ApplyResources((object) this.errorPanel, "errorPanel");
    this.errorPanel.Name = "errorPanel";
    componentResourceManager.ApplyResources((object) this.lbErrorState, "lbErrorState");
    this.lbErrorState.Name = "lbErrorState";
    this.panelHint.BorderStyle = BorderStyle.Fixed3D;
    this.panelHint.Controls.Add((Control) this.labelWarning);
    this.panelHint.Controls.Add((Control) this.pictureHint);
    componentResourceManager.ApplyResources((object) this.panelHint, "panelHint");
    this.panelHint.Name = "panelHint";
    componentResourceManager.ApplyResources((object) this.labelWarning, "labelWarning");
    this.labelWarning.Name = "labelWarning";
    componentResourceManager.ApplyResources((object) this.pictureHint, "pictureHint");
    this.pictureHint.Name = "pictureHint";
    this.pictureHint.TabStop = false;
    componentResourceManager.ApplyResources((object) this._hintTextBox, "_hintTextBox");
    this._hintTextBox.Name = "_hintTextBox";
    this._hintTextBox.ReadOnly = true;
    this.ilError.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilError.ImageStream");
    this.ilError.TransparentColor = Color.Transparent;
    this.ilError.Images.SetKeyName(0, "gear_warning.png");
    this.ilError.Images.SetKeyName(1, "delete.png");
    this.ilError.Images.SetKeyName(2, "garbage.png");
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this._treeList);
    this.Controls.Add((Control) this.errorPanel);
    this.Controls.Add((Control) this.tbOptionWork);
    this.Controls.Add((Control) this.cmsOptionWork);
    this.Controls.Add((Control) this._hintTextBox);
    this.Controls.Add((Control) this.panelHint);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (IncompatibilityEditor);
    this._treeList.EndInit();
    this.cellEditor.EndInit();
    this.repositoryItemImageComboBox2.EndInit();
    this.repositoryItemComboBox1.EndInit();
    ((ISupportInitialize) this.pbError).EndInit();
    this.errorPanel.ResumeLayout(false);
    this.panelHint.ResumeLayout(false);
    ((ISupportInitialize) this.pictureHint).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  public sealed class PathPart
  {
    public PathPart(object option, object value)
    {
      this.Option = option;
      this.Value = value;
    }

    public object Option { get; private set; }

    public object Value { get; private set; }

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is IncompatibilityEditor.PathPart pathPart && object.Equals(this.Option, pathPart.Option) && object.Equals(this.Value, pathPart.Value);
    }

    public override int GetHashCode()
    {
      return (this.Option != null ? this.Option.GetHashCode() : 0) ^ (this.Value != null ? this.Value.GetHashCode() : 0);
    }
  }
}
