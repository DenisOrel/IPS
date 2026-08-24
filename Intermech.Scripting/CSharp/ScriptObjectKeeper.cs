// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ScriptObjectKeeper
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Базовый класс объекта-хранителя, содержащего проинициализированный и готовый к использованию объект сценария C#.
/// Такие объекты-хранители применяются в тех случаях, когда обращение к сценарию C# не может быть
/// сведено к единственному вызову метода Execute.
/// </summary>
/// <remarks>
/// Реализация не является thread safe. Объекты-хранители и содержащиеся в них объекты сценариев
/// привязаны к потоку выполнения (thread), в котором они были созданы, и могут использоваться
/// только из этого потока.
/// </remarks>
public abstract class ScriptObjectKeeper : IScriptObjectKeeper, IDisposable
{
  private int initialThreadId;
  private object scriptObject;
  private bool isDisposed;

  /// <summary>Создает объект.</summary>
  /// <param name="initialThreadId">Идентификатор потока, в котором был создан объект сценария</param>
  /// <param name="scriptObject">Объект сценария</param>
  /// <exception cref="T:System.ArgumentNullException">параметр <paramref name="scriptObject" /> содержит null</exception>
  protected ScriptObjectKeeper(int initialThreadId, object scriptObject)
  {
    if (scriptObject == null)
      throw new ArgumentNullException(nameof (scriptObject));
    this.initialThreadId = initialThreadId;
    this.scriptObject = scriptObject;
  }

  /// <summary>
  /// Освобождает ресурсы сценария C# и очищает объект сценария.
  /// </summary>
  public void Dispose()
  {
    this.CheckForIllegalCrossThreadCalls();
    if (this.isDisposed)
      return;
    this.DoDispose(this.scriptObject);
    this.isDisposed = true;
  }

  /// <summary>
  /// Освобождает ресурсы сценария C# и очищает объект сценария.
  /// </summary>
  /// <param name="scriptObject">Объект сценария</param>
  protected abstract void DoDispose(object scriptObject);

  private void CheckForNotDisposed()
  {
    if (this.isDisposed)
      throw new ObjectDisposedException(this.GetType().FullName);
  }

  /// <summary>Возвращает объект сценария C#.</summary>
  /// <exception cref="T:System.InvalidOperationException">Обращения из других потоков управления запрещены</exception>
  /// <exception cref="T:System.ObjectDisposedException">Ресурсы объекта-хранителя и сценария уже были освобождены</exception>
  public object ScriptObject
  {
    [DebuggerStepThrough] get
    {
      this.CheckState();
      return this.scriptObject;
    }
  }

  /// <summary>Проверяет корректность состояния текущего объекта.</summary>
  protected virtual void CheckState()
  {
    this.CheckForIllegalCrossThreadCalls();
    this.CheckForNotDisposed();
  }

  private void CheckForIllegalCrossThreadCalls()
  {
    if (Thread.CurrentThread.ManagedThreadId != this.initialThreadId)
      throw new InvalidOperationException($"Объект типа '{this.GetType()}' не является thread-safe, а обращения к нему допустимы только из того потока, в котором он был создан.");
  }
}
