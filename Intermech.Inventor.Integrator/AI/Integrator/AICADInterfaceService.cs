// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.AICADInterfaceService
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using Intermech.CADInterface.Proxies;
using Intermech.Runtime.ComInterop;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Interop.CADInterface;

#nullable disable
namespace Intermech.AI.Integrator;

internal sealed class AICADInterfaceService(IIntegrator owner) : CADInterfaceService(owner, AIConsts.ApplicationName, (ComObjectProvider) new ProgIdProvider("IMPDM.AICADSystem", true))
{
  protected override CADSystemProxy CreateCADSystemProxy(
    ICADSystem2 cadInterface,
    CADSystemProxyBuilder builder)
  {
    return (CADSystemProxy) new AIProxy(cadInterface, builder);
  }

  protected override CADSystemProxyBuilder CreateProxyBuilder()
  {
    return (CADSystemProxyBuilder) new AIProxyBuilder();
  }
}
