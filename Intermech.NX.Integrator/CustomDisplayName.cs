// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.CustomDisplayName
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.NX.Integrator;

internal class CustomDisplayName : DisplayNameAttribute
{
  public CustomDisplayName(string displayName)
  {
    object obj = (object) Localization.rma.GetString(displayName);
    this.DisplayNameValue = obj != null ? (string) obj : string.Empty;
  }
}
