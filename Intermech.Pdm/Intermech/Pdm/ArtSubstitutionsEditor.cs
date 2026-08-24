// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ArtSubstitutionsEditor
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using ImSSP;
using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Docking;
using Intermech.Docking.Rendering;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Navigator.Views;
using Intermech.Pdm.Substitutes;
using Intermech.Search;
using Intermech.Search.Pdm.Substitutes;
using Intermech.Search.UI;
using Intermech.Search.Utilities;
using Intermech.VirtualTreeView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Pdm;

internal sealed class ArtSubstitutionsEditor : Form
{
  private static readonly Color AuxiliaryPositionColor = Color.LightYellow;
  private static readonly Color DesignActualVariantColor = Color.LightGreen;
  private static readonly Color EqualPositionColor = Color.FromArgb(196, 224 /*0xE0*/, 224 /*0xE0*/);
  private const int CaptionSubstitutesTreeColumnWidthSettingKey = 3003;
  private ISelectedItems _selectedItems;
  private PDMSubstitutesEditorOptionsHolder _pdmSubstitutesEditorOptionsHolder;
  private AdvancedServiceContainer _advancedServiceContainer;
  private MyObjectElement _parentItem = new MyObjectElement();
  private SubstitutesEditorCommand _substitutionsEditorCommand = SubstitutesEditorCommand.EditSubstitutes;
  private long _desiredNewGroupNumber = -1;
  private ArticlesPartsPackage _articlesPartsPackage;
  private List<long> _createdRelationIds = new List<long>(0);
  private List<long> _deletedRelationIds = new List<long>(0);
  private List<long> _changedRelationIds = new List<long>(0);
  private List<long> _substitutesRelationIds = new List<long>(0);
  private Dictionary<long, long> _checkOutObjects = new Dictionary<long, long>();
  private SubstituteObjects _substituteObjects;
  private RelationAttributesPackage _relationAttributesPackage;
  private Dictionary<long, string> _remarkDictionaryByRelationID;
  private CommandManager _commandManager;
  private INavGraphicsCache _navGraphicsCache;
  private ICategoryTypeIconService _categoryTypeIconService;
  private IPDMSubstitutesService _pdmSubstitutesService;
  private ISubstitutesRemarksService _substsitutesRemarksService;
  private ISubstitutesSettings _substitutesSettings;
  private INamedImageList _namedImageList;
  private static Icon _iconGroup;
  private static Icon _iconActualSubstitute;
  private static Icon _iconSubstitute;
  private Dictionary<int, Icon> _iconDictionaryByObjectTypeID = new Dictionary<int, Icon>();
  private NodeIDPath _positionsGridRootNodeIDPath;
  private INode _positionsGridRootNode;
  private HybridDictionary _formSettings = new HybridDictionary(0, true);
  private const int fsDocLayout = 1;
  private const int ccGridHeight = 1000;
  private List<long> _contextIds;
  private List<NodeColumnID> _additionalNodeColumnIds = new List<NodeColumnID>();
  private SubstitutesEditorMode _substitutionsEditorMode;
  private bool _isChanged;
  private bool _hasError;
  private Rectangle _dragBoxFromMouseDown;
  private Point _screenOffset;
  private Row _dropTargetRow;
  private bool _disableTreeEvents;
  private ArticleRelationState _articleRelationState;
  private bool _hasSelectedGroups;
  private bool _hasSelectedSubstitutes;
  private bool _hasSelectedRelations;
  private bool _hasSelectedDesignerActualRelations;
  private List<long> _selectedGroupNumbers = new List<long>();
  private Dictionary<long, List<long>> _selectedSubstituteNumberDictionaryByGroupNumber = new Dictionary<long, List<long>>();
  private List<long> _selectedRelationIdsList = new List<long>();
  private long[] _selectedSubstituteGroupNumbers = new long[0];
  private Tuple<long, long>[] _selectedSubstituteNumbers = new Tuple<long, long>[0];
  public long[] _selectedRelationIds = new long[0];
  private int _selectedGroupsCount;
  private int _selectedSubstitutesCount;
  private int _selectedRelationsCount;
  private long _selectedGroupNumber = -1;
  private long _selectedSubstituteNumber = -1;
  private List<ArtSubstitutionsEditor.Relation> _relations;
  private IContainer components;
  private DocumentContainer documentContainer;
  private DockControl dockSubstitutes;
  private Intermech.Bars.ToolBar tbSubstitutes;
  private DockControl dockComposition;
  private Intermech.Bars.ToolBar tbComposition;
  private DockContainer bottomLeft;
  private ImageList imagesTreeview;
  private ImageList imagesList;
  private DockManager dockManager;
  private DockContainer leftDock;
  private DockContainer rightDock;
  private DockContainer bottomDock;
  private DockContainer topDock;
  private Panel panelBottom;
  private CheckBox _autoExpandSubstitutesCheckBox;
  private CheckBox _autoExpandGroupsCheckBox;
  private Button _cancelButton;
  private Button _okButton;
  private DockContainer dockContainer;
  private SubstitutesView _positionsGrid;
  private Intermech.VirtualTreeView.VirtualTreeView _substitutesTree;
  private ImageList imagesToolbars;
  private ButtonItem _createGroupButton;
  private ButtonItem _createAllowableSubstituteButton;
  private ButtonItem _actualizeSubstituteButton;
  private MenuBar menuSubstitutes;
  private ContextMenuBarItem contextMenuSubstitutes;
  private MenuButtonItem mnpAddGroup;
  private MenuButtonItem mnpCheck;
  private MenuButtonItem mnpAddSubstitute;
  private MenuButtonItem mnpActualizeSubstitute;
  private ButtonItem _checkSubstitutesButton;
  private ImageList imagesMenus;
  private MenuBar menuComposition;
  private ContextMenuBarItem contextMenuComposition;
  private MenuButtonItem mnpCheck2;
  private MenuButtonItem mnpToActual;
  private MenuButtonItem mnpToSubstitute;
  private ButtonItem btnTracing;
  private MenuButtonItem mnpColumnsSetup;
  private Panel panelLegend;
  private Label labelDesignerSubstitute;
  private ButtonItem _markDesignActualVariantButtonItem;
  private MenuButtonItem _markDesignActualVariantMenuButtonItem;
  private MenuButtonItem mnpVirtualComposition;
  private ImageList imageNewDesign;
  private MenuButtonItem mnpDelete;
  private ButtonItem _deleteButton;
  private ButtonItem _addToActualSubstituteButton;
  private ButtonItem _addToAllowableSubstituteButton;
  internal CellEditor groupNameEditor;
  private TextBox textBoxGroupNameEditor;
  private DropDownMenuItem btVirtualComposition;
  private MenuButtonItem btDefault;
  private MenuButtonItem btActual;
  private MenuButtonItem btWithoutComposition;
  private ButtonItem btnTrack;
  private MenuButtonItem mnpTrack;
  private Column _captionSubstitutesTreeColumn;
  private Column _noteSubstitutesTreeColumn;
  private DockControl dockMessages;
  private MessageList _messageList;
  private Intermech.Bars.ToolBar tbMessages;
  private ButtonItem btClear;
  private ButtonItem _markAuxiliaryPositionButtonItem;
  private ButtonItem _markEqualPositionButtonItem;
  private Panel _auxiliaryPositionColorPanel;
  private Panel _deignActualVariantColorPanel;
  private Label label1;
  private ButtonItem _moveUpButtonItem;
  private ButtonItem _moveDownButtonItem;
  private Panel _equalPositionColorPanel;
  private Label labelEqualPosition;

  public static DialogResult Execute(
    string FormCaption,
    ISelectedItems items,
    IServiceProvider viewServices,
    SubstitutesEditorCommand formCommand,
    long desiredGroupNumber,
    out long[] NewRels,
    out long[] DelRels,
    out long[] ChRels,
    out long[] SubstRels,
    out Dictionary<long, long> ChkOuts)
  {
    using (ArtSubstitutionsEditor substitutionsEditor = new ArtSubstitutionsEditor(FormCaption, items, viewServices, formCommand, desiredGroupNumber))
    {
      NewRels = new long[0];
      DelRels = new long[0];
      ChRels = new long[0];
      SubstRels = new long[0];
      ChkOuts = new Dictionary<long, long>();
      if (substitutionsEditor._hasError)
        return DialogResult.Abort;
      int num = (int) substitutionsEditor.ShowDialog();
      NewRels = substitutionsEditor._createdRelationIds.ToArray();
      DelRels = substitutionsEditor._deletedRelationIds.ToArray();
      ChRels = substitutionsEditor._changedRelationIds.ToArray();
      SubstRels = substitutionsEditor._substitutesRelationIds.ToArray();
      ChkOuts = substitutionsEditor._checkOutObjects;
      return (DialogResult) num;
    }
  }

  public ArtSubstitutionsEditor()
  {
    this.InitializeComponent();
    this._positionsGrid.BlockUISettingsDisableChildrenViewGrouping = true;
    this._auxiliaryPositionColorPanel.BackColor = ArtSubstitutionsEditor.AuxiliaryPositionColor;
    this._deignActualVariantColorPanel.BackColor = ArtSubstitutionsEditor.DesignActualVariantColor;
    this._equalPositionColorPanel.BackColor = ArtSubstitutionsEditor.EqualPositionColor;
  }

  public ArtSubstitutionsEditor(
    string caption,
    ISelectedItems selectedItems,
    IServiceProvider serviceProvider,
    SubstitutesEditorCommand substitutionsEditorCommand,
    long desiredGroupNumber = -1)
  {
    this._selectedItems = selectedItems != null ? selectedItems : throw new ArgumentNullException();
    this._advancedServiceContainer = new AdvancedServiceContainer(serviceProvider);
    this._substitutionsEditorCommand = substitutionsEditorCommand;
    this._desiredNewGroupNumber = desiredGroupNumber;
    this.InitializeComponent();
    this.InitializeMoreComponents();
    this.InitializeServices();
    this._positionsGrid.BlockUISettingsDisableChildrenViewGrouping = true;
    this._positionsGrid.DisableGroupBox = true;
    this._auxiliaryPositionColorPanel.BackColor = ArtSubstitutionsEditor.AuxiliaryPositionColor;
    this._deignActualVariantColorPanel.BackColor = ArtSubstitutionsEditor.DesignActualVariantColor;
    this._equalPositionColorPanel.BackColor = ArtSubstitutionsEditor.EqualPositionColor;
    this._hasError = !this.LoadData();
  }

  private void SubstitutesEditor_Load(object sender, EventArgs e)
  {
  }

  private void MessageList_SelectedIndexChanged(object sender, EventArgs e)
  {
    int selectedIndex = this._messageList.SelectedIndex;
    if (selectedIndex < 0)
      return;
    _Message message = this._messageList.Messages[selectedIndex];
    if (!(message is ArtSubstitutionsEditor.SubstitutesEditorMessage))
      return;
    object[] data = (object[]) ((ArtSubstitutionsEditor.SubstitutesEditorMessage) message).Data;
    this.GotoSubstitute(data.Length != 0 ? (long) data[0] : -1L, data.Length > 1 ? Convert.ToInt64(data[1]) : -1L, data.Length > 2 ? (List<long>) data[2] : new List<long>());
  }

  private void SubstitutesEditor_FormClosed(object sender, FormClosedEventArgs e)
  {
    this._formSettings[(object) 3000] = (object) this._autoExpandGroupsCheckBox.Checked;
    this._formSettings[(object) 3001] = (object) this._autoExpandSubstitutesCheckBox.Checked;
    this._formSettings[(object) 3002] = (object) this._positionsGrid.GetVirtualMode();
    this._formSettings[(object) 3003] = (object) this._captionSubstitutesTreeColumn.Width;
    FormStorage.SaveLayout((Control) this, (IDictionary) this._formSettings);
  }

  private void PositionsGrid_ShowCustomContextMenu(object sender, ContextMenuEventArgs e)
  {
    this.contextMenuComposition.Show(e.Control, e.Location);
  }

  private void PositionsGrid_GridDragDrop(object sender, DragEventArgs e)
  {
    e.Effect = DragDropEffects.None;
    if (!this._positionsGrid.AllowDrop || !e.Data.GetDataPresent(typeof (IOSource)))
      return;
    if ((e.Data.GetData(typeof (IOSource)) as IOSource).Control != this._substitutesTree)
      return;
    try
    {
      this.GatherSelectedInfo();
      List<long> relationIds = this.GatherSelectedRelations();
      this._disableTreeEvents = true;
      this.RemoveRelations(relationIds);
      this._substituteObjects.Groups.Sort();
      this.CorrectDesignActualVariant();
      this.RebuildTree();
    }
    finally
    {
      this._disableTreeEvents = false;
      this._isChanged = true;
      this.UpdateControls();
    }
  }

  private void DoCulumnsSetup(object sender, EventArgs e)
  {
    SubstitutesVirtualMode virtualMode = this._positionsGrid.GetVirtualMode();
    this._positionsGrid.SetColumnsCommand(this._selectedItems, (IServiceProvider) this._advancedServiceContainer, (object) null);
    switch (virtualMode)
    {
      case SubstitutesVirtualMode.ActualComposition:
        this.DoActualComposition((object) null, (EventArgs) null);
        break;
      case SubstitutesVirtualMode.WithoutSubstitutes:
        this.DoWithoutSubstitutes((object) null, (EventArgs) null);
        break;
      default:
        this.DoDefaultComposition((object) null, (EventArgs) null);
        break;
    }
    this.AddTreeColumns();
    this.RebuildTree();
    this.UpdateControls();
  }

  private void DoNextCompositionStyle(object sender, EventArgs e)
  {
    switch (this._positionsGrid.GetVirtualMode())
    {
      case SubstitutesVirtualMode.States:
        this.DoActualComposition(sender, e);
        break;
      case SubstitutesVirtualMode.ActualComposition:
        this.DoWithoutSubstitutes(sender, e);
        break;
      case SubstitutesVirtualMode.WithoutSubstitutes:
        this.DoDefaultComposition(sender, e);
        break;
    }
  }

  private void DoDefaultComposition(object sender, EventArgs e)
  {
    this._positionsGrid.SetVirtualMode(SubstitutesVirtualMode.States, this._articlesPartsPackage);
    this.btVirtualComposition.Tag = (object) SubstitutesVirtualMode.States;
    this.btVirtualComposition.Text = LocalizationHolder.rm.GetString("Pdm_541");
    this.btVirtualComposition.ToolTipText = LocalizationHolder.rm.GetString("Pdm_542");
    this.btVirtualComposition.ImageIndex = this.btDefault.ImageIndex;
    this.btDefault.Checked = true;
    this.btActual.Checked = false;
    this.btWithoutComposition.Checked = false;
    this.btnTrack.Enabled = true;
  }

  private void DoActualComposition(object sender, EventArgs e)
  {
    this._positionsGrid.SetVirtualMode(SubstitutesVirtualMode.ActualComposition, this._articlesPartsPackage);
    this.btVirtualComposition.Tag = (object) SubstitutesVirtualMode.ActualComposition;
    this.btVirtualComposition.Text = LocalizationHolder.rm.GetString("Pdm_543");
    this.btVirtualComposition.ToolTipText = LocalizationHolder.rm.GetString("Pdm_544");
    this.btVirtualComposition.ImageIndex = this.btActual.ImageIndex;
    this.btDefault.Checked = false;
    this.btActual.Checked = true;
    this.btWithoutComposition.Checked = false;
    this.btnTrack.Enabled = true;
  }

  private void DoWithoutSubstitutes(object sender, EventArgs e)
  {
    this._positionsGrid.SetVirtualMode(SubstitutesVirtualMode.WithoutSubstitutes, this._articlesPartsPackage);
    this.btVirtualComposition.Tag = (object) SubstitutesVirtualMode.WithoutSubstitutes;
    this.btVirtualComposition.Text = LocalizationHolder.rm.GetString("Pdm_545");
    this.btVirtualComposition.ToolTipText = LocalizationHolder.rm.GetString("Pdm_546");
    this.btVirtualComposition.ImageIndex = this.btWithoutComposition.ImageIndex;
    this.btDefault.Checked = false;
    this.btActual.Checked = false;
    this.btWithoutComposition.Checked = true;
    this.btnTrack.Enabled = false;
  }

  private void BarManager_RendererChanged(object sender, EventArgs e)
  {
    IToolBarRenderer renderer = (sender as BarManager).Renderer;
    this.tbComposition.Renderer = renderer;
    this.tbMessages.Renderer = renderer;
    this.tbSubstitutes.Renderer = renderer;
    this.menuComposition.Renderer = renderer;
    this.menuSubstitutes.Renderer = renderer;
  }

  private void DoTrack(object sender, EventArgs e)
  {
    this.GotoRelations(this._positionsGrid.SelectedRelationsFromComposition);
    this.UpdateControls();
  }

  private void AutoExpandGroupsCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    this._autoExpandSubstitutesCheckBox.Enabled = this._autoExpandGroupsCheckBox.Checked;
  }

  private void PositionsGrid_SelectedItemsChanged(object sender, EventArgs e)
  {
    this.UpdateControls();
  }

  private void contextMenuGroups_BeforePopup(object sender, MenuPopupEventArgs e)
  {
    this.UpdateControls();
  }

  private void SubstitutesTree_SelectionChanged(object sender, EventArgs e)
  {
    RowSelectionList selectedRows = this._substitutesTree.SelectedRows;
    if (selectedRows != null)
    {
      List<long> longList1 = new List<long>();
      List<Tuple<long, long>> tupleList = new List<Tuple<long, long>>();
      List<long> longList2 = new List<long>();
      foreach (Row row in selectedRows)
      {
        object obj = row.Item;
        if (row.Level == 1 && obj is long num2)
          longList1.Add(num2);
        else if (row.Level == 2 && obj is List<long>)
        {
          Row parentRow = row.ParentRow;
          if (parentRow != null && parentRow.Item is long num)
            tupleList.Add(new Tuple<long, long>(num, (long) row.ChildIndex));
        }
        else if (row.Level == 3 && obj is long num1)
          longList2.Add(num1);
      }
      this._selectedSubstituteGroupNumbers = longList1.ToArray();
      this._selectedSubstituteNumbers = tupleList.ToArray();
      this._selectedRelationIds = longList2.ToArray();
    }
    else
    {
      this._selectedSubstituteGroupNumbers = new long[0];
      this._selectedSubstituteNumbers = new Tuple<long, long>[0];
      this._selectedRelationIds = new long[0];
    }
    this.UpdateControls();
    if (!this.btnTracing.Checked)
      return;
    List<INodeID> nodeIDs = new List<INodeID>();
    List<long> relations = new List<long>();
    if (this._selectedGroupNumber >= 1L && this._selectedSubstituteNumber == -1L && this._selectedRelationIdsList.Count == 0)
      this._substituteObjects.GatherRelations(this._selectedGroupNumber, ref relations);
    if (this._selectedGroupNumber >= 1L && this._selectedSubstituteNumber >= 0L && this._selectedRelationIdsList.Count == 0)
      this._substituteObjects.GatherRelations(this._selectedGroupNumber, this._selectedSubstituteNumber, ref relations);
    for (int index = 0; index < relations.Count; ++index)
    {
      SubstitutesNodeID nodeId4RelationId = this.GetSubstitutesNodeID4RelationID(relations[index]);
      if (nodeId4RelationId != null)
        nodeIDs.Add((INodeID) nodeId4RelationId);
    }
    if (this._selectedGroupNumber >= 1L && this._selectedSubstituteNumber >= 0L && this._selectedRelationIdsList.Count > 0)
    {
      for (int index = 0; index < this._selectedRelationIdsList.Count; ++index)
      {
        SubstitutesNodeID nodeId4RelationId = this.GetSubstitutesNodeID4RelationID(this._selectedRelationIdsList[index]);
        if (nodeId4RelationId != null)
          nodeIDs.Add((INodeID) nodeId4RelationId);
      }
    }
    this._positionsGrid.SelectNodes(nodeIDs);
  }

  private void SubstitutesTree_GetCellData(object sender, GetCellDataEventArgs e)
  {
    if (e.Row.Level == 1)
    {
      if (e.Column == this._captionSubstitutesTreeColumn)
        e.CellData.Value = (object) this._substituteObjects.GetSubstGroupName((long) e.Row.Item);
      if (e.Column != this._noteSubstitutesTreeColumn)
        return;
      string str = string.Empty;
      if (this._articlesPartsPackage != null)
      {
        List<long> relations = new List<long>();
        this._substituteObjects.GatherRelations((long) e.Row.Item, ref relations);
        str = this.GetGroupSuffix(this.CheckRelationsState(relations));
      }
      e.CellData.Value = (object) str;
      if (this._substituteObjects[(long) e.Row.Item].Count == 0)
      {
        Color cellBkStartColor = this._navGraphicsCache.CurrentColorsScheme.WarningCellBkStartColor;
        Color color = cellBkStartColor;
        LinearGradientMode cellGradientMode = this._navGraphicsCache.CurrentColorsScheme.WarningCellGradientMode;
        e.CellData.OddStyle = new Style(e.Row.Tree.RowOddStyle, new StyleDelta()
        {
          BackColor = cellBkStartColor,
          GradientColor = color,
          GradientMode = cellGradientMode
        });
        e.CellData.EvenStyle = new Style(e.Row.Tree.RowEvenStyle, new StyleDelta()
        {
          BackColor = cellBkStartColor,
          GradientColor = color,
          GradientMode = cellGradientMode
        });
        e.CellData.Value = (object) LocalizationHolder.rm.GetString("Pdm_298");
      }
      else
      {
        if (string.IsNullOrEmpty(str))
          return;
        Color cellBkStartColor = this._navGraphicsCache.CurrentColorsScheme.HintCellBkStartColor;
        Color color = cellBkStartColor;
        LinearGradientMode cellGradientMode = this._navGraphicsCache.CurrentColorsScheme.HintCellGradientMode;
        e.CellData.OddStyle = new Style(e.Row.Tree.RowOddStyle, new StyleDelta()
        {
          BackColor = cellBkStartColor,
          GradientColor = color,
          GradientMode = cellGradientMode
        });
        e.CellData.EvenStyle = new Style(e.Row.Tree.RowEvenStyle, new StyleDelta()
        {
          BackColor = cellBkStartColor,
          GradientColor = color,
          GradientMode = cellGradientMode
        });
      }
    }
    else
    {
      if (e.Row.Level == 2)
      {
        object obj = e.Row.Item;
        bool flag = false;
        if (obj is List<long>)
        {
          Row parentRow = e.Row.ParentRow;
          if (parentRow != null && parentRow.Item is long substituteGroupNumber && this._substituteObjects.IsDesignActualVariant(substituteGroupNumber, (long) e.Row.ChildIndex))
            flag = true;
        }
        if (flag)
        {
          e.CellData.EvenStyle = this.CreateStyleWithNewBackColor(e.CellData.EvenStyle, ArtSubstitutionsEditor.DesignActualVariantColor);
          e.CellData.OddStyle = this.CreateStyleWithNewBackColor(e.CellData.OddStyle, ArtSubstitutionsEditor.DesignActualVariantColor);
        }
        else
        {
          e.CellData.EvenStyle = this.CreateStyleWithNewBackColor(e.CellData.EvenStyle, Color.White);
          e.CellData.OddStyle = this.CreateStyleWithNewBackColor(e.CellData.OddStyle, Color.White);
        }
        if (e.Column == this._captionSubstitutesTreeColumn)
        {
          string substGroupName = this._substituteObjects.GetSubstGroupName((long) e.Row.ParentRow.Item);
          e.CellData.Value = e.Row.ChildIndex == 0 ? (object) string.Format(LocalizationHolder.rm.GetString("Pdm_299"), (object) substGroupName, (object) e.Row.ChildIndex) : (object) string.Format(LocalizationHolder.rm.GetString("Pdm_300"), (object) substGroupName, (object) e.Row.ChildIndex);
          if (e.Row.ChildIndex > 0)
            return;
        }
        if (e.Column == this._noteSubstitutesTreeColumn)
        {
          List<List<long>> substituteObject = this._substituteObjects[(long) e.Row.ParentRow.Item];
          if (e.Row.ChildIndex == 0 && substituteObject.Count == 1)
            e.CellData.Value = (object) LocalizationHolder.rm.GetString("Pdm_301");
        }
      }
      if (e.Row.Level != 3)
        return;
      long num = (long) e.Row.Item;
      SubstitutesNodeID nodeId4RelationId = this.GetSubstitutesNodeID4RelationID(num);
      if (this._substituteObjects.IsAuxiliaryPosition(num))
      {
        e.CellData.EvenStyle = this.CreateStyleWithNewBackColor(e.CellData.EvenStyle, ArtSubstitutionsEditor.AuxiliaryPositionColor);
        e.CellData.OddStyle = this.CreateStyleWithNewBackColor(e.CellData.OddStyle, ArtSubstitutionsEditor.AuxiliaryPositionColor);
      }
      else if (this._substituteObjects.IsEqualPosition(num))
      {
        Style withNewBackColor1 = this.CreateStyleWithNewBackColor(e.CellData.EvenStyle, ArtSubstitutionsEditor.EqualPositionColor);
        withNewBackColor1.GradientColor = Color.White;
        e.CellData.EvenStyle = withNewBackColor1;
        Style withNewBackColor2 = this.CreateStyleWithNewBackColor(e.CellData.OddStyle, ArtSubstitutionsEditor.EqualPositionColor);
        withNewBackColor2.GradientColor = Color.White;
        e.CellData.OddStyle = withNewBackColor2;
      }
      else
      {
        e.CellData.EvenStyle = this.CreateStyleWithNewBackColor(e.CellData.EvenStyle, Color.White);
        e.CellData.OddStyle = this.CreateStyleWithNewBackColor(e.CellData.OddStyle, Color.White);
      }
      if (e.Column == this._captionSubstitutesTreeColumn)
      {
        e.CellData.Value = (object) nodeId4RelationId.Caption;
      }
      else
      {
        if (e.Column == this._noteSubstitutesTreeColumn)
        {
          object obj = this._remarkDictionaryByRelationID.ContainsKey(num) ? (object) this._remarkDictionaryByRelationID[num] : (object) string.Empty;
          e.CellData.Value = (object) obj?.ToString();
        }
        if (string.IsNullOrEmpty(e.Column.DataField))
          return;
        string dataField = e.Column.DataField;
        e.CellData.Value = this._positionsGrid.GetCellValue(nodeId4RelationId, dataField);
      }
    }
  }

  private Style CreateStyleWithNewBackColor(Style style, Color backColor)
  {
    return new Style(style, new StyleDelta()
    {
      BackColor = backColor,
      GradientColor = backColor
    });
  }

  private void SubstitutesTree_GetChildren(object sender, GetChildrenEventArgs e)
  {
    if (e.Row.Item == null)
      return;
    if (e.Row.Level == 0)
      e.Children = (IList) this._substituteObjects.Groups;
    else if (e.Row.Level == 1)
    {
      e.Children = (IList) this._substituteObjects[(long) e.Row.Item];
    }
    else
    {
      if (e.Row.Level != 2)
        return;
      List<long> source = this._substituteObjects[(long) e.Row.ParentRow.Item, (long) e.Row.ChildIndex];
      if (source != null)
        source = source.OrderBy<long, long>((Func<long, long>) (o => this._substituteObjects.GetPositionNumber(o))).ToList<long>();
      e.Children = (IList) source;
    }
  }

  private void SubstitutesTree_GetRowData(object sender, GetRowDataEventArgs e)
  {
    e.RowData.AutoFitHeight = true;
    if (e.Row.Item == null)
      return;
    if (e.Row.Level == 1)
      e.RowData.Icon = ArtSubstitutionsEditor._iconGroup;
    else if (e.Row.Level == 2)
    {
      e.RowData.Icon = e.Row.ChildIndex == 0 ? ArtSubstitutionsEditor._iconActualSubstitute : ArtSubstitutionsEditor._iconSubstitute;
    }
    else
    {
      if (e.Row.Level != 3)
        return;
      SubstitutesNodeID nodeId4RelationId = this.GetSubstitutesNodeID4RelationID((long) e.Row.Item);
      e.RowData.ImageList = this._categoryTypeIconService.ImageList;
      e.RowData.ImageSize = 32 /*0x20*/;
      e.RowData.ImageIndex = Images32x16_Cache.GetImage32x16Index(nodeId4RelationId.CategoryID, nodeId4RelationId.TypeID, (NavigatorTreeNode) null);
    }
  }

  private void OKButon_Click(object sender, EventArgs e)
  {
    if (this._substituteObjects != null)
      this._substituteObjects.Groups.Sort();
    this.CheckSubstitutes();
    if (!this._messageList.Messages.HasErrors)
    {
      try
      {
        bool flag = this.WriteDatabaseInfo();
        this.DialogResult = flag ? DialogResult.OK : DialogResult.None;
        if (!flag)
          return;
        PDMPlugin.UpdateHiddenCompositions();
      }
      finally
      {
        this._substituteObjects.GroupsAffected = (Dictionary<long, string>) null;
      }
    }
    else
    {
      this.dockMessages.Open(DockLocation.Document, false);
      this.dockMessages.Focus();
      this.UpdateControls();
    }
  }

  private void SubstitutesEditor_FormClosing(object sender, FormClosingEventArgs e)
  {
    if (!this._isChanged || this.DialogResult == DialogResult.OK || MessageBox.Show(LocalizationHolder.rm.GetString(sc_16889.ssp_pdm_16890()), Strings.Attention, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.No)
      return;
    e.Cancel = true;
  }

  private void CheckSubstitutesButton_Click(object sender, EventArgs e)
  {
    if (this._substituteObjects != null)
      this._substituteObjects.Groups.Sort();
    this.CheckSubstitutes();
    if (!this._messageList.Messages.HasErrors)
      return;
    this.dockMessages.Open(DockLocation.Document, false);
    this.dockMessages.Focus();
    this.UpdateControls();
  }

  private void DeleteButton_Click(object sender, EventArgs e)
  {
    if (this._substitutionsEditorMode != SubstitutesEditorMode.AdminMode || this._substituteObjects == null)
      return;
    this.GatherSelectedInfo();
    if (this._selectedSubstituteNumberDictionaryByGroupNumber.Count == 0 && !this._hasSelectedGroups && !this._hasSelectedSubstitutes && !this._hasSelectedRelations || MessageBox.Show(LocalizationHolder.rm.GetString(sc_16889.ssp_pdm_16891()), Strings.Attention, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
      return;
    List<long> longList1 = new List<long>();
    foreach (KeyValuePair<long, List<long>> keyValuePair in this._selectedSubstituteNumberDictionaryByGroupNumber)
      longList1.Add(keyValuePair.Key);
    longList1.Sort();
    this._substituteObjects.GroupsAffected = longList1.Count <= 0 ? (Dictionary<long, string>) null : new Dictionary<long, string>();
    for (int index1 = longList1.Count - 1; index1 >= 0; --index1)
    {
      this._substituteObjects.GroupsAffected.Add(longList1[index1], this._substituteObjects.GetSubstGroupName(longList1[index1]));
      List<long> longList2 = this._selectedSubstituteNumberDictionaryByGroupNumber[longList1[index1]];
      if (longList2.Count == 0)
      {
        this.RemoveGroup(longList1[index1]);
      }
      else
      {
        longList2.Sort();
        for (int index2 = longList2.Count - 1; index2 >= 0; --index2)
          this.RemoveSubstitute(longList1[index1], longList2[index2]);
      }
    }
    this.RemoveRelations(this._selectedRelationIdsList);
    this._substituteObjects.Groups.Sort();
    this.RebuildTree();
    this._positionsGrid.RebuildVirtualGrid(this._articlesPartsPackage);
    this._isChanged = true;
    this.UpdateControls();
  }

  private void CreateGroupButton_Click(object sender, EventArgs e)
  {
    if (this._substitutionsEditorMode != SubstitutesEditorMode.AdminMode)
      return;
    if (this._substituteObjects == null)
    {
      if (SubstituteObjects.attrSubstituteGroupNo == -1)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
          this._substituteObjects = new SubstituteObjects(sessionKeeper.Session);
      }
      else
        this._substituteObjects = new SubstituteObjects();
      this._substitutesTree.DataSource = (object) this._substituteObjects;
      if (this._substituteObjects != null)
        this._substituteObjects.Groups.Sort();
      this.RebuildTree();
    }
    long group1 = 1;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long[] fromOtherInstances = ((ISubstitutesServerService) sessionKeeper.Session.GetCustomService(typeof (ISubstitutesServerService))).GetExistsSubstituteGroupNumbersFromOtherInstances(sessionKeeper.Session.SessionGUID, this._parentItem.ObjectID, this._parentItem.RelationType);
      while (true)
      {
        if (!((IEnumerable<long>) fromOtherInstances).Contains<long>(group1))
          goto label_16;
label_14:
        ++group1;
        continue;
label_16:
        if (this._substituteObjects.Groups.Contains(group1))
          goto label_14;
        break;
      }
    }
    long group2 = this._substituteObjects.NewGroup(group1);
    this._substituteObjects.Groups.Sort();
    this._substituteObjects.NewSubstitute(group2, 0L);
    this._substituteObjects.NewSubstitute(group2, 1L);
    this.RebuildTree();
    this._positionsGrid.RebuildVirtualGrid(this._articlesPartsPackage);
    this.GotoSubstitute(group2, 0L, (List<long>) null);
    this._isChanged = true;
    this.UpdateControls();
  }

  private void CreateAllowableSubstituteButton_Click(object sender, EventArgs e)
  {
    if (this._substitutionsEditorMode != SubstitutesEditorMode.AdminMode || this._substitutesTree.SelectedRows == null || this._substitutesTree.SelectedRows.Count <= 0)
      return;
    this.GatherSelectedInfo();
    if (this._selectedGroupNumbers.Count != 1 || this._selectedGroupNumber < 1L)
      return;
    long substitute = this._substituteObjects.NewSubstitute(this._selectedGroupNumber, -1L);
    this._substituteObjects.Groups.Sort();
    this.RebuildTree();
    this._positionsGrid.RebuildVirtualGrid(this._articlesPartsPackage);
    this.GotoSubstitute(this._selectedGroupNumber, substitute, (List<long>) null);
    this._isChanged = true;
    this.UpdateControls();
  }

  private void ActualizeSubstituteButton_Click(object sender, EventArgs e)
  {
    if (this._substitutionsEditorMode == SubstitutesEditorMode.ReadOnly || this._substitutesTree.SelectedRows == null || this._substitutesTree.SelectedRows.Count <= 0)
      return;
    this.GatherSelectedInfo();
    if (this._selectedGroupNumbers.Count != 1 || this._selectedSubstituteNumber <= 0L)
      return;
    if (this._selectedSubstitutesCount != 1)
      return;
    try
    {
      this._substituteObjects.SwapSubstitutes(this._selectedGroupNumbers[0], 0L, this._selectedSubstituteNumber);
    }
    catch (Exception ex)
    {
      this._messageList.Messages.Add(new _Message(_MessageType.Error, ex.Message));
    }
    this.RebuildTree();
    this._positionsGrid.RebuildVirtualGrid(this._articlesPartsPackage);
    this.GotoSubstitute(this._selectedGroupNumbers[0], 0L, (List<long>) null);
    this._isChanged = true;
    this.UpdateControls();
  }

  private void AddToActualSubstituteButton_Click(object sender, EventArgs e)
  {
    if (this._substitutionsEditorMode != SubstitutesEditorMode.AdminMode)
      return;
    this.GatherSelectedInfo();
    int itemsCount = this._positionsGrid.ItemsCount;
    int count = this._positionsGrid.SelectedItems != null ? this._positionsGrid.SelectedItems.Count : 0;
    if (itemsCount < 2 || count == 0 || this._selectedGroupNumbers.Count != 1)
      return;
    List<long> relationsFromComposition = this._positionsGrid.SelectedRelationsFromComposition;
    long selectedGroupNumber = this._selectedGroupNumber;
    this.AddRelationsToSubstitute(selectedGroupNumber, 0L, relationsFromComposition);
    this.GotoSubstitute(selectedGroupNumber, 0L, relationsFromComposition);
  }

  private void AddToAllowableSubstituteButton_Click(object sender, EventArgs e)
  {
    if (this._substitutionsEditorMode != SubstitutesEditorMode.AdminMode)
      return;
    this.GatherSelectedInfo();
    int itemsCount = this._positionsGrid.ItemsCount;
    int count = this._positionsGrid.SelectedItems != null ? this._positionsGrid.SelectedItems.Count : 0;
    if (itemsCount < 2 || count == 0 || this._selectedGroupNumbers.Count != 1 || this._selectedSubstituteNumber < 1L)
      return;
    List<long> relationsFromComposition = this._positionsGrid.SelectedRelationsFromComposition;
    long selectedGroupNumber = this._selectedGroupNumber;
    long substituteNumber = this._selectedSubstituteNumber;
    this.AddRelationsToSubstitute(selectedGroupNumber, substituteNumber, relationsFromComposition);
    this.GotoSubstitute(selectedGroupNumber, substituteNumber, relationsFromComposition);
  }

  private void SubstitutesTree_DragEnter(object sender, DragEventArgs e)
  {
    if (this._disableTreeEvents)
      return;
    this._dropTargetRow = (Row) null;
    e.Effect = DragDropEffects.None;
    if (!this._substitutesTree.AllowDrop || !e.Data.GetDataPresent(typeof (IOSource)) || (e.Data.GetData(typeof (IOSource)) as IOSource).Control == this._substitutesTree)
      return;
    e.Effect = DragDropEffects.All;
  }

  private void SubstitutesTree_DragOver(object sender, DragEventArgs e)
  {
    if (this._disableTreeEvents)
      return;
    e.Effect = DragDropEffects.None;
    if (!this._substitutesTree.AllowDrop)
      return;
    IOSource data = e.Data.GetData(typeof (IOSource)) as IOSource;
    if (!e.Data.GetDataPresent(typeof (IOSource)) || data.Control == this._substitutesTree)
      return;
    e.Effect = DragDropEffects.All;
  }

  private void SubstitutesTree_DragDrop(object sender, DragEventArgs e)
  {
    if (this._disableTreeEvents || !e.Data.GetDataPresent(typeof (IOSource)))
      return;
    IOSource data = e.Data.GetData(typeof (IOSource)) as IOSource;
    if (data.SelectedItems == null || data.SelectedItems.Count == 0)
      return;
    if (this._dropTargetRow == null)
    {
      this._dropTargetRow = (Row) null;
      if (MessageBox.Show(LocalizationHolder.rm.GetString(sc_16889.ssp_pdm_16892()), LocalizationHolder.rm.GetString("Pdm_310"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
        return;
      this.DoAddToNewActualSubstitute();
    }
    else
    {
      long num1 = -1;
      long num2 = -1;
      if (this._dropTargetRow.Level == 1)
      {
        num1 = (long) this._dropTargetRow.Item;
        num2 = 0L;
      }
      if (this._dropTargetRow.Level == 2)
      {
        num1 = (long) this._dropTargetRow.ParentRow.Item;
        num2 = (long) this._dropTargetRow.ChildIndex;
      }
      if (this._dropTargetRow.Level == 3)
      {
        num1 = (long) this._dropTargetRow.ParentRow.ParentRow.Item;
        num2 = (long) this._dropTargetRow.ParentRow.ChildIndex;
      }
      this._dropTargetRow = (Row) null;
      if (data.Control == this._substitutesTree)
      {
        List<long> relationIds = this.GatherSelectedRelations();
        this._disableTreeEvents = true;
        try
        {
          this.RemoveRelations(relationIds);
          this._substituteObjects.Groups.Sort();
        }
        finally
        {
          this._disableTreeEvents = false;
        }
      }
      List<long> relationsFromItems = this.GetRelationsFromItems(data.SelectedItems);
      this.AddRelationsToSubstitute(num1, num2, relationsFromItems);
      this.CorrectDesignActualVariant();
      this.GotoSubstitute(num1, num2, relationsFromItems);
    }
  }

  private void SubstitutesTree_GetAllowedRowDropLocations(
    object sender,
    GetAllowedRowDropLocationsEventArgs e)
  {
    if (this._disableTreeEvents)
      return;
    this._dropTargetRow = e.Row;
    e.AllowedDropLocations = this._dropTargetRow != null ? RowDropLocation.OnRow : RowDropLocation.BelowRow;
  }

  private void SubstitutesTree_GetRowDropEffect(object sender, GetRowDropEffectEventArgs e)
  {
    if (this._disableTreeEvents || !e.Data.GetDataPresent(typeof (IOSource)))
      return;
    this._dropTargetRow = e.Row;
    e.DropEffect = DragDropEffects.All;
  }

  private void SubstitutesTree_MouseDown(object sender, MouseEventArgs e)
  {
    if (this._disableTreeEvents)
      return;
    ISelectedItems substitutesSelectedItems = this.GetSubstitutesSelectedItems();
    if (substitutesSelectedItems != null && substitutesSelectedItems.Count > 0)
    {
      Size dragSize = SystemInformation.DragSize;
      dragSize.Width += 4;
      dragSize.Height += 4;
      this._dragBoxFromMouseDown = new Rectangle(new Point(e.X - dragSize.Width / 2, e.Y - dragSize.Height / 2), dragSize);
    }
    else
      this._dragBoxFromMouseDown = Rectangle.Empty;
    if (e.Y >= this._substitutesTree.HeaderHeight)
      return;
    this._dragBoxFromMouseDown = Rectangle.Empty;
  }

  private void SubstitutesTree_MouseMove(object sender, MouseEventArgs e)
  {
    if (this._disableTreeEvents || (e.Button & MouseButtons.Left) != MouseButtons.Left)
      return;
    this.TreeStartDragDrop(e.Location);
  }

  private void SubstitutesTree_MouseUp(object sender, MouseEventArgs e)
  {
    if (e.Button != MouseButtons.Right || e.Location.Y <= this._substitutesTree.HeaderHeight)
      return;
    this.contextMenuSubstitutes.Show((Control) this._substitutesTree, e.Location);
  }

  private void SubstitutesTree_SetCellValue(object sender, SetCellValueEventArgs e)
  {
    if (e.Column != this._captionSubstitutesTreeColumn || e.Row.Level != 1 || e.NewValue == null)
      return;
    string newName = e.NewValue.ToString();
    if (newName == string.Empty)
      return;
    this._substituteObjects.SetSubstGroupName((long) e.Row.Item, newName);
    this._positionsGrid.RebuildVirtualGrid(this._articlesPartsPackage);
    this._isChanged = true;
    this.UpdateControls();
  }

  private void SubstitutesTree_BeforeShowCellEdit(object sender, BeforeShowCellEditEventArgs e)
  {
    if (e.Column == this._captionSubstitutesTreeColumn && e.Row.Level == 1)
    {
      this._substituteObjects.GetSubstGroupName((long) e.Row.Item);
      e.Cancel = false;
    }
    else
      e.Cancel = true;
  }

  private void MarkDesignActualVariantButtonItem_Click(object sender, EventArgs e)
  {
    this._substituteObjects.SetDesignActualVariant(this._selectedSubstituteNumbers[0].Item1, this._selectedSubstituteNumbers[0].Item2, !this._markDesignActualVariantButtonItem.Checked);
    this._isChanged = true;
    this.RebuildTree();
    this.UpdateControls();
  }

  private void MarkAuxiliaryPositionButtonItem_Click(object sender, EventArgs e)
  {
    foreach (long selectedRelationId in this._selectedRelationIds)
      this._substituteObjects.SetAuxiliaryPosition(selectedRelationId, !this._markAuxiliaryPositionButtonItem.Checked);
    this._isChanged = true;
    this.RebuildTree();
    this.UpdateControls();
  }

  private void MarkEqualPositionButtonItem_Click(object sender, EventArgs e)
  {
    foreach (long selectedRelationId in this._selectedRelationIds)
      this._substituteObjects.SetEqualPosition(selectedRelationId, !this._markEqualPositionButtonItem.Checked);
    this._isChanged = true;
    this.RebuildTree();
    this.UpdateControls();
  }

  private void MoveUpButtonItem_Click(object sender, EventArgs e) => this.MoveUp();

  private void MoveDownButtonItem_Click(object sender, EventArgs e) => this.MoveDown();

  private bool IsSelectedDesignActualVariant()
  {
    return this._selectedSubstituteGroupNumbers.Length == 0 && this._selectedSubstituteNumbers.Length == 1 && this._substituteObjects.IsDesignActualVariant(this._selectedSubstituteNumbers[0].Item1, this._selectedSubstituteNumbers[0].Item2) && this._selectedRelationIds.Length == 0;
  }

  private bool IsSelectedSubstitute()
  {
    return this._selectedSubstituteGroupNumbers.Length == 0 && this._selectedSubstituteNumbers.Length == 1 && this._selectedRelationIds.Length == 0;
  }

  private bool IsSelectedAuxiliaryPositions()
  {
    return this._selectedSubstituteGroupNumbers.Length == 0 && this._selectedSubstituteNumbers.Length == 0 && this._selectedRelationIds.Length != 0 && ((IEnumerable<long>) this._selectedRelationIds).Where<long>((Func<long, bool>) (o => !this._substituteObjects.IsAuxiliaryPosition(o))).Count<long>() == 0;
  }

  private bool IsSelectedEqualPositions()
  {
    return this._selectedSubstituteGroupNumbers.Length == 0 && this._selectedSubstituteNumbers.Length == 0 && this._selectedRelationIds.Length != 0 && ((IEnumerable<long>) this._selectedRelationIds).Where<long>((Func<long, bool>) (o => !this._substituteObjects.IsEqualPosition(o))).Count<long>() == 0;
  }

  private bool IsSelectedEditableAuxiliaryOrEqualPositions()
  {
    return this._selectedSubstituteGroupNumbers.Length == 0 && this._selectedSubstituteNumbers.Length == 0 && this._selectedRelationIds.Length != 0 && ((IEnumerable<long>) this._selectedRelationIds).Where<long>((Func<long, bool>) (o => this._substituteObjects.IsNotEditableAuxiliaryPosition(o))).Count<long>() == 0;
  }

  private void InitializeMoreComponents()
  {
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service1)
    {
      service1.RendererChanged += new EventHandler(this.BarManager_RendererChanged);
      this.BarManager_RendererChanged((object) service1, EventArgs.Empty);
    }
    IFiltrationService service2 = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    this._contextIds = service2.Filtration.Tags != null ? service2.Filtration.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] as List<long> : (List<long>) null;
    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
    this.Size = new Size(workingArea.Width / 100 * 90, workingArea.Height / 100 * 80 /*0x50*/);
    int width1 = workingArea.Width;
    Size size = this.Size;
    int width2 = size.Width;
    int x = (width1 - width2) / 2;
    int height1 = workingArea.Height;
    size = this.Size;
    int height2 = size.Height;
    int y = (height1 - height2) / 2;
    this.Location = new Point(x, y);
    this.dockComposition.Manager = this.dockManager;
    this.dockSubstitutes.Manager = this.dockManager;
    this.dockMessages.Manager = this.dockManager;
    (this.dockComposition.LayoutSystem as DocumentLayoutSystem).ScrollingEnabled = false;
    (this.dockSubstitutes.LayoutSystem as DocumentLayoutSystem).ScrollingEnabled = false;
    FormStorage.LoadLayout((Control) this, (IDictionary) this._formSettings);
    if (this._formSettings == null)
      this._formSettings = new HybridDictionary();
    if (this._formSettings.Contains((object) 3003))
      this._captionSubstitutesTreeColumn.Width = Convert.ToInt32(this._formSettings[(object) 3003]);
    if (this._formSettings == null)
      this._formSettings = new HybridDictionary(0, true);
    this._autoExpandGroupsCheckBox.Checked = this._formSettings[(object) 3000] != null && this._formSettings[(object) 3000].Equals((object) true);
    this._autoExpandSubstitutesCheckBox.Checked = this._formSettings[(object) 3001] != null && this._formSettings[(object) 3001].Equals((object) true);
    if (ArtSubstitutionsEditor._iconGroup == null)
    {
      ArtSubstitutionsEditor._iconGroup = Intermech.Interfaces.ImageHelper.BitmapToIcon(this.imagesTreeview.Images[0] as Bitmap);
      ArtSubstitutionsEditor._iconActualSubstitute = Intermech.Interfaces.ImageHelper.BitmapToIcon(this.imagesTreeview.Images[1] as Bitmap);
      ArtSubstitutionsEditor._iconSubstitute = Intermech.Interfaces.ImageHelper.BitmapToIcon(this.imagesTreeview.Images[2] as Bitmap);
    }
    this.mnpColumnsSetup.Image = this._namedImageList != null ? this._namedImageList.ImageList.Images[this._namedImageList.ImageIndex("imgViewSettings")] : this.mnpColumnsSetup.Image;
  }

  private void InitializeServices()
  {
    this._categoryTypeIconService = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._pdmSubstitutesService = ServicesManager.GetService(typeof (IPDMSubstitutesService)) as IPDMSubstitutesService;
    this._navGraphicsCache = ServicesManager.GetService(typeof (INavGraphicsCache)) as INavGraphicsCache;
    this._pdmSubstitutesEditorOptionsHolder = this._advancedServiceContainer.GetService(typeof (PDMSubstitutesEditorOptionsHolder)) as PDMSubstitutesEditorOptionsHolder;
    if (this._pdmSubstitutesEditorOptionsHolder == null)
      this._pdmSubstitutesEditorOptionsHolder = new PDMSubstitutesEditorOptionsHolder(PDMSubstitutesEditorMode.Default, AVSSpecificationForm.Single, (List<long>) null);
    this._substsitutesRemarksService = ServicesManager.GetService(typeof (ISubstitutesRemarksService)) as ISubstitutesRemarksService;
    this._substitutesSettings = ServicesManager.GetService(typeof (ISubstitutesSettings)) as ISubstitutesSettings;
    this._namedImageList = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._commandManager = new CommandManager();
    this._commandManager.ActiveTarget = (ICommandTarget) this._positionsGrid;
    IDefaultCommands4ObjTypes service = ServicesManager.GetService(typeof (IDefaultCommands4ObjTypes)) as IDefaultCommands4ObjTypes;
    this._advancedServiceContainer.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.InDialog));
    this._advancedServiceContainer.AddService(typeof (INotificationService), ServicesManager.GetService(typeof (INotificationService)));
    this._advancedServiceContainer.AddService(typeof (IDefaultCommands4ObjTypes), (object) service);
    this._advancedServiceContainer.AddService(typeof (ICommandManager), (object) this._commandManager);
  }

  private bool LoadData()
  {
    this._relations = new List<ArtSubstitutionsEditor.Relation>();
    if (this._parentItem != null)
      this._parentItem.Clear();
    this._substituteObjects = (SubstituteObjects) null;
    this._articlesPartsPackage = (ArticlesPartsPackage) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      SubstituteObjects.InitStaticFields(sessionKeeper.Session);
      this._additionalNodeColumnIds = new List<NodeColumnID>(SubstituteObjects.AttrColumns.Count);
      for (int index = 0; index < SubstituteObjects.AttrColumns.Count; ++index)
        this._additionalNodeColumnIds.Add(new NodeColumnID(SubstituteObjects.AttrColumns[index].AttributeID, SubstituteObjects.AttrColumns[index].AttributeSource));
      List<int> intList = new List<int>();
      this._remarkDictionaryByRelationID = new Dictionary<long, string>();
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._parentItem = PdmObject.GetItemInfo(sessionKeeper.Session, this._selectedItems.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID, this._selectedItems.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID);
      this._parentItem.RelationType = (this._selectedItems.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID).RelationType;
      try
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._parentItem.ObjectID, false);
        if (dbObject != null)
          this._substitutionsEditorMode = !dbObject.ReadOnly ? SubstitutesEditorMode.AdminMode : SubstitutesEditorMode.UserMode;
        List<long> listInstances = (sessionKeeper.Session.GetCustomService(typeof (IArticleService)) as IArticleService).GetListInstances(this._parentItem.ObjectID, (object) sessionKeeper.Session.SessionGUID);
        if (listInstances != null)
        {
          if (listInstances.Count > 1)
          {
            listInstances.Remove(this._parentItem.ObjectID);
            listInstances.Insert(0, this._parentItem.ObjectID);
            this._articlesPartsPackage = (sessionKeeper.Session.GetCustomService(typeof (ISubstitutesService)) as ISubstitutesService).FindCommonAndVariableParts(sessionKeeper.Session.SessionGUID, string.Empty, this._parentItem.ObjectID, this._parentItem.RelationType, this._pdmSubstitutesEditorOptionsHolder.Form);
          }
        }
      }
      catch
      {
        this._substitutionsEditorMode = SubstitutesEditorMode.ReadOnly;
        throw;
      }
    }
    SubstitutesDescriptor rootDescriptor = new SubstitutesDescriptor(PDMPluginConsts.CategorySubstitutes, 0, (IServiceProvider) this._advancedServiceContainer, string.Empty, this._contextIds, this._parentItem.ObjectID, -1, this._parentItem.RelationType, string.Empty, 0L, this._parentItem.Version, this._parentItem.BaseVersion, this._additionalNodeColumnIds);
    NodeIDPath path = new NodeIDPath((IDescriptor) rootDescriptor);
    EtherealNode etherealNode = new EtherealNode((IDescriptor) rootDescriptor);
    INodeQuery query = etherealNode.GetQuery(ContentType.Folders);
    query.Execute((object) null, 1);
    INodeID recordNodeId = query.GetRecordNodeID(0);
    this._positionsGridRootNodeIDPath = new NodeIDPath(path, recordNodeId);
    this._positionsGridRootNode = etherealNode.GetChild(recordNodeId);
    if (this._positionsGridRootNode is IContextAware positionsGridRootNode)
      positionsGridRootNode.Services = (IServiceProvider) this._advancedServiceContainer;
    this._positionsGrid.Initialize((IDescriptor) rootDescriptor, (IServiceProvider) this._advancedServiceContainer);
    this._positionsGrid.Activate((IView) null);
    this._relationAttributesPackage = this._positionsGrid.RelationsAttributes;
    this._positionsGrid.SetVirtualMode(SubstitutesVirtualMode.States, this._articlesPartsPackage);
    this._positionsGrid.SelectItems(this._selectedItems);
    this.PrepareProcessedRelations();
    this.AddTreeColumns();
    this._substituteObjects = this._positionsGrid.Substitutes != null ? this._positionsGrid.Substitutes.Clone() as SubstituteObjects : (SubstituteObjects) null;
    this._substituteObjects.RebuildGroups();
    this._positionsGrid.SubstitutesVirtual = this._substituteObjects;
    this._substitutesTree.DataSource = (object) this._substituteObjects;
    if (this._substituteObjects != null)
      this._substituteObjects.Groups.Sort();
    this.RebuildTree();
    if (this._substitutesTree.RootRow.NumChildren > 0 && this._autoExpandGroupsCheckBox.Checked)
      this._substitutesTree.RootRow.ExpandChildren(this._autoExpandSubstitutesCheckBox.Checked);
    string str = $" - \"{this._parentItem.Caption}\".  {string.Format(Strings.Hint0, (object) this._positionsGrid.ItemsCount)}";
    string input;
    switch (this._substitutionsEditorCommand)
    {
      case SubstitutesEditorCommand.CreateGroup:
        input = Strings.CreateSubstituteGroup + str;
        break;
      case SubstitutesEditorCommand.ActualizeSubstitute:
        input = Strings.ActualizeSubstitute + str;
        break;
      case SubstitutesEditorCommand.DeleteSubstitutes:
        input = Strings.DeleteSubstituteGroup + str;
        break;
      default:
        input = Strings.FormCaption + str;
        break;
    }
    this.Text = Regex.Replace(input, "(\r|\n)", "");
    this.DoTrack((object) null, (EventArgs) null);
    this._positionsGrid.AllowDrop = this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    this._substitutesTree.AllowDrop = this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    switch (this._formSettings == null || this._formSettings[(object) 3002] == null ? SubstitutesVirtualMode.States : (SubstitutesVirtualMode) this._formSettings[(object) 3002])
    {
      case SubstitutesVirtualMode.ActualComposition:
        this.DoActualComposition((object) null, (EventArgs) null);
        break;
      case SubstitutesVirtualMode.WithoutSubstitutes:
        this.DoWithoutSubstitutes((object) null, (EventArgs) null);
        break;
      default:
        this.DoDefaultComposition((object) null, (EventArgs) null);
        break;
    }
    this.UpdateControls();
    if (this._substitutionsEditorCommand == SubstitutesEditorCommand.CreateGroup && this._positionsGrid.SelectedItems.Count > 0)
      this.DoAddToNewActualSubstitute();
    if (this._substitutionsEditorMode != SubstitutesEditorMode.AdminMode)
      this.CheckSubstitutesButton_Click((object) this, (EventArgs) null);
    return true;
  }

  private void TreeStartDragDrop(Point location)
  {
    if (this._disableTreeEvents || !(this._dragBoxFromMouseDown != Rectangle.Empty) || this._dragBoxFromMouseDown.Contains(location.X, location.Y) || location.Y <= this._substitutesTree.HeaderHeight)
      return;
    if (this._substitutesTree.SelectedRow == null || !this._substitutesTree.AllowDrop)
    {
      this._dragBoxFromMouseDown = Rectangle.Empty;
    }
    else
    {
      this._screenOffset = SystemInformation.WorkingArea.Location;
      int num = (int) this._substitutesTree.DoDragDrop((object) new IOSource((object) this._substitutesTree, (IServiceProvider) this._advancedServiceContainer, this.GetSubstitutesSelectedItems()), DragDropEffects.Copy | DragDropEffects.Scroll);
    }
  }

  private void AddTreeColumns()
  {
    this._positionsGrid.Grid.ColWidthChanging -= new iGColWidthEventHandler(this.PositionsGrid_ColWidthChanging);
    foreach (Column column in this._substitutesTree.Columns)
    {
      if (column != this._captionSubstitutesTreeColumn && column != this._noteSubstitutesTreeColumn)
        column.Changed -= new EventHandler(this.SubstitutesTreeColumn_Changed);
    }
    this._substitutesTree.Columns.Clear();
    this._substitutesTree.Columns.Add(this._captionSubstitutesTreeColumn);
    foreach (NodeColumn nodeColumn in (List<NodeColumn>) this._positionsGrid.GetNodeColumns())
    {
      if (!nodeColumn.ID.Equals((object) "F_CAPTION") && !nodeColumn.ID.Equals((object) "F_STATUSES") && !nodeColumn.ID.Equals((object) ObligatoryObjectAttributes.CAPTION) && !nodeColumn.ID.Equals((object) ObligatoryObjectAttributes.F_ELEMENT_STATUSES) && !nodeColumn.ID.Equals((object) -50) && !nodeColumn.ID.Equals((object) -77) && !(nodeColumn.DataType == typeof (byte[])))
      {
        Column column = new Column();
        column.Name = nodeColumn.Key;
        column.DataField = nodeColumn.Key;
        column.Caption = !UISettings.ShowShortAttributeNames ? nodeColumn.Caption : nodeColumn.ShortCaption;
        column.AutoSizePolicy = ColumnAutoSizePolicy.Manual;
        column.Width = nodeColumn.Width;
        column.Sortable = false;
        column.Movable = false;
        column.ToolTip = nodeColumn.Hint;
        column.MinWidth = 30;
        column.Resizable = true;
        column.HeaderStyle.HorzAlignment = StringAlignment.Near;
        INodeColumnTransform defaultTransform = ((IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes))).GetDefaultTransform(nodeColumn.SchemeGuid, nodeColumn.ID);
        Type type = defaultTransform != null ? defaultTransform.DataType : nodeColumn.DataType;
        if (type == typeof (int) || type == typeof (long) || type == typeof (double) || type == typeof (DateTime))
          column.HeaderStyle.HorzAlignment = StringAlignment.Far;
        column.Changed += new EventHandler(this.SubstitutesTreeColumn_Changed);
        this._substitutesTree.Columns.Add(column);
      }
    }
    this._substitutesTree.Columns.Add(this._noteSubstitutesTreeColumn);
    this._positionsGrid.Grid.ColWidthChanging += new iGColWidthEventHandler(this.PositionsGrid_ColWidthChanging);
  }

  private void SubstitutesTreeColumn_Changed(object sender, EventArgs e)
  {
    Column substitutesTreeColumn = (Column) sender;
    iGCol iGcol = this._positionsGrid.Grid.Cols.Cast<iGCol>().FirstOrDefault<iGCol>((Func<iGCol, bool>) (o => o.Tag is NodeColumn tag && tag.Key == substitutesTreeColumn.DataField));
    if (iGcol == null)
      return;
    this._positionsGrid.Grid.ColWidthChanging -= new iGColWidthEventHandler(this.PositionsGrid_ColWidthChanging);
    try
    {
      iGcol.Width = substitutesTreeColumn.Width;
      NodeColumn nodeColumn = this._positionsGrid.GetNodeColumn(iGcol.Index);
      if (nodeColumn != null)
        nodeColumn.Width = substitutesTreeColumn.Width;
      this._positionsGrid.GridSaveState((Stream) null);
    }
    finally
    {
      this._positionsGrid.Grid.ColWidthChanging += new iGColWidthEventHandler(this.PositionsGrid_ColWidthChanging);
    }
  }

  private void PositionsGrid_ColWidthChanging(object sender, iGColWidthEventArgs e)
  {
    iGCol col = this._positionsGrid.Grid.Cols[e.ColIndex];
    NodeColumn nodeColumn;
    if (col == null || (nodeColumn = col.Tag as NodeColumn) == null)
      return;
    Column column = this._substitutesTree.Columns.FirstOrDefault<Column>((Func<Column, bool>) (o => o.DataField == nodeColumn.Key));
    if (column == null)
      return;
    column.Changed -= new EventHandler(this.SubstitutesTreeColumn_Changed);
    try
    {
      column.Width = col.Width;
    }
    finally
    {
      column.Changed += new EventHandler(this.SubstitutesTreeColumn_Changed);
    }
  }

  private void RebuildTree()
  {
    bool disableTreeEvents = this._disableTreeEvents;
    try
    {
      this._disableTreeEvents = true;
      if (this._substituteObjects != null)
      {
        this._substituteObjects.Groups.Sort();
        this._substituteObjects.SortPositions();
      }
      if (this._substsitutesRemarksService != null)
        this._remarkDictionaryByRelationID = this._substsitutesRemarksService.CalcSubstituteRemarks(this._substitutesSettings, this._substituteObjects);
      this._substitutesTree.UpdateRows(true);
    }
    finally
    {
      this._disableTreeEvents = disableTreeEvents;
    }
  }

  private ArticleRelationState CheckRelationsState(List<long> prjLinkIDs)
  {
    ArticleRelationState articleRelationState1 = ArticleRelationState.Unknown;
    if (this._articlesPartsPackage == null)
      return articleRelationState1;
    for (int index = 0; index < prjLinkIDs.Count; ++index)
    {
      ArticleRelationState articleRelationState2 = this.GetArticleRelationState(this._parentItem.ObjectID, prjLinkIDs[index]);
      if (articleRelationState1 != ArticleRelationState.Unknown && articleRelationState1 != articleRelationState2)
        return ArticleRelationState.Unknown;
      articleRelationState1 = articleRelationState2;
    }
    return articleRelationState1;
  }

  private void GatherSelectedInfo()
  {
    this._articleRelationState = ArticleRelationState.Unknown;
    this._hasSelectedGroups = false;
    this._hasSelectedSubstitutes = false;
    this._hasSelectedRelations = false;
    this._hasSelectedDesignerActualRelations = true;
    this._selectedSubstituteNumberDictionaryByGroupNumber.Clear();
    this._selectedGroupNumbers.Clear();
    this._selectedRelationIdsList.Clear();
    this._selectedGroupsCount = 0;
    this._selectedSubstitutesCount = 0;
    this._selectedRelationsCount = 0;
    this._selectedGroupNumber = -1L;
    this._selectedSubstituteNumber = -1L;
    int itemsCount = this._positionsGrid.ItemsCount;
    if (this._substitutesTree.SelectedRows != null && this._substitutesTree.SelectedRows.Count > 0)
    {
      for (int index = 0; index < this._substitutesTree.SelectedRows.Count; ++index)
      {
        if (this._substitutesTree.SelectedRows[index].Level == 1 && this._substitutesTree.SelectedRows[index].Item != null)
        {
          this._hasSelectedGroups = true;
          ++this._selectedGroupsCount;
          long key = (long) this._substitutesTree.SelectedRows[index].Item;
          if (!this._selectedGroupNumbers.Contains(key))
            this._selectedGroupNumbers.Add(key);
          if (!this._selectedSubstituteNumberDictionaryByGroupNumber.ContainsKey(key))
            this._selectedSubstituteNumberDictionaryByGroupNumber.Add(key, new List<long>());
        }
        if (this._substitutesTree.SelectedRows[index].Level == 2 && this._substitutesTree.SelectedRows[index].ParentRow.Item != null)
        {
          this._hasSelectedSubstitutes = true;
          ++this._selectedSubstitutesCount;
          long childIndex = (long) this._substitutesTree.SelectedRows[index].ChildIndex;
          long key = (long) this._substitutesTree.SelectedRows[index].ParentRow.Item;
          if (!this._selectedSubstituteNumberDictionaryByGroupNumber.ContainsKey(key))
            this._selectedSubstituteNumberDictionaryByGroupNumber.Add(key, new List<long>());
          this._selectedSubstituteNumberDictionaryByGroupNumber[key].Add(childIndex);
        }
        if (this._substitutesTree.SelectedRows[index].Level == 3 && this._substitutesTree.SelectedRows[index].Item != null)
        {
          this._hasSelectedRelations = true;
          ++this._selectedRelationsCount;
          long prjLinkID = (long) this._substitutesTree.SelectedRows[index].Item;
          this._hasSelectedDesignerActualRelations &= this._substituteObjects.IsRelationDesignerActualVariant(prjLinkID);
          if (!this._selectedRelationIdsList.Contains(prjLinkID))
            this._selectedRelationIdsList.Add(prjLinkID);
        }
      }
    }
    if (this._substitutesTree.SelectedRow != null)
    {
      Row row = this._substitutesTree.SelectedRow;
      if (row.Level == 2)
        this._selectedSubstituteNumber = (long) row.ChildIndex;
      for (; row.Level > 1 && row.ParentRow != null; row = row.ParentRow)
      {
        if (row.Level == 2)
          this._selectedSubstituteNumber = (long) row.ChildIndex;
      }
      if (row != null && row.Level == 1 && row.Item != null)
      {
        this._selectedGroupNumber = (long) row.Item;
        if (!this._selectedGroupNumbers.Contains(this._selectedGroupNumber))
          this._selectedGroupNumbers.Add(this._selectedGroupNumber);
      }
    }
    if (this._hasSelectedDesignerActualRelations && (this._selectedSubstitutesCount > 1 || this._selectedGroupsCount > 1))
      this._hasSelectedDesignerActualRelations = false;
    if (this._selectedRelationsCount == 0)
      this._hasSelectedDesignerActualRelations = false;
    this._articleRelationState = this.CheckRelationsState(this._selectedRelationIdsList);
  }

  private List<long> GatherSelectedRelations()
  {
    List<long> relations = new List<long>();
    if (this._disableTreeEvents || this._substitutesTree.SelectedRows == null || this._substitutesTree.SelectedRows.Count <= 0)
      return relations;
    for (int index = 0; index < this._substitutesTree.SelectedRows.Count; ++index)
    {
      if (this._substitutesTree.SelectedRows[index].Level == 1 && this._substitutesTree.SelectedRows[index].Item != null)
        this._substituteObjects.GatherRelations((long) this._substitutesTree.SelectedRows[index].Item, ref relations);
      if (this._substitutesTree.SelectedRows[index].Level == 2 && this._substitutesTree.SelectedRows[index].ParentRow.Item != null)
        this._substituteObjects.GatherRelations((long) this._substitutesTree.SelectedRows[index].ParentRow.Item, (long) this._substitutesTree.SelectedRows[index].ChildIndex, ref relations);
      if (this._substitutesTree.SelectedRows[index].Level == 3 && this._substitutesTree.SelectedRows[index].ParentRow.ParentRow.Item != null && this._substitutesTree.SelectedRows[index].Item != null && !relations.Contains((long) this._substitutesTree.SelectedRows[index].Item))
        relations.Add((long) this._substitutesTree.SelectedRows[index].Item);
    }
    return relations;
  }

  private void UpdateControls()
  {
    if (this._disableTreeEvents)
      return;
    this._autoExpandSubstitutesCheckBox.Enabled = this._autoExpandGroupsCheckBox.Checked;
    this._okButton.Enabled = this._isChanged && this._substitutionsEditorMode != SubstitutesEditorMode.ReadOnly;
    this._okButton.Visible = this._okButton.Enabled;
    this._cancelButton.Enabled = true;
    this.GatherSelectedInfo();
    int itemsCount = this._positionsGrid.ItemsCount;
    int count = this._positionsGrid.SelectedItems != null ? this._positionsGrid.SelectedItems.Count : 0;
    this._deleteButton.Enabled = this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode && (this._selectedSubstituteNumberDictionaryByGroupNumber.Count > 0 || this._hasSelectedGroups || this._hasSelectedSubstitutes || this._hasSelectedRelations);
    this._deleteButton.Visible = this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    this.mnpDelete.Enabled = this._deleteButton.Enabled;
    this.mnpDelete.Visible = this._deleteButton.Visible;
    this._createGroupButton.Enabled = itemsCount > 1 && this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    this._createGroupButton.Visible = this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    this.mnpAddGroup.Enabled = this._createGroupButton.Enabled;
    this.mnpAddGroup.Visible = this._createGroupButton.Visible;
    this._createAllowableSubstituteButton.Enabled = itemsCount > 1 && this._selectedGroupNumbers.Count == 1 && this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    this._createAllowableSubstituteButton.Visible = this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    this.mnpAddSubstitute.Enabled = this._createAllowableSubstituteButton.Enabled;
    this.mnpAddSubstitute.Visible = this._createAllowableSubstituteButton.Visible;
    this._actualizeSubstituteButton.Enabled = this._selectedGroupNumbers.Count == 1 && this._selectedSubstituteNumber > 0L && this._selectedSubstitutesCount == 1 && this._substitutionsEditorMode != SubstitutesEditorMode.ReadOnly;
    this._actualizeSubstituteButton.Visible = this._substitutionsEditorMode != SubstitutesEditorMode.ReadOnly;
    this.mnpActualizeSubstitute.Enabled = this._actualizeSubstituteButton.Enabled;
    this.mnpActualizeSubstitute.Visible = this._actualizeSubstituteButton.Visible;
    this._addToActualSubstituteButton.Enabled = count > 0 && this._selectedGroupNumbers.Count == 1 && this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    this._addToActualSubstituteButton.Visible = this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    this.mnpToActual.Enabled = this._addToActualSubstituteButton.Enabled;
    this.mnpToActual.Visible = this._addToActualSubstituteButton.Visible;
    this._addToAllowableSubstituteButton.Enabled = count > 0 && this._selectedGroupNumbers.Count == 1 && this._selectedSubstituteNumber >= 1L && this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    this._addToAllowableSubstituteButton.Visible = this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode;
    this.mnpToSubstitute.Enabled = this._addToAllowableSubstituteButton.Enabled;
    this.mnpToSubstitute.Visible = this._addToAllowableSubstituteButton.Visible;
    this.btnTrack.Enabled = count > 0 && !this.btWithoutComposition.Checked;
    this._markAuxiliaryPositionButtonItem.Checked = this.IsSelectedAuxiliaryPositions();
    this._markAuxiliaryPositionButtonItem.Enabled = this.CanMarkAuxiliaryOrEqualPosition();
    this._markEqualPositionButtonItem.Checked = this.IsSelectedEqualPositions();
    this._markEqualPositionButtonItem.Enabled = this._selectedSubstituteNumber >= 1L && this.CanMarkAuxiliaryOrEqualPosition();
    this._markDesignActualVariantButtonItem.Checked = this._markDesignActualVariantMenuButtonItem.Checked = this.IsSelectedDesignActualVariant();
    this._markDesignActualVariantButtonItem.Enabled = this._markDesignActualVariantMenuButtonItem.Enabled = this.CanMarkDesignActualVariant();
    this._moveUpButtonItem.Enabled = this.CanMoveUp();
    this._moveDownButtonItem.Enabled = this.CanMoveDown();
  }

  private bool CanMarkDesignActualVariant()
  {
    return this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode && this.IsSelectedSubstitute();
  }

  private bool CanMarkAuxiliaryOrEqualPosition()
  {
    return this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode && this.IsSelectedEditableAuxiliaryOrEqualPositions() && this._parentItem != null && this._parentItem.RelationType != -1 && SubstitutesHelper.IsAuxiliaryOrEqualPositionsSupported(this._parentItem.RelationType);
  }

  private bool CanMoveUp()
  {
    return this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode && this._selectedSubstituteGroupNumbers.Length == 0 && this._selectedSubstituteNumbers.Length == 0 && this._selectedRelationIds.Length != 0 && this._substitutesTree.SelectedRows.All<Row>((Func<Row, bool>) (o => o.ChildIndex > 0));
  }

  private bool CanMoveDown()
  {
    return this._substitutionsEditorMode == SubstitutesEditorMode.AdminMode && this._selectedSubstituteGroupNumbers.Length == 0 && this._selectedSubstituteNumbers.Length == 0 && this._selectedRelationIds.Length != 0 && this._substitutesTree.SelectedRows.All<Row>((Func<Row, bool>) (o => o.ChildIndex < o.ParentRow.ChildItems.Count - 1));
  }

  private void MoveUp()
  {
    foreach (KeyValuePair<Tuple<long, long>, List<long>> selectedRelationIds in this.GetSelectedRelationIdsDictionary())
      this._substituteObjects.MovePositionsUp(selectedRelationIds.Key.Item1, selectedRelationIds.Key.Item2, selectedRelationIds.Value.ToArray());
    this._isChanged = true;
    this.RebuildTree();
    this.UpdateControls();
  }

  private Dictionary<Tuple<long, long>, List<long>> GetSelectedRelationIdsDictionary()
  {
    Dictionary<Tuple<long, long>, List<long>> relationIdsDictionary = new Dictionary<Tuple<long, long>, List<long>>();
    foreach (Row selectedRow in this._substitutesTree.SelectedRows)
    {
      Tuple<long, long> key = new Tuple<long, long>((long) selectedRow.ParentRow.ParentRow.Item, (long) selectedRow.ParentRow.ChildIndex);
      List<long> longList = (List<long>) null;
      if (!relationIdsDictionary.TryGetValue(key, out longList))
      {
        longList = new List<long>();
        relationIdsDictionary.Add(key, longList);
      }
      longList.Add((long) selectedRow.Item);
    }
    return relationIdsDictionary;
  }

  private void MoveDown()
  {
    foreach (KeyValuePair<Tuple<long, long>, List<long>> selectedRelationIds in this.GetSelectedRelationIdsDictionary())
      this._substituteObjects.MovePositionsDown(selectedRelationIds.Key.Item1, selectedRelationIds.Key.Item2, selectedRelationIds.Value.ToArray());
    this._isChanged = true;
    this.RebuildTree();
    this.UpdateControls();
  }

  private string GetGroupSuffix(ArticleRelationState relationsState)
  {
    if (this._pdmSubstitutesEditorOptionsHolder.Form == AVSSpecificationForm.A)
    {
      if (relationsState == ArticleRelationState.CommonPart)
        return LocalizationHolder.rm.GetString("Pdm_537");
      if (relationsState == ArticleRelationState.VariablePart)
        return LocalizationHolder.rm.GetString("Pdm_538");
    }
    return string.Empty;
  }

  private Icon GetObjTypeIcon(int objTypeID)
  {
    objTypeID = Math.Max(objTypeID, -1);
    if (this._iconDictionaryByObjectTypeID.ContainsKey(objTypeID))
      return this._iconDictionaryByObjectTypeID[objTypeID];
    if (this._categoryTypeIconService.IndexOf(4, objTypeID) < 0)
      return (Icon) null;
    Icon icon = this._categoryTypeIconService.GetIcon(4, objTypeID);
    this._iconDictionaryByObjectTypeID.Add(objTypeID, icon);
    return icon;
  }

  private ISelectedItems GetSubstitutesSelectedItems()
  {
    if (this._disableTreeEvents)
      return (ISelectedItems) null;
    NodeIDCollection nodeIDs = new NodeIDCollection();
    List<long> longList = this.GatherSelectedRelations();
    NodeItems substitutesSelectedItems = new NodeItems(this._positionsGridRootNodeIDPath, this._positionsGridRootNode, nodeIDs, (IServiceProvider) this._advancedServiceContainer);
    for (int index = 0; index < longList.Count; ++index)
    {
      INodeID nodeId4RelationId = (INodeID) this.GetSubstitutesNodeID4RelationID(longList[index]);
      nodeIDs.Add(nodeId4RelationId);
    }
    return (ISelectedItems) substitutesSelectedItems;
  }

  private void CheckSubstitutes()
  {
    this._messageList.Messages.Clear();
    if (this._substitutionsEditorMode == SubstitutesEditorMode.ReadOnly)
    {
      this._messageList.Messages.Add(new _Message(_MessageType.Error, Strings.Error6));
    }
    else
    {
      foreach (long relationId in this._substituteObjects.GetRelationIds())
      {
        long groupNumber = this._substituteObjects.GetGroupNumber(relationId);
        long substituteNumber = this._substituteObjects.GetSubstituteNumber(relationId);
        if (groupNumber != -1L && substituteNumber != -1L)
        {
          long[] relationIdsInSubstitute = this._substituteObjects.GetRelationIdsInSubstitute(groupNumber, substituteNumber);
          long[] array = ((IEnumerable<long>) relationIdsInSubstitute).Where<long>((Func<long, bool>) (o => this._substituteObjects.IsAuxiliaryPosition(o))).ToArray<long>();
          if (array.Length != 0 && relationIdsInSubstitute.Length == array.Length)
            this._messageList.Messages.Add((_Message) new ArtSubstitutionsEditor.SubstitutesEditorMessage(_MessageType.Error, $"Все позиции в заменителе {groupNumber}.{substituteNumber} помечены как вспомогательные", (object) new object[2]
            {
              (object) groupNumber,
              (object) substituteNumber
            }));
        }
      }
      int index1 = 0;
      for (int count = this._substituteObjects.Groups.Count; index1 < count; ++index1)
      {
        List<List<long>> longListList = this._substituteObjects.Items[this._substituteObjects.Groups[index1]];
        long group = this._substituteObjects.Groups[index1];
        List<long> longList = longListList == null || longListList.Count <= 0 ? (List<long>) null : longListList[0];
        if (longList == null)
        {
          this._messageList.Messages.Add((_Message) new ArtSubstitutionsEditor.SubstitutesEditorMessage(_MessageType.Error, string.Format(Strings.Error0a, (object) group), (object) new object[1]
          {
            (object) group
          }));
        }
        else
        {
          if (longList.Count <= 0)
            this._messageList.Messages.Add((_Message) new ArtSubstitutionsEditor.SubstitutesEditorMessage(_MessageType.Error, string.Format(Strings.Error1, (object) group), (object) new object[2]
            {
              (object) group,
              (object) 0
            }));
          if (longListList.Count == 1)
            this._messageList.Messages.Add((_Message) new ArtSubstitutionsEditor.SubstitutesEditorMessage(_MessageType.Error, string.Format(Strings.Error2, (object) group), (object) new object[2]
            {
              (object) group,
              (object) 0
            }));
          for (int index2 = 1; index2 < longListList.Count; ++index2)
          {
            int num = index2;
            if (longListList[index2].Count <= 0)
              this._messageList.Messages.Add((_Message) new ArtSubstitutionsEditor.SubstitutesEditorMessage(_MessageType.Error, string.Format(Strings.Error3, (object) group, (object) index2), (object) new object[2]
              {
                (object) group,
                (object) num
              }));
          }
        }
      }
      if (this._messageList.Messages.IsEmpty)
      {
        this._messageList.Messages.Add(new _Message(_MessageType.Success, Strings.CheckSuccess));
        if (this._substitutionsEditorMode == SubstitutesEditorMode.ReadOnly)
          this._messageList.Messages.Add(new _Message(_MessageType.Information, Strings.Error8));
        if (this._substitutionsEditorMode != SubstitutesEditorMode.UserMode)
          return;
        this._messageList.Messages.Add(new _Message(_MessageType.Information, Strings.Error7));
      }
      else
      {
        if (this._substitutionsEditorMode == SubstitutesEditorMode.ReadOnly)
          this._messageList.Messages.Add(new _Message(_MessageType.Information, Strings.Error8));
        if (this._substitutionsEditorMode != SubstitutesEditorMode.UserMode)
          return;
        this._messageList.Messages.Add(new _Message(_MessageType.Information, Strings.Error7));
      }
    }
  }

  private void GotoSubstitute(long group, long substitute, List<long> relations)
  {
    if (this._disableTreeEvents)
      return;
    try
    {
      if (this._substituteObjects == null || group < 1L || !this._substituteObjects.Groups.Contains(group))
        return;
      this._substitutesTree.SelectedRows.Clear();
      Row row1 = (Row) null;
      for (int childIndex = 0; childIndex < this._substitutesTree.RootRow.NumChildren; ++childIndex)
      {
        if ((long) this._substitutesTree.RootRow.ChildRowByIndex(childIndex).Item == group)
        {
          row1 = this._substitutesTree.RootRow.ChildRowByIndex(childIndex);
          break;
        }
      }
      if (row1 == null)
        return;
      row1.EnsureVisible();
      row1.Expand();
      if (substitute < 0L || substitute > (long) (row1.NumChildren - 1))
      {
        row1.Selected = true;
      }
      else
      {
        Row row2 = row1.ChildRowByIndex(Convert.ToInt32(substitute));
        if (row2 == null)
          return;
        row2.EnsureVisible();
        row2.Expand();
        if (relations == null || relations.Count == 0)
        {
          row2.Selected = true;
        }
        else
        {
          for (int childIndex = 0; childIndex < row2.NumChildren; ++childIndex)
          {
            Row row3 = row2.ChildRowByIndex(childIndex);
            if (relations.Contains((long) row3.Item))
            {
              row3.Selected = true;
              row3.EnsureVisible();
            }
          }
        }
      }
    }
    finally
    {
      this.UpdateControls();
    }
  }

  private void GotoRelations(List<long> relations)
  {
    if (this._disableTreeEvents)
      return;
    try
    {
      this._substitutesTree.SelectedRows.Clear();
      for (int index = 0; index < relations.Count; ++index)
      {
        long Group;
        long SubstInGroup;
        if (this._substituteObjects.IndexOf(relations[index], out Group, out SubstInGroup))
        {
          Row row1 = (Row) null;
          for (int childIndex = 0; childIndex < this._substitutesTree.RootRow.NumChildren; ++childIndex)
          {
            if ((long) this._substitutesTree.RootRow.ChildRowByIndex(childIndex).Item == Group)
            {
              row1 = this._substitutesTree.RootRow.ChildRowByIndex(childIndex);
              break;
            }
          }
          if (row1 != null)
          {
            row1.EnsureVisible();
            row1.Expand();
            if (SubstInGroup >= 0L && SubstInGroup <= (long) (row1.NumChildren - 1))
            {
              Row row2 = row1.ChildRowByIndex(Convert.ToInt32(SubstInGroup));
              if (row2 != null)
              {
                row2.EnsureVisible();
                row2.Expand();
                for (int childIndex = 0; childIndex < row2.NumChildren; ++childIndex)
                {
                  Row row3 = row2.ChildRowByIndex(childIndex);
                  if (relations.Contains((long) row3.Item))
                  {
                    row3.Selected = true;
                    row3.EnsureVisible();
                  }
                }
              }
            }
          }
        }
      }
    }
    finally
    {
      this.UpdateControls();
    }
  }

  private List<long> GetRelationsFromItems(ISelectedItems items)
  {
    List<long> relationsFromItems = new List<long>();
    if (items == null || items.Count == 0)
      return relationsFromItems;
    for (int index = 0; index < items.Count; ++index)
    {
      if (items.GetItemData(index, typeof (IDBRelationID)) is IDBRelationID itemData)
        relationsFromItems.Add(itemData.Value);
    }
    return relationsFromItems;
  }

  private void AddRelationsToSubstitute(
    long groupNumber,
    long substituteNumber,
    List<long> relationIds)
  {
    this._messageList.Messages.Clear();
    List<long> longList1 = new List<long>();
    List<long> longList2 = new List<long>();
    List<long> relations = new List<long>();
    this._substituteObjects.GatherRelations(groupNumber, ref relations);
    ArticleRelationState articleRelationState = relations.Count > 0 ? this.CheckRelationsState(relations) : ArticleRelationState.Unknown;
    int index1 = 0;
    for (int count = relationIds.Count; index1 < count; ++index1)
    {
      long num = relationIds[index1];
      ArtSubstitutionsEditor.Relation prototypeRelationId = this.CreateNewRelationWithPrototypeRelationID(groupNumber, substituteNumber, num);
      if (this._substituteObjects.IndexOf(num) > 0L)
        num = prototypeRelationId.ID;
      if (relations.Count > 0)
      {
        if (this._articlesPartsPackage != null && articleRelationState != this.GetArticleRelationState(this._parentItem.ObjectID, num))
        {
          longList2.Add(num);
          continue;
        }
      }
      else
      {
        relations.Add(num);
        articleRelationState = this.CheckRelationsState(relations);
      }
      this._substituteObjects.AddRelation(groupNumber, substituteNumber, num);
      this._substituteObjects.SetObjectID(num, prototypeRelationId.PartID);
      SubstitutesNodeID nodeId4RelationId = this.GetSubstitutesNodeID4RelationID(num);
      string positionNumber = $"{-1L}";
      if (nodeId4RelationId != null)
      {
        List<NodeColumnID> attributes = nodeId4RelationId.Attributes;
        object[] values = nodeId4RelationId.Values;
        for (int index2 = 0; index2 < attributes.Count; ++index2)
        {
          NodeColumnID nodeColumnId = attributes[index2];
          if (nodeColumnId.AttributeID != SubstitutesConstants.SubstituteGroupNumberAttributeTypeID && nodeColumnId.AttributeID != SubstitutesConstants.SubstituteNumberAttributeTypeID)
          {
            if (nodeColumnId.AttributeID == Intermech.Pdm.Substitutes.Constants.PositionAttributeTypeID)
              positionNumber = values[index2] == null ? $"{-1L}" : Convert.ToString(values[index2]);
            this._substituteObjects.SetRelationAttributeValue(num, nodeColumnId.AttributeID, values[index2]);
          }
        }
      }
      this._substituteObjects.SetAuxiliaryFlagIfNeed(num, prototypeRelationId.PartID, positionNumber);
      string substGroupName = this._substituteObjects.GetSubstGroupName(groupNumber);
      this._substituteObjects.SetSubstGroupName(groupNumber, substGroupName);
    }
    this.RebuildTree();
    this._positionsGrid.RebuildVirtualGrid(this._articlesPartsPackage);
    this._isChanged = true;
    this.UpdateControls();
    for (int index3 = longList1.Count - 1; index3 >= 0; --index3)
    {
      long Group;
      long SubstInGroup;
      this._substituteObjects.IndexOf(longList1[index3], out Group, out SubstInGroup);
      SubstitutesNodeID nodeId4RelationId = this.GetSubstitutesNodeID4RelationID(longList1[index3]);
      this._messageList.Messages.Insert(0, (_Message) new ArtSubstitutionsEditor.SubstitutesEditorMessage(_MessageType.Error, string.Format(Strings.Hint4, (object) nodeId4RelationId.ObjectID, (object) nodeId4RelationId.Caption, (object) Group, (object) SubstInGroup), (object) new object[3]
      {
        (object) Group,
        (object) SubstInGroup,
        (object) new List<long>(1) { longList1[index3] }
      }));
    }
    string format = Strings.Hint5;
    if (articleRelationState == ArticleRelationState.CommonPart)
      format = Strings.Hint6;
    for (int index4 = longList2.Count - 1; index4 >= 0; --index4)
    {
      long Group;
      long SubstInGroup;
      this._substituteObjects.IndexOf(longList2[index4], out Group, out SubstInGroup);
      SubstitutesNodeID nodeId4RelationId = this.GetSubstitutesNodeID4RelationID(longList2[index4]);
      List<long> longList3 = new List<long>(1);
      longList3.Add(longList2[index4]);
      if (nodeId4RelationId != null)
        this._messageList.Messages.Insert(0, (_Message) new ArtSubstitutionsEditor.SubstitutesEditorMessage(_MessageType.Error, string.Format(format, (object) nodeId4RelationId.ObjectID, (object) nodeId4RelationId.Caption), (object) new object[3]
        {
          (object) Group,
          (object) SubstInGroup,
          (object) longList3
        }));
    }
  }

  private void DoAddToNewActualSubstitute()
  {
    if (this._substitutionsEditorMode != SubstitutesEditorMode.AdminMode)
      return;
    this.GatherSelectedInfo();
    int itemsCount = this._positionsGrid.ItemsCount;
    int count = this._positionsGrid.SelectedItems != null ? this._positionsGrid.SelectedItems.Count : 0;
    if (itemsCount < 2 || count == 0)
      return;
    List<long> relationsFromComposition = this._positionsGrid.SelectedRelationsFromComposition;
    long num = this._substituteObjects.NewGroup(this._desiredNewGroupNumber);
    this._substituteObjects.Groups.Sort();
    this._substituteObjects.NewSubstitute(num, 0L);
    this._substituteObjects.NewSubstitute(num, 1L);
    this.AddRelationsToSubstitute(num, 0L, relationsFromComposition);
    this.GotoSubstitute(num, 0L, relationsFromComposition);
  }

  private void CorrectDesignActualVariant()
  {
    for (int index1 = 0; index1 < this._substituteObjects.Groups.Count; ++index1)
    {
      List<List<long>> longListList = this._substituteObjects.Items[this._substituteObjects.Groups[index1]];
      long group = this._substituteObjects.Groups[index1];
      bool flag = false;
      int substNo = -1;
      for (int index2 = 0; index2 < longListList.Count; ++index2)
      {
        List<long> longList = longListList[index2];
        for (int index3 = 0; index3 < longList.Count; ++index3)
        {
          flag |= this._substituteObjects.IsRelationDesignerActualVariant(longList[index3]);
          if (flag)
            break;
        }
        if (flag)
        {
          substNo = index2;
          break;
        }
      }
      this._substituteObjects.SetGroupAttrValue(group, SubstituteObjects.attrDesignActualVariant, (object) DBNull.Value);
      if (flag)
        this._substituteObjects.SetSubstAttrValue(group, (long) substNo, SubstituteObjects.attrDesignActualVariant, (object) 1L);
      this.RebuildTree();
      this._positionsGrid.RebuildVirtualGrid(this._articlesPartsPackage);
    }
  }

  private bool WriteDatabaseInfo()
  {
    if (this._substitutionsEditorMode == SubstitutesEditorMode.ReadOnly)
      return false;
    SaveSubstitutesParams substitutesParams = new SaveSubstitutesParams()
    {
      Pack = this.GetSubstitutePack(),
      ProjectVersionID = this._parentItem.ObjectID,
      RelationTypeID = this._parentItem.RelationType,
      GroupsAffected = this._substituteObjects.GroupsAffected
    };
    List<long> longList = this.FindInstancesVersionIds(this._parentItem.ObjectID) ?? new List<long>(0);
    if (longList.Count > 1)
      substitutesParams.InstanceVersionIds = longList.ToArray();
    if (longList.Count > 1 && (this._pdmSubstitutesEditorOptionsHolder.Form == AVSSpecificationForm.B || this._pdmSubstitutesEditorOptionsHolder.Form == AVSSpecificationForm.A && this._pdmSubstitutesEditorOptionsHolder.Mode == PDMSubstitutesEditorMode.DialogMultiInstances))
    {
      Dictionary<long, RelationAttributesPackage> forArtSelectForm = this.CreateRelationAttributesPackageDictionaryForArtSelectForm(substitutesParams);
      if (ArtSelectForm.Execute((IServiceProvider) this._advancedServiceContainer, ref forArtSelectForm, "cad001e2-306c-11d8-b4e9-00304f19f545", this._parentItem.RelationType, this._contextIds, this._additionalNodeColumnIds, this._parentItem.ObjectID) != DialogResult.OK || forArtSelectForm.Count == 0)
        return false;
      substitutesParams.InstanceVersionIds = forArtSelectForm.Keys.ToArray<long>();
    }
    ((ISubstitutesClientService) ServicesManager.GetService(typeof (ISubstitutesClientService))).SaveSubstitutes(substitutesParams);
    return true;
  }

  private List<long> FindInstancesVersionIds(long objectVersionID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return (sessionKeeper.Session.GetCustomService(typeof (IArticleService)) as IArticleService).GetListInstances(objectVersionID, (object) sessionKeeper.Session.SessionGUID);
  }

  private Dictionary<long, RelationAttributesPackage> CreateRelationAttributesPackageDictionaryForArtSelectForm(
    SaveSubstitutesParams saveSubstitutesParams)
  {
    Dictionary<long, RelationAttributesPackage> forArtSelectForm = new Dictionary<long, RelationAttributesPackage>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (KeyValuePair<long, AnalyzeSaveSubsitutesResult.SaveSubsitutesChangesPack> changesPack in (sessionKeeper.Session.GetCustomService(typeof (ISubstitutesServerService)) as ISubstitutesServerService).AnalyzeSaveSubstitutes(sessionKeeper.Session.SessionGUID, saveSubstitutesParams).ChangesPackDictionary)
      {
        long key = changesPack.Key;
        AnalyzeSaveSubsitutesResult.SaveSubsitutesChangesPack subsitutesChangesPack = changesPack.Value;
        RelationAttributesPackage attributesPackage = new RelationAttributesPackage(new List<int>()
        {
          SubstitutesConstants.SubstituteGroupNumberAttributeTypeID,
          SubstitutesConstants.SubstituteGroupNameAttributeTypeID,
          SubstitutesConstants.SubstituteNumberAttributeTypeID,
          SubstitutesConstants.SubstituteNameAttributeTypeID
        });
        foreach (Intermech.Search.Relation toAddRelation in subsitutesChangesPack.ToAddRelations)
          attributesPackage.Values[toAddRelation.ID] = new object[4]
          {
            toAddRelation.Attributes.GetAttributeValue(SubstitutesConstants.SubstituteGroupNumberAttributeTypeID),
            toAddRelation.Attributes.GetAttributeValue(SubstitutesConstants.SubstituteGroupNameAttributeTypeID),
            toAddRelation.Attributes.GetAttributeValue(SubstitutesConstants.SubstituteNumberAttributeTypeID),
            toAddRelation.Attributes.GetAttributeValue(SubstitutesConstants.SubstituteNameAttributeTypeID)
          };
        foreach (Intermech.Search.Relation toChangeRelation in subsitutesChangesPack.ToChangeRelations)
        {
          object attributeValue1 = toChangeRelation.Attributes.GetAttributeValue(SubstitutesConstants.SubstituteGroupNumberAttributeTypeID);
          object attributeValue2 = toChangeRelation.Attributes.GetAttributeValue(SubstitutesConstants.SubstituteNumberAttributeTypeID);
          if (attributeValue1 != null && attributeValue2 != null)
            attributesPackage.Values[toChangeRelation.ID] = new object[4]
            {
              attributeValue1,
              toChangeRelation.Attributes.GetAttributeValue(SubstitutesConstants.SubstituteGroupNameAttributeTypeID),
              attributeValue2,
              toChangeRelation.Attributes.GetAttributeValue(SubstitutesConstants.SubstituteNameAttributeTypeID)
            };
        }
        foreach (long toClearRelationId in subsitutesChangesPack.ToClearRelationIds)
          attributesPackage.Values[toClearRelationId] = (object[]) null;
        forArtSelectForm[key] = attributesPackage;
      }
    }
    return forArtSelectForm;
  }

  private SubstitutePack GetSubstitutePack()
  {
    SubstitutePack substitutePack = new SubstitutePack();
    foreach (long relationId in this._substituteObjects.GetRelationIds())
    {
      if (this._substituteObjects.GetGroupNumber(relationId) != -1L)
      {
        SubstituteGroup substituteGroup = substitutePack.Groups[this._substituteObjects.GetGroupNumber(relationId)];
        if (substituteGroup == null)
        {
          substituteGroup = new SubstituteGroup();
          substituteGroup.Number = this._substituteObjects.GetGroupNumber(relationId);
          substituteGroup.Name = this._substituteObjects.GetSubstGroupName(this._substituteObjects.GetGroupNumber(relationId));
          substitutePack.Groups.Add(substituteGroup);
        }
        Substitute substitute = substituteGroup.Substitutes[this._substituteObjects.GetSubstituteNumber(relationId)];
        if (substitute == null)
        {
          substitute = new Substitute();
          substitute.Number = this._substituteObjects.GetSubstituteNumber(relationId);
          substitute.Name = $"{this._substituteObjects.GetGroupNumber(relationId)}.{this._substituteObjects.GetSubstituteNumber(relationId)}";
          substitute.IsDesignerActualVariant = this._substituteObjects.IsDesignActualVariant(this._substituteObjects.GetGroupNumber(relationId), this._substituteObjects.GetSubstituteNumber(relationId));
          substituteGroup.Substitutes.Add(substitute);
        }
        substitute.Positions.Add(new SubstitutePosition(relationId, this._substituteObjects.GetObjectID(relationId))
        {
          IsAuxiliary = this._substituteObjects.IsAuxiliaryPosition(relationId),
          IsEqual = this._substituteObjects.IsEqualPosition(relationId),
          ObjectVersionID = this._substituteObjects.GetObjectVersionID(relationId),
          Number = this._substituteObjects.GetPositionNumber(relationId)
        });
      }
    }
    return substitutePack;
  }

  private void PrepareProcessedRelations()
  {
    foreach (SubstitutesNodeID substitutesNodeId in this._positionsGrid.GetSubstitutesNodeIds())
      this._relations.Add(this.CreateRelationFromSubstitutesNodeID(substitutesNodeId));
  }

  private ArtSubstitutionsEditor.Relation CreateRelationFromSubstitutesNodeID(
    SubstitutesNodeID substitutesNodeID)
  {
    return new ArtSubstitutionsEditor.Relation()
    {
      ID = substitutesNodeID.PrjLinkID,
      Guid = substitutesNodeID.RelGuid,
      PartID = substitutesNodeID.ID,
      ProjectVersionID = substitutesNodeID.ProjID,
      TypeID = substitutesNodeID.RelationTypeID,
      PartVersionID = substitutesNodeID.ObjectID
    };
  }

  private SubstitutesNodeID GetSubstitutesNodeID4RelationID(long relationID)
  {
    SubstitutesNodeID nodeId4RelationId = this._positionsGrid[relationID];
    if (nodeId4RelationId == null)
    {
      ArtSubstitutionsEditor.Relation relation = this._relations.FirstOrDefault<ArtSubstitutionsEditor.Relation>((Func<ArtSubstitutionsEditor.Relation, bool>) (o => o.ID == relationID));
      nodeId4RelationId = relation != null ? this._positionsGrid[relation.PrototypeID] : (SubstitutesNodeID) null;
    }
    return nodeId4RelationId;
  }

  private bool CheckRelationDuplicates(long groupNumber, long substituteNumber, long relationID)
  {
    ArtSubstitutionsEditor.Relation relation = this._relations.FirstOrDefault<ArtSubstitutionsEditor.Relation>((Func<ArtSubstitutionsEditor.Relation, bool>) (o => o.ID == relationID));
    long[] relationIds = this._substituteObjects.GetRelationIds();
    long[] array = ((IEnumerable<long>) relationIds).Where<long>((Func<long, bool>) (o => this._substituteObjects.GetObjectID(o) == relation.PartID)).ToArray<long>();
    if (array.Length == 0)
      return false;
    foreach (long num in array)
    {
      long duplicateRelationID = num;
      if (this._substituteObjects.GetGroupNumber(duplicateRelationID) == groupNumber && this._substituteObjects.GetSubstituteNumber(duplicateRelationID) == substituteNumber)
        return true;
      IEnumerable<long> source = ((IEnumerable<long>) relationIds).Where<long>((Func<long, bool>) (o => this._substituteObjects.GetGroupNumber(duplicateRelationID) == this._substituteObjects.GetGroupNumber(o) && this._substituteObjects.GetSubstituteNumber(o) != 0L));
      if (this._substituteObjects.GetGroupNumber(duplicateRelationID) == groupNumber && source.Count<long>() == 0 && this._substituteObjects.GetSubstituteNumber(duplicateRelationID) == 0L)
        return true;
    }
    return false;
  }

  private ArtSubstitutionsEditor.Relation CreateNewRelationWithPrototypeRelationID(
    long groupNumber,
    long substituteNumber,
    long relationID)
  {
    ArtSubstitutionsEditor.Relation relation = this._relations.FirstOrDefault<ArtSubstitutionsEditor.Relation>((Func<ArtSubstitutionsEditor.Relation, bool>) (o => o.ID == relationID));
    ArtSubstitutionsEditor.Relation prototypeRelationId;
    if (this._substituteObjects.IndexOf(relationID) > 0L)
    {
      prototypeRelationId = new ArtSubstitutionsEditor.Relation()
      {
        ID = this.CreateUniqueRelationID(),
        PartID = relation.PartID,
        ProjectVersionID = relation.ProjectVersionID,
        PrototypeID = relationID,
        TypeID = relation.TypeID,
        PartVersionID = relation.PartVersionID
      };
      this._relations.Add(prototypeRelationId);
    }
    else
      prototypeRelationId = relation;
    return prototypeRelationId;
  }

  private long CreateUniqueRelationID()
  {
    ArtSubstitutionsEditor.Relation relation = this._relations.OrderBy<ArtSubstitutionsEditor.Relation, long>((Func<ArtSubstitutionsEditor.Relation, long>) (o => o.ID)).First<ArtSubstitutionsEditor.Relation>();
    return relation.ID > -2L ? -2L : relation.ID - 1L;
  }

  private void RemoveGroup(long number)
  {
    this.RemoveCreatedRelations(this._substituteObjects.GetRelationIdsInGroup(number));
    this._substituteObjects.RemoveGroup(number);
  }

  private void RemoveCreatedRelations(long[] relationIds)
  {
    foreach (long relationId in relationIds)
    {
      long relationID = relationId;
      ArtSubstitutionsEditor.Relation relation = this._relations.FirstOrDefault<ArtSubstitutionsEditor.Relation>((Func<ArtSubstitutionsEditor.Relation, bool>) (o => o.ID == relationID && !RelationHelper.IsUnknownRelationID(o.PrototypeID)));
      if (relation != null)
        this._relations.Remove(relation);
    }
  }

  private void RemoveSubstitute(long groupNumber, long substituteNumber)
  {
    this.RemoveCreatedRelations(this._substituteObjects.GetRelationIdsInSubstitute(groupNumber, substituteNumber));
    this._substituteObjects.RemoveSubstitute(groupNumber, substituteNumber);
  }

  private void RemoveRelations(List<long> relationIds)
  {
    this._substituteObjects.RemoveRelations(relationIds);
    this.RemoveCreatedRelations(relationIds.ToArray());
  }

  private ArticleRelationState GetArticleRelationState(long articleVersionID, long relationID)
  {
    ArtSubstitutionsEditor.Relation relation = this._relations.FirstOrDefault<ArtSubstitutionsEditor.Relation>((Func<ArtSubstitutionsEditor.Relation, bool>) (o => o.ID == relationID));
    relationID = relation.PrototypeID == 0L ? relationID : relation.PrototypeID;
    return this._articlesPartsPackage.GetRelationState(articleVersionID, relationID);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.tbComposition.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.tbMessages.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.tbSubstitutes.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.menuComposition.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.menuSubstitutes.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged -= new EventHandler(this.BarManager_RendererChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ArtSubstitutionsEditor));
    this.imageNewDesign = new ImageList(this.components);
    this.imagesMenus = new ImageList(this.components);
    this.imagesToolbars = new ImageList(this.components);
    this.imagesList = new ImageList(this.components);
    this.imagesTreeview = new ImageList(this.components);
    this.panelBottom = new Panel();
    this._autoExpandSubstitutesCheckBox = new CheckBox();
    this._autoExpandGroupsCheckBox = new CheckBox();
    this._cancelButton = new Button();
    this._okButton = new Button();
    this._positionsGrid = new SubstitutesView();
    this.documentContainer = new DocumentContainer();
    this.dockSubstitutes = new DockControl();
    this._substitutesTree = new Intermech.VirtualTreeView.VirtualTreeView();
    this._captionSubstitutesTreeColumn = new Column();
    this.groupNameEditor = new CellEditor();
    this.textBoxGroupNameEditor = new TextBox();
    this._noteSubstitutesTreeColumn = new Column();
    this.tbSubstitutes = new Intermech.Bars.ToolBar();
    this._createGroupButton = new ButtonItem();
    this._createAllowableSubstituteButton = new ButtonItem();
    this._markAuxiliaryPositionButtonItem = new ButtonItem();
    this._markEqualPositionButtonItem = new ButtonItem();
    this._deleteButton = new ButtonItem();
    this._actualizeSubstituteButton = new ButtonItem();
    this._checkSubstitutesButton = new ButtonItem();
    this.btnTracing = new ButtonItem();
    this._markDesignActualVariantButtonItem = new ButtonItem();
    this._moveUpButtonItem = new ButtonItem();
    this._moveDownButtonItem = new ButtonItem();
    this.panelLegend = new Panel();
    this.labelEqualPosition = new Label();
    this._equalPositionColorPanel = new Panel();
    this._auxiliaryPositionColorPanel = new Panel();
    this._deignActualVariantColorPanel = new Panel();
    this.label1 = new Label();
    this.labelDesignerSubstitute = new Label();
    this.menuSubstitutes = new MenuBar();
    this.contextMenuSubstitutes = new ContextMenuBarItem();
    this.mnpAddGroup = new MenuButtonItem();
    this.mnpAddSubstitute = new MenuButtonItem();
    this.mnpDelete = new MenuButtonItem();
    this.mnpActualizeSubstitute = new MenuButtonItem();
    this.mnpCheck = new MenuButtonItem();
    this._markDesignActualVariantMenuButtonItem = new MenuButtonItem();
    this.dockComposition = new DockControl();
    this.menuComposition = new MenuBar();
    this.contextMenuComposition = new ContextMenuBarItem();
    this.mnpTrack = new MenuButtonItem();
    this.mnpToActual = new MenuButtonItem();
    this.mnpToSubstitute = new MenuButtonItem();
    this.mnpVirtualComposition = new MenuButtonItem();
    this.mnpCheck2 = new MenuButtonItem();
    this.mnpColumnsSetup = new MenuButtonItem();
    this.tbComposition = new Intermech.Bars.ToolBar();
    this._addToActualSubstituteButton = new ButtonItem();
    this._addToAllowableSubstituteButton = new ButtonItem();
    this.btVirtualComposition = new DropDownMenuItem();
    this.btDefault = new MenuButtonItem();
    this.btActual = new MenuButtonItem();
    this.btWithoutComposition = new MenuButtonItem();
    this.btnTrack = new ButtonItem();
    this.bottomLeft = new DockContainer();
    this.leftDock = new DockContainer();
    this.dockManager = new DockManager();
    this.rightDock = new DockContainer();
    this.bottomDock = new DockContainer();
    this.topDock = new DockContainer();
    this.dockContainer = new DockContainer();
    this.dockMessages = new DockControl();
    this._messageList = new MessageList();
    this.tbMessages = new Intermech.Bars.ToolBar();
    this.btClear = new ButtonItem();
    this.panelBottom.SuspendLayout();
    this.documentContainer.SuspendLayout();
    this.dockSubstitutes.SuspendLayout();
    this._substitutesTree.BeginInit();
    this.panelLegend.SuspendLayout();
    this.dockComposition.SuspendLayout();
    this.dockContainer.SuspendLayout();
    this.dockMessages.SuspendLayout();
    this.SuspendLayout();
    this.imageNewDesign.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageNewDesign.ImageStream");
    this.imageNewDesign.TransparentColor = Color.Transparent;
    this.imageNewDesign.Images.SetKeyName(0, "add_to_main.ico");
    this.imageNewDesign.Images.SetKeyName(1, "add_to_alt.ico");
    this.imageNewDesign.Images.SetKeyName(2, "Карточка.png");
    this.imageNewDesign.Images.SetKeyName(3, "only_main.ico");
    this.imageNewDesign.Images.SetKeyName(4, "refresh.ico");
    this.imageNewDesign.Images.SetKeyName(5, "group.ico");
    this.imageNewDesign.Images.SetKeyName(6, "add_alt.ico");
    this.imageNewDesign.Images.SetKeyName(7, "del.ico");
    this.imageNewDesign.Images.SetKeyName(8, "make_main.ico");
    this.imageNewDesign.Images.SetKeyName(9, "look_err.ico");
    this.imageNewDesign.Images.SetKeyName(10, "look_comp.ico");
    this.imageNewDesign.Images.SetKeyName(11, "make_des.ico");
    this.imageNewDesign.Images.SetKeyName(12, "clean_err.ico");
    this.imageNewDesign.Images.SetKeyName(13, "main.ico");
    this.imageNewDesign.Images.SetKeyName(14, "alt.ico");
    this.imageNewDesign.Images.SetKeyName(15, "Настройка отображения.ico");
    this.imageNewDesign.Images.SetKeyName(16 /*0x10*/, "composition.ico");
    this.imageNewDesign.Images.SetKeyName(17, "without_substitutes.ico");
    this.imageNewDesign.Images.SetKeyName(18, "вспомогательная_позиция.png");
    this.imageNewDesign.Images.SetKeyName(19, "отметить_равнозначные.png");
    this.imageNewDesign.Images.SetKeyName(20, "вверх.png");
    this.imageNewDesign.Images.SetKeyName(21, "вниз.png");
    this.imagesMenus.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesMenus.ImageStream");
    this.imagesMenus.TransparentColor = Color.Transparent;
    this.imagesMenus.Images.SetKeyName(0, "table_add_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(1, "table_delete_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(2, "index_add_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(3, "index_delete_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(4, "index_preferences_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(5, "index_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(6, "index_down_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(7, "index_up_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(8, "row_delete_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(9, "substitutes_actual_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(10, "row_add_before_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(11, "tables2_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(12, "row_add_after_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(13, "tables_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(14, "row_add_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(15, "table_view_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(16 /*0x10*/, "properties_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(17, "Настройка отображения.ico");
    this.imagesMenus.Images.SetKeyName(18, "specification_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(19, "index_view_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(20, "table_replace_16x16x256.ico");
    this.imagesMenus.Images.SetKeyName(21, "substsico.ico");
    this.imagesToolbars.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolbars.ImageStream");
    this.imagesToolbars.TransparentColor = Color.Transparent;
    this.imagesToolbars.Images.SetKeyName(0, "table_add_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(1, "table_delete_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(2, "index_add_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(3, "index_delete_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(4, "index_preferences_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(5, "index_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(6, "index_down_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(7, "index_up_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(8, "row_delete_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(9, "substitutes_actual_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(10, "row_add_before_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(11, "tables2_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(12, "row_add_after_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(13, "tables_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(14, "row_add_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(15, "table_view_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(16 /*0x10*/, "properties_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(17, "Настройка отображения.ico");
    this.imagesToolbars.Images.SetKeyName(18, "specification_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(19, "index_view_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(20, "table_replace_24x24x256.ico");
    this.imagesToolbars.Images.SetKeyName(21, "substsico.ico");
    this.imagesList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesList.ImageStream");
    this.imagesList.TransparentColor = Color.Transparent;
    this.imagesList.Images.SetKeyName(0, "check_16x16x256.ico");
    this.imagesList.Images.SetKeyName(1, "information_16x16x256.ico");
    this.imagesList.Images.SetKeyName(2, "warning_16x16x256.ico");
    this.imagesList.Images.SetKeyName(3, "delete_16x16x256.ico");
    this.imagesList.Images.SetKeyName(4, "document_plain.png");
    this.imagesTreeview.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesTreeview.ImageStream");
    this.imagesTreeview.TransparentColor = Color.Transparent;
    this.imagesTreeview.Images.SetKeyName(0, "group.ico");
    this.imagesTreeview.Images.SetKeyName(1, "main.ico");
    this.imagesTreeview.Images.SetKeyName(2, "alt.ico");
    this.panelBottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
    this.panelBottom.Controls.Add((Control) this._autoExpandSubstitutesCheckBox);
    this.panelBottom.Controls.Add((Control) this._autoExpandGroupsCheckBox);
    this.panelBottom.Controls.Add((Control) this._cancelButton);
    this.panelBottom.Controls.Add((Control) this._okButton);
    componentResourceManager.ApplyResources((object) this.panelBottom, "panelBottom");
    this.panelBottom.Name = "panelBottom";
    componentResourceManager.ApplyResources((object) this._autoExpandSubstitutesCheckBox, "_autoExpandSubstitutesCheckBox");
    this._autoExpandSubstitutesCheckBox.Checked = true;
    this._autoExpandSubstitutesCheckBox.CheckState = CheckState.Checked;
    this._autoExpandSubstitutesCheckBox.Cursor = Cursors.Hand;
    this._autoExpandSubstitutesCheckBox.Name = "_autoExpandSubstitutesCheckBox";
    this._autoExpandSubstitutesCheckBox.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._autoExpandGroupsCheckBox, "_autoExpandGroupsCheckBox");
    this._autoExpandGroupsCheckBox.Checked = true;
    this._autoExpandGroupsCheckBox.CheckState = CheckState.Checked;
    this._autoExpandGroupsCheckBox.Cursor = Cursors.Hand;
    this._autoExpandGroupsCheckBox.Name = "_autoExpandGroupsCheckBox";
    this._autoExpandGroupsCheckBox.UseVisualStyleBackColor = true;
    this._autoExpandGroupsCheckBox.CheckedChanged += new EventHandler(this.AutoExpandGroupsCheckBox_CheckedChanged);
    componentResourceManager.ApplyResources((object) this._cancelButton, "_cancelButton");
    this._cancelButton.Cursor = Cursors.Hand;
    this._cancelButton.DialogResult = DialogResult.Cancel;
    this._cancelButton.Name = "_cancelButton";
    componentResourceManager.ApplyResources((object) this._okButton, "_okButton");
    this._okButton.Cursor = Cursors.Hand;
    this._okButton.Name = "_okButton";
    this._okButton.Click += new EventHandler(this.OKButon_Click);
    this._positionsGrid.AllowCustomGroupValues = true;
    this._positionsGrid.AllowDrop = true;
    this._positionsGrid.BackColor = SystemColors.Control;
    this._positionsGrid.Control = (object) this._positionsGrid;
    this._positionsGrid.DisableColumnsGrouping = true;
    this._positionsGrid.DisableDelayedUpdates = true;
    this._positionsGrid.DisableDoubleClicks = true;
    this._positionsGrid.DisableGroupBox = true;
    this._positionsGrid.DisableHeaderContextMenu = true;
    this._positionsGrid.DisableIMContextMenu = true;
    this._positionsGrid.DisableKeyDownEvents = false;
    this._positionsGrid.DisableKeyUpEvents = true;
    this._positionsGrid.DisablePacketsReading = true;
    this._positionsGrid.DisableParentSelectedItems = true;
    this._positionsGrid.DisableStatusBar = true;
    this._positionsGrid.DisableToolBar = true;
    componentResourceManager.ApplyResources((object) this._positionsGrid, "_positionsGrid");
    this._positionsGrid.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this._positionsGrid.Name = "_positionsGrid";
    this._positionsGrid.Remarks = (RelationAttributesPackage) null;
    this._positionsGrid.SubstitutesVirtual = (SubstituteObjects) null;
    this._positionsGrid.ViewContentType = ContentType.Folders | ContentType.NonFolders;
    this._positionsGrid.ShowCustomContextMenu += new EventHandler<ContextMenuEventArgs>(this.PositionsGrid_ShowCustomContextMenu);
    this._positionsGrid.GridDragDrop += new EventHandler<DragEventArgs>(this.PositionsGrid_GridDragDrop);
    this._positionsGrid.SelectedItemsChanged += new EventHandler(this.PositionsGrid_SelectedItemsChanged);
    this.documentContainer.Controls.Add((Control) this.dockSubstitutes);
    this.documentContainer.Controls.Add((Control) this.dockComposition);
    this.documentContainer.Cursor = Cursors.Default;
    this.documentContainer.DockingManager = DockingManager.Whidbey;
    componentResourceManager.ApplyResources((object) this.documentContainer, "documentContainer");
    this.documentContainer.Guid = new Guid("05cb8464-6bff-43ae-966e-0eff42ce1735");
    this.documentContainer.LayoutSystem = new SplitLayoutSystem(250, 400, Orientation.Horizontal, new LayoutSystemBase[2]
    {
      (LayoutSystemBase) new DocumentLayoutSystem(775, 209, new DockControl[1]
      {
        this.dockComposition
      }, this.dockComposition),
      (LayoutSystemBase) new DocumentLayoutSystem(775, 233, new DockControl[1]
      {
        this.dockSubstitutes
      }, this.dockSubstitutes)
    });
    this.documentContainer.Manager = (DockManager) null;
    this.documentContainer.Name = "documentContainer";
    this.documentContainer.Renderer = (RendererBase) null;
    this.documentContainer.ShowImageInDocumentTab = true;
    this.dockSubstitutes.AllowedStates = DockLocation.Left | DockLocation.Right | DockLocation.Top | DockLocation.Bottom | DockLocation.Document;
    this.dockSubstitutes.Closable = false;
    this.dockSubstitutes.Collapsible = false;
    this.dockSubstitutes.Controls.Add((Control) this._substitutesTree);
    this.dockSubstitutes.Controls.Add((Control) this.tbSubstitutes);
    this.dockSubstitutes.Controls.Add((Control) this.panelLegend);
    this.dockSubstitutes.Controls.Add((Control) this.menuSubstitutes);
    componentResourceManager.ApplyResources((object) this.dockSubstitutes, "dockSubstitutes");
    this.dockSubstitutes.Floatable = false;
    this.dockSubstitutes.FloatingLocation = new Point(835, 301);
    this.dockSubstitutes.Guid = new Guid("ffebc2c0-0b0a-4e73-a899-921c2656f2fc");
    this.dockSubstitutes.HideOnClose = true;
    this.dockSubstitutes.Name = "dockSubstitutes";
    this.dockSubstitutes.ShowImageInDocumentTab = true;
    this.dockSubstitutes.TabImage = (Image) componentResourceManager.GetObject("dockSubstitutes.TabImage");
    this._substitutesTree.AllowDrop = true;
    this._substitutesTree.BackColor = SystemColors.Control;
    this._substitutesTree.Columns.Add(this._captionSubstitutesTreeColumn);
    this._substitutesTree.Columns.Add(this._noteSubstitutesTreeColumn);
    this._substitutesTree.DisableHeaderContextMenu = true;
    componentResourceManager.ApplyResources((object) this._substitutesTree, "_substitutesTree");
    this._substitutesTree.Editors.Add(this.groupNameEditor);
    this._substitutesTree.ImageList = (ImageList) null;
    this._substitutesTree.LineStyle = LineStyle.Dot;
    this._substitutesTree.MainColumn = this._captionSubstitutesTreeColumn;
    this._substitutesTree.Name = "_substitutesTree";
    this._substitutesTree.RowSelectedUnfocusedStyle.BackColor = SystemColors.Highlight;
    this._substitutesTree.RowSelectedUnfocusedStyle.ForeColor = SystemColors.HighlightText;
    this._substitutesTree.RowStyle.BorderColor = SystemColors.Control;
    this._substitutesTree.RowStyle.BorderStyle = Border3DStyle.Adjust;
    this._substitutesTree.RowStyle.BorderWidth = 1;
    this._substitutesTree.RowStyle.VertAlignment = (StringAlignment) componentResourceManager.GetObject("_substitutesTree.RowStyle.VertAlignment");
    this._substitutesTree.RowStyle.WordWrap = (bool) componentResourceManager.GetObject("_substitutesTree.RowStyle.WordWrap");
    this._substitutesTree.SelectBeforeEdit = true;
    this._substitutesTree.ShowRootRow = false;
    this._substitutesTree.SuppressErrorMessages = true;
    this._substitutesTree.BeforeShowCellEdit += new BeforeShowCellEditHandler(this.SubstitutesTree_BeforeShowCellEdit);
    this._substitutesTree.GetAllowedRowDropLocations += new GetAllowedRowDropLocationsHandler(this.SubstitutesTree_GetAllowedRowDropLocations);
    this._substitutesTree.GetCellData += new GetCellDataHandler(this.SubstitutesTree_GetCellData);
    this._substitutesTree.GetChildren += new GetChildrenHandler(this.SubstitutesTree_GetChildren);
    this._substitutesTree.GetRowData += new GetRowDataHandler(this.SubstitutesTree_GetRowData);
    this._substitutesTree.GetRowDropEffect += new GetRowDropEffectHandler(this.SubstitutesTree_GetRowDropEffect);
    this._substitutesTree.SelectionChanged += new EventHandler(this.SubstitutesTree_SelectionChanged);
    this._substitutesTree.SetCellValue += new SetCellValueHandler(this.SubstitutesTree_SetCellValue);
    this._substitutesTree.DragDrop += new DragEventHandler(this.SubstitutesTree_DragDrop);
    this._substitutesTree.DragEnter += new DragEventHandler(this.SubstitutesTree_DragEnter);
    this._substitutesTree.DragOver += new DragEventHandler(this.SubstitutesTree_DragOver);
    this._substitutesTree.MouseDown += new MouseEventHandler(this.SubstitutesTree_MouseDown);
    this._substitutesTree.MouseMove += new MouseEventHandler(this.SubstitutesTree_MouseMove);
    this._substitutesTree.MouseUp += new MouseEventHandler(this.SubstitutesTree_MouseUp);
    componentResourceManager.ApplyResources((object) this._captionSubstitutesTreeColumn, "_captionSubstitutesTreeColumn");
    this._captionSubstitutesTreeColumn.CellEditor = this.groupNameEditor;
    this._captionSubstitutesTreeColumn.CellStyle.BorderWidth = 0;
    this._captionSubstitutesTreeColumn.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("_captionSubstitutesTreeColumn.HeaderStyle.HorzAlignment");
    this._captionSubstitutesTreeColumn.HeaderStyle.WordWrap = (bool) componentResourceManager.GetObject("_captionSubstitutesTreeColumn.HeaderStyle.WordWrap");
    this._captionSubstitutesTreeColumn.Movable = false;
    this._captionSubstitutesTreeColumn.Name = "_captionSubstitutesTreeColumn";
    this._captionSubstitutesTreeColumn.Sortable = false;
    this.groupNameEditor.Control = (Control) this.textBoxGroupNameEditor;
    this.groupNameEditor.UseCellColors = false;
    componentResourceManager.ApplyResources((object) this.textBoxGroupNameEditor, "textBoxGroupNameEditor");
    this.textBoxGroupNameEditor.Name = "textBoxGroupNameEditor";
    this._noteSubstitutesTreeColumn.AutoSizePolicy = ColumnAutoSizePolicy.AutoSize;
    componentResourceManager.ApplyResources((object) this._noteSubstitutesTreeColumn, "_noteSubstitutesTreeColumn");
    this._noteSubstitutesTreeColumn.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("_noteSubstitutesTreeColumn.HeaderStyle.HorzAlignment");
    this._noteSubstitutesTreeColumn.HeaderStyle.WordWrap = (bool) componentResourceManager.GetObject("_noteSubstitutesTreeColumn.HeaderStyle.WordWrap");
    this._noteSubstitutesTreeColumn.Movable = false;
    this._noteSubstitutesTreeColumn.Name = "_noteSubstitutesTreeColumn";
    this._noteSubstitutesTreeColumn.Sortable = false;
    this.tbSubstitutes.AllowVerticalDock = false;
    this.tbSubstitutes.Closable = false;
    this.tbSubstitutes.DockLine = 3;
    this.tbSubstitutes.FullMenus = true;
    this.tbSubstitutes.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.tbSubstitutes.Hidden = false;
    this.tbSubstitutes.ImageList = this.imageNewDesign;
    this.tbSubstitutes.Items.AddRange(new ToolbarItemBase[11]
    {
      (ToolbarItemBase) this._createGroupButton,
      (ToolbarItemBase) this._createAllowableSubstituteButton,
      (ToolbarItemBase) this._markAuxiliaryPositionButtonItem,
      (ToolbarItemBase) this._markEqualPositionButtonItem,
      (ToolbarItemBase) this._deleteButton,
      (ToolbarItemBase) this._actualizeSubstituteButton,
      (ToolbarItemBase) this._checkSubstitutesButton,
      (ToolbarItemBase) this.btnTracing,
      (ToolbarItemBase) this._markDesignActualVariantButtonItem,
      (ToolbarItemBase) this._moveUpButtonItem,
      (ToolbarItemBase) this._moveDownButtonItem
    });
    componentResourceManager.ApplyResources((object) this.tbSubstitutes, "tbSubstitutes");
    this.tbSubstitutes.MinimumFloatingSize = new Size(250, 30);
    this.tbSubstitutes.Movable = false;
    this.tbSubstitutes.Name = "tbSubstitutes";
    this.tbSubstitutes.Overflow = ToolBarOverflow.Wrap;
    this.tbSubstitutes.Stretch = true;
    componentResourceManager.ApplyResources((object) this._createGroupButton, "_createGroupButton");
    this._createGroupButton.ImageIndex = 5;
    this._createGroupButton.Click += new EventHandler(this.CreateGroupButton_Click);
    this._createAllowableSubstituteButton.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._createAllowableSubstituteButton, "_createAllowableSubstituteButton");
    this._createAllowableSubstituteButton.ImageIndex = 6;
    this._createAllowableSubstituteButton.Click += new EventHandler(this.CreateAllowableSubstituteButton_Click);
    componentResourceManager.ApplyResources((object) this._markAuxiliaryPositionButtonItem, "_markAuxiliaryPositionButtonItem");
    this._markAuxiliaryPositionButtonItem.ImageIndex = 18;
    this._markAuxiliaryPositionButtonItem.Click += new EventHandler(this.MarkAuxiliaryPositionButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._markEqualPositionButtonItem, "_markEqualPositionButtonItem");
    this._markEqualPositionButtonItem.ImageIndex = 19;
    this._markEqualPositionButtonItem.Click += new EventHandler(this.MarkEqualPositionButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._deleteButton, "_deleteButton");
    this._deleteButton.ImageIndex = 7;
    this._deleteButton.Click += new EventHandler(this.DeleteButton_Click);
    this._actualizeSubstituteButton.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._actualizeSubstituteButton, "_actualizeSubstituteButton");
    this._actualizeSubstituteButton.ImageIndex = 8;
    this._actualizeSubstituteButton.Click += new EventHandler(this.ActualizeSubstituteButton_Click);
    this._checkSubstitutesButton.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._checkSubstitutesButton, "_checkSubstitutesButton");
    this._checkSubstitutesButton.ImageIndex = 9;
    this._checkSubstitutesButton.Click += new EventHandler(this.CheckSubstitutesButton_Click);
    this.btnTracing.AutoToggle = AutoToggleType.Single;
    this.btnTracing.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnTracing, "btnTracing");
    this.btnTracing.ImageIndex = 10;
    componentResourceManager.ApplyResources((object) this._markDesignActualVariantButtonItem, "_markDesignActualVariantButtonItem");
    this._markDesignActualVariantButtonItem.ImageIndex = 11;
    this._markDesignActualVariantButtonItem.Click += new EventHandler(this.MarkDesignActualVariantButtonItem_Click);
    this._moveUpButtonItem.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this._moveUpButtonItem, "_moveUpButtonItem");
    this._moveUpButtonItem.ImageIndex = 20;
    this._moveUpButtonItem.Click += new EventHandler(this.MoveUpButtonItem_Click);
    componentResourceManager.ApplyResources((object) this._moveDownButtonItem, "_moveDownButtonItem");
    this._moveDownButtonItem.ImageIndex = 21;
    this._moveDownButtonItem.Click += new EventHandler(this.MoveDownButtonItem_Click);
    this.panelLegend.Controls.Add((Control) this.labelEqualPosition);
    this.panelLegend.Controls.Add((Control) this._equalPositionColorPanel);
    this.panelLegend.Controls.Add((Control) this._auxiliaryPositionColorPanel);
    this.panelLegend.Controls.Add((Control) this._deignActualVariantColorPanel);
    this.panelLegend.Controls.Add((Control) this.label1);
    this.panelLegend.Controls.Add((Control) this.labelDesignerSubstitute);
    componentResourceManager.ApplyResources((object) this.panelLegend, "panelLegend");
    this.panelLegend.Name = "panelLegend";
    componentResourceManager.ApplyResources((object) this.labelEqualPosition, "labelEqualPosition");
    this.labelEqualPosition.Name = "labelEqualPosition";
    this._equalPositionColorPanel.BackColor = Color.FromArgb(196, 224 /*0xE0*/, 224 /*0xE0*/);
    this._equalPositionColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
    componentResourceManager.ApplyResources((object) this._equalPositionColorPanel, "_equalPositionColorPanel");
    this._equalPositionColorPanel.Name = "_equalPositionColorPanel";
    this._auxiliaryPositionColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
    componentResourceManager.ApplyResources((object) this._auxiliaryPositionColorPanel, "_auxiliaryPositionColorPanel");
    this._auxiliaryPositionColorPanel.Name = "_auxiliaryPositionColorPanel";
    this._deignActualVariantColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
    componentResourceManager.ApplyResources((object) this._deignActualVariantColorPanel, "_deignActualVariantColorPanel");
    this._deignActualVariantColorPanel.Name = "_deignActualVariantColorPanel";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.labelDesignerSubstitute, "labelDesignerSubstitute");
    this.labelDesignerSubstitute.Name = "labelDesignerSubstitute";
    componentResourceManager.ApplyResources((object) this.menuSubstitutes, "menuSubstitutes");
    this.menuSubstitutes.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuSubstitutes.Hidden = false;
    this.menuSubstitutes.ImageList = this.imageNewDesign;
    this.menuSubstitutes.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuSubstitutes
    });
    this.menuSubstitutes.Name = "menuSubstitutes";
    this.menuSubstitutes.OwnerForm = (Form) this;
    componentResourceManager.ApplyResources((object) this.contextMenuSubstitutes, "contextMenuSubstitutes");
    this.contextMenuSubstitutes.Items.AddRange(new ToolbarItemBase[6]
    {
      (ToolbarItemBase) this.mnpAddGroup,
      (ToolbarItemBase) this.mnpAddSubstitute,
      (ToolbarItemBase) this.mnpDelete,
      (ToolbarItemBase) this.mnpActualizeSubstitute,
      (ToolbarItemBase) this.mnpCheck,
      (ToolbarItemBase) this._markDesignActualVariantMenuButtonItem
    });
    this.contextMenuSubstitutes.ShowText = true;
    this.contextMenuSubstitutes.BeforePopup += new MenuItemBase.BeforePopupEventHandler(this.contextMenuGroups_BeforePopup);
    componentResourceManager.ApplyResources((object) this.mnpAddGroup, "mnpAddGroup");
    this.mnpAddGroup.ImageIndex = 5;
    this.mnpAddGroup.ShowText = true;
    this.mnpAddGroup.Click += new EventHandler(this.CreateGroupButton_Click);
    componentResourceManager.ApplyResources((object) this.mnpAddSubstitute, "mnpAddSubstitute");
    this.mnpAddSubstitute.ImageIndex = 6;
    this.mnpAddSubstitute.ShowText = true;
    this.mnpAddSubstitute.Click += new EventHandler(this.CreateAllowableSubstituteButton_Click);
    componentResourceManager.ApplyResources((object) this.mnpDelete, "mnpDelete");
    this.mnpDelete.ImageIndex = 7;
    this.mnpDelete.ShowText = true;
    this.mnpDelete.Click += new EventHandler(this.DeleteButton_Click);
    this.mnpActualizeSubstitute.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpActualizeSubstitute, "mnpActualizeSubstitute");
    this.mnpActualizeSubstitute.ImageIndex = 8;
    this.mnpActualizeSubstitute.ShowText = true;
    this.mnpActualizeSubstitute.Click += new EventHandler(this.ActualizeSubstituteButton_Click);
    this.mnpCheck.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpCheck, "mnpCheck");
    this.mnpCheck.ImageIndex = 9;
    this.mnpCheck.ShowText = true;
    this.mnpCheck.Click += new EventHandler(this.CheckSubstitutesButton_Click);
    componentResourceManager.ApplyResources((object) this._markDesignActualVariantMenuButtonItem, "_markDesignActualVariantMenuButtonItem");
    this._markDesignActualVariantMenuButtonItem.ImageIndex = 11;
    this._markDesignActualVariantMenuButtonItem.ShowText = true;
    this._markDesignActualVariantMenuButtonItem.Click += new EventHandler(this.MarkDesignActualVariantButtonItem_Click);
    this.dockComposition.AllowedStates = DockLocation.Left | DockLocation.Right | DockLocation.Top | DockLocation.Bottom | DockLocation.Document;
    this.dockComposition.Closable = false;
    this.dockComposition.Collapsible = false;
    this.dockComposition.Controls.Add((Control) this._positionsGrid);
    this.dockComposition.Controls.Add((Control) this.menuComposition);
    this.dockComposition.Controls.Add((Control) this.tbComposition);
    componentResourceManager.ApplyResources((object) this.dockComposition, "dockComposition");
    this.dockComposition.Floatable = false;
    this.dockComposition.FloatingLocation = new Point(515, 279);
    this.dockComposition.Guid = new Guid("ec4e1b66-a09b-4233-b81e-34125636167c");
    this.dockComposition.HideOnClose = true;
    this.dockComposition.Name = "dockComposition";
    this.dockComposition.ShowImageInDocumentTab = true;
    this.dockComposition.TabImage = (Image) componentResourceManager.GetObject("dockComposition.TabImage");
    componentResourceManager.ApplyResources((object) this.menuComposition, "menuComposition");
    this.menuComposition.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.menuComposition.Hidden = false;
    this.menuComposition.ImageList = this.imageNewDesign;
    this.menuComposition.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuComposition
    });
    this.menuComposition.Name = "menuComposition";
    this.menuComposition.OwnerForm = (Form) this;
    componentResourceManager.ApplyResources((object) this.contextMenuComposition, "contextMenuComposition");
    this.contextMenuComposition.Items.AddRange(new ToolbarItemBase[6]
    {
      (ToolbarItemBase) this.mnpTrack,
      (ToolbarItemBase) this.mnpToActual,
      (ToolbarItemBase) this.mnpToSubstitute,
      (ToolbarItemBase) this.mnpVirtualComposition,
      (ToolbarItemBase) this.mnpCheck2,
      (ToolbarItemBase) this.mnpColumnsSetup
    });
    this.contextMenuComposition.ShowText = true;
    this.contextMenuComposition.BeforePopup += new MenuItemBase.BeforePopupEventHandler(this.contextMenuGroups_BeforePopup);
    this.mnpTrack.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpTrack, "mnpTrack");
    this.mnpTrack.ImageIndex = 10;
    this.mnpTrack.ShowText = true;
    this.mnpTrack.Click += new EventHandler(this.DoTrack);
    this.mnpToActual.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpToActual, "mnpToActual");
    this.mnpToActual.ImageIndex = 0;
    this.mnpToActual.ShowText = true;
    this.mnpToActual.Click += new EventHandler(this.AddToActualSubstituteButton_Click);
    componentResourceManager.ApplyResources((object) this.mnpToSubstitute, "mnpToSubstitute");
    this.mnpToSubstitute.ImageIndex = 1;
    this.mnpToSubstitute.ShowText = true;
    this.mnpToSubstitute.Click += new EventHandler(this.AddToAllowableSubstituteButton_Click);
    this.mnpVirtualComposition.AutoToggle = AutoToggleType.Single;
    this.mnpVirtualComposition.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpVirtualComposition, "mnpVirtualComposition");
    this.mnpVirtualComposition.ImageIndex = 3;
    this.mnpVirtualComposition.ShowText = true;
    componentResourceManager.ApplyResources((object) this.mnpCheck2, "mnpCheck2");
    this.mnpCheck2.ImageIndex = 9;
    this.mnpCheck2.ShowText = true;
    this.mnpCheck2.Click += new EventHandler(this.CheckSubstitutesButton_Click);
    this.mnpColumnsSetup.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.mnpColumnsSetup, "mnpColumnsSetup");
    this.mnpColumnsSetup.ImageIndex = 15;
    this.mnpColumnsSetup.ShowText = true;
    this.mnpColumnsSetup.Click += new EventHandler(this.DoCulumnsSetup);
    this.tbComposition.AllowVerticalDock = false;
    this.tbComposition.Closable = false;
    this.tbComposition.DockLine = 3;
    this.tbComposition.FullMenus = true;
    this.tbComposition.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.tbComposition.Hidden = false;
    this.tbComposition.ImageList = this.imageNewDesign;
    this.tbComposition.Items.AddRange(new ToolbarItemBase[4]
    {
      (ToolbarItemBase) this._addToActualSubstituteButton,
      (ToolbarItemBase) this._addToAllowableSubstituteButton,
      (ToolbarItemBase) this.btVirtualComposition,
      (ToolbarItemBase) this.btnTrack
    });
    componentResourceManager.ApplyResources((object) this.tbComposition, "tbComposition");
    this.tbComposition.MinimumFloatingSize = new Size(250, 30);
    this.tbComposition.Movable = false;
    this.tbComposition.Name = "tbComposition";
    this.tbComposition.Overflow = ToolBarOverflow.Wrap;
    this.tbComposition.Stretch = true;
    componentResourceManager.ApplyResources((object) this._addToActualSubstituteButton, "_addToActualSubstituteButton");
    this._addToActualSubstituteButton.ImageIndex = 0;
    this._addToActualSubstituteButton.Click += new EventHandler(this.AddToActualSubstituteButton_Click);
    componentResourceManager.ApplyResources((object) this._addToAllowableSubstituteButton, "_addToAllowableSubstituteButton");
    this._addToAllowableSubstituteButton.ImageIndex = 1;
    this._addToAllowableSubstituteButton.Click += new EventHandler(this.AddToAllowableSubstituteButton_Click);
    this.btVirtualComposition.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btVirtualComposition, "btVirtualComposition");
    this.btVirtualComposition.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.btDefault,
      (ToolbarItemBase) this.btActual,
      (ToolbarItemBase) this.btWithoutComposition
    });
    this.btVirtualComposition.ShowText = true;
    this.btVirtualComposition.Click += new EventHandler(this.DoNextCompositionStyle);
    this.btDefault.AutoToggle = AutoToggleType.Single;
    this.btDefault.Checked = true;
    componentResourceManager.ApplyResources((object) this.btDefault, "btDefault");
    this.btDefault.ImageIndex = 16 /*0x10*/;
    this.btDefault.ShowText = true;
    this.btDefault.Click += new EventHandler(this.DoDefaultComposition);
    this.btActual.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.btActual, "btActual");
    this.btActual.ImageIndex = 3;
    this.btActual.ShowText = true;
    this.btActual.Click += new EventHandler(this.DoActualComposition);
    this.btWithoutComposition.AutoToggle = AutoToggleType.Single;
    componentResourceManager.ApplyResources((object) this.btWithoutComposition, "btWithoutComposition");
    this.btWithoutComposition.ImageIndex = 17;
    this.btWithoutComposition.ShowText = true;
    this.btWithoutComposition.Click += new EventHandler(this.DoWithoutSubstitutes);
    this.btnTrack.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnTrack, "btnTrack");
    this.btnTrack.Enabled = false;
    this.btnTrack.ImageIndex = 10;
    this.btnTrack.Click += new EventHandler(this.DoTrack);
    componentResourceManager.ApplyResources((object) this.bottomLeft, "bottomLeft");
    this.bottomLeft.DockingManager = DockingManager.Whidbey;
    this.bottomLeft.Guid = new Guid("13d3ed8f-906d-4ae4-8b15-5e6e12838558");
    this.bottomLeft.LayoutSystem = new SplitLayoutSystem(250, 400);
    this.bottomLeft.Manager = (DockManager) null;
    this.bottomLeft.Name = "bottomLeft";
    this.bottomLeft.Renderer = (RendererBase) null;
    componentResourceManager.ApplyResources((object) this.leftDock, "leftDock");
    this.leftDock.Guid = new Guid("1d46a6c2-f12e-41e4-87eb-d37cb80c04e6");
    this.leftDock.LayoutSystem = new SplitLayoutSystem(250, 400);
    this.leftDock.Manager = this.dockManager;
    this.leftDock.Name = "leftDock";
    this.leftDock.Renderer = (RendererBase) null;
    this.dockManager.DockingManager = DockingManager.Whidbey;
    this.dockManager.DocumentContainer = this.documentContainer;
    this.dockManager.ImageList = this.imagesTreeview;
    this.dockManager.OwnerForm = (Form) this;
    componentResourceManager.ApplyResources((object) this.rightDock, "rightDock");
    this.rightDock.Guid = new Guid("bf855203-2e3c-4edc-9128-d2770a12b572");
    this.rightDock.LayoutSystem = new SplitLayoutSystem(250, 400);
    this.rightDock.Manager = this.dockManager;
    this.rightDock.Name = "rightDock";
    this.rightDock.Renderer = (RendererBase) null;
    componentResourceManager.ApplyResources((object) this.bottomDock, "bottomDock");
    this.bottomDock.Guid = new Guid("13d3ed8f-906d-4ae4-8b15-5e6e12838558");
    this.bottomDock.LayoutSystem = new SplitLayoutSystem(250, 400);
    this.bottomDock.Manager = this.dockManager;
    this.bottomDock.Name = "bottomDock";
    this.bottomDock.Renderer = (RendererBase) null;
    componentResourceManager.ApplyResources((object) this.topDock, "topDock");
    this.topDock.Guid = new Guid("b81b004f-4572-4f1a-b1aa-39cf5360ee74");
    this.topDock.LayoutSystem = new SplitLayoutSystem(250, 400);
    this.topDock.Manager = this.dockManager;
    this.topDock.Name = "topDock";
    this.topDock.Renderer = (RendererBase) null;
    this.dockContainer.Controls.Add((Control) this.dockMessages);
    componentResourceManager.ApplyResources((object) this.dockContainer, "dockContainer");
    this.dockContainer.DockingManager = DockingManager.Whidbey;
    this.dockContainer.Guid = new Guid("13d3ed8f-906d-4ae4-8b15-5e6e12838558");
    this.dockContainer.LayoutSystem = new SplitLayoutSystem(250, 400, Orientation.Vertical, new LayoutSystemBase[1]
    {
      (LayoutSystemBase) new ControlLayoutSystem(777, 137, new DockControl[1]
      {
        this.dockMessages
      }, this.dockMessages)
    });
    this.dockContainer.Manager = this.dockManager;
    this.dockContainer.Name = "dockContainer";
    this.dockContainer.Renderer = (RendererBase) null;
    this.dockMessages.AllowedStates = DockLocation.Left | DockLocation.Right | DockLocation.Top | DockLocation.Bottom | DockLocation.Document;
    this.dockMessages.Closable = false;
    this.dockMessages.Collapsible = false;
    this.dockMessages.Controls.Add((Control) this._messageList);
    this.dockMessages.Controls.Add((Control) this.tbMessages);
    componentResourceManager.ApplyResources((object) this.dockMessages, "dockMessages");
    this.dockMessages.Floatable = false;
    this.dockMessages.FloatingLocation = new Point(835, 320);
    this.dockMessages.Guid = new Guid("75217e13-af5c-4a1d-a5f4-a65336b9be4c");
    this.dockMessages.HideOnClose = true;
    this.dockMessages.Name = "dockMessages";
    this.dockMessages.TabImage = (Image) componentResourceManager.GetObject("dockMessages.TabImage");
    componentResourceManager.ApplyResources((object) this._messageList, "_messageList");
    this._messageList.Name = "_messageList";
    this._messageList.SelectedIndexChanged += new EventHandler(this.MessageList_SelectedIndexChanged);
    this.tbMessages.AllowVerticalDock = false;
    this.tbMessages.DockLine = 3;
    this.tbMessages.FullMenus = true;
    this.tbMessages.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.tbMessages.Hidden = true;
    this.tbMessages.ImageList = this.imageNewDesign;
    this.tbMessages.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.btClear
    });
    componentResourceManager.ApplyResources((object) this.tbMessages, "tbMessages");
    this.tbMessages.MinimumFloatingSize = new Size(250, 30);
    this.tbMessages.Name = "tbMessages";
    this.tbMessages.Overflow = ToolBarOverflow.Wrap;
    this.tbMessages.Stretch = true;
    componentResourceManager.ApplyResources((object) this.btClear, "btClear");
    this.btClear.ImageIndex = 12;
    this.AcceptButton = (IButtonControl) this._okButton;
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.CancelButton = (IButtonControl) this._cancelButton;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this.documentContainer);
    this.Controls.Add((Control) this.dockContainer);
    this.Controls.Add((Control) this.panelBottom);
    this.Controls.Add((Control) this.leftDock);
    this.Controls.Add((Control) this.rightDock);
    this.Controls.Add((Control) this.bottomDock);
    this.Controls.Add((Control) this.topDock);
    this.Controls.Add((Control) this.bottomLeft);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ArtSubstitutionsEditor);
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.Tag = (object) " ";
    this.FormClosing += new FormClosingEventHandler(this.SubstitutesEditor_FormClosing);
    this.FormClosed += new FormClosedEventHandler(this.SubstitutesEditor_FormClosed);
    this.Load += new EventHandler(this.SubstitutesEditor_Load);
    this.panelBottom.ResumeLayout(false);
    this.panelBottom.PerformLayout();
    this.documentContainer.ResumeLayout(false);
    this.dockSubstitutes.ResumeLayout(false);
    this._substitutesTree.EndInit();
    this.panelLegend.ResumeLayout(false);
    this.panelLegend.PerformLayout();
    this.dockComposition.ResumeLayout(false);
    this.dockContainer.ResumeLayout(false);
    this.dockMessages.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private class Relation
  {
    public const long UnknownRelationID = 0;

    public Relation()
    {
      this.ID = 0L;
      this.ProjectVersionID = 0L;
      this.PartID = 0L;
      this.PrototypeID = 0L;
      this.TypeID = -1;
    }

    public long ID { get; set; }

    public Guid Guid { get; set; }

    public long ProjectVersionID { get; set; }

    public long PartID { get; set; }

    public long PrototypeID { get; set; }

    public int TypeID { get; set; }

    public long PartVersionID { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == this)
        return true;
      return obj is ArtSubstitutionsEditor.Relation relation && relation.ID == this.ID;
    }

    public override int GetHashCode() => (int) this.ID;
  }

  private class SubstitutesEditorMessage : _Message
  {
    public SubstitutesEditorMessage(_MessageType type, string text, object data)
      : base(type, text)
    {
      this.Data = data;
    }

    public object Data { get; private set; }
  }
}
