// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.TableColumn
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Reporting;

public class TableColumn
{
  public TableColumn()
  {
    this.Width = double.NaN;
    this.Alignment = Alignment.Center;
  }

  public double ActualWidth { get; internal set; }

  public Alignment Alignment { get; set; }

  public bool IsHeader { get; set; }

  public double Width { get; set; }
}
