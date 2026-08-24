// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PluginClass
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.Controls;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Interface.SafeDataProxy;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Summary description for PluginClass.</summary>
public class PluginClass : IPlugin
{
  public IAppManager appManager;
  private IDataWriter dataWriter;
  private IMetadataInfo metadataInfo;
  /// <summary>
  /// Список доступных задач для проверки и начальной инициализации задач перекачки
  /// </summary>
  protected List<IPumpTask> verificationsList = new List<IPumpTask>();
  /// <summary>Список доступных задач перекачки</summary>
  protected List<IPumpTask> pumpsList = new List<IPumpTask>();
  /// <summary>Список доступных задач финальной перекачки</summary>
  protected List<IPumpTask> finalPumpsList = new List<IPumpTask>();
  /// <summary>Псевдоним подключения к базе данных</summary>
  protected string aliasString = "DATABASE CONNECTION";
  /// <summary>
  /// Глобальный идентификатор простой связи между объектами
  /// </summary>
  public Guid reltypeSimpleGuid = new Guid("cad00022-306c-11d8-b4e9-00304f19f545");
  /// <summary>Глобальный идентификатор простой связи с сортировкой</summary>
  public Guid reltypeSortedGuid = new Guid("cad00151-306c-11d8-b4e9-00304f19f545");
  protected IDbConnection baseConnect;

  public IDataWriter Idw
  {
    get
    {
      if (this.dataWriter == null)
        this.dataWriter = ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter;
      return this.dataWriter;
    }
  }

  public IMetadataInfo Imdi
  {
    get
    {
      if (this.metadataInfo == null)
        this.metadataInfo = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
      return this.metadataInfo;
    }
  }

  /// <summary>Конструктор</summary>
  /// <param name="manager"></param>
  public PluginClass(IAppManager manager) => this.appManager = manager;

  /// <summary>Вспомогатеьный объект подключения к базам ИНТЕРМЕХ</summary>
  public IMConnection imConnection { get; private set; }

  /// <summary>Тип базы к которой производится подключение</summary>
  public IDataBaseType idbType { get; private set; }

  /// <summary>Ссылка на объект базы данных</summary>
  public IDataBase idb { get; private set; }

  /// <summary>подключение к базе</summary>
  /// <returns>интерфейс на подключение к базе</returns>
  protected IDbConnection OpenDbConnection(ConnStrType connectionType)
  {
    return this.OpenDbConnection(connectionType, (ReadIniDelegate) null);
  }

  public IDbConnection OpenDbConnection(
    string Alias,
    string dbName,
    string dsString,
    string aliasType)
  {
    this.imConnection = new IMConnection(Alias, dbName, dsString, aliasType);
    string aliasString = this.aliasString;
    IDataBase idb = this.idb;
    try
    {
      this.aliasString = Alias;
      ConnectionSetting connectionSetting = SavedConnectionStrings.Items["SEARCH4"];
      return this.DoConnect(this.imConnection, connectionSetting.UserName, connectionSetting.Password, true);
    }
    finally
    {
      this.aliasString = aliasString;
      this.idb = idb;
    }
  }

  /// <summary>
  /// Подключение к дополнительной БД, отличной от основной БД, с которой работает плагин
  /// </summary>
  /// <returns>true - подключено успешно.</returns>
  public IDbConnection CustomDbConnection(ConnStrType targetDb)
  {
    string aliasString = this.aliasString;
    IDataBase idb = this.idb;
    try
    {
      this.aliasString = targetDb.ToDbAlias();
      return this.OpenDbConnection(targetDb);
    }
    finally
    {
      this.aliasString = aliasString;
      this.idb = idb;
    }
  }

  private IDbConnection DoConnect(
    IMConnection imConnection,
    string userName,
    string password,
    bool silent)
  {
    if (this.idbType == null)
      this.idbType = this.appManager.DBManager.GetDbType(imConnection.DataBaseType);
    if (TraceSupport.PluginConnections.Enabled)
    {
      Trace.WriteLine($"PluginClass.doConnect(): aliasString = {this.aliasString}");
      Trace.WriteLine($"PluginClass.doConnect(): idbType {(this.idbType != null ? (object) imConnection.DataBaseType : (object) "is null")}");
    }
    this.idb = this.appManager.DBManager.FindDbByAlias(this.aliasString);
    if (this.idb == null && this.idbType != null)
    {
      this.idb = this.appManager.DBManager.CreateDBConnection(this.idbType, this.aliasString);
      if (TraceSupport.PluginConnections.Enabled)
        Trace.WriteLine("PluginClass.doConnect(): call appManager.DBManager.CreateDBConnection");
    }
    if (TraceSupport.PluginConnections.Enabled)
      Trace.WriteLine($"PluginClass.doConnect(): idb = {(this.idb != null ? (object) this.idb.DataBaseType : (object) "is null")}");
    IDbConnection dbConnection = (IDbConnection) null;
    if (this.idb != null)
    {
      this.idb = this.idb is SafeDataBaseProxy ? this.idb : (IDataBase) new SafeDataBaseProxy(this.idb, (ISafeProxyErrorHandler) new ImpExpErrorHandler(this.appManager));
      dbConnection = this.idb.DbConnection;
      int num = 3;
      if (silent)
        num = 1;
      string upper = imConnection.Alias.ToUpper();
      if (SavedConnectionStrings.Items.ContainsKey(upper))
      {
        silent = true;
        userName = SavedConnectionStrings.Items[upper].UserName;
        password = SavedConnectionStrings.Items[upper].Password;
      }
      if (TraceSupport.PluginConnections.Enabled)
        Trace.WriteLine($"PluginClass.doConnect(): idbConn.State = {dbConnection.State}");
      if (dbConnection.State != ConnectionState.Open)
      {
        while (dbConnection.State != ConnectionState.Open && num > 0 && (silent || IMConnection.Login(ref userName, ref password, $"Подключение к {imConnection.Name}")))
        {
          --num;
          dbConnection.ConnectionString = imConnection.GetConnectionString(userName, password);
          try
          {
            string key = "";
            string connString = "";
            if (!silent)
            {
              key = imConnection.Alias.ToUpper();
              connString = dbConnection.ConnectionString;
            }
            this.appManager.AddInfoMessage($"Попытка подключения к {imConnection.Alias}");
            dbConnection.Open();
            if (!silent)
            {
              if (!SavedConnectionStrings.Items.ContainsKey(key))
                SavedConnectionStrings.Items.Add(key, new ConnectionSetting(connString, userName, password));
            }
          }
          catch (Exception ex)
          {
            this.appManager.AddErrorMessage($"Ошибка при подключении к базе ({(object) ex.GetType()}) :{ex.Message}");
            this.appManager.AddExceptionToLog(ex);
          }
        }
        if (dbConnection.State != ConnectionState.Open)
        {
          this.appManager.AddErrorMessage($"Не удалось произвести подключение к базе данных {dbConnection.Database}");
        }
        else
        {
          this.idb.OnAfterConnect();
          this.appManager.AddInfoMessage($"Произведено подключение к базе данных {dbConnection.Database}");
        }
      }
      else
      {
        this.appManager.AddInfoMessage($"Использование существующего подключения к базе данных {dbConnection.Database}");
        if (!SavedConnectionStrings.Items.ContainsKey(upper))
          SavedConnectionStrings.Items.Add(upper, new ConnectionSetting(dbConnection.ConnectionString, userName, password));
      }
    }
    return dbConnection;
  }

  protected IDbConnection OpenDbConnection(ConnStrType connectionType, ReadIniDelegate readIniFunc)
  {
    if (TraceSupport.PluginConnections.Enabled)
      Trace.WriteLine($"PluginClass.openDbConnection(): connectionType = {connectionType}");
    try
    {
      this.imConnection = new IMConnection(connectionType, readIniFunc);
    }
    catch (Exception ex)
    {
      this.appManager.AddErrorMessage($"Ошибка при создании строки подключения для {connectionType}: {ex.Message}");
      return (IDbConnection) null;
    }
    if (TraceSupport.PluginConnections.Enabled)
      Trace.WriteLine($"PluginClass.openDbConnection(): imConnection.ConnectionString = '{this.imConnection.ConnectionString}'");
    return this.DoConnect(this.imConnection, "", "", false);
  }

  /// <summary>Отключение от базы</summary>
  protected void CloseDbConnection(IDbConnection idbConn)
  {
    string str = string.Empty;
    if (idbConn != null)
    {
      if (idbConn.State != ConnectionState.Closed)
      {
        try
        {
          str = idbConn.Database;
          idbConn.Close();
          idbConn = (IDbConnection) null;
        }
        catch (Exception ex)
        {
          this.appManager.AddErrorMessage($"Ошибка при отключении от базы ({(object) ex.GetType()}): {ex.Message}");
        }
      }
    }
    this.appManager.AddInfoMessage($"Произведено отключение от базы данных {str}");
  }

  public virtual Guid GUID => Guid.Empty;

  public virtual string Description => "Base class for plugins";

  public virtual string Name => "Base plugin implementation";

  public virtual StepControl[] GetSettingsControls() => new StepControl[0];

  public virtual bool InitSettings() => true;

  public virtual bool Execute() => true;

  public virtual IPumpTask[] GetVerifications() => this.verificationsList.ToArray();

  public virtual IPumpTask[] GetPumps() => this.pumpsList.ToArray();

  public virtual bool IsConnected()
  {
    if (TraceSupport.PluginConnections.Enabled)
      Trace.WriteLine($"PluginClass.IsConnected(): baseConnect {(this.baseConnect != null ? (object) "not null" : (object) "is null")}");
    if (TraceSupport.PluginConnections.Enabled && this.baseConnect != null)
      Trace.WriteLine($"PluginClass.IsConnected(): baseConnect.State = {this.baseConnect.State}");
    return this.baseConnect != null && this.baseConnect.State == ConnectionState.Open;
  }

  public virtual bool BaseConnect()
  {
    this.baseConnect = this.OpenDbConnection(ConnStrType.Search);
    return this.IsConnected();
  }

  public virtual bool BaseDisconnect()
  {
    this.CloseDbConnection(this.baseConnect);
    return this.IsConnected();
  }

  public virtual string[] ConnectInfo => (string[]) null;

  public string Location => Assembly.GetAssembly(this.GetType()).Location;

  public virtual IPumpTask[] GetFinalPumps() => this.finalPumpsList?.ToArray();
}
