// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.SelectedCoatingAndGlueAnalizer
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Navigator.Controls;
using System;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class SelectedCoatingAndGlueAnalizer : SelectedItemsAnalyzer
{
  public override SelectedItemsAnalyzerResult Analyze(
    ISelectionWindow sender,
    ISelectedItemsHost itemsHost)
  {
    SelectedItemsAnalyzerResult itemsAnalyzerResult = base.Analyze(sender, itemsHost);
    if (itemsAnalyzerResult == SelectedItemsAnalyzerResult.Enabled)
      itemsAnalyzerResult = !string.IsNullOrEmpty((!(itemsHost.SelectedItems is IMHView.IMHSelectedItems selectedItems) || !selectedItems.Selectable ? (IMHMaterialRecordID) null : selectedItems.GetItemData(0, (Type) null) as IMHMaterialRecordID)?.Designation) ? SelectedItemsAnalyzerResult.Enabled : SelectedItemsAnalyzerResult.Disabled;
    return itemsAnalyzerResult;
  }
}
