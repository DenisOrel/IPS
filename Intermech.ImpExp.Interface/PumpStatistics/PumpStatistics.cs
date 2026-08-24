// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.Interface.PumpStatistics;

/// <summary>Статистика работы отдельного пампера</summary>
public class PumpStatistics
{
  public PumpStatistics(Guid pumpGuid) => this.PumpGuid = pumpGuid;

  /// <summary>Уникальный идентификатор родительского пампера</summary>
  public Guid PumpGuid { get; }

  /// <summary>Строковая статистика</summary>
  public Dictionary<string, string> StringStat { get; } = new Dictionary<string, string>();

  /// <summary>Целочисленная статистика</summary>
  public Dictionary<string, int> IntStat { get; } = new Dictionary<string, int>();

  /// <summary>Очистить статистику</summary>
  public void Clear()
  {
    this.IntStat.Clear();
    this.StringStat.Clear();
  }

  /// <summary>Получить всю доступную статистику(ключи)</summary>
  /// <returns></returns>
  public IReadOnlyCollection<string> GetStatisticsKeys()
  {
    return (IReadOnlyCollection<string>) this.IntStat.Keys.Concat<string>((IEnumerable<string>) this.StringStat.Keys).ToList<string>();
  }

  /// <summary>
  /// Получить всю доступную целочисленную статистику(ключи)
  /// </summary>
  /// <returns></returns>
  public IReadOnlyCollection<string> GetIntStatisticsKeys()
  {
    return (IReadOnlyCollection<string>) this.IntStat.Keys.ToList<string>();
  }

  /// <summary>Получить всю доступную строковую статистику(ключи)</summary>
  /// <returns></returns>
  public IReadOnlyCollection<string> GetStrStatisticsKeys()
  {
    return (IReadOnlyCollection<string>) this.StringStat.Keys.ToList<string>();
  }

  /// <summary>Получить значение строковой статистики</summary>
  /// <param name="statisticsName">Наименование статистики</param>
  /// <param name="defaultValue">Значение по умолчанию, если статистика отсутствует</param>
  /// <returns></returns>
  public string GetStatisticsString(string statisticsName, string defaultValue = "")
  {
    string statisticsString;
    if (!this.StringStat.TryGetValue(statisticsName, out statisticsString))
      statisticsString = defaultValue;
    return statisticsString;
  }

  /// <summary>Занести значение строковой статистики</summary>
  /// <param name="statisticsName"></param>
  /// <param name="value"></param>
  public void SetStatisticsString(string statisticsName, string value)
  {
    this.StringStat[statisticsName] = value;
  }

  /// <summary>Получить значение целочисленной статистики.</summary>
  /// <param name="statisticsName">Наименование статистики</param>
  /// <param name="defaultValue">Значение по умолчанию, если статистика отсутствует</param>
  /// <returns></returns>
  public int GetStatisticsInt(string statisticsName, int defaultValue = 0)
  {
    int statisticsInt;
    if (!this.IntStat.TryGetValue(statisticsName, out statisticsInt))
      statisticsInt = defaultValue;
    return statisticsInt;
  }

  /// <summary>Задать значение строковой статистики</summary>
  /// <param name="statisticsName"></param>
  /// <param name="value"></param>
  public void SetStatisticsInt(string statisticsName, int value)
  {
    this.IntStat[statisticsName] = value;
  }

  /// <summary>Увеличить текущее значение на incValue</summary>
  /// <param name="statisticsName"></param>
  /// <param name="incValue"></param>
  public void IncrementStatisticsInt(string statisticsName, int incValue)
  {
    if (!this.IntStat.ContainsKey(statisticsName))
      this.IntStat[statisticsName] = incValue;
    else
      this.IntStat[statisticsName] += incValue;
  }

  /// <summary>Уменьшить текущее значение на decValue</summary>
  /// <param name="statisticsName"></param>
  /// <param name="incValue"></param>
  public void DecrementStatisticsInt(string statisticsName, int decValue)
  {
    if (!this.IntStat.ContainsKey(statisticsName))
      this.IntStat[statisticsName] = decValue;
    else
      this.IntStat[statisticsName] -= decValue;
  }
}
