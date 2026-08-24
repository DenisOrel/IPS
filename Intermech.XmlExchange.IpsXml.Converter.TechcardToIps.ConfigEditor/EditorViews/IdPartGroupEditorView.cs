// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.IdPartGroupEditorView
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews;

public class IdPartGroupEditorView : UserControl
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private TextBox txbId;
  private Label lblId;
  private Label lblCondition;
  private TextBox txbDescr;
  private Label lblDescr;
  private ComboBox cbxType;

  public IdPartGroupEditorView() => this.InitializeComponent();

  public IdPartGroup TargetConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public bool ApplyChanges()
  {
    if (this.TargetConfig != null)
    {
      this.TargetConfig.Id = this.txbId.Text;
      this.TargetConfig.Description = this.txbDescr.Text;
      this.TargetConfig.Condition = this.cbxType.SelectedItem != null ? (ConditionType) this.cbxType.SelectedItem : ConditionType.And;
    }
    return true;
  }

  private void ApplyConfigToControl(IdPartGroup config)
  {
    this.SuspendLayout();
    this.txbId.Text = config.Id;
    this.txbDescr.Text = config.Description;
    this.cbxType.BeginUpdate();
    this.cbxType.Items.Clear();
    Enum.GetValues(typeof (ConditionType)).Cast<ConditionType>().ToList<ConditionType>().ForEach((Action<ConditionType>) (value => this.cbxType.Items.Add((object) value)));
    this.cbxType.SelectedItem = (object) config.Condition;
    this.cbxType.DisplayMember = "ToXMLTag";
    this.cbxType.EndUpdate();
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

  private void cbx_SelectedValueChanged(object sender, EventArgs e)
  {
    EventHandler<bool> onDataChanged = this.OnDataChanged;
    if (onDataChanged == null)
      return;
    onDataChanged(sender, this.IsDataChanged());
  }

  private bool IsDataChanged()
  {
    if (this.txbId.Text != this.TargetConfig.Id || this.txbDescr.Text != this.TargetConfig.Description)
      return true;
    return this.cbxType.SelectedItem != null && (ConditionType) this.cbxType.SelectedItem != this.TargetConfig.Condition;
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
    this.lblCondition = new Label();
    this.txbDescr = new TextBox();
    this.lblDescr = new Label();
    this.txbId = new TextBox();
    this.lblId = new Label();
    this.cbxType = new ComboBox();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.lblCondition, 0, 4);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbDescr, 0, 3);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblDescr, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbId, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblId, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.cbxType, 0, 5);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 6;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.Size = new Size(305, 149);
    this.tableLayoutPanel1.TabIndex = 0;
    this.lblCondition.AutoSize = true;
    this.lblCondition.Dock = DockStyle.Fill;
    this.lblCondition.Location = new Point(3, 78);
    this.lblCondition.Name = "lblCondition";
    this.lblCondition.Size = new Size(299, 13);
    this.lblCondition.TabIndex = 6;
    this.lblCondition.Text = "Условие:";
    this.txbDescr.Dock = DockStyle.Fill;
    this.txbDescr.Location = new Point(3, 55);
    this.txbDescr.Name = "txbDescr";
    this.txbDescr.Size = new Size(299, 20);
    this.txbDescr.TabIndex = 5;
    this.txbDescr.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblDescr.AutoSize = true;
    this.lblDescr.Dock = DockStyle.Fill;
    this.lblDescr.Location = new Point(3, 39);
    this.lblDescr.Name = "lblDescr";
    this.lblDescr.Size = new Size(299, 13);
    this.lblDescr.TabIndex = 4;
    this.lblDescr.Text = "Описание:";
    this.txbId.Dock = DockStyle.Fill;
    this.txbId.Location = new Point(3, 16 /*0x10*/);
    this.txbId.Name = "txbId";
    this.txbId.Size = new Size(299, 20);
    this.txbId.TabIndex = 0;
    this.txbId.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblId.AutoSize = true;
    this.lblId.Dock = DockStyle.Fill;
    this.lblId.Location = new Point(3, 0);
    this.lblId.Name = "lblId";
    this.lblId.Size = new Size(299, 13);
    this.lblId.TabIndex = 1;
    this.lblId.Text = "Идентификатор:";
    this.cbxType.Dock = DockStyle.Fill;
    this.cbxType.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbxType.FormattingEnabled = true;
    this.cbxType.Location = new Point(3, 94);
    this.cbxType.Name = "cbxType";
    this.cbxType.Size = new Size(299, 21);
    this.cbxType.TabIndex = 7;
    this.cbxType.SelectedValueChanged += new EventHandler(this.cbx_SelectedValueChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (IdPartGroupEditorView);
    this.Size = new Size(305, 149);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
