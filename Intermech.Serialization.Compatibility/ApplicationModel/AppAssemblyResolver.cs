// Decompiled with JetBrains decompiler
// Type: Intermech.ApplicationModel.AppAssemblyResolver
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

#nullable disable
namespace Intermech.ApplicationModel;

public class AppAssemblyResolver
{
  private readonly string baseDirectory;
  private readonly object syncRoot;
  private Action<string> logger;
  private bool enabled;
  [ThreadStatic]
  private int resolveHandlerDepth;
  private static readonly object s_instanceSyncRoot = new object();
  private static AppAssemblyResolver s_instance;
  private static IAppAssemblyResolveFilter s_resolveFilter = (IAppAssemblyResolveFilter) new AppAssemblyResolveFilter();

  public AppAssemblyResolver()
  {
    this.baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    this.syncRoot = new object();
  }

  public string BaseDirectory
  {
    [DebuggerStepThrough] get => this.baseDirectory;
  }

  public Action<string> Logger
  {
    [DebuggerStepThrough] get
    {
      lock (this.syncRoot)
        return this.logger;
    }
    [DebuggerStepThrough] set
    {
      lock (this.syncRoot)
        this.logger = value;
    }
  }

  public bool Enabled
  {
    [DebuggerStepThrough] get
    {
      lock (this.syncRoot)
        return this.enabled;
    }
    [DebuggerStepThrough] set
    {
      lock (this.syncRoot)
      {
        if (this.enabled == value)
          return;
        if (this.enabled)
          this.DoDisable();
        else
          this.DoEnable();
        this.enabled = value;
      }
    }
  }

  public bool Resolving
  {
    [DebuggerStepThrough] get => this.resolveHandlerDepth != 0;
  }

  protected virtual void DoEnable()
  {
    AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(this.OnAssemblyLoad);
    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.OnAssemblyResolve);
  }

  protected virtual void DoDisable()
  {
    AppDomain.CurrentDomain.AssemblyLoad -= new AssemblyLoadEventHandler(this.OnAssemblyLoad);
    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(this.OnAssemblyResolve);
  }

  private void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
  {
    Action<string> logger = this.logger;
    if (logger == null)
      return;
    logger($"The assembly '{args.LoadedAssembly.FullName}' is loaded from '{args.LoadedAssembly.Location}'");
  }

  private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
  {
    AppAssemblyResolveRequest request = new AppAssemblyResolveRequest(new AssemblyName(args.Name), args.RequestingAssembly);
    IAppAssemblyResolveFilter resolveFilter = AppAssemblyResolver.ResolveFilter;
    if (resolveFilter != null && !resolveFilter.CanResolve(request.AssemblyName))
      return (Assembly) null;
    ++this.resolveHandlerDepth;
    try
    {
      return this.OnAssemblyResolveCore(request);
    }
    finally
    {
      --this.resolveHandlerDepth;
    }
  }

  private Assembly OnAssemblyResolveCore(AppAssemblyResolveRequest request)
  {
    Assembly assembly1;
    if (request.IsStrongNamed)
    {
      Assembly assembly2 = this.DoTryRedirectStrongNamedAssembly(request);
      if ((object) assembly2 == null)
        assembly2 = this.DoTryResolveStrongNamedAssembly(request);
      assembly1 = assembly2;
    }
    else
      assembly1 = this.DoTryResolveSimpleNamedAssembly(request);
    return assembly1;
  }

  protected virtual Assembly DoTryRedirectStrongNamedAssembly(AppAssemblyResolveRequest request)
  {
    return (Assembly) null;
  }

  protected virtual Assembly DoTryResolveStrongNamedAssembly(AppAssemblyResolveRequest request)
  {
    return (Assembly) null;
  }

  protected virtual Assembly DoTryResolveSimpleNamedAssembly(AppAssemblyResolveRequest request)
  {
    return (Assembly) null;
  }

  protected Assembly TryGetLoadedStrongNamedAssembly(AppAssemblyResolveRequest request)
  {
    Assembly[] array = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (x =>
    {
      AssemblyName name = x.GetName();
      return string.Equals(name.Name, request.SimpleName, StringComparison.CurrentCultureIgnoreCase) && ((IEnumerable<byte>) name.GetPublicKeyToken()).SequenceEqual<byte>((IEnumerable<byte>) request.PublicKeyToken);
    })).ToArray<Assembly>();
    if (array.Length == 0)
      return (Assembly) null;
    return array.Length == 1 ? array[0] : throw new Exception($"The assembly '{request.AssemblyName}' should be loaded only once.");
  }

  protected Assembly TryGetLoadedSimpleNamedAssembly(AppAssemblyResolveRequest request)
  {
    Assembly[] array = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (x => string.Equals(x.GetName().Name, request.SimpleName, StringComparison.CurrentCultureIgnoreCase))).ToArray<Assembly>();
    if (array.Length == 0)
      return (Assembly) null;
    return array.Length == 1 ? array[0] : throw new Exception($"The assembly '{request.AssemblyName}' should be loaded only once.");
  }

  protected Assembly DoTryAutoLoadAssembly(AppAssemblyResolveRequest request)
  {
    return AppPluginLoader.Instance is IAppAssemblyAutoLoader instance ? instance.TryAutoLoadAssembly(request) : (Assembly) null;
  }

  public static AppAssemblyResolver Instance
  {
    [DebuggerStepThrough] get
    {
      lock (AppAssemblyResolver.s_instanceSyncRoot)
        return AppAssemblyResolver.s_instance;
    }
    [DebuggerStepThrough] set
    {
      lock (AppAssemblyResolver.s_instanceSyncRoot)
      {
        if (AppAssemblyResolver.s_instance == value)
          return;
        if (AppAssemblyResolver.s_instance != null)
          AppAssemblyResolver.s_instance.Enabled = false;
        AppAssemblyResolver.s_instance = value;
        if (AppAssemblyResolver.s_instance == null)
          return;
        AppAssemblyResolver.s_instance.Enabled = true;
      }
    }
  }

  public static IAppAssemblyResolveFilter ResolveFilter
  {
    [DebuggerStepThrough] get => AppAssemblyResolver.s_resolveFilter;
    [DebuggerStepThrough] set
    {
      Interlocked.Exchange<IAppAssemblyResolveFilter>(ref AppAssemblyResolver.s_resolveFilter, value);
    }
  }
}
