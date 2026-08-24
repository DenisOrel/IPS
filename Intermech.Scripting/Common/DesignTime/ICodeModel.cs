// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ICodeModel
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс расширяемой кодовой модели для исходного кода сценариев.
/// Кодовая модель предоставляет языковые сервисы для редактора исходного кода: навигация, автодополнение и пр.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface ICodeModel
{
  /// <summary>
  /// Возвращает или задает журнал для протоколирования работы текущего объекта.
  /// Значение свойства может быть не задано и равно null.
  /// </summary>
  Action<string> Log { get; set; }

  /// <summary>
  /// Возвращает или задает параметры разбора исходного текста сценария.
  /// Значение может быть не задано и равно null.
  /// </summary>
  Dictionary<string, string> ParseOptions { get; set; }

  /// <summary>
  /// Проверяет статус синхронизации внутреннего состояния кодовой модели и редактора исходного кода сценария.
  /// Редактор обязан вызывать этот метод каждый раз перед обновлением кодовой модели и получением языковых данных.
  /// В свою очередь, методы обновления кодовой модели и получения языковых данных работают только в состоянии полной синхронизации.
  /// </summary>
  /// <returns>Статус синхронизации кодовой модели и редактора исходного кода сценария</returns>
  CodeModelSynchronizationStatus CheckSynchronizationStatus();

  /// <summary>
  /// Открывает исходный код сценария для дальнейшей работы.
  /// После успешного выполнения метода кодовая модель переходит в синхронизированное состояние.
  /// Предварительный вызов метода <see cref="M:Intermech.Scripting.Common.DesignTime.ICodeModel.CheckSynchronizationStatus" /> не требуется.
  /// </summary>
  /// <param name="text">Исходный код сценария</param>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="text" /> содержит null</exception>
  void OpenText(string text);

  /// <summary>
  /// Закрывает исходный код сценария и освобождает все связанные с ним ресурсы.
  /// После выполнения метода модель переходит в началаное несинхронизированное состояние.
  /// Предварительно вызывать метод <see cref="M:Intermech.Scripting.Common.DesignTime.ICodeModel.CheckSynchronizationStatus" /> не требуется.
  /// </summary>
  /// <param name="throwIfError">Признак, требуется ли пробрасывать дальше необработанные исключения</param>
  void CloseText(bool throwIfError);

  /// <summary>
  /// Обновляет кодовую модель, передавая ей изменения в исходно коде сценария, которые произошли с момента открытия кода сценария.
  /// Предварительно требуется вызвать метод <see cref="M:Intermech.Scripting.Common.DesignTime.ICodeModel.CheckSynchronizationStatus" /> и убедиться в наличии полной синхронизации с редактором.
  /// </summary>
  /// <param name="changes">Список изменений</param>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="changes" /> содержит null</exception>
  /// <exception cref="T:System.InvalidOperationException">Метод проверки синхронизации не был вызван</exception>
  void ChangeText(List<ScriptTextChange> changes);
}
