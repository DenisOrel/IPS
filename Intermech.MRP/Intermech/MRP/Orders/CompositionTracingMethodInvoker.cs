// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.CompositionTracingMethodInvoker
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Делегат для вызова внутренних методов контрола из фонового потока
/// </summary>
/// <param name="args"></param>
internal delegate void CompositionTracingMethodInvoker(CompositionTracingEventArgs args);
