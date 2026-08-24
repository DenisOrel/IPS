// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IMetadataInfo
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Интерфейс на кэш метаданных</summary>
public interface IMetadataInfo
{
  /// <summary>
  /// Получение нового значения уникального идентификатора. Если включена опция,
  /// то идентификатор будет специально изменен (cae0...)
  /// </summary>
  /// <returns>новый Guid</returns>
  Guid NewPumpGuid();

  /// <summary>Интерфейс кэша типов объектов</summary>
  IObjectTypeItemList ObjectTypes { get; }

  /// <summary>Интерфейс кэша типов атрибутов</summary>
  IAttributeTypeItemList AttributeTypes { get; }

  /// <summary>Интерфейс кэша групп атрибутов</summary>
  IAttributeGroupItemList AttributeGroups { get; }

  /// <summary>Интерфейс кэша типов связей</summary>
  IRelationTypeItemList RelationTypes { get; }

  /// <summary>Получение метаданных с сервера ИНТЕРМЕХ</summary>
  /// <returns>В случае успешного получения метаданных возвращается true, иначе - false</returns>
  bool MetadataLoadFromServer();

  /// <summary>Сохранение метаданных на сервере ИНТЕРМЕХ</summary>
  /// <returns>В случае успешного сохранения метаданных возвращается true, иначе - false</returns>
  bool MetadataSaveToServer();

  bool MetadataApplyChanges();

  int GetLCLevel(int stepID);

  /// <summary>
  /// Изменение значения поля (для записи с указанным ключом) в заданной таблице метаданных
  /// </summary>
  /// <param name="tableName">Название таблицы в метаданных</param>
  /// <param name="key">Значение ключа по которому будет проводиться поиск записи</param>
  /// <param name="fieldName">Наименование поля в котором будет изменено значение</param>
  /// <param name="value">Значение, которое надо установить</param>
  /// <returns>Если установка значения прошла успешно - true, иначе - false</returns>
  bool MetadataEditTableFieldValue(string tableName, object key, string fieldName, object value);

  /// <summary>
  /// Получение пользовательской сессии подключения к серверу ИНТЕРМЕХ
  /// </summary>
  /// <returns>Интерфейс пользовательской сессии</returns>
  IUserSession UserSession { get; }

  /// <summary>Подключение к серверу</summary>
  /// <returns>В случае успешного подключения возвращается true, иначе - false</returns>
  bool Login();

  /// <summary>
  /// Идентификатор пользователя, делающего закачку в новой системе
  /// </summary>
  long UserID { get; }

  /// <summary>
  /// Глобальный идентификатор пользователя, делающего закачку в новой системе
  /// </summary>
  Guid UserGUID { get; }

  /// <summary>
  /// Получить идентификатор шага ЖЦ на который при закачке переводить объект типа objectType (IPS)
  /// для архива archiveID (SEARCH). В случае неудачи возвращает Consts.UnknownLCStepId
  /// </summary>
  int GetLCStepForArchiveType(int archiveID, int objectType, int docStateID);

  /// <summary>Импортированные объекты</summary>
  IImportedObjects ImportedObjects { get; }

  /// <summary>Импортированные связи</summary>
  IImportedRelations ImportedRelations { get; }

  /// <summary>Материалы</summary>
  IMaterials Materials { get; }

  /// <summary>Импортированные пользователи</summary>
  IImportedUsers ImportedUsers { get; }

  /// <summary>
  /// Проверяет версию базы назначения и возвращает соотвествующий результат
  /// </summary>
  /// <param name="showErrorMessage">Отображать ли сообщение об несоответсвии базы назвачения</param>
  /// <returns></returns>
  bool CheckDBVersion(bool showErrorMessage);

  /// <summary>Интерфейс на серверный объект - импортер</summary>
  IDBImporter dbImporter { get; }

  /// <summary>Корректное завершение работы</summary>
  void Close();
}
