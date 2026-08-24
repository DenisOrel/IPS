// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.Figure
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Reporting;

public abstract class Figure : ReportItem
{
  public int FigureNumber { get; set; }

  public string FigureText { get; set; }

  public string GetFullCaption(ReportStyle style)
  {
    return string.Format(style.FigureTextFormatString, (object) this.FigureNumber, (object) this.FigureText);
  }
}
