// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ParamValueConfigLoaders.ValueConfigLoader
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.ParamValueConfig;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.LoadService;
using Intermech.XmlExchange.IpsXml.Interfaces.Logger;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Load.Loaders.ParamValueConfigLoaders;

[ConfigLoader(NodeType.ValueConfig)]
internal class ValueConfigLoader(ConfigLoadService loadersService, IpsXmlLogger logger) : 
  BaseConfigLoader<ValueConfig>(loadersService, logger)
{
  protected override void OnLoadAddParams(ValueConfig target, XElement source)
  {
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_value_config"));
    target.AttrId = this.GetAttrValue(source, AttrType.AttrId);
    target.LinkedValueId = this.GetAttrValue(source, AttrType.LinkedValueId);
    target.Destination = this.GetAttrValue(source, AttrType.Destination).ParseValueDestType();
    target.DestFieldName = this.GetAttrValue(source, AttrType.FieldName);
    XAttribute xattribute = source.Attribute((XName) AttrType.Export.ToXMLTag());
    target.Export = xattribute == null || xattribute.Value == "1";
    target.SurrSymbol = this.GetAttrValue(source, AttrType.SurrSymbol);
    target.GroupId = this.GetAttrValue(source, AttrType.GroupId);
    target.Delimiter = this.GetAttrValue(source, AttrType.Delimiter);
    target.GroupCond = this.GetAttrValue(source, AttrType.Condition).ParseConditionType();
    XElement source1 = source.Element((XName) NodeType.ValueConverterReference.ToXMLTag());
    if (source1 != null)
      target.ConverterReference = this.LoadersService.LoadConfig(source1) as ValueConverterReference;
    if (target.ConverterReference == null)
      target.ConverterReference = new ValueConverterReference();
    this.Logger.Info(LocalizationHolder.rm.GetString("msg_load_value_config_complete"));
  }
}
