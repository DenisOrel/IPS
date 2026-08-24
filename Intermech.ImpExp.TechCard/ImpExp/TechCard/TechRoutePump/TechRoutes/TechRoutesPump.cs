// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechRoutePump.TechRoutes.TechRoutesPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoutes;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechRoutePump.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechRoutePump.TechRoutes;

[TaskDescription("Инициализация данных для перекачки - Вариант маршрута", "Перекачка данных - Вариант маршрута")]
internal class TechRoutesPump(PluginClass plugin) : TechRouteCommonPump(plugin)
{
  private Dictionary<int, List<int>> _parent2KeyList = new Dictionary<int, List<int>>();
  protected Guid _guid = new Guid("{FFE301A2-D3E6-4a21-A44A-917CAEEB5D41}");
  protected IAttributeTypeItem _atTechRouteVidAttr;
  protected IAttributeTypeItem _atTechRouteNaznAttr;
  protected IAttributeTypeItem _atTechRouteTipAttr;
  protected int _otTechRouteObrabTemplateID = -1;
  protected int _otTechRouteAssemTemplateID = -1;

  private void ReadParentObjectKeys()
  {
    this._parent2KeyList.Clear();
    try
    {
      IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
      command.CommandText = string.Format("SELECT \r\n                                                    {0},\r\n                                                    {2}     \r\n                                                  FROM \r\n                                                    {1} \r\n                                                  WHERE \r\n                                                    {0} <> 0 \r\n                                                  ORDER BY {0},{2}", (object) "F_VERSION_FOR", (object) "TC_NROUTES", (object) "F_KEY");
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal1 = dataReader.GetOrdinal("F_VERSION_FOR");
        int ordinal2 = dataReader.GetOrdinal("F_KEY");
        int num = 0;
        List<int> intList = (List<int>) null;
        while (dataReader.Read())
        {
          int int32_1 = dataReader.IsDBNull(ordinal1) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal1]);
          int int32_2 = dataReader.IsDBNull(ordinal2) ? 0 : BasePumpHelper.ToInt32(dataReader[ordinal2]);
          if (int32_1 != 0)
          {
            if (num != int32_1)
            {
              intList = new List<int>();
              intList.Add(int32_1);
              this._parent2KeyList.Add(int32_1, intList);
              num = int32_1;
            }
            if (int32_1 != int32_2)
              intList.Add(int32_2);
          }
        }
        dataReader.Close();
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage("Невозможно получить перечень родительских идентификаторов версий расцеховочных маршрутов: " + ex.Message);
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  private int GetRecordVersionNo(int parentKey, int recordKey, out int prevRecordKey)
  {
    int recordVersionNo1 = -1;
    prevRecordKey = -1;
    List<int> intList;
    if (!this._parent2KeyList.TryGetValue(parentKey, out intList))
      return recordVersionNo1;
    int recordVersionNo2 = intList.IndexOf(recordKey);
    if (recordVersionNo2 == -1 || recordVersionNo2 <= 0)
      return recordVersionNo2;
    prevRecordKey = intList[recordVersionNo2 - 1];
    return recordVersionNo2;
  }

  private void LinkRoutesWithTemplates(
    TechObjectRecordBase route,
    long routeIpsObjId,
    int relTypeId,
    List<TechRelParam> links)
  {
    int int32_1 = Convert.ToInt32(route.Fields["F_TMP_SBORKI"]);
    if (!this.IsCloneRecord(route))
    {
      long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechRouteTemplate, (object) int32_1);
      if (newKey != 0L)
        links.Add(new TechRelParam(routeIpsObjId, newKey, relTypeId, this.objTypeID, this._otTechRouteAssemTemplateID)
        {
          Sort = links.Count
        });
    }
    else
    {
      TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechRouteTemplate, (object) int32_1, relTypeId, route, routeIpsObjId, this.objTypeID, this._otTechRouteAssemTemplateID);
      if (techRelParam != null)
      {
        techRelParam.Sort = links.Count;
        links.Add(techRelParam);
      }
    }
    int int32_2 = Convert.ToInt32(route.Fields["F_TMP_OBRABOTKI"]);
    if (!this.IsCloneRecord(route))
    {
      long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechRouteTemplate, (object) int32_2);
      if (newKey == 0L)
        return;
      links.Add(new TechRelParam(routeIpsObjId, newKey, relTypeId, this.objTypeID, this._otTechRouteObrabTemplateID)
      {
        Sort = links.Count
      });
    }
    else
    {
      TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechRouteTemplate, (object) int32_2, relTypeId, route, routeIpsObjId, this.objTypeID, this._otTechRouteObrabTemplateID);
      if (techRelParam == null)
        return;
      techRelParam.Sort = links.Count;
      links.Add(techRelParam);
    }
  }

  protected override void InitData()
  {
    base.InitData();
    this._recType = "C";
    this._recTypeID = 3;
    this._tableName = "TC_NROUTES";
  }

  protected virtual IDataReader GetCheckRecordReader()
  {
    if (!this.TableExists(this._tableName))
      return (IDataReader) null;
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandTimeout = 0;
    command.CommandText = $"SELECT * FROM {this._tableName.ToUpper()} WHERE F_STATUS = 2 ";
    return command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior);
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
      {
        int recTypeId = 2;
        return TechDataBuilder<PumpClass>.GetPumpModeCond(condField, recTypeId);
      });
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
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
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechRouteVidAttrGuid);
      if (byGuid1 != null)
        this._atTechRouteVidAttr = byGuid1;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechRouteNaznAttrGuid);
      if (byGuid2 != null)
        this._atTechRouteNaznAttr = byGuid2;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechRouteTipAttrGuid);
      if (byGuid3 != null)
        this._atTechRouteTipAttr = byGuid3;
      IObjectTypeItem byGuid4 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otRoutesTemplatesObjTypeGuid);
      if (byGuid4 != null)
        this._otTechRouteObrabTemplateID = byGuid4.ID;
      IObjectTypeItem byGuid5 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otRoutesTemplatesObjTypeGuid);
      if (byGuid5 != null)
        this._otTechRouteAssemTemplateID = byGuid5.ID;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechRoute;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.TechRouteTemplate,
      ImportingCategory.TechRouteElem
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[3]
    {
      ImportingCategory.Users,
      ImportingCategory.TechRouteParents,
      ImportingCategory.BaseTechObjectsVersionsCache
    };
  }

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjTypeOrParamType(entCode, attrGuid);
  }

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjLinkOrParamType(entCode, attrGuid);
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    record.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
    return string.Empty;
  }

  protected override void CheckBaseRecords()
  {
    using (IDataReader checkRecordReader = this.GetCheckRecordReader())
    {
      this.GetTpObjRec().ParseSchema((IDictionary<string, int>) this.GetTableColumns(checkRecordReader));
      while (checkRecordReader.Read())
      {
        TechObjectRecord tpObjRec = this.GetTpObjRec();
        this.PumpLoadDataRec(checkRecordReader, tpObjRec);
        string recordPumpMode = this.GetRecordPumpMode(tpObjRec);
        if (recordPumpMode != string.Empty)
        {
          string caption = Convert.ToString(tpObjRec.Fields["F_NAME"]);
          string description = Convert.ToString(tpObjRec.Fields["F_PRIM"]);
          Conflict conflict = new Conflict(tpObjRec.Key, TechcardConsts.TypeConsts.otRouteObjTypeGuid, caption, description, recordPumpMode);
          TechCardPlugin.InitializationConflictList.Add(conflict);
        }
      }
      checkRecordReader.Close();
    }
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int key = record.Key;
    string str1 = Convert.ToString(record.Fields["F_NAME"]);
    int newKey = Convert.ToInt32(record.Fields["F_VER"]);
    int int32_1 = Convert.ToInt32(record.Fields["F_VERSION_FOR"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_VID"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_NAZN"]);
    int int32_4 = Convert.ToInt32(record.Fields["F_TIP"]);
    int result = Convert.ToInt32(record.Fields["F_USER"]);
    object entityValue = this._techParmList.GetEntityValue("%RUC");
    if (entityValue != null)
      int.TryParse(Convert.ToString(entityValue), out result);
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATA_VVODA"]);
    int int32_5 = Convert.ToInt32(record.Fields["F_RECORD_STATE"]);
    objRecord.Caption = str1.Truncate(Consts.MaxStringSize - 2);
    if (this._parent2KeyList.ContainsKey(key) && this._import_data_main.GetValue(ImportingCategory.TechRouteParents, (object) key) == null)
      this._import_data_main.AddValue(ImportingCategory.TechRouteParents, (object) key, 0L, objRecord.IdGuid.ToString());
    if (int32_1 != 0 && int32_1 != key)
    {
      DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechRouteParents, (object) int32_1);
      if (dictionaryValue != null && !string.IsNullOrEmpty(dictionaryValue.Caption))
      {
        Guid guid = Guid.Empty;
        try
        {
          guid = new Guid(dictionaryValue.Caption);
        }
        catch
        {
        }
        if (!guid.Equals(Guid.Empty))
        {
          objRecord.Id = 0L;
          objRecord.IdGuid = (object) guid;
          objRecord.ObjectVerType = 1;
          int prevRecordKey;
          newKey = this.GetRecordVersionNo(int32_1, key, out prevRecordKey);
          objRecord.ParentVersionNo = newKey - 1;
          objRecord.ParentVersionId = this._import_data_main.GetNewKey(this.GetTechCategory(), (object) prevRecordKey);
        }
      }
    }
    objRecord.VersionId = newKey;
    if (this._atObozAttrType != null)
      this._techParmList.AddAttribute(this._atObozAttrType, (object) str1);
    if (this._atNaimAttrType != null)
      this._techParmList.AddAttribute(this._atNaimAttrType, (object) str1);
    if (this._atTechRouteVidAttr != null)
    {
      string str2 = string.Empty;
      switch (int32_2)
      {
        case 1:
          str2 = "Единственный";
          break;
        case 2:
          str2 = "Множественный";
          break;
      }
      this._techParmList.AddAttribute(this._atTechRouteVidAttr, (object) str2);
    }
    if (this._atTechRouteNaznAttr != null)
    {
      string str3 = string.Empty;
      switch (int32_3)
      {
        case 1:
          str3 = "Собственный";
          break;
        case 2:
          str3 = "По кооперации";
          break;
        case 3:
          str3 = "Для запчастей";
          break;
      }
      this._techParmList.AddAttribute(this._atTechRouteNaznAttr, (object) str3);
    }
    if (this._atTechRouteTipAttr != null)
    {
      string str4 = string.Empty;
      switch (int32_4)
      {
        case 1:
          str4 = "Постоянный";
          break;
        case 2:
          str4 = "Временный";
          break;
      }
      this._techParmList.AddAttribute(this._atTechRouteTipAttr, (object) str4);
    }
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(result);
    if (userInfoBySearchId != null)
    {
      objRecord.OwnerId = userInfoBySearchId.NewObjectID;
      objRecord.OwnerGuid = (object) (userInfoBySearchId.Tag as UserTag).Guid;
    }
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    base.FillTechObject(objRecord, record);
    objRecord.IsBaseVersion = int32_5 == 1 || int32_5 == 2;
    if (!objRecord.IsBaseVersion)
      return;
    long oldKey = TechPumpBase.GenBaseTechObjectsVersionsCacheKey(record.Key, LinkedObjectType.Route);
    if (this._import_data_main.GetValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) oldKey) != null)
      return;
    this._import_data_main.AddValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) oldKey, (long) newKey);
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechRoutesObject();

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> links = new List<TechRelParam>();
    int relTechRelationId = this._relTechRelationID;
    if (!TechSettingsHelper.IgnoreRouteTemplates)
      this.LinkRoutesWithTemplates(recBase, ipsObjId, relTechRelationId, links);
    return links;
  }

  public override void Exam() => base.Exam();

  public override void Pump()
  {
    this.ReadParentObjectKeys();
    base.Pump();
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._parent2KeyList.Clear();
    this._parent2KeyList = (Dictionary<int, List<int>>) null;
  }

  protected override Guid GUID => this._guid;
}
