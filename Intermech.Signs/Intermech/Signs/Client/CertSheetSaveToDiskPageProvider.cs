// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetSaveToDiskPageProvider
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.Signs.Client;

public class CertSheetSaveToDiskPageProvider : ISaveToDiskPageProvider
{
  public bool CheckItems(ISelectedItems items) => true;

  public ISaveToDiskPage InitPage(ISelectedItems items, ISaveToDiskOptions options)
  {
    if (!this.CheckItems(items))
      return (ISaveToDiskPage) null;
    return (ISaveToDiskPage) new CertSheetSaveToDiskPage(items)
    {
      SaveToDiskOptions = options
    };
  }
}
