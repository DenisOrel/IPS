// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump.ScenarioPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Intermech.Diagnostics;
using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using Intermech.ImpExp.TechCard.Condition4Type;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;

[TaskDescription("Инициализация перекачки сценариев Techcard", "Перекачка сценариев Techcard")]
[TaskType(PumperType.MetaData)]
internal class ScenarioPump : TechPumpBase
{
  private readonly Guid _guid = new Guid("C62DE36F-3253-47e2-A9C8-7221BF7B77C7");
  private List<Scenario> _scenarioList = new List<Scenario>();
  private Dictionary<string, int> _scenarioCaption2Idx = new Dictionary<string, int>();
  private int _formListAtrTypeId;

  protected override ImportingCategory[] GetCategoriesByNeed2FillTechObject()
  {
    return new ImportingCategory[5]
    {
      ImportingCategory.ImbaseBlobs,
      ImportingCategory.TechVidIzdPump,
      ImportingCategory.TechVidZagPump,
      ImportingCategory.ImbaseCatalogs,
      ImportingCategory.TechCeh
    };
  }

  private QuickObjectInfo GetImageGuidBySlideInfo(int slideId, IUserSession session)
  {
    long newKey = this._import_data_main.GetNewKey(ImportingCategory.ImbaseBlobs, (object) slideId);
    if (newKey != 0L)
      return session.GetObjectInfo(newKey);
    return new QuickObjectInfo() { ObjectTypeID = -1 };
  }

  private string GetScenarioCaption(Scenario scenario)
  {
    string name = scenario.ToString();
    IpsProductionObj production = this.GetProduction(scenario);
    if (production != null && production.ProdInfo.ProductionID != 19)
      name = $"{name} ({production.ProdInfo.Name})";
    return this.GetUniqueName(name);
  }

  private string GetUniqueName(string name)
  {
    int num1;
    if (this._scenarioCaption2Idx.TryGetValue(name, out num1))
    {
      int num2 = num1 + 1;
      this._scenarioCaption2Idx[name] = num2;
      name = $"{name} {num2}";
    }
    else
      this._scenarioCaption2Idx.Add(name, num1);
    return name;
  }

  private void CorrectScriptDefaultValue([NotNull] Scenario scenario)
  {
    for (int index1 = 1; index1 < scenario.RowCount; ++index1)
    {
      for (int index2 = 0; index2 < scenario.ColCount; ++index2)
      {
        ScenarioCell cell = scenario.Cells[index2, index1];
        if (cell != null && !string.IsNullOrEmpty(cell.DefaultValue))
        {
          CellValueType cellValueType = cell.Type;
          if (index2 >= 1 && index1 >= 1)
            cellValueType = CellValueType.Code;
          string defaultValue;
          if (cellValueType == CellValueType.Code && this.GetScriptDefaultValue(cell, out defaultValue))
            cell.DefaultValue = defaultValue;
        }
      }
    }
  }

  private bool GetScriptDefaultValue([NotNull] ScenarioCell cell, out string defaultValue)
  {
    defaultValue = cell.DefaultValue;
    int result;
    Entity entity;
    if (!int.TryParse(defaultValue, out result) || !TechPumpData.Entities.EntitiesList.TryGetValue(cell.Value, out entity))
      return false;
    DictionaryValue dictionaryValue = ImbaseLinkConvertor.Instance.ConvertValue(entity, result, this._import_data_imbase);
    if (dictionaryValue == null)
      return false;
    if (string.IsNullOrEmpty(dictionaryValue.Caption))
    {
      QuickObjectInfo objectInfo = this.plugin.Idw.GetUserSession().GetObjectInfo(dictionaryValue.NewObjectID);
      if (!objectInfo.Empty)
        defaultValue = dictionaryValue.Caption = objectInfo.Caption;
    }
    else
      defaultValue = dictionaryValue.Caption;
    return true;
  }

  private IpsProductionObj GetProduction(Scenario scenario)
  {
    if (scenario.Property?.Catalog == null)
      return (IpsProductionObj) null;
    if (TechPumpData.Production.Productions == null)
      return (IpsProductionObj) null;
    IpsProductionObj production;
    TechPumpData.Production.Productions.TryGetValue(scenario.Property.Catalog.Production, out production);
    return production;
  }

  private void AddTpObjectType(Scenario scenario, IAttributeTypeItem atGuidOtsAt)
  {
    Guid importingObjectType = ScenarioUtils.GetImportingObjectType(scenario);
    switch (scenario.Kind)
    {
      case ScenarioKind.Zagot:
      case ScenarioKind.Mat:
      case ScenarioKind.Comm:
      case ScenarioKind.Dce:
      case ScenarioKind.RouteTemplate:
      case ScenarioKind.RouteElement:
      case ScenarioKind.ZagSourceSearch:
      case ScenarioKind.ZagSourceImbase:
      case ScenarioKind.GroupComm:
      case ScenarioKind.TypeComm:
        this._impObjList.AddAttribute(atGuidOtsAt.ID, AttrValueType.stringVal, (object) importingObjectType, 0);
        break;
    }
  }

  private byte[] Pack(MemoryStream ms)
  {
    MemoryStream baseOutputStream = new MemoryStream();
    DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream((Stream) baseOutputStream, new Deflater(3));
    deflaterOutputStream.Write(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
    deflaterOutputStream.Flush();
    deflaterOutputStream.Finish();
    return baseOutputStream.GetBuffer();
  }

  private void WriteCondition(Scenario scenario, int attrTypeId, ScenarioKind kidsId)
  {
    object condition4ThisObject = this.GetCondition4ThisObject(scenario, kidsId);
    if (condition4ThisObject == null)
      return;
    string tempFileName = Path.GetTempFileName();
    try
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      MemoryStream ms = new MemoryStream();
      MemoryStream serializationStream = ms;
      object graph = condition4ThisObject;
      binaryFormatter.Serialize((Stream) serializationStream, graph);
      byte[] buffer = this.Pack(ms);
      int length = buffer.Length;
      FileStream fileStream = new FileStream(tempFileName, FileMode.Create);
      fileStream.Write(buffer, 0, length);
      fileStream.Flush();
      fileStream.Close();
      this._impObjList.AddAttributeBlob(attrTypeId, tempFileName, Convert.ToInt64(length), "Condition", ArcMethods.ZLibPacked);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Невозможно создать условие на сценарий по причине: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  private object GetCondition4ThisObject(Scenario scenario, ScenarioKind kidsId)
  {
    TempFormula baseFormula = (TempFormula) null;
    switch (kidsId)
    {
      case ScenarioKind.Zagot:
        if (scenario.Property.VidDet != 0 || scenario.Property.VidZag != 0)
        {
          baseFormula = Condition4Zag.GetCondition((TempFormula) null, scenario, this._import_data_main);
          break;
        }
        break;
      case ScenarioKind.ZagSourceSearch:
        baseFormula = Condition4Search.GetCondition((TempFormula) null, scenario);
        break;
      case ScenarioKind.ZagSourceImbase:
        baseFormula = Condition4Imbase.GetCondition((TempFormula) null, scenario);
        break;
    }
    if (((IEnumerable<ScenarioKind>) new ScenarioKind[3]
    {
      ScenarioKind.Comm,
      ScenarioKind.GroupComm,
      ScenarioKind.TypeComm
    }).Contains<ScenarioKind>(scenario.Kind) && scenario.Property.Catalog != null && scenario.Property.Catalog.Production != 19)
      baseFormula = Condition4Production.GetCondition(baseFormula, scenario, this._import_data_main);
    if (scenario.Property != null && scenario.Property.Catalog != null && scenario.Property.Catalog.CatalogId > 0)
      baseFormula = Condition4ObjType.GetCondition(baseFormula, scenario, this._import_data_main);
    if (baseFormula != null)
    {
      baseFormula.AutoConvert = false;
      baseFormula.DropMeasure = true;
      int BadToken;
      string errorMsg;
      if (!baseFormula.Compile(out BadToken, out errorMsg))
      {
        string str = BadToken != -1 ? $"({baseFormula[BadToken].text})" : string.Empty;
        this.plugin.appManager.AddWarningMessage($"Ошибка компиляции формулы \"{baseFormula.Text}\": {errorMsg} {str}");
      }
    }
    return (object) baseFormula;
  }

  private void impObjList_AfterImportEvent(object sender, EventArgs e)
  {
    try
    {
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      if (userSession == null)
      {
        this.plugin.appManager.AddWarningMessage("Невозможно связать \"Формы редактирования атрибутов\" со справочниками т.к. невозможно получить сессию.");
      }
      else
      {
        IImportedObjectList importedObjectList = sender as IImportedObjectList;
        for (int index = 0; index < importedObjectList.Items.Count; ++index)
        {
          try
          {
            ImportingObject importingObject = importedObjectList.Items[index];
            if (importingObject != null)
            {
              if (importingObject.Object != null)
              {
                if (importingObject.Object.ObjectGuid is Guid)
                {
                  Guid ipsObjGuid = (Guid) importingObject.Object.ObjectGuid;
                  if (!(ipsObjGuid == Guid.Empty))
                  {
                    long objectId = importingObject.Object.Object_id;
                    if (objectId != 0L)
                    {
                      IList<Scenario> list = (IList<Scenario>) this._scenarioList.Where<Scenario>((System.Func<Scenario, bool>) (item => item.Property.ObjectGuid == ipsObjGuid)).ToList<Scenario>();
                      if (list == null || list.Count == 0)
                      {
                        string Message = $"Сценарий c GUID ='{ipsObjGuid.ToString()}' не найден в списке ScenarioList";
                        TechcardConsts.Plugin.appManager.AddErrorMessage(Message);
                      }
                      else
                      {
                        foreach (Scenario scenario in (IEnumerable<Scenario>) list)
                        {
                          this._import_data_main.AddValue(this.GetTechCategory(), (object) scenario.key, objectId);
                          if (scenario.Property != null && scenario.Property.Catalog != null && scenario.Property.Catalog.CatalogId != 0 && scenario.Property.Catalog.FoldersId.Count > 0)
                          {
                            foreach (int recKey in scenario.Property.Catalog.FoldersId)
                            {
                              long newKey = ImportingDataHelper.Instance.GetNewKey(this._import_data_imbase, ImportingCategory.ImbaseFolders, (object) TechcardConsts.Utils.CodeHashCode(scenario.Property.Catalog.CatalogId, recKey));
                              if (newKey != 0L)
                              {
                                try
                                {
                                  IDBObject dbObject = userSession.GetObject(newKey, false);
                                  if (dbObject != null)
                                  {
                                    IDBAttribute attributeById = dbObject.GetAttributeByID(this._formListAtrTypeId);
                                    if (attributeById != null)
                                      attributeById.AddValue((object) objectId);
                                    else
                                      dbObject.Attributes.AddAttribute(this._formListAtrTypeId, false, new object[1]
                                      {
                                        (object) objectId
                                      });
                                  }
                                  else
                                    this.plugin.appManager.AddWarningMessage($"IPS объект справочника IMBASE {newKey} не найден, невозможно создать связь сценария со справочником.");
                                }
                                catch (Exception ex)
                                {
                                  this.plugin.appManager.AddWarningMessage($"Невозможно модифицировать объект {newKey} по причине: {ex.Message}");
                                  if (ex is OutOfMemoryException)
                                    throw;
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
          catch (Exception ex)
          {
            this.plugin.appManager.AddWarningMessage("Ошибка модификации объекта IMBASE: " + ex.Message);
            if (ex is OutOfMemoryException)
              throw;
          }
        }
      }
    }
    catch (Exception ex)
    {
      string Message = $"Невозможно связать сценарии со справочниками по причине: {ex.Message}";
      TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  protected override void InitData()
  {
    this._sortFieldName = "F_ORDER";
    this._recType = "Сценарии";
    this._recTypeID = -3;
    this._tableName = "TC_SCRIPTS";
  }

  protected override void LoadMetaData4Pump()
  {
    IAttributeTypeItem byGuid = this.plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atFormListAttributeTypeGuid);
    if (byGuid != null)
      this._formListAtrTypeId = byGuid.ID;
    base.LoadMetaData4Pump();
  }

  protected override TechDataSource GetDataSource()
  {
    if (this._dataSource == null)
    {
      TechDataBuilderSimple<TechPumpBase> dataBuilder = new TechDataBuilderSimple<TechPumpBase>((TechPumpBase) this);
      dataBuilder.PumpModeCondFunc = (Func<string, string, string>) ((condField, dopType) => string.Empty);
      this._dataSource = new TechDataSource((ITechDataBuilder) dataBuilder);
    }
    return this._dataSource;
  }

  protected override TechObjectRecord GetTpObjRec() => new TechObjectRecord();

  protected override void CheckBaseRecords()
  {
  }

  private ScenarioDbRecordsHolder ReadScenarioListAtBase()
  {
    this.PumpCheckPoint("Считывание структуры таблиц", 0);
    int tableRecordsCount1 = this.GetTableRecordsCount("TC_SCRIPTS");
    int tableRecordsCount2 = this.GetTableRecordsCount("TC_SCCELLS");
    int tableRecordsCount3 = this.GetTableRecordsCount("TC_SCNAMECOL");
    int tableRecordsCount4 = this.GetTableRecordsCount("TC_SCNAMEROW");
    int tableRecordsCount5 = this.GetTableRecordsCount("TC_SCRIPTS_XREF");
    int tableRecordsCount6 = this.GetTableRecordsCount("TC_ZSCEN");
    IDataReader defaultDataReader1 = this.GetDefaultDataReader("TC_SCRIPTS");
    IDataReader defaultDataReader2 = this.GetDefaultDataReader("TC_SCCELLS");
    IDataReader defaultDataReader3 = this.GetDefaultDataReader("TC_SCNAMECOL");
    IDataReader defaultDataReader4 = this.GetDefaultDataReader("TC_SCNAMEROW");
    IDataReader defaultDataReader5 = this.GetDefaultDataReader("TC_SCRIPTS_XREF");
    IDataReader defaultDataReader6 = this.GetDefaultDataReader("TC_SCRIPTRAS");
    IDataReader dataReader = this.GetDataReader($"SELECT  \r\n                                               a.{"F_SCENKEY"}, \r\n                                               a.{"F_ZAGCODE"}, \r\n                                               b.{"F_TYPENAME"}, \r\n                                               b.{"F_VIDDET"}, \r\n                                               b.{"F_CALCSCEN"}, \r\n                                               b.{"F_PRODUCTION"} \r\n                                             FROM \r\n                                               {"TC_ZSCEN"} a, {"TC_ZSCENTYPES"} b \r\n                                             WHERE \r\n                                               a.{"F_SCENTYPE"} = b.{"F_KEY"}");
    ScenarioDbRecordsHolder scenarioDbRecordsHolder;
    try
    {
      this.ExamCheckPoint("Считывание схем таблиц", 1);
      ScenarioDbRecord.TC_SCRIPTS.ParseSchema(this.GetTableColumns(defaultDataReader1));
      ScenarioDbRecord.TC_SCCELLS.ParseSchema(this.GetTableColumns(defaultDataReader2));
      ScenarioDbRecord.TC_SCNAMECOL.ParseSchema(this.GetTableColumns(defaultDataReader3));
      ScenarioDbRecord.TC_SCNAMEROW.ParseSchema(this.GetTableColumns(defaultDataReader4));
      ScenarioDbRecord.TC_SCRIPTS_XREF.ParseSchema(this.GetTableColumns(defaultDataReader5));
      ScenarioDbRecord.TC_SCRIPTRAS.ParseSchema(this.GetTableColumns(defaultDataReader6));
      ScenarioDbRecord.TC_ZSCEN.ParseSchema(dataReader);
      scenarioDbRecordsHolder = new ScenarioDbRecordsHolder(new Dictionary<int, ScenarioDbRecord.TC_ZSCEN>(), new Dictionary<int, List<ScenarioDbRecord.TC_SCRIPTRAS>>(), new Dictionary<int, List<ScenarioDbRecord.TC_SCRIPTRAS>>(), new Dictionary<int, List<ScenarioDbRecord.TC_SCRIPTS_XREF>>(), new List<ScenarioDbRecord.TC_SCRIPTS>(), new List<ScenarioDbRecord.TC_SCCELLS>(), new List<ScenarioDbRecord.TC_SCNAMECOL>(), new List<ScenarioDbRecord.TC_SCNAMEROW>());
      int index1 = 0;
      while (defaultDataReader1.Read())
      {
        scenarioDbRecordsHolder.Scripts.Add(new ScenarioDbRecord.TC_SCRIPTS(defaultDataReader1));
        ++index1;
        if (index1 % this.CheckCount == 0 || index1 == tableRecordsCount1)
          this.PumpCheckPoint($"Считывание сценариев ({index1}/{tableRecordsCount1})", this.CalculatePercent(tableRecordsCount1, index1, 2, 10));
      }
      int index2 = 0;
      while (defaultDataReader2.Read())
      {
        scenarioDbRecordsHolder.ScrCells.Add(new ScenarioDbRecord.TC_SCCELLS(defaultDataReader2));
        ++index2;
        if (index2 % this.CheckCount == 0 || index2 == tableRecordsCount2)
          this.PumpCheckPoint($"Считывание ячеек сценариев ({index2}/{tableRecordsCount2})", this.CalculatePercent(tableRecordsCount2, index2, 11, 19));
      }
      int index3 = 0;
      while (defaultDataReader3.Read())
      {
        scenarioDbRecordsHolder.ScrNameCol.Add(new ScenarioDbRecord.TC_SCNAMECOL(defaultDataReader3));
        ++index3;
        if (index3 % this.CheckCount == 0 || index3 == tableRecordsCount3)
          this.PumpCheckPoint($"Считывание колонок сценариев ({index3}/{tableRecordsCount3})", this.CalculatePercent(tableRecordsCount3, index3, 20, 28));
      }
      int index4 = 0;
      while (defaultDataReader4.Read())
      {
        scenarioDbRecordsHolder.ScrNameRow.Add(new ScenarioDbRecord.TC_SCNAMEROW(defaultDataReader4));
        ++index4;
        if (index4 % this.CheckCount == 0 || index4 == tableRecordsCount4)
          this.PumpCheckPoint($"Считывание строк сценариев ({index4}/{tableRecordsCount4})", this.CalculatePercent(tableRecordsCount4, index4, 29, 37));
      }
      int index5 = 0;
      while (defaultDataReader5.Read())
      {
        ScenarioDbRecord.TC_SCRIPTS_XREF tcScriptsXref = new ScenarioDbRecord.TC_SCRIPTS_XREF(defaultDataReader5);
        List<ScenarioDbRecord.TC_SCRIPTS_XREF> tcScriptsXrefList;
        if (!scenarioDbRecordsHolder.ScrScriptXRef.TryGetValue(tcScriptsXref.Script, out tcScriptsXrefList))
        {
          tcScriptsXrefList = new List<ScenarioDbRecord.TC_SCRIPTS_XREF>();
          scenarioDbRecordsHolder.ScrScriptXRef.Add(tcScriptsXref.Script, tcScriptsXrefList);
        }
        tcScriptsXrefList.Add(tcScriptsXref);
        ++index5;
        if (index5 % this.CheckCount == 0 || index5 == tableRecordsCount5)
          this.PumpCheckPoint($"Считывание связей со справочниками ({index5}/{tableRecordsCount5})", this.CalculatePercent(tableRecordsCount5, index5, 38, 41));
      }
      int index6 = 0;
      while (dataReader.Read())
      {
        ScenarioDbRecord.TC_ZSCEN tcZscen = new ScenarioDbRecord.TC_ZSCEN(dataReader);
        if (!scenarioDbRecordsHolder.zscen.ContainsKey(tcZscen.Key))
          scenarioDbRecordsHolder.zscen.Add(tcZscen.Key, tcZscen);
        ++index6;
        if (index6 % this.CheckCount == 0 || index6 == tableRecordsCount5)
          this.PumpCheckPoint($"Считывание таблицы TC_ZSCEN ({index6}/{tableRecordsCount6})", this.CalculatePercent(tableRecordsCount6, index6, 42, 45));
      }
      int index7 = 0;
      while (defaultDataReader6.Read())
      {
        ScenarioDbRecord.TC_SCRIPTRAS tcScriptras = new ScenarioDbRecord.TC_SCRIPTRAS(defaultDataReader6);
        List<ScenarioDbRecord.TC_SCRIPTRAS> tcScriptrasList;
        if (tcScriptras.Scen == 0)
        {
          if (!scenarioDbRecordsHolder.ScrCellRas.TryGetValue(tcScriptras.Cell, out tcScriptrasList))
          {
            tcScriptrasList = new List<ScenarioDbRecord.TC_SCRIPTRAS>();
            scenarioDbRecordsHolder.ScrCellRas.Add(tcScriptras.Cell, tcScriptrasList);
          }
        }
        else if (!scenarioDbRecordsHolder.ScrScenRas.TryGetValue(tcScriptras.Scen, out tcScriptrasList))
        {
          tcScriptrasList = new List<ScenarioDbRecord.TC_SCRIPTRAS>();
          scenarioDbRecordsHolder.ScrScenRas.Add(tcScriptras.Scen, tcScriptrasList);
        }
        tcScriptrasList.Add(tcScriptras);
        ++index7;
        if (index7 % this.CheckCount == 0 || index7 == tableRecordsCount5)
          this.PumpCheckPoint($"Считывание информации об обновляемости полей ({index7}/{tableRecordsCount5})", this.CalculatePercent(tableRecordsCount5, index7, 46, 50));
      }
    }
    catch
    {
      this.PumpCheckPoint("Считывание и формирование сценариев прервано из-за ошибки.", 100);
      throw;
    }
    finally
    {
      defaultDataReader1.Close();
      defaultDataReader2.Close();
      defaultDataReader3.Close();
      defaultDataReader4.Close();
      defaultDataReader5.Close();
      defaultDataReader6.Close();
      dataReader.Close();
    }
    this.PumpCheckPoint("Считывание и формирование сценариев Techcard успешно завершено.", 100);
    return scenarioDbRecordsHolder;
  }

  private bool IsScrPumpedBefore(int scenarioKey)
  {
    return this._import_data_main.GetNewKey(this.GetTechCategory(), (object) scenarioKey) != 0L;
  }

  protected override ImportingCategory GetTechCategory() => ImportingCategory.TechScenarioPump;

  protected override Guid GUID => this._guid;

  public ScenarioPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = false;
    this.taskPump.Repumpble = false;
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Подготовка к перекачке сценариев", 0);
    this.ExamCheckPoint("Подготовка к перекачке сценариев завершена", 100);
  }

  public override void Pump()
  {
    if (!TechSettingsHelper.PumpMetaDataType.HasFlag((Enum) TechPumpMetaDataType.ScriptForms))
    {
      this.plugin.appManager.AddInfoMessage("Перекачка сценариев отключена в настройках");
      this.PumpCheckPoint("Перекачка данных отключена", 0);
    }
    else
    {
      this._impRelList = this.plugin.Idw.CreateImportedRelationListWithStatistics(this.GUID);
      this._impObjList = this.plugin.Idw.CreateImportedObjectListWithStatistics(this.GUID);
      this._impObjList.AfterImportEvent += new AfterImportEventDelegate(this.impObjList_AfterImportEvent);
      this.LoadMetaData4Pump();
      this.LoadImportingCategoryData();
      IObjectTypeItem byGuid1 = TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(TechcardConsts.TypeConsts.otScenarioObjTypeGuid);
      IAttributeTypeItem byGuid2 = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atNaimAttrTypeGuid);
      IAttributeTypeItem byGuid3 = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atGuidOTsAttrTypeGuid);
      IAttributeTypeItem byGuid4 = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atFormAttrTypeGuid);
      IAttributeTypeItem byGuid5 = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atConditionGuid);
      IAttributeTypeItem byGuid6 = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atImageLinkGuid);
      IAttributeTypeItem byGuid7 = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atProductionAttrTypeGuid);
      IAttributeTypeItem byGuid8 = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atTechTypeKeyAttrGuid);
      this.PumpCheckPoint("Закачка сценариев", 0);
      IUserSession userSession = TechcardConsts.Plugin.Idw.GetUserSession();
      if (userSession == null)
      {
        this.plugin.appManager.AddWarningMessage("Невозможно получить пользовательскую сессию. Это может привести к невозможности закачки эскизов сценариев.");
        this.PumpCheckPoint("Ошибка закачки сценариев!", 0);
      }
      int id = byGuid1.ID;
      try
      {
        ScenarioDbRecordsHolder recordsHolder = this.ReadScenarioListAtBase();
        if (recordsHolder == null)
        {
          this.plugin.appManager.AddErrorMessage("Закачка сценариев остановлена из-за ошибки чтения данных из базы");
          this.PumpCheckPoint("Закачка сценариев прервана из-за ошибки", 0);
          return;
        }
        int index = 0;
        ScenarioBuilder scenarioBuilder = new ScenarioBuilder(recordsHolder);
        IComparer<Scenario> visualComparer = (IComparer<Scenario>) new ScenarioVisualComparer();
        foreach (ScenarioDbRecord.TC_SCRIPTS script in recordsHolder.Scripts)
        {
          Scenario scenario = scenarioBuilder.Build(script);
          if (!(ScenarioUtils.GetImportingObjectType(scenario) == Guid.Empty) && !this.IsScrPumpedBefore(scenario.key))
          {
            Scenario scenario1 = this._scenarioList.FirstOrDefault<Scenario>((System.Func<Scenario, bool>) (item => visualComparer.Compare(item, scenario) == 0));
            if (scenario1 != null)
            {
              scenario.Property.ObjectGuid = scenario1.Property.ObjectGuid;
              this._scenarioList.Add(scenario);
            }
            else
            {
              this._scenarioList.Add(scenario);
              ObjectRecord objectRecord = this._impObjList.AddObject(id, 0);
              scenario.Property.ObjectGuid = (Guid) objectRecord.ObjectGuid;
              objectRecord.Caption = this.GetScenarioCaption(scenario);
              if (scenario.Property.SlideId != 0 && userSession != null)
              {
                QuickObjectInfo imageGuidBySlideInfo = this.GetImageGuidBySlideInfo(scenario.Property.SlideId, userSession);
                if (!imageGuidBySlideInfo.Empty)
                {
                  scenario.Property.SlideGuid = imageGuidBySlideInfo.VersionGuid;
                  this._impObjList.AddAttributeLink(byGuid6.ID, imageGuidBySlideInfo.ObjectID, imageGuidBySlideInfo.Caption);
                }
              }
              this.CorrectScriptDefaultValue(scenario);
              XmlDocument xml = Scenario2XmlConverter.SaveToXml(scenario);
              string tmpFileName = this.GetTmpFileName();
              string filename = tmpFileName;
              xml.Save(filename);
              FileInfo fileInfo = new FileInfo(tmpFileName);
              this._impObjList.AddAttributeBlob(byGuid4.ID, tmpFileName, fileInfo.Length, scenario.Name, ArcMethods.NotPacked);
              this._impObjList.AddAttributeStr(byGuid2.ID, objectRecord.Caption);
              this._impObjList.AddAttributeInt(byGuid8.ID, (long) scenario.key);
              this.WriteCondition(scenario, byGuid5.ID, scenario.Kind);
              this.AddTpObjectType(scenario, byGuid3);
              AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Imdi.UserSession, this._impObjList);
              IpsProductionObj production = this.GetProduction(scenario);
              if (production != null)
                this._impObjList.AddAttributeInt(byGuid7.ID, production.ObjID);
              ++index;
              if (index % this.CheckCount == 0 || index == recordsHolder.Scripts.Count)
                this.PumpCheckPoint($"Закачка сценариев ({index}/{recordsHolder.Scripts.Count})", this.CalculatePercent(recordsHolder.Scripts.Count, index, 51, 99));
            }
          }
        }
      }
      catch (Exception ex)
      {
        string str = $"Закачка сценариев прервана из-за ошибки: {ex.Message}";
        this.plugin.appManager.AddErrorMessage(str);
        this.PumpCheckPoint(str, 100);
        if (ex is OutOfMemoryException)
          throw;
      }
      finally
      {
        this._impObjList.Import();
        this._scenarioList.Clear();
        this.ReleasePumpData();
      }
      this.PumpCheckPoint("Закачка сценариев успешно завершена", 100);
    }
  }

  protected override void ReleasePumpData()
  {
    base.ReleasePumpData();
    this._scenarioList.Clear();
    this._scenarioList = (List<Scenario>) null;
    this._scenarioCaption2Idx.Clear();
    this._scenarioCaption2Idx = (Dictionary<string, int>) null;
  }
}
