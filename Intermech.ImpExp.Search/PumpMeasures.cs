// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.PumpMeasures
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search;

[TaskDescription("Инициализация данных для перекачки единиц измерения", "Перекачка данных о единицах измерения")]
[TaskType(PumperType.MetaData)]
public class PumpMeasures : PumpSearchClass
{
  private int _attrPhisValueName;
  private int _attrMeasureName;
  private int _attrMeasureShortName;
  private int _attrMeasureKoefficient;
  private int _relTypeId;
  private const string tableNameMeasures = "MU";
  private const string fieldNameMuId = "MU_ID";
  private const string fieldNameDimension = "DIMENSION";
  private const string fieldNameMSystem = "MSYSTEM";
  private const string fieldNameMuIdBase = "MU_ID_BASE";
  private const string fieldNameMuName = "MU_NAME";
  private const string fieldNameMuShName = "MU_SHORT_NAME";
  private const string fieldNameK = "K";

  protected override Guid GUID => new Guid("3EE0A2EF-446E-41ce-8DA1-FA8FB4507AE7");

  public PumpMeasures(SearchPlugin plugin)
    : base(plugin)
  {
    this._attrPhisValueName = plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrMeasureName = plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrMeasureShortName = plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00005-306c-11d8-b4e9-00304f19f545")).ID;
    this._attrMeasureKoefficient = plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad00025-306c-11d8-b4e9-00304f19f545")).ID;
    this._relTypeId = plugin.Imdi.RelationTypes.GetByGuid(plugin.reltypeSimpleGuid).ID;
  }

  public override void Exam()
  {
    IPhysicalValues service1 = ServicesManager.GetService(typeof (IPhysicalValues)) as IPhysicalValues;
    IMeasures service2 = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    int tableRecordsCount = this.GetTableRecordsCount("MU");
    IDataReader sequentialDataReader = this.GetSequentialDataReader("MU");
    try
    {
      Dictionary<string, int> tableColumns = this.GetTableColumns(sequentialDataReader);
      int idxMuId = tableColumns["MU_ID"];
      int idxDimension = tableColumns["DIMENSION"];
      int idxMSystem = tableColumns["MSYSTEM"];
      int idxMuIdBase = tableColumns["MU_ID_BASE"];
      int idxMuName = tableColumns["MU_NAME"];
      int idxMuShortName = tableColumns["MU_SHORT_NAME"];
      int idxK = tableColumns["K"];
      string format = "Проверка данных о единицах измерения и физических величинах ({0} из {1})";
      Dictionary<string, List<PumpMeasures.SearchMeasureItem>> dictionary = new Dictionary<string, List<PumpMeasures.SearchMeasureItem>>();
      int index = 0;
      while (sequentialDataReader.Read())
      {
        ++index;
        this.ExamCheckPoint(string.Format(format, (object) index, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index, 2));
        PumpMeasures.SearchMeasureItem searchMeasureItem = new PumpMeasures.SearchMeasureItem(sequentialDataReader, idxMuId, idxDimension, idxMSystem, idxMuIdBase, idxMuName, idxMuShortName, idxK);
        IMeasureItem measure = service2.GetMeasure(searchMeasureItem.ShortName);
        if (measure == null)
        {
          if (service1.GetPhysicalValue(searchMeasureItem.Dimension) != null && searchMeasureItem.Koef == 1.0)
          {
            this.plugin.appManager.AddErrorMessage($"Ед.измерения \"{searchMeasureItem.Name}\" не может быть импортирована с коэффициентом 1, т.к. для физ.величины \"{searchMeasureItem.Dimension}\" уже существует ед. измерения с таким коэффициентом");
          }
          else
          {
            List<PumpMeasures.SearchMeasureItem> searchMeasureItemList;
            if (!dictionary.TryGetValue(searchMeasureItem.Dimension, out searchMeasureItemList))
            {
              searchMeasureItemList = new List<PumpMeasures.SearchMeasureItem>(1)
              {
                searchMeasureItem
              };
              dictionary.Add(searchMeasureItem.Dimension, searchMeasureItemList);
            }
            else
              searchMeasureItemList.Add(searchMeasureItem);
          }
        }
        else if (!measure.Koef.Equals(searchMeasureItem.Koef))
          this.plugin.appManager.AddErrorMessage($"Коэффициент импортируемой ед.измерения \"{searchMeasureItem.Name}\" отличается от коэффициента ед.измерения зарегистрированной в системе");
      }
      foreach (KeyValuePair<string, List<PumpMeasures.SearchMeasureItem>> keyValuePair in dictionary)
      {
        int num = 0;
        foreach (PumpMeasures.SearchMeasureItem searchMeasureItem in keyValuePair.Value)
        {
          if (searchMeasureItem.Koef == 1.0)
            ++num;
        }
        if (num == 0)
          this.plugin.appManager.AddErrorMessage($"Для импортируемой физической величины \"{keyValuePair.Key}\" не найдено ни одной базовой ед.измерения");
        else if (num > 1)
          this.plugin.appManager.AddErrorMessage($"Для импортируемой физической величины \"{keyValuePair.Key}\" не найдено более одной базовой ед.измерения");
      }
    }
    finally
    {
      sequentialDataReader.Close();
    }
    this.ExamCheckPoint("Проверка данных успешно завершена", 100);
  }

  public override void Pump()
  {
    int objType = -1;
    if (ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service1)
      objType = service1.ObjectTypes.GetByGuid(new Guid("cad00048-306c-11d8-b4e9-00304f19f545")).ID;
    this.PumpCheckPoint("Определение количества записей", 0);
    int tableRecordsCount = this.GetTableRecordsCount("MU");
    int index = 0;
    IDataReader sequentialDataReader = this.GetSequentialDataReader("MU");
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    try
    {
      this.PumpCheckPoint("Получение индексов полей", 1);
      Dictionary<string, int> tableColumns = this.GetTableColumns(sequentialDataReader);
      int idxMuId = tableColumns["MU_ID"];
      int idxDimension = tableColumns["DIMENSION"];
      int idxMSystem = tableColumns["MSYSTEM"];
      int idxMuIdBase = tableColumns["MU_ID_BASE"];
      int idxMuName = tableColumns["MU_NAME"];
      int idxMuShortName = tableColumns["MU_SHORT_NAME"];
      int idxK = tableColumns["K"];
      string format = "Закачка данных о единицах измерения и физических величинах ({0} из {1})";
      IImportedObjectList importedObjectList = this.plugin.Idw.CreateImportedObjectList(0);
      IPhysicalValues service2 = ServicesManager.GetService(typeof (IPhysicalValues)) as IPhysicalValues;
      IMeasures service3 = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
      while (sequentialDataReader.Read())
      {
        ++index;
        this.PumpCheckPoint(string.Format(format, (object) index, (object) tableRecordsCount), this.CalculatePercent(tableRecordsCount, index, 2));
        PumpMeasures.SearchMeasureItem searchMeasureItem = new PumpMeasures.SearchMeasureItem(sequentialDataReader, idxMuId, idxDimension, idxMSystem, idxMuIdBase, idxMuName, idxMuShortName, idxK);
        if (searchMeasureItem.Dimension.ToLower() == "штуки")
          searchMeasureItem.Dimension = "Количество";
        if (!searchMeasureItem.ShortName.Contains("##") && !service3.MeasureExists(searchMeasureItem.ShortName) && !searchMeasureItem.Dimension.Contains("##"))
        {
          if (!service2.PhysicalValueExists(searchMeasureItem.Dimension))
          {
            importedObjectList.AddObject(objType, 0, searchMeasureItem.Dimension);
            importedObjectList.AddAttributeStr(this._attrPhisValueName, searchMeasureItem.Dimension);
            AttributesHelper.AddObligatoryObjectAttributes(this.plugin.Idw.GetUserSession(), importedObjectList);
            importedObjectList.Import();
            if (importedObjectList.Items[0].Object.Object_id == 0L)
              throw new Exception($"Физическая величина \"{searchMeasureItem.Dimension}\" не импортирована. См. серверный лог");
            service2.AddPhysicalValue(importedObjectList.Items[0].Object.Object_id, searchMeasureItem.Dimension, (Guid) importedObjectList.Items[0].Object.ObjectGuid);
            importedObjectList.Items.Clear();
            ++num2;
          }
          long id = service2.GetPhysicalValue(searchMeasureItem.Dimension).Id;
          ++num1;
          if (service3.AddMeasure(searchMeasureItem.ShortName, searchMeasureItem.Name, searchMeasureItem.Koef, id) == 0L)
            throw new Exception($"Единица измерения \"{searchMeasureItem.ShortName}\" не импортирована. См. серверный лог");
          ++num4;
          ++num3;
        }
      }
    }
    finally
    {
      sequentialDataReader.Close();
    }
    this.plugin.appManager.AddInfoMessage($"Добавлено физических величин: {num2.ToString()} из {num1.ToString()}");
    this.plugin.appManager.AddInfoMessage($"Добавлено единиц измерения: {num4.ToString()} из {num3.ToString()}");
    this.PumpCheckPoint("Перекачка данных успешно завершена", 100);
  }

  private struct SearchMeasureItem
  {
    public long Id;
    public string Dimension;
    public string MSystem;
    public long IdBase;
    public string Name;
    public string ShortName;
    public double Koef;

    public SearchMeasureItem(
      IDataReader idr,
      int idxMuId,
      int idxDimension,
      int idxMSystem,
      int idxMuIdBase,
      int idxMuName,
      int idxMuShortName,
      int idxK)
    {
      this.Id = idr.IsDBNull(idxMuId) ? 0L : (long) BasePumpHelper.ToInt32(idr[idxMuId]);
      this.Dimension = idr.IsDBNull(idxDimension) ? string.Empty : idr.GetString(idxDimension).Trim();
      this.MSystem = idr.IsDBNull(idxMSystem) ? string.Empty : idr.GetString(idxMSystem).Trim();
      this.IdBase = idr.IsDBNull(idxMuIdBase) ? 0L : (long) BasePumpHelper.ToInt32(idr[idxMuIdBase]);
      this.Name = idr.IsDBNull(idxMuName) ? string.Empty : idr.GetString(idxMuName).Trim();
      this.ShortName = idr.IsDBNull(idxMuShortName) ? string.Empty : idr.GetString(idxMuShortName).Trim();
      this.Koef = idr.IsDBNull(idxK) ? 0.0 : BasePumpHelper.ToDouble(idr[idxK]);
    }
  }
}
