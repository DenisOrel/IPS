// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.SerializationService.ConfigSerializationService
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.Attributes;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Serialization.SerializationService;

internal sealed class ConfigSerializationService
{
  private IpsXmlLogger _logger;
  private Dictionary<NodeType, object> _serializers = new Dictionary<NodeType, object>();

  public ConfigSerializationService(IServiceProvider services)
  {
    this._logger = services.GetService<IpsXmlLogger>();
    this.InitializeSerializers();
  }

  public void Serialize(BaseConfig config, XElement parentConfigNode)
  {
    this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_serialize_config"), (object) config.Id));
    object obj;
    if (!this._serializers.TryGetValue(config.ToNodeType(), out obj))
    {
      this._logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_serializer_not_found_for_config"), (object) config.Id));
    }
    else
    {
      this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_serializer_found"), (object) obj.GetType().Name));
      MethodInfo methodInfo = ((IEnumerable<MethodInfo>) obj.GetType().GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (methodType => methodType.GetCustomAttribute<SerializeMethodAttribute>() != null)).FirstOrDefault<MethodInfo>();
      if (methodInfo == (MethodInfo) null)
      {
        this._logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_no_serialize_method_in_serializer"), (object) obj.GetType().Name));
      }
      else
      {
        this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_invoke_serialize_method"), (object) methodInfo.Name));
        if (methodInfo.Invoke(obj, new object[2]
        {
          (object) config,
          (object) parentConfigNode
        }) is BaseConfig)
          this._logger.Info(LocalizationHolder.rm.GetString("msg_config_serialized"));
        else
          this._logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_error_config_serialization"), (object) config.Id));
      }
    }
  }

  private void InitializeSerializers()
  {
    this._logger.Info(LocalizationHolder.rm.GetString("msg_config_serializers_initialization"));
    ((IEnumerable<Type>) Assembly.GetExecutingAssembly().GetTypes()).ToList<Type>().ForEach((Action<Type>) (type =>
    {
      ConfigSerializerAttribute customAttribute = type.GetCustomAttribute<ConfigSerializerAttribute>();
      if (customAttribute == null)
        return;
      object instance = Activator.CreateInstance(type, (object) this, (object) this._logger);
      if (instance != null)
      {
        this._serializers[customAttribute.NodeType] = instance;
        this._logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_config_serializer_initialized"), (object) type.Name, (object) customAttribute.NodeType.ToXMLTag()));
      }
      else
        this._logger.Error(string.Format(LocalizationHolder.rm.GetString("msg_config_config_serializer_initialization"), (object) type.Name));
    }));
    this._logger.Info(LocalizationHolder.rm.GetString("msg_config_serializers_initialization_complete"));
  }
}
