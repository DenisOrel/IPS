// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.EntitySetting
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[Serializable]
public class EntitySetting
{
  private EntMeasureProdSetting _prodSett;
  private EntityProperties _properties;
  private object _pumpTo;
  private EntityPumModes _pumpMode;
  private EntitySetting.AttributeBelongs _attrBelongs;
  private Guid _objectType = Guid.Empty;
  [NonSerialized]
  private int _objectTypeId = -1;
  protected internal Entity _entity;

  private void UpdateSettData()
  {
    this._objectTypeId = -1;
    if (this._entity == null)
      return;
    this._entity._pumpToAttrType = (IAttributeTypeItem) null;
  }

  private int GetObjectTypeId()
  {
    if (this._objectTypeId != -1 && this._objectTypeId != 0)
      return this._objectTypeId;
    IMetadataInfo imdi = TechcardConsts.Plugin.Imdi;
    if (imdi != null && !this._objectType.Equals(Guid.Empty))
    {
      IObjectTypeItem byGuid = imdi.ObjectTypes.GetByGuid(this._objectType);
      if (byGuid != null)
      {
        this._objectTypeId = byGuid.ID;
        return this._objectTypeId;
      }
    }
    return -1;
  }

  internal void InitializeProperties(Entity entity, IEnumerable<Entity> entCollection)
  {
    if (entity == null)
      return;
    this.Properties.Name = entity.Name;
    this.Properties.FieldType = EntityHelper.GetFieldTypesByType(entity.Type);
    this.Properties.ShortName = this.Properties.Alias = entity.Code;
    this.Properties.Status = EntityPumpStatus.None;
  }

  internal EntitySetting(Entity entity)
  {
    this._prodSett = new EntMeasureProdSetting(this);
    this._properties = new EntityProperties();
    this._entity = entity;
  }

  public void CopyData(EntitySetting source)
  {
    if (source == null)
      return;
    this.PumpMode = source.PumpMode;
    this.PumpTo = source.PumpTo;
    this._properties = source.Properties.Clone();
    this._objectType = source._objectType;
    this._attrBelongs = source._attrBelongs;
    this.JoinGroupId = source.JoinGroupId;
    this._prodSett = new EntMeasureProdSetting(this);
    this._prodSett.CopyData(source._prodSett);
    this.UpdateSettData();
  }

  public EntitySetting Clone()
  {
    EntitySetting entitySetting = new EntitySetting(this._entity);
    entitySetting.CopyData(this);
    return entitySetting;
  }

  internal Entity Entity
  {
    [DebuggerStepThrough] get => this._entity;
  }

  internal EntMeasureProdSetting MeasProdSettings
  {
    get
    {
      if (this._prodSett == null)
        this._prodSett = new EntMeasureProdSetting(this);
      return this._prodSett;
    }
    set
    {
      if (value == null)
        return;
      this._prodSett = value;
    }
  }

  public EntityProperties Properties
  {
    [DebuggerStepThrough] get => this._properties;
  }

  public EntityPumModes PumpMode
  {
    [DebuggerStepThrough] get => this._pumpMode;
    set => this._pumpMode = value;
  }

  public EntitySetting.AttributeBelongs AttributeBelong
  {
    [DebuggerStepThrough] get => this._attrBelongs;
    set => this._attrBelongs = value;
  }

  public object PumpTo
  {
    [DebuggerStepThrough] get => this._pumpTo;
    set
    {
      if (this._pumpTo == value)
        return;
      this._pumpTo = value;
      this.UpdateSettData();
    }
  }

  public Guid ObjectType
  {
    [DebuggerStepThrough] get => this._objectType;
    set
    {
      if (this._objectType == value)
        return;
      this._objectType = value;
      this.UpdateSettData();
    }
  }

  public int ObjectTypeID => this.GetObjectTypeId();

  public string JoinGroupId { get; set; } = string.Empty;

  public enum AttributeBelongs
  {
    [Description("Качать в объект")] ToObject,
    [Description("Качать в связь")] ToLink,
    [Description("Качать в связь и объект")] ToLinkAndObject,
  }
}
