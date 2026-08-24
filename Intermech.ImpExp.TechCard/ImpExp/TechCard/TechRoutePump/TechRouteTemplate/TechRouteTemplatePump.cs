// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechRoutePump.TechRouteTemplate.TechRouteTemplatePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRouteTemplate;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechRoutePump.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechRoutePump.TechRouteTemplate;

[TaskDescription("Инициализация данных для перекачки - Шаблон расцеховки", "Перекачка данных - Шаблон расцеховки")]
internal class TechRouteTemplatePump(PluginClass plugin) : TechRouteCommonPump(plugin)
{
  private readonly Guid _guid = new Guid("{E71445A7-E5CD-4cf1-9101-465B01123FD0}");
  private int _otRouteTemplIzgot = -1;
  private int _otRouteTemplSborka = -1;

  protected override void InitData()
  {
    base.InitData();
    this._recType = "G";
    this._recTypeID = 7;
    this._tableName = "TC_NROUTE_TEMPLATES";
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
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otRoutesTemplIzgotObjTypeGuid);
      if (byGuid1 != null)
        this._otRouteTemplIzgot = byGuid1.ID;
      IObjectTypeItem byGuid2 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otRoutesTemplSborkaObjTypeGuid);
      if (byGuid2 != null)
        this._otRouteTemplSborka = byGuid2.ID;
      base.LoadMetaData4Pump();
    }
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechRouteTemplate;

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.Users
    };
  }

  protected virtual IDataReader GetCheckRecordReader()
  {
    if (!this.TableExists(this._tableName))
      return (IDataReader) null;
    IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
    command.CommandTimeout = 0;
    command.CommandText = $"SELECT * FROM {this._tableName.ToUpper()} WHERE F_RECORD_STATE = 2 ";
    return command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior);
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
          Conflict conflict = new Conflict(tpObjRec.Key, TechcardConsts.TypeConsts.otRoutesTemplatesObjTypeGuid, caption, string.Empty, recordPumpMode);
          TechCardPlugin.InitializationConflictList.Add(conflict);
        }
      }
      checkRecordReader.Close();
    }
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) =>
      {
        int recTypeId = 107;
        return TechDataBuilder<PumpClass>.GetPumpModeCond(condField, recTypeId);
      });
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    string str = Convert.ToString(record.Fields["F_NAME"]);
    int int32 = Convert.ToInt32(record.Fields["F_USER"]);
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    switch (Convert.ToInt32(record.Fields["F_TYPE"]))
    {
      case 1:
        objRecord.ObjectType = this._otRouteTemplIzgot;
        break;
      case 2:
        objRecord.ObjectType = this._otRouteTemplSborka;
        break;
      default:
        this.plugin.appManager.AddWarningMessage($"Тип раблона расцеховкине определен для записи F_KEY'={Convert.ToString(record.Fields["F_KEY"])}'");
        return;
    }
    objRecord.Caption = str.Truncate(Consts.MaxStringSize - 2);
    if (this._atNaimAttrType != null)
      this._techParmList.AddAttribute(this._atNaimAttrType, (object) str);
    if (this._atObozAttrType != null)
      this._techParmList.AddAttribute(this._atObozAttrType, (object) str);
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(int32);
    if (userInfoBySearchId != null)
    {
      objRecord.OwnerId = userInfoBySearchId.NewObjectID;
      if (userInfoBySearchId.Tag is UserTag tag)
        objRecord.OwnerGuid = (object) tag.Guid;
    }
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    base.FillTechObject(objRecord, record);
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechRouteTemplateObject();
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return new List<TechRelParam>();
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  protected override Guid GUID => this._guid;
}
