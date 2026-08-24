// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeDocSettingsView
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.DatabaseConfigurator;
using Intermech.Diagnostics;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal class OfficeDocSettingsView : IAdditionalView
{
  [CanBeNull]
  private OfficeDocSettingsTabPage _officeTabPage;

  [NotNull]
  public IAdditionalTabPage GetPage(Guid aInstGuid)
  {
    return (IAdditionalTabPage) this._officeTabPage ?? (IAdditionalTabPage) (this._officeTabPage = new OfficeDocSettingsTabPage(aInstGuid));
  }
}
