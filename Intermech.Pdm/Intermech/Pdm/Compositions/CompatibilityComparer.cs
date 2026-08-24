// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompatibilityComparer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator.Queries;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompatibilityComparer(
  RecordMapping mapping,
  CompareObjectsInfo info,
  CompareDifferences currentDifferences,
  long curObjectID) : CompositionsComparer(mapping, info, currentDifferences, curObjectID)
{
  protected override bool CompareRow(
    IUserSession session,
    DataRow compareRow,
    long curObjectID,
    string idColumnName,
    long id,
    Dictionary<long, DataTable> results)
  {
    foreach (KeyValuePair<long, DataTable> result in results)
    {
      if (result.Key != curObjectID)
      {
        DataRow[] rows = this.Select(result.Value, idColumnName, (object) id);
        if (rows == null || rows.Length == 0)
          return false;
        if (this.info.CompareAttributes != null)
        {
          for (int index1 = 0; index1 < this.info.CompareAttributes.Count; ++index1)
          {
            IDBAttributeType attributeType = session.GetAttributeType(this.info.CompareAttributes[index1]);
            int index2 = this.info.ColumnAttributes.IndexOf(new NodeColumnID((object) attributeType.AttributeID, AttributeSourceTypes.Relation));
            if (index2 != -1 && !this.CompareAttribute(rows, compareRow, attributeType, index2))
              return false;
          }
        }
      }
    }
    return true;
  }
}
