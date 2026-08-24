// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord.ImbaseObjectRecordMetaPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectData;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.BaseMetaPump;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord;

internal abstract class ImbaseObjectRecordMetaPump(PluginClass plugin) : TechMetaPumpBase(plugin)
{
  private Dictionary<string, List<string>> _field2EntityCache = new Dictionary<string, List<string>>();

  private void LoadField2EntityData()
  {
    if (string.IsNullOrEmpty(this.TableName))
      return;
    int num = 0;
    foreach (ImTableInfo imTableInfo in TechPumpData.Tables.ImTablesData.GetAllTableInfo())
    {
      if (imTableInfo != null && !(imTableInfo.TableName != this.TableName))
      {
        num = imTableInfo.TableKey;
        break;
      }
    }
    if (num == 0)
      return;
    foreach (EntityReference entityReference in TechPumpData.Entities.EntityRefDataList.Values)
    {
      if (entityReference != null && entityReference.Reference == num)
      {
        string key;
        switch (entityReference.Field)
        {
          case -1:
            key = "F_NAME";
            break;
          case 0:
            key = "F_LEVEL";
            break;
          default:
            key = TechPumpData.Tables.ImFieldsData.GetFieldName(entityReference.Field);
            break;
        }
        if (!(key == string.Empty))
        {
          List<string> stringList;
          if (!this._field2EntityCache.TryGetValue(key, out stringList))
          {
            stringList = new List<string>();
            this._field2EntityCache.Add(key, stringList);
          }
          stringList.Add(entityReference.Code);
        }
      }
    }
  }

  protected override void ExamSubData(string dopType)
  {
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override TechDataSource GetDataSource()
  {
    return this._dataSource ?? (this._dataSource = new TechDataSource((ITechDataBuilder) new ImbaseObjectRecordMetaDataBuilder<TechPumpBase>((TechPumpBase) this)));
  }

  protected override bool PumpLoadSubData_Loaded(
    string dopType,
    TechObjectRecordBase recBase,
    TechObjectRecordSub dopRecord)
  {
    return dopRecord.ParentKey > recBase.baseKey;
  }

  protected override void LoadMetaData4Pump()
  {
    base.LoadMetaData4Pump();
    this.LoadField2EntityData();
  }

  protected override void LoadTechBaseParams(TechObjectRecord record)
  {
    if (record == null)
      return;
    foreach (string key in (IEnumerable<string>) record.Fields.Keys)
    {
      List<string> stringList;
      if (this._field2EntityCache.TryGetValue(key, out stringList))
      {
        object fieldValue = record.GetFieldValue(key);
        foreach (string code in stringList)
          this._techParmList.AddEntity(code, fieldValue);
      }
    }
  }

  protected override void LoadTechSubParamsCustom(TechObjectDataSub techDataRec, string dopType)
  {
    if (techDataRec == null || dopType != "REC")
      return;
    foreach (TechObjectRecordSub record in techDataRec.Records)
    {
      foreach (string key in (IEnumerable<string>) record.Fields.Keys)
      {
        if (!key.Equals(string.Empty))
        {
          object fieldValue = record.GetFieldValue(key);
          if (fieldValue != null)
          {
            this._techParmList.AddEntity(key, fieldValue);
            List<string> stringList;
            if (!(key == "F_KEY") && !(key == "F_LEVEL") && this._field2EntityCache.TryGetValue(key, out stringList))
            {
              foreach (string code in stringList)
                this._techParmList.AddEntity(code, fieldValue);
            }
          }
        }
      }
    }
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._field2EntityCache.Clear();
    this._field2EntityCache = (Dictionary<string, List<string>>) null;
  }
}
