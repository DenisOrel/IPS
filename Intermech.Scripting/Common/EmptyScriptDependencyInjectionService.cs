// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.EmptyScriptDependencyInjectionService
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Пустой сервис для внедрения зависимостей в объекты сценариев.
/// Реализация класса является thread safe и доступной через remoting.
/// </summary>
internal sealed class EmptyScriptDependencyInjectionService : ScriptDependencyInjectionService
{
  private static readonly EmptyScriptDependencyInjectionService defaultInstance = new EmptyScriptDependencyInjectionService();

  /// <summary>Возвращает значения для свойств сценария.</summary>
  /// <param name="scriptCodeKey">Уникальный идентификатор сценария</param>
  /// <param name="invocationData">Объект, описывающий обращение к сценарию</param>
  /// <param name="propertyTypes">Типы свойств сценария</param>
  /// <returns>Массив значений для свойств сценария. Длина массива значений должна совпадать с длиной <paramref name="propertyTypes" /></returns>
  protected override object[] DoResolveProperties(
    ScriptCodeKey scriptCodeKey,
    ScriptInvocationData invocationData,
    string[] propertyTypes)
  {
    return new object[propertyTypes.Length];
  }

  public static EmptyScriptDependencyInjectionService Default
  {
    [DebuggerStepThrough] get => EmptyScriptDependencyInjectionService.defaultInstance;
  }
}
