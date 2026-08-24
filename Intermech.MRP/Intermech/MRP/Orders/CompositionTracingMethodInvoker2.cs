// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.CompositionTracingMethodInvoker2
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Делегат для вызова внутреннего метода по заполнению грида из фонового потока
/// </summary>
/// <param name="obj">Родительский объект</param>
/// <param name="status">Результат трассировки его состава</param>
internal delegate void CompositionTracingMethodInvoker2(
  CompositionObject obj,
  PdmCompositionBrowserJobStatus status);
