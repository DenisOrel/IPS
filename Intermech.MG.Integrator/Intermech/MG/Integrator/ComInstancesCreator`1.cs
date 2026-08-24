// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ComInstancesCreator`1
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Runtime.ComInterop;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.MG.Integrator;

internal static class ComInstancesCreator<TComInterface>
{
  public static TComInterface GetInstance(string progID, string x64ProgID)
  {
    try
    {
      return ComInstancesCreator<TComInterface>.CreateInstance(progID, true);
    }
    catch
    {
      if (Environment.Is64BitProcess)
        return ComInstancesCreator<TComInterface>.CreateInstance(x64ProgID, false);
      throw;
    }
  }

  private static TComInterface CreateInstance(string progID, bool inprocessServer)
  {
    ProgIdProvider progIdProvider = new ProgIdProvider(progID, inprocessServer);
    // ISSUE: reference to a compiler-generated field
    if (ComInstancesCreator<TComInterface>.\u003C\u003Eo__1.\u003C\u003Ep__0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ComInstancesCreator<TComInterface>.\u003C\u003Eo__1.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, TComInterface>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TComInterface), typeof (ComInstancesCreator<TComInterface>)));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    TComInterface instance = ComInstancesCreator<TComInterface>.\u003C\u003Eo__1.\u003C\u003Ep__0.Target((CallSite) ComInstancesCreator<TComInterface>.\u003C\u003Eo__1.\u003C\u003Ep__0, progIdProvider.TryGetRunningInstance());
    if ((object) instance == null)
    {
      // ISSUE: reference to a compiler-generated field
      if (ComInstancesCreator<TComInterface>.\u003C\u003Eo__1.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ComInstancesCreator<TComInterface>.\u003C\u003Eo__1.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, TComInterface>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TComInterface), typeof (ComInstancesCreator<TComInterface>)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      instance = ComInstancesCreator<TComInterface>.\u003C\u003Eo__1.\u003C\u003Ep__1.Target((CallSite) ComInstancesCreator<TComInterface>.\u003C\u003Eo__1.\u003C\u003Ep__1, progIdProvider.CreateInstance());
    }
    return instance;
  }
}
