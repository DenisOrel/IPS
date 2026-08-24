// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.IdPartGroupLoader
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders;

[ConfigLoader(NodeType.IdPartGroup)]
internal class IdPartGroupLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConfigLoader<IdPartGroup>(loadersService, logger)
{
  protected override void OnLoadAddParams(IdPartGroup target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_id_group_config"));
    XAttribute xattribute = source.Attribute((XName) AttrType.Condition.ToXMLTag());
    if (xattribute != null && !string.IsNullOrEmpty(xattribute.Value))
      target.Condition = xattribute.Value.ParseConditionType();
    foreach (XElement element in source.Elements())
    {
      BaseConfig baseConfig = this.LoadersService.LoadConfig(element);
      if (baseConfig is BaseIdPart)
        target.Content.Add(baseConfig as BaseIdPart);
    }
    if (target.Content.Count == 0)
      this.Logger.Warn(string.Format(LocalizationHolder.rm.GetString("msg_no_childs_for_parent_found_id_config_group"), (object) target.Id, (object) target.Name, (object) NodeType.IdPart.ToXMLTag(), (object) NodeType.IdPartGroup.ToXMLTag()));
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_id_group_config_complete"));
  }
}
