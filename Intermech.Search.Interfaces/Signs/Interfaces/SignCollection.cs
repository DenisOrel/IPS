// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.SignCollection
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>
/// Класс, содержащий информацию необходимую для подписания объекта
/// </summary>
[Serializable]
public class SignCollection
{
  private long rankID;
  private List<long> listOfIDs = new List<long>();
  private string resolution = string.Empty;
  private List<string> listOfGraphs = new List<string>();
  private long userID;
  private string userName;
  private string password;

  /// <summary>Логин/отображаемое имя? пользователя</summary>
  public string UserName
  {
    get => this.userName;
    set => this.userName = value;
  }

  /// <summary>Пароль  (для подписания от имени)</summary>
  public string Password
  {
    get => this.password;
    set => this.password = value;
  }

  /// <summary>ID подписывающего пользователя</summary>
  public long UserID
  {
    get => this.userID;
    set => this.userID = value;
  }

  /// <summary>ID должности</summary>
  public long RankID
  {
    get => this.rankID;
    set => this.rankID = value;
  }

  /// <summary>Список ID-ков подписываемых объектов</summary>
  public List<long> ListOfIDs
  {
    get => this.listOfIDs;
    set => this.listOfIDs = value;
  }

  /// <summary>Резолюция</summary>
  public string Resolution
  {
    get => this.resolution;
    set => this.resolution = value;
  }

  /// <summary>Список граф, в которых ставится подпись</summary>
  public List<string> ListOfGraphs
  {
    get => this.listOfGraphs;
    set => this.listOfGraphs = value;
  }
}
