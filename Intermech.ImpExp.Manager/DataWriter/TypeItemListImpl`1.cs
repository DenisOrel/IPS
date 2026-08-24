// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.TypeItemListImpl`1
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal class TypeItemListImpl<T> : 
  List<T>,
  ITypeItemList<T>,
  IList<T>,
  ICollection<T>,
  IEnumerable<T>,
  IEnumerable,
  IList,
  ICollection
{
  protected IDataWriterProxy dataWriter;
  protected Dictionary<int, T> dictionaryId = new Dictionary<int, T>();
  protected Dictionary<Guid, T> dictionaryGuid = new Dictionary<Guid, T>();
  protected Dictionary<string, T> dictionaryName = new Dictionary<string, T>();
  protected string tableName = string.Empty;
  private object nullObject;
  private int nextId = -1;

  protected int NextID
  {
    get
    {
      if (this.nextId == -1)
        this.nextId = Convert.ToInt32(this.dataWriter.GetNextID(this.tableName));
      return this.nextId;
    }
  }

  public TypeItemListImpl(IDataWriterProxy dataWriter, string table)
  {
    this.dataWriter = dataWriter;
    this.tableName = table;
  }

  protected virtual bool addToHTs(T item)
  {
    if (!(item is ITypeItem typeItem))
      return false;
    string str = typeItem.Name.ToUpper().Trim();
    if (this.ExistsByGuid(typeItem.GUID) || this.ExistsById(typeItem.ID) || this.ExistsByName(str))
      return false;
    this.dictionaryId.Add(typeItem.ID, item);
    this.dictionaryGuid.Add(typeItem.GUID, item);
    this.dictionaryName.Add(str, item);
    return true;
  }

  protected void removeFromHTs(T item)
  {
    if (!(item is ITypeItem typeItem))
      return;
    this.dictionaryId.Remove(typeItem.ID);
    this.dictionaryGuid.Remove(typeItem.GUID);
    this.dictionaryName.Remove(typeItem.Name.ToUpper());
  }

  public new virtual void Add(T item)
  {
    if (!this.addToHTs(item))
      return;
    base.Add(item);
  }

  public new virtual void AddRange(IEnumerable<T> collection)
  {
    foreach (T obj in collection)
      this.Add(obj);
  }

  public new virtual void Insert(int index, T item)
  {
    if (!this.addToHTs(item))
      return;
    base.Insert(index, item);
  }

  public new virtual void InsertRange(int index, IEnumerable<T> collection)
  {
    int num = index;
    foreach (T obj in collection)
      base.Insert(num++, obj);
  }

  public new virtual void Clear()
  {
    this.dictionaryId.Clear();
    this.dictionaryGuid.Clear();
    this.dictionaryName.Clear();
    base.Clear();
  }

  public virtual void Remove(T item)
  {
    this.removeFromHTs(item);
    base.Remove(item);
  }

  public new virtual int RemoveAll(Predicate<T> match)
  {
    int num = 0;
    if (match == null)
      throw new ArgumentException("Argument is null");
    for (int index = this.Count - 1; index >= 0; --index)
    {
      if (match(this[index]))
      {
        this.RemoveAt(index);
        ++num;
      }
    }
    return num;
  }

  public new virtual void RemoveAt(int index)
  {
    this.removeFromHTs(this[index]);
    base.RemoveAt(index);
  }

  public new virtual void RemoveRange(int index, int count)
  {
    for (int index1 = index; index1 < count; ++index1)
      this.RemoveAt(index1);
  }

  public T GetByID(int id)
  {
    return this.dictionaryId.ContainsKey(id) ? this.dictionaryId[id] : (T) this.nullObject;
  }

  public T GetByGuid(Guid guid)
  {
    return this.dictionaryGuid.ContainsKey(guid) ? this.dictionaryGuid[guid] : (T) this.nullObject;
  }

  public T GetByName(string name)
  {
    string key = name.ToUpper().Trim();
    return this.dictionaryName.ContainsKey(key) ? this.dictionaryName[key] : (T) this.nullObject;
  }

  public T[] GetItems() => this.ToArray();

  public int GenNextID()
  {
    int nextId = this.NextID;
    ++this.nextId;
    return nextId;
  }

  public bool ExistsById(int id) => this.dictionaryId.ContainsKey(id);

  public bool ExistsByGuid(Guid guid) => this.dictionaryGuid.ContainsKey(guid);

  public bool ExistsByName(string name) => this.dictionaryName.ContainsKey(name.ToUpper().Trim());
}
