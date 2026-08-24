// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ICodeCompletionProvider
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс провайдера для автодополнения кода сценариев.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface ICodeCompletionProvider
{
  void GetResult(
    IReadOnlyTextDocument document,
    int documentOffset,
    bool manualMode,
    bool includeInsigtItems,
    ICodeCompletionResultBuilder resultBuilder);
}
