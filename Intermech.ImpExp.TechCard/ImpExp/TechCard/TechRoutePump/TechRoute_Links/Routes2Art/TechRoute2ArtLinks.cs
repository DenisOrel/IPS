// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechRoutePump.TechRoute_Links.Routes2Art.TechRoute2ArtLinks
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.Routes2Art;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechRoutePump.TechRoute_Links.Routes2Art;

[TaskDescription("Инициализация данных для перекачки - Связь объектов с МО", "Перекачка данных - Связь объектов с МО")]
internal class TechRoute2ArtLinks(PluginClass plugin) : TechLinkBasePump(plugin)
{
  private readonly Guid _guid = new Guid("{E1D9BFCE-7E3A-4880-B1C2-051112413A69}");
  private IDictionary<string, List<long>> _procRoutes = (IDictionary<string, List<long>>) new Dictionary<string, List<long>>();

  private void ReadProcRoutes()
  {
    foreach (KeyValuePair<object, DictionaryValue> keyValuePair in this._import_data_main.GetCategory(ImportingCategory.TechManufacturingRouting))
    {
      string key = string.Join("_", ((string) keyValuePair.Key).Split('_'), 0, 3);
      List<long> longList;
      if (!this._procRoutes.TryGetValue(key, out longList))
      {
        longList = new List<long>();
        this._procRoutes.Add(key, longList);
      }
      longList.Add(keyValuePair.Value.NewObjectID);
    }
  }

  protected override void InitData()
  {
    base.InitData();
    this._relTypeGuid = TechCardConsts.RelTypes.TechRelationGuid;
    this._categoryA = ImportingCategory.None;
    this._categoryB = ImportingCategory.None;
    this._fieldAName = "F_KEY";
    this._fieldBName = "F_OBJ_KEY";
    this._tableName = "TC_OBJ2LINK";
    this._recType = "Связь МО с ТП";
  }

  protected override Guid GUID => this._guid;

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechRoute2ArtLinks;

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.TechRoute2ArtUniqueLinks;
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new ProcRouteObject();

  protected override long GetNewKeyB(TechObjectRecordBase record, int imObjBId)
  {
    return this.GetIpsObjId(record);
  }

  protected override int CreateRelations(
    string pumpBegin,
    int relTypeId,
    int i,
    int recCount,
    TechObjectRecordBase record)
  {
    int int32 = Convert.ToInt32(record.Fields[this.GetFieldNameB(record)]);
    long newKeyB = this.GetNewKeyB(record, int32);
    List<long> longList;
    if (newKeyB == 0L || record is ProcRouteObject procRouteObject && TechSettingsHelper.PumpLinksOnlyWithActual && this._import_data_main.GetValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) TechPumpBase.GenBaseTechObjectsVersionsCacheKey(procRouteObject.LinkedObj.ObjKeyKey, procRouteObject.LinkedObj.ObjType)) == null || !this._procRoutes.TryGetValue(this.GenerateUniqueRecordHash(record), out longList))
      return i;
    long num = this.plugin.Imdi.ImportedObjects.GetID(newKeyB);
    if (num == 0L)
      num = newKeyB;
    foreach (long ipsObjectAId in longList)
    {
      string oldKey = $"{ipsObjectAId}_{num}_{relTypeId}";
      if (this._import_data_main.GetValue(this.GetTechUniqueCategory(), (object) oldKey) == null)
      {
        RelationRecord techRel = this.CreateTechRel(ipsObjectAId, newKeyB, relTypeId);
        if (techRel != null)
        {
          Guid prjLinkGuid = (Guid) techRel.PrjLinkGuid;
          this.AddRelAtr(techRel, record);
          this.FillLinkObligatoryAttributes();
        }
        if (techRel != null && !string.IsNullOrEmpty(oldKey))
          this._import_data_main.AddValue(this.GetTechUniqueCategory(), (object) oldKey, techRel.PrjLinkId, techRel.PrjLinkGuid.ToString());
        ++i;
        if (i % this.CheckCount == 0 || i == recCount - 1)
          this.PumpCheckPoint($"{pumpBegin} ({i} из {recCount})", this.CalculatePercent(recCount, Math.Min(i, recCount), 0, 100));
      }
    }
    return i;
  }

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    return base.GetUniqueRecordHash(record);
  }

  protected override TechDataSource GetDataSource()
  {
    this.ReadProcRoutes();
    if (this._dataSource == null)
    {
      TechRoute2ArtDataBuilder<TechPumpBase> dataBuilder = new TechRoute2ArtDataBuilder<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
      {
        string pumpModeCond = TechDataBuilder<PumpClass>.GetPumpModeCond(condField, Convert.ToInt32((object) Intermech.ImpExp.TechCard.Common.DataManager.DataManager.ObjDataType.odtArtKey));
        string str = TechSettingsHelper.PumpMode == TechPumpMode.tpmProdZakList ? TechDataBuilder<PumpClass>.GetPumpModeCond("F_ZAK_TCKEY", Convert.ToInt32((object) Intermech.ImpExp.TechCard.Common.DataManager.DataManager.ObjDataType.odtProdZakKey)) : string.Empty;
        if (str.IsEmpty())
          return pumpModeCond;
        return $"({pumpModeCond} and {str})";
      });
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  private string GenerateUniqueRecordHash(TechObjectRecordBase recBase)
  {
    return $"{Convert.ToInt32(recBase.Fields["F_ART_TCKEY"])}_{Convert.ToInt32(recBase.Fields["F_PROJ_TCKEY"])}_{Convert.ToInt32(recBase.Fields["F_ZAK_TCKEY"])}";
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[9]
    {
      ImportingCategory.Articles,
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechRoute,
      ImportingCategory.TechZagot,
      ImportingCategory.TechMatGrPump,
      ImportingCategory.TechManufacturingRouting,
      ImportingCategory.VtdCache,
      ImportingCategory.BaseTechObjectsVersionsCache,
      this.GetTechCategory()
    };
  }

  private long GetIpsObjId(TechObjectRecordBase record)
  {
    int int32_1 = Convert.ToInt32(record.Fields["F_OBJ_TYPE"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_OBJ_KEY"]);
    if (int32_2 == 0)
      return 0;
    long ipsObjId = 0;
    switch (int32_1)
    {
      case 0:
        this.plugin.appManager.AddWarningMessage(record.Key.ToString() + ": Неизвестный тип записи");
        break;
      case 1:
        if (int32_2 != 0)
        {
          int int32_3 = Convert.ToInt32(record.Fields["F_ART_TCKEY"]);
          DictionaryValue dictValue = ImportingDataHelper.Instance.GetValue(this._import_data_main, ImportingCategory.TechProcessPump, (object) int32_2);
          if (dictValue != null)
          {
            TechDiffTag diffTag = TechDiffTag.GetDiffTag(dictValue);
            if (diffTag == null || diffTag.IsCloneListEmpty)
            {
              ipsObjId = dictValue.NewObjectID;
              if (ipsObjId == 0L && !this.IsVedomost(int32_2))
              {
                this.plugin.appManager.AddNewWarningMessage($"Невозможно найти техпроцес F_VERSION={int32_2} в кэше");
                break;
              }
              break;
            }
            foreach (KeyValuePair<int, long> clone in diffTag.CloneList)
            {
              if (int32_3 == clone.Key)
                return clone.Value;
            }
            ipsObjId = 0L;
            break;
          }
          if (!this.IsVedomost(int32_2))
          {
            this.plugin.appManager.AddNewWarningMessage($"Невозможно найти документ F_VERSION={int32_2} в кэше");
            break;
          }
          break;
        }
        break;
      case 2:
        ipsObjId = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechRoute, (object) int32_2);
        break;
      case 3:
        ipsObjId = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechZagot, (object) int32_2);
        break;
      case 4:
        ipsObjId = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechMatGrPump, (object) int32_2);
        break;
      default:
        this.plugin.appManager.AddWarningMessage(record.Key.ToString() + ": Неизвестный тип записи");
        break;
    }
    return ipsObjId;
  }

  private bool IsVedomost(int docVerKey)
  {
    return ImportingDataHelper.Instance.GetValue(this._import_data_main, ImportingCategory.VtdCache, (object) docVerKey) != null;
  }
}
