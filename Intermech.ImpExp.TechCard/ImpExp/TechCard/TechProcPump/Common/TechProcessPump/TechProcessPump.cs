// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump.TechProcessPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Extensions;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.ArcArtPump;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Document;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Interfaces;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcPump;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump;
using Intermech.ImpExp.TechCard.TechProcPump.Common.TechDiff;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common.TechProcessPump;

[TaskDescription("Инициализация данных для перекачки - Техпроцесс", "Перекачка данных - Техпроцесс")]
internal class TechProcessPump(PluginClass plugin) : TechPumpBase(plugin)
{
  private IDictionary<int, ICollection<long>> _tpList = (IDictionary<int, ICollection<long>>) new Dictionary<int, ICollection<long>>();
  private IDictionary<int, ICollection<int>> _tpBasicList = (IDictionary<int, ICollection<int>>) new Dictionary<int, ICollection<int>>();
  private bool _tpComplectPumpMode;
  private string _tpComplectPumpDir = string.Empty;
  private readonly ICollection<long> _importesObjectFromSearchHashSet = (ICollection<long>) new HashSet<long>();
  private readonly ICollection<int> _obligatorySearchDocAttributes = (ICollection<int>) new HashSet<int>();
  private int[] _obligatorySearchDocAttrArray;
  private IPortalSearchDocumentVersionCache _portalImportedTPs;
  private readonly Guid _guid = new Guid("{C758FB63-CFB0-48cf-9F51-8A45E0D0DA9A}");
  protected int _otTechTPGroupObjTypeID = -1;
  protected int _otTechTPTypeObjTypeID = -1;
  protected int _otTechTPOneObjTypeID = -1;
  protected int _otTechTpBaseObjTypeID = -1;
  protected IAttributeTypeItem _atProductionAttrType;
  protected IAttributeTypeItem _atFileAttrType;
  protected IAttributeTypeItem _atGtpContextAttr;
  protected IAttributeTypeItem _atBasicTpAttrType;

  protected override void InitData()
  {
    this._recType = "P";
    this._recTypeID = 15;
    this._tableName = "TC_ARCDOCS";
    this._dopTypes.Add("I");
    this._dopTypes.Add("F");
    this._dopTypes.Add("S");
    this._dopTypes.Add("D");
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
      IObjectTypeItem byGuid1 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechTPGroupObjTypeGuid);
      if (byGuid1 != null)
        this._otTechTPGroupObjTypeID = byGuid1.ID;
      IObjectTypeItem byGuid2 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechTPTypeObjTypeGuid);
      if (byGuid2 != null)
        this._otTechTPTypeObjTypeID = byGuid2.ID;
      IObjectTypeItem byGuid3 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechTPOneObjTypeGuid);
      if (byGuid3 != null)
        this._otTechTPOneObjTypeID = byGuid3.ID;
      IObjectTypeItem byGuid4 = imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otTechTPBaseObjTypeGuid);
      if (byGuid4 != null)
        this._otTechTpBaseObjTypeID = byGuid4.ID;
      IAttributeTypeItem byGuid5 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atProductionAttrTypeGuid);
      if (byGuid5 != null)
        this._atProductionAttrType = byGuid5;
      IAttributeTypeItem byGuid6 = imdi.AttributeTypes.GetByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"));
      if (byGuid6 != null)
        this._atFileAttrType = byGuid6;
      IAttributeTypeItem byGuid7 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atGtpContextAttrGUID);
      if (byGuid7 != null)
        this._atGtpContextAttr = byGuid7;
      IAttributeTypeItem byGuid8 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atBasicTpAttrGuid);
      if (byGuid8 != null)
        this._atBasicTpAttrType = byGuid8;
      if (TechSettingsHelper.TPComplectPumpMode)
      {
        this._tpComplectPumpMode = TechSettingsHelper.TPComplectPumpMode;
        this._tpComplectPumpDir = TechSettingsHelper.TPComplectPumpDir;
        if (!Directory.Exists(TechSettingsHelper.TPComplectPumpDir))
          this._tpComplectPumpMode = false;
        if (this._tpComplectPumpMode && !this.objAnyAttr && !this.objAttrList.Contains(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545")))
          this._tpComplectPumpMode = false;
      }
      this.LoadObligatorySearchAttributes();
    }
  }

  protected override void LoadMetaData4StoppedPump() => this.entTypeRec = this.GetEntityTypeRec();

  protected void LoadObligatorySearchAttributes()
  {
    IMetadataInfo imdi = this.plugin.Imdi;
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad0132b-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad007a4-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(SystemGUIDs.attributeArchive).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad007a1-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad007a2-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad00255-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad00770-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad0077d-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad0079e-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad0079f-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad007a0-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttributes.Add(imdi.AttributeTypes.GetByGuid(new Guid("cad0013a-306c-11d8-b4e9-00304f19f545")).ID);
    this._obligatorySearchDocAttrArray = this._obligatorySearchDocAttributes.ToArray<int>();
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechProcessPump;

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[6]
    {
      ImportingCategory.Documents,
      ImportingCategory.Users,
      ImportingCategory.ObjectGUIDs,
      ImportingCategory.IdGuids,
      ImportingCategory.TcKeyToObjGuid,
      ImportingCategory.BaseTechObjectsVersionsCache
    };
  }

  protected override ImportingCategory[] GetCategoriesByNeed2CreateTechRel()
  {
    return new ImportingCategory[2]
    {
      ImportingCategory.Documents,
      ImportingCategory.Articles
    };
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
      this._dataSource = new TechDataSource((ITechDataBuilder) new TechProcDataBuilder<TechPumpBase>((TechPumpBase) this));
    return this._dataSource;
  }

  protected string GetPumpModeCond(string condField, string dopType)
  {
    return TechDataBuilder<PumpClass>.GetPumpModeCond(condField, -2);
  }

  protected virtual IDataReader GetCheckRecordReader()
  {
    if (!this.TableExists(this._tableName))
      return (IDataReader) null;
    string sqlCommon;
    new TechProcDataBuilder<TechPumpBase>((TechPumpBase) this).GetTechDataReaderSqlCommands(out sqlCommon, out string _);
    return this.GetDataReader(sqlCommon + " AND  ( F_PRODUCTION <> 19 AND    ( a.F_TCKEY <> b.F_KEY OR       a.F_DESIGNATION IS NULL AND      b.F_DESIGNATION IS NULL AND      a.F_NAME IS NULL AND      a.F_NAME IS NULL    )  )");
  }

  private void LoadDopInfo()
  {
    try
    {
      string str = string.Empty;
      if (this._lastObjID != 0L)
        str = $" AND {"F_OBJ_KEY"} > {this._lastObjID}";
      string pumpModeCond = this.GetPumpModeCond("F_OBJ_KEY", string.Empty);
      if (pumpModeCond != string.Empty)
        str = $"{str} AND {pumpModeCond}";
      IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
      command.CommandText = "SELECT F_OBJ_KEY, F_ART_TCKEY, F_KEY FROM  TC_OBJ2LINK WHERE F_OBJ_TYPE = 1 " + str;
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal1 = dataReader.GetOrdinal("F_KEY");
        int ordinal2 = dataReader.GetOrdinal("F_OBJ_KEY");
        int ordinal3 = dataReader.GetOrdinal("F_ART_TCKEY");
        while (dataReader.Read())
        {
          int int32_1 = BasePumpHelper.ToInt32(dataReader[ordinal1]);
          int int32_2 = BasePumpHelper.ToInt32(dataReader[ordinal2]);
          int int32_3 = BasePumpHelper.ToInt32(dataReader[ordinal3]);
          if (int32_2 != 0)
          {
            if (this._tpList.ContainsKey(int32_2))
              this._tpList[int32_2].Add(TechcardConsts.Utils.CodeHashCode(int32_1, int32_3));
            else
              this._tpList.Add(int32_2, (ICollection<long>) new List<long>()
              {
                TechcardConsts.Utils.CodeHashCode(int32_1, int32_3)
              });
          }
        }
        dataReader.Close();
      }
      this._portalImportedTPs = ApplicationServices.Container.GetService<IPortalSearchDocumentVersionCache>();
      if (!this._portalImportedTPs.Loaded)
        this._portalImportedTPs.Load();
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddErrorMessage($"Невозможно загрузить информацию о техпроцессах: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    try
    {
      string str = string.Empty;
      if (this._lastObjID != 0L)
        str = $" AND {"F_OBJKEY"} > {this._lastObjID}";
      string pumpModeCond = this.GetPumpModeCond("F_OBJKEY", string.Empty);
      if (pumpModeCond != string.Empty)
        str = $"{str} AND {pumpModeCond}";
      IDbCommand command = TechcardConsts.ConnectionManager.CreateCommand();
      command.CommandText = "SELECT F_OBJKEY, F_ARTKEY, F_KEY FROM  TP_BASICDOC WHERE F_OBJTYPE = 1 " + str;
      using (IDataReader dataReader = command.ExecuteReader(TechcardConsts.ConnectionManager.CommandBehavior))
      {
        int ordinal4 = dataReader.GetOrdinal("F_OBJKEY");
        int ordinal5 = dataReader.GetOrdinal("F_ARTKEY");
        while (dataReader.Read())
        {
          int int32_4 = BasePumpHelper.ToInt32(dataReader[ordinal4]);
          int int32_5 = BasePumpHelper.ToInt32(dataReader[ordinal5]);
          if (int32_4 != 0)
          {
            ICollection<int> ints;
            if (!this._tpBasicList.TryGetValue(int32_4, out ints))
            {
              ints = (ICollection<int>) new HashSet<int>();
              this._tpBasicList[int32_4] = ints;
            }
            ints.Add(int32_5);
          }
        }
        dataReader.Close();
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddErrorMessage($"Невозможно загрузить информацию о базовых техпроцессах: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
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
        if (recordPumpMode != string.Empty && recordPumpMode != "_")
        {
          Conflict conflict = new Conflict(tpObjRec.Key, TechcardConsts.TypeConsts.otTechTPBaseObjTypeGuid, Convert.ToString(tpObjRec.Fields["F_NAME"]), Convert.ToString(tpObjRec.Fields["F_DESIGNATION"]), recordPumpMode);
          TechCardPlugin.InitializationConflictList.Add(conflict);
        }
      }
      checkRecordReader.Close();
    }
  }

  protected override string GetRecordPumpMode(TechObjectRecord record)
  {
    int int32_1 = Convert.ToInt32(record.Fields["F_KEY1"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_TCKEY"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_PRODUCTION"]);
    int int32_4 = Convert.ToInt32(record.Fields["F_VERSION"]);
    string str1 = Convert.ToString(record.Fields["F_DESIGNATION"]);
    string str2 = Convert.ToString(record.Fields["F_NAME"]);
    string str3 = Convert.ToString(record.Fields["F_DESIGNATION1"]);
    string str4 = Convert.ToString(record.Fields["F_NAME1"]);
    if (int32_3 == 19)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return string.Empty;
    }
    if (int32_2 != int32_1)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return int32_4 == 0 ? "нет данных по техпроцессу, необходимо пересохранить техпроцесс" : "нет ссылки документа на версию";
    }
    if (str1 == string.Empty && str2 == string.Empty && str3 == string.Empty && str4 == string.Empty)
    {
      record.RecMode = TechObjectRecord.PumpMode.NotPump;
      return "не указано имя техпроцесса";
    }
    record.RecMode = TechObjectRecord.PumpMode.ObjectAndLinks;
    return string.Empty;
  }

  protected override TechObjectRecord GetTpObjRec() => (TechObjectRecord) new TechProcessObject();

  protected override List<TechRelParam> CreateTechRelList(
    TechObjectRecordBase recBase,
    long ipsObjId)
  {
    return new List<TechRelParam>();
  }

  protected override long AddHorizontalRelation(
    long parentRelId,
    long childRelId,
    Guid relationGuid)
  {
    long num = -1;
    int techGtpRelationId = this._relTechGTPRelationID;
    RelationRecord relationRecord = this._impRelList.AddRelation(parentRelId, childRelId, techGtpRelationId);
    if (relationRecord != null)
    {
      num = relationRecord.PrjLinkId;
      if (relationGuid != Guid.Empty && this._atTechLinkAtRelGTPRelation != null)
        this._impRelList.AddAttributeStr(this._atTechLinkAtRelGTPRelation.ID, relationGuid.ToString());
    }
    this.FillLinkObligatoryAttributes();
    return num;
  }

  protected override void PumpDiffRec(TechObjectRecord record)
  {
    if (record == null)
      return;
    int int32_1 = Convert.ToInt32(record.Fields["F_KIND"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_TCKEY"]);
    if (int32_1 != 7 && int32_1 != 6)
      return;
    List<int> intList = new List<int>();
    foreach (TechDiffElement techDiffElement in TechDiffCache.DiffRecList.GetArtListByObjID(record.Key))
    {
      if (!intList.Contains(techDiffElement.ArtTcKey))
      {
        TechObjectRecord cloneRecord = this.CreateCloneRecord(record);
        cloneRecord.Key = -techDiffElement.Key;
        cloneRecord.diff_ArtTcKey = techDiffElement.ArtTcKey;
        intList.Add(techDiffElement.ArtTcKey);
        this._techParmList = new TechParamList();
        this.LoadTechParams(cloneRecord);
        this.PumpBaseRec(cloneRecord);
      }
    }
    ICollection<long> longs;
    if (!this._tpList.TryGetValue(int32_2, out longs))
      return;
    foreach (long hashCode in (IEnumerable<long>) longs)
    {
      int recTypeId;
      int recKey;
      TechcardConsts.Utils.DecodeHashCode(hashCode, out recTypeId, out recKey);
      if (!intList.Contains(recKey))
      {
        TechObjectRecord cloneRecord = this.CreateCloneRecord(record);
        cloneRecord.Key = -recTypeId;
        cloneRecord.diff_ArtTcKey = recKey;
        this._techParmList = new TechParamList();
        this.LoadTechParams(cloneRecord);
        this.PumpBaseRec(cloneRecord);
        intList.Add(recKey);
      }
    }
  }

  protected override ObjectRecord CreateTechObject(TechObjectRecord record)
  {
    long objectID = 0;
    bool flag = this.IsCloneRecord((TechObjectRecordBase) record);
    int int32_1 = Convert.ToInt32(record.Fields["F_VERSION"]);
    int procTypeId;
    if (flag)
    {
      procTypeId = 1;
    }
    else
    {
      int docId = Math.Abs(Convert.ToInt32(record.Fields["F_DOCID"]));
      objectID = this.GetIpsObjectIdByDocId(docId, int32_1).IpsObjectVerId;
      procTypeId = Convert.ToInt32(record.Fields["F_KIND"]);
      if (objectID == 0L)
        this.plugin.appManager.AddInfoMessage($"Технологический документ '{Convert.ToString(record.GetFieldValue("F_DESIGNATION"))}' (F_KEY = {record.Key} F_DOC_ID={docId}) не найден в списке мигрированных документов Search");
    }
    int objType4ProcType = this.GetIpsObjType4ProcType(procTypeId);
    if (objType4ProcType == 0)
      return (ObjectRecord) null;
    ObjectRecord objectRec;
    if (objectID == 0L | flag || this._importesObjectFromSearchHashSet.Contains(objectID))
    {
      objectRec = this._impObjList.AddObject(objType4ProcType, 0);
      objectRec.VersionId = int32_1;
      if (!flag)
      {
        int int32_2 = Convert.ToInt32(record.Fields["F_TCKEY"]);
        string caption = this._import_data_main.GetCaption(ImportingCategory.TcKeyToObjGuid, (object) int32_2);
        Guid result;
        if (!Guid.TryParse(caption, out result))
          result = Guid.Empty;
        if (int32_1 != 0)
        {
          objectRec.Id = 0L;
          objectRec.ObjectVerType = 1;
          objectRec.ParentVersionNo = int32_1 - 1;
          (long IpsObjectVerId, Guid IpsObjectGuid) ipsObjectIdByDocId = this.GetIpsObjectIdByDocId(Convert.ToInt32(record.Fields["F_DOCID"]), int32_1 - 1);
          objectRec.ParentVersionId = ipsObjectIdByDocId.IpsObjectVerId;
          if (result != ipsObjectIdByDocId.IpsObjectGuid)
          {
            this.plugin.appManager.AddWarningMessage($"Guid объектов в кэшах документов Search и Techcard отличаются! {ipsObjectIdByDocId.IpsObjectGuid} <> {result}");
            result = ipsObjectIdByDocId.IpsObjectGuid;
          }
        }
        if (result != Guid.Empty)
          objectRec.IdGuid = (object) result;
        if (caption != objectRec.IdGuid.ToString())
        {
          DictionaryValue dictionaryValue = this._import_data_main.GetValue(ImportingCategory.TcKeyToObjGuid, (object) int32_2);
          if (dictionaryValue == null)
            this._import_data_main.AddValue(ImportingCategory.TcKeyToObjGuid, (object) int32_2, 0L, objectRec.IdGuid.ToString());
          else
            dictionaryValue.Caption = objectRec.IdGuid.ToString();
        }
      }
    }
    else
    {
      this._importesObjectFromSearchHashSet.Add(objectID);
      try
      {
        this._impObjList.UseObject(objectID);
      }
      catch (Exception ex)
      {
        this.RemoveBaseRec((TechObjectRecordBase) record);
        this.plugin.appManager.AddWarningMessage(string.Format("Невозможно модифицировать существующий объект ТП (typeID={3}) \"{0}\" по причине: {1}{2}", (object) objectID, (object) ex.Message, (object) (Environment.NewLine + ex.StackTrace), (object) this._recTypeID));
        if (ex is OutOfMemoryException)
          throw;
        this.DoHandleImportObjectsException(ex);
        return (ObjectRecord) null;
      }
      objectRec = new ObjectRecord();
      objectRec.ObjectGuid = (object) Guid.Empty;
      objectRec.Object_id = objectID;
      objectRec.ObjectType = objType4ProcType;
    }
    int currentIndex = this._impObjList.Items.CurrentIndex;
    this._techBaseImportList.Add((TechObjectRecordBase) record, currentIndex);
    this.FillTechObject(objectRec, record);
    if (objectRec.IsBaseVersion)
    {
      long oldKey = TechPumpBase.GenBaseTechObjectsVersionsCacheKey(record.Key, LinkedObjectType.TechProc);
      if (this._import_data_main.GetValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) oldKey) == null)
        this._import_data_main.AddValue(ImportingCategory.BaseTechObjectsVersionsCache, (object) oldKey, (long) int32_1);
    }
    return objectRec;
  }

  private EntityTypeRec GetEntityTypeRec()
  {
    this.entTypeRec = TechPumpData.EntTypeList.GetRecByType(15);
    EntityTypeRec recByType = TechPumpData.EntTypeList.GetRecByType(8);
    if (recByType != null)
    {
      foreach (Entity entity in recByType.CodeList.Values)
        this.entTypeRec.AddEntity(entity);
    }
    return this.entTypeRec;
  }

  public override void Exam()
  {
    this.entTypeRec = this.GetEntityTypeRec();
    bool flag;
    using (IDataReader dataReader = this.GetDataReader(string.Format("SELECT \r\n                                    a.{0}\r\n                                FROM   \r\n                                    {1} a, {2} b\r\n                                WHERE \r\n                                    a.{0} = b.{3} and\r\n                                    b.{4} > 0   \r\n                                GROUP BY\r\n                                    a.{0}, a.{5}\r\n                                HAVING COUNT (*) > 1 ", (object) "F_TCKEY", (object) "TP_VERSIONS", (object) "TC_ARCDOCS", (object) "F_KEY", (object) "F_DOCID", (object) "F_VERSION")))
      flag = dataReader.Read();
    if (flag && MessageBox.Show($"В базе Imbase обнаружены не уникальные ТП!{Environment.NewLine}Рекомендуется перед импортом запустить в TechCFG команду 'Администрирование'-'Корректировка базы'. Прервать импорт ?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
      Application.Exit();
    base.Exam();
  }

  public override void Pump() => base.Pump();

  protected override void PumpLoadData()
  {
    this.LoadDopInfo();
    base.PumpLoadData();
  }

  protected override void PumpLoadTechDiffData()
  {
    if (TechDiffCache.DiffPumper != null)
    {
      TechDiffCache.DiffPumper.LoadDiffData((int) this._lastObjID, this._recTypeID, 8);
      this._allowDiffObjects = TechDiffCache.DiffRecList.Count > 0;
    }
    else
      this._allowDiffObjects = false;
  }

  protected override void FillTechObject(ObjectRecord objRecord, TechObjectRecord record)
  {
    string str1;
    string str2 = str1 = Convert.ToString(record.Fields["F_DESIGNATION"]);
    string str3;
    string str4 = str3 = Convert.ToString(record.Fields["F_NAME"]);
    string str5 = Convert.ToString(record.Fields["F_DESIGNATION1"]);
    string str6 = Convert.ToString(record.Fields["F_NAME1"]);
    string empty = string.Empty;
    if (str1.Equals(empty))
      str2 = str5;
    if (str3.Equals(string.Empty))
      str4 = str6;
    int int32_1 = Convert.ToInt32(record.Fields["F_PRODUCTION"]);
    int int32_2 = Convert.ToInt32(record.Fields["F_USER"]);
    int int32_3 = Convert.ToInt32(record.Fields["F_VERSION"]);
    int int32_4 = Convert.ToInt32(record.Fields["F_DOCID"]);
    IpsProductionObj ipsProductionObj;
    if (TechPumpData.Production.Productions.TryGetValue(int32_1, out ipsProductionObj) && ipsProductionObj == null)
      this.plugin.appManager.AddWarningMessage($"Вид производства № {(object) int32_1} не найден");
    if (this.IsCloneRecord((TechObjectRecordBase) record))
    {
      string str7 = record.diff_ArtTcKey.ToString();
      ArcArtsObject arcArtsObject;
      if (TechPumpData.TechObjects.ArcArtList.TryGetValue((long) record.diff_ArtTcKey, out arcArtsObject))
      {
        str7 = !string.IsNullOrEmpty(arcArtsObject.Designation) ? arcArtsObject.Designation : arcArtsObject.ArtId.ToString();
        str4 = !string.IsNullOrEmpty(arcArtsObject.Name) ? arcArtsObject.Name : str4;
      }
      str2 = ipsProductionObj != null ? $"{str7} {ipsProductionObj.ProdInfo.Loc_Litera}1 ТП [{str2}]" : $"{str7} ТП [{str2}]";
    }
    if (objRecord != null)
      objRecord.Caption = str2.Truncate(Consts.MaxStringSize - 2);
    if (this._atProductionAttrType != null && ipsProductionObj != null)
      this._techParmList.AddAttribute(this._atProductionAttrType, (object) ipsProductionObj.ObjID, ipsProductionObj.ProdInfo.Name);
    if (this._atNaimAttrType != null)
      this._techParmList.AddOrUpdateEntity("NDoc", (object) str4);
    if (this._atObozAttrType != null)
      this._techParmList.AddOrUpdateEntity("ODoc", (object) str2);
    if (this._atLastLevelSeek != null)
      this._techParmList.AddAttribute(this._atLastLevelSeek, (object) (long) this.GetFirstStepLifecycle());
    if (objRecord != null)
    {
      DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(int32_2);
      if (userInfoBySearchId != null)
      {
        objRecord.OwnerId = userInfoBySearchId.NewObjectID;
        if (userInfoBySearchId.Tag is UserTag tag)
          objRecord.OwnerGuid = (object) tag.Guid;
      }
    }
    if (this._tpComplectPumpMode && this._atFileAttrType != null)
    {
      string str8 = $"{this._tpComplectPumpDir}\\{$"{int32_4}_{int32_3}.tpr"}";
      FileInfo fileInfo = new FileInfo(str8);
      if (fileInfo.Exists)
      {
        AttributeRecord attributeRecord = this._impObjList.AddAttributeBlob(this._atFileAttrType.ID, str8, fileInfo.Length, str2 + ".tpr", ArcMethods.NotPacked);
        DocumentTag tag = (DocumentTag) this._import_data_main.GetTag(ImportingCategory.Documents, (object) int32_4);
        AddVersionInfo addVersionInfo;
        if (tag != null && tag.AddVersionInfo != null && tag.AddVersionInfo.TryGetValue(int32_3, out addVersionInfo) && addVersionInfo != null)
          attributeRecord.InlistId = (int) addVersionInfo.FileCount;
      }
    }
    int result = 0;
    object entityValue = this._techParmList.GetEntityValue("Ceh");
    if (entityValue != null)
      int.TryParse(entityValue.ToString(), out result);
    record.Fields["F_CEH_ID"] = (object) result;
    if (this.IsCloneRecord((TechObjectRecordBase) record) && this._atGtpContextAttr != null)
      this._techParmList.AddAttribute(this._atGtpContextAttr, (object) true);
    ICollection<int> ints;
    if (this._tpBasicList.TryGetValue(Math.Abs(record.Key), out ints) && (this.IsCloneRecord((TechObjectRecordBase) record) ? (ints.Contains(record.diff_ArtTcKey) ? 1 : 0) : (ints.Count >= 1 ? 1 : 0)) != 0)
      this._techParmList.AddAttribute(this._atBasicTpAttrType, (object) true);
    base.FillTechObject(objRecord, record);
  }

  protected int GetIpsObjType4ProcType(int procTypeId)
  {
    int objType4ProcType = -1;
    switch (procTypeId)
    {
      case 1:
      case 4:
        objType4ProcType = this._otTechTPOneObjTypeID;
        break;
      case 6:
        objType4ProcType = this._otTechTPTypeObjTypeID;
        break;
      case 7:
        objType4ProcType = this._otTechTPGroupObjTypeID;
        break;
    }
    return objType4ProcType;
  }

  protected override void AddValue2Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase,
    TechParamList recParmList)
  {
    if (!this.IsCloneRecord(recBase))
    {
      long ipsObjectVerId = this.GetIpsObjectIdByDocId(Convert.ToInt32(recBase.Fields["F_DOCID"]), Convert.ToInt32(recBase.Fields["F_VERSION"])).IpsObjectVerId;
      if (ipsObjectVerId != 0L)
        newKey = ipsObjectVerId;
    }
    base.AddValue2Cache(oldKey, newKey, recBase, recParmList);
  }

  protected override TechRecordObjectTag GetTagValue4Cache(
    object oldKey,
    long newKey,
    TechObjectRecordBase recBase)
  {
    int objType4ProcType = this.GetIpsObjType4ProcType(!this.IsCloneRecord(recBase) ? Convert.ToInt32(recBase.Fields["F_KIND"]) : 1);
    int result = 0;
    int int32_1 = Convert.ToInt32(recBase.Fields["F_PRODUCTION"]);
    int int32_2 = Convert.ToInt32(recBase.Fields["F_USER"]);
    object obj;
    recBase.Fields.TryGetValue("F_CEH_ID", out obj);
    if (obj != null)
      int.TryParse(obj.ToString(), out result);
    int cehCode = result;
    TechProcCacheInfo techObject = new TechProcCacheInfo(objType4ProcType, cehCode)
    {
      ProductionCode = int32_1,
      UserCode = int32_2
    };
    DictionaryValue userInfoBySearchId = this.GetUserInfoBySearchId(int32_2);
    if (userInfoBySearchId != null)
    {
      techObject.OwnerId = userInfoBySearchId.NewObjectID;
      if (userInfoBySearchId.Tag is UserTag tag)
        techObject.OwnerGuid = tag.Guid;
    }
    return new TechRecordObjectTag((object) techObject);
  }

  protected (long IpsObjectVerId, Guid IpsObjectGuid) GetIpsObjectIdByDocId(int docId, int verId)
  {
    DocumentTag tag = (DocumentTag) this._import_data_main.GetTag(ImportingCategory.Documents, (object) docId);
    long num;
    if (tag?.Versions != null && tag.Versions.TryGetValue(verId, out num))
    {
      if (!MetaDataHelper.IsObjectTypeChildOf(this.plugin.Imdi.ImportedObjects.GetObjectTypeID(num), this._otTechTpBaseObjTypeID))
        return (0L, Guid.Empty);
      Guid result = Guid.Empty;
      long newKey = this._import_data_main.GetNewKey(ImportingCategory.ObjectGUIDs, (object) num);
      if (newKey != 0L && !Guid.TryParse(this._import_data_main.GetCaption(ImportingCategory.IdGuids, (object) newKey), out result))
        result = Guid.Empty;
      return (num, result);
    }
    PortalSearchDocumentVersion objectInCache = this._portalImportedTPs.FindObjectInCache(this._portalImportedTPs.GetUniqueObjId((object) docId, (object) verId));
    return objectInCache == null ? (0L, Guid.Empty) : (objectInCache.IpsObjVerId, objectInCache.IpsObjGuid);
  }

  protected override void AddAttribute2ImpObjectList(ITechParamAttribute techAttribute)
  {
    ObjectRecord objectRecord = this._impObjList.Items[this._impObjList.Items.CurrentIndex].Object;
    if ((objectRecord.ObjectGuid == null ? 1 : (objectRecord.ObjectGuid.Equals((object) Guid.Empty) ? 1 : 0)) != 0 && techAttribute.AttributeType != null && this._obligatorySearchDocAttributes.Contains(techAttribute.AttributeType.ID))
      return;
    base.AddAttribute2ImpObjectList(techAttribute);
  }

  protected override void FillObjectObligatoryAttributes(TechObjectRecord record)
  {
    int objectIndex;
    if (!this._techBaseImportList.TryGetValue((TechObjectRecordBase) record, out objectIndex))
      this.plugin.appManager.AddWarningMessage("Не найден в кэше индекс для записи Key=" + (object) record.Key);
    else
      AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Imdi.UserSession, this._impObjList, objectIndex, this._obligatorySearchDocAttrArray);
  }

  protected override void ReleasePumpData()
  {
    this._tpList?.Clear();
    this._tpList = (IDictionary<int, ICollection<long>>) null;
    this._tpBasicList?.Clear();
    this._tpBasicList = (IDictionary<int, ICollection<int>>) null;
    this._importesObjectFromSearchHashSet?.Clear();
    this._obligatorySearchDocAttributes?.Clear();
    base.ReleasePumpData();
  }

  protected override Guid GUID => this._guid;
}
