// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.NavigationRange
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Диапазон в документе. Содержит позиции начала и конца диапазона
/// </summary>
[Serializable]
public class NavigationRange : IComparable
{
  public NavigationRange(NavigationPosition start, NavigationPosition end)
  {
    if (start == null)
      throw new ArgumentNullException(nameof (start));
    if (end == null)
      throw new ArgumentNullException(nameof (end));
    this.Start = start;
    this.End = end;
  }

  public NavigationPosition Start { get; }

  public NavigationPosition End { get; }

  public int CompareTo(object obj)
  {
    if (obj == null)
      return 1;
    int num = obj is NavigationRange navigationRange ? this.Start.CompareTo((object) navigationRange.Start) : throw new ArgumentException("Сравниваемый объект не является объектом типа NavigationRange", nameof (obj));
    return num == 0 ? this.End.CompareTo((object) navigationRange.End) : num;
  }
}
