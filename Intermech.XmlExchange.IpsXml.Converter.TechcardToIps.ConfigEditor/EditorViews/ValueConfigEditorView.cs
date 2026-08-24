// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.ValueConfigEditorView
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamValueConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews;

public class ValueConfigEditorView : UserControl
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private ComboBox cbxOwner;
  private Label lblOwner;
  private ComboBox cbxSubType;
  private Label lblSubType;
  private ComboBox cbxDestination;
  private Label lblDestination;
  private TextBox txbFieldName;
  private Label lblId;

  public ValueConfigEditorView() => this.InitializeComponent();

  public ValueConfig TargetConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public bool ApplyChanges()
  {
    if (this.TargetConfig == null)
      return false;
    this.TargetConfig.Id = this.txbFieldName.Text;
    return true;
  }

  private void ApplyConfigToControl(ValueConfig config)
  {
    this.SuspendLayout();
    this.txbFieldName.Text = config.Id;
    this.cbxDestination.BeginUpdate();
    this.cbxDestination.Items.Clear();
    Enum.GetValues(typeof (ParamConfigType)).Cast<ParamConfigType>().ToList<ParamConfigType>().ForEach((Action<ParamConfigType>) (value =>
    {
      if (value == ParamConfigType.Unknown)
        return;
      this.cbxDestination.Items.Add((object) value);
    }));
    this.cbxDestination.DisplayMember = "ToXMLTag";
    this.cbxDestination.EndUpdate();
    this.cbxSubType.BeginUpdate();
    this.cbxSubType.Items.Clear();
    Enum.GetValues(typeof (ParamSubType)).Cast<ParamSubType>().ToList<ParamSubType>().ForEach((Action<ParamSubType>) (value =>
    {
      if (value == ParamSubType.Unknown)
        return;
      this.cbxSubType.Items.Add((object) value);
    }));
    this.cbxSubType.DisplayMember = "ToXMLTag";
    this.cbxSubType.EndUpdate();
    this.cbxOwner.BeginUpdate();
    this.cbxOwner.Items.Clear();
    Enum.GetValues(typeof (ParamType)).Cast<ParamType>().ToList<ParamType>().ForEach((Action<ParamType>) (value => this.cbxOwner.Items.Add((object) value)));
    this.cbxOwner.DisplayMember = "ToXMLTag";
    this.cbxOwner.EndUpdate();
    this.ResumeLayout();
  }

  public event EventHandler<bool> OnDataChanged;

  private void txb_TextChanged(object sender, EventArgs e)
  {
  }

  private void cbx_SelectedValueChanged(object sender, EventArgs e)
  {
  }

  private bool IsDataChanged() => this.txbFieldName.Text != this.TargetConfig.Id;

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
    this.cbxDestination = new ComboBox();
    this.lblDestination = new Label();
    this.txbFieldName = new TextBox();
    this.lblId = new Label();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this.cbxOwner, 0, 7);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblOwner, 0, 6);
    this.tableLayoutPanel1.Controls.Add((Control) this.cbxSubType, 0, 5);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblSubType, 0, 4);
    this.tableLayoutPanel1.Controls.Add((Control) this.cbxDestination, 0, 3);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblDestination, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbFieldName, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblId, 0, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 8;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
    this.tableLayoutPanel1.Size = new Size(381, 272);
    this.tableLayoutPanel1.TabIndex = 2;
    this.cbxOwner.Dock = DockStyle.Fill;
    this.cbxOwner.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbxOwner.FormattingEnabled = true;
    this.cbxOwner.Location = new Point(3, 135);
    this.cbxOwner.Name = "cbxOwner";
    this.cbxOwner.Size = new Size(375, 21);
    this.cbxOwner.TabIndex = 11;
    this.lblOwner.AutoSize = true;
    this.lblOwner.Dock = DockStyle.Fill;
    this.lblOwner.Location = new Point(3, 119);
    this.lblOwner.Name = "lblOwner";
    this.lblOwner.Size = new Size(375, 13);
    this.lblOwner.TabIndex = 10;
    this.lblOwner.Text = "Владелец:";
    this.cbxSubType.Dock = DockStyle.Fill;
    this.cbxSubType.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbxSubType.FormattingEnabled = true;
    this.cbxSubType.Location = new Point(3, 95);
    this.cbxSubType.Name = "cbxSubType";
    this.cbxSubType.Size = new Size(375, 21);
    this.cbxSubType.TabIndex = 9;
    this.lblSubType.AutoSize = true;
    this.lblSubType.Dock = DockStyle.Fill;
    this.lblSubType.Location = new Point(3, 79);
    this.lblSubType.Name = "lblSubType";
    this.lblSubType.Size = new Size(375, 13);
    this.lblSubType.TabIndex = 6;
    this.lblSubType.Text = "Подтип:";
    this.cbxDestination.Dock = DockStyle.Fill;
    this.cbxDestination.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbxDestination.FormattingEnabled = true;
    this.cbxDestination.Location = new Point(3, 55);
    this.cbxDestination.Name = "cbxDestination";
    this.cbxDestination.Size = new Size(375, 21);
    this.cbxDestination.TabIndex = 8;
    this.lblDestination.AutoSize = true;
    this.lblDestination.Dock = DockStyle.Fill;
    this.lblDestination.Location = new Point(3, 39);
    this.lblDestination.Name = "lblDestination";
    this.lblDestination.Size = new Size(375, 13);
    this.lblDestination.TabIndex = 4;
    this.lblDestination.Text = "Назначение:";
    this.txbFieldName.Dock = DockStyle.Fill;
    this.txbFieldName.Location = new Point(3, 16 /*0x10*/);
    this.txbFieldName.Name = "txbFieldName";
    this.txbFieldName.Size = new Size(375, 20);
    this.txbFieldName.TabIndex = 2;
    this.lblId.AutoSize = true;
    this.lblId.Dock = DockStyle.Fill;
    this.lblId.Location = new Point(3, 0);
    this.lblId.Name = "lblId";
    this.lblId.Size = new Size(375, 13);
    this.lblId.TabIndex = 0;
    this.lblId.Text = "Наименование:";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (ValueConfigEditorView);
    this.Size = new Size(381, 272);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
