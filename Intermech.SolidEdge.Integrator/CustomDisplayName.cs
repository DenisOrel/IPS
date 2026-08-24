// Decompiled with JetBrains decompiler
// Type: Intermech.SolidEdge.Integrator.CustomDisplayName
// Assembly: Intermech.SolidEdge.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 213B90F8-0434-43B8-B8F6-9AF19E139193
// Assembly location: D:\IPS\Client\Intermech.SolidEdge.Integrator.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.SolidEdge.Integrator;

internal class CustomDisplayName : DisplayNameAttribute
{
  public CustomDisplayName(string displayName)
  {
    object obj = (object) Localization.rma.GetString(displayName);
    this.DisplayNameValue = obj != null ? (string) obj : string.Empty;
  }
}
