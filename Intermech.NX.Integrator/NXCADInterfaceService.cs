// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXCADInterfaceService
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.CADInterface.Proxies;
using Intermech.Runtime.ComInterop;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Interop.CADInterface;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXCADInterfaceService(IIntegrator owner) : CADInterfaceService(owner, Plugin.IntegratorAppName, (ComObjectProvider) new ClsidProvider(Plugin.NXCLSID, true))
{
  protected override CADSystemProxy CreateCADSystemProxy(
    ICADSystem2 cadInterface,
    CADSystemProxyBuilder builder)
  {
    return (CADSystemProxy) new NXProxy(cadInterface, builder);
  }

  protected override CADSystemProxyBuilder CreateProxyBuilder()
  {
    return (CADSystemProxyBuilder) new NXProxyBuilder();
  }
}
