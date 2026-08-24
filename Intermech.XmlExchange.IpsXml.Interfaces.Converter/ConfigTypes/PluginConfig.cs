// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.PluginConfig
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public class PluginConfig
{
  public OutPutFileInfo OutPutFileInfo { get; } = new OutPutFileInfo();

  public NodeConfigs NodeConfigs { get; } = new NodeConfigs();

  public ValueConverters ValueConverters { get; } = new ValueConverters();

  public LoggerConfig LoggerConfig { get; } = new LoggerConfig();

  public void LoadConfig(string fileName) => this.InternalLoadConfig(XDocument.Load(fileName));

  protected virtual void InternalLoadConfig(XDocument doc)
  {
    XElement configNode1 = doc.Root.Element((XName) "output_file_info");
    if (configNode1 != null)
      this.OutPutFileInfo.LoadFromXML(configNode1);
    XElement configNode2 = doc.Root.Element((XName) "nodes");
    if (configNode2 != null)
      this.NodeConfigs.LoadFromXML(configNode2);
    XElement configNode3 = doc.Root.Element((XName) "convertions");
    if (configNode3 != null)
      this.ValueConverters.LoadFromXML(configNode3);
    XElement configNode4 = doc.Root.Element((XName) "logger_config");
    if (configNode4 == null)
      return;
    this.LoggerConfig.LoadFromXML(configNode4);
  }
}
