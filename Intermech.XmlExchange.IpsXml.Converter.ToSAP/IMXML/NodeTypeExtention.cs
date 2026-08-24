// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.ToSAP.IMXML.NodeTypeExtention
// Assembly: Intermech.XmlExchange.IpsXml.Converter.ToSAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946972C6-4ABC-4C4A-94A5-3ADC51FD9A58
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.ToSAP.dll

using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.ToSAP.IMXML;

public static class NodeTypeExtention
{
  public static string ToIMXMLTag(this IMXMLFormat.NodeType target)
  {
    return EnumTypeHelper.GetCaption((Enum) target);
  }
}
