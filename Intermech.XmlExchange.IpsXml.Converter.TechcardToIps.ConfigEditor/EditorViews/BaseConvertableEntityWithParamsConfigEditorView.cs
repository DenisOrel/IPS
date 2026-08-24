// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.BaseConvertableEntityWithParamsConfigEditorView
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

public class BaseConvertableEntityWithParamsConfigEditorView : UserControl
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private TextBox txbDescr;
  private Label lblDescr;
  private TextBox txbId;
  private Label lblId;

  public BaseConvertableEntityWithParamsConfigEditorView() => this.InitializeComponent();

  public BaseConvertableEntityWithParamsConfig TargetConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public bool ApplyChanges()
  {
    if (this.TargetConfig == null)
      return false;
    TechcardToIpsConfig service = this.GlobalServices.GetService<TechcardToIpsConfig>();
    if (service.ObjectConfigs.Contains(this.txbId.Text) && service.ObjectConfigs[this.txbId.Text] != this.TargetConfig)
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("msgDublicateId"), LocalizationHolder.rm.GetString("cptAppName"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }
    this.TargetConfig.Id = this.txbId.Text;
    this.TargetConfig.Description = this.txbDescr.Text;
    return true;
  }

  private void ApplyConfigToControl(BaseConvertableEntityWithParamsConfig config)
  {
    this.SuspendLayout();
    this.txbId.Text = config.Id;
    this.txbDescr.Text = config.Description;
    this.ResumeLayout();
  }

  public event EventHandler<bool> OnDataChanged;

  private void txb_TextChanged(object sender, EventArgs e)
  {
    EventHandler<bool> onDataChanged = this.OnDataChanged;
    if (onDataChanged == null)
      return;
    onDataChanged(sender, this.IsDataChanged());
  }

  private bool IsDataChanged()
  {
    return this.txbId.Text != this.TargetConfig.Id || this.txbDescr.Text != this.TargetConfig.Description;
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
    this.txbDescr = new TextBox();
    this.lblDescr = new Label();
    this.txbId = new TextBox();
    this.lblId = new Label();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.txbDescr, 0, 3);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblDescr, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbId, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblId, 0, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 4;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.Size = new Size(356, 88);
    this.tableLayoutPanel1.TabIndex = 1;
    this.txbDescr.Dock = DockStyle.Fill;
    this.txbDescr.Location = new Point(3, 55);
    this.txbDescr.Name = "txbDescr";
    this.txbDescr.Size = new Size(350, 20);
    this.txbDescr.TabIndex = 5;
    this.txbDescr.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblDescr.AutoSize = true;
    this.lblDescr.Dock = DockStyle.Fill;
    this.lblDescr.Location = new Point(3, 39);
    this.lblDescr.Name = "lblDescr";
    this.lblDescr.Size = new Size(350, 13);
    this.lblDescr.TabIndex = 4;
    this.lblDescr.Text = "Описание:";
    this.txbId.Dock = DockStyle.Fill;
    this.txbId.Location = new Point(3, 16 /*0x10*/);
    this.txbId.Name = "txbId";
    this.txbId.Size = new Size(350, 20);
    this.txbId.TabIndex = 0;
    this.txbId.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblId.AutoSize = true;
    this.lblId.Dock = DockStyle.Fill;
    this.lblId.Location = new Point(3, 0);
    this.lblId.Name = "lblId";
    this.lblId.Size = new Size(350, 13);
    this.lblId.TabIndex = 1;
    this.lblId.Text = "Идентификатор:";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = "ObjectConfigEditorView";
    this.Size = new Size(356, 88);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
