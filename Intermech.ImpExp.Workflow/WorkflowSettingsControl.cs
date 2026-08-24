// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.WorkflowSettingsControl
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Workflow.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace Intermech.ImpExp.Workflow;

public class WorkflowSettingsControl : StepControl
{
  private string _termCaption = "";
  private string _bigCaption = "";
  private string _complCaption = "";
  private Dictionary<int, int> BigProcesses;
  private bool _settingsLoaded;
  private Image _image;
  private IContainer components;
  private GroupBox groupBox1;
  private Timer DataTimer;
  private CheckBox pumpProcessesCheckBox;
  private CheckBox pumpSchemesCheckBox;
  private GroupBox processGroupBox;
  private CheckBox checkBox3;
  private Panel panel2;
  private DateTimePicker PumpDTPicker2;
  private Label label4;
  private Label label3;
  private DateTimePicker PumpDTPicker1;
  private CheckBox PumpDTCheckBox;
  private AutoSizeLabel PumpBigNotice;
  private CheckBox pumpBigCheckBox;
  private AutoSizeLabel PumpTerminatedNotice;
  private CheckBox pumpTerminatedCheckBox;
  private AutoSizeLabel autoSizeLabel1;
  private CheckBox pumpCompletedCheckBox;

  public WorkflowSettingsControl()
  {
    this.InitializeComponent();
    this._termCaption = this.pumpTerminatedCheckBox.Text;
    this._bigCaption = this.pumpBigCheckBox.Text;
    this._complCaption = this.pumpCompletedCheckBox.Text;
    this.pumpTerminatedCheckBox.Text = string.Format(this._termCaption, (object) "...");
    this.pumpBigCheckBox.Text = string.Format(this._bigCaption, (object) "...");
    this.pumpCompletedCheckBox.Text = string.Format(this._complCaption, (object) "...");
    this.PumpBigNotice.Text = string.Format(this.PumpBigNotice.Text, (object) PumpWorkflowSettings.BigSchemeActivitiesCount);
  }

  public override bool isMetadataSettingsStep => false;

  public override void RefreshControl()
  {
    base.RefreshControl();
    if (this.BigProcesses != null)
      return;
    this.DataTimer.Enabled = true;
  }

  private void DataTimer_Tick(object sender, EventArgs e)
  {
    this.DataTimer.Enabled = false;
    GC.Collect();
    this.pumpCompletedCheckBox.Text = string.Format(this._complCaption, (object) BasePumpHelper.S4IntQuery("select count(*) from workflowschemestable where status = 6"));
    this.pumpCompletedCheckBox.Refresh();
    this.pumpTerminatedCheckBox.Text = string.Format(this._termCaption, (object) BasePumpHelper.S4IntQuery("select count(*) from workflowschemestable where status = 5"));
    this.pumpTerminatedCheckBox.Refresh();
    this.BigProcesses = new Dictionary<int, int>();
    using (IDataReader dataReader = BasePumpHelper.S4Query($"select schemeid, count(activityid) from activitiestable group by schemeid having count(activityid) > {PumpWorkflowSettings.BigSchemeActivitiesCount}"))
    {
      while (dataReader.Read())
        this.BigProcesses.Add(BasePumpHelper.ToInt32(dataReader[0]), BasePumpHelper.ToInt32(dataReader[1]));
    }
    this.pumpBigCheckBox.Text = string.Format(this._bigCaption, (object) this.BigProcesses.Count);
  }

  private void LoadSettings()
  {
    if (this._settingsLoaded)
      return;
    this._settingsLoaded = true;
    this.pumpSchemesCheckBox.Checked = PumpWorkflowSettings.HasOption(WFOptions.PumpSchemes);
    this.pumpProcessesCheckBox.Checked = PumpWorkflowSettings.HasOption(WFOptions.PumpProcesses);
    this.pumpTerminatedCheckBox.Checked = PumpWorkflowSettings.HasOption(WFOptions.PumpTerminated);
    this.pumpCompletedCheckBox.Checked = PumpWorkflowSettings.HasOption(WFOptions.PumpCompleted);
    this.pumpBigCheckBox.Checked = PumpWorkflowSettings.HasOption(WFOptions.PumpBig);
    this.PumpDTCheckBox.Checked = PumpWorkflowSettings.HasOption(WFOptions.PumpByDateTime);
    if (this.PumpDTCheckBox.Checked)
    {
      this.PumpDTPicker1.Value = PumpWorkflowSettings.StartDT;
      this.PumpDTPicker2.Value = PumpWorkflowSettings.EndDT.Date.AddDays(-1.0);
    }
    this.pumpProcessesCheckBox_CheckedChanged((object) null, (EventArgs) null);
  }

  public override SaveSettingsResult SaveSettings()
  {
    PumpWorkflowSettings.Options = (WFOptions) 0;
    if (this.pumpSchemesCheckBox.Checked)
      PumpWorkflowSettings.Options |= WFOptions.PumpSchemes;
    if (this.pumpProcessesCheckBox.Checked)
      PumpWorkflowSettings.Options |= WFOptions.PumpProcesses;
    if (this.pumpTerminatedCheckBox.Checked)
      PumpWorkflowSettings.Options |= WFOptions.PumpTerminated;
    if (this.pumpCompletedCheckBox.Checked)
      PumpWorkflowSettings.Options |= WFOptions.PumpCompleted;
    if (this.pumpBigCheckBox.Checked)
      PumpWorkflowSettings.Options |= WFOptions.PumpBig;
    if (this.PumpDTCheckBox.Checked)
    {
      PumpWorkflowSettings.Options |= WFOptions.PumpByDateTime;
      PumpWorkflowSettings.StartDT = this.PumpDTPicker1.Value;
      PumpWorkflowSettings.EndDT = this.PumpDTPicker2.Value;
      PumpWorkflowSettings.EndDT = PumpWorkflowSettings.EndDT.Date.AddDays(1.0);
    }
    CacheCategory cacheCategory = PumpCache.Category[ImportingCategory.ProcessesToSkip];
    cacheCategory.Clear();
    foreach (KeyValuePair<int, int> bigProcess in this.BigProcesses)
    {
      if (cacheCategory.GetNewKey((object) bigProcess.Key) == 0L)
        cacheCategory.AddValue((object) bigProcess.Key, (long) bigProcess.Value);
    }
    (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).SetSettings("WFSETTINGS", new Dictionary<string, SaveSettingsAttribute[]>()
    {
      {
        "Common",
        new List<SaveSettingsAttribute>()
        {
          new SaveSettingsAttribute("Options", PumpWorkflowSettings.ToString()),
          new SaveSettingsAttribute("StartDT", PumpWorkflowSettings.StartDT.ToBinary().ToString()),
          new SaveSettingsAttribute("EndDT", PumpWorkflowSettings.EndDT.ToBinary().ToString())
        }.ToArray()
      }
    });
    return base.SaveSettings();
  }

  private void PumpDTCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    this.PumpDTPicker1.Enabled = this.PumpDTCheckBox.Checked;
    this.PumpDTPicker2.Enabled = this.PumpDTCheckBox.Checked;
  }

  protected override string getCaption() => "Настройки перекачки данных маршрутизатора";

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgRouterParams")];
    return this._image;
  }

  private void WorkflowSettingsControl_VisibleChanged(object sender, EventArgs e)
  {
    if (PumpHelper.Plugin == null)
      return;
    this.LoadSettings();
  }

  public static void SetControlsReadOnly(Control parent, bool _ro)
  {
    WorkflowSettingsControl.SetControlsReadOnly(parent, _ro, (List<Control>) null);
  }

  public static void SetControlsReadOnly(Control parent, bool _ro, List<Control> ExcludeControls)
  {
    Form form = (Form) null;
    foreach (Control control in (ArrangedElementCollection) parent.Controls)
    {
      if (ExcludeControls == null || ExcludeControls.IndexOf(control) == -1)
      {
        switch (control)
        {
          case TextBox _:
            (control as TextBox).ReadOnly = _ro;
            goto label_10;
          case Button _:
            if (form == null)
              form = control.FindForm();
            if (form != null && form.AcceptButton != control && form.CancelButton != control)
            {
              control.Enabled = !_ro;
              goto label_10;
            }
            goto label_10;
          case GroupBox _:
          case Panel _:
          case Form _:
          case TabPage _:
          case TabControl _:
          case Label _:
label_10:
            if (control.HasChildren)
            {
              WorkflowSettingsControl.SetControlsReadOnly(control, _ro, ExcludeControls);
              continue;
            }
            continue;
          default:
            control.Enabled = !_ro;
            goto label_10;
        }
      }
    }
  }

  private void pumpProcessesCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    WorkflowSettingsControl.SetControlsReadOnly((Control) this.processGroupBox, !this.pumpProcessesCheckBox.Checked);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (WorkflowSettingsControl));
    this.groupBox1 = new GroupBox();
    this.processGroupBox = new GroupBox();
    this.checkBox3 = new CheckBox();
    this.panel2 = new Panel();
    this.PumpDTPicker2 = new DateTimePicker();
    this.label4 = new Label();
    this.label3 = new Label();
    this.PumpDTPicker1 = new DateTimePicker();
    this.PumpDTCheckBox = new CheckBox();
    this.pumpBigCheckBox = new CheckBox();
    this.pumpTerminatedCheckBox = new CheckBox();
    this.pumpCompletedCheckBox = new CheckBox();
    this.pumpProcessesCheckBox = new CheckBox();
    this.pumpSchemesCheckBox = new CheckBox();
    this.DataTimer = new Timer(this.components);
    this.PumpBigNotice = new AutoSizeLabel();
    this.PumpTerminatedNotice = new AutoSizeLabel();
    this.autoSizeLabel1 = new AutoSizeLabel();
    this.groupBox1.SuspendLayout();
    this.processGroupBox.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.groupBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.groupBox1.Controls.Add((Control) this.processGroupBox);
    this.groupBox1.Controls.Add((Control) this.pumpProcessesCheckBox);
    this.groupBox1.Controls.Add((Control) this.pumpSchemesCheckBox);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Padding = new Padding(20, 15, 10, 10);
    this.groupBox1.Size = new Size(613, 465);
    this.groupBox1.TabIndex = 3;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Настройки перекачки данных маршрутизатора";
    this.processGroupBox.Controls.Add((Control) this.checkBox3);
    this.processGroupBox.Controls.Add((Control) this.panel2);
    this.processGroupBox.Controls.Add((Control) this.PumpDTCheckBox);
    this.processGroupBox.Controls.Add((Control) this.PumpBigNotice);
    this.processGroupBox.Controls.Add((Control) this.pumpBigCheckBox);
    this.processGroupBox.Controls.Add((Control) this.PumpTerminatedNotice);
    this.processGroupBox.Controls.Add((Control) this.pumpTerminatedCheckBox);
    this.processGroupBox.Controls.Add((Control) this.autoSizeLabel1);
    this.processGroupBox.Controls.Add((Control) this.pumpCompletedCheckBox);
    this.processGroupBox.Dock = DockStyle.Fill;
    this.processGroupBox.Location = new Point(20, 72);
    this.processGroupBox.Name = "processGroupBox";
    this.processGroupBox.Padding = new Padding(10);
    this.processGroupBox.Size = new Size(583, 383);
    this.processGroupBox.TabIndex = 2;
    this.processGroupBox.TabStop = false;
    this.processGroupBox.Text = "Перекачка процессов";
    this.checkBox3.AutoSize = true;
    this.checkBox3.Dock = DockStyle.Top;
    this.checkBox3.Location = new Point(10, 225);
    this.checkBox3.Name = "checkBox3";
    this.checkBox3.Size = new Size(563, 17);
    this.checkBox3.TabIndex = 31 /*0x1F*/;
    this.checkBox3.Text = "Перекачивать давно выполняющиеся процессы";
    this.checkBox3.UseVisualStyleBackColor = true;
    this.checkBox3.Visible = false;
    this.panel2.AutoSize = true;
    this.panel2.Controls.Add((Control) this.PumpDTPicker2);
    this.panel2.Controls.Add((Control) this.label4);
    this.panel2.Controls.Add((Control) this.label3);
    this.panel2.Controls.Add((Control) this.PumpDTPicker1);
    this.panel2.Dock = DockStyle.Top;
    this.panel2.Location = new Point(10, 166);
    this.panel2.Name = "panel2";
    this.panel2.Padding = new Padding(0, 0, 0, 30);
    this.panel2.Size = new Size(563, 59);
    this.panel2.TabIndex = 30;
    this.PumpDTPicker2.Enabled = false;
    this.PumpDTPicker2.Location = new Point(225, 6);
    this.PumpDTPicker2.Name = "PumpDTPicker2";
    this.PumpDTPicker2.Size = new Size(155, 20);
    this.PumpDTPicker2.TabIndex = 15;
    this.label4.AutoSize = true;
    this.label4.Location = new Point(197, 10);
    this.label4.Name = "label4";
    this.label4.Size = new Size(22, 13);
    this.label4.TabIndex = 14;
    this.label4.Text = "по:";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(6, 10);
    this.label3.Name = "label3";
    this.label3.Size = new Size(17, 13);
    this.label3.TabIndex = 13;
    this.label3.Text = "С:";
    this.PumpDTPicker1.Enabled = false;
    this.PumpDTPicker1.Location = new Point(26, 6);
    this.PumpDTPicker1.Name = "PumpDTPicker1";
    this.PumpDTPicker1.Size = new Size(155, 20);
    this.PumpDTPicker1.TabIndex = 12;
    this.PumpDTPicker1.Value = new DateTime(2004, 1, 1, 0, 0, 0, 0);
    this.PumpDTCheckBox.AutoSize = true;
    this.PumpDTCheckBox.Dock = DockStyle.Top;
    this.PumpDTCheckBox.Location = new Point(10, 149);
    this.PumpDTCheckBox.Name = "PumpDTCheckBox";
    this.PumpDTCheckBox.Size = new Size(563, 17);
    this.PumpDTCheckBox.TabIndex = 29;
    this.PumpDTCheckBox.Text = "Перекачивать только процессы, стартованные в период:";
    this.PumpDTCheckBox.UseVisualStyleBackColor = true;
    this.pumpBigCheckBox.AutoSize = true;
    this.pumpBigCheckBox.Dock = DockStyle.Top;
    this.pumpBigCheckBox.ForeColor = Color.Black;
    this.pumpBigCheckBox.Location = new Point(10, 107);
    this.pumpBigCheckBox.Name = "pumpBigCheckBox";
    this.pumpBigCheckBox.Size = new Size(563, 17);
    this.pumpBigCheckBox.TabIndex = 26;
    this.pumpBigCheckBox.Text = "Перекачивать большие процессы ({0})";
    this.pumpBigCheckBox.UseVisualStyleBackColor = true;
    this.pumpTerminatedCheckBox.AutoSize = true;
    this.pumpTerminatedCheckBox.Dock = DockStyle.Top;
    this.pumpTerminatedCheckBox.Location = new Point(10, 65);
    this.pumpTerminatedCheckBox.Name = "pumpTerminatedCheckBox";
    this.pumpTerminatedCheckBox.Size = new Size(563, 17);
    this.pumpTerminatedCheckBox.TabIndex = 25;
    this.pumpTerminatedCheckBox.Text = "Перекачивать прерванные процессы ({0})";
    this.pumpTerminatedCheckBox.UseVisualStyleBackColor = true;
    this.pumpCompletedCheckBox.AutoSize = true;
    this.pumpCompletedCheckBox.Dock = DockStyle.Top;
    this.pumpCompletedCheckBox.Location = new Point(10, 23);
    this.pumpCompletedCheckBox.Name = "pumpCompletedCheckBox";
    this.pumpCompletedCheckBox.Size = new Size(563, 17);
    this.pumpCompletedCheckBox.TabIndex = 32 /*0x20*/;
    this.pumpCompletedCheckBox.Text = "Перекачивать выполненные процессы ({0})";
    this.pumpCompletedCheckBox.UseVisualStyleBackColor = true;
    this.pumpProcessesCheckBox.AutoSize = true;
    this.pumpProcessesCheckBox.Dock = DockStyle.Top;
    this.pumpProcessesCheckBox.Location = new Point(20, 50);
    this.pumpProcessesCheckBox.Name = "pumpProcessesCheckBox";
    this.pumpProcessesCheckBox.Padding = new Padding(0, 0, 0, 5);
    this.pumpProcessesCheckBox.Size = new Size(583, 22);
    this.pumpProcessesCheckBox.TabIndex = 1;
    this.pumpProcessesCheckBox.Text = "Перекачивать процессы";
    this.pumpProcessesCheckBox.UseVisualStyleBackColor = true;
    this.pumpProcessesCheckBox.CheckedChanged += new EventHandler(this.pumpProcessesCheckBox_CheckedChanged);
    this.pumpSchemesCheckBox.AutoSize = true;
    this.pumpSchemesCheckBox.Dock = DockStyle.Top;
    this.pumpSchemesCheckBox.Location = new Point(20, 28);
    this.pumpSchemesCheckBox.Name = "pumpSchemesCheckBox";
    this.pumpSchemesCheckBox.Padding = new Padding(0, 0, 0, 5);
    this.pumpSchemesCheckBox.Size = new Size(583, 22);
    this.pumpSchemesCheckBox.TabIndex = 0;
    this.pumpSchemesCheckBox.Text = "Перекачивать шаблоны процессов";
    this.pumpSchemesCheckBox.UseVisualStyleBackColor = true;
    this.DataTimer.Tick += new EventHandler(this.DataTimer_Tick);
    this.PumpBigNotice.Dock = DockStyle.Top;
    this.PumpBigNotice.Location = new Point(10, 124);
    this.PumpBigNotice.Name = "PumpBigNotice";
    this.PumpBigNotice.Padding = new Padding(0, 5, 0, 20);
    this.PumpBigNotice.Size = new Size(563, 25);
    this.PumpBigNotice.TabIndex = 28;
    this.PumpBigNotice.Text = componentResourceManager.GetString("PumpBigNotice.Text");
    this.PumpTerminatedNotice.Dock = DockStyle.Top;
    this.PumpTerminatedNotice.Location = new Point(10, 82);
    this.PumpTerminatedNotice.Name = "PumpTerminatedNotice";
    this.PumpTerminatedNotice.Padding = new Padding(0, 5, 0, 20);
    this.PumpTerminatedNotice.Size = new Size(563, 25);
    this.PumpTerminatedNotice.TabIndex = 27;
    this.PumpTerminatedNotice.Text = "Настройка определяет, нужно ли перекачивать процессы, которые были прерваны";
    this.autoSizeLabel1.Dock = DockStyle.Top;
    this.autoSizeLabel1.Location = new Point(10, 40);
    this.autoSizeLabel1.Name = "autoSizeLabel1";
    this.autoSizeLabel1.Padding = new Padding(0, 5, 0, 20);
    this.autoSizeLabel1.Size = new Size(563, 25);
    this.autoSizeLabel1.TabIndex = 33;
    this.autoSizeLabel1.Text = "Настройка определяет, нужно ли перекачивать процессы, выполнение которых закончено";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.Controls.Add((Control) this.groupBox1);
    this.Name = nameof (WorkflowSettingsControl);
    this.Size = new Size(613, 465);
    this.VisibleChanged += new EventHandler(this.WorkflowSettingsControl_VisibleChanged);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.processGroupBox.ResumeLayout(false);
    this.processGroupBox.PerformLayout();
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.ResumeLayout(false);
  }
}
