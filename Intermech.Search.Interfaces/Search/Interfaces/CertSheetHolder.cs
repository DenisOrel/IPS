// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Interfaces.CertSheetHolder
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Localization;

#nullable disable
namespace Intermech.Search.Interfaces;

public class CertSheetHolder
{
  /// <summary>Бланк УЛ по умолчанию (поставочная база)</summary>
  public static readonly string CertSheetBlankGUID = "cadd9670-306c-11d8-b4e9-00304f19f545";
  /// <summary>Модуль настроек для удостоверяющих листов</summary>
  public const string ModuleCertSheet = "CLIENT";
  /// <summary>Секция настроек для удостоверяющих листов</summary>
  public const string SectionCertSheet = "CERTSHEETS";
  /// <summary>
  /// Параметр указания бланка для формирования удостоверяющих листов
  /// </summary>
  public const string ParamBlankGuid = "BLANKGUID";
  /// <summary>
  /// Атрибут графы 9 для формирования удостоверяющих листов
  /// </summary>
  public const string ParamG09AttributeGuid = "G09ATTRIBUTEGUID";
  /// <summary>
  /// Атрибут графы 10 для формирования удостоверяющих листов
  /// </summary>
  public const string ParamG10AttributeGuid = "G10ATTRIBUTEGUID";
  /// <summary>Режим сортировки граф в УЛ</summary>
  public const string CertSheetGraphSort = "CERTSHEETGRAPHSORT";
  /// <summary>
  /// 
  /// </summary>
  public static readonly string DefaultParamBlankGuid = CertSheetHolder.CertSheetBlankGUID;
  /// <summary>Имя общей папки для удостоверяющих листов</summary>
  public const string ParamCertSheetCommonFolder = "COMMONFOLDER";
  /// <summary>
  /// 
  /// </summary>
  public static string DefaultParamCertSheetCommonFolder = LocalizationHolder.rm.GetString("CertSheetCommonFolder");
  /// <summary>Выводить только актальные подписи</summary>
  public const string ParamActualSignsOnly = "ACTUALSIGNSONLY";
  /// <summary>
  /// 
  /// </summary>
  public static bool DefaultParamActualSignsOnly = false;
  /// <summary>
  /// Атрибут графы 10 для формирования удостоверяющих листов по умолчанию
  /// </summary>
  public static readonly string DefaultParamG09AttributeGuid = string.Empty;
  /// <summary>
  /// Атрибут графы 10 для формирования удостоверяющих листов по умолчанию
  /// </summary>
  public static readonly string DefaultParamG10AttributeGuid = string.Empty;
}
