// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.OutputConfigControl
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

public class OutputConfigControl : UserControl
{
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private Label label1;
  private TextBox txbOutputDir;
  private Button button1;
  private ImageList Images;
  private FolderBrowserDialog dlgFolderBrowser;

  public OutputConfigControl() => this.InitializeComponent();

  public OutputConfig TargetConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public void ApplyChanges()
  {
    if (this.TargetConfig == null)
      return;
    this.TargetConfig.WorkDir = this.txbOutputDir.Text;
  }

  public event EventHandler<bool> OnDataChanged;

  private void ApplyConfigToControl(OutputConfig config)
  {
    this.txbOutputDir.Text = this.TargetConfig?.WorkDir;
  }

  private void button1_Click(object sender, EventArgs e)
  {
    if (!string.IsNullOrEmpty(this.txbOutputDir.Text) && Directory.Exists(this.txbOutputDir.Text))
      this.dlgFolderBrowser.SelectedPath = this.txbOutputDir.Text;
    if (this.dlgFolderBrowser.ShowDialog() != DialogResult.OK)
      return;
    this.txbOutputDir.TextChanged -= new EventHandler(this.txbOutputDir_TextChanged);
    this.txbOutputDir.Text = this.dlgFolderBrowser.SelectedPath;
    EventHandler<bool> onDataChanged = this.OnDataChanged;
    if (onDataChanged != null)
      onDataChanged(sender, this.txbOutputDir.Text != this.TargetConfig?.WorkDir);
    this.txbOutputDir.TextChanged += new EventHandler(this.txbOutputDir_TextChanged);
  }

  private void txbOutputDir_TextChanged(object sender, EventArgs e)
  {
    EventHandler<bool> onDataChanged = this.OnDataChanged;
    if (onDataChanged == null)
      return;
    onDataChanged(sender, this.txbOutputDir.Text != this.TargetConfig?.WorkDir);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OutputConfigControl));
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.label1 = new Label();
    this.txbOutputDir = new TextBox();
    this.button1 = new Button();
    this.Images = new ImageList(this.components);
    this.dlgFolderBrowser = new FolderBrowserDialog();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 2;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.txbOutputDir, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.button1, 1, 1);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 2;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.Size = new Size(491, 56);
    this.tableLayoutPanel1.TabIndex = 0;
    this.label1.AutoSize = true;
    this.label1.Dock = DockStyle.Fill;
    this.label1.Location = new Point(3, 3);
    this.label1.Margin = new Padding(3, 3, 3, 0);
    this.label1.Name = "label1";
    this.label1.Size = new Size(447, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Директория для выгрузки";
    this.txbOutputDir.Dock = DockStyle.Fill;
    this.txbOutputDir.Location = new Point(3, 19);
    this.txbOutputDir.Name = "txbOutputDir";
    this.txbOutputDir.Size = new Size(447, 20);
    this.txbOutputDir.TabIndex = 1;
    this.txbOutputDir.TextChanged += new EventHandler(this.txbOutputDir_TextChanged);
    this.button1.Dock = DockStyle.Fill;
    this.button1.ForeColor = SystemColors.ButtonFace;
    this.button1.ImageIndex = 0;
    this.button1.ImageList = this.Images;
    this.button1.Location = new Point(458, 16 /*0x10*/);
    this.button1.Margin = new Padding(5, 0, 5, 0);
    this.button1.MaximumSize = new Size(28, 28);
    this.button1.Name = "button1";
    this.button1.Size = new Size(28, 28);
    this.button1.TabIndex = 2;
    this.button1.UseVisualStyleBackColor = true;
    this.button1.Click += new EventHandler(this.button1_Click);
    this.Images.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("Images.ImageStream");
    this.Images.TransparentColor = Color.Fuchsia;
    this.Images.Images.SetKeyName(0, "open.bmp");
    this.Images.Images.SetKeyName(1, "open.bmp");
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (OutputConfigControl);
    this.Size = new Size(491, 56);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
