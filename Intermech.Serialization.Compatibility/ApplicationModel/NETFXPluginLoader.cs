// Decompiled with JetBrains decompiler
// Type: Intermech.ApplicationModel.NETFXPluginLoader
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Intermech.ApplicationModel;

public class NETFXPluginLoader : AppPluginLoader, IAppAssemblyAutoLoader
{
  private List<NETFXPluginContext> pluginContexts;
  private bool autoLoadMode;

  public NETFXPluginLoader() => this.pluginContexts = new List<NETFXPluginContext>(32 /*0x20*/);

  private NETFXPluginContext GetOrAddPluginContext(string pluginFile)
  {
    lock (this.pluginContexts)
    {
      foreach (NETFXPluginContext pluginContext in this.pluginContexts)
      {
        if (this.IsPathEquals(pluginContext.PluginFile, pluginFile))
          return pluginContext;
      }
      NETFXPluginContext addPluginContext = new NETFXPluginContext(pluginFile, this.GetPluginDirectory(pluginFile));
      this.pluginContexts.Add(addPluginContext);
      return addPluginContext;
    }
  }

  private NETFXPluginContext FindPluginContext(Func<NETFXPluginContext, bool> match)
  {
    lock (this.pluginContexts)
      return this.pluginContexts.FirstOrDefault<NETFXPluginContext>(match);
  }

  protected override Assembly DoLoadPlugin(string pluginFile)
  {
    return this.LoadAssemblyFromPath(this.GetOrAddPluginContext(pluginFile), pluginFile);
  }

  private Assembly LoadAssemblyFromPath(NETFXPluginContext pluginContext, string assemblyFile)
  {
    return this.IsPathEquals(pluginContext.PluginDirectory, this.BaseDirectory) ? Assembly.Load(AssemblyName.GetAssemblyName(assemblyFile)) : Assembly.LoadFrom(assemblyFile);
  }

  private string IsPluginAssembly(AssemblyName assemblyName, bool basePluginsMode)
  {
    if (basePluginsMode)
      return (string) null;
    return this.FindPluginContext((Func<NETFXPluginContext, bool>) (x => !this.IsPathEquals(x.PluginDirectory, this.BaseDirectory) && x.TryResolve(assemblyName) != null))?.Id;
  }

  private Assembly LoadPluginAssembly(AssemblyName assemblyName, string pluginId)
  {
    NETFXPluginContext pluginContext = this.FindPluginContext((Func<NETFXPluginContext, bool>) (x => x.Id == pluginId));
    if (pluginContext != null)
    {
      string assemblyFile = pluginContext.TryResolve(assemblyName);
      if (assemblyFile != null)
        return this.LoadAssemblyFromPath(pluginContext, assemblyFile);
    }
    throw new FileLoadException($"Could not load the assembly '{assemblyName}' in the context of plugin '{pluginId}'.");
  }

  Assembly IAppAssemblyAutoLoader.TryAutoLoadAssembly(AppAssemblyResolveRequest request)
  {
    if (this.autoLoadMode)
      return (Assembly) null;
    this.autoLoadMode = true;
    try
    {
      return this.TryAutoLoadAssemblyInternal(request);
    }
    finally
    {
      this.autoLoadMode = false;
    }
  }

  private Assembly TryAutoLoadAssemblyInternal(AppAssemblyResolveRequest request)
  {
    string str = Path.Combine(this.BaseDirectory, request.SimpleName + ".dll");
    if (File.Exists(str))
      return Assembly.Load(AssemblyName.GetAssemblyName(str));
    string pluginId = this.IsPluginAssembly(request.AssemblyName, false);
    return pluginId != null ? this.LoadPluginAssembly(request.AssemblyName, pluginId) : (Assembly) null;
  }
}
