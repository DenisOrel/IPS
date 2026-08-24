// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.NavigationItem
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Класс-описатель типов и элементов, которые могут встретиться в сценариях
/// </summary>
[Serializable]
public class NavigationItem
{
  public NavigationItem(
    string fullName,
    NavigationRange range,
    NavigationRange selectionRange,
    CodeCompletionItemType type)
    : this(fullName, range, selectionRange, type, new List<NavigationItem>())
  {
  }

  public NavigationItem(
    string fullName,
    NavigationRange range,
    NavigationRange selectionRange,
    CodeCompletionItemType type,
    List<NavigationItem> children)
  {
    if (string.IsNullOrEmpty(fullName))
      throw new ArgumentNullException(nameof (fullName));
    if (range == null)
      throw new ArgumentNullException(nameof (range));
    if (selectionRange == null)
      throw new ArgumentNullException(nameof (selectionRange));
    if (children == null)
      throw new ArgumentNullException(nameof (children));
    this.FullName = fullName;
    this.Type = type;
    this.Range = range;
    this.SelectionRange = selectionRange;
    this.Children = children;
  }

  public string FullName { get; private set; }

  public CodeCompletionItemType Type { get; private set; }

  /// <summary>
  /// Диапазон, содержащий полное определение описываемого символа
  /// </summary>
  public NavigationRange Range { get; private set; }

  /// <summary>Диапазон, указывающий расположение имени символа</summary>
  public NavigationRange SelectionRange { get; private set; }

  /// <summary>
  /// Список элементов, относящихся к данному типу. Для элементов список пуст
  /// </summary>
  public List<NavigationItem> Children { get; private set; }

  public void CopyData(NavigationItem navigationItem)
  {
    this.Type = navigationItem != null ? navigationItem.Type : throw new ArgumentNullException(nameof (navigationItem));
    this.Range = navigationItem.Range;
    this.SelectionRange = navigationItem.SelectionRange;
    this.Children.Clear();
    this.Children.AddRange((IEnumerable<NavigationItem>) navigationItem.Children);
  }

  public override bool Equals(object obj)
  {
    return obj is NavigationItem navigationItem && this.FullName == navigationItem.FullName;
  }

  public override int GetHashCode()
  {
    return 733961487 + EqualityComparer<string>.Default.GetHashCode(this.FullName);
  }

  public override string ToString() => this.FullName;
}
