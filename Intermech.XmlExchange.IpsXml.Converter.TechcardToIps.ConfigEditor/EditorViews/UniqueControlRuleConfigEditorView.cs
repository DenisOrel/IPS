// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.UniqueControlRuleConfigEditorView
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews;

public class UniqueControlRuleConfigEditorView : UserControl
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private ComboBox cbxUniqueRuleType;
  private Label lblId;
  private TextBox txbId;
  private Label lblUniqueRuleType;

  public UniqueControlRuleConfigEditorView() => this.InitializeComponent();

  public UniqueControlRuleConfig TargetConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public bool ApplyChanges()
  {
    if (this.TargetConfig == null)
      return false;
    this.TargetConfig.Rule = this.cbxUniqueRuleType.SelectedItem != null ? (UniqueControlRule) this.cbxUniqueRuleType.SelectedItem : UniqueControlRule.NoControl;
    this.TargetConfig.Id = this.txbId.Text;
    return true;
  }

  private void ApplyConfigToControl(UniqueControlRuleConfig config)
  {
    this.SuspendLayout();
    this.txbId.Text = config.Id;
    this.cbxUniqueRuleType.BeginUpdate();
    this.cbxUniqueRuleType.Items.Clear();
    Enum.GetValues(typeof (UniqueControlRule)).Cast<UniqueControlRule>().ToList<UniqueControlRule>().ForEach((Action<UniqueControlRule>) (value => this.cbxUniqueRuleType.Items.Add((object) value)));
    this.cbxUniqueRuleType.SelectedItem = (object) config.Rule;
    this.cbxUniqueRuleType.DisplayMember = "ToXMLTag";
    this.cbxUniqueRuleType.EndUpdate();
    this.ResumeLayout();
  }

  public event EventHandler<bool> OnDataChanged;

  private void cbx_UniqueRuleTypeChanged(object sender, EventArgs e)
  {
    EventHandler<bool> onDataChanged = this.OnDataChanged;
    if (onDataChanged != null)
      onDataChanged(sender, this.txbId.Text != this.TargetConfig.Id || this.cbxUniqueRuleType.SelectedItem != null && (UniqueControlRule) this.cbxUniqueRuleType.SelectedItem != this.TargetConfig.Rule);
    this.txbId.Enabled = this.cbxUniqueRuleType.SelectedItem != null && (UniqueControlRule) this.cbxUniqueRuleType.SelectedItem == UniqueControlRule.IdControl;
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
    this.lblUniqueRuleType = new Label();
    this.cbxUniqueRuleType = new ComboBox();
    this.lblId = new Label();
    this.txbId = new TextBox();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.lblUniqueRuleType, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.cbxUniqueRuleType, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblId, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbId, 0, 3);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 4;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.Size = new Size(150, 150);
    this.tableLayoutPanel1.TabIndex = 9;
    this.lblUniqueRuleType.AutoSize = true;
    this.lblUniqueRuleType.Dock = DockStyle.Fill;
    this.lblUniqueRuleType.Location = new Point(3, 0);
    this.lblUniqueRuleType.Name = "lblUniqueRuleType";
    this.lblUniqueRuleType.Size = new Size(144 /*0x90*/, 13);
    this.lblUniqueRuleType.TabIndex = 14;
    this.lblUniqueRuleType.Text = "Тип контроля:";
    this.cbxUniqueRuleType.Dock = DockStyle.Top;
    this.cbxUniqueRuleType.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbxUniqueRuleType.FormattingEnabled = true;
    this.cbxUniqueRuleType.Location = new Point(3, 16 /*0x10*/);
    this.cbxUniqueRuleType.Name = "cbxUniqueRuleType";
    this.cbxUniqueRuleType.Size = new Size(144 /*0x90*/, 21);
    this.cbxUniqueRuleType.TabIndex = 10;
    this.cbxUniqueRuleType.SelectedIndexChanged += new EventHandler(this.cbx_UniqueRuleTypeChanged);
    this.lblId.AutoSize = true;
    this.lblId.Dock = DockStyle.Fill;
    this.lblId.Location = new Point(3, 40);
    this.lblId.Name = "lblId";
    this.lblId.Size = new Size(144 /*0x90*/, 13);
    this.lblId.TabIndex = 11;
    this.lblId.Text = "Идентификатор правила:";
    this.txbId.Dock = DockStyle.Fill;
    this.txbId.Location = new Point(3, 56);
    this.txbId.Name = "txbId";
    this.txbId.Size = new Size(144 /*0x90*/, 20);
    this.txbId.TabIndex = 13;
    this.txbId.TextChanged += new EventHandler(this.cbx_UniqueRuleTypeChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (UniqueControlRuleConfigEditorView);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
