// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.StepControls.StepControlPump
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

public class StepControlPump : ThreadedStepControl, IConfigurable
{
  private Image _image;
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;

  public StepControlPump(object owner)
    : base(owner)
  {
    this.InitializeComponent();
    this.isPump = true;
  }

  protected override string getCaption() => "Перекачка данных";

  protected override Image getImage()
  {
    if (this._image == null && ServicesManager.GetService(typeof (IBigImageList)) is IBigImageList service)
      this._image = service.ImageList.Images[service.ImageIndex("imgImportData")];
    return this._image;
  }

  public override void RefreshControl()
  {
    base.RefreshControl();
    if (this.owner == null || !(this.owner is WizardForm))
      return;
    foreach (IPumpTask pumps in (this.owner as WizardForm).pumpsCollection)
      this.AddListViewItem(pumps.GUID, pumps.Description);
  }

  public override SaveSettingsResult SaveSettings()
  {
    if (!(ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo).CheckDBVersion(true))
      return SaveSettingsResult.ssrRetry;
    WizardForm owner = this.owner as WizardForm;
    int count1 = owner.pumpsCollection.Count;
    PumpEvents.RaiseOnStartPump(owner.pumpsCollection);
    int count2 = owner.pumpsCollection.Count;
    if (count1 != count2)
      this.RefreshControl();
    this.StartMainThread(owner.pumpsCollection);
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
    this.tableLayoutPanel1.Size = new Size(756, (int) byte.MaxValue);
    this.tableLayoutPanel1.TabIndex = 2;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (StepControlPump);
    this.Size = new Size(756, (int) byte.MaxValue);
    this.ResumeLayout(false);
  }

  void IConfigurable.LoadConfiguration() => this.LoadConfiguration();

  void IConfigurable.LoadConfiguration(IConfiguration cfg) => this.LoadConfiguration(cfg);

  void IConfigurable.SaveConfiguration() => this.SaveConfiguration();

  void IConfigurable.SaveConfiguration(IConfiguration cfg) => this.SaveConfiguration(cfg);
}
