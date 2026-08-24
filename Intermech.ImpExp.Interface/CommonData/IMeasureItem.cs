// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.IMeasureItem
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData;

/// <summary>
/// Интерфейс для представления информации о единице измерения
/// </summary>
public interface IMeasureItem
{
  /// <summary>
  /// Идентификатор единицы измерения в контексте текущего сеанса перекачки
  /// </summary>
  long Id { get; }

  /// <summary>Глобальный идентификатор единицы измерения</summary>
  Guid GUID { get; }

  /// <summary>Короткое имя единицы измерения (например, "кг")</summary>
  string ShortName { get; }

  /// <summary>Название единицы измерения (например, "килограмм")</summary>
  string LongName { get; }

  /// <summary>Коэффициент приведения к базовой единице измерения</summary>
  double Koef { get; }

  /// <summary>
  /// Идентификатор физической величины к которой относится данная ед. изм.
  /// </summary>
  long PhysicalValueID { get; }

  /// <summary>Идентификатор базовой ед. измерения</summary>
  long BaseMeasureId { get; }
}
