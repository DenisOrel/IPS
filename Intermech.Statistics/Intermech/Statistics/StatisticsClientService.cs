// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.StatisticsClientService
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Calendars;
using Intermech.Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

#nullable disable
namespace Intermech.Statistics;

internal class StatisticsClientService : IStatisticsClientService
{
  public CommandSettings ReadStatisticObjectsCommandSettings(
    IUserSession session,
    long statisticObjectId)
  {
    IDBAttribute attributeByGuid = session.GetObject(statisticObjectId).GetAttributeByGuid(new Guid(StatisticsConst.CollectionSettings));
    CommandSettings commandSettings = (CommandSettings) null;
    using (MemoryStream aDestStream = new MemoryStream())
    {
      using (new StreamReader((Stream) aDestStream, Encoding.UTF8))
      {
        new BlobProcReader(attributeByGuid, 0, (Stream) aDestStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).ReadData(session);
        if (aDestStream.Length > 0L)
        {
          aDestStream.Position = 0L;
          commandSettings = (CommandSettings) new XmlSerializer(typeof (CommandSettings)).Deserialize((Stream) aDestStream);
          List<Period> periods = this.CreatePeriods(commandSettings, session);
          commandSettings.InitPeriods(periods);
        }
      }
    }
    return commandSettings;
  }

  private List<Period> CreatePeriods(CommandSettings commandSettings, IUserSession session)
  {
    IDBObject dbObject = session.GetObject(new Guid("cad01582-306c-11d8-b4e9-00304f19f545"), false);
    if (dbObject == null)
      throw new KernelException("В системе не найден стандартный календарь.");
    ICalendar calendar = (ApplicationServices.Container.GetService<ICalendarsService>() ?? throw new KernelException("Не найден сервис для работы с каландарями.")).GetCalendar(session, dbObject.ObjectID, false);
    if (calendar == null)
      throw new KernelException("Не удалось получить интерфейс для работы со стандартным календарем.");
    List<Period> periods = new List<Period>();
    DateTime fromDate = CollectPeriodsHelper.PreviousDateTime(commandSettings.StartDateTime, commandSettings.CollectPeriod);
    DateTime end = DateTime.MinValue;
    int number = 0;
    while (end < commandSettings.EndDateTime)
    {
      DateTime dateTime = CollectPeriodsHelper.NextDateTime(fromDate, commandSettings.CollectPeriod, number);
      end = CollectPeriodsHelper.NextDateTime(fromDate.AddSeconds(-1.0), commandSettings.CollectPeriod, number + 1);
      if (end >= commandSettings.EndDateTime)
        end = commandSettings.EndDateTime;
      if (commandSettings.IgnoreNotWorkingDays)
      {
        if (calendar.GetDayByDate(dateTime).DayType != DayType.Holiday)
          periods.Add(new Period(dateTime, end));
      }
      else
        periods.Add(new Period(dateTime, end));
      ++number;
    }
    return periods;
  }

  public void WriteStatisticObjectsCommandSettings(
    IUserSession session,
    long statisticObjectId,
    CommandSettings commandSettings)
  {
    IDBAttribute attributeByGuid = session.GetObject(statisticObjectId).GetAttributeByGuid(new Guid(StatisticsConst.CollectionSettings));
    commandSettings.ObjectID = statisticObjectId;
    using (MemoryStream aSourceStream = new MemoryStream())
    {
      using (StreamWriter streamWriter = new StreamWriter((Stream) aSourceStream, Encoding.UTF8))
      {
        new XmlSerializer(typeof (CommandSettings)).Serialize((TextWriter) streamWriter, (object) commandSettings);
        BlobInformation aBlobInformation = new BlobInformation(aSourceStream.Length, 0L, DateTime.Now, "statisticsSettings.xml", ArcMethods.ZLibPacked, string.Empty);
        new BlobProcWriter(attributeByGuid, 0, aBlobInformation, (Stream) aSourceStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
      }
    }
  }

  public CommandStatisticsTypesEnum ReadStatisticsCommandType(
    IUserSession session,
    long statisticObjectId)
  {
    return session.GetObject(statisticObjectId).GetAttributeByGuid(new Guid(StatisticsConst.CollectMethod)).Value.ToString().ToEnum<CommandStatisticsTypesEnum>(CommandStatisticsTypesEnum.None);
  }
}
