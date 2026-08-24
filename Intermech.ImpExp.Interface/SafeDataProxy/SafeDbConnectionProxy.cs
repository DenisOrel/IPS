// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SafeDataProxy.SafeDbConnectionProxy
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
/// Обертка над стандартным IDbConnection для "перехвата ошибок" при чтении данных и возможности записи логов
/// </summary>
internal sealed class SafeDbConnectionProxy : IDbConnection, IDisposable
{
  private IDbConnection _target;
  private ISafeProxyErrorHandler _errorHandler;

  public SafeDbConnectionProxy([NotNull] IDbConnection target, [NotNull] ISafeProxyErrorHandler errorHandler)
  {
    this._target = target;
    this._errorHandler = errorHandler;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Dispose()
  {
    this._target?.Dispose();
    this._target = (IDbConnection) null;
    this._errorHandler = (ISafeProxyErrorHandler) null;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IDbTransaction BeginTransaction() => this._target.BeginTransaction();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IDbTransaction BeginTransaction(IsolationLevel il) => this._target.BeginTransaction(il);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Close() => this._target.Close();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ChangeDatabase(string databaseName) => this._target.ChangeDatabase(databaseName);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IDbCommand CreateCommand()
  {
    return (IDbCommand) new SafeDbCommandProxy(this._target.CreateCommand(), this._errorHandler);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void Open() => this._target.Open();

  public string ConnectionString
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.ConnectionString;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set => this._target.ConnectionString = value;
  }

  public int ConnectionTimeout
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.ConnectionTimeout;
  }

  public string Database
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.Database;
  }

  public ConnectionState State
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this._target.State;
  }
}
