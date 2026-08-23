// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.IArchiveService
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Archives.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Interfaces;

/// <summary>Интерфейс для работы с архивами</summary>
public interface IArchiveService
{
  /// <summary>
  /// Функция проверяет все ли документы архива arcID размещены в шкафу, который указан для этого архива
  /// </summary>
  /// <param name="arcID">Ид. архива</param>
  /// <param name="sessionGuid">Гуид сессии</param>
  /// <returns>Возвращает текст с результатами проверки либо генерит исключение со списком объектов, у которых есть документы из неправильных шкафов</returns>
  string ValidateDocsStorageID(long arcID, Guid sessionGuid);

  /// <summary>
  /// Функция переносит файлы документов архива arcID в шкаф, который указан для этого архива
  /// </summary>
  /// <param name="arcID">Ид. архива</param>
  /// <param name="sessionGuid">Гуид сессии</param>
  /// <returns>Возвращает количество перемещенных файлов</returns>
  int RemoveDocs2ArcStorage(long arcID, Guid sessionGuid);

  /// <summary>Метод проверки настроек архива</summary>
  /// <param name="archiveID">ID архива</param>
  /// <param name="mode">Режим использования типов архива</param>
  /// <param name="typesIDs">Список ID типов архива</param>
  /// <param name="sessionGuid">GUID сессии</param>
  /// <returns>true - если содержимое архива </returns>
  bool CheckArchiveSettings(
    long archiveID,
    ArchiveTypesUsingMode mode,
    List<int> typesIDs,
    Guid sessionGuid);

  /// <summary>Метод для обновлени структуры архива</summary>
  /// <param name="arcIDs">Список ID архивов для которых необходимо обновлять атрибуты (если архив вложен, то его тоже необходимо вписывать)</param>
  /// <param name="attrTypeIDs">список ID-ков для добавления</param>
  /// <param name="action"> тип действия производимый с атрибутом</param>
  /// <param name="sessionID">ID сессии пользователя</param>
  void UpdateArchiveStructure(
    List<long> arcIDs,
    List<int> attrTypeIDs,
    ArchiveStructureChangeAction action,
    Guid sessionID);

  /// <summary>
  /// Собирает список идентификаторов типов из списка гуидов атрибута "Глобальные идентификаторы типов объекта"
  /// </summary>
  /// <param name="archiveID">Атрибут "Глобальные идентификаторы типов объекта"</param>
  /// <param name="isNeedChildsIDs">true - если нужен список с полным перечислением включенных дочерних типов</param>
  /// <param name="sessionGuid">Гуид сессии</param>
  /// <returns>Список ID типов</returns>
  List<int> GetArchivePermittedTypesIDs(long archiveID, bool isNeedChildsIDs, Guid sessionGuid);

  /// <summary>
  /// Удаляет назначенные пользователем значения по умолчанию для атрибутов структуры архива.
  /// </summary>
  /// <param name="archiveId">Ид архива.</param>
  /// <param name="attrTypeIDsForDeleting">Типы атрибутов, значения которых надо удалить</param>
  /// <param name="sessionID">Гуид сессии.</param>
  void DeleteAttributesFromDefaultAttrValuesAttribute(
    long archiveId,
    List<int> attrTypeIDsForDeleting,
    Guid sessionID);

  /// <summary>
  /// Получить значения по умолчанию атрибутов структуры архива (Гуид-значение).
  /// </summary>
  /// <param name="archiveId">ID архива.</param>
  /// <param name="sessionID">Гуид сессии.</param>
  /// <returns>
  /// Значения по умолчанию атрибутов структуры архива (Гуид-значение).
  /// </returns>
  Dictionary<Guid, object> GetArchiveStructureDefaultAttrValues(long archiveId, Guid sessionID);

  /// <summary>
  /// Возвращает режим проверки прав доступа архивов к изделиям
  /// </summary>
  /// <returns>true - проверять права</returns>
  bool GetAtriclesAccessMode();

  /// <summary>
  /// Проверяет можно ли поместить объект objectID в архив archiveID. Проверяются только правила архива - не ЭЦП и т.п.
  /// </summary>
  /// <param name="sessionID">Гуид сессии</param>
  /// <param name="archiveID">Ид. архива</param>
  /// <param name="objectID">Ид. объекта</param>
  void ValidatePlaceToArchive(Guid sessionID, long archiveID, long objectID);

  /// <summary>
  /// Заменять настройки видимости объектов настройками видимости архивов
  /// </summary>
  bool CopyArcVisibility { get; }
}
