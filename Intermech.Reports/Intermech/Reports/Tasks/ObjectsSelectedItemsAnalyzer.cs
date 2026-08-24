// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Tasks.ObjectsSelectedItemsAnalyzer
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.DataFormats;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Reports.Tasks;

/// <summary>
/// Анализатор разрешает кнопку "ОК" только в том случае, если выделены объекты из списка допустимых
/// </summary>
internal class ObjectsSelectedItemsAnalyzer : SelectedItemsAnalyzer
{
  /// <summary>Список допустимых к выбору объектов</summary>
  private readonly List<long> _objectIds = new List<long>();

  /// <summary>
  /// Создать анализатор, добавить в список к выбору только указанные объекты
  /// </summary>
  /// <param name="objectType">Разрешённый к выбору объекты</param>
  public ObjectsSelectedItemsAnalyzer(params long[] objectIds)
  {
    if (objectIds == null)
      return;
    this._objectIds.AddRange((IEnumerable<long>) objectIds);
    this._objectIds.Sort();
  }

  /// <summary>
  /// Выполнить анализирование указанной коллекции элементов, выделенных в окне
  /// </summary>
  /// <param name="sender">Окно, в котором осуществляется выбор элементов</param>
  /// <param name="itemsHost">Служба окна, которая предоставляет коллекцию выделенных элементов</param>
  /// <returns>Результат проверки</returns>
  public override SelectedItemsAnalyzerResult Analyze(
    ISelectionWindow sender,
    ISelectedItemsHost itemsHost)
  {
    SelectedItemsAnalyzerResult itemsAnalyzerResult = base.Analyze(sender, itemsHost);
    if (itemsAnalyzerResult == SelectedItemsAnalyzerResult.Disabled)
      return itemsAnalyzerResult;
    ISelectedItems selectedItems = itemsHost.SelectedItems;
    for (int index = 0; index < selectedItems.Count; ++index)
    {
      if (!(selectedItems.GetItemData(index, typeof (IDBObjectID)) is IDBObjectID itemData) || this._objectIds.BinarySearch(itemData.Value) < 0)
        return SelectedItemsAnalyzerResult.Disabled;
    }
    return SelectedItemsAnalyzerResult.Enabled;
  }
}
