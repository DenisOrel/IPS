// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.UserRankInformation
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>
/// Класс с информацией о должности
/// (содержит графы)
/// </summary>
public class UserRankInformation
{
  private long _rankID;
  private string _rankCaption = string.Empty;
  private List<string> _graphs;

  private UserRankInformation()
  {
  }

  /// <summary>Конструктор</summary>
  /// <param name="rankID">Идентификатор должности</param>
  public UserRankInformation(long rankID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(rankID);
      this._rankID = rankID;
      this._rankCaption = objectInfo.Caption;
      this._graphs = new List<string>();
    }
  }

  /// <summary>Идентификатор должности</summary>
  public long RankID => this._rankID;

  /// <summary>Наименование должности</summary>
  public string RankCaption => this._rankCaption;

  /// <summary>Графы для подписи</summary>
  public List<string> Graphs => this._graphs;

  /// <summary>Клонирование объекта</summary>
  /// <param name="info">Исходный объекта</param>
  /// <returns>Новый объект</returns>
  public static UserRankInformation Clone(UserRankInformation info)
  {
    return new UserRankInformation()
    {
      _rankID = info._rankID,
      _rankCaption = info._rankCaption,
      _graphs = new List<string>((IEnumerable<string>) info._graphs)
    };
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode() => base.GetHashCode();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public override bool Equals(object obj)
  {
    return obj is UserRankInformation ? (obj as UserRankInformation)._rankID.Equals(this._rankID) : base.Equals(obj);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  public override string ToString() => this._rankCaption;
}
