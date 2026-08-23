// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.Graphs4TypeStruct
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Класс для хранения связки [ID типа объекта] - [Графы]</summary>
public class Graphs4TypeStruct
{
  private int _id = -1;
  private List<string> _graphs;

  /// <summary>Конструктор</summary>
  /// <param name="objectType">Тип объекта</param>
  /// <param name="graphs">Графы для подписей</param>
  public Graphs4TypeStruct(int objectType, List<string> graphs)
  {
    this._id = objectType;
    this._graphs = new List<string>((IEnumerable<string>) graphs);
  }

  /// <summary>Тип объекта</summary>
  public int ObjectType => this._id;

  /// <summary>Графы для подписей</summary>
  public List<string> Graphs => this._graphs;
}
