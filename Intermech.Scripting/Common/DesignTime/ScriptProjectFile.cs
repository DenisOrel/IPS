// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ScriptProjectFile
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Text;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Абстракция для файла сценария, открытого в IDE.
/// Реализация не обязана быть thread safe.
/// </summary>
public abstract class ScriptProjectFile
{
  /// <summary>Возвращает содержимое файла в виде массива байт.</summary>
  /// <returns>Содержимое файла</returns>
  public abstract byte[] GetContent();

  /// <summary>Возвращает содержимое файла в текста.</summary>
  /// <param name="encoding">Кодировка файла</param>
  /// <returns>Содержимое файла</returns>
  public abstract string GetContentAsText(Encoding encoding);

  /// <summary>Задает содержимое файла в виде массива байт.</summary>
  /// <param name="content">Содержимое файла</param>
  public abstract void SetContent(byte[] content);

  /// <summary>
  /// Задает содержимое файла в текста в указанной кодировке.
  /// </summary>
  /// <param name="text">Содержимое файла</param>
  /// <param name="encoding">Кодировка файла</param>
  public abstract void SetContentAsText(string text, Encoding encoding);

  /// <summary>Возвращает признак пустого файла.</summary>
  /// <returns>Признак пустого файла</returns>
  public abstract bool IsEmpty();
}
