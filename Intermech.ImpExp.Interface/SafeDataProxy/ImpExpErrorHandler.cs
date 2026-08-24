// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SafeDataProxy.ImpExpErrorHandler
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Diagnostics;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface.SafeDataProxy;

/// <summary>Базовый обработчик ошибок при миграции</summary>
public class ImpExpErrorHandler : ISafeProxyErrorHandler
{
  /// <summary>
  /// 
  /// </summary>
  private readonly IAppManager _appManager;
  /// <summary>
  /// 
  /// </summary>
  private readonly bool _throwException;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="appManager"></param>
  /// <param name="throwException"></param>
  /// <remarks>Вне зависимости от режима обработки - всегда пишем логи</remarks>
  public ImpExpErrorHandler([NotNull] IAppManager appManager, bool throwException = false)
  {
    this._appManager = appManager;
    this._throwException = throwException;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public void ProceedException(object sender, [NotNull] SafeDataExceptionEventArgs e)
  {
    if (!this._throwException)
      e.Handled = true;
    if (e.Exception == null)
      return;
    try
    {
      this._appManager.AddExceptionToLog(e.Exception);
      string str = e.ExtraInformation != null ? Convert.ToString(e.ExtraInformation) : (string) null;
      if (string.IsNullOrEmpty(str))
        return;
      this._appManager.AddErrorMessage("Extra Information : " + str);
    }
    catch (Exception ex)
    {
    }
  }
}
