// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Tasks.GenerateBaseCommand
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
using Intermech.Interfaces.ParamsStorage;
using Intermech.Interfaces.Reports;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Reports.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Reports.Tasks;

/// <summary>
/// Базовый класс для генерации / обновления комплектов для объекта
/// </summary>
public abstract class GenerateBaseCommand : ReportBaseCommand
{
  /// <summary>Ид. архива</summary>
  protected long _archiveID;
  /// <summary>Параметры задачи</summary>
  protected AttributeValues[] _repParams;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void readOnlyComplect_btnOKChanged(object sender, EventArgs e)
  {
    if (!(sender is Button button) || button.Enabled)
      return;
    button.Enabled = true;
  }

  /// <summary>Заполнение параметров задачи</summary>
  /// <param name="complectObjInfo">Информация о комплекте документов</param>
  protected bool DoExecute_ParamsLoad(ObjInfoItem complectObjInfo)
  {
    this._repParams = (AttributeValues[]) null;
    if (ObjInfoItem.IsEmpty((ITypedInfoItem) complectObjInfo))
      return true;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._repParams = sessionKeeper.Session.GetObject(complectObjInfo.ObjectID, true).GetAttributesValues(GetAttributeValuesModes.CheckVisibility);
    return true;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="complectObjInfo">Информация о комплекте документов</param>
  /// <param name="scriptObjInfo">Информация о скрипте генерации комплекта документов</param>
  protected bool DoExecute_ParamsDialog(ObjInfoItem complectObjInfo, ObjInfoItem scriptObjInfo)
  {
    if (ObjInfoItem.IsEmpty((ITypedInfoItem) scriptObjInfo))
      return false;
    IParamsStorageObject paramsStorageObject = ServiceUtils.GetService<IParamsStorageService>((object) ApplicationServices.Container, false)?.GetObject(ReportUtils.GetContainerName(scriptObjInfo.ObjectID));
    if (paramsStorageObject != null)
    {
      string caption = LocalizationHolder.rm.GetString("Reports_58");
      AttributeValues[] resultValues;
      if (paramsStorageObject.ShowDialog(caption, true, this._repParams, out resultValues) == DialogResult.Cancel)
        return false;
      this.DoExecute_ParamsSave(complectObjInfo, resultValues);
      this._repParams = resultValues;
    }
    return true;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="complectObjInfo"></param>
  /// <param name="newRepParams"></param>
  protected void DoExecute_ParamsSave(ObjInfoItem complectObjInfo, AttributeValues[] newRepParams)
  {
    if (newRepParams == null || ObjInfoItem.IsEmpty((ITypedInfoItem) complectObjInfo))
      return;
    Dictionary<int, AttributeValues> dictionary = this._repParams != null ? ((IEnumerable<AttributeValues>) this._repParams).ToDictionary<AttributeValues, int, AttributeValues>((System.Func<AttributeValues, int>) (item => item.AttributeID), (System.Func<AttributeValues, AttributeValues>) (item => item)) : new Dictionary<int, AttributeValues>();
    List<AttributeValues> attributeValuesList = new List<AttributeValues>();
    foreach (AttributeValues newRepParam in newRepParams)
    {
      if (newRepParam != null)
      {
        AttributeValues attributeValues;
        dictionary.TryGetValue(newRepParam.AttributeID, out attributeValues);
        if (attributeValues == null || !attributeValues.Equals(newRepParam))
          attributeValuesList.Add(newRepParam);
      }
    }
    if (attributeValuesList.Count == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(complectObjInfo.ObjectID, true);
      if (dbObject == null)
        return;
      if (dbObject.ReadOnly && dbObject.ObjectModifyMode == ObjectModifyModes.Checkout && dbObject.CheckoutBy == 0L)
        dbObject = dbObject.CheckOut(false);
      if (dbObject.ReadOnly)
        return;
      dbObject.SetAttributesValues(attributeValuesList.ToArray(), false, true);
    }
  }

  /// <summary>
  /// Проверка комплектов документов на возможность редактирования / взятия на редактирование
  /// </summary>
  /// <param name="complect2CheckList"></param>
  /// <returns></returns>
  protected bool DoExecute_AllowComplectEdit(ref List<ObjInfoItem> complect2CheckList)
  {
    if (complect2CheckList == null || complect2CheckList.Count == 0)
      return false;
    List<ObjInfoItem> readOnlyComplectInfoItems = new List<ObjInfoItem>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (ObjInfoItem objInfoItem in complect2CheckList)
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(objInfoItem.ObjectID, false);
        if (dbObject != null)
        {
          switch (dbObject.ObjectModifyMode)
          {
            case ObjectModifyModes.InBase:
            case ObjectModifyModes.CreateVersion:
            case ObjectModifyModes.CantModify:
              if (dbObject.ReadOnly)
              {
                readOnlyComplectInfoItems.Add(objInfoItem);
                continue;
              }
              continue;
            case ObjectModifyModes.Checkout:
              if (dbObject.CheckoutBy != 0L && dbObject.CheckoutBy != sessionKeeper.Session.UserID)
              {
                readOnlyComplectInfoItems.Add(objInfoItem);
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
    }
    complect2CheckList = complect2CheckList.Where<ObjInfoItem>((System.Func<ObjInfoItem, bool>) (item => !readOnlyComplectInfoItems.Contains(item))).ToList<ObjInfoItem>();
    if (readOnlyComplectInfoItems.Count != 0)
    {
      bool flag = complect2CheckList.Count > 0;
      Intermech.Navigator.Controls.SelectionWindow form = Intermech.Navigator.SelectionWindow.CreateForm(LocalizationHolder.rm.GetString(sc_17689.ssp_imclient_17690()), LocalizationHolder.rm.GetString("Reports_61"), (IDescriptor) new ListDescriptor(CategoryHelper.ReportCategoryID, ReportsConsts.DocPackageBaseTypeID, LocalizationHolder.rm.GetString("Reports_47"), (IList) readOnlyComplectInfoItems.Select<ObjInfoItem, long>((System.Func<ObjInfoItem, long>) (item => item.ObjectID)).ToArray<long>()), typeof (IDBObjectID), (DynamicSelectionEventHandler) null, SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect);
      try
      {
        if (flag)
        {
          form.btOK.Text = LocalizationHolder.rm.GetString(sc_17689.ssp_imclient_17691());
          form.btOK.Visible = true;
          form.btOK.Enabled = true;
          form.btOK.EnabledChanged += new EventHandler(this.readOnlyComplect_btnOKChanged);
        }
        else
        {
          form.btOK.Visible = false;
          form.btOK.Enabled = false;
        }
        form.btCancel.Text = LocalizationHolder.rm.GetString("Reports_62");
        DialogResult dialogResult = form.ShowDialog();
        if (flag)
        {
          if (dialogResult == DialogResult.OK)
            goto label_25;
        }
        return false;
      }
      finally
      {
        form.btOK.EnabledChanged -= new EventHandler(this.readOnlyComplect_btnOKChanged);
        Intermech.Navigator.SelectionWindow.CloseWindow(form);
      }
    }
label_25:
    return true;
  }

  /// <summary>
  /// Выбор архива, если хотя бы один из скриптов этого требует
  /// </summary>
  /// <param name="scripts"></param>
  /// <param name="archiveId"></param>
  /// <returns></returns>
  protected bool DoExecute_SelectArchive(IEnumerable<ObjInfoItem> scripts, out long archiveId)
  {
    archiveId = 0L;
    if (scripts == null)
      return false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DBRecordSetParams dbRsp = new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(ReportsConsts.NeedArchiveAttributeTypeID, RelationalOperators.Equal, (object) true, (object) null, LogicalOperators.NONE, 0, false, AttributeSourceTypes.Auto, ColumnContents.Text)
      }, new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) -2, SortOrders.NONE, 0)
      }, recordCount: 1);
      DataTable objectDataEx = DataHelper.GetObjectDataEx(-1, sessionKeeper.Session, dbRsp, scripts);
      if (objectDataEx != null)
      {
        if (objectDataEx.Rows.Count != 0)
          goto label_9;
      }
      return true;
    }
label_9:
    IDBTypedObjectID[] dbTypedObjectIdArray = (IDBTypedObjectID[]) Intermech.Navigator.SelectionWindow.Select(LocalizationHolder.rm.GetString("Reports_63"), (IDescriptor) new Intermech.Navigator.DBObjectTypes.Descriptor(MetaDataHelper.GetObjectTypeID("cad0011e-306c-11d8-b4e9-00304f19f545")), typeof (IDBTypedObjectID), SelectionOptions.Default);
    if (dbTypedObjectIdArray == null || dbTypedObjectIdArray.Length == 0)
      return false;
    archiveId = dbTypedObjectIdArray[0].ObjectID;
    return true;
  }

  /// <summary>Режим запуска задачи</summary>
  public ReportTaskMode TaskMode { get; set; }
}
