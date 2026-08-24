// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpDataTableBlobs
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("", "Перекачка данных таблиц Imbase")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpDataTableBlobs(ImbasePlugin plugin) : PumpImbaseClass(plugin)
{
  private string attrSizeAndParamGuid = "cad00211-306c-11d8-b4e9-00304f19f545";

  protected override Guid GUID => new Guid("25C375E2-049E-4BBA-B112-15B5DB14F169");

  public override void Pump()
  {
    this.PumpCheckPoint("Определение количества записей для закачки информации о таблицах IMBASE", 0);
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cacheData = service.GetCache(ImportingCategory.ImbaseGroups, ImportingCategory.ImbaseTablesAttributes, ImportingCategory.ImbaseTableBlobs, ImportingCategory.ImbaseGroupsAttributes, ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseTables, ImportingCategory.ImbaseFolders, ImportingCategory.ImbaseFolderKeyToLevel, ImportingCategory.ImbaseFoldersGuids, ImportingCategory.ImbasePVForTables);
    List<string> deleteFiles = new List<string>();
    try
    {
      List<ImbaseGroup> tablesList = PumpImbaseTablesHelper.GetTablesList(cacheData, ImportingCategory.ImbaseTableBlobs, 0);
      int count = tablesList.Count;
      int index1 = 0;
      string format = "Закачка блобов для таблиц IMBASE ({0} из {1})";
      DataTable tableAttributes = ImbaseImpHelper.GetTableAttributes();
      DataTable tableData = ImbaseImpHelper.GetTableData();
      List<Tuple<string, List<int>>> packetTables = new List<Tuple<string, List<int>>>();
      LinkToFolderDecoder linkDecoder = new LinkToFolderDecoder(cacheData);
      IImportedObjectList iol = this.plugin.Idw.CreateImportedObjectList(40);
      iol.NewObjectsOnlyInList = false;
      iol.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
      {
        for (int index2 = 0; index2 < packetTables.Count; ++index2)
        {
          long objectId = iol.Items[index2].Object.Object_id;
          cacheData.AddValue(ImportingCategory.ImbaseTableBlobs, (object) packetTables[index2].Item1, objectId);
          if (packetTables[index2].Item2.Count > 0)
            this.plugin.Imdi.dbImporter.SetImbaseTableAttributes(objectId, packetTables[index2].Item2);
        }
        packetTables.Clear();
      });
      foreach (ImbaseGroup table in tablesList)
      {
        ++index1;
        this.PumpCheckPoint(string.Format(format, (object) index1, (object) count), this.CalculatePercent(count, index1, 2, 99));
        long newKey = cacheData.GetNewKey(ImportingCategory.ImbaseTables, (object) table.TableName);
        if (newKey != 0L)
          this.PumpTableBlob(iol, this.plugin.Imdi.UserSession, cacheData, table, deleteFiles, tableAttributes.Clone(), tableData.Clone(), packetTables, newKey, linkDecoder);
      }
      iol.Import();
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseGroups, ImportingCategory.ImbaseTablesAttributes, ImportingCategory.ImbaseTableBlobs, ImportingCategory.ImbaseGroupsAttributes, ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseTables, ImportingCategory.ImbaseFolders, ImportingCategory.ImbaseFolderKeyToLevel, ImportingCategory.ImbaseFoldersGuids, ImportingCategory.ImbasePVForTables);
      foreach (string path in deleteFiles)
        File.Delete(path);
    }
    this.PumpCheckPoint("Добавление блобов таблицам IMBASE успешно завершено", 100);
  }

  private void PumpTableBlob(
    IImportedObjectList iol,
    IUserSession session,
    IImportingData cacheData,
    ImbaseGroup table,
    List<string> deleteFiles,
    DataTable tableAttrs,
    DataTable tableData,
    List<Tuple<string, List<int>>> packetTables,
    long tableID,
    LinkToFolderDecoder linkDecoder)
  {
    iol.UseObject(tableID);
    Dictionary<Guid, int> sortedColumns = new Dictionary<Guid, int>();
    List<int> intList = new List<int>();
    DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ImbaseGroupsAttributes, (object) table.Key);
    if (dictionaryValue != null && dictionaryValue.Tag is ImbaseGroupAttributes tag1 && tag1.Attributes != null)
    {
      foreach (GroupAttribute attribute in tag1.Attributes)
      {
        if (!sortedColumns.ContainsKey(attribute.AttrGuid))
          sortedColumns.Add(attribute.AttrGuid, attribute.Sort);
      }
    }
    int num1 = 0;
    List<Guid> guidList1 = new List<Guid>();
    Dictionary<Guid, DataRow> dictionary1 = new Dictionary<Guid, DataRow>();
    DataSet dataSet = new DataSet("IMS_TABLE_RECORDS");
    IDataReader defaultDataReader = this.GetDefaultDataReader(table.TableName);
    if (defaultDataReader != null)
    {
      try
      {
        object tag2 = (object) cacheData.GetTag(ImportingCategory.ImbaseTablesAttributes, (object) table.Key);
        Guid guid1 = Guid.Empty;
        List<Guid> guidList2 = new List<Guid>();
        if (tag2 != null)
        {
          if (tag2 is ImbaseTableAttributes)
          {
            Dictionary<Guid, string> dictionary2 = !(tag2 is ImbaseTableAttributes imbaseTableAttributes) || imbaseTableAttributes.Attributes == null ? new Dictionary<Guid, string>(1) : new Dictionary<Guid, string>(imbaseTableAttributes.Attributes.Count);
            if (imbaseTableAttributes != null && imbaseTableAttributes.Attributes != null)
            {
              imbaseTableAttributes.Attributes.Sort((Comparison<TableAttribute>) ((attr1, attr2) =>
              {
                int num2 = 0;
                int num3 = 0;
                return sortedColumns.TryGetValue(attr1.AttributeGuid, out num2) && sortedColumns.TryGetValue(attr2.AttributeGuid, out num3) ? num2.CompareTo(num3) : 0;
              }));
              for (int index = 0; index < imbaseTableAttributes.Attributes.Count; ++index)
              {
                AttributeOptions attributeOptions = ImbaseImpHelper.SetOptionsForAttribute(session, imbaseTableAttributes.Attributes[index].AttributeGuid, imbaseTableAttributes.Attributes[index].EnterMode);
                DataRow row = tableAttrs.NewRow();
                if (imbaseTableAttributes.Attributes[index].EnterMode == ImEnterMode.IEM_FOLDER)
                  guidList2.Add(imbaseTableAttributes.Attributes[index].AttributeGuid);
                row["F_ATTRIBUTE_GUID"] = (object) imbaseTableAttributes.Attributes[index].AttributeGuid.ToString();
                row["F_REQUIRED"] = (object) (int) imbaseTableAttributes.Attributes[index].AddMode;
                row["F_COMPUTED"] = (object) (int) imbaseTableAttributes.Attributes[index].ComputeMode;
                row["F_FORMULA"] = (object) imbaseTableAttributes.Attributes[index].ImFormula;
                row["F_UNIQUE"] = (object) 0;
                row["F_DEFAULT_VALUE"] = (object) imbaseTableAttributes.Attributes[index].DefVal;
                int num4 = (int) imbaseTableAttributes.Attributes[index].MaskFlag;
                if (attributeOptions != AttributeOptions.None)
                  num4 = (int) ((AttributeOptions) num4 | attributeOptions);
                row["F_OPTIONS"] = (object) num4;
                row["F_MASK"] = (object) string.Empty;
                row["F_UNITS"] = !(imbaseTableAttributes.Attributes[index].Measure == Guid.Empty) ? (object) imbaseTableAttributes.Attributes[index].Measure : (object) string.Empty;
                row["F_DISPLAY"] = (object) imbaseTableAttributes.Attributes[index].Display;
                tableAttrs.Rows.Add(row);
                if (imbaseTableAttributes.Attributes[index].ComputeMode == ComputeValueModes.NotComputableValue)
                  dictionary1.Add(imbaseTableAttributes.Attributes[index].AttributeGuid, row);
                dictionary2.Add(imbaseTableAttributes.Attributes[index].AttributeGuid, imbaseTableAttributes.Attributes[index].DefVal);
                if (imbaseTableAttributes.Attributes[index].AddMode == RequiredModes.Manual && imbaseTableAttributes.Attributes[index].ComputeMode == ComputeValueModes.NotComputableValue)
                  guidList1.Add(imbaseTableAttributes.Attributes[index].AttributeGuid);
                if (imbaseTableAttributes.Attributes[index].IsGuid)
                  guid1 = imbaseTableAttributes.Attributes[index].AttributeGuid;
                intList.Add(this.plugin.Imdi.AttributeTypes.GetByGuid(imbaseTableAttributes.Attributes[index].AttributeGuid).ID);
              }
              tableAttrs.AcceptChanges();
            }
            object tag3 = (object) cacheData.GetTag(ImportingCategory.ImbaseGroupsAttributes, (object) table.Key);
            if (tag3 != null && tag3 is ImbaseGroupAttributes && tag3 is ImbaseGroupAttributes imbaseGroupAttributes && imbaseGroupAttributes.Attributes != null)
            {
              TableAttributesPV tag4 = cacheData.GetTag(ImportingCategory.ImbasePVForTables, (object) table.Key) as TableAttributesPV;
              ImDataTableItemFactory tableItemFactory = new ImDataTableItemFactory(cacheData, table.TableName, defaultDataReader, this.plugin.Idw.AppManager, (ICollection<GroupAttribute>) imbaseGroupAttributes.Attributes, DataTableItemOptions.ImageLinkGuids);
              IDictionaryEnumerator en = (IDictionaryEnumerator) tableItemFactory.FieldsTypes.GetEnumerator();
              while (en.MoveNext())
              {
                if (!guidList1.Contains((Guid) en.Key))
                {
                  DataColumn column = new DataColumn(((Guid) en.Key).ToString(), (Type) en.Value);
                  tableData.Columns.Add(column);
                  if (tag4 != null)
                  {
                    TableAttributePV tableAttributePv = tag4.Values.Find((Predicate<TableAttributePV>) (x => x.AttributeGuid.Equals((Guid) en.Key)));
                    if (tableAttributePv != null)
                      column.ExtendedProperties[(object) "F_FILTERED_POSSIBLE_VALUES"] = (object) tableAttributePv.Values.ToArray();
                  }
                }
              }
              if (tableData.Columns.Contains("cad00020-306c-11d8-b4e9-00304f19f545") && tableData.Columns.Contains(this.attrSizeAndParamGuid))
              {
                DataTable dataTable = new DataTable();
                foreach (DataColumn column1 in (InternalDataCollectionBase) tableData.Columns)
                {
                  if (!(column1.ColumnName == "cad00020-306c-11d8-b4e9-00304f19f545"))
                  {
                    if (column1.ColumnName == this.attrSizeAndParamGuid.ToString())
                    {
                      DataColumn column2 = tableData.Columns["cad00020-306c-11d8-b4e9-00304f19f545"];
                      DataColumn column3 = new DataColumn(column2.ColumnName, column2.DataType);
                      dataTable.Columns.Add(column3);
                    }
                    else
                    {
                      DataColumn column4 = new DataColumn(column1.ColumnName, column1.DataType);
                      dataTable.Columns.Add(column4);
                    }
                  }
                }
                tableData = dataTable;
              }
              bool flag = true;
              while (defaultDataReader.Read())
              {
                ImDataTableItem imDataTableItem = (ImDataTableItem) tableItemFactory.NewItem(defaultDataReader);
                DataRow row = tableData.NewRow();
                Guid guid2 = Guid.NewGuid();
                IDictionaryEnumerator enumerator = (IDictionaryEnumerator) imDataTableItem.Data.GetEnumerator();
                if (flag)
                {
                  foreach (KeyValuePair<Guid, DataRow> keyValuePair in dictionary1)
                  {
                    if (!imDataTableItem.Data.ContainsKey(keyValuePair.Key))
                    {
                      keyValuePair.Value["F_REQUIRED"] = (object) 0;
                      keyValuePair.Value.Table.AcceptChanges();
                    }
                  }
                  flag = false;
                }
                while (enumerator.MoveNext())
                {
                  if (!guidList1.Contains((Guid) enumerator.Key))
                  {
                    Type dataType = tableData.Columns[((Guid) enumerator.Key).ToString()].DataType;
                    if (enumerator.Value != null)
                    {
                      object obj = enumerator.Value;
                      if (obj.GetType() == typeof (string) && AttributesHelper.IsNumericType(dataType))
                        obj = CompareValuesHelper.NormalizedValue(obj) != null ? (object) Convert.ToString(obj).Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) : (object) 0;
                      if (guidList2.Contains((Guid) enumerator.Key))
                      {
                        DecoderItem decoderItem = new DecoderItem(obj.ToString());
                        if (linkDecoder.Decode(decoderItem, tableID) && decoderItem.FolderGuid != Guid.Empty)
                          obj = (object) decoderItem.FolderGuid.ToString();
                        else if (!string.IsNullOrEmpty(decoderItem.ErrorMessage))
                          this.plugin.Idw.AppManager.AddWarningMessage(decoderItem.ErrorMessage);
                      }
                      this.SetValueToRow(row, (Guid) enumerator.Key, obj, dataType, table.TableName);
                    }
                    else
                    {
                      string empty = string.Empty;
                      if (dictionary2.TryGetValue((Guid) enumerator.Key, out empty))
                        this.SetValueToRow(row, (Guid) enumerator.Key, (object) empty, dataType, table.TableName);
                    }
                    if (guid1 != Guid.Empty && guid1.Equals((Guid) enumerator.Key))
                    {
                      string str = Convert.ToString(enumerator.Value);
                      if (GuidHelper.IsGuid(str))
                        guid2 = new Guid(str);
                    }
                  }
                }
                row["F_GUID"] = (object) guid2;
                row["F_KEY"] = (object) imDataTableItem.RecKey;
                tableData.Rows.Add(row);
                if (imDataTableItem.RecKey >= num1)
                  num1 = imDataTableItem.RecKey + 1;
              }
              tableData.AcceptChanges();
              tableData.Columns["F_KEY"].AutoIncrementSeed = (long) num1;
            }
            dataSet.Tables.AddRange(new DataTable[2]
            {
              tableAttrs,
              tableData
            });
          }
        }
      }
      finally
      {
        defaultDataReader.Close();
      }
    }
    packetTables.Add(new Tuple<string, List<int>>(table.TableName, intList));
    if (dataSet.Tables == null || dataSet.Tables.Count <= 0)
      return;
    string fileFullName;
    PumpImbaseTablesHelper.AddTableBlobAttribute(table, dataSet, iol, out fileFullName);
    deleteFiles.Add(fileFullName);
  }

  private void SetValueToRow(
    DataRow row,
    Guid nameKey,
    object value,
    Type type,
    string tableName)
  {
    try
    {
      if (type != typeof (string) && CompareValuesHelper.NormalizedValue(value) == null)
        row[nameKey.ToString()] = (object) DBNull.Value;
      else
        row[nameKey.ToString()] = Convert.ChangeType(value, type);
    }
    catch (FormatException ex1)
    {
      if (AttributesHelper.IsNumericType(type))
      {
        char[] chArray = new char[10]
        {
          '0',
          '1',
          '2',
          '3',
          '4',
          '5',
          '6',
          '7',
          '8',
          '9'
        };
        string str = Convert.ToString(value);
        bool flag = false;
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < str.Length; ++index)
        {
          if (chArray.Equals((object) str[index]))
            stringBuilder.Append(str[index]);
          else if (Convert.ToString(str[index]).Equals(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) && !flag)
          {
            flag = true;
            stringBuilder.Append(str[index]);
          }
        }
        value = stringBuilder.Length <= 0 ? (object) 0 : (object) stringBuilder.ToString();
        try
        {
          row[nameKey.ToString()] = Convert.ChangeType(value, type);
        }
        catch (FormatException ex2)
        {
          this.plugin.appManager.AddWarningMessage($"Ошибка при приведении обработанного значения \"{value}\" к типу {type.ToString()} в колонке {nameKey} таблицы {tableName} : {ex2.Message}");
        }
      }
      else
        this.plugin.appManager.AddWarningMessage($"Ошибка при приведении значения \"{value}\" к типу {type.ToString()} в колонке {nameKey} таблицы {tableName} : {ex1.Message}");
    }
  }
}
