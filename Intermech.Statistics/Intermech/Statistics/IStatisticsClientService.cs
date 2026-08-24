// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.IStatisticsClientService
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Interfaces;
using Intermech.Statistics.Interfaces;

#nullable disable
namespace Intermech.Statistics;

internal interface IStatisticsClientService
{
  void WriteStatisticObjectsCommandSettings(
    IUserSession session,
    long statisticObjectId,
    CommandSettings commandSettings);

  CommandStatisticsTypesEnum ReadStatisticsCommandType(IUserSession session, long statisticObjectId);
}
