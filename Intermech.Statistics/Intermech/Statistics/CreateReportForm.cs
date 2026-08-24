// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.CreateReportForm
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Client.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public class CreateReportForm : Form
{
  private StatisticNodeItem _statisticNodeItem;
  private IContainer components;
  private Panel btnPanel;
  private Button btnCancel;
  private Button btnCreateReport;
  private ReportParametersControl reportParametersControl;

  public CreateReportForm()
  {
    this.InitializeComponent();
    this.btnCreateReport.Click += new EventHandler(this.reportParametersControl.On_Panel_Closing);
  }

  private void btnCancel_Click(object sender, EventArgs e) => this.Close();

  private void btnCreateReport_Click(object sender, EventArgs e)
  {
    try
    {
      new StatisticsReportGenerator(this.reportParametersControl.AssembleReportParams(), this._statisticNodeItem).Generate();
      this.Close();
    }
    catch (Exception ex)
    {
      throw new KernelException("Ошибка в генерации отчета статистики. Внутренняя ошибка: " + ex.Message, ex);
    }
  }

  public void Build(StatisticNodeItem statisticNodeItem)
  {
    this._statisticNodeItem = statisticNodeItem;
    this.reportParametersControl.Build(statisticNodeItem);
  }

  private void CreateReportForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void CreateReportForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.btnPanel = new Panel();
    this.btnCancel = new Button();
    this.btnCreateReport = new Button();
    this.reportParametersControl = new ReportParametersControl();
    this.btnPanel.SuspendLayout();
    this.SuspendLayout();
    this.btnPanel.Controls.Add((Control) this.btnCancel);
    this.btnPanel.Controls.Add((Control) this.btnCreateReport);
    this.btnPanel.Dock = DockStyle.Bottom;
    this.btnPanel.Location = new Point(0, 433);
    this.btnPanel.Name = "btnPanel";
    this.btnPanel.Size = new Size(522, 52);
    this.btnPanel.TabIndex = 0;
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(259, 10);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(121, 30);
    this.btnCancel.TabIndex = 1;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.btnCreateReport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCreateReport.Location = new Point(386, 10);
    this.btnCreateReport.Name = "btnCreateReport";
    this.btnCreateReport.Size = new Size(124, 30);
    this.btnCreateReport.TabIndex = 0;
    this.btnCreateReport.Text = "Сформировать отчет";
    this.btnCreateReport.UseVisualStyleBackColor = true;
    this.btnCreateReport.Click += new EventHandler(this.btnCreateReport_Click);
    this.reportParametersControl.Dock = DockStyle.Fill;
    this.reportParametersControl.Location = new Point(0, 0);
    this.reportParametersControl.Name = "reportParametersControl";
    this.reportParametersControl.Size = new Size(522, 433);
    this.reportParametersControl.TabIndex = 1;
    this.AcceptButton = (IButtonControl) this.btnCreateReport;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(522, 485);
    this.Controls.Add((Control) this.reportParametersControl);
    this.Controls.Add((Control) this.btnPanel);
    this.Name = nameof (CreateReportForm);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Настройка параметров отчета";
    this.FormClosed += new FormClosedEventHandler(this.CreateReportForm_FormClosed);
    this.Load += new EventHandler(this.CreateReportForm_Load);
    this.btnPanel.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
