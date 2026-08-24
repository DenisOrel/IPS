// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.MainConfiguration
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Client.Core;
using Intermech.PropertyEditors.ChangeHighlighting;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.ImpExp.Interface;

[DefaultProperty("DataMigrate")]
public class MainConfiguration : ICloneable
{
  private int _packetSize;
  private readonly int _defaultPacketSize = 1000;
  private int _commandTimeout;
  private readonly int _defaultCommandTimeout = 30;
  private const string _commonGroup = "Общие";
  private const string _foldersGroup = "Папки";

  public MainConfiguration()
  {
  }

  public MainConfiguration(
    ChangeTrackingListAdapter<PluginItem> plugins,
    string cacheTempFolder,
    string settingsTempFolder,
    string unknownMeasure,
    bool dataMigrate,
    bool dropIndexes,
    int packetSize,
    int commandTimeout,
    bool plPumpingResume)
  {
    this.Plugins = plugins;
    this.CacheTempFolder = cacheTempFolder;
    this.SettingsTempFolder = settingsTempFolder;
    this.UnknownMeasure = unknownMeasure;
    this.DataMigrate = dataMigrate;
    this.DropIndexes = dropIndexes;
    this._packetSize = packetSize;
    this._commandTimeout = commandTimeout;
    this.PLPumpingResume = plPumpingResume;
  }

  /// <summary>Таблица атрибутов детали</summary>
  [Category("Общие")]
  [DisplayName("Загружаемые модули")]
  [Description("Это свойство позволяет задать список загружаемых адаптеров баз данных и плагинов с задачами по миграции данных.")]
  [Editor(typeof (PluginItemUIEditor), typeof (UITypeEditor))]
  public ChangeTrackingListAdapter<PluginItem> Plugins { get; set; }

  [Category("Папки")]
  [DisplayName("Папка файлов кэша")]
  [Description("Относительный путь к папке с файлами кэша.")]
  public string CacheTempFolder { get; set; }

  [Category("Папки")]
  [DisplayName("Папка файлов настроек")]
  [Description("Относительный путь к папке с файлами настроек.")]
  public string SettingsTempFolder { get; set; }

  [Category("Общие")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DisplayName("Миграция данных")]
  [Description("Миграция данных. В данном режиме будут импортированы все данные ( включая изделия, документы, расцеховки, заготовки, техпроцессы и т.д.). В противном случае будут импортированы только метаданные (справочники Imbase, информация по типам объектов и атрибутов, формы редактирования, экспертная система)")]
  public bool DataMigrate { get; set; }

  [Category("Общие")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DisplayName("Удаление индексов")]
  [Description("Флаг удаления индексов, при установке в значении ДА в начале процесса миграции данных в таблицах базы назначения будут удалены некоторые индексы для повышения скорости вставки данных. В конце миграции индексы будут восстановлены. Этот режим рекомендуется использовать только при очень больших объемах переносимой информации.")]
  public bool DropIndexes { get; set; }

  [Category("Общие")]
  [DisplayName("Ед.измерения по умолчанию")]
  [Description("Единица измерения подставляемая в случае невозможности определения из данных Imbase")]
  public string UnknownMeasure { get; set; }

  [Category("Общие")]
  [DisplayName("Размер пакета")]
  [Description("Количество объектов или связей импортируемых одним пакетом. Пакет импортируется в одной транзакции!")]
  public int PacketSize
  {
    get => this._packetSize <= 0 ? this._defaultPacketSize : this._packetSize;
    set => this._packetSize = value;
  }

  [Category("Общие")]
  [DisplayName("Таймаут запроса в исходную БД")]
  [Description("Таймаут запроса в исходную БД")]
  public int CommandTimeout
  {
    get => this._commandTimeout <= 0 ? this._defaultCommandTimeout : this._commandTimeout;
    set => this._commandTimeout = value;
  }

  [Category("Общие")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DisplayName("Режим докачки производственных ведомостей")]
  [Description("Режим докачки производственных ведомостей")]
  public bool PLPumpingResume { get; set; }

  public MainConfiguration Clone()
  {
    return new MainConfiguration(this.Plugins.Clone(), this.CacheTempFolder, this.SettingsTempFolder, this.UnknownMeasure, this.DataMigrate, this.DropIndexes, this.PacketSize, this.CommandTimeout, this.PLPumpingResume);
  }

  object ICloneable.Clone() => (object) this.Clone();
}
