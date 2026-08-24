// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.OutPutFileInfo
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public class OutPutFileInfo : BaseConfigNode
{
  private string _version = string.Empty;
  private string _encoding = string.Empty;
  private string _fileName = string.Empty;

  public override void LoadFromXML(XElement configNode)
  {
    base.LoadFromXML(configNode);
    this._fileName = this.Name;
    XAttribute xattribute1 = configNode.Attribute((XName) "encoding");
    this._encoding = xattribute1 != null ? xattribute1.Value : string.Empty;
    XAttribute xattribute2 = configNode.Attribute((XName) "version");
    this._version = xattribute2 != null ? xattribute2.Value : string.Empty;
  }

  public string Encoding => this._encoding;

  public string Version => this._version;

  public string FileName
  {
    get => this._fileName;
    set => this._fileName = value;
  }
}
