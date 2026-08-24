// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.VidIzd.VidIzdPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.VidIzd;

[TaskDescription("Инициализация данных для перекачки - Виды изделий", "Перекачка данных - Виды изделий")]
[TaskType(PumperType.MetaData)]
internal class VidIzdPump(PluginClass plugin) : ImbaseObjectRecordMetaPump(plugin)
{
  private readonly Guid _guid = new Guid("{91666E23-727A-4820-8561-FF98FBF1BC1E}");
  private IAttributeTypeItem _atImbaseReferenceAttr;

  protected override void InitData()
  {
    this._recType = "Вид изделия";
    this._tableName = string.Empty;
    this.objTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otArticleTypes).ID;
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechVidIzdPump;

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    string caption = Convert.ToString(recBase.Fields["F_NAME"]);
    this._import_data_main.AddValue(this.GetTechCategory(), oldKey, newKey, caption);
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
      IAttributeTypeItem byGuid = imdi.AttributeTypes.GetByGuid(Intermech.Imbase.Consts.ImbaseObjectRefAttGUID);
      if (byGuid == null)
        return;
      this._atImbaseReferenceAttr = byGuid;
    }
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new ImbaseObjectRecordDynamic(this._tableName);
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    string str = Convert.ToString(record.Fields["F_NAME"]);
    objRecord.Caption = str.Truncate(Intermech.Consts.MaxStringSize - 2);
    this._techParmList.AddAttribute(this._atNaimAttrType, (object) str);
    int result;
    if (int.TryParse(Convert.ToString(record.Fields["F_LEVEL"]), out result) && result != 0)
    {
      DictionaryValue dictionaryValue = this._import_data_imbase.GetValue(ImportingCategory.ImbaseFolders, (object) TechcardConsts.Utils.CodeHashCode(this._imTableCode, result));
      if (dictionaryValue != null)
        this._techParmList.AddAttribute(this._atImbaseReferenceAttr, (object) dictionaryValue);
    }
    base.FillTechObject(objRecord, record);
  }

  public override void Exam()
  {
    if (this._tableName.Equals(string.Empty))
    {
      ImTableInfo tableInfo = TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.VidIzd);
      if (tableInfo != null)
      {
        this._tableName = tableInfo.TableName;
        this._imTableCode = tableInfo.TableKey;
      }
    }
    base.Exam();
  }

  public override void Pump()
  {
    if (this._tableName.Equals(string.Empty))
    {
      ImTableInfo tableInfo = TechPumpData.Tables.ImTablesData.GetTableInfo(TechcardConsts.imTablesConsts.VidIzd);
      if (tableInfo != null)
      {
        this._tableName = tableInfo.TableName;
        this._imTableCode = tableInfo.TableKey;
      }
    }
    base.Pump();
  }

  protected override Guid GUID => this._guid;
}
