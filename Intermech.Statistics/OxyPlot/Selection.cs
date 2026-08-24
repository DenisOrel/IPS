// Decompiled with JetBrains decompiler
// Type: OxyPlot.Selection
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OxyPlot;

public class Selection
{
  private static readonly Selection EverythingSelection = new Selection();
  private readonly Dictionary<Selection.SelectionItem, bool> selection = new Dictionary<Selection.SelectionItem, bool>();

  public static Selection Everything => Selection.EverythingSelection;

  public bool IsEverythingSelected() => this == Selection.EverythingSelection;

  public IEnumerable<int> GetSelectedItems()
  {
    return this.selection.Keys.Select<Selection.SelectionItem, int>((Func<Selection.SelectionItem, int>) (si => si.Index));
  }

  public IEnumerable<int> GetSelectedItems(Enum feature)
  {
    return this.selection.Keys.Where<Selection.SelectionItem>((Func<Selection.SelectionItem, bool>) (si => object.Equals((object) si.Feature, (object) feature))).Select<Selection.SelectionItem, int>((Func<Selection.SelectionItem, int>) (si => si.Index));
  }

  public void Clear() => this.selection.Clear();

  public bool IsItemSelected(int index, Enum feature = null)
  {
    return this.IsEverythingSelected() || this.selection.ContainsKey(new Selection.SelectionItem(index, feature));
  }

  public void Select(int index, Enum feature = null)
  {
    this.selection[new Selection.SelectionItem(index, feature)] = true;
  }

  public void Unselect(int index, Enum feature = null)
  {
    Selection.SelectionItem key = new Selection.SelectionItem(index, feature);
    if (!this.selection.ContainsKey(key))
      throw new InvalidOperationException($"Item {(object) index} and feature {(object) feature} is not selected. Cannot unselect.");
    this.selection.Remove(key);
  }

  public struct SelectionItem(int index, Enum feature) : IEquatable<Selection.SelectionItem>
  {
    private readonly int index = index;
    private readonly Enum feature = feature;

    public int Index => this.index;

    public Enum Feature => this.feature;

    public bool Equals(Selection.SelectionItem other)
    {
      return other.index == this.index && object.Equals((object) other.feature, (object) this.feature);
    }

    public override int GetHashCode()
    {
      return this.feature == null ? this.index.GetHashCode() : this.index.GetHashCode() ^ this.feature.GetHashCode();
    }
  }
}
