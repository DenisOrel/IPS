// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.ReportParametersControl
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Document.Client.Report;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public class ReportParametersControl : UserControl
{
  private StatisticNodeItem _statisticNodeItem;
  private IContainer components;
  private Panel panel3;
  private ComboBox cbTemplate;
  private Label label2;
  private TextBox tbReportCaption;
  private Label label3;
  private TextBox tbReportName;
  private Label label1;
  private Label label4;
  private Label label5;
  private ComboBox cbReportDateFormat;
  private ComboBox cbDateFormat;
  private Label label6;
  private NumericUpDown numericUpDown1;
  private Panel pnlColumnNumber;
  private CheckBox cbShowOnlyIntervalStartDate;

  public ReportParametersControl()
  {
    this.InitializeComponent();
    this.Name = nameof (ReportParametersControl);
  }

  public void Build(StatisticNodeItem statisticNodeItem)
  {
    this._statisticNodeItem = statisticNodeItem;
    this.tbReportName.Text = this._statisticNodeItem.Caption;
    this.tbReportCaption.Text = this._statisticNodeItem.Caption;
    this.FillTemplates();
    this.FillDateFormat();
    this.FillReportCreatingDateFormat();
  }

  private void FillReportCreatingDateFormat()
  {
    this.cbReportDateFormat.Items.Clear();
    foreach (DatePrintFormats datePrintFormats in (DatePrintFormats[]) Enum.GetValues(typeof (DatePrintFormats)))
      this.cbReportDateFormat.Items.Add((object) new MyElement((object) datePrintFormats, EnumDescConverter.GetEnumDescription((Enum) datePrintFormats), (object) null));
    this.cbReportDateFormat.SelectedIndex = 0;
  }

  private void FillDateFormat()
  {
    this.cbDateFormat.Items.Clear();
    foreach (DatePrintFormats datePrintFormats in (DatePrintFormats[]) Enum.GetValues(typeof (DatePrintFormats)))
    {
      if (datePrintFormats != DatePrintFormats.None)
        this.cbDateFormat.Items.Add((object) new MyElement((object) datePrintFormats, EnumDescConverter.GetEnumDescription((Enum) datePrintFormats), (object) null));
    }
    this.cbDateFormat.SelectedIndex = 0;
  }

  public StatisticsReportParams AssembleReportParams()
  {
    try
    {
      return new StatisticsReportParams()
      {
        ReportCaption = this.tbReportCaption.Text,
        ReportName = this.tbReportName.Text,
        TemplateGuid = (Guid) ((MyElement) this.cbTemplate.SelectedItem).Value,
        ReportCreatingDateFormat = (DatePrintFormats) ((MyElement) this.cbReportDateFormat.SelectedItem).Value,
        DataColumnsNumber = Convert.ToInt32(this.numericUpDown1.Value),
        DateFormat = (DatePrintFormats) ((MyElement) this.cbDateFormat.SelectedItem).Value,
        ShowOnlyIntervalStartDate = this.cbShowOnlyIntervalStartDate.Checked
      };
    }
    catch (Exception ex)
    {
      throw new KernelException("Ошибка при сохранении параметров отчета.", ex);
    }
  }

  private void FillTemplates()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.cbTemplate.Items.Clear();
      QuickObjectInfo objectInfo1 = sessionKeeper.Session.GetObjectInfo(StatisticsConst.MultilevelHorizontalTemplateGuid);
      if (!objectInfo1.Empty)
        this.cbTemplate.Items.Add((object) new MyElement((object) StatisticsConst.MultilevelHorizontalTemplateGuid, objectInfo1.Caption, (object) null));
      QuickObjectInfo objectInfo2 = sessionKeeper.Session.GetObjectInfo(StatisticsConst.HorizontalA4TemplateGuid);
      if (!objectInfo2.Empty)
        this.cbTemplate.Items.Add((object) new MyElement((object) StatisticsConst.HorizontalA4TemplateGuid, objectInfo2.Caption, (object) null));
      objectInfo2 = sessionKeeper.Session.GetObjectInfo(StatisticsConst.VerticalA4TemplateGuid);
      if (!objectInfo2.Empty)
        this.cbTemplate.Items.Add((object) new MyElement((object) StatisticsConst.VerticalA4TemplateGuid, objectInfo2.Caption, (object) null));
      objectInfo2 = sessionKeeper.Session.GetObjectInfo(StatisticsConst.VerticalA3TemplateGuid);
      if (!objectInfo2.Empty)
        this.cbTemplate.Items.Add((object) new MyElement((object) StatisticsConst.VerticalA3TemplateGuid, objectInfo2.Caption, (object) null));
      objectInfo2 = sessionKeeper.Session.GetObjectInfo(StatisticsConst.HorizontalA3TemplateGuid);
      if (!objectInfo2.Empty)
        this.cbTemplate.Items.Add((object) new MyElement((object) StatisticsConst.HorizontalA3TemplateGuid, objectInfo2.Caption, (object) null));
      this.cbTemplate.SelectedIndex = 0;
    }
  }

  private void cbTemplate_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (!(this.cbTemplate.SelectedItem is MyElement selectedItem) || !(selectedItem.Value is Guid guid))
      return;
    if (guid == StatisticsConst.MultilevelHorizontalTemplateGuid)
      this.pnlColumnNumber.Visible = true;
    else
      this.pnlColumnNumber.Visible = false;
  }

  private void ReportParametersControl_Load(object sender, EventArgs e)
  {
    if (!(ServicesManager.GetService(typeof (IConfigurationManager)) is IConfigurationManager service))
      return;
    IConfiguration configuration1 = service.Open("FormStorage");
    if (configuration1 == null)
      return;
    string name = $"{(object) this.GetType()}_{this.Name}";
    IConfiguration configuration2 = configuration1.Open(name);
    if (configuration2 == null)
      return;
    if (configuration2.HasProperty("templateGuid"))
    {
      Guid guid = new Guid(configuration2.GetProperty("templateGuid"));
      for (int index = 0; index < this.cbTemplate.Items.Count; ++index)
      {
        if (guid == (Guid) ((MyElement) this.cbTemplate.Items[index]).Value)
        {
          this.cbTemplate.SelectedIndex = index;
          break;
        }
      }
      if (configuration2.HasProperty("columnNumber") && guid == StatisticsConst.MultilevelHorizontalTemplateGuid)
        this.numericUpDown1.Text = configuration2.GetProperty("columnNumber");
    }
    if (configuration2.HasProperty("reportCreatingDateFormat"))
    {
      string property = configuration2.GetProperty("reportCreatingDateFormat");
      for (int index = 0; index < this.cbReportDateFormat.Items.Count; ++index)
      {
        if (((int) ((MyElement) this.cbReportDateFormat.Items[index]).Value).ToString() == property)
        {
          this.cbReportDateFormat.SelectedIndex = index;
          break;
        }
      }
    }
    if (configuration2.HasProperty("dateFormat"))
    {
      string property = configuration2.GetProperty("dateFormat");
      for (int index = 0; index < this.cbDateFormat.Items.Count; ++index)
      {
        if (((int) ((MyElement) this.cbDateFormat.Items[index]).Value).ToString() == property)
        {
          this.cbDateFormat.SelectedIndex = index;
          break;
        }
      }
    }
    if (!configuration2.HasProperty("showOnlyIntervalStartDate"))
      return;
    if (Convert.ToBoolean(configuration2.GetProperty("showOnlyIntervalStartDate")))
      this.cbShowOnlyIntervalStartDate.Checked = true;
    else
      this.cbShowOnlyIntervalStartDate.Checked = false;
  }

  public void On_Panel_Closing(object sender, EventArgs e)
  {
    if (!(ServicesManager.GetService(typeof (IConfigurationManager)) is IConfigurationManager service))
      return;
    IConfiguration configuration1 = service.Open("FormStorage") ?? service.Create("FormStorage");
    string name = $"{(object) this.GetType()}_{this.Name}";
    IConfiguration configuration2 = configuration1.Open(name) ?? configuration1.Add(name);
    Guid guid = (Guid) ((MyElement) this.cbTemplate.SelectedItem).Value;
    configuration2.SetProperty("templateGuid", guid.ToString());
    if (guid == StatisticsConst.MultilevelHorizontalTemplateGuid)
      configuration2.SetProperty("columnNumber", this.numericUpDown1.Text);
    configuration2.SetProperty("reportCreatingDateFormat", ((int) ((MyElement) this.cbReportDateFormat.SelectedItem).Value).ToString());
    configuration2.SetProperty("dateFormat", ((int) ((MyElement) this.cbDateFormat.SelectedItem).Value).ToString());
    configuration2.SetProperty("showOnlyIntervalStartDate", this.cbShowOnlyIntervalStartDate.Checked.ToString());
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.panel3 = new Panel();
    this.cbTemplate = new ComboBox();
    this.label2 = new Label();
    this.tbReportCaption = new TextBox();
    this.label3 = new Label();
    this.tbReportName = new TextBox();
    this.label1 = new Label();
    this.label4 = new Label();
    this.label5 = new Label();
    this.cbReportDateFormat = new ComboBox();
    this.cbDateFormat = new ComboBox();
    this.label6 = new Label();
    this.numericUpDown1 = new NumericUpDown();
    this.pnlColumnNumber = new Panel();
    this.cbShowOnlyIntervalStartDate = new CheckBox();
    this.panel3.SuspendLayout();
    this.numericUpDown1.BeginInit();
    this.pnlColumnNumber.SuspendLayout();
    this.SuspendLayout();
    this.panel3.Controls.Add((Control) this.cbTemplate);
    this.panel3.Controls.Add((Control) this.label2);
    this.panel3.Controls.Add((Control) this.tbReportCaption);
    this.panel3.Controls.Add((Control) this.label3);
    this.panel3.Controls.Add((Control) this.tbReportName);
    this.panel3.Controls.Add((Control) this.label1);
    this.panel3.Dock = DockStyle.Top;
    this.panel3.Location = new Point(0, 0);
    this.panel3.Name = "panel3";
    this.panel3.Size = new Size(707, 85);
    this.panel3.TabIndex = 6;
    this.cbTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.cbTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbTemplate.FormattingEnabled = true;
    this.cbTemplate.Location = new Point(148, 58);
    this.cbTemplate.Name = "cbTemplate";
    this.cbTemplate.Size = new Size(544, 21);
    this.cbTemplate.TabIndex = 9;
    this.cbTemplate.SelectedIndexChanged += new EventHandler(this.cbTemplate_SelectedIndexChanged);
    this.label2.AutoSize = true;
    this.label2.ImeMode = ImeMode.NoControl;
    this.label2.Location = new Point(5, 58);
    this.label2.Name = "label2";
    this.label2.Size = new Size(85, 13);
    this.label2.TabIndex = 8;
    this.label2.Text = "Шаблон отчёта:";
    this.tbReportCaption.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbReportCaption.Location = new Point(148, 30);
    this.tbReportCaption.Name = "tbReportCaption";
    this.tbReportCaption.Size = new Size(544, 20);
    this.tbReportCaption.TabIndex = 1;
    this.label3.AutoSize = true;
    this.label3.ImeMode = ImeMode.NoControl;
    this.label3.Location = new Point(5, 33);
    this.label3.Name = "label3";
    this.label3.Size = new Size(100, 13);
    this.label3.TabIndex = 2;
    this.label3.Text = "Заголовок отчёта:";
    this.tbReportName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbReportName.Location = new Point(148, 5);
    this.tbReportName.Name = "tbReportName";
    this.tbReportName.Size = new Size(544, 20);
    this.tbReportName.TabIndex = 0;
    this.label1.AutoSize = true;
    this.label1.ImeMode = ImeMode.NoControl;
    this.label1.Location = new Point(5, 8);
    this.label1.Name = "label1";
    this.label1.Size = new Size(122, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Наименование отчёта:";
    this.label4.AutoSize = true;
    this.label4.ImeMode = ImeMode.NoControl;
    this.label4.Location = new Point(3, 100);
    this.label4.Name = "label4";
    this.label4.Size = new Size(142, 13);
    this.label4.TabIndex = 8;
    this.label4.Text = "Формат отображения дат:";
    this.label5.AutoSize = true;
    this.label5.ImeMode = ImeMode.NoControl;
    this.label5.Location = new Point(3, 176 /*0xB0*/);
    this.label5.Name = "label5";
    this.label5.Size = new Size(151, 13);
    this.label5.TabIndex = 10;
    this.label5.Text = "Дата формирования отчёта:";
    this.cbReportDateFormat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.cbReportDateFormat.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbReportDateFormat.FormattingEnabled = true;
    this.cbReportDateFormat.Location = new Point(6, 192 /*0xC0*/);
    this.cbReportDateFormat.Name = "cbReportDateFormat";
    this.cbReportDateFormat.Size = new Size(684, 21);
    this.cbReportDateFormat.TabIndex = 12;
    this.cbDateFormat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.cbDateFormat.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbDateFormat.FormattingEnabled = true;
    this.cbDateFormat.Location = new Point(6, 116);
    this.cbDateFormat.Name = "cbDateFormat";
    this.cbDateFormat.Size = new Size(684, 21);
    this.cbDateFormat.TabIndex = 13;
    this.label6.AutoSize = true;
    this.label6.Location = new Point(3, 9);
    this.label6.Name = "label6";
    this.label6.Size = new Size(242, 13);
    this.label6.TabIndex = 14;
    this.label6.Text = "Количество столбцов с данными на странице:";
    this.numericUpDown1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.numericUpDown1.Location = new Point(250, 7);
    this.numericUpDown1.Minimum = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this.numericUpDown1.Name = "numericUpDown1";
    this.numericUpDown1.Size = new Size(437, 20);
    this.numericUpDown1.TabIndex = 16 /*0x10*/;
    this.numericUpDown1.Value = new Decimal(new int[4]
    {
      10,
      0,
      0,
      0
    });
    this.pnlColumnNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.pnlColumnNumber.Controls.Add((Control) this.label6);
    this.pnlColumnNumber.Controls.Add((Control) this.numericUpDown1);
    this.pnlColumnNumber.Location = new Point(3, 219);
    this.pnlColumnNumber.Name = "pnlColumnNumber";
    this.pnlColumnNumber.Size = new Size(704, 36);
    this.pnlColumnNumber.TabIndex = 17;
    this.cbShowOnlyIntervalStartDate.AutoSize = true;
    this.cbShowOnlyIntervalStartDate.Checked = true;
    this.cbShowOnlyIntervalStartDate.CheckState = CheckState.Checked;
    this.cbShowOnlyIntervalStartDate.Location = new Point(6, 143);
    this.cbShowOnlyIntervalStartDate.Name = "cbShowOnlyIntervalStartDate";
    this.cbShowOnlyIntervalStartDate.Size = new Size(288, 17);
    this.cbShowOnlyIntervalStartDate.TabIndex = 18;
    this.cbShowOnlyIntervalStartDate.Text = "Показывать только даты начала интервалов сбора";
    this.cbShowOnlyIntervalStartDate.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.cbShowOnlyIntervalStartDate);
    this.Controls.Add((Control) this.pnlColumnNumber);
    this.Controls.Add((Control) this.cbDateFormat);
    this.Controls.Add((Control) this.label5);
    this.Controls.Add((Control) this.cbReportDateFormat);
    this.Controls.Add((Control) this.label4);
    this.Controls.Add((Control) this.panel3);
    this.Name = nameof (ReportParametersControl);
    this.Size = new Size(707, 376);
    this.Load += new EventHandler(this.ReportParametersControl_Load);
    this.panel3.ResumeLayout(false);
    this.panel3.PerformLayout();
    this.numericUpDown1.EndInit();
    this.pnlColumnNumber.ResumeLayout(false);
    this.pnlColumnNumber.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
