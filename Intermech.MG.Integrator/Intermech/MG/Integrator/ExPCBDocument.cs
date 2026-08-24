// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBDocument
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators.Electrical;
using MGCPCB;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBDocument : MGProjectItem<Document>
{
  private SpecialAttributesService _specService;

  public ExPCBDocument(
    ExPCBProject parent,
    Document document,
    MGIntegratorSettings integratorSettings)
    : base((IMGProject) parent, document, integratorSettings)
  {
    ExPCBPartEditor partEditor = new ExPCBPartEditor();
    partEditor.OpenDB(this.Instance);
    this._specService = new SpecialAttributesService(partEditor);
  }

  public override List<IElectricalComponent> Components
  {
    get
    {
      List<IElectricalComponent> components = new List<IElectricalComponent>();
      MGCPCB.Components o = this.Instance.get_Components();
      try
      {
        foreach (Component component in (IMGCPCBComponents) o)
          components.Add((IElectricalComponent) this.GetComponent(component));
        return components;
      }
      finally
      {
        Marshal.FinalReleaseComObject((object) o);
      }
    }
  }

  private ExPCBComponent GetComponent(Component component)
  {
    ExPCBComponent component1 = new ExPCBComponent(component, this.integratorSettings, this._specService);
    this.relatedObjects.Add((IDisposable) component1);
    return component1;
  }

  public override IElectricalComponent AssemblyComponent
  {
    get
    {
      return (IElectricalComponent) new ExPCBAssemblyComponent(this.Instance, this.integratorSettings);
    }
  }

  public override void Dispose()
  {
    base.Dispose();
    if (this._specService == null)
      return;
    this._specService.Dispose();
  }
}
