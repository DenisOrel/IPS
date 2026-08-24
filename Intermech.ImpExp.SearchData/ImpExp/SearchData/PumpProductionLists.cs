// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpProductionLists
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Interface.SafeDataProxy;
using Intermech.ImpExp.SearchData.ItemFactories;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.MRP2;
using ntermech.ImpExp.Interface.Search;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация метаданных для производственных заказов", "Перекачка производственных заказов")]
internal class PumpProductionLists : PumpClass
{
  private readonly SearchDataPlugin plugin;
  private SettingsGroup _settingsGroup;
  private readonly string _settingsName = "ZPC_ATTRIBUTES";
  private Dictionary<ProductionListItemAttribute, SettingsAttributeTypeItem> _attributes;
  protected List<int> exitAssemblyTypes;
  protected IMeasures measures;
  protected int attrZFrom;
  protected int attrZTill;
  protected ProductionListIDHelper idHelper;
  private bool _ignoreImportedPL;

  protected override Guid GUID => new Guid("{BD381159-353B-4F12-99BD-C0CB4D36AAAD}");

  public PumpProductionLists(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  protected MRP2Consts.ProductionLinkFlag ConvertFromOPCODE(int opcode)
  {
    switch (opcode)
    {
      case 1:
        return MRP2Consts.ProductionLinkFlag.Added;
      case 2:
      case 5:
        return MRP2Consts.ProductionLinkFlag.Deleted;
      case 3:
      case 4:
        return MRP2Consts.ProductionLinkFlag.Modified;
      default:
        return MRP2Consts.ProductionLinkFlag.Copied;
    }
  }

  protected void ReadPLList(int startPercent, int endPercent)
  {
    int index = 0;
    int relationTypeId = MetaDataHelper.GetRelationTypeID("cadd9a57-306c-11d8-b4e9-00304f19f545");
    int objectTypeId = MetaDataHelper.GetObjectTypeID("cadd9a5c-306c-11d8-b4e9-00304f19f545");
    int attributeId = MetaDataHelper.GetAttributeID((object) new Guid("cad0132b-306c-11d8-b4e9-00304f19f545"));
    IDBRelationCollection relationCollection = BasePumpHelper.Session.GetRelationCollection(relationTypeId);
    IDBObjectCollection objectCollection = BasePumpHelper.Session.GetObjectCollection(objectTypeId);
    int int32_1 = Convert.ToInt32(BasePumpHelper.S4ObjectQuery("select count(art.art_id) from articles art, v_articles a LEFT JOIN zpc z ON a.art_id = z.part_aid and a.art_ver_id = z.part_ver WHERE a.ART_ID = art.ART_ID AND a.ART_VER_ID = art.ART_VER_ID AND a.SECTION_ID = 99999990 and z.PARENT_ZREC_ID = -1 and z.zakaz_id > 0"));
    using (IDataReader dataReader = BasePumpHelper.S4Query("select a.art_id, a.designatio, a.name, u.fullname, g.f_guid from articles art, v_articles a LEFT JOIN guids_art g ON a.art_id = g.art_id LEFT JOIN zpc z ON a.art_id = z.part_aid and a.art_ver_id = z.part_ver LEFT JOIN users u ON a.author = u.user_id WHERE a.ART_ID = art.ART_ID AND a.ART_VER_ID = art.ART_VER_ID AND a.SECTION_ID = 99999990 and z.PARENT_ZREC_ID = -1 and z.zakaz_id > 0 ORDER BY a.art_id, a.art_ver_id"))
    {
      while (dataReader.Read())
      {
        ++index;
        int int32_2 = dataReader.GetInt32(0);
        string str1 = dataReader.IsDBNull(1) ? string.Empty : dataReader.GetString(1);
        string str2 = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetString(2);
        string str3 = dataReader.GetString(3);
        string str4 = !dataReader.IsDBNull(4) ? dataReader.GetString(4) : string.Empty;
        bool flag = false;
        List<long> longList = new List<long>();
        ConditionStructure[] conditionStructureArray;
        if (string.IsNullOrEmpty(str4) || !GuidHelper.IsGuid(str4))
          conditionStructureArray = new ConditionStructure[1]
          {
            new ConditionStructure(attributeId, RelationalOperators.Equal, (object) int32_2, LogicalOperators.AND, 0, false)
          };
        else
          conditionStructureArray = new ConditionStructure[1]
          {
            new ConditionStructure(-18, RelationalOperators.Equal, (object) new Guid(str4), LogicalOperators.AND, 0, false)
          };
        ConditionStructure[] conditions = conditionStructureArray;
        foreach (DataRow row in (InternalDataCollectionBase) objectCollection.Select(new DBRecordSetParams(conditions, new object[1]
        {
          (object) -2
        })).Rows)
        {
          if (relationCollection.ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
          {
            (object) -2
          }), Convert.ToInt64(row[0])).Rows.Count > 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          this.plugin.ProductionListsSettingsControl.PList.Add(new Tuple<int, string, string, string>(int32_2, str1, str2, str3));
        this.ExamCheckPoint($"Обработка записи о производственной ведомости ({index} из {int32_1})", this.CalculatePercent(int32_1, index, startPercent, endPercent));
      }
    }
  }

  public override void Exam()
  {
    this._settingsGroup = new SettingsGroup("Атрибуты позиции произв.заказа", SettingsGroupType.ProductionListAttributes);
    this._settingsGroup.ObjectCreated += new ObjectCreatedEventHandler(this.AttributesTypesCreated);
    (ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) as ISettingsGroupService).Groups.Add((ISettingsGroup) this._settingsGroup);
    IAttributeTypeToCreateList service = ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    this.ExamCheckPoint("Получение метаданных таблицы ZPC", 1);
    IDataReader shemaDataReader = this.GetShemaDataReader("ZPC");
    ITableFieldInfo[] dataTableFields = (ITableFieldInfo[]) null;
    try
    {
      dataTableFields = PumpItemFactory.GetFieldsInfo(shemaDataReader);
    }
    finally
    {
      shemaDataReader.Close();
    }
    this.ExamCheckPoint("Определение количества записей в " + ProductionListItemAttributeFactory.TableName, 2);
    int tableRecordsCount = this.GetTableRecordsCount(ProductionListItemAttributeFactory.TableName);
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings(this._settingsName);
    this._attributes = new Dictionary<ProductionListItemAttribute, SettingsAttributeTypeItem>(tableRecordsCount);
    this.ExamCheckPoint("Получение данных из таблицы " + ProductionListItemAttributeFactory.TableName, 3);
    IDataReader sequentialDataReader = this.GetSequentialDataReader(ProductionListItemAttributeFactory.TableName, ProductionListItemAttributeFactory.TableColumns);
    int index = 0;
    try
    {
      ProductionListItemAttributeFactory attributeFactory = new ProductionListItemAttributeFactory(ProductionListItemAttributeFactory.TableName, sequentialDataReader, dataTableFields, this.plugin.Idw.AppManager);
      while (sequentialDataReader.Read())
      {
        ++index;
        ProductionListItemAttribute key = attributeFactory.NewItem(sequentialDataReader);
        SettingsAttributeTypeItem sattrItem = new SettingsAttributeTypeItem(key.AttributeName, key.AttributeName, key.DBFieldName, key.AttributeType);
        SaveSettingsAttribute[] array;
        if (settings != null && settings.TryGetValue(key.DBFieldName, out array))
        {
          SaveSettingsAttribute settingsAttribute = Array.Find<SaveSettingsAttribute>(array, (Predicate<SaveSettingsAttribute>) (x => x.AttributeName.Equals("GUID")));
          IAttributeTypeToCreate byGuid = service.GetByGuid(new Guid(settingsAttribute.AttributeValue));
          if (byGuid != null)
            sattrItem.AttrGuid = byGuid.GUID;
        }
        if (sattrItem.AttrGuid == Guid.Empty)
        {
          IAttributeTypeToCreate attribute = SearchHelper.FindAttribute(service, sattrItem, key.AttributeName, key.DBFieldName, key.AttributeType, key.AttributeSize, this.plugin.Imdi.NewPumpGuid(), (object) null, MultiValueModes.SingleValue);
          sattrItem.AttrGuid = attribute.GUID;
        }
        this._attributes.Add(key, sattrItem);
        this._settingsGroup.GroupItems.Add((ISettingsGroupItem) sattrItem);
        this.ExamCheckPoint($"Обработка записи из таблицы {ProductionListItemAttributeFactory.TableName} ({index} из {tableRecordsCount})", this.CalculatePercent(tableRecordsCount, index, 4, 70));
      }
    }
    finally
    {
      sequentialDataReader.Close();
    }
    this.ExamCheckPoint("Определение доступных производственных ведомостей для миграции", 71);
    this.ReadPLList(71, 99);
    this.ExamCheckPoint("Инициализация метаданных успешно завершена", 100);
  }

  private void AttributesTypesCreated()
  {
    IAttributeTypeToCreateList service1 = ServicesManager.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings));
    ICache service2 = ServicesManager.GetService(typeof (ICache)) as ICache;
    try
    {
      service2.DeleteCache(ImportingCategory.ProductionListsCompositionAttributes);
      IImportingData cache = service2.GetCache(ImportingCategory.ProductionListsCompositionAttributes);
      Dictionary<string, SaveSettingsAttribute[]> dictionary = new Dictionary<string, SaveSettingsAttribute[]>(1);
      foreach (KeyValuePair<ProductionListItemAttribute, SettingsAttributeTypeItem> attribute in this._attributes)
      {
        IAttributeTypeToCreate byGuid = service1.GetByGuid(attribute.Value.AttrGuid);
        cache.AddValue((object) attribute.Key.DBFieldName, (long) byGuid.FieldType, byGuid.GUID.ToString());
        List<SaveSettingsAttribute> settingsAttributeList = new List<SaveSettingsAttribute>()
        {
          new SaveSettingsAttribute("GUID", byGuid.GUID.ToString())
        };
        dictionary.Add(attribute.Key.DBFieldName, settingsAttributeList.ToArray());
        this.plugin.Imdi.RelationTypes.LinkAttributeTypeToRelationType(byGuid.LocalID, MRP2Consts.reltypeIdProductComposition, RequiredModes.Manual, string.Empty, ComputeValueModes.NotComputableValue, string.Empty, byGuid.DefaultValue != null ? byGuid.DefaultValue.ToString() : string.Empty, (short) 0, false, AttributeOptions.None, string.Empty, 0, 0);
      }
    }
    finally
    {
      service2?.ReleaseCache(ImportingCategory.ProductionListsCompositionAttributes);
      if (this._attributes != null)
        this._attributes.Clear();
    }
  }

  private List<string> GetAdditionalAttributes(IImportingData cacheData)
  {
    Dictionary<object, DictionaryValue> category = cacheData.GetCategory(ImportingCategory.ProductionListsCompositionAttributes);
    return category == null || category.Count <= 0 ? (List<string>) null : category.Select<KeyValuePair<object, DictionaryValue>, string>((System.Func<KeyValuePair<object, DictionaryValue>, string>) (x => Convert.ToString(x.Key))).ToList<string>();
  }

  private bool CheckAttribute(List<int> enabledAttributes, int attributeID)
  {
    return enabledAttributes == null || enabledAttributes.Contains(attributeID);
  }

  protected List<PumpProductionLists.RelationAttribute> GetRelationAttributes(
    ProductionListItem item,
    IImportingData cacheData,
    int relationType,
    List<int> enabledAttributes)
  {
    List<PumpProductionLists.RelationAttribute> relationAttributes = new List<PumpProductionLists.RelationAttribute>();
    if (this.CheckAttribute(enabledAttributes, PumpHelper.AttrCountID))
    {
      IMeasureItem measure = this.measures.GetMeasure(item.MUShortName);
      if (measure != null)
        relationAttributes.Add(new PumpProductionLists.RelationAttribute(PumpHelper.AttrCountID, FieldTypes.ftMeasured, (object) (item.CountPC * measure.Koef), (object) measure.BaseMeasureId, (object) $"{item.CountPC} {item.MUShortName}"));
    }
    if (this.CheckAttribute(enabledAttributes, PumpHelper.AttrPositionID))
      relationAttributes.Add(new PumpProductionLists.RelationAttribute(PumpHelper.AttrPositionID, FieldTypes.ftString, (object) item.Positio));
    SpecificationSection specificationSection;
    if (this.CheckAttribute(enabledAttributes, PumpHelper.AttrSPSectionID) && PumpHelper.SpecificationSections.TryGetValue(item.Razdel, out specificationSection))
      relationAttributes.Add(new PumpProductionLists.RelationAttribute(PumpHelper.AttrSPSectionID, FieldTypes.ftObjectLink, (object) specificationSection.ObjectID, (object) specificationSection.Caption));
    if (this.CheckAttribute(enabledAttributes, PumpHelper.AttrTypeNoteID))
      relationAttributes.Add(new PumpProductionLists.RelationAttribute(PumpHelper.AttrTypeNoteID, FieldTypes.ftString, (object) item.Note));
    MRP2Consts.ProductionLinkFlag productionLinkFlag = this.ConvertFromOPCODE(item.OPCode);
    if (this.CheckAttribute(enabledAttributes, MRP2Consts.attrIdChangeCode))
      relationAttributes.Add(new PumpProductionLists.RelationAttribute(MRP2Consts.attrIdChangeCode, FieldTypes.ftInteger, (object) (int) productionLinkFlag));
    if (this.CheckAttribute(enabledAttributes, MRP2Consts.attrIdDeleteTag))
      relationAttributes.Add(new PumpProductionLists.RelationAttribute(MRP2Consts.attrIdDeleteTag, FieldTypes.ftInteger, (object) (productionLinkFlag == MRP2Consts.ProductionLinkFlag.Deleted ? 1 : 0)));
    if (item.ZFrom > 0 && this.CheckAttribute(enabledAttributes, this.attrZFrom))
      relationAttributes.Add(new PumpProductionLists.RelationAttribute(this.attrZFrom, FieldTypes.ftInteger, (object) item.ZFrom));
    if (item.ZTill > 0 && this.CheckAttribute(enabledAttributes, this.attrZTill))
      relationAttributes.Add(new PumpProductionLists.RelationAttribute(this.attrZTill, FieldTypes.ftInteger, (object) item.ZTill));
    if (item.AdditionalItems != null)
    {
      IMetadataInfo service = ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
      foreach (KeyValuePair<string, object> additionalItem in item.AdditionalItems)
      {
        if (CompareValuesHelper.NormalizedValue(additionalItem.Value) != null)
        {
          DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ProductionListsCompositionAttributes, (object) additionalItem.Key);
          if (dictionaryValue == null)
          {
            BasePumpHelper.AppManager.AddWarningMessage($"Для столбца {additionalItem.Key} не найден соответствующий атрибут");
          }
          else
          {
            IAttributeTypeItem byGuid = service.AttributeTypes.GetByGuid(new Guid(dictionaryValue.Caption));
            if (this.CheckAttribute(enabledAttributes, byGuid.ID))
            {
              switch (byGuid.AttrValueType)
              {
                case 1:
                case 10:
                  relationAttributes.Add(new PumpProductionLists.RelationAttribute(byGuid.ID, FieldTypes.ftString, (object) Convert.ToString(additionalItem.Value)));
                  continue;
                case 2:
                case 14:
                  long result1;
                  if (long.TryParse(Convert.ToString(additionalItem.Value), out result1))
                  {
                    relationAttributes.Add(new PumpProductionLists.RelationAttribute(byGuid.ID, FieldTypes.ftInteger, (object) result1));
                    continue;
                  }
                  continue;
                case 3:
                  double result2;
                  if (double.TryParse(Convert.ToString(additionalItem.Value), out result2))
                  {
                    relationAttributes.Add(new PumpProductionLists.RelationAttribute(byGuid.ID, FieldTypes.ftDouble, (object) result2));
                    continue;
                  }
                  continue;
                case 4:
                  DateTime result3;
                  if (DateTime.TryParse(Convert.ToString(additionalItem.Value), out result3))
                  {
                    relationAttributes.Add(new PumpProductionLists.RelationAttribute(byGuid.ID, FieldTypes.ftDateTime, (object) result3));
                    continue;
                  }
                  continue;
                case 12:
                  bool result4;
                  if (bool.TryParse(Convert.ToString(additionalItem.Value), out result4))
                  {
                    relationAttributes.Add(new PumpProductionLists.RelationAttribute(byGuid.ID, FieldTypes.ftInteger, (object) (result4 ? 1 : 0)));
                    continue;
                  }
                  continue;
                default:
                  BasePumpHelper.AppManager.AddWarningMessage($"Нет обработчика для типа атрибута {(Enum) (FieldTypes) byGuid.AttrValueType} для столбца {additionalItem.Key}");
                  continue;
              }
            }
          }
        }
      }
    }
    return relationAttributes;
  }

  private string GetArticleCaption(int artID, int artVerID)
  {
    using (IDataReader dataReader = BasePumpHelper.S4Query("select a.designatio, a.name from v_articles a where a.art_id = @p1 and a.art_ver_id = @p2", (object) artID, (object) artVerID))
    {
      if (dataReader.Read())
        return PumpHelper.GetArticleCaption(Convert.ToString(dataReader[0]), Convert.ToString(dataReader[1]));
    }
    return string.Empty;
  }

  protected virtual long GetArticleID(
    IUserSession session,
    IImportingData cacheData,
    int artID,
    int artVerID,
    string searchHash,
    out string caption,
    out int objectType,
    out long id)
  {
    long oldKey = 0;
    if (!string.IsNullOrEmpty(searchHash))
    {
      DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ProductionCopiesHash, (object) searchHash);
      if (dictionaryValue != null)
      {
        id = (dictionaryValue.Tag as ObjectInfoEx).ID;
        objectType = (dictionaryValue.Tag as ObjectInfoEx).ObjectType;
        caption = dictionaryValue.Caption;
        return dictionaryValue.NewObjectID;
      }
    }
    DictionaryValue dictionaryValue1 = cacheData.GetValue(ImportingCategory.Articles, (object) artID);
    if (dictionaryValue1 != null && dictionaryValue1.Tag is ArticleTag tag1 && tag1.Versions.TryGetValue(artVerID, out oldKey))
    {
      id = dictionaryValue1.NewObjectID;
      objectType = cacheData.GetTag(ImportingCategory.ObjectGUIDs, (object) oldKey) is ObjectInfo tag ? tag.ObjectType : -1;
      caption = this.GetArticleCaption(artID, artVerID);
      return oldKey;
    }
    objectType = -1;
    caption = string.Empty;
    id = 0L;
    return 0;
  }

  protected int GetProductionCopyTypeID(int parentRecId, int articleTypeID)
  {
    return parentRecId != 100 || !this.exitAssemblyTypes.Contains(articleTypeID) ? MRP2Consts.GetCopyType(this.plugin.Imdi.UserSession, articleTypeID) : MRP2Consts.objtypeIdExitAssembly;
  }

  protected virtual long FindDocumentation(
    int artId,
    int artVerId,
    out string caption,
    out int objectTypeID,
    out long id)
  {
    caption = string.Empty;
    objectTypeID = -1;
    id = 0L;
    return -1;
  }

  protected string GetPCDSE(
    IImportingData cacheData,
    string pcDseKey,
    long articleObjectId,
    Guid articleIdGuid)
  {
    string empty = string.Empty;
    DictionaryValue dictionaryValue1 = cacheData.GetValue(ImportingCategory.ProductionСopyIDUniqueCache, (object) pcDseKey);
    string caption;
    if (dictionaryValue1 != null)
    {
      caption = dictionaryValue1.Caption;
    }
    else
    {
      DictionaryValue dictionaryValue2 = cacheData.GetValue(ImportingCategory.ArticlePCDSECache, (object) articleObjectId);
      if (dictionaryValue2 != null)
      {
        caption = string.IsNullOrEmpty(dictionaryValue2.Caption) ? (articleIdGuid != Guid.Empty ? articleIdGuid : this.GetObjectIdGuid(articleObjectId, Guid.NewGuid())).ToString() : dictionaryValue2.Caption;
      }
      else
      {
        caption = (articleIdGuid != Guid.Empty ? articleIdGuid : this.GetObjectIdGuid(articleObjectId, Guid.NewGuid())).ToString();
        cacheData.AddValue(ImportingCategory.ArticlePCDSECache, (object) articleObjectId, 0L, caption);
      }
      cacheData.AddValue(ImportingCategory.ProductionСopyIDUniqueCache, (object) pcDseKey, 0L, caption);
    }
    return caption;
  }

  private Guid GetObjectIdGuid(long objectId, Guid defaultValue)
  {
    Guid guid = this.plugin.Imdi.ImportedObjects.GetGUID(objectId);
    if (guid != Guid.Empty)
      return guid;
    IDBObject dbObject = this.plugin.Imdi.UserSession.GetObject(objectId, false);
    return dbObject == null ? defaultValue : dbObject.GUID;
  }

  protected virtual long CreateProductionСopy(
    IUserSession session,
    IImportingData cacheData,
    IImportedObjectList writer,
    IImportingData currentZakazCache,
    ProductionListItem productionListItem,
    bool writeMessage,
    out string caption,
    out int objectTypeID,
    out long id,
    int ctx_id,
    string searchHash,
    out bool isNewObject)
  {
    caption = string.Empty;
    objectTypeID = -1;
    id = 0L;
    isNewObject = false;
    DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ProductionCopiesHash, (object) searchHash);
    if (dictionaryValue != null)
    {
      caption = dictionaryValue.Caption;
      objectTypeID = (dictionaryValue.Tag as ObjectInfoEx).ObjectType;
      id = (dictionaryValue.Tag as ObjectInfoEx).ID;
      return dictionaryValue.NewObjectID;
    }
    int partArticleId = productionListItem.PartArticleID;
    int partArticleVer = productionListItem.PartArticleVer;
    int zparentRecId = productionListItem.ZParentRecID;
    if (productionListItem.Razdel == 1)
      return this.FindDocumentation(partArticleId, partArticleVer, out caption, out objectTypeID, out id);
    if (!(currentZakazCache.GetTag((object) ProductionListsCache.CacheKey(partArticleId, partArticleVer)) is ImportingObjectTag tag))
    {
      if (writeMessage)
        BasePumpHelper.AppManager.AddWarningMessage($"Изделие part_aid={partArticleId} part_ver={partArticleVer} не найдено в кэше закачанных изделий");
      return 0;
    }
    ImportingObject importingObject = tag.Clone();
    long objectId1 = importingObject.Object.Object_id;
    Guid articleIdGuid = importingObject.Object.IdGuid != null ? (Guid) importingObject.Object.IdGuid : Guid.Empty;
    caption = importingObject.Object.Caption;
    if (ctx_id != 0 || importingObject.Object.ObjectType == MRP2Consts.objtypeIdProductionLists)
    {
      objectTypeID = importingObject.Object.ObjectType;
      id = importingObject.Object.Id;
      return objectId1;
    }
    importingObject.Object.VersionId = 0;
    importingObject.LCSteps = (List<LCStepRecord>) null;
    importingObject.Object.Object_id = 0L;
    importingObject.Object.Id = 0L;
    importingObject.Object.ObjectGuid = (object) this.plugin.Imdi.NewPumpGuid();
    importingObject.Object.IdGuid = (object) this.plugin.Imdi.NewPumpGuid();
    importingObject.Object.ObjectType = this.GetProductionCopyTypeID(zparentRecId, importingObject.Object.ObjectType);
    AttributeRecord attributeRecord = importingObject.Attributes.Find((Predicate<AttributeRecord>) (x => x.AttributeId == PumpHelper.AttrContentModifiedDate));
    if (attributeRecord != null)
      attributeRecord.DateValue = (object) DateTime.UtcNow;
    objectTypeID = importingObject.Object.ObjectType;
    writer.AddItem(importingObject);
    string pcdse = this.GetPCDSE(cacheData, $"{partArticleId}", objectId1, articleIdGuid);
    writer.AddAttributeStr(MRP2Consts.attrIdPKDSE_Id, pcdse);
    writer.AddAttributeStr(MRP2Consts.attrIdHashSearch, searchHash);
    writer.AddAttributeLink(MRP2Consts.attrIdArticleLink, objectId1, importingObject.Object.Caption);
    AttributesHelper.CorrectObligatoryObjectAttributes(session, writer);
    writer.Import();
    if (writer.Items[0] == null)
    {
      BasePumpHelper.AppManager.AddWarningMessage($"Изделие part_aid={partArticleId} part_ver={partArticleVer} не было импортировано. Ошибка: {Convert.ToString((object) writer.GetImportError(0))}");
      return 0;
    }
    long objectId2 = writer.Items[0].Object.Object_id;
    id = writer.Items[0].Object.Id;
    cacheData.AddValue(ImportingCategory.ProductionСopyIDCache, (object) objectId2, id, pcdse);
    cacheData.AddValue(ImportingCategory.ProductionCopiesHash, (object) searchHash, objectId2, caption, (ITagImportObject) new ObjectInfoEx(objectTypeID, id));
    if (cacheData.GetValue(ImportingCategory.ArticlePCDSECache, (object) objectId1) == null && !string.IsNullOrEmpty(pcdse))
      cacheData.AddValue(ImportingCategory.ArticlePCDSECache, (object) objectId1, 0L, pcdse);
    writer.Items.Clear();
    isNewObject = true;
    return objectId2;
  }

  protected virtual long CreateRelation(
    IUserSession session,
    IImportingData cacheData,
    IImportedRelationList writerRelations,
    long projID,
    ProductionListItem item,
    int relationType)
  {
    writerRelations.AddRelationFromID(projID, item.PartID, relationType);
    List<int> enabledAttributes;
    this.idHelper.EnabledRelationAttributes.TryGetValue(relationType, out enabledAttributes);
    List<PumpProductionLists.RelationAttribute> relationAttributes = this.GetRelationAttributes(item, cacheData, relationType, enabledAttributes);
    if (this.CheckAttribute(enabledAttributes, PumpHelper.AttrVerLinkID))
      writerRelations.AddAttributeInt(PumpHelper.AttrVerLinkID, item.PartObjectID);
    foreach (PumpProductionLists.RelationAttribute relationAttribute in relationAttributes)
    {
      switch (relationAttribute.FieldType)
      {
        case FieldTypes.ftString:
          writerRelations.AddAttributeStr(relationAttribute.AttributeID, (string) relationAttribute.Value1);
          continue;
        case FieldTypes.ftInteger:
          writerRelations.AddAttributeInt(relationAttribute.AttributeID, Convert.ToInt64(relationAttribute.Value1));
          continue;
        case FieldTypes.ftDouble:
          writerRelations.AddAttributeDouble(relationAttribute.AttributeID, (double) relationAttribute.Value1);
          continue;
        case FieldTypes.ftDateTime:
          writerRelations.AddAttributeDate(relationAttribute.AttributeID, (DateTime) relationAttribute.Value1);
          continue;
        case FieldTypes.ftObjectLink:
          writerRelations.AddAttributeLink(relationAttribute.AttributeID, (long) relationAttribute.Value1, (string) relationAttribute.Value2);
          continue;
        case FieldTypes.ftMeasured:
          writerRelations.AddAttributeMeasure(relationAttribute.AttributeID, (double) relationAttribute.Value1, (long) relationAttribute.Value2, (string) relationAttribute.Value3);
          continue;
        default:
          continue;
      }
    }
    AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, writerRelations);
    writerRelations.Import();
    long relation;
    if (writerRelations.Items[0] != null)
    {
      relation = writerRelations.Items[0].Relation.PrjLinkId;
    }
    else
    {
      BasePumpHelper.AppManager.AddWarningMessage($"Связь projID={projID} partID={item.PartID} типа={relationType} не создана, см.серверный лог");
      relation = 0L;
    }
    writerRelations.Items.Clear();
    return relation;
  }

  private string HashData(string data)
  {
    using (SHA256 shA256 = SHA256.Create())
      return BitConverter.ToString(shA256.ComputeHash(Encoding.UTF8.GetBytes(data))).Replace("-", "").ToLower();
  }

  private int GetContext(string linkType)
  {
    int context = 0;
    if (!string.IsNullOrEmpty(linkType))
      PumpHelper.LinkTypesMapper.TryGetValue(linkType[0], out context);
    return context;
  }

  private Tuple<string, long, int, string, long> RecursivePumpItem(
    IImportingData currentZakazCache,
    IImportedObjectList writer,
    IImportedRelationList relationWriter,
    IImportingData cacheData,
    ProductionListItem item,
    IUserSession session,
    out bool isNew)
  {
    isNew = true;
    List<ProductionListItem> productionListItemList = new List<ProductionListItem>();
    if (this._ignoreImportedPL && item.Razdel == 99999990)
    {
      cacheData.ClearValue(207, (object) item.ID);
      cacheData.ClearValue(223, (object) item.ID);
    }
    else
    {
      DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ProductionListsObjects, (object) item.ID);
      if (dictionaryValue != null)
      {
        isNew = false;
        ProductionCopyInfo tag = dictionaryValue.Tag as ProductionCopyInfo;
        return new Tuple<string, long, int, string, long>(tag.Hash, dictionaryValue.NewObjectID, tag.ObjectType, dictionaryValue.Caption, tag.ID);
      }
    }
    int context = this.GetContext(item.LinkType);
    if (context == 0)
    {
      using (IDataReader dataReader = BasePumpHelper.S4Query("select z.*, m.mu_short_name, a.art_ver_id from zpc z, mu m, articles a where z.zakaz_id = @p1 and z.z_ver = @p2 and z.parent_zrec_id = @p3 and z.mu_id = m.mu_id and z.part_aid = a.art_id order by z.zakaz_id, z.zrec_id", (object) item.ZakazID, (object) item.ZakazVer, (object) item.ZRecID))
      {
        ProductionListItemFactory productionListItemFactory = new ProductionListItemFactory(dataReader, this.GetAdditionalAttributes(cacheData), this.plugin.Idw.AppManager);
        while (dataReader.Read())
          productionListItemList.Add(productionListItemFactory.NewItem(dataReader));
      }
    }
    string str1 = $"{item.PartArticleID}-{item.PartArticleVer}-{item.CountPC}-{item.Positio}-{item.Note}-{item.Material}-{item.Format}";
    string str2 = $"{item.PartArticleID}-{item.PartArticleVer}";
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    if (productionListItemList.Count > 0)
    {
      foreach (ProductionListItem productionListItem in productionListItemList)
      {
        Tuple<string, long, int, string, long> tuple = this.RecursivePumpItem(currentZakazCache, writer, relationWriter, cacheData, productionListItem, session, out bool _);
        stringBuilder2.AppendLine(tuple.Item1);
        stringBuilder1.AppendLine(tuple.Item1);
        productionListItem.PartObjectID = tuple.Item2;
        productionListItem.PartObjectTypeID = tuple.Item3;
        productionListItem.PartCaption = tuple.Item4;
        productionListItem.PartID = tuple.Item5;
      }
    }
    stringBuilder2.AppendLine(str1);
    string hash = stringBuilder2.ToString();
    stringBuilder1.AppendLine(str2);
    string searchHash = this.HashData(stringBuilder1.ToString());
    string caption = string.Empty;
    int objectType = -1;
    long id = 0;
    bool isNewObject = true;
    long num;
    if (item.Razdel == 99999990)
    {
      num = this.GetArticleID(session, cacheData, item.PartArticleID, item.PartArticleVer, string.Empty, out caption, out objectType, out id);
      if (num == 0L)
      {
        BasePumpHelper.AppManager.AddWarningMessage($"Объект part_aid={item.ZakazID} part_ver={item.ZakazVer} не был создан, см.серверный лог");
        return new Tuple<string, long, int, string, long>(hash, 0L, -1, string.Empty, 0L);
      }
      objectType = MRP2Consts.objtypeIdProductionLists;
    }
    else
    {
      num = this.CreateProductionСopy(session, cacheData, writer, currentZakazCache, item, true, out caption, out objectType, out id, context, searchHash, out isNewObject);
      if (num == 0L)
        return new Tuple<string, long, int, string, long>(hash, 0L, -1, string.Empty, 0L);
    }
    if (isNewObject && productionListItemList.Count > 0)
    {
      foreach (ProductionListItem productionListItem in productionListItemList)
      {
        if (productionListItem.PartObjectID != 0L && productionListItem.PartObjectID != -1L)
        {
          if (this._ignoreImportedPL && item.Razdel == 99999990)
            cacheData.ClearValue(206, (object) productionListItem.ID);
          if (cacheData.GetNewKey(ImportingCategory.ProductionListsPositions, (object) productionListItem.ID) == 0L)
          {
            int relationType;
            if (productionListItem.Razdel == 1)
            {
              relationType = MRP2Consts.reltypeIdDocumentComposition;
            }
            else
            {
              switch (this.GetContext(productionListItem.LinkType))
              {
                case 2:
                  relationType = this.idHelper.TechTypeID;
                  break;
                case 3:
                  relationType = this.idHelper.ProdTypeID;
                  break;
                default:
                  relationType = productionListItem.PartObjectTypeID != MRP2Consts.objtypeIdProductionLists ? this.idHelper.SimpleTypeID : this.idHelper.PlplTypeID;
                  break;
              }
            }
            long relation = this.CreateRelation(session, cacheData, relationWriter, num, productionListItem, relationType);
            cacheData.AddValue(ImportingCategory.ProductionListsPositions, (object) productionListItem.ID, relation);
          }
        }
      }
    }
    if (item.OPCode == 4 && item.OPVars > 0)
    {
      long oldKey = BasePumpHelper.MakeCacheKey2(item.ZakazID, item.OPVars, item.ZakazVer);
      if (cacheData.GetValue(ImportingCategory.ProductionListsArticlesLinks, (object) oldKey) == null)
        cacheData.AddValue(ImportingCategory.ProductionListsArticlesLinks, (object) oldKey, num, caption);
    }
    cacheData.AddValue(ImportingCategory.ProductionListsObjects, (object) item.ID, num, caption, (ITagImportObject) new ProductionCopyInfo(objectType, hash, id));
    return new Tuple<string, long, int, string, long>(hash, num, objectType, caption, id);
  }

  private void ReadProductionCopiesHashCacheFromDataTable(
    DataTable pcList,
    IImportingData cacheData)
  {
    foreach (DataRow row in (InternalDataCollectionBase) pcList.Rows)
    {
      long int64_1 = Convert.ToInt64(row[0]);
      long int64_2 = Convert.ToInt64(row[1]);
      string oldKey = Convert.ToString(row[4]);
      if (!string.IsNullOrEmpty(oldKey) && cacheData.GetNewKey(ImportingCategory.ProductionCopiesHash, (object) oldKey) == 0L)
        cacheData.AddValue(ImportingCategory.ProductionCopiesHash, (object) oldKey, int64_1, Convert.ToString(row[3]), (ITagImportObject) new ObjectInfoEx(Convert.ToInt32(row[2]), int64_2));
      string caption = Convert.ToString(row[5]);
      if (!string.IsNullOrEmpty(caption) && cacheData.GetValue(ImportingCategory.ProductionСopyIDCache, (object) int64_1) == null)
        cacheData.AddValue(ImportingCategory.ProductionСopyIDCache, (object) int64_1, int64_2, caption);
      long int64_3 = row[6] == DBNull.Value || row[6] == null ? 0L : Convert.ToInt64(row[6]);
      if (int64_3 != 0L && cacheData.GetValue(ImportingCategory.ArticlePCDSECache, (object) int64_3) == null && !string.IsNullOrEmpty(caption))
        cacheData.AddValue(ImportingCategory.ArticlePCDSECache, (object) int64_3, 0L, caption);
    }
  }

  private void ReadProductionCopiesHashCache(IUserSession session, IImportingData cacheData)
  {
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[7]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -3, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -7, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -50, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdHashSearch, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdPKDSE_Id, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdArticleLink, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.Index, SortOrders.NONE, 0)
    });
    this.ReadProductionCopiesHashCacheFromDataTable(session.GetObjectCollection(MRP2Consts.objtypeIdProductionCopy).Select(paramSet), cacheData);
  }

  public override void Pump()
  {
    SimpleLogger logger = BasePumpHelper.Logger;
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cacheData = (IImportingData) new SafeImportingDataProxy(service.GetCache(ImportingCategory.ProductionListsPositions, ImportingCategory.ProductionListsObjects, ImportingCategory.Articles, ImportingCategory.ArticlePCDSECache, ImportingCategory.ProductionListsCompositionAttributes, ImportingCategory.ProductionListsArticlesLinks, ImportingCategory.ProductionСopyIDCache, ImportingCategory.ProductionСopyIDUniqueCache, ImportingCategory.EnabledProductionLists, ImportingCategory.DocumentLinksToProductionLists, ImportingCategory.ProductionCopiesHash, ImportingCategory.SettingsProductionLists, ImportingCategory.Documents, ImportingCategory.ObjectGUIDs, ImportingCategory.ProductionCopiesDocLinks), (ISafeProxyErrorHandler) new ImpExpErrorHandler(this.plugin.appManager));
    string str = ConfigurationManager.AppSettings.Get("Search.IgnoreImportedPL");
    if (!string.IsNullOrEmpty(str))
      bool.TryParse(str, out this._ignoreImportedPL);
    this.measures = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    this.exitAssemblyTypes = MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad00132-306c-11d8-b4e9-00304f19f545"));
    this.exitAssemblyTypes.AddRange((IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad0025f-306c-11d8-b4e9-00304f19f545")));
    this.exitAssemblyTypes.AddRange((IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(new Guid("cad0025e-306c-11d8-b4e9-00304f19f545")));
    this.attrZFrom = MetaDataHelper.GetAttributeTypeID("cadd9a74-306c-11d8-b4e9-00304f19f545");
    this.attrZTill = MetaDataHelper.GetAttributeTypeID("cadd9a75-306c-11d8-b4e9-00304f19f545");
    this.idHelper = new ProductionListIDHelper();
    try
    {
      Dictionary<object, DictionaryValue> category1 = cacheData.GetCategory(ImportingCategory.EnabledProductionLists);
      Dictionary<object, DictionaryValue> category2 = cacheData.GetCategory(ImportingCategory.SettingsProductionLists);
      IImportedObjectList importedObjectList = this.plugin.Idw.CreateImportedObjectList(0);
      IImportedRelationList importedRelationList = this.plugin.Idw.CreateImportedRelationList(0);
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      this.PumpCheckPoint("Подготовка кэша производственных копий для перекачки", 1);
      this.ReadProductionCopiesHashCache(userSession, cacheData);
      DictionaryValue dictionaryValue;
      bool flag = category2 != null && category2.TryGetValue((object) ProductionListsSettingsControl.BlockPLListInSearchConfigName, out dictionaryValue) && Convert.ToBoolean(dictionaryValue.NewObjectID);
      this.PumpCheckPoint("Определение количества производственных заказов для перекачки", 5);
      int count = category1.Count;
      int index = 0;
      foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category1)
      {
        int int32 = Convert.ToInt32(keyValuePair.Key);
        IImportingData cache = service.GetCache(ProductionListsCache.GetCategoryID(int32));
        try
        {
          List<ProductionListItem> productionListItemList = new List<ProductionListItem>();
          ProductionListItemFactory productionListItemFactory = (ProductionListItemFactory) null;
          using (IDataReader dataReader = BasePumpHelper.S4Query("select z.*, m.mu_short_name, a.art_ver_id from zpc z, mu m, articles a where z.zakaz_id = @p1 and z.parent_zrec_id = -1 and z.mu_id = m.mu_id and z.part_aid = a.art_id order by z.zakaz_id, z.zrec_id", (object) int32))
          {
            while (dataReader.Read())
            {
              if (productionListItemFactory == null)
                productionListItemFactory = new ProductionListItemFactory(dataReader, this.GetAdditionalAttributes(cacheData), this.plugin.Idw.AppManager);
              productionListItemList.Add(productionListItemFactory.NewItem(dataReader));
            }
          }
          ++index;
          this.PumpCheckPoint($"Миграция производственных заказов ({index} из {count})", this.CalculatePercent(count, index, 6, 94));
          foreach (ProductionListItem productionListItem in productionListItemList)
          {
            BasePumpHelper.AppManager.AddInfoMessage($"Миграция ведомости zakaz_id={productionListItem.ZakazID} z_ver={productionListItem.ZakazVer}");
            bool isNew;
            Tuple<string, long, int, string, long> tuple = this.RecursivePumpItem(cache, importedObjectList, importedRelationList, cacheData, productionListItem, userSession, out isNew);
            if (tuple.Item2 != 0L && isNew && cacheData.GetNewKey(ImportingCategory.ProductionCopiesDocLinks, (object) productionListItem.ID) == 0L)
            {
              this.RestoreDocLinks(userSession, cacheData.GetCategory(ImportingCategory.Documents), cacheData.GetCategory(ImportingCategory.ObjectGUIDs), productionListItem.PartArticleID, productionListItem.PartArticleVer, tuple.Item2);
              cacheData.AddValue(ImportingCategory.ProductionCopiesDocLinks, (object) productionListItem.ID, 1L);
            }
          }
          if (flag)
          {
            BasePumpHelper.S4NonQuery($"delete from bo_rights where instanc_id = {int32} and group_id = 999999999 and right_id in (103, 104, 108, 109)");
            BasePumpHelper.S4NonQuery($"insert into bo_rights(group_id, bo_id, right_id, instanc_id, deny_it, grant_type, calendarid, user_id, DATE_TYPE, grant_it) values(999999999, 37, 103, {int32}, 'Y', 0, 0, -1, 0, 'N')");
            BasePumpHelper.S4NonQuery($"insert into bo_rights(group_id, bo_id, right_id, instanc_id, deny_it, grant_type, calendarid, user_id, DATE_TYPE, grant_it) values(999999999, 37, 104, {int32}, 'Y', 0, 0, -1, 0, 'N')");
            BasePumpHelper.S4NonQuery($"insert into bo_rights(group_id, bo_id, right_id, instanc_id, deny_it, grant_type, calendarid, user_id, DATE_TYPE, grant_it) values(999999999, 37, 108, {int32}, 'Y', 0, 0, -1, 0, 'N')");
            BasePumpHelper.S4NonQuery($"insert into bo_rights(group_id, bo_id, right_id, instanc_id, deny_it, grant_type, calendarid, user_id, DATE_TYPE, grant_it) values(999999999, 37, 109, {int32}, 'Y', 0, 0, -1, 0, 'N')");
          }
        }
        finally
        {
          service.ReleaseCache(ProductionListsCache.GetCategoryID(int32));
        }
      }
      this.PumpCheckPoint("Восстановление ссылок в копиях изделий на позиции производственных заказов", 95);
      Dictionary<object, DictionaryValue> category3 = cacheData.GetCategory(ImportingCategory.ProductionListsArticlesLinks);
      if (category3.Count > 0)
      {
        foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category3)
        {
          if (keyValuePair.Value.NewObjectID != 0L)
          {
            int hi;
            int lo;
            int short_lo;
            BasePumpHelper.ExtractCacheKey2((long) keyValuePair.Key, out hi, out lo, out short_lo);
            object obj = BasePumpHelper.S4ObjectQuery("select prjlink_id from zpc z where zakaz_id=@p1 and z_ver=@p2 and zrec_id=@p3", (object) hi, (object) short_lo, (object) lo);
            if (obj == null)
            {
              BasePumpHelper.AppManager.AddWarningMessage($"Позиция zakaz_id={hi} z_ver={short_lo} zrec_id={lo}  не найдена, ссылка не восстановлена");
            }
            else
            {
              long newKey = cacheData.GetNewKey(ImportingCategory.ProductionListsPositions, (object) Convert.ToInt64(obj));
              if (newKey == 0L)
              {
                BasePumpHelper.AppManager.AddWarningMessage($"Связь для позиции заказа prjlink_id={obj} не найдена, ссылка в ней не восстановлена");
              }
              else
              {
                this.AddRelationAttribute(userSession, cacheData, importedRelationList, newKey, keyValuePair.Value.NewObjectID, keyValuePair.Value.Caption);
                cacheData.SetNewKey(ImportingCategory.ProductionListsArticlesLinks, keyValuePair.Key, 0L);
              }
            }
          }
        }
      }
      this.PumpCheckPoint("Перекачка производственных заказов успешно завершена", 100);
    }
    catch (Exception ex)
    {
      logger.Write($"=========Pump abort ({ex.Message}\r\n{ex.StackTrace})\r\n\r\n");
      throw;
    }
    finally
    {
      service.ReleaseCache(ImportingCategory.ProductionListsPositions, ImportingCategory.ProductionListsObjects, ImportingCategory.Articles, ImportingCategory.ArticlePCDSECache, ImportingCategory.ProductionListsCompositionAttributes, ImportingCategory.ProductionListsArticlesLinks, ImportingCategory.ProductionСopyIDCache, ImportingCategory.ProductionСopyIDUniqueCache, ImportingCategory.DocumentLinksToProductionLists, ImportingCategory.ProductionCopiesHash, ImportingCategory.SettingsProductionLists, ImportingCategory.Documents, ImportingCategory.ObjectGUIDs, ImportingCategory.ProductionCopiesDocLinks);
    }
  }

  protected virtual void RestoreDocLinks(
    IUserSession session,
    Dictionary<object, DictionaryValue> documentsCache,
    Dictionary<object, DictionaryValue> objectGuids,
    int partArticleID,
    int partArticleVer,
    long plObjectID)
  {
  }

  protected virtual void AddRelationAttribute(
    IUserSession session,
    IImportingData cacheData,
    IImportedRelationList writerRelations,
    long relationID,
    long linkObjectID,
    string linkCaption)
  {
    writerRelations.UseRelation(relationID);
    writerRelations.AddAttributeLink(MRP2Consts.attrIdReplacedBy, linkObjectID, linkCaption);
    writerRelations.Import();
    writerRelations.Items.Clear();
  }

  public static long ProductionListKey(int artID, int artVerID)
  {
    return ((long) artID << 32 /*0x20*/) + (long) artVerID;
  }

  public static PumpClass GetPumpClass(bool specialModePL, SearchDataPlugin plugin)
  {
    return !specialModePL ? (PumpClass) new PumpProductionLists(plugin) : (PumpClass) new PumpProductionListsAfterPumping(plugin);
  }

  protected class CompositionItem
  {
    public int ZRecID { get; private set; }

    public int ZakazVer { get; private set; }

    public long ObjectID { get; private set; }

    public int ObjectType { get; private set; }

    public CompositionItem(int zrecID, int zakazVer, long objectID, int objectType)
    {
      this.ZRecID = zrecID;
      this.ZakazVer = zakazVer;
      this.ObjectID = objectID;
      this.ObjectType = objectType;
    }
  }

  protected class RelationAttribute
  {
    public int AttributeID { get; set; }

    public FieldTypes FieldType { get; set; }

    public object Value1 { get; set; }

    public object Value2 { get; set; }

    public object Value3 { get; set; }

    public RelationAttribute(int attributeID, FieldTypes fieldType, object value1)
      : this(attributeID, fieldType, value1, (object) null, (object) null)
    {
    }

    public RelationAttribute(int attributeID, FieldTypes fieldType, object value1, object value2)
      : this(attributeID, fieldType, value1, value2, (object) null)
    {
    }

    public RelationAttribute(
      int attributeID,
      FieldTypes fieldType,
      object value1,
      object value2,
      object value3)
    {
      this.AttributeID = attributeID;
      this.FieldType = fieldType;
      this.Value1 = value1;
      this.Value2 = value2;
      this.Value3 = value3;
    }
  }
}
