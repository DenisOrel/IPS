// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Tasks.ReportsGenerateTask
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Interfaces.Expert;
using Intermech.Interfaces.Reports;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Reports.Tasks;

/// <summary>Класс генерации документов</summary>
/// <summary>
/// 
/// </summary>
/// <param name="taskParams"></param>
internal class ReportsGenerateTask(IReportTaskParams taskParams) : 
  ReportsBaseTask(taskParams),
  IReportsGenerateTask,
  IReportsBaseTask
{
  /// <summary>Проверка параметров задачи</summary>
  /// <returns></returns>
  protected override ExpertResult ValidateParams()
  {
    return this.Params.ObjectId == 0L || this.Params.ScriptObjId == 0L ? ExpertResult.NoCalcParms : ExpertResult.OK;
  }

  /// <summary>Метод выполнения</summary>
  /// <param name="changeLog"></param>
  /// <returns></returns>
  protected override ExpertResult ExecuteInternal(out List<ChangeInfo> changeLog)
  {
    if (!this.IsActive)
    {
      changeLog = (List<ChangeInfo>) null;
      return ExpertResult.WrongTaskId;
    }
    return this._expertSrv.GenerateComplect(new CompGenParms(this.ExpertTaskId, this.Params.ScriptObjId, this.Params.ObjectId)
    {
      DopComplects = this.Params.TaskMode.HasFlag((Enum) ReportTaskMode.AdditionalComplect)
    }, out changeLog);
  }
}
