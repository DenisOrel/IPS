// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.PluginsManager
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#nullable disable
namespace Intermech.ImpExp.Manager;

public class PluginsManager : IImpExpPluginsManager
{
  private IAppManager appManager;
  public SortedDictionary<string, Assembly> AssembliesList = new SortedDictionary<string, Assembly>();
  private Type pluginType;
  private string adapterTypeName = string.Empty;

  public List<IPlugin> PluginsList { get; private set; }

  public PluginsManager(IAppManager form)
  {
    this.PluginsList = new List<IPlugin>();
    AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(this.CurrentDomain_AssemblyLoad);
    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.CurrentDomain_AssemblyResolve);
    this.pluginType = typeof (IPlugin);
    this.adapterTypeName = typeof (IDataBaseType).FullName;
    this.appManager = form;
  }

  private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
  {
    return this.AssembliesList.ContainsKey(args.Name) ? this.AssembliesList[args.Name] : (Assembly) null;
  }

  private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
  {
    if (this.AssembliesList.ContainsKey(args.LoadedAssembly.FullName))
      return;
    this.AssembliesList.Add(args.LoadedAssembly.FullName, args.LoadedAssembly);
  }

  public void LoadPlugins(IConfiguration maincfg)
  {
    this.PluginsList.Clear();
    foreach (PluginItem plugin in (ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.Plugins)
    {
      if (plugin.Enable)
        this.loadPluginFile(Intermech.ImpExp.Interface.PathHelper.Normalize(plugin.FileName));
    }
  }

  private void loadPluginFile(string pluginPath)
  {
    FileInfo fileInfo = new FileInfo(pluginPath);
    try
    {
      Assembly assembly = Assembly.LoadFrom(pluginPath);
      this.appManager.AddInfoMessage("Загружен файл расширения : " + fileInfo.Name);
      if (!(assembly != (Assembly) null))
        return;
      foreach (Type type in assembly.GetTypes())
      {
        try
        {
          if (type.IsClass && !type.IsAbstract && this.pluginType.IsAssignableFrom(type))
            this.loadPlugin(type);
          if (type.IsClass)
          {
            if (type.GetInterface(this.adapterTypeName) != (Type) null)
              this.loadAdapter(type);
          }
        }
        catch (Exception ex)
        {
          this.appManager.AddErrorMessage($"Ошибка при загрузке модуля расширения {fileInfo.Name} :{ExceptionLogger.GetExceptionInfo(ex)}");
        }
      }
    }
    catch (Exception ex)
    {
      this.appManager.AddErrorMessage($"Ошибка загрузки файла расширения {fileInfo.Name} :{ExceptionLogger.GetExceptionInfo(ex)}");
    }
  }

  private void loadPlugin(Type pluginType)
  {
    IPlugin instance = (IPlugin) Activator.CreateInstance(pluginType, (object) this.appManager);
    this.PluginsList.Add(instance);
    if (instance is IConfigurable)
      (instance as IConfigurable).LoadConfiguration();
    this.appManager.AddInfoMessage("Загружен модуль расширения: " + instance.Name);
  }

  private void loadAdapter(Type adapterType)
  {
    IDataBaseType instance = (IDataBaseType) Activator.CreateInstance(adapterType);
    this.appManager.DBManager.RegisterDbType(instance);
    this.appManager.AddInfoMessage("Загружен модуль подключения к БД: " + instance.DataBaseType());
  }
}
