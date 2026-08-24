// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.TechRouteFilter
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Bars;
using Intermech.Client.Core.Navigator.Classes.Providers;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Parts;
using Intermech.Techcard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.MRP2;

/// <summary>
/// Класс для фильтрации маршрутов обработки в окне производственной ведомости по входимости
/// </summary>
public class TechRouteFilter : INavigatorVirtualColumnProvider, ISpecialFieldsSupported
{
  private static TechRouteFilterState _globalFilterState = TechRouteFilterState.trfDisabled;
  public TechRouteFilterState FilterState = TechRouteFilter._globalFilterState;
  private static DBRecordSetParams recordSetParams = new DBRecordSetParams(new ConditionStructure[0], new ColumnDescriptor[4]
  {
    new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID),
    new ColumnDescriptor((object) TechCardConsts.AttributeTypes.MemberOfProductionReportObjectAttrID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
    new ColumnDescriptor((object) TechCardConsts.AttributeTypes.MemberOfProductionReportVersionAttrID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
    new ColumnDescriptor((object) TechCardConsts.AttributeTypes.MemberOfExitAssemblyAttrID, AttributeSourceTypes.Object, ColumnContents.String, ColumnNameMapping.ID, SortOrders.NONE, 0)
  });
  private readonly NavigatorTreeView _navigatorTreeView;

  public List<object> GetSpecialFields() => (List<object>) null;

  public object MapColumnToField(INodeItems nodeItems, NodeColumn column) => (object) null;

  public DataTable GetDataTable(INodeQuery nodeQuery, NavigatorVirtualColumnProviderArgs args)
  {
    if (args.SourceTable != null && this.FilterState != TechRouteFilterState.trfDisabled && args.TypeInfo.Kind == AttributableElements.Relation && (args.TypeInfo.TypeID == -1 || args.TypeInfo.TypeID == TechConsts.RelType_TechComposition_ID))
    {
      CompositeNode oNode;
      if (nodeQuery is INodePartContextAware partContextAware && partContextAware.NodePart is INodePart nodePart && (oNode = nodePart.Owner as CompositeNode) != null)
      {
        NavigatorTreeView service = partContextAware.Services.GetService<NavigatorTreeView>(false);
        if (service == null)
          return args.SourceTable;
        NavigatorTreeNode navigatorTreeNode1 = service.NodesEnumeration((System.Func<NavigatorTreeNode, bool>) (item => item.Handler == oNode)).FirstOrDefault<NavigatorTreeNode>();
        if (navigatorTreeNode1 == null)
          return args.SourceTable;
        long num1;
        long aExitAsmObjectId = num1 = 0L;
        long aProdListObjectId = num1;
        long aProdListId = num1;
        long aAsmObjectId = num1;
        long objectId = (navigatorTreeNode1.NodeID as NodeID).ObjectID;
        NavigatorTreeNode parent = navigatorTreeNode1.Parent;
        if (parent != null)
          aAsmObjectId = (parent.NodeID as NodeID).ObjectID;
        for (NavigatorTreeNode navigatorTreeNode2 = navigatorTreeNode1; navigatorTreeNode2.Parent != null; navigatorTreeNode2 = navigatorTreeNode2.Parent)
        {
          if (MetaDataHelper.IsObjectTypeChildOf((navigatorTreeNode2.Parent.NodeID as NodeID).ObjectTypeID, MRP2Consts.objtypeIdProductionLists))
          {
            aProdListId = (navigatorTreeNode2.Parent.NodeID as NodeID).ID;
            aProdListObjectId = (navigatorTreeNode2.Parent.NodeID as NodeID).ObjectID;
            if (MetaDataHelper.IsObjectTypeChildOf((navigatorTreeNode2.NodeID as NodeID).ObjectTypeID, MRP2Consts.objtypeIdExitAssembly))
            {
              aExitAsmObjectId = (navigatorTreeNode2.NodeID as NodeID).ObjectID;
              break;
            }
            break;
          }
        }
        if (aAsmObjectId == 0L || aProdListObjectId == 0L)
          return args.SourceTable;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          TechRouteFilterInfo fltInfo = new TechRouteFilterInfo(sessionKeeper.Session, aProdListObjectId, aProdListId, aExitAsmObjectId, aAsmObjectId);
          int columnIndex1 = ((IEnumerable<object>) args.Mapping.Fields).IndexOfFirst<object>((Predicate<object>) (x => x is NodeColumnID nodeColumnId1 && nodeColumnId1.AttributeID == -7));
          int columnIndex2 = ((IEnumerable<object>) args.Mapping.Fields).IndexOfFirst<object>((Predicate<object>) (x => x is NodeColumnID nodeColumnId2 && nodeColumnId2.AttributeID == -2));
          if (columnIndex1 != -1)
          {
            if (columnIndex2 != -1)
            {
              List<DataRow> dataRowList = new List<DataRow>();
              DataRow dataRow = (DataRow) null;
              int num2 = 0;
              IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(TechConsts.RelType_TechComposition_ID);
              relationCollection.ObjectTypeID = TechCardConsts.ObjectTypes.ProcRoutingEntryID;
              foreach (DataRow row in (InternalDataCollectionBase) args.SourceTable.Rows)
              {
                if (DataSetProcessor.GetInt32Value(row, columnIndex1, -1) == TechCardConsts.ObjectTypes.ProcRoutingID)
                {
                  long int64Value = DataSetProcessor.GetInt64Value(row, columnIndex2, 0L);
                  bool defRoute;
                  if (this.IsRowFiltered(sessionKeeper.Session, relationCollection, row, fltInfo, int64Value, out defRoute))
                    dataRowList.Add(row);
                  else
                    ++num2;
                  if (defRoute && dataRow == null)
                    dataRow = row;
                }
              }
              if (dataRowList.Count > 0)
              {
                if (num2 == 0 && dataRow != null)
                  dataRowList.Remove(dataRow);
                foreach (DataRow row in dataRowList)
                  args.SourceTable.Rows.Remove(row);
                args.SourceTable.AcceptChanges();
              }
            }
          }
        }
      }
    }
    return args.SourceTable;
  }

  public TechRouteFilter(NavigatorTreeView navigatorTreeView)
  {
    this._navigatorTreeView = navigatorTreeView;
  }

  public static void InitFilterState(IUserSession session)
  {
    TechRouteFilter._globalFilterState = (TechRouteFilterState) session.Configurations.ReadInteger("MRP2", "MRP2", "TechFilterState", (long) TechRouteFilter._globalFilterState, DBConfigMode.UserAndGlobal);
  }

  private bool IsRowFiltered(
    IUserSession session,
    IDBRelationCollection dbRelCol,
    DataRow row,
    TechRouteFilterInfo fltInfo,
    long routeObjId,
    out bool defRoute)
  {
    defRoute = false;
    if (this.FilterState == TechRouteFilterState.trfWithDefault)
    {
      IDBAttribute objectAttribute = session.GetObjectAttribute(routeObjId, (object) TechCardConsts.AttributeTypes.ProcRouteDefaultAttrID, false, false);
      defRoute = objectAttribute != null && objectAttribute.AsString != "";
    }
    DataTable dataTable = dbRelCol.ConsistFrom(TechRouteFilter.recordSetParams, routeObjId);
    if (dataTable == null || dataTable.Rows.Count == 0)
      return true;
    foreach (DataRow row1 in (InternalDataCollectionBase) dataTable.Rows)
    {
      long int64Value1 = DataSetProcessor.GetInt64Value(row1, 0, 0L);
      long int64Value2 = DataSetProcessor.GetInt64Value(row1, 1, 0L);
      long int64Value3 = DataSetProcessor.GetInt64Value(row1, 2, 0L);
      string stringValue = DataSetProcessor.GetStringValue(row1, 3, "");
      if ((int64Value3 == fltInfo.ProdListObjectId || int64Value2 == fltInfo.ProdListId) && stringValue == fltInfo.ExitAsmPKDSE)
      {
        IDBAttribute objectAttribute = session.GetObjectAttribute(int64Value1, (object) TechCardConsts.AttributeTypes.MemberOfAssemblyCopyAttrID, false, false);
        if (objectAttribute == null || objectAttribute.IsNull)
        {
          if (fltInfo.AsmPKDSE == string.Empty)
            return false;
        }
        else if (((IEnumerable<object>) objectAttribute.Values).IndexOf<object>((Predicate<object>) (x => (string) x == fltInfo.AsmPKDSE)) != -1)
          return false;
      }
    }
    return true;
  }

  internal void ClickHandler(object sender, EventArgs e)
  {
    switch (sender)
    {
      case DropDownMenuItem dropDownMenuItem:
        switch (this.FilterState)
        {
          case TechRouteFilterState.trfDisabled:
            this.FilterState = TechRouteFilterState.trfEnabled;
            dropDownMenuItem.Items[1].Checked = true;
            dropDownMenuItem.ImageIndex = dropDownMenuItem.Items[1].ImageIndex;
            break;
          case TechRouteFilterState.trfEnabled:
            this.FilterState = TechRouteFilterState.trfWithDefault;
            dropDownMenuItem.Items[2].Checked = true;
            dropDownMenuItem.ImageIndex = dropDownMenuItem.Items[2].ImageIndex;
            break;
          default:
            this.FilterState = TechRouteFilterState.trfDisabled;
            dropDownMenuItem.Items[0].Checked = true;
            dropDownMenuItem.ImageIndex = dropDownMenuItem.Items[0].ImageIndex;
            break;
        }
        dropDownMenuItem.Checked = this.FilterState != 0;
        break;
      case MenuButtonItem menuButtonItem:
        this.FilterState = (TechRouteFilterState) menuButtonItem.Tag;
        menuButtonItem.Parent.Checked = this.FilterState != 0;
        menuButtonItem.Parent.ImageIndex = menuButtonItem.ImageIndex;
        break;
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.Configurations.WriteInteger("MRP2", "MRP2", "TechFilterState", (long) this.FilterState, sessionKeeper.Session.UserID);
      TechRouteFilter._globalFilterState = this.FilterState;
    }
    this._navigatorTreeView.RefreshNode(this._navigatorTreeView.RootNode);
    this._navigatorTreeView.FocusedNode?.Expand();
  }
}
