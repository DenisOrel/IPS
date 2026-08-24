// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.TracingAllObjectsCompleteEventArgs
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Аргументы события от компонента "CompositionTracing".
/// Генерируется когда полностью завершается трассировка всех объектов
/// </summary>
public class TracingAllObjectsCompleteEventArgs : CompositionTracingEventArgs
{
  /// <summary>
  /// Коллекция обработанных объектов состава и результаты трассировки
  /// </summary>
  public Dictionary<CompositionObject, PdmCompositionBrowserJobStatus> Result;

  /// <summary>Создать пустые аргументы</summary>
  public TracingAllObjectsCompleteEventArgs()
  {
  }

  /// <summary>Создать заполненные аргументы</summary>
  /// <param name="result">Коллекция обработанных объектов состава и результаты трассировки</param>
  public TracingAllObjectsCompleteEventArgs(
    Dictionary<CompositionObject, PdmCompositionBrowserJobStatus> result)
  {
    this.Result = result;
  }
}
