// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IImportedObjectList
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

public interface IImportedObjectList : IImportedAttributeList
{
  IImportedObjectListItems Items { get; }

  int PacketSize { get; set; }

  /// <summary>
  /// Признак оставления в списке импортированных объектов только новых
  ///  </summary>
  bool NewObjectsOnlyInList { get; set; }

  event AfterImportEventDelegate AfterImportEvent;

  /// <summary>
  /// Получить ошибку при импорте объекта под номером index в списке Items
  /// </summary>
  /// <param name="index"></param>
  /// <returns></returns>
  Exception GetImportError(int index);

  /// <summary>Добавление объекта (сокращенная версия)</summary>
  /// <param name="objType">Идентификатор типа объекта</param>
  /// <param name="owner">Идентификатор владельца в SEARCH</param>
  /// <returns>Идентификатор созданного объекта</returns>
  ObjectRecord AddObject(int objType, int owner);

  /// <summary>Добавление объекта (сокращенная версия)</summary>
  /// <param name="objType">Идентификатор типа объекта</param>
  /// <param name="owner">Идентификатор владельца в SEARCH</param>
  /// <param name="caption">Заголовок объекта</param>
  /// <returns>Идентификатор созданного объекта</returns>
  ObjectRecord AddObject(int objType, int owner, string caption);

  /// <summary>Добавление объекта</summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  ObjectRecord AddObject(ObjectRecord obj);

  /// <summary>Добавление объекта</summary>
  /// <param name="objType">&gt;Идентификатор типа объекта</param>
  /// <param name="owner">Идентификатор владельца в SEARCH</param>
  /// <param name="lcStep">Идентификтор этапа ЖЦ</param>
  /// <param name="versionId">Порядковый номер версии объекта</param>
  /// <param name="userId">Идентификатор пользователя в SEARCH, взявшего версию объекта на редактирование</param>
  /// <param name="objVerType">признак версии/экземпляра/актуальной версии</param>
  /// <param name="modifDate">Дата последней модификации объекта </param>
  /// <param name="lewelId">Идентификатор уровня продвижения</param>
  /// <param name="createDate">Дата создания версии объекта</param>
  /// <param name="caption">Заголовок объекта</param>
  /// <returns>Идентификатор созданного объекта</returns>
  ObjectRecord AddObject(
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

  void AddItem(ImportingObject importingObject);

  /// <summary>Импорт пакета данных</summary>
  void Import();

  /// <summary>
  /// Тоже самое, что и функция <b>UseObject(ObjectRecord obj)</b>, только тут сделаеццо запрос в кэш и сформируеццо ObjectRecord
  /// </summary>
  /// <param name="objectID">Идентификатор ВЕРСИИ объекта</param>
  void UseObject(long objectID);

  /// <summary>
  /// Установить курсор в списке объектов для импорта на объект obj. Если в списке такого объекта не обнаружено,
  /// объект добавляеццо в конец списка, и курсор ставиццо на него.
  /// </summary>
  /// <param name="obj"></param>
  void UseObject(ObjectRecord obj);

  /// <summary>
  /// Установить курсор в списке объектов для импорта на объект obj. Если в списке такого объекта не обнаружено (по GUID),
  /// то пробуем искать запись по ид.
  /// </summary>
  /// <param name="objectID">Guid ВЕРСИИ объекта</param>
  /// <param name="objectID">ИД.  ВЕРСИИ объекта</param>
  void UseObject(Guid objectGuid, long objectID);
}
