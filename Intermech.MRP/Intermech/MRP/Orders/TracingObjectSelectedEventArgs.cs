// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.TracingObjectSelectedEventArgs
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces.Compositions;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Аргументы события от компонента "CompositionTracing".
/// Генерируется когда в гриде выбрана строка, содержащая ссылку на объект состава,
/// который требуется отобразить в дереве Навигатора и на закладках
/// </summary>
public class TracingObjectSelectedEventArgs : CompositionTracingEventArgs
{
  /// <summary>
  /// Родительский объект состава, который трассировался.
  /// Именно его дочерний объект состава представлен в данных аргументах
  /// </summary>
  public CompositionObject Parent;
  /// <summary>
  /// Путь к дочернему узлу от корневого узла конфигурируемого состава
  /// </summary>
  public RelationPath Path;

  /// <summary>Создать пустые аргументы</summary>
  public TracingObjectSelectedEventArgs()
  {
  }

  /// <summary>Создать заполненные аргументы</summary>
  /// <param name="parent">Родительский объект состава, который трассировался.
  /// Именно его дочерний объект состава представлен в данных аргументах</param>
  /// <param name="path">Путь к дочернему узлу от корневого узла конфигурируемого состава</param>
  public TracingObjectSelectedEventArgs(CompositionObject parent, RelationPath path)
  {
    this.Parent = parent;
    this.Path = path;
  }
}
