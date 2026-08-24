// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Common.PortalImportedObjectCache`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Common;

public abstract class PortalImportedObjectCache<T> : IPortalImportedObjectCache<T> where T : PortalImportedObject
{
  private readonly Dictionary<string, T> _cache = new Dictionary<string, T>();

  protected abstract string GetQueryToPortalImportData();

  protected abstract IDataBase GetDbConnection();

  protected abstract T CreateImportedData();

  protected abstract bool FillImportedData(T target, IDataReader dataReader);

  public abstract string GetUniqueObjId(T target);

  public abstract string GetUniqueObjId(params object[] idParams);

  public IReadOnlyCollection<string> Ids => (IReadOnlyCollection<string>) this._cache.Keys;

  public IReadOnlyCollection<T> Objects => (IReadOnlyCollection<T>) this._cache.Values;

  public abstract Guid ObjectType { get; }

  public T FindObjectInCache(string uniqueId)
  {
    T obj;
    return this._cache.TryGetValue(uniqueId, out obj) ? obj : default (T);
  }

  public T this[string uniqueId] => this.FindObjectInCache(uniqueId);

  public virtual void Load()
  {
    this._cache.Clear();
    string portalImportData = this.GetQueryToPortalImportData();
    try
    {
      IDataBase dbConnection = this.GetDbConnection();
      if (dbConnection == null)
        return;
      using (IDbCommand command = dbConnection.CreateCommand())
      {
        command.CommandText = portalImportData;
        using (IDataReader dataReader = command.ExecuteReader())
        {
          while (dataReader.Read())
          {
            T importedData = this.CreateImportedData();
            if (this.FillImportedData(importedData, dataReader))
            {
              string uniqueObjId = this.GetUniqueObjId(importedData);
              if (this._cache.TryGetValue(uniqueObjId, out T _))
                TechcardConsts.Plugin.appManager.AddWarningMessage($"Чтение импортированных через портал данных: Cache: {this.GetType()} - обнаружен дубликат для записи Id = {uniqueObjId}");
              else
                this._cache.Add(uniqueObjId, importedData);
            }
          }
        }
      }
      this.Loaded = true;
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage($"Ошибка получения импортированных через портал данных: {ex.Message} Cache: {this.GetType()}");
    }
  }

  public bool Loaded { get; private set; }
}
