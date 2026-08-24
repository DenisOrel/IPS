// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IScriptDisplayBehavior
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс поведения сценариев во время отображения в IDE.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface IScriptDisplayBehavior
{
  /// <summary>Возвращает имя сценария для отображения в окнах IDE.</summary>
  /// <returns>Имя сценария для отображения в окнах IDE</returns>
  string GetDisplayName();
}
