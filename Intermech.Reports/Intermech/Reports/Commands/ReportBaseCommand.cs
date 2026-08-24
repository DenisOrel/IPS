// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Commands.ReportBaseCommand
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Reports;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Protection;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Reports.Commands;

/// <summary>Базовый класс для контекстных команд</summary>
public abstract class ReportBaseCommand
{
  /// <summary>
  /// 
  /// </summary>
  protected ISelectedItems _items;
  /// <summary>
  /// 
  /// </summary>
  protected IServiceProvider _viewServices;
  /// <summary>
  /// 
  /// </summary>
  protected object _additionalInfo;
  /// <summary>
  /// 
  /// </summary>
  protected IList<ObjInfoItem> _objInfoList;

  /// <summary>"Основной" метод выполнения команды</summary>
  private void DoExecute()
  {
    if (!this.DoExecute_ValidateParams() || !this.DoExecute_LoadObjInfo())
      return;
    this.DoExecute_Command();
  }

  /// <summary>Анализ параметров команды</summary>
  /// <returns></returns>
  protected virtual bool DoExecute_ValidateParams()
  {
    if (this._items == null || this._items.Count == 0 || this._viewServices == null)
      return false;
    IViewState service = ServiceUtils.GetService<IViewState>((object) this._viewServices, false);
    return ((service != null ? (long) service.ViewState : 0L) & 2L) == 0L && ReportUtils.GetSelectedItemsInfo(this._items, out this._objInfoList, true) && this._objInfoList.Count != 0;
  }

  /// <summary>Загрузка информации об объектах</summary>
  /// <returns></returns>
  protected virtual bool DoExecute_LoadObjInfo()
  {
    return ReportUtils.GetSelectedItemsInfo(this._items, out this._objInfoList, true) && this._objInfoList.Count != 0;
  }

  /// <summary>Выполнение команды</summary>
  protected abstract void DoExecute_Command();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  public void Execute(ISelectedItems items, IServiceProvider viewServices, object additionalInfo)
  {
    IProtectionKey service = ServiceUtils.GetService<IProtectionKey>((object) ApplicationServices.Container, true);
    int index = (Environment.TickCount & 15) * 2;
    byte[] numArray = ReportsProtection.Key[index];
    byte[] inArray = new byte[numArray.Length];
    int appId = ReportsProtection.appId;
    byte[] queryData = numArray;
    byte[] response = inArray;
    int num = service.Query(true, appId, queryData, response);
    if (!num.Equals(0) || !Convert.ToBase64String(inArray).Equals(Convert.ToBase64String(ReportsProtection.Key[index + 1])))
      throw new ProtectionException(string.Format(LocalizationHolder.rm.GetString("Reports_44"), (object) LocalizationHolder.rm.GetString("Reports_23"), (object) num));
    this._items = items;
    this._viewServices = viewServices;
    this._additionalInfo = additionalInfo;
    ServiceUtils.GetService<IReportsService>((object) ApplicationServices.Container, true);
    this.DoExecute();
  }
}
