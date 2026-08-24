// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.StatisticsPlotPrinter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public class StatisticsPlotPrinter
{
  private readonly PlotView _plotView;
  private bool _isLegendVisibleByDefault;
  private bool _needPrintLegend;
  private int _printPageCount = 1;
  private int _currentPrintingPage = 1;
  private bool _printLegendsOutside;
  private bool _renderLegendsInsidePrintByDefault;
  private LegendPosition _legendPositionByDefault;
  private ElementCollection<Axis> _axesByDefault;
  private LegendOrientation _legendOrientationByDefault;
  private const int HeightBoundsForPicture = 70;
  private const int WidthBoundsForPicture = 90;

  public StatisticsPlotPrinter(PlotView plotView)
  {
    this._plotView = plotView;
    this._isLegendVisibleByDefault = plotView.Model.IsLegendVisible;
    this._renderLegendsInsidePrintByDefault = this._plotView.Model.RenderLegendsInsidePrint;
    this._legendPositionByDefault = this._plotView.Model.LegendPosition;
    this._legendOrientationByDefault = this._plotView.Model.LegendOrientation;
    this._axesByDefault = new ElementCollection<Axis>((Model) this._plotView.Model);
  }

  public void Print()
  {
    try
    {
      PlotPrintSettingsForm printSettingsForm = new PlotPrintSettingsForm();
      if (printSettingsForm.ShowDialog() != DialogResult.OK)
        return;
      this._printPageCount = printSettingsForm.PrintPageCount;
      this._needPrintLegend = printSettingsForm.NeedToPrintLegend;
      this._currentPrintingPage = 1;
      this._printLegendsOutside = false;
      PrintDialog printDialog = new PrintDialog()
      {
        Document = new PrintDocument()
      };
      printDialog.Document.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
      if (printDialog.ShowDialog() != DialogResult.OK)
        return;
      printDialog.Document.DocumentName = this._plotView.Model.Title;
      printDialog.Document.DefaultPageSettings.Landscape = true;
      printDialog.Document.Print();
    }
    finally
    {
      this._plotView.Model.IsLegendVisible = this._isLegendVisibleByDefault;
      this._plotView.Model.RenderLegendsInsidePrint = this._renderLegendsInsidePrintByDefault;
      this._plotView.Model.LegendPosition = this._legendPositionByDefault;
      this._plotView.Model.LegendOrientation = this._legendOrientationByDefault;
      foreach (Axis axis in this._axesByDefault)
        this._plotView.Model.Axes.Add(axis);
    }
  }

  private void pd_PrintPage(object sender, PrintPageEventArgs ev)
  {
    if (this._printPageCount == 1)
    {
      this._plotView.Model.IsLegendVisible = this._needPrintLegend;
      this.PrintCurrentModel(ev);
    }
    else
    {
      if (!this._needPrintLegend)
        this._plotView.Model.IsLegendVisible = false;
      ev.HasMorePages = this._currentPrintingPage < this._printPageCount;
      double absoluteMinimum = this._plotView.Model.DefaultXAxis.AbsoluteMinimum;
      double num = (this._plotView.Model.DefaultXAxis.AbsoluteMaximum - absoluteMinimum) / (double) this._printPageCount;
      this._plotView.Model.DefaultXAxis.Zoom(absoluteMinimum + (double) (this._currentPrintingPage - 1) * num, absoluteMinimum + (double) this._currentPrintingPage * num);
      this.PrintCurrentModel(ev);
      ++this._currentPrintingPage;
    }
  }

  private void PrintCurrentModel(PrintPageEventArgs ev)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      new PngExporter()
      {
        Width = (ev.PageBounds.Width - 90),
        Height = (ev.PageBounds.Height - 70),
        Background = OxyColors.White
      }.Export((IPlotModel) this._plotView.Model, (Stream) memoryStream);
      Image image = Image.FromStream((Stream) memoryStream);
      Point point = new Point(0, 0);
      ev.Graphics.DrawImage(image, point);
    }
  }
}
