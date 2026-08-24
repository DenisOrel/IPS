// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseGroups
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal sealed class ImbaseGroups
{
  public static ImbaseGroups instance = (ImbaseGroups) null;
  public static SettingsGroup sgCatalogs = new SettingsGroup("Imbase (Каталоги)", SettingsGroupType.ImbaseCatalogs);
  public static SettingsGroup sgRef = new SettingsGroup("Imbase (Справочники)", SettingsGroupType.ImbaseCatalogs);
  public static SettingsGroup sgTechRef = new SettingsGroup("Imbase (Справочники Техкард)", SettingsGroupType.ImbaseCatalogs);
  public static SettingsGroup sgTables = new SettingsGroup("Imbase (Таблицы)", SettingsGroupType.ImTables);
  private static ISettingsGroupService sgService = (ISettingsGroupService) null;
  internal static Dictionary<int, IImTablesItem> imTablesDict = new Dictionary<int, IImTablesItem>();
  internal static Dictionary<string, IImTablesItem> imTableNamesDict = new Dictionary<string, IImTablesItem>();
  private const string strClass = "КЛАСС";
  private const string strGost = "ГОСТ";
  private const string strRazmer = "РАЗМЕРЫ И ПАРАМЕТРЫ";

  public ICollection<IImTablesItem> AllTables
  {
    get => (ICollection<IImTablesItem>) ImbaseGroups.imTablesDict.Values;
  }

  static ImbaseGroups()
  {
    ImbaseGroups.instance = new ImbaseGroups();
    ImbaseGroups.sgService = ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) as ISettingsGroupService;
    if (ImbaseGroups.sgService != null)
    {
      ImbaseGroups.sgService.Groups.Add((ISettingsGroup) ImbaseGroups.sgCatalogs);
      ImbaseGroups.sgService.Groups.Add((ISettingsGroup) ImbaseGroups.sgRef);
      ImbaseGroups.sgService.Groups.Add((ISettingsGroup) ImbaseGroups.sgTechRef);
      ImbaseGroups.sgService.Groups.Add((ISettingsGroup) ImbaseGroups.sgTables);
      ImbaseGroups.sgTables.Visible = false;
    }
    ImbaseGroups.sgTables.ObjectCreated += new ObjectCreatedEventHandler(ImbaseGroups.instance.sgTables_AttributesTypesCreated);
  }

  public bool TableExistsByKey(int tableKey) => ImbaseGroups.imTablesDict.ContainsKey(tableKey);

  public IImTablesItem TableGetByKey(int tableKey)
  {
    IImTablesItem imTablesItem = (IImTablesItem) null;
    return ImbaseGroups.imTablesDict.TryGetValue(tableKey, out imTablesItem) ? imTablesItem : (IImTablesItem) null;
  }

  public bool TableExistsByName(string tableName)
  {
    return ImbaseGroups.imTableNamesDict.ContainsKey(tableName);
  }

  public IImTablesItem TableGetByName(string tableName)
  {
    IImTablesItem imTablesItem = (IImTablesItem) null;
    return ImbaseGroups.imTableNamesDict.TryGetValue(tableName, out imTablesItem) ? imTablesItem : (IImTablesItem) null;
  }

  public bool TableAdd(int tableKey, IImTablesItem item)
  {
    if (ImbaseGroups.imTablesDict.ContainsKey(tableKey))
      return false;
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    ImbaseGroups.imTablesDict.Add(tableKey, item);
    ImbaseGroups.imTableNamesDict.Add(item.TableName, item);
    switch (item.TableType)
    {
      case ImTablesType.IMTT_CATALOG:
        ImbaseGroups.sgCatalogs.GroupItems.Add((ISettingsGroupItem) item);
        break;
      case ImTablesType.IMTT_CTLREF:
        ImbaseGroups.sgRef.GroupItems.Add((ISettingsGroupItem) item);
        break;
      case ImTablesType.IMTT_TECHREF:
        ImbaseGroups.sgTechRef.GroupItems.Add((ISettingsGroupItem) item);
        break;
      case ImTablesType.IMTT_TABLE:
        ImbaseGroups.sgTables.GroupItems.Add((ISettingsGroupItem) item);
        break;
    }
    return true;
  }

  private void sgTables_AttributesTypesCreated()
  {
    string str1 = "Импортированные из IMBASE";
    string str2 = "Конструкторские";
    IMetadataInfo service1 = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    if (!service1.AttributeGroups.ExistsByName(str1))
      service1.AttributeGroups.Add(str1);
    IAttributeGroupItem byName1 = service1.AttributeGroups.GetByName(str1);
    if (!service1.AttributeGroups.ExistsByName(str2))
      service1.AttributeGroups.Add(str2);
    IAttributeGroupItem byName2 = service1.AttributeGroups.GetByName(str2);
    ISaveSettings service2 = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    service2.ClearSettings("IMBASEFIELD");
    Dictionary<string, SaveSettingsAttribute[]> settings = new Dictionary<string, SaveSettingsAttribute[]>();
    IDataWriter service3 = ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter;
    ICache service4 = ServicesManager.GetService(typeof (ICache)) as ICache;
    service4.DeleteCache(ImportingCategory.ImbaseTablesAttributes, ImportingCategory.ImbaseGroupsAttributes, ImportingCategory.ImbasePVForTables);
    IImportingData cache = service4.GetCache(ImportingCategory.ImbaseTablesAttributes, ImportingCategory.ImbaseGroups, ImportingCategory.ImbaseGroupsAttributes, ImportingCategory.ImbaseBindedMeasures, ImportingCategory.ImbasePVForTables);
    IMeasures service5 = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    try
    {
      IAttributeTypeToCreateList service6 = ServicesManager.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
      Dictionary<Guid, List<int>> dictionary1 = new Dictionary<Guid, List<int>>();
      Dictionary<Guid, List<int>> dictionary2 = new Dictionary<Guid, List<int>>();
      Dictionary<Guid, List<int>> dictionary3 = new Dictionary<Guid, List<int>>();
      foreach (IImTablesItem imTablesItem in ImbaseGroups.imTablesDict.Values)
      {
        List<GroupAttribute> attributes = new List<GroupAttribute>(imTablesItem.SettingsItems.Count);
        TableAttributesPV tag = new TableAttributesPV();
        foreach (IImFieldsItem settingsItem in imTablesItem.SettingsItems)
        {
          IAttributeTypeToCreate byGuid1 = service6.GetByGuid(settingsItem.AttrGuid);
          IAttributeTypeItem byGuid2 = service1.AttributeTypes.GetByGuid(settingsItem.AttrGuid);
          if (byGuid2 != null)
          {
            if (byGuid1.HasValueInList)
            {
              List<int> intList1 = (List<int>) null;
              if (!dictionary3.TryGetValue(byGuid2.GUID, out intList1))
              {
                intList1 = new List<int>();
                dictionary3.Add(byGuid2.GUID, intList1);
              }
              Dictionary<object, string> dictionary4 = new Dictionary<object, string>();
              for (int index = 0; index < byGuid1.ValuesListIds.Count; ++index)
              {
                string str3 = string.Empty;
                if (byGuid2.AttrValueType == 13)
                {
                  string empty = string.Empty;
                  if (!byGuid1.ValuesListMeasureIDs.TryGetValue(byGuid1.ValuesListIds[index], out empty))
                  {
                    bool flag = true;
                    List<int> intList2 = (List<int>) null;
                    if (!dictionary1.TryGetValue(byGuid2.GUID, out intList2))
                      dictionary1.Add(byGuid2.GUID, new List<int>((IEnumerable<int>) new int[1]
                      {
                        byGuid1.ValuesListIds[index]
                      }));
                    else if (intList2.Contains(byGuid1.ValuesListIds[index]))
                      flag = false;
                    else
                      dictionary1[byGuid2.GUID].Add(byGuid1.ValuesListIds[index]);
                    if (flag)
                    {
                      service3.AppManager.AddWarningMessage($"Не удалось добавить допустимые значения из списка \"{byGuid1.ValuesListIds[index]}\" к атрибуту \"{byGuid2.Name}\" т.к. для списка в поле таблицы Imbase не указана единица измерения.");
                      continue;
                    }
                    continue;
                  }
                  long newKey = cache.GetNewKey(ImportingCategory.ImbaseBindedMeasures, (object) empty);
                  if (newKey == 0L)
                  {
                    bool flag = true;
                    List<int> intList3 = (List<int>) null;
                    if (!dictionary2.TryGetValue(byGuid2.GUID, out intList3))
                      dictionary2.Add(byGuid2.GUID, new List<int>((IEnumerable<int>) new int[1]
                      {
                        byGuid1.ValuesListIds[index]
                      }));
                    else if (intList3.Contains(byGuid1.ValuesListIds[index]))
                      flag = false;
                    else
                      dictionary2[byGuid2.GUID].Add(byGuid1.ValuesListIds[index]);
                    if (flag)
                    {
                      service3.AppManager.AddWarningMessage($"Не удалось добавить допустимые значения из списка \"{byGuid1.ValuesListIds[index]}\" к атрибуту \"{byGuid2.Name}\" т.к. для значения \"{empty}\" в кэше не найден соответствующий идентификатор единицы измерения.");
                      continue;
                    }
                    continue;
                  }
                  str3 = " " + service5.GetMeasure(newKey).ShortName;
                }
                if (!intList1.Contains(byGuid1.ValuesListIds[index]))
                {
                  ImLookupDataType dataType = ImLookupDataType.ldtNmd;
                  if (settingsItem.Data.Contains("F_STR"))
                    dataType = ImLookupDataType.ldtStr;
                  else if (settingsItem.Data.Contains("F_DBL"))
                    dataType = ImLookupDataType.ldtDbl;
                  else if (settingsItem.Data.Contains("F_INT"))
                    dataType = ImLookupDataType.ldtInt;
                  foreach (IImLookupItem imLookupItem in ImbasePumpServiceImpl.imPlugin.imbaseLookups.GetListById(byGuid1.ValuesListIds[index]))
                  {
                    object key = ImbasePumpServiceImpl.imPlugin.imbaseLookups.GetLookupValue(imLookupItem, dataType);
                    if (byGuid2.AttrValueType == 13)
                    {
                      try
                      {
                        string str4 = Convert.ToString(key);
                        string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                        string s = str4.Replace(".", decimalSeparator).Replace(",", decimalSeparator);
                        double result = 0.0;
                        if (!double.TryParse(s, out result))
                        {
                          service3.AppManager.AddWarningMessage($"Не удалось привести значение \"{s}\" к double для аттрибута \"{byGuid2.Name}\" ");
                          continue;
                        }
                        key = (object) (Convert.ToString(result, (IFormatProvider) CultureInfo.InvariantCulture) + str3);
                      }
                      catch (Exception ex)
                      {
                        service3.AppManager.AddWarningMessage($"Не удалось добавить допустимое значение \"{key}\" к атрибуту \"{byGuid2.Name}\": {ex.Message}");
                        key = (object) null;
                      }
                    }
                    if (key != null && !dictionary4.ContainsKey(key))
                      dictionary4.Add(key, imLookupItem.Name);
                  }
                  intList1.Add(byGuid1.ValuesListIds[index]);
                }
              }
              if (dictionary4.Count > 0)
              {
                List<object> values = new List<object>();
                int inListID = 0;
                foreach (KeyValuePair<object, string> keyValuePair in dictionary4)
                {
                  if (byGuid2.AttrValueType == 1 && keyValuePair.Value.Length > byGuid2.MaxSize)
                  {
                    service3.AppManager.AddWarningMessage($"Не удалось добавить допустимое значение \"{keyValuePair.Key}\" к атрибуту \"{byGuid2.Name}\". Недопустимая длина значения.");
                  }
                  else
                  {
                    values.Add(keyValuePair.Key);
                    if (!byGuid2.AddPossibleValue(inListID, keyValuePair.Key, keyValuePair.Value))
                      service3.AppManager.AddWarningMessage($"Не удалось добавить допустимое значение \"{keyValuePair.Key}\" к атрибуту \"{byGuid2.Name}\". Ошибка при приведении типов.");
                    else
                      ++inListID;
                  }
                }
                tag.Values.Add(new TableAttributePV(settingsItem.AttrGuid, values));
              }
            }
            if (byGuid1.IsNew)
            {
              service1.AttributeGroups.LinkAttributeTypeToGroup(byGuid2.ID, byGuid2.GUID, byName1.ID);
              if (!byGuid2.ShortName.Equals(string.Empty))
                service1.AttributeGroups.LinkAttributeTypeToGroup(byGuid2.ID, byGuid2.GUID, byName2.ID);
            }
            attributes.Add(new GroupAttribute(settingsItem.Sort, settingsItem.Flags, settingsItem.Width, (int) settingsItem.DataMode, settingsItem.Required, (int) settingsItem.DataType, (int) settingsItem.EnterMode, (int) settingsItem.AttrFieldType, (int) settingsItem.PumpPosible, settingsItem.Key, settingsItem.Field, settingsItem.Units, settingsItem.Data, settingsItem.LongName, settingsItem.ExistsInBase, settingsItem.AttrGuid));
            settings.Add(settingsItem.UniqueKey, new List<SaveSettingsAttribute>()
            {
              new SaveSettingsAttribute("GUID", byGuid2.GUID.ToString())
            }.ToArray());
          }
        }
        if (tag.Values.Count > 0)
          cache.AddValue(ImportingCategory.ImbasePVForTables, (object) imTablesItem.Key, 0L, (ITagImportObject) tag);
        if (attributes.Count > 0)
          cache.AddValue(ImportingCategory.ImbaseGroupsAttributes, (object) imTablesItem.Key, long.MinValue, (ITagImportObject) new ImbaseGroupAttributes(attributes));
      }
      int num1 = 0;
      Dictionary<int, string> dictionary5 = new Dictionary<int, string>();
      int num2 = 13;
      int num3 = 2;
      int num4 = 1;
      int num5 = 3;
      int num6 = 61440 /*0xF000*/;
      OptimizationModes inViewMode = OptimizationModes.Read;
      IPhysicalValues service7 = ServicesManager.GetService(typeof (IPhysicalValues)) as IPhysicalValues;
      ImbaseMeasureDefine imbaseMeasureDefine = new ImbaseMeasureDefine(cache, service5, service7);
      Guid guid1 = Guid.Empty;
      IConfigurationService service8 = ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService;
      if (service8.Configuration.UnknownMeasure != null && service8.Configuration.UnknownMeasure != string.Empty)
        guid1 = imbaseMeasureDefine.GetMeasure(0L, service8.Configuration.UnknownMeasure);
      IDbCommand command = ImbasePumpServiceImpl.imPlugin.idb.DbConnection.CreateCommand();
      foreach (IImTablesItem tabRecTypes in ImbasePumpServiceImpl.imPlugin.imbaseTables.tabRecTypesList)
      {
        command.CommandText = $"SELECT * FROM {tabRecTypes.TableName}";
        IDataReader dataReader = (IDataReader) null;
        try
        {
          dataReader = command.ExecuteReader(CommandBehavior.SchemaOnly);
          DataTable schemaTable = dataReader.GetSchemaTable();
          List<TableAttribute> attributes = new List<TableAttribute>(tabRecTypes.SettingsItems.Count);
          List<Guid> guidList = new List<Guid>(tabRecTypes.SettingsItems.Count);
          List<Guid> fieldNames = new List<Guid>(tabRecTypes.SettingsItems.Count);
          foreach (IImFieldsItem settingsItem1 in tabRecTypes.SettingsItems)
          {
            IAttributeTypeItem byGuid = service1.AttributeTypes.GetByGuid(settingsItem1.AttrGuid);
            fieldNames.Add(settingsItem1.AttrGuid);
            if (!guidList.Contains(settingsItem1.AttrGuid))
            {
              if (ImbasePlugin.IsTableToPump(tabRecTypes.TableName) && byGuid != null)
              {
                string str5 = string.Empty;
                string defVal = string.Empty;
                if (!settingsItem1.Data.Equals(string.Empty) && settingsItem1.DataMode == ImDataMode.IDM_DATA && settingsItem1.EnterMode == ImEnterMode.IEM_SIMPLE && (byGuid.AttrValueType == num3 || byGuid.AttrValueType == num4 || byGuid.AttrValueType == num5 || byGuid.AttrValueType == num2))
                {
                  if (byGuid.AttrValueType == num2)
                  {
                    string data = settingsItem1.Data;
                    string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    string newValue = decimalSeparator;
                    string s = data.Replace(".", newValue).Replace(",", decimalSeparator);
                    double num7 = 0.0;
                    ref double local = ref num7;
                    if (double.TryParse(s, out local))
                      defVal = num7.ToString();
                    else
                      service3.AppManager.AddWarningMessage($"Не удалось добавить значение по-умолчанию \"{settingsItem1.Data}\" к атрибуту \"{byGuid.Name}\" таблицы {tabRecTypes.TableName}. Ошибка приведения к типу double.");
                  }
                  else
                    defVal = byGuid.MaxSize <= 0 || byGuid.MaxSize >= settingsItem1.Data.Length ? settingsItem1.Data : settingsItem1.Data.Substring(0, byGuid.MaxSize);
                }
                if (!settingsItem1.ExistsInBase)
                {
                  str5 = settingsItem1.Data;
                  if (!str5.Equals(string.Empty))
                  {
                    Dictionary<string, IImFieldsItem> fields = new Dictionary<string, IImFieldsItem>();
                    foreach (ISettingsItem settingsItem2 in tabRecTypes.SettingsItems)
                    {
                      IImFieldsItem imFieldsItem = settingsItem2 as IImFieldsItem;
                      fields.Add(imFieldsItem.Field, imFieldsItem);
                    }
                    str5 = new ImbaseFormulaParser(settingsItem1, (IDictionary<string, IImFieldsItem>) fields).Parse(str5);
                  }
                }
                Guid measure = Guid.Empty;
                string empty = string.Empty;
                if (byGuid.AttrValueType == num2)
                {
                  measure = imbaseMeasureDefine.GetMeasure((long) byGuid.MaxSize, settingsItem1.Units);
                  if (measure == Guid.Empty)
                    measure = !(guid1 == Guid.Empty) ? guid1 : throw new Exception($"Единица измерения '{settingsItem1.Units}', указанная в поле {settingsItem1.Field} таблицы F_TABLE_ID={settingsItem1.TableId} в базе назначения не найдена!");
                }
                AttributeOptions maskFlag = (AttributeOptions) (settingsItem1.Flags & num6);
                RequiredModes addMode = RequiredModes.Manual;
                ComputeValueModes computeMode = ComputeValueModes.NotComputableValue;
                if (settingsItem1.EnterMode == ImEnterMode.IEM_ASPARENT && schemaTable.Select($"[ColumnName]={DataSetProcessor.QString(settingsItem1.Field)}").Length != 0)
                  settingsItem1.EnterMode = ImEnterMode.IEM_SIMPLE;
                ImbaseImpHelper.FormingComputedFlags(settingsItem1.EnterMode, str5 != string.Empty, ref addMode, ref computeMode);
                attributes.Add(new TableAttribute(byGuid.GUID, addMode, computeMode, inViewMode, maskFlag, str5, defVal, measure, empty, settingsItem1.EnterMode));
                if (!byGuid.ExistsInBase)
                {
                  if (!dictionary5.ContainsKey(byGuid.ID))
                    dictionary5.Add(byGuid.ID, str5);
                  else if (!dictionary5[byGuid.ID].Equals(string.Empty) && !dictionary5[byGuid.ID].Equals(str5))
                    dictionary5[byGuid.ID] = string.Empty;
                }
              }
              guidList.Add(settingsItem1.AttrGuid);
            }
          }
          TableAttribute tableAttribute = ImbaseImpHelper.CheckNameColumn(fieldNames);
          if (tableAttribute != null)
          {
            Guid guid2 = new Guid("cad00020-306c-11d8-b4e9-00304f19f545");
            if (guidList.Contains(guid2))
              attributes[guidList.IndexOf(guid2)] = tableAttribute;
            else
              attributes.Add(tableAttribute);
          }
          cache.AddValue(ImportingCategory.ImbaseTablesAttributes, (object) tabRecTypes.Key, long.MinValue, (ITagImportObject) new ImbaseTableAttributes(attributes));
          ++num1;
        }
        catch (Exception ex)
        {
          service3.AppManager.AddErrorMessage($"Таблица {tabRecTypes.TableName} импортирована не будет или будет импортирована с ошибками! Ошибка обработки: {ex.Message}");
        }
        finally
        {
          dataReader?.Close();
        }
      }
    }
    finally
    {
      service2.SetSettings("IMBASEFIELD", settings);
      service4?.ReleaseCache(ImportingCategory.ImbaseGroups, ImportingCategory.ImbaseTablesAttributes, ImportingCategory.ImbaseGroupsAttributes, ImportingCategory.ImbaseBindedMeasures, ImportingCategory.ImbasePVForTables);
      ImbasePumpServiceImpl.imPlugin.imbaseLookups.Clear();
      ImbasePumpServiceImpl.imPlugin.imbaseTables.Clear();
    }
  }
}
