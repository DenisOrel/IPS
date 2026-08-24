// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDComponent
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Data;
using Intermech.Tools.Integrators.Electrical;
using Interop.Viewdraw;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DXDComponent : MGComponent<IVdComp>
{
  private FunctionalGroup _functionalGroup;

  public DXDComponent(IVdComp component, MGIntegratorSettings integratorSettings)
    : this(component, integratorSettings, (FunctionalGroup) null)
  {
  }

  public DXDComponent(
    IVdComp component,
    MGIntegratorSettings integratorSettings,
    FunctionalGroup functionalGroup)
    : base(component, integratorSettings)
  {
    this._functionalGroup = functionalGroup;
  }

  public override string UID
  {
    get
    {
      IVdAttr attribute = this.Instance.FindAttribute("@NAME");
      return attribute == null || string.IsNullOrEmpty(attribute.Value) ? this.Instance.UID : attribute.Value;
    }
  }

  public override string PartNumber
  {
    get => this.GetComponentValue(this.integratorSettings, IDCache.Default.Name.Text);
  }

  public override string PosDesignation
  {
    get => this.InternalGetPropertyValue(this.integratorSettings.PartPosDesignationAttribute);
  }

  protected override string InternalGetPropertyValue(string parameterName)
  {
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    IVdAttr attribute = this.Instance.FindAttribute(parameterName);
    try
    {
      return attribute != null ? (!string.IsNullOrEmpty(attribute.Value) ? attribute.Value : attribute.EitherValue) : (string) null;
    }
    finally
    {
      if (attribute != null)
        Marshal.FinalReleaseComObject((object) attribute);
    }
  }

  public override void SetPropertyValue(string parameterName, object value)
  {
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    IVdAttr o = this.Instance.FindAttribute(parameterName);
    try
    {
      if (o == null)
      {
        // ISSUE: reference to a compiler-generated method
        o = this.Instance.AddAttribute(parameterName, 0, 0, VdVisibilityFlag.VDINVISIBLE);
      }
      string str = Convert.ToString(value);
      if (!(o.Value != str))
        return;
      o.Value = str;
    }
    finally
    {
      if (o != null)
        Marshal.FinalReleaseComObject((object) o);
    }
  }

  public override IComponentProperty GetProperty(string parameterName)
  {
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    IVdAttr attribute = this.Instance.FindAttribute(parameterName);
    return attribute == null ? (IComponentProperty) null : (IComponentProperty) this.GetProperty(attribute);
  }

  private DXDComponentProperty GetProperty(IVdAttr attribute)
  {
    DXDComponentProperty property = new DXDComponentProperty(attribute);
    this.relatedObjects.Add((IDisposable) property);
    return property;
  }

  public override FunctionalGroup FunctionalGroup
  {
    get => this._functionalGroup;
    set => this._functionalGroup = value;
  }
}
