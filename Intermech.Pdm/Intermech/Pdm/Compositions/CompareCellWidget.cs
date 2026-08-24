// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareCellWidget
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls.VirtualTree;
using Intermech.Interfaces.Pdm;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class CompareCellWidget(RowWidget rowWidget, Column column) : ValueCellWidget(rowWidget, column)
{
  protected CompositionItem GetFromRow(Row row)
  {
    CompositionItem fromRow = (CompositionItem) null;
    if (row.Item is CompositionItem)
      fromRow = (CompositionItem) row.Item;
    return fromRow;
  }
}
