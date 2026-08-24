// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.PhysicalValuesImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData;

internal class PhysicalValuesImpl : IPhysicalValues
{
  private int physValueTypeId;
  private int attrPhisValueName;
  private Dictionary<long, IPhysicalValueItem> physicalValues = new Dictionary<long, IPhysicalValueItem>();
  private Dictionary<string, long> physicalValueNames = new Dictionary<string, long>();

  public IPhysicalValueItem[] GetAllPhysicalValues()
  {
    List<IPhysicalValueItem> physicalValueItemList = new List<IPhysicalValueItem>();
    foreach (IPhysicalValueItem physicalValueItem in this.physicalValues.Values)
      physicalValueItemList.Add(physicalValueItem);
    return physicalValueItemList.ToArray();
  }

  public bool PhysicalValueExists(string physiclValueName)
  {
    return this.physicalValueNames.ContainsKey(physiclValueName);
  }

  public IPhysicalValueItem GetPhysicalValue(long objectId)
  {
    return this.physicalValues.ContainsKey(objectId) ? this.physicalValues[objectId] : (IPhysicalValueItem) null;
  }

  public IPhysicalValueItem GetPhysicalValue(string physValName)
  {
    return this.PhysicalValueExists(physValName) ? this.physicalValues[this.physicalValueNames[physValName]] : (IPhysicalValueItem) null;
  }

  public IPhysicalValueItem GetPhysicalValueByBaseId(long baseObjectId)
  {
    return this.physicalValues.ContainsKey(baseObjectId) ? this.physicalValues[baseObjectId] : (IPhysicalValueItem) null;
  }

  public void AddPhysicalValue(long objID, string name, Guid guid)
  {
    this.physicalValues.Add(objID, (IPhysicalValueItem) new PhysicalValuesImpl.PhysicalValueItem(objID, name));
    this.physicalValueNames.Add(name, objID);
  }

  public void Reload()
  {
    if (!(ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service))
      return;
    this.physValueTypeId = service.ObjectTypes.GetByGuid(new Guid("cad00048-306c-11d8-b4e9-00304f19f545")).ID;
    this.attrPhisValueName = service.AttributeTypes.GetByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).ID;
    IUserSession userSession = service.UserSession;
    IDBObjectCollection objectCollection = userSession.GetObjectCollection(this.physValueTypeId);
    DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[3]
    {
      (object) ObligatoryObjectAttributes.F_OBJECT_ID,
      (object) ObligatoryObjectAttributes.F_GUID,
      (object) this.attrPhisValueName
    });
    ObligatoryObjectAttributesHelper.GetCaption(ObligatoryObjectAttributes.F_OBJECT_ID);
    ObligatoryObjectAttributesHelper.GetCaption(ObligatoryObjectAttributes.F_GUID);
    string name = userSession.GetAttributeType(this.attrPhisValueName).Name;
    this.physicalValues.Clear();
    this.physicalValueNames.Clear();
    try
    {
      foreach (DataRow row in (InternalDataCollectionBase) objectCollection.Select(paramSet).Rows)
        this.AddPhysicalValue(Convert.ToInt64(row[0]), Convert.ToString(row[name]).Trim(), new Guid(Convert.ToString(row[1]).Trim()));
    }
    catch (Exception ex)
    {
    }
  }

  private class PhysicalValueItem : IPhysicalValueItem
  {
    private long id;
    private string name;
    private Dictionary<long, IMeasureItem> measures;
    internal long defaultMeasureID;

    public long Id => this.id;

    public string Name => this.name;

    public Dictionary<long, IMeasureItem> Measures
    {
      get => this.measures;
      set => this.measures = value;
    }

    public long DefaultMeasureID
    {
      get => this.defaultMeasureID;
      set
      {
        if (this.defaultMeasureID == value)
          return;
        this.defaultMeasureID = value;
      }
    }

    public PhysicalValueItem(long objId, string name)
    {
      this.id = objId;
      this.name = name;
      this.measures = new Dictionary<long, IMeasureItem>();
    }
  }
}
