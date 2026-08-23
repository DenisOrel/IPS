// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.IColumnCaptionsHelper
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Interfaces;

/// <summary>
/// интерфейс для возвращения названия столбца
/// по его имени в таблице
/// </summary>
public interface IColumnCaptionsHelper
{
  /// <summary>заполнить кэш соответсвия</summary>
  /// <returns></returns>
  Dictionary<string, string> FillColumnCaptionsCach();
}
