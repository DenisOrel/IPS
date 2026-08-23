// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.OpenKeyType
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>тип открытого ключа</summary>
public enum OpenKeyType
{
  /// <summary>
  ///  простой тип ключа
  /// формат guid_провайдера:ключ
  /// </summary>
  Simple,
  /// <summary>
  /// расширенная (и, пока, стандартная) версия ключа
  /// формат  имя_контейнера:guid_провайдера:ключ
  /// </summary>
  Extended,
}
