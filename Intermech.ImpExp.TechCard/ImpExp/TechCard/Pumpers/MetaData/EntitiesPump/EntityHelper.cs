// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntityHelper
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal static class EntityHelper
{
  private static bool GetAttributeGuid(
    Entity entity,
    ICollection<string> entityCodes,
    out Guid attributeGuid)
  {
    attributeGuid = Guid.Empty;
    if (entity == null)
      return false;
    if (entity.Settings.PumpTo == null)
      return true;
    if (entity.Settings.PumpTo is Entity pumpTo)
    {
      if (entityCodes.Contains(pumpTo.Code))
      {
        string Message = $"Зацикливание понятий! Стек понятий: {entity.Code}, {string.Join(", ", entityCodes.ToArray<string>())}";
        TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
        return false;
      }
      entityCodes.Add(pumpTo.Code);
      return EntityHelper.GetAttributeGuid(pumpTo, entityCodes, out attributeGuid);
    }
    attributeGuid = (Guid) entity.Settings.PumpTo;
    return true;
  }

  public static bool GetAttributeGuid(Entity entity, out Guid attributeGuid)
  {
    return EntityHelper.GetAttributeGuid(entity, (ICollection<string>) new List<string>(), out attributeGuid);
  }

  public static FieldTypes GetFieldTypesByType(string entType)
  {
    switch (entType)
    {
      case "1":
      case "2":
      case "3":
      case "E":
      case "S":
        return FieldTypes.ftString;
      case "4":
      case "I":
      case "P":
        return FieldTypes.ftInteger;
      case "B":
        return FieldTypes.ftBoolean;
      case "D":
        return FieldTypes.ftDateTime;
      case "K":
        return FieldTypes.ftMemo;
      case "R":
        return FieldTypes.ftDouble;
      default:
        return FieldTypes.ftUnknown;
    }
  }

  public static List<FieldTypes> GetPosibleTypes(Entity entity)
  {
    List<FieldTypes> posibleTypes = new List<FieldTypes>();
    if (entity == null || entity.Settings == null || entity.Settings.Properties == null)
      return posibleTypes;
    if (entity.IsMasterAttr && entity.Settings.Properties.FieldType == FieldTypes.ftObjectLink)
    {
      posibleTypes.Add(FieldTypes.ftObjectLink);
      return posibleTypes;
    }
    FieldTypes fieldTypesByType = EntityHelper.GetFieldTypesByType(entity.Type);
    posibleTypes.Add(fieldTypesByType);
    if (fieldTypesByType != FieldTypes.ftString)
      posibleTypes.Add(FieldTypes.ftString);
    switch (fieldTypesByType - 1)
    {
      case FieldTypes.ftUnknown:
        if (!posibleTypes.Contains(FieldTypes.ftMemo))
          posibleTypes.Add(FieldTypes.ftMemo);
        if (!posibleTypes.Contains(FieldTypes.ftString))
          posibleTypes.Add(FieldTypes.ftString);
        if (!posibleTypes.Contains(FieldTypes.ftBoolean))
          posibleTypes.Add(FieldTypes.ftBoolean);
        if (!posibleTypes.Contains(FieldTypes.ftDateTime))
          posibleTypes.Add(FieldTypes.ftDateTime);
        if (!posibleTypes.Contains(FieldTypes.ftDouble))
          posibleTypes.Add(FieldTypes.ftDouble);
        if (!posibleTypes.Contains(FieldTypes.ftInteger))
          posibleTypes.Add(FieldTypes.ftInteger);
        if (!posibleTypes.Contains(FieldTypes.ftMeasured))
          posibleTypes.Add(FieldTypes.ftMeasured);
        if (!posibleTypes.Contains(FieldTypes.ftPassword))
          posibleTypes.Add(FieldTypes.ftPassword);
        if (!posibleTypes.Contains(FieldTypes.ftObjectLink))
        {
          posibleTypes.Add(FieldTypes.ftObjectLink);
          goto case FieldTypes.ftDouble;
        }
        goto case FieldTypes.ftDouble;
      case FieldTypes.ftString:
        if (!posibleTypes.Contains(FieldTypes.ftDouble))
          posibleTypes.Add(FieldTypes.ftDouble);
        if (!posibleTypes.Contains(FieldTypes.ftMeasured))
        {
          posibleTypes.Add(FieldTypes.ftMeasured);
          goto case FieldTypes.ftDouble;
        }
        goto case FieldTypes.ftDouble;
      case FieldTypes.ftInteger:
        if (!posibleTypes.Contains(FieldTypes.ftDateTime))
          posibleTypes.Add(FieldTypes.ftDateTime);
        if (!posibleTypes.Contains(FieldTypes.ftMeasured))
          posibleTypes.Add(FieldTypes.ftMeasured);
        if (!posibleTypes.Contains(FieldTypes.ftInteger))
        {
          posibleTypes.Add(FieldTypes.ftInteger);
          goto case FieldTypes.ftDouble;
        }
        goto case FieldTypes.ftDouble;
      case FieldTypes.ftDouble:
      case FieldTypes.ftBlob:
        return posibleTypes;
      case FieldTypes.ftObjectLink:
        if (!posibleTypes.Contains(FieldTypes.ftString))
        {
          posibleTypes.Add(FieldTypes.ftString);
          goto case FieldTypes.ftDouble;
        }
        goto case FieldTypes.ftDouble;
      case FieldTypes.ftPassword:
        if (!posibleTypes.Contains(FieldTypes.ftString))
        {
          posibleTypes.Add(FieldTypes.ftString);
          goto case FieldTypes.ftDouble;
        }
        goto case FieldTypes.ftDouble;
      case FieldTypes.ftBoolean:
        if (!posibleTypes.Contains(FieldTypes.ftInteger))
        {
          posibleTypes.Add(FieldTypes.ftInteger);
          goto case FieldTypes.ftDouble;
        }
        goto case FieldTypes.ftDouble;
      default:
        string Message = "Метод Entity.GetType() не умеет обрабатывать " + fieldTypesByType.ToString();
        TechcardConsts.Plugin.appManager.AddErrorMessage(Message);
        goto case FieldTypes.ftDouble;
    }
  }

  public static List<Entity> ParseHashtable(Dictionary<DataRow, Entity> table)
  {
    List<Entity> hashtable = new List<Entity>(table != null ? table.Count : 0);
    if (table == null)
      return hashtable;
    hashtable.AddRange((IEnumerable<Entity>) table.Values);
    return hashtable;
  }

  public static EntityExistsStatus CheckEntitySett(
    IEnumerable<Entity> entCollection,
    Entity orgEntity)
  {
    if (entCollection == null || orgEntity == null)
      return EntityExistsStatus.None;
    foreach (Entity ent in entCollection)
    {
      if (ent.Settings.PumpMode == EntityPumModes.NewAttr && !object.Equals((object) orgEntity, (object) ent))
      {
        if (string.Equals(ent.Settings.Properties.Name, orgEntity.Settings.Properties.Name, StringComparison.CurrentCultureIgnoreCase))
          return EntityExistsStatus.ByName;
        if (!string.Equals(ent.Settings.Properties.ShortName, string.Empty) && string.Equals(ent.Settings.Properties.ShortName, orgEntity.Settings.Properties.ShortName))
          return EntityExistsStatus.ByShortName;
        if (!string.Equals(ent.Settings.Properties.Alias, string.Empty) && string.Equals(ent.Settings.Properties.Alias, orgEntity.Settings.Properties.Alias))
          return EntityExistsStatus.ByAlias;
      }
    }
    return EntityExistsStatus.None;
  }

  public static bool CheckEntitySett_SelfRef(
    Entity entity,
    List<Entity> refEntities,
    out Entity failEntity,
    out bool byCycle)
  {
    failEntity = (Entity) null;
    byCycle = false;
    if (entity == null || entity.Settings == null)
      return false;
    switch (entity.Settings.PumpMode)
    {
      case EntityPumModes.NewAttr:
        failEntity = entity.Settings.Properties.Status.Equals((object) EntityPumpStatus.None) || entity.Settings.Properties.Status.Equals((object) EntityPumpStatus.NotPump) ? entity : (Entity) null;
        break;
      case EntityPumModes.ExistAttr:
        Guid pumpTo1 = (Guid) entity.Settings.PumpTo;
        failEntity = pumpTo1 == Guid.Empty ? entity : (Entity) null;
        break;
      case EntityPumModes.ExistEntity:
        if (!(entity.Settings.PumpTo is Entity pumpTo2))
        {
          failEntity = entity;
          break;
        }
        if (object.Equals((object) entity, (object) pumpTo2) || refEntities.Contains(pumpTo2))
        {
          failEntity = entity;
          byCycle = true;
          return false;
        }
        refEntities.Add(entity);
        return EntityHelper.CheckEntitySett_SelfRef(pumpTo2, refEntities, out failEntity, out byCycle);
    }
    return failEntity == null;
  }

  public static List<Entity> GetEntitySett_RefList(List<Entity> entityList, Entity entity)
  {
    List<Entity> entitySettRefList = new List<Entity>();
    if (entity == null || entityList == null)
      return entitySettRefList;
    foreach (Entity entity1 in entityList)
    {
      if (entity1 != null && entity1.Settings != null && entity1.Settings.PumpMode == EntityPumModes.ExistEntity && object.Equals((object) (entity1.Settings.PumpTo as Entity), (object) entity) && !entitySettRefList.Contains(entity1))
      {
        entitySettRefList.Add(entity1);
        entitySettRefList.AddRange((IEnumerable<Entity>) EntityHelper.GetEntitySett_RefList(entityList, entity1));
      }
    }
    return entitySettRefList;
  }

  private static void GetEntitySett_RefUpList(Entity entity, ref List<Entity> entityList)
  {
    if (entity == null || entityList == null || entityList.Contains(entity))
      return;
    entityList.Insert(0, entity);
    if (entity.Settings.PumpMode != EntityPumModes.ExistEntity || !(entity.Settings.PumpTo is Entity pumpTo) || pumpTo.Equals((object) entity))
      return;
    EntityHelper.GetEntitySett_RefUpList(pumpTo, ref entityList);
  }

  public static List<Entity> GetEntitySett_RefUpList(Entity entity)
  {
    List<Entity> entityList = new List<Entity>(1);
    EntityHelper.GetEntitySett_RefUpList(entity, ref entityList);
    return entityList;
  }
}
