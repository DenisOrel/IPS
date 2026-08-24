// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Ips.IpsXmlParam
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Ips, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: BB701E43-1D04-4071-82FB-E63B4898E0B4
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Ips.dll

using Intermech.Interfaces;
using Intermech.IpsXmlViewer.Interfaces;
using System;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Ips;

public class IpsXmlParam : IXmlParam, IXmlEntity
{
  private string _dictID;
  private object sourceAttr;

  public IpsXmlParam(string dictID, object attribute)
  {
    this._dictID = dictID;
    this.sourceAttr = attribute;
  }

  public string Id
  {
    get
    {
      return this.sourceAttr is IImAttribute ? (this.sourceAttr as IImAttribute).F_ATTRIBUTE_ID : this._dictID;
    }
  }

  public string Name
  {
    get
    {
      if (!(this.sourceAttr is IImAttribute))
        return this._dictID;
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType((this.sourceAttr as IImAttribute).GetAsInt32("F_ATTRIBUTE_ID", 0));
      return attributeType == null ? string.Empty : attributeType.Name;
    }
  }

  public string Value
  {
    get
    {
      return !(this.sourceAttr is IImAttribute) ? Convert.ToString(this.sourceAttr) : (this.sourceAttr as IImAttribute).ToString();
    }
  }

  public string Description => $"{this.Name}={this.Value}";
}
