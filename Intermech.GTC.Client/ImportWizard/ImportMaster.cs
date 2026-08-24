// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.ImportWizard.ImportMaster
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.GTC.Interfaces;
using Intermech.UI.Winforms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client.ImportWizard;

public class ImportMaster : WizardForm
{
  private IImportConfig _importSettings;
  private IContainer components;

  public ImportMaster()
  {
    this.InitializeComponent();
    this._importSettings = (IImportConfig) new ImportConfig();
    this.Pages.Add((IWizardPage) new StartPage(this._importSettings));
    this.Pages.Add((IWizardPage) new EndPage(this._importSettings));
  }

  public IImportConfig ImportSettings => this._importSettings;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.SuspendLayout();
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(634, 412);
    this.Name = nameof (ImportMaster);
    this.Text = "Импорт каталога GTC";
    this.ResumeLayout(false);
  }
}
