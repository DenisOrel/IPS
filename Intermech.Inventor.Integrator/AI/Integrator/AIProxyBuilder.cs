// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.AIProxyBuilder
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using Intermech.CADInterface.Proxies;

#nullable disable
namespace Intermech.AI.Integrator;

internal sealed class AIProxyBuilder : CADSystemProxyBuilder
{
  protected override IModelConfigurationNameMangler DoCreateConfigurationNameMangler()
  {
    return (IModelConfigurationNameMangler) new DynamicModelConfigurationNameMangler("AI Default Configuration");
  }
}
