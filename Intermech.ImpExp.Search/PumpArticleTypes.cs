// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpArticleTypes
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Search.ItemFactories;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.LifeCycles;
using ntermech.ImpExp.Interface.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки типов изделий Search", "Перекачка данных о типах изделий Search")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpArticleTypes(SearchPlugin plugin) : PumpSearchClass(plugin, "ART_TYPES")
{
  private Dictionary<int, IArticleTypesItem> articleTypesDict = new Dictionary<int, IArticleTypesItem>();
  private Dictionary<Guid, IArticleTypesItem> articleTypesDictGuid = new Dictionary<Guid, IArticleTypesItem>();
  private Dictionary<IArticleTypesItem, SettingsObjectTypeItem> articleTypesSettingsDict = new Dictionary<IArticleTypesItem, SettingsObjectTypeItem>();
  private const string _groupName = "Типы изделий";
  private Guid _guid = new Guid("{737800B2-CD20-4f75-80A9-62D7E497D7EA}");

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    (ServicesManager.GetService(typeof (ICache)) as ICache).GetCache(ImportingCategory.ArticleTypes);
    SettingsGroup settingsGroup = new SettingsGroup("Типы изделий", SettingsGroupType.ArticleTypes);
    if (ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) is ISettingsGroupService service1)
      service1.Groups.Add((ISettingsGroup) settingsGroup);
    settingsGroup.ObjectCreated += new ObjectCreatedEventHandler(this.ObjectTypesCreated);
    this.ExamCheckPoint("Определение количества записей", 0);
    int tableRecordsCount1 = this.GetTableRecordsCount(ArticleTypesItemFactory.TableName);
    int index1 = 0;
    this.ExamCheckPoint("Получение данных из таблицы " + ArticleTypesItemFactory.TableName, 1);
    IDataReader sequentialDataReader1 = this.GetSequentialDataReader(ArticleTypesItemFactory.TableName, ArticleTypesItemFactory.TableColumns);
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings(this.SettingsName);
    IObjectTypeToCreateList service2 = ServicesManager.ServiceContainer.GetService(typeof (IObjectTypeToCreateList)) as IObjectTypeToCreateList;
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    IDBObjectType objectType1 = userSession.GetObjectType(new Guid("cad00268-306c-11d8-b4e9-00304f19f545"));
    IDBRelationType relationType1 = userSession.GetRelationType(objectType1.DefaultRelation);
    IDBLCSchema lcSchema1 = userSession.GetLCSchema(objectType1.SchemaID);
    IMetadataInfo imdi = this.plugin.Imdi;
    DataTable toTable = userSession.GetObjectTypeCollection(objectType1.ObjectType).SelectRecursive(string.Empty);
    IDBObjectType objectType2 = userSession.GetObjectType(new Guid("cad00880-306c-11d8-b4e9-00304f19f545"));
    DataTable dataTable = userSession.GetObjectTypeCollection(objectType2.ObjectType).SelectRecursive(string.Empty);
    foreach (DataRow row in (InternalDataCollectionBase) toTable.Rows)
      SearchHelper.NormalizeObjTypeNames(row);
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      SearchHelper.NormalizeObjTypeNames(row);
      DataSetProcessor.AddRow(toTable, row, false);
    }
    toTable.AcceptChanges();
    string searchImagesFolder = SearchHelper.GetSearchImagesFolder();
    try
    {
      string format = $"Обработка записи из таблицы {ArticleTypesItemFactory.TableName} ({{0}} из {{1}})";
      ArticleTypesItemFactory typesItemFactory = new ArticleTypesItemFactory(ArticleTypesItemFactory.TableName, sequentialDataReader1, this.plugin.Idw.AppManager);
      while (sequentialDataReader1.Read())
      {
        IArticleTypesItem key = typesItemFactory.NewItem(sequentialDataReader1, imdi.NewPumpGuid());
        if (key.SectionId != 99999990)
        {
          bool flag1 = false;
          SaveSettingsAttribute[] settingsAttributeArray = (SaveSettingsAttribute[]) null;
          if (service1 != null)
          {
            SettingsObjectTypeItem settingsObjectTypeItem = new SettingsObjectTypeItem(key.SectName, string.Empty, key.DocType);
            if (settings != null && settings.ContainsKey(key.SectName))
            {
              settingsAttributeArray = settings[key.SectName];
              if (settingsAttributeArray != null && settingsAttributeArray.Length != 0)
              {
                foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
                {
                  if (settingsAttribute.AttributeName.Equals("GUID"))
                  {
                    settingsObjectTypeItem.AttrGuid = new Guid(settingsAttribute.AttributeValue);
                    flag1 = true;
                  }
                }
              }
            }
            if (!flag1)
            {
              Guid empty = Guid.Empty;
              bool flag2 = this.SpecialArtTypes(key, ref empty);
              if (flag2)
                settingsObjectTypeItem.AttrGuid = empty;
              if (!flag2)
              {
                IObjectTypeToCreate objectTypeToCreate = service2.GetByName(key.SectName);
                if (objectTypeToCreate == null)
                {
                  DataRow[] dataRowArray = toTable.Select(string.Format("F_OBJ_TYPE_NAME = {0} OR F_OBJ_NAME = {0}", (object) DataSetProcessor.QString(SearchHelper.NormalizeName(key.SectName))));
                  if (dataRowArray.Length == 1)
                    objectTypeToCreate = service2.GetByGuid(new Guid(Convert.ToString(dataRowArray[0]["F_GUID"])));
                }
                if (objectTypeToCreate == null)
                  objectTypeToCreate = service2.GetByShortName(key.SectName.Length > Consts.MaxShortNameLength ? key.SectName.Substring(0, Consts.MaxShortNameLength) : key.SectName);
                if (objectTypeToCreate != null)
                {
                  if ((objectTypeToCreate.VersionMode != ObjectVersionModes.Abstract || key.VersionMode == ObjectVersionModes.Abstract) && toTable.Select(string.Format("F_OBJ_TYPE_NAME = {0} OR F_OBJ_NAME = {0}", (object) DataSetProcessor.QString(SearchHelper.NormalizeName(objectTypeToCreate.Name)))).Length != 0)
                  {
                    settingsObjectTypeItem.AttrGuid = objectTypeToCreate.GUID;
                    flag2 = true;
                  }
                  else
                    settingsObjectTypeItem.Error = new ItemError(ItemErrorType.Renamed, $"Тип изделий исходной базы переименован, так как в базе назначения существует тип объектов \"{objectTypeToCreate.Name}\", который либо абстрактный, либо не входит в иерархию типов изделий и строительных типов");
                }
              }
              if (!flag2)
              {
                settingsObjectTypeItem.AttrGuid = key.Guid;
                key.Icon = (byte[]) null;
                if (key.Bitmap.Trim() != string.Empty && searchImagesFolder != string.Empty)
                  key.Icon = SearchHelper.GetIcon(Path.Combine(searchImagesFolder, key.Bitmap));
                string str = settingsObjectTypeItem.Error == null || !settingsObjectTypeItem.Error.ErrorMessages.Exists((Predicate<MessageItem>) (x => x.ErrorType == ItemErrorType.Renamed)) ? key.SectName : $"{key.SectName}_{key.Guid.ToString()}";
                IObjectTypeToCreate objectTypeToCreate = service2.AddItem(true, str, string.Empty, str, key.Guid, long.MaxValue, key.Icon, key.VersionMode);
                if (objectTypeToCreate == null)
                  throw new Exception($"Тип изделий \"{key.SectName}\" не добавлен в список миграции!");
                if (settingsAttributeArray != null)
                {
                  foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
                  {
                    switch (settingsAttribute.AttributeName)
                    {
                      case "ANY_ATTRIBUTE":
                        bool boolean = Convert.ToBoolean(settingsAttribute.AttributeValue);
                        if (objectTypeToCreate.AnyAttributes != boolean)
                        {
                          objectTypeToCreate.AnyAttributes = boolean;
                          break;
                        }
                        break;
                      case "ICON":
                        byte[] numArray = Convert.FromBase64String(settingsAttribute.AttributeValue);
                        if (objectTypeToCreate.Icon != numArray)
                        {
                          objectTypeToCreate.Icon = numArray;
                          break;
                        }
                        break;
                      case "INST_NAME":
                        if (objectTypeToCreate.InstanceName != settingsAttribute.AttributeValue)
                        {
                          objectTypeToCreate.InstanceName = settingsAttribute.AttributeValue;
                          break;
                        }
                        break;
                      case "LC_SCHEME":
                        IDBLCSchema lcSchema2 = userSession.GetLCSchema(new Guid(settingsAttribute.AttributeValue), false);
                        if (lcSchema2 != null)
                        {
                          objectTypeToCreate.LcShemaId = lcSchema2.GUID;
                          break;
                        }
                        break;
                      case "NOTE":
                        if (objectTypeToCreate.Note != settingsAttribute.AttributeValue)
                        {
                          objectTypeToCreate.Note = settingsAttribute.AttributeValue;
                          break;
                        }
                        break;
                      case "PARENT":
                        IObjectTypeItem byGuid1 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid(settingsAttribute.AttributeValue));
                        if (byGuid1 != null)
                        {
                          objectTypeToCreate.ParentTypeId = byGuid1.GUID;
                          break;
                        }
                        break;
                      case "RELATION_TYPE":
                        IRelationTypeItem byGuid2 = this.plugin.Imdi.RelationTypes.GetByGuid(new Guid(settingsAttribute.AttributeValue));
                        if (byGuid2 != null)
                        {
                          objectTypeToCreate.DefaultRelationId = byGuid2.GUID;
                          break;
                        }
                        break;
                      case "SHORT_NAME":
                        if (objectTypeToCreate.ShortName != settingsAttribute.AttributeValue)
                        {
                          objectTypeToCreate.ShortName = settingsAttribute.AttributeValue;
                          break;
                        }
                        break;
                      case "VERSIONABLE":
                        ObjectVersionModes int32 = (ObjectVersionModes) Convert.ToInt32(settingsAttribute.AttributeValue);
                        if (objectTypeToCreate.VersionMode != int32)
                        {
                          objectTypeToCreate.VersionMode = int32;
                          break;
                        }
                        break;
                    }
                  }
                }
                if (objectTypeToCreate.ParentTypeId == Guid.Empty)
                {
                  objectTypeToCreate.ParentTypeId = (objectType1 as IDBGuid).GUID;
                  key.ParentID = objectTypeToCreate.ParentTypeId;
                }
                if (objectTypeToCreate.DefaultRelationId == Guid.Empty)
                {
                  IDBRelationType relationType2 = userSession.GetRelationType(objectType1.DefaultRelation);
                  objectTypeToCreate.DefaultRelationId = (relationType2 as IDBGuid).GUID;
                  key.DefRelation = objectTypeToCreate.DefaultRelationId;
                }
                if (objectTypeToCreate.LcShemaId == Guid.Empty)
                {
                  IDBLCSchema lcSchema3 = userSession.GetLCSchema(objectType1.SchemaID);
                  objectTypeToCreate.LcShemaId = lcSchema3.GUID;
                  key.LCScheme = objectTypeToCreate.LcShemaId;
                }
              }
            }
            this.articleTypesDict.Add(key.SectionId, key);
            this.articleTypesDictGuid.Add(key.Guid, key);
            this.articleTypesSettingsDict.Add(key, settingsObjectTypeItem);
            ++index1;
            this.ExamCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount1), this.CalculatePercent(tableRecordsCount1, index1, 2, 29));
          }
        }
      }
    }
    finally
    {
      sequentialDataReader1.Close();
    }
    this.ExamCheckPoint("Определение иерархии типов изделий", 30);
    int tableRecordsCount2 = this.GetTableRecordsCount(ArticleTypeLinksItemFactory.TableName);
    int index2 = 0;
    this.ExamCheckPoint("Получение данных из таблицы " + ArticleTypeLinksItemFactory.TableName, 31 /*0x1F*/);
    IDataReader sequentialDataReader2 = this.GetSequentialDataReader(ArticleTypeLinksItemFactory.TableName);
    try
    {
      string format = $"Обработка записи из таблицы {ArticleTypeLinksItemFactory.TableName} ({{0}} из {{1}})";
      ArticleTypeLinksItemFactory linksItemFactory = new ArticleTypeLinksItemFactory(ArticleTypeLinksItemFactory.TableName, sequentialDataReader2, this.plugin.Idw.AppManager);
      while (sequentialDataReader2.Read())
      {
        ++index2;
        this.ExamCheckPoint(string.Format(format, (object) index2, (object) tableRecordsCount2), this.CalculatePercent(tableRecordsCount2, index2, 32 /*0x20*/, 69));
        IArticleTypeLinksItem articleTypeLinksItem = linksItemFactory.NewItem(sequentialDataReader2);
        if (articleTypeLinksItem.LinkType != 1)
        {
          IArticleTypesItem key1 = this.articleTypesDict.ContainsKey(articleTypeLinksItem.ObjectType) ? this.articleTypesDict[articleTypeLinksItem.ObjectType] : (IArticleTypesItem) null;
          if (key1 != null)
          {
            SettingsObjectTypeItem settingsObjectTypeItem1 = this.articleTypesSettingsDict[key1];
            IObjectTypeToCreate byGuid3 = service2.GetByGuid(settingsObjectTypeItem1.AttrGuid);
            if (articleTypeLinksItem.InObjectType == -1)
            {
              key1.IsTreeRoot = true;
              if (key1.Guid.Equals(settingsObjectTypeItem1.AttrGuid))
              {
                if (byGuid3.ParentTypeId == Guid.Empty)
                {
                  byGuid3.ParentTypeId = (objectType1 as IDBGuid).GUID;
                  key1.ParentID = byGuid3.ParentTypeId;
                }
                if (byGuid3.DefaultRelationId == Guid.Empty)
                {
                  byGuid3.DefaultRelationId = (relationType1 as IDBGuid).GUID;
                  key1.DefRelation = byGuid3.DefaultRelationId;
                }
                if (byGuid3.LcShemaId == Guid.Empty)
                {
                  byGuid3.LcShemaId = lcSchema1.GUID;
                  key1.LCScheme = byGuid3.LcShemaId;
                }
              }
              settingsObjectTypeItem1.ParentItem = (SettingsObjectTypeItem) null;
              settingsGroup.GroupItems.Add((ISettingsGroupItem) settingsObjectTypeItem1);
            }
            else
            {
              IArticleTypesItem key2 = this.articleTypesDict.ContainsKey(articleTypeLinksItem.InObjectType) ? this.articleTypesDict[articleTypeLinksItem.InObjectType] : (IArticleTypesItem) null;
              if (key2 != null)
              {
                SettingsObjectTypeItem settingsObjectTypeItem2 = this.articleTypesSettingsDict[key2];
                if (key1.Guid.Equals(settingsObjectTypeItem1.AttrGuid))
                {
                  settingsObjectTypeItem1.ParentItem = settingsObjectTypeItem2;
                  IObjectTypeToCreate byGuid4 = service2.GetByGuid(settingsObjectTypeItem1.AttrGuid);
                  if (byGuid4 != null)
                  {
                    byGuid4.ParentTypeId = settingsObjectTypeItem2.AttrGuid;
                    if (byGuid3.ParentTypeId == Guid.Empty)
                    {
                      byGuid3.ParentTypeId = key2.Guid;
                      key1.ParentID = byGuid3.ParentTypeId;
                    }
                    if (byGuid3.DefaultRelationId == Guid.Empty)
                    {
                      byGuid3.DefaultRelationId = key2.DefRelation;
                      key1.DefRelation = byGuid3.DefaultRelationId;
                    }
                    if (byGuid3.LcShemaId == Guid.Empty)
                    {
                      byGuid3.LcShemaId = key2.LCScheme;
                      key1.LCScheme = byGuid3.LcShemaId;
                    }
                  }
                }
                settingsObjectTypeItem2.SettingsItems.Add((ISettingsItem) settingsObjectTypeItem1);
              }
            }
          }
        }
      }
    }
    finally
    {
      sequentialDataReader2.Close();
    }
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  private bool SpecialArtTypes(IArticleTypesItem item, ref Guid objTypeGuid)
  {
    if (item.SectName.ToLower() == "материалы")
      objTypeGuid = new Guid("cad0081d-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "строительные объекты")
      objTypeGuid = new Guid("cad00880-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "заказы")
      objTypeGuid = new Guid("cad00580-306c-11d8-b4e9-00304f19f545");
    else if (item.SectionId == 99999990)
      objTypeGuid = new Guid("cadd92e9-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "альбомы")
      objTypeGuid = new Guid("cadd9363-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "копии")
      objTypeGuid = new Guid("cadd9364-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "шкафы")
      objTypeGuid = new Guid("cadd9642-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "объекты отд")
      objTypeGuid = new Guid("cad00001-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "проектная документация")
      objTypeGuid = new Guid("cad008ed-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "комплекты чертежей")
      objTypeGuid = new Guid("cad008d7-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "комплектовочные")
      objTypeGuid = new Guid("cad015b1-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "раздел не определен")
      objTypeGuid = new Guid("cad00001-306c-11d8-b4e9-00304f19f545");
    else if (item.SectName.ToLower() == "машиностроительные объекты")
    {
      objTypeGuid = new Guid("cad00268-306c-11d8-b4e9-00304f19f545");
    }
    else
    {
      if (!(item.SectName.ToLower() == "папки"))
        return false;
      objTypeGuid = new Guid("cadd9643-306c-11d8-b4e9-00304f19f545");
    }
    return true;
  }

  private void ObjectTypesCreated()
  {
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo));
    IObjectTypeToCreateList service1 = ServicesManager.ServiceContainer.GetService(typeof (IObjectTypeToCreateList)) as IObjectTypeToCreateList;
    IAttributeTypeToCreateList service2 = ServicesManager.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    ICache service3 = ServicesManager.GetService(typeof (ICache)) as ICache;
    service3.DeleteCache(ImportingCategory.ArticleTypes);
    IImportingData cache = service3.GetCache(ImportingCategory.ArticleTypes);
    IDataReader sequentialDataReader = this.GetSequentialDataReader(Params4DocTypesItemFactory.TableName, Params4DocTypesItemFactory.TableColumns);
    try
    {
      ISaveSettings service4 = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
      Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>();
      IDictionaryEnumerator enumerator = (IDictionaryEnumerator) this.articleTypesSettingsDict.GetEnumerator();
      while (enumerator.MoveNext())
      {
        IArticleTypesItem key = enumerator.Key as IArticleTypesItem;
        SettingsObjectTypeItem settingsObjectTypeItem = enumerator.Value as SettingsObjectTypeItem;
        if (!(settingsObjectTypeItem.AttrGuid == Guid.Empty))
        {
          IImportingData importingData = cache;
          // ISSUE: variable of a boxed type
          __Boxed<int> sectionId = (System.ValueType) key.SectionId;
          Guid guid = settingsObjectTypeItem.AttrGuid;
          string caption = guid.ToString();
          importingData.AddValue(ImportingCategory.ArticleTypes, (object) sectionId, -1L, caption);
          List<SaveSettingsAttribute> settingsAttributeList1 = new List<SaveSettingsAttribute>();
          guid = settingsObjectTypeItem.AttrGuid;
          settingsAttributeList1.Add(new SaveSettingsAttribute("GUID", guid.ToString()));
          List<SaveSettingsAttribute> settingsAttributeList2 = settingsAttributeList1;
          IObjectTypeToCreate byGuid1 = service1.GetByGuid(settingsObjectTypeItem.AttrGuid);
          guid = key.Guid;
          if (guid.Equals(settingsObjectTypeItem.AttrGuid))
          {
            if (byGuid1.InstanceName != key.SectName)
              settingsAttributeList2.Add(new SaveSettingsAttribute("INST_NAME", byGuid1.InstanceName));
            if (byGuid1.ShortName != key.SectName)
              settingsAttributeList2.Add(new SaveSettingsAttribute("SHORT_NAME", byGuid1.ShortName));
            if (byGuid1.Note != string.Empty)
              settingsAttributeList2.Add(new SaveSettingsAttribute("NOTE", byGuid1.Note));
            if (byGuid1.ParentTypeId != key.ParentID)
            {
              IObjectTypeToCreate byGuid2 = service1.GetByGuid(byGuid1.ParentTypeId);
              if (byGuid2 != null)
              {
                List<SaveSettingsAttribute> settingsAttributeList3 = settingsAttributeList2;
                guid = byGuid2.GUID;
                SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("PARENT", guid.ToString());
                settingsAttributeList3.Add(settingsAttribute);
              }
            }
            if (byGuid1.DefaultRelationId != key.DefRelation)
            {
              IRelationTypeItem byGuid3 = this.plugin.Imdi.RelationTypes.GetByGuid(byGuid1.DefaultRelationId);
              if (byGuid3 != null)
              {
                List<SaveSettingsAttribute> settingsAttributeList4 = settingsAttributeList2;
                guid = byGuid3.GUID;
                SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("RELATION_TYPE", guid.ToString());
                settingsAttributeList4.Add(settingsAttribute);
              }
            }
            if (byGuid1.LcShemaId != key.LCScheme)
            {
              IDBLCSchema lcSchema = userSession.GetLCSchema(byGuid1.LcShemaId, false);
              if (lcSchema != null)
              {
                List<SaveSettingsAttribute> settingsAttributeList5 = settingsAttributeList2;
                guid = lcSchema.GUID;
                SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("LC_SCHEME", guid.ToString());
                settingsAttributeList5.Add(settingsAttribute);
              }
            }
            if (byGuid1.VersionMode != key.VersionMode)
              settingsAttributeList2.Add(new SaveSettingsAttribute("VERSIONABLE", ((int) byGuid1.VersionMode).ToString()));
            if (byGuid1.AnyAttributes != key.AnyAttribute)
              settingsAttributeList2.Add(new SaveSettingsAttribute("ANY_ATTRIBUTE", byGuid1.AnyAttributes.ToString()));
            if (byGuid1.Icon != key.Icon)
            {
              string str = string.Empty;
              if (byGuid1.Icon != null && byGuid1.Icon.Length != 0)
                str = Convert.ToBase64String(byGuid1.Icon);
              settingsAttributeList2.Add(new SaveSettingsAttribute("ICON", str));
            }
          }
          else if (settingsObjectTypeItem.Caption != key.SectName)
            key.SectName = settingsObjectTypeItem.Caption;
          if (PumpArticleAttributes.articleAttributes.ContainsKey(key.SectionId))
          {
            foreach (TypeAttributeItem typeAttributeItem in PumpArticleAttributes.articleAttributes[key.SectionId])
            {
              IAttributeTypeToCreate byGuid4 = service2.GetByGuid(typeAttributeItem.GUID);
              this.plugin.Imdi.ObjectTypes.LinkAttributeTypeToObjectType(byGuid4.LocalID, byGuid1.LocalID, true, RequiredModes.Manual, string.Empty, ComputeValueModes.NotComputableValue, string.Empty, UniqueValueModes.NotUnique, 0, byGuid4.DefaultValue != null ? byGuid4.DefaultValue.ToString() : string.Empty, OptimizationModes.Write, false, AttributeOptions.None, string.Empty, 0, 0);
            }
          }
          if (settingsAttributeList2.Count > 0)
          {
            if (settings.ContainsKey(key.SectName))
              settings[key.SectName] = settingsAttributeList2.ToArray();
            else
              settings.Add(key.SectName, settingsAttributeList2.ToArray());
          }
        }
      }
      if (settings.Count > 0)
        service4.SetSettings(this.SettingsName, settings);
      else
        service4.ClearSettings(this.SettingsName);
    }
    finally
    {
      sequentialDataReader.Close();
      service3?.ReleaseCache(ImportingCategory.ArticleTypes);
      if (PumpArticleAttributes.articleAttributes != null)
        PumpArticleAttributes.articleAttributes.Clear();
    }
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Определение количества записей", 0);
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.ArticleTypes);
    try
    {
      string format = "Сохранение новых идентификаторов типов изделий ({0} из {1})";
      int index = 0;
      Dictionary<object, DictionaryValue> category = cache.GetCategory();
      int count = category.Count;
      foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category)
      {
        ++index;
        this.PumpCheckPoint(string.Format(format, (object) index, (object) count), this.CalculatePercent(count, index, 1, 99));
        DictionaryValue dictionaryValue = keyValuePair.Value;
        IDBObjectType objectType = userSession.GetObjectType(new Guid(dictionaryValue.Caption), false);
        if (objectType == null)
          this.plugin.appManager.AddErrorMessage($"Тип изделий (search id= {keyValuePair.Key}, guid = {dictionaryValue.Caption} я не найден в базе назначения!");
        else
          cache.SetNewKey((object) (long) keyValuePair.Key, Convert.ToInt64(objectType.ObjectType));
      }
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ArticleTypes);
    }
    this.PumpCheckPoint("Сохранение новых идентификаторов типов изделий успешно завершена", 100);
  }
}
