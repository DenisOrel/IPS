// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpCompositionAttributes
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Search.ItemFactories;
using Intermech.Interfaces.Client;
using ntermech.ImpExp.Interface.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки атрибутов проектной связи Search", "Перекачка данных об атрибутах проектной связи Search")]
[TaskType(PumperType.MetaData)]
internal class PumpCompositionAttributes(SearchPlugin plugin) : PumpSearchClass(plugin, "PC_ATTRIBUTES")
{
  private Guid _guid = new Guid("43F10359-6A12-4e08-9C4D-B412C28E55BD");
  private Dictionary<TypeAttributeItem, SettingsAttributeTypeItem> _compositionAttributesDict;
  private const string _groupName = "Атрибуты проектной связи";
  private const string _tableName = "PC_PARAMS";
  private const string _tableColumns = "PARAM_ID, P_LABEL, P_FIELD, CFG_DATA, ISINHERITED";

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    SettingsGroup settingsGroup = new SettingsGroup("Атрибуты проектной связи", SettingsGroupType.CompositionAttributes);
    if (ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) is ISettingsGroupService service1)
      service1.Groups.Add((ISettingsGroup) settingsGroup);
    settingsGroup.ObjectCreated += new ObjectCreatedEventHandler(this._sgGroup_ObjectCreated);
    IAttributeTypeToCreateList service2 = ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings(this.SettingsName);
    string tableName = "PC_PARAMS";
    DataTable dataTable = new DataTable();
    IDataReader shemaDataReader = this.GetShemaDataReader(tableName);
    try
    {
      dataTable = shemaDataReader.GetSchemaTable();
    }
    finally
    {
      shemaDataReader.Close();
    }
    this.ExamCheckPoint("Определение количества атрибутов", 1);
    int tableRecordsCount = this.GetTableRecordsCount("PC_PARAMS");
    int index = 0;
    string format = $"Обработка записи из таблицы {CompositionAttributeFactory.TableName} ({{0}} из {{1}})";
    if (tableRecordsCount > 0)
    {
      this._compositionAttributesDict = new Dictionary<TypeAttributeItem, SettingsAttributeTypeItem>(tableRecordsCount);
      IDataReader sequentialDataReader = this.GetSequentialDataReader(CompositionAttributeFactory.TableName, CompositionAttributeFactory.TableColumns);
      try
      {
        CompositionAttributeFactory attributeFactory = new CompositionAttributeFactory(CompositionAttributeFactory.TableName, sequentialDataReader, this.plugin.appManager);
        while (sequentialDataReader.Read())
        {
          ++index;
          ICompositionAttribute compositionAttribute = attributeFactory.NewItem(sequentialDataReader);
          TypeAttributeItem key = new TypeAttributeItem()
          {
            AttributeName = compositionAttribute.Name,
            AttributeSize = compositionAttribute.Size,
            DBFieldName = compositionAttribute.DBField,
            DefaultValue = (object) this.plugin.GetDefaultValue(tableName, compositionAttribute.DBField),
            GUID = this.plugin.Imdi.NewPumpGuid()
          };
          DataRow[] dataRowArray = dataTable.Select($"{"ColumnName"}='{key.DBFieldName}'");
          if (dataRowArray.Length != 0)
          {
            if (compositionAttribute.IsImbaseLink)
            {
              key.AttributeType = FieldTypes.ftObjectLink;
              key.CreateObjTypeGUID = new Guid("cad0081d-306c-11d8-b4e9-00304f19f545");
            }
            else
            {
              int size;
              key.AttributeType = PumpAttributesHelper.GetFieldTypeFromSchemaRow(dataRowArray[0], out size);
              key.AttributeSize = size;
            }
            SettingsAttributeTypeItem sattrItem = new SettingsAttributeTypeItem(key.AttributeName, key.AttributeName, key.DBFieldName, key.AttributeType);
            if (service2 != null)
            {
              bool flag = false;
              IAttributeTypeToCreate attributeTypeToCreate = (IAttributeTypeToCreate) null;
              if (settings != null && settings.ContainsKey(key.DBFieldName))
              {
                SaveSettingsAttribute[] settingsAttributeArray = settings[key.DBFieldName];
                if (settingsAttributeArray != null && settingsAttributeArray.Length != 0)
                {
                  foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
                  {
                    if (settingsAttribute.AttributeName.Equals("GUID"))
                    {
                      Guid guid = new Guid(settingsAttribute.AttributeValue);
                      attributeTypeToCreate = service2.GetByGuid(guid);
                      if (attributeTypeToCreate != null)
                      {
                        sattrItem.AttrGuid = guid;
                        key.GUID = guid;
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
                attributeTypeToCreate = SearchHelper.FindAttribute(service2, sattrItem, key.AttributeName, key.DBFieldName, key.AttributeType, key.AttributeSize, key.GUID, key.DefaultValue, MultiValueModes.SingleValue);
                if (attributeTypeToCreate.FieldType == FieldTypes.ftObjectLink)
                {
                  attributeTypeToCreate.CreatedObjectType = key.CreateObjTypeGUID;
                  sattrItem.Error = new ItemError(ItemErrorType.Warning, "Проверьте атрибут \"Тип создаваемого по ссылке объекта\"");
                }
                if (settings != null && settings.ContainsKey(key.AttributeName))
                {
                  SaveSettingsAttribute[] settingsAttributeArray = settings[key.AttributeName];
                  if (settingsAttributeArray != null && settingsAttributeArray.Length != 0)
                  {
                    foreach (SaveSettingsAttribute settingsAttribute in settingsAttributeArray)
                    {
                      if (settingsAttribute.AttributeName.Equals("NEW_NAME"))
                        attributeTypeToCreate.Name = settingsAttribute.AttributeValue;
                      if (settingsAttribute.AttributeName.Equals("FIELDTYPE"))
                        attributeTypeToCreate.FieldType = (FieldTypes) Convert.ToInt32(settingsAttribute.AttributeValue);
                      if (settingsAttribute.AttributeName.Equals("SIZE"))
                        attributeTypeToCreate.Size = (long) Convert.ToInt32(settingsAttribute.AttributeValue);
                      if (attributeTypeToCreate.FieldType == FieldTypes.ftObjectLink && settingsAttribute.AttributeName.Equals("OBJTYPE_GUID"))
                        attributeTypeToCreate.CreatedObjectType = new Guid(settingsAttribute.AttributeValue);
                    }
                  }
                }
              }
              if (attributeTypeToCreate.FieldType == FieldTypes.ftObjectLink)
              {
                if (!flag)
                  attributeTypeToCreate.CreatedObjectType = key.CreateObjTypeGUID;
                sattrItem.Error = new ItemError(ItemErrorType.Warning, "Проверьте атрибут \"Тип создаваемого по ссылке объекта\"");
              }
            }
            this._compositionAttributesDict.Add(key, sattrItem);
            settingsGroup.GroupItems.Add((ISettingsGroupItem) sattrItem);
          }
          this.ExamCheckPoint(string.Format(format, (object) index, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index, 2, 99));
        }
      }
      finally
      {
        sequentialDataReader.Close();
      }
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
      service4.DeleteCache(ImportingCategory.CompositionAttributes);
      IImportingData cache = service4.GetCache(ImportingCategory.CompositionAttributes);
      Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>(1);
      IDictionaryEnumerator enumerator = (IDictionaryEnumerator) this._compositionAttributesDict.GetEnumerator();
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
            this.plugin.appManager.AddWarningMessage($"Для ссылочного атрибута {key.DBFieldName.ToLower()} проектной связи не был указан тип создаваемого объекта!");
          if (cache.GetNewKey(ImportingCategory.CompositionAttributes, (object) key.DBFieldName.ToLower()) == 0L)
            cache.AddValue(ImportingCategory.CompositionAttributes, (object) key.DBFieldName.ToLower(), (long) byGuid1.ID, guid.ToString());
          else
            cache.SetNewKey(ImportingCategory.CompositionAttributes, (object) key.DBFieldName.ToLower(), (long) byGuid1.ID);
        }
        else if (cache.GetNewKey(ImportingCategory.CompositionAttributes, (object) key.DBFieldName.ToLower()) == 0L)
          cache.AddValue(ImportingCategory.CompositionAttributes, (object) key.DBFieldName.ToLower(), (long) byGuid1.ID);
        else
          cache.SetNewKey(ImportingCategory.CompositionAttributes, (object) key.DBFieldName.ToLower(), (long) byGuid1.ID);
        List<SaveSettingsAttribute> settingsAttributeList1 = new List<SaveSettingsAttribute>();
        Guid guid1;
        if (key.GUID != byGuid1.GUID)
        {
          List<SaveSettingsAttribute> settingsAttributeList2 = settingsAttributeList1;
          guid1 = byGuid1.GUID;
          SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("GUID", guid1.ToString());
          settingsAttributeList2.Add(settingsAttribute);
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
            SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("FIELDTYPE", num.ToString());
            settingsAttributeList3.Add(settingsAttribute);
          }
          if (byGuid1.MaxSize != key.AttributeSize)
          {
            List<SaveSettingsAttribute> settingsAttributeList4 = settingsAttributeList1;
            num = byGuid1.MaxSize;
            SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("SIZE", num.ToString());
            settingsAttributeList4.Add(settingsAttribute);
          }
          if (settingsAttributeList1.Count > 0)
          {
            List<SaveSettingsAttribute> settingsAttributeList5 = settingsAttributeList1;
            guid1 = byGuid1.GUID;
            SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("GUID", guid1.ToString());
            settingsAttributeList5.Add(settingsAttribute);
          }
          if (byGuid1.AttrValueType == 8)
          {
            List<SaveSettingsAttribute> settingsAttributeList6 = settingsAttributeList1;
            guid1 = byGuid2.CreatedObjectType;
            SaveSettingsAttribute settingsAttribute = new SaveSettingsAttribute("OBJTYPE_GUID", guid1.ToString());
            settingsAttributeList6.Add(settingsAttribute);
          }
        }
        if (settingsAttributeList1.Count > 0)
        {
          if (settings.ContainsKey(key.DBFieldName))
            settings[key.DBFieldName] = settingsAttributeList1.ToArray();
          else
            settings.Add(key.DBFieldName, settingsAttributeList1.ToArray());
        }
      }
      if (settings.Count > 0)
        service3.SetSettings(this.SettingsName, settings);
      else
        service3.ClearSettings(this.SettingsName);
    }
    finally
    {
      service4?.ReleaseCache(ImportingCategory.CompositionAttributes);
      if (this._compositionAttributesDict != null)
        this._compositionAttributesDict.Clear();
    }
  }
}
