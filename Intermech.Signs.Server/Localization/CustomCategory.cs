// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.CustomCategory
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.Localization;

internal class CustomCategory(string сategory) : CategoryAttribute(сategory)
{
  protected override string GetLocalizedString(string value)
  {
    return LocalizationHolder.rma.GetString(value) == null ? string.Empty : LocalizationHolder.rma.GetString(value);
  }
}
