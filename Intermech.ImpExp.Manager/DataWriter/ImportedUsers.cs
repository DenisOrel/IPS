// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ImportedUsers
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class ImportedUsers : IImportedUsers
{
  private IImportingData _cacheData;

  public ImportedUsers()
  {
    this._cacheData = (ServicesManager.GetService(typeof (ICache)) as ICache).GetCache(ImportingCategory.Users);
  }

  public void Close()
  {
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      return;
    ImportingCategory[] importingCategoryArray = new ImportingCategory[1]
    {
      ImportingCategory.Users
    };
    service.ReleaseCache(importingCategoryArray);
  }

  public Guid GetGUID(int oldID)
  {
    return !(this._cacheData.GetTag(ImportingCategory.Users, (object) oldID) is UserTag tag) ? Guid.Empty : tag.Guid;
  }

  public string GetUserName(int oldID)
  {
    return this._cacheData.GetCaption(ImportingCategory.Users, (object) oldID);
  }

  public void AddValue(int oldID, long objectID, string caption, Guid objectGuid)
  {
    this._cacheData.AddValue(ImportingCategory.Users, (object) oldID, objectID, caption, (ITagImportObject) new UserTag(objectGuid));
  }

  public long GetNewKey(int oldID)
  {
    return this._cacheData.GetNewKey(ImportingCategory.Users, (object) oldID);
  }

  public Dictionary<object, DictionaryValue> Category
  {
    get => this._cacheData.GetCategory(ImportingCategory.Users);
  }

  public DictionaryValue GetValue(int oldID) => this._cacheData.GetValue((object) oldID);
}
