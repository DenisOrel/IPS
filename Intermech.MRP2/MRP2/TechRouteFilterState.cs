// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.TechRouteFilterState
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

#nullable disable
namespace Intermech.MRP2;

/// <summary>
/// тип фильтрации маршрутов обработки, отключена/включена/включена_с_МО_по_умолчанию
/// </summary>
public enum TechRouteFilterState
{
  /// <summary>фильтрация МО отключена</summary>
  trfDisabled,
  /// <summary>фильтрация МО включена</summary>
  trfEnabled,
  /// <summary>
  /// фильрация МО включена, но если ничего нет то показывать маршрут по умолчанию
  /// </summary>
  trfWithDefault,
}
