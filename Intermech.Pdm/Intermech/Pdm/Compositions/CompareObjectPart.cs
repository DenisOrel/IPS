// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareObjectPart
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompareObjectPart(IServiceProvider services, int objectTypeID) : ContainsPart(objectTypeID, services)
{
  protected override INodeQuery GetQuery(ConditionStructure[] conditions)
  {
    this.counter = 0L;
    return this.Owner is CompareObjectNode owner ? (INodeQuery) new CompareObjectQuery((INodeQuerySupport) this, owner.Info, owner.CompareObjects, owner.ObjectID, owner.Reader, owner.CurrentDifferences) : (INodeQuery) null;
  }

  public override NodeColumnCollection GetSupportedColumns(string ColumnSetName)
  {
    NodeColumnCollection columns = new NodeColumnCollection();
    CompareObjectNode owner = (CompareObjectNode) this.Owner;
    if (owner.Info.RelationTypes != null && owner.Info.RelationTypes.Count > 0)
    {
      foreach (KeyValuePair<int, bool> relationType in owner.Info.RelationTypes)
        Helper.AddRelationTypeColumns(columns, relationType.Key);
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
    return columns;
  }

  public override NodeColumnCollection GetDefaultColumns()
  {
    CompareObjectNode owner = (CompareObjectNode) this.Owner;
    NodeColumnCollection defaultColumns = new NodeColumnCollection();
    IColumnSchemes service = (IColumnSchemes) ServicesManager.GetService(typeof (IColumnSchemes));
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_TYPE));
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_OBJECT_ID));
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_ID));
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.CAPTION));
    defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.ObjectObligatoryColumnSchemeGuid, (object) ObligatoryObjectAttributes.F_CHKOUT_BY));
    if (owner.Info.CompareAttributes != null && owner.Info.CompareAttributes.Count > 0)
    {
      for (int index = 0; index < owner.Info.CompareAttributes.Count; ++index)
        defaultColumns.Add(service.CreateColumn(Intermech.Navigator.Consts.RelationColumnSchemeGuid, (object) owner.Info.CompareAttributes[index]));
    }
    return defaultColumns;
  }
}
