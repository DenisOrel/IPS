// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Tasks.ComplectGenerateBackgroundTask
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Interfaces.Client;
using Intermech.Interfaces.Reports;
using Intermech.Localization;

#nullable disable
namespace Intermech.Reports.Tasks;

/// <summary>Класс генерации комплекта в фоновом режиме</summary>
/// <summary>Конструктор</summary>
/// <param name="paramsObjects">Параметры запроса</param>
internal class ComplectGenerateBackgroundTask(IReportTaskParams taskParams) : 
  ComplectBackgroundBaseTask(taskParams),
  IReportBackgroundGenerateTask,
  IReportBackgroundTask,
  IBackgroundTask
{
  /// <summary>Создание задачи</summary>
  protected override void CreateReportTask()
  {
    this._reportsTask = (ReportsBaseTask) new ReportsGenerateTask(this.Params);
  }

  /// <summary>Инициализация параметров класса</summary>
  protected override void InitializeData()
  {
    base.InitializeData();
    this._category = LocalizationHolder.rm.GetString("Reports_1");
    this._name = string.Format(LocalizationHolder.rm.GetString("Reports_2"), (object) this._category, (object) this.ObjectInfo.Caption);
    this.Value = (object) 0;
    this.MinimumValue = 0;
  }
}
