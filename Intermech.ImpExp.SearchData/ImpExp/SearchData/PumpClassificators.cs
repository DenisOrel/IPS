// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpClassificators
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.SearchData.ItemFactories;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.SelectionService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки классификаторов Search", "Перекачка данных о классификаторах Search")]
internal class PumpClassificators(SearchDataPlugin plugin) : PumpClass((PluginClass) plugin)
{
  protected override Guid GUID => new Guid("D0C490EA-EC57-47ff-A371-0C938BB53457");

  public override void Exam()
  {
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Чтение метаданных", 0);
    IMetadataInfo service1 = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    IObjectTypeItem byGuid1 = service1.ObjectTypes.GetByGuid(new Guid("cad0014e-306c-11d8-b4e9-00304f19f545"));
    IObjectTypeItem byGuid2 = service1.ObjectTypes.GetByGuid(new Guid("cad00150-306c-11d8-b4e9-00304f19f545"));
    IObjectTypeItem byGuid3 = service1.ObjectTypes.GetByGuid(new Guid("cad0014f-306c-11d8-b4e9-00304f19f545"));
    IObjectTypeItem byGuid4 = service1.ObjectTypes.GetByGuid(new Guid("cad00140-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid5 = service1.AttributeTypes.GetByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid6 = service1.AttributeTypes.GetByGuid(new Guid("cad001d7-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid7 = service1.AttributeTypes.GetByGuid(new Guid("cad0014d-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid8 = service1.AttributeTypes.GetByGuid(new Guid("cad00021-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid9 = service1.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid10 = service1.AttributeTypes.GetByGuid(new Guid("cad0013e-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid11 = service1.AttributeTypes.GetByGuid(new Guid("cad00e8f-306c-11d8-b4e9-00304f19f545"));
    IAttributeTypeItem byGuid12 = service1.AttributeTypes.GetByGuid(new Guid("cad0013d-306c-11d8-b4e9-00304f19f545"));
    IRelationTypeItem rellTypeSorted = service1.RelationTypes.GetByGuid(new Guid("cad00151-306c-11d8-b4e9-00304f19f545"));
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    ICache service2 = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cacheData = service2.GetCache(ImportingCategory.Classificators, ImportingCategory.ClassificatorsImages, ImportingCategory.Articles, ImportingCategory.Documents);
    Dictionary<string, IClassificatorItem> classificatorsDict = (Dictionary<string, IClassificatorItem>) null;
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    try
    {
      int tableRecordsCount = this.GetTableRecordsCount(ClassificatorsFactory.TableName);
      this.SetCountPumpRecords(tableRecordsCount);
      classificatorsDict = new Dictionary<string, IClassificatorItem>(tableRecordsCount);
      IDataReader sequentialDataReader1 = this.GetSequentialDataReader(ClassificatorsFactory.TableName, ClassificatorsFactory.TableColumns);
      string strB = string.Empty;
      int index1 = 0;
      try
      {
        string format = $"Обработка записи из таблицы {ClassificatorsFactory.TableName} ({{0}} из {{1}})";
        ClassificatorsFactory classificatorsFactory = new ClassificatorsFactory(sequentialDataReader1, this.plugin.Idw.AppManager);
        while (sequentialDataReader1.Read())
        {
          ++index1;
          this.PumpCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, 2, 20));
          IClassificatorItem classificatorItem = classificatorsFactory.NewItem(sequentialDataReader1);
          DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.Classificators, (object) classificatorItem.FolderKey);
          if (dictionaryValue != null && !string.IsNullOrEmpty(dictionaryValue.Caption))
          {
            dictionary.Add(classificatorItem.FolderKey, dictionaryValue.Caption);
            if (dictionaryValue.Caption.CompareTo(strB) > 0)
              strB = dictionaryValue.Caption;
          }
          if (classificatorItem.Owner < 0)
            classificatorItem.Owner = 0;
          classificatorItem.ObjTypeID = classificatorItem.FolderLev != 0 ? byGuid2.ID : (classificatorItem.Owner == 0 ? byGuid1.ID : byGuid3.ID);
          classificatorsDict.Add(classificatorItem.FolderKey, classificatorItem);
        }
      }
      finally
      {
        sequentialDataReader1.Close();
      }
      this.PumpCheckPoint("Разбитие классификаторов по уровням", 21);
      SortedDictionary<int, List<IClassificatorItem>> sortedDictionary = new SortedDictionary<int, List<IClassificatorItem>>();
      List<IClassificatorItem> classificatorItemList = new List<IClassificatorItem>();
      foreach (IClassificatorItem classificatorItem in classificatorsDict.Values)
      {
        if (!sortedDictionary.ContainsKey(classificatorItem.FolderLev))
          sortedDictionary.Add(classificatorItem.FolderLev, new List<IClassificatorItem>());
        sortedDictionary[classificatorItem.FolderLev].Add(classificatorItem);
        if (classificatorItem.FileBody != null && classificatorItem.FileBody.Length != 0)
          classificatorItemList.Add(classificatorItem);
      }
      int packetSize = (ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize;
      IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
      string format1 = "Импорт изображения ({0} из {1})";
      int index2 = 0;
      List<string> stringList = new List<string>(classificatorItemList.Count);
      List<string> imagesList = new List<string>(packetSize);
      iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
      {
        for (int index3 = 0; index3 < iolIm.Items.Count; ++index3)
        {
          if (iolIm.Items[index3].Object.Object_id != 0L)
            cacheData.AddValue(ImportingCategory.ClassificatorsImages, (object) imagesList[index3], iolIm.Items[index3].Object.Object_id);
          else
            this.plugin.appManager.AddWarningMessage($"Иконка классификатора {imagesList[index3]} не импортирована. См. серверный лог.");
        }
        imagesList.Clear();
      });
      try
      {
        if (classificatorItemList.Count > 0)
          this.PumpCheckPoint("Загрузка библиотечных изображений", 22);
        string format2 = "image{0}.{1}";
        foreach (IClassificatorItem classificatorItem in classificatorItemList)
        {
          ++index2;
          if (cacheData.GetNewKey(ImportingCategory.ClassificatorsImages, (object) classificatorItem.FolderKey) == 0L)
          {
            string fileNote = classificatorItem.FolderName != string.Empty ? classificatorItem.FolderName : Guid.NewGuid().ToString();
            this.PumpCheckPoint(string.Format(format1, (object) index2, (object) classificatorItemList.Count), this.CalculatePercent(classificatorItemList.Count, index2, 23, 40));
            iolIm.AddObject(byGuid4.ID, Convert.ToInt32(classificatorItem.Owner), classificatorItem.FolderName);
            string str = Path.Combine(Path.GetTempPath(), string.Format(format2, (object) index2, (object) classificatorItem.BitmapType));
            stringList.Add(str);
            int fileSize = 0;
            FileStream fileStream = File.OpenWrite(str);
            try
            {
              fileStream.Write(classificatorItem.FileBody, 0, classificatorItem.FileBody.Length);
              fileSize = Convert.ToInt32(fileStream.Length);
            }
            finally
            {
              fileStream.Flush();
              fileStream.Close();
            }
            iolIm.AddAttributeBlob(byGuid12.ID, str, (long) fileSize, fileNote, ArcMethods.ZLibPacked);
            AttributesHelper.AddObligatoryObjectAttributes(userSession, iolIm);
            imagesList.Add(classificatorItem.FolderKey);
          }
        }
        iolIm.Import();
      }
      finally
      {
        foreach (string path in stringList)
          File.Delete(path);
      }
      this.PumpCheckPoint("Чтение включенных в классификаторы объекты", 41);
      Dictionary<string, PumpClassificators.IncludedObjects> included = new Dictionary<string, PumpClassificators.IncludedObjects>();
      IDataReader sequentialDataReader2 = this.GetSequentialDataReader("CLASS_ARTS", "FOLDER_KEY, ART_ID");
      try
      {
        while (sequentialDataReader2.Read())
        {
          string key = sequentialDataReader2.IsDBNull(0) ? string.Empty : sequentialDataReader2.GetString(0);
          int int32 = sequentialDataReader2.IsDBNull(1) ? 0 : Convert.ToInt32(sequentialDataReader2[1]);
          PumpClassificators.IncludedObjects includedObjects = (PumpClassificators.IncludedObjects) null;
          if (!included.TryGetValue(key, out includedObjects))
          {
            includedObjects = new PumpClassificators.IncludedObjects();
            included.Add(key, includedObjects);
          }
          if (!includedObjects.Articles.Contains(int32))
            includedObjects.Articles.Add(int32);
        }
      }
      finally
      {
        sequentialDataReader2.Close();
      }
      IDataReader sequentialDataReader3 = this.GetSequentialDataReader("CLASS_DOCS", "FOLDER_KEY, DOC_ID");
      try
      {
        while (sequentialDataReader3.Read())
        {
          string key = sequentialDataReader3.IsDBNull(0) ? string.Empty : sequentialDataReader3.GetString(0);
          int int32 = sequentialDataReader3.IsDBNull(1) ? 0 : Convert.ToInt32(sequentialDataReader3[1]);
          PumpClassificators.IncludedObjects includedObjects = (PumpClassificators.IncludedObjects) null;
          if (!included.TryGetValue(key, out includedObjects))
          {
            includedObjects = new PumpClassificators.IncludedObjects();
            included.Add(key, includedObjects);
          }
          if (!includedObjects.Documents.Contains(int32))
            includedObjects.Documents.Add(int32);
        }
      }
      finally
      {
        sequentialDataReader3.Close();
      }
      this.PumpCheckPoint("Загрузка классификаторов", 51);
      string format3 = "Импорт классификатора ({0} из {1})";
      IDictionaryEnumerator enumerator = (IDictionaryEnumerator) sortedDictionary.GetEnumerator();
      int index4 = 0;
      string newKey = string.Empty;
      List<PumpClassificators.Keys> packetKeys = new List<PumpClassificators.Keys>();
      while (enumerator.MoveNext())
      {
        List<IClassificatorItem> levelClassif = enumerator.Value as List<IClassificatorItem>;
        if (levelClassif != null && levelClassif.Count > 0)
        {
          IImportedObjectList iol = this.plugin.Idw.CreateImportedObjectList(0);
          iol.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
          {
            try
            {
              IImportedRelationList importedRelationList = this.plugin.Idw.CreateImportedRelationList();
              for (int index5 = 0; index5 < iol.Items.Count; ++index5)
              {
                IClassificatorItem classificatorItem1 = levelClassif[index5];
                if (iol.Items[index5].Object.Object_id != 0L)
                {
                  List<long> longList = new List<long>();
                  PumpClassificators.IncludedObjects includedObjects = (PumpClassificators.IncludedObjects) null;
                  if (included.TryGetValue(packetKeys[index5].OldKey, out includedObjects))
                  {
                    if (includedObjects.Articles.Count > 0)
                    {
                      for (int index6 = 0; index6 < includedObjects.Articles.Count; ++index6)
                      {
                        DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.Articles, (object) includedObjects.Articles[index6]);
                        long newObjectId = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
                        if (newObjectId != 0L)
                        {
                          ArticleTag tag = dictionaryValue.Tag as ArticleTag;
                          try
                          {
                            long objectID;
                            if (!tag.Versions.TryGetValue(tag.VersionID, out objectID))
                              this.plugin.appManager.AddWarningMessage($"Изделие {newObjectId} не включено в классификатор {iol.Items[index5].Object.Object_id}, так как версия {tag.VersionID} не была закачана.");
                            else if (longList.IndexOf(objectID) < 0)
                            {
                              longList.Add(objectID);
                              this.plugin.Idw.IncludeObjectIntoSelection(iol.Items[index5].Object.Object_id, packetKeys[index5].NewKey, objectID, newObjectId);
                            }
                          }
                          catch (Exception ex)
                          {
                            this.plugin.appManager.AddWarningMessage($"Изделие {newObjectId} не включено в классификатор {iol.Items[index5].Object.Object_id} : {ex.Message}");
                          }
                        }
                      }
                    }
                    if (includedObjects.Documents.Count > 0)
                    {
                      for (int index7 = 0; index7 < includedObjects.Documents.Count; ++index7)
                      {
                        DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.Documents, (object) includedObjects.Documents[index7]);
                        long newObjectId = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
                        if (newObjectId != 0L)
                        {
                          DocumentTag tag = dictionaryValue.Tag as DocumentTag;
                          try
                          {
                            long version = tag.Versions[tag.VersionID];
                            if (longList.IndexOf(version) < 0)
                            {
                              longList.Add(version);
                              this.plugin.Idw.IncludeObjectIntoSelection(iol.Items[index5].Object.Object_id, packetKeys[index5].NewKey, version, newObjectId);
                            }
                          }
                          catch (Exception ex)
                          {
                            this.plugin.appManager.AddWarningMessage($"Документ {newObjectId} не включен в классификатор {iol.Items[index5].Object.Object_id}: {ex.Message}");
                          }
                        }
                      }
                    }
                  }
                  classificatorItem1.ObjectID = iol.Items[index5].Object.Object_id;
                  string key = classificatorItem1.FolderKey.Remove(classificatorItem1.FolderKey.Length - 2, 2);
                  if (classificatorItem1.FolderLev > 0)
                  {
                    IClassificatorItem classificatorItem2;
                    if (classificatorsDict.TryGetValue(key, out classificatorItem2))
                    {
                      if (classificatorItem2.ObjectID != 0L)
                      {
                        importedRelationList.AddRelation(classificatorItem2.ObjectID, classificatorItem1.ObjectID, rellTypeSorted.ID);
                        AttributesHelper.AddObligatoryRelationAttributes(this.plugin.Idw, importedRelationList);
                      }
                      else
                        this.plugin.appManager.AddWarningMessage($"Классификатор {classificatorItem2.FolderKey} не импортирован. Восстановление его связей невозможно.");
                    }
                    else
                      this.plugin.appManager.AddWarningMessage($"Родительский классификатор {key} для {classificatorItem1.FolderKey} не найден.");
                  }
                  cacheData.AddValue(ImportingCategory.Classificators, (object) classificatorItem1.FolderKey, classificatorItem1.ObjectID);
                }
                else
                  this.plugin.appManager.AddWarningMessage($"Классификатор {classificatorItem1.FolderKey} не импортирован. См. серверный лог.");
              }
              importedRelationList.Import();
            }
            catch (Exception ex)
            {
              this.plugin.appManager.AddWarningMessage($"Ошибка во время включения объектов в классификаторы: {ex.Message} StackTrace: {ex.StackTrace}");
            }
            finally
            {
              packetKeys.Clear();
            }
          });
          foreach (IClassificatorItem classificatorItem in levelClassif)
          {
            ++index4;
            this.PumpCheckPoint(string.Format(format3, (object) index4, (object) classificatorsDict.Count), this.CalculatePercent(classificatorsDict.Count, index4, 10, 99));
            if (classificatorItem.ObjectID == 0L)
            {
              classificatorItem.ImageObjectID = cacheData.GetNewKey(ImportingCategory.ClassificatorsImages, (object) classificatorItem.FolderName);
              if (classificatorItem.FolderLev == 0)
              {
                strB = ClassifierKeyValueGenerator.GetNextKeyValue(strB);
                dictionary.Add(classificatorItem.FolderKey, strB);
                newKey = strB;
              }
              else
              {
                string empty = string.Empty;
                if (classificatorItem.FolderKey.Length > 2 && dictionary.TryGetValue(classificatorItem.FolderKey.Substring(0, 2), out empty))
                  newKey = empty + classificatorItem.FolderKey.Substring(2);
                else
                  this.plugin.appManager.AddWarningMessage($"Ключ \"{classificatorItem.FolderKey}\" папки классификатора \"{classificatorItem.FolderName}\" не восстановлен.");
              }
              iol.AddObject(classificatorItem.ObjTypeID, classificatorItem.Owner, classificatorItem.FolderName);
              iol.AddAttributeStr(byGuid9.ID, classificatorItem.FolderName);
              iol.AddAttributeStr(byGuid6.ID, $"{byGuid5.GUID}={classificatorItem.Formula}");
              if (classificatorItem.FolderLev == 0)
                iol.AddAttributeInt(byGuid11.ID, 4L);
              iol.AddAttributeStr(byGuid7.ID, newKey);
              iol.AddAttributeStr(byGuid8.ID, classificatorItem.Note);
              if (classificatorItem.FileBody != null && classificatorItem.FileBody.Length != 0 && classificatorItem.ImageObjectID != 0L)
                iol.AddAttributeLink(byGuid10.ID, classificatorItem.ImageObjectID, classificatorItem.FolderName);
              AttributesHelper.AddObligatoryObjectAttributes(userSession, iol);
              packetKeys.Add(new PumpClassificators.Keys(classificatorItem.FolderKey, newKey));
            }
          }
          if (packetKeys.Count > 0)
            iol.Import();
        }
      }
      this.PumpCheckPoint("Загрузка классификаторов успешно завершена", 100);
    }
    finally
    {
      service2?.ReleaseCache(ImportingCategory.Classificators, ImportingCategory.ClassificatorsImages, ImportingCategory.Articles, ImportingCategory.Documents);
      if (classificatorsDict != null)
        classificatorsDict.Clear();
    }
  }

  private class IncludedObjects
  {
    public List<int> Articles;
    public List<int> Documents;

    public IncludedObjects()
    {
      this.Articles = new List<int>();
      this.Documents = new List<int>();
    }
  }

  private class Keys
  {
    public string OldKey;
    public string NewKey;

    public Keys(string oldKey, string newKey)
    {
      this.OldKey = oldKey;
      this.NewKey = newKey;
    }
  }
}
