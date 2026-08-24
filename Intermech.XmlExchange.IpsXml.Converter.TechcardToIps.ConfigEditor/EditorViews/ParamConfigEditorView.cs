// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.ParamConfigEditorView
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

public class ParamConfigEditorView : UserControl
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private TextBox txbDescr;
  private Label lblId;
  private Label lblDescr;
  private TextBox txbId;
  private Label lblSubType;
  private Label lblType;
  private ComboBox cbxSubType;
  private ComboBox cbxType;
  private ComboBox cbxOwner;
  private Label lblOwner;

  public ParamConfigEditorView() => this.InitializeComponent();

  public ParamConfig TargetConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public bool ApplyChanges()
  {
    if (this.TargetConfig == null)
      return false;
    this.TargetConfig.Id = this.txbId.Text;
    this.TargetConfig.Description = this.txbDescr.Text;
    this.TargetConfig.ConfigType = this.cbxType.SelectedItem != null ? (ParamConfigType) this.cbxType.SelectedItem : ParamConfigType.Const;
    this.TargetConfig.ParamSubType = this.cbxType.SelectedItem != null ? (ParamSubType) this.cbxType.SelectedItem : ParamSubType.SearchParam;
    this.TargetConfig.ParamParentType = this.cbxType.SelectedItem != null ? (ParamType) this.cbxType.SelectedItem : ParamType.Object;
    return true;
  }

  private void ApplyConfigToControl(ParamConfig config)
  {
    this.SuspendLayout();
    this.txbId.Text = config.Id;
    this.txbDescr.Text = config.Description;
    this.cbxType.BeginUpdate();
    this.cbxType.Items.Clear();
    Enum.GetValues(typeof (ParamConfigType)).Cast<ParamConfigType>().ToList<ParamConfigType>().ForEach((Action<ParamConfigType>) (value =>
    {
      if (value == ParamConfigType.Unknown)
        return;
      this.cbxType.Items.Add((object) value);
    }));
    this.cbxType.SelectedItem = (object) config.ConfigType;
    this.cbxType.DisplayMember = "ToXMLTag";
    this.cbxType.EndUpdate();
    this.cbxSubType.BeginUpdate();
    this.cbxSubType.Items.Clear();
    Enum.GetValues(typeof (ParamSubType)).Cast<ParamSubType>().ToList<ParamSubType>().ForEach((Action<ParamSubType>) (value =>
    {
      if (value == ParamSubType.Unknown)
        return;
      this.cbxSubType.Items.Add((object) value);
    }));
    this.cbxSubType.SelectedItem = (object) config.ParamSubType;
    this.cbxSubType.DisplayMember = "ToXMLTag";
    this.cbxSubType.EndUpdate();
    this.cbxOwner.BeginUpdate();
    this.cbxOwner.Items.Clear();
    Enum.GetValues(typeof (ParamType)).Cast<ParamType>().ToList<ParamType>().ForEach((Action<ParamType>) (value => this.cbxOwner.Items.Add((object) value)));
    this.cbxOwner.SelectedItem = (object) config.ParamParentType;
    this.cbxOwner.DisplayMember = "ToXMLTag";
    this.cbxOwner.EndUpdate();
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
    if (this.txbId.Text != this.TargetConfig.Id || this.txbDescr.Text != this.TargetConfig.Description || this.cbxType.SelectedItem != null && (ParamConfigType) this.cbxType.SelectedItem != this.TargetConfig.ConfigType || this.cbxSubType.SelectedItem != null && (ParamSubType) this.cbxSubType.SelectedItem != this.TargetConfig.ParamSubType)
      return true;
    return this.cbxOwner.SelectedItem != null && (ParamType) this.cbxOwner.SelectedItem != this.TargetConfig.ParamParentType;
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
    this.cbxOwner = new ComboBox();
    this.lblOwner = new Label();
    this.cbxSubType = new ComboBox();
    this.lblSubType = new Label();
    this.cbxType = new ComboBox();
    this.lblType = new Label();
    this.txbDescr = new TextBox();
    this.lblDescr = new Label();
    this.txbId = new TextBox();
    this.lblId = new Label();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.cbxOwner, 0, 9);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblOwner, 0, 8);
    this.tableLayoutPanel1.Controls.Add((Control) this.cbxSubType, 0, 7);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblSubType, 0, 6);
    this.tableLayoutPanel1.Controls.Add((Control) this.cbxType, 0, 5);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblType, 0, 4);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbDescr, 0, 3);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblDescr, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbId, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblId, 0, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 10;
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
    this.tableLayoutPanel1.Size = new Size(330, 201);
    this.tableLayoutPanel1.TabIndex = 1;
    this.cbxOwner.Dock = DockStyle.Fill;
    this.cbxOwner.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbxOwner.FormattingEnabled = true;
    this.cbxOwner.Location = new Point(3, 174);
    this.cbxOwner.Name = "cbxOwner";
    this.cbxOwner.Size = new Size(324, 21);
    this.cbxOwner.TabIndex = 11;
    this.cbxOwner.SelectedValueChanged += new EventHandler(this.cbx_SelectedValueChanged);
    this.lblOwner.AutoSize = true;
    this.lblOwner.Dock = DockStyle.Fill;
    this.lblOwner.Location = new Point(3, 158);
    this.lblOwner.Name = "lblOwner";
    this.lblOwner.Size = new Size(324, 13);
    this.lblOwner.TabIndex = 10;
    this.lblOwner.Text = "Владелец:";
    this.cbxSubType.Dock = DockStyle.Fill;
    this.cbxSubType.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbxSubType.FormattingEnabled = true;
    this.cbxSubType.Location = new Point(3, 134);
    this.cbxSubType.Name = "cbxSubType";
    this.cbxSubType.Size = new Size(324, 21);
    this.cbxSubType.TabIndex = 9;
    this.cbxSubType.SelectedValueChanged += new EventHandler(this.cbx_SelectedValueChanged);
    this.lblSubType.AutoSize = true;
    this.lblSubType.Dock = DockStyle.Fill;
    this.lblSubType.Location = new Point(3, 118);
    this.lblSubType.Name = "lblSubType";
    this.lblSubType.Size = new Size(324, 13);
    this.lblSubType.TabIndex = 6;
    this.lblSubType.Text = "Подтип:";
    this.cbxType.Dock = DockStyle.Fill;
    this.cbxType.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbxType.FormattingEnabled = true;
    this.cbxType.Location = new Point(3, 94);
    this.cbxType.Name = "cbxType";
    this.cbxType.Size = new Size(324, 21);
    this.cbxType.TabIndex = 8;
    this.cbxType.SelectedValueChanged += new EventHandler(this.cbx_SelectedValueChanged);
    this.lblType.AutoSize = true;
    this.lblType.Dock = DockStyle.Fill;
    this.lblType.Location = new Point(3, 78);
    this.lblType.Name = "lblType";
    this.lblType.Size = new Size(324, 13);
    this.lblType.TabIndex = 4;
    this.lblType.Text = "Тип:";
    this.txbDescr.Dock = DockStyle.Fill;
    this.txbDescr.Location = new Point(3, 55);
    this.txbDescr.Name = "txbDescr";
    this.txbDescr.Size = new Size(324, 20);
    this.txbDescr.TabIndex = 3;
    this.txbDescr.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblDescr.AutoSize = true;
    this.lblDescr.Dock = DockStyle.Fill;
    this.lblDescr.Location = new Point(3, 39);
    this.lblDescr.Name = "lblDescr";
    this.lblDescr.Size = new Size(324, 13);
    this.lblDescr.TabIndex = 1;
    this.lblDescr.Text = "Описание:";
    this.txbId.Dock = DockStyle.Fill;
    this.txbId.Location = new Point(3, 16 /*0x10*/);
    this.txbId.Name = "txbId";
    this.txbId.Size = new Size(324, 20);
    this.txbId.TabIndex = 2;
    this.txbId.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblId.AutoSize = true;
    this.lblId.Dock = DockStyle.Fill;
    this.lblId.Location = new Point(3, 0);
    this.lblId.Name = "lblId";
    this.lblId.Size = new Size(324, 13);
    this.lblId.TabIndex = 0;
    this.lblId.Text = "Идентификатор:";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (ParamConfigEditorView);
    this.Size = new Size(330, 201);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
