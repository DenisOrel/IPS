// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.TableRow
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System.Collections.Generic;

#nullable disable
namespace OxyPlot.Reporting;

public class TableRow
{
  public TableRow() => this.Cells = (IList<TableCell>) new List<TableCell>();

  public IList<TableCell> Cells { get; private set; }

  public bool IsHeader { get; set; }
}
