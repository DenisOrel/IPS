// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDCommandsProvider
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DXDCommandsProvider(IIntegrator integrator) : ECADCommandsProvider(integrator, MGConsts.ObjTypeDXDProject)
{
  protected override ElementList elementList => (ElementList) new DXDElementList();

  protected override string elementListCommandName => "DXD.CreateElementList";
}
