// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.ReportBackgroundBaseTask
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Client.Core;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Reports;

/// <summary>Класс-родитель для работы в потоке</summary>
/// <remarks>Для совместимости со старым кодом only</remarks>
public abstract class ReportBackgroundBaseTask : CustomThreadBackgroundTask
{
  /// <summary>Флаг наличия ошибок / сообщений</summary>
  private bool _hasErrors;
  /// <summary>Категория процесса (для вывода в IOutputView)</summary>
  protected string _category = string.Empty;

  /// <summary>Инициализация параметров класса</summary>
  protected override void InitializeData()
  {
    base.InitializeData();
    this._canStop = true;
    this._canPause = true;
    this._canResume = true;
    this._canTerminate = true;
    this._state = BackgroundTaskState.Running;
    DockManager service = ServiceUtils.GetService<DockManager>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    this._mainThreadControl = (Control) service.DocumentContainer ?? (Control) service.ActiveDockControl;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="e"></param>
  protected override void DoThrowException(Exception e)
  {
    ExceptionHelper.ExceptionService.ShowException(e);
    IOutputView service = ServiceUtils.GetService<IOutputView>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    service.WriteString(this._category, string.Format(LocalizationHolder.rm.GetString("Reports_4"), (object) this._name));
    service.WriteString(this._category, string.Format(LocalizationHolder.rm.GetString("Reports_5"), (object) e.Message));
    service.WriteString(this._category, e.StackTrace ?? string.Empty);
    service.Activate(this._category);
    service.ShowView();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="text"></param>
  protected override void DoWriteOutput(string text)
  {
    IOutputView service = ServiceUtils.GetService<IOutputView>((object) ApplicationServices.Container, true);
    if (service == null)
      return;
    service.WriteString(this._category, text);
    service.Activate(this._category);
    if (this._hasErrors)
      return;
    service.ShowView();
    this._hasErrors = true;
  }
}
