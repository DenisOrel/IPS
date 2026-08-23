// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignsOutputProperties
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Checksums;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Настройки параметров вывода подписи</summary>
internal class SignsOutputProperties
{
  /// <summary>Разрешает и запрещает использование вывода подписей.</summary>
  private bool _SignOutputEnabledParam;
  /// <summary>Разрешает и запрещает использование вывода подписей.</summary>
  private bool _SignOutputEnabledDevelopParam = true;
  /// <summary>
  /// Имя параметра, в который будет передаваться фамилия из ЭП.
  /// </summary>
  private string _SignSurnameParam = LocalizationHolder.rm.GetString("SignGraph");
  /// <summary>
  /// Имя параметра, в который будет передаваться значение ЭП в соответствии с настройкой вывода ЭП
  /// </summary>
  private string _SignValueParam = LocalizationHolder.rm.GetString(nameof (SignValueParam));
  /// <summary>
  /// Имя параметра, в который будет передаваться должность, в которой подписана ЭП;
  /// </summary>
  private string _SignRankParam = LocalizationHolder.rm.GetString(nameof (SignRankParam));
  /// <summary>
  /// Имя параметра, в который будет передаваться наименование графы для подписи;
  /// </summary>
  private string _SignGraphNameParam = LocalizationHolder.rm.GetString("SignNameParam");
  /// <summary>
  /// Имя параметра, в который будет передаваться дата из ЭП;
  /// </summary>
  private string _SignDateParam = LocalizationHolder.rm.GetString(nameof (SignDateParam));
  /// <summary>
  /// Имя параметра, в который будет передаваться формат даты для вывода
  /// </summary>
  private string _SignDateFormatParam = LocalizationHolder.rm.GetString("SignDateFormatParam");
  /// <summary>Способ получения контрольной суммы;</summary>
  private ChecksumAlgorithm _CheckSumType;
  /// <summary>Наименование свойства, в которое передается сумма;</summary>
  private string _CheckSumAttribute = "CRC32";
  internal bool _inited;

  internal void ApplyUpdates()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBConfigurations service = ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations;
      service.WriteBool("SIGNS", "OUTPUT_PARAMS", "SIGN_OUTPUT_ENABLED_DEVELOP", this._SignOutputEnabledDevelopParam, 0L);
      service.WriteBool("SIGNS", "OUTPUT_PARAMS", "SIGN_OUTPUT_ENABLED", this._SignOutputEnabledParam, 0L);
      service.WriteString("SIGNS", "OUTPUT_PARAMS", "SIGN_SURNAME_PARAM", this._SignSurnameParam, 0L);
      service.WriteString("SIGNS", "OUTPUT_PARAMS", "SIGN_VALUE_PARAM", this._SignValueParam, 0L);
      service.WriteString("SIGNS", "OUTPUT_PARAMS", "SIGN_DATE_PARAM", this._SignDateParam, 0L);
      service.WriteString("SIGNS", "OUTPUT_PARAMS", "SIGN_GRAPH_NAME_PARAM", this._SignGraphNameParam, 0L);
      service.WriteString("SIGNS", "OUTPUT_PARAMS", "SIGN_RANK_PARAM", this._SignRankParam, 0L);
      service.WriteString("SIGNS", "OUTPUT_PARAMS", "SIGN_CHECKSUMTYPE_PARAM", ((int) this._CheckSumType).ToString(), 0L);
      service.WriteString("SIGNS", "OUTPUT_PARAMS", "SIGN_CHECKSUM_PARAM", this._CheckSumAttribute, 0L);
      service.WriteString("SIGNS", "OUTPUT_PARAMS", "SIGN_DATE_FORMAT", this._SignDateFormatParam, 0L);
      SignsHolder.SignsOutputParametersInit(sessionKeeper.Session);
      if (!(sessionKeeper.Session.GetCustomService(typeof (ISignsService)) is ISignsService customService))
        return;
      customService.SaveOutputParams(sessionKeeper.Session.SessionGUID);
    }
  }

  internal void LoadCurrentValues()
  {
    IDBConfigurations service = ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations;
    this._SignOutputEnabledDevelopParam = service.ReadBool("SIGNS", "OUTPUT_PARAMS", "SIGN_OUTPUT_ENABLED_DEVELOP", true, DBConfigMode.GlobalOnly);
    this._SignOutputEnabledParam = service.ReadBool("SIGNS", "OUTPUT_PARAMS", "SIGN_OUTPUT_ENABLED", false, DBConfigMode.GlobalOnly);
    this._SignSurnameParam = service.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_SURNAME_PARAM", "[Графа для подписи]", DBConfigMode.GlobalOnly);
    this._SignValueParam = service.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_VALUE_PARAM", "[Графа для подписи]_ЭЦП", DBConfigMode.GlobalOnly);
    this._SignDateParam = service.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_DATE_PARAM", "[Графа для подписи]_Дата", DBConfigMode.GlobalOnly);
    this._SignRankParam = service.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_RANK_PARAM", "[Графа для подписи]_Должность", DBConfigMode.GlobalOnly);
    this._SignGraphNameParam = service.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_GRAPH_NAME_PARAM", "[Графа для подписи]_Графа", DBConfigMode.GlobalOnly);
    this._CheckSumType = (ChecksumAlgorithm) service.ReadInteger("SIGNS", "OUTPUT_PARAMS", "SIGN_CHECKSUMTYPE_PARAM", 0L, DBConfigMode.GlobalOnly);
    this._CheckSumAttribute = service.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_CHECKSUM_PARAM", "CRC32", DBConfigMode.GlobalOnly);
    this._SignDateFormatParam = service.ReadString("SIGNS", "OUTPUT_PARAMS", "SIGN_DATE_FORMAT", "dd.MM.yyyy", DBConfigMode.GlobalOnly);
  }

  private void CheckInited()
  {
    if (this._inited)
      return;
    this.LoadCurrentValues();
    this._inited = true;
  }

  [CustomDescription("SignOutputEnabledParamDescription")]
  [CustomDisplayName("SignOutputEnabledParamCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  public bool SignOutputEnabledParam
  {
    get
    {
      this.CheckInited();
      return this._SignOutputEnabledParam;
    }
    set => this._SignOutputEnabledParam = value;
  }

  [CustomDescription("SignOutputEnabledDevelopParamDescription")]
  [CustomDisplayName("SignOutputEnabledDevelopParamCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  public bool SignOutputEnabledDevelopParam
  {
    get
    {
      this.CheckInited();
      return this._SignOutputEnabledDevelopParam;
    }
    set => this._SignOutputEnabledDevelopParam = value;
  }

  [CustomDescription("SignSurnameParamDescription")]
  [CustomDisplayName("SignSurnameParamCaption")]
  [TypeConverter(typeof (string))]
  public string SignSurnameParam
  {
    get
    {
      this.CheckInited();
      return this._SignSurnameParam;
    }
    set => this._SignSurnameParam = value;
  }

  [CustomDescription("SignValueParamDescription")]
  [CustomDisplayName("SignValueParamCaption")]
  [TypeConverter(typeof (string))]
  public string SignValueParam
  {
    get
    {
      this.CheckInited();
      return this._SignValueParam;
    }
    set => this._SignValueParam = value;
  }

  [CustomDescription("SignDateParamDescription")]
  [CustomDisplayName("SignDateParamCaption")]
  [TypeConverter(typeof (string))]
  public string SignDateParam
  {
    get
    {
      this.CheckInited();
      return this._SignDateParam;
    }
    set => this._SignDateParam = value;
  }

  [CustomDescription("SignRankParamDescription")]
  [CustomDisplayName("SignRankParamCaption")]
  [TypeConverter(typeof (string))]
  public string SignRankParam
  {
    get
    {
      this.CheckInited();
      return this._SignRankParam;
    }
    set => this._SignRankParam = value;
  }

  [CustomDescription("SignGraphNameParamDescription")]
  [CustomDisplayName("SignGraphNameParamCaption")]
  [TypeConverter(typeof (string))]
  public string SignGraphNameParam
  {
    get
    {
      this.CheckInited();
      return this._SignGraphNameParam;
    }
    set => this._SignGraphNameParam = value;
  }

  [CustomDescription("SignDateOutputFormatDescription")]
  [CustomDisplayName("SignDateOutputFormatCaption")]
  [TypeConverter(typeof (string))]
  public string SignDateOutputFormat
  {
    get
    {
      this.CheckInited();
      return this._SignDateFormatParam;
    }
    set => this._SignDateFormatParam = value;
  }

  [CustomDescription("CheckSumAttributeDescription")]
  [CustomDisplayName("CheckSumAttributeCaption")]
  [TypeConverter(typeof (string))]
  public string CheckSumAttribute
  {
    get
    {
      this.CheckInited();
      return this._CheckSumAttribute;
    }
    set => this._CheckSumAttribute = value;
  }

  [CustomDescription("CheckSumTypeDescription")]
  [CustomDisplayName("CheckSumTypeCaption")]
  [TypeConverter(typeof (string))]
  public ChecksumAlgorithm CheckSumType
  {
    get
    {
      this.CheckInited();
      return this._CheckSumType;
    }
    set => this._CheckSumType = value;
  }
}
