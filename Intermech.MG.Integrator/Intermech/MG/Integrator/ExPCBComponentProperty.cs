// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBComponentProperty
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using MGCPCB;
using System;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBComponentProperty(Property property) : ComponentProperty<Property>(property)
{
  public override object Value
  {
    get => (object) this.Instance.Value;
    set => this.Instance.Value = Convert.ToString(value);
  }
}
