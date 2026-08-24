// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ScriptDependencyInjectionService
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Базовый класс для сервиса внедрения зависимостей в объекты сценариев.
/// Реализация класса должна быть thread safe.
/// </summary>
public abstract class ScriptDependencyInjectionService : ScriptExecutorServiceBase
{
  /// <summary>Возвращает значения для свойств сценария.</summary>
  /// <param name="scriptCodeKey">Уникальный идентификатор сценария</param>
  /// <param name="invocationData">Объект, описывающий обращение к сценарию</param>
  /// <param name="propertyTypes">Типы свойств сценария</param>
  /// <returns>Массив значений для свойств сценария. Длина массива значений должна совпадать с длиной <paramref name="propertyTypes" /></returns>
  /// <exception cref="T:System.ArgumentNullException">Парамерт <paramref name="scriptCodeKey" /> не должен быть равен null; параметр <paramref name="invocationData" /> не должен быть равен null; параметр <paramref name="propertyTypes" /> не должен быть равен null</exception>
  public object[] ResolveProperties(
    ScriptCodeKey scriptCodeKey,
    ScriptInvocationData invocationData,
    string[] propertyTypes)
  {
    if (scriptCodeKey == null)
      throw new ArgumentNullException(nameof (scriptCodeKey));
    if (invocationData == null)
      throw new ArgumentNullException(nameof (invocationData));
    if (propertyTypes == null)
      throw new ArgumentNullException(nameof (propertyTypes));
    object[] objArray = this.DoResolveProperties(scriptCodeKey, invocationData, propertyTypes);
    if (objArray == null || objArray.Length != propertyTypes.Length)
      throw new InvalidOperationException("Длина массива возвращаемых значений должна совпадать с длиной массива имен свойств.");
    return objArray;
  }

  /// <summary>Возвращает значения для свойств сценария.</summary>
  /// <param name="scriptCodeKey">Уникальный идентификатор сценария</param>
  /// <param name="invocationData">Объект, описывающий обращение к сценарию</param>
  /// <param name="propertyTypes">Типы свойств сценария</param>
  /// <returns>Массив значений для свойств сценария. Длина массива значений должна совпадать с длиной <paramref name="propertyTypes" /></returns>
  protected abstract object[] DoResolveProperties(
    ScriptCodeKey scriptCodeKey,
    ScriptInvocationData invocationData,
    string[] propertyTypes);
}
