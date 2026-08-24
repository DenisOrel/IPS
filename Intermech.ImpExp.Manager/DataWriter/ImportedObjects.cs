// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ImportedObjects
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal class ImportedObjects : IImportedObjects
{
  private readonly IImportingData _cacheData;
  private readonly IImportingData _ids;
  private readonly IImportingData _types;

  public ImportedObjects()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    this._cacheData = service.GetCache(ImportingCategory.ObjectGUIDs);
    this._ids = service.GetCache(ImportingCategory.IdGuids);
    this._types = service.GetCache(ImportingCategory.ObjectTypes);
  }

  public void AddValue(long objectID, long id, int objectTypeID, Guid objectGuid, Guid guid)
  {
    this._cacheData.AddValue(ImportingCategory.ObjectGUIDs, (object) objectID, id, objectGuid.ToString(), (ITagImportObject) new ObjectInfo(objectTypeID));
    if (this._ids.GetValue(ImportingCategory.IdGuids, (object) id) == null)
      this._ids.AddValue(ImportingCategory.IdGuids, (object) id, 0L, guid.ToString());
    if (this._types.GetNewKey(ImportingCategory.ObjectTypes, (object) id) != 0L)
      return;
    this._types.AddValue(ImportingCategory.ObjectTypes, (object) id, (long) objectTypeID);
  }

  public long GetID(long objectID)
  {
    return this._cacheData.GetNewKey(ImportingCategory.ObjectGUIDs, (object) objectID);
  }

  public DictionaryValue GetInfo(long objectID)
  {
    return this._cacheData.GetValue(ImportingCategory.ObjectGUIDs, (object) objectID);
  }

  public Guid GetObjectGUID(long objectID)
  {
    string caption = this._cacheData.GetCaption(ImportingCategory.ObjectGUIDs, (object) objectID);
    return !(caption != string.Empty) || !GuidHelper.IsGuid(caption) ? Guid.Empty : new Guid(caption);
  }

  public int GetObjectTypeID(long objectID)
  {
    return !(this._cacheData.GetTag(ImportingCategory.ObjectGUIDs, (object) objectID) is ObjectInfo tag) ? -1 : tag.ObjectType;
  }

  public int GetObjectTypeIDForID(long id)
  {
    long newKey = this._types.GetNewKey(ImportingCategory.ObjectTypes, (object) id);
    return newKey == 0L ? -1 : Convert.ToInt32(newKey);
  }

  public Guid GetGUID(long objectID)
  {
    long id = this.GetID(objectID);
    if (id == 0L)
      return Guid.Empty;
    string caption = this._ids.GetCaption(ImportingCategory.IdGuids, (object) id);
    return !(caption != string.Empty) || !GuidHelper.IsGuid(caption) ? Guid.Empty : new Guid(caption);
  }

  public System.Collections.Generic.Dictionary<object, DictionaryValue> Dictionary
  {
    get => this._cacheData.GetCategory(ImportingCategory.ObjectGUIDs);
  }
}
