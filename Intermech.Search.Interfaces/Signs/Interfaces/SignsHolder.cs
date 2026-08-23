// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.SignsHolder
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Checksums;
using Intermech.ComponentModel;
using Intermech.Interfaces;
using Intermech.Interfaces.Plugins;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Держатель всех используемых констант и др.</summary>
public class SignsHolder
{
  /// <summary>
  /// Проверять сертификаты
  /// Если true, то производится дополнительная проверка сертификата через сервер отзыва сертификатов (online или offline - определяется RevocationMode )
  /// при подписывании и при проверке подписи.
  /// </summary>
  public static bool DoRevocation = false;
  /// <summary>
  /// Если выполнять доп проверку сертификатов, то режим - online или offline - через сеть или offline списки
  /// </summary>
  public static X509RevocationMode RevocationMode = X509RevocationMode.Online;
  /// <summary>
  /// Режим разработчика для подписей - дополнительное логирование и все такое
  /// </summary>
  public static bool SignsDeveloperMode = false;
  /// <summary>
  /// Диалог подтверждения при подписывании одиночных объектов
  /// </summary>
  public static bool ConfirmSingleSigning = true;
  /// <summary>
  /// Режим подписи только через сертификаты.
  /// если false, то при наличии открытых ключей они предлагаются для подписи наравне с сертификатами.
  /// если true, то для подписи предлагаются только сертификаты независимо от наличия у пользователей открытых ключей.
  /// </summary>
  public static bool CertificateSigningOnlyMode = true;
  /// <summary>
  /// Режим копирования подписи при создании версии объекта
  /// false - не копировать подписи в версию
  /// true - копировать подписи в версию
  /// </summary>
  public static bool CopySignsToVersionMode = false;
  /// <summary>
  /// Режим проверки наличия акутальной подписи в выбранной графе при подписании
  /// false - не проверять наличие актуальной подписи
  /// true - проверять наличие актуальной копии
  /// </summary>
  public static bool СheckExistingCopyActualityMode = false;
  /// <summary>Текст для отображения актуальных простых ЭП;</summary>
  public static string TextForActualSimpleSign = LocalizationHolder.rm.GetString("Signs_TextForActualSimpleSign");
  /// <summary>Текст для отображения неактуальных простых ЭП</summary>
  public static string TextForNonActualSimpleSign = LocalizationHolder.rm.GetString("Signs_TextForNonActualSimpleSign");
  /// <summary>Способ отображения квалифицированнных ЭП</summary>
  public static SignsHolder.SignDisplayMode QualifiedSignDisplayMode = SignsHolder.SignDisplayMode.Text;
  /// <summary>Текст для отображения актуальных квалифицированных ЭП</summary>
  public static string TextForActualQualifiedSign = LocalizationHolder.rm.GetString("Signs_TextForActualQualifiedSign");
  /// <summary>
  /// Количество последних символов ключа квалифицированной ЭП
  /// </summary>
  public static uint QualifiedSignKeyLastSymbolsNumber = 20;
  /// <summary>
  /// Параметр "Текст для отображения неактуальных квалифицированных ЭП"
  /// </summary>
  public static string TextForNonActualQualifiedSign = LocalizationHolder.rm.GetString("Signs_TextForNonActualQualifiedSign");
  /// <summary>
  /// Проверять наличие актуальной подписи, поставленной тем же пользователем
  /// false - можно переподписывать
  /// true - переподписывать нельзя
  /// </summary>
  public static bool CheckActualSignMadeBySameUser = false;
  /// <summary>Имя модуля для ЭЦП</summary>
  public const string ModuleSigns = "SIGNS";
  /// <summary>Секция настроек для сертификатов</summary>
  public const string SectionCertificates = "CERTIFICATES";
  /// <summary>Секция настроек интерфейса подписывания</summary>
  public const string SectionInterface = "INTERFACE";
  /// <summary>Секция настроек отображения подписей</summary>
  public const string SectionSignDisplaying = "SIGNDISPLAYING";
  /// <summary>
  /// Параметр режима проверки сертификатов  true - проверять через серверы/списки отзыва, false - не проверять.
  /// примечание: название ONLINE_REVOCATION несколько не соответствует смыслу настройки, скорее это DO_REVOCATION; менять уже поздно, поэтому как есть, так есть
  /// </summary>
  public const string ParamDoRevocation = "ONLINE_REVOCATION";
  /// <summary>
  /// при ParamOnlineRevocation = true, true - проверять online, false - проверять offline
  /// по докам CryptoAPI:
  /// Online — проверяет сертификаты в цепочке скачивая новые списки CRL из свойства CDP сертификата, игнорируя локальный кеш CRL (используется по умолчанию)
  /// Offline — проверяет сертификаты на отзыв с использованием кешированного CRL (если есть).
  /// </summary>
  public const string ParamOnlineModeRevocation = "ONLINE_MODE_REVOCATION";
  /// <summary>Режим разработчика для модуля подписей</summary>
  public const string ParamSignsDeveloperMode = "SIGNS_DEVELOPER_MODE";
  /// <summary>
  /// Диалог подтверждения при подписывании одиночных объектов
  /// </summary>
  public const string ParamConfirmSingleSigning = "USER_CONFIRM_SINGLE_SIGNING";
  /// <summary>Параметр режима проверки сертификатов</summary>
  public const string ParamCertificateSigningOnly = "CERTIFICATE_SIGNING_ONLY";
  /// <summary>
  /// Параметр режима копирования подписей при создании версии объекта
  /// </summary>
  public const string ParamCopySignsToVersion = "COPY_SIGNS_TO_VERSION";
  /// <summary>
  /// Параметр проверки наличия актуальной подписи в выбранной графе при подписании
  /// </summary>
  public const string ParamCheckExistingCopyActuality = "CHECK_COPY_ACTUALITY";
  /// <summary>
  /// Параметр проверки наличия актуальной подписи, поставленной тем же пользователем
  /// </summary>
  public const string ParamCheckActualSignMadeBySameUser = "CHECK_ACTUAL_SIGN_BY_SAME_USER";
  /// <summary>
  /// Параметр "Текст для отображения актуальных простых ЭП"
  /// </summary>
  public const string ParamTextForActualSimpleSign = "TEXT_FOR_ACTUAL_SIMPLE_SIGN";
  /// <summary>
  /// Параметр "Текст для отображения неактуальных простых ЭП"
  /// </summary>
  public const string ParamTextForNonActualSimpleSign = "TEXT_FOR_NON_ACTUAL_SIMPLE_SIGN";
  /// <summary>Параметр "Способ отображения квалифицированнных ЭП"</summary>
  public const string ParamQualifiedSignDisplayMode = "QUALIFIED_SIGN_DISPLAY_MODE";
  /// <summary>
  /// Параметр "Текст для отображения актуальных квалифицированных ЭП"
  /// </summary>
  public const string ParamTextForActualQualifiedSign = "TEXT_FOR_ACTUAL_QUALIFIED_SIGN";
  /// <summary>
  /// Параметр "Количество последних символов ключа квалифицированной ЭП"
  /// </summary>
  public const string ParamQualifiedSignKeyLastSymbolsNumber = "SIGN_KEY_LAST_SYMBOLS_NUMBER";
  /// <summary>
  /// Параметр "Текст для отображения неактуальных квалифицированных ЭП"
  /// </summary>
  public const string ParamTextForNonActualQualifiedSign = "TEXT_FOR_NON_ACTUAL_QUAL_SIGN";
  /// <summary>значения по умолчанию</summary>
  public const bool DefaultParamDoRevocation = false;
  public const bool DefaultParamOnlineModeRevocation = true;
  public const bool DefaultParamSignsDeveloperMode = false;
  public const bool DefaultParamConfirmSingleSigning = true;
  public const bool DefaultParamCertificateSigningOnly = true;
  public const bool DefaultParamCopySignsToVersion = false;
  public const bool DefaultParamCheckExistingCopyActuality = false;
  public const bool DefaultParamCheckActualSignMadeBySameUser = false;
  public const string DefaultParamTextForActualSimpleSign = "<Подп.>";
  public const string DefaultParamTextForNonActualSimpleSign = "???";
  public const string DefaultParamTextForActualQualifiedSign = "<Подп.>";
  public const string DefaultParamTextForNonActualQualifiedSign = "???";
  public const uint DefaultParamQualifiedSignKeyLastSymbolsNumber = 20;
  public const SignsHolder.SignDisplayMode DefaultParamQualifiedSignDisplayMode = SignsHolder.SignDisplayMode.Text;
  private static bool signOutputEnabled;
  private static bool signOutputEnabledDevelop;
  private static ChecksumAlgorithm checkSumType = ChecksumAlgorithm.Crc32;
  private static string checkSumAttribute = "CRC32";
  /// <summary>
  /// Имя параметра, в который будет передаваться фамилия из ЭП.
  /// </summary>
  private static string signSurnameParam = LocalizationHolder.rm.GetString("SignGraph");
  private static string signValueParam = LocalizationHolder.rm.GetString(nameof (SignValueParam));
  private static string signDateParam = LocalizationHolder.rm.GetString(nameof (SignDateParam));
  private static string signRankParam = LocalizationHolder.rm.GetString(nameof (SignRankParam));
  private static string signGraphNameParam = LocalizationHolder.rm.GetString(nameof (SignGraphNameParam));
  private static string signDateFormatParam = LocalizationHolder.rm.GetString(nameof (SignDateFormatParam));
  /// <summary>Секция настроек для параметров вывода подписей</summary>
  public const string SectionOutputParams = "OUTPUT_PARAMS";
  /// <summary>
  /// Параметр, в котором будет храниться разрешение на вывод подписей
  /// </summary>
  public const string ParamSignOutputEnabledParam = "SIGN_OUTPUT_ENABLED";
  /// <summary>
  /// Параметр, в котором будет храниться разрешение на вывод подписей
  /// </summary>
  public const string ParamSignOutputEnabledDevelopParam = "SIGN_OUTPUT_ENABLED_DEVELOP";
  /// <summary>Параметр, в который будет передаваться фамилия из ЭП</summary>
  public const string ParamSignSurnameParam = "SIGN_SURNAME_PARAM";
  /// <summary>
  /// Параметр, в который будет передаваться значение ЭП в соответствии с настройкой вывода ЭП
  /// </summary>
  public const string ParamSignValueParam = "SIGN_VALUE_PARAM";
  /// <summary>Параметр, в который будет передаваться дата из ЭП;</summary>
  public const string ParamSignDateParam = "SIGN_DATE_PARAM";
  /// <summary>
  /// Параметр, в который будет передаваться наименование графы ЭП;
  /// </summary>
  public const string ParamSignGraphNameParam = "SIGN_GRAPH_NAME_PARAM";
  /// <summary>
  /// Параметр, в который будет передаваться должность из ЭП;
  /// </summary>
  public const string ParamSignRankParam = "SIGN_RANK_PARAM";
  /// <summary>Способ получения контрольной суммы;</summary>
  public const string ParamCheckSumTypeParam = "SIGN_CHECKSUMTYPE_PARAM";
  /// <summary>Наименование свойства, в которое передается сумма;</summary>
  public const string ParamCheckSumAttribute = "SIGN_CHECKSUM_PARAM";
  /// <summary>
  /// Параметр, в который будет передаваться формат вывода даты подписи;
  /// </summary>
  public const string ParamSignDateFormat = "SIGN_DATE_FORMAT";
  public const string DefaultParamSignSurnameParam = "[Графа для подписи]";
  public const string DefaultParamSignValueParam = "[Графа для подписи]_ЭЦП";
  public const string DefaultParamSignDateParam = "[Графа для подписи]_Дата";
  public const string DefaultParamSignRankParam = "[Графа для подписи]_Должность";
  public const string DefaultParamSignGraphNameParam = "[Графа для подписи]_Графа";
  public const string DefaultParamSignDateFormat = "dd.MM.yyyy";
  public const string DefaultParamCheckSumAttribute = "CRC32";
  /// <summary>Является ли юзер администратором</summary>
  public static bool isAdmin = false;
  /// <summary>Контейнер для хранения ключей в криптопровайдерах</summary>
  public const string IntermechKeysContainer = "Intermech_Keys_Container";
  /// <summary>Имя плагина "Архивы"</summary>
  public static readonly string ArchivesName = LocalizationHolder.rm.GetString("Search.Interfaces_8");
  /// <summary>Чранит значени "Загружен ли плагин "Архивы""</summary>
  public static bool isArchivesLoaded = false;
  /// <summary>Имя плагина "Конфигуратор базы данных"</summary>
  public static readonly string DatabaseConfiguratorName = LocalizationHolder.rm.GetString("Search.Interfaces_9");
  /// <summary>
  /// Чранит значени "Загружен ли плагин "Конфигуратор базы данных""
  /// </summary>
  public static bool isDatabaseConfiguratorLoaded = false;
  /// <summary>Factory сервис навигатора</summary>
  public static object Factory = (object) null;
  /// <summary>Сервис главного меню/</summary>
  public static object Bar = (object) null;
  /// <summary>Тип объекта "Должность"</summary>
  public static readonly Guid RankTypeGuid = new Guid("cad00147-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа объекта "Должность"</summary>
  public static int RankTypeID = 0;
  /// <summary>Тип объекта "Архив"</summary>
  public static readonly Guid ArchTypeGuid = new Guid("cad0011e-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа объекта "Архив"</summary>
  public static int ArchTypeID = 0;
  /// <summary>Тип объекта "Контейнер атрибутов"</summary>
  public static readonly Guid ContainerObjectTypeGuid = new Guid("cad0013b-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор тип объекта "Контейнер атрибутов"</summary>
  public static int ContainerObjectTypeID = 0;
  /// <summary>Тип объекта "Подпись"</summary>
  public static readonly Guid SignObjectTypeGuid = new Guid("cad00137-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа объектов "Подпись"</summary>
  public static int SignObjectTypeID = 0;
  /// <summary>Тип объекта "Подпись с криптозащитой"</summary>
  public static readonly Guid CryptoSignObjectTypeGuid = new Guid("cad00138-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа объекта "Подпись с криптозащитой"</summary>
  public static int CryptoSignObjectTypeID = 0;
  /// <summary>Тип объекта "Внешний криптопровайдер"</summary>
  [Obsolete("Внешние криптопровайдеры упразднены в IPS V5 SP6")]
  public static readonly Guid ExternalCryptoObjectTypeGuid = new Guid("cad00153-306c-11d8-b4e9-00304f19f545");
  /// <summary>Атрибут "Должность по штатному расписанию"</summary>
  public static readonly Guid StaffPositionAttrGuid = new Guid("cadd9b72-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор атрибута "Должность по штатному расписанию"
  /// </summary>
  public static int StaffPositionAttrID = 0;
  /// <summary>Тип объекта "Документы"</summary>
  public static readonly Guid DocumentObjectTypeGuid = new Guid("cad00070-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа объекта "Документы"</summary>
  public static int DocumentObjectTypeID = 0;
  /// <summary>Тип связи "Подпись"</summary>
  public static readonly Guid SignRelationTypeGuid = new Guid("cad00139-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа связи "Подпись"</summary>
  public static int SignRelationTypeID = 0;
  /// <summary>Атрибут "Графа для подписи"</summary>
  public static readonly Guid GraphAttrTypeGuid = new Guid("cad00141-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атирибута "Шаг ЖЦ + тип объекта"</summary>
  public static int LCStepObjectTypeID = 0;
  /// <summary>Атрибут "Шаг ЖЦ + тип объекта"</summary>
  public static readonly Guid LCStepObjectTypeGuid = new Guid("cad00922-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Графа для подписи"</summary>
  public static int GraphAttrTypeID = 0;
  /// <summary>Атрибут "Настройка подписей"</summary>
  public static readonly Guid SignsSetupAttrTypeGuid = new Guid("cad00148-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "ЭЦП (данные для хэша)"</summary>
  public static int SignDataSequenceTypeID = 0;
  /// <summary>Атрибут "ЭЦП (данные для хэша)"</summary>
  public static readonly Guid SignDataSequenceAttrTypeGuid = new Guid("cadd968c-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Настройка подписей"</summary>
  public static int SignsSetupAttrTypeID = 0;
  /// <summary>тип атрибута "Защита объекта"</summary>
  public static readonly Guid HashProtectionAttrTypeGuid = new Guid("cad0014b-306c-11d8-b4e9-00304f19f545");
  /// <summary>идентификатор типа атрибута "Защита объекта"</summary>
  public static int HashProtectionAttrTypeID = 0;
  /// <summary>Наименование типа атрибута "Защита объекта"</summary>
  public static string HashProtectionAttrTypeName = "";
  /// <summary>Атрибут "Должность"</summary>
  public static readonly Guid RankAttrTypeGuid = new Guid("cad00142-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Должность"</summary>
  public static int RankAttrTypeID = 0;
  /// <summary>Атрибут "Наименование"</summary>
  public static readonly Guid NaimAttrTypeGuid = new Guid("cad00020-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентифкатор типа атрибута "Наименование"</summary>
  public static int NaimAttrTypeID = 0;
  /// <summary>Атрибут "Выводимое имя"</summary>
  public static readonly Guid VisibleNameAttrTypeGuid = new Guid("cad0001d-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Выводимое имя"</summary>
  public static int VisibleNameAttrTypeID = 0;
  /// <summary>Атрибут "Идентификатор алгоритма криптопровайдера"</summary>
  public static readonly Guid CryptoAlgIDAttrTypeGuid = new Guid("cad00159-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Идентифкиаор алгоритма криптопровайдера"
  /// </summary>
  public static int CryptoAlgIDAttrTypeID = 0;
  /// <summary>Атрибут "Идентификатор типа криптопровайдера"</summary>
  public static readonly Guid CryptoTypeAttrTypeGuid = new Guid("cad0015a-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Идентификатор типа криптопровайдера"
  /// </summary>
  public static int CryptoTypeAttrTypeID = 0;
  /// <summary>Атрибут "Архив"</summary>
  public static readonly Guid ArchAttrTypeGuid = SystemGUIDs.attributeArchive;
  /// <summary>Идентификатор типа атрибута "Архив"</summary>
  public static int ArchAttrTypeID = 0;
  /// <summary>Атрибут "Архивная копия"</summary>
  public static readonly Guid InArchiveAttrTypeGuid = new Guid("cad00144-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Архивная копия"</summary>
  public static int InArchiveAttrTypeID = 0;
  /// <summary>Атрибут "Глобальный идентификатор типа объектов"</summary>
  public static readonly Guid GlobalTypeObjectTypeAttrGuid = new Guid("cad00149-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Глобальный идентификатор типа объектов"
  /// </summary>
  public static int GlobalTypeObjectTypeAttrID = 0;
  /// <summary>Атрибут "Шаг жизненного цикла для проверки подписей"</summary>
  public static readonly Guid LCStepForSignsAttrTypeGuid = new Guid("cad0014c-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Шаг жизенного цикла для проверки подписей"
  /// </summary>
  public static int LCStepForSignsAttrTypeID = 0;
  /// <summary>
  /// Атрибут "Уровень продвижения объекта для проверки подписей"
  /// </summary>
  public static readonly Guid LCLevelForSignsAttrTypeGuid = new Guid("cad0015b-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Иеднтификатор типа атрибута "Уровень продвижения объекта для проверки подписей"
  /// </summary>
  public static int LCLevelForSignsAttrTypeID = 0;
  /// <summary>Атрибут "Дата модификации содержимого объекта"</summary>
  public static readonly Guid ModifyDateAttrTypeGuid = new Guid("cad0013a-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Дата модификации содержимого объекта"
  /// </summary>
  public static int ModifyDateAttrTypeID = 0;
  /// <summary>Атрибут "Подписал"</summary>
  public static readonly Guid SignUpAttrTypeGuid = new Guid("cad00143-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Подписал"</summary>
  public static int SignUpAttrTypeID = 0;
  /// <summary>Наименование типа атрибута "Подписал"</summary>
  public static string SignUpAttrTypeName = "";
  /// <summary>Атрибут "Открытые ключи"</summary>
  public static readonly Guid OpenKeysAttrTypeGuid = new Guid("cad00152-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Открытые ключи"</summary>
  public static int OpenKeysAttrTypeID = 0;
  /// <summary>Атрибут "Дата создания объекта"</summary>
  public static readonly Guid CreateDateAttrTypeGuid = new Guid("cad0013c-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Дата создания объекта"</summary>
  public static int CreateDateAttrTypeID = 0;
  /// <summary>Атрибут "Версия подписи"</summary>
  public static readonly Guid SignVersionAttrTypeGuid = new Guid("cad00145-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Версия подписи"</summary>
  public static int SignVersionAttrTypeID = 0;
  /// <summary>Атрибут "Электронная цифровая подпись"</summary>
  public static readonly Guid EDSAttrTypeGuid = new Guid("cad00146-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Электронная цифровая подпись"
  /// </summary>
  public static int EDSAttrTypeID = 0;
  /// <summary>Атрибут "Фамилия пользователя в графах для подписи"</summary>
  public static readonly Guid FIOInSignAttrTypeGuid = new Guid("cad0036e-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Фамилия пользователя в графах для подписи"
  /// </summary>
  public static int FIOInSignAttrTypeID = 0;
  /// <summary>Guid атрибута типа  Резолюция</summary>
  public static readonly Guid ResolutionAttrTypeGuid = new Guid("cad0147f-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Резолюция"</summary>
  public static int ResolutionAttrTypeID = 0;
  /// <summary>Атрибут Дата подписания"</summary>
  public static readonly Guid DateOfSignatureGuid = new Guid("cad014cb-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атрибута "Дата подписания"</summary>
  public static int DateOfSignatureID = 0;
  /// <summary>Атрибут "Фамилия пользователя в графах для подписи"</summary>
  public static readonly Guid SurnameForSignAttrTypeGuid = new Guid("cad0036e-306c-11d8-b4e9-00304f19f545");
  /// <summary>
  /// Идентификатор типа атрибута "Фамилия пользователя в графах для подписи"
  /// </summary>
  public static int SurnameForSignAttrTypeID = 0;
  /// <summary>Уровень продвижения "Уволен"</summary>
  public static int FiredUserLevelID = 0;
  /// <summary>Атрибут "И. О."</summary>
  public static readonly Guid SignUpIOAttrTypeGuid = new Guid("cadd91f5-306c-11d8-b4e9-00304f19f545");
  /// <summary>Идентификатор типа атирибута "И. О."</summary>
  public static int SignUpIOAttrTypeID = 0;

  /// <summary>Разрешает и запрещает использование вывода подписей</summary>
  public static bool SignOutputEnabled
  {
    get => SignsHolder.signOutputEnabled;
    private set => SignsHolder.signOutputEnabled = value;
  }

  /// <summary>
  /// Разрешает и запрещает использование вывода подписей в документы на этапе в разработке
  /// </summary>
  public static bool SignOutputEnabledDevelop
  {
    get => SignsHolder.signOutputEnabledDevelop;
    private set => SignsHolder.signOutputEnabledDevelop = value;
  }

  /// <summary>Способ получения контрольной суммы;</summary>
  public static ChecksumAlgorithm CheckSumType
  {
    get => SignsHolder.checkSumType;
    set => SignsHolder.checkSumType = value;
  }

  /// <summary>Наименование свойства, в которое передается сумма;</summary>
  public static string CheckSumAttribute
  {
    get => SignsHolder.checkSumAttribute;
    set => SignsHolder.checkSumAttribute = value;
  }

  /// <summary>Gets or sets the sign surname param.</summary>
  /// <value>The sign surname param.</value>
  public static string SignSurnameParam
  {
    get => SignsHolder.signSurnameParam;
    private set => SignsHolder.signSurnameParam = value;
  }

  /// <summary>
  /// Имя параметра, в который будет передаваться значение ЭП в соответствии с настройкой вывода ЭП
  /// </summary>
  public static string SignValueParam
  {
    get => SignsHolder.signValueParam;
    private set => SignsHolder.signValueParam = value;
  }

  /// <summary>
  /// Имя параметра, в который будет передаваться дата из ЭП;
  /// </summary>
  public static string SignDateParam
  {
    get => SignsHolder.signDateParam;
    private set => SignsHolder.signDateParam = value;
  }

  /// <summary>
  /// Имя параметра, в который будет передаваться должность ЭП;
  /// </summary>
  public static string SignRankParam
  {
    get => SignsHolder.signRankParam;
    private set => SignsHolder.signRankParam = value;
  }

  /// <summary>
  /// Имя параметра, в который будет передаваться наименование графы ЭП;
  /// </summary>
  public static string SignGraphNameParam
  {
    get => SignsHolder.signGraphNameParam;
    private set => SignsHolder.signGraphNameParam = value;
  }

  /// <summary>
  /// Имя параметра, в который будет передаваться формат вывода даты в документы
  /// </summary>
  public static string SignDateFormatParam
  {
    get => SignsHolder.signDateFormatParam;
    private set => SignsHolder.signDateFormatParam = value;
  }

  /// <summary>Инициализация констант</summary>
  /// <param name="session">Пользовательская сессия</param>
  /// <param name="serviceProvider">Сервисы</param>
  public static void Init(IUserSession session, IServiceProvider serviceProvider)
  {
    SignsHolder.isAdmin = session.IsAdmin;
    if (serviceProvider != null)
    {
      foreach (IPlugin plugin in (IEnumerable<IPlugin>) (serviceProvider.GetService(typeof (IPluginManager)) as IPluginManager).Plugins)
      {
        foreach (IPackage package in (IEnumerable<IPackage>) plugin.Packages)
        {
          if (package.Name.Equals(SignsHolder.ArchivesName))
            SignsHolder.isArchivesLoaded = true;
          if (package.Name.Equals(SignsHolder.DatabaseConfiguratorName))
            SignsHolder.isDatabaseConfiguratorLoaded = true;
        }
      }
    }
    SignsHolder.SignRelationTypeID = session.GetRelationType(SignsHolder.SignRelationTypeGuid).RelationType;
    SignsHolder.RankTypeID = session.GetObjectType(SignsHolder.RankTypeGuid).ObjectType;
    SignsHolder.ArchTypeID = session.GetObjectType(SignsHolder.ArchTypeGuid).ObjectType;
    SignsHolder.SignObjectTypeID = session.GetObjectType(SignsHolder.SignObjectTypeGuid).ObjectType;
    SignsHolder.CryptoSignObjectTypeID = session.GetObjectType(SignsHolder.CryptoSignObjectTypeGuid).ObjectType;
    SignsHolder.ContainerObjectTypeID = session.GetObjectType(SignsHolder.ContainerObjectTypeGuid).ObjectType;
    SignsHolder.StaffPositionAttrID = session.GetAttributeType(SignsHolder.StaffPositionAttrGuid).AttributeID;
    SignsHolder.DocumentObjectTypeID = session.GetObjectType(SignsHolder.DocumentObjectTypeGuid).ObjectType;
    SignsHolder.GraphAttrTypeID = session.GetAttributeType(SignsHolder.GraphAttrTypeGuid).AttributeID;
    SignsHolder.SignDataSequenceTypeID = session.GetAttributeType(SignsHolder.SignDataSequenceAttrTypeGuid).AttributeID;
    SignsHolder.SignsSetupAttrTypeID = session.GetAttributeType(SignsHolder.SignsSetupAttrTypeGuid).AttributeID;
    IDBAttributeType attributeType1 = session.GetAttributeType(SignsHolder.HashProtectionAttrTypeGuid, false);
    if (attributeType1 != null)
    {
      SignsHolder.HashProtectionAttrTypeID = attributeType1.AttributeID;
      SignsHolder.HashProtectionAttrTypeName = attributeType1.Name;
    }
    SignsHolder.RankAttrTypeID = session.GetAttributeType(SignsHolder.RankAttrTypeGuid).AttributeID;
    SignsHolder.NaimAttrTypeID = session.GetAttributeType(SignsHolder.NaimAttrTypeGuid).AttributeID;
    SignsHolder.ArchAttrTypeID = session.GetAttributeType(SignsHolder.ArchAttrTypeGuid).AttributeID;
    SignsHolder.GlobalTypeObjectTypeAttrID = session.GetAttributeType(SignsHolder.GlobalTypeObjectTypeAttrGuid).AttributeID;
    SignsHolder.LCStepForSignsAttrTypeID = session.GetAttributeType(SignsHolder.LCStepForSignsAttrTypeGuid).AttributeID;
    SignsHolder.LCLevelForSignsAttrTypeID = session.GetAttributeType(SignsHolder.LCLevelForSignsAttrTypeGuid).AttributeID;
    SignsHolder.InArchiveAttrTypeID = session.GetAttributeType(SignsHolder.InArchiveAttrTypeGuid).AttributeID;
    SignsHolder.ModifyDateAttrTypeID = session.GetAttributeType(SignsHolder.ModifyDateAttrTypeGuid).AttributeID;
    IDBAttributeType attributeType2 = session.GetAttributeType(SignsHolder.SignUpAttrTypeGuid);
    SignsHolder.SignUpAttrTypeID = attributeType2.AttributeID;
    SignsHolder.SignUpAttrTypeName = attributeType2.Name;
    SignsHolder.OpenKeysAttrTypeID = session.GetAttributeType(SignsHolder.OpenKeysAttrTypeGuid).AttributeID;
    SignsHolder.VisibleNameAttrTypeID = session.GetAttributeType(SignsHolder.VisibleNameAttrTypeGuid).AttributeID;
    SignsHolder.CryptoAlgIDAttrTypeID = session.GetAttributeType(SignsHolder.CryptoAlgIDAttrTypeGuid).AttributeID;
    SignsHolder.CryptoTypeAttrTypeID = session.GetAttributeType(SignsHolder.CryptoTypeAttrTypeGuid).AttributeID;
    SignsHolder.CreateDateAttrTypeID = session.GetAttributeType(SignsHolder.CreateDateAttrTypeGuid).AttributeID;
    SignsHolder.SignVersionAttrTypeID = session.GetAttributeType(SignsHolder.SignVersionAttrTypeGuid).AttributeID;
    SignsHolder.EDSAttrTypeID = session.GetAttributeType(SignsHolder.EDSAttrTypeGuid).AttributeID;
    SignsHolder.FIOInSignAttrTypeID = session.GetAttributeType(SignsHolder.FIOInSignAttrTypeGuid).AttributeID;
    SignsHolder.LCStepObjectTypeID = session.GetAttributeType(SignsHolder.LCStepObjectTypeGuid).AttributeID;
    SignsHolder.FiredUserLevelID = session.GetLifecycleStep(new Guid("cadd9504-306c-11d8-b4e9-00304f19f545")).LevelID;
    SignsHolder.DateOfSignatureID = session.GetAttributeType(SignsHolder.DateOfSignatureGuid).AttributeID;
    IDBAttributeType attributeType3 = session.GetAttributeType(SignsHolder.ResolutionAttrTypeGuid, false);
    if (attributeType3 != null)
      SignsHolder.ResolutionAttrTypeID = attributeType3.AttributeID;
    IDBAttributeType attributeType4 = session.GetAttributeType(SignsHolder.SignUpIOAttrTypeGuid, false);
    if (attributeType4 != null)
      SignsHolder.SignUpIOAttrTypeID = attributeType4.AttributeID;
    SignsHolder.SurnameForSignAttrTypeID = session.GetAttributeType(SignsHolder.SurnameForSignAttrTypeGuid).AttributeID;
    SignsHolder.SignsCommonParametersInit(session);
    SignsHolder.SignsOutputParametersInit(session);
    SignsHolder.SignsUserParametersInit(session);
  }

  /// <summary>Инициализация общих параметров.</summary>
  /// <param name="session">Пользовательская сессия.</param>
  public static void SignsCommonParametersInit(IUserSession session)
  {
    SignsHolder.DoRevocation = session.Configurations.ReadBool("SIGNS", "CERTIFICATES", "ONLINE_REVOCATION", false, DBConfigMode.GlobalOnly);
    SignsHolder.RevocationMode = session.Configurations.ReadBool("SIGNS", "CERTIFICATES", "ONLINE_MODE_REVOCATION", true, DBConfigMode.GlobalOnly) ? X509RevocationMode.Online : X509RevocationMode.Offline;
    SignsHolder.SignsDeveloperMode = session.Configurations.ReadBool("SIGNS", "CERTIFICATES", "SIGNS_DEVELOPER_MODE", false, DBConfigMode.GlobalOnly);
    SignsHolder.CertificateSigningOnlyMode = true;
    SignsHolder.CopySignsToVersionMode = session.Configurations.ReadBool("SIGNS", "CERTIFICATES", "COPY_SIGNS_TO_VERSION", false, DBConfigMode.GlobalOnly);
    SignsHolder.СheckExistingCopyActualityMode = session.Configurations.ReadBool("SIGNS", "CERTIFICATES", "CHECK_COPY_ACTUALITY", false, DBConfigMode.GlobalOnly);
    SignsHolder.TextForActualSimpleSign = session.Configurations.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_ACTUAL_SIMPLE_SIGN", "<Подп.>", DBConfigMode.GlobalOnly);
    SignsHolder.TextForNonActualSimpleSign = session.Configurations.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_NON_ACTUAL_SIMPLE_SIGN", "???", DBConfigMode.GlobalOnly);
    SignsHolder.QualifiedSignDisplayMode = (SignsHolder.SignDisplayMode) session.Configurations.ReadInteger("SIGNS", "SIGNDISPLAYING", "QUALIFIED_SIGN_DISPLAY_MODE", 0L, DBConfigMode.GlobalOnly);
    SignsHolder.TextForActualQualifiedSign = session.Configurations.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_ACTUAL_QUALIFIED_SIGN", "<Подп.>", DBConfigMode.GlobalOnly);
    SignsHolder.QualifiedSignKeyLastSymbolsNumber = (uint) session.Configurations.ReadInteger("SIGNS", "SIGNDISPLAYING", "SIGN_KEY_LAST_SYMBOLS_NUMBER", 20L, DBConfigMode.GlobalOnly);
    SignsHolder.TextForNonActualQualifiedSign = session.Configurations.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_NON_ACTUAL_QUAL_SIGN", "???", DBConfigMode.GlobalOnly);
    SignsHolder.CheckActualSignMadeBySameUser = session.Configurations.ReadBool("SIGNS", "CERTIFICATES", "CHECK_ACTUAL_SIGN_BY_SAME_USER", false, DBConfigMode.GlobalOnly);
  }

  public static void SignsUserParametersInit(IUserSession session)
  {
    SignsHolder.ConfirmSingleSigning = session.Configurations.ReadBool("SIGNS", "INTERFACE", "USER_CONFIRM_SINGLE_SIGNING", true, DBConfigMode.UserOnly);
  }

  /// <summary>Инициализация параметров вывода подписей.</summary>
  /// <param name="session">Пользовательская сессия</param>
  public static void SignsOutputParametersInit(IUserSession session)
  {
    SignsHolder.SignOutputEnabled = session.Configurations.ReadBool("SIGNS", "OUTPUT_PARAMS", "SIGN_OUTPUT_ENABLED", false, DBConfigMode.GlobalOnly);
    SignsHolder.SignOutputEnabledDevelop = session.Configurations.ReadBool("SIGNS", "OUTPUT_PARAMS", "SIGN_OUTPUT_ENABLED_DEVELOP", true, DBConfigMode.GlobalOnly);
    SignsHolder.SignSurnameParam = session.Configurations.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_SURNAME_PARAM", "[Графа для подписи]", DBConfigMode.GlobalOnly);
    SignsHolder.SignValueParam = session.Configurations.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_VALUE_PARAM", "[Графа для подписи]_ЭЦП", DBConfigMode.GlobalOnly);
    SignsHolder.SignDateParam = session.Configurations.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_DATE_PARAM", "[Графа для подписи]_Дата", DBConfigMode.GlobalOnly);
    SignsHolder.SignRankParam = session.Configurations.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_RANK_PARAM", "[Графа для подписи]_Должность", DBConfigMode.GlobalOnly);
    SignsHolder.SignGraphNameParam = session.Configurations.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_GRAPH_NAME_PARAM", "[Графа для подписи]_Графа", DBConfigMode.GlobalOnly);
    SignsHolder.CheckSumType = (ChecksumAlgorithm) session.Configurations.ReadInteger("SIGNS", "OUTPUT_PARAMS", "SIGN_CHECKSUMTYPE_PARAM", 0L, DBConfigMode.GlobalOnly);
    SignsHolder.CheckSumAttribute = session.Configurations.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_CHECKSUM_PARAM", "CRC32", DBConfigMode.GlobalOnly);
    SignsHolder.SignDateFormatParam = session.Configurations.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_DATE_FORMAT", "dd.MM.yyyy", DBConfigMode.GlobalOnly);
  }

  /// <summary>Способ отображения квалифицированных ЭП</summary>
  [TypeConverter(typeof (EnumCustomConverter))]
  [Serializable]
  public enum SignDisplayMode
  {
    /// <summary>Текст</summary>
    [Description("Текст")] Text,
    /// <summary>Символы ключа</summary>
    [Description("Символы ключа")] KeySymbols,
  }
}
