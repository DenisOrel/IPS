// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.MeasuresImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData;

internal class MeasuresImpl : IMeasures
{
  private int measureTypeId;
  private Dictionary<long, MeasuresImpl.MeasureItem> measuresDict = new Dictionary<long, MeasuresImpl.MeasureItem>();
  private Dictionary<string, long> measuresShortNamesDict = new Dictionary<string, long>();
  private int attrMeasureName;
  private int attrMeasureShortName;
  private int attrMeasureKoef;
  private int attrMeasureDefault;
  private int reltypeSimple = -1;

  public bool MeasureExists(string shortName) => this.measuresShortNamesDict.ContainsKey(shortName);

  public bool MeasureExists(long measureObjId) => this.measuresDict.ContainsKey(measureObjId);

  public IMeasureItem GetMeasure(long measureObjId)
  {
    return !this.measuresDict.ContainsKey(measureObjId) ? (IMeasureItem) null : (IMeasureItem) this.measuresDict[measureObjId];
  }

  public IMeasureItem GetMeasure(string measureShortName)
  {
    return this.measuresShortNamesDict.ContainsKey(measureShortName) && this.measuresDict.ContainsKey(this.measuresShortNamesDict[measureShortName]) ? (IMeasureItem) this.measuresDict[this.measuresShortNamesDict[measureShortName]] : (IMeasureItem) null;
  }

  public long AddMeasure(string shortName, string longName, double koef, long physicalValueId)
  {
    long objId = 0;
    if (ServicesManager.GetService(typeof (IDataWriter)) is IDataWriter service1)
    {
      IPhysicalValues service = ServicesManager.GetService(typeof (IPhysicalValues)) as IPhysicalValues;
      IImportedObjectList importedObjectList = service1.CreateImportedObjectList(0);
      importedObjectList.AddObject(this.measureTypeId, 0, longName);
      importedObjectList.AddAttributeStr(this.attrMeasureName, longName);
      importedObjectList.AddAttributeStr(this.attrMeasureShortName, shortName);
      importedObjectList.AddAttributeDouble(this.attrMeasureKoef, koef);
      IPhysicalValueItem physicalValue = service.GetPhysicalValue(physicalValueId);
      bool flag = physicalValue.Measures == null || physicalValue.Measures.Count == 0;
      importedObjectList.AddAttributeInt(this.attrMeasureDefault, flag ? 1L : 0L);
      AttributesHelper.AddObligatoryObjectAttributes(service1.GetUserSession(), importedObjectList);
      importedObjectList.Import();
      Guid empty = Guid.Empty;
      if (importedObjectList.Items[0].Object.Object_id == 0L)
        throw new Exception($"Единица измерения \"{longName}\" не импортирована");
      IImportedRelationList importedRelationList = service1.CreateImportedRelationList(0);
      RelationRecord relationRecord = importedRelationList.AddRelation(physicalValueId, importedObjectList.Items[0].Object.Object_id, this.reltypeSimple);
      importedRelationList.Import();
      if (relationRecord.PrjLinkId == 0L || relationRecord.PrjLinkId == -1L)
        service1.AppManager.AddWarningMessage($"Связь между единицей измерения \"{longName}\" и физической величиной \"{physicalValue.Name}\" не импортирована");
      objId = importedObjectList.Items[0].Object.Object_id;
      Guid objectGuid = (Guid) importedObjectList.Items[0].Object.ObjectGuid;
      this.AddToCache(shortName, longName, koef, physicalValueId, objectGuid, objId, service, physicalValue.DefaultMeasureID);
    }
    return objId;
  }

  private void AddToCache(
    string shortName,
    string longName,
    double koef,
    long physicalValueId,
    Guid guid,
    long objId,
    IPhysicalValues physicalValues,
    long baseMeasureId)
  {
    MeasuresImpl.MeasureItem measureItem1 = new MeasuresImpl.MeasureItem(objId, shortName, longName, koef, physicalValueId, baseMeasureId, guid);
    if (!this.measuresDict.ContainsKey(objId))
      this.measuresDict.Add(objId, measureItem1);
    if (!this.measuresShortNamesDict.ContainsKey(shortName))
      this.measuresShortNamesDict.Add(shortName, objId);
    if (physicalValues == null)
      return;
    IPhysicalValueItem physicalValue = physicalValues.GetPhysicalValue(physicalValueId);
    if (physicalValue == null)
      return;
    if (physicalValue.Measures == null)
      physicalValue.Measures = new Dictionary<long, IMeasureItem>(1);
    IMeasureItem measureItem2 = (IMeasureItem) null;
    if (!physicalValue.Measures.TryGetValue(objId, out measureItem2))
      physicalValue.Measures.Add(objId, (IMeasureItem) measureItem1);
  }

  public void Reload()
  {
    if (!(ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service1))
      return;
    this.measureTypeId = service1.ObjectTypes.GetByGuid(new Guid("cad0000b-306c-11d8-b4e9-00304f19f545")).ID;
    this.attrMeasureName = service1.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID;
    this.attrMeasureShortName = service1.AttributeTypes.GetByGuid(new Guid("cad00005-306c-11d8-b4e9-00304f19f545")).ID;
    this.attrMeasureKoef = service1.AttributeTypes.GetByGuid(new Guid("cad00025-306c-11d8-b4e9-00304f19f545")).ID;
    this.attrMeasureDefault = service1.AttributeTypes.GetByGuid(new Guid("cad001a7-306c-11d8-b4e9-00304f19f545")).ID;
    this.reltypeSimple = service1.RelationTypes.GetByGuid(new Guid("cad00022-306c-11d8-b4e9-00304f19f545")).ID;
    IUserSession userSession = service1.UserSession;
    this.measuresDict.Clear();
    this.measuresShortNamesDict.Clear();
    if (!(ServicesManager.GetService(typeof (IPhysicalValues)) is IPhysicalValues service2) || userSession == null)
      return;
    IDBObjectCollection objectCollection = userSession.GetObjectCollection(this.measureTypeId);
    IPhysicalValueItem[] allPhysicalValues = service2.GetAllPhysicalValues();
    ConditionStructure conditionStructure = new ConditionStructure(0, RelationalOperators.EntersIn, (object) 0, LogicalOperators.NONE, 0, true);
    object[] columns = new object[6]
    {
      (object) -2,
      (object) this.attrMeasureDefault,
      (object) this.attrMeasureKoef,
      (object) this.attrMeasureShortName,
      (object) this.attrMeasureName,
      (object) -12
    };
    for (int index = 0; index < allPhysicalValues.Length; ++index)
    {
      IPhysicalValueItem physicalValueItem = allPhysicalValues[index];
      conditionStructure.Value = (object) physicalValueItem.Id;
      DataTable dataTable = objectCollection.Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        conditionStructure
      }, columns));
      DataRow[] dataRowArray = dataTable.Select($"[{dataTable.Columns[2].ColumnName}] = 1");
      physicalValueItem.DefaultMeasureID = dataRowArray != null && dataRowArray.Length != 0 ? Convert.ToInt64(dataRowArray[0][0]) : throw new Exception($"Для физической величины \"{physicalValueItem.Name}\" не указана базовая ед.измерения");
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        this.AddToCache(Convert.ToString(row[3]), Convert.ToString(row[4]), Convert.ToDouble(row[2]), physicalValueItem.Id, new Guid(Convert.ToString(row[5])), Convert.ToInt64(row[0]), service2, physicalValueItem.DefaultMeasureID);
    }
  }

  private class MeasureItem : IMeasureItem
  {
    private long id;
    private Guid Guid;
    private string shortName;
    private string longName;
    private double koef;
    private long physicalValueId = -1;
    private long baseMeasureID;

    public long Id => this.id;

    public Guid GUID => this.Guid;

    public string ShortName => this.shortName;

    public string LongName => this.longName;

    public double Koef => this.koef;

    public long PhysicalValueID => this.physicalValueId;

    public MeasureItem(
      long objId,
      string shortName,
      string longName,
      double koef,
      long physValID,
      long baseMeasureId,
      Guid guid)
    {
      this.id = objId;
      this.shortName = shortName;
      this.longName = longName;
      this.koef = koef;
      this.physicalValueId = physValID;
      this.Guid = guid;
      this.baseMeasureID = baseMeasureId;
    }

    public long BaseMeasureId => this.baseMeasureID;
  }
}
