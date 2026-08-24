// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpImbaseCatalogs
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.SelectionService;
using Intermech.Kernel.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("Получение информации о каталогах Imbase", "Перекачка каталогов Imbase")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpImbaseCatalogs(ImbasePlugin plugin) : PumpImbaseClass(plugin)
{
  internal static Guid _guid = new Guid("{7FAC2211-5166-4aec-B2BB-F89C45739194}");
  private Guid _attrSPGUID = Guid.Empty;
  private int _attrCreateType;
  private Dictionary<int, string> _dictionarySp;
  private string _lastCatalogKey = string.Empty;
  private string _nextFolderKey = string.Empty;
  private bool _replaceComposition;
  private int _attrMainPerehTextID;
  private int _attrDopPerehTextID;
  private int _attrCommentTextID;
  private int _attrNameMaxSize;

  protected override Guid GUID => PumpImbaseCatalogs._guid;

  public override void Pump()
  {
    this.PumpCheckPoint("Определение количества записей для закачки информации о каталогах IMBASE", 0);
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseGroups, ImportingCategory.ImbaseTablesAttributes, ImportingCategory.ImbaseCatalogsCreated, ImportingCategory.ImbaseFolderKeyToLevel, ImportingCategory.ImbaseTables, ImportingCategory.ImbaseGroupsAttributes, ImportingCategory.ImbaseFolderRelations, ImportingCategory.ImbaseCatalogs, ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseFolders, ImportingCategory.ImbaseTableLinks, ImportingCategory.ImbaseCatalogBinding, ImportingCategory.ImbaseCatalogBindingType, ImportingCategory.ImbaseFoldersGuids, ImportingCategory.ImbaseTcPerehLinks, ImportingCategory.ImbaseTcCommectsLinks, ImportingCategory.ImbaseLinksFolder, ImportingCategory.ImbaseLinksTableLinks, ImportingCategory.ImbaseBindedMeasures, ImportingCategory.OborudFolders, ImportingCategory.OborudFieldTypes, ImportingCategory.ImbaseTableLinksInFolders, ImportingCategory.ImbaseTableLinksKeyToObjectID, ImportingCategory.ImbaseFoldersClassificators, ImportingCategory.ImbaseMixTables);
    this._attrSPGUID = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00210-306c-11d8-b4e9-00304f19f545")).GUID;
    this._attrCreateType = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00203-306c-11d8-b4e9-00304f19f545")).ID;
    IAttributeTypeItem byGuid1 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad005ce-306c-11d8-b4e9-00304f19f545"));
    if (byGuid1 != null)
      this._attrMainPerehTextID = byGuid1.ID;
    IAttributeTypeItem byGuid2 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad005cf-306c-11d8-b4e9-00304f19f545"));
    if (byGuid2 != null)
      this._attrDopPerehTextID = byGuid2.ID;
    IAttributeTypeItem byGuid3 = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad005d4-306c-11d8-b4e9-00304f19f545"));
    if (byGuid3 != null)
      this._attrCommentTextID = byGuid3.ID;
    IObjectTypeItem byGuid4 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad0057f-306c-11d8-b4e9-00304f19f545"));
    IObjectTypeItem byGuid5 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad0025e-306c-11d8-b4e9-00304f19f545"));
    IObjectTypeItem byGuid6 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00132-306c-11d8-b4e9-00304f19f545"));
    IObjectTypeItem byGuid7 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00250-306c-11d8-b4e9-00304f19f545"));
    IObjectTypeItem byGuid8 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00252-306c-11d8-b4e9-00304f19f545"));
    IObjectTypeItem byGuid9 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad0038d-306c-11d8-b4e9-00304f19f545"));
    IObjectTypeItem byGuid10 = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad00172-306c-11d8-b4e9-00304f19f545"));
    this._attrNameMaxSize = this.plugin.Imdi.AttributeTypes.GetByID(ImbaseIDHelper.AttrIdName).MaxSize;
    this._dictionarySp = new Dictionary<int, string>(9);
    this._dictionarySp.Add(1, byGuid4.GUID.ToString());
    this._dictionarySp.Add(2, byGuid5.GUID.ToString());
    this._dictionarySp.Add(3, byGuid6.GUID.ToString());
    this._dictionarySp.Add(4, byGuid7.GUID.ToString());
    this._dictionarySp.Add(5, byGuid8.GUID.ToString());
    this._dictionarySp.Add(6, byGuid9.GUID.ToString());
    this._dictionarySp.Add(7, byGuid10.GUID.ToString());
    this._dictionarySp.Add(8, "cad0025d-306c-11d8-b4e9-00304f19f545");
    this._dictionarySp.Add(9, "cad00271-306c-11d8-b4e9-00304f19f545");
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    this._replaceComposition = cache.GetNewKey(ImportingCategory.ImbaseCatalogBindingType, (object) "Replace") == 1L;
    Dictionary<object, DictionaryValue> category = cache.GetCategory(ImportingCategory.ImbaseGroups);
    if (category != null && category.Count > 0)
    {
      List<ImbaseGroup> imbaseGroupList = new List<ImbaseGroup>(category.Count);
      try
      {
        this.PumpCheckPoint("Подготовка списка создаваемых каталогов IMBASE", 1);
        foreach (DictionaryValue dictionaryValue in category.Values)
        {
          if (dictionaryValue.Tag is ImbaseGroup tag && (tag.TableType == 1 || tag.TableType == 2 || tag.TableType == 3) && ImbasePlugin.IsCatalogToPump(tag.TableName) && cache.GetNewKey(ImportingCategory.ImbaseCatalogs, (object) tag.Key) == 0L && cache.GetNewKey(ImportingCategory.ImbaseCatalogBinding, (object) tag.TableName) == 1L)
            imbaseGroupList.Add(tag);
        }
        int count = imbaseGroupList.Count;
        int index = 0;
        string format = "Закачка данных о каталогах IMBASE ({0} из {1})";
        string empty = string.Empty;
        foreach (ImbaseGroup tableRec in imbaseGroupList)
        {
          ++index;
          string str = string.Format(format, (object) index, (object) count);
          int percent = this.CalculatePercent(count, index, 2, 99);
          this.PumpCheckPoint(str, percent);
          this.pumpCatalog(userSession, tableRec, cache, percent, str);
        }
      }
      finally
      {
        service?.ReleaseCache(ImportingCategory.ImbaseGroups, ImportingCategory.ImbaseTablesAttributes, ImportingCategory.ImbaseCatalogsCreated, ImportingCategory.ImbaseFolderKeyToLevel, ImportingCategory.ImbaseTables, ImportingCategory.ImbaseGroupsAttributes, ImportingCategory.ImbaseFolderRelations, ImportingCategory.ImbaseCatalogs, ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseFolders, ImportingCategory.ImbaseTableLinks, ImportingCategory.ImbaseCatalogBinding, ImportingCategory.ImbaseCatalogBindingType, ImportingCategory.ImbaseFoldersGuids, ImportingCategory.ImbaseTcPerehLinks, ImportingCategory.ImbaseTcCommectsLinks, ImportingCategory.ImbaseLinksFolder, ImportingCategory.ImbaseLinksTableLinks, ImportingCategory.ImbaseBindedMeasures, ImportingCategory.OborudFolders, ImportingCategory.OborudFieldTypes, ImportingCategory.ImbaseTableLinksInFolders, ImportingCategory.ImbaseTableLinksKeyToObjectID, ImportingCategory.ImbaseFoldersClassificators, ImportingCategory.ImbaseMixTables);
      }
    }
    this.PumpCheckPoint("Создание каталогов IMBASE успешно завершено", 100);
  }

  private string GenerateNextCatalogKey(IUserSession uSession)
  {
    string empty = string.Empty;
    string nextCatalogKey;
    if (this._lastCatalogKey == string.Empty)
    {
      if (!(uSession.GetCustomService(typeof (ISelectionsService)) is ISelectionsService customService))
        throw new Exception("Не найден сервис ISelectionsService на сервере приложений");
      nextCatalogKey = customService.GenerateNextTopLevelKey((object) uSession.SessionGUID, ImbaseIDHelper.ObjTypeIdImCtl);
    }
    else
      nextCatalogKey = ClassifierKeyValueGenerator.GetNextKeyValue(this._lastCatalogKey);
    this._lastCatalogKey = nextCatalogKey;
    return nextCatalogKey;
  }

  private void pumpCatalog(
    IUserSession uSession,
    ImbaseGroup tableRec,
    IImportingData cacheData,
    int progress,
    string curProcessStr)
  {
    string caption1 = cacheData.GetCaption(ImportingCategory.ImbaseCatalogBinding, (object) tableRec.TableName);
    IMetadataInfo service1 = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    ISelectionsService customService = uSession.GetCustomService(typeof (ISelectionsService)) as ISelectionsService;
    string str1 = tableRec.TableName.ToUpper().Trim();
    bool flag1 = str1.Equals("CADMECH") || str1.Equals("TEHNIKON");
    int num1 = 0;
    string str2 = tableRec.TableName + "_REC";
    Dictionary<int, ImCatalogItem> items = new Dictionary<int, ImCatalogItem>();
    Dictionary<int, ImCatalogRecItem> folderRecDict = new Dictionary<int, ImCatalogRecItem>();
    Dictionary<long, List<int>> foldersAttributes = new Dictionary<long, List<int>>();
    List<long> longList1 = new List<long>();
    string empty = string.Empty;
    bool flag2 = str1.ToUpper().Equals("TC_OBORUD");
    IDBObject dbObject = (IDBObject) null;
    DictionaryValue dictionaryValue1 = cacheData.GetValue(ImportingCategory.ImbaseCatalogsCreated, (object) tableRec.Key);
    string str3;
    long num2;
    if (dictionaryValue1 != null)
    {
      str3 = dictionaryValue1.Caption;
      num2 = dictionaryValue1.NewObjectID;
      if (str3 != string.Empty)
        this._nextFolderKey = customService.GenerateNextClassifierKey((object) uSession, ImbaseIDHelper.ObjTypeIdImCtl, str3, ImbaseIDHelper.ObjTypeIdImFolder);
    }
    else
    {
      if (caption1 != string.Empty)
        dbObject = uSession.GetObject(new Guid(caption1), false);
      if (dbObject == null)
      {
        str3 = this.GenerateNextCatalogKey(uSession);
        IImportedObjectList importedObjectList = this.plugin.Idw.CreateImportedObjectList(0);
        string caption2 = this.CheckNameSize(tableRec.Description);
        importedObjectList.AddObject(ImbaseIDHelper.ObjTypeIdImCtl, num1, caption2);
        importedObjectList.AddAttributeStr(ImbaseIDHelper.AttrIdName, caption2);
        importedObjectList.AddAttributeStr(ImbaseIDHelper.AttrIdClassifierKey, str3);
        string str4 = string.Empty;
        switch (tableRec.TableType)
        {
          case 1:
            str4 = "Каталоги";
            break;
          case 2:
            str4 = "Справочники";
            break;
          case 3:
            str4 = "Технологические справочники";
            break;
        }
        importedObjectList.AddAttributeStr(ImbaseIDHelper.AttrIdImTypeCtl, str4);
        importedObjectList.AddAttributeInt(ImbaseIDHelper.AttrIdImCode, (long) tableRec.Key);
        importedObjectList.AddAttributeStr(ImbaseIDHelper.AttrIdTableName, tableRec.TableName);
        AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), importedObjectList);
        importedObjectList.Import();
        num2 = importedObjectList.Items[0].Object.Object_id;
        importedObjectList.Items.Clear();
        this._nextFolderKey = string.Empty;
      }
      else
      {
        num2 = dbObject.ObjectID;
        bool flag3 = false;
        IDBAttribute attributeById = dbObject.GetAttributeByID(ImbaseIDHelper.AttrIdClassifierKey);
        if (!string.IsNullOrEmpty(attributeById.AsString))
        {
          str3 = attributeById.AsString;
        }
        else
        {
          str3 = this.GenerateNextCatalogKey(uSession);
          attributeById.AsString = str3;
          flag3 = true;
        }
        if (this._replaceComposition && !flag3)
        {
          DBRecordSetParams paramSet1 = new DBRecordSetParams(new ConditionStructure[1]
          {
            new ConditionStructure(ImbaseIDHelper.AttrIdClassifierKey, RelationalOperators.StartString, (object) str3, LogicalOperators.AND, 0, false)
          }, new ColumnDescriptor[2]
          {
            new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, SortOrders.NONE, 0),
            new ColumnDescriptor((object) ImbaseIDHelper.AttrIdImLinkTable, ColumnContents.ID, ColumnNameMapping.Index, SortOrders.NONE, 0)
          });
          IDBObjectCollection objectCollection = uSession.GetObjectCollection(ImbaseIDHelper.ObjTypeIdImTabLink);
          this.PumpCheckPoint($"Запрос состава каталога \"{tableRec.Description}\" в базе назначения", progress);
          DBRecordSetParams paramSet2 = paramSet1;
          DataTable dataTable1 = objectCollection.Select(paramSet2);
          List<long> longList2 = new List<long>();
          for (int index = 0; index < dataTable1.Rows.Count; ++index)
          {
            if (dataTable1.Rows[index][1] != DBNull.Value)
            {
              long int64 = Convert.ToInt64(dataTable1.Rows[index][1]);
              if (!longList2.Contains(int64))
                longList2.Add(int64);
            }
          }
          string str5 = $"Удаление состава каталога \"{tableRec.Description}\"";
          IDBRelationCollection relationCollection = uSession.GetRelationCollection(uSession.IdentHelper.SortedRelationTypeID);
          relationCollection.ChildObjectTypes = (IList<int>) MetaDataHelper.OptimizeChildObjectTypes((IEnumerable<int>) MetaDataHelper.GetLocalObjectTypeChildrenIDRecursive(ImbaseIDHelper.ObjTypeIdImObject));
          DataTable dataTable2 = relationCollection.ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
          {
            (object) -2
          }), num2, false);
          for (int index = 0; index < dataTable2.Rows.Count; ++index)
          {
            this.PumpCheckPoint($"{str5}: {index + 1} из {dataTable2.Rows.Count}", progress);
            try
            {
              uSession.GetObject(Convert.ToInt64(dataTable2.Rows[index][0]), false)?.Delete(0L);
            }
            catch (Exception ex)
            {
              this.plugin.appManager.AddWarningMessage($"Ошибка при удалении папки/ярлыка {dataTable2.Rows[index][0]} : {ex.Message}");
              this.plugin.appManager.AddExceptionToLog(ex);
            }
          }
          List<long> longList3 = new List<long>();
          long newKey = cacheData.GetNewKey(ImportingCategory.ImbaseTables, (object) "IM_M_OBJS_PROPS");
          if (newKey != 0L)
            longList3.Add(newKey);
          DataTable dataTable3 = uSession.GetObjectCollection(ImbaseIDHelper.ObjTypeIdImTabLink).Select(paramSet1);
          for (int index = 0; index < dataTable3.Rows.Count; ++index)
          {
            object obj = dataTable3.Rows[index][1];
            if (obj != DBNull.Value)
            {
              long int64 = Convert.ToInt64(obj);
              if (!longList3.Contains(int64))
                longList3.Add(int64);
            }
          }
          this.PumpCheckPoint($"Удаление неиспользуемых таблиц каталога \"{tableRec.Description}\"", progress);
          for (int index = 0; index < longList2.Count; ++index)
          {
            if (!longList3.Contains(longList2[index]))
            {
              try
              {
                uSession.GetObject(longList2[index])?.Delete(0L);
              }
              catch (Exception ex)
              {
                this.plugin.appManager.AddWarningMessage($"Ошибка при удалении таблицы {longList2[index]} : {Helper.GetExceptionMessage(ex)}");
                this.plugin.appManager.AddExceptionToLog(ex);
              }
            }
          }
          this.PumpCheckPoint(curProcessStr, progress);
        }
        this._nextFolderKey = customService.GenerateNextClassifierKey((object) uSession, ImbaseIDHelper.ObjTypeIdImCtl, str3, ImbaseIDHelper.ObjTypeIdImFolder);
      }
      cacheData.AddValue(ImportingCategory.ImbaseCatalogsCreated, (object) tableRec.Key, num2, str3);
    }
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(ImbaseIDHelper.AttrIdClassifierKey, RelationalOperators.StartString, (object) str3, LogicalOperators.AND, 0, false)
    }, new List<ColumnDescriptor>()
    {
      new ColumnDescriptor((object) -2, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ImbaseIDHelper.AttrIdImCode, SortOrders.NONE, 0),
      new ColumnDescriptor((object) ImbaseIDHelper.AttrIdName, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -3, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -7, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -12, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -13, SortOrders.DESC, 1)
    }.ToArray());
    DataTable dataTable = uSession.GetObjectCollection(ImbaseIDHelper.ObjTypeIdImFolder).Select(paramSet);
    Dictionary<int, PumpImbaseCatalogs.Folder> dictionary1 = new Dictionary<int, PumpImbaseCatalogs.Folder>(dataTable.Rows.Count);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      if (CompareValuesHelper.NormalizedValue(dataTable.Rows[index][1]) != null)
      {
        int int32 = Convert.ToInt32(dataTable.Rows[index][1]);
        if (!dictionary1.ContainsKey(int32))
          dictionary1.Add(int32, new PumpImbaseCatalogs.Folder(Convert.ToString(dataTable.Rows[index][2]), Convert.ToInt64(dataTable.Rows[index][0]), Convert.ToInt64(dataTable.Rows[index][3]), Convert.ToInt32(dataTable.Rows[index][4]), new Guid(Convert.ToString(dataTable.Rows[index][5]))));
      }
    }
    IDbCommand command = this.plugin.idb2.DbConnection.CreateCommand();
    command.CommandText = "SELECT * FROM " + tableRec.TableName.ToUpper();
    IDataReader idr = command.ExecuteReader(CommandBehavior.Default);
    try
    {
      ImCatalogItemFactory catalogItemFactory = new ImCatalogItemFactory(tableRec.TableName, idr, this.plugin.appManager);
      IImportedObjectList iol = this.plugin.Idw.CreateImportedObjectList();
      iol.NewObjectsOnlyInList = false;
      List<ImCatalogItem> importedFolders = new List<ImCatalogItem>();
      iol.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
      {
        for (int index = 0; index < iol.Items.Count; ++index)
        {
          ImportingObject importingObject = iol.Items[index];
          if (importingObject == null)
          {
            this.plugin.appManager.AddWarningMessage($"Папка F_KEY = \"{importedFolders[index].RecKey}\" каталога \"{tableRec.Description}\" не закачана !");
          }
          else
          {
            ImCatalogItem imCatalogItem = importedFolders[index];
            imCatalogItem.ObjectId = importingObject.Object.Object_id;
            long catalogCacheKey = this.GetCatalogCacheKey(tableRec.Key, imCatalogItem.RecLevel);
            cacheData.AddValue(ImportingCategory.ImbaseFolders, (object) catalogCacheKey, imCatalogItem.ObjectId);
            cacheData.AddValue(ImportingCategory.ImbaseFoldersGuids, (object) catalogCacheKey, imCatalogItem.ObjectId, imCatalogItem.Guid.ToString());
            cacheData.AddValue(ImportingCategory.ImbaseFolderKeyToLevel, (object) this.GetCatalogCacheKey(tableRec.Key, imCatalogItem.RecKey), catalogCacheKey);
            cacheData.AddValue(ImportingCategory.ImbaseFoldersClassificators, (object) catalogCacheKey, imCatalogItem.ObjectId, imCatalogItem.ClassifierKey);
            List<int> intList;
            if (!foldersAttributes.TryGetValue(importingObject.Object.Object_id, out intList))
            {
              intList = new List<int>();
              foldersAttributes.Add(importingObject.Object.Object_id, intList);
            }
            foreach (AttributeRecord attribute in iol.Items[index].Attributes)
            {
              if (!intList.Contains(attribute.AttributeId))
                intList.Add(attribute.AttributeId);
            }
          }
        }
        importedFolders.Clear();
      });
      Dictionary<int, GroupAttribute> dictionary2 = (Dictionary<int, GroupAttribute>) null;
      if (flag2)
      {
        dictionary2 = new Dictionary<int, GroupAttribute>();
        DictionaryValue dictionaryValue2 = cacheData.GetValue(ImportingCategory.ImbaseGroupsAttributes, (object) tableRec.Key);
        if (dictionaryValue2 != null && dictionaryValue2.Tag is ImbaseGroupAttributes tag && tag.Attributes != null)
        {
          foreach (GroupAttribute attribute in tag.Attributes)
            dictionary2.Add(attribute.Key, attribute);
        }
      }
      while (idr.Read())
      {
        ImCatalogItem imCatalogItem = (ImCatalogItem) catalogItemFactory.NewItem(idr);
        long catalogCacheKey = this.GetCatalogCacheKey(tableRec.Key, imCatalogItem.RecLevel);
        if (cacheData.GetNewKey(ImportingCategory.ImbaseFolders, (object) catalogCacheKey) == 0L)
        {
          bool flag4 = false;
          PumpImbaseCatalogs.Folder folder = (PumpImbaseCatalogs.Folder) null;
          if (!dictionary1.TryGetValue(imCatalogItem.RecKey, out folder))
            flag4 = true;
          else if (folder.Name != imCatalogItem.RecNAME)
            flag4 = true;
          if (flag4)
          {
            string caption3 = this.CheckNameSize(imCatalogItem.RecNAME);
            ObjectRecord objectRecord = iol.AddObject(ImbaseIDHelper.ObjTypeIdImFolder, num1, caption3);
            imCatalogItem.Guid = (Guid) objectRecord.ObjectGuid;
            iol.AddAttributeStr(ImbaseIDHelper.AttrIdName, caption3);
            iol.AddAttributeInt(ImbaseIDHelper.AttrIdImCode, (long) imCatalogItem.RecKey);
            iol.AddAttributeStr(ImbaseIDHelper.AttrIdClassifierKey, string.Empty);
            if (imCatalogItem.RecMASK > 0)
            {
              iol.AddAttributeInt(ImbaseIDHelper.AttrFlags, (long) imCatalogItem.RecMASK);
              if ((imCatalogItem.RecMASK & 8) == 8)
                iol.AddAttributeStr(ImbaseIDHelper.AttrVisibility, string.Empty);
            }
            if (imCatalogItem.RecGraphID > 0)
            {
              DictionaryValue dictionaryValue3 = cacheData.GetValue(ImportingCategory.ImbaseBlobs, (object) imCatalogItem.RecGraphID);
              if (dictionaryValue3 != null)
                iol.AddAttributeLink(ImbaseIDHelper.AttrIdPicture, dictionaryValue3.NewObjectID, dictionaryValue3.Caption);
              else
                this.plugin.appManager.AddWarningMessage($"Картинка F_GRAPHID={imCatalogItem.RecGraphID} для папки F_KEY={imCatalogItem.RecKey} не найдена!");
            }
            if (imCatalogItem.RecTextID > 0)
            {
              using (IDataReader dataReader = this.GetDataReader($"SELECT * FROM IM_BLOBS WHERE F_KEY={imCatalogItem.RecTextID}"))
              {
                if (dataReader.Read())
                  ComentTextAttribute.Create(iol, dataReader, this.plugin.Idw.AppManager);
              }
            }
            if (flag2)
            {
              Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
              using (IDataReader dataReader = this.GetDataReader($"SELECT F_PARCODE, F_SORT FROM TC_PASPSORT WHERE F_STCODE = {imCatalogItem.RecLevel}"))
              {
                while (dataReader.Read())
                {
                  int int32 = this.getInt32(dataReader, 0);
                  if (!dictionary3.ContainsKey(int32))
                    dictionary3.Add(int32, this.getInt32(dataReader, 1));
                }
              }
              foreach (KeyValuePair<int, int> keyValuePair in dictionary3)
              {
                object obj = (object) null;
                GroupAttribute groupAttribute = (GroupAttribute) null;
                if (dictionary2.TryGetValue(-1 * keyValuePair.Key, out groupAttribute))
                {
                  switch (cacheData.GetNewKey(ImportingCategory.OborudFieldTypes, (object) (-1 * keyValuePair.Key)))
                  {
                    case 0:
                      continue;
                    case 1:
                      using (IDataReader dataReader = this.GetDataReader($"SELECT F_VALUE FROM TC_OBSTRING WHERE F_STCODE = {imCatalogItem.RecLevel} AND F_PARCODE = {keyValuePair.Key}"))
                      {
                        while (dataReader.Read())
                          obj = (object) this.getString(dataReader, 0);
                        break;
                      }
                    case 2:
                      using (IDataReader dataReader = this.GetDataReader($"SELECT F_VALUE FROM TC_OBINT WHERE F_STCODE = {imCatalogItem.RecLevel} AND F_PARCODE = {keyValuePair.Key}"))
                      {
                        while (dataReader.Read())
                          obj = (object) this.getInt32(dataReader, 0);
                        break;
                      }
                    case 3:
                      using (IDataReader dataReader = this.GetDataReader($"SELECT F_VALUE FROM TC_OBFLOAT WHERE F_STCODE = {imCatalogItem.RecLevel} AND F_PARCODE = {keyValuePair.Key}"))
                      {
                        while (dataReader.Read())
                          obj = (object) this.getDouble(dataReader, 0);
                        break;
                      }
                    case 6:
                      using (IDataReader dataReader = this.GetDataReader($"SELECT F_VALUE FROM TC_OBARRAY WHERE F_STCODE = {imCatalogItem.RecLevel} AND F_PARCODE = {keyValuePair.Key} ORDER BY F_N"))
                      {
                        List<double> doubleList = new List<double>();
                        while (dataReader.Read())
                          doubleList.Add(this.getDouble(dataReader, 0));
                        obj = (object) doubleList.ToArray();
                        break;
                      }
                  }
                  IAttributeTypeItem byGuid = this.plugin.Imdi.AttributeTypes.GetByGuid(groupAttribute.AttrGuid);
                  if (byGuid != null)
                    this.AddAtribute(byGuid, iol, cacheData, groupAttribute, obj, $"{imCatalogItem.RecNAME}({imCatalogItem.RecLevel})");
                }
              }
            }
            AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), iol);
          }
          else
          {
            if (service1.ImportedObjects.GetInfo(folder.ObjectID) == null)
              service1.ImportedObjects.AddValue(folder.ObjectID, folder.ID, folder.ObjectType, folder.ObjectGuid, Guid.Empty);
            iol.UseObject(folder.ObjectID);
            imCatalogItem.Guid = folder.ObjectGuid;
            longList1.Add(folder.ObjectID);
          }
          importedFolders.Add(imCatalogItem);
          items.Add(imCatalogItem.RecLevel, imCatalogItem);
        }
      }
      iol.Import();
    }
    finally
    {
      idr.Close();
    }
    List<int> sortedArray = new List<int>(items.Count);
    this.AddChildKeys(0, items, sortedArray);
    if (sortedArray.Count > 0)
      sortedArray.RemoveAt(0);
    string str6 = "";
    for (int index = 0; index < sortedArray.Count; ++index)
    {
      ImCatalogItem imCatalogItem = items[sortedArray[index]];
      ImCatalogItem ownerFolder = (ImCatalogItem) null;
      bool root = imCatalogItem.RecOwner == 0;
      if (root || items.TryGetValue(imCatalogItem.RecOwner, out ownerFolder))
      {
        this.CheckClassifierFolderKey(str3, root, ownerFolder, imCatalogItem);
        if (root)
          str6 = imCatalogItem.ClassifierKey;
      }
    }
    IConfigurationService service2 = ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService;
    List<string> relationKeys = new List<string>(service2.Configuration.PacketSize);
    IImportedRelationList irl = this.plugin.Idw.CreateImportedRelationList();
    irl.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
    {
      for (int index = 0; index < irl.Items.Count; ++index)
      {
        string[] strArray = relationKeys[index].Split('|');
        if (irl.Items[index] == null)
          this.plugin.appManager.AddWarningMessage($"Связь между папками {strArray[0]} и {strArray[1]} не закачана !");
        else
          cacheData.AddValue(ImportingCategory.ImbaseFolderRelations, (object) relationKeys[index], irl.Items[index].Relation.PrjLinkId);
      }
      relationKeys.Clear();
    });
    IImportedObjectList importedObjectList1 = this.plugin.Idw.CreateImportedObjectList();
    foreach (ImCatalogItem imCatalogItem in items.Values)
    {
      long projId;
      if (imCatalogItem.RecOwner == 0)
        projId = num2;
      else if (items.ContainsKey(imCatalogItem.RecOwner))
        projId = items[imCatalogItem.RecOwner].ObjectId;
      else
        continue;
      string oldKey = $"{projId}|{imCatalogItem.ObjectId}";
      if (cacheData.GetNewKey(ImportingCategory.ImbaseFolderRelations, (object) oldKey) == 0L)
      {
        if (!longList1.Contains(imCatalogItem.ObjectId))
        {
          irl.AddRelation(projId, imCatalogItem.ObjectId, ImbaseIDHelper.RelTypeIDImSorted);
          irl.AddAttributeInt(ImbaseIDHelper.AttrIdImSort, (long) (imCatalogItem.RecSORT * 100));
          relationKeys.Add(oldKey);
        }
        importedObjectList1.UseObject(imCatalogItem.ObjectId);
        importedObjectList1.AddAttributeStr(ImbaseIDHelper.AttrIdClassifierKey, imCatalogItem.ClassifierKey).IsNew = false;
      }
    }
    importedObjectList1.Import();
    irl.Import();
    object tag1 = (object) cacheData.GetTag(ImportingCategory.ImbaseGroupsAttributes, (object) tableRec.Key);
    string key1 = string.Empty;
    int num3 = int.MaxValue;
    string key2 = string.Empty;
    imbaseGroupAttributes = (ImbaseGroupAttributes) null;
    if (tag1 != null && tag1 is ImbaseGroupAttributes && tag1 is ImbaseGroupAttributes imbaseGroupAttributes && imbaseGroupAttributes.Attributes != null)
    {
      foreach (GroupAttribute attribute in imbaseGroupAttributes.Attributes)
      {
        if (attribute.DataType == 1 && attribute.DataMode == 1 && key1.Equals(string.Empty))
          key1 = attribute.Field;
        if (num3 > attribute.Sort)
        {
          key2 = attribute.Field;
          num3 = attribute.Sort;
        }
      }
    }
    IDataReader defaultDataReader = this.GetDefaultDataReader(str2);
    if (defaultDataReader != null)
    {
      try
      {
        if (imbaseGroupAttributes != null)
        {
          if (imbaseGroupAttributes.Attributes != null)
          {
            ImCatalogRecItemFactory catalogRecItemFactory = new ImCatalogRecItemFactory(cacheData, str2, defaultDataReader, this.plugin.appManager, (ICollection<GroupAttribute>) imbaseGroupAttributes.Attributes);
            IImportedObjectList iol = this.plugin.Idw.CreateImportedObjectList();
            iol.NewObjectsOnlyInList = true;
            List<PumpImbaseCatalogs.LinkRecord> importedRecords = new List<PumpImbaseCatalogs.LinkRecord>();
            iol.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
            {
              for (int index = 0; index < iol.Items.Count; ++index)
              {
                if (iol.Items[index] != null)
                {
                  importedRecords[index].Record.ObjectID = iol.Items[index].Object.Object_id;
                  cacheData.AddValue(ImportingCategory.ImbaseTableLinks, (object) importedRecords[index].ID, iol.Items[index].Object.Object_id, (ITagImportObject) new TableLinkTag(tableRec.Key, importedRecords[index].Record.RecLevel));
                  cacheData.AddValue(ImportingCategory.ImbaseTableLinksKeyToObjectID, (object) (((long) tableRec.Key << 32 /*0x20*/) + (long) importedRecords[index].Record.RecKey), iol.Items[index].Object.Object_id, iol.Items[index].Object.Caption);
                }
                else
                  this.plugin.appManager.AddWarningMessage($"Объект F_KEY = \"{importedRecords[index].Record.RecKey}\" каталога \"{tableRec.Description}\" не импортирован ! См. серверный лог.");
              }
              importedRecords.Clear();
            });
            while (defaultDataReader.Read())
            {
              bool flag5 = false;
              ImCatalogRecItem record = (ImCatalogRecItem) catalogRecItemFactory.NewItem(defaultDataReader);
              long catalogCacheKey = this.GetCatalogCacheKey(tableRec.Key, record.RecKey);
              long newKey = cacheData.GetNewKey(ImportingCategory.ImbaseTableLinks, (object) catalogCacheKey);
              if (newKey != 0L)
              {
                record.ObjectID = newKey;
                folderRecDict.Add(record.RecKey, record);
              }
              else if (record.RecLevel >= 0)
              {
                bool flag6;
                if (record.IsTableLink || record.IsMixTableLink)
                {
                  object obj1 = (object) null;
                  if (record.FieldsValues.TryGetValue(key1, out obj1))
                  {
                    string oldKey = Convert.ToString(obj1);
                    DictionaryValue dictionaryValue4 = cacheData.GetValue(ImportingCategory.ImbaseTables, (object) oldKey);
                    long newObjectId = dictionaryValue4 != null ? dictionaryValue4.NewObjectID : 0L;
                    string str7 = dictionaryValue4 != null ? this.CheckNameSize(dictionaryValue4.Caption) : string.Empty;
                    if (record.IsTableLink)
                    {
                      iol.AddObject(ImbaseIDHelper.ObjTypeIdImTabLink, num1, str7);
                      iol.AddAttributeLink(ImbaseIDHelper.AttrIdImLinkTable, newObjectId, str7);
                    }
                    else
                      iol.AddObject(ImbaseIDHelper.ObjTypeIdImTabMixData, num1, str7);
                    iol.AddAttribute(ImbaseIDHelper.AttrIdName, AttrValueType.stringVal, (object) str7, 0);
                    iol.AddAttributeStr(ImbaseIDHelper.AttrIdClassifierKey, string.Empty);
                    if (record.Data.ContainsKey(this._attrSPGUID) & flag1)
                    {
                      try
                      {
                        object obj2 = record.Data[this._attrSPGUID];
                        if (CompareValuesHelper.NormalizedValue(obj2) != null)
                        {
                          string str8 = Convert.ToString(obj2).Trim();
                          if (str8.Contains(","))
                            obj2 = (object) str8.Split(',')[0];
                          iol.AddAttributeStr(this._attrCreateType, this._dictionarySp[Convert.ToInt32(obj2)]);
                        }
                      }
                      catch (Exception ex)
                      {
                        this.plugin.appManager.AddWarningMessage($"Ошибка при добавлении для ссылки на таблицу Imbase \"{str7}\" атрибута \"Тип создаваемого объекта\": {ex.Message} ");
                        this.plugin.appManager.AddExceptionToLog(ex);
                      }
                    }
                  }
                  else
                  {
                    this.plugin.appManager.AddWarningMessage($"Не найдено поле {key1} в котором содержится имя таблицы каталога ID = {(object) record.ObjectID}");
                    flag5 = false;
                  }
                  flag6 = true;
                }
                else
                {
                  object obj3 = (object) null;
                  if (record.FieldsValues.TryGetValue(key2, out obj3))
                  {
                    string str9 = this.CheckNameSize(Convert.ToString(obj3));
                    iol.AddObject(ImbaseIDHelper.ObjTypeIdImCtlRec, num1, str9);
                    iol.AddAttributeStr(ImbaseIDHelper.AttrIdClassifierKey, string.Empty);
                    iol.AddAttribute(ImbaseIDHelper.AttrIdName, AttrValueType.stringVal, (object) str9, 0);
                    if (record.Data.ContainsKey(this._attrSPGUID) & flag1)
                    {
                      try
                      {
                        object obj4 = record.Data[this._attrSPGUID];
                        if (CompareValuesHelper.NormalizedValue(obj4) != null)
                          iol.AddAttributeStr(this._attrCreateType, this._dictionarySp[Convert.ToInt32(obj4)].ToString());
                      }
                      catch (Exception ex)
                      {
                        this.plugin.appManager.AddWarningMessage($"Ошибка при добавлении для записи каталога Imbase \"{str9}\" атрибута \"Тип создаваемого объекта\": {ex.Message} ");
                        this.plugin.appManager.AddExceptionToLog(ex);
                      }
                    }
                    flag6 = true;
                  }
                  else
                  {
                    this.plugin.appManager.AddWarningMessage($"Не найдено поле {key2} в котором содержится имя таблицы каталога ID = {(object) record.ObjectID}");
                    flag6 = false;
                  }
                }
                if (flag6)
                {
                  catalogRecItemFactory.AddFieldsToRecord(catalogCacheKey, false, (ImDataTableItem) record, iol, cacheData);
                  iol.AddAttributeInt(ImbaseIDHelper.AttrIdImCode, (long) record.RecKey);
                  AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), iol);
                  folderRecDict.Add(record.RecKey, record);
                  importedRecords.Add(new PumpImbaseCatalogs.LinkRecord(catalogCacheKey, record));
                }
              }
              else
              {
                ImCatalogItem imCatalogItem;
                if (items.TryGetValue(Math.Abs(record.RecLevel), out imCatalogItem) && imCatalogItem.ObjectId != -1L && imCatalogItem.ObjectId != 0L)
                {
                  iol.UseObject(imCatalogItem.ObjectId);
                  List<int> presentAttributes;
                  foldersAttributes.TryGetValue(imCatalogItem.ObjectId, out presentAttributes);
                  catalogRecItemFactory.AddFieldsToRecord(this.GetCatalogCacheKey(tableRec.Key, imCatalogItem.RecLevel), true, (ImDataTableItem) record, iol, cacheData, presentAttributes);
                }
              }
            }
            iol.Import();
          }
        }
      }
      finally
      {
        defaultDataReader.Close();
      }
    }
    switch (str1)
    {
      case "TC_PEREH":
        this.PumpCatalogLinks("TC_PEREH_LINKS", cacheData, ImportingCategory.ImbaseTcPerehLinks, this._attrDopPerehTextID, this._attrMainPerehTextID, folderRecDict, num1);
        break;
      case "TC_COMMENTS":
        this.PumpCatalogLinks("TC_COMMENT_LINKS", cacheData, ImportingCategory.ImbaseTcCommectsLinks, this._attrCommentTextID, 0, folderRecDict, num1);
        break;
    }
    IImportedRelationList irlFolder = this.plugin.Idw.CreateImportedRelationList();
    List<string> importedRelations = new List<string>(service2.Configuration.PacketSize);
    irlFolder.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
    {
      for (int index = 0; index < irlFolder.Items.Count; ++index)
      {
        if (irlFolder.Items[index] != null && irlFolder.Items[index].Relation.PrjLinkId != 0L)
        {
          cacheData.AddValue(ImportingCategory.ImbaseTableLinksInFolders, (object) importedRelations[index], irlFolder.Items[index].Relation.PrjLinkId);
        }
        else
        {
          string[] strArray = importedRelations[index].Split('|');
          this.plugin.appManager.AddWarningMessage($"Связь между папкой Imbase {strArray[0]} и ярлыком на таблицу Imbase {strArray[1]} не импортирована ! См. серверный лог.");
        }
      }
      importedRelations.Clear();
    });
    IImportedObjectList importedObjectList2 = this.plugin.Idw.CreateImportedObjectList();
    foreach (ImCatalogRecItem imCatalogRecItem in folderRecDict.Values)
    {
      int key3 = Math.Abs(imCatalogRecItem.RecLevel);
      if (key3 == 0)
      {
        string oldKey = $"{num2}|{imCatalogRecItem.ObjectID}";
        if (imCatalogRecItem.ObjectID > 0L && cacheData.GetNewKey(ImportingCategory.ImbaseTableLinksInFolders, (object) oldKey) == 0L)
        {
          irlFolder.AddRelation(num2, imCatalogRecItem.ObjectID, ImbaseIDHelper.RelTypeIDImSorted);
          irlFolder.AddAttributeInt(ImbaseIDHelper.AttrIdImSort, (long) imCatalogRecItem.RecSort);
          AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, irlFolder);
          importedRelations.Add(oldKey);
          importedObjectList2.UseObject(imCatalogRecItem.ObjectID);
          str6 = ClassifierKeyValueGenerator.GetNextKeyValue(str6);
          importedObjectList2.AddAttributeStr(ImbaseIDHelper.AttrIdClassifierKey, str3 + str6).IsNew = false;
        }
      }
      else if (items.ContainsKey(key3))
      {
        ImCatalogItem imCatalogItem = items[key3];
        if (imCatalogItem != null)
        {
          string oldKey = $"{imCatalogItem.ObjectId}|{imCatalogRecItem.ObjectID}";
          if (imCatalogRecItem.ObjectID > 0L && cacheData.GetNewKey(ImportingCategory.ImbaseTableLinksInFolders, (object) oldKey) == 0L)
          {
            irlFolder.AddRelation(imCatalogItem.ObjectId, imCatalogRecItem.ObjectID, ImbaseIDHelper.RelTypeIDImSorted);
            irlFolder.AddAttributeInt(ImbaseIDHelper.AttrIdImSort, (long) imCatalogRecItem.RecSort);
            AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, irlFolder);
            importedRelations.Add(oldKey);
            importedObjectList2.UseObject(imCatalogRecItem.ObjectID);
            importedObjectList2.AddAttributeStr(ImbaseIDHelper.AttrIdClassifierKey, imCatalogItem.GetNextChildKey()).IsNew = false;
          }
        }
      }
    }
    importedObjectList2.Import();
    irlFolder.Import();
    cacheData.AddValue(ImportingCategory.ImbaseCatalogs, (object) tableRec.Key, num2, tableRec.Description);
    items.Clear();
    foldersAttributes.Clear();
    folderRecDict.Clear();
    longList1.Clear();
    dictionary1.Clear();
  }

  private void AddAtributeToList(
    IAttributeTypeItem ati,
    IImportedObjectList iol,
    IImportingData cacheData,
    object value,
    int id_val,
    string mu,
    string folderName)
  {
    AttrValueType attrValtype;
    switch ((FieldTypes) ati.AttrValueType)
    {
      case FieldTypes.ftString:
        attrValtype = AttrValueType.stringVal;
        break;
      case FieldTypes.ftInteger:
      case FieldTypes.ftAutoInc:
        attrValtype = AttrValueType.integerVal;
        break;
      case FieldTypes.ftDouble:
      case FieldTypes.ftMeasured:
        attrValtype = AttrValueType.doubleVal;
        break;
      case FieldTypes.ftBoolean:
        attrValtype = AttrValueType.integerVal;
        break;
      default:
        attrValtype = AttrValueType.unknownVal;
        break;
    }
    try
    {
      if (ati.AttrValueType == 13)
      {
        bool flag = false;
        IMeasures service = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
        long newKey = cacheData.GetNewKey(ImportingCategory.ImbaseBindedMeasures, (object) mu);
        if (newKey != 0L)
        {
          IMeasureItem measure = service.GetMeasure(newKey);
          if (measure != null && CompareValuesHelper.NormalizedValue(value) != null)
          {
            string str = Convert.ToString(value).Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            iol.AddAttributeMeasure(ati.ID, Convert.ToDouble(str) * measure.Koef, measure.BaseMeasureId, $"{str} {measure.ShortName}", id_val);
            flag = true;
          }
        }
        if (flag)
          return;
        iol.AddAttributeNull(ati.ID);
      }
      else if (attrValtype == AttrValueType.unknownVal)
        iol.AddAttributeNull(ati.ID);
      else
        iol.AddAttribute(ati.ID, attrValtype, value, id_val);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Значение \"{value}\" для атрибута \"{ati.Name}\" папки \"{folderName}\" не добавлено: {ex.Message}");
      this.plugin.appManager.AddExceptionToLog(ex);
      iol.AddAttributeNull(ati.ID);
    }
  }

  private void AddAtribute(
    IAttributeTypeItem ati,
    IImportedObjectList iol,
    IImportingData cacheData,
    GroupAttribute item,
    object value,
    string folderName)
  {
    if (value is double[])
    {
      if ((value as double[]).Length != 0)
      {
        if (ati.MultiValueMode == MultiValueModes.MultiValues)
        {
          for (int id_val = 0; id_val < (value as double[]).Length; ++id_val)
            this.AddAtributeToList(ati, iol, cacheData, (object) (value as double[])[id_val], id_val, item.Units, folderName);
        }
        else if (ati.MultiValueMode == MultiValueModes.MultiValuesFromList)
        {
          if (ati.GetPossibleValues() == null)
          {
            this.plugin.appManager.AddWarningMessage($"Значения для атрибута id={ati.ID} не добавлено, т.к. отсутсвуют допустимые значения.");
          }
          else
          {
            for (int id_val = 0; id_val < (value as double[]).Length; ++id_val)
              this.AddAtributeToList(ati, iol, cacheData, (object) (value as double[])[id_val], id_val, item.Units, folderName);
          }
        }
        else if (ati.MultiValueMode == MultiValueModes.SingleValue)
        {
          this.AddAtributeToList(ati, iol, cacheData, (object) (value as double[])[0], 0, item.Units, folderName);
        }
        else
        {
          if (ati.MultiValueMode != MultiValueModes.SingleValueFromList)
            return;
          this.AddAtributeToList(ati, iol, cacheData, (object) (value as double[])[0], 0, item.Units, folderName);
        }
      }
      else
        iol.AddAttributeNull(ati.ID);
    }
    else
      this.AddAtributeToList(ati, iol, cacheData, value, 0, item.Units, folderName);
  }

  private void PumpCatalogLinks(
    string tableName,
    IImportingData cacheData,
    ImportingCategory category,
    int textAttrId,
    int fixedTextAttrId,
    Dictionary<int, ImCatalogRecItem> folderRecDict,
    int ownerUser)
  {
    IDataReader dataReader = this.GetDataReader($"SELECT F_KEY, F_LEVEL, F_SORT, F_FLAGS, F_BLOB FROM {tableName}");
    List<string> stringList = new List<string>();
    try
    {
      IImportedObjectList iol = this.plugin.Idw.CreateImportedObjectList();
      List<ImCatalogRecItem> importedRecs = new List<ImCatalogRecItem>();
      iol.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
      {
        for (int index = 0; index < iol.Items.Count; ++index)
        {
          if (iol.Items[index] != null)
          {
            importedRecs[index].ObjectID = iol.Items[index].Object.Object_id;
            cacheData.AddValue(category, (object) importedRecs[index].RecKey, iol.Items[index].Object.Object_id);
          }
          else
            this.plugin.appManager.AddWarningMessage($"Запись F_KEY = \"{importedRecs[index].RecKey}\" каталога \"{tableName}\" не импортирована ! См. серверный лог.");
        }
        importedRecs.Clear();
      });
      try
      {
        char[] chArray = new char[3]{ '\r', '\n', '\t' };
        while (dataReader.Read())
        {
          ImCatalogRecItem imCatalogRecItem = new ImCatalogRecItem();
          imCatalogRecItem.RecKey = this.getInt32(dataReader, 0);
          imCatalogRecItem.RecLevel = this.getInt32(dataReader, 1);
          imCatalogRecItem.RecSort = this.getInt32(dataReader, 2);
          int int32 = this.getInt32(dataReader, 3);
          string str1 = this.getString(dataReader, 4);
          if (str1.Length > 4 && str1[str1.Length - 1] == '\a' && str1[str1.Length - 2] == '\u0005' && str1[str1.Length - 3] == '\u0003' && str1[str1.Length - 4] == '\u0001')
            str1 = str1.Remove(str1.Length - 4, 4);
          if (cacheData.GetNewKey(category, (object) imCatalogRecItem.RecKey) == 0L)
          {
            string str2 = str1.Length > Consts.MaxStringSize ? str1.Substring(0, Consts.MaxStringSize) : str1;
            foreach (char oldChar in chArray)
              str2 = str2.Replace(oldChar, ' ');
            string caption = this.CheckNameSize(str2);
            iol.AddObject(ImbaseIDHelper.ObjTypeIdImCtlRec, ownerUser, caption);
            iol.AddAttributeStr(ImbaseIDHelper.AttrIdName, caption);
            iol.AddAttributeInt(ImbaseIDHelper.AttrIdImCode, (long) imCatalogRecItem.RecKey);
            iol.AddAttributeStr(ImbaseIDHelper.AttrIdClassifierKey, string.Empty);
            if (textAttrId != 0)
            {
              string str3 = Path.Combine(Path.GetTempPath(), $"memo{Guid.NewGuid()}.tmp");
              char[] charArray = str1.TrimEnd().ToCharArray();
              if (charArray.Length == 0)
              {
                iol.AddAttributeNull(textAttrId);
              }
              else
              {
                using (FileStream output = new FileStream(str3, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                  BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
                  try
                  {
                    binaryWriter.Write(charArray, 0, charArray.Length);
                  }
                  finally
                  {
                    binaryWriter.Flush();
                    binaryWriter.Close();
                    stringList.Add(str3);
                  }
                }
                FileInfo fileInfo = new FileInfo(str3);
                if (fileInfo.Exists && fileInfo.Length > 0L)
                  iol.AddAttributeBlob(textAttrId, str3, fileInfo.Length, string.Empty, ArcMethods.NotPacked);
              }
            }
            if (fixedTextAttrId != 0 && int32 > 0)
            {
              string str4 = Path.Combine(Path.GetTempPath(), $"memo{Guid.NewGuid()}.tmp");
              char[] charArray = str1.Substring(0, Math.Min(int32, str1.Length)).TrimEnd().ToCharArray();
              if (charArray.Length == 0)
              {
                iol.AddAttributeNull(fixedTextAttrId);
              }
              else
              {
                using (FileStream output = new FileStream(str4, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                  BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
                  try
                  {
                    binaryWriter.Write(charArray, 0, charArray.Length);
                  }
                  finally
                  {
                    binaryWriter.Flush();
                    binaryWriter.Close();
                    stringList.Add(str4);
                  }
                }
                FileInfo fileInfo = new FileInfo(str4);
                if (fileInfo.Exists && fileInfo.Length > 0L)
                  iol.AddAttributeBlob(fixedTextAttrId, str4, fileInfo.Length, string.Empty, ArcMethods.NotPacked);
              }
            }
            folderRecDict.Add(imCatalogRecItem.RecKey, imCatalogRecItem);
            importedRecs.Add(imCatalogRecItem);
          }
        }
      }
      finally
      {
        dataReader.Close();
      }
      iol.Import();
    }
    finally
    {
      foreach (string str in stringList)
      {
        if (new FileInfo(str).Exists)
          File.Delete(str);
      }
    }
  }

  private long GetCatalogCacheKey(int parentKey, int level)
  {
    return ((long) parentKey << 32 /*0x20*/) + (long) level;
  }

  private void AddChildKeys(
    int parentKey,
    Dictionary<int, ImCatalogItem> items,
    List<int> sortedArray)
  {
    sortedArray.Add(parentKey);
    IDictionaryEnumerator enumerator = (IDictionaryEnumerator) items.GetEnumerator();
    while (enumerator.MoveNext())
    {
      if (((ImCatalogItem) enumerator.Value).RecOwner == parentKey)
        this.AddChildKeys((int) enumerator.Key, items, sortedArray);
    }
  }

  private string CheckNameSize(string value)
  {
    if (string.IsNullOrEmpty(value) || value.Length <= this._attrNameMaxSize)
      return value;
    this.plugin.appManager.AddWarningMessage($"Значение \"{value}\" для атрибута \"Наименование\" было обрезано до {this._attrNameMaxSize} символов");
    return value.Substring(0, this._attrNameMaxSize);
  }

  private void CheckClassifierFolderKey(
    string startKey,
    bool root,
    ImCatalogItem ownerFolder,
    ImCatalogItem item)
  {
    if (!root)
    {
      item.ClassifierKey = ownerFolder.GetNextChildKey();
    }
    else
    {
      this._nextFolderKey = ClassifierKeyValueGenerator.GetNextKeyValue(this._nextFolderKey);
      item.ClassifierKey = startKey + this._nextFolderKey;
    }
  }

  private string createErrorMessage(string errMessage, int index)
  {
    return $"Ошибка при получении значения поля {index}: {errMessage}";
  }

  protected int getInt32(IDataReader dataReader, int index)
  {
    int int32 = 0;
    object obj = (object) null;
    try
    {
      obj = dataReader[index];
      if (DBNull.Value.Equals(obj))
        return int32;
      int32 = Convert.ToInt32(obj);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage(this.createErrorMessage($"Expected Int32 - found {obj?.GetType()} \"{obj}\" ." + ex.Message, index));
    }
    return int32;
  }

  protected double getDouble(IDataReader dataReader, int index)
  {
    double num = 0.0;
    object obj = (object) null;
    try
    {
      obj = dataReader[index];
      if (DBNull.Value.Equals(obj))
        return num;
      num = Convert.ToDouble(obj);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage(this.createErrorMessage($"Expected Double - found {obj?.GetType()} \"{obj}\" ." + ex.Message, index));
    }
    return num;
  }

  protected string getString(IDataReader dataReader, int index)
  {
    string str = "";
    object obj = (object) null;
    try
    {
      obj = dataReader[index];
      if (DBNull.Value.Equals(obj))
        return str;
      str = Convert.ToString(obj);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage(this.createErrorMessage($"Expected String - found {obj?.GetType()} \"{obj}\" ." + ex.Message, index));
    }
    return str;
  }

  protected DateTime getDateTime(IDataReader dataReader, int index)
  {
    DateTime dateTime = DateTime.Now;
    try
    {
      dateTime = dataReader.IsDBNull(index) ? DateTime.Now : dataReader.GetDateTime(index);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage(this.createErrorMessage(ex.Message, index));
    }
    return dateTime;
  }

  private class Folder
  {
    public string Name;
    public long ObjectID;
    public long ID;
    public int ObjectType;
    public Guid ObjectGuid;

    public Folder(string name, long objectID, long id, int objectType, Guid guid)
    {
      this.Name = name;
      this.ObjectID = objectID;
      this.ID = id;
      this.ObjectType = objectType;
      this.ObjectGuid = guid;
    }
  }

  private class LinkRecord
  {
    public long ID;
    public ImCatalogRecItem Record;

    public LinkRecord(long id, ImCatalogRecItem record)
    {
      this.ID = id;
      this.Record = record;
    }
  }
}
