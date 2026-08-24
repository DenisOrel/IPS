// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.BaseConfigLoader`1
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders;

internal abstract class BaseConfigLoader<ConfigClass> where ConfigClass : BaseConfig, new()
{
  private IpsXmlLogger _logger;
  private ConfigLoadService _loadersService;

  public BaseConfigLoader(ConfigLoadService loadersService, IpsXmlLogger logger)
  {
    this._logger = logger;
    this._loadersService = loadersService;
  }

  [LoadMethod]
  public ConfigClass Load(XElement source)
  {
    ConfigClass target = new ConfigClass();
    this.LoadBaseParams(target, source);
    this.OnLoadAddParams(target, source);
    return target;
  }

  protected IpsXmlLogger Logger => this._logger;

  protected ConfigLoadService LoadersService => this._loadersService;

  protected abstract void OnLoadAddParams(ConfigClass target, XElement source);

  protected string GetAttrValue(XElement source, AttrType attrType)
  {
    XAttribute xattribute = source.Attribute((XName) attrType.ToXMLTag());
    return xattribute == null ? string.Empty : xattribute.Value;
  }

  private void LoadBaseParams(ConfigClass target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_base_config_params"));
    target.Id = this.GetAttrValue(source, AttrType.Id);
    target.Name = this.GetAttrValue(source, AttrType.Name);
    target.Description = this.GetAttrValue(source, AttrType.Description);
    target.Value = this.GetAttrValue(source, AttrType.Value);
    int result;
    if (int.TryParse(this.GetAttrValue(source, AttrType.Order), out result))
      target.Order = result;
    else
      target.Order = -1;
    if (string.IsNullOrEmpty(target.Id))
      this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_warn_no_id_found_in_config"), (object) source.Name, (object) source.Parent.Name));
    this.Logger.Info(string.Format(LocalizationHolder.rm.GetString("msg_load_base_config_params_complete"), (object) target.Id, (object) target.Name));
  }
}
