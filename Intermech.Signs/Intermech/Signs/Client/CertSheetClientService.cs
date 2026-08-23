// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetClientService
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Document.Model;
using Intermech.Signs.Interfaces;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

public class CertSheetClientService : ICertSheetClientService
{
  public List<ImDocument> CreateCertSheets(
    List<long> docIdList,
    bool silentMode,
    ref ExpiredAuthFileUsing expiredAuthFileUsingMode)
  {
    using (CertSheetForm certSheetForm = new CertSheetForm())
    {
      if (certSheetForm.ShowDialog(docIdList) == DialogResult.OK)
        return new CertSheetProcessor(certSheetForm.CertSheetControl.GetCertSheetOptions()).CreateCertSheets(silentMode, ref expiredAuthFileUsingMode);
    }
    return (List<ImDocument>) null;
  }
}
