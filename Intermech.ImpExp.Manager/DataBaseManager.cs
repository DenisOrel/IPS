// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataBaseManager
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager;

public class DataBaseManager : IDataBaseManager
{
  private IAppManager _appManager;
  private List<IDataBaseType> _dataBaseTypes;
  private Dictionary<string, IDataBase> _dataBases;

  public DataBaseManager(IAppManager manager)
  {
    this._appManager = manager;
    this._dataBaseTypes = new List<IDataBaseType>();
    this._dataBases = new Dictionary<string, IDataBase>();
  }

  public bool RegisterDbType(IDataBaseType dbType)
  {
    if (dbType == null)
      return false;
    foreach (IDataBaseType dataBaseType in this._dataBaseTypes)
    {
      if (dataBaseType != null && dataBaseType.DataBaseType() == dbType.DataBaseType())
        return false;
    }
    this._dataBaseTypes.Add(dbType);
    return true;
  }

  public IDataBaseType GetDbType(string dbTypeName)
  {
    foreach (IDataBaseType dataBaseType in this._dataBaseTypes)
    {
      if (dataBaseType != null && dataBaseType.DataBaseType() == dbTypeName)
        return dataBaseType;
    }
    return (IDataBaseType) null;
  }

  public IDataBase FindDbByAlias(string dbAlias)
  {
    return this._dataBases.ContainsKey(dbAlias) ? this._dataBases[dbAlias] : (IDataBase) null;
  }

  public IDataBase CreateDBConnection(IDataBaseType dbType, string dbAlias)
  {
    if (dbType != null && !this._dataBases.ContainsKey(dbAlias))
    {
      IDataBase newDataBase = dbType.GetNewDataBase();
      this._dataBases.Add(dbAlias, newDataBase);
      this._appManager.AddInfoMessage("Создано новое подключение с псевдонимом: " + dbAlias);
      return newDataBase;
    }
    this._appManager.AddWarningMessage($"Подключение c псевдонимом {dbAlias} уже существует");
    return (IDataBase) null;
  }
}
