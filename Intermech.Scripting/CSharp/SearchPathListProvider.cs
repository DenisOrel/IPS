// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.SearchPathListProvider
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Базовый класс для провайдера путей поиска сборок, необходимых для компиляции сценария.
/// Реализация должна быть thread safe и доступной через remoting.
/// </summary>
public abstract class SearchPathListProvider : ScriptExecutorServiceBase
{
  /// <summary>
  /// Возвращает коллекцию путей поиска сборок для компиляции сценария.
  /// </summary>
  /// <returns>Коллекция путей к каталогам</returns>
  public abstract ICollection<string> GetSearchPathList();
}
