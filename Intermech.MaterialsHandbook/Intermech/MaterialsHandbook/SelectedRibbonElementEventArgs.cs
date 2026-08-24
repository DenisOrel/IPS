// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.SelectedRibbonElementEventArgs
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

#nullable disable
namespace Intermech.MaterialsHandbook;

public class SelectedRibbonElementEventArgs
{
  public int ColumnCount { get; set; }

  public SelectedElement Element { get; }

  public int ElementsCount { get; }

  public int Index { get; }

  public bool IsUnitedRow { get; set; }

  public int RowCount { get; set; }

  public SelectedRibbonElementEventArgs(SelectedElement element, int index, int elementsCount)
  {
    this.Element = element;
    this.Index = index;
    this.ElementsCount = elementsCount;
    this.ColumnCount = 0;
    this.RowCount = 0;
  }
}
