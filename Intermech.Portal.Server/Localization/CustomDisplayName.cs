// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.CustomDisplayName
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.Localization;

internal class CustomDisplayName : DisplayNameAttribute
{
  public CustomDisplayName(string displayName)
  {
    object obj = (object) LocalizationHolder.rma.GetString(displayName);
    this.DisplayNameValue = obj != null ? (string) obj : string.Empty;
  }
}
