// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ServiceProcess.LocalServiceConnector`1
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Reflection;

#nullable disable
namespace Intermech.Scripting.CSharp.ServiceProcess;

/// <summary>
/// Вспомогательный объект, который позволяет установить подключение к сервису в текущем процессе,
/// изначально спроектированному для работы в отдельном процессе.
/// </summary>
/// <typeparam name="T">Интерфейс головного объекта приложения</typeparam>
/// <remarks>Реализация не является thread safe.</remarks>
internal sealed class LocalServiceConnector<T>
{
  private readonly string executablePath;
  private readonly string rootTypeName;
  private Assembly localServicesAssembly;
  private T localServiceCache;

  public LocalServiceConnector(string executablePath, string rootTypeName)
  {
    if (executablePath == null)
      throw new ArgumentNullException(nameof (executablePath));
    if (rootTypeName == null)
      throw new ArgumentNullException(nameof (rootTypeName));
    this.executablePath = executablePath;
    this.rootTypeName = rootTypeName;
  }

  public bool IsConnected => (object) this.localServiceCache != null;

  public T GetOrConnect()
  {
    if (this.localServicesAssembly == (Assembly) null)
      this.localServicesAssembly = Assembly.LoadFrom(this.executablePath);
    if ((object) this.localServiceCache == null)
      this.localServiceCache = (T) this.localServicesAssembly.CreateInstance(this.rootTypeName);
    return this.localServiceCache;
  }
}
