// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.LoggerConfig
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public class LoggerConfig : BaseConfigNode
{
  public const string DefaultLogFileName = "convert.log";

  public override void LoadFromXML(XElement configNode)
  {
    base.LoadFromXML(configNode);
    XAttribute xattribute1 = configNode.Attribute((XName) "errors");
    this.Errors = xattribute1 != null && xattribute1.Value.Equals("true");
    XAttribute xattribute2 = configNode.Attribute((XName) "warnings");
    this.Warnings = xattribute2 != null && xattribute2.Value.Equals("true");
    XAttribute xattribute3 = configNode.Attribute((XName) "infos");
    this.Infos = xattribute3 != null && xattribute3.Value.Equals("true");
  }

  public LoggerConfig()
  {
    this.Infos = true;
    this.Warnings = true;
    this.Errors = true;
  }

  public bool Infos { get; set; }

  public bool Warnings { get; set; }

  public bool Errors { get; set; }
}
