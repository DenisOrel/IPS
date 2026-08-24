// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ImportSettingsPropertiesPage
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Localization;
using System;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class ImportSettingsPropertiesPage : TransferSettingsPropertyPage
{
  protected override ITransferSettings Settings => (ITransferSettings) new ImportSettings();

  public override string PageName => LocalizationHolder.rm.GetString("Site.Client_91");

  public override string HelpTopicID
  {
    get => throw new Exception(LocalizationHolder.rm.GetString("Site.Client_90"));
  }

  public override string HeaderText => string.Empty;
}
