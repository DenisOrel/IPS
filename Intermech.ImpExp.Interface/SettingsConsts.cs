// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SettingsConsts
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

public class SettingsConsts
{
  /// <summary>
  /// Атрибут нода для сохранения гуида назначенного типа объектов
  /// </summary>
  public const string sAttributeGUID = "GUID";
  /// <summary>
  /// Атрибут нода для сохранения мдентификатора назначенного типа объектов
  /// </summary>
  public const string sAttributeID = "ID";
  /// <summary>Атрибут нода для сохранения гуида родительского типа</summary>
  public const string sAttributeParent = "PARENT";
  /// <summary>
  /// Атрибут нода для сохранения гуида типа связи по-умолчанию
  /// </summary>
  public const string sAttributeDefRel = "RELATION_TYPE";
  /// <summary>Атрибут нода для сохранения гуида схемы ЖЦ</summary>
  public const string sAttributeLCScheme = "LC_SCHEME";
  /// <summary>
  /// Атрибут нода для сохранения названия объектов данного типа
  /// </summary>
  public const string sAttributeInstName = "INST_NAME";
  /// <summary>Атрибут нода для сохранения краткого наименования</summary>
  public const string sAttributeShortName = "SHORT_NAME";
  /// <summary>Атрибут нода для сохранения комментариев</summary>
  public const string sAttributeNote = "NOTE";
  /// <summary>Атрибут нода для сохранения версионности</summary>
  public const string sAttributeVersionable = "VERSIONABLE";
  /// <summary>Атрибут нода для сохранения свойства "Любой атрибут"</summary>
  public const string sAttributeAnyAttribute = "ANY_ATTRIBUTE";
  /// <summary>Атрибут нода для сохранения иконки</summary>
  public const string sAttributeIcon = "ICON";
  /// <summary>Атрибут нода для сохранения результата проверки</summary>
  public const string sAttributeCheck = "CHECKRES";
  /// <summary>Атрибут нода для сохранения нового имени</summary>
  public const string sAttributeName = "NEW_NAME";
  /// <summary>Атрибут нода для сохранения типа атрибута</summary>
  public const string sAttributeFieldType = "FIELDTYPE";
  /// <summary>Атрибут нода для сохранения длины</summary>
  public const string sAttributeSize = "SIZE";
  /// <summary>Атрибут нода для сохранения длины</summary>
  public const string sAttributeAlias = "ALIAS";
  /// <summary>Атрибут нода для GUIDa типа объекта</summary>
  public const string sAttributeObjType = "OBJTYPE_GUID";
  public const string sAttributeEnabled = "ENABLED";
  /// <summary>Название нода для атрибута Imbase</summary>
  public const string nAttributeImbase = "IMBASEFIELD";
  /// <summary>Название нода для привязки каталога Imbase</summary>
  public const string nCatalogImbase = "IMBASECATALOG";
  /// <summary>Название нода для ед. измеренияImbase</summary>
  public const string nMeasureImbase = "MEASURES";
  /// <summary>Настроеные пользователем понятиия</summary>
  public const string entitys = "SaveSettingsEnts";
  /// <summary>Настройки TechCard</summary>
  public const string TechCardSettings = "TECHCARDSETTINGS";
}
