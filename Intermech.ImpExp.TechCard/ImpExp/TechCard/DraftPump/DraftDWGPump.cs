// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.DraftPump.DraftDWGPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.DraftPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using Intermech.TechAcad.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.TechCard.DraftPump;

[TaskDescription("Инициализация данных для перекачки - Эскизы AutoCad", "Перекачка данных - Эскизы AutoCad")]
internal class DraftDWGPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private readonly Guid _guid = new Guid("{BC5D578C-597A-4322-96C4-4E025BF5A9B3}");
  private Dictionary<int, List<int>> _pict2PerehInfoList = new Dictionary<int, List<int>>();
  private Dictionary<int, List<int>> _pict2ArtInfoList = new Dictionary<int, List<int>>();
  private Dictionary<int, int> _oper2DraftIdList = new Dictionary<int, int>();
  private int _currentTcKey;
  protected int _rtTechDraftRelationId;
  private int _atSketchNameTypeId;
  private int _atSketchListIdGuidAttrId;
  protected Dictionary<int, List<TechObjectRecordBase>> _picList4Tp = new Dictionary<int, List<TechObjectRecordBase>>();

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TP_PIC");
  }

  protected override TechDataSource GetDataSource()
  {
    return this._dataSource ?? (this._dataSource = new TechDataSource((ITechDataBuilder) new DraftDwgDataBuilder<TechPumpBase>((TechPumpBase) this)));
  }

  protected string GetPumpModeCond(string condField, string dopType)
  {
    return TechDataBuilder<PumpClass>.GetPumpModeCond(condField, -2);
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    int int32_1 = Convert.ToInt32(record.Fields["F_TCKEY"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_COUNT"]);
    if (this._import_data_main.GetValue(ImportingCategory.TechProcessPump, (object) int32_1) == null)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (this._currentTcKey != int32_1)
      this._currentTcKey = int32_1;
    List<TechObjectRecordBase> objectRecordBaseList;
    if (!this._picList4Tp.TryGetValue(this._currentTcKey, out objectRecordBaseList))
    {
      objectRecordBaseList = new List<TechObjectRecordBase>();
      this._picList4Tp.Add(this._currentTcKey, objectRecordBaseList);
    }
    int count = objectRecordBaseList.Count;
    objectRecordBaseList.Add((TechObjectRecordBase) record);
    record.SetFieldValue("F_ORDER", (object) count);
    if (this._picList4Tp[this._currentTcKey].Count >= int32_2)
    {
      record.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
      return string.Empty;
    }
    record.RecMode = TechObjectRecord.PumpMode.NotPump;
    return string.Empty;
  }

  protected override void CheckBaseRecords()
  {
  }

  private Dictionary<int, int> Load_Oper2DraftIDInfo()
  {
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    string str = string.Empty;
    try
    {
      Entity entity;
      if (!TechPumpData.Entities.EntitiesList.TryGetValue("A173", out entity) || entity == null)
        this.plugin.appManager.AddErrorMessage($"Код {"A173"} не найден в списке понятий.");
      if (entity != null && entity.Tag > 0)
      {
        int num = entity.Tag % 10 + 1;
        str = $"SELECT DISTINCT\r\n                                                {"F_PARENTKEY"}, \r\n                                                {"F" + (object) (entity.Tag / 10)} AS {"F_VALUE"}  \r\n                                              FROM \r\n                                                {"TP_OPER"}_I \r\n                                              WHERE \r\n                                                {"F_ROW"} = {num}";
      }
      else
        str = $"SELECT DISTINCT\r\n                                                {"F_PARENTKEY"}, \r\n                                                {"F_VALUE"} \r\n                                              FROM \r\n                                                {"TP_OPER"}_D \r\n                                              WHERE \r\n                                                {"F_ENTITY"} = '{"A173"}'";
      string pumpModeCond = this.GetPumpModeCond("F_TCKEY", string.Empty);
      if (pumpModeCond != string.Empty)
        str = $"{str} AND {pumpModeCond}";
      using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
      {
        command.CommandText = str;
        using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
        {
          int ordinal1 = dataReader.GetOrdinal("F_PARENTKEY");
          int ordinal2 = dataReader.GetOrdinal("F_VALUE");
          while (dataReader.Read())
          {
            int int32_1 = dataReader.IsDBNull(ordinal1) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal1]);
            if (int32_1 != 0)
            {
              int int32_2 = Convert.ToInt32(dataReader.IsDBNull(ordinal2) ? string.Empty : dataReader.GetString(ordinal2));
              try
              {
                dictionary[int32_1] = int32_2;
              }
              catch
              {
              }
            }
          }
          dataReader.Close();
        }
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка при выполнении запроса '{str}': {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    return dictionary;
  }

  protected virtual Dictionary<int, List<int>> Load_Pict2PerehodLinksInfo()
  {
    Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
    try
    {
      using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
      {
        string str1 = string.Empty;
        if (this._lastObjID != 0L)
          str1 = $" WHERE {"F_PICTKEY"} > {this._lastObjID}";
        string pumpModeCond = this.GetPumpModeCond("F_TCKEY", string.Empty);
        if (pumpModeCond != string.Empty)
        {
          string str2 = $"{"F_PICTKEY"} IN (         SELECT {"F_KEY"}     FROM   {"TP_PIC"}     WHERE  {pumpModeCond}           )      ";
          str1 = str1 != string.Empty ? $"{str1} AND {str2}" : " WHERE " + str2;
        }
        command.CommandText = string.Format("SELECT \r\n                                                        * \r\n                                                      FROM \r\n                                                        {0} \r\n                                                        {2} \r\n                                                      ORDER BY \r\n                                                        {1}", (object) "TP_PIC_PER", (object) "F_PICTKEY", (object) str1);
        using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
        {
          int ordinal1 = dataReader.GetOrdinal("F_PICTKEY");
          int ordinal2 = dataReader.GetOrdinal("F_PEREHKEY");
          while (dataReader.Read())
          {
            int int32_1 = dataReader.IsDBNull(ordinal1) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal1]);
            int int32_2 = dataReader.IsDBNull(ordinal2) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal2]);
            if (int32_1 != 0)
            {
              List<int> intList;
              if (!dictionary.TryGetValue(int32_1, out intList))
              {
                intList = new List<int>();
                dictionary.Add(int32_1, intList);
              }
              intList.Add(int32_2);
            }
          }
          dataReader.Close();
        }
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage(string.Format("Невозможно прочитать информацию о связях эскизов AutoCad c переходами(таблица {1}): {0}", (object) ex.Message, (object) "TP_PIC_PER"));
      if (ex is OutOfMemoryException)
        throw;
    }
    return dictionary;
  }

  protected virtual Dictionary<int, List<int>> Load_Pict2ArtLinksInfo()
  {
    Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
    try
    {
      using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
      {
        string str1 = string.Empty;
        if (this._lastObjID != 0L)
          str1 = $" WHERE {"F_PICTKEY"} > {this._lastObjID}";
        string pumpModeCond = this.GetPumpModeCond("F_TCKEY", string.Empty);
        if (pumpModeCond != string.Empty)
        {
          string str2 = $"{"F_PICTKEY"} IN (         SELECT {"F_KEY"}     FROM   {"TP_PIC"}     WHERE  {pumpModeCond}           )      ";
          str1 = str1 != string.Empty ? $"{str1} AND {str2}" : " WHERE " + str2;
        }
        command.CommandText = string.Format("SELECT \r\n                                                        * \r\n                                                      FROM \r\n                                                        {0} \r\n                                                        {2} \r\n                                                      ORDER BY \r\n                                                        {1}", (object) "TP_PIC_ART", (object) "F_PICTKEY", (object) str1);
        using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
        {
          int ordinal1 = dataReader.GetOrdinal("F_PICTKEY");
          int ordinal2 = dataReader.GetOrdinal("F_ARTKEY");
          while (dataReader.Read())
          {
            int int32_1 = dataReader.IsDBNull(ordinal1) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal1]);
            int int32_2 = dataReader.IsDBNull(ordinal2) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal2]);
            if (int32_1 != 0)
            {
              List<int> intList;
              if (!dictionary.TryGetValue(int32_1, out intList))
              {
                intList = new List<int>();
                dictionary.Add(int32_1, intList);
              }
              intList.Add(int32_2);
            }
          }
          dataReader.Close();
        }
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage(string.Format("Невозможно прочитать информацию о связях эскизов AutoCad c изделиями (таблица {1}): {0}", (object) ex.Message, (object) "TP_PIC_ART"));
      if (ex is OutOfMemoryException)
        throw;
    }
    return dictionary;
  }

  protected void FillRelationAttributes(TechObjectRecordBase rec, long ipsDwgObjectId)
  {
    int int32_1 = Convert.ToInt32(rec.Fields["F_OPERKEY"]);
    int int32_2 = Convert.ToInt32(rec.Fields["F_NUMBER"]);
    int int32_3 = Convert.ToInt32(rec.Fields["F_ORDER"]);
    this.AddLinkAttribute(Convert.ToString(rec.Fields["F_NAME"]), ipsDwgObjectId, int32_3, int32_1, int32_2);
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> techRelList = new List<TechRelParam>();
    int int32_1 = Convert.ToInt32(recBase.Fields["F_TCKEY"]);
    List<TechObjectRecordBase> objectRecordBaseList;
    if (!this._picList4Tp.TryGetValue(int32_1, out objectRecordBaseList))
      return techRelList;
    foreach (TechObjectRecordBase objectRecordBase in objectRecordBaseList)
    {
      int int32_2 = Convert.ToInt32(objectRecordBase.Fields["F_OPERKEY"]);
      if (int32_2 != 0)
      {
        DictionaryValue dictValue = this._import_data_main.GetValue(ImportingCategory.TechOperation, (object) int32_2);
        if (dictValue != null)
        {
          RelationRecord relationRecord1 = this._impRelList.AddRelation(dictValue.NewObjectID, ipsObjId, this._rtTechDraftRelationId);
          this.FillRelationAttributes(objectRecordBase, ipsObjId);
          this.FillLinkSortParam(new TechRelParam(dictValue.NewObjectID, ipsObjId, this._rtTechDraftRelationId, this._otOperTypeID, this.objTypeID)
          {
            RelRec = relationRecord1
          }, objectRecordBase);
          this.FillLinkObligatoryAttributes();
          List<int> intList;
          if (this._pict2ArtInfoList.TryGetValue(objectRecordBase.Key, out intList))
          {
            TechDiffTag diffTag = TechDiffTag.GetDiffTag(dictValue);
            if (diffTag != null && !diffTag.IsCloneListEmpty)
            {
              foreach (int key in intList)
              {
                long num;
                if (diffTag.CloneList.TryGetValue(key, out num) && num != 0L)
                {
                  RelationRecord relationRecord2 = this._impRelList.AddRelation(num, ipsObjId, this._rtTechDraftRelationId);
                  this.FillRelationAttributes(objectRecordBase, ipsObjId);
                  this.FillLinkSortParam(new TechRelParam(num, ipsObjId, this._rtTechDraftRelationId, this._otOperTypeID, this.objTypeID)
                  {
                    RelRec = relationRecord2
                  }, objectRecordBase);
                  if ((Guid) relationRecord1.PrjLinkGuid != Guid.Empty)
                    this._impRelList.AddAttributeStr(this._atTechLinkAtRelGTPRelation.ID, relationRecord1.PrjLinkGuid.ToString());
                  this.FillLinkObligatoryAttributes();
                }
              }
            }
          }
        }
        else
          this.plugin.appManager.AddWarningMessage($"Операция с F_KEY='{int32_2}' не найдена в кэше закаченных объектов");
        List<int> intList1;
        if (this._pict2PerehInfoList.TryGetValue(objectRecordBase.Key, out intList1))
        {
          foreach (int key in intList1)
          {
            long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechPerehPump, (object) key);
            if (newKey != 0L)
            {
              RelationRecord relationRecord = this._impRelList.AddRelation(newKey, ipsObjId, this._rtTechDraftRelationId);
              this.FillRelationAttributes(objectRecordBase, ipsObjId);
              this.FillLinkSortParam(new TechRelParam(newKey, ipsObjId, this._rtTechDraftRelationId, this._otPerehTypeID, this.objTypeID)
              {
                RelRec = relationRecord
              }, objectRecordBase);
              this.FillLinkObligatoryAttributes();
            }
            else
              this.plugin.appManager.AddWarningMessage($"Переход с F_KEY='{key}' не найден в кэше закаченных объектов");
          }
        }
      }
    }
    int objTypeId = TechPumpData.TechType.TechTypeList.GetObjTypeId(TechcardConsts.TpRecordType.Passport);
    long newKey1 = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechProcessPump, (object) int32_1);
    if (newKey1 != 0L)
    {
      RelationRecord relationRecord = this._impRelList.AddRelation(newKey1, ipsObjId, this._relTechRelationID);
      this.FillLinkSortParam(new TechRelParam(newKey1, ipsObjId, this._relTechRelationID, objTypeId, this.objTypeID)
      {
        RelRec = relationRecord
      }, recBase);
      this.FillLinkObligatoryAttributes();
    }
    TechDiffTag techDiffTagByOldKey = this.GetTechDiffTagByOldKey(ImportingCategory.TechProcessPump, (object) int32_1);
    if (techDiffTagByOldKey != null && !techDiffTagByOldKey.IsCloneListEmpty)
    {
      foreach (long num in techDiffTagByOldKey.CloneList.Values)
      {
        RelationRecord relationRecord = this._impRelList.AddRelation(num, ipsObjId, this._relTechRelationID);
        this.FillLinkSortParam(new TechRelParam(num, ipsObjId, this._relTechRelationID, objTypeId, this.objTypeID)
        {
          RelRec = relationRecord
        }, recBase);
        this.FillLinkObligatoryAttributes();
      }
    }
    this._picList4Tp.Remove(int32_1);
    return techRelList;
  }

  protected override ObjectRecord CreateTechObject(TechObjectRecord record)
  {
    int int32_1 = Convert.ToInt32(record.Fields["F_DOCID"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_VERSION"]);
    long objectID = 0;
    TechDraftTag tag = (TechDraftTag) this._import_data_main.GetTag(ImportingCategory.TechDrafts, (object) TechcardConsts.Utils.CodeHashCode(int32_1, int32_2));
    if (tag?.Drafts != null)
    {
      using (Dictionary<string, long>.Enumerator enumerator = tag.Drafts.GetEnumerator())
      {
        if (enumerator.MoveNext())
          objectID = enumerator.Current.Value;
      }
    }
    else
      this.plugin.appManager.AddWarningMessage($"Для ТП (F_DOCID='{int32_1}', F_VERSION = '{int32_2}')  не найден эскиз в кэше закаченных объектов");
    if (objectID == 0L)
      return (ObjectRecord) null;
    try
    {
      this._impObjList.UseObject(objectID);
      int currentIndex = this._impObjList.Items.CurrentIndex;
      this._techBaseImportList.Add((TechObjectRecordBase) record, currentIndex);
      this.FillTechObject((ObjectRecord) null, record);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Невозможно модифицировать существующий объект ТП \"{objectID}\" по причине: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
      this.DoHandleImportObjectsException(ex);
      return (ObjectRecord) null;
    }
    return new ObjectRecord()
    {
      ObjectGuid = (object) Guid.Empty,
      Object_id = objectID
    };
  }

  protected void AddLinkAttribute(
    string name,
    long ipsDwgObjectId,
    int order,
    int operId,
    int number)
  {
    XmlDocument doc = new XmlDocument();
    XmlNode xml4OneSketch = this.GetXml4OneSketch(doc, name, ipsDwgObjectId, order, operId, number);
    if (xml4OneSketch != null)
      doc.AppendChild(xml4OneSketch);
    string tmpFileName = this.GetTmpFileName();
    doc.Save(tmpFileName);
    FileInfo fileInfo = new FileInfo(tmpFileName);
    if (fileInfo.Exists)
      this._impRelList.AddAttributeBlob(this._atSketchListIdGuidAttrId, tmpFileName, fileInfo.Length, "TechAcadSketchLinkList.xml", ArcMethods.NotPacked);
    this._impRelList.AddAttributeStr(this._atSketchNameTypeId, this.GetSketchName(operId, number));
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    this.AddSketchList2ObjectAttribute();
  }

  protected void AddSketchList2ObjectAttribute()
  {
    XmlDocument doc = new XmlDocument();
    if (!this.GetSketchListXml(doc))
      return;
    string tmpFileName = this.GetTmpFileName();
    doc.Save(tmpFileName);
    FileInfo fileInfo = new FileInfo(tmpFileName);
    if (!fileInfo.Exists)
      return;
    this._impObjList.AddAttributeBlob(this._atSketchListIdGuidAttrId, tmpFileName, fileInfo.Length, "TechAcadSketchObjectList.xml", ArcMethods.NotPacked);
  }

  protected TechAcadSketchObject GetOneSketch(
    string name,
    long ipsDwgObjectId,
    int order,
    int operId,
    int number)
  {
    return new TechAcadSketchObject((ISketchObject) null)
    {
      Name = name,
      SketchID = this.GetSketchName(operId, number),
      OrderID = (long) order,
      Status = ChangeStatus.Added
    };
  }

  private string GetSketchName(int operId, int number)
  {
    int num;
    this._oper2DraftIdList.TryGetValue(operId, out num);
    return $"OPR_{num:D}_{number:D}";
  }

  protected XmlNode GetXml4OneSketch(
    XmlDocument doc,
    string name,
    long ipsDwgObjectId,
    int order,
    int operId,
    int number)
  {
    TechAcadSketchObject oneSketch = this.GetOneSketch(name, ipsDwgObjectId, order, operId, number);
    new TechAcadSketchObjectList() { Items = { oneSketch } }.SaveSketchCollection(doc);
    return (XmlNode) null;
  }

  protected bool GetSketchListXml(XmlDocument doc)
  {
    TechAcadSketchObjectList sketchObjectList = new TechAcadSketchObjectList();
    long ipsDwgObjectId = 0;
    foreach (TechObjectRecordBase objectRecordBase in this._picList4Tp[this._currentTcKey])
    {
      int int32_1 = Convert.ToInt32(objectRecordBase.Fields["F_ORDER"]);
      string name = Convert.ToString(objectRecordBase.Fields["F_NAME"]);
      int int32_2 = Convert.ToInt32(objectRecordBase.Fields["F_DOCID"]);
      int int32_3 = Convert.ToInt32(objectRecordBase.Fields["F_OPERKEY"]);
      int int32_4 = Convert.ToInt32(objectRecordBase.Fields["F_NUMBER"]);
      int int32_5 = Convert.ToInt32(objectRecordBase.Fields["F_VERSION"]);
      if (ipsDwgObjectId == 0L)
      {
        TechDraftTag tag = (TechDraftTag) this._import_data_main.GetTag(ImportingCategory.TechDrafts, (object) TechcardConsts.Utils.CodeHashCode(int32_2, int32_5));
        if (tag != null && tag.Drafts != null)
        {
          using (Dictionary<string, long>.Enumerator enumerator = tag.Drafts.GetEnumerator())
          {
            if (enumerator.MoveNext())
              ipsDwgObjectId = enumerator.Current.Value;
          }
        }
        else
          this.plugin.appManager.AddErrorMessage($"Зскиз для ТП (F_DOCID = {int32_2}, F_VERSION = {int32_5}) не найден в списке закаченных объектов.");
      }
      if (ipsDwgObjectId == 0L)
        return false;
      sketchObjectList.Items.Add(this.GetOneSketch(name, ipsDwgObjectId, int32_1, int32_3, int32_4));
    }
    sketchObjectList.SaveSketchCollection(doc);
    return true;
  }

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "Эскизы AutoCad";
    this._tableName = "TP_PIC";
    if (!this.plugin.Imdi.ObjectTypes.ExistsByGuid(TechCardConsts.ObjectTypes.DraftCadmechGUID))
      return;
    this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.DraftCadmechGUID).ID;
  }

  protected override void LoadMetaData4Pump()
  {
    IAttributeTypeItem byGuid1 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.SketchNameGuid);
    if (byGuid1 != null)
      this._atSketchNameTypeId = byGuid1.ID;
    IAttributeTypeItem byGuid2 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechCardConsts.AttributeTypes.SketchListGUIDAttrGuid);
    if (byGuid2 != null)
      this._atSketchListIdGuidAttrId = byGuid2.ID;
    IRelationTypeItem byGuid3 = this.plugin.Imdi.RelationTypes.GetByGuid(TechCardConsts.RelTypes.TechDraftRelationGuid);
    if (byGuid3 != null)
      this._rtTechDraftRelationId = byGuid3.ID;
    base.LoadMetaData4Pump();
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechDraftDWG;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[4]
    {
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechOperation,
      ImportingCategory.TechDrafts,
      ImportingCategory.TechPerehPump
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.TechDrafts
    };
  }

  public override void Exam() => base.Exam();

  protected override void PumpLoadData()
  {
    this._pict2PerehInfoList = this.Load_Pict2PerehodLinksInfo();
    this._oper2DraftIdList = this.Load_Oper2DraftIDInfo();
    this._pict2ArtInfoList = this.Load_Pict2ArtLinksInfo();
    base.PumpLoadData();
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._picList4Tp.Clear();
    this._picList4Tp = (Dictionary<int, List<TechObjectRecordBase>>) null;
    this._pict2PerehInfoList.Clear();
    this._pict2PerehInfoList = (Dictionary<int, List<int>>) null;
    this._pict2ArtInfoList.Clear();
    this._pict2ArtInfoList = (Dictionary<int, List<int>>) null;
    this._oper2DraftIdList.Clear();
    this._oper2DraftIdList = (Dictionary<int, int>) null;
  }
}
