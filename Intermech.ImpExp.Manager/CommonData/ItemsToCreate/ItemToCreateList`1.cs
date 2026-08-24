// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.ItemsToCreate.ItemToCreateList`1
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData.ItemsToCreate;

internal class ItemToCreateList<T> where T : IItemToCreate
{
  private Dictionary<long, T> localIdsDict = new Dictionary<long, T>();
  private Dictionary<string, T> namesDict = new Dictionary<string, T>();
  private Dictionary<Guid, T> guidsDict = new Dictionary<Guid, T>();
  private List<T> itemsList = new List<T>();
  private IItemToCreateSelectDialog selectDialog;

  protected virtual bool add(T item)
  {
    if (!this.addToDictionaries(item))
      return false;
    this.itemsList.Add(item);
    return true;
  }

  protected virtual bool addToDictionaries(T item)
  {
    if (item.Name == string.Empty)
      item.Name = item.GUID.ToString();
    if (this.ExistsByName(item.Name) || this.ExistsByGuid(item.GUID))
      return false;
    string key = item.Name.ToUpper().Trim();
    if (!key.Equals(string.Empty))
      this.namesDict.Add(key, item);
    this.guidsDict.Add(item.GUID, item);
    this.localIdsDict.Add((long) item.LocalID, item);
    return true;
  }

  public IList<T> Items => (IList<T>) this.itemsList;

  public virtual void Clear()
  {
    this.localIdsDict.Clear();
    this.namesDict.Clear();
    this.guidsDict.Clear();
    this.itemsList.Clear();
  }

  public bool ExistsByName(string name) => this.namesDict.ContainsKey(name.ToUpper().Trim());

  public bool ExistsByGuid(Guid guid) => this.guidsDict.ContainsKey(guid);

  public T GetByLocalId(long id)
  {
    return this.localIdsDict.ContainsKey(id) ? this.localIdsDict[id] : default (T);
  }

  public T GetByName(string name)
  {
    string key = name.ToUpper().Trim();
    return this.namesDict.ContainsKey(key) ? this.namesDict[key] : default (T);
  }

  public T GetByGuid(Guid guid) => this.ExistsByGuid(guid) ? this.guidsDict[guid] : default (T);

  public void UpdateCasheName(string oldName)
  {
    if (oldName.Equals(string.Empty))
      return;
    T byName = this.GetByName(oldName);
    this.namesDict.Remove(oldName.ToUpper().Trim());
    if ((object) byName == null)
      return;
    this.namesDict.Add(byName.Name.ToUpper().Trim(), byName);
  }

  public IItemToCreateSelectDialog SelectDialog
  {
    get
    {
      if (this.selectDialog == null)
        this.selectDialog = (IItemToCreateSelectDialog) new ItemToCreateSelectDialog(typeof (T));
      return this.selectDialog;
    }
  }
}
