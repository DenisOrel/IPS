// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Controls.ChartDisplayControl
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Statistics.Interfaces;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics.Controls;

public class ChartDisplayControl : UserControl
{
  private CollectedStatistics _collectedStatistics;
  private readonly ContextMenu _chartMenu = new ContextMenu();
  private readonly MenuItem _btnShowLegend = new MenuItem();
  private bool _chartIsDrown;
  private IContainer components;
  private TabControl tabControl1;
  private TabPage lineChartTabPage;
  private TabPage columnChartTabPage;
  private PlotView linearChartView;
  private Label noData;
  private PlotView columnChartView;
  private Label lblBuildLinearChart;
  private Label lblBuildChartColumn;

  public bool ChartIsDrown => this._chartIsDrown;

  public ChartDisplayControl()
  {
    this.InitializeComponent();
    this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
    this._btnShowLegend.Checked = true;
    this._btnShowLegend.Text = "Легенда";
    this._btnShowLegend.Click += new EventHandler(this.btnShowLegend_Click);
    this._chartMenu.MenuItems.Add(this._btnShowLegend);
    this.linearChartView.ContextMenu = this._chartMenu;
    this.columnChartView.ContextMenu = this._chartMenu;
  }

  public void OpenChart(CollectedStatistics collectedStatistics)
  {
    try
    {
      this._collectedStatistics = collectedStatistics;
      this.DrawChart();
    }
    catch (Exception ex)
    {
      throw new KernelException("Возникла ошибка при построении графика.", ex);
    }
  }

  public void SetEmptyPage(string message)
  {
    this._collectedStatistics = (CollectedStatistics) null;
    this._chartIsDrown = false;
    this.noData.Visible = true;
    this.noData.Text = message;
    this.tabControl1.Visible = false;
    this.ClearModels();
  }

  public void SetWaitingPage()
  {
    this._chartIsDrown = false;
    this.lblBuildChartColumn.Visible = this.lblBuildLinearChart.Visible = true;
    this.tabControl1.Visible = true;
    this.ClearModels();
  }

  public void PrintCurrentChart()
  {
    StatisticsPlotPrinter statisticsPlotPrinter = (StatisticsPlotPrinter) null;
    if (this.tabControl1.SelectedTab == this.lineChartTabPage)
      statisticsPlotPrinter = new StatisticsPlotPrinter(this.linearChartView);
    else if (this.tabControl1.SelectedTab == this.columnChartTabPage)
      statisticsPlotPrinter = new StatisticsPlotPrinter(this.columnChartView);
    statisticsPlotPrinter?.Print();
  }

  private void DrawChart()
  {
    if (this._collectedStatistics.StatisticsResultValues.Count > StatisticsConst.MAX_RESULT_VALUE_AMOUNT_ON_CHART)
      this.SetEmptyPage(StatisticsConst.TOO_MUCH_DATA_MESSAGE);
    else if (!this._collectedStatistics.HasValuePoints())
    {
      this.SetEmptyPage(StatisticsConst.CHART_ABSENCE_MESSAGE);
    }
    else
    {
      try
      {
        if (this.TryDrawChart())
        {
          this.noData.Visible = false;
          this.lblBuildChartColumn.Visible = this.lblBuildLinearChart.Visible = false;
          this.tabControl1.Visible = true;
          this._chartIsDrown = true;
        }
        else
          this.SetEmptyPage(StatisticsConst.CHART_ABSENCE_MESSAGE);
      }
      catch
      {
        this.SetEmptyPage(StatisticsConst.CHART_ABSENCE_MESSAGE);
        throw;
      }
    }
  }

  private bool TryDrawChart()
  {
    if (this.tabControl1.SelectedTab == this.tabControl1.TabPages["lineChartTabPage"])
      return this.linearChartView.Model != null || this.PlaceStatisticsOnLinearPlot();
    if (this.tabControl1.SelectedTab != this.tabControl1.TabPages["columnChartTabPage"])
      return false;
    return this.columnChartView.Model != null || this.PlaceStatisticsOnColumnPlot();
  }

  private bool PlaceStatisticsOnColumnPlot()
  {
    this.columnChartView.Model = new PlotModel()
    {
      Title = this._collectedStatistics.Caption,
      LegendSymbolLength = 24.0
    };
    this.columnChartView.Model.LegendPlacement = LegendPlacement.Outside;
    this.columnChartView.Model.LegendPosition = LegendPosition.BottomLeft;
    this.columnChartView.Model.LegendOrientation = LegendOrientation.Horizontal;
    this.columnChartView.Model.LegendBorder = OxyColors.Black;
    CategoryAxis categoryAxis1 = new CategoryAxis();
    categoryAxis1.Position = AxisPosition.Bottom;
    categoryAxis1.Angle = 30.0;
    categoryAxis1.StringFormat = this.GetDatePattern(this._collectedStatistics.CollectPeriod);
    CategoryAxis categoryAxis2 = categoryAxis1;
    LinearAxis yaxisForGraphics = this.GetYAxisForGraphics();
    this.columnChartView.Model.Axes.Add((Axis) categoryAxis2);
    this.columnChartView.Model.Axes.Add((Axis) yaxisForGraphics);
    OxyPlotColumnChartPresenter columnChartPresenter = new OxyPlotColumnChartPresenter(this._collectedStatistics);
    foreach (Graph graph in columnChartPresenter.Graphs)
      this.SetGraphToColumnSeries(graph);
    foreach (Period period in columnChartPresenter.GetPeriods())
    {
      switch (this._collectedStatistics.CollectPeriod)
      {
        case CollectPeriodsEnum.Hour:
          categoryAxis2.Labels.Add(period.EndDateTime.ToString($"{CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern} {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}"));
          continue;
        case CollectPeriodsEnum.Day:
          categoryAxis2.Labels.Add(period.StartDateTime.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
          continue;
        case CollectPeriodsEnum.Week:
          categoryAxis2.Labels.Add(period.StartDateTime.ToString($"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}-{period.EndDateTime.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern)}"));
          continue;
        case CollectPeriodsEnum.Month:
          categoryAxis2.Labels.Add(string.Format($"{period.StartDateTime.Month.ToString((IFormatProvider) CultureInfo.CurrentCulture)}.{period.StartDateTime.Year.ToString((IFormatProvider) CultureInfo.CurrentCulture)}"));
          continue;
        case CollectPeriodsEnum.Year:
          categoryAxis2.Labels.Add(period.StartDateTime.Year.ToString());
          continue;
        default:
          continue;
      }
    }
    int index = this.columnChartView.Model.Axes.IndexOf((Axis) yaxisForGraphics);
    this.columnChartView.Model.Axes[index].AbsoluteMaximum = columnChartPresenter.MaxValue + 1.0;
    this.columnChartView.Model.Axes[index].AbsoluteMinimum = columnChartPresenter.MinValue;
    this.columnChartView.Model.IsLegendVisible = this._btnShowLegend.Checked;
    return true;
  }

  private void SetGraphToColumnSeries(Graph graph)
  {
    ColumnSeries columnSeries1 = new ColumnSeries();
    columnSeries1.Title = graph.Caption;
    ColumnSeries columnSeries2 = columnSeries1;
    for (int index = 1; index < this._collectedStatistics.Periods.Count; ++index)
      columnSeries2.Items.Add(new ColumnItem(graph.Points[index].Y));
    this.columnChartView.Model.Series.Add((OxyPlot.Series.Series) columnSeries2);
  }

  private bool PlaceStatisticsOnLinearPlot()
  {
    this.linearChartView.Model = new PlotModel()
    {
      Title = this._collectedStatistics.Caption,
      LegendSymbolLength = 24.0
    };
    this.linearChartView.Model.LegendPlacement = LegendPlacement.Outside;
    this.linearChartView.Model.LegendPosition = LegendPosition.BottomLeft;
    this.linearChartView.Model.LegendOrientation = LegendOrientation.Horizontal;
    this.linearChartView.Model.LegendBorder = OxyColors.Black;
    DateTimeAxisWithDeterminedTicks withDeterminedTicks1 = new DateTimeAxisWithDeterminedTicks();
    withDeterminedTicks1.Position = AxisPosition.Bottom;
    withDeterminedTicks1.AbsoluteMaximum = DateTimeAxis.ToDouble(this._collectedStatistics.EndDateTime) + 50.0;
    withDeterminedTicks1.AbsoluteMinimum = DateTimeAxis.ToDouble(this._collectedStatistics.StartDateTime) - 1.0;
    withDeterminedTicks1.Angle = 30.0;
    withDeterminedTicks1.StringFormat = this.GetDatePattern(this._collectedStatistics.CollectPeriod);
    withDeterminedTicks1.TickValues = this.GetTicksValues(this._collectedStatistics.Periods);
    DateTimeAxisWithDeterminedTicks withDeterminedTicks2 = withDeterminedTicks1;
    LinearAxis yaxisForGraphics = this.GetYAxisForGraphics();
    this.linearChartView.Model.Axes.Add((Axis) withDeterminedTicks2);
    this.linearChartView.Model.Axes.Add((Axis) yaxisForGraphics);
    OxyPlotLinearChartPresenter linearChartPresenter = new OxyPlotLinearChartPresenter(this._collectedStatistics);
    foreach (Graph graph in linearChartPresenter.Graphs)
      this.SetGraphToLineSeries(graph);
    int index = this.linearChartView.Model.Axes.IndexOf((Axis) yaxisForGraphics);
    this.linearChartView.Model.Axes[index].AbsoluteMaximum = linearChartPresenter.MaxValue + 1.0;
    this.linearChartView.Model.Axes[index].AbsoluteMinimum = linearChartPresenter.MinValue - 1.0;
    this.linearChartView.Model.IsLegendVisible = this._btnShowLegend.Checked;
    return true;
  }

  private IList<double> GetTicksValues(List<Period> collectedStatisticsPeriods)
  {
    List<double> ticksValues = new List<double>();
    foreach (Period statisticsPeriod in collectedStatisticsPeriods)
      ticksValues.Add(Axis.ToDouble((object) statisticsPeriod.EndDateTime));
    return (IList<double>) ticksValues;
  }

  private string GetDatePattern(
    CollectPeriodsEnum collectedStatisticsCollectPeriod)
  {
    return collectedStatisticsCollectPeriod == CollectPeriodsEnum.Hour ? $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern} {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}" : CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
  }

  private LinearAxis GetYAxisForGraphics()
  {
    switch (this._collectedStatistics.StatisticsType)
    {
      case CommandStatisticsTypesEnum.CreatedDate:
      case CommandStatisticsTypesEnum.SignDate:
      case CommandStatisticsTypesEnum.LCStepDate:
      case CommandStatisticsTypesEnum.LCLevelDate:
      case CommandStatisticsTypesEnum.DateAttrValue:
      case CommandStatisticsTypesEnum.RevertCountTask:
        LinearAxis yaxisForGraphics1 = new LinearAxis();
        yaxisForGraphics1.Position = AxisPosition.Left;
        return yaxisForGraphics1;
      case CommandStatisticsTypesEnum.ProcessTemplate:
      case CommandStatisticsTypesEnum.TimeInTask:
      case CommandStatisticsTypesEnum.TimeOneTaskFormUsers:
        DaysHoursMinutesTimeSpanAxis yaxisForGraphics2 = new DaysHoursMinutesTimeSpanAxis();
        yaxisForGraphics2.Position = AxisPosition.Left;
        yaxisForGraphics2.AbsoluteMinimum = 0.0;
        return (LinearAxis) yaxisForGraphics2;
      default:
        throw new Exception("Неверный тип сбора статистики");
    }
  }

  private void SetGraphToLineSeries(Graph graph)
  {
    LineSeries lineSeries1 = new LineSeries();
    lineSeries1.Title = graph.Caption;
    lineSeries1.StrokeThickness = 3.0;
    lineSeries1.LineStyle = LineStyle.Automatic;
    lineSeries1.MarkerType = MarkerType.Circle;
    lineSeries1.MarkerSize = 5.0;
    lineSeries1.MarkerStroke = OxyColors.White;
    lineSeries1.MarkerFill = OxyColors.Automatic;
    lineSeries1.MarkerStrokeThickness = 1.5;
    lineSeries1.DataFieldX = "PeriodsEnd";
    lineSeries1.DataFieldY = "Value";
    LineSeries lineSeries2 = lineSeries1;
    lineSeries2.Points.AddRange((IEnumerable<DataPoint>) graph.Points);
    this.linearChartView.Model.Series.Add((OxyPlot.Series.Series) lineSeries2);
  }

  private void ClearModels()
  {
    this.linearChartView.Model = (PlotModel) null;
    this.columnChartView.Model = (PlotModel) null;
  }

  private void btnShowLegend_Click(object sender, EventArgs e)
  {
    if (this._btnShowLegend.Checked)
    {
      if (this.linearChartView.Model != null && this.linearChartView.Model.IsLegendVisible)
      {
        this.linearChartView.Model.IsLegendVisible = false;
        this.linearChartView.InvalidatePlot(false);
      }
      if (this.columnChartView.Model != null && this.columnChartView.Model.IsLegendVisible)
      {
        this.columnChartView.Model.IsLegendVisible = false;
        this.columnChartView.InvalidatePlot(false);
      }
      this._btnShowLegend.Checked = false;
    }
    else
    {
      if (this.linearChartView.Model != null && !this.linearChartView.Model.IsLegendVisible)
      {
        this.linearChartView.Model.IsLegendVisible = true;
        this.linearChartView.InvalidatePlot(false);
      }
      if (this.columnChartView.Model != null && !this.columnChartView.Model.IsLegendVisible)
      {
        this.columnChartView.Model.IsLegendVisible = true;
        this.columnChartView.InvalidatePlot(false);
      }
      this._btnShowLegend.Checked = true;
    }
  }

  private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this._collectedStatistics == null)
      return;
    try
    {
      this.DrawChart();
    }
    catch (Exception ex)
    {
      throw new KernelException("Возникла ошибка при построении графика.", ex);
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.tabControl1 = new TabControl();
    this.columnChartTabPage = new TabPage();
    this.lblBuildChartColumn = new Label();
    this.columnChartView = new PlotView();
    this.lineChartTabPage = new TabPage();
    this.lblBuildLinearChart = new Label();
    this.linearChartView = new PlotView();
    this.noData = new Label();
    this.tabControl1.SuspendLayout();
    this.columnChartTabPage.SuspendLayout();
    this.lineChartTabPage.SuspendLayout();
    this.SuspendLayout();
    this.tabControl1.Controls.Add((Control) this.columnChartTabPage);
    this.tabControl1.Controls.Add((Control) this.lineChartTabPage);
    this.tabControl1.Dock = DockStyle.Fill;
    this.tabControl1.Location = new Point(0, 0);
    this.tabControl1.Name = "tabControl1";
    this.tabControl1.SelectedIndex = 0;
    this.tabControl1.Size = new Size(555, 407);
    this.tabControl1.TabIndex = 0;
    this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
    this.columnChartTabPage.AutoScroll = true;
    this.columnChartTabPage.AutoScrollMinSize = new Size(400, 400);
    this.columnChartTabPage.Controls.Add((Control) this.lblBuildChartColumn);
    this.columnChartTabPage.Controls.Add((Control) this.columnChartView);
    this.columnChartTabPage.Location = new Point(4, 22);
    this.columnChartTabPage.Name = "columnChartTabPage";
    this.columnChartTabPage.Padding = new Padding(3);
    this.columnChartTabPage.Size = new Size(547, 381);
    this.columnChartTabPage.TabIndex = 1;
    this.columnChartTabPage.Text = "Гистограмма";
    this.columnChartTabPage.UseVisualStyleBackColor = true;
    this.lblBuildChartColumn.Anchor = AnchorStyles.None;
    this.lblBuildChartColumn.AutoSize = true;
    this.lblBuildChartColumn.BackColor = SystemColors.Control;
    this.lblBuildChartColumn.Location = new Point(168, 185);
    this.lblBuildChartColumn.Name = "lblBuildChartColumn";
    this.lblBuildChartColumn.Size = new Size(143, 13);
    this.lblBuildChartColumn.TabIndex = 3;
    this.lblBuildChartColumn.Text = "Выполняются расчеты.";
    this.lblBuildChartColumn.Visible = false;
    this.columnChartView.Dock = DockStyle.Fill;
    this.columnChartView.Location = new Point(3, 3);
    this.columnChartView.Name = "columnChartView";
    this.columnChartView.PanCursor = Cursors.Hand;
    this.columnChartView.Size = new Size(524, 400);
    this.columnChartView.TabIndex = 2;
    this.columnChartView.Text = "plotView2";
    this.columnChartView.ZoomHorizontalCursor = Cursors.SizeWE;
    this.columnChartView.ZoomRectangleCursor = Cursors.SizeNWSE;
    this.columnChartView.ZoomVerticalCursor = Cursors.SizeNS;
    this.lineChartTabPage.AutoScroll = true;
    this.lineChartTabPage.AutoScrollMinSize = new Size(400, 400);
    this.lineChartTabPage.BackColor = SystemColors.Control;
    this.lineChartTabPage.Controls.Add((Control) this.lblBuildLinearChart);
    this.lineChartTabPage.Controls.Add((Control) this.linearChartView);
    this.lineChartTabPage.Location = new Point(4, 22);
    this.lineChartTabPage.Name = "lineChartTabPage";
    this.lineChartTabPage.Padding = new Padding(3);
    this.lineChartTabPage.Size = new Size(547, 381);
    this.lineChartTabPage.TabIndex = 0;
    this.lineChartTabPage.Text = "Линейный график";
    this.lblBuildLinearChart.Anchor = AnchorStyles.None;
    this.lblBuildLinearChart.AutoSize = true;
    this.lblBuildLinearChart.BackColor = SystemColors.Control;
    this.lblBuildLinearChart.Location = new Point(168, 185);
    this.lblBuildLinearChart.Name = "lblBuildLinearChart";
    this.lblBuildLinearChart.Size = new Size(143, 13);
    this.lblBuildLinearChart.TabIndex = 2;
    this.lblBuildLinearChart.Text = "Выполняются расчеты.";
    this.lblBuildLinearChart.Visible = false;
    this.linearChartView.Dock = DockStyle.Fill;
    this.linearChartView.Location = new Point(3, 3);
    this.linearChartView.Name = "linearChartView";
    this.linearChartView.PanCursor = Cursors.Hand;
    this.linearChartView.Size = new Size(524, 400);
    this.linearChartView.TabIndex = 1;
    this.linearChartView.Text = "plotView2";
    this.linearChartView.ZoomHorizontalCursor = Cursors.SizeWE;
    this.linearChartView.ZoomRectangleCursor = Cursors.SizeNWSE;
    this.linearChartView.ZoomVerticalCursor = Cursors.SizeNS;
    this.noData.BackColor = SystemColors.Control;
    this.noData.Dock = DockStyle.Fill;
    this.noData.Location = new Point(0, 0);
    this.noData.Name = "noData";
    this.noData.Size = new Size(555, 407);
    this.noData.TabIndex = 2;
    this.noData.Text = "Отсутствуют данные для отображения графика.";
    this.noData.TextAlign = ContentAlignment.MiddleCenter;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tabControl1);
    this.Controls.Add((Control) this.noData);
    this.Name = nameof (ChartDisplayControl);
    this.Size = new Size(555, 407);
    this.tabControl1.ResumeLayout(false);
    this.columnChartTabPage.ResumeLayout(false);
    this.columnChartTabPage.PerformLayout();
    this.lineChartTabPage.ResumeLayout(false);
    this.lineChartTabPage.PerformLayout();
    this.ResumeLayout(false);
  }
}
