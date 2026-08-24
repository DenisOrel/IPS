// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.EmptySearchPathListProvider
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Пустой провайдер путей поиска сборок, необходимых для компиляции сценария.
/// Реализация класса является thread safe и доступной через remoting.
/// </summary>
public sealed class EmptySearchPathListProvider : SearchPathListProvider
{
  private static readonly EmptySearchPathListProvider defaultInstance = new EmptySearchPathListProvider();
  private string[] cachedResult;

  public EmptySearchPathListProvider() => this.cachedResult = new string[0];

  /// <summary>
  /// Возвращает пустую коллекцию путей поиска сборок для компиляции сценария.
  /// </summary>
  /// <returns>Пустая коллекция путей к каталогам</returns>
  public override ICollection<string> GetSearchPathList()
  {
    return (ICollection<string>) this.cachedResult;
  }

  public static EmptySearchPathListProvider Default
  {
    [DebuggerStepThrough] get => EmptySearchPathListProvider.defaultInstance;
  }
}
