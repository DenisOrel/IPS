// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Tasks.ComplectGenerateCommand
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using ImSSP;
using Intermech.Expert;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Expert;
using Intermech.Interfaces.Reports;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Reports.Tasks;

/// <summary>Выполнение команд меню генерации комплекта</summary>
public class ComplectGenerateCommand : GenerateBaseCommand
{
  /// <summary>Флаг уникальности КТД для пары: объект / ид. скрипта</summary>
  private static bool set_mode_KtdUnique = true;
  /// <summary>Кэш вида объект -&gt; скрипт генерации комплектов</summary>
  private IDictionary<ObjInfoItem, ObjInfoItem> _object2ScriptInfoList;
  /// <summary>Кэш вида : объект -&gt; его краткое описание</summary>
  private readonly IDictionary<ObjInfoItem, QuickObjectInfo> _object2QuickObjectInfo = (IDictionary<ObjInfoItem, QuickObjectInfo>) new Dictionary<ObjInfoItem, QuickObjectInfo>();
  /// <summary>
  /// Кэш вида объект -&gt; перечень существующих комплектов
  /// </summary>
  private IDictionary<ObjInfoItem, IList<ComplectGenerateCommand.ComplectObjInfo>> _object2ComplectUpdate;

  /// <summary>
  /// Загрузка информации о скриптах генерации документов для исходных объектов
  /// </summary>
  /// <returns></returns>
  private bool DoExecute_LoadScriptInfo()
  {
    if (this._objInfoList == null || this._objInfoList.Count == 0)
      return false;
    IDictionary<ObjInfoItem, IList<ObjInfoItem>> object2ScriptPackageInfo;
    if (!ComplectGenerateCommand.GetScriptPackage4Objects(this._objInfoList, out object2ScriptPackageInfo))
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString(sc_17672.ssp_imclient_17673()), LocalizationHolder.rm.GetString("Reports_9"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }
    if (object2ScriptPackageInfo.Count == 0)
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString(sc_17672.ssp_imclient_17674()), LocalizationHolder.rm.GetString("Reports_9"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }
    ObjInfoItem objInfoItem = (ObjInfoItem) null;
    Dictionary<ObjInfoItem, ObjInfoItem> dictionary = new Dictionary<ObjInfoItem, ObjInfoItem>(object2ScriptPackageInfo.Count);
    IList<ObjInfoItem> sampleScriptList = object2ScriptPackageInfo.Values.FirstOrDefault<IList<ObjInfoItem>>();
    if (sampleScriptList != null && sampleScriptList.Count > 1 && object2ScriptPackageInfo.Values.All<IList<ObjInfoItem>>((System.Func<IList<ObjInfoItem>, bool>) (item => GenericListHelper.Compare<ObjInfoItem>(item, sampleScriptList) == 0)))
    {
      long[] ids = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("Reports_64"), string.Empty, (IDescriptor) new ListDescriptor(CategoryHelper.ReportCategoryID, ReportsConsts.ScriptPackageTypeID, LocalizationHolder.rm.GetString(sc_17672.ssp_imclient_17675()), (IList) sampleScriptList.Select<ObjInfoItem, long>((System.Func<ObjInfoItem, long>) (scriptInfo => scriptInfo.ObjectID)).ToArray<long>()), SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect | SelectionOptions.ForceRebuildNavTree);
      if (ids != null && ids.Length != 0)
        objInfoItem = sampleScriptList.FirstOrDefault<ObjInfoItem>((System.Func<ObjInfoItem, bool>) (elem => elem.ObjectID == ids[0]));
      if (ObjInfoItem.IsEmpty((ITypedInfoItem) objInfoItem))
        return false;
      foreach (KeyValuePair<ObjInfoItem, IList<ObjInfoItem>> keyValuePair in (IEnumerable<KeyValuePair<ObjInfoItem, IList<ObjInfoItem>>>) object2ScriptPackageInfo)
        dictionary.Add(keyValuePair.Key, objInfoItem);
      this._object2ScriptInfoList = (IDictionary<ObjInfoItem, ObjInfoItem>) dictionary;
      return true;
    }
    foreach (KeyValuePair<ObjInfoItem, IList<ObjInfoItem>> keyValuePair in (IEnumerable<KeyValuePair<ObjInfoItem, IList<ObjInfoItem>>>) object2ScriptPackageInfo)
    {
      IList<ObjInfoItem> source = keyValuePair.Value;
      switch (source.Count)
      {
        case 0:
          if (!ObjInfoItem.IsEmpty((ITypedInfoItem) objInfoItem))
          {
            dictionary.Add(keyValuePair.Key, objInfoItem);
            continue;
          }
          continue;
        case 1:
          objInfoItem = source[0];
          goto case 0;
        default:
          string caption;
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(keyValuePair.Key.ObjectID);
            if (!objectInfo.Empty)
              caption = objectInfo.Caption;
            else
              continue;
          }
          long[] ids = Intermech.Navigator.SelectionWindow.SelectObjects(string.Format(LocalizationHolder.rm.GetString(sc_17672.ssp_imclient_17676()), (object) caption, (object) keyValuePair.Key.ObjectID), string.Empty, (IDescriptor) new ListDescriptor(CategoryHelper.ReportCategoryID, ReportsConsts.ScriptPackageTypeID, LocalizationHolder.rm.GetString("Reports_36"), (IList) source.Select<ObjInfoItem, long>((System.Func<ObjInfoItem, long>) (scriptInfo => scriptInfo.ObjectID)).ToArray<long>()), SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect | SelectionOptions.ForceRebuildNavTree);
          if (ids != null && ids.Length != 0)
          {
            objInfoItem = source.FirstOrDefault<ObjInfoItem>((System.Func<ObjInfoItem, bool>) (elem => elem.ObjectID == ids[0]));
            goto case 0;
          }
          goto case 0;
      }
    }
    this._object2ScriptInfoList = (IDictionary<ObjInfoItem, ObjInfoItem>) dictionary;
    return true;
  }

  /// <summary>
  /// Загрузка информации по существующим комплектам и их анализ
  /// </summary>
  /// <returns></returns>
  private bool DoExecute_LoadComplectInfo()
  {
    if (this._objInfoList == null || this._objInfoList.Count == 0 || this._object2ScriptInfoList == null || this._object2ScriptInfoList.Count == 0 || !ComplectGenerateCommand.CheckComplectInfo4Objects(this._object2ScriptInfoList, out this._object2ComplectUpdate))
      return false;
    if (this._object2ComplectUpdate.Count == 0)
      return true;
    List<ObjInfoItem> allComplectInfoList = new List<ObjInfoItem>();
    foreach (IList<ComplectGenerateCommand.ComplectObjInfo> collection in (IEnumerable<IList<ComplectGenerateCommand.ComplectObjInfo>>) this._object2ComplectUpdate.Values)
      allComplectInfoList.AddRange((IEnumerable<ObjInfoItem>) collection);
    GenericListHelper.MakeUnique<ObjInfoItem>(allComplectInfoList);
    if (allComplectInfoList.Count > 0)
    {
      long[] array = allComplectInfoList.Select<ObjInfoItem, long>((System.Func<ObjInfoItem, long>) (item => item.ObjectID)).ToArray<long>();
      Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new ObjectsSelectedItemsAnalyzer(array), true);
      long[] ids = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString(sc_17672.ssp_imclient_17677()), string.Empty, (IDescriptor) new ListDescriptor(CategoryHelper.ReportCategoryID, ReportsConsts.DocPackageBaseTypeID, LocalizationHolder.rm.GetString("Reports_47"), (IList) array), SelectionOptions.SelectObjects | SelectionOptions.ForceRebuildNavTree);
      if (ids != null && ids.Length != 0)
      {
        allComplectInfoList = allComplectInfoList.Where<ObjInfoItem>((System.Func<ObjInfoItem, bool>) (item => Array.IndexOf<long>(ids, item.ObjectID) != -1)).ToList<ObjInfoItem>();
      }
      else
      {
        allComplectInfoList.Clear();
        return false;
      }
    }
    if (allComplectInfoList.Count == 0)
    {
      this._object2ComplectUpdate.Clear();
      return true;
    }
    if (this.TaskMode == ReportTaskMode.Default && !this.DoExecute_AllowComplectEdit(ref allComplectInfoList))
      return false;
    this._object2ComplectUpdate = (IDictionary<ObjInfoItem, IList<ComplectGenerateCommand.ComplectObjInfo>>) this._object2ComplectUpdate.ToDictionary<KeyValuePair<ObjInfoItem, IList<ComplectGenerateCommand.ComplectObjInfo>>, ObjInfoItem, IList<ComplectGenerateCommand.ComplectObjInfo>>((System.Func<KeyValuePair<ObjInfoItem, IList<ComplectGenerateCommand.ComplectObjInfo>>, ObjInfoItem>) (item => item.Key), (System.Func<KeyValuePair<ObjInfoItem, IList<ComplectGenerateCommand.ComplectObjInfo>>, IList<ComplectGenerateCommand.ComplectObjInfo>>) (item => (IList<ComplectGenerateCommand.ComplectObjInfo>) item.Value.Where<ComplectGenerateCommand.ComplectObjInfo>((System.Func<ComplectGenerateCommand.ComplectObjInfo, bool>) (subItem => allComplectInfoList.Contains((ObjInfoItem) subItem))).ToList<ComplectGenerateCommand.ComplectObjInfo>()));
    return true;
  }

  /// <summary>Запуск генерации комплектов</summary>
  private void DoExecute_StartTask()
  {
    if (this._object2ScriptInfoList == null || this._object2ScriptInfoList.Count == 0 || this._object2ComplectUpdate == null || !this.DoExecute_LoadArchiveInfo() || !this.DoExecute_LoadObjectInfo())
      return;
    List<ComplectBackgroundBaseTask> backgroundBaseTaskList = new List<ComplectBackgroundBaseTask>(this._object2ScriptInfoList.Count);
    SessionKeeper sessionKeeper = (SessionKeeper) null;
    try
    {
      if (this._object2ScriptInfoList.Count > 1)
        sessionKeeper = new SessionKeeper();
      foreach (KeyValuePair<ObjInfoItem, ObjInfoItem> object2ScriptInfo in (IEnumerable<KeyValuePair<ObjInfoItem, ObjInfoItem>>) this._object2ScriptInfoList)
      {
        if (!ObjInfoItem.IsEmpty((ITypedInfoItem) object2ScriptInfo.Value))
        {
          ComplectBackgroundBaseTask task = (ComplectBackgroundBaseTask) null;
          long objectId = object2ScriptInfo.Key.ObjectID;
          IList<ComplectGenerateCommand.ComplectObjInfo> complectObjInfoList;
          if (this._object2ComplectUpdate.TryGetValue(object2ScriptInfo.Key, out complectObjInfoList))
          {
            foreach (ComplectGenerateCommand.ComplectObjInfo complectObjInfo in (IEnumerable<ComplectGenerateCommand.ComplectObjInfo>) complectObjInfoList)
            {
              this.DoExecute_ParamsLoad((ObjInfoItem) complectObjInfo);
              if (this._object2ScriptInfoList.Count == 1)
                this.DoExecute_ParamsDialog((ObjInfoItem) complectObjInfo, object2ScriptInfo.Value);
              task = (ComplectBackgroundBaseTask) new ComplectUpdateBackgroundTask((IReportTaskParams) new ReportTaskParams(objectId, object2ScriptInfo.Value.ObjectID, complectObjInfo.ObjectID)
              {
                ArchiveId = (complectObjInfo.ArchiveId != 0L ? complectObjInfo.ArchiveId : this._archiveID),
                TaskMode = this.TaskMode
              });
            }
          }
          else
          {
            if (this._object2ScriptInfoList.Count == 1)
              this.DoExecute_ParamsDialog((ObjInfoItem) null, object2ScriptInfo.Value);
            task = (ComplectBackgroundBaseTask) new ComplectGenerateBackgroundTask((IReportTaskParams) new ReportTaskParams(objectId, object2ScriptInfo.Value.ObjectID)
            {
              ArchiveId = this._archiveID,
              TaskMode = this.TaskMode
            });
          }
          if (task != null)
          {
            task.Params.Attributes = this._repParams;
            QuickObjectInfo quickObjectInfo;
            if (this._object2QuickObjectInfo.TryGetValue(object2ScriptInfo.Key, out quickObjectInfo))
              task.ObjectInfo = quickObjectInfo;
            if (this._object2ScriptInfoList.Count >= 20)
              task.Options = task.Options.AddFlags<ReportTaskOptions>(ReportTaskOptions.HideDocWindow);
            backgroundBaseTaskList.Add(task);
            ReportsClientCache.Services.BackgroundTaskView.AddTask((IBackgroundTask) task);
          }
        }
      }
    }
    finally
    {
      sessionKeeper?.Dispose();
    }
    Thread.Sleep(100);
    foreach (ComplectBackgroundBaseTask backgroundBaseTask in backgroundBaseTaskList)
      backgroundBaseTask.Execute();
  }

  /// <summary>Загрузка описания объектов</summary>
  /// <remarks>Для оптимизации выполнения большого числа задач BB# 1305816 </remarks>
  /// <returns></returns>
  private bool DoExecute_LoadObjectInfo()
  {
    DBRecordSetParams dbRsp = new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(-15, RelationalOperators.NOP, (object) null, LogicalOperators.NONE, 0, false)
    }, new ColumnDescriptor[5]
    {
      new ColumnDescriptor((object) -2),
      new ColumnDescriptor((object) -50),
      new ColumnDescriptor((object) -7),
      new ColumnDescriptor((object) -18),
      new ColumnDescriptor((object) -3)
    });
    DataTable objectDataEx;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      objectDataEx = DataHelper.GetObjectDataEx(-1, sessionKeeper.Session, dbRsp, (IEnumerable<ObjInfoItem>) new List<ObjInfoItem>((IEnumerable<ObjInfoItem>) this._object2ScriptInfoList.Keys));
    if (objectDataEx == null || objectDataEx.Rows.Count == 0)
      return false;
    foreach (DataRow row in (InternalDataCollectionBase) objectDataEx.Rows)
    {
      string str = Convert.ToString(row[3]);
      QuickObjectInfo quickObjectInfo = new QuickObjectInfo(Convert.ToInt64(row[0]), Convert.ToString(row[1]), Convert.ToInt32(row[2]), GuidHelper.IsGuid(str) ? new Guid(str) : Guid.Empty, Convert.ToInt64(row[4]));
      this._object2QuickObjectInfo[new ObjInfoItem(quickObjectInfo.ObjectID, quickObjectInfo.ObjectTypeID)] = quickObjectInfo;
    }
    return true;
  }

  /// <summary>
  /// 
  /// </summary>
  private bool DoExecute_LoadArchiveInfo()
  {
    List<ObjInfoItem> scripts = new List<ObjInfoItem>();
    foreach (KeyValuePair<ObjInfoItem, ObjInfoItem> object2ScriptInfo in (IEnumerable<KeyValuePair<ObjInfoItem, ObjInfoItem>>) this._object2ScriptInfoList)
    {
      IList<ComplectGenerateCommand.ComplectObjInfo> source;
      if (!this._object2ComplectUpdate.TryGetValue(object2ScriptInfo.Key, out source) || source == null || source.Count == 0)
        scripts.Add(object2ScriptInfo.Value);
      else if (source.Any<ComplectGenerateCommand.ComplectObjInfo>((System.Func<ComplectGenerateCommand.ComplectObjInfo, bool>) (complectInfo => complectInfo.ArchiveId == 0L)))
        scripts.Add(object2ScriptInfo.Value);
    }
    return scripts.Count == 0 || this.DoExecute_SelectArchive((IEnumerable<ObjInfoItem>) scripts, out this._archiveID);
  }

  /// <summary>
  /// 
  /// </summary>
  protected override void DoExecute_Command()
  {
    if (!this.DoExecute_LoadScriptInfo() || !this.DoExecute_LoadComplectInfo())
      return;
    this.DoExecute_StartTask();
  }

  /// <summary>
  /// Проверка наличия существующих комплектов для объектов
  /// </summary>
  /// <param name="object2ScriptInfo"></param>
  /// <param name="object2ComplectInfo"></param>
  /// <remarks>В качестве значения создаем именно список, т.к. в
  ///  данный момент может быть несколько комплектов - возможно user захочет обновить их всех
  ///  (хотя это совсем и не логично)
  /// </remarks>
  /// <returns></returns>
  public static bool CheckComplectInfo4Objects(
    IDictionary<ObjInfoItem, ObjInfoItem> object2ScriptInfo,
    out IDictionary<ObjInfoItem, IList<ComplectGenerateCommand.ComplectObjInfo>> object2ComplectInfo)
  {
    object2ComplectInfo = object2ScriptInfo != null ? (IDictionary<ObjInfoItem, IList<ComplectGenerateCommand.ComplectObjInfo>>) new Dictionary<ObjInfoItem, IList<ComplectGenerateCommand.ComplectObjInfo>>(object2ScriptInfo.Count) : throw new ArgumentException(nameof (object2ScriptInfo));
    if (!ComplectGenerateCommand.set_mode_KtdUnique)
      return true;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ConditionStructure[] joinedConditions = new ConditionStructure[1]
      {
        new ConditionStructure(-7, RelationalOperators.In, (object) MetaDataHelper.GetObjectTypeChildrenIDRecursive(ReportsConsts.DocPackageBaseTypeID).ToArray(), LogicalOperators.NONE, 0, false)
      };
      ColumnDescriptor[] columns = new ColumnDescriptor[2]
      {
        new ColumnDescriptor((object) ReportsConsts.ScriptPackageAttrTypeID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.Guid, SortOrders.NONE, 0),
        new ColumnDescriptor((object) ReportsConsts.ArchiveAttributeTypeID, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.Guid, SortOrders.NONE, 0)
      };
      List<ObjInfoItem> list1 = new List<ObjInfoItem>((IEnumerable<ObjInfoItem>) object2ScriptInfo.Values);
      GenericListHelper.MakeUnique<ObjInfoItem>(list1);
      IDictionary<ObjInfoItem, ObjInfoItem>[] dictionaryArray;
      if (list1.Count > 150)
        dictionaryArray = (IDictionary<ObjInfoItem, ObjInfoItem>[]) GenericListHelper.SplitByChanks<ObjInfoItem, ObjInfoItem>(object2ScriptInfo, 150);
      else
        dictionaryArray = new IDictionary<ObjInfoItem, ObjInfoItem>[1]
        {
          object2ScriptInfo
        };
      DataTable toTable = (DataTable) null;
      foreach (IDictionary<ObjInfoItem, ObjInfoItem> dictionary in dictionaryArray)
      {
        List<long> list2 = dictionary.Values.Select<ObjInfoItem, long>((System.Func<ObjInfoItem, long>) (item => Math.Abs(item.ObjectID))).ToList<long>();
        GenericListHelper.MakeUnique<long>(list2);
        for (int count = list2.Count; count < 150; ++count)
          list2.Add(0L);
        ConditionStructure[] conditions = ConditionStructure.Join(joinedConditions, new ConditionStructure[1]
        {
          new ConditionStructure(ReportsConsts.ScriptPackageAttrTypeID, RelationalOperators.In, (object) list2.ToArray(), (object) null, LogicalOperators.NOT, 0, false, AttributeSourceTypes.Object, ColumnContents.ID)
        });
        DataTable childSostavData = DataHelper.GetChildSostavData((IEnumerable<ObjInfoItem>) new List<ObjInfoItem>((IEnumerable<ObjInfoItem>) dictionary.Keys), sessionKeeper.Session, (IEnumerable<int>) new int[1]
        {
          ReportsConsts.SimpleWithSortRelationID
        }, false, (IEnumerable<ConditionStructure>) conditions, (IEnumerable<ColumnDescriptor>) columns, (HybridDictionary) null);
        if (toTable == null)
          toTable = childSostavData;
        else
          DataSetProcessor.AddTable(toTable, childSostavData, false);
      }
      toTable?.AcceptChanges();
      if (toTable != null)
      {
        int columnIndex1 = toTable.Columns.IndexOf("F_PROJ_ID");
        int columnIndex2 = toTable.Columns.IndexOf("F_OBJECT_ID");
        int columnIndex3 = toTable.Columns.IndexOf(ReportsConsts.ScriptPackageAttrTypeGuid.ToString());
        int columnIndex4 = toTable.Columns.IndexOf(ReportsConsts.ArchiveAttributeTypeGuid.ToString());
        foreach (DataRow row in (InternalDataCollectionBase) toTable.Rows)
        {
          ObjInfoItem key = new ObjInfoItem(Convert.ToInt64(row[columnIndex1]));
          long int64_1 = Convert.ToInt64(row[columnIndex3]);
          long int64_2 = Convert.ToInt64(row[columnIndex2]);
          ObjInfoItem objInfoItem;
          if (object2ScriptInfo.TryGetValue(key, out objInfoItem) && int64_1 == Math.Abs(objInfoItem.ObjectID))
          {
            IList<ComplectGenerateCommand.ComplectObjInfo> complectObjInfoList;
            if (!object2ComplectInfo.TryGetValue(key, out complectObjInfoList))
            {
              complectObjInfoList = (IList<ComplectGenerateCommand.ComplectObjInfo>) new List<ComplectGenerateCommand.ComplectObjInfo>();
              object2ComplectInfo.Add(key, complectObjInfoList);
            }
            complectObjInfoList.Add(new ComplectGenerateCommand.ComplectObjInfo(int64_2, row[columnIndex4] != DBNull.Value ? Convert.ToInt64(row[columnIndex4]) : 0L));
          }
        }
      }
    }
    return true;
  }

  /// <summary>
  /// Загрузка информации о скриптах генерации документов для объектов
  /// </summary>
  /// <param name="objectInfoList">Список с описанием объектов</param>
  /// <param name="object2ScriptPackageInfo">Кэш вида : Объект -&gt; Перечень скриптов генерации</param>
  /// <param name="checkScriptCondition">Проверка условий ЭС для скриптов, если они заданы</param>
  /// <returns>True - если хотя бы для одного объекта найдены скрипты (без проверки условий)</returns>
  public static bool GetScriptPackage4Objects(
    IList<ObjInfoItem> objectInfoList,
    out IDictionary<ObjInfoItem, IList<ObjInfoItem>> object2ScriptPackageInfo,
    bool checkScriptCondition = true)
  {
    object2ScriptPackageInfo = (IDictionary<ObjInfoItem, IList<ObjInfoItem>>) new Dictionary<ObjInfoItem, IList<ObjInfoItem>>();
    if (objectInfoList == null)
      return false;
    Dictionary<Guid, int> dictionary1 = new Dictionary<Guid, int>();
    int result1;
    foreach (ObjInfoItem objectInfo in (IEnumerable<ObjInfoItem>) objectInfoList)
    {
      if (!((TypedInfoItem) objectInfo == (TypedInfoItem) null) && objectInfo.ObjTypeID != -1)
      {
        result1 = objectInfo.ObjTypeID;
        List<int> parentsIdReverse = MetaDataHelper.GetObjectTypeParentsIDReverse(result1);
        if (!parentsIdReverse.Contains(result1))
          parentsIdReverse.Add(result1);
        foreach (int objTypeID in parentsIdReverse)
        {
          Guid objectTypeGuid = MetaDataHelper.GetObjectTypeGuid(objTypeID);
          if (objectTypeGuid != Guid.Empty && !dictionary1.ContainsKey(objectTypeGuid))
            dictionary1.Add(objectTypeGuid, objTypeID);
        }
      }
    }
    List<Guid> guidList = new List<Guid>((IEnumerable<Guid>) dictionary1.Keys);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      session.DBObjectsCacheStart();
      try
      {
        IExpertServer service = ServiceUtils.GetService<IExpertServer>((object) session, true);
        bool flag = MetaDataHelper.GetAttribute4ObjectType(ReportsConsts.ScriptPackageTypeID, ReportsConsts.ConditionAttrTypeID) != null;
        IExpertUser expertUser = (IExpertUser) null;
        if (flag)
          expertUser = ServiceUtils.GetService<IExpertUser>((object) ServicesManager.ServiceContainer, true);
        Dictionary<int, List<ObjInfoItem>> dictionary2 = new Dictionary<int, List<ObjInfoItem>>();
        if (session.GetObjectCollection(ReportsConsts.ScriptPackageTypeID) == null)
          return false;
        List<ConditionStructure> conditionStructureList = new List<ConditionStructure>()
        {
          new ConditionStructure(ReportsConsts.ObjTypeResultAttrTypeID, RelationalOperators.In, (object) guidList.ToArray(), LogicalOperators.NONE, 0, false)
        };
        List<ColumnDescriptor> columnDescriptorList = new List<ColumnDescriptor>()
        {
          new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0),
          new ColumnDescriptor((object) ReportsConsts.ObjTypeResultAttrTypeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0),
          new ColumnDescriptor((object) -7, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Guid, SortOrders.NONE, 0)
        };
        DBRecordSetParams dbRecordSetParams = new DBRecordSetParams(conditionStructureList.ToArray(), columnDescriptorList.ToArray());
        ref DBRecordSetParams local = ref dbRecordSetParams;
        HybridDictionary hybridDictionary = new HybridDictionary();
        object key1 = (object) "{7FB30639-2F65-4407-B78E-523547B1B133}";
        hybridDictionary[key1] = (object) true;
        local.Tags = hybridDictionary;
        DBRecordSetParams dbRsp = dbRecordSetParams;
        DataTable objectData = DataHelper.GetObjectData(ReportsConsts.ScriptPackageTypeID, session, dbRsp, (IEnumerable<long>) null);
        if (objectData != null && objectData.Rows.Count != 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) objectData.Rows)
          {
            long result2;
            long.TryParse(row[0].ToString(), out result2);
            if (result2 != 0L)
            {
              string str = row[1].ToString();
              if (GuidHelper.IsGuid(str))
              {
                Guid key2 = new Guid(str);
                int key3;
                if (dictionary1.TryGetValue(key2, out key3))
                {
                  int.TryParse(row[2].ToString(), out result1);
                  ObjInfoItem objInfoItem = new ObjInfoItem(result2, result1);
                  List<ObjInfoItem> objInfoItemList;
                  if (dictionary2.TryGetValue(key3, out objInfoItemList))
                  {
                    if (!objInfoItemList.Contains(objInfoItem))
                      objInfoItemList.Add(objInfoItem);
                  }
                  else
                  {
                    objInfoItemList = new List<ObjInfoItem>()
                    {
                      objInfoItem
                    };
                    dictionary2.Add(key3, objInfoItemList);
                  }
                }
              }
            }
          }
        }
        if (dictionary2.Count == 0)
          return false;
        foreach (ObjInfoItem objectInfo in (IEnumerable<ObjInfoItem>) objectInfoList)
        {
          long objectId = objectInfo.ObjectID;
          int objTypeId = objectInfo.ObjTypeID;
          List<ObjInfoItem> collection;
          if (!dictionary2.TryGetValue(objTypeId, out collection))
          {
            foreach (int key4 in MetaDataHelper.GetObjectTypeParentsIDReverse(objTypeId))
            {
              if (dictionary2.TryGetValue(key4, out collection))
                break;
            }
          }
          if (collection != null && collection.Count != 0)
          {
            if (flag & checkScriptCondition)
            {
              List<ObjInfoItem> objInfoItemList = new List<ObjInfoItem>();
              using (IExpertTask expertTask = expertUser.GetExpertTask())
              {
                foreach (ObjInfoItem objInfoItem in collection)
                {
                  switch (service.CalcFormula(expertTask.TaskId, objInfoItem.ObjectID, ReportsConsts.ConditionAttrTypeGuid, objectId, out key1))
                  {
                    case ExpertResult.OK:
                    case ExpertResult.ObjectNotFound:
                      continue;
                    default:
                      objInfoItemList.Add(objInfoItem);
                      continue;
                  }
                }
              }
              if (objInfoItemList.Count != 0)
              {
                collection = new List<ObjInfoItem>((IEnumerable<ObjInfoItem>) collection);
                foreach (ObjInfoItem objInfoItem in objInfoItemList)
                  collection.Remove(objInfoItem);
              }
            }
            if (collection.Count != 0)
              object2ScriptPackageInfo.Add(objectInfo, (IList<ObjInfoItem>) collection);
          }
        }
      }
      finally
      {
        session.DBObjectsCacheStop();
      }
    }
    return true;
  }

  /// <summary>Объект - описание комплекта документов</summary>
  /// <summary>Конструктор</summary>
  /// <param name="objectId">Ид. версии объекта</param>
  /// <param name="objTypeId">Ид. типа объекта</param>
  public class ComplectObjInfo(long objectId, int objTypeId) : ObjInfoItem(objectId, objTypeId)
  {
    /// <summary>Конструктор</summary>
    /// <param name="objectId">Ид. версии объекта</param>
    public ComplectObjInfo(long objectId, long archiveId = 0)
      : this(objectId, -1)
    {
      this.ArchiveId = archiveId;
    }

    /// <summary>Ид. архива для объекта</summary>
    public long ArchiveId { get; }
  }
}
