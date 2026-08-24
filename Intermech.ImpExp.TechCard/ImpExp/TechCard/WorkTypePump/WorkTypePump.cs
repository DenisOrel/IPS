// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.WorkTypePump.WorkTypePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.WorkTypePump;

[TaskDescription("Инициализация данных для перекачки - Виды работ", "Перекачка данных - Виды работ")]
[TaskType(PumperType.MetaData)]
internal class WorkTypePump(PluginClass plugin) : ImbaseObjectRecordMetaPump(plugin)
{
  private readonly Guid _guid = new Guid("{0A8DA8C4-992F-42cb-A404-2D0D5BFADF02}");
  private Dictionary<int, List<long>> _tcWork2ProdLinks = new Dictionary<int, List<long>>();
  private int _otProdObjectTypeId = -1;
  protected IAttributeTypeItem _atCodWorkTypeAttr;
  protected IAttributeTypeItem _atShortNaimAttrType;
  protected IAttributeTypeItem _atShortNameWorkVidAttr;

  protected override void InitData()
  {
    this._sortFieldName = "F_OWNER";
    this._recType = "Вид работ";
    this._tableName = string.Empty;
    this._dopTypes.Add("REC");
  }

  protected override void LoadMetaData4Pump()
  {
    base.LoadMetaData4Pump();
    IObjectTypeItem byGuid1 = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otWorksVidObjTypeGuid);
    if (byGuid1 != null)
      this.objTypeID = byGuid1.ID;
    else
      this.objTypeID = -1;
    IObjectTypeItem byGuid2 = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otProductionObjTypeGuid);
    if (byGuid2 != null)
      this._otProdObjectTypeId = byGuid2.ID;
    IAttributeTypeItem byGuid3 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atCodWorkTypeAttrGuid);
    if (byGuid3 != null)
      this._atCodWorkTypeAttr = byGuid3;
    IAttributeTypeItem byGuid4 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atShortNaimAttrTypeGuid);
    if (byGuid4 != null)
      this._atShortNaimAttrType = byGuid4;
    IAttributeTypeItem byGuid5 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atShortNameWorkVidAttrGuid);
    if (byGuid5 != null)
      this._atShortNameWorkVidAttr = byGuid5;
    IAttributeTypeItem byGuid6 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atNaimAttrTypeGuid);
    if (byGuid6 != null)
      this._atNaimAttrType = byGuid6;
    IAttributeTypeItem byGuid7 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atObozAttrTypeGuid);
    if (byGuid7 != null)
      this._atObozAttrType = byGuid7;
    IAttributeTypeItem byGuid8 = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atLastLevelSeek);
    if (byGuid8 == null)
      return;
    this._atLastLevelSeek = byGuid8;
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechWorkTypes;

  public void LoadLinksDopInfo()
  {
    try
    {
      IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
      command.CommandText = "select * from TC_WORK2PROD_LINKS";
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal1 = dataReader.GetOrdinal("F_PRODUCTION");
        int ordinal2 = dataReader.GetOrdinal("F_WORKCODE");
        while (dataReader.Read())
        {
          IpsProductionObj ipsProductionObj;
          if (TechPumpData.Production.Productions.TryGetValue(BasePumpHelper.ToInt32(dataReader[ordinal1]), out ipsProductionObj))
          {
            long objId = ipsProductionObj.ObjID;
            int int32 = BasePumpHelper.ToInt32(dataReader[ordinal2]);
            List<long> longList;
            if (!this._tcWork2ProdLinks.TryGetValue(int32, out longList))
            {
              longList = new List<long>();
              this._tcWork2ProdLinks.Add(int32, longList);
            }
            if (!longList.Contains(objId))
              longList.Add(objId);
          }
        }
        dataReader.Close();
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка чтения информации о связи выдов работ с видами производства: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new ImbaseObjectRecordDynamic(this._tableName);
  }

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    this._tableName = TechPumpData.Tables.ImTablesData.GetTableName(TechcardConsts.imTablesConsts.WorkType);
    if (this._tableName == string.Empty)
    {
      this.plugin.appManager.AddErrorMessage("Таблица справочника \"Виды работ\" не найдена. Данные не могут быть импортированы!");
    }
    else
    {
      this.LoadLinksDopInfo();
      base.Pump();
    }
  }

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    ITechParamBase entity = recParmList != null ? (ITechParamBase) recParmList.GetEntity("%1ВР") : (ITechParamBase) null;
    string caption = entity == null || entity.Value == null ? Convert.ToString(recBase.Fields["F_NAME"]) : entity.Value.ToString();
    this._import_data_main.AddValue(this.GetTechCategory(), oldKey, newKey, caption);
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    string str = Convert.ToString(record.Fields["F_NAME"]);
    if (this._atNaimAttrType != null)
      this._techParmList.AddAttribute(this._atNaimAttrType, (object) str);
    if (this._atObozAttrType != null)
      this._techParmList.AddAttribute(this._atObozAttrType, (object) str);
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    object obj = this._techParmList.GetEntity("%1ВР")?.Value;
    if (obj != null && this._atCodWorkTypeAttr != null)
      this._techParmList.AddAttribute(this._atCodWorkTypeAttr, obj);
    if (obj != null && this._atShortNaimAttrType != null)
      this._techParmList.AddAttribute(this._atShortNaimAttrType, obj);
    if (obj != null && this._atShortNameWorkVidAttr != null)
      this._techParmList.AddAttribute(this._atShortNameWorkVidAttr, obj);
    objRecord.Caption = (obj != null ? obj.ToString() : str).Truncate(Consts.MaxStringSize - 2);
    base.FillTechObject(objRecord, record);
  }

  public override void FillObjectParams(
    TechObjectRecord record,
    TechParamList parmList,
    ObjectRecord objectRec)
  {
    base.FillObjectParams(record, parmList, objectRec);
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> techRelList = new List<TechRelParam>();
    List<long> longList;
    if (!this._tcWork2ProdLinks.TryGetValue(recBase.Key, out longList) || longList == null)
      return techRelList;
    foreach (long ipsObjectB in longList)
    {
      if (ipsObjectB != 0L)
      {
        TechRelParam techRelParam = new TechRelParam(ipsObjectB, ipsObjId, this._relTechRelationID, this._otProdObjectTypeId, this.objTypeID);
        techRelList.Add(techRelParam);
      }
    }
    return techRelList;
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._tcWork2ProdLinks.Clear();
    this._tcWork2ProdLinks = (Dictionary<int, List<long>>) null;
  }

  protected override Guid GUID => this._guid;
}
