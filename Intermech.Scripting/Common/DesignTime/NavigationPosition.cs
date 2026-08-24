// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.NavigationPosition
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Позиция в документе: номер линии и отступ от начала линии. Не zero-based
/// </summary>
[Serializable]
public class NavigationPosition : IComparable
{
  public NavigationPosition(int line, int character)
  {
    if (line <= 0)
      throw new ArgumentException(nameof (line));
    if (character <= 0)
      throw new ArgumentException(nameof (character));
    this.Line = line;
    this.Character = character;
  }

  public int Line { get; }

  public int Character { get; }

  public int CompareTo(object obj)
  {
    if (obj == null)
      return 1;
    if (!(obj is NavigationPosition navigationPosition))
      throw new ArgumentException("Сравниваемый объект не является объектом типа NavigationPosition", nameof (obj));
    int num1 = this.Line;
    int num2 = num1.CompareTo(navigationPosition.Line);
    if (num2 != 0)
      return num2;
    num1 = this.Character;
    return num1.CompareTo(navigationPosition.Character);
  }
}
