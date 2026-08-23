// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Interfaces.DocGroupType
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

#nullable disable
namespace Intermech.Search.Interfaces;

/// <summary>группирующий признак</summary>
public enum DocGroupType
{
  /// <summary>обычный документ</summary>
  None,
  /// <summary>документ-извещение</summary>
  Eco,
  /// <summary>документ с составом</summary>
  Composition,
}
