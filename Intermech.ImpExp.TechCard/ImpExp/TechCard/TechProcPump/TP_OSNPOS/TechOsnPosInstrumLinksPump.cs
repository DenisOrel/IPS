// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_OSNPOS.TechOsnPosInstrumLinksPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OSNPOS;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_OSNPOS;

[TaskDescription("Инициализация данных для перекачки - Создание связей Позиций с Инструментами", "Перекачка данных - Создание связей Позиций с Инструментами")]
internal class TechOsnPosInstrumLinksPump(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private string _prevRecHash = string.Empty;
  private Dictionary<string, int> _recHash2KeyCache = new Dictionary<string, int>();
  private Dictionary<string, Dictionary<int, long>> _recHash2CloneInfo = new Dictionary<string, Dictionary<int, long>>();
  private readonly Guid _guid = new Guid("{AA4B8B1E-47D7-480d-ADE6-271CD843EA54}");

  protected override Guid GUID => this._guid;

  protected override void InitData()
  {
    this._relTypeGuid = TechcardConsts.TypeConsts.rtTechRelationGuid;
    this._tableName = "TP_OSNPOS";
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override bool CheckRecordByCache(TechObjectRecordBase recbase) => false;

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechPosInstrumLinks;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.TechOsnPos
    };
  }

  private string GetRecordHash(int operKey, int perehKey, int posKey)
  {
    return $"{operKey}_{perehKey}_{posKey}";
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechOsnPosInstrumLinksDataBuilder<TechPumpBase> dataBuilder = new TechOsnPosInstrumLinksDataBuilder<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => TechDataBuilder<PumpClass>.GetPumpModeCond(dopType == "" ? "F_DOCTCKEY" : "F_TCKEY", -2));
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return base.GetDataSource();
  }

  protected override int CreateRelations(
    string pumpBegin,
    int relTypeId,
    int i,
    int recCount,
    TechObjectRecordBase record)
  {
    if (record == null)
      return i;
    int int32_1 = Convert.ToInt32(record.Fields["F_OPERKEY"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_PEREHKEY"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_POS"]);
    int int32_4 = Convert.ToInt32(record.Fields["F_INSTRUM"]);
    string recordHash = this.GetRecordHash(int32_1, int32_2, int32_3);
    if (this._prevRecHash != recordHash)
    {
      this._recHash2KeyCache.Remove(this._prevRecHash);
      this._recHash2CloneInfo.Remove(this._prevRecHash);
    }
    if (int32_4 == 0)
    {
      int num;
      if (this._recHash2KeyCache.TryGetValue(recordHash, out num))
      {
        this.plugin.appManager.AddWarningMessage($"Обнаружен дубликат записи '{recordHash}' c F_KEY = {record.Key} для обработанной записи с F_KEY = {num} ");
        return i;
      }
      this._recHash2KeyCache.Add(recordHash, record.Key);
    }
    if (int32_3 != 0 && int32_4 != 0)
      this.CreateRelation(recordHash, record);
    this._prevRecHash = recordHash;
    ++i;
    if (i % this.CheckCount == 0 || i == recCount - 1)
      this.PumpCheckPoint($"{pumpBegin} ({i} из {recCount})", this.CalculatePercent(recCount, i, 0, 100));
    return i;
  }

  private void CreateRelation(string recHash, TechObjectRecordBase record)
  {
    int oldKey;
    if (record == null || this._import_data_main.GetNewKey(this.GetTechCategory(), (object) record.Key) != 0L || !this._recHash2KeyCache.TryGetValue(recHash, out oldKey))
      return;
    DictionaryValue dictValue1 = this._import_data_main.GetValue(ImportingCategory.TechOsnPos, (object) record.Key);
    if (dictValue1 == null)
      return;
    long newObjectId1 = dictValue1.NewObjectID;
    if (newObjectId1 == 0L)
      return;
    SortedDictionary<int, long> sortedDictionary = new SortedDictionary<int, long>();
    sortedDictionary.Add(0, newObjectId1);
    if (dictValue1.Tag is TechRecordObjectTag)
    {
      TechDiffTag diffTag = TechDiffTag.GetDiffTag(dictValue1);
      if (diffTag != null && !diffTag.IsCloneListEmpty)
      {
        foreach (KeyValuePair<int, long> clone in diffTag.CloneList)
          sortedDictionary.Add(clone.Key, clone.Value);
      }
    }
    Dictionary<int, long> dictionary;
    if (!this._recHash2CloneInfo.TryGetValue(recHash, out dictionary))
    {
      DictionaryValue dictValue2 = this._import_data_main.GetValue(ImportingCategory.TechOsnPos, (object) oldKey);
      if (dictValue2 == null)
        return;
      long newObjectId2 = dictValue2.NewObjectID;
      if (newObjectId2 == 0L)
        return;
      dictionary = new Dictionary<int, long>();
      this._recHash2CloneInfo.Add(recHash, dictionary);
      dictionary.Add(0, newObjectId2);
      if (dictValue2.Tag is TechRecordObjectTag)
      {
        TechDiffTag diffTag = TechDiffTag.GetDiffTag(dictValue2);
        if (diffTag != null && !diffTag.IsCloneListEmpty)
        {
          foreach (KeyValuePair<int, long> clone in diffTag.CloneList)
            dictionary.Add(clone.Key, clone.Value);
        }
      }
    }
    Guid guid = Guid.Empty;
    foreach (KeyValuePair<int, long> keyValuePair in sortedDictionary)
    {
      int key = keyValuePair.Key;
      long partId = keyValuePair.Value;
      long projId;
      if (dictionary.TryGetValue(key, out projId))
      {
        RelationRecord linkRel = this._impRelList.AddRelation(projId, partId, this._relTechRelationID);
        if (linkRel != null)
        {
          if (key == 0)
            guid = (Guid) linkRel.PrjLinkGuid;
          else if (guid != Guid.Empty && this._atTechLinkAtRelGTPRelation != null)
            this._impRelList.AddAttributeStr(this._atTechLinkAtRelGTPRelation.ID, guid.ToString());
          this.AddRelAtr(linkRel, record);
          this.FillLinkObligatoryAttributes();
        }
      }
    }
    this._import_data_main.AddValue(this.GetTechCategory(), (object) record.Key, 1L);
  }

  protected override void ClearTmpData()
  {
    base.ClearTmpData();
    this._recHash2KeyCache.Clear();
    this._recHash2KeyCache = (Dictionary<string, int>) null;
    this._recHash2CloneInfo.Clear();
    this._recHash2CloneInfo = (Dictionary<string, Dictionary<int, long>>) null;
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic();
  }

  public override void Exam()
  {
    bool flag;
    using (IDataReader dataReader = this.GetDataReader(string.Format("SELECT \r\n                               F_OPERKEY, F_PEREHKEY, F_POS, COUNT(*) \r\n                            FROM \r\n                              TP_OSNPOS\r\n                            WHERE \r\n                              F_INSTRUM = 0\r\n                            GROUP BY \r\n                              F_OPERKEY, F_PEREHKEY, F_POS\r\n                            HAVING COUNT(*) > 1")))
      flag = dataReader.Read();
    if (flag && MessageBox.Show($"В базе Imbase обнаружены не уникальные записи в таблице TP_OSNPOS !{Environment.NewLine}Рекомендуется перед импортом запустить в TechCFG команду 'Администрирование'-'Корректировка базы'. Прервать импорт ?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
      Application.Exit();
    base.Exam();
  }

  public override void Pump() => base.Pump();
}
