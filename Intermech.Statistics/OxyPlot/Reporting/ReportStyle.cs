// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.ReportStyle
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Reporting;

public class ReportStyle
{
  public ReportStyle(string titleFontFamily = "Arial", string bodyTextFontFamily = "Verdana", string tableTextFontFamily = "Courier New")
  {
    this.DefaultStyle = new ParagraphStyle()
    {
      FontFamily = bodyTextFontFamily,
      FontSize = 11.0,
      SpacingAfter = 10.0
    };
    this.HeaderStyles = new ParagraphStyle[5];
    this.HeaderStyles[0] = new ParagraphStyle()
    {
      BasedOn = this.DefaultStyle,
      FontFamily = titleFontFamily,
      SpacingBefore = 12.0,
      SpacingAfter = 3.0
    };
    for (int index = 1; index < this.HeaderStyles.Length; ++index)
      this.HeaderStyles[index] = new ParagraphStyle()
      {
        BasedOn = this.HeaderStyles[index - 1]
      };
    foreach (ParagraphStyle headerStyle in this.HeaderStyles)
      headerStyle.Bold = true;
    this.HeaderStyles[0].FontSize = 16.0;
    this.HeaderStyles[1].FontSize = 14.0;
    this.HeaderStyles[2].FontSize = 13.0;
    this.HeaderStyles[3].FontSize = 12.0;
    this.HeaderStyles[4].FontSize = 11.0;
    this.HeaderStyles[0].PageBreakBefore = true;
    this.HeaderStyles[1].PageBreakBefore = false;
    this.BodyTextStyle = new ParagraphStyle()
    {
      BasedOn = this.DefaultStyle
    };
    this.FigureTextStyle = new ParagraphStyle()
    {
      BasedOn = this.DefaultStyle,
      Italic = true
    };
    this.TableTextStyle = new ParagraphStyle()
    {
      BasedOn = this.DefaultStyle,
      FontFamily = tableTextFontFamily,
      SpacingAfter = 0.0,
      LeftIndentation = 3.0,
      RightIndentation = 3.0
    };
    this.TableHeaderStyle = new ParagraphStyle()
    {
      BasedOn = this.TableTextStyle,
      Bold = true
    };
    this.TableCaptionStyle = new ParagraphStyle()
    {
      BasedOn = this.DefaultStyle,
      Italic = true,
      SpacingBefore = 10.0,
      SpacingAfter = 3.0
    };
    this.Margins = new OxyThickness(25.0);
    this.FigureTextFormatString = "Figure {0}. {1}";
    this.TableCaptionFormatString = "Table {0}. {1}";
  }

  public ParagraphStyle BodyTextStyle { get; set; }

  public ParagraphStyle DefaultStyle { get; set; }

  public string FigureTextFormatString { get; set; }

  public ParagraphStyle FigureTextStyle { get; set; }

  public ParagraphStyle[] HeaderStyles { get; set; }

  public OxyThickness Margins { get; set; }

  public string TableCaptionFormatString { get; set; }

  public ParagraphStyle TableCaptionStyle { get; set; }

  public ParagraphStyle TableHeaderStyle { get; set; }

  public ParagraphStyle TableTextStyle { get; set; }
}
