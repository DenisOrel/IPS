// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpArticleAttributes
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Search.ItemFactories;
using Intermech.ImpExp.SearchData;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using ntermech.ImpExp.Interface.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки атрибутов типов изделий Search", "Перекачка данных об атрибутах типов изделий Search")]
[TaskType(PumperType.MetaData)]
internal class PumpArticleAttributes(SearchPlugin plugin) : PumpSearchClass(plugin, "ART_ATTRIBUTES")
{
  private Guid _guid = new Guid("1EC7066B-6A5E-4a8c-A327-C42CF9A33A38");
  internal static Dictionary<int, List<TypeAttributeItem>> articleAttributes;
  private Dictionary<TypeAttributeItem, SettingsAttributeTypeItem> _articleAttributesDict;
  private const string _groupName = "Атрибуты типов изделий";

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    SettingsGroup settingsGroup = new SettingsGroup("Атрибуты типов изделий", SettingsGroupType.ArticleAttributes);
    if (ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) is ISettingsGroupService service1)
      service1.Groups.Add((ISettingsGroup) settingsGroup);
    settingsGroup.ObjectCreated += new ObjectCreatedEventHandler(this._sgGroup_ObjectCreated);
    IAttributeTypeToCreateList service2 = ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings(this.SettingsName);
    Dictionary<int, List<AtricleTypeField>> dictionary1 = new Dictionary<int, List<AtricleTypeField>>();
    Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
    this.ExamCheckPoint("Определение количества записей", 0);
    int tableRecordsCount = this.GetTableRecordsCount(ArticleTypesItemFactory.TableName);
    this.ExamCheckPoint("Получение данных из таблицы " + ArticleTypesItemFactory.TableName, 1);
    IDataReader sequentialDataReader = this.GetSequentialDataReader(ArticleTypesItemFactory.TableName, ArticleTypesItemFactory.TableColumns);
    int index1 = 0;
    string format1 = $"Обработка записи из таблицы {ArticleTypesItemFactory.TableName} ({{0}} из {{1}})";
    try
    {
      ArticleTypesItemFactory typesItemFactory = new ArticleTypesItemFactory(ArticleTypesItemFactory.TableName, sequentialDataReader, this.plugin.Idw.AppManager);
      while (sequentialDataReader.Read())
      {
        ++index1;
        IArticleTypesItem articleTypesItem = typesItemFactory.NewItem(sequentialDataReader, this.plugin.Imdi.NewPumpGuid());
        if (articleTypesItem.SectionId != 99999990)
        {
          dictionary1.Add(articleTypesItem.SectionId, articleTypesItem.CfgData);
          dictionary2.Add(articleTypesItem.SectionId, articleTypesItem.SectName);
          this.ExamCheckPoint(string.Format(format1, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, 2, 40));
        }
      }
    }
    finally
    {
      sequentialDataReader.Close();
    }
    this.ExamCheckPoint("Получение и обработка схем таблиц ", 41);
    int index2 = 0;
    int count1 = dictionary1.Count;
    string format2 = "Обработка метаданных таблицы ({0} из {1})";
    PumpArticleAttributes.articleAttributes = new Dictionary<int, List<TypeAttributeItem>>(dictionary1.Count);
    this._articleAttributesDict = new Dictionary<TypeAttributeItem, SettingsAttributeTypeItem>(dictionary1.Count);
    IDictionaryEnumerator enumerator1 = (IDictionaryEnumerator) dictionary1.GetEnumerator();
    while (enumerator1.MoveNext())
    {
      ++index2;
      int key = (int) enumerator1.Key;
      try
      {
        this.ExamCheckPoint(string.Format(format2, (object) index2, (object) count1), this.CalculatePercent(count1, index2, 42, 80 /*0x50*/));
        sequentialDataReader = this.GetSequentialDataReader($"SECT_{key}");
        List<AtricleTypeField> atricleTypeFieldList = enumerator1.Value as List<AtricleTypeField>;
        DataTable schemaTable = sequentialDataReader.GetSchemaTable();
        List<TypeAttributeItem> typeAttributeItemList = new List<TypeAttributeItem>(schemaTable.Rows.Count);
        foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
        {
          string dbFieldName = Convert.ToString(row["ColumnName"]);
          if (!dbFieldName.Equals("ART_ID"))
          {
            string attributeName = string.Empty;
            bool flag = false;
            for (int index3 = 0; index3 < atricleTypeFieldList.Count; ++index3)
            {
              if (atricleTypeFieldList[index3].Name.Equals(dbFieldName))
              {
                attributeName = atricleTypeFieldList[index3].Caption;
                flag = atricleTypeFieldList[index3].ImbaseObject;
                break;
              }
            }
            if (attributeName == string.Empty)
              attributeName = dbFieldName;
            if (key != 99999916 || !PumpHelper.SkipAttrsForCopies.Contains(dbFieldName.ToLower()))
            {
              if (!flag)
              {
                if (attributeName.ToLower().Equals("материал"))
                {
                  typeAttributeItemList.Add(new TypeAttributeItem(key, dbFieldName, "Материал", FieldTypes.ftObjectLink)
                  {
                    CreateObjTypeGUID = new Guid("cad0081d-306c-11d8-b4e9-00304f19f545")
                  });
                }
                else
                {
                  int size;
                  FieldTypes typeFromSchemaRow = PumpAttributesHelper.GetFieldTypeFromSchemaRow(row, out size);
                  typeAttributeItemList.Add(new TypeAttributeItem(key, dbFieldName, attributeName, typeFromSchemaRow, size));
                }
              }
              else
                typeAttributeItemList.Add(new TypeAttributeItem(key, dbFieldName, attributeName, FieldTypes.ftObjectLink)
                {
                  CreateObjTypeGUID = new Guid("cad0081d-306c-11d8-b4e9-00304f19f545")
                });
            }
          }
        }
        if (typeAttributeItemList.Count > 0)
          PumpArticleAttributes.articleAttributes.Add(key, typeAttributeItemList);
      }
      finally
      {
        sequentialDataReader.Close();
      }
    }
    int index4 = 0;
    int count2 = PumpArticleAttributes.articleAttributes.Count;
    string format3 = "Проверка атрибутов типов изделий ({0} из {1})";
    IDictionaryEnumerator enumerator2 = (IDictionaryEnumerator) PumpArticleAttributes.articleAttributes.GetEnumerator();
    while (enumerator2.MoveNext())
    {
      this.ExamCheckPoint(string.Format(format3, (object) index4, (object) count2), this.CalculatePercent(count2, index4, 81, 99));
      SettingsGroupItem settingsGroupItem = new SettingsGroupItem(dictionary2[(int) enumerator2.Key]);
      foreach (TypeAttributeItem key1 in (List<TypeAttributeItem>) enumerator2.Value)
      {
        SettingsAttributeTypeItem sattrItem = new SettingsAttributeTypeItem(key1.AttributeName, key1.AttributeName, key1.DBFieldName, key1.AttributeType);
        bool flag = false;
        string key2 = this.SetSaveSettingsName((ITypeAttributeItem) key1);
        if (settings != null && settings.ContainsKey(key2))
        {
          SaveSettingsAttribute[] settingsAttributeArray = settings[key2];
          if (settingsAttributeArray != null && settingsAttributeArray.Length != 0)
          {
            foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
            {
              if (settingsAttribute.AttributeName.Equals("GUID"))
              {
                Guid guid = new Guid(settingsAttribute.AttributeValue);
                if (service2.GetByGuid(guid) != null)
                {
                  sattrItem.AttrGuid = guid;
                  key1.GUID = guid;
                  flag = true;
                  break;
                }
                break;
              }
            }
          }
        }
        if (!flag)
        {
          IAttributeTypeToCreate attribute = SearchHelper.FindAttribute(service2, sattrItem, key1.AttributeName, key1.DBFieldName, key1.AttributeType, key1.AttributeSize, key1.GUID, key1.DefaultValue, MultiValueModes.SingleValue);
          if (attribute.FieldType == FieldTypes.ftObjectLink)
            attribute.CreatedObjectType = key1.CreateObjTypeGUID;
          if (settings != null && settings.ContainsKey(key1.AttributeName))
          {
            SaveSettingsAttribute[] settingsAttributeArray = settings[key1.AttributeName];
            if (settingsAttributeArray != null && settingsAttributeArray.Length != 0)
            {
              foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
              {
                if (settingsAttribute.AttributeName.Equals("NEW_NAME"))
                  attribute.Name = settingsAttribute.AttributeValue;
                if (settingsAttribute.AttributeName.Equals("FIELDTYPE"))
                  attribute.FieldType = (FieldTypes) Convert.ToInt32(settingsAttribute.AttributeValue);
                if (settingsAttribute.AttributeName.Equals("SIZE"))
                  attribute.Size = (long) Convert.ToInt32(settingsAttribute.AttributeValue);
                if (attribute.FieldType == FieldTypes.ftObjectLink && settingsAttribute.AttributeName.Equals("OBJTYPE_GUID"))
                  attribute.CreatedObjectType = new Guid(settingsAttribute.AttributeValue);
              }
            }
          }
        }
        this._articleAttributesDict.Add(key1, sattrItem);
        settingsGroupItem.SettingsItems.Add((ISettingsItem) sattrItem);
      }
      settingsGroup.GroupItems.Add((ISettingsGroupItem) settingsGroupItem);
    }
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  private void _sgGroup_ObjectCreated()
  {
    IMetadataInfo service1 = ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    IAttributeTypeToCreateList service2 = ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    ISaveSettings service3 = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    ICache service4 = ServicesManager.GetService(typeof (ICache)) as ICache;
    try
    {
      service4.DeleteCache(ImportingCategory.ArticleAttributes);
      IImportingData cache = service4.GetCache(ImportingCategory.ArticleAttributes);
      Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>(1);
      IDictionaryEnumerator enumerator = (IDictionaryEnumerator) this._articleAttributesDict.GetEnumerator();
      while (enumerator.MoveNext())
      {
        TypeAttributeItem key = (TypeAttributeItem) enumerator.Key;
        SettingsAttributeTypeItem attributeTypeItem = (SettingsAttributeTypeItem) enumerator.Value;
        IAttributeTypeItem byGuid1 = service1.AttributeTypes.GetByGuid(attributeTypeItem.AttrGuid);
        IAttributeTypeToCreate byGuid2 = service2.GetByGuid(attributeTypeItem.AttrGuid);
        if (byGuid1.AttrValueType == 8)
        {
          Guid guid = new Guid("cad0081d-306c-11d8-b4e9-00304f19f545");
          Guid createdObjectType = byGuid2.CreatedObjectType;
          if (byGuid2.CreatedObjectType != Guid.Empty)
            guid = byGuid2.CreatedObjectType;
          else
            this.plugin.appManager.AddWarningMessage($"Для ссылочного атрибута {key.DBFieldName.ToLower()} типа {key.TypeID} не был указан тип создаваемого объекта!");
          cache.AddValue(ImportingCategory.ArticleAttributes, (object) $"{key.TypeID}.{key.DBFieldName.ToLower()}", (long) byGuid1.ID, guid.ToString());
        }
        else
          cache.AddValue(ImportingCategory.ArticleAttributes, (object) $"{key.TypeID}.{key.DBFieldName.ToLower()}", (long) byGuid1.ID);
        List<SaveSettingsAttribute> settingsAttributeList1 = new List<SaveSettingsAttribute>();
        List<SaveSettingsAttribute> settingsAttributeList2 = settingsAttributeList1;
        Guid guid1 = byGuid1.GUID;
        SaveSettingsAttribute settingsAttribute1 = new SaveSettingsAttribute("GUID", guid1.ToString());
        settingsAttributeList2.Add(settingsAttribute1);
        guid1 = key.GUID;
        if (!guid1.Equals(byGuid1.GUID))
        {
          key.GUID = byGuid1.GUID;
        }
        else
        {
          if (byGuid1.Name != key.AttributeName)
            settingsAttributeList1.Add(new SaveSettingsAttribute("NEW_NAME", byGuid1.Name));
          int num;
          if ((FieldTypes) byGuid1.AttrValueType != key.AttributeType)
          {
            List<SaveSettingsAttribute> settingsAttributeList3 = settingsAttributeList1;
            num = byGuid1.AttrValueType;
            SaveSettingsAttribute settingsAttribute2 = new SaveSettingsAttribute("FIELDTYPE", num.ToString());
            settingsAttributeList3.Add(settingsAttribute2);
          }
          if (byGuid1.MaxSize != key.AttributeSize)
          {
            List<SaveSettingsAttribute> settingsAttributeList4 = settingsAttributeList1;
            num = byGuid1.MaxSize;
            SaveSettingsAttribute settingsAttribute3 = new SaveSettingsAttribute("SIZE", num.ToString());
            settingsAttributeList4.Add(settingsAttribute3);
          }
          if (settingsAttributeList1.Count > 0)
          {
            List<SaveSettingsAttribute> settingsAttributeList5 = settingsAttributeList1;
            guid1 = byGuid1.GUID;
            SaveSettingsAttribute settingsAttribute4 = new SaveSettingsAttribute("GUID", guid1.ToString());
            settingsAttributeList5.Add(settingsAttribute4);
          }
          if (byGuid1.AttrValueType == 8)
          {
            List<SaveSettingsAttribute> settingsAttributeList6 = settingsAttributeList1;
            guid1 = byGuid2.CreatedObjectType;
            SaveSettingsAttribute settingsAttribute5 = new SaveSettingsAttribute("OBJTYPE_GUID", guid1.ToString());
            settingsAttributeList6.Add(settingsAttribute5);
          }
        }
        if (settingsAttributeList1.Count > 0)
          settings.Add(this.SetSaveSettingsName((ITypeAttributeItem) key), settingsAttributeList1.ToArray());
      }
      if (settings.Count > 0)
        service3.SetSettings(this.SettingsName, settings);
      else
        service3.ClearSettings(this.SettingsName);
    }
    finally
    {
      service4?.ReleaseCache(ImportingCategory.ArticleAttributes);
      if (this._articleAttributesDict != null)
        this._articleAttributesDict.Clear();
    }
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Загрузка допустимых значений для атрибута \"Уровень доступа\"", 0);
    try
    {
      IDBAttributeType attributeType = (ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo)) as IMetadataInfo).UserSession.GetAttributeType(new Guid("cad00816-306c-11d8-b4e9-00304f19f545"));
      DataTable possibleValues = attributeType.GetPossibleValues();
      bool flag = false;
      IDataReader sequentialDataReader = this.GetSequentialDataReader(AccessItemFactory.TableName, AccessItemFactory.TableColumns);
      if (sequentialDataReader == null)
        return;
      AccessItemFactory accessItemFactory = new AccessItemFactory(sequentialDataReader, this.plugin.Idw.AppManager);
      try
      {
        int count = possibleValues.Rows.Count;
        while (sequentialDataReader.Read())
        {
          Tuple<int, string> tuple = accessItemFactory.NewItem(sequentialDataReader);
          int num = tuple.Item1;
          string str = tuple.Item2;
          if (possibleValues.Select($"F_INTEGER_VALUE = {num}").Length == 0)
          {
            DataRow row = possibleValues.NewRow();
            row["F_INLIST_ID"] = (object) count;
            row["F_INTEGER_VALUE"] = (object) num;
            row["F_DESCRIPTION"] = (object) str;
            possibleValues.Rows.Add(row);
            ++count;
            flag = true;
          }
        }
      }
      finally
      {
        sequentialDataReader.Close();
      }
      if (!flag)
        return;
      attributeType.SetNewPossibleValues(possibleValues);
    }
    finally
    {
      this.PumpCheckPoint("Загрузка допустимых значений для атрибута \"Уровень доступа\" успешно завершена", 100);
    }
  }

  private string SetSaveSettingsName(ITypeAttributeItem item)
  {
    return $"{item.TypeID.ToString()}|{item.AttributeName}";
  }
}
