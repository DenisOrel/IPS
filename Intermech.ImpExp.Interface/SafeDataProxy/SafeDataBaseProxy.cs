// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SafeDataProxy.SafeDataBaseProxy
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Diagnostics;
using System.Data;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.Interface.SafeDataProxy;

/// <summary>
/// Обертка над стандартным IDataBase для "перехвата ошибок" при чтении данных и возможности записи логов
/// </summary>
public sealed class SafeDataBaseProxy : IDataBase
{
  /// <summary>
  /// 
  /// </summary>
  private readonly IDataBase _target;
  /// <summary>
  /// 
  /// </summary>
  private readonly ISafeProxyErrorHandler _errorHandler;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="target"></param>
  /// <param name="errorHandler"></param>
  public SafeDataBaseProxy([NotNull] IDataBase target, [NotNull] ISafeProxyErrorHandler errorHandler)
  {
    this._target = target;
    this._errorHandler = errorHandler;
  }

  /// <summary>
  /// 
  /// </summary>
  public string DataBaseType => this._target.DataBaseType;

  /// <summary>
  /// 
  /// </summary>
  public IDbConnection DbConnection
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      IDbConnection dbConnection = this._target.DbConnection;
      return !(dbConnection is SafeDbConnectionProxy) ? (IDbConnection) new SafeDbConnectionProxy(dbConnection, this._errorHandler) : dbConnection;
    }
  }

  public int MaxInOperator => this._target.MaxInOperator;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IDbCommand CreateCommand()
  {
    IDbCommand command = this._target.CreateCommand();
    return !(command is SafeDbCommandProxy) ? (IDbCommand) new SafeDbCommandProxy(command, this._errorHandler) : command;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IDataReader GetDataReader(string sqlText)
  {
    IDataReader dataReader = this._target.GetDataReader(sqlText);
    return !(dataReader is SafeDataReaderProxy) ? (IDataReader) new SafeDataReaderProxy(dataReader, this._errorHandler) : dataReader;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IDbDataAdapter GetDataAdapter(string sqlText) => this._target.GetDataAdapter(sqlText);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool TableExists(string tableName) => this._target.TableExists(tableName);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public string GetIntField(string fieldName, string asFieldName)
  {
    return this._target.GetIntField(fieldName, asFieldName);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void OnAfterConnect() => this._target.OnAfterConnect();
}
