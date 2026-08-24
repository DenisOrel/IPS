// Decompiled with JetBrains decompiler
// Type: OxyPlot.SelectableElement
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OxyPlot;

public abstract class SelectableElement : Element
{
  private Selection selection;

  protected SelectableElement()
  {
    this.Selectable = true;
    this.SelectionMode = SelectionMode.All;
  }

  public event EventHandler SelectionChanged;

  public bool Selectable { get; set; }

  public SelectionMode SelectionMode { get; set; }

  protected OxyColor ActualSelectedColor
  {
    get
    {
      return this.Parent != null ? this.Parent.SelectionColor.GetActualColor(Model.DefaultSelectionColor) : Model.DefaultSelectionColor;
    }
  }

  public bool IsSelected() => this.selection != null;

  public IEnumerable<int> GetSelectedItems()
  {
    this.EnsureSelection();
    return this.selection.GetSelectedItems();
  }

  public void ClearSelection()
  {
    this.selection = (Selection) null;
    this.OnSelectionChanged();
  }

  public void Unselect()
  {
    this.selection = (Selection) null;
    this.OnSelectionChanged();
  }

  public bool IsItemSelected(int index)
  {
    if (this.selection == null)
      return false;
    return index == -1 ? this.selection.IsEverythingSelected() : this.selection.IsItemSelected(index);
  }

  public void Select()
  {
    this.selection = Selection.Everything;
    this.OnSelectionChanged();
  }

  public void SelectItem(int index)
  {
    if (this.SelectionMode == SelectionMode.All)
      throw new InvalidOperationException("Use the Select() method when using SelectionMode.All");
    this.EnsureSelection();
    if (this.SelectionMode == SelectionMode.Single)
      this.selection.Clear();
    this.selection.Select(index);
    this.OnSelectionChanged();
  }

  public void UnselectItem(int index)
  {
    if (this.SelectionMode == SelectionMode.All)
      throw new InvalidOperationException("Use the Unselect() method when using SelectionMode.All");
    this.EnsureSelection();
    this.selection.Unselect(index);
    this.OnSelectionChanged();
  }

  protected OxyColor GetSelectableColor(OxyColor originalColor, int index = -1)
  {
    if (originalColor.IsUndefined())
      return OxyColors.Undefined;
    return this.IsItemSelected(index) ? this.ActualSelectedColor : originalColor;
  }

  protected OxyColor GetSelectableFillColor(OxyColor originalColor, int index = -1)
  {
    return this.GetSelectableColor(originalColor, index);
  }

  private void EnsureSelection()
  {
    if (this.selection != null)
      return;
    this.selection = new Selection();
  }

  private void OnSelectionChanged(EventArgs args = null)
  {
    EventHandler selectionChanged = this.SelectionChanged;
    if (selectionChanged == null)
      return;
    selectionChanged((object) this, args);
  }
}
