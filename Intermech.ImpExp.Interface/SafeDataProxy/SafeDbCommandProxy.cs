// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SafeDataProxy.SafeDbCommandProxy
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Diagnostics;
using System;
using System.Data;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.Interface.SafeDataProxy;

/// <summary>
/// Обертка над стандартным IDbCommand для "перехвата ошибок" при чтении данных и возможности записи логов
/// </summary>
internal sealed class SafeDbCommandProxy : IDbCommandProxy, IDbCommand, IDisposable
{
  /// <summary>
  /// 
  /// </summary>
  private IDbCommand _target;
  /// <summary>
  /// 
  /// </summary>
  private ISafeProxyErrorHandler _errorHandler;

  public SafeDbCommandProxy([NotNull] IDbCommand target, [NotNull] ISafeProxyErrorHandler errorHandler)
  {
    this._target = target;
    this._errorHandler = errorHandler;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Dispose()
  {
    this._target?.Dispose();
    this._target = (IDbCommand) null;
    this._errorHandler = (ISafeProxyErrorHandler) null;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Prepare() => this._target.Prepare();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Cancel() => this._target.Cancel();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IDbDataParameter CreateParameter() => this._target.CreateParameter();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int ExecuteNonQuery() => this._target.ExecuteNonQuery();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IDataReader ExecuteReader()
  {
    return (IDataReader) new SafeDataReaderProxy(this._target.ExecuteReader(), this._errorHandler);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IDataReader ExecuteReader(CommandBehavior behavior)
  {
    return (IDataReader) new SafeDataReaderProxy(this._target.ExecuteReader(behavior), this._errorHandler);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public object ExecuteScalar() => this._target.ExecuteScalar();

  public IDbConnection Connection
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.Connection;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this._target.Connection = value;
  }

  public IDbTransaction Transaction
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.Transaction;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this._target.Transaction = value;
  }

  public string CommandText
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.CommandText;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this._target.CommandText = value;
  }

  public int CommandTimeout
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.CommandTimeout;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this._target.CommandTimeout = value;
  }

  public CommandType CommandType
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.CommandType;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this._target.CommandType = value;
  }

  public IDataParameterCollection Parameters
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.Parameters;
  }

  public UpdateRowSource UpdatedRowSource
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.UpdatedRowSource;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this._target.UpdatedRowSource = value;
  }

  /// <summary>
  /// 
  /// </summary>
  public IDbCommand Target => this._target;
}
