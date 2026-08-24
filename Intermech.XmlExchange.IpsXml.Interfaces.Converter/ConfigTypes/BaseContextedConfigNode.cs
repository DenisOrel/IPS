// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.BaseContextedConfigNode
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public abstract class BaseContextedConfigNode : BaseConfigNode
{
  private string _context = string.Empty;

  public override void LoadFromXML(XElement configNode)
  {
    base.LoadFromXML(configNode);
    XAttribute xattribute = configNode.Attribute((XName) "context");
    this._context = xattribute != null ? xattribute.Value : string.Empty;
  }

  public string Context => this._context;

  public override string GetUniqueID()
  {
    string uniqueId = base.GetUniqueID();
    return this._context.Equals(string.Empty) ? uniqueId : $"{uniqueId}_{this._context}";
  }
}
