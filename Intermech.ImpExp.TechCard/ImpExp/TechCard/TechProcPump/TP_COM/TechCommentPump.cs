// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TP_COM.TechCommentPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump.TP_OPER;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.TP_COM;

[TaskDescription("Инициализация данных для перекачки - Комментарии", "Перекачка данных - Комментарии")]
internal class TechCommentPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private readonly Guid _guid = new Guid("{1231F47B-225D-4D19-8522-C542D345E111}");
  protected int _otMaterialTypeID = -1;
  protected int _otZagotTypeID = -1;
  protected int _atCommentTextAtrID;

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechComment;

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[5]
    {
      ImportingCategory.TechMatPump,
      ImportingCategory.TechProcessPump,
      ImportingCategory.TechOperation,
      ImportingCategory.TechPerehPump,
      ImportingCategory.TechZagot
    };
  }

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "K";
    this._recTypeID = 10;
    this._tableName = "TP_COM";
    this._dopTypes.Add("S");
    this._dopTypes.Add("I");
    this._dopTypes.Add("F");
    this._dopTypes.Add("D");
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
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atCommentTextAtrGuid);
      if (byGuid1 != null)
        this._atCommentTextAtrID = byGuid1.ID;
      IObjectTypeItem byGuid2 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otMaterialsObjTypeGuid);
      if (byGuid2 != null)
        this._otMaterialTypeID = byGuid2.ID;
      IObjectTypeItem byGuid3 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otZagotGUID);
      if (byGuid3 != null)
        this._otZagotTypeID = byGuid3.ID;
      base.LoadMetaData4Pump();
    }
  }

  public override bool CheckObjTypeOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjTypeOrParamType(entCode, attrGuid);
  }

  public override bool CheckObjLinkOrParamType(string entCode, Guid attrGuid)
  {
    return base.CheckObjLinkOrParamType(entCode, attrGuid);
  }

  protected override void CheckBaseRecords()
  {
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    if (objRecord == null || record == null || record.RecMode != TechObjectRecord.PumpMode.ObjectAndLinks && record.RecMode != TechObjectRecord.PumpMode.ObjectOnly)
      return;
    int int32_1 = Convert.ToInt32(record.Fields["F_DOCTCKEY"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_OPERKEY"]);
    DateTime dateTime = Convert.ToDateTime(record.Fields["F_DATE"]);
    FileInfo fileInfo = (FileInfo) null;
    if (record.FieldExist("F_TEXT"))
      fileInfo = (FileInfo) record.Fields["F_TEXT"];
    string rftValue = string.Empty;
    if (fileInfo != null && System.IO.File.Exists(fileInfo.FullName))
    {
      using (FileStream input = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.ReadWrite))
      {
        Encoding utF8 = Encoding.UTF8;
        BinaryReader binaryReader = new BinaryReader((Stream) input);
        try
        {
          byte[] bytes = binaryReader.ReadBytes((int) Math.Min((long) this.captionLength, input.Length));
          char[] chars = utF8.GetChars(bytes);
          if (chars.Length != 0)
            rftValue = new string(chars);
        }
        catch (Exception ex)
        {
          string Message = $"Ошибка чтения файла \"{fileInfo.FullName}\": {ex.Message} ";
          TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
          if (ex is OutOfMemoryException)
            throw;
        }
        finally
        {
          binaryReader.Close();
          input.Close();
        }
      }
    }
    string strValue;
    if (DataConvertor.ConvertRtfToStr(rftValue, out strValue))
    {
      rftValue = strValue;
      if (fileInfo != null)
      {
        System.IO.File.WriteAllText(fileInfo.FullName, strValue, Encoding.GetEncoding(1251));
        fileInfo = new FileInfo(fileInfo.FullName);
      }
    }
    if (rftValue != string.Empty)
      objRecord.Caption = rftValue.Truncate(Consts.MaxStringSize - 2);
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    if (rftValue != string.Empty && this._atNaimAttrType != null)
      this._techParmList.AddAttribute(this._atNaimAttrType, (object) rftValue);
    if (fileInfo != null && this._atCommentTextAtrID != 0)
      this._impObjList.AddAttributeBlob(this._atCommentTextAtrID, fileInfo.FullName, fileInfo.Length, string.Empty, ArcMethods.NotPacked);
    if (dateTime != DateTime.MinValue)
      objRecord.ObjCreate = dateTime.ToUniversalTime();
    if (int32_2 != 0)
    {
      if (this._import_data_main.GetValue(ImportingCategory.TechOperation, (object) int32_2)?.Tag is TechRecordObjectTag tag1 && tag1.Object is TechOperationCacheInfo operationCacheInfo)
      {
        if (operationCacheInfo.OwnerGuid != Guid.Empty)
          objRecord.OwnerGuid = (object) operationCacheInfo.OwnerGuid;
        if (operationCacheInfo.OwnerId != 0L)
          objRecord.OwnerId = operationCacheInfo.OwnerId;
      }
    }
    else
    {
      TechProcCacheInfo techProcCacheInfo = (TechProcCacheInfo) null;
      if (this._import_data_main.GetValue(ImportingCategory.TechProcessPump, (object) int32_1)?.Tag is TechRecordObjectTag tag2)
        techProcCacheInfo = tag2.Object as TechProcCacheInfo;
      if (techProcCacheInfo != null)
      {
        if (techProcCacheInfo.OwnerGuid != Guid.Empty)
          objRecord.OwnerGuid = (object) techProcCacheInfo.OwnerGuid;
        if (techProcCacheInfo.OwnerId != 0L)
          objRecord.OwnerId = techProcCacheInfo.OwnerId;
      }
    }
    base.FillTechObject(objRecord, record);
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

  protected override TechObjectRecord GetTpObjRec()
  {
    return (TechObjectRecord) new TechObjectRecordDynamic("TP_COM");
  }

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    int int32_1 = Convert.ToInt32(recBase.Fields["F_DOCTCKEY"]);
    int int32_2 = Convert.ToInt32(recBase.Fields["F_OPERKEY"]);
    int int32_3 = Convert.ToInt32(recBase.Fields["F_MATERIALKEY"]);
    int int32_4 = Convert.ToInt32(recBase.Fields["F_PEREHKEY"]);
    int int32_5 = Convert.ToInt32(recBase.Fields["F_ZAGOTKEY"]);
    int relTechRelationId = this._relTechRelationID;
    List<TechRelParam> techRelList = new List<TechRelParam>();
    if (int32_4 != 0)
    {
      if (!this.IsCloneRecord(recBase))
      {
        long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechPerehPump, (object) int32_4);
        if (newKey != 0L)
        {
          TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otPerehTypeID, this.objTypeID);
          techRelList.Add(techRelParam);
        }
      }
      else
      {
        TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechPerehPump, (object) int32_4, relTechRelationId, recBase, ipsObjId, this._otPerehTypeID, this.objTypeID);
        if (techRelParam != null)
          techRelList.Add(techRelParam);
      }
    }
    else if (int32_2 != 0)
    {
      if (!this.IsCloneRecord(recBase))
      {
        long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechOperation, (object) int32_2);
        if (newKey != 0L)
        {
          TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otOperTypeID, this.objTypeID);
          techRelList.Add(techRelParam);
        }
      }
      else
      {
        TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechOperation, (object) int32_2, relTechRelationId, recBase, ipsObjId, this._otOperTypeID, this.objTypeID);
        if (techRelParam != null)
          techRelList.Add(techRelParam);
      }
    }
    else if (int32_1 != 0)
    {
      DictionaryValue dictionaryValue = ImportingDataHelper.Instance.GetValue(this._import_data_main, ImportingCategory.TechProcessPump, (object) int32_1);
      int result = -1;
      if (dictionaryValue != null && dictionaryValue.Tag is TechRecordObjectTag)
      {
        object obj = ((TechRecordObjectTag) dictionaryValue.Tag).Object;
        if (obj is TechProcCacheInfo techProcCacheInfo)
          result = techProcCacheInfo.ObjTypeId;
        else
          int.TryParse(obj.ToString(), out result);
      }
      if (!this.IsCloneRecord(recBase))
      {
        long newObjectId = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
        if (newObjectId != 0L)
        {
          TechRelParam techRelParam = new TechRelParam(newObjectId, ipsObjId, relTechRelationId, result, this.objTypeID);
          techRelList.Add(techRelParam);
        }
      }
      else
      {
        TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechProcessPump, (object) int32_1, relTechRelationId, recBase, ipsObjId, result, this.objTypeID);
        if (techRelParam != null)
          techRelList.Add(techRelParam);
      }
    }
    else if (int32_3 != 0)
    {
      if (!this.IsCloneRecord(recBase))
      {
        long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechMatPump, (object) int32_3);
        if (newKey != 0L)
        {
          TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otMaterialTypeID, this.objTypeID);
          techRelList.Add(techRelParam);
        }
      }
      else
      {
        TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechMatPump, (object) int32_3, relTechRelationId, recBase, ipsObjId, this._otMaterialTypeID, this.objTypeID);
        if (techRelParam != null)
          techRelList.Add(techRelParam);
      }
    }
    else if (int32_5 != 0)
    {
      if (!this.IsCloneRecord(recBase))
      {
        long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_main, ImportingCategory.TechZagot, (object) int32_5);
        if (newKey != 0L)
        {
          TechRelParam techRelParam = new TechRelParam(newKey, ipsObjId, relTechRelationId, this._otZagotTypeID, this.objTypeID);
          techRelList.Add(techRelParam);
        }
      }
      else
      {
        TechRelParam techRelParam = this.AddRelationByObject(ImportingCategory.TechZagot, (object) int32_5, relTechRelationId, recBase, ipsObjId, this._otZagotTypeID, this.objTypeID);
        if (techRelParam != null)
          techRelList.Add(techRelParam);
      }
    }
    return techRelList;
  }

  public override void Exam() => base.Exam();

  public override void Pump() => base.Pump();

  protected override Guid GUID => this._guid;
}
