// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.CustomCategory
// Assembly: Intermech.Sales, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D9A9043-6548-439B-99F7-AF22F44A5D2B
// Assembly location: D:\IPS\Client\Intermech.Sales.dll

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
