// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SafeDataProxy.ISafeProxyErrorHandler
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Diagnostics;

#nullable disable
namespace Intermech.ImpExp.Interface.SafeDataProxy;

/// <summary>Интерфейс обработчика ошибок</summary>
public interface ISafeProxyErrorHandler
{
  /// <summary>Метод для обработки исключительной ситуаций</summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  void ProceedException(object sender, [NotNull] SafeDataExceptionEventArgs e);
}
