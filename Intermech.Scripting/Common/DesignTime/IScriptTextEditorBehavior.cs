// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IScriptTextEditorBehavior
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс поведения сценариев во время редактирования в IDE.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface IScriptTextEditorBehavior
{
  /// <summary>
  /// Создает опции для <see cref="T:Intermech.Scripting.Common.DesignTime.ICodeModel" />
  /// </summary>
  /// <param name="scriptProjectOptions">Опции сценария</param>
  /// <param name="runtimeOptions">Опции среды выполнения</param>
  /// <returns>Опции для <see cref="T:Intermech.Scripting.Common.DesignTime.ICodeModel" /></returns>
  Dictionary<string, string> TryCreateCodeModelOptions(
    Dictionary<string, string> scriptProjectOptions,
    Dictionary<string, string> runtimeOptions);

  /// <summary>Возвращает провайдер автодополнения кода сценария.</summary>
  /// <param name="xmlDocPathList">Коллекция путей к папкам с xml-документацией</param>
  /// <returns>Провайдер автодополнения или null</returns>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="xmlDocPathList" /> не должен быть равен null</exception>
  ICodeCompletionProvider TryGetCodeCompletionProvider(ICollection<string> xmlDocPathList);
}
