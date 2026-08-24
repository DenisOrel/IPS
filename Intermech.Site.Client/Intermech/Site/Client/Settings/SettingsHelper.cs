// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.SettingsHelper
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using System;

#nullable disable
namespace Intermech.Site.Client.Settings;

internal static class SettingsHelper
{
  public static long GetReceiptTemplateID(IUserSession session)
  {
    IDBConfigurations service = ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations;
    return SettingsHelper.GetReceiptTemplateID(session, service);
  }

  public static long GetReceiptTemplateID(IUserSession session, IDBConfigurations configs)
  {
    QuickObjectInfo objectInfo = session.GetObjectInfo(new Guid("cad0028f-306c-11d8-b4e9-00304f19f545"));
    return configs.ReadInteger(PortalConsts.PortalClientModuleName, "GENERAL_SETTINGS", "RECEIPT_TEMPL_ID", objectInfo.ObjectID, DBConfigMode.GlobalOnly);
  }
}
