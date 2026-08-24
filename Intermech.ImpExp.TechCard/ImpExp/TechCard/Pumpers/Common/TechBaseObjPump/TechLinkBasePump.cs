// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.TechLinkBasePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

[TaskDescription("Инициализация данных для перекачки - базовый класс для установки связей", "Перекачка данных - базовый класс для установки связей")]
internal abstract class TechLinkBasePump : TechPumpBase
{
  protected ImportingCategory _categoryA;
  protected ImportingCategory _categoryB;
  protected string _fieldAName;
  protected string _fieldBName;
  protected Guid _relTypeGuid;
  private Dictionary<Guid, int> _relationGuidKeys = new Dictionary<Guid, int>();

  protected virtual bool CheckRecordByCache(TechObjectRecordBase recBase)
  {
    return this._import_data_main.GetNewKey(this.GetTechCategory(), (object) recBase.Key) != 0L;
  }

  protected virtual bool CheckRecord(TechObjectRecordBase record)
  {
    return Convert.ToInt32(record.Fields[this._fieldAName]) != 0 && Convert.ToInt32(record.Fields[this._fieldBName]) != 0;
  }

  protected override string GetRecordWithParamsPumpMode(TechObjectRecord record)
  {
    string withParamsPumpMode = base.GetRecordWithParamsPumpMode(record);
    if (record.RecMode == TechObjectRecord.PumpMode.NotPump || record.RecMode == TechObjectRecord.PumpMode.Unknown)
      return withParamsPumpMode;
    string uniqueRecordHash = this.GetUniqueRecordHash((TechObjectRecordBase) record);
    if (string.IsNullOrEmpty(uniqueRecordHash))
      return withParamsPumpMode;
    DictionaryValue dictionaryValue = this._import_data_main.GetValue(this.GetTechUniqueCategory(), (object) uniqueRecordHash);
    record.RecMode = dictionaryValue == null ? record.RecMode : TechObjectRecord.PumpMode.NotPump;
    return withParamsPumpMode;
  }

  protected virtual long ConvertFieldAValue(int fieldAValue) => (long) fieldAValue;

  protected virtual int ConvertFieldBValue(int fieldBValue) => fieldBValue;

  protected virtual string GetFieldNameA(TechObjectRecordBase record) => this._fieldAName;

  protected virtual string GetFieldNameB(TechObjectRecordBase record) => this._fieldBName;

  protected virtual ImportingCategory GetACategory(TechObjectRecordBase record) => this._categoryA;

  protected virtual ImportingCategory GetBCategory(TechObjectRecordBase record) => this._categoryB;

  protected virtual long GetNewKeyA(TechObjectRecordBase record, int imObjAId)
  {
    return ImportingDataHelper.Instance.GetNewKey(this._import_data_main, this.GetACategory(record), (object) this.ConvertFieldAValue(imObjAId));
  }

  protected virtual long GetNewKeyB(TechObjectRecordBase record, int imObjBId)
  {
    return ImportingDataHelper.Instance.GetNewKey(this._import_data_main, this.GetBCategory(record), (object) this.ConvertFieldBValue(imObjBId));
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[2]
    {
      this._categoryA,
      this._categoryB
    };
  }

  protected virtual RelationRecord CreateTechRel(
    long ipsObjectAId,
    long ipsObjectBId,
    int relTypeId)
  {
    return this._impRelList.AddRelation(ipsObjectAId, ipsObjectBId, relTypeId);
  }

  protected virtual void AddRelAtr(RelationRecord linkRel, TechObjectRecordBase record)
  {
    if (this._atTechTypeKeyAttr == null)
      return;
    this._impRelList.AddAttribute(this._atTechTypeKeyAttr.ID, (AttrValueType) this._atTechTypeKeyAttr.AttrValueType, (object) record.Key, 0);
  }

  protected virtual int CreateRelations(
    string pumpBegin,
    int relTypeId,
    int i,
    int recCount,
    TechObjectRecordBase record)
  {
    int int32_1 = Convert.ToInt32(record.Fields[this.GetFieldNameA(record)]);
    int int32_2 = Convert.ToInt32(record.Fields[this.GetFieldNameB(record)]);
    long newKeyA = this.GetNewKeyA(record, int32_1);
    long newKeyB = this.GetNewKeyB(record, int32_2);
    if (newKeyA == 0L || newKeyB == 0L)
      return i;
    RelationRecord techRel = this.CreateTechRel(newKeyA, newKeyB, relTypeId);
    if (techRel != null)
    {
      Guid prjLinkGuid = (Guid) techRel.PrjLinkGuid;
      this.AddRelAtr(techRel, record);
      this._relationGuidKeys.Add(prjLinkGuid, record.Key);
      this.FillLinkObligatoryAttributes();
    }
    if (techRel != null)
    {
      string uniqueRecordHash = this.GetUniqueRecordHash(record);
      if (!string.IsNullOrEmpty(uniqueRecordHash))
        this._import_data_main.AddValue(this.GetTechUniqueCategory(), (object) uniqueRecordHash, techRel.PrjLinkId, techRel.PrjLinkGuid.ToString());
    }
    ++i;
    if (i % this.CheckCount == 0 || i == recCount - 1)
      this.PumpCheckPoint($"{pumpBegin} ({i} из {recCount})", this.CalculatePercent(recCount, Math.Min(i, recCount), 0, 100));
    return i;
  }

  protected override void impRelList_AfterImportEvent(object sender, EventArgs e)
  {
    if (!(sender is IImportedRelationList importedRelationList))
      return;
    for (int index = 0; index < importedRelationList.Items.Count; ++index)
    {
      ImportingRelation importingRelation = importedRelationList.Items[index];
      if (importingRelation != null && importingRelation.Relation?.PrjLinkGuid != null)
      {
        Guid prjLinkGuid = (Guid) importingRelation.Relation.PrjLinkGuid;
        int oldKey;
        if (this._relationGuidKeys.TryGetValue(prjLinkGuid, out oldKey))
        {
          this._import_data_main.AddValue(this.GetTechCategory(), (object) oldKey, importingRelation.Relation.PrjLinkId);
          this._relationGuidKeys.Remove(prjLinkGuid);
        }
      }
    }
  }

  protected override void ClearTmpData()
  {
    base.ClearTmpData();
    this._relationGuidKeys.Clear();
    this._relationGuidKeys = (Dictionary<Guid, int>) null;
  }

  public TechLinkBasePump(PluginClass plugin)
    : base(plugin)
  {
  }

  public override void Exam()
  {
    string examDescription = this.ExamDescription;
    this.ExamCheckPoint(examDescription, 0);
    this.ExamCheckPoint(examDescription + " успешно завершена", 100);
  }

  public override void Pump()
  {
    this.LoadMetaData4Pump();
    this.LoadImportingCategoryData();
    if (TechCache.isResumeMode || this.IsMetadataPumper)
    {
      SavePoint savePoint = TechCache.SavePoint;
      this.LoadMetaData4StoppedPump();
      if (savePoint != null && savePoint.PumpGuid == this.GUID && !savePoint.RePumpMode)
        this.AnalyzeStoppedData();
    }
    string pumpDescription = this.PumpDescription;
    this.PumpCheckPoint(pumpDescription, 0);
    int id = this.plugin.Imdi.RelationTypes.GetByGuid(this._relTypeGuid).ID;
    this._impRelList = this.plugin.Idw.CreateImportedRelationList();
    this._impRelList.AfterImportEvent += new AfterImportEventDelegate(((TechPumpBase) this).impRelList_AfterImportEvent);
    TechDataSource dataSource = this.GetDataSource();
    try
    {
      int i = 0;
      TechDataReaderInfo dataReaderInfo = dataSource.GetDataReaderInfo(string.Empty);
      this.plugin.appManager.AddInfoMessage($"Количество записей источника данных: {dataReaderInfo.RecordCount}");
      TechObjectRecord tpObjRec1 = this.GetTpObjRec();
      IDataReader dataReader = dataReaderInfo.DataReader;
      Dictionary<string, int> tableColumns = this.GetTableColumns(dataReader);
      tpObjRec1.ParseSchema((IDictionary<string, int>) tableColumns);
      int recordCount = dataReaderInfo.RecordCount;
      while (dataReader.Read())
      {
        TechObjectRecord tpObjRec2 = this.GetTpObjRec();
        if (tpObjRec2 != null)
        {
          this.PumpLoadDataRec(dataReader, tpObjRec2);
          if (!this.CheckRecordByCache((TechObjectRecordBase) tpObjRec2))
          {
            string recordPumpMode = this.GetRecordPumpMode(tpObjRec2);
            if (tpObjRec2.RecMode == TechObjectRecord.PumpMode.Unknown || tpObjRec2.RecMode == TechObjectRecord.PumpMode.NotPump)
            {
              if (!recordPumpMode.Equals(string.Empty))
                this.AddMessageIfRecordNotPump(tpObjRec2, recordPumpMode);
            }
            else
            {
              string withParamsPumpMode = this.GetRecordWithParamsPumpMode(tpObjRec2);
              if (tpObjRec2.RecMode == TechObjectRecord.PumpMode.Unknown || tpObjRec2.RecMode == TechObjectRecord.PumpMode.NotPump)
              {
                if (!withParamsPumpMode.Equals(string.Empty))
                  this.AddMessageIfRecordNotPump(tpObjRec2, withParamsPumpMode);
              }
              else
                i = this.CreateRelations(pumpDescription, id, i, recordCount, (TechObjectRecordBase) tpObjRec2);
            }
          }
        }
      }
      dataReader.Close();
      if (this._impObjList != null)
        this._impObjList.Import();
      this._impRelList.Import();
    }
    finally
    {
      dataSource.Close();
      this.ReleasePumpData();
      this.PumpCheckPoint(pumpDescription + " завершена", 100);
    }
  }
}
