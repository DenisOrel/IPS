// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.ReportsService
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Reports;
using Intermech.Reports.Tasks;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Reports;

/// <summary>Класс управления комплектами документов</summary>
internal sealed class ReportsService : MarshalByRefObject, IReportsService
{
  /// <summary>Получить интерфейс задачи генерации комплектов</summary>
  /// <param name="mode">Режим генерации</param>
  /// <param name="taskParams">Параметры задачи</param>
  /// <returns></returns>
  public IReportsBaseTask GetReportTask(ReportMode mode, IReportTaskParams taskParams)
  {
    switch (mode)
    {
      case ReportMode.Create:
        return (IReportsBaseTask) new ReportsGenerateTask(taskParams);
      case ReportMode.CreateVersion:
        return (IReportsBaseTask) new ReportsCreateVersionTask(taskParams);
      case ReportMode.Update:
        return (IReportsBaseTask) new ReportsUpdateTask(taskParams);
      case ReportMode.CreateOrUpdate:
        return taskParams.PackageObjId == 0L && !ReportsService.CheckExistingComplect(taskParams) ? (IReportsBaseTask) new ReportsGenerateTask(taskParams) : (IReportsBaseTask) new ReportsUpdateTask(taskParams);
      default:
        return (IReportsBaseTask) null;
    }
  }

  /// <summary>
  /// Получить интерфейс задачи генерации комплектов в фоновом потоке
  /// </summary>
  /// <param name="mode">Режим генерации</param>
  /// <param name="taskParams">Параметры задачи</param>
  /// <returns></returns>
  public IReportBackgroundTask GetReportBackgroundTask(
    ReportMode mode,
    IReportTaskParams taskParams)
  {
    switch (mode)
    {
      case ReportMode.Create:
        return (IReportBackgroundTask) new ComplectGenerateBackgroundTask(taskParams);
      case ReportMode.CreateVersion:
        return (IReportBackgroundTask) new ComplectCreateVersionBackgroundTask(taskParams);
      case ReportMode.Update:
        return (IReportBackgroundTask) new ComplectUpdateBackgroundTask(taskParams);
      case ReportMode.CreateOrUpdate:
        return taskParams.PackageObjId == 0L && !ReportsService.CheckExistingComplect(taskParams) ? (IReportBackgroundTask) new ComplectGenerateBackgroundTask(taskParams) : (IReportBackgroundTask) new ComplectUpdateBackgroundTask(taskParams);
      default:
        return (IReportBackgroundTask) null;
    }
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="taskParams"></param>
  /// <returns></returns>
  private static bool CheckExistingComplect(IReportTaskParams taskParams)
  {
    ObjInfoItem key = taskParams != null ? new ObjInfoItem(taskParams.ObjectId) : throw new ArgumentNullException(nameof (taskParams));
    IDictionary<ObjInfoItem, IList<ComplectGenerateCommand.ComplectObjInfo>> object2ComplectInfo;
    IList<ComplectGenerateCommand.ComplectObjInfo> complectObjInfoList;
    if (!ComplectGenerateCommand.CheckComplectInfo4Objects((IDictionary<ObjInfoItem, ObjInfoItem>) new Dictionary<ObjInfoItem, ObjInfoItem>()
    {
      {
        key,
        new ObjInfoItem(taskParams.ScriptObjId)
      }
    }, out object2ComplectInfo) || !object2ComplectInfo.TryGetValue(key, out complectObjInfoList) || complectObjInfoList.Count == 0)
      return false;
    taskParams.PackageObjId = complectObjInfoList[0].ObjectID;
    taskParams.ArchiveId = complectObjInfoList[0].ArchiveId;
    return true;
  }
}
