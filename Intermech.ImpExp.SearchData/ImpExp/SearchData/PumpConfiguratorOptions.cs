// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpConfiguratorOptions
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки опций конфигуратора составов", "Перекачка опций конфигуратора составов")]
internal class PumpConfiguratorOptions : PumpClass
{
  protected SearchDataPlugin plugin;

  public PumpConfiguratorOptions(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  protected override Guid GUID => new Guid("7C9F66DD-AA13-4927-BF97-2C3770FCC7C1");

  public override void Exam()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    service.DeleteCache(ImportingCategory.ConfiguratorCategories);
    IImportingData cache = service.GetCache(ImportingCategory.ConfiguratorCategories);
    try
    {
      string format = "Кэширование категорий опций из базы назначения ({0} из {1})";
      this.ExamCheckPoint("Чтение и кэширование категорий опций из базы назначения", 1);
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      IDBAttributeType attributeType = userSession.GetAttributeType(new Guid("cad015a7-306c-11d8-b4e9-00304f19f545"));
      DataTable dataTable = userSession.GetObjectCollection(new Guid("cad015af-306c-11d8-b4e9-00304f19f545")).Select(new DBRecordSetParams((ConditionStructure[]) null, new object[3]
      {
        (object) -2,
        (object) -12,
        (object) attributeType.AttributeID
      }));
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        this.ExamCheckPoint(string.Format(format, (object) (index + 1), (object) dataTable.Rows.Count), this.CalculatePercent(dataTable.Rows.Count, index + 1, 2, 99));
        cache.AddValue(ImportingCategory.ConfiguratorCategories, (object) Convert.ToString(dataTable.Rows[index][2]), Convert.ToInt64(dataTable.Rows[index][0]), Convert.ToString(dataTable.Rows[index][1]), (ITagImportObject) null);
      }
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ConfiguratorCategories);
    }
    this.ExamCheckPoint("Кэширование категорий опций успешно завершено", 100);
  }

  public override void Pump()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.ConfiguratorCategories, ImportingCategory.ConfiguratorOptions, ImportingCategory.ConfiguratorOptionValues);
    IUserSession userSession = this.plugin.Idw.GetUserSession();
    try
    {
      this.PumpCheckPoint("Перекачка категорий опций", 1);
      this.PumpCategories(cache, userSession);
      this.PumpCheckPoint("Перекачка опций", 3);
      this.PumpOptions(cache, userSession, this.GetNoCategoryID(cache));
      this.PumpCheckPoint("Перекачка опций успешно завершена", 100);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ConfiguratorCategories, ImportingCategory.ConfiguratorOptions, ImportingCategory.ConfiguratorOptionValues);
    }
  }

  private long GetNoCategoryID(IImportingData cacheData)
  {
    foreach (KeyValuePair<object, DictionaryValue> keyValuePair in cacheData.GetCategory(ImportingCategory.ConfiguratorCategories))
    {
      if (keyValuePair.Value.Caption.Equals("cad0159f-306c-11d8-b4e9-00304f19f545"))
        return keyValuePair.Value.NewObjectID;
    }
    return 0;
  }

  private void PumpOptions(IImportingData cacheData, IUserSession session, long noCategoryID)
  {
    int packetSize = (ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize;
    int objectType = session.GetObjectType(new Guid("cad015b0-306c-11d8-b4e9-00304f19f545")).ObjectType;
    int attributeId1 = session.GetAttributeType(new Guid("cad015a4-306c-11d8-b4e9-00304f19f545")).AttributeID;
    int attributeId2 = session.GetAttributeType(new Guid("cad015aa-306c-11d8-b4e9-00304f19f545")).AttributeID;
    int attributeId3 = session.GetAttributeType(new Guid("cad015a5-306c-11d8-b4e9-00304f19f545")).AttributeID;
    int attributeId4 = session.GetAttributeType(new Guid("cad015a8-306c-11d8-b4e9-00304f19f545")).AttributeID;
    int attributeId5 = session.GetAttributeType(new Guid("cad00021-306c-11d8-b4e9-00304f19f545")).AttributeID;
    int attributeId6 = session.GetAttributeType(new Guid("cad015a2-306c-11d8-b4e9-00304f19f545")).AttributeID;
    IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
    List<PumpConfiguratorOptions.Option> package = new List<PumpConfiguratorOptions.Option>(packetSize);
    iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
    {
      for (int index1 = 0; index1 < iolIm.Items.Count; ++index1)
      {
        if (iolIm.Items[index1].Object.Object_id != 0L && iolIm.Items[index1].Object.Object_id != -1L)
        {
          List<int> optionValues = new List<int>(package[index1].OptionValues.Count);
          for (int index2 = 0; index2 < package[index1].OptionValues.Count; ++index2)
          {
            cacheData.AddValue(ImportingCategory.ConfiguratorOptionValues, (object) package[index1].OptionValues[index2].OptValueID, Convert.ToInt64(package[index1].OptID), package[index1].OptionValues[index2].Index.ToString());
            optionValues.Add(package[index1].OptionValues[index2].OptValueID);
          }
          cacheData.AddValue(ImportingCategory.ConfiguratorOptions, (object) package[index1].OptID, iolIm.Items[index1].Object.Object_id, package[index1].Caption, (ITagImportObject) new ArticleOptionsTag((Guid) iolIm.Items[index1].Object.ObjectGuid, optionValues));
        }
        else
          this.plugin.appManager.AddWarningMessage($"Опция {package[index1]} не импортирована. См. серверный лог.");
      }
      package.Clear();
    });
    string format = "Обработка записи из таблицы PC_OPTION_LIST ({0} из {1})";
    int index3 = 0;
    int tableRecordsCount = this.GetTableRecordsCount("pc_option_list");
    using (IDbCommand command = this.plugin.idb.DbConnection.CreateCommand())
    {
      command.CommandText = "select opt_id, opt_type, opt_code, opt_name, opt_description, category from pc_option_list";
      IDataReader dataReader1 = command.ExecuteReader();
      try
      {
        while (dataReader1.Read())
        {
          ++index3;
          this.PumpCheckPoint(string.Format(format, (object) index3, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index3, 4, 99));
          int int32 = BasePumpHelper.ToInt32(dataReader1[0]);
          if (cacheData.GetNewKey(ImportingCategory.ConfiguratorOptions, (object) int32) == 0L)
          {
            string str1 = dataReader1.IsDBNull(2) ? string.Empty : dataReader1.GetString(2);
            string caption = dataReader1.IsDBNull(3) ? string.Empty : dataReader1.GetString(3);
            PumpConfiguratorOptions.Option option = new PumpConfiguratorOptions.Option(int32, caption);
            iolIm.AddObject(objectType, 0, caption);
            int num1 = dataReader1.IsDBNull(1) ? -1 : BasePumpHelper.ToInt32(dataReader1[1]);
            FieldTypes fieldTypes = FieldTypes.ftUnknown;
            switch (num1)
            {
              case 0:
                fieldTypes = FieldTypes.ftString;
                break;
              case 1:
                fieldTypes = FieldTypes.ftInteger;
                break;
              case 2:
                fieldTypes = FieldTypes.ftDouble;
                break;
              case 3:
                fieldTypes = FieldTypes.ftDateTime;
                break;
            }
            iolIm.AddAttributeInt(attributeId2, (long) fieldTypes);
            iolIm.AddAttributeStr(attributeId3, str1);
            iolIm.AddAttributeStr(attributeId4, caption);
            string str2 = dataReader1.IsDBNull(4) ? string.Empty : dataReader1.GetString(4);
            iolIm.AddAttributeStr(attributeId5, str2);
            string str3 = dataReader1.IsDBNull(5) ? string.Empty : dataReader1.GetString(5);
            long num2 = str3 == string.Empty ? noCategoryID : cacheData.GetNewKey(ImportingCategory.ConfiguratorCategories, (object) str3);
            iolIm.AddAttributeLink(attributeId1, num2, str3);
            OptionValuesCollection valuesCollection = new OptionValuesCollection();
            using (IDataReader dataReader2 = BasePumpHelper.S4Query(this.plugin.idb2.DbConnection, "select optval_id, opt_valuecode, opt_value, opt_valdesc from pc_option_values where opt_id=@p1", CommandBehavior.Default, (object) int32))
            {
              int index4 = 0;
              while (dataReader2.Read())
              {
                option.OptionValues.Add(new PumpConfiguratorOptions.OptionValueLink(dataReader2.IsDBNull(0) ? 0 : BasePumpHelper.ToInt32(dataReader2[0]), index4));
                valuesCollection.Add(new OptionValue(index4.ToString(), dataReader2.IsDBNull(1) ? string.Empty : dataReader2.GetString(1), dataReader2.IsDBNull(2) ? string.Empty : dataReader2.GetString(2), dataReader2.IsDBNull(3) ? string.Empty : dataReader2.GetString(3), Guid.Empty, OptionValueFlags.None, Guid.Empty, DateTime.UtcNow));
                ++index4;
              }
            }
            if (valuesCollection.Count > 0)
            {
              IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeId6);
              List<string> stringList = StringsHelper.SplitString(valuesCollection.ToString(attributeId6), (int) attributeType.SizeType);
              for (int index5 = 0; index5 < stringList.Count; ++index5)
                iolIm.AddAttribute(attributeId6, AttrValueType.stringVal, (object) stringList[index5], index5);
            }
            AttributesHelper.AddObligatoryObjectAttributes(session, iolIm);
            package.Add(option);
          }
        }
      }
      finally
      {
        dataReader1.Close();
      }
      iolIm.Import();
    }
  }

  private void PumpCategories(IImportingData cacheData, IUserSession session)
  {
    int packetSize = (ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize;
    int objectType = session.GetObjectType(new Guid("cad015af-306c-11d8-b4e9-00304f19f545")).ObjectType;
    int attributeId = session.GetAttributeType(new Guid("cad015a7-306c-11d8-b4e9-00304f19f545")).AttributeID;
    IImportedObjectList iolIm = this.plugin.Idw.CreateImportedObjectList();
    List<string> package = new List<string>(packetSize);
    iolIm.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
    {
      for (int index = 0; index < iolIm.Items.Count; ++index)
      {
        if (iolIm.Items[index].Object.Object_id != 0L && iolIm.Items[index].Object.Object_id != -1L)
          cacheData.AddValue(ImportingCategory.ConfiguratorCategories, (object) package[index], iolIm.Items[index].Object.Object_id);
        else
          this.plugin.appManager.AddWarningMessage($"Категория опций {package[index]} не импортирована. См. серверный лог.");
      }
      package.Clear();
    });
    using (IDbCommand command = this.plugin.idb.DbConnection.CreateCommand())
    {
      command.CommandText = "select distinct(category) from pc_option_list";
      IDataReader dataReader = command.ExecuteReader();
      try
      {
        while (dataReader.Read())
        {
          string str = dataReader.IsDBNull(0) ? string.Empty : dataReader.GetString(0);
          if (!(str == string.Empty) && cacheData.GetNewKey(ImportingCategory.ConfiguratorCategories, (object) str) == 0L)
          {
            iolIm.AddObject(objectType, 0, str);
            iolIm.AddAttributeStr(attributeId, str);
            AttributesHelper.AddObligatoryObjectAttributes(BasePumpHelper.Session, iolIm);
            package.Add(str);
          }
        }
      }
      finally
      {
        dataReader.Close();
      }
      iolIm.Import();
    }
  }

  private class Option
  {
    public int OptID;
    public string Caption;
    public List<PumpConfiguratorOptions.OptionValueLink> OptionValues;

    public Option(int optID, string caption)
    {
      this.OptID = optID;
      this.Caption = caption;
      this.OptionValues = new List<PumpConfiguratorOptions.OptionValueLink>();
    }
  }

  private class OptionValueLink
  {
    public int OptValueID;
    public int Index;

    public OptionValueLink(int optValueID, int index)
    {
      this.OptValueID = optValueID;
      this.Index = index;
    }
  }
}
