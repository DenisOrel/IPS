// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService.ConfigLoadService
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;

internal sealed class ConfigLoadService
{
  private IpsXmlLogger _logger;
  private Dictionary<NodeType, object> loaders = new Dictionary<NodeType, object>();

  public ConfigLoadService(IServiceProvider services)
  {
    this._logger = services.GetService<IpsXmlLogger>();
    this.InitializeLoaders();
  }

  public BaseConfig LoadConfig(XElement source)
  {
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_load_config"), (object) source.Name));
    object obj1;
    if (!this.loaders.TryGetValue(source.Name.ToString().ParseNodeType(), out obj1))
    {
      this._logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_loader_not_found_for_node"), (object) source.Name));
      return (BaseConfig) null;
    }
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_loader_found"), (object) obj1.GetType().Name));
    MethodInfo methodInfo = ((IEnumerable<MethodInfo>) obj1.GetType().GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (methodType => methodType.GetCustomAttribute<LoadMethodAttribute>() != null)).FirstOrDefault<MethodInfo>();
    if (methodInfo == (MethodInfo) null)
    {
      this._logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_no_load_method_in_loader"), (object) obj1.GetType().Name, (object) source.Name));
      return (BaseConfig) null;
    }
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_invoke_load_method"), (object) methodInfo.Name));
    object obj2 = methodInfo.Invoke(obj1, new object[1]
    {
      (object) source
    });
    if (obj2 is BaseConfig)
    {
      this._logger.Info(LocalizationHolder.rm.GetString("msg_node_config_loaded"));
      return obj2 as BaseConfig;
    }
    this._logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_error_node_config_load"), (object) source.Name));
    return (BaseConfig) null;
  }

  private void InitializeLoaders()
  {
    this._logger.Info(LocalizationHolder.rm.GetString("msg_node_loaders_initialization"));
    ((IEnumerable<Type>) Assembly.GetExecutingAssembly().GetTypes()).ToList<Type>().ForEach((Action<Type>) (type =>
    {
      ConfigLoaderAttribute customAttribute = type.GetCustomAttribute<ConfigLoaderAttribute>();
      if (customAttribute == null)
        return;
      object instance = Activator.CreateInstance(type, (object) this, (object) this._logger);
      if (instance != null)
      {
        this.loaders[customAttribute.NodeType] = instance;
        this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_node_loader_initialized"), (object) type.Name, (object) customAttribute.NodeType.ToXMLTag()));
      }
      else
        this._logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_error_node_loader_initialization"), (object) type.Name));
    }));
    this._logger.Info(LocalizationHolder.rm.GetString("msg_node_loaders_initialization_complete"));
  }
}
