// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.IScriptObjectKeeper
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Интерфейс объекта-хранителя, содержащего проинициализированный и готовый к использованию объект сценария C#.
/// Такие объекты-хранители применяются в тех случаях, когда обращение к сценарию C# не может быть
/// сведено к единственному вызову метода Execute.
/// </summary>
/// <remarks>
/// Реализация не является thread safe. Объекты-хранители и содержащиеся в них объекты сценариев
/// привязаны к потоку выполнения (thread), в котором они были созданы, и могут использоваться
/// только из этого потока.
/// </remarks>
public interface IScriptObjectKeeper : IDisposable
{
  /// <summary>Возвращает объект сценария C#.</summary>
  /// <exception cref="T:System.InvalidOperationException">Обращения из других потоков управления запрещены</exception>
  /// <exception cref="T:System.ObjectDisposedException">Ресурсы объекта-хранителя и сценария уже были освобождены</exception>
  object ScriptObject { get; }
}
