// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.CustomDescription
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.Localization;

internal class CustomDescription : DescriptionAttribute
{
  public CustomDescription(string description)
  {
    object obj = (object) Intermech.Localization.Localization.rma.GetString(description);
    this.DescriptionValue = obj != null ? (string) obj : string.Empty;
  }
}
