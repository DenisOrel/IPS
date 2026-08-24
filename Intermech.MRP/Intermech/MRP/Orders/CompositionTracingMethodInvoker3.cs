// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.CompositionTracingMethodInvoker3
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Делегат для вызова внутреннего метода по заполнению статусной панели
/// </summary>
/// <param name="totalObjects">Суммарное количество обработанных объектов</param>
/// <param name="progress">Текущее состояние для индикатора прогресса</param>
/// <param name="maxProgress">Максимальное значение для индикатора прогресса</param>
internal delegate void CompositionTracingMethodInvoker3(
  long totalObjects,
  int progress,
  int maxProgress);
