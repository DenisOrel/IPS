// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.Format.NodeTypeExtention
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;

public static class NodeTypeExtention
{
  private static Dictionary<NodeType, string> _EnumToString = new Dictionary<NodeType, string>();
  private static Dictionary<string, NodeType> _StringToEnum = new Dictionary<string, NodeType>();

  static NodeTypeExtention()
  {
    TypesExtensions.FillEnumValues<NodeType>(NodeTypeExtention._EnumToString, NodeTypeExtention._StringToEnum);
  }

  public static string ToTechXMLTag(this NodeType source)
  {
    string str;
    return NodeTypeExtention._EnumToString.TryGetValue(source, out str) ? str : string.Empty;
  }

  public static NodeType StringToEnum(string source)
  {
    NodeType nodeType;
    return NodeTypeExtention._StringToEnum.TryGetValue(source, out nodeType) ? nodeType : NodeType.Unknown;
  }

  public static bool IsTechObj(this NodeType target)
  {
    if ((uint) (target - 2) > 32U /*0x20*/)
    {
      switch (target)
      {
        case NodeType.RefDoc:
        case NodeType.EcoDoc:
        case NodeType.OldComment:
        case NodeType.Sketch:
        case NodeType.MBOM:
        case NodeType.PROC:
        case NodeType.WorkShowEnter:
          break;
        default:
          return false;
      }
    }
    return true;
  }
}
