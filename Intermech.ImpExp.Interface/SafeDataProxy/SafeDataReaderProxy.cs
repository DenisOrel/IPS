// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SafeDataProxy.SafeDataReaderProxy
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
/// Обертка над стандартным IDataReader для "перехвата ошибок" при чтении данных и возможности записи логов
/// </summary>
internal sealed class SafeDataReaderProxy : IDataReader, IDisposable, IDataRecord
{
  /// <summary>
  /// 
  /// </summary>
  private IDataReader _target;
  /// <summary>
  /// 
  /// </summary>
  private ISafeProxyErrorHandler _errorHandler;

  /// <summary>Получение идентифицирующего значения для строки</summary>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private string GetRowIdInformation()
  {
    string str = string.Empty;
    try
    {
      str = !this._target.IsDBNull(0) ? Convert.ToString(this._target[0]) : string.Empty;
    }
    catch (Exception ex)
    {
    }
    return !string.IsNullOrEmpty(str) ? " rowId = " + str : str;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="exception"></param>
  /// <param name="extraInfoFunc"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private bool HandleException(Exception exception, Func<string> extraInfoFunc = null)
  {
    if (this._errorHandler == null)
      return false;
    SafeDataExceptionEventArgs e = new SafeDataExceptionEventArgs(exception);
    if (extraInfoFunc != null)
      e.ExtraInformation = (object) extraInfoFunc();
    this._errorHandler.ProceedException((object) this, e);
    return e.Handled;
  }

  /// <summary>Конструктор</summary>
  /// <param name="target"></param>
  /// <param name="schemaCacheItem"></param>
  public SafeDataReaderProxy([NotNull] IDataReader target, ISafeProxyErrorHandler errorHandler = null)
  {
    this._target = target;
    this._errorHandler = errorHandler;
  }

  /// <summary>
  /// 
  /// </summary>
  public void Dispose()
  {
    this._target?.Dispose();
    this._target = (IDataReader) null;
    this._errorHandler = (ISafeProxyErrorHandler) null;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public string GetName(int i) => this._target.GetName(i);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public string GetDataTypeName(int i) => this._target.GetDataTypeName(i);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Type GetFieldType(int i) => this._target.GetFieldType(i);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public object GetValue(int i)
  {
    try
    {
      return this._target.GetValue(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return (object) DBNull.Value;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int GetValues(object[] values)
  {
    try
    {
      return this._target.GetValues(values);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"values = {values} {this.GetRowIdInformation()}")))
        return 0;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int GetOrdinal(string name) => this._target.GetOrdinal(name);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool GetBoolean(int i)
  {
    try
    {
      return this._target.GetBoolean(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return false;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte GetByte(int i)
  {
    try
    {
      return this._target.GetByte(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return 0;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length)
  {
    try
    {
      return this._target.GetBytes(i, fieldOffset, buffer, bufferOffset, length);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return 0;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public char GetChar(int i)
  {
    try
    {
      return this._target.GetChar(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return char.MinValue;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length)
  {
    try
    {
      return this._target.GetChars(i, fieldOffset, buffer, bufferOffset, length);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return 0;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Guid GetGuid(int i)
  {
    try
    {
      return this._target.GetGuid(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return new Guid();
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public short GetInt16(int i)
  {
    try
    {
      return this._target.GetInt16(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return 0;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int GetInt32(int i)
  {
    try
    {
      return this._target.GetInt32(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return 0;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public long GetInt64(int i)
  {
    try
    {
      return this._target.GetInt64(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return 0;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float GetFloat(int i)
  {
    try
    {
      return this._target.GetFloat(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return 0.0f;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public double GetDouble(int i)
  {
    try
    {
      return this._target.GetDouble(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return 0.0;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public string GetString(int i)
  {
    try
    {
      return this._target.GetString(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return (string) null;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Decimal GetDecimal(int i)
  {
    try
    {
      return this._target.GetDecimal(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return 0M;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DateTime GetDateTime(int i)
  {
    try
    {
      return this._target.GetDateTime(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return new DateTime();
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public IDataReader GetData(int i)
  {
    try
    {
      return this._target.GetData(i);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
        return (IDataReader) null;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool IsDBNull(int i) => this._target.IsDBNull(i);

  public int FieldCount => this._target.FieldCount;

  public object this[int i]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      try
      {
        return this._target[i];
      }
      catch (Exception ex)
      {
        if (this.HandleException(ex, (Func<string>) (() => $"fieldIndex = {i} {this.GetRowIdInformation()}")))
          return (object) DBNull.Value;
        throw;
      }
    }
  }

  public object this[string name]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      try
      {
        return this._target[name];
      }
      catch (Exception ex)
      {
        if (this.HandleException(ex, (Func<string>) (() => $"fieldName = {name} {this.GetRowIdInformation()}")))
          return (object) DBNull.Value;
        throw;
      }
    }
  }

  public void Close() => this._target?.Close();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DataTable GetSchemaTable() => this._target.GetSchemaTable();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool NextResult() => this._target.NextResult();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Read() => this._target.Read();

  public int Depth => this._target.Depth;

  public bool IsClosed => this._target.IsClosed;

  public int RecordsAffected => this._target.RecordsAffected;
}
