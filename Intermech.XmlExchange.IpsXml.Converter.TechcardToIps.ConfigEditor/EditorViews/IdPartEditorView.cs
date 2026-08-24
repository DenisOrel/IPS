// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.IdPartEditorView
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

public class IdPartEditorView : UserControl
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private TextBox txbId;
  private Label lblId;
  private Label lblParamType;
  private TextBox txbDescr;
  private Label lblDescr;
  private ComboBox cbxParamType;
  private Label lblParamName;
  private Label lblParamSubType;
  private TextBox txbParamName;
  private ComboBox cbxParamSubType;
  private TextBox txbParamValue;
  private Label lblParamValue;

  public IdPartEditorView() => this.InitializeComponent();

  public IdPart TargetConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public bool ApplyChanges()
  {
    if (this.TargetConfig != null)
    {
      this.TargetConfig.Id = this.txbId.Text;
      this.TargetConfig.Description = this.txbDescr.Text;
      this.TargetConfig.ParamType = this.cbxParamType.SelectedItem != null ? (ParamType) this.cbxParamType.SelectedItem : ParamType.Object;
      this.TargetConfig.ParamSubType = this.cbxParamType.SelectedItem != null ? (ParamSubType) this.cbxParamSubType.SelectedItem : ParamSubType.TechcardParam;
      this.TargetConfig.Name = this.txbParamName.Text;
      this.TargetConfig.Value = this.txbParamValue.Text;
    }
    return true;
  }

  private void ApplyConfigToControl(IdPart config)
  {
    this.SuspendLayout();
    this.txbId.Text = config.Id;
    this.txbDescr.Text = config.Description;
    this.cbxParamType.BeginUpdate();
    this.cbxParamType.Items.Clear();
    Enum.GetValues(typeof (ParamType)).Cast<ParamType>().ToList<ParamType>().ForEach((Action<ParamType>) (value => this.cbxParamType.Items.Add((object) value)));
    this.cbxParamType.SelectedItem = (object) config.ParamType;
    this.cbxParamType.DisplayMember = "ToXMLTag";
    this.cbxParamType.EndUpdate();
    this.cbxParamSubType.BeginUpdate();
    this.cbxParamSubType.Items.Clear();
    Enum.GetValues(typeof (ParamSubType)).Cast<ParamSubType>().ToList<ParamSubType>().ForEach((Action<ParamSubType>) (value =>
    {
      if (value == ParamSubType.Unknown)
        return;
      this.cbxParamSubType.Items.Add((object) value);
    }));
    this.cbxParamSubType.SelectedItem = (object) config.ParamSubType;
    this.cbxParamSubType.DisplayMember = "ToXMLTag";
    this.cbxParamSubType.EndUpdate();
    this.txbParamName.Text = config.Name;
    this.txbParamValue.Text = config.Value;
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
    return this.txbId.Text != this.TargetConfig.Id || this.txbDescr.Text != this.TargetConfig.Description || this.cbxParamType.SelectedItem != null && (ParamType) this.cbxParamType.SelectedItem != this.TargetConfig.ParamType || this.cbxParamSubType.SelectedItem != null && (ParamSubType) this.cbxParamSubType.SelectedItem != this.TargetConfig.ParamSubType || this.txbParamName.Text != this.TargetConfig.Name || this.txbParamValue.Text != this.TargetConfig.Value;
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
    this.txbParamValue = new TextBox();
    this.lblParamValue = new Label();
    this.lblId = new Label();
    this.txbId = new TextBox();
    this.lblDescr = new Label();
    this.txbDescr = new TextBox();
    this.lblParamType = new Label();
    this.cbxParamType = new ComboBox();
    this.lblParamSubType = new Label();
    this.cbxParamSubType = new ComboBox();
    this.lblParamName = new Label();
    this.txbParamName = new TextBox();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.txbParamValue, 0, 11);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblParamValue, 0, 10);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblId, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbId, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblDescr, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbDescr, 0, 3);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblParamType, 0, 4);
    this.tableLayoutPanel1.Controls.Add((Control) this.cbxParamType, 0, 5);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblParamSubType, 0, 6);
    this.tableLayoutPanel1.Controls.Add((Control) this.cbxParamSubType, 0, 7);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblParamName, 0, 8);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbParamName, 0, 9);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 12;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.Size = new Size(416, 260);
    this.tableLayoutPanel1.TabIndex = 0;
    this.txbParamValue.Dock = DockStyle.Fill;
    this.txbParamValue.Location = new Point(3, 213);
    this.txbParamValue.Name = "txbParamValue";
    this.txbParamValue.Size = new Size(410, 20);
    this.txbParamValue.TabIndex = 15;
    this.txbParamValue.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblParamValue.AutoSize = true;
    this.lblParamValue.Dock = DockStyle.Fill;
    this.lblParamValue.Location = new Point(3, 197);
    this.lblParamValue.Name = "lblParamValue";
    this.lblParamValue.Size = new Size(410, 13);
    this.lblParamValue.TabIndex = 14;
    this.lblParamValue.Text = "Значение параметра(только для bool типов IdConfig) _any_ - любое значение:";
    this.lblId.AutoSize = true;
    this.lblId.Dock = DockStyle.Fill;
    this.lblId.Location = new Point(3, 0);
    this.lblId.Name = "lblId";
    this.lblId.Size = new Size(410, 13);
    this.lblId.TabIndex = 1;
    this.lblId.Text = "Идентификатор:";
    this.txbId.Dock = DockStyle.Fill;
    this.txbId.Location = new Point(3, 16 /*0x10*/);
    this.txbId.Name = "txbId";
    this.txbId.Size = new Size(410, 20);
    this.txbId.TabIndex = 0;
    this.txbId.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblDescr.AutoSize = true;
    this.lblDescr.Dock = DockStyle.Fill;
    this.lblDescr.Location = new Point(3, 39);
    this.lblDescr.Name = "lblDescr";
    this.lblDescr.Size = new Size(410, 13);
    this.lblDescr.TabIndex = 4;
    this.lblDescr.Text = "Описание:";
    this.txbDescr.Dock = DockStyle.Fill;
    this.txbDescr.Location = new Point(3, 55);
    this.txbDescr.Name = "txbDescr";
    this.txbDescr.Size = new Size(410, 20);
    this.txbDescr.TabIndex = 5;
    this.txbDescr.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblParamType.AutoSize = true;
    this.lblParamType.Dock = DockStyle.Fill;
    this.lblParamType.Location = new Point(3, 78);
    this.lblParamType.Name = "lblParamType";
    this.lblParamType.Size = new Size(410, 13);
    this.lblParamType.TabIndex = 6;
    this.lblParamType.Text = "Владелец параметра:";
    this.cbxParamType.Dock = DockStyle.Fill;
    this.cbxParamType.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbxParamType.FormattingEnabled = true;
    this.cbxParamType.Location = new Point(3, 94);
    this.cbxParamType.Name = "cbxParamType";
    this.cbxParamType.Size = new Size(410, 21);
    this.cbxParamType.TabIndex = 7;
    this.cbxParamType.SelectedValueChanged += new EventHandler(this.cbx_SelectedValueChanged);
    this.lblParamSubType.AutoSize = true;
    this.lblParamSubType.Dock = DockStyle.Fill;
    this.lblParamSubType.Location = new Point(3, 118);
    this.lblParamSubType.Name = "lblParamSubType";
    this.lblParamSubType.Size = new Size(410, 13);
    this.lblParamSubType.TabIndex = 10;
    this.lblParamSubType.Text = "Тип параметра:";
    this.cbxParamSubType.Dock = DockStyle.Fill;
    this.cbxParamSubType.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbxParamSubType.FormattingEnabled = true;
    this.cbxParamSubType.Location = new Point(3, 134);
    this.cbxParamSubType.Name = "cbxParamSubType";
    this.cbxParamSubType.Size = new Size(410, 21);
    this.cbxParamSubType.TabIndex = 13;
    this.cbxParamSubType.SelectedValueChanged += new EventHandler(this.cbx_SelectedValueChanged);
    this.lblParamName.AutoSize = true;
    this.lblParamName.Dock = DockStyle.Fill;
    this.lblParamName.Location = new Point(3, 158);
    this.lblParamName.Name = "lblParamName";
    this.lblParamName.Size = new Size(410, 13);
    this.lblParamName.TabIndex = 8;
    this.lblParamName.Text = "Параметр:";
    this.txbParamName.Dock = DockStyle.Fill;
    this.txbParamName.Location = new Point(3, 174);
    this.txbParamName.Name = "txbParamName";
    this.txbParamName.Size = new Size(410, 20);
    this.txbParamName.TabIndex = 9;
    this.txbParamName.TextChanged += new EventHandler(this.txb_TextChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (IdPartEditorView);
    this.Size = new Size(416, 260);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
