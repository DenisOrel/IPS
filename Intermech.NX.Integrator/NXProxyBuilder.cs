// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXProxyBuilder
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.CADInterface.Proxies;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXProxyBuilder : CADSystemProxyBuilder
{
  protected override CADDocumentProxy DoCreateDocument(
    ICADDocumentProvider provider,
    CADSystemProxy appProxy)
  {
    return (CADDocumentProxy) new NXDocument(provider, appProxy);
  }

  protected override IModelConfigurationNameMangler DoCreateConfigurationNameMangler()
  {
    return (IModelConfigurationNameMangler) new DynamicModelConfigurationNameMangler("NX Default Configuration");
  }
}
