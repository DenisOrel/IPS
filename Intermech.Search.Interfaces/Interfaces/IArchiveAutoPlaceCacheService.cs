// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.IArchiveAutoPlaceCacheService
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Archives.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Interfaces;

/// <summary>Сервис для работы с кэшем авторазмещения в архиве</summary>
public interface IArchiveAutoPlaceCacheService
{
  /// <summary>Заполняет кэш из базы.</summary>
  void FillCache();

  /// <summary>Сохраняет в кэше новые настройки для архива.</summary>
  /// <param name="archiveID">ИД архива.</param>
  /// <param name="docTypesIDs">Список типов документов.</param>
  /// <param name="usersIDs">Список ИД пользователей.</param>
  void SaveAutoPlaceSettingsInCache(long archiveID, List<int> docTypesIDs, List<long> usersIDs);

  /// <summary>
  /// Получает из кэша ИД архива, в который пользователь может положить данный тип документа.
  /// </summary>
  /// <param name="docTypeID">Тип созданного документа</param>
  /// <param name="userID">ИД пользователя</param>
  /// <param name="sessionGuid">GUID сессии</param>
  /// <returns>ИД архива по умолчанию.Intermech.Consts.UnknownObjectId - если архив не найден.</returns>
  long GetArchiveIdFromCaсhe(int docTypeID, long userID, Guid sessionGuid);

  /// <summary>
  /// Найти пересечения назначенных типов и пользователей с другими архивами.
  /// Поиск осуществляется по полному совпадению пользователя, группы или подразделения и типа документа
  /// </summary>
  /// <param name="archID">Идентификатор архива, для которого ищем совпадения.</param>
  /// <param name="typeIDs">Список ИД типов документов</param>
  /// <param name="userIDs">Список ИД пользователей</param>
  /// <param name="wrongTypeIDs">Типы документов, которые есть в другом архиве</param>
  /// <param name="wrongUsersIDs">Пользователи, которые есть в другом архиве</param>
  /// <returns>Словарь найденных с другими архивами пересечений.</returns>
  Dictionary<long, TypesAndUsers> FindArchiveSettingsIntersections(
    long archID,
    List<int> typeIDs,
    List<long> userIDs,
    out List<int> wrongTypeIDs,
    out List<long> wrongUsersIDs);

  /// <summary>Удаляет из кэша архив и его настройки.</summary>
  /// <param name="objectID">ИД архива.</param>
  void DeleteArchiveFromCache(long objectID);
}
