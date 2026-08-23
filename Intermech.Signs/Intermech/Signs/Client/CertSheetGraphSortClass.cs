// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetGraphSortClass
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

#nullable disable
namespace Intermech.Signs.Client;

internal class CertSheetGraphSortClass
{
  public CertSheetGraphSortMethod CertSheetGraphSortMethod { get; set; }

  public CertSheetGraphSortClass(CertSheetGraphSortMethod certSheetGraphSortMethod)
  {
    this.CertSheetGraphSortMethod = certSheetGraphSortMethod;
  }

  public override string ToString()
  {
    return CertSheetGrapSorthHelper.GetCaption(this.CertSheetGraphSortMethod);
  }
}
