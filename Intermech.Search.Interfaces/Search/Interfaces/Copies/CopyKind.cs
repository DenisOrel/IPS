// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Interfaces.Copies.CopyKind
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

#nullable disable
namespace Intermech.Search.Interfaces.Copies;

/// <summary>Вид копии документа.</summary>
public enum CopyKind
{
  /// <summary>Пустой атрибут.</summary>
  Empty,
  /// <summary>Твердая копия (бумажная)</summary>
  Hard,
  /// <summary>Электронная копия</summary>
  Electronic,
}
