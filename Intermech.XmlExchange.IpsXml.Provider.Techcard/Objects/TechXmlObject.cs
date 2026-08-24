// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.Objects.TechXmlObject
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using Intermech.XmlExchange.IpsXml.Interfaces;
using Intermech.XmlExchange.IpsXml.Provider.Techcard.Format;
using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard.Objects;

public class TechXmlObject : BaseTechXmlObject, IXmlObject, IXmlDataEntity, IXmlEntity
{
  public bool IsHead { get; set; }

  public override string Description
  {
    get
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string str;
      if (this.NodeType == NodeType.Art)
      {
        IXmlParam xmlParam1 = (IXmlParam) null;
        IXmlParam xmlParam2 = (IXmlParam) null;
        foreach (IXmlParam xmlParam3 in (IEnumerable<IXmlParam>) this.XmlParams)
        {
          if (string.Compare(xmlParam3.Name, "Обозначение", true) == 0)
            xmlParam1 = xmlParam3;
          else if (string.Compare(xmlParam3.Name, "Наименование", true) == 0)
            xmlParam2 = xmlParam3;
          if (xmlParam1 != null)
          {
            if (xmlParam2 != null)
              break;
          }
        }
        if (xmlParam1 != null)
          empty2 = xmlParam1.Value;
        if (xmlParam2 != null)
          empty3 = xmlParam2.Value;
        str = !string.IsNullOrEmpty(empty2) ? $"\"{empty2}\" {empty3}" : empty3;
      }
      else
        str = "unknown";
      return $"{base.Description}: {str}";
    }
  }
}
