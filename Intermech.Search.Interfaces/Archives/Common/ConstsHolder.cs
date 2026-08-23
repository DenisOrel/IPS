// Decompiled with JetBrains decompiler
// Type: Intermech.Archives.Common.ConstsHolder
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Archives.Common;

/// <summary>Хранилище констант</summary>
public class ConstsHolder
{
  /// <summary>Guid типа "Архив"</summary>
  public static readonly Guid ArcTypeGuid = new Guid("cad0011e-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа "Архив"</summary>
  public static int ArcTypeID = 0;
  /// <summary>Guid типа объекта "Документы"</summary>
  public static readonly Guid DocTypeGuid = new Guid("cad00070-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа объекта "Документы"</summary>
  public static int DocTypeID = 0;
  /// <summary>тип объектов "Расцеховочный маршрут"</summary>
  public static readonly Guid CehRouteGUID = new Guid("cad001e5-306c-11d8-b4e9-00304f19f545");
  /// <summary>ID типа объектов "Расцеховочный маршрут"</summary>
  public static int CehRouteID = -1;
  /// <summary>
  /// Guid атрибута "Рассылка копий по расцеховочному маршруту"
  /// </summary>
  public static readonly Guid SendCopyByCehRouteAttrGuid = new Guid("cadd960f-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// ID атрибута "Рассылка копий по расцеховочному маршруту"
  /// </summary>
  public static int SendCopyByCehRouteAttrID = 0;
  /// <summary>тип объектов "Расцеховочный элемент"</summary>
  public static readonly Guid ElemRouteGUID = new Guid("cad001e8-306c-11d8-b4e9-00304f19f545");
  /// <summary>ID типа объектов "Расцеховочный элемент"</summary>
  public static int ElemRouteID = -1;
  /// <summary>тип объектов "Комплект технологических документов"</summary>
  public static readonly Guid ComplectTechDocGUID = new Guid("cad00169-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// ID типа объектов "Комплект технологических документов"
  /// </summary>
  public static int ComplectTechDocID = -1;
  /// <summary>ID типа объектов Листы рассылки</summary>
  public static int DeliveryListID = -1;
  /// <summary>Guid типа объектов Копии документов</summary>
  public static readonly string CopyOfDocumentGuid = "cadd9364-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID типа объектов Копии документов</summary>
  public static int CopyOfDocumentID = -1;
  /// <summary>Guid типа объектов  Альбом документов</summary>
  public static readonly string DocAlbumGuid = "cadd9363-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID типа объектов  Альбом документов</summary>
  public static int DocAlbumID = -1;
  /// <summary>ID типа объектов Пользователи</summary>
  public static int UsersTypeID = -1;
  /// <summary>ID типа объектов Группа пользователей</summary>
  public static int UserGroupTypeID = -1;
  /// <summary>ID типа объектов Организационные единицы</summary>
  public static int OrganizationUnitsTypeID = -1;
  /// <summary>ID типа объектов Узлы информационной системы</summary>
  public static int SitesTypeID = -1;
  /// <summary>ИД типа связи Простая связь между объектами</summary>
  public static int RelTypeSimpleId = -1;
  /// <summary>Guid типа атрибута "Архив"</summary>
  public static readonly Guid ArcAttrGuid = SystemGUIDs.attributeArchive;
  /// <summary>Идентификатор типа атрибута "Архив"</summary>
  public static int ArchiveAttrID = 0;
  /// <summary>
  /// Guid типа атрибута "Глобальные идентификаторы типов объекта"
  /// </summary>
  public static readonly Guid AttributeObjectTypeGuids = new Guid("cad00149-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// ID типа атрибута "Глобальные идентификаторы типов объекта"
  /// </summary>
  public static int AttributeObjectTypeGuidsID = 0;
  /// <summary>Guid типа атрибута "Структура архива"</summary>
  public static readonly Guid ArchiveStructureAttrGuid = new Guid("cad0005f-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Структура архива"</summary>
  public static int ArchiveStructureAttrID = 0;
  /// <summary>
  /// Guid типа атрибута "Автоматически размещающиеся в архиве типы документов"
  /// </summary>
  public static readonly Guid AutoPlaceDocTypesAttrGuid = new Guid("cadd96aa-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Автоматически размещающиеся в архиве типы документов"
  /// </summary>
  public static int AutoPlaceDocTypesAttrID = 0;
  /// <summary>
  /// Guid типа атрибута "Пользователи, автоматически размещающие в архив документы"
  /// </summary>
  public static readonly Guid UsersCanAutoPlaceDocsAttrGuid = new Guid("cadd96ab-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Пользователи, автоматически размещающие в архив документы"
  /// </summary>
  public static int UsersCanAutoPlaceDocsAttrID = 0;
  /// <summary>
  /// Guid атрибута Архивы. Используется для задания принадлжености в выборках
  /// </summary>
  public static readonly Guid ArchivesForSelectionGuid = new Guid("cad01485-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// ID атрибута Архивы. Используется для задания принадлжености в выборках
  /// </summary>
  public static int ArchivesForSelectionID = 0;
  /// <summary>
  /// Guid типа атрибута "Создавать версии документов в архиве"
  /// </summary>
  public static readonly Guid CanCreateDocVersionInArchiveGuid = new Guid("cadd94c7-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// ID типа атрибута "Создавать версии документов в архиве"
  /// </summary>
  public static int CanCreateDocVersionInArchiveID = 0;
  /// <summary>Guid атрибута "Режим использования разрешенных типов"</summary>
  public static readonly Guid ArchiveTypesUsingModeGuid = new Guid("cadd94cc-306c-11d8-b4e9-00304f19f545");
  /// <summary>ID атрибута "Режим использования разрешенных типов"</summary>
  public static int ArchiveTypesUsingModeID = 0;
  /// <summary>Guid атрибута Инвентарный номер (ОТД)</summary>
  public static readonly string InventoryNumberGuid = "cadd935b-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Инвентарный номер (ОТД)</summary>
  public static int InventoryNumberID = 0;
  /// <summary>Guid атрибута Взамен инвентарного номера</summary>
  public static readonly string PreviousInventoryNumberGuid = "cadd9354-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Взамен инвентарного номера</summary>
  public static int PreviousInventoryNumberID = 0;
  /// <summary>Guid атрибута Заменен на инвентарный номер</summary>
  public static readonly string NewInventoryNumberGuid = "cadd9358-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Заменен на инвентарный номер</summary>
  public static int NewInventoryNumberID = 0;
  /// <summary>ID атрибута Идентификатор документа (ОТД)</summary>
  public static int OriginalObjectID = 0;
  /// <summary>
  /// Атрибут "Дата регистрации в ОТД" типа объекта Документ
  /// </summary>
  public static readonly string OTDRegisteredDate = "cadd941c-306c-11d8-b4e9-00304f19f545";
  /// <summary>
  /// ID атрибута "Дата регистрации в ОТД" типа объекта Документ
  /// </summary>
  public static int OTDRegisteredDateID = 0;
  /// <summary>Атрибут "Зарегистрировал в ОТД" типа объекта Документ</summary>
  public static readonly string OTDRegistrator = "cadd941d-306c-11d8-b4e9-00304f19f545";
  /// <summary>
  /// ID атрибута "Зарегистрировал в ОТД" типа объекта Документ
  /// </summary>
  public static int OTDRegistratorID = 0;
  /// <summary>Guid атрибута Идентификатор версии документа</summary>
  public static readonly string OriginalObjectVersionGuid = "cadd9359-306c-11d8-b4e9-00304f19f545";
  /// <summary>
  /// ID атрибута Идентификатор версии документа
  /// !В атрибут писать строго модуль версии объекта!
  /// </summary>
  public static int OriginalObjectVersionID = 0;
  /// <summary>Guid атрибута Актуальная копия документа</summary>
  public static readonly string ActualCopyGuid = "cadd9352-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Актуальная копия документа</summary>
  public static int ActualCopyID = 0;
  /// <summary>ID атрибута Абоненты</summary>
  public static int SubscribersID = 0;
  /// <summary>Guid атрибута  Дата постановки на уведомление</summary>
  public static readonly string SubscribersDateGuid = "cadd9357-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута  Дата постановки на уведомление</summary>
  public static int SubscribersDateID = 0;
  /// <summary>Guid атрибута   Количество копий</summary>
  public static readonly string NumberOfCopiesGuid = "cadd935c-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута   Количество копий</summary>
  public static int NumberOfCopiesID = 0;
  /// <summary>Guid атрибута Кто подписал абонента копий</summary>
  public static readonly string ListOwnerGuid = "cadd935e-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Кто подписал абонента копий</summary>
  public static int ListOwnerID = 0;
  /// <summary>Guid атрибута Примечания к абонентам</summary>
  public static readonly string NotesForSubscribersGuid = "cadd9c75-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Примечания к абонентам</summary>
  public static int NotesForSubscribersID = 0;
  /// <summary>Guid атрибута Номер копии</summary>
  public static readonly string IndexOfCopyGuid = "cadd9360-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута  Номер копии</summary>
  public static int IndexOfCopyID = 0;
  /// <summary>Guid атрибута Абонент</summary>
  public static readonly string AlbumSubscriberGuid = "cadd9350-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Абонент</summary>
  public static int AlbumSubscriberID = 0;
  /// <summary>Guid атрибута Получатель копии</summary>
  public static readonly string RecipientGuid = "cadd9361-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Получатель копии</summary>
  public static int RecipientID = 0;
  /// <summary>Guid атрибута  Дата возврата копии</summary>
  public static readonly string ReturnDateGuid = "cadd9355-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута  Дата возврата копии</summary>
  public static int ReturnDateID = 0;
  /// <summary>Guid атрибута Дата получения копии</summary>
  public static readonly string ReceiptDateGuid = "cadd9356-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Дата получения копии</summary>
  public static int ReceiptDateID = 0;
  /// <summary>Guid атрибута Количество листов в документе</summary>
  public static readonly string ListsCountGuid = "cad014b0-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Количество листов в документе</summary>
  public static int ListsCountID = 0;
  /// <summary>Guid атрибута Количество листов формата А4</summary>
  public static readonly string A4ListNumberGuid = "cadd935d-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Количество листов формата А4</summary>
  public static int A4ListNumberID = 0;
  /// <summary>Guid атрибута Вид копии</summary>
  public static readonly string CopyKindAttrGuid = "cadd9666-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Вид копии</summary>
  public static int CopyKindAttrID = 0;
  /// <summary>Guid атрибута  Вернул копию</summary>
  public static readonly string WhoReturnGuid = "cadd9353-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута  Вернул копию</summary>
  public static int WhoReturnID = 0;
  /// <summary>Guid атрибута Извещение</summary>
  public static readonly string EcoAttrGuid = "cadd9645-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID атрибута Извещение</summary>
  public static int EcoAttrID = 0;
  /// <summary>
  /// Guid типа атрибута "Значения по умолчанию атрибутов структуры архива"
  /// </summary>
  public static readonly Guid ArchiveStructureAttrValuesByDefaultAttrGuid = new Guid("cadd99ad-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Значения по умолчанию атрибутов структуры архива"
  /// </summary>
  public static int ArchiveStructureAttrValuesByDefaultAttrID = 0;
  /// <summary>Уровень продвижения "Производство и эксплуатация"</summary>
  public const string LevelManufacturingGuid = "cad00011-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID уровня продвижения "Производство и эксплуатация"</summary>
  public static int LevelManufacturingId = 0;
  /// <summary>
  /// Уровень продвижения "Удалено", обозначающий удаленные объекты
  /// </summary>
  public const string LevelDeletedGuid = "cad0000e-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID уровня продвижения "Удалено"</summary>
  public static int LevelDeletedId = 0;
  /// <summary>Уровень продвижения "Хранение"</summary>
  public const string LevelKeepingGuid = "cad009de-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID уровня продвижения "Хранение"</summary>
  public static int LevelKeepingId = 0;
  /// <summary>имя схемы колонок для копий докуемнтов</summary>
  public static Guid CopySchemeName = new Guid("{726FF395-F196-4280-F538-771E4BEDBFFB}");
  /// <summary>Guid шага ЖЦ Выслана</summary>
  public static readonly string SendLCStepGuid = "cadd936d-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID шага ЖЦ Выслана</summary>
  public static int SendLCStepID = -1;
  /// <summary>Guid шага ЖЦ Создана</summary>
  public static readonly string CreateLCStepGuid = "cadd936e-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID шага ЖЦ Создана</summary>
  public static int CreateLCStepID = -1;
  /// <summary>Guid шага ЖЦ Возвращена</summary>
  public static readonly string ReturnLCStepGuid = "cadd936f-306c-11d8-b4e9-00304f19f545";
  /// <summary>ID шага ЖЦ Возвращена</summary>
  public static int ReturnLCStepID = -1;
  /// <summary>
  /// Разделитель между Гуидом и значением для атрибута Значения по умолчанию атрибутов структуры архива
  /// </summary>
  public static string Separator = "|";
  /// <summary>имя модуля в настройках</summary>
  public static string MODULE_NAME = "Archive";
  /// <summary>имя секции настроек</summary>
  public static string SETTINGS = "Settings";
  /// <summary>имя параметра - автоматическая генерация номера ОТД?</summary>
  public static string AUTOGENERATION = "AutoGeneration";
  /// <summary>имя параметра - - рассылать уведомления?</summary>
  public static string EMAIL_NOTIFY = "EmailNotify";
  /// <summary>
  ///  имя параметра - Кто вернул копию равно Получатель копии?
  /// </summary>
  public static string RECIPIENT_RETURN_COPY = "IsRecipReturnCopy";
  /// <summary>
  ///  имя параметра - Автоматически создавать копии документа при публикации
  /// </summary>
  public static string AUTO_CREATE_COPY = "AutoCreateCopy";
  /// <summary>
  /// имя параметра - Разрешить отправлять копии документов не возвращая устаревшие копии
  /// </summary>
  public static string ALLOW_SEND_COPIES = "AllowSendCopies";
  /// <summary>имя параметра - Уровень продвижения</summary>
  public static string LEVEL = "Level";
  /// <summary>
  /// имя  в секции настроек для хранения информации о формулах
  /// </summary>
  public static string INVENTORY_SECTION_NAME = "InventorySection";
  /// <summary>
  /// имя в секции настроек для хранения информации об абонентах
  /// </summary>
  public static string SUBSCRIBERS_SECTION_NAME = "SubscribersSection";
  /// <summary>набор классификаторов для вычисления номера ОТД</summary>
  public static string CLASSIFIERS = "Classifiers";
  /// <summary>использовать классификатор для вычисления номера ОТД?</summary>
  public static string USE_CLASSIFIERS = "UseClassifiers";
  /// <summary>уведомлять Абонента и Получателя о высылке копии</summary>
  public static string SUBSCR_NOTIFY = "SubscrNotify";
  /// <summary>
  /// список колонок для отображения на закладке
  /// Структура архива
  /// </summary>
  public static List<string> ArchiveStructureColumns = new List<string>((IEnumerable<string>) new string[21]
  {
    "F_ATTRIBUTE_ID",
    "F_NAME",
    "F_SHORT_NAME",
    "F_ALIAS",
    "F_NOTE",
    "F_DEFAULT_VALUE",
    "F_MULTIPLE_VALUED",
    "F_ATTRIBUTE_TYPE",
    "F_SIZE_TYPE",
    "F_LEVEL_ID",
    "F_FORMULA",
    "F_LANGUAGE_ID",
    "F_GUID",
    "F_AREA_ID",
    "F_UNIQUE",
    "F_OPTIMIZED",
    "F_CONTENT",
    "F_OPTIONS",
    "F_MASK",
    "F_MASTER_ID",
    "F_SOURCE_ID"
  });
  /// <summary>
  /// кэш соответствия имя столбца - название столбца.
  /// н-р, F_ATTRIBUTE_ID - Идентификатор атрибута
  /// </summary>
  public static Dictionary<string, string> ColumnCaptionsCach;

  /// <summary>Инициализация констант</summary>
  static ConstsHolder()
  {
    ConstsHolder.SubscribersID = MetaDataHelper.GetAttributeTypeID(SystemGUIDs.attributeSubscribers);
    ConstsHolder.SubscribersDateID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.SubscribersDateGuid);
    ConstsHolder.NumberOfCopiesID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.NumberOfCopiesGuid);
    ConstsHolder.NotesForSubscribersID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.NotesForSubscribersGuid);
    ConstsHolder.ListOwnerID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.ListOwnerGuid);
    ConstsHolder.InventoryNumberID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.InventoryNumberGuid);
    ConstsHolder.PreviousInventoryNumberID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.PreviousInventoryNumberGuid);
    ConstsHolder.NewInventoryNumberID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.NewInventoryNumberGuid);
    ConstsHolder.OriginalObjectID = MetaDataHelper.GetAttributeTypeID(SystemGUIDs.attributeOriginalObject);
    ConstsHolder.OTDRegisteredDateID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.OTDRegisteredDate);
    ConstsHolder.OTDRegistratorID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.OTDRegistrator);
    ConstsHolder.OriginalObjectVersionID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.OriginalObjectVersionGuid);
    ConstsHolder.ActualCopyID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.ActualCopyGuid);
    ConstsHolder.IndexOfCopyID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.IndexOfCopyGuid);
    ConstsHolder.AlbumSubscriberID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.AlbumSubscriberGuid);
    ConstsHolder.RecipientID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.RecipientGuid);
    ConstsHolder.ReturnDateID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.ReturnDateGuid);
    ConstsHolder.ReceiptDateID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.ReceiptDateGuid);
    ConstsHolder.ListsCountID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.ListsCountGuid);
    ConstsHolder.A4ListNumberID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.A4ListNumberGuid);
    ConstsHolder.CopyKindAttrID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.CopyKindAttrGuid);
    ConstsHolder.WhoReturnID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.WhoReturnGuid);
    ConstsHolder.AttributeObjectTypeGuidsID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.AttributeObjectTypeGuids);
    ConstsHolder.ArchiveTypesUsingModeID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.ArchiveTypesUsingModeGuid);
    ConstsHolder.SendCopyByCehRouteAttrID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.SendCopyByCehRouteAttrGuid);
    ConstsHolder.EcoAttrID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.EcoAttrGuid);
    ConstsHolder.ArchiveStructureAttrValuesByDefaultAttrID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.ArchiveStructureAttrValuesByDefaultAttrGuid);
    ConstsHolder.DocTypeID = MetaDataHelper.GetAttributeTypeID(ConstsHolder.DocTypeGuid);
    ConstsHolder.DeliveryListID = MetaDataHelper.GetObjectTypeID(SystemGUIDs.objtypeDeliveryList);
    ConstsHolder.CopyOfDocumentID = MetaDataHelper.GetObjectTypeID(ConstsHolder.CopyOfDocumentGuid);
    ConstsHolder.DocAlbumID = MetaDataHelper.GetObjectTypeID(ConstsHolder.DocAlbumGuid);
    ConstsHolder.CehRouteID = MetaDataHelper.GetObjectTypeID(ConstsHolder.CehRouteGUID);
    ConstsHolder.ElemRouteID = MetaDataHelper.GetObjectTypeID(ConstsHolder.ElemRouteGUID);
    ConstsHolder.ComplectTechDocID = MetaDataHelper.GetObjectTypeID(ConstsHolder.ComplectTechDocGUID);
    ConstsHolder.UsersTypeID = MetaDataHelper.GetObjectTypeID("cad00002-306c-11d8-b4e9-00304f19f545");
    ConstsHolder.UserGroupTypeID = MetaDataHelper.GetObjectTypeID("cad00003-306c-11d8-b4e9-00304f19f545");
    ConstsHolder.OrganizationUnitsTypeID = MetaDataHelper.GetObjectTypeID("cadd9235-306c-11d8-b4e9-00304f19f545");
    ConstsHolder.SitesTypeID = MetaDataHelper.GetObjectTypeID(PortalConsts.objtypeSites);
    ConstsHolder.RelTypeSimpleId = MetaDataHelper.GetRelationTypeID("cad00022-306c-11d8-b4e9-00304f19f545");
    ConstsHolder.LevelManufacturingId = MetaDataHelper.GetLCLevelID("cad00011-306c-11d8-b4e9-00304f19f545");
    ConstsHolder.LevelKeepingId = MetaDataHelper.GetLCLevelID("cad009de-306c-11d8-b4e9-00304f19f545");
    ConstsHolder.LevelDeletedId = MetaDataHelper.GetLCLevelID("cad0000e-306c-11d8-b4e9-00304f19f545");
    ConstsHolder.SendLCStepID = MetaDataHelper.GetLCStepID(ConstsHolder.SendLCStepGuid);
    ConstsHolder.CreateLCStepID = MetaDataHelper.GetLCStepID(ConstsHolder.CreateLCStepGuid);
    ConstsHolder.ReturnLCStepID = MetaDataHelper.GetLCStepID(ConstsHolder.ReturnLCStepGuid);
  }
}
