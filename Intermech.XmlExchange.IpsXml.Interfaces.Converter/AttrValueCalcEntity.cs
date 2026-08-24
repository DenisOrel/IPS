// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Converter.AttrValueCalcEntity
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9BB5546-0F4A-4E7E-A111-8011765E8C48
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Converter.dll

using Intermech.IpsXmlViewer.Interfaces;
using Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Converter;

public class AttrValueCalcEntity
{
  private AttrConfig _targetAttrConfig;
  private XElement _attrOwnerNode;
  private IImObject _sourceIMObject;
  private IImRelation _sourceIMRelation;
  private List<ValueConfig> _valueConfigs = new List<ValueConfig>();
  private List<string> _attrValues = new List<string>();

  public AttrValueCalcEntity(
    AttrConfig targetAttrConfig,
    XElement attrOwnerNode,
    IImObject sourceIMObject,
    IImRelation sourceIMRelation)
  {
    this._targetAttrConfig = targetAttrConfig;
    this._attrOwnerNode = attrOwnerNode;
    this._sourceIMObject = sourceIMObject;
    this._sourceIMRelation = sourceIMRelation;
  }

  public AttrConfig TargetAttrConfig => this._targetAttrConfig;

  public IList<ValueConfig> ValueConfigs => (IList<ValueConfig>) this._valueConfigs;

  public XElement AttrOwnerNode => this._attrOwnerNode;

  public IImObject SourceIMObject => this._sourceIMObject;

  public IImRelation SourceIMRelation => this._sourceIMRelation;

  public List<string> AttrValue
  {
    get => this._attrValues;
    set => this._attrValues = value;
  }

  public void sortGrouppedValuesConfigs()
  {
    this._valueConfigs.Sort((Comparison<ValueConfig>) ((_left, _right) => _left is LocalValueConfig && _right is LocalValueConfig ? (_left as LocalValueConfig).OrderInGroup - (_right as LocalValueConfig).OrderInGroup : 0));
  }
}
