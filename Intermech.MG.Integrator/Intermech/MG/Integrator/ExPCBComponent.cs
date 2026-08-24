// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBComponent
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators.Electrical;
using MGCPCB;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBComponent : MGComponent<Component>
{
  private SpecialAttributesService _specService;

  public ExPCBComponent(
    Component component,
    MGIntegratorSettings integratorSettings,
    SpecialAttributesService specService)
    : base(component, integratorSettings)
  {
    this._specService = specService;
  }

  public override string UID => this.Instance.RefDes;

  public override string PartNumber
  {
    get => this.InternalGetPropertyValue(SpecialAttributesConsts.PartNumberAtribute);
  }

  public override string PosDesignation
  {
    get => this.InternalGetPropertyValue(SpecialAttributesConsts.RefDesignatorAtribute);
  }

  protected override string InternalGetPropertyValue(string attributeName)
  {
    string propertyValue;
    if (this._specService.ReadValue(this.Instance, attributeName, out propertyValue))
      return propertyValue;
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    Property property = this.Instance.FindProperty(attributeName);
    try
    {
      return property == null ? this._specService.ReadCustomValue(this.Instance, attributeName) : property.Value;
    }
    finally
    {
      if (property != null)
        Marshal.FinalReleaseComObject((object) property);
    }
  }

  public override void SetPropertyValue(string attributeName, object value)
  {
    if (this._specService.WriteValue(this.Instance, attributeName, Convert.ToString(value)))
      return;
    // ISSUE: reference to a compiler-generated method
    // ISSUE: variable of a compiler-generated type
    Property property = this.Instance.FindProperty(attributeName);
    try
    {
      if (property == null)
      {
        // ISSUE: reference to a compiler-generated method
        this.Instance.PutProperty(attributeName, Convert.ToString(value));
      }
      else
        property.Value = Convert.ToString(value);
    }
    finally
    {
      if (property != null)
        Marshal.FinalReleaseComObject((object) property);
    }
  }

  public override IComponentProperty GetProperty(string attributeName)
  {
    throw new NotImplementedException();
  }

  public override FunctionalGroup FunctionalGroup
  {
    get => (FunctionalGroup) null;
    set
    {
    }
  }
}
