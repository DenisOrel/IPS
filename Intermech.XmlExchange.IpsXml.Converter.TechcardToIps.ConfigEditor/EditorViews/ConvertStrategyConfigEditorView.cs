// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.ConvertStrategyConfigEditorView
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews;

public class ConvertStrategyConfigEditorView : UserControl
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private TextBox txbStrategyClassName;
  private Label lblScriptFileName;
  private Label lblStrategyClassName;
  private TextBox txbScriptFileName;
  private Button button1;
  private ImageList Images;
  private OpenFileDialog dlgOpenFile;

  public ConvertStrategyConfigEditorView() => this.InitializeComponent();

  public ConvertStrategyConfig TargetConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public bool ApplyChanges()
  {
    this.TargetConfig.ScriptFileName = this.txbScriptFileName.Text;
    this.TargetConfig.StrategyClassName = this.txbStrategyClassName.Text;
    return true;
  }

  private void ApplyConfigToControl(ConvertStrategyConfig config)
  {
    this.SuspendLayout();
    this.txbScriptFileName.Text = config.ScriptFileName;
    this.txbStrategyClassName.Text = config.StrategyClassName;
    this.ResumeLayout();
  }

  public event EventHandler<bool> OnDataChanged;

  private void txb_TextChanged(object sender, EventArgs e)
  {
    EventHandler<bool> onDataChanged = this.OnDataChanged;
    if (onDataChanged == null)
      return;
    onDataChanged(sender, this.txbScriptFileName.Text != this.TargetConfig.ScriptFileName || this.txbStrategyClassName.Text != this.TargetConfig.StrategyClassName);
  }

  private void button1_Click(object sender, EventArgs e)
  {
    if (!string.IsNullOrEmpty(this.txbScriptFileName.Text) && File.Exists(this.txbScriptFileName.Text))
      this.dlgOpenFile.FileName = this.txbScriptFileName.Text;
    if (this.dlgOpenFile.ShowDialog() != DialogResult.OK)
      return;
    this.txbScriptFileName.TextChanged -= new EventHandler(this.txb_TextChanged);
    this.txbScriptFileName.Text = this.dlgOpenFile.FileName;
    EventHandler<bool> onDataChanged = this.OnDataChanged;
    if (onDataChanged != null)
      onDataChanged(sender, this.txbScriptFileName.Text != this.TargetConfig.ScriptFileName);
    this.txbScriptFileName.TextChanged += new EventHandler(this.txb_TextChanged);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConvertStrategyConfigEditorView));
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.button1 = new Button();
    this.txbStrategyClassName = new TextBox();
    this.lblScriptFileName = new Label();
    this.lblStrategyClassName = new Label();
    this.txbScriptFileName = new TextBox();
    this.Images = new ImageList(this.components);
    this.dlgOpenFile = new OpenFileDialog();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 2;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.Controls.Add((Control) this.button1, 1, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbStrategyClassName, 0, 3);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblScriptFileName, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.lblStrategyClassName, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbScriptFileName, 0, 1);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 4;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Size = new Size(299, 85);
    this.tableLayoutPanel1.TabIndex = 1;
    this.button1.Dock = DockStyle.Fill;
    this.button1.ForeColor = SystemColors.ButtonFace;
    this.button1.ImageIndex = 0;
    this.button1.ImageList = this.Images;
    this.button1.Location = new Point(266, 13);
    this.button1.Margin = new Padding(5, 0, 5, 0);
    this.button1.MaximumSize = new Size(28, 28);
    this.button1.Name = "button1";
    this.button1.Size = new Size(28, 28);
    this.button1.TabIndex = 4;
    this.button1.UseVisualStyleBackColor = true;
    this.button1.Click += new EventHandler(this.button1_Click);
    this.txbStrategyClassName.Dock = DockStyle.Fill;
    this.txbStrategyClassName.Location = new Point(3, 57);
    this.txbStrategyClassName.Name = "txbStrategyClassName";
    this.txbStrategyClassName.Size = new Size((int) byte.MaxValue, 20);
    this.txbStrategyClassName.TabIndex = 3;
    this.txbStrategyClassName.TextChanged += new EventHandler(this.txb_TextChanged);
    this.lblScriptFileName.AutoSize = true;
    this.lblScriptFileName.Dock = DockStyle.Fill;
    this.lblScriptFileName.Location = new Point(3, 0);
    this.lblScriptFileName.Name = "lblScriptFileName";
    this.lblScriptFileName.Size = new Size((int) byte.MaxValue, 13);
    this.lblScriptFileName.TabIndex = 0;
    this.lblScriptFileName.Text = "Файл скрипта:";
    this.lblStrategyClassName.AutoSize = true;
    this.lblStrategyClassName.Dock = DockStyle.Fill;
    this.lblStrategyClassName.Location = new Point(3, 41);
    this.lblStrategyClassName.Name = "lblStrategyClassName";
    this.lblStrategyClassName.Size = new Size((int) byte.MaxValue, 13);
    this.lblStrategyClassName.TabIndex = 1;
    this.lblStrategyClassName.Text = "Имя класса стратегии конвертации:";
    this.txbScriptFileName.Dock = DockStyle.Fill;
    this.txbScriptFileName.Location = new Point(3, 16 /*0x10*/);
    this.txbScriptFileName.Name = "txbScriptFileName";
    this.txbScriptFileName.Size = new Size((int) byte.MaxValue, 20);
    this.txbScriptFileName.TabIndex = 2;
    this.txbScriptFileName.TextChanged += new EventHandler(this.txb_TextChanged);
    this.Images.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("Images.ImageStream");
    this.Images.TransparentColor = Color.Fuchsia;
    this.Images.Images.SetKeyName(0, "open.bmp");
    this.Images.Images.SetKeyName(1, "open.bmp");
    this.dlgOpenFile.DefaultExt = "cs";
    this.dlgOpenFile.Filter = "Script files|*.cs";
    this.dlgOpenFile.Title = "Выбрать файл скрипта";
    this.dlgOpenFile.RestoreDirectory = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (ConvertStrategyConfigEditorView);
    this.Size = new Size(299, 85);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
