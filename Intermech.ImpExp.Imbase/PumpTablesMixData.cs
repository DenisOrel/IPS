// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpTablesMixData
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("", "Перекачка таблиц рецептур")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpTablesMixData : PumpImbaseClass
{
  private readonly string _ownerColName;
  private readonly string _mixColName;
  private readonly string _countColName;
  public static Guid _guid = new Guid("{B07722BE-AFF7-4FBF-81DD-0E10E01A9094}");

  public PumpTablesMixData(ImbasePlugin plugin)
    : base(plugin)
  {
    this._ownerColName = Intermech.Imbase.Consts.LinkToCompoundObjectAttGUID.ToString();
    this._mixColName = Intermech.Imbase.Consts.LinkToComponentOfCompositeObjectAttGuid.ToString();
    this._countColName = "cad00267-306c-11d8-b4e9-00304f19f545";
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Определение количества записей для закачки информации о таблицах рецептур", 0);
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseGroups, ImportingCategory.ImbaseTables, ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseMixTables);
    try
    {
      List<ImbaseGroup> tablesList = PumpImbaseTablesHelper.GetTablesList(cache, ImportingCategory.ImbaseTables, 2);
      int count = tablesList.Count;
      this.SetCountPumpRecords(count);
      int index = 0;
      string format = "Закачка таблиц рецептур ({0} из {1})";
      foreach (ImbaseGroup tableRec in tablesList)
      {
        ++index;
        this.PumpCheckPoint(string.Format(format, (object) index, (object) count), this.CalculatePercent(count, index, 2, 99));
        this.PumpTable(tableRec, cache);
      }
      this.PumpCheckPoint("Создание таблиц рецептур успешно завершено", 100);
    }
    finally
    {
      service.ReleaseCache(ImportingCategory.ImbaseGroups, ImportingCategory.ImbaseTables, ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseMixTables);
    }
  }

  private void PumpTable(ImbaseGroup tableRec, IImportingData cacheData)
  {
    DataTable tableAttributes = ImbaseImpHelper.GetTableAttributes();
    DataTable tableData = ImbaseImpHelper.GetTableData();
    this.CreateAttributeRow(tableAttributes, Intermech.Imbase.Consts.LinkToCompoundObjectAttGUID, true);
    tableData.Columns.Add(this._ownerColName, typeof (string));
    this.CreateAttributeRow(tableAttributes, Intermech.Imbase.Consts.LinkToComponentOfCompositeObjectAttGuid, true);
    tableData.Columns.Add(this._mixColName, typeof (string));
    this.CreateAttributeRow(tableAttributes, new Guid("cad00267-306c-11d8-b4e9-00304f19f545"), false);
    tableData.Columns.Add(this._countColName, AttributesTypeHelper.GetTypeOfAttributeValue(FieldTypes.ftMeasured));
    int ownerUser = 0;
    IImportedObjectList importedTableObjectList = PumpImbaseTablesHelper.GetImportedTableObjectList(this.plugin, tableRec, cacheData, ownerUser, ImbaseIDHelper.ObjTypeIdImTabMixData);
    DataSet dataSet = new DataSet("IMS_TABLE_RECORDS");
    IDataReader defaultDataReader = this.GetDefaultDataReader(tableRec.TableName, "F_OWNER, F_MIX, F_UNITS, F_VALUE, F_KEY");
    try
    {
      while (defaultDataReader.Read())
      {
        DataRow row = tableData.NewRow();
        row[this._ownerColName] = (object) ReaderHelper.GetString(defaultDataReader, 0);
        row[this._mixColName] = (object) ReaderHelper.GetString(defaultDataReader, 1);
        string shortName = ReaderHelper.GetString(defaultDataReader, 2);
        if (shortName != string.Empty)
        {
          MeasureDescriptor descriptor = MeasureHelper.FindDescriptor(shortName);
          if (!descriptor.Empty && !defaultDataReader.IsDBNull(3))
            row[this._countColName] = (object) new MeasuredValue(ReaderHelper.GetDouble(defaultDataReader, 3), descriptor.MeasureID);
        }
        row["F_KEY"] = (object) ReaderHelper.GetInt32(defaultDataReader, 4);
        row["F_GUID"] = (object) this.plugin.Imdi.NewPumpGuid();
        tableData.Rows.Add(row);
      }
    }
    finally
    {
      defaultDataReader.Close();
    }
    dataSet.Tables.AddRange(new DataTable[2]
    {
      tableAttributes,
      tableData
    });
    string fileFullName;
    PumpImbaseTablesHelper.AddTableBlobAttribute(tableRec, dataSet, importedTableObjectList, out fileFullName);
    AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), importedTableObjectList);
    importedTableObjectList.Import();
    tableRec.ObjectID = importedTableObjectList.Items[0].Object.Object_id;
    cacheData.AddValue(ImportingCategory.ImbaseTables, (object) tableRec.TableName, tableRec.ObjectID, tableRec.Description);
    cacheData.AddValue(ImportingCategory.ImbaseMixTables, (object) tableRec.TableName, tableRec.ObjectID);
    File.Delete(fileFullName);
  }

  private void CreateAttributeRow(DataTable tableAttrs, Guid attributeGuid, bool isTableRecordRef)
  {
    IAttributeTypeItem byGuid = this.plugin.Imdi.AttributeTypes.GetByGuid(attributeGuid);
    DataRow row = tableAttrs.NewRow();
    row["F_ATTRIBUTE_GUID"] = (object) attributeGuid.ToString();
    row["F_REQUIRED"] = (object) 2;
    row["F_COMPUTED"] = (object) 0;
    row["F_FORMULA"] = (object) string.Empty;
    row["F_UNIQUE"] = (object) 0;
    row["F_DEFAULT_VALUE"] = byGuid.DefaultValue;
    row["F_OPTIONS"] = (object) (isTableRecordRef ? 131072 /*0x020000*/ : 0);
    row["F_MASK"] = (object) string.Empty;
    row["F_UNITS"] = (object) string.Empty;
    row["F_DISPLAY"] = (object) string.Empty;
    tableAttrs.Rows.Add(row);
  }

  protected override Guid GUID => PumpTablesMixData._guid;
}
