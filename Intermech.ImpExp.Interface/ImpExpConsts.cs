// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImpExpConsts
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Константы программы закачки</summary>
public class ImpExpConsts
{
  /// <summary>Имя конфигурационного файла</summary>
  public const string ConfigFileName = "Intermech.ImpExp.Manager.cfg";
  /// <summary>Имя лога импорта на стороне сервера</summary>
  public const string ImportLogFileName = "ImpExpImport.log";
  /// <summary>Имя лога импорта на стороне клинта</summary>
  public const string LogFileName = "import.log";
  /// <summary>Имя папки с сохраненными файлами кэша</summary>
  public const string CacheFolderName = "ImpExp.TempData";
  public const string F_OBJECT_GUID = "F_OBJECT_GUID";
  public const string F_ATTRIBUTE_GUID = "F_ATTRIBUTE_GUID";
  public const string F_UNITS = "F_UNITS";
  public const string F_DISPLAY = "F_DISPLAY";
  public const string F_MATERIAL = "MATERIAL";
  public const string ART_ID = "ART_ID";
  public const string DOC_ID = "DOC_ID";
  public const string IMS_TABLE_RECORDS = "IMS_TABLE_RECORDS";
  public const int CurrentPumpVersion = 11;
  public const string DB_SETTINGS_NAME_SEARCH = "SEARCH4";
  public const string DB_SETTINGS_NAME_DOCUMS = "DOCUMS4";
  public const string DB_SETTINGS_NAME_IMBASE = "IMBASE";
  public const string DB_ALIAS_NAME_SEARCH = "SEARCH PLUGIN CONNECTION";
  public const string DB_ALIAS_NAME_IMBASE = "DATABASE CONNECTION";
}
