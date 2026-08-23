// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Interfaces.Copies.IInventoryNumberGenerator
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Search.Interfaces.Copies;

/// <summary>Служба для автоматической генерации номера ОТД</summary>
public interface IInventoryNumberGenerator
{
  /// <summary>вернуть  сгенерированный номер ОТД</summary>
  /// <param name="objectID">версия объекта, для которого будем генерировать номер</param>
  /// <param name="objectType">тип объекта, для которого будем генерировать номер</param>
  /// <param name="formula">сгенерированная формула</param>
  /// <returns> если в формуле были счётчики - запиши в словарь имя счётчика-его новое значение.
  /// нужно для восстановления значений счётчиков, если пользователь нажал Отмена в диалоге регистрации документа</returns>
  Dictionary<string, long> GenerateNumber(long objectID, int objectType, out string formula);

  /// <summary>Обработать формулу. Вернуть сгенерированный номер</summary>
  /// <param name="formula"> формула для генерации номера</param>
  /// <param name="objectID"> ID версии объекта</param>
  /// <param name="parentTypeID">тип объекта, для которого задана формула.
  /// может не совпадать с типом объекта, для которого генерится формула, это значит,
  /// что формула унаследована у указанного типа объектов</param>
  Dictionary<string, long> ParseFormula(ref string formula, long objectID, long parentTypeID);

  /// <summary>
  ///  восстанавливаем значение счётчика,
  ///  если пользователь нажал Отмена в диалоге присвоения инвентарного номера
  /// </summary>
  /// <param name="counters"> словарик  имя счётчика - текущее значение счётчика </param>
  void RestoreCounters(Dictionary<string, long> counters);
}
