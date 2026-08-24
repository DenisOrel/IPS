// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.ConstValueConfigEditorView
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews;

public class ConstValueConfigEditorView : UserControl
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private Label lblConstName;
  private Label lblConstValue;
  private TextBox txbConstValue;
  private TextBox txbConstName;

  public ConstValueConfigEditorView() => this.InitializeComponent();

  public ConstValueConfig TargetConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public bool ApplyChanges()
  {
    if (this.TargetConfig == null)
      return false;
    TechcardToIpsConfig service = this.GlobalServices.GetService<TechcardToIpsConfig>();
    if (service.ConstValueConfigs.Contains(this.txbConstName.Text) && service.ConstValueConfigs[this.txbConstName.Text] != this.TargetConfig)
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("msgDublicateId"), LocalizationHolder.rm.GetString("cptAppName"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }
    this.TargetConfig.Name = this.txbConstName.Text;
    this.TargetConfig.Value = this.txbConstValue.Text;
    if (this.TargetConfig.Id != this.TargetConfig.Name)
    {
      service.ConstValueConfigs.Remove(this.TargetConfig.Id);
      this.TargetConfig.Id = this.TargetConfig.Name;
      service.ConstValueConfigs[this.TargetConfig.Id] = this.TargetConfig;
    }
    return true;
  }

  private void ApplyConfigToControl(ConstValueConfig config)
  {
    this.SuspendLayout();
    this.txbConstName.Text = config.Id;
    this.txbConstValue.Text = config.Value;
    this.ResumeLayout();
  }

  public event EventHandler<bool> OnDataChanged;

  private void txb_TextChanged(object sender, EventArgs e)
  {
    EventHandler<bool> onDataChanged = this.OnDataChanged;
    if (onDataChanged == null)
      return;
    onDataChanged(sender, this.txbConstName.Text != this.TargetConfig?.Id || this.txbConstValue.Text != this.TargetConfig?.Value);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.txbConstValue = new TextBox();
    this.lblConstName = new Label();
    this.lblConstValue = new Label();
    this.txbConstName = new TextBox();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.txbConstValue, 0, 3);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblConstName, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblConstValue, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbConstName, 0, 1);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 4;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Size = new Size(522, 121);
    this.tableLayoutPanel1.TabIndex = 0;
    this.txbConstValue.Dock = DockStyle.Fill;
    this.txbConstValue.Location = new Point(3, 55);
    this.txbConstValue.Name = "txbConstValue";
    this.txbConstValue.Size = new Size(516, 20);
    this.txbConstValue.TabIndex = 3;
    this.txbConstValue.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblConstName.AutoSize = true;
    this.lblConstName.Dock = DockStyle.Fill;
    this.lblConstName.Location = new Point(3, 0);
    this.lblConstName.Name = "lblConstName";
    this.lblConstName.Size = new Size(516, 13);
    this.lblConstName.TabIndex = 0;
    this.lblConstName.Text = "Наименование:";
    this.lblConstValue.AutoSize = true;
    this.lblConstValue.Dock = DockStyle.Fill;
    this.lblConstValue.Location = new Point(3, 39);
    this.lblConstValue.Name = "lblConstValue";
    this.lblConstValue.Size = new Size(516, 13);
    this.lblConstValue.TabIndex = 1;
    this.lblConstValue.Text = "Значение:";
    this.txbConstName.Dock = DockStyle.Fill;
    this.txbConstName.Location = new Point(3, 16 /*0x10*/);
    this.txbConstName.Name = "txbConstName";
    this.txbConstName.Size = new Size(516, 20);
    this.txbConstName.TabIndex = 2;
    this.txbConstName.TextChanged += new EventHandler(this.txb_TextChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (ConstValueConfigEditorView);
    this.Size = new Size(522, 121);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
