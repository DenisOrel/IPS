// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpDocTypes
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
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки типов документов Search", "Перекачка данных о типах документов Search")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpDocTypes(SearchPlugin plugin) : PumpSearchClass(plugin, "DOC_TYPES")
{
  private Guid _guid = new Guid("{A8FD9F9F-3770-4189-B5CD-E21ED548ADDE}");
  internal List<Tuple<IDocTypeItem, SettingsObjectTypeItem>> docTypesSettings;
  private const string GroupName = "Типы документов";
  private SettingsGroup _sgGroup;
  private SpecialDocumentTypes _specialDocumentTypes;

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this._sgGroup = new SettingsGroup("Типы документов", SettingsGroupType.DocTypes);
    this._specialDocumentTypes = new SpecialDocumentTypes();
    if (ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) is ISettingsGroupService service1)
      service1.Groups.Add((ISettingsGroup) this._sgGroup);
    this._sgGroup.ObjectCreated += new ObjectCreatedEventHandler(this.ObjectTypesCreated);
    this.ExamCheckPoint("Определение количества записей", 1);
    int tableRecordsCount = this.GetTableRecordsCount(DocTypeItemFactory.TableName);
    int index1 = 0;
    this.docTypesSettings = new List<Tuple<IDocTypeItem, SettingsObjectTypeItem>>();
    this.ExamCheckPoint("Получение данных из таблицы " + DocTypeItemFactory.TableName, 2);
    IDataReader sequentialDataReader = this.GetSequentialDataReader(DocTypeItemFactory.TableName, DocTypeItemFactory.TableColumns);
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    IDBObjectType objectType1 = userSession.GetObjectType(new Guid("cad00070-306c-11d8-b4e9-00304f19f545"));
    Guid anObjectTypeGuid = new Guid("cadd9ab3-306c-11d8-b4e9-00304f19f545");
    IDBObjectType objectType2 = userSession.GetObjectType(anObjectTypeGuid);
    Guid guid1 = (userSession.GetRelationType(objectType2.DefaultRelation) as IDBGuid).GUID;
    Guid guid2 = new Guid("cad00809-306c-11d8-b4e9-00304f19f545");
    DataTable toTable = userSession.GetObjectTypeCollection(objectType1.ObjectType).SelectRecursive("F_OBJ_TYPE_NAME");
    IDBObjectType objectType3 = userSession.GetObjectType(new Guid("cad00163-306c-11d8-b4e9-00304f19f545"));
    DataTable dataTable = userSession.GetObjectTypeCollection(objectType3.ObjectType).SelectRecursive("F_OBJ_TYPE_NAME");
    foreach (DataRow row in (InternalDataCollectionBase) toTable.Rows)
      SearchHelper.NormalizeObjTypeNames(row);
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      SearchHelper.NormalizeObjTypeNames(row);
      DataSetProcessor.AddRow(toTable, row, false);
    }
    toTable.AcceptChanges();
    IObjectTypeToCreateList service2 = ServicesManager.ServiceContainer.GetService(typeof (IObjectTypeToCreateList)) as IObjectTypeToCreateList;
    string searchImagesFolder = SearchHelper.GetSearchImagesFolder();
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings(this.SettingsName);
    try
    {
      string format = $"Обработка записи из таблицы {DocTypeItemFactory.TableName} ({{0}} из {{1}})";
      DocTypeItemFactory docTypeItemFactory = new DocTypeItemFactory(DocTypeItemFactory.TableName, sequentialDataReader, this.plugin.Idw.AppManager);
      while (sequentialDataReader.Read())
      {
        ++index1;
        this.ExamCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, 3, 99));
        IDocTypeItem docTypeItem = docTypeItemFactory.NewItem(sequentialDataReader, this.plugin.Imdi.NewPumpGuid());
        if (CompareValuesHelper.NormalizedValue((object) docTypeItem.DocExt) != null && docTypeItem.DocExt != "*" && docTypeItem.DocExt.Trim() != string.Empty && docTypeItem.DocExt[0] != '.' && !docTypeItem.DocExt.Contains("%"))
          docTypeItem.DocExt = $".{docTypeItem.DocExt}";
        if (CompareValuesHelper.NormalizedValue((object) docTypeItem.LinkedExt) != null)
        {
          string[] strArray = docTypeItem.LinkedExt.Split(';');
          docTypeItem.LinkedExt = string.Empty;
          if (strArray != null && strArray.Length != 0)
          {
            for (int index2 = 0; index2 < strArray.Length; ++index2)
            {
              if (strArray[index2].Trim() != string.Empty && !strArray[index2].Contains("%") && strArray[index2][0] != '.')
                docTypeItem.LinkedExt += $".{strArray[index2]},";
            }
            if (docTypeItem.LinkedExt.Length > 0)
              docTypeItem.LinkedExt = docTypeItem.LinkedExt.Remove(docTypeItem.LinkedExt.Length - 1, 1);
          }
        }
        bool flag1 = false;
        SaveSettingsAttribute[] settingsAttributeArray = (SaveSettingsAttribute[]) null;
        if (service1 != null)
        {
          SettingsObjectTypeItem settingsObjectTypeItem = new SettingsObjectTypeItem(docTypeItem.DocName, string.Empty, docTypeItem.DocType);
          if (settings != null && settings.ContainsKey(docTypeItem.DocName))
          {
            settingsAttributeArray = settings[docTypeItem.DocName];
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
            Guid guid3 = this._specialDocumentTypes.Find(docTypeItem.DocName);
            bool flag2 = guid3 != Guid.Empty;
            if (flag2)
            {
              settingsObjectTypeItem.AttrGuid = guid3;
            }
            else
            {
              bool flag3 = false;
              IObjectTypeToCreate objectTypeToCreate1 = service2.GetByName(docTypeItem.DocName);
              if (objectTypeToCreate1 == null)
              {
                DataRow[] dataRowArray = toTable.Select(string.Format("F_OBJ_TYPE_NAME = {0} OR F_OBJ_NAME = {0}", (object) DataSetProcessor.QString(SearchHelper.NormalizeName(docTypeItem.DocName))));
                if (dataRowArray.Length == 1)
                  objectTypeToCreate1 = service2.GetByGuid(new Guid(Convert.ToString(dataRowArray[0]["F_GUID"])));
              }
              if (objectTypeToCreate1 == null)
              {
                objectTypeToCreate1 = service2.GetByShortName(docTypeItem.DocCode);
                if (objectTypeToCreate1 != null)
                  flag3 = true;
              }
              if (objectTypeToCreate1 != null)
              {
                if ((objectTypeToCreate1.VersionMode != ObjectVersionModes.Abstract || docTypeItem.VersionMode == ObjectVersionModes.Abstract) && toTable.Select($"F_OBJ_TYPE_NAME = {DataSetProcessor.QString(SearchHelper.NormalizeName(objectTypeToCreate1.Name))}").Length != 0)
                {
                  settingsObjectTypeItem.AttrGuid = objectTypeToCreate1.GUID;
                  flag2 = true;
                }
                else if (!flag3)
                {
                  List<string> stringList = new List<string>()
                  {
                    $"Тип документов исходной базы переименован, так как в базе назначения{$"существует тип объектов \"{objectTypeToCreate1.Name}\", который либо абстрактный,"}либо не входит в иерархию типов документов и технологических типов"
                  };
                  IObjectTypeToCreate byShortName = service2.GetByShortName(docTypeItem.DocCode);
                  if (byShortName != null)
                  {
                    stringList.Add($"Код типа документов \"{docTypeItem.DocCode}\" исходной базы очищен, так как в базе назначения" + $"существует тип объектов \"{byShortName.Name}\" с таким кодом типа документов");
                    docTypeItem.DocCode = string.Empty;
                  }
                  settingsObjectTypeItem.Error = new ItemError(ItemErrorType.Renamed, stringList.ToArray());
                }
                else
                {
                  settingsObjectTypeItem.Error = new ItemError(ItemErrorType.Renamed, $"Код типа документов \"{docTypeItem.DocCode}\" исходной базы очищен, так как в базе назначения" + $"существует тип объектов \"{objectTypeToCreate1.Name}\" с таким кодом типа документов");
                  docTypeItem.DocCode = string.Empty;
                }
              }
              if (!flag2)
              {
                settingsObjectTypeItem.AttrGuid = docTypeItem.Guid;
                docTypeItem.Icon = (byte[]) null;
                if (docTypeItem.Bitmap.Trim() != string.Empty && searchImagesFolder != string.Empty)
                  docTypeItem.Icon = SearchHelper.GetIcon(Path.Combine(searchImagesFolder, docTypeItem.Bitmap));
                string str = settingsObjectTypeItem.Error == null || !settingsObjectTypeItem.Error.ErrorMessages.Exists((Predicate<MessageItem>) (x => x.ErrorType == ItemErrorType.Renamed)) ? docTypeItem.DocName : $"{docTypeItem.DocName}_{docTypeItem.Guid.ToString()}";
                IObjectTypeToCreate objectTypeToCreate2 = service2.AddItem(true, str, docTypeItem.DocCode, str, docTypeItem.Guid, long.MaxValue, docTypeItem.Icon, docTypeItem.VersionMode);
                if (settingsAttributeArray != null)
                {
                  foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
                  {
                    switch (settingsAttribute.AttributeName)
                    {
                      case "ANY_ATTRIBUTE":
                        bool boolean = Convert.ToBoolean(settingsAttribute.AttributeValue);
                        if (objectTypeToCreate2.AnyAttributes != boolean)
                        {
                          objectTypeToCreate2.AnyAttributes = boolean;
                          break;
                        }
                        break;
                      case "ICON":
                        byte[] numArray = Convert.FromBase64String(settingsAttribute.AttributeValue);
                        if (objectTypeToCreate2.Icon != numArray)
                        {
                          objectTypeToCreate2.Icon = numArray;
                          break;
                        }
                        break;
                      case "INST_NAME":
                        if (objectTypeToCreate2.InstanceName != settingsAttribute.AttributeValue)
                        {
                          objectTypeToCreate2.InstanceName = settingsAttribute.AttributeValue;
                          break;
                        }
                        break;
                      case "LC_SCHEME":
                        IDBLCSchema lcSchema = userSession.GetLCSchema(new Guid(settingsAttribute.AttributeValue), false);
                        if (lcSchema != null)
                        {
                          objectTypeToCreate2.LcShemaId = lcSchema.GUID;
                          break;
                        }
                        break;
                      case "NOTE":
                        if (objectTypeToCreate2.Note != settingsAttribute.AttributeValue)
                        {
                          objectTypeToCreate2.Note = settingsAttribute.AttributeValue;
                          break;
                        }
                        break;
                      case "PARENT":
                        IObjectTypeItem byGuid1 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid(settingsAttribute.AttributeValue));
                        if (byGuid1 != null)
                        {
                          objectTypeToCreate2.ParentTypeId = byGuid1.GUID;
                          break;
                        }
                        break;
                      case "RELATION_TYPE":
                        IRelationTypeItem byGuid2 = this.plugin.Imdi.RelationTypes.GetByGuid(new Guid(settingsAttribute.AttributeValue));
                        if (byGuid2 != null)
                        {
                          objectTypeToCreate2.DefaultRelationId = byGuid2.GUID;
                          break;
                        }
                        break;
                      case "SHORT_NAME":
                        if (objectTypeToCreate2.ShortName != settingsAttribute.AttributeValue)
                        {
                          objectTypeToCreate2.ShortName = settingsAttribute.AttributeValue;
                          break;
                        }
                        break;
                      case "VERSIONABLE":
                        ObjectVersionModes int32 = (ObjectVersionModes) Convert.ToInt32(settingsAttribute.AttributeValue);
                        if (objectTypeToCreate2.VersionMode != int32)
                        {
                          objectTypeToCreate2.VersionMode = int32;
                          break;
                        }
                        break;
                    }
                  }
                }
                if (objectTypeToCreate2.ParentTypeId == Guid.Empty)
                {
                  objectTypeToCreate2.ParentTypeId = anObjectTypeGuid;
                  docTypeItem.ParentID = anObjectTypeGuid;
                }
                if (objectTypeToCreate2.DefaultRelationId == Guid.Empty)
                {
                  objectTypeToCreate2.DefaultRelationId = guid1;
                  docTypeItem.DefRelation = guid1;
                }
                if (objectTypeToCreate2.LcShemaId == Guid.Empty)
                {
                  objectTypeToCreate2.LcShemaId = guid2;
                  docTypeItem.LCScheme = guid2;
                }
              }
            }
          }
          this._sgGroup.GroupItems.Add((ISettingsGroupItem) settingsObjectTypeItem);
          this.docTypesSettings.Add(new Tuple<IDocTypeItem, SettingsObjectTypeItem>(docTypeItem, settingsObjectTypeItem));
        }
      }
    }
    finally
    {
      sequentialDataReader.Close();
    }
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  private void ObjectTypesCreated()
  {
    ICache service1 = ServicesManager.GetService(typeof (ICache)) as ICache;
    service1.DeleteCache(ImportingCategory.DocTypes);
    IImportingData cache = service1.GetCache(ImportingCategory.DocTypes);
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    IDataReader sequentialDataReader = this.GetSequentialDataReader(Params4DocTypesItemFactory.TableName, Params4DocTypesItemFactory.TableColumns);
    try
    {
      IParams4DocTypesItems params4DocTypesItems = new Params4DocTypesItemFactory(Params4DocTypesItemFactory.TableName, sequentialDataReader, this.plugin.Idw.AppManager).Params4DocTypes(sequentialDataReader);
      ISaveSettings service2 = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
      Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>();
      foreach (Tuple<IDocTypeItem, SettingsObjectTypeItem> docTypesSetting in this.docTypesSettings)
      {
        IDocTypeItem docTypeItem = docTypesSetting.Item1;
        SettingsObjectTypeItem settingsObjectTypeItem = docTypesSetting.Item2;
        if (!(settingsObjectTypeItem.AttrGuid == Guid.Empty))
        {
          List<SaveSettingsAttribute> settingsAttributeList1 = new List<SaveSettingsAttribute>();
          Guid guid = settingsObjectTypeItem.AttrGuid;
          settingsAttributeList1.Add(new SaveSettingsAttribute("GUID", guid.ToString()));
          List<SaveSettingsAttribute> settingsAttributeList2 = settingsAttributeList1;
          guid = docTypeItem.Guid;
          if (!guid.Equals(settingsObjectTypeItem.AttrGuid))
          {
            IObjectTypeItem byGuid = this.plugin.Imdi.ObjectTypes.GetByGuid(settingsObjectTypeItem.AttrGuid);
            if (byGuid != null)
              cache.AddValue(ImportingCategory.DocTypes, (object) docTypeItem.DocType, Convert.ToInt64(byGuid.ID));
            if (settingsObjectTypeItem.Caption != docTypeItem.DocName)
              docTypeItem.DocName = settingsObjectTypeItem.Caption;
          }
          else
          {
            guid = docTypeItem.Guid;
            DocTypesSettings tag = new DocTypesSettings(guid.ToString(), docTypeItem.LinkedExt, docTypeItem.DocExt, docTypeItem.DocCode, docTypeItem.DocName, docTypeItem.ProtoName, docTypeItem.Classif, docTypeItem.DrawStamp, docTypeItem.Suffix, docTypeItem.FileBody);
            IObjectTypeItem byGuid1 = this.plugin.Imdi.ObjectTypes.GetByGuid(docTypeItem.Guid);
            cache.AddValue(ImportingCategory.DocTypes, (object) docTypeItem.DocType, Convert.ToInt64(byGuid1.ID), (ITagImportObject) tag);
            if (byGuid1.ObjectName != docTypeItem.DocName)
              settingsAttributeList2.Add(new SaveSettingsAttribute("INST_NAME", byGuid1.ObjectName));
            if (byGuid1.ShortName != docTypeItem.DocName)
              settingsAttributeList2.Add(new SaveSettingsAttribute("SHORT_NAME", byGuid1.ShortName));
            if (byGuid1.Note != string.Empty)
              settingsAttributeList2.Add(new SaveSettingsAttribute("NOTE", byGuid1.Note));
            if (byGuid1.ParentID != docTypeItem.ParentID)
            {
              IObjectTypeItem byGuid2 = this.plugin.Imdi.ObjectTypes.GetByGuid(byGuid1.ParentID);
              if (byGuid2 != null)
              {
                List<SaveSettingsAttribute> settingsAttributeList3 = settingsAttributeList2;
                guid = byGuid2.GUID;
                SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("PARENT", guid.ToString());
                settingsAttributeList3.Add(settingsAttribute);
              }
            }
            if (byGuid1.RelationID != docTypeItem.DefRelation)
            {
              IRelationTypeItem byGuid3 = this.plugin.Imdi.RelationTypes.GetByGuid(byGuid1.RelationID);
              if (byGuid3 != null)
              {
                List<SaveSettingsAttribute> settingsAttributeList4 = settingsAttributeList2;
                guid = byGuid3.GUID;
                SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("RELATION_TYPE", guid.ToString());
                settingsAttributeList4.Add(settingsAttribute);
              }
            }
            if (byGuid1.ShemaId != docTypeItem.LCScheme)
            {
              IDBLCSchema lcSchema = userSession.GetLCSchema(byGuid1.ShemaId, false);
              if (lcSchema != null)
              {
                List<SaveSettingsAttribute> settingsAttributeList5 = settingsAttributeList2;
                guid = lcSchema.GUID;
                SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("LC_SCHEME", guid.ToString());
                settingsAttributeList5.Add(settingsAttribute);
              }
            }
            if (byGuid1.VersionableMode != docTypeItem.VersionMode)
              settingsAttributeList2.Add(new SaveSettingsAttribute("VERSIONABLE", ((int) byGuid1.VersionableMode).ToString()));
            if (byGuid1.AnyAttribute != docTypeItem.AnyAttribute)
              settingsAttributeList2.Add(new SaveSettingsAttribute("ANY_ATTRIBUTE", byGuid1.AnyAttribute.ToString()));
            if (byGuid1.Icon != docTypeItem.Icon)
            {
              string str = string.Empty;
              if (byGuid1.Icon != null && byGuid1.Icon.Length != 0)
                str = Convert.ToBase64String(byGuid1.Icon);
              settingsAttributeList2.Add(new SaveSettingsAttribute("ICON", str));
            }
            List<int> groups = params4DocTypesItems.GetGroups(docTypeItem.DocType);
            if (groups != null)
            {
              foreach (int key1 in groups)
              {
                List<int> intList = (List<int>) null;
                if (PumpThematicParamsGroups.ThematicParamsInGroups.TryGetValue(key1, out intList))
                {
                  foreach (int key2 in intList)
                  {
                    IAttributeTypeItem byGuid4 = this.plugin.Imdi.AttributeTypes.GetByGuid(PumpThematicParams.ThematicParams[key2]);
                    this.plugin.Imdi.ObjectTypes.LinkAttributeTypeToObjectType(byGuid4.ID, byGuid1.ID, true, RequiredModes.Manual, string.Empty, ComputeValueModes.NotComputableValue, string.Empty, UniqueValueModes.NotUnique, 0, byGuid4.DefaultValue != null ? byGuid4.DefaultValue.ToString() : string.Empty, OptimizationModes.Write, false, AttributeOptions.None, string.Empty, 0, 0);
                  }
                }
              }
            }
          }
          if (settings.ContainsKey(docTypeItem.DocName))
            settings[docTypeItem.DocName] = settingsAttributeList2.ToArray();
          else
            settings.Add(docTypeItem.DocName, settingsAttributeList2.ToArray());
        }
      }
      if (settings.Count > 0)
        service2.SetSettings(this.SettingsName, settings);
      else
        service2.ClearSettings(this.SettingsName);
    }
    finally
    {
      sequentialDataReader.Close();
      service1?.ReleaseCache(ImportingCategory.DocTypes);
      if (this.docTypesSettings != null)
        this.docTypesSettings.Clear();
      if (PumpThematicParams.ThematicParams != null)
        PumpThematicParams.ThematicParams.Clear();
      if (PumpThematicParamsGroups.ThematicParamsInGroups != null)
        PumpThematicParamsGroups.ThematicParamsInGroups.Clear();
    }
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Определение количества записей", 0);
    this.PumpCheckPoint("Загрузка свойств типов документов", 10);
    string format = "Импорт свойств типа документа ({0} из {1})";
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    IDocumentTypeSettingsService customService1 = userSession.GetCustomService(typeof (IDocumentTypeSettingsService)) as IDocumentTypeSettingsService;
    IObjectTypeItem byGuid1 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00346-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid2 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid3 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00149-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid4 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad001d9-306c-11d8-b4e9-00304f19f545"));
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cacheData = service.GetCache(ImportingCategory.DocTypes, ImportingCategory.ArticleTypes, ImportingCategory.Settings4DocTypes);
    IDataReader sequentialDataReader = this.GetSequentialDataReader(MadeTypesItemFactory.TableName, MadeTypesItemFactory.TableColumns);
    try
    {
      IMadeTypesItem madeTypesItem = new MadeTypesItemFactory(MadeTypesItemFactory.TableName, sequentialDataReader, this.plugin.Idw.AppManager).MadeTypesItems(sequentialDataReader);
      IContainerService customService2 = userSession.GetCustomService(typeof (IContainerService)) as IContainerService;
      if (customService1 != null)
      {
        int index1 = 0;
        IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
        List<long> importDocTypes = new List<long>();
        iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          for (int index2 = 0; index2 < iolIm.Items.Count; ++index2)
          {
            if (iolIm.Items[index2].Object.Object_id != 0L)
              cacheData.AddValue(ImportingCategory.Settings4DocTypes, (object) importDocTypes[index2], iolIm.Items[index2].Object.Object_id);
          }
          importDocTypes.Clear();
        });
        List<string> stringList = new List<string>();
        try
        {
          Dictionary<object, DictionaryValue> category = cacheData.GetCategory(ImportingCategory.DocTypes);
          if (category != null && category.Count > 0)
          {
            foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category)
            {
              ++index1;
              this.PumpCheckPoint(string.Format(format, (object) index1, (object) category.Count), this.CalculatePercent(category.Count, index1, 10, 99));
              DictionaryValue dictionaryValue = keyValuePair.Value;
              long key = (long) keyValuePair.Key;
              if (cacheData.GetNewKey(ImportingCategory.Settings4DocTypes, (object) key) == 0L && dictionaryValue.Tag != null)
              {
                DocTypesSettings tag = (DocTypesSettings) dictionaryValue.Tag;
                DocumentTypeSettings documentTypeSettingsData = new DocumentTypeSettings();
                documentTypeSettingsData.AdditionalDocumentFileExts = tag.LinkedExt;
                documentTypeSettingsData.DocumentFileExt = tag.DocExt;
                documentTypeSettingsData.DocumentNameInStamp = tag.DrawStamp == 1;
                documentTypeSettingsData.DocumentTypeCode = tag.DocCode;
                documentTypeSettingsData.DocumentTypeCodeInDesignation = tag.Suffix == 1;
                documentTypeSettingsData.DocumentTypeName = tag.DocName;
                List<int> objectTypes = madeTypesItem.GetObjectTypes(Convert.ToInt32(key));
                if (objectTypes != null)
                {
                  string str1 = string.Empty;
                  foreach (int oldKey in objectTypes)
                  {
                    string caption = cacheData.GetCaption(ImportingCategory.ArticleTypes, (object) oldKey);
                    if (caption != string.Empty)
                      str1 = $"{str1}{caption.ToString()},";
                  }
                  if (str1 != string.Empty)
                  {
                    string str2 = str1.Remove(str1.Length - 1, 1);
                    documentTypeSettingsData.OutputObjectTypes = str2;
                  }
                }
                IDBObjectType objectType = userSession.GetObjectType(new Guid(tag.Guid));
                cacheData.SetNewKey(ImportingCategory.DocTypes, (object) key, Convert.ToInt64(objectType.ObjectType));
                customService1.SetSettings(userSession.SessionGUID, objectType.ObjectType, documentTypeSettingsData);
                if (tag.ProtoName != string.Empty && tag.FileBody != null)
                {
                  iolIm.AddObject(byGuid1.ID, 0, tag.DocName);
                  int fileSize = 0;
                  string str = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.tmp");
                  stringList.Add(str);
                  FileStream fileStream = File.OpenWrite(str);
                  try
                  {
                    fileStream.Write(tag.FileBody, 0, tag.FileBody.Length);
                    fileSize = Convert.ToInt32(fileStream.Length);
                  }
                  finally
                  {
                    fileStream.Flush();
                    fileStream.Close();
                  }
                  iolIm.AddAttributeBlob(byGuid2.ID, str, (long) fileSize, tag.ProtoName, ArcMethods.ZLibPacked);
                  iolIm.AddAttributeStr(byGuid3.ID, tag.Guid.ToString());
                  AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), iolIm);
                  importDocTypes.Add(key);
                }
                if (tag.Classif == "I")
                  customService2.GetContainerForObjectType((object) userSession.SessionGUID, new Guid(tag.Guid), true).Attributes.AddAttribute(byGuid4.ID, false).AsInteger = 1L;
              }
            }
          }
          if (iolIm.Items.Count > 0)
            iolIm.Import();
        }
        finally
        {
          foreach (string path in stringList)
            File.Delete(path);
        }
      }
    }
    finally
    {
      sequentialDataReader.Close();
      service?.ReleaseCache(ImportingCategory.DocTypes, ImportingCategory.ArticleTypes, ImportingCategory.Settings4DocTypes);
    }
    this.PumpCheckPoint("Загрузка свойств типов документов успешно завершена", 100);
  }
}
