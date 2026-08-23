// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetSaveToDiskPage
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Navigator.Interfaces;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

public class CertSheetSaveToDiskPage : ISaveToDiskPage
{
  private CertSheetControl certSheetControl;
  private ISelectedItems items;
  private ISaveToDiskOptions options;

  public string PageName => "CertSheetPage";

  public int Index => 10;

  public string Caption => "УЛ";

  public UserControl Control
  {
    get
    {
      if (this.certSheetControl == null)
      {
        this.certSheetControl = new CertSheetControl();
        this.certSheetControl.SaveToDiskInterfaceFlag = true;
      }
      return (UserControl) this.certSheetControl;
    }
  }

  public bool CommitEnabled
  {
    get => this.options == null || this.options.OptionSaveFolder != string.Empty;
  }

  public ISaveToDiskProcessor Commit()
  {
    return this.certSheetControl == null ? (ISaveToDiskProcessor) null : (ISaveToDiskProcessor) new CertSheetProcessor(this.certSheetControl.GetCertSheetOptions());
  }

  public void Cancel()
  {
  }

  public ISaveToDiskOptions SaveToDiskOptions
  {
    get => this.options;
    set => this.options = value;
  }

  public CertSheetSaveToDiskPage(ISelectedItems items) => this.items = items;
}
