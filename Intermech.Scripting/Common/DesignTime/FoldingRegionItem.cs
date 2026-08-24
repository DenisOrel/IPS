// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.FoldingRegionItem
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Класс-описатель содержащий информацию для свёртки регионов кода
/// </summary>
[Serializable]
public class FoldingRegionItem
{
  /// <summary>
  /// Начало расположения директивы региона в тексте сценария
  /// </summary>
  public int StartOffset { get; private set; }

  /// <summary>
  /// Имя региона которое будет отображаться после его сворачивания
  /// </summary>
  public string RegionName { get; private set; }

  /// <summary>
  /// Конец расположения директивы региона в тексте сценария
  /// </summary>
  public int EndOffset { get; private set; }

  public FoldingRegionItem(int startOffset, int endOffset, string regionName)
  {
    if (startOffset < 0)
      throw new ArgumentException("Начало расположения директивы региона не может быть отрицательным.", nameof (startOffset));
    if (endOffset < 0)
      throw new ArgumentException("Конец расположения директивы региона не может быть отрицательным.", nameof (endOffset));
    if (endOffset <= startOffset)
      throw new ArgumentException("Конец расположения директивы региона не может быть меньше или равен началу.", nameof (endOffset));
    if (string.IsNullOrEmpty(regionName))
      throw new ArgumentNullException(nameof (regionName));
    this.StartOffset = startOffset;
    this.EndOffset = endOffset;
    this.RegionName = regionName;
  }
}
