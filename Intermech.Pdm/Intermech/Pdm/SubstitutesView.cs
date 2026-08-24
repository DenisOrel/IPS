// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SubstitutesView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Pdm;

public sealed class SubstitutesView : ChildrenView
{
  private Dictionary<long, SubstitutesNodeID> _items = new Dictionary<long, SubstitutesNodeID>();
  private SubstituteObjects _substitutes;
  private SubstituteObjects _substitutesVirtual;
  private RelationAttributesPackage _remarks;
  private SubstitutesVirtualMode _virtualMode = SubstitutesVirtualMode.States;
  private static int _substitutesNoteID = -1;
  private long _parentObjectID;
  private IContainer components;

  public SubstitutesView() => this.AllowEditing = false;

  public SubstituteObjects Substitutes
  {
    get
    {
      if (this._substitutes == null)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
          this._substitutes = new SubstituteObjects(sessionKeeper.Session);
      }
      return this._substitutes;
    }
  }

  public SubstituteObjects SubstitutesVirtual
  {
    get => this._substitutesVirtual;
    set => this._substitutesVirtual = value;
  }

  public RelationAttributesPackage Remarks
  {
    get => this._remarks;
    set => this._remarks = value;
  }

  public SubstitutesVirtualMode GetVirtualMode() => this._virtualMode;

  public void SetVirtualMode(SubstitutesVirtualMode value, ArticlesPartsPackage package)
  {
    this._virtualMode = value;
    this.RebuildVirtualGrid(package);
  }

  public SubstitutesNodeID this[long PrjLinkID]
  {
    get => !this._items.ContainsKey(PrjLinkID) ? (SubstitutesNodeID) null : this._items[PrjLinkID];
  }

  public int ItemsCount => this._items == null ? 0 : this._items.Count;

  public List<long> SelectedRelationsFromComposition
  {
    get
    {
      List<long> relationsFromComposition = new List<long>();
      if (this.SelectedItems == null || this.SelectedItems.Count == 0)
        return relationsFromComposition;
      for (int index = 0; index < this.SelectedItems.Count; ++index)
      {
        if (this.SelectedItems.GetItemData(index, typeof (IDBRelationID)) is IDBRelationID itemData)
          relationsFromComposition.Add(itemData.Value);
      }
      return relationsFromComposition;
    }
  }

  public RelationAttributesPackage RelationsAttributes
  {
    get
    {
      if (this._path == null)
        return (RelationAttributesPackage) null;
      if (!(this._path.RootDescriptor is SubstitutesDescriptor rootDescriptor))
        return (RelationAttributesPackage) null;
      List<int> attributes = new List<int>(rootDescriptor.Attributes != null ? rootDescriptor.Attributes.Count : 0);
      if (rootDescriptor.Attributes != null)
      {
        for (int index = 0; index < rootDescriptor.Attributes.Count; ++index)
          attributes.Add((int) rootDescriptor.Attributes[index].ID);
      }
      RelationAttributesPackage relationsAttributes = new RelationAttributesPackage(attributes);
      foreach (KeyValuePair<long, SubstitutesNodeID> keyValuePair in this._items)
        relationsAttributes.Values.Add(keyValuePair.Key, keyValuePair.Value.Values);
      return relationsAttributes;
    }
  }

  public void SelectItems(ISelectedItems items)
  {
    if (items == null || items.Count == 0 || this._grid.Rows.Count == 0)
      return;
    this._grid.PerformAction(iGActions.DeselectAll);
    for (int index = 0; index < items.Count; ++index)
    {
      SubstitutesNodeID nodeID = items.GetItemData(index, typeof (IDBRelationID)) is IDBRelationID itemData ? this[itemData.Value] : (SubstitutesNodeID) null;
      if (nodeID != null)
      {
        iGRow rowWithNodeId = this.GetRowWithNodeID((INodeID) nodeID);
        if (rowWithNodeId != null)
          this.SetSelectedForRow(rowWithNodeId, true);
      }
    }
    if (this._grid.SelectedCells.Count == 0)
      return;
    this._grid.SelectedCells[0].Row.EnsureVisible();
    this._grid.SetCurRow(this._grid.SelectedCells[0].Row.Index);
  }

  public void RebuildVirtualGrid(ArticlesPartsPackage articlePackage)
  {
    try
    {
      this._grid.Redraw = false;
      this._grid.BeginUpdate();
      IElementStatusesClientService service = ServicesManager.GetService(typeof (IElementStatusesClientService)) as IElementStatusesClientService;
      iGCol statesCol = (iGCol) null;
      iGCol groupNameCol = (iGCol) null;
      iGCol nameCol = (iGCol) null;
      for (int index = 0; index < this._grid.Cols.Count; ++index)
      {
        iGCol col = this._grid.Cols[index];
        if (col.Tag is NodeColumn tag)
        {
          if (tag.ID.Equals((object) "F_STATUSES"))
            statesCol = col;
          tag.ID.Equals((object) SubstitutesView._substitutesNoteID);
          if (tag.ID.Equals((object) SubstituteObjects.attrSubstituteGroupName))
            groupNameCol = col;
          if (tag.ID.Equals((object) SubstituteObjects.attrSubstituteName))
            nameCol = col;
        }
      }
      for (int index = 0; index < this._grid.Rows.Count; ++index)
      {
        iGRow row = this._grid.Rows[index];
        INodeID nodeIdForRow = this.GetNodeIDForRow(row);
        if (nodeIdForRow != null && nodeIdForRow is SubstitutesNodeID snode)
        {
          if (this._virtualMode == SubstitutesVirtualMode.None || this._substitutesVirtual == null)
          {
            row.Visible = true;
            this.SetStatesValue(row, statesCol, snode, this.Substitutes, service, articlePackage);
            this.SetGroupNameValue(row, groupNameCol, snode, this.Substitutes, this.Substitutes.RelationAttributes);
            this.SetNameValue(row, nameCol, snode, this.Substitutes, this.Substitutes.RelationAttributes);
          }
          else
          {
            long Group;
            long SubstInGroup;
            this._substitutesVirtual.IndexOf(snode.PrjLinkID, out Group, out SubstInGroup);
            row.Visible = this._virtualMode != SubstitutesVirtualMode.WithoutSubstitutes ? this._virtualMode != SubstitutesVirtualMode.ActualComposition || SubstInGroup == 0L || this._substitutesVirtual.IndexOf(snode.PrjLinkID) < 0L : Group == 0L || this._substitutesVirtual.IndexOf(snode.PrjLinkID) < 0L;
            this.SetGroupNameValue(row, groupNameCol, snode, this._substitutesVirtual, this._remarks);
            this.SetNameValue(row, nameCol, snode, this._substitutesVirtual, this._remarks);
            this.SetStatesValue(row, statesCol, snode, this._substitutesVirtual, service, articlePackage);
          }
        }
      }
    }
    finally
    {
      this._grid.EndUpdate();
      this._grid.Redraw = true;
    }
  }

  public object GetCellValue(SubstitutesNodeID snode, string key)
  {
    if (this.GetNodeColumns().Find(key) == null)
      return (object) null;
    iGRow rowWithNodeId = this.GetRowWithNodeID((INodeID) snode);
    if (rowWithNodeId == null)
      return (object) null;
    object cellValue = (object) null;
    foreach (iGCell cell in (IEnumerable) rowWithNodeId.Cells)
    {
      if (cell.ColKey == key || cell.Col.Tag is NodeColumn tag && tag.Key == key)
      {
        cellValue = cell.Value;
        break;
      }
    }
    return cellValue;
  }

  internal IEnumerable<SubstitutesNodeID> GetSubstitutesNodeIds()
  {
    List<SubstitutesNodeID> substitutesNodeIds = new List<SubstitutesNodeID>();
    foreach (iGRow row in (IEnumerable) this._grid.Rows)
    {
      if (row.Type == iGRowType.Normal && this.GetNodeIDForRow(row) is SubstitutesNodeID nodeIdForRow)
        substitutesNodeIds.Add(nodeIdForRow);
    }
    return (IEnumerable<SubstitutesNodeID>) substitutesNodeIds;
  }

  [CustomDescription("Attribute.Pdm_24")]
  public event CustomCellBackgroundEventHandler ShowCellCustomBackground;

  public override void Initialize(ISelectedItems items, IServiceProvider services)
  {
    base.Initialize(items, services);
    if (!(this._services.GetService(typeof (IViewState)) is IViewState))
      this._services.AddService(typeof (IViewState), (object) new ViewStateService());
    if (SubstitutesView._substitutesNoteID == -1)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        SubstitutesView._substitutesNoteID = sessionKeeper.Session.IdentHelper.GetAttributeID("cad00274-306c-11d8-b4e9-00304f19f545");
    }
    this._parentObjectID = (items.GetParentData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
  }

  public override void Initialize(
    NodeIDPath parentPath,
    INode parentNode,
    INodeID nodeId,
    IServiceProvider services)
  {
    base.Initialize(parentPath, parentNode, nodeId, services);
    if (!(this._services.GetService(typeof (IViewState)) is IViewState))
      this._services.AddService(typeof (IViewState), (object) new ViewStateService());
    if (SubstitutesView._substitutesNoteID == -1)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        SubstitutesView._substitutesNoteID = sessionKeeper.Session.IdentHelper.GetAttributeID("cad00274-306c-11d8-b4e9-00304f19f545");
    }
    this._parentObjectID = (nodeId as SubstitutesNodeID).ObjectID;
  }

  public override void Initialize(IDescriptor rootDescriptor, IServiceProvider services)
  {
    base.Initialize(rootDescriptor, services);
    if (!(this._services.GetService(typeof (IViewState)) is IViewState))
      this._services.AddService(typeof (IViewState), (object) new ViewStateService());
    if (SubstitutesView._substitutesNoteID == -1)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
        SubstitutesView._substitutesNoteID = sessionKeeper.Session.IdentHelper.GetAttributeID("cad00274-306c-11d8-b4e9-00304f19f545");
    }
    this._parentObjectID = (rootDescriptor as SubstitutesDescriptor).ObjID;
  }

  protected override int FetchCount => -1;

  public override string StateStreamPrefix => "PDM_" + base.StateStreamPrefix;

  protected override void UpdateControls()
  {
    base.UpdateControls();
    this._readNextToolStripDropDownButton.Visible = false;
    this._readAllToolStripDropDownButton.Visible = false;
    this._embeddedViewsDropDownMenuItem.Visible = false;
    this.UpdateRowsHeight();
  }

  protected override void GridReloadIfNeed()
  {
    base.GridReloadIfNeed();
    this.GridSaveState((Stream) null);
  }

  protected override void RaiseDataTableChanged()
  {
    base.RaiseDataTableChanged();
    this.BuildSubstitutes();
  }

  protected override void CustomDrawCellBackground(object sender, iGCustomDrawCellEventArgs e)
  {
    if (this.ShowCellCustomBackground != null)
    {
      iGCell cell = this._grid.Rows[e.RowIndex].Cells[e.ColIndex];
      INodeID nodeIdForRow = this.GetNodeIDForRow(e.RowIndex);
      this.ShowCellCustomBackground((object) this, new CustomCellBackgroundEventArgs(e, this._grid, cell, nodeIdForRow));
    }
    else
      base.CustomDrawCellBackground(sender, e);
  }

  protected override void GridColWidthChanged(object sender, iGColWidthEventArgs e)
  {
    base.GridColWidthChanged(sender, e);
    this.UpdateRowsHeight();
  }

  public override void SetColumns(NodeColumnCollection columns, bool reloadGrid)
  {
    base.SetColumns(columns, reloadGrid);
    this.UpdateRowsHeight();
  }

  protected override void AfterContentsSorted()
  {
    base.AfterContentsSorted();
    this.SetVirtualMode(this.GetVirtualMode(), (ArticlesPartsPackage) null);
  }

  protected override void AfterContentsGrouped()
  {
    base.AfterContentsGrouped();
    this.SetVirtualMode(this.GetVirtualMode(), (ArticlesPartsPackage) null);
  }

  private void BuildSubstitutes()
  {
    this.Substitutes.Clear();
    this._items.Clear();
    int count = this._grid.Cols.Count;
    if (this._grid.Rows.Count == 0 || count < 2 || !(this.Node is SubstitutesNode))
      return;
    foreach (iGRow row in (IEnumerable) this._grid.Rows)
    {
      if (row.Type == iGRowType.Normal && this.GetNodeIDForRow(row) is SubstitutesNodeID nodeIdForRow)
      {
        this.Substitutes.AddRelation(nodeIdForRow.SubstitutesGroupNoID, nodeIdForRow.SubstituteInGroup, nodeIdForRow.PrjLinkID, nodeIdForRow.ObjectID);
        this.Substitutes.SetObjectID(nodeIdForRow.PrjLinkID, nodeIdForRow.ID);
        this.Substitutes.SetRelationAttributes(nodeIdForRow.PrjLinkID, nodeIdForRow.Attributes, nodeIdForRow.Values);
        string relationPositionNumber = this.Substitutes.GetRelationPositionNumber(nodeIdForRow.PrjLinkID);
        this.Substitutes.SetAuxiliaryFlagIfNeed(nodeIdForRow.PrjLinkID, nodeIdForRow.ID, relationPositionNumber);
        if (nodeIdForRow.SubstitutesGroupNoID > 0L)
        {
          object obj = nodeIdForRow[SubstituteObjects.attrSubstituteGroupName];
          if (obj != null && obj != DBNull.Value)
            this.Substitutes.SetSubstGroupName(nodeIdForRow.SubstitutesGroupNoID, obj.ToString());
        }
        this._items.Add(nodeIdForRow.PrjLinkID, nodeIdForRow);
      }
    }
  }

  private void SetGroupNameValue(
    iGRow row,
    iGCol groupNameCol,
    SubstitutesNodeID snode,
    SubstituteObjects substs,
    RelationAttributesPackage attrs)
  {
    if (row == null || groupNameCol == null || snode == null || substs == null || attrs == null)
      return;
    object relationAttribute = substs.RelationAttributes[snode.PrjLinkID, SubstituteObjects.attrSubstituteGroupName];
    row.Cells[groupNameCol.Index].Value = relationAttribute;
  }

  private void SetNameValue(
    iGRow row,
    iGCol nameCol,
    SubstitutesNodeID snode,
    SubstituteObjects substs,
    RelationAttributesPackage attrs)
  {
    if (row == null || nameCol == null || snode == null || substs == null || attrs == null)
      return;
    object relationAttribute = substs.RelationAttributes[snode.PrjLinkID, SubstituteObjects.attrSubstituteName];
    row.Cells[nameCol.Index].Value = relationAttribute;
  }

  private void SetStatesValue(
    iGRow row,
    iGCol statesCol,
    SubstitutesNodeID snode,
    SubstituteObjects substs,
    IElementStatusesClientService svc,
    ArticlesPartsPackage articlePackage)
  {
    if (row == null || statesCol == null || snode == null || substs == null || svc == null)
      return;
    long Group;
    long SubstInGroup;
    substs.IndexOf(snode.PrjLinkID, out Group, out SubstInGroup);
    ArticleRelationState articleRelationState = ArticleRelationState.Unknown;
    if (articlePackage != null)
      articleRelationState = articlePackage.GetRelationState(this._parentObjectID, snode.PrjLinkID);
    if (statesCol == null)
      return;
    RelationAsSubstitutes relationAsSubstitutes = RelationAsSubstitutes.rsNoSubstitutes;
    if (Group != 0L)
    {
      relationAsSubstitutes = RelationAsSubstitutes.rsSubstitute;
      if (SubstInGroup == 0L)
        relationAsSubstitutes = RelationAsSubstitutes.rsActualSubstitute;
    }
    short int16_1 = Convert.ToInt16((object) relationAsSubstitutes);
    svc.SetElementStatuses16("cad005f4-306c-11d8-b4e9-00304f19f545", row.Cells[statesCol.Index].Value as byte[], int16_1);
    short int16_2 = Convert.ToInt16((object) articleRelationState);
    svc.SetElementStatuses16("{793BEF65-E7BC-40B5-A0FA-003472E7F548}", row.Cells[statesCol.Index].Value as byte[], int16_2);
  }

  private void UpdateRowsHeight()
  {
    for (int index = 0; index < this._grid.Cols.Count; ++index)
    {
      this._grid.Cols[index].CellStyle.TextFormatFlags |= iGStringFormatFlags.WordWrap;
      this._grid.Cols[index].CellStyle.TextTrimming = iGStringTrimming.Word;
    }
    this._grid.Rows.AutoHeight();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ((ISupportInitialize) this._grid).BeginInit();
    this.SuspendLayout();
    this._grid.DefaultAutoGroupRow.Height = 21;
    this._grid.FrozenArea.ColCount = 1;
    this._grid.FrozenArea.SortFrozenRows = true;
    this._grid.GroupBox.BackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintForeColor = SystemColors.ControlText;
    this._grid.GroupBox.Text = "Перетащите заголовок колонки в эту область для группировки по значениям этой колонки";
    this._grid.GroupBox.Visible = true;
    this._grid.Header.AutoHeightFlags = iGHdrAutoHeightFlags.OnAddCol | iGHdrAutoHeightFlags.OnRemoveCol | iGHdrAutoHeightFlags.OnShowCol | iGHdrAutoHeightFlags.OnContentsChange | iGHdrAutoHeightFlags.OnThemeChange | iGHdrAutoHeightFlags.OnResizeCol;
    this._grid.Header.Height = 19;
    this._grid.LayoutObject.Flags = iGLayoutFlags.Grouping | iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this._grid.Size = new Size(626, 267);
    this.buttonHeightSet.Padding.Bottom = 0;
    this.buttonHeightSet.Padding.Left = 0;
    this.buttonHeightSet.Padding.Right = 0;
    this.buttonHeightSet.Padding.Top = 0;
    this.Name = nameof (SubstitutesView);
    ((ISupportInitialize) this._grid).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
