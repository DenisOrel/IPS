// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntitySettingsIpsAttributes
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal class EntitySettingsIpsAttributes
{
  public void Setup(IEnumerable<Entity> entities)
  {
    if (entities == null)
      return;
    foreach (Entity entity in entities)
    {
      if (!entity.LockedSettings)
      {
        EntityProperties properties = entity.Settings.Properties;
        if (properties.Status == EntityPumpStatus.None && properties.Status != EntityPumpStatus.NotPump && !EntitySettingsIpsAttributes.LookupIpsAttribute(entity, entities))
          properties.Status = EntityHelper.CheckEntitySett(entities, entity) != EntityExistsStatus.None ? EntityPumpStatus.None : EntityPumpStatus.New;
      }
    }
  }

  public static bool LookupIpsAttribute(Entity entity, IEnumerable<Entity> entities)
  {
    if (entities == null)
      return false;
    EntitySetting settings = entity.Settings;
    EntityProperties properties = settings?.Properties;
    if (properties == null)
      return false;
    IAttributeTypeItem attrType = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByAlias(properties.Alias);
    if (attrType == null && !string.IsNullOrEmpty(entity.Code))
      attrType = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByName(entity.Code);
    if (attrType == null && !string.IsNullOrEmpty(properties.Name))
      attrType = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByName(properties.Name) ?? TechcardConsts.Plugin.Imdi.AttributeTypes.GetByName(properties.Name.Trim());
    if (attrType == null)
      return false;
    if (((FieldTypes) attrType.AttrValueType == properties.FieldType || properties.FieldType == FieldTypes.ftDouble && attrType.AttrValueType == 13) && attrType.MultiValueMode == properties.MultipleValued)
    {
      if (entities.Any<Entity>((Func<Entity, bool>) (otherEntity =>
      {
        if (otherEntity.RecordID != entity.RecordID || !(otherEntity.Settings.PumpTo is Guid) || !((Guid) otherEntity.Settings.PumpTo == attrType.GUID) || otherEntity.Settings.PumpMode != EntityPumModes.ExistAttr)
          return false;
        return otherEntity.Settings.Properties.Status == EntityPumpStatus.Commited || otherEntity.Settings.Properties.Status == EntityPumpStatus.Exists;
      })))
      {
        properties.Name = $"{properties.Name}_{properties.Alias}";
        properties.Status = EntityPumpStatus.None;
        return true;
      }
      settings.PumpMode = EntityPumModes.ExistAttr;
      settings.PumpTo = (object) attrType.GUID;
      properties.Status = EntityPumpStatus.Commited;
      return true;
    }
    properties.Status = EntityPumpStatus.None;
    return true;
  }
}
