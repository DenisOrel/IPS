// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.StepControls.StepControlInit
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Manager.StepControls;

public class StepControlInit : ThreadedStepControl, IConfigurable
{
  private Image _image;
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;

  public StepControlInit(object owner)
    : base(owner)
  {
    this.InitializeComponent();
  }

  protected override string getCaption() => "Подготовка метаданных";

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgReadData")];
    return this._image;
  }

  public override void RefreshControl()
  {
    base.RefreshControl();
    if (this.owner == null || !(this.owner is WizardForm))
      return;
    foreach (IPumpTask inits in (this.owner as WizardForm).initsCollection)
      this.AddListViewItem(inits.GUID, inits.Description);
  }

  public override SaveSettingsResult SaveSettings()
  {
    this.StartMainThread((this.owner as WizardForm).initsCollection);
    return SaveSettingsResult.ssrError;
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
    this.SuspendLayout();
    this.tableLayoutPanel1.BackgroundImageLayout = ImageLayout.None;
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 6;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 81.12676f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.Size = new Size(766, 282);
    this.tableLayoutPanel1.TabIndex = 1;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (StepControlInit);
    this.Size = new Size(766, 282);
    this.ResumeLayout(false);
  }

  void IConfigurable.LoadConfiguration() => this.LoadConfiguration();

  void IConfigurable.LoadConfiguration(IConfiguration cfg) => this.LoadConfiguration(cfg);

  void IConfigurable.SaveConfiguration() => this.SaveConfiguration();

  void IConfigurable.SaveConfiguration(IConfiguration cfg) => this.SaveConfiguration(cfg);
}
