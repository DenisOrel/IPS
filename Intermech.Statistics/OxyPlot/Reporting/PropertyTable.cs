// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.PropertyTable
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections;
using System.Linq;
using System.Reflection;

#nullable disable
namespace OxyPlot.Reporting;

public class PropertyTable : ItemsTable
{
  public PropertyTable(IEnumerable items, bool itemsInRows)
    : base(itemsInRows)
  {
    this.Alignment = Alignment.Left;
    object[] array = items.Cast<object>().ToArray<object>();
    this.UpdateFields((IEnumerable) array);
    this.Items = (IEnumerable) array;
  }

  private Type GetItemType(IEnumerable items)
  {
    Type itemType = (Type) null;
    foreach (object obj in items)
    {
      Type type = obj.GetType();
      if (itemType == (Type) null)
        itemType = type;
      if (type != itemType)
        return (Type) null;
    }
    return itemType;
  }

  private void UpdateFields(IEnumerable items)
  {
    Type itemType = this.GetItemType(items);
    if (itemType == (Type) null)
      return;
    this.Columns.Clear();
    foreach (PropertyInfo property in itemType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
      this.Fields.Add(new ItemsTableField(property.Name, property.Name, alignment: Alignment.Left));
  }
}
