// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.EntMeasureProdSetting
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[Serializable]
internal class EntMeasureProdSetting : ICustomTypeDescriptor, ICloneable
{
  private Dictionary<long, List<int>> _measure2ProdList = new Dictionary<long, List<int>>();
  private long _phisicValueID = -1;
  private string _entityWithMeasure;
  protected EntitySetting _entSett;

  private void AddMeasure4Production(long measureId, int productId)
  {
    List<int> intList;
    if (!this._measure2ProdList.TryGetValue(measureId, out intList) || intList == null)
    {
      intList = new List<int>();
      this._measure2ProdList[measureId] = intList;
    }
    if (intList.Contains(productId))
      return;
    intList.Add(productId);
  }

  private long GetMeasure4Production(int productId)
  {
    int[] productionIds = this.ProductionIDs;
    if (productionIds == null || productionIds.Length == 0)
      return this.GetMeasure();
    if (this._measure2ProdList.Count == 1)
    {
      KeyValuePair<long, List<int>> keyValuePair = this._measure2ProdList.First<KeyValuePair<long, List<int>>>();
      if (keyValuePair.Value.Count == 0)
        return keyValuePair.Key;
    }
    foreach (KeyValuePair<long, List<int>> measure2Prod in this._measure2ProdList)
    {
      if (measure2Prod.Value.Contains(productId) || (productId == -1 || productId == 0) && measure2Prod.Value.Count == 0)
        return measure2Prod.Key;
    }
    return -1;
  }

  internal void CopyData(EntMeasureProdSetting source)
  {
    if (source == null)
      return;
    this._phisicValueID = source._phisicValueID;
    this._entityWithMeasure = source._entityWithMeasure;
    this._measure2ProdList.Clear();
    foreach (KeyValuePair<long, List<int>> measure2Prod in source._measure2ProdList)
    {
      List<int> intList = measure2Prod.Value != null ? new List<int>((IEnumerable<int>) measure2Prod.Value) : new List<int>();
      this._measure2ProdList.Add(measure2Prod.Key, intList);
    }
  }

  public EntMeasureProdSetting(EntitySetting entitySetting) => this._entSett = entitySetting;

  public long GetMeasure()
  {
    foreach (long key in this._measure2ProdList.Keys)
    {
      if (key != -1L)
        return key;
    }
    return -1;
  }

  public void SetMeasure(long measureId)
  {
    this._measure2ProdList.Clear();
    this._measure2ProdList.Add(measureId, new List<int>());
  }

  public bool isMeasureSet()
  {
    if (this._measure2ProdList.Count == 0)
      return false;
    foreach (long key in this._measure2ProdList.Keys)
    {
      if (key == -1L)
        return false;
    }
    return true;
  }

  public PropertyDescriptorCollection GetProperties() => this.GetProperties(new Attribute[0]);

  public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
  {
    PropertyDescriptorCollection properties1 = TypeDescriptor.GetProperties((object) this, attributes, true);
    PropertyDescriptorCollection properties2 = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
    PropertyDescriptor descr = properties1["Measure2ProdList"];
    foreach (MeasureDescriptor oldValue in EntityDescriptor.GetMeasureDescriptorsByPhisicalValueId(this._phisicValueID))
    {
      lPropertyDescriptor propertyDescriptor = new lPropertyDescriptor(descr, (object) oldValue);
      propertyDescriptor.SetDisplayName(oldValue.LongName);
      properties2.Add((PropertyDescriptor) propertyDescriptor);
    }
    return properties2;
  }

  public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes((object) this, true);

  public object GetPropertyOwner(PropertyDescriptor pd) => (object) this;

  public string GetClassName() => TypeDescriptor.GetClassName((object) this, true);

  public string GetComponentName() => TypeDescriptor.GetComponentName((object) this, true);

  public TypeConverter GetConverter() => TypeDescriptor.GetConverter((object) this, true);

  public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent((object) this, true);

  public PropertyDescriptor GetDefaultProperty()
  {
    return TypeDescriptor.GetDefaultProperty((object) this, true);
  }

  public object GetEditor(Type editorBaseType)
  {
    return TypeDescriptor.GetEditor((object) this, editorBaseType, true);
  }

  public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents((object) this, true);

  public EventDescriptorCollection GetEvents(Attribute[] attributes)
  {
    return TypeDescriptor.GetEvents((object) this, attributes, true);
  }

  [Editor(typeof (Measure2ProductionListDropDownEditor), typeof (UITypeEditor))]
  [TypeConverter(typeof (Measure2ProductionListConverter))]
  public Dictionary<long, List<int>> Measure2ProdList
  {
    get => this._measure2ProdList;
    set => this._measure2ProdList = value;
  }

  public int[] ProductionIDs
  {
    get
    {
      return this._entSett == null || this._entSett.Entity == null ? (int[]) null : this._entSett.Entity.Productions;
    }
  }

  public long PhysicalValueId
  {
    get => this._phisicValueID;
    set => this._phisicValueID = value;
  }

  public long this[int prodId]
  {
    get => this.GetMeasure4Production(prodId);
    set => this.AddMeasure4Production(value, prodId);
  }

  public string EntityWithMeasure
  {
    get => this._entityWithMeasure;
    set => this._entityWithMeasure = value;
  }

  public object Clone()
  {
    EntMeasureProdSetting measureProdSetting = new EntMeasureProdSetting(this._entSett);
    measureProdSetting.CopyData(this);
    return (object) measureProdSetting;
  }
}
