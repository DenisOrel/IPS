// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.ConfigurationEnumerator
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal sealed class ConfigurationEnumerator : IEnumerator<IConfiguration>, IDisposable, IEnumerator
{
  private List<IConfiguration> _list;
  private int _index;

  public ConfigurationEnumerator(XmlNode node)
  {
    this._list = new List<IConfiguration>();
    foreach (XmlNode childNode in node.ChildNodes)
      this._list.Add((IConfiguration) new ConfigurationImpl(childNode));
    this.Reset();
  }

  public IConfiguration Current => this._list[this._index];

  public void Dispose() => this._list.Clear();

  object IEnumerator.Current => (object) this._list[this._index];

  public bool MoveNext()
  {
    ++this._index;
    return this._index < this._list.Count;
  }

  public void Reset() => this._index = -1;
}
