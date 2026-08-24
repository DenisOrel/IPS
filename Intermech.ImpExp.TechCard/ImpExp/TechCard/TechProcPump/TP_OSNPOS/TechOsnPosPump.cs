// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_OSNPOS.TechOsnPosPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_OSNPOS;

[TaskDescription("Инициализация данных для перекачки - Позиции", "Перекачка данных - Позиции")]
internal class TechOsnPosPump(PluginClass plugin) : TechPumpBase(plugin)
{
  protected Guid _guid = new Guid("{1231F47B-255D-4D19-8522-C542D345D451}");
  private Dictionary<int, List<int>> _tpList = new Dictionary<int, List<int>>();
  protected Dictionary<int, List<int>> _osnPosDopInfo = new Dictionary<int, List<int>>();
  private int _lastDocTcKey;
  private List<string> _uniqueKeys = new List<string>();
  protected int _otTechInstrumentObjTypeID = -1;
  protected IAttributeTypeItem _atTechInstrumPosNom;

  protected override void InitData()
  {
    if (this.plugin.Imdi.ObjectTypes.ExistsByGuid(TechcardConsts.TypeConsts.otInstrumentalPositionObjTypeGuid))
      this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otInstrumentalPositionObjTypeGuid).ID;
    this._sortFieldName = "F_ORDER";
    this._recType = "L";
    this._recTypeID = 11;
    this._tableName = "TP_OSNPOS";
  }

  protected override void LoadMetaData4Pump()
  {
    IMetadataInfo imdi = this.plugin.Imdi;
    if (imdi == null)
    {
      this.plugin.appManager.AddErrorMessage("Ошибка получения кэша метаданных");
    }
    else
    {
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechInstrumentObjTypeGuid);
      if (byGuid1 != null)
        this._otTechInstrumentObjTypeID = byGuid1.ID;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechInstrumPosNomGuid);
      if (byGuid2 != null)
        this._atTechInstrumPosNom = byGuid2;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechOsnPos;

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[3]
    {
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump,
      ImportingCategory.TechOsnPosUnicalKeys
    };
  }

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    int int32 = Convert.ToInt32(recBase.Fields["F_INSTRUM"]);
    string techToolParentKey = TechOsnPosPump.GenerateTechToolParentKey(recBase);
    if (!techToolParentKey.Equals(string.Empty))
    {
      try
      {
        string caption = int32 > 0 ? int32.ToString() : string.Empty;
        if (this._import_data_main.GetValue(ImportingCategory.TechOsnPosUnicalKeys, (object) techToolParentKey) == null)
          this._import_data_main.AddValue(ImportingCategory.TechOsnPosUnicalKeys, (object) techToolParentKey, newKey, caption);
      }
      catch (Exception ex)
      {
        if (!(ex is ArgumentException))
          throw;
      }
    }
    base.AddValue2Cache(oldKey, newKey, recBase, recParmList);
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? "F_DOCTCKEY" : "F_TCKEY", -2));
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  private void LoadDopInfo()
  {
    try
    {
      string str = "";
      string pumpModeCond = TechDataBuilder<PumpClass>.GetPumpModeCond("F_OBJ_KEY", -2);
      if (pumpModeCond != string.Empty)
        str = $"{str} AND {pumpModeCond}";
      IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
      command.CommandText = $"SELECT DISTINCT \r\n                                                    {"F_OBJ_KEY"}, \r\n                                                    {"F_ART_TCKEY"} \r\n                                                  FROM \r\n                                                    {"TC_OBJ2LINK"} \r\n                                                  WHERE \r\n                                                    {"F_OBJ_TYPE"} = {1} \r\n                                                    {str}";
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal1 = dataReader.GetOrdinal("F_OBJ_KEY");
        int ordinal2 = dataReader.GetOrdinal("F_ART_TCKEY");
        while (dataReader.Read())
        {
          int int32_1 = BasePumpHelper.ToInt32(dataReader[ordinal1]);
          int int32_2 = BasePumpHelper.ToInt32(dataReader[ordinal2]);
          if (int32_1 != 0)
          {
            if (this._tpList.ContainsKey(int32_1))
            {
              this._tpList[int32_1].Add(int32_2);
            }
            else
            {
              List<int> intList = new List<int>()
              {
                int32_2
              };
              this._tpList.Add(int32_1, intList);
            }
          }
        }
        dataReader.Close();
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddErrorMessage($"Невозможно загрузить инфорамцию о техпроцессах: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32_1 = Convert.ToInt32(record.Fields["F_POS"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_INSTRUM"]);
    objRecord.Caption = (int32_2 != 0 ? $"Инструмент {int32_2}" : $"Позиция {int32_1}").Truncate(Consts.MaxStringSize - 2);
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    if (int32_2 == 0 && this._atTechInstrumPosNom != null)
      this._techParmList.AddAttribute(this._atTechInstrumPosNom, (object) int32_1);
    base.FillTechObject(objRecord, record);
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TP_OSNPOS");
  }

  protected override int GetObjectType(TechObjectRecordBase record)
  {
    return Convert.ToInt32(record.Fields["F_INSTRUM"]) != 0 ? this._otTechInstrumentObjTypeID : this.objTypeID;
  }

  protected override ObjectRecord CreateTechObject(TechObjectRecord record)
  {
    return base.CreateTechObject(record);
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> techRelList = new List<TechRelParam>();
    int int32_1 = Convert.ToInt32(recBase.Fields["F_OPERKEY"]);
    int int32_2 = Convert.ToInt32(recBase.Fields["F_PEREHKEY"]);
    int relTechRelationId = this._relTechRelationID;
    int objectType = this.GetObjectType(recBase);
    int int32_3 = Convert.ToInt32(recBase.Fields["F_POS"]);
    int int32_4 = Convert.ToInt32(recBase.Fields["F_INSTRUM"]);
    if (int32_3 == 0 || int32_4 == 0)
    {
      int num = 0;
      int ipsObjTypeB = -1;
      ImportingCategory category = ImportingCategory.None;
      if (int32_1 == 0 && int32_2 != 0)
      {
        num = int32_2;
        ipsObjTypeB = this._otPerehTypeID;
        category = ImportingCategory.TechPerehPump;
      }
      else if (int32_1 != 0)
      {
        num = int32_1;
        ipsObjTypeB = this._otOperTypeID;
        category = ImportingCategory.TechOperation;
      }
      if (num != 0)
      {
        if (this.IsCloneRecord(recBase))
        {
          TechRelParam techRelParam = this.AddRelationByObject(category, (object) num, relTechRelationId, recBase, ipsObjId, ipsObjTypeB, objectType);
          if (techRelParam != null)
            techRelList.Add(techRelParam);
        }
        else
        {
          long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, category, (object) num);
          if (newKey != 0L)
          {
            TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, ipsObjTypeB, objectType);
            techRelList.Add(techRelParam);
          }
        }
      }
    }
    return techRelList;
  }

  protected override void PumpBaseRec(TechObjectRecord record)
  {
    if (this.CheckRecordLessThenLastKey(record))
      return;
    try
    {
      int int32 = Convert.ToInt32(record.Fields["F_DOCTCKEY"]);
      if (this._lastDocTcKey == int32)
      {
        string techToolParentKey = TechOsnPosPump.GenerateTechToolParentKey((TechObjectRecordBase) record);
        if (this._uniqueKeys.Contains(techToolParentKey))
          return;
        this._uniqueKeys.Add(techToolParentKey);
      }
      else
      {
        this._uniqueKeys.Clear();
        this._lastDocTcKey = int32;
      }
      base.PumpBaseRec(record);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка обработки записи \"{record.Key}\" таблицы \"{record.TableName}\": {ex.Message}{Environment.NewLine + ex.StackTrace}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  protected override void PumpDiffRec(TechObjectRecord record)
  {
    int int32 = Convert.ToInt32(record.Fields["F_DOCTCKEY"]);
    List<int> intList1 = new List<int>();
    List<int> intList2;
    if (!this._tpList.TryGetValue(int32, out intList2))
      return;
    foreach (int artId in intList2)
    {
      if (!intList1.Contains(artId) && this.FindParentIpsObject(artId, (TechObjectRecordBase) record))
      {
        TechObjectRecord cloneRecord = this.CreateCloneRecord(record);
        cloneRecord.Key = -this.GetUnicalDiffRecKey(artId, record.Key);
        cloneRecord.diff_ArtTcKey = artId;
        this._techParmList = new TechParamList();
        this.LoadTechParams(cloneRecord);
        this.PumpBaseRec(cloneRecord);
        intList1.Add(artId);
      }
    }
  }

  private bool FindParentIpsObject(int artId, TechObjectRecordBase record)
  {
    if (artId == 0)
      return false;
    int int32_1 = Convert.ToInt32(record.Fields["F_OPERKEY"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_PEREHKEY"]);
    TechDiffTag diffTag = TechDiffTag.GetDiffTag(int32_2 == 0 ? this._import_data_main.GetValue(ImportingCategory.TechOperation, (object) int32_1) : this._import_data_main.GetValue(ImportingCategory.TechPerehPump, (object) int32_2));
    return diffTag != null && diffTag.IsCloneListEmpty && diffTag.CloneList.ContainsKey(artId);
  }

  private int GetUnicalDiffRecKey(int artId, int recId)
  {
    return Convert.ToInt32(TechcardConsts.Utils.CodeHashCode(artId, recId) % (long) (int.MaxValue / this.CheckCount));
  }

  protected override void AddValue4Clone2Cache(
    IImportingData masterImport,
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase)
  {
  }

  protected override void PumpLoadTechDiffData() => this.LoadDopInfo();

  protected override void ClearTmpData()
  {
    this._tpList.Clear();
    this._tpList = (Dictionary<int, List<int>>) null;
    this._osnPosDopInfo.Clear();
    this._osnPosDopInfo = (Dictionary<int, List<int>>) null;
    this._uniqueKeys.Clear();
    this._uniqueKeys = (List<string>) null;
    base.ClearTmpData();
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  private static string GetCurentImbLinkKey(TechObjectRecordBase recBase)
  {
    int num = Convert.ToInt32(recBase.Fields["F_OPERKEY"]);
    int int32_1 = Convert.ToInt32(recBase.Fields["F_PEREHKEY"]);
    int int32_2 = Convert.ToInt32(recBase.Fields["F_DOCTCKEY"]);
    if (int32_1 > 0)
      num = 0;
    int iOperKey = num;
    int iPerehKey = int32_1;
    return TechOsnPosPump.GetCurentImbLinkKey(int32_2, iOperKey, iPerehKey);
  }

  private static string GetCurentImbLinkKey(int iTpKey, int iOperKey, int iPerehKey)
  {
    return $"{iTpKey}_{iOperKey}_{iPerehKey}";
  }

  public static string GenerateTechToolParentKey(TechObjectRecordBase recBase)
  {
    int int32_1 = Convert.ToInt32(recBase.Fields["F_INSTRUM"]);
    int int32_2 = Convert.ToInt32(recBase.Fields["F_POS"]);
    int num = Convert.ToInt32(recBase.Fields["F_OPERKEY"]);
    int int32_3 = Convert.ToInt32(recBase.Fields["F_PEREHKEY"]);
    int int32_4 = Convert.ToInt32(recBase.Fields["F_DOCTCKEY"]);
    if (int32_3 > 0)
      num = 0;
    int pos = int32_2;
    int curTpKey = int32_4;
    int curOperKey = num;
    int curPerehKey = int32_3;
    int diffArtTcKey = recBase.diff_ArtTcKey;
    return TechOsnPosPump.GenerateTechToolParentKey(int32_1, pos, curTpKey, curOperKey, curPerehKey, diffArtTcKey);
  }

  private static string GenerateTechToolParentKey(
    int instrumNom,
    int pos,
    int curTpKey,
    int curOperKey,
    int curPerehKey,
    int artId)
  {
    if (instrumNom == 0 && pos == 0)
      return string.Empty;
    string curentImbLinkKey = TechOsnPosPump.GetCurentImbLinkKey(curTpKey, curOperKey, curPerehKey);
    return $"{instrumNom}_{pos}_{curentImbLinkKey}_{artId}";
  }

  protected override Guid GUID => this._guid;
}
