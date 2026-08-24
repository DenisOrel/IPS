// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Tasks.ComplectUpdateCommand
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using ImSSP;
using Intermech.DataFormats;
using Intermech.Expert;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Reports;
using Intermech.Kernel.Search;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Reports.Tasks;

/// <summary>
/// Выполнение команд меню обновить комплект / создать версию
/// </summary>
internal sealed class ComplectUpdateCommand : GenerateBaseCommand
{
  /// <summary>
  /// 
  /// </summary>
  private readonly ReportMode _mode;
  /// <summary>Информация о родительском объекте комплекта</summary>
  private ObjInfoItem _projObjInfo;
  /// <summary>
  /// 
  /// </summary>
  private ObjInfoItem _scriptObjInfo;
  /// <summary>
  /// 
  /// </summary>
  private ObjInfoItem _complectObjInfo;

  /// <summary>
  /// 
  /// </summary>
  private void DoExecute_StartTask()
  {
    if (ObjInfoItem.IsEmpty((ITypedInfoItem) this._projObjInfo) || ObjInfoItem.IsEmpty((ITypedInfoItem) this._scriptObjInfo))
      return;
    ComplectBackgroundBaseTask task = (ComplectBackgroundBaseTask) null;
    ReportTaskParams taskParams = new ReportTaskParams(this._projObjInfo.ObjectID, this._scriptObjInfo.ObjectID, this._complectObjInfo.ObjectID)
    {
      ArchiveId = this._archiveID,
      Attributes = this._repParams,
      TaskMode = this.TaskMode
    };
    Thread.Sleep(new Random().Next(100));
    switch (this._mode)
    {
      case ReportMode.CreateVersion:
        task = (ComplectBackgroundBaseTask) new ComplectCreateVersionBackgroundTask((IReportTaskParams) taskParams);
        break;
      case ReportMode.Update:
        task = (ComplectBackgroundBaseTask) new ComplectUpdateBackgroundTask((IReportTaskParams) taskParams);
        break;
    }
    if (task == null)
      return;
    task.Execute();
    ReportsClientCache.Services.BackgroundTaskView.AddTask((IBackgroundTask) task);
  }

  /// <summary>Загрузка информации о скрипте генерации комплекта</summary>
  /// <returns></returns>
  private bool DoExecute_LoadScriptInfo()
  {
    this._scriptObjInfo = (ObjInfoItem) null;
    long result1 = 0;
    long result2 = 0;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._complectObjInfo.ObjectID);
      IDBAttribute attributeByGuid1 = dbObject.GetAttributeByGuid(ReportsConsts.ScriptPackageAttrTypeGuid);
      if (attributeByGuid1 != null && attributeByGuid1.Value != null)
        long.TryParse(attributeByGuid1.Value.ToString(), out result1);
      IDBAttribute attributeByGuid2 = dbObject.GetAttributeByGuid(ReportsConsts.ArchiveAttributeTypeGuid);
      if (attributeByGuid2 != null)
      {
        if (attributeByGuid2.Value != null)
          long.TryParse(attributeByGuid2.Value.ToString(), out result2);
      }
    }
    if (result1 == 0L)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(ReportsConsts.ScriptPackageAttrTypeID);
      int num = (int) MessageBox.Show(string.Format(LocalizationHolder.rm.GetString(sc_17682.ssp_imclient_17683()), (object) attributeType.Name, (object) this._complectObjInfo.ObjectID), LocalizationHolder.rm.GetString("Reports_32"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }
    int num1 = -1;
    int objectType;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(result1, false);
      if (dbObject == null)
      {
        int num2 = (int) MessageBox.Show(string.Format(LocalizationHolder.rm.GetString(sc_17682.ssp_imclient_17684()), (object) MetaDataHelper.GetObjectName(ReportsConsts.ScriptPackageTypeID), (object) result1), LocalizationHolder.rm.GetString(sc_17682.ssp_imclient_17685()), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      objectType = dbObject.ObjectType;
      IDBAttribute attributeById = dbObject.GetAttributeByID(ReportsConsts.ObjTypeResultAttrTypeID);
      if (attributeById != null)
      {
        if (attributeById.Value != null)
        {
          if (attributeById.Value != DBNull.Value)
          {
            string asString = attributeById.AsString;
            if (GuidHelper.IsGuid(asString))
              num1 = MetaDataHelper.GetObjectTypeID(asString);
          }
        }
      }
    }
    this._scriptObjInfo = new ObjInfoItem(result1, objectType);
    if (num1 == -1)
    {
      int num3 = (int) MessageBox.Show(string.Format(LocalizationHolder.rm.GetString(sc_17682.ssp_imclient_17686()), (object) result1, (object) MetaDataHelper.GetAttributeTypeName(ReportsConsts.ObjTypeResultAttrTypeID)), LocalizationHolder.rm.GetString("Reports_32"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }
    List<int> parentsIdReverse = MetaDataHelper.GetObjectTypeParentsIDReverse(this._projObjInfo.ObjTypeID);
    if (!parentsIdReverse.Contains(this._projObjInfo.ObjTypeID))
      parentsIdReverse.Add(this._projObjInfo.ObjTypeID);
    if (!parentsIdReverse.Contains(num1))
    {
      string text;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(this._projObjInfo.ObjectID);
        text = string.Format(LocalizationHolder.rm.GetString(sc_17682.ssp_imclient_17687()), (object) objectInfo.Caption, (object) this._projObjInfo.ObjectID, (object) result1, (object) MetaDataHelper.GetObjectName(this._projObjInfo.ObjTypeID));
        List<ConditionStructure> conditionStructureList = new List<ConditionStructure>(1)
        {
          new ConditionStructure(-7, RelationalOperators.In, (object) MetaDataHelper.GetObjectTypeChildrenIDRecursive(ReportsConsts.DocPackageBaseTypeID).ToArray(), LogicalOperators.NONE, 0, false)
        };
        DataTable parentSostavData = DataHelper.GetParentSostavData(this._complectObjInfo, sessionKeeper.Session, (IEnumerable<int>) new int[1]
        {
          ReportsConsts.SimpleWithSortRelationID
        }, false, (IEnumerable<ConditionStructure>) conditionStructureList.ToArray());
        if (parentSostavData != null)
        {
          if (parentSostavData.Rows.Count > 0)
            text = text + sc_17682.ssp_imclient_17688() + LocalizationHolder.rm.GetString("Reports_51");
        }
      }
      int num4 = (int) MessageBox.Show(text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }
    switch (this._mode)
    {
      case ReportMode.CreateVersion:
        this._archiveID = result2;
        break;
      case ReportMode.Update:
        if (result2 != 0L)
        {
          this._archiveID = result2;
          break;
        }
        if (!this.DoExecute_SelectArchive((IEnumerable<ObjInfoItem>) new ObjInfoItem[1]
        {
          this._scriptObjInfo
        }, out this._archiveID))
          return false;
        break;
    }
    return true;
  }

  /// <summary>Конструктор</summary>
  /// <param name="mode"></param>
  public ComplectUpdateCommand(ReportMode mode) => this._mode = mode;

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  protected override bool DoExecute_ValidateParams()
  {
    if (!base.DoExecute_ValidateParams() || !(this._items.GetParentData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID parentData))
      return false;
    this._projObjInfo = new ObjInfoItem(parentData.ObjectID, parentData.ObjectType);
    return !ObjInfoItem.IsEmpty((ITypedInfoItem) this._projObjInfo);
  }

  /// <summary>Загрузка информации об объектах</summary>
  /// <returns></returns>
  protected override bool DoExecute_LoadObjInfo()
  {
    if (!base.DoExecute_LoadObjInfo() || this._objInfoList == null || this._objInfoList.Count == 0)
      return false;
    this._complectObjInfo = this._objInfoList[0];
    return MetaDataHelper.IsObjectTypeChildOf(this._complectObjInfo.ObjTypeID, ReportsConsts.DocPackageBaseTypeID);
  }

  /// <summary>Выполнение команды</summary>
  protected override void DoExecute_Command()
  {
    if (!this.DoExecute_CheckAccess() || !this.DoExecute_ParamsLoad(this._complectObjInfo) || !this.DoExecute_LoadScriptInfo() || !this.DoExecute_ParamsDialog(this._complectObjInfo, this._scriptObjInfo))
      return;
    this.DoExecute_StartTask();
  }

  /// <summary>
  /// Проверка пров доступа на комплект / возможность модификации объекта
  /// </summary>
  /// <returns></returns>
  private bool DoExecute_CheckAccess()
  {
    if (this._mode == ReportMode.CreateVersion)
      return true;
    List<ObjInfoItem> complect2CheckList = new List<ObjInfoItem>()
    {
      this._complectObjInfo
    };
    return this.TaskMode != ReportTaskMode.Default || this.DoExecute_AllowComplectEdit(ref complect2CheckList);
  }
}
