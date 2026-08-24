// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SubstitutesPart
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm;

public class SubstitutesPart : RelatedObjectsPart
{
  private NodeColumnID ncSubstitutesGroupNoID;
  private NodeColumnID ncSubstituteInGroup;
  private List<NodeColumnID> ncAdvAttributes;
  private IServiceProvider _services;
  private string _filtrationOwnerID;
  private List<long> _contexts;
  private List<NodeColumnID> _attributes = new List<NodeColumnID>();

  public SubstitutesPart(
    IServiceProvider services,
    int projObjTypeID,
    long projID,
    int relationTypeID,
    string filtrationOwnerID,
    List<long> contexts,
    List<NodeColumnID> attributes)
    : base(projObjTypeID, projID, RelatedObjectsRole.Composition, relationTypeID, services)
  {
    this._services = services;
    this._filtrationOwnerID = filtrationOwnerID;
    this._contexts = contexts;
    this.ncAdvAttributes = attributes == null || attributes.Count <= 0 ? (List<NodeColumnID>) null : new List<NodeColumnID>(attributes.Count);
    this._attributes = attributes;
    if (attributes == null)
      return;
    for (int index = 0; index < attributes.Count; ++index)
      this.ncAdvAttributes.Add(attributes[index].Clone() as NodeColumnID);
  }

  public override INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    int int32_1 = Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_OBJECT_TYPE)]);
    long int64_1 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_OBJECT_ID)]);
    long int64_2 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_ID)]);
    long int64_3 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) RelatedPartBase.ncF_PRJLINK_ID)]);
    long int64_4 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncMODIFICATION_ID)]);
    int int32_2 = Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_LC_STEP)]);
    string caption = Convert.ToString(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncCAPTION)]);
    long int64_5 = fieldValues[adapter.GetFieldIndex((object) this.ncSubstitutesGroupNoID)] != DBNull.Value ? Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) this.ncSubstitutesGroupNoID)]) : 0L;
    long int64_6 = fieldValues[adapter.GetFieldIndex((object) this.ncSubstituteInGroup)] != DBNull.Value ? Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) this.ncSubstituteInGroup)]) : 0L;
    long int64_7 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_CHKOUT_BY)]);
    long int64_8 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncOWNER)]);
    long int64_9 = adapter.GetFieldIndex((object) ObjectsPartBase.ncSORTING) < 0 || fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncSORTING)] == DBNull.Value ? 0L : Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncSORTING)]);
    long int64_10 = adapter.GetFieldIndex((object) ObjectsPartBase.ncVERSION) < 0 || fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncVERSION)] == DBNull.Value ? 0L : Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncVERSION)]);
    long int64_11 = adapter.GetFieldIndex((object) ObjectsPartBase.ncBASE_VERSION) < 0 || fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncBASE_VERSION)] == DBNull.Value ? 0L : Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncBASE_VERSION)]);
    string siteID = adapter.GetFieldIndex((object) ObjectsPartBase.ncSITE_ID) < 0 || fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncSITE_ID)] == DBNull.Value ? string.Empty : Convert.ToString(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncSITE_ID)]);
    object[] values = this.ncAdvAttributes != null ? new object[this.ncAdvAttributes.Count] : (object[]) null;
    if (this.ncAdvAttributes != null)
    {
      for (int index = 0; index < this.ncAdvAttributes.Count; ++index)
      {
        int fieldIndex = adapter.GetFieldIndex((object) this.ncAdvAttributes[index]);
        values[index] = fieldIndex >= 0 ? fieldValues[fieldIndex] : (object) null;
        values[index] = values[index] != DBNull.Value ? values[index] : (object) null;
      }
    }
    return (INodeID) new SubstitutesNodeID((CreateObjectNodeParams) new CreateSubstituteNodeParams(int32_1, int64_1, int64_2, int64_7, int64_3, int32_2, caption, this._relTypeID, int64_8, int64_9, ObjectFiltrationState.fsNotRequired, int64_10, int64_11, siteID, int64_4, this._filtrationOwnerID, this._contexts, this._objTypeID, this._objID, this._attributes, values, int64_5, int64_6), this._services);
  }

  public override NodeColumnCollection GetDefaultColumns()
  {
    NodeColumnCollection defaultColumns = new NodeColumnCollection();
    Guid columnSchemeGuid = Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid;
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.RelationColumnSchemeGuid, (object) sessionKeeper.Session.IdentHelper.GetAttributeID("cad00270-306c-11d8-b4e9-00304f19f545"), NodeColumnSortOrder.Ascending, 0), 100);
      int attributeId1 = sessionKeeper.Session.IdentHelper.GetAttributeID("cad0001f-306c-11d8-b4e9-00304f19f545");
      defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectColumnSchemeGuid, (object) attributeId1, NodeColumnSortOrder.None, -1), 250);
      int attributeId2 = sessionKeeper.Session.IdentHelper.GetAttributeID("cad00020-306c-11d8-b4e9-00304f19f545");
      defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectColumnSchemeGuid, (object) attributeId2, NodeColumnSortOrder.None, -1), 250);
      defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.RelationColumnSchemeGuid, (object) sessionKeeper.Session.IdentHelper.GetAttributeID("cad00267-306c-11d8-b4e9-00304f19f545"), NodeColumnSortOrder.None, -1), 100);
    }
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.NavigatorColumnSchemeGuid, (object) "F_STATUSES"), 100);
    return defaultColumns;
  }

  public override NodeColumnCollection GetSupportedColumns(string ColumnSetName)
  {
    NodeColumnCollection columns = new NodeColumnCollection();
    Helper.AddObjectTypeColumns(columns, this._objTypeID);
    Helper.AddRelationTypeColumns(columns, this._relTypeID);
    Helper.AddObligatoryColumns(columns, true, true);
    Helper.AddObligatoryColumnsAdv(columns);
    Helper.AddObligatoryColumnsRelation(columns);
    Helper.AddObligatoryColumnsRelationAdv(columns);
    Helper.AddAllColumns(columns);
    Helper.AddAllColumnsRelation(columns);
    return columns;
  }

  protected override INodeQuery GetQuery(ConditionStructure[] conditions)
  {
    if (this._relTypeID != -1)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetRelationsApplicabilityCollection().GetApplicabilitiesList(this._relTypeID, -1, this._objTypeID).Rows.Count == 0)
          return (INodeQuery) null;
      }
    }
    return (INodeQuery) new SubstitutesQuery(this._services, (INodeQuerySupport) this, this._objID, this._objTypeID, this._role, this._relTypeID, conditions, this._filtrationOwnerID, this._contexts);
  }

  public override List<object> GetSpecialFields()
  {
    List<object> specialFields = base.GetSpecialFields();
    if (!specialFields.Contains((object) ObjectsPartBase.ncF_ID))
      specialFields.Add((object) ObjectsPartBase.ncF_ID);
    if (!specialFields.Contains((object) ObjectsPartBase.ncCAPTION))
      specialFields.Add((object) ObjectsPartBase.ncCAPTION);
    if (!specialFields.Contains((object) ObjectsPartBase.ncOWNER))
      specialFields.Add((object) ObjectsPartBase.ncOWNER);
    if (!specialFields.Contains((object) ObjectsPartBase.ncSORTING))
      specialFields.Add((object) ObjectsPartBase.ncSORTING);
    if (!specialFields.Contains((object) ObjectsPartBase.ncVERSION))
      specialFields.Add((object) ObjectsPartBase.ncVERSION);
    if (!specialFields.Contains((object) ObjectsPartBase.ncBASE_VERSION))
      specialFields.Add((object) ObjectsPartBase.ncBASE_VERSION);
    if (!specialFields.Contains((object) ObjectsPartBase.ncSITE_ID))
      specialFields.Add((object) ObjectsPartBase.ncSITE_ID);
    if (!specialFields.Contains((object) ObjectsPartBase.ncMODIFICATION_ID))
      specialFields.Add((object) ObjectsPartBase.ncMODIFICATION_ID);
    if (this.ncSubstitutesGroupNoID == null)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        this.ncSubstitutesGroupNoID = new NodeColumnID((object) sessionKeeper.Session.IdentHelper.SubstitutesGroupNoID, AttributeSourceTypes.Relation);
        this.ncSubstituteInGroup = new NodeColumnID((object) sessionKeeper.Session.IdentHelper.SubstituteInGroup, AttributeSourceTypes.Relation);
      }
    }
    if (!specialFields.Contains((object) this.ncSubstitutesGroupNoID))
      specialFields.Add((object) this.ncSubstitutesGroupNoID);
    if (!specialFields.Contains((object) this.ncSubstituteInGroup))
      specialFields.Add((object) this.ncSubstituteInGroup);
    if (!specialFields.Contains((object) ObjectsPartBase.ncF_LC_STEP))
      specialFields.Add((object) ObjectsPartBase.ncF_LC_STEP);
    if (this.ncAdvAttributes != null)
    {
      for (int index = 0; index < this.ncAdvAttributes.Count; ++index)
      {
        if (!specialFields.Contains((object) this.ncAdvAttributes[index]))
          specialFields.Add((object) this.ncAdvAttributes[index]);
      }
    }
    return specialFields;
  }

  public override INode GetChild(INodeID nodeID)
  {
    return nodeID is SubstitutesNodeID substitutesNodeId ? (INode) new SubstitutesNode(substitutesNodeId.Services, substitutesNodeId.FiltrationOwnerID, substitutesNodeId.Contexts, substitutesNodeId.ProjObjType, substitutesNodeId.ProjID, substitutesNodeId.ObjectID, substitutesNodeId.ObjectTypeID, substitutesNodeId.RelationTypeID, substitutesNodeId.PrjLinkID, substitutesNodeId.LCStepID, substitutesNodeId.Caption, substitutesNodeId.SubstitutesGroupNoID, substitutesNodeId.SubstituteInGroup, substitutesNodeId.CheckedOutBy, substitutesNodeId.Owner, substitutesNodeId.Sorting, substitutesNodeId.Version, substitutesNodeId.BaseVersion, substitutesNodeId.Attributes, substitutesNodeId.Values) : (INode) null;
  }
}
