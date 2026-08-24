// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDComponentProperty
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Interop.Viewdraw;
using System;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DXDComponentProperty(IVdAttr attribute) : ComponentProperty<IVdAttr>(attribute)
{
  public override object Value
  {
    get
    {
      if (this.Instance.Value != null && this.Instance.Value != string.Empty)
        return (object) this.Instance.Value;
      if (this.Instance.EitherValue != null && this.Instance.EitherValue != string.Empty)
        return (object) this.Instance.EitherValue;
      if (this.Instance.InstanceValue != null && this.Instance.InstanceValue != string.Empty)
        return (object) this.Instance.InstanceValue;
      if (this.Instance.OatValue != null && this.Instance.OatValue != string.Empty)
        return (object) this.Instance.OatValue;
      if (this.Instance.TextString != null && this.Instance.TextString != string.Empty)
      {
        string[] strArray = this.Instance.TextString.Split('=');
        if (strArray != null && strArray.Length == 2)
          return (object) strArray[1];
      }
      return (object) this.Instance.Value;
    }
    set
    {
      if ((object) this.Instance.Value == value)
        return;
      this.Instance.Value = Convert.ToString(value);
    }
  }
}
