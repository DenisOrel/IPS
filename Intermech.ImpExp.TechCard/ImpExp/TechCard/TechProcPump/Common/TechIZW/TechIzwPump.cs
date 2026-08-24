// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechIZW.TechIzwPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ECO.Client;
using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.Common.TechIZW;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common.TechIZW;

[TaskDescription("Инициализация данных для перекачки - Извещения об изменениях", "Перекачка данных - Извещения об изменениях")]
internal class TechIzwPump : TechPumpBase
{
  private readonly Guid _guid = new Guid("{63978970-D1D5-4d2c-9410-3C30CC3BD233}");
  private int _rtChangeAtNotificationRelationId = -1;
  private IAttributeTypeItem _atDellObjectIfDellIzw;
  private IAttributeTypeItem _atObjectMissionAdd;
  private IAttributeTypeItem _atFlagsTypeGuid;
  private IAttributeTypeItem _atModificationId;
  private IAttributeTypeItem _atCompositionVersionId;
  public const string TypeName_KtpEntities = "Понятия для КТП";

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "Понятия для КТП";
    this._tableName = "TC_ARCDOCS";
    IObjectTypeItem byGuid = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00349-306c-11d8-b4e9-00304f19f545"));
    if (byGuid != null)
      this.objTypeID = byGuid.ID;
    this._dopTypes.Add("S");
    this._dopTypes.Add("I");
    this._dopTypes.Add("F");
    this._dopTypes.Add("D");
  }

  protected override void LoadMetaData4Pump()
  {
    IRelationTypeItem byGuid = this.plugin.Imdi.RelationTypes.GetByGuid(new Guid("cad0036b-306c-11d8-b4e9-00304f19f545"));
    if (byGuid != null)
      this._rtChangeAtNotificationRelationId = byGuid.ID;
    this._atDellObjectIfDellIzw = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid(RevReqHelper.guidAttrDelWhenExcluded));
    this._atObjectMissionAdd = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atObjectMissionAddAtrTypeGuid);
    this._atFlagsTypeGuid = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atFlagsTypeGuid);
    this._atModificationId = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad014d2-306c-11d8-b4e9-00304f19f545"));
    this._atCompositionVersionId = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad001c2-306c-11d8-b4e9-00304f19f545"));
    base.LoadMetaData4Pump();
  }

  protected override TechDataSource GetDataSource()
  {
    return this._dataSource ?? (this._dataSource = new TechDataSource((ITechDataBuilder) new TechIzwDataBuilder<TechPumpBase>((TechPumpBase) this)));
  }

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TC_ARCDOCS");
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    if (Convert.ToInt32(record.Fields["F_OBJKEYNEW"]) != 0)
      return base.GetRecordPumpMode(record);
    record.RecMode = TechObjectRecord.PumpMode.NotPump;
    return string.Empty;
  }

  protected override string GetRecordWithParamsPumpMode(TechObjectRecord record)
  {
    string withParamsPumpMode = base.GetRecordWithParamsPumpMode(record);
    if (record.RecMode == TechObjectRecord.PumpMode.ObjectAndLinks)
    {
      long newKey = 0;
      int int32 = Convert.ToInt32(record.Fields["F_DOCID"]);
      if (int32 != 0)
      {
        DocumentTag tag = (DocumentTag) this._import_data_main.GetTag(ImportingCategory.Documents, (object) int32);
        if (tag?.Versions != null && (tag.Versions.TryGetValue(0, out newKey) || tag.Versions.TryGetValue(-1, out newKey)))
          this.plugin.appManager.AddInfoMessage($"Для записи Key={(object) record.Key} найден ранее импортированный документ Search. ObjectId = {(object) newKey}");
      }
      if (newKey != 0L)
      {
        string uniqueRecordHash = this.GetUniqueRecordHash((TechObjectRecordBase) record);
        if (!string.IsNullOrEmpty(uniqueRecordHash))
        {
          DictionaryValue dictionaryValue = this._import_data_main.GetValue(this.GetTechUniqueCategory(), (object) uniqueRecordHash);
          if (dictionaryValue != null)
          {
            this._import_data_main.SetNewKey(this.GetTechUniqueCategory(), (object) uniqueRecordHash, newKey);
            dictionaryValue.Caption = (string) null;
          }
          else
            this._import_data_main.AddValue(this.GetTechUniqueCategory(), (object) uniqueRecordHash, newKey, string.Empty);
          record.RecMode = TechObjectRecord.PumpMode.LinkOnly;
        }
      }
    }
    return withParamsPumpMode;
  }

  protected override string GetUniqueRecordHash(TechObjectRecordBase record)
  {
    return TechcardConsts.Utils.CodeHashCode(Convert.ToInt32(record.Fields["F_DOCTCKEY"]), 0).ToString();
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechIzwPump;

  protected override ImportingCategory GetTechUniqueCategory()
  {
    return ImportingCategory.TechIzwUniquePump;
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[5]
    {
      ImportingCategory.TechRoute,
      ImportingCategory.TechZagot,
      ImportingCategory.TechMatGrPump,
      ImportingCategory.Documents,
      ImportingCategory.TechIzwUniqueObjectLinkInfoPump
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[1]
    {
      ImportingCategory.Users
    };
  }

  private long GetOpsObjectVIdByIzwObject(int objectTypeId, int objectId)
  {
    long objectVidByIzwObject = 0;
    switch (objectTypeId)
    {
      case 3:
        objectVidByIzwObject = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechRoute, (object) objectId);
        break;
      case 23:
        objectVidByIzwObject = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechZagot, (object) objectId);
        break;
      case 24:
        objectVidByIzwObject = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechMatGrPump, (object) objectId);
        break;
    }
    return objectVidByIzwObject;
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    List<TechRelParam> techRelList = new List<TechRelParam>();
    if (recBase == null || ipsObjId == 0L)
      return techRelList;
    int int32_1 = Convert.ToInt32(recBase.Fields["F_KEY"]);
    int int32_2 = Convert.ToInt32(recBase.Fields["F_RECORDID"]);
    int int32_3 = Convert.ToInt32(recBase.Fields["F_OBJKEYNEW"]);
    if (int32_3 == 0)
      return techRelList;
    long objectVidByIzwObject = this.GetOpsObjectVIdByIzwObject(int32_2, int32_3);
    if (objectVidByIzwObject == 0L)
      return techRelList;
    long id = this.plugin.Imdi.ImportedObjects.GetID(objectVidByIzwObject);
    if (id == 0L)
      return techRelList;
    string oldKey = $"{ipsObjId}_{id}";
    long newKey = this._import_data_main.GetNewKey(ImportingCategory.TechIzwUniqueObjectLinkInfoPump, (object) oldKey);
    if (newKey != 0L)
    {
      this.plugin.appManager.AddWarningMessage($"Дубликат связи. ProjObjId = {ipsObjId} PartId = {id} для записи с F_KEY = {int32_1}. Исходная запись F_KEY = {newKey}");
      return techRelList;
    }
    this._import_data_main.AddValue(ImportingCategory.TechIzwUniqueObjectLinkInfoPump, (object) oldKey, (long) int32_1);
    RelationRecord relationRecord = this._impRelList.AddRelation(ipsObjId, objectVidByIzwObject, this._rtChangeAtNotificationRelationId);
    TechRelParam techRelParam = new TechRelParam(ipsObjId, objectVidByIzwObject, this._rtChangeAtNotificationRelationId, this.objTypeID, -1)
    {
      RelRec = relationRecord
    };
    this.FillLinkSortParam(techRelParam, recBase);
    this.FillRelationAttributes(recBase, techRelParam);
    return techRelList;
  }

  private void FillRelationAttributes(TechObjectRecordBase recBase, TechRelParam relationParam)
  {
    if (recBase == null)
      return;
    if (this._atObjectMissionAdd != null)
      this._impRelList.AddAttributeInt(this._atObjectMissionAdd.ID, 0L);
    if (this._atFlagsTypeGuid != null)
      this._impRelList.AddAttributeInt(this._atFlagsTypeGuid.ID, 0L);
    if (this._atDellObjectIfDellIzw != null)
      this._impRelList.AddAttributeInt(this._atDellObjectIfDellIzw.ID, 0L);
    if (this._atModificationId != null)
      this._impRelList.AddAttributeInt(this._atModificationId.ID, relationParam.IpsObjectBid);
    if (this._atCompositionVersionId == null)
      return;
    this._impRelList.AddAttributeInt(this._atCompositionVersionId.ID, relationParam.IpsObjectAid);
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    string str1 = Convert.ToString(record.Fields["F_DESIGNATION"]);
    string str2 = Convert.ToString(record.Fields["F_NAME"]);
    int int32 = Convert.ToInt32(record.Fields["F_USER"]);
    objRecord.Caption = str1.Truncate(Consts.MaxStringSize - 2);
    if (objRecord.Caption == string.Empty)
      objRecord.Caption = str2.Truncate(Consts.MaxStringSize - 2);
    if (this._atNaimAttrType != null)
      this._techParmList.AddEntity("ODoc", (object) str2);
    if (this._atObozAttrType != null)
      this._techParmList.AddEntity("NDoc", (object) str1);
    DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(int32);
    if (userInfoBySearchId == null)
      return;
    objRecord.OwnerId = userInfoBySearchId.NewObjectID;
    objRecord.OwnerGuid = (object) (userInfoBySearchId.Tag as UserTag).Guid;
  }

  protected override void FillObjectObligatoryAttributes(TechObjectRecord record)
  {
    if (record.RecMode == TechObjectRecord.PumpMode.LinkOnly)
      return;
    base.FillObjectObligatoryAttributes(record);
  }

  public TechIzwPump(PluginClass plugin)
    : base(plugin)
  {
    this.InitData();
  }

  public override void Pump() => base.Pump();

  public override void Exam() => base.Exam();

  protected override Guid GUID => this._guid;
}
