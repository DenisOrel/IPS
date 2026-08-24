// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDInterfaceService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Integrators;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class DXDInterfaceService(IIntegrator owner) : MGInterfaceService<DXDApplication>(owner, MGConsts.DXDApplicationName, MGConsts.DXDProgID)
{
  protected override string[] ApplicationClassID
  {
    get
    {
      return new string[3]
      {
        MGConsts.DXDClassID,
        MGConsts.DXDClassID21,
        MGConsts.DXDClassID22
      };
    }
  }

  protected override void OnCadObjectInitialize(object cadObject)
  {
    // ISSUE: reference to a compiler-generated field
    if (DXDInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      DXDInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Initialize", (IEnumerable<Type>) null, typeof (DXDInterfaceService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    DXDInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) DXDInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__0, cadObject, Environment.GetEnvironmentVariable(MGConsts.WDirVariable), Environment.GetEnvironmentVariable(MGConsts.LicenseVariable));
    // ISSUE: reference to a compiler-generated field
    if (DXDInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      DXDInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Visible", typeof (DXDInterfaceService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj = DXDInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__1.Target((CallSite) DXDInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__1, cadObject, true);
  }

  protected override void DoTestApplicationObject(DXDApplication proxy)
  {
    if (!proxy.IsAlive)
      throw new COMException("CAD object is dead.");
  }

  protected override DXDApplication DoCreateApplicationObject()
  {
    object orCreateCadObject = this.FindOrCreateCadObject();
    // ISSUE: reference to a compiler-generated field
    if (DXDInterfaceService.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      DXDInterfaceService.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, Type, IIntegrator, MGIntegratorSettings, object, DXDInterfaceService, DXDApplication>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (DXDInterfaceService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return DXDInterfaceService.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) DXDInterfaceService.\u003C\u003Eo__5.\u003C\u003Ep__0, typeof (DXDApplication), this.Integrator, this.settingsSvc.GetSettings(), orCreateCadObject, this);
  }

  protected override IAttributeCodec GetArticleCodec(
    MGSettingsService settingsSvc,
    bool isAssemblyCodec)
  {
    return (IAttributeCodec) new MGArticleCodec<DXDComponent>(this.settingsSvc, isAssemblyCodec);
  }

  protected override IAttributeCodec GetDocumentCodec(MGSettingsService settingsSvc)
  {
    return (IAttributeCodec) new MGDocumentCodec<DXDComponent>(this.settingsSvc);
  }
}
