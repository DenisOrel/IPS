// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpImbaseFields
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.Controls;
using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("Получение информации о полях Imbase", "Перекачка полей Imbase")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpImbaseFields(ImbasePlugin plugin) : PumpImbaseClass(plugin)
{
  internal static Guid guid = new Guid("{58276F14-22A5-4ecd-A92A-A7EBD59BCBC2}");
  private IAttributeTypeToCreateList _attrService;
  private Dictionary<int, IImFieldsItem> _imFieldsDict;

  protected override Guid GUID => PumpImbaseFields.guid;

  private IAttributeTypeToCreateList attrService
  {
    get
    {
      if (this._attrService == null)
        this._attrService = ServicesManager.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
      return this._attrService;
    }
  }

  private long GetPhysicalValueFromUnits(
    string unitsStr,
    IMeasures measures,
    IImportingData cacheData)
  {
    if (unitsStr != string.Empty)
    {
      long newKey = cacheData.GetNewKey((object) unitsStr);
      if (newKey != 0L)
        return measures.GetMeasure(newKey).PhysicalValueID;
    }
    return -1;
  }

  public void UpdateMeasureFields()
  {
    IMeasures service1 = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    ICache service2 = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service2.GetCache(ImportingCategory.ImbaseBindedMeasures);
    try
    {
      List<Guid> guidList1 = new List<Guid>();
      List<Guid> guidList2 = new List<Guid>();
      if (this._imFieldsDict == null)
        return;
      foreach (IImFieldsItem imFieldsItem in this._imFieldsDict.Values)
      {
        IAttributeTypeToCreate byGuid = this.attrService.GetByGuid(imFieldsItem.AttrGuid);
        if (byGuid != null && byGuid.FieldType == FieldTypes.ftMeasured && !guidList2.Contains(imFieldsItem.AttrGuid))
        {
          long physicalValueFromUnits = this.GetPhysicalValueFromUnits(imFieldsItem.Units, service1, cache);
          if (!guidList1.Contains(imFieldsItem.AttrGuid))
          {
            imFieldsItem.Width = physicalValueFromUnits;
            byGuid.Size = physicalValueFromUnits;
            guidList1.Add(imFieldsItem.AttrGuid);
          }
          else if (byGuid.Size == -1L)
          {
            imFieldsItem.Width = physicalValueFromUnits;
            byGuid.Size = physicalValueFromUnits;
          }
          else if (byGuid.Size != physicalValueFromUnits && physicalValueFromUnits != -1L)
          {
            imFieldsItem.Width = -1L;
            byGuid.Size = -1L;
            guidList2.Add(imFieldsItem.AttrGuid);
          }
        }
      }
    }
    finally
    {
      service2.ReleaseCache(ImportingCategory.ImbaseBindedMeasures);
    }
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Определение количества записей", 0);
    int tableRecordsCount1 = this.GetTableRecordsCount(ImFieldsItemFactory.TableName);
    int index1 = 0;
    string empty1 = string.Empty;
    int num1 = tableRecordsCount1 / 100;
    ImbaseGroups instance = ImbaseGroups.instance;
    this.ExamCheckPoint("Получение данных из базы", 1);
    string format1 = string.Empty;
    if (this.plugin.idb.DataBaseType == "IntermechConnection.Interbase")
      format1 = "SELECT * FROM {0} ORDER BY F_LONGNAME DESC, F_SHORTNAME ASC";
    else if (this.plugin.idb.DataBaseType == "IntermechConnection.MsSQL")
      format1 = "SELECT * FROM {0} ORDER BY F_LONGNAME ASC, F_SHORTNAME ASC";
    else if (this.plugin.idb.DataBaseType == "IntermechConnection.Oracle")
      format1 = "SELECT * FROM {0} ORDER BY F_LONGNAME DESC, F_SHORTNAME DESC";
    IDataReader dataReader1 = this.GetDataReader(string.Format(format1, (object) ImFieldsItemFactory.TableName));
    try
    {
      this.ExamCheckPoint("Получение индексов полей", 2);
      ImFieldsItemFactory fieldsItemFactory = new ImFieldsItemFactory(dataReader1, this.plugin.Idw.AppManager);
      this._imFieldsDict = new Dictionary<int, IImFieldsItem>(tableRecordsCount1);
      string format2 = "Получение информации о полях таблиц Imbase ({0} из {1})";
      while (dataReader1.Read())
      {
        ++index1;
        if (index1 % num1 == 1 || index1 == tableRecordsCount1)
          this.ExamCheckPoint(string.Format(format2, (object) index1, (object) tableRecordsCount1), this.CalculatePercent(tableRecordsCount1, index1, 3, 40));
        IImFieldsItem imFieldsItem = fieldsItemFactory.NewItem(dataReader1) as IImFieldsItem;
        if (ImbasePlugin.IsFieldPump(imFieldsItem.Key))
        {
          if (imFieldsItem.DataType == ImDataTypeEx.IEX_SET)
            imFieldsItem.DataType = ImDataTypeEx.IEX_STRING;
          if (imFieldsItem.DataMode == ImDataMode.IDM_IMAGE || imFieldsItem.DataMode == ImDataMode.IDM_TEXT)
            imFieldsItem.DataType = ImDataTypeEx.IEX_REF;
          if (!imFieldsItem.Units.Equals(string.Empty))
            this.plugin.imbaseSettingsMeasures.AddMeasure(imFieldsItem.Units);
          IImTablesItem byKey = instance.TableExistsByKey(imFieldsItem.TableId) ? instance.TableGetByKey(imFieldsItem.TableId) : (IImTablesItem) null;
          if (byKey != null && (ImbasePlugin.IsTableToPump(byKey.TableName) || ImbasePlugin.IsCatalogToPump(byKey.TableName)))
          {
            imFieldsItem.ExistsInBase = byKey.FieldExistInBase(imFieldsItem.Field);
            this._imFieldsDict.Add(imFieldsItem.Key, imFieldsItem);
            byKey.SettingsItems.Add((ISettingsItem) imFieldsItem);
            if (imFieldsItem.ExistsInBase)
            {
              foreach (ITableFieldInfo existingField in (IEnumerable<ITableFieldInfo>) byKey.ExistingFields)
              {
                if (existingField.ColumnName.Equals(imFieldsItem.Field))
                {
                  imFieldsItem.Width = (long) existingField.ColumnSize;
                  break;
                }
              }
            }
            imFieldsItem.LongName.ToUpper().Trim();
            if (imFieldsItem.AttrFieldType == FieldTypes.ftUnknown && imFieldsItem.ExistsInBase)
            {
              ITableFieldInfo fieldInfo = byKey.GetFieldInfo(imFieldsItem.Field);
              if (fieldInfo != null)
              {
                imFieldsItem.AttrFieldType = Helper.GetFieldType(fieldInfo.DataType.FullName, fieldInfo.NumericScale, fieldInfo.IsLong);
                imFieldsItem.DataType = imFieldsItem.AttrFieldType != FieldTypes.ftInteger ? (imFieldsItem.AttrFieldType != FieldTypes.ftDouble ? ImDataTypeEx.IEX_STRING : ImDataTypeEx.IEX_FLOAT) : ImDataTypeEx.IEX_INTEGER;
              }
            }
          }
        }
      }
    }
    finally
    {
      dataReader1.Close();
    }
    Dictionary<int, string> dictionary1 = new Dictionary<int, string>();
    bool flag = false;
    try
    {
      dataReader1 = this.GetDataReader("SELECT F_NAME, F_KEY FROM TC_MEASURE WHERE F_KEY <> 0");
      while (dataReader1.Read())
      {
        string MeasureName = dataReader1.GetString(0);
        dictionary1.Add(ImbaseHelper.ToInt32(dataReader1[1]), MeasureName);
        this.plugin.imbaseSettingsMeasures.AddMeasure(MeasureName);
      }
      flag = true;
    }
    catch
    {
    }
    finally
    {
      dataReader1.Close();
    }
    ICache service1 = ServicesManager.GetService(typeof (ICache)) as ICache;
    service1.DeleteCache(ImportingCategory.OborudFieldTypes);
    IImportingData cache1 = service1.GetCache(ImportingCategory.OborudFieldTypes);
    IImTablesItem byName = instance.TableGetByName("TC_OBORUD");
    if (byName != null)
    {
      int tableRecordsCount2 = this.GetTableRecordsCount("TC_PASPNAME");
      IDataReader dataReader2 = this.GetDataReader($"SELECT F_KEY, F_NAME, F_SHORTNAME, F_TYPE, F_TP{(flag ? (object) ", F_MEASURE" : (object) string.Empty)} FROM TC_PASPNAME ORDER BY F_KEY");
      this.ExamCheckPoint("Получение информации о дополнительных параметрах каталога оборудования", 41);
      int index2 = 0;
      try
      {
        while (dataReader2.Read())
        {
          ++index2;
          this.ExamCheckPoint($"Получение информации о дополнительных параметрах каталога оборудования ({index2} из {tableRecordsCount2})", this.CalculatePercent(tableRecordsCount2, index2, 42, 50));
          int int32_1 = ImbaseHelper.ToInt32(dataReader2[0]);
          string longName = dataReader2.IsDBNull(1) ? string.Empty : dataReader2.GetString(1);
          string shortName = dataReader2.IsDBNull(2) ? string.Empty : dataReader2.GetString(2);
          ImDataTypeEx imDataTypeEx = ImDataTypeEx.IEX_UNKNOWN;
          switch (ImbaseHelper.ToInt32(dataReader2[3]))
          {
            case 0:
              imDataTypeEx = ImDataTypeEx.IEX_STRING;
              break;
            case 1:
              imDataTypeEx = ImDataTypeEx.IEX_FLOAT;
              break;
            case 2:
              imDataTypeEx = ImDataTypeEx.IEX_INTEGER;
              break;
            case 3:
              imDataTypeEx = ImDataTypeEx.IEX_SET;
              break;
          }
          string empty2 = string.Empty;
          if (flag)
          {
            int int32_2 = ImbaseHelper.ToInt32(dataReader2[5]);
            if (int32_2 != 0)
              dictionary1.TryGetValue(int32_2, out empty2);
          }
          ImFieldsItem imFieldsItem = new ImFieldsItem(-1 * int32_1, byName.Key, Convert.ToString(int32_1), longName, shortName, empty2, 0, 0, ImDataMode.IDM_DATA, 0, imDataTypeEx, 0L, ImEnterMode.IEM_SIMPLE, string.Empty);
          cache1.AddValue(ImportingCategory.OborudFieldTypes, (object) imFieldsItem.Key, (long) imDataTypeEx);
          this._imFieldsDict.Add(imFieldsItem.Key, (IImFieldsItem) imFieldsItem);
          byName.SettingsItems.Add((ISettingsItem) imFieldsItem);
        }
      }
      finally
      {
        dataReader2.Close();
        service1.ReleaseCache(ImportingCategory.OborudFieldTypes);
      }
    }
    Dictionary<int, Dictionary<int, string>> dictionary2 = new Dictionary<int, Dictionary<int, string>>();
    foreach (IImFieldsItem imFieldsItem in this._imFieldsDict.Values)
    {
      if (imFieldsItem.DataMode == ImDataMode.IDM_TABLE)
      {
        if (!dictionary2.ContainsKey(imFieldsItem.TableId))
          dictionary2.Add(imFieldsItem.TableId, new Dictionary<int, string>());
        dictionary2[imFieldsItem.TableId].Add(imFieldsItem.Key, imFieldsItem.Field);
      }
    }
    ICache service2 = ServicesManager.GetService(typeof (ICache)) as ICache;
    service2.DeleteCache(ImportingCategory.ImbaseTablesInCatalogs);
    IImportingData cache2 = service2.GetCache(ImportingCategory.ImbaseTablesInCatalogs);
    try
    {
      foreach (KeyValuePair<int, Dictionary<int, string>> keyValuePair in dictionary2)
      {
        if (instance.TableExistsByKey(keyValuePair.Key) && (instance.TableGetByKey(keyValuePair.Key).TableType == ImTablesType.IMTT_CATALOG || instance.TableGetByKey(keyValuePair.Key).TableType == ImTablesType.IMTT_CTLREF || instance.TableGetByKey(keyValuePair.Key).TableType == ImTablesType.IMTT_TECHREF))
        {
          string tableName = instance.TableGetByKey(keyValuePair.Key).TableName;
          string empty3 = string.Empty;
          int num2 = 0;
          foreach (string str in keyValuePair.Value.Values)
          {
            if (!empty3.Equals(string.Empty))
              empty3 += ",";
            empty3 += str;
            ++num2;
          }
          string empty4 = string.Empty;
          IDataReader defaultDataReader = this.GetDefaultDataReader(tableName + "_REC", empty3);
          if (defaultDataReader != null)
          {
            try
            {
              while (defaultDataReader.Read())
              {
                for (int i = 0; i < num2; ++i)
                {
                  string oldKey = defaultDataReader.IsDBNull(i) ? string.Empty : defaultDataReader.GetString(i);
                  if (!oldKey.Equals(string.Empty) && cache2.GetNewKey(ImportingCategory.ImbaseTablesInCatalogs, (object) oldKey) == 0L)
                    cache2.AddValue(ImportingCategory.ImbaseTablesInCatalogs, (object) oldKey, long.MinValue, tableName);
                }
              }
            }
            finally
            {
              defaultDataReader.Close();
            }
          }
        }
      }
    }
    finally
    {
      service2?.ReleaseCache(ImportingCategory.ImbaseTablesInCatalogs);
    }
    string format3 = "Обработка информации о полях таблиц Imbase ({0} из {1})";
    int count = this._imFieldsDict.Count;
    int index3 = 0;
    int num3 = 10;
    Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings("IMBASEFIELD");
    List<ImbaseAttribute> imbaseAttributeList = new List<ImbaseAttribute>(this._imFieldsDict.Values.Count);
    Dictionary<string, ImbaseAttribute> dictionary3 = new Dictionary<string, ImbaseAttribute>(this._imFieldsDict.Values.Count);
    service2.GetCache(ImportingCategory.ImbaseBindedMeasures);
    try
    {
      foreach (IImFieldsItem fieldRec in this._imFieldsDict.Values)
      {
        ++index3;
        if (index3 % num3 == 1 || index3 == count)
          this.ExamCheckPoint(string.Format(format3, (object) index3, (object) count), this.CalculatePercent(count, index3, 51, 90));
        if (fieldRec != null)
        {
          IImTablesItem byKey = instance.TableGetByKey(fieldRec.TableId);
          this.checkField(fieldRec, byKey.TableName, settings);
          MultiValueModes multiValueMode = this.GetMultiValueMode(fieldRec.EnterMode == ImEnterMode.IEM_LIST || fieldRec.EnterMode == ImEnterMode.IEM_LISTONLY, fieldRec.DataType);
          string checkName = ImbaseAttribute.GetCheckName(fieldRec.LongName, fieldRec.ShortName, fieldRec.AttrFieldType, Convert.ToInt32(fieldRec.Width), fieldRec.AttrGuid, fieldRec.PumpPosible, multiValueMode);
          ImbaseAttribute attr = (ImbaseAttribute) null;
          if (dictionary3.TryGetValue(checkName, out attr))
            attr.Keys.Add(fieldRec.Key);
          if (attr == null)
          {
            attr = new ImbaseAttribute(fieldRec.Key, fieldRec.AttrGuid, fieldRec.LongName, fieldRec.ShortName, fieldRec.DataType, fieldRec.AttrFieldType, fieldRec.ExistsInBase, multiValueMode, Convert.ToInt32(fieldRec.Width), fieldRec.Units);
            IAttributeTypeToCreate byGuid = this.attrService.GetByGuid(fieldRec.AttrGuid);
            if (byGuid != null)
              attr.BindingAttribute = new AttributeTypeAttProxy(fieldRec.AttrGuid, byGuid.Name);
            attr.CheckResult = fieldRec.PumpPosible;
            this.plugin.attributesControl.AddAtribute(attr);
            imbaseAttributeList.Add(attr);
            dictionary3.Add(checkName, attr);
          }
          attr.PresentInTables.Add(byKey.Description != string.Empty ? byKey.Description : byKey.TableName);
          attr.TableIDs.Add(new TableInfo(byKey.Key, byKey.TableType));
        }
      }
    }
    finally
    {
      service2.ReleaseCache(ImportingCategory.ImbaseBindedMeasures);
    }
    this.plugin.attributesControl.BindingChange += new AttributeBindingChange(this.attributesControl_BindingChange);
    this.ExamCheckPoint($"Получение данных завершено (обработано {index3} записей)", 100);
    dictionary3.Clear();
  }

  private void attributesControl_BindingChange(object sender, AttributeBindingEventArgs args)
  {
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    List<int> tableIDs = new List<int>(args.TablesKeys.Count);
    foreach (TableInfo tablesKey in args.TablesKeys)
    {
      tableIDs.Add(tablesKey.TableID);
      switch (tablesKey.TableType)
      {
        case ImTablesType.IMTT_CATALOG:
          flag2 = true;
          continue;
        case ImTablesType.IMTT_CTLREF:
          flag3 = true;
          continue;
        case ImTablesType.IMTT_TECHREF:
          flag4 = true;
          continue;
        case ImTablesType.IMTT_TABLE:
          flag1 = true;
          continue;
        default:
          flag1 = false;
          continue;
      }
    }
    IAttributeTypeToCreate byGuid = this.attrService.GetByGuid(args.BindingAttribute);
    if (byGuid.IsNew)
    {
      foreach (IImFieldsItem imFieldsItem in this._imFieldsDict.Values)
      {
        if (args.AttributeKeys.Contains(imFieldsItem.Key))
        {
          int num = imFieldsItem.EnterMode == ImEnterMode.IEM_LIST ? 1 : (imFieldsItem.EnterMode == ImEnterMode.IEM_LISTONLY ? 1 : 0);
          int id = num != 0 ? this.getListId(imFieldsItem.Data) : -1;
          if (num != 0 && id != -1)
            byGuid.AddValueInListId(id, imFieldsItem.Units);
        }
      }
    }
    if (flag1)
      this.FindInGroups(ImbaseGroups.sgTables, tableIDs, args);
    if (flag2)
      this.FindInGroups(ImbaseGroups.sgCatalogs, tableIDs, args);
    if (flag3)
      this.FindInGroups(ImbaseGroups.sgRef, tableIDs, args);
    if (!flag4)
      return;
    this.FindInGroups(ImbaseGroups.sgTechRef, tableIDs, args);
  }

  private void FindInGroups(
    SettingsGroup group,
    List<int> tableIDs,
    AttributeBindingEventArgs args)
  {
    for (int index1 = 0; index1 < group.GroupItems.Count; ++index1)
    {
      if (group.GroupItems[index1] is IImTablesItem groupItem && tableIDs.Contains(groupItem.Key) && ImbaseGroups.sgTables.GroupItems[index1].SettingsItems != null)
      {
        for (int index2 = 0; index2 < group.GroupItems[index1].SettingsItems.Count; ++index2)
        {
          if (group.GroupItems[index1].SettingsItems[index2] is IImFieldsItem settingsItem && args.AttributeKeys.Contains(settingsItem.Key))
          {
            settingsItem.AttrGuid = args.BindingAttribute;
            settingsItem.PumpPosible = args.CheckResult;
          }
        }
      }
    }
  }

  private void checkField(
    IImFieldsItem fieldRec,
    string tableName,
    Dictionary<string, SaveSettingsAttribute[]> saveSettings)
  {
    IAttributeTypeToCreate attributeTypeToCreate = (IAttributeTypeToCreate) null;
    SaveSettingsAttribute[] settingsAttributeArray = (SaveSettingsAttribute[]) null;
    AttributeCheckResult attributeCheckResult = AttributeCheckResult.cresOk;
    Guid g = Guid.Empty;
    try
    {
      if (saveSettings != null && saveSettings.TryGetValue(fieldRec.UniqueKey, out settingsAttributeArray))
      {
        for (int index = 0; index < settingsAttributeArray.Length; ++index)
        {
          if (settingsAttributeArray[index].AttributeName == "GUID")
          {
            attributeTypeToCreate = this.attrService.GetByGuid(new Guid(settingsAttributeArray[index].AttributeValue));
            break;
          }
        }
      }
      FieldTypes fieldTypes = fieldRec.AttrFieldType;
      string longName = fieldRec.LongName;
      string shortName = fieldRec.ShortName;
      if (fieldTypes == FieldTypes.ftString && !fieldRec.ExistsInBase)
        fieldRec.Width = (long) ImbaseHelper.StringCalcFieldSize;
      if (fieldRec.DataType == ImDataTypeEx.IEX_USER)
        attributeTypeToCreate = this.attrService.GetByGuid(new Guid("cad00d1a-306c-11d8-b4e9-00304f19f545"));
      bool lv = fieldRec.EnterMode == ImEnterMode.IEM_LIST || fieldRec.EnterMode == ImEnterMode.IEM_LISTONLY;
      int num = lv ? this.getListId(fieldRec.Data) : -1;
      if (attributeTypeToCreate == null)
      {
        string str1 = ImbaseImpHelper.CheckSpecialNames(longName, fieldTypes);
        attributeTypeToCreate = this.attrService.GetByName(str1);
        if (shortName != string.Empty)
        {
          if (attributeTypeToCreate != null && attributeTypeToCreate.ShortName != shortName)
            attributeTypeToCreate = (IAttributeTypeToCreate) null;
          if (attributeTypeToCreate == null)
            attributeTypeToCreate = this.attrService.GetByName(ImbaseImpHelper.GetDoubleName(str1, shortName, true));
          if (attributeTypeToCreate == null)
          {
            str1 = ImbaseImpHelper.GetDoubleName(str1, shortName, false);
            attributeTypeToCreate = this.attrService.GetByName(str1);
          }
        }
        if (attributeTypeToCreate != null && attributeTypeToCreate.FieldType == FieldTypes.ftSystem)
        {
          fieldRec.LongName += "^";
          str1 = fieldRec.LongName;
          attributeTypeToCreate = this.attrService.GetByName(fieldRec.LongName);
        }
        if (attributeTypeToCreate != null)
        {
          if (!attributeTypeToCreate.Name.Equals(str1) && attributeTypeToCreate.Name.ToLower().Equals(str1.ToLower()) && !str1.All<char>((System.Func<char, bool>) (x => char.IsUpper(x) || !char.IsLetter(x))) && attributeTypeToCreate.Name.IndexOf(' ') > 0 && str1.IndexOf(' ') > 0)
          {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            char ch;
            for (int index = str1.Length - 1; index >= 0; --index)
            {
              stringBuilder.Append(str1[index]);
              if (!flag)
              {
                ch = str1[index];
                if (ch.Equals(' '))
                  flag = true;
                ch = attributeTypeToCreate.Name[index];
                if (!ch.Equals(str1[index]))
                {
                  if (char.IsUpper(str1[index]))
                    stringBuilder.Append('^');
                  else
                    stringBuilder.Append('`');
                }
              }
            }
            str1 = string.Empty;
            for (int index = stringBuilder.Length - 1; index >= 0; --index)
            {
              string str2 = str1;
              ch = stringBuilder[index];
              string str3 = ch.ToString();
              str1 = str2 + str3;
            }
            attributeTypeToCreate = this.attrService.GetByName(str1) ?? this.CreateNewAttribute(fieldRec, fieldTypes, str1, shortName, lv, num);
          }
          if (shortName == string.Empty && attributeTypeToCreate.ShortName != string.Empty)
            this.plugin.appManager.AddWarningMessage($"Поле \"{str1}\" без короткого наименования таблицы \"{tableName}\" было автоматически привязано к атрибуту \"{attributeTypeToCreate.Name}\" с коротким наименованием \"{attributeTypeToCreate.ShortName}\"");
          if (attributeTypeToCreate.IsNew)
          {
            if (lv && num != -1)
              attributeTypeToCreate.AddValueInListId(num, fieldRec.Units);
            if (!lv && fieldRec.DataType == ImDataTypeEx.IEX_SET && attributeTypeToCreate.MultiValueMode == MultiValueModes.SingleValue)
              attributeTypeToCreate.MultiValueMode = MultiValueModes.MultiValues;
          }
        }
        else
          attributeTypeToCreate = this.CreateNewAttribute(fieldRec, fieldTypes, str1, shortName, lv, num);
      }
      if (attributeTypeToCreate.FieldType == FieldTypes.ftString && attributeTypeToCreate.Size < fieldRec.Width)
        attributeTypeToCreate.Size = fieldRec.Width > (long) Consts.MaxStringSize ? (long) Consts.MaxStringSize : fieldRec.Width;
      if (fieldTypes == FieldTypes.ftUnknown)
      {
        fieldRec.AttrFieldType = fieldTypes = FieldTypes.ftString;
        fieldRec.Width = (long) ImbaseHelper.StringCalcFieldSize;
      }
      g = attributeTypeToCreate.GUID;
      if ((fieldTypes == FieldTypes.ftDouble || fieldTypes == FieldTypes.ftInteger) && attributeTypeToCreate.FieldType == FieldTypes.ftMeasured)
        attributeCheckResult = AttributeCheckResult.cresConvert;
      else if (fieldTypes == FieldTypes.ftMeasured && (attributeTypeToCreate.FieldType == FieldTypes.ftInteger || attributeTypeToCreate.FieldType == FieldTypes.ftDouble))
      {
        if (attributeTypeToCreate.IsNew)
        {
          attributeTypeToCreate.FieldType = FieldTypes.ftMeasured;
          foreach (IImFieldsItem imFieldsItem in this._imFieldsDict.Values)
          {
            if (imFieldsItem.AttrGuid.Equals(g) && imFieldsItem.AttrFieldType != FieldTypes.ftMeasured)
              imFieldsItem.PumpPosible = AttributeCheckResult.cresConvert;
          }
        }
        else
          attributeCheckResult = AttributeCheckResult.cresConvert;
      }
      else if ((fieldTypes == FieldTypes.ftString && (attributeTypeToCreate.FieldType == FieldTypes.ftInteger || attributeTypeToCreate.FieldType == FieldTypes.ftDouble || attributeTypeToCreate.FieldType == FieldTypes.ftMeasured) || attributeTypeToCreate.FieldType == FieldTypes.ftMeasured) && (fieldRec.EnterMode == ImEnterMode.IEM_SIMPLE || fieldRec.EnterMode == ImEnterMode.IEM_UNKNOWN))
      {
        IImTablesItem byKey = ImbaseGroups.instance.TableGetByKey(fieldRec.TableId);
        if (byKey != null)
        {
          string empty = string.Empty;
          string tableName1;
          switch (byKey.TableType)
          {
            case ImTablesType.IMTT_CATALOG:
            case ImTablesType.IMTT_CTLREF:
            case ImTablesType.IMTT_TECHREF:
              tableName1 = byKey.TableName + "_REC";
              break;
            case ImTablesType.IMTT_TABLE:
              tableName1 = byKey.TableName;
              break;
            default:
              tableName1 = string.Empty;
              break;
          }
          if (tableName1.Equals(string.Empty))
            return;
          IDataReader dataReader = (IDataReader) null;
          try
          {
            dataReader = this.GetDefaultDataReader(tableName1, fieldRec.Field);
            while (dataReader.Read())
            {
              if (!dataReader.IsDBNull(0))
              {
                string str = dataReader[0].ToString();
                string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                string s = str.Replace(".", decimalSeparator).Replace(",", decimalSeparator);
                double result = double.MinValue;
                if (s != string.Empty && !double.TryParse(s, out result))
                {
                  attributeCheckResult = AttributeCheckResult.cresLost;
                  return;
                }
              }
            }
            attributeCheckResult = AttributeCheckResult.cresConvert;
          }
          catch (Exception ex)
          {
            attributeCheckResult = AttributeCheckResult.cresLost;
            this.plugin.appManager.AddWarningMessage($"Ошибка при чтении значений таблицы {tableName1} : {ex.Message}");
          }
          finally
          {
            dataReader?.Close();
          }
        }
        else
          attributeCheckResult = AttributeCheckResult.cresLost;
      }
      else
        attributeCheckResult = AttributesHelper.CheckTypes(fieldTypes, attributeTypeToCreate.FieldType, fieldRec.Width, attributeTypeToCreate.Size, this.GetMultiValueMode(lv, fieldRec.DataType), attributeTypeToCreate.MultiValueMode);
    }
    finally
    {
      fieldRec.PumpPosible = attributeCheckResult;
      fieldRec.AttrGuid = g;
      if ((fieldRec.Options & AttributeOptions.ImbaseFlag_IMHGen) > AttributeOptions.None && (attributeTypeToCreate.Options & AttributeOptions.ImbaseFlag_IMHGen) == AttributeOptions.None)
        attributeTypeToCreate.Options |= AttributeOptions.ImbaseFlag_IMHGen;
      if ((fieldRec.Options & AttributeOptions.ImbaseFlag_UsedInTables) > AttributeOptions.None && (attributeTypeToCreate.Options & AttributeOptions.ImbaseFlag_UsedInTables) == AttributeOptions.None)
        attributeTypeToCreate.Options |= AttributeOptions.ImbaseFlag_UsedInTables;
    }
  }

  private MultiValueModes GetMultiValueMode(bool lv, ImDataTypeEx dataType)
  {
    return !lv ? (dataType == ImDataTypeEx.IEX_SET ? MultiValueModes.MultiValues : MultiValueModes.SingleValue) : (dataType == ImDataTypeEx.IEX_SET ? MultiValueModes.MultiValuesFromList : MultiValueModes.SingleValueFromList);
  }

  private int getListId(string dataStr)
  {
    string oldValue = "F_OWNER=";
    string[] strArray = dataStr.Split(',');
    string str1 = strArray.Length > 1 ? strArray[1] : string.Empty;
    string str2 = str1.Contains(oldValue) ? str1.Replace(oldValue, string.Empty) : string.Empty;
    return !str2.Equals(string.Empty) ? Convert.ToInt32(str2) : -1;
  }

  private IAttributeTypeToCreate CreateNewAttribute(
    IImFieldsItem fieldRec,
    FieldTypes ftField,
    string nameField,
    string shortNameField,
    bool lv,
    int lvID)
  {
    if (ftField == FieldTypes.ftUnknown)
    {
      ftField = FieldTypes.ftString;
      fieldRec.Width = (long) ImbaseHelper.StringCalcFieldSize;
    }
    MultiValueModes multiValueMode = this.GetMultiValueMode(lv, fieldRec.DataType);
    long size = ftField != FieldTypes.ftString ? fieldRec.Width : (fieldRec.Width > (long) Consts.MaxStringSize ? (long) Consts.MaxStringSize : fieldRec.Width);
    IAttributeTypeToCreate newAttribute = this.attrService.AddItem(true, nameField, shortNameField, string.Empty, ftField, size, this.plugin.Imdi.NewPumpGuid(), long.MaxValue, false, -1, string.Empty, multiValueMode);
    if (fieldRec.DataMode == ImDataMode.IDM_IMAGE && ftField == FieldTypes.ftObjectLink)
    {
      newAttribute.CreatedObjectType = new Guid("cad00140-306c-11d8-b4e9-00304f19f545");
      newAttribute.Size = (long) ImbaseIDHelper.ObjTypeIdImLibImage;
    }
    if (lv && lvID != -1)
      newAttribute.AddValueInListId(lvID, fieldRec.Units);
    return newAttribute;
  }
}
