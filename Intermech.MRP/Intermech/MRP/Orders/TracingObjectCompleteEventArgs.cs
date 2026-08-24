// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.TracingObjectCompleteEventArgs
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Аргументы события от компонента "CompositionTracing".
/// Генерируется когда завершается трассировка состава очередного объекта
/// </summary>
public class TracingObjectCompleteEventArgs : CompositionTracingEventArgs
{
  /// <summary>
  /// Ключ для корневого объекта конфигурируемого состава, который использовался при трассировке
  /// </summary>
  public RelationPair RootKey;
  /// <summary>Объект состава, который трассировался</summary>
  public CompositionObject Object;
  /// <summary>
  /// Полный путь к трассируемому объекту от корневого объекта конфигурируемого состава
  /// </summary>
  public RelationPath ObjectPath;
  /// <summary>Результаты трассировки указанного объекта</summary>
  public PdmCompositionBrowserJobStatus JobStatus;

  /// <summary>Создать пустые аргументы</summary>
  public TracingObjectCompleteEventArgs()
  {
  }

  /// <summary>Создать заполненные аргументы</summary>
  /// <param name="rootKey">Ключ для корневого объекта конфигурируемого состава, который использовался при трассировке</param>
  /// <param name="obj">Объект состава, который трассировался</param>
  /// <param name="objPath">Полный путь к трассируемому объекту от корневого объекта конфигурируемого состава</param>
  /// <param name="jobStatus">Результаты трассировки указанного объекта</param>
  public TracingObjectCompleteEventArgs(
    RelationPair rootKey,
    CompositionObject obj,
    RelationPath objPath,
    PdmCompositionBrowserJobStatus jobStatus)
  {
    this.RootKey = rootKey;
    this.Object = obj;
    this.ObjectPath = objPath;
    this.JobStatus = jobStatus;
  }
}
