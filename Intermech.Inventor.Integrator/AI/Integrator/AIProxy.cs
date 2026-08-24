// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.AIProxy
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using Intermech.CADInterface.Proxies;
using Interop.CADInterface;
using System.Collections.Generic;

#nullable disable
namespace Intermech.AI.Integrator;

internal sealed class AIProxy(ICADSystem2 rawCADSystem, CADSystemProxyBuilder builder) : 
  CADSystemProxy(rawCADSystem, builder)
{
  protected override void FilterOpenFiles(List<string> pathList)
  {
    base.FilterOpenFiles(pathList);
    pathList.Remove("VirtualDocument");
  }
}
