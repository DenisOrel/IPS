// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IImportingData
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Интерфейс для работы с закэшированными импортированными данными
/// </summary>
public interface IImportingData
{
  /// <summary>Добавить значение в кэш</summary>
  /// <param name="category">Категория импортированного объекта</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  void AddValue(ImportingCategory category, object oldKey, long newKey);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="category">Идентификатор категории импортированного объекта</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  void AddValue(int category, object oldKey, long newKey);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="category">Категория импортированного объекта</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  /// <param name="caption">Заголовок объекта</param>
  void AddValue(ImportingCategory category, object oldKey, long newKey, string caption);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="category">Идентификатор категории импортированного объекта</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  /// <param name="caption">Заголовок объекта</param>
  void AddValue(int category, object oldKey, long newKey, string caption);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="category">Категория импортированного объекта</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  /// <param name="tag">Доп. данные (используем по личному разумению)</param>
  void AddValue(ImportingCategory category, object oldKey, long newKey, ITagImportObject tag);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="category">Идентификатор категории импортированного объекта</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  /// <param name="tag">Доп. данные (используем по личному разумению)</param>
  void AddValue(int category, object oldKey, long newKey, ITagImportObject tag);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="category">Категория импортированного объекта</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  /// <param name="caption">Заголовок объекта</param>
  /// <param name="tag">Доп. данные (используем по личному разумению)</param>
  void AddValue(
    ImportingCategory category,
    object oldKey,
    long newKey,
    string caption,
    ITagImportObject tag);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="category">Идентификатор категории импортированного объекта</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  /// <param name="caption">Заголовок объекта</param>
  /// <param name="tag">Доп. данные (используем по личному разумению)</param>
  void AddValue(int category, object oldKey, long newKey, string caption, ITagImportObject tag);

  /// <summary>Получить идентификатор в новой базе</summary>
  /// <param name="category">Категория объектов</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <returns>Идентификатор в новой базе, либо Consts.UnknownObjectId, если не найден в кэше</returns>
  long GetNewKey(ImportingCategory category, object oldKey);

  /// <summary>Получить идентификатор в новой базе</summary>
  /// <param name="category">Идентификатор категории объектов</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <returns>Идентификатор в новой базе, либо Consts.UnknownObjectId, если не найден в кэше</returns>
  long GetNewKey(int category, object oldKey);

  /// <summary>Получить заголовок</summary>
  /// <param name="category">Категория объектов</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <returns></returns>
  string GetCaption(ImportingCategory category, object oldKey);

  /// <summary>Получить заголовок</summary>
  /// <param name="category">Идентификатор категории объектов</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <returns></returns>
  string GetCaption(int category, object oldKey);

  /// <summary>Получить значение</summary>
  /// <param name="category">Категория объектов</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <returns>Идентификатор в новой базе, либо null, если не найден в кэше</returns>
  ITagImportObject GetTag(ImportingCategory category, object oldKey);

  /// <summary>Получить значение</summary>
  /// <param name="category">Идентификатор категории объектов</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <returns>Идентификатор в новой базе, либо null, если не найден в кэше</returns>
  ITagImportObject GetTag(int category, object oldKey);

  /// <summary>Получить полностью значение из кэша</summary>
  /// <param name="category">Категория импортированного объекта</param>
  /// <param name="oldKey">Старый идентификатор</param>
  DictionaryValue GetValue(ImportingCategory category, object oldKey);

  /// <summary>Получить полностью значение из кэша</summary>
  /// <param name="category">Идентификатор категории объектов</param>
  /// <param name="oldKey">Старый идентификатор</param>
  DictionaryValue GetValue(int category, object oldKey);

  /// <summary>Получить полностью всю категорию</summary>
  /// <param name="category">Категория объектов</param>
  /// <returns>Dictionary для категории, если не найдено - null</returns>
  Dictionary<object, DictionaryValue> GetCategory(ImportingCategory category);

  /// <summary>Получить полностью всю категорию</summary>
  /// <param name="category">Идентификатор категории объектов</param>
  /// <returns>Dictionary для категории, если не найдено - null</returns>
  Dictionary<object, DictionaryValue> GetCategory(int category);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  void AddValue(object oldKey, long newKey);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  /// <param name="caption">Заголовок объекта</param>
  void AddValue(object oldKey, long newKey, string caption);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  /// <param name="tag">Доп. данные (используем по личному разумению)</param>
  void AddValue(object oldKey, long newKey, ITagImportObject tag);

  /// <summary>Добавить значение в кэш</summary>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <param name="newKey">Идентификатор в новой базе</param>
  /// <param name="caption">Заголовок объекта</param>
  /// <param name="tag">Доп. данные (используем по личному разумению)</param>
  void AddValue(object oldKey, long newKey, string caption, ITagImportObject tag);

  /// <summary>Получить идентификатор в новой базе</summary>
  /// <param name="category">Категория объектов</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <returns>Идентификатор в новой базе, либо -1, если не найден в кэше</returns>
  long GetNewKey(object oldKey);

  /// <summary>Получить заголовок</summary>
  /// <param name="category">Категория объектов</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <returns></returns>
  string GetCaption(object oldKey);

  /// <summary>Получить значение</summary>
  /// <param name="category">Категория объектов</param>
  /// <param name="oldKey">Старый идентификатор</param>
  /// <returns>Идентификатор в новой базе, либо null, если не найден в кэше</returns>
  ITagImportObject GetTag(object oldKey);

  /// <summary>Получить полностью значение из кэша</summary>
  /// <param name="oldKey">Старый идентификатор</param>
  DictionaryValue GetValue(object oldKey);

  /// <summary>Получить полностью всю категорию</summary>
  /// <param name="category">Категория объектов</param>
  /// <returns>Dictionary для категории, если не найдено - null</returns>
  Dictionary<object, DictionaryValue> GetCategory();

  /// <summary>Установить значение ID в новой базе в кэше</summary>
  bool SetNewKey(ImportingCategory category, object oldKey, long newKey);

  /// <summary>Установить значение ID в новой базе в кэше</summary>
  bool SetNewKey(int category, object oldKey, long newKey);

  /// <summary>Установить значение ID в новой базе в кэше</summary>
  bool SetNewKey(object oldKey, long newKey);

  /// <summary>Признак того, что категория category зачитана в кэш</summary>
  bool IsCategoryPresent(ImportingCategory category);

  /// <summary>Признак того, что категория category зачитана в кэш</summary>
  bool IsCategoryPresent(int category);

  /// <summary>
  /// Удаляет значение из кэша. Не поддерживает кэши со строковым ключом (RecordType.String).
  /// </summary>
  bool ClearValue(int category, object oldKey);
}
