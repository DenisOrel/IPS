// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.ValueConfig
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public class ValueConfig : BaseConfigNode, IEquatable<ValueConfig>
{
  private ConfigFormat.AttrValueDataType _valueDataType;
  private ConvertValueLink _convertValueLink = new ConvertValueLink();
  private string _surroundSymbol = string.Empty;
  private string _attrName = string.Empty;
  private string _attrInternalFieldName = "F_VALUE";
  private bool _base64Encode;

  public override void LoadFromXML(XElement configNode)
  {
    base.LoadFromXML(configNode);
    XAttribute xattribute1 = configNode.Attribute((XName) "attr_name");
    this._attrName = xattribute1 != null ? xattribute1.Value : string.Empty;
    XAttribute xattribute2 = configNode.Attribute((XName) "sur_symb");
    this._surroundSymbol = xattribute2 != null ? xattribute2.Value : string.Empty;
    XAttribute xattribute3 = configNode.Attribute((XName) "vtype");
    this._valueDataType = xattribute3 != null ? ConfigFormat.ParseAttrValueDataType(xattribute3.Value) : ConfigFormat.AttrValueDataType.avdtString;
    XAttribute xattribute4 = configNode.Attribute((XName) "attr_internal_field_name");
    this._attrInternalFieldName = xattribute4 != null ? xattribute4.Value : "F_VALUE";
    XElement configNode1 = configNode.Element((XName) "conv_link");
    if (configNode1 != null)
      this._convertValueLink.LoadFromXML(configNode1);
    XAttribute xattribute5 = configNode.Attribute((XName) "base64encode");
    this._base64Encode = xattribute5 != null && !xattribute5.Value.Equals("false");
  }

  public virtual ConfigFormat.AttrValueType ValueType => ConfigFormat.AttrValueType.avtUnknown;

  public ConfigFormat.AttrValueDataType ValueDataType => this._valueDataType;

  public ConvertValueLink ConvertValueLink => this._convertValueLink;

  public string SurroundSymbol => this._surroundSymbol;

  public string AttrName => this._attrName;

  public string AttrInternalFieldName
  {
    get => this._attrInternalFieldName;
    set => this._attrInternalFieldName = value;
  }

  public bool Base64Encode
  {
    get => this._base64Encode;
    set => this._base64Encode = value;
  }

  public override int GetHashCode()
  {
    return this.Name.GetHashCode() ^ this.AttrName.GetHashCode() ^ this.Description.GetHashCode() ^ this.AttrInternalFieldName.GetHashCode() ^ this.ValueDataType.GetHashCode() ^ this.ValueType.GetHashCode();
  }

  public bool Equals(ValueConfig other)
  {
    return this.Name == other.Name && this.AttrName == other.AttrName && this.Description == other.Description && this.AttrInternalFieldName == other.AttrInternalFieldName && this.ValueDataType == other.ValueDataType && this.ValueType == other.ValueType;
  }
}
