// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBApiSession
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Tools.Integrators;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBApiSession : ApplicationApiSession<ExPCBApplication>
{
  public ExPCBApiSession(IIntegrator integrator)
    : base(integrator)
  {
  }

  public ExPCBApiSession(IApplicationApiService apiService)
    : base(apiService)
  {
  }
}
