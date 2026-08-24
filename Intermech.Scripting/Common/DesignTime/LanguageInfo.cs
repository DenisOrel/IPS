// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.LanguageInfo
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Предоставляет информацию об имени языка сценариев, расширении его файлов и др.
/// Реализация является thread-safe.
/// </summary>
public sealed class LanguageInfo
{
  private readonly string name;
  private readonly string sourceExtension;
  private readonly bool isDynamic;

  /// <summary>Создает объект.</summary>
  /// <param name="name">Имя языка</param>
  /// <param name="sourceExtension">Расширение для файлов сценариев на этом языке. Должно начинаться с точки</param>
  /// <param name="isDynamic">Признак динамически типизированного языка</param>
  public LanguageInfo(string name, string sourceExtension, bool isDynamic)
  {
    if (string.IsNullOrEmpty(name))
      throw new ArgumentException("Не задано имя языка программирования", nameof (name));
    if (string.IsNullOrEmpty(sourceExtension))
      throw new ArgumentException("Не задано расширение для файлов сценариев.", nameof (sourceExtension));
    if (sourceExtension[0] != '.')
      throw new ArgumentException("Расширение для файлов сценариев должно начинаться с точки.", nameof (sourceExtension));
    this.name = name;
    this.sourceExtension = sourceExtension;
    this.isDynamic = isDynamic;
  }

  /// <summary>Возвращает имя языка программирования.</summary>
  public string Name => this.name;

  /// <summary>
  /// Возвращает расширение для файлов сценариев. Должно начинаться с точки.
  /// </summary>
  public string SourceExtension => this.sourceExtension;

  /// <summary>Возвращает признак динамически типизированного языка.</summary>
  public bool IsDynamic => this.isDynamic;

  /// <summary>Возвращает текстовое представление объекта.</summary>
  /// <returns>Строка и именем языка и расширением его файлов</returns>
  public override string ToString() => $"{this.name} (*{this.sourceExtension})";
}
