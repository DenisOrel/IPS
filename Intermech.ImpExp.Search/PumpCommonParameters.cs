// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpCommonParameters
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
using ntermech.ImpExp.Interface.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskType(PumperType.MetaData)]
internal abstract class PumpCommonParameters : PumpSearchClass
{
  protected Dictionary<TypeAttributeItem, SettingsAttributeTypeItem> attributesList;
  protected int objectTypeID;

  protected abstract ImportingCategory CacheCategory { get; }

  protected abstract SettingsGroupType SettingsGroupType { get; }

  protected abstract string ConfigTableName { get; }

  protected abstract string DataTableName { get; }

  protected abstract string IDColumnName { get; }

  protected abstract string SettingsCaption { get; }

  public PumpCommonParameters(SearchPlugin plugin, string settingsName, int objectTypeID)
    : base(plugin, settingsName)
  {
    this.objectTypeID = objectTypeID;
  }

  public override void Exam()
  {
    this.attributesList = new Dictionary<TypeAttributeItem, SettingsAttributeTypeItem>();
    SettingsGroup settingsGroup = new SettingsGroup(this.SettingsCaption, this.SettingsGroupType);
    if (ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) is ISettingsGroupService service1)
      service1.Groups.Add((ISettingsGroup) settingsGroup);
    settingsGroup.ObjectCreated += new ObjectCreatedEventHandler(this.Group_ObjectCreated);
    IAttributeTypeToCreateList service2 = ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings(this.SettingsName);
    Dictionary<string, string> dictionary = this.ReadFieldCaptions(0, 40);
    this.ExamCheckPoint("Получение данных из таблицы " + this.DataTableName, 41);
    IDataReader sequentialDataReader = this.GetSequentialDataReader(this.DataTableName);
    try
    {
      DataTable schemaTable = sequentialDataReader.GetSchemaTable();
      int index = 0;
      string format = $"Обработка записи из таблицы {this.DataTableName}({{0}} из {{1}})";
      foreach (DataRow row in (InternalDataCollectionBase) schemaTable.Rows)
      {
        ++index;
        this.ExamCheckPoint(string.Format(format, (object) index, (object) schemaTable.Rows.Count), this.CalculatePercent(schemaTable.Rows.Count, index, 42, 99));
        string str = Convert.ToString(row["ColumnName"]);
        if (!str.Equals(this.IDColumnName))
        {
          int size;
          FieldTypes typeFromSchemaRow = PumpAttributesHelper.GetFieldTypeFromSchemaRow(row, out size);
          string attributeName;
          if (!dictionary.TryGetValue(str, out attributeName))
            attributeName = str;
          TypeAttributeItem typeAttributeItem = new TypeAttributeItem(this.objectTypeID, str, attributeName, typeFromSchemaRow, size);
          SettingsAttributeTypeItem attributeTypeItem = this.CheckAttribute(service2, settings, typeAttributeItem);
          this.attributesList.Add(typeAttributeItem, attributeTypeItem);
          settingsGroup.GroupItems.Add((ISettingsGroupItem) attributeTypeItem);
        }
      }
    }
    finally
    {
      sequentialDataReader.Close();
    }
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  protected Dictionary<string, string> ReadFieldCaptions(int startPercent, int endPercent)
  {
    this.ExamCheckPoint("Определение количества записей таблицы " + this.ConfigTableName, startPercent);
    int tableRecordsCount = this.GetTableRecordsCount(this.ConfigTableName);
    this.ExamCheckPoint("Получение данных из таблицы " + this.ConfigTableName, startPercent + 1);
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    using (IDataReader sequentialDataReader = this.GetSequentialDataReader(this.ConfigTableName, "P_FIELD, P_LABEL"))
    {
      int index = 0;
      string format = $"Обработка записи из таблицы {this.ConfigTableName} ({{0}} из {{1}})";
      while (sequentialDataReader.Read())
      {
        ++index;
        string key = sequentialDataReader.GetString(0);
        if (!dictionary.ContainsKey(key))
          dictionary.Add(key, sequentialDataReader.GetString(1));
        this.ExamCheckPoint(string.Format(format, (object) index, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index, startPercent + 2, endPercent));
      }
    }
    return dictionary;
  }

  protected SettingsAttributeTypeItem CheckAttribute(
    IAttributeTypeToCreateList attributeTypeToCreateListService,
    Dictionary<string, SaveSettingsAttribute[]> saveSettings,
    TypeAttributeItem attributeItem)
  {
    SettingsAttributeTypeItem sattrItem = new SettingsAttributeTypeItem(attributeItem.AttributeName, attributeItem.AttributeName, attributeItem.DBFieldName, attributeItem.AttributeType);
    bool flag = false;
    if (saveSettings != null && saveSettings.ContainsKey(attributeItem.DBFieldName))
    {
      SaveSettingsAttribute[] saveSetting = saveSettings[attributeItem.DBFieldName];
      if (saveSetting != null && saveSetting.Length != 0)
      {
        foreach (SaveSettingsAttribute settingsAttribute in saveSetting)
        {
          if (settingsAttribute.AttributeName.Equals("GUID"))
          {
            Guid guid = new Guid(settingsAttribute.AttributeValue);
            if (attributeTypeToCreateListService.GetByGuid(guid) != null)
            {
              sattrItem.AttrGuid = guid;
              attributeItem.GUID = guid;
              flag = true;
              break;
            }
            flag = false;
            break;
          }
        }
      }
    }
    if (!flag)
    {
      IAttributeTypeToCreate attribute = SearchHelper.FindAttribute(attributeTypeToCreateListService, sattrItem, attributeItem.AttributeName, attributeItem.DBFieldName, attributeItem.AttributeType, attributeItem.AttributeSize, attributeItem.GUID, attributeItem.DefaultValue, MultiValueModes.SingleValue);
      if (attribute.IsNew && saveSettings != null && saveSettings.ContainsKey(attributeItem.AttributeName))
      {
        SaveSettingsAttribute[] saveSetting = saveSettings[attributeItem.AttributeName];
        if (saveSetting != null && saveSetting.Length != 0)
        {
          foreach (SaveSettingsAttribute settingsAttribute in saveSetting)
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
    return sattrItem;
  }

  private void Group_ObjectCreated()
  {
    IMetadataInfo service1 = ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList));
    ISaveSettings service2 = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    ICache service3 = ServicesManager.GetService(typeof (ICache)) as ICache;
    try
    {
      service3.DeleteCache(this.CacheCategory);
      IImportingData cache = service3.GetCache(this.CacheCategory);
      Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>(1);
      foreach (KeyValuePair<TypeAttributeItem, SettingsAttributeTypeItem> attributes in this.attributesList)
      {
        IAttributeTypeItem byGuid = service1.AttributeTypes.GetByGuid(attributes.Value.AttrGuid);
        if (cache.GetNewKey(this.CacheCategory, (object) attributes.Key.DBFieldName.ToLower()) == 0L)
          cache.AddValue(this.CacheCategory, (object) attributes.Key.DBFieldName.ToLower(), (long) byGuid.ID);
        else
          cache.SetNewKey(this.CacheCategory, (object) attributes.Key.DBFieldName.ToLower(), (long) byGuid.ID);
        List<SaveSettingsAttribute> settingsAttributeList1 = new List<SaveSettingsAttribute>();
        Guid guid;
        if (attributes.Key.GUID != byGuid.GUID)
        {
          List<SaveSettingsAttribute> settingsAttributeList2 = settingsAttributeList1;
          guid = byGuid.GUID;
          SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("GUID", guid.ToString());
          settingsAttributeList2.Add(settingsAttribute);
          attributes.Key.GUID = byGuid.GUID;
        }
        else
        {
          if (byGuid.Name != attributes.Key.AttributeName)
            settingsAttributeList1.Add(new SaveSettingsAttribute("NEW_NAME", byGuid.Name));
          int num;
          if ((FieldTypes) byGuid.AttrValueType != attributes.Key.AttributeType)
          {
            List<SaveSettingsAttribute> settingsAttributeList3 = settingsAttributeList1;
            num = byGuid.AttrValueType;
            SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("FIELDTYPE", num.ToString());
            settingsAttributeList3.Add(settingsAttribute);
          }
          if (byGuid.MaxSize != attributes.Key.AttributeSize)
          {
            List<SaveSettingsAttribute> settingsAttributeList4 = settingsAttributeList1;
            num = byGuid.MaxSize;
            SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("SIZE", num.ToString());
            settingsAttributeList4.Add(settingsAttribute);
          }
          if (settingsAttributeList1.Count > 0)
          {
            List<SaveSettingsAttribute> settingsAttributeList5 = settingsAttributeList1;
            guid = byGuid.GUID;
            SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("GUID", guid.ToString());
            settingsAttributeList5.Add(settingsAttribute);
          }
        }
        if (settingsAttributeList1.Count > 0)
        {
          if (settings.ContainsKey(attributes.Key.DBFieldName))
            settings[attributes.Key.DBFieldName] = settingsAttributeList1.ToArray();
          else
            settings.Add(attributes.Key.DBFieldName, settingsAttributeList1.ToArray());
        }
      }
      if (settings.Count > 0)
        service2.SetSettings(this.SettingsName, settings);
      else
        service2.ClearSettings(this.SettingsName);
    }
    finally
    {
      service3?.ReleaseCache(this.CacheCategory);
      if (this.attributesList != null)
        this.attributesList.Clear();
    }
  }

  public override void Pump()
  {
    ICache service1 = ServicesManager.GetService(typeof (ICache)) as ICache;
    string objectTypeName = MetaDataHelper.GetObjectTypeName(this.objectTypeID);
    this.PumpCheckPoint("Привязка общих параметров к типу объектов " + objectTypeName, 0);
    try
    {
      Dictionary<object, DictionaryValue> category = service1.GetCache(this.CacheCategory).GetCategory();
      if (category != null && category.Count > 0)
      {
        IMetadataInfo service2 = ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
        IDBObjectType objectType = service2.UserSession.GetObjectType(this.objectTypeID);
        if (!objectType.AnyAttributes)
        {
          IDBAttribute4ObjectTypeCollection attributes = objectType.Attributes as IDBAttribute4ObjectTypeCollection;
          int count = category.Count;
          int index = 0;
          string format = $"Привязка {{0}} к типу объектов {objectTypeName} ({{1}} из {{2}})";
          foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category)
          {
            int newObjectId = (int) keyValuePair.Value.NewObjectID;
            this.PumpCheckPoint(string.Format(format, keyValuePair.Key, (object) index, (object) count), this.CalculatePercent(count, index, 99));
            if (attributes.GetAttributeByID(newObjectId, false) == null)
            {
              IDBAttributeType attributeType = service2.UserSession.GetAttributeType(newObjectId);
              attributes.Create(new Attribute4ObjectTypeProperties(newObjectId, this.objectTypeID, InheritModes.Public, RequiredModes.Manual, attributeType.ValidationRule, attributeType.Computed, attributeType.Formula, attributeType.UniqueMode, attributeType.LevelID, attributeType.DefaultValue, attributeType.OptimizationMode, attributeType.IsContent, attributeType.Options, attributeType.Mask, attributeType.MasterAttributeID, attributeType.SourceAttributeID));
            }
          }
        }
      }
      this.PumpCheckPoint($"Привязка общих параметров к типу объектов {objectTypeName} завершена", 100);
    }
    finally
    {
      service1.ReleaseCache(this.CacheCategory);
    }
  }
}
