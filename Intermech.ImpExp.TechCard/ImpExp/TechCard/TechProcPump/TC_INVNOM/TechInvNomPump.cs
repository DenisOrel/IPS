// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TC_INVNOM.TechInvNomPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TC_INVNOM;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TC_INVNOM;

[TaskDescription("Инициализация данных для перекачки - Инвентарные номера", "Перекачка данных - Инвентарные номера")]
internal class TechInvNomPump : TechPumpBase
{
  private IAttributeTypeItem _atImbaseReferenceAttr;
  private IAttributeTypeItem _atImbaseCodeAttr;
  private readonly Guid _guid = new Guid("{D8926B08-8DD2-44de-862F-3828274EABA2}");
  private string _field4ModelCode = string.Empty;
  private string _field4ModelName = string.Empty;
  internal readonly List<InvNomStructRec> structList = new List<InvNomStructRec>();
  private static readonly string TypeName = "Инвентарные номера";

  protected override Guid GUID => this._guid;

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    return TechcardConsts.Utils.CodeHashCode(Convert.ToInt32(record.Fields["F_KEY"]), 0).ToString();
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TC_INVNOM");
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override string GetRecordWithParamsPumpMode(TechObjectRecord record)
  {
    string withParamsPumpMode = base.GetRecordWithParamsPumpMode(record);
    if (record.RecMode != TechObjectRecord.PumpMode.LinkOnly)
      return withParamsPumpMode;
    record.RecMode = TechObjectRecord.PumpMode.NotPump;
    return withParamsPumpMode;
  }

  protected override TechDataSource GetDataSource()
  {
    return this._dataSource ?? (this._dataSource = new TechDataSource((ITechDataBuilder) new TechInvNomDataBuilder<TechInvNomPump>(this)));
  }

  protected override void InitData()
  {
    this._recType = TechInvNomPump.TypeName;
    this._recTypeID = -2;
    this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechCardConsts.ObjectTypes.OborudGUID).ID;
    this._tableName = "TC_INVNOM";
    if (TechPumpData.TechType.TechTypeList.ContainsKey(this._recTypeID))
      return;
    TechTypeInfo techTypeInfo = new TechTypeInfo()
    {
      RecordID = this._recTypeID,
      Name = this._recType,
      Type = "-2"
    };
    techTypeInfo.TypeSett = new TechTypeSett()
    {
      ObjType = TechCardConsts.ObjectTypes.OborudGUID,
      Mode = TechTypePumpMode.ExistObjType
    };
    TechPumpData.TechType.TechTypeList.Add(this._recTypeID, techTypeInfo);
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechInvNomPump;

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.TechInvNomUniquePump;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.TechInvNomTablePump
    };
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return new List<TechRelParam>();
  }

  private Entity GetEntityByTC_INVNOM_FieldName(string fieldName)
  {
    Entity tcInvnomFieldName;
    TechPumpData.Entities.EntitiesList.TryGetValue(InvNomStructRec.GenerateEntityName(fieldName), out tcInvnomFieldName);
    return tcInvnomFieldName;
  }

  public override void LoadTechParams(TechObjectRecord record)
  {
    this._techParmList.Clear();
    if (record == null)
      return;
    foreach (string key in (IEnumerable<string>) record.Fields.Keys)
    {
      Entity tcInvnomFieldName = this.GetEntityByTC_INVNOM_FieldName(key);
      if (tcInvnomFieldName != null)
      {
        object fieldValue = record.GetFieldValue(key);
        if (fieldValue != null)
          this._techParmList.AddEntity(tcInvnomFieldName.Code, fieldValue);
      }
    }
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    object fieldValue1 = record.GetFieldValue("F3");
    object fieldValue2 = record.GetFieldValue(this._field4ModelCode);
    object fieldValue3 = record.GetFieldValue(this._field4ModelName);
    if (fieldValue1 != null || fieldValue3 != null)
    {
      string str = Convert.ToString(fieldValue3);
      if (fieldValue1 != null)
        str = $"{str} Инв.N {fieldValue1}";
      objRecord.Caption = str.Truncate(Intermech.Consts.MaxStringSize - 2);
    }
    int result;
    int.TryParse(Convert.ToString(fieldValue2), out result);
    DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TechInvNomTablePump, (object) result);
    this._techParmList.AddAttribute(this._atImbaseReferenceAttr, (object) (dictionaryValue != null ? dictionaryValue.NewObjectID : 0L));
    this._techParmList.AddAttribute(this._atImbaseCodeAttr, (object) record.Key);
    base.FillTechObject(objRecord, record);
  }

  private Entity GetEntityByInvNomStructRec(InvNomStructRec rec)
  {
    Entity byInvNomStructRec = new Entity(InvNomStructRec.GenerateEntityName(rec.FieldName), rec.Name, this._recType, this._recTypeID);
    byInvNomStructRec.IsPermisibleAttr2TypeObj = true;
    byInvNomStructRec.Type = rec.TypeString;
    EntityReference entityReference = new EntityReference();
    entityReference.Reference = rec.TableId;
    entityReference.Field = rec.ImbaseRecId;
    entityReference.Code = byInvNomStructRec.Code;
    entityReference.MasterCode = byInvNomStructRec.Code;
    byInvNomStructRec.EntityReference = entityReference;
    Entity entity1;
    if (!string.IsNullOrEmpty(rec.Entity) && TechPumpData.Entities.EntitiesList.TryGetValue(rec.Entity, out entity1))
    {
      byInvNomStructRec.Settings.PumpTo = (object) entity1;
      byInvNomStructRec.Settings.PumpMode = EntityPumModes.ExistEntity;
    }
    switch (rec.DataType)
    {
      case -101:
        byInvNomStructRec.IsMasterAttr = true;
        byInvNomStructRec.Type = "I";
        byInvNomStructRec.InitializeSetting((IEnumerable<Entity>) TechPumpData.Entities.EntitiesList.Values);
        byInvNomStructRec.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
        break;
      case 111:
      case 121:
      case 131:
        if (rec.DataType == 111 && entityReference.Field == 0)
          entityReference.Field = -1;
        string keyField = rec.KeyField;
        entityReference.MasterCode = InvNomStructRec.GenerateEntityName(keyField);
        Entity entity2;
        if (TechPumpData.Entities.EntitiesList.TryGetValue(entityReference.MasterCode, out entity2))
        {
          if (!entity2.IsMasterAttr)
          {
            entity2.IsMasterAttr = true;
            entity2.Type = "I";
          }
          if (entity2.Settings.Properties.FieldType != FieldTypes.ftObjectLink)
          {
            entity2.InitializeSetting((IEnumerable<Entity>) TechPumpData.Entities.EntitiesList.Values);
            entity2.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
            break;
          }
          break;
        }
        break;
    }
    byInvNomStructRec.InitializeSetting((IEnumerable<Entity>) TechPumpData.Entities.EntitiesList.Values);
    return byInvNomStructRec;
  }

  private void LoadCatalogStruct()
  {
    int modelCatalogId = TechInvNomPump.GetModelCatalogId();
    int recordsCount = this.GetRecordsCount($"select count(*) from {"TC_INVNOMSTRUCT"}");
    this.ExamCheckPoint("Структуры инструментальных номеров", 0);
    using (IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand())
    {
      int val1 = 0;
      EntityTypeRec entityTypeRec;
      if (!TechPumpData.EntTypeList.TryGetValue(this._recTypeID, out entityTypeRec))
      {
        entityTypeRec = new EntityTypeRec();
        TechPumpData.EntTypeList.Add(this._recTypeID, entityTypeRec);
      }
      command.CommandText = string.Format("SELECT DISTINCT \r\n                                                    a.*, \r\n                                                    b.{1} \r\n                                                  FROM \r\n                                                    {0} a left join {2} b \r\n                                                    on a.{3} = b.{4} \r\n                                                  ORDER BY \r\n                                                    {5} DESC", (object) "TC_INVNOMSTRUCT", (object) "F_TABLE", (object) "IM_TABLES", (object) "F_LU_TABLE_ID", (object) "F_KEY", (object) "F_KEYFIELD");
      using (IDataReader dbReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        this.structList.Clear();
        while (dbReader.Read())
        {
          ++val1;
          if (val1 % 100 == 0)
            this.ExamCheckPoint($"Получение структуры инструментальных номеров {val1} из {recordsCount}", this.CalculatePercent(recordsCount, Math.Min(val1, recordsCount), 2));
          if (!dbReader.IsDBNull(dbReader.GetOrdinal("F_KEYFIELD")))
          {
            dbReader.GetString(dbReader.GetOrdinal("F_KEYFIELD"));
          }
          else
          {
            string empty = string.Empty;
          }
          int int32_1 = BasePumpHelper.ToInt32(dbReader[dbReader.GetOrdinal("F_LU_TABLE_ID")]);
          string str1 = dbReader.IsDBNull(dbReader.GetOrdinal("F_TABLE")) ? string.Empty : dbReader.GetString(dbReader.GetOrdinal("F_TABLE"));
          string str2 = dbReader.GetString(dbReader.GetOrdinal("F_FIELDNAME"));
          int int32_2 = BasePumpHelper.ToInt32(dbReader[dbReader.GetOrdinal("F_DATATYPE")]);
          if (int32_1 > 0 && str1.Equals(string.Empty))
          {
            this.plugin.appManager.AddWarningMessage($"Ошибка получение структуры инструментального номера F_FIELDNAME=\"{str2}\". Имя таблицы не найдено в таблице IM_TABLES");
          }
          else
          {
            InvNomStructRec rec = new InvNomStructRec(dbReader);
            this.structList.Add(rec);
            if (int32_1 == modelCatalogId)
            {
              if (int32_2 == -101)
                this._field4ModelCode = str2;
              if (int32_2 == 111)
                this._field4ModelName = str2;
            }
            Entity byInvNomStructRec = this.GetEntityByInvNomStructRec(rec);
            if (!TechPumpData.Entities.EntitiesList.ContainsKey(byInvNomStructRec.Code))
              TechPumpData.Entities.EntitiesList.Add(byInvNomStructRec.Code, byInvNomStructRec);
            entityTypeRec.AddEntity(byInvNomStructRec);
          }
        }
        dbReader.Close();
      }
    }
    this.ExamCheckPoint("Получение структуры инструментальных номеров завершено", 100);
  }

  protected override void LoadMetaData4StoppedPump()
  {
    this.LoadCatalogStruct();
    base.LoadMetaData4StoppedPump();
  }

  internal static int GetModelCatalogId()
  {
    int modelCatalogId = 0;
    EntityReference entityReference;
    if (TechPumpData.Entities.EntityRefDataList.TryGetValue("A138", out entityReference))
    {
      modelCatalogId = entityReference.Reference;
      if (modelCatalogId != 0)
        return modelCatalogId;
      string message = "Не задана привязка к справочникам для понятия \"A138\"";
      TechcardConsts.Plugin.appManager.AddNewWarningMessage(message);
    }
    else
    {
      string message = $"Не найдена информация о привязке к справочникам для понятия \"{"A138"}\"";
      TechcardConsts.Plugin.appManager.AddNewWarningMessage(message);
    }
    ImTableInfo tableInfo = TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.Oborud);
    if (tableInfo != null)
      return tableInfo.TableKey;
    string message1 = $"Не найдена информация о справочнике типа записи \"{"Оборудование"}\" Id={(Enum) TechcardConsts.imTablesConsts.Oborud}";
    TechcardConsts.Plugin.appManager.AddNewWarningMessage(message1);
    return modelCatalogId;
  }

  public TechInvNomPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = false;
    this.taskPump.Repumpble = false;
  }

  protected override void LoadMetaData4Pump()
  {
    base.LoadMetaData4Pump();
    IMetadataInfo imdi = this.plugin.Imdi;
    if (imdi == null)
    {
      this.plugin.appManager.AddErrorMessage("Ошибка получения кэша метаданных");
    }
    else
    {
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(Intermech.Imbase.Consts.ImbaseObjectRefAttGUID);
      if (byGuid1 != null)
        this._atImbaseReferenceAttr = byGuid1;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(Intermech.Imbase.Consts.ImbaseInternalOldKeyAttGUID);
      if (byGuid2 == null)
        return;
      this._atImbaseCodeAttr = byGuid2;
    }
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this.structList.Clear();
  }

  public override void Pump() => base.Pump();

  public override void Exam()
  {
    this.LoadCatalogStruct();
    base.Exam();
  }
}
