// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IDataWriter
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

/// <summary>Интерфейс для работы с базой</summary>
public interface IDataWriter
{
  /// <summary>Интерфейс главного модуля приложения</summary>
  IAppManager AppManager { get; }

  /// <summary>
  /// Получение пользовательской сессии подключения к серверу ИНТЕРМЕХ
  /// </summary>
  /// <returns>Интерфейс пользовательской сессии</returns>
  IUserSession GetUserSession();

  /// <summary>
  /// Добавление предопределенного системного объекта. Используется для назначения системному объекту
  /// нового идентификатора (для использования в пределах сеанса закачки) и добавления его в общий
  /// кэш системных объектов
  /// </summary>
  /// <param name="sysObjGuid">Глобальный идентификатор предопределенного системного объекта</param>
  /// <returns>Идентификатор добавленного объекта</returns>
  long AddSysObject(Guid sysObjGuid);

  /// <summary>Добавление объекта</summary>
  /// <param name="objType">&gt;Идентификатор типа объекта</param>
  /// <param name="owner">Идентификатор владельца SEARCH</param>
  /// <param name="lcStep">Идентификтор этапа ЖЦ</param>
  /// <param name="versionId">Порядковый номер версии объекта</param>
  /// <param name="userId">Идентификатор SEARCH пользователя, взявшего версию объекта на редактирование</param>
  /// <param name="objVerType">признак версии/экземпляра/актуальной версии</param>
  /// <param name="modifDate">Дата последней модификации объекта </param>
  /// <param name="lewelId">Идентификатор уровня продвижения</param>
  /// <param name="createDate">Дата создания версии объекта</param>
  /// <param name="caption">Заголовок объекта</param>
  /// <returns>Идентификатор созданного объекта</returns>
  long AddObject(
    int objType,
    int owner,
    int lcStep,
    int versionId,
    int userId,
    int objVerType,
    DateTime modifDate,
    int lewelId,
    DateTime createDate,
    string caption);

  /// <summary>Добавление объекта (сокращенная версия)</summary>
  /// <param name="objType">Идентификатор типа объекта</param>
  /// <param name="owner">Идентификатор владельца SEARCH</param>
  /// <param name="caption">Заголовок объекта</param>
  /// <returns>Идентификатор созданного объекта</returns>
  long AddObject(int objType, int owner, string caption);

  /// <summary>Добавление объекта (сокращенная версия)</summary>
  /// <param name="objType">Идентификатор типа объекта</param>
  /// <param name="owner">Идентификатор владельца SEARCH</param>
  /// <param name="caption">Заголовок объекта</param>
  /// <param name="objectGuid">GUID версии объекта</param>
  /// <returns>Идентификатор созданного объекта</returns>
  long AddObject(int objType, int owner, string caption, Guid objectGuid);

  /// <summary>Добавление объекта (сокращенная версия)</summary>
  /// <param name="objType">Идентификатор типа объекта</param>
  /// <param name="owner">Идентификатор владельца SEARCH</param>
  /// <returns>Идентификатор созданного объекта</returns>
  long AddObject(int objType, int owner);

  /// <summary>Создание прав доступа</summary>
  /// <param name="importingRecs">Массив структур, описывающих права доступа для импорта</param>
  /// <returns>Возвращает массив идентификаторов записей по правам доступа, если элемент = Consts.UnknownObjectId значит права доступа не перекачались.
  /// вся инфа из-за чего не перекачались на сервере в логе</returns>
  long[] ImportSequrity(SecurityRecord[] importingRecs);

  /// <summary>Добавление атрибута объекта</summary>
  /// <param name="objId">Идентификатор объекта</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="attrValtype">Тип значения атрибута</param>
  /// <param name="attrVal">Значение атрибута</param>
  /// <param name="numInList">Номер значения в списке (0 - если атрибут не список значений)</param>
  /// <returns>Идентификатор атрибута, либо -1 при неудаче</returns>
  int AddAttribute(
    long objId,
    int attrType,
    AttrValueType attrValtype,
    object attrVal,
    int numInList);

  /// <summary>Добавление null атрибута объекта</summary>
  /// <param name="objId">Идентификатор объекта</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  int AddAttributeNull(long objId, int attrType);

  /// <summary>Добавление строкового атрибута объекта</summary>
  /// <param name="objId">Идентификатор объекта</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  int AddAttributeStr(long objId, int attrType, string value);

  /// <summary>Добавление целочисленного атрибута объекта</summary>
  /// <param name="objId">Идентификатор объекта</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  int AddAttributeInt(long objId, int attrType, long value);

  /// <summary>Добавление вещественного атрибута объекта</summary>
  /// <param name="objId">Идентификатор объекта</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  int AddAttributeDouble(long objId, int attrType, double value);

  /// <summary>Добавление временн'ого атрибута объекта</summary>
  /// <param name="objId">Идентификатор объекта</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  int AddAttributeDate(long objId, int attrType, DateTime value);

  /// <summary>Добавление ссылочного атрибута объекта</summary>
  /// <param name="objId">Идентификатор объекта</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Идентификатор версии ссылочного объекта</param>
  /// <param name="caption">Заголовок ссылочного объекта</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  int AddAttributeLink(long objId, int attrType, long value, string caption);

  /// <summary>
  /// Добавление вещественного атрибута выраженного в единицах измерения
  /// </summary>
  /// <param name="objId">Идентификатор объекта</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <param name="measureID">Иденификатор единицы измерения</param>
  /// <param name="strValue">Строка со значением</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  int AddAttributeMeasure(
    long objId,
    int attrType,
    double value,
    long measureID,
    string strValue);

  /// <summary>Добавление blob-атрибута объекта</summary>
  /// <param name="objId">Идентификатор объекта</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="filePath">Путь к файлу</param>
  /// <param name="fileSize">Размер файла</param>
  /// <param name="fileNote">Комментарии к файлу</param>
  /// <param name="arcMethod">Метод упаковки</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  int AddAttributeBlob(
    long objId,
    int attrType,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod);

  /// <summary>Добавление связи между объектами</summary>
  /// <param name="projId">Идентификатор версии объекта в сосстав которого производится добавление</param>
  /// <param name="partId">Идентификатор дочернего объекта (не версии!!!)</param>
  /// <param name="relType">Идентификатор вида связи</param>
  /// <param name="crtDate">Дата создания связи</param>
  /// <returns>Идентификатор созданной связи</returns>
  long AddRelationFromID(long projId, long partId, int relType, DateTime crtDate);

  /// <summary>
  /// Добавление связи между объектами (Начало действия связи - текущее время,
  /// дата окончания действия связи - максимальное значение DateTime)
  /// </summary>
  /// <param name="projId">Идентификатор версии объекта в сосстав которого производится добавление</param>
  /// <param name="partId">Идентификатор версии объекта, который надо добавить к составу</param>
  /// <param name="relType">Идентификатор вида связи</param>
  /// <returns>Идентификатор созданной связи</returns>
  long AddRelation(long projId, long partId, int relType);

  /// <summary>Добавление связи между объектами</summary>
  /// <param name="projId">Идентификатор версии объекта в сосстав которого производится добавление</param>
  /// <param name="partId">Идентификатор версии объекта, который надо добавить к составу</param>
  /// <param name="relType">Идентификатор вида связи</param>
  /// <param name="crtDate">Дата создания связи</param>
  /// <returns>Идентификатор созданной связи</returns>
  long AddRelation(long projId, long partId, int relType, DateTime crtDate);

  /// <summary>Добавление атрибута к связи</summary>
  /// <param name="relId">Идентификатор связи</param>
  /// <param name="attrTypeId">Идентификатор типа атрибута</param>
  /// <param name="numInList">Номер в списке сортировки атрибутов</param>
  /// <param name="attrValtype">Тип данных значения атрибута</param>
  /// <param name="attrVal">Значение атрибута</param>
  /// <returns>если добавление прошло успешно - true, иначе - false</returns>
  bool AddRelationAttribyte(
    long relId,
    int attrTypeId,
    int numInList,
    AttrValueType attrValtype,
    object attrVal);

  /// <summary>Добавление строкового атрибута к связи</summary>
  /// <param name="relId">Идентификатор связи</param>
  /// <param name="attrTypeId">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>если добавление прошло успешно - true, иначе - false</returns>
  bool AddRelationAttribyteStr(long relId, int attrTypeId, string value);

  /// <summary>Добавление целочисленного атрибута к связи</summary>
  /// <param name="relId">Идентификатор связи</param>
  /// <param name="attrTypeId">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>если добавление прошло успешно - true, иначе - false</returns>
  bool AddRelationAttribyteInt(long relId, int attrTypeId, long value);

  /// <summary>Добавление вещественного атрибута к связи</summary>
  /// <param name="relId">Идентификатор связи</param>
  /// <param name="attrTypeId">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>если добавление прошло успешно - true, иначе - false</returns>
  bool AddRelationAttribyteDbl(long relId, int attrTypeId, double value);

  /// <summary>Добавление временн'ого атрибута  к связи</summary>
  /// <param name="relId">Идентификатор связи</param>
  /// <param name="attrTypeId">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>если добавление прошло успешно - true, иначе - false</returns>
  bool AddRelationAttribyteDate(long relId, int attrTypeId, DateTime value);

  /// <summary>Добавление ссылочного атрибута связи</summary>
  /// <param name="relId">Идентификатор связи</param>
  /// <param name="attrTypeId">Идентификатор типа атрибута</param>
  /// <param name="value">Идентификатор версии ссылочного объекта</param>
  /// <param name="caption">Заголовок ссылочного объекта</param>
  /// <returns>если добавление прошло успешно - true, иначе - false</returns>
  bool AddRelationAttribyteLink(long relId, int attrType, long value, string caption);

  /// <summary>
  /// Добавление вещественного атрибута связи выраженного в единицах измерения
  /// </summary>
  /// <param name="relId">Идентификатор связи</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <param name="measureID">Иденификатор единицы измерения</param>
  /// <param name="strValue">Строка со значением</param>
  /// <returns>если добавление прошло успешно - true, иначе - false</returns>
  bool AddRelationAttribyteMeasure(
    long relId,
    int attrType,
    double value,
    long measureID,
    string strValue);

  /// <summary>Добавление blob-атрибута связи</summary>
  /// <param name="relId">Идентификатор связи</param>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="filePath">Путь к файлу</param>
  /// <param name="fileSize">Размер файла</param>
  /// <param name="fileNote">Комментарии к файлу</param>
  /// <param name="arcMethod">Метод упаковки</param>
  /// <returns>если добавление прошло успешно - true, иначе - false</returns>
  bool AddRelationAttribyteBlob(
    long relId,
    int attrType,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod);

  IImportedRelationList CreateImportedRelationList();

  IImportedRelationList CreateImportedRelationList(int packetSize);

  IImportedObjectList CreateImportedObjectList();

  IImportedObjectList CreateImportedObjectList(int packetSize);

  /// <summary>
  /// Создание списка для импорта объектов с поддержкой записи статистики для уникального пампера
  /// </summary>
  /// <param name="ownerGuid">Уникальный идентификатор пампера</param>
  /// <returns></returns>
  IImportedObjectList CreateImportedObjectListWithStatistics(Guid ownerGuid);

  /// <summary>
  /// Создание списка для импорта объектов с поддержкой записи статистики для уникального пампера
  /// </summary>
  /// <param name="ownerGuid">Уникальный идентификатор пампера</param>
  /// <param name="packetSize">Размер пакета для записи</param>
  /// <returns></returns>
  IImportedObjectList CreateImportedObjectListWithStatistics(Guid ownerGuid, int packetSize);

  /// <summary>
  /// Создание списка для импорта связей с поддержкой записи статистики для уникального пампера
  /// </summary>
  /// <param name="ownerGuid">Уникальный идентификатор пампера</param>
  /// <returns></returns>
  IImportedRelationList CreateImportedRelationListWithStatistics(Guid ownerGuid);

  /// <summary>
  /// Создание списка для импорта связей с поддержкой записи статистики для уникального пампера
  /// </summary>
  /// <param name="ownerGuid">Уникальный идентификатор пампера</param>
  /// <param name="packetSize">Размер пакета для записи</param>
  /// <returns></returns>
  IImportedRelationList CreateImportedRelationListWithStatistics(Guid ownerGuid, int packetSize);

  /// <summary>Установить иерархию версий</summary>
  /// <param name="tree">IMS_VERSIONS_TREE</param>
  /// <returns>Результат</returns>
  bool SetVersionsTree(DataTable treeTable);

  /// <summary>Включить объект в ручную выборку</summary>
  /// <param name="selectionID">Идентификатор выборки</param>
  /// <param name="objectID">Идентификатор версии объекта</param>
  /// <param name="id">Идентификатор объекта</param>
  /// <param name="selectionKey"></param>
  /// <returns></returns>
  bool IncludeObjectIntoSelection(long selectionID, string key, long objectID, long id);

  /// <summary>
  /// Получить интерфейс на реализацию перекачки данных фильтрации папок Imbase для Techcard
  /// </summary>
  /// <returns></returns>
  IFoldersFilter GetFoldersFilterPumper();
}
