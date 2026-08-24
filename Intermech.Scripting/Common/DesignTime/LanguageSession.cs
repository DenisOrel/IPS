// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.LanguageSession
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public abstract class LanguageSession : ILanguageSession, IDisposable
{
  private UTF8EncodingDetector encodingDetector;
  private bool isDisposed;

  protected LanguageSession() => this.encodingDetector = new UTF8EncodingDetector();

  public void Dispose()
  {
    if (this.isDisposed)
      return;
    this.DoDispose();
    this.isDisposed = true;
  }

  protected virtual void DoDispose()
  {
  }

  public bool IsDisposed
  {
    [DebuggerStepThrough] get => this.isDisposed;
  }

  protected void CheckNotDisposed()
  {
    if (this.isDisposed)
      throw new ObjectDisposedException(this.GetType().FullName);
  }

  /// <summary>Читает текст сценария из указанного массива байт.</summary>
  /// <param name="content">Массив байт с кодом сценария</param>
  /// <returns>Текст сценария и его кодировка</returns>
  /// <exception cref="T:System.ArgumentNullException">Параметр <paramref name="content" /> не должен быть равен null</exception>
  /// <exception cref="T:Intermech.Scripting.ScriptExecutorException">Не удалось найти указанный файл</exception>
  public Tuple<string, Encoding> LoadScriptCode(byte[] content)
  {
    if (content == null)
      throw new ArgumentNullException(nameof (content));
    this.CheckNotDisposed();
    return this.DoLoadScriptCode(content);
  }

  /// <summary>Читает текст сценария из указанного массива байт.</summary>
  /// <param name="content">Массив байт с кодом сценария</param>
  /// <returns>Текст сценария и его кодировка</returns>
  protected virtual Tuple<string, Encoding> DoLoadScriptCode(byte[] content)
  {
    using (MemoryStream memoryStream = new MemoryStream(content, false))
    {
      Encoding encoding = this.encodingDetector.Detect((Stream) memoryStream);
      memoryStream.Seek(0L, SeekOrigin.Begin);
      using (StreamReader streamReader = new StreamReader((Stream) memoryStream, encoding))
        return Tuple.Create<string, Encoding>(streamReader.ReadToEnd(), encoding);
    }
  }

  /// <summary>
  /// Возвращает опции среды, необходимые для выполнения сценария.
  /// </summary>
  /// <param name="scriptProjectOptions">Опции сценария</param>
  /// <returns>Опции среды выполнения</returns>
  public Dictionary<string, string> GetRuntimeOptions(
    Dictionary<string, string> scriptProjectOptions)
  {
    return scriptProjectOptions != null ? this.DoGetRuntimeOptions(scriptProjectOptions) : throw new ArgumentNullException(nameof (scriptProjectOptions));
  }

  /// <summary>
  /// Возвращает опции среды, необходимые для выполнения сценария.
  /// </summary>
  /// <param name="scriptProjectOptions">Опции сценария</param>
  /// <returns>Опции среды выполнения</returns>
  protected virtual Dictionary<string, string> DoGetRuntimeOptions(
    Dictionary<string, string> scriptProjectOptions)
  {
    return new Dictionary<string, string>(0);
  }

  /// <summary>
  /// Выполняет код сценария в режиме выполнения анонимного модуля.
  /// </summary>
  /// <param name="scriptCode">Код сценария</param>
  /// <param name="invocationParameters">Параметры вызова сценария</param>
  /// <returns>Результат выполнения</returns>
  /// <exception cref="T:System.ArgumentException">Не задан код сценария</exception>
  /// <exception cref="T:Intermech.Scripting.ScriptStructureException">Сценарий не имеет точки входа</exception>
  /// <exception cref="T:Intermech.Scripting.ScriptCompilationException">Синтаксическая ошибка в коде сценария</exception>
  /// <exception cref="T:Intermech.Scripting.ScriptExecutorException">Другие ошибки загрузки или компиляции сценария</exception>
  public ScriptDebugInvocationResult Execute(
    string scriptCode,
    ScriptDebugInvocationParameters invocationParameters)
  {
    ArgContract.CheckScriptCode(scriptCode);
    if (invocationParameters == null)
      throw new ArgumentNullException(nameof (invocationParameters));
    this.CheckNotDisposed();
    return this.DoExecute(scriptCode, invocationParameters);
  }

  protected abstract ScriptDebugInvocationResult DoExecute(
    string scriptCode,
    ScriptDebugInvocationParameters invocationParameters);
}
