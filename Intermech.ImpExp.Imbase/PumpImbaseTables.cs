// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpImbaseTables
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("Получение информации о таблицах Imbase", "Перекачка таблиц Imbase")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpImbaseTables(ImbasePlugin plugin) : PumpImbaseClass(plugin)
{
  internal static Guid guid = new Guid("{CA3B7648-A15D-40cc-A77C-171135D4080A}");
  public List<IImTablesItem> tabRecTypesList;

  protected override Guid GUID => PumpImbaseTables.guid;

  public void Clear()
  {
    if (this.tabRecTypesList == null)
      return;
    this.tabRecTypesList.Clear();
  }

  public override void Exam()
  {
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    IDBAttributeType attributeType1 = userSession.GetAttributeType(new Guid("cad00200-306c-11d8-b4e9-00304f19f545"));
    IDBAttributeType attributeType2 = userSession.GetAttributeType(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
    DataTable dataTable = userSession.GetObjectCollection(new Guid("cad00221-306c-11d8-b4e9-00304f19f545")).Select(new DBRecordSetParams((ConditionStructure[]) null, new object[3]
    {
      (object) -12,
      (object) attributeType2.AttributeID,
      (object) attributeType1.AttributeID
    }));
    Dictionary<Guid, CatalogPres> catalogsPres = new Dictionary<Guid, CatalogPres>(dataTable.Rows.Count);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      DataRow row = dataTable.Rows[index];
      ImTablesType type;
      switch (Convert.ToString(row[2]))
      {
        case "Справочники":
          type = ImTablesType.IMTT_CTLREF;
          break;
        case "Технологические справочники":
          type = ImTablesType.IMTT_TECHREF;
          break;
        default:
          type = ImTablesType.IMTT_CATALOG;
          break;
      }
      Guid guid = new Guid(Convert.ToString(row[0]));
      catalogsPres.Add(guid, new CatalogPres(guid, Convert.ToString(row[1]), type));
    }
    ImbasePlugin.selectCatalogsForm.AddCatalogs(catalogsPres);
    this.ExamCheckPoint("Определение количества записей", 0);
    int tableRecordsCount = this.GetTableRecordsCount(ImTablesItemFactory.TableName);
    int index1 = 0;
    ImbaseGroups instance = ImbaseGroups.instance;
    this.ExamCheckPoint("Получение данных из базы", 4);
    IDataReader defaultDataReader = this.GetDefaultDataReader(ImTablesItemFactory.TableName);
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    service.DeleteCache(ImportingCategory.ImbaseGroups);
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseGroups);
    try
    {
      this.ExamCheckPoint("Получение индексов полей", 5);
      ImTablesItemFactory tablesItemFactory = new ImTablesItemFactory(defaultDataReader, this.plugin.Idw.AppManager);
      string format = "Получение информации о каталоге (таблице) Imbase ({0} из {1})";
      this.tabRecTypesList = new List<IImTablesItem>(tableRecordsCount);
      while (defaultDataReader.Read())
      {
        ++index1;
        if (index1 % 100 == 1 || index1 == tableRecordsCount)
          this.ExamCheckPoint(string.Format(format, (object) index1, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index1, 5, 40));
        IImTablesItem imTablesItem = tablesItemFactory.NewItem(defaultDataReader);
        bool flag = imTablesItem.TableType == ImTablesType.IMTT_CATALOG || imTablesItem.TableType == ImTablesType.IMTT_CTLREF || imTablesItem.TableType == ImTablesType.IMTT_TECHREF;
        if ((!flag || ImbasePlugin.IsCatalogToPump(imTablesItem.TableName)) && (flag || ImbasePlugin.IsTableToPump(imTablesItem.TableName)))
        {
          if (flag)
            this.plugin.catalogBindingControl.AddCatalog(imTablesItem, catalogsPres);
          if (instance.TableExistsByName(imTablesItem.TableName))
            throw new Exception($"Неверные данные в таблице IM_TABLES: Присутсвуют несколько записей о таблице {imTablesItem.TableName}. Дальнейшая работа программы невозможна.");
          instance.TableAdd(imTablesItem.Key, imTablesItem);
          if (imTablesItem.TableType == ImTablesType.IMTT_TABLE)
            this.tabRecTypesList.Add(imTablesItem);
        }
      }
    }
    finally
    {
      defaultDataReader.Close();
    }
    this.ExamCheckPoint("Определение количества записей", 41);
    try
    {
      int count = instance.AllTables.Count;
      int index2 = 0;
      string format = "Получение информации о схеме таблицы Imbase ({0} из {1})";
      List<string> stringList = new List<string>();
      foreach (IImTablesItem allTable in (IEnumerable<IImTablesItem>) instance.AllTables)
      {
        ++index2;
        if (index2 % 20 == 1 || index2 == count)
          this.ExamCheckPoint(string.Format(format, (object) index2, (object) count), this.CalculatePercent(count, index2, 41, 99));
        string tableName = allTable.TableName;
        if (allTable.TableType == ImTablesType.IMTT_CATALOG || allTable.TableType == ImTablesType.IMTT_CTLREF || allTable.TableType == ImTablesType.IMTT_TECHREF)
          tableName += "_REC";
        IDataReader dataReader1 = (IDataReader) null;
        IDataReader dataReader2;
        try
        {
          if (allTable.TableType == ImTablesType.IMTT_CATALOG)
          {
            try
            {
              dataReader1 = this.GetDataReader($"select t.f_level, count(t.f_key) cou from {allTable.TableName} t group by t.f_level having count(t.f_key) > 1");
              if (dataReader1.Read())
                throw new Exception("Присутствуют неуникальные значения в F_LEVEL");
            }
            finally
            {
              dataReader1.Close();
            }
          }
          dataReader2 = this.GetShemaDataReader(tableName);
        }
        catch (Exception ex)
        {
          dataReader2 = (IDataReader) null;
          this.plugin.appManager.AddWarningMessage($"Ошибка при получении схемы таблицы {tableName} : {ex.Message}");
        }
        if (dataReader2 != null)
        {
          try
          {
            cache.AddValue(ImportingCategory.ImbaseGroups, (object) allTable.Key, long.MinValue, (ITagImportObject) new ImbaseGroup(allTable.Key, (int) allTable.TableType, (int) allTable.State, allTable.Openmode, allTable.Order, allTable.Nextkey, allTable.TextID, allTable.GraphID, allTable.Access, allTable.TableName, allTable.Description, allTable.User, allTable.Created, allTable.Modified, allTable.RecordsTypeGuid));
            this.GetTableColumns(dataReader2);
            foreach (ITableFieldInfo tableFieldInfo in PumpItemFactory.GetFieldsInfo(dataReader2))
              allTable.ExistingFields.Add(tableFieldInfo);
          }
          finally
          {
            dataReader2.Close();
          }
        }
      }
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseGroups);
    }
    this.ExamCheckPoint($"Получение данных завершено (обработано {index1} записей)", 100);
  }
}
