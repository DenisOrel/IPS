// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.CustomCategory
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.NX.Integrator;

internal class CustomCategory(string сategory) : CategoryAttribute(сategory)
{
  protected override string GetLocalizedString(string value)
  {
    return Localization.rma.GetString(value) == null ? string.Empty : Localization.rma.GetString(value);
  }
}
