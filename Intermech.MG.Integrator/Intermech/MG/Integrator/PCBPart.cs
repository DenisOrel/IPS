// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.PCBPart
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class PCBPart
{
  public string Number { get; private set; }

  public string Description { get; set; }

  public object Instance { get; private set; }

  public PCBPart(object instance)
  {
    // ISSUE: reference to a compiler-generated field
    if (PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (PCBPart)));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, string> target1 = PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__1.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, string>> p1 = PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__1;
    // ISSUE: reference to a compiler-generated field
    if (PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (Number), typeof (PCBPart), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj1 = PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__0, instance);
    this.Number = target1((CallSite) p1, obj1);
    // ISSUE: reference to a compiler-generated field
    if (PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (PCBPart)));
    }
    // ISSUE: reference to a compiler-generated field
    Func<CallSite, object, string> target2 = PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__3.Target;
    // ISSUE: reference to a compiler-generated field
    CallSite<Func<CallSite, object, string>> p3 = PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__3;
    // ISSUE: reference to a compiler-generated field
    if (PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, nameof (Description), typeof (PCBPart), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
      {
        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
      }));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    object obj2 = PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__2.Target((CallSite) PCBPart.\u003C\u003Eo__12.\u003C\u003Ep__2, instance);
    this.Description = target2((CallSite) p3, obj2);
    this.Instance = instance;
  }
}
