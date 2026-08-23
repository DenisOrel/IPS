// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsProperties
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Общие настройки ЭЦП</summary>
internal class SignsProperties
{
  /// <summary>Подписи, совместимые с Search</summary>
  private bool _CompatibleSigns;
  /// <summary>
  /// Использовать для подписи только сертификаты, не использовать открытые ключи IPS  // OBSOLETE! сейчас всегда DEFAULT = TRUE
  /// </summary>
  private bool _CertificateSigningOnlyMode = true;
  /// <summary>Использовать для проверки серверы отзыва сертификатов</summary>
  private bool _DoRevocationMode;
  /// <summary>При _DoRevocationMode проверять Online или offline</summary>
  private bool _OnlineModeRevocationMode = true;
  /// <summary>Режим разработчика для подписей (подробные логи итп)</summary>
  private bool _SignsDeveloperMode;
  /// <summary>Копировать подписи при создании версии объекта</summary>
  private bool _CopySignsToVersionMode;
  /// <summary>
  /// Проверять наличие актуальной подписи в выбранной графе при подписании
  /// </summary>
  private bool _CheckExistingCopyActualityMode;
  /// <summary>
  /// Может ли пользователь переподписать свою же актуальную подпись
  /// </summary>
  private bool _CheckActualSignMadeBySameUser;
  /// <summary>Текст для отображения актуальных простых ЭП;</summary>
  private string _TextForActualSimpleSign = LocalizationHolder.rm.GetString("Signs_TextForActualSimpleSign");
  /// <summary>Текст для отображения неактуальных простых ЭП</summary>
  private string _TextForNonActualSimpleSign = LocalizationHolder.rm.GetString("Signs_TextForNonActualSimpleSign");
  /// <summary>Способ отображения квалифицированнных ЭП</summary>
  private SignsHolder.SignDisplayMode _QualifiedSignDisplayMode;
  /// <summary>Текст для отображения актуальных квалифицированных ЭП</summary>
  private string _TextForActualQualifiedSign = LocalizationHolder.rm.GetString("Signs_TextForActualQualifiedSign");
  /// <summary>
  /// Количество последних символов ключа квалифицированной ЭП
  /// </summary>
  private uint _QualifiedSignKeyLastSymbolsNumber = 20;
  /// <summary>
  /// Параметр "Текст для отображения неактуальных квалифицированных ЭП"
  /// </summary>
  private string _TextForNonActualQualifiedSign = LocalizationHolder.rm.GetString("Signs_TextForNonActualQualifiedSign");
  internal bool _inited;

  internal void ApplyUpdates()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBConfigurations service = ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations;
      service.WriteBool("KERNEL", "SIGNS", "COMPATIBLE", this._CompatibleSigns, 0L);
      service.WriteBool("SIGNS", "CERTIFICATES", "ONLINE_REVOCATION", this._DoRevocationMode, 0L);
      service.WriteBool("SIGNS", "CERTIFICATES", "ONLINE_MODE_REVOCATION", this._OnlineModeRevocationMode, 0L);
      service.WriteBool("SIGNS", "CERTIFICATES", "SIGNS_DEVELOPER_MODE", this._SignsDeveloperMode, 0L);
      service.WriteBool("SIGNS", "CERTIFICATES", "CERTIFICATE_SIGNING_ONLY", true, 0L);
      service.WriteBool("SIGNS", "CERTIFICATES", "COPY_SIGNS_TO_VERSION", this._CopySignsToVersionMode, 0L);
      service.WriteBool("SIGNS", "CERTIFICATES", "CHECK_COPY_ACTUALITY", this._CheckExistingCopyActualityMode, 0L);
      service.WriteString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_ACTUAL_SIMPLE_SIGN", this._TextForActualSimpleSign, 0L);
      service.WriteString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_NON_ACTUAL_SIMPLE_SIGN", this._TextForNonActualSimpleSign, 0L);
      service.WriteInteger("SIGNS", "SIGNDISPLAYING", "QUALIFIED_SIGN_DISPLAY_MODE", (long) this._QualifiedSignDisplayMode, 0L);
      service.WriteString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_ACTUAL_QUALIFIED_SIGN", this._TextForActualQualifiedSign, 0L);
      service.WriteInteger("SIGNS", "SIGNDISPLAYING", "SIGN_KEY_LAST_SYMBOLS_NUMBER", (long) this._QualifiedSignKeyLastSymbolsNumber, 0L);
      service.WriteString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_NON_ACTUAL_QUAL_SIGN", this._TextForNonActualQualifiedSign, 0L);
      service.WriteBool("SIGNS", "CERTIFICATES", "CHECK_ACTUAL_SIGN_BY_SAME_USER", this._CheckActualSignMadeBySameUser, 0L);
      SignsHolder.SignsCommonParametersInit(sessionKeeper.Session);
      if (!(sessionKeeper.Session.GetCustomService(typeof (ISignsService)) is ISignsService customService))
        return;
      customService.SaveSignsParams(sessionKeeper.Session.SessionGUID);
    }
  }

  internal void LoadCurrentValues()
  {
    IDBConfigurations service = ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations;
    this._CompatibleSigns = service.ReadBool("KERNEL", "SIGNS", "COMPATIBLE", false, DBConfigMode.GlobalOnly);
    this._DoRevocationMode = service.ReadBool("SIGNS", "CERTIFICATES", "ONLINE_REVOCATION", false, DBConfigMode.GlobalOnly);
    this._OnlineModeRevocationMode = service.ReadBool("SIGNS", "CERTIFICATES", "ONLINE_MODE_REVOCATION", true, DBConfigMode.GlobalOnly);
    this._SignsDeveloperMode = service.ReadBool("SIGNS", "CERTIFICATES", "SIGNS_DEVELOPER_MODE", false, DBConfigMode.GlobalOnly);
    this._CertificateSigningOnlyMode = true;
    this._CopySignsToVersionMode = service.ReadBool("SIGNS", "CERTIFICATES", "COPY_SIGNS_TO_VERSION", false, DBConfigMode.GlobalOnly);
    this._CheckExistingCopyActualityMode = service.ReadBool("SIGNS", "CERTIFICATES", "CHECK_COPY_ACTUALITY", false, DBConfigMode.GlobalOnly);
    this._TextForActualSimpleSign = service.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_ACTUAL_SIMPLE_SIGN", "<Подп.>", DBConfigMode.GlobalOnly);
    this._TextForNonActualSimpleSign = service.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_NON_ACTUAL_SIMPLE_SIGN", "???", DBConfigMode.GlobalOnly);
    this._QualifiedSignDisplayMode = (SignsHolder.SignDisplayMode) service.ReadInteger("SIGNS", "SIGNDISPLAYING", "QUALIFIED_SIGN_DISPLAY_MODE", 0L, DBConfigMode.GlobalOnly);
    this._TextForActualQualifiedSign = service.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_ACTUAL_QUALIFIED_SIGN", "<Подп.>", DBConfigMode.GlobalOnly);
    this._QualifiedSignKeyLastSymbolsNumber = (uint) service.ReadInteger("SIGNS", "SIGNDISPLAYING", "SIGN_KEY_LAST_SYMBOLS_NUMBER", 20L, DBConfigMode.GlobalOnly);
    this._TextForNonActualQualifiedSign = service.ReadString("SIGNS", "SIGNDISPLAYING", "TEXT_FOR_NON_ACTUAL_QUAL_SIGN", "???", DBConfigMode.GlobalOnly);
    this._CheckActualSignMadeBySameUser = service.ReadBool("SIGNS", "CERTIFICATES", "CHECK_ACTUAL_SIGN_BY_SAME_USER", false, DBConfigMode.GlobalOnly);
  }

  private void CheckInited()
  {
    if (this._inited)
      return;
    this.LoadCurrentValues();
    this._inited = true;
  }

  [CustomDescription("CompatibleSignsDescription")]
  [CustomDisplayName("CompatibleSignsCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DefaultValue(false)]
  public bool CompatibleSigns
  {
    get
    {
      this.CheckInited();
      return this._CompatibleSigns;
    }
    set => this._CompatibleSigns = value;
  }

  [CustomDescription("DoRevocationDescription")]
  [CustomDisplayName("DoRevocationCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DefaultValue(false)]
  public bool DoRevocationMode
  {
    get
    {
      this.CheckInited();
      return this._DoRevocationMode;
    }
    set => this._DoRevocationMode = value;
  }

  [CustomDescription("OnlineRevocationModeDescription")]
  [CustomDisplayName("OnlineRevocationModeCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DefaultValue(true)]
  public bool OnlineRevocationMode
  {
    get
    {
      this.CheckInited();
      return this._OnlineModeRevocationMode;
    }
    set => this._OnlineModeRevocationMode = value;
  }

  [CustomDescription("SignsDeveloperModeDescription")]
  [CustomDisplayName("SignsDeveloperModeCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DefaultValue(false)]
  public bool SignsDeveloperMode
  {
    get
    {
      this.CheckInited();
      return this._SignsDeveloperMode;
    }
    set => this._SignsDeveloperMode = value;
  }

  /// <summary>// OBSOLETE! сейчас всегда DEFAULT = TRUE</summary>
  [CustomDescription("CertificateSigningOnlyDescription")]
  [CustomDisplayName("CertificateSigningOnlyCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DefaultValue(true)]
  [Browsable(false)]
  public bool CertificateSigningOnlyMode
  {
    get
    {
      this.CheckInited();
      return this._CertificateSigningOnlyMode;
    }
    set => this._CertificateSigningOnlyMode = value;
  }

  [CustomDescription("CopySignsToVersionDescription")]
  [CustomDisplayName("CopySignsToVersionCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DefaultValue(false)]
  public bool CopySignsToVersionMode
  {
    get
    {
      this.CheckInited();
      return this._CopySignsToVersionMode;
    }
    set => this._CopySignsToVersionMode = value;
  }

  [CustomDescription("CheckExistingCopyActualityDescription")]
  [CustomDisplayName("CheckExistingCopyActualityCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DefaultValue(false)]
  public bool CheckExistingCopyActualityMode
  {
    get
    {
      this.CheckInited();
      return this._CheckExistingCopyActualityMode;
    }
    set => this._CheckExistingCopyActualityMode = value;
  }

  [CustomDescription("CheckActualSignMadeBySameUserDescription")]
  [CustomDisplayName("CheckActualSignMadeBySameUserCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DefaultValue(false)]
  public bool CheckActualSignMadeMadeBySameUser
  {
    get
    {
      this.CheckInited();
      return this._CheckActualSignMadeBySameUser;
    }
    set => this._CheckActualSignMadeBySameUser = value;
  }

  [CustomDescription("TextForActualSimpleSignDescription")]
  [CustomDisplayName("TextForActualSimpleSignCaption")]
  [TypeConverter(typeof (string))]
  [DefaultValue("<Подп.>")]
  public string TextForActualSimpleSign
  {
    get
    {
      this.CheckInited();
      return this._TextForActualSimpleSign;
    }
    set => this._TextForActualSimpleSign = value;
  }

  [CustomDescription("TextForNonActualSimpleSignDescription")]
  [CustomDisplayName("TextForNonActualSimpleSignCaption")]
  [TypeConverter(typeof (string))]
  [DefaultValue("???")]
  public string TextForNonActualSimpleSign
  {
    get
    {
      this.CheckInited();
      return this._TextForNonActualSimpleSign;
    }
    set => this._TextForNonActualSimpleSign = value;
  }

  [CustomDescription("QualifiedSignDisplayModeDescription")]
  [CustomDisplayName("QualifiedSignDisplayModeCaption")]
  [DefaultValue(0)]
  public SignsHolder.SignDisplayMode QualifiedSignDisplayMode
  {
    get
    {
      this.CheckInited();
      return this._QualifiedSignDisplayMode;
    }
    set => this._QualifiedSignDisplayMode = value;
  }

  [CustomDescription("TextForActualQualifiedSignDescription")]
  [CustomDisplayName("TextForActualQualifiedSignCaption")]
  [TypeConverter(typeof (string))]
  [DefaultValue("<Подп.>")]
  public string TextForActualQualifiedSign
  {
    get
    {
      this.CheckInited();
      return this._TextForActualQualifiedSign;
    }
    set => this._TextForActualQualifiedSign = value;
  }

  [CustomDescription("TextForNonActualQualifiedSignDescription")]
  [CustomDisplayName("TextForNonActualQualifiedSignCaption")]
  [TypeConverter(typeof (string))]
  [DefaultValue("???")]
  public string TextForNonActualQualifiedSign
  {
    get
    {
      this.CheckInited();
      return this._TextForNonActualQualifiedSign;
    }
    set => this._TextForNonActualQualifiedSign = value;
  }

  [CustomDescription("QualifiedSignKeyLastSymbolsNumberDescription")]
  [CustomDisplayName("QualifiedSignKeyLastSymbolsNumberCaption")]
  [TypeConverter(typeof (long))]
  [DefaultValue(0)]
  public uint QualifiedSignKeyLastSymbolsNumber
  {
    get
    {
      this.CheckInited();
      return this._QualifiedSignKeyLastSymbolsNumber;
    }
    set => this._QualifiedSignKeyLastSymbolsNumber = value;
  }
}
