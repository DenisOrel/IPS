// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpObjectPoperties
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("Получение информации о таблице 'Свойства материала'", "Перекачка таблицы  'Свойства материала'")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpObjectPoperties : PumpImbaseClass
{
  internal const string tableIM_M_OBJS_PROPS = "IM_M_OBJS_PROPS";
  private int _indexKey;
  private int _indexBlobID = 1;
  private int _indexMaterial = 2;
  private int _objTypeMaterialProp = -1;
  private int _attributeMaterialProp = -1;
  internal static Guid guid = new Guid("{226A6C0C-FADB-490A-88B4-A2F39FBE9ECE}");

  protected override Guid GUID => PumpObjectPoperties.guid;

  public PumpObjectPoperties(ImbasePlugin plugin)
    : base(plugin)
  {
    IUserSession userSession = plugin.Imdi.UserSession;
    this._objTypeMaterialProp = userSession.GetObjectType(new Guid("cadd93d1-306c-11d8-b4e9-00304f19f545")).ObjectType;
    this._attributeMaterialProp = userSession.GetAttributeType(new Guid("cadd93d3-306c-11d8-b4e9-00304f19f545")).AttributeID;
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Чтение информации о таблице 'Свойства для объектов'", 0);
    if (this.plugin.idb.TableExists("IM_M_OBJS_PROPS"))
    {
      ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
      service.DeleteCache(ImportingCategory.ImbaseObjectProps);
      service.DeleteCache(ImportingCategory.ImbaseObjectPropsItems);
      IImportingData cache = service.GetCache(ImportingCategory.ImbaseObjectProps, ImportingCategory.ImbaseObjectPropsItems);
      try
      {
        IUserSession userSession = this.plugin.Imdi.UserSession;
        DataTable dataTable = userSession.GetObjectCollection(ImbaseIDHelper.ObjTypeIdImTab).Select(new DBRecordSetParams(new ConditionStructure[1]
        {
          new ConditionStructure(ImbaseIDHelper.AttrIdTableName, RelationalOperators.Equal, (object) "IM_M_OBJS_PROPS", LogicalOperators.AND, 0, false)
        }, new object[1]{ (object) -2 }));
        if (dataTable.Rows.Count > 0)
        {
          IDBObject dbObject = userSession.GetObject(Convert.ToInt64(dataTable.Rows[0][0]));
          if (dbObject.GetAttributeByID(ImbaseIDHelper.AttrTableData) != null)
          {
            if (dbObject.ObjectModifyMode == ObjectModifyModes.Checkout)
              dbObject = dbObject.CheckOut();
            dbObject.GetAttributeByID(ImbaseIDHelper.AttrTableData).Clear();
            if (dbObject.ObjectModifyMode == ObjectModifyModes.Checkout)
              dbObject.CheckIn();
          }
          cache.AddValue(ImportingCategory.ImbaseObjectProps, (object) "F_OBJECT_ID", dbObject.ObjectID, dbObject.ID.ToString());
        }
        int tableRecordsCount = this.GetTableRecordsCount("IM_M_OBJS_PROPS");
        int index = 0;
        string empty = string.Empty;
        int num = tableRecordsCount / 100;
        this.ExamCheckPoint("Получение данных из таблицы 'Свойства для объектов'", 1);
        IDataReader dataReader = this.GetDataReader($"SELECT * FROM {"IM_M_OBJS_PROPS"}");
        try
        {
          string format = "Получение данных из таблицы 'Свойства для объектов' ({0} из {1})";
          while (dataReader.Read())
          {
            ++index;
            if (index % num == 1 || index == tableRecordsCount)
              this.ExamCheckPoint(string.Format(format, (object) index, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index, 3, 99));
            long int64_1 = Convert.ToInt64(dataReader[this._indexKey]);
            long int64_2 = dataReader.IsDBNull(this._indexBlobID) ? 0L : Convert.ToInt64(dataReader[this._indexBlobID]);
            string caption = dataReader.IsDBNull(this._indexMaterial) ? string.Empty : dataReader.GetString(this._indexMaterial);
            if (int64_2 != 0L)
              cache.AddValue(ImportingCategory.ImbaseObjectPropsItems, (object) int64_1, int64_2, caption);
          }
        }
        finally
        {
          dataReader.Close();
        }
      }
      finally
      {
        service?.ReleaseCache(ImportingCategory.ImbaseObjectProps, ImportingCategory.ImbaseObjectPropsItems);
      }
    }
    this.ExamCheckPoint("Получение данных завершено", 100);
  }

  public override void Pump()
  {
    if (!this.plugin.idb.TableExists("IM_M_OBJS_PROPS"))
    {
      this.PumpCheckPoint("Обработка данных из таблицы  'Свойства для объектов' успешно завершена", 100);
    }
    else
    {
      ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
      IImportingData cacheData = service.GetCache(ImportingCategory.ImbaseObjectProps, ImportingCategory.ImbaseObjectPropsItems, ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseTables);
      try
      {
        Dictionary<object, DictionaryValue> category = cacheData.GetCategory(ImportingCategory.ImbaseObjectPropsItems);
        Dictionary<int, PumpObjectPoperties.TableRec> table = new Dictionary<int, PumpObjectPoperties.TableRec>(category.Count);
        int index1 = 0;
        string format = "Обработка данных из таблицы 'Свойства для объектов' ({0} из {1})";
        int num = category.Count / 100;
        ImBlobsItemFactory blobsItemFactory = (ImBlobsItemFactory) null;
        bool flag1 = true;
        Dictionary<int, PumpObjectPoperties.TableRec> packetTable = new Dictionary<int, PumpObjectPoperties.TableRec>((ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize);
        int owner = 0;
        IImportedObjectList iol = this.plugin.Idw.CreateImportedObjectList();
        iol.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          int index2 = 0;
          foreach (KeyValuePair<int, PumpObjectPoperties.TableRec> keyValuePair in packetTable)
          {
            if (iol.Items[index2].Object.Object_id != 0L && iol.Items[index2].Object.Object_id != -1L)
            {
              cacheData.AddValue(ImportingCategory.ImbaseBlobs, (object) keyValuePair.Key, iol.Items[index2].Object.Object_id);
              keyValuePair.Value.BlobID = iol.Items[index2].Object.Object_id;
              PumpObjectPoperties.TableRec tableRec;
              if (!table.TryGetValue(keyValuePair.Key, out tableRec))
              {
                tableRec = keyValuePair.Value;
                table.Add(keyValuePair.Key, tableRec);
              }
              else
              {
                for (int index3 = 0; index3 < keyValuePair.Value.Material.Count; ++index3)
                  tableRec.Material.Add(keyValuePair.Value.Material[index3]);
              }
            }
            ++index2;
          }
          packetTable.Clear();
        });
        foreach (KeyValuePair<object, DictionaryValue> keyValuePair in category)
        {
          ++index1;
          if (index1 % num == 1 || index1 == category.Count)
            this.PumpCheckPoint(string.Format(format, (object) index1, (object) category.Count), this.CalculatePercent(category.Count, index1, 1, 90));
          IDataReader dataReader = this.GetDataReader($"SELECT {ImBlobsItemFactory.TableColumns} FROM {ImBlobsItemFactory.TableName} WHERE F_KEY={keyValuePair.Value.NewObjectID}");
          try
          {
            if (dataReader.Read())
            {
              if (flag1)
              {
                blobsItemFactory = new ImBlobsItemFactory(dataReader, this.plugin.Idw.AppManager);
                flag1 = false;
              }
              IImBlobsItem imBlobsItem = blobsItemFactory.NewItem(dataReader, BlobType.MaterialProps);
              long newKey = cacheData.GetNewKey(ImportingCategory.ImbaseBlobs, (object) imBlobsItem.Key);
              if (newKey == 0L)
              {
                PumpObjectPoperties.TableRec tableRec;
                if (packetTable.TryGetValue(imBlobsItem.Key, out tableRec))
                {
                  tableRec.Material.Add(keyValuePair.Value.Caption);
                }
                else
                {
                  iol.AddObject(this._objTypeMaterialProp, owner, keyValuePair.Value.Caption);
                  iol.AddAttribute(ImbaseIDHelper.AttrIdName, AttrValueType.stringVal, (object) keyValuePair.Value.Caption, 0);
                  iol.AddAttributeBlob(this._attributeMaterialProp, imBlobsItem.TmpFileName, imBlobsItem.FileSize, imBlobsItem.Source, imBlobsItem.IsZipped ? ArcMethods.ZLibPacked : ArcMethods.NotPacked);
                  AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), iol);
                  packetTable.Add(imBlobsItem.Key, new PumpObjectPoperties.TableRec(Convert.ToInt64(imBlobsItem.Key), keyValuePair.Value.NewObjectID, keyValuePair.Value.Caption));
                }
              }
              else
              {
                PumpObjectPoperties.TableRec tableRec;
                if (!table.TryGetValue(imBlobsItem.Key, out tableRec))
                {
                  tableRec = new PumpObjectPoperties.TableRec(Convert.ToInt64(imBlobsItem.Key), newKey, keyValuePair.Value.Caption);
                  table.Add(imBlobsItem.Key, tableRec);
                }
                else
                  tableRec.Material.Add(keyValuePair.Value.Caption);
              }
            }
          }
          finally
          {
            dataReader.Close();
          }
        }
        iol.Import();
        this.PumpCheckPoint("Формирование новых данных", 91);
        DataTable tableAttributes = ImbaseImpHelper.GetTableAttributes();
        string columnName1 = "cadd941e-306c-11d8-b4e9-00304f19f545";
        DataRow row1 = tableAttributes.NewRow();
        row1["F_ATTRIBUTE_GUID"] = (object) columnName1;
        row1["F_REQUIRED"] = (object) 2;
        row1["F_COMPUTED"] = (object) 0;
        row1["F_FORMULA"] = (object) string.Empty;
        row1["F_UNIQUE"] = (object) 0;
        row1["F_DEFAULT_VALUE"] = (object) string.Empty;
        row1["F_OPTIONS"] = (object) 131072 /*0x020000*/;
        row1["F_MASK"] = (object) string.Empty;
        row1["F_UNITS"] = (object) string.Empty;
        row1["F_DISPLAY"] = (object) string.Empty;
        tableAttributes.Rows.Add(row1);
        string columnName2 = "cadd941f-306c-11d8-b4e9-00304f19f545";
        DataRow row2 = tableAttributes.NewRow();
        row2["F_ATTRIBUTE_GUID"] = (object) columnName2;
        row2["F_REQUIRED"] = (object) 2;
        row2["F_COMPUTED"] = (object) 0;
        row2["F_FORMULA"] = (object) string.Empty;
        row2["F_UNIQUE"] = (object) 0;
        row2["F_DEFAULT_VALUE"] = (object) string.Empty;
        row2["F_OPTIONS"] = (object) 131072 /*0x020000*/;
        row2["F_MASK"] = (object) string.Empty;
        row2["F_UNITS"] = (object) string.Empty;
        row2["F_DISPLAY"] = (object) string.Empty;
        tableAttributes.Rows.Add(row2);
        tableAttributes.AcceptChanges();
        DataTable tableData = ImbaseImpHelper.GetTableData();
        tableData.Columns.Add(new DataColumn(columnName1, typeof (long)));
        tableData.Columns.Add(new DataColumn(columnName2, typeof (string)));
        foreach (KeyValuePair<int, PumpObjectPoperties.TableRec> keyValuePair in table)
        {
          for (int index4 = 0; index4 < keyValuePair.Value.Material.Count; ++index4)
          {
            DataRow row3 = tableData.NewRow();
            row3["F_GUID"] = (object) Guid.NewGuid();
            row3["F_KEY"] = (object) keyValuePair.Key;
            row3[columnName1] = (object) keyValuePair.Value.BlobID;
            row3[columnName2] = (object) keyValuePair.Value.Material[index4];
            tableData.Rows.Add(row3);
          }
        }
        tableData.AcceptChanges();
        DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ImbaseObjectProps, (object) "F_OBJECT_ID");
        iol = this.plugin.Idw.CreateImportedObjectList(0);
        bool flag2 = true;
        if (dictionaryValue == null || dictionaryValue.NewObjectID == 0L)
        {
          iol.AddObject(ImbaseIDHelper.ObjTypeIdImTab, owner, "Свойства для объектов");
          iol.AddAttributeStr(ImbaseIDHelper.AttrIdTableName, "IM_M_OBJS_PROPS");
        }
        else
        {
          ObjectRecord objectRecord = new ObjectRecord()
          {
            Object_id = dictionaryValue.NewObjectID,
            VersionId = -1,
            Id = Convert.ToInt64(dictionaryValue.Caption),
            ObjectType = ImbaseIDHelper.ObjTypeIdImTab
          };
          iol.UseObject(objectRecord);
          flag2 = false;
        }
        string str = Path.Combine(Path.GetTempPath(), $"{"IM_M_OBJS_PROPS"}.tmp");
        long fileSize = 0;
        DataSet graph = new DataSet("IMS_TABLE_RECORDS");
        graph.Tables.AddRange(new DataTable[2]
        {
          tableAttributes,
          tableData
        });
        using (MemoryStream memoryStream = new MemoryStream())
        {
          BinaryFormatter binaryFormatter = new BinaryFormatter();
          graph.RemotingFormat = SerializationFormat.Binary;
          FileStream outStream = new FileStream(str, FileMode.Create, FileAccess.Write);
          try
          {
            binaryFormatter.Serialize((Stream) memoryStream, (object) graph);
            memoryStream.Position = 0L;
            ((IPackedStream) ServicesManager.ServiceContainer.GetService(typeof (IPackedStream))).PackStream((Stream) outStream, (Stream) memoryStream, 9);
          }
          finally
          {
            outStream.Flush();
            fileSize = outStream.Length;
            outStream.Close();
          }
        }
        if (flag2)
          AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), iol);
        iol.AddAttributeBlob(ImbaseIDHelper.AttrTableData, str, fileSize, "IM_M_OBJS_PROPS", ArcMethods.ZLibPacked).IsNew = flag2;
        iol.Import();
        long newKey1 = dictionaryValue == null || dictionaryValue.NewObjectID == 0L ? iol.Items[0].Object.Object_id : dictionaryValue.NewObjectID;
        if (cacheData.GetNewKey(ImportingCategory.ImbaseTables, (object) "IM_M_OBJS_PROPS") == -1L)
          cacheData.AddValue(ImportingCategory.ImbaseTables, (object) "IM_M_OBJS_PROPS", newKey1, "Свойства для объектов");
        else
          cacheData.SetNewKey(ImportingCategory.ImbaseTables, (object) "IM_M_OBJS_PROPS", newKey1);
        File.Delete(str);
        this.PumpCheckPoint("Обработка данных из таблицы  'Свойства для объектов' успешно завершена", 100);
      }
      finally
      {
        service?.ReleaseCache(ImportingCategory.ImbaseObjectProps, ImportingCategory.ImbaseObjectPropsItems, ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseTables);
      }
    }
  }

  private class TableRec
  {
    public long Key;
    public long BlobID;
    public List<string> Material;

    public TableRec(long key, long blobID, string material)
    {
      this.Key = key;
      this.BlobID = blobID;
      this.Material = new List<string>() { material };
    }
  }
}
