// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes.FixedFuncValueConfig
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;

public class FixedFuncValueConfig : ValueConfig
{
  private ConfigFormat.FixedFuncType _funcType = ConfigFormat.FixedFuncType.fftCurDate;

  public override void LoadFromXML(XElement configNode)
  {
    base.LoadFromXML(configNode);
    this._funcType = ConfigFormat.ParseFixedFuncType(configNode.Attribute((XName) "name").Value);
  }

  public override ConfigFormat.AttrValueType ValueType => ConfigFormat.AttrValueType.avtFixedFunc;

  public ConfigFormat.FixedFuncType FuncType => this._funcType;
}
