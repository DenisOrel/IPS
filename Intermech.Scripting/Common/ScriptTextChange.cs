// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ScriptTextChange
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>Содержит информацию об изменении в сценарии</summary>
[Serializable]
public class ScriptTextChange : IEquatable<ScriptTextChange>
{
  /// <summary>Создает объект.</summary>
  /// <exception cref="T:System.ArgumentOutOfRangeException">Значения  параметров <paramref name="offset" /> и/или <paramref name="removedLength" /> имеют отрицательное значение</exception>
  /// <exception cref="T:System.ArgumentNullException">Значение параметра <paramref name="insertedText" /> равно null</exception>
  public ScriptTextChange(int offset, int removedLength, string insertedText)
  {
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (removedLength < 0)
      throw new ArgumentOutOfRangeException(nameof (removedLength));
    if (insertedText == null)
      throw new ArgumentNullException(nameof (insertedText));
    this.Offset = offset;
    this.RemovedLength = removedLength;
    this.InsertedText = insertedText;
  }

  /// <summary>Отступ первого символа изменения от начала документа</summary>
  public int Offset { get; }

  /// <summary>Длина удаленного изменением текста</summary>
  public int RemovedLength { get; }

  /// <summary>Вставленный данным изменением текст</summary>
  public string InsertedText { get; }

  /// <summary>
  /// Проверяет эквивалентность текущего и указанного объектов.
  /// </summary>
  /// <param name="other">Другой объект</param>
  /// <returns>true, если объекты эквивалентны, а иначе - false</returns>
  public bool Equals(ScriptTextChange other)
  {
    return other != null && this.Offset == other.Offset && this.InsertedText == other.InsertedText && this.RemovedLength == other.RemovedLength;
  }

  /// <summary>
  /// Проверяет эквивалентность текущего и указанного объектов.
  /// </summary>
  /// <param name="other">Другой объект</param>
  /// <returns>true, если объекты эквивалентны, а иначе - false</returns>
  public override bool Equals(object obj)
  {
    return !(obj is ScriptTextChange other) ? base.Equals(obj) : this.Equals(other);
  }

  /// <summary>Возвращает хеш-код текущего объекта.</summary>
  /// <returns>хеш-код текущего объекта</returns>
  public override int GetHashCode()
  {
    return ((2091745087 * -1521134295 + this.Offset) * -1521134295 + this.RemovedLength) * -1521134295 + this.InsertedText.GetHashCode();
  }
}
