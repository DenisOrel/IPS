// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.Hasher
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.ImpExp.Workflow;

internal class Hasher
{
  private HasherList _list = new HasherList();
  private string _name = "hasher";

  public Hasher(string name)
  {
    this._name = name;
    this.Load();
  }

  private string FileName => $"c:\\{this._name}.dat";

  public int Load()
  {
    this._list.Clear();
    if (!File.Exists(this.FileName))
      return 0;
    FileStream serializationStream = new FileStream(this.FileName, FileMode.Open);
    try
    {
      this._list = new BinaryFormatter().Deserialize((Stream) serializationStream) as HasherList;
      return this._list.Count;
    }
    finally
    {
      serializationStream.Close();
    }
  }

  public void Save()
  {
    FileStream serializationStream = new FileStream(this.FileName, FileMode.Create);
    try
    {
      new BinaryFormatter().Serialize((Stream) serializationStream, (object) this._list);
    }
    finally
    {
      serializationStream.Close();
    }
  }

  public long Find(ref string s)
  {
    long num = 0;
    this._list.TryGetValue(s, out num);
    return num;
  }

  public void Add(ref string s, long id)
  {
    this._list.Add(s, id);
    this.Save();
  }

  public void RemoveNonExistentObjects(IUserSession session)
  {
    HasherList hasherList = new HasherList();
    foreach (KeyValuePair<string, long> keyValuePair in (Dictionary<string, long>) this._list)
    {
      if (session.GetObject(keyValuePair.Value, false) != null)
        hasherList.Add(keyValuePair.Key, keyValuePair.Value);
    }
    this._list = hasherList;
  }
}
