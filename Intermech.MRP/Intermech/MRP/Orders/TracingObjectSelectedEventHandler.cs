// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.TracingObjectSelectedEventHandler
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Делегат события от компонента "CompositionTracing".
/// Генерируется когда в гриде выбрана строка, содержащая ссылку на объект состава,
/// который требуется отобразить в дереве Навигатора и на закладках
/// </summary>
/// <param name="sender">Отправитель события</param>
/// <param name="args">Аргументы события</param>
public delegate void TracingObjectSelectedEventHandler(
  object sender,
  TracingObjectSelectedEventArgs args);
