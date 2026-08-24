// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IAttributePossibleValue
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

/// <summary>
/// Интерфейс для представления допустимого значения атрибута (для атрибута со значениями из списка)
/// </summary>
public interface IAttributePossibleValue
{
  /// <summary>Номер значения в списке</summary>
  int InListId { get; }

  /// <summary>Описание значения ()</summary>
  string Description { get; }

  /// <summary>Строковое значение</summary>
  string ValueString { get; }

  /// <summary>Целочисленное значение</summary>
  int ValueInteger { get; }

  /// <summary>Вещественнное значение</summary>
  double ValueDouble { get; }

  /// <summary>Значение в формате дата/время</summary>
  DateTime ValueDateTime { get; }
}
