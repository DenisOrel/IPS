// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBAssemblyComponent
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces.Client;
using Intermech.Tools.Integrators.Electrical;
using MGCPCB;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBAssemblyComponent(
  Document component,
  MGIntegratorSettings integratorSettings) : MGComponent<Document>(component, integratorSettings)
{
  public override string UID
  {
    get
    {
      return this.Instance.FullName.Replace(ClientContext.FileVault.WorkArea.AreaPath, string.Empty).TrimStart('\\');
    }
  }

  protected override string InternalGetPropertyValue(string propertyName) => string.Empty;

  public override void SetPropertyValue(string propertyName, object value)
  {
  }

  public override IComponentProperty GetProperty(string propertyName) => (IComponentProperty) null;

  public override FunctionalGroup FunctionalGroup
  {
    get => (FunctionalGroup) null;
    set
    {
    }
  }

  public override string PartNumber => this.UID;

  public override string PosDesignation => string.Empty;
}
