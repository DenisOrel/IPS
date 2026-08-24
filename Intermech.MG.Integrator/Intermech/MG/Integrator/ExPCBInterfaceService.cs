// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBInterfaceService
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

internal sealed class ExPCBInterfaceService(IIntegrator owner) : MGInterfaceService<ExPCBApplication>(owner, MGConsts.ExPCBApplicationName, MGConsts.ExPCBProgID)
{
  protected override string[] ApplicationClassID
  {
    get
    {
      return new string[3]
      {
        MGConsts.ExPCBClassID,
        MGConsts.ExPCBClassID21,
        MGConsts.ExPCBClassID22
      };
    }
  }

  protected override void OnCadObjectInitialize(object cadObject)
  {
    // ISSUE: reference to a compiler-generated field
    if (ExPCBInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ExPCBInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Visible", typeof (ExPCBInterfaceService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj = ExPCBInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) ExPCBInterfaceService.\u003C\u003Eo__3.\u003C\u003Ep__0, cadObject, true);
  }

  protected override ExPCBApplication DoCreateApplicationObject()
  {
    object orCreateCadObject = this.FindOrCreateCadObject();
    // ISSUE: reference to a compiler-generated field
    if (ExPCBInterfaceService.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ExPCBInterfaceService.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, Type, IIntegrator, MGIntegratorSettings, object, ExPCBApplication>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (ExPCBInterfaceService), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return ExPCBInterfaceService.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) ExPCBInterfaceService.\u003C\u003Eo__4.\u003C\u003Ep__0, typeof (ExPCBApplication), this.Integrator, this.settingsSvc.GetSettings(), orCreateCadObject);
  }

  protected override void DoTestApplicationObject(ExPCBApplication applicationObject)
  {
    if (!applicationObject.IsAlive)
      throw new COMException("CAD object is dead.");
  }

  protected override IAttributeCodec GetArticleCodec(
    MGSettingsService settingsSvc,
    bool isAssemblyCodec)
  {
    return isAssemblyCodec ? (IAttributeCodec) new MGArticleCodec<ExPCBAssemblyComponent>(this.settingsSvc, isAssemblyCodec) : (IAttributeCodec) new MGArticleCodec<ExPCBComponent>(this.settingsSvc, isAssemblyCodec);
  }

  protected override void DoReleaseApplicationObject(ExPCBApplication applicationObject)
  {
    applicationObject.CloseProject();
    base.DoReleaseApplicationObject(applicationObject);
  }

  protected override IAttributeCodec GetDocumentCodec(MGSettingsService settingsSvc)
  {
    return (IAttributeCodec) new MGDocumentCodec<ExPCBAssemblyComponent>(this.settingsSvc);
  }
}
