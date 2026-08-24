// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Provider.Techcard.Params.TechXmlParams
// Assembly: Intermech.XmlExchange.IpsXml.Provider.Techcard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6433FBE8-382D-4C90-9782-A3F865DC9A28
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Provider.Techcard.dll

using Intermech.XmlExchange.IpsXml.Interfaces;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Provider.Techcard.Params;

public class TechXmlParams : 
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
