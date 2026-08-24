// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.DataWriter.IImportedAttributeList
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Interface.DataWriter;

/// <summary>
/// Интерфейс для добавления данных аттрибутов для последующего импорта
/// </summary>
public interface IImportedAttributeList
{
  /// <summary>Добавление атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="attrValtype">Тип значения атрибута</param>
  /// <param name="attrVal">Значение атрибута</param>
  /// <param name="numInList">Номер значения в списке (0 - если атрибут не список значений)</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttribute(
    int attrType,
    AttrValueType attrValtype,
    object attrVal,
    int numInList);

  /// <summary>Добавление атрибута</summary>
  /// <param name="relAttr"></param>
  /// <returns></returns>
  AttributeRecord AddAttribute(AttributeRecord relAttr);

  /// <summary>Добавление null атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeNull(int attrType);

  /// <summary>Добавление null атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeNull(int attrType, int numInList);

  /// <summary>Добавление строкового атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeStr(int attrType, string value);

  /// <summary>Добавление целочисленного атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeInt(int attrType, long value);

  /// <summary>Добавление вещественного атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeDouble(int attrType, double value);

  /// <summary>Добавление временн'ого атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeDate(int attrType, DateTime value);

  /// <summary>Добавление ссылочного атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Идентификатор версии ссылочного объекта</param>
  /// <param name="caption">Заголовок ссылочного объекта</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeLink(int attrType, long value, string caption);

  /// <summary>Добавление ссылочного атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Идентификатор версии ссылочного объекта</param>
  /// <param name="caption">Заголовок ссылочного объекта</param>
  /// <param name="numInList">Номер значения в списке (0 - если атрибут не список значений)</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeLink(int attrType, long value, string caption, int inListID);

  /// <summary>
  /// Добавление вещественного атрибута выраженного в единицах измерения
  /// </summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <param name="measureID">Иденификатор единицы измерения</param>
  /// <param name="strValue">Строка со значением</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeMeasure(int attrType, double value, long measureID, string strValue);

  /// <summary>
  /// Добавление вещественного атрибута выраженного в единицах измерения
  /// </summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="value">Значение атрибута</param>
  /// <param name="measureID">Иденификатор единицы измерения</param>
  /// <param name="strValue">Строка со значением</param>
  /// <param name="inListID">Номер в списке</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeMeasure(
    int attrType,
    double value,
    long measureID,
    string strValue,
    int inListID);

  /// <summary>Добавление blob-атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="filePath">Путь к файлу</param>
  /// <param name="fileSize">Размер файла</param>
  /// <param name="fileNote">Комментарии к файлу</param>
  /// <param name="arcMethod">Метод упаковки</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeBlob(
    int attrType,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod);

  /// <summary>Добавление blob-атрибута</summary>
  /// <param name="attrType">Идентификатор типа атрибута</param>
  /// <param name="filePath">Путь к файлу</param>
  /// <param name="fileSize">Размер файла</param>
  /// <param name="fileNote">Комментарии к файлу</param>
  /// <param name="arcMethod">Метод упаковки</param>
  /// <param name="inListId">Номер в списке</param>
  /// <returns>Идентификатор типа созданного атрибута</returns>
  AttributeRecord AddAttributeBlob(
    int attrType,
    string filePath,
    long fileSize,
    string fileNote,
    ArcMethods arcMethod,
    int inListId);

  /// <summary>
  /// Изменяет у ТЕКУЩЕГО ЭЛЕМЕНТА (ОБЪЕКТА / СВЯЗИ) значение СТРОКОВОГО атрибута
  /// </summary>
  /// <param name="attributeId"></param>
  /// <param name="newValue"></param>
  void ReplaceAttributeStr(int attributeId, string newValue);
}
