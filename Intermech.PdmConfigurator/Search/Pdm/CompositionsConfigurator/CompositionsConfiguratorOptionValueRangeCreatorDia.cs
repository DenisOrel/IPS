// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionsConfigurator.CompositionsConfiguratorOptionValueRangeCreatorDialog
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionsConfigurator;

public class CompositionsConfiguratorOptionValueRangeCreatorDialog : Form
{
  private IContainer components;
  private CompositionsConfiguratorOptionValueRangeCreatorControl _compositionsConfiguratorOptionValueRangeCreatorControl;

  public CompositionsConfiguratorOptionValueRangeCreatorDialog() => this.InitializeComponent();

  public CompositionsConfiguratorOptionValueRangeCreatorControl Control
  {
    get => this._compositionsConfiguratorOptionValueRangeCreatorControl;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._compositionsConfiguratorOptionValueRangeCreatorControl = new CompositionsConfiguratorOptionValueRangeCreatorControl();
    this.SuspendLayout();
    this._compositionsConfiguratorOptionValueRangeCreatorControl.Dock = DockStyle.Fill;
    this._compositionsConfiguratorOptionValueRangeCreatorControl.Location = new Point(0, 0);
    this._compositionsConfiguratorOptionValueRangeCreatorControl.Name = "_compositionsConfiguratorOptionValueRangeCreatorControl";
    this._compositionsConfiguratorOptionValueRangeCreatorControl.Size = new Size(694, 117);
    this._compositionsConfiguratorOptionValueRangeCreatorControl.TabIndex = 0;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(694, 117);
    this.Controls.Add((System.Windows.Forms.Control) this._compositionsConfiguratorOptionValueRangeCreatorControl);
    this.Name = nameof (CompositionsConfiguratorOptionValueRangeCreatorDialog);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Создание диапазона значений опции конфигуратора составов";
    this.ResumeLayout(false);
  }
}
