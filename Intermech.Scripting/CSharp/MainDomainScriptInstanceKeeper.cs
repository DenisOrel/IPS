// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.MainDomainScriptInstanceKeeper
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Reflection;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Объект-хранитель, содержащий проинициализированный и готовый к использованию объект сценария C#.
/// </summary>
/// <remarks>
/// Реализация не является thread safe. Объекты-хранители и содержащиеся в них объекты сценариев
/// привязаны к потоку выполнения (thread), в котором они были созданы, и могут использоваться
/// только из этого потока.
/// </remarks>
internal sealed class MainDomainScriptInstanceKeeper : ScriptObjectKeeper
{
  private MainDomainScriptRuntimeHelper scriptRuntimeHelper;

  internal MainDomainScriptInstanceKeeper(
    int initialThreadId,
    object scriptInstance,
    MainDomainScriptRuntimeHelper scriptRuntimeHelper)
    : base(initialThreadId, scriptInstance)
  {
    this.scriptRuntimeHelper = scriptRuntimeHelper;
  }

  /// <summary>
  /// Освобождает ресурсы сценария C# и очищает объект сценария.
  /// </summary>
  /// <param name="scriptObject">Объект сценария</param>
  protected override void DoDispose(object scriptObject)
  {
    this.scriptRuntimeHelper.ScriptContextProperty.SetValue(scriptObject, (object) null);
    if (!this.scriptRuntimeHelper.HasServiceProperties)
      return;
    foreach (PropertyInfo serviceProperty in this.scriptRuntimeHelper.ServiceProperties)
      serviceProperty.SetValue(scriptObject, (object) null);
  }
}
