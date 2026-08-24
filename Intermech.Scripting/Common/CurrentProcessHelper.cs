// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.CurrentProcessHelper
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common;

internal static class CurrentProcessHelper
{
  private static readonly CurrentProcessService currentProcessService = new CurrentProcessService();

  /// <summary>Возвращает идентификатор текущего процесса.</summary>
  public static int ProcessId => CurrentProcessHelper.currentProcessService.ProcessId;

  /// <summary>
  /// Возвращает уникальный ключ расположения текущего процесса на диске.
  /// </summary>
  public static string LocationKey => CurrentProcessHelper.currentProcessService.LocationKey;
}
