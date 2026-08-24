// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareRuleComboItemsList
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class CompareRuleComboItemsList : List<CompareRuleComboItem>
{
  private void LoadList(IUserSession session, List<int> objectTypes)
  {
    DataTable dataTable = session.GetObjectCollection(new Guid("cadd9a98-306c-11d8-b4e9-00304f19f545")).Select(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[3]
    {
      new ColumnDescriptor((object) -2, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -50, SortOrders.ASC, 1),
      new ColumnDescriptor((object) -7, SortOrders.DESC, 0)
    }));
    this.Clear();
    RulesFilter rulesFilter1 = new RulesFilter(objectTypes[0]);
    RulesFilter rulesFilter2 = (RulesFilter) null;
    if (objectTypes.Count > 1)
      rulesFilter2 = new RulesFilter(objectTypes[1]);
    foreach (KeyValuePair<Guid, string> virtualScheme in VirtualCompoitionSettings.VirtualSchemes)
      this.Add(new CompareRuleComboItem(virtualScheme.Key, virtualScheme.Value));
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      IDBObject dbObject = session.GetObject(Convert.ToInt64(row[0]));
      IDBAttribute attributeByGuid = dbObject.GetAttributeByGuid(new Guid("cad00149-306c-11d8-b4e9-00304f19f545"));
      if (!rulesFilter1.InFilter(attributeByGuid) && (rulesFilter2 == null || !rulesFilter2.InFilter(attributeByGuid)))
        this.Add(new CompareRuleComboItem(dbObject.ObjectGUID, Convert.ToString(row[1])));
    }
  }

  public static CompareRuleComboItemsList Load(IUserSession session, List<int> objectTypes)
  {
    CompareRuleComboItemsList ruleComboItemsList = new CompareRuleComboItemsList();
    ruleComboItemsList.LoadList(session, objectTypes);
    return ruleComboItemsList;
  }
}
