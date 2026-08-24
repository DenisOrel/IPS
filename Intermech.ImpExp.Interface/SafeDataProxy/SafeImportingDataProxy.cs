// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SafeDataProxy.SafeImportingDataProxy
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.Interface.SafeDataProxy;

/// <summary>
/// Обертка над стандартным IImportingData для "перехвата ошибок" при добавлении данных и возможности записи логов
/// </summary>
public sealed class SafeImportingDataProxy : IImportingData
{
  /// <summary>
  /// 
  /// </summary>
  private readonly IImportingData _target;
  /// <summary>
  /// 
  /// </summary>
  private readonly ISafeProxyErrorHandler _errorHandler;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="exception"></param>
  /// <param name="extraInfoFunc"></param>
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

  /// <summary>
  /// 
  /// </summary>
  /// <param name="target"></param>
  /// <param name="errorHandler"></param>
  public SafeImportingDataProxy([NotNull] IImportingData target, [NotNull] ISafeProxyErrorHandler errorHandler)
  {
    this._target = target;
    this._errorHandler = errorHandler;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(ImportingCategory category, object oldKey, long newKey)
  {
    try
    {
      this._target.AddValue(category, oldKey, newKey);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"category = {category}, oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(int category, object oldKey, long newKey)
  {
    try
    {
      this._target.AddValue(category, oldKey, newKey);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"category = {category}, oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(ImportingCategory category, object oldKey, long newKey, string caption)
  {
    try
    {
      this._target.AddValue(category, oldKey, newKey, caption);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"category = {category}, oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(int category, object oldKey, long newKey, string caption)
  {
    try
    {
      this._target.AddValue(category, oldKey, newKey, caption);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"category = {category}, oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(
    ImportingCategory category,
    object oldKey,
    long newKey,
    ITagImportObject tag)
  {
    try
    {
      this._target.AddValue(category, oldKey, newKey, tag);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"category = {category}, oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(int category, object oldKey, long newKey, ITagImportObject tag)
  {
    try
    {
      this._target.AddValue(category, oldKey, newKey, tag);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"category = {category}, oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(
    ImportingCategory category,
    object oldKey,
    long newKey,
    string caption,
    ITagImportObject tag)
  {
    try
    {
      this._target.AddValue(category, oldKey, newKey, caption, tag);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"category = {category}, oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(
    int category,
    object oldKey,
    long newKey,
    string caption,
    ITagImportObject tag)
  {
    try
    {
      this._target.AddValue(category, oldKey, newKey, caption, tag);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"category = {category}, oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public long GetNewKey(ImportingCategory category, object oldKey)
  {
    return this._target.GetNewKey(category, oldKey);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public long GetNewKey(int category, object oldKey) => this._target.GetNewKey(category, oldKey);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public string GetCaption(ImportingCategory category, object oldKey)
  {
    return this._target.GetCaption(category, oldKey);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public string GetCaption(int category, object oldKey)
  {
    return this._target.GetCaption(category, oldKey);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ITagImportObject GetTag(ImportingCategory category, object oldKey)
  {
    return this._target.GetTag(category, oldKey);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ITagImportObject GetTag(int category, object oldKey)
  {
    return this._target.GetTag(category, oldKey);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DictionaryValue GetValue(ImportingCategory category, object oldKey)
  {
    return this._target.GetValue(category, oldKey);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DictionaryValue GetValue(int category, object oldKey)
  {
    return this._target.GetValue(category, oldKey);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Dictionary<object, DictionaryValue> GetCategory(ImportingCategory category)
  {
    return this._target.GetCategory(category);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Dictionary<object, DictionaryValue> GetCategory(int category)
  {
    return this._target.GetCategory(category);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(object oldKey, long newKey)
  {
    try
    {
      this._target.AddValue(oldKey, newKey);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(object oldKey, long newKey, string caption)
  {
    try
    {
      this._target.AddValue(oldKey, newKey, caption);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(object oldKey, long newKey, ITagImportObject tag)
  {
    try
    {
      this._target.AddValue(oldKey, newKey, tag);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void AddValue(object oldKey, long newKey, string caption, ITagImportObject tag)
  {
    try
    {
      this._target.AddValue(oldKey, newKey, caption, tag);
    }
    catch (Exception ex)
    {
      if (this.HandleException(ex, (Func<string>) (() => $"oldKey = {oldKey}, newKey = {newKey}")))
        return;
      throw;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public long GetNewKey(object oldKey) => this._target.GetNewKey(oldKey);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public string GetCaption(object oldKey) => this._target.GetCaption(oldKey);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ITagImportObject GetTag(object oldKey) => this._target.GetTag(oldKey);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DictionaryValue GetValue(object oldKey) => this._target.GetValue(oldKey);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Dictionary<object, DictionaryValue> GetCategory() => this._target.GetCategory();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool SetNewKey(ImportingCategory category, object oldKey, long newKey)
  {
    return this._target.SetNewKey(category, oldKey, newKey);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool SetNewKey(int category, object oldKey, long newKey)
  {
    return this._target.SetNewKey(category, oldKey, newKey);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool SetNewKey(object oldKey, long newKey) => this._target.SetNewKey(oldKey, newKey);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool IsCategoryPresent(ImportingCategory category)
  {
    return this._target.IsCategoryPresent(category);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool IsCategoryPresent(int category) => this._target.IsCategoryPresent(category);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool ClearValue(int category, object oldKey) => this._target.ClearValue(category, oldKey);
}
