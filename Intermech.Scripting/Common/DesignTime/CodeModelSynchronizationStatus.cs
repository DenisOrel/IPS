// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.CodeModelSynchronizationStatus
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Статус синхронизации кодовой модели и редактора исходного кода.
/// </summary>
public enum CodeModelSynchronizationStatus
{
  /// <summary>
  /// Нет синхронизации, так как кодовой модели не был передан исходный код.
  /// </summary>
  NonSynchronized,
  /// <summary>
  /// Полная синхронизация: обновление кодовой модели и языковые запросы будут работать
  /// </summary>
  Synchronized,
  /// <summary>
  /// Потеря синхронизации: для использования кодовой модели требуется заново передать ей полный исходный код.
  /// </summary>
  SynchronizationLost,
}
