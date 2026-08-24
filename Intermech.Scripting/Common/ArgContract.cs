// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ArgContract
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.IO;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Предоставляет удобные методы для проверки аргументов методов исполнителя сценариев.
/// </summary>
public static class ArgContract
{
  /// <summary>Проверяет корректность имени сценария.</summary>
  /// <param name="scriptName">Имя сценария, в качестве которого можно использовать имя файла сценария</param>
  /// <exception cref="T:System.ArgumentException">Имя сценария не задано</exception>
  public static void CheckScriptName(string scriptName)
  {
    if (string.IsNullOrEmpty(scriptName))
      throw new ArgumentException("Имя сценария не задано.", nameof (scriptName));
  }

  /// <summary>Проверяет корректность имени файла сценария.</summary>
  /// <param name="scriptFileName">Имя файла сценария</param>
  /// <exception cref="T:System.ArgumentException">Имя файла сценария не задано</exception>
  public static void CheckScriptFileName(string scriptFileName)
  {
    if (string.IsNullOrEmpty(scriptFileName))
      throw new ArgumentException("Имя файла сценария не задано.", nameof (scriptFileName));
  }

  /// <summary>
  /// Проверяет корректность абсолютного пути к файлу сценария.
  /// </summary>
  /// <param name="scriptFullPath">Абсолютный путь к файлу сценария</param>
  /// <exception cref="T:System.ArgumentException">Абсолютный путь к файлу сценария не задано или не корректен</exception>
  public static void CheckScriptFullPath(string scriptFullPath)
  {
    if (string.IsNullOrEmpty(scriptFullPath))
      throw new ArgumentException("Путь к файлу сценария не задан.", nameof (scriptFullPath));
    if (!Path.IsPathRooted(scriptFullPath))
      throw new ArgumentException("Путь к файлу сценария должен быть абсолютным.", nameof (scriptFullPath));
  }

  /// <summary>Проверяет корректность пути к файлу сценария.</summary>
  /// <param name="scriptPath">Путь к файлу сценария. Может быть как абсолютным, так и относительным</param>
  /// <exception cref="T:System.ArgumentException">Путь к файлу сценария не задан или не корректен</exception>
  public static void CheckScriptPath(string scriptPath)
  {
    if (string.IsNullOrEmpty(scriptPath))
      throw new ArgumentException("Путь к файлу сценария не задан.", nameof (scriptPath));
  }

  /// <summary>Проверяет корректность кода сценария.</summary>
  /// <param name="scriptCode">Код сценария</param>
  /// <exception cref="T:System.ArgumentException">Код сценария не задан</exception>
  public static void CheckScriptCode(string scriptCode)
  {
    if (string.IsNullOrEmpty(scriptCode))
      throw new ArgumentException("Код сценария не задан.", nameof (scriptCode));
  }

  /// <summary>Проверяет корректность имени функции.</summary>
  /// <param name="functionName">Имя функции</param>
  /// <exception cref="T:System.ArgumentException">Имя функции некорректно</exception>
  internal static void CheckFunctionName(string functionName)
  {
    if (string.IsNullOrEmpty(functionName))
      throw new ArgumentException("Не задано имя функции, предоставляемой сценарием.", nameof (functionName));
  }

  /// <summary>
  /// Проверяет корректность имени типа создаваемого объекта.
  /// </summary>
  /// <param name="typeName">Имя типа типа создаваемого объекта</param>
  /// <exception cref="T:System.ArgumentException">Имя типа некорректно</exception>
  internal static void CheckTypeName(string typeName)
  {
    if (string.IsNullOrEmpty(typeName))
      throw new ArgumentException("Не задано имя типа, предоставляемого сценарием.", nameof (typeName));
  }
}
