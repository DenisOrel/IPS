// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.SignParams
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Параметры вывода подписи</summary>
[Serializable]
public struct SignParams
{
  /// <summary>
  /// Имя параметра, в который будет передаваться фамилия из ЭП. (Графа для подписи.)
  /// </summary>
  private readonly string _signSurnameParam;
  /// <summary>Фамилия подписавшего</summary>
  private readonly string _surname;
  /// <summary>Имя параметра, в который передается значение подписи</summary>
  private readonly string _signValueParam;
  /// <summary>Значение подписи</summary>
  private readonly string _signValue;
  /// <summary>Дата подписи</summary>
  private readonly DateTime _signDate;
  /// <summary>Имя параметра, в который передается дата подписи</summary>
  private readonly string _signDateParam;
  /// <summary>Графа для подписи</summary>
  private readonly string _graphName;
  /// <summary>
  /// Имя параметра, в который передается наименование графы для подписи
  /// </summary>
  private readonly string _signGraphNameParam;
  /// <summary>Должность</summary>
  private readonly string _rank;
  /// <summary>
  /// Имя параметра, в который передается должность подписавшего
  /// </summary>
  private readonly string _signRankParam;
  /// <summary>Идентификатор версии объекта подписи</summary>
  private readonly long _signId;
  /// <summary>Идентификатор типа объекта подписи</summary>
  private readonly int _signObjType;
  /// <summary>
  /// Дата модификации подписи (судя по коду, НЕ то же самое, что дата подписи)
  /// </summary>
  private readonly DateTime _signModificationDate;
  /// <summary>Статус подписи</summary>
  private readonly SignStatuses _signStatus;
  /// <summary>
  /// Дата подписи как отформатированная по настройкам строка
  /// </summary>
  private readonly string _signDateAsFormattedString;

  /// <summary>Идентификатор версии объекта подписи</summary>
  public long SignObjectId => this._signId;

  /// <summary>
  /// Имя параметра, в который будет передаваться фамилия из ЭП. (Графа для подписи.)
  /// </summary>
  public string SignSurnameParam => this._signSurnameParam;

  /// <summary>Фамилия подписавшего</summary>
  public string Surname => this._surname;

  /// <summary>Имя параметра, в который передается значение подписи</summary>
  public string SignValueParam => this._signValueParam;

  /// <summary>Значение подписи</summary>
  public string SignValue => this._signValue;

  /// <summary>Дата подписи</summary>
  public DateTime SignDate => this._signDate;

  /// <summary>Имя параметра, в который передается дата подписи</summary>
  public string SignDateParam => this._signDateParam;

  /// <summary>Графа для подписи</summary>
  public string GraphName => this._graphName;

  /// <summary>
  /// Имя параметра, в который передается наименование графы подписи
  /// </summary>
  public string GraphNameParam => this._signGraphNameParam;

  /// <summary>Должность</summary>
  public string Rank => this._rank;

  /// <summary>
  /// Имя параметра, в который передается должность подписавшего
  /// </summary>
  public string RankParam => this._signRankParam;

  /// <summary>Дата модификации подписи</summary>
  public DateTime SignModificationDate => this._signModificationDate;

  /// <summary>Тип объекта подписи</summary>
  public int SignObjType => this._signObjType;

  /// <summary>Статус подписи</summary>
  public SignStatuses SignStatus => this._signStatus;

  /// <summary>Отформатированная по настройке дата подписи</summary>
  public string SignDateAsFormattedString => this._signDateAsFormattedString;

  /// <summary>Конструктор</summary>
  /// <param name="signSurnameParam">Параметр Графа для подписи</param>
  /// <param name="surname">Кто подписал</param>
  /// <param name="signValueParam">Параметр для хранения значения подписи.</param>
  /// <param name="signValue">Значение подписи</param>
  /// <param name="signDateParam">Параметр для хранения даты подписи</param>
  /// <param name="signDate">Дата подписи</param>
  /// <param name="graphName">Графа для подписи</param>
  /// <param name="rank">Должность</param>
  /// <param name="signObjectId">Идентификатор версии объекта</param>
  /// <param name="signStatus">Статус подписи</param>
  public SignParams(
    string signSurnameParam,
    string surname,
    string signValueParam,
    string signValue,
    string signDateParam,
    DateTime signDate,
    string graphName,
    string rank,
    long signObjectId,
    SignStatuses signStatus,
    string signDateAsFormattedString)
  {
    this._signSurnameParam = signSurnameParam;
    this._surname = surname;
    this._signValueParam = signValueParam;
    this._signValue = signValue;
    this._signDateParam = signDateParam;
    this._signDate = signDate;
    this._graphName = graphName;
    this._rank = rank;
    this._signId = signObjectId;
    this._signModificationDate = this._signDate;
    this._signStatus = signStatus;
    this._signObjType = SignsHolder.SignObjectTypeID;
    this._signGraphNameParam = string.Empty;
    this._signRankParam = string.Empty;
    this._signDateAsFormattedString = signDateAsFormattedString;
  }

  /// <summary>Конструктор</summary>
  /// <param name="signSurnameParam">Параметр Графа для подписи</param>
  /// <param name="surname">Кто подписал</param>
  /// <param name="signValueParam">Параметр для хранения значения подписи.</param>
  /// <param name="signValue">Значение подписи</param>
  /// <param name="signDateParam">Параметр для хранения даты подписи</param>
  /// <param name="signDate">Дата подписи</param>
  /// <param name="graphName">Графа для подписи</param>
  /// <param name="rank">Должность</param>
  /// <param name="signObjectId">Идентификатор версии объекта</param>
  /// <param name="signObjType">Идентификатор типа объекта</param>
  /// <param name="signModDate">Дата модификации подписи</param>
  /// <param name="signStatus">Статус подписи</param>
  public SignParams(
    string signSurnameParam,
    string surname,
    string signValueParam,
    string signValue,
    string signDateParam,
    DateTime signDate,
    string graphName,
    string rank,
    long signObjectId,
    int signObjType,
    DateTime signModDate,
    SignStatuses signStatus,
    string signDateAsFormattedString)
  {
    this._signSurnameParam = signSurnameParam;
    this._surname = surname;
    this._signValueParam = signValueParam;
    this._signValue = signValue;
    this._signDateParam = signDateParam;
    this._signDate = signDate;
    this._graphName = graphName;
    this._rank = rank;
    this._signId = signObjectId;
    this._signObjType = signObjType;
    this._signModificationDate = signModDate;
    this._signStatus = signStatus;
    this._signGraphNameParam = string.Empty;
    this._signRankParam = string.Empty;
    this._signDateAsFormattedString = signDateAsFormattedString;
  }

  /// <summary>Конструктор</summary>
  /// <param name="signSurnameParam">Параметр Графа для подписи</param>
  /// <param name="surname">Кто подписал</param>
  /// <param name="signValueParam">Параметр для хранения значения подписи.</param>
  /// <param name="signValue">Значение подписи</param>
  /// <param name="signDateParam">Параметр для хранения даты подписи</param>
  /// <param name="signDate">Дата подписи</param>
  /// <param name="signGraphNameParam">Параметр для хранения наименования графы</param>
  /// <param name="graphName">Графа для подписи</param>
  /// <param name="signRankParam">Параметр для хранения должности, в которой поставлена подпись</param>
  /// <param name="rank">Должность</param>
  /// <param name="signObjectId">Идентификатор версии объекта</param>
  /// <param name="signObjType">Идентификатор типа объекта</param>
  /// <param name="signModDate">Дата модификации подписи</param>
  /// <param name="signStatus">Статус подписи</param>
  public SignParams(
    string signSurnameParam,
    string surname,
    string signValueParam,
    string signValue,
    string signDateParam,
    DateTime signDate,
    string signGraphNameParam,
    string graphName,
    string signRankParam,
    string rank,
    long signObjectId,
    int signObjType,
    DateTime signModDate,
    SignStatuses signStatus,
    string signDateAsFormattedString)
  {
    this._signSurnameParam = signSurnameParam;
    this._surname = surname;
    this._signValueParam = signValueParam;
    this._signValue = signValue;
    this._signDateParam = signDateParam;
    this._signDate = signDate;
    this._graphName = graphName;
    this._rank = rank;
    this._signId = signObjectId;
    this._signObjType = signObjType;
    this._signModificationDate = signModDate;
    this._signStatus = signStatus;
    this._signGraphNameParam = signGraphNameParam;
    this._signRankParam = signRankParam;
    this._signDateAsFormattedString = signDateAsFormattedString;
  }

  /// <summary>Получить текущий статус данной подписи</summary>
  /// <param name="ius">Пользовательская сессия</param>
  /// <param name="objectId">ИД подписанного объекта</param>
  /// <returns>Статус подписи</returns>
  public SignStatuses GetSignStatus(IUserSession ius, long objectId)
  {
    DateTime asDateTime = ius.GetObject(objectId).GetAttributeByID(SignsHolder.ModifyDateAttrTypeID).AsDateTime;
    return SignHelper.TranslateStatus(ius, objectId, this._signId, this._signObjType, asDateTime, this._signModificationDate);
  }
}
