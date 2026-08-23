// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetGrapSorthHelper
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using System;

#nullable disable
namespace Intermech.Signs.Client;

internal class CertSheetGrapSorthHelper
{
  public static string GetCaption(CertSheetGraphSortMethod method)
  {
    return EnumTypeHelper.GetCaption((Enum) method);
  }

  public static CertSheetGraphSortMethod GetCertSheetGraphSortMethod(string s)
  {
    return (CertSheetGraphSortMethod) EnumTypeHelper.GetEnumValue(typeof (CertSheetGraphSortMethod), s);
  }
}
