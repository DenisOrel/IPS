// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.SignActionInfo
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Interfaces;
using Intermech.Localization;
using System;

#nullable disable
namespace Intermech.Signs;

public static class SignActionInfo
{
  public static Guid SignUpActionGuid = new Guid("{39B66524-06E4-56b4-B2B3-6E7C158B20C3}");
  public static readonly FormDesignerAction SignUpExecute = new FormDesignerAction(SignActionInfo.SignUpActionGuid, LocalizationHolder.rm.GetString("Signs_97"));
}
