// Decompiled with JetBrains decompiler
// Type: OxyPlot.Reporting.ParagraphStyle
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

#nullable disable
namespace OxyPlot.Reporting;

public class ParagraphStyle
{
  private const string DefaultFont = "Arial";
  private const double DefaultFontSize = 11.0;
  private bool? bold;
  private string fontFamily;
  private double? fontSize;
  private bool? italic;
  private double? leftIndentation;
  private double? lineSpacing;
  private bool? pageBreakBefore;
  private double? rightIndentation;
  private double? spacingAfter;
  private double? spacingBefore;
  private OxyColor textColor;

  public ParagraphStyle BasedOn { get; set; }

  public bool Bold
  {
    get
    {
      if (this.bold.HasValue)
        return this.bold.Value;
      return this.BasedOn != null && this.BasedOn.Bold;
    }
    set => this.bold = new bool?(value);
  }

  public string FontFamily
  {
    get
    {
      if (this.fontFamily != null)
        return this.fontFamily;
      return this.BasedOn != null ? this.BasedOn.FontFamily : "Arial";
    }
    set => this.fontFamily = value;
  }

  public double FontSize
  {
    get
    {
      if (this.fontSize.HasValue)
        return this.fontSize.Value;
      return this.BasedOn != null ? this.BasedOn.FontSize : 11.0;
    }
    set => this.fontSize = new double?(value);
  }

  public bool Italic
  {
    get
    {
      if (this.italic.HasValue)
        return this.italic.Value;
      return this.BasedOn != null && this.BasedOn.Italic;
    }
    set => this.italic = new bool?(value);
  }

  public double LeftIndentation
  {
    get
    {
      if (this.leftIndentation.HasValue)
        return this.leftIndentation.Value;
      return this.BasedOn != null ? this.BasedOn.LeftIndentation : 0.0;
    }
    set => this.leftIndentation = new double?(value);
  }

  public double LineSpacing
  {
    get
    {
      if (this.lineSpacing.HasValue)
        return this.lineSpacing.Value;
      return this.BasedOn != null ? this.BasedOn.LineSpacing : 1.0;
    }
    set => this.lineSpacing = new double?(value);
  }

  public bool PageBreakBefore
  {
    get
    {
      if (this.pageBreakBefore.HasValue)
        return this.pageBreakBefore.Value;
      return this.BasedOn != null && this.BasedOn.PageBreakBefore;
    }
    set => this.pageBreakBefore = new bool?(value);
  }

  public double RightIndentation
  {
    get
    {
      if (this.rightIndentation.HasValue)
        return this.rightIndentation.Value;
      return this.BasedOn != null ? this.BasedOn.RightIndentation : 0.0;
    }
    set => this.rightIndentation = new double?(value);
  }

  public double SpacingAfter
  {
    get
    {
      if (this.spacingAfter.HasValue)
        return this.spacingAfter.Value;
      return this.BasedOn != null ? this.BasedOn.SpacingAfter : 0.0;
    }
    set => this.spacingAfter = new double?(value);
  }

  public double SpacingBefore
  {
    get
    {
      if (this.spacingBefore.HasValue)
        return this.spacingBefore.Value;
      return this.BasedOn != null ? this.BasedOn.SpacingBefore : 0.0;
    }
    set => this.spacingBefore = new double?(value);
  }

  public OxyColor TextColor
  {
    get
    {
      if (!this.textColor.IsUndefined())
        return this.textColor;
      return this.BasedOn != null ? this.BasedOn.TextColor : OxyColors.Black;
    }
    set => this.textColor = value;
  }
}
