// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.ISignsService
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Интерфейс серверной части службы подписей</summary>
public interface ISignsService
{
  /// <summary>
  /// Подписать объекты (создаются объекты подписей)
  /// Для внутренних подписей и криптоподписей.
  /// Для криптоподписей требуется дополнительное заполнение подписей на стороне клиента.
  /// </summary>
  /// <param name="infoForSigning"> Информация необходимая для подписания объекта </param>
  /// <param name="sessionGuid">Guid сессии</param>
  /// <param name="signTypeID"> тип объекта- подписи </param>
  /// <returns>Связка [идентификатор объекта] - [идентификаторы объектов-подписей]</returns>
  Dictionary<long, List<long>> Sign(
    SignCollection infoForSigning,
    Guid sessionGuid,
    int signTypeID);

  /// <summary>
  /// Подписать объекты от имени (создаются объекты подписей).
  /// Только для внутренних подписей.
  /// </summary>
  /// <param name="infoForSigning"> Информация необходимая для подписания объекта </param>
  /// <param name="sessionGuid"> Guid сессии</param>
  /// <returns>Связка [идентификатор объекта] - [идентификатор объекта-подписи]</returns>
  Dictionary<long, List<long>> SignAs(SignCollection infoForSigning, Guid sessionGuid);

  /// <summary>
  /// Проверить наличие всех подписей в базе для объектов (для завершения редактирования)
  /// </summary>
  /// <param name="objectIDs">Список ID объектов</param>
  /// <param name="gSet">Настройка подписей для проверки, null для того чтобы брать из базы</param>
  /// <param name="sessionGuid">Guid сессии</param>
  /// <param name="raiseException">Генерировать Exception при ошибке</param>
  /// <returns>true - при наличии всех подписей</returns>
  bool CheckSigns(long[] objectIDs, GraphsSet gSet, Guid sessionGuid, bool raiseException);

  /// <summary>
  /// Проверить наличие всех подписей в базе для объектов (для завершения редактирования)
  /// </summary>
  /// <param name="objectIDs">Список ID объектов</param>
  /// <param name="gSet">Настройка подписей для проверки, null для того чтобы брать из базы</param>
  /// <param name="sessionGuid">Guid сессии</param>
  /// <param name="raiseException">Генерировать Exception при ошибке</param>
  /// <param name="errorMessage">Текст ошибки</param>
  /// <param name="additionalInfo">Зарезервировано</param>
  /// <returns>true - при наличии всех подписей</returns>
  bool CheckSigns(
    long[] objectIDs,
    GraphsSet gSet,
    Guid sessionGuid,
    bool raiseException,
    out string errorMessage,
    out object[] additionalInfo);

  /// <summary>
  /// Проверить наличие подписей в базе для объектов для указанного пользователя
  /// </summary>
  /// <param name="objectIDs">Список ID объектов</param>
  /// <param name="gSet">Настройка подписей для проверки, null для того чтобы брать из базы</param>
  /// <param name="userID">id пользователя; -1 - подписи всех пользователей</param>
  /// <param name="sessionGuid">Guid сессии</param>
  /// <param name="raiseException">Генерировать Exception при ошибке</param>
  /// <returns>true - при наличии всех подписей</returns>
  bool CheckSigns(
    long[] objectIDs,
    GraphsSet gSet,
    long userID,
    Guid sessionGuid,
    bool raiseException);

  /// <summary>
  /// Проверить наличие подписей в базе для объектов для указанного пользователя
  /// </summary>
  /// <param name="objectIDs">Список ID объектов</param>
  /// <param name="gSet">Настройка подписей для проверки, null для того чтобы брать из базы</param>
  /// <param name="userID">id пользователя; -1 - подписи всех пользователей</param>
  /// <param name="sessionGuid">Guid сессии</param>
  /// <param name="raiseException">Генерировать Exception при ошибке</param>
  /// <param name="errorMessage">Текст ошибки</param>
  /// <param name="additionalInfo">Зарезервировано</param>
  /// <returns>true - при наличии всех подписей</returns>
  bool CheckSigns(
    long[] objectIDs,
    GraphsSet gSet,
    long userID,
    Guid sessionGuid,
    bool raiseException,
    out string errorMessage,
    out object[] additionalInfo);

  /// <summary>
  /// Проверить наличие всех подписей в базе для объектов (для помещения в архив)
  /// </summary>
  /// <param name="objectIDs">Список ID объектов</param>
  /// <param name="archiveID">ID архива</param>
  /// <param name="gSet">Настройка подписей для проверки, null для того чтобы брать из базы</param>
  /// <param name="sessionGuid">Guid сессии</param>
  /// <param name="raiseException">Генерировать Exception</param>
  /// <param name="change"> изменение контроля подписей</param>
  /// <returns>true - при наличии всех подписей</returns>
  bool CheckSigns(
    long[] objectIDs,
    long archiveID,
    GraphsSet gSet,
    Guid sessionGuid,
    bool raiseException,
    bool change);

  bool CheckSigns(
    long[] objectIDs,
    long archiveID,
    GraphsSet gSet,
    Guid sessionGuid,
    bool raiseException,
    bool change,
    out string errorMessage,
    out object[] additionalInfo);

  /// <summary>Проверяет наличие подписей для объектов</summary>
  /// <param name="objectIDs">Объекты для проверки</param>
  /// <param name="sessionGuid">Guid сессии (отсюда берется UserID)</param>
  /// <param name="useStrongCheck4All">Использовать строгий контроль для всех граф</param>
  /// <returns>true - если хотя бы одна подпись корректна для каждого из объектов</returns>
  bool CheckSignsEx(long[] objectIDs, Guid sessionGuid, bool useStrongCheck4All);

  /// <summary>Проверяет наличие подписей для объектов</summary>
  /// <param name="objectIDs">Объекты для проверки</param>
  /// <param name="sessionGuid">Guid сессии</param>
  /// <param name="userID">Идентификатор пользователя для проверки</param>
  /// <param name="useStrongCheck4All">Использовать строгий контроль для все граф</param>
  /// <returns>true - если хотя бы одна подпись корректна для каждого из объектов</returns>
  bool CheckSignsEx(long[] objectIDs, Guid sessionGuid, long userID, bool useStrongCheck4All);

  /// <summary>
  /// Очистка серверного кэша.
  /// Функция обслуживания.
  /// </summary>
  void CleanCache();

  /// <summary>
  /// Получить хэш подписи.
  /// Только для внутренних подписей.
  /// </summary>
  /// <param name="signObjectId">Идентификатор версии объекта подписи</param>
  /// <returns>Строка с хэшем</returns>
  string GetSignHash(long signObjectId, Guid sessionGuid);

  /// <summary>
  /// Проверить правильность подписи. Вызывать при проверке подписей на valid.
  /// Для внутренних подписей и криптоподписей.
  /// </summary>
  /// <param name="objectID"> Идентификатор подписанного объекта</param>
  /// <param name="signObjectID">Идентификатор объекта Подпись</param>
  /// <param name="sessionGuid">Guid сессии</param>
  /// <returns>true - если хэш-код правильный</returns>
  bool CheckHashCode(long objectID, long signObjectID, Guid sessionGuid);

  /// <summary>
  /// Проверить правильность подписи. Вызывать при проверке подписей на valid.
  /// Для внутренних подписей и криптоподписей.
  /// </summary>
  /// <param name="objectID"> Идентификатор подписанного объекта</param>
  /// <param name="signObjectID">Идентификатор объекта Подпись</param>
  /// <param name="sessionGuid">Guid сессии</param>
  /// <param name="certificatesRawData">сертификаты подписанта, если криптоподпись</param>
  /// <returns>true - если хэш-код правильный</returns>
  bool CheckHashCode(
    long objectID,
    long signObjectID,
    Guid sessionGuid,
    out byte[] certificatesRawData);

  /// <summary>
  /// Копировать подписи у родительского объекта при создании версии.
  /// Только для внутренних подписей.
  /// </summary>
  /// <param name="createdObject">Созданный объект</param>
  /// <param name="session"></param>
  void CreateCopySigns(IDBObject createdObject, IUserSession session);

  /// <summary>
  /// Имеет ли объект подписи.
  /// Для внутренних подписей и криптоподписей.
  /// </summary>
  /// <param name="objectID">ID объекта.</param>
  /// <param name="session">Сессия.</param>
  /// <returns>true - если у объекта есть подписи</returns>
  bool HasSignedGraphs(long objectID, IUserSession session);

  /// <summary>
  /// Имеет ли объект подписи от имени конкретного пользователя.
  /// Для внутренних подписей и криптоподписей.
  /// </summary>
  /// <param name="objectID">ID объекта.</param>
  /// <param name="userID">ID подписавшего; -1 - все пользователи</param>
  /// <param name="session">Сессия.</param>
  /// <returns>true - если у объекта есть подписи</returns>
  bool HasSignedGraphs(long objectID, long userID, IUserSession session);

  /// <summary>Вернуть настройку подписей для должности</summary>
  /// <param name="rankID">The rank ID.</param>
  /// <param name="sessionGuid">The session GUID.</param>
  /// <returns>xml документ настроек в виде массива байт</returns>
  byte[] GetRankSignsSetup(long rankID, Guid sessionGuid);

  /// <summary>
  /// Административная функция: производит перерасчет хэшей подписей
  /// Только для внутренних подписей.
  /// </summary>
  /// <param name="sessionGuid"></param>
  /// <param name="message">Диагностическое сообщение о результатах работы</param>
  /// <returns>Количество обработанных подписей; -1-нет прав для выполнения действия</returns>
  long UpdateSignsHashes(Guid sessionGuid, out string message);

  /// <summary>
  /// Административная функция: производит приведение подписей к последней версии, используемой в системе
  /// Только для внутренних подписей.
  /// </summary>
  void ConvertSignsToLastVersion(Guid sessionGuid);

  /// <summary>Получает список параметров подписей версии объекта.</summary>
  /// <param name="objectId">ИД версии объекта</param>
  /// <param name="sessionGuid">Пользовательская сессия</param>
  /// <param name="CanShowTime">Показывать ли время. true - отображается дата и время. false - только дата.</param>
  /// <returns>Список параметров подписей версии объекта.</returns>
  List<SignParams> GetObjectSignsParams(long objectId, Guid sessionGuid, bool CanShowTime = false);

  /// <summary>Сохраняет параметры вывода подписей на сервере</summary>
  /// <param name="sessionGuid">ГУИД сессии</param>
  void SaveOutputParams(Guid sessionGuid);

  /// <summary>Сохраняет общие параметры подписей на сервере</summary>
  /// <param name="sessionGuid">ГУИД сессии</param>
  void SaveSignsParams(Guid sessionGuid);

  string PatchSignGraphsForAllArchives(Dictionary<string, string> substitutes, Guid sessionGuid);

  string PatchSignGraphsForLCStepsAndLCLevels(
    Dictionary<string, string> substitutes,
    Guid sessionGuid);

  string PatchSignGraphsForRanks(Dictionary<string, string> substitutes, Guid sessionGuid);
}
