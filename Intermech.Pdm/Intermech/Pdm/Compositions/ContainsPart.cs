// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.ContainsPart
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
namespace Intermech.Pdm.Compositions;

internal class ContainsPart(int objectType, IServiceProvider services) : ObjectsPart(objectType, services)
{
  protected long counter;
  private static readonly NodeColumnID ncPRJLINK_ID = new NodeColumnID((object) ObligatoryObjectAttributes.F_PRJLINK_ID, AttributeSourceTypes.Relation);
  private static readonly NodeColumnID ncRELATION_TYPE = new NodeColumnID((object) ObligatoryObjectAttributes.F_RELATION_TYPE, AttributeSourceTypes.Relation);
  private static readonly NodeColumnID ncSortIndex = new NodeColumnID((object) MetaDataHelper.GetAttributeTypeID("cad00202-306c-11d8-b4e9-00304f19f545"), AttributeSourceTypes.Relation);
  private static readonly NodeColumnID ncPROJ_ID = new NodeColumnID((object) ObligatoryObjectAttributes.F_PROJ_ID, AttributeSourceTypes.Relation);
  private static readonly NodeColumnID ncPRJ_GUID = new NodeColumnID((object) ObligatoryObjectAttributes.F_PRJ_GUID, AttributeSourceTypes.Relation);

  protected override INodeQuery GetQuery(ConditionStructure[] conditions)
  {
    this.counter = 0L;
    return this.Owner is ContainsNode owner && owner.ObjectID != 0L && owner.Scheme != null ? (INodeQuery) new ContainsQuery((INodeQuerySupport) this, owner.ObjectID, owner.Scheme, owner.Reader, owner.InProducts, owner.RealQuery) : (INodeQuery) null;
  }

  public override NodeColumnCollection GetDefaultColumns() => ((ContainsNode) this.Owner).Columns;

  public override object MapColumnToField(NodeColumn column)
  {
    object field = base.MapColumnToField(column);
    if (field != null)
      return field;
    if (column.SchemeGuid == Intermech.Navigator.Consts.ObjectColumnSchemeGuid || column.SchemeGuid == Intermech.Navigator.Consts.CurrentObjectColumnSchemeGuid || column.SchemeGuid == Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid)
      return (object) new NodeColumnID(column.ID, AttributeSourceTypes.Object);
    return column.SchemeGuid == Intermech.Navigator.Consts.RelationColumnSchemeGuid || column.SchemeGuid == Intermech.Navigator.Consts.CurrentRelationColumnSchemeGuid || column.SchemeGuid == Intermech.Navigator.Consts.RelationObligatoryColumnSchemeGuid ? (object) new NodeColumnID(column.ID, AttributeSourceTypes.Relation) : (object) null;
  }

  public override List<object> GetSpecialFields()
  {
    List<object> specialFields = new List<object>();
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    specialFields.Add((object) ObjectsPartBase.ncF_OBJECT_TYPE);
    specialFields.Add((object) ObjectsPartBase.ncF_OBJECT_ID);
    specialFields.Add((object) ObjectsPartBase.ncF_ID);
    specialFields.Add((object) ObjectsPartBase.ncF_CHKOUT_BY);
    specialFields.Add((object) ContainsPart.ncPRJLINK_ID);
    specialFields.Add((object) ContainsPart.ncRELATION_TYPE);
    specialFields.Add((object) ObjectsPartBase.ncF_LC_STEP);
    specialFields.Add((object) ObjectsPartBase.ncCAPTION);
    specialFields.Add((object) ObjectsPartBase.ncOWNER);
    specialFields.Add((object) ObjectsPartBase.ncVERSION);
    specialFields.Add((object) ObjectsPartBase.ncBASE_VERSION);
    specialFields.Add((object) ObjectsPartBase.ncSITE_ID);
    specialFields.Add((object) ObjectsPartBase.ncMODIFICATION_ID);
    specialFields.Add((object) ContainsPart.ncSortIndex);
    specialFields.Add((object) ContainsPart.ncPROJ_ID);
    specialFields.Add((object) ContainsPart.ncPRJ_GUID);
    return specialFields;
  }

  public override INodeID CreateNodeId(object[] fieldValues, RecordAdapter adapter)
  {
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    int int32_1 = Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_OBJECT_TYPE)]);
    long int64_1 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_OBJECT_ID)]);
    long int64_2 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_ID)]);
    long int64_3 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_CHKOUT_BY)]);
    object fieldValue1 = fieldValues[adapter.GetFieldIndex((object) ContainsPart.ncPRJLINK_ID)];
    long int64_4 = fieldValue1 != DBNull.Value ? Convert.ToInt64(fieldValue1) : 0L;
    int int32_2 = Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) ContainsPart.ncRELATION_TYPE)]);
    int int32_3 = Convert.ToInt32(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncF_LC_STEP)]);
    string caption = Convert.ToString(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncCAPTION)]);
    long int64_5 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncOWNER)]);
    long int64_6 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncVERSION)]);
    long int64_7 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncBASE_VERSION)]);
    string siteID = Convert.ToString(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncSITE_ID)]);
    long int64_8 = Convert.ToInt64(fieldValues[adapter.GetFieldIndex((object) ObjectsPartBase.ncMODIFICATION_ID)]);
    object fieldValue2 = fieldValues[adapter.GetFieldIndex((object) ContainsPart.ncPROJ_ID)];
    long int64_9 = fieldValue2 != DBNull.Value ? Convert.ToInt64(fieldValue2) : 0L;
    Guid guidValue = DataSetProcessor.GetGuidValue(fieldValues[adapter.GetFieldIndex((object) ContainsPart.ncPRJ_GUID)], Guid.Empty);
    int fieldIndex = adapter.GetFieldIndex((object) ContainsPart.ncSortIndex);
    long int64_10 = fieldIndex < 0 || fieldValues[fieldIndex] == DBNull.Value ? 0L : Convert.ToInt64(fieldValues[fieldIndex]);
    return (INodeID) new ContainsNodeID((object) new Tuple<long, long, long>(int64_4, int64_1, int64_4 != 0L ? 0L : ++this.counter), new CreateObjectNodeParams(int32_1, int64_1, int64_2, int64_3, int64_4, int32_3, caption, int32_2, int64_5, int64_10, ObjectFiltrationState.fsNotRequired, int64_6, int64_7, siteID, int64_9, guidValue, int64_8));
  }

  public override NodeColumnCollection GetSupportedColumns(string ColumnSetName)
  {
    NodeColumnCollection columns = new NodeColumnCollection();
    if (((ContainsNode) this.Owner).SchemeObjectTypes != null)
    {
      foreach (int schemeObjectType in ((ContainsNode) this.Owner).SchemeObjectTypes)
        Helper.AddObjectTypeColumns(columns, schemeObjectType);
    }
    if (((ContainsNode) this.Owner).SchemeRelationTypes != null)
    {
      foreach (int schemeRelationType in ((ContainsNode) this.Owner).SchemeRelationTypes)
        Helper.AddRelationTypeColumns(columns, schemeRelationType);
    }
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    foreach (ObligatoryObjectAttributes objectAttributes in Enum.GetValues(typeof (ObligatoryObjectAttributes)))
    {
      if (ObligatoryObjectAttributesHelper.GetAttributeSourceType(objectAttributes) == AttributeSourceTypes.Object)
        columns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) objectAttributes));
      else if (ObligatoryObjectAttributesHelper.GetAttributeSourceType(objectAttributes) == AttributeSourceTypes.Relation)
        columns.Add(service.CreateColumn(Intermech.Navigator.Consts.RelationObligatoryColumnSchemeGuid, (object) objectAttributes));
    }
    Helper.AddAllColumns(columns);
    Helper.AddAllColumnsRelation(columns);
    return columns;
  }
}
