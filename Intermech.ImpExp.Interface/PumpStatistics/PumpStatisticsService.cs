// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PumpStatistics.PumpStatisticsService
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Interface.PumpStatistics;

/// <summary>
/// Сервис по управлению статистиками работы отдельных памперов
/// </summary>
public class PumpStatisticsService
{
  private readonly Dictionary<Guid, Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics> _statistics = new Dictionary<Guid, Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics>();
  /// <summary>
  /// Отдельная папка внутри директории с данными миграции для ведения статистики
  /// </summary>
  private const string PumpStatisticsFolder = "PumpStats";
  /// <summary>Расширение файла со статистикой отдельного пампера</summary>
  private const string PumpStatisticsFileExt = ".stat";

  /// <summary>Доступ к отдельной статистике по Guid пампера</summary>
  /// <param name="pumpGuid">Уникальный идентификатор пампера</param>
  /// <param name="create">Создавать ли статистику пампера, если она отсутствует</param>
  /// <returns>Статистика пампера. null, если статистика отсутствует в списке и createIfNotExists = false</returns>
  private Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics GetPumpStatistics(
    Guid pumpGuid,
    bool create = true)
  {
    Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics pumpStatistics;
    if (this._statistics.TryGetValue(pumpGuid, out pumpStatistics) || !create)
      return pumpStatistics;
    pumpStatistics = new Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics(pumpGuid);
    this._statistics.Add(pumpGuid, pumpStatistics);
    return pumpStatistics;
  }

  /// <summary>
  /// Получить полный путь к директории для ведения статистики
  /// </summary>
  /// <returns></returns>
  private static string GetPumpStatisticsDirectory(bool createIfNotExists)
  {
    string path = Intermech.ImpExp.Interface.PathHelper.Normalize(Path.Combine(ApplicationServices.Container.GetService<IConfigurationService>().Configuration.SettingsTempFolder, "PumpStats"));
    if (!Directory.Exists(path) & createIfNotExists)
    {
      try
      {
        Directory.CreateDirectory(path);
      }
      catch (Exception ex)
      {
        ILogFile service = ApplicationServices.Container.GetService<ILogFile>();
        if (service != null)
        {
          service.WriteMessage(ex.Message);
          service.WriteMessage(ex.StackTrace);
        }
        else
          throw;
      }
    }
    return path;
  }

  /// <summary>
  /// Получить полный путь к файлу статистики работы отдельного пампера
  /// </summary>
  /// <param name="pumpGuid">Уникальный идентификатор пампера</param>
  /// <returns></returns>
  private static string GetPumpStatisticsFileName(Guid pumpGuid)
  {
    return Path.Combine(PumpStatisticsService.GetPumpStatisticsDirectory(true), pumpGuid.ToString().Replace("-", string.Empty) + ".stat");
  }

  /// <summary>Прочитать содержимое узла статистики</summary>
  /// <param name="reader">Узел статистики</param>
  /// <returns></returns>
  private async Task<Dictionary<string, string>> ReadInnerStat(XmlReader reader)
  {
    Dictionary<string, string> res = new Dictionary<string, string>();
    XmlReader statReader = reader.ReadSubtree();
    if (statReader.ReadToDescendant("stat_value"))
    {
      do
      {
        if (statReader.NodeType == XmlNodeType.Element && !(statReader.Name != "stat_value") && statReader.MoveToAttribute("name"))
        {
          string key = statReader.Value;
          if (statReader.MoveToAttribute("value"))
            res[key] = statReader.Value;
        }
      }
      while (await statReader.ReadAsync().ConfigureAwait(false));
    }
    statReader.Close();
    return res;
  }

  /// <summary>Доступ к статистике работы отдельного пампера</summary>
  /// <param name="pumpGuid">Уникальный идентификатор отдельного пампера. Если статистика не велась, то вернет новую</param>
  /// <param name="load">Загрузка статистики из файла</param>
  /// <returns>Статистика работы пампера</returns>
  public Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics this[Guid pumpGuid, bool create = true]
  {
    get => this.GetPumpStatistics(pumpGuid, create);
  }

  /// <summary>Очистить всю собранную статистику</summary>
  public void Clear()
  {
    try
    {
      string statisticsDirectory = PumpStatisticsService.GetPumpStatisticsDirectory(false);
      if (Directory.Exists(statisticsDirectory))
        Directory.Delete(statisticsDirectory, true);
    }
    catch (Exception ex)
    {
      ILogFile service = ApplicationServices.Container.GetService<ILogFile>();
      if (service != null)
      {
        service.WriteMessage(ex.Message);
        service.WriteMessage(ex.StackTrace);
      }
      else
        throw;
    }
    this._statistics.Clear();
  }

  /// <summary>
  /// Загрузить статистику работы отдельного пампера асинхронно
  /// </summary>
  /// <param name="pumpGuid">Уникальный идентификатор отдельного пампера</param>
  /// <returns>awaitable task с загружаемой статистикой</returns>
  public async Task<Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics> LoadAsync(
    Guid pumpGuid)
  {
    Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics stat = this.GetPumpStatistics(pumpGuid);
    string statisticsFileName = PumpStatisticsService.GetPumpStatisticsFileName(pumpGuid);
    if (!File.Exists(statisticsFileName))
      return stat;
    try
    {
      XmlReaderSettings settings = new XmlReaderSettings()
      {
        Async = true
      };
      using (StreamReader fs = File.OpenText(statisticsFileName))
      {
        using (XmlReader reader = XmlReader.Create((TextReader) fs, settings))
        {
          if (reader.ReadToFollowing("int_stat"))
            (await this.ReadInnerStat(reader).ConfigureAwait(false)).ToList<KeyValuePair<string, string>>().ForEach((Action<KeyValuePair<string, string>>) (statValue =>
            {
              int result;
              if (!int.TryParse(statValue.Value, out result))
                result = 0;
              stat.IntStat[statValue.Key] = result;
            }));
          if (reader.ReadToFollowing("str_stat"))
            (await this.ReadInnerStat(reader).ConfigureAwait(false)).ToList<KeyValuePair<string, string>>().ForEach((Action<KeyValuePair<string, string>>) (statValue => stat.StringStat[statValue.Key] = statValue.Value));
        }
      }
    }
    catch (Exception ex)
    {
      ILogFile service = ApplicationServices.Container.GetService<ILogFile>();
      if (service != null)
      {
        service.WriteMessage(ex.Message);
        service.WriteMessage(ex.StackTrace);
      }
      else
        throw;
    }
    return stat;
  }

  /// <summary>
  /// Сохранить статистику работы отдельного пампера асинхронно
  /// </summary>
  /// <param name="pumpGuid">Уникальный идентификатор отдельного пампера</param>
  /// <returns>awaitable task сохраняемой статистики</returns>
  public async Task SaveAsync(Guid pumpGuid)
  {
    Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics stat = this[pumpGuid, false];
    if (stat == null)
      return;
    string statisticsFileName = PumpStatisticsService.GetPumpStatisticsFileName(pumpGuid);
    XmlWriterSettings settings = new XmlWriterSettings()
    {
      Async = true
    };
    try
    {
      using (FileStream fs = File.OpenWrite(statisticsFileName))
      {
        using (XmlWriter writer = XmlWriter.Create((Stream) fs, settings))
        {
          ConfiguredTaskAwaitable configuredTaskAwaitable1 = writer.WriteStartElementAsync(string.Empty, "stat", string.Empty).ConfigureAwait(false);
          await configuredTaskAwaitable1;
          configuredTaskAwaitable1 = writer.WriteAttributeStringAsync(string.Empty, "guid", string.Empty, stat.PumpGuid.ToString()).ConfigureAwait(false);
          await configuredTaskAwaitable1;
          configuredTaskAwaitable1 = writer.WriteStartElementAsync(string.Empty, "int_stat", string.Empty).ConfigureAwait(false);
          await configuredTaskAwaitable1;
          foreach (KeyValuePair<string, int> keyValuePair in stat.IntStat)
          {
            KeyValuePair<string, int> statInfo = keyValuePair;
            configuredTaskAwaitable1 = writer.WriteStartElementAsync(string.Empty, "stat_value", string.Empty).ConfigureAwait(false);
            await configuredTaskAwaitable1;
            configuredTaskAwaitable1 = writer.WriteAttributeStringAsync(string.Empty, "name", string.Empty, statInfo.Key).ConfigureAwait(false);
            await configuredTaskAwaitable1;
            configuredTaskAwaitable1 = writer.WriteAttributeStringAsync(string.Empty, "value", string.Empty, statInfo.Value.ToString()).ConfigureAwait(false);
            await configuredTaskAwaitable1;
            configuredTaskAwaitable1 = writer.WriteEndElementAsync().ConfigureAwait(false);
            await configuredTaskAwaitable1;
            statInfo = new KeyValuePair<string, int>();
          }
          ConfiguredTaskAwaitable configuredTaskAwaitable2 = writer.WriteEndElementAsync().ConfigureAwait(false);
          await configuredTaskAwaitable2;
          configuredTaskAwaitable2 = writer.WriteStartElementAsync(string.Empty, "str_stat", string.Empty).ConfigureAwait(false);
          await configuredTaskAwaitable2;
          foreach (KeyValuePair<string, string> keyValuePair in stat.StringStat)
          {
            KeyValuePair<string, string> statInfo = keyValuePair;
            configuredTaskAwaitable2 = writer.WriteStartElementAsync(string.Empty, "stat_value", string.Empty).ConfigureAwait(false);
            await configuredTaskAwaitable2;
            configuredTaskAwaitable2 = writer.WriteAttributeStringAsync(string.Empty, "name", string.Empty, statInfo.Key).ConfigureAwait(false);
            await configuredTaskAwaitable2;
            configuredTaskAwaitable2 = writer.WriteAttributeStringAsync(string.Empty, "value", string.Empty, statInfo.Value).ConfigureAwait(false);
            await configuredTaskAwaitable2;
            configuredTaskAwaitable2 = writer.WriteEndElementAsync().ConfigureAwait(false);
            await configuredTaskAwaitable2;
            statInfo = new KeyValuePair<string, string>();
          }
          ConfiguredTaskAwaitable configuredTaskAwaitable3 = writer.WriteEndElementAsync().ConfigureAwait(false);
          await configuredTaskAwaitable3;
          configuredTaskAwaitable3 = writer.WriteEndElementAsync().ConfigureAwait(false);
          await configuredTaskAwaitable3;
          configuredTaskAwaitable3 = writer.FlushAsync().ConfigureAwait(false);
          await configuredTaskAwaitable3;
        }
      }
    }
    catch (Exception ex)
    {
      ILogFile service = ApplicationServices.Container.GetService<ILogFile>();
      if (service != null)
      {
        service.WriteMessage(ex.Message);
        service.WriteMessage(ex.StackTrace);
      }
      else
        throw;
    }
  }
}
