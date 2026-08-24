// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig.IdPartGroup
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 675F82E7-A3E4-4C10-BC83-A1D6F7097D09
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.Attributes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes.IdConfig;

[ConfigNodeType(NodeType.IdPartGroup)]
public class IdPartGroup : BaseIdPart
{
  private List<BaseIdPart> _content = new List<BaseIdPart>();

  public List<BaseIdPart> Content => this._content;

  public ConditionType Condition { get; set; }
}
