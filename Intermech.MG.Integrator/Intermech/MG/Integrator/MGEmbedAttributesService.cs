// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGEmbedAttributesService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.DataExchange;
using Intermech.Tools.Integrators;
using System.Diagnostics;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGEmbedAttributesService(IIntegrator owner) : EmbedAttributesService(owner)
{
  private DocumentEmbedAttributesDriver driver;

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this.driver = (DocumentEmbedAttributesDriver) new MGDocumentEmbedAttributesDriver(this.Integrator);
  }

  protected override IEmbedAttributesDriver Driver
  {
    [DebuggerStepThrough] get => (IEmbedAttributesDriver) this.driver;
  }

  protected override void ConfigureDriverParameters() => base.ConfigureDriverParameters();

  protected override void ResetDriverParameters() => base.ResetDriverParameters();
}
