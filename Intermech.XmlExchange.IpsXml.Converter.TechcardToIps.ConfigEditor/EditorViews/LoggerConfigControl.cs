// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.LoggerConfigControl
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews;

public class LoggerConfigControl : UserControl
{
  private IContainer components;
  private CheckBox cbxInfos;
  private CheckBox cbxWarns;
  private CheckBox cbxErrors;

  public LoggerConfigControl() => this.InitializeComponent();

  public LoggerConfig TargetConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public void ApplyChanges()
  {
    if (this.TargetConfig == null)
      return;
    this.TargetConfig.Infos = this.cbxInfos.Checked;
    this.TargetConfig.Warnings = this.cbxWarns.Checked;
    this.TargetConfig.Errors = this.cbxErrors.Checked;
  }

  private void ApplyConfigToControl(LoggerConfig config)
  {
    this.SuspendLayout();
    this.cbxInfos.Checked = config.Infos;
    this.cbxWarns.Checked = config.Warnings;
    this.cbxErrors.Checked = config.Errors;
    this.ResumeLayout();
  }

  public event EventHandler<bool> OnDataChanged;

  private void cbx_CheckedChanged(object sender, EventArgs e)
  {
    bool? infos = this.TargetConfig?.Infos;
    bool flag1 = this.cbxInfos.Checked;
    int num;
    if (infos.GetValueOrDefault() == flag1 & infos.HasValue)
    {
      bool? warnings = this.TargetConfig?.Warnings;
      bool flag2 = this.cbxWarns.Checked;
      if (warnings.GetValueOrDefault() == flag2 & warnings.HasValue)
      {
        bool? errors = this.TargetConfig?.Errors;
        bool flag3 = this.cbxErrors.Checked;
        num = !(errors.GetValueOrDefault() == flag3 & errors.HasValue) ? 1 : 0;
        goto label_4;
      }
    }
    num = 1;
label_4:
    bool e1 = num != 0;
    EventHandler<bool> onDataChanged = this.OnDataChanged;
    if (onDataChanged == null)
      return;
    onDataChanged(sender, e1);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.cbxInfos = new CheckBox();
    this.cbxWarns = new CheckBox();
    this.cbxErrors = new CheckBox();
    this.SuspendLayout();
    this.cbxInfos.AutoSize = true;
    this.cbxInfos.Dock = DockStyle.Top;
    this.cbxInfos.Location = new Point(0, 0);
    this.cbxInfos.Name = "cbxInfos";
    this.cbxInfos.Size = new Size(213, 17);
    this.cbxInfos.TabIndex = 0;
    this.cbxInfos.Text = "Информационные сообщения";
    this.cbxInfos.UseVisualStyleBackColor = true;
    this.cbxInfos.CheckedChanged += new EventHandler(this.cbx_CheckedChanged);
    this.cbxWarns.AutoSize = true;
    this.cbxWarns.Dock = DockStyle.Top;
    this.cbxWarns.Location = new Point(0, 17);
    this.cbxWarns.Name = "cbxWarns";
    this.cbxWarns.Size = new Size(213, 17);
    this.cbxWarns.TabIndex = 1;
    this.cbxWarns.Text = "Предупреждения";
    this.cbxWarns.UseVisualStyleBackColor = true;
    this.cbxWarns.CheckedChanged += new EventHandler(this.cbx_CheckedChanged);
    this.cbxErrors.AutoSize = true;
    this.cbxErrors.Dock = DockStyle.Top;
    this.cbxErrors.Location = new Point(0, 34);
    this.cbxErrors.Name = "cbxErrors";
    this.cbxErrors.Size = new Size(213, 17);
    this.cbxErrors.TabIndex = 2;
    this.cbxErrors.Text = "Ошибки";
    this.cbxErrors.UseVisualStyleBackColor = true;
    this.cbxErrors.CheckedChanged += new EventHandler(this.cbx_CheckedChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.cbxErrors);
    this.Controls.Add((Control) this.cbxWarns);
    this.Controls.Add((Control) this.cbxInfos);
    this.Name = nameof (LoggerConfigControl);
    this.Size = new Size(213, 53);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
