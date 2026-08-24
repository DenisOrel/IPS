// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Interfaces.Ips.IpsXmlParams
// Assembly: Intermech.XmlExchange.IpsXml.Interfaces.Ips, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: BB701E43-1D04-4071-82FB-E63B4898E0B4
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Interfaces.Ips.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Interfaces.Ips;

public class IpsXmlParams : 
  IXmlParams,
  IReadOnlyList<IXmlParam>,
  IReadOnlyCollection<IXmlParam>,
  IEnumerable<IXmlParam>,
  IEnumerable
{
  private IList<IXmlParam> _parmList = (IList<IXmlParam>) new List<IXmlParam>();

  public IEnumerator<IXmlParam> GetEnumerator() => this._parmList.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._parmList.GetEnumerator();

  public IXmlParam this[int index] => this._parmList[index];

  public int Count => this._parmList.Count;

  public void AddParam(IXmlParam param) => this._parmList.Add(param);

  public void Clear() => this._parmList.Clear();
}
