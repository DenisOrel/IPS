// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.IPhysicalValueItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData;

/// <summary>
/// Интерфейс для представления информации о физической величине
/// </summary>
public interface IPhysicalValueItem
{
  /// <summary>
  /// Идентификатор физической величины в контексте текущего сеанса закачки
  /// </summary>
  long Id { get; }

  /// <summary>Наименование физической величины</summary>
  string Name { get; }

  /// <summary>
  /// Набор единиц измерения, относящихся к данной физической величине
  /// (хэш-таблица со значениями IMeasureItem.Id =&gt; IMeasureItem)
  /// </summary>
  Dictionary<long, IMeasureItem> Measures { get; set; }

  /// <summary>
  /// Иденитфткатор единицы измерения, используемой поумолчанию
  /// </summary>
  long DefaultMeasureID { get; set; }
}
