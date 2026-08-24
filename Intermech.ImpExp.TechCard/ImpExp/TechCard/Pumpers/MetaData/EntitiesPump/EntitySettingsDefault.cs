// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntitySettingsDefault
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal class EntitySettingsDefault
{
  private const long TimeMeasureObjectId = 2374;
  private const int ST_TP = 14;
  private const int ST_OPER = 4;
  private const int ST_STEP = 6;
  private const int ST_ADDSTEP = 39;
  private static readonly IDictionary<string, Entity> _defaultSettings = (IDictionary<string, Entity>) new Dictionary<string, Entity>();

  private static void ReadEntitySettings(IReadOnlyList<Entity> entitiesToConfig)
  {
    EntitySettingsDefault._defaultSettings.Clear();
    EntitySettingsDefault.SetDefaultConfigToNormScenEntities(entitiesToConfig);
    Entity entity1 = new Entity("%ZAG", string.Empty)
    {
      Settings = {
        PumpTo = (object) new Guid("cad005e3-306c-11d8-b4e9-00304f19f545"),
        PumpMode = EntityPumModes.ExistAttr,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftObjectLink
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity1.Code] = entity1;
    Entity entity2 = new Entity("%МТР", string.Empty)
    {
      Settings = {
        PumpTo = (object) new Guid("cad0038c-306c-11d8-b4e9-00304f19f545"),
        PumpMode = EntityPumModes.ExistAttr,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftObjectLink
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity2.Code] = entity2;
    Entity entity3 = new Entity("N_ОП", string.Empty)
    {
      Settings = {
        PumpTo = (object) TechCardConsts.AttributeTypes.ObjectNumAttrGuid,
        PumpMode = EntityPumModes.ExistAttr,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftString
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity3.Code] = entity3;
    Entity entity4 = new Entity("Nпер", string.Empty)
    {
      Settings = {
        PumpTo = (object) TechCardConsts.AttributeTypes.ObjectNumAttrGuid,
        PumpMode = EntityPumModes.ExistAttr,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftString
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity4.Code] = entity4;
    Entity entity5 = new Entity("%wn", string.Empty)
    {
      Settings = {
        PumpTo = (object) TechCardConsts.AttributeTypes.ObjectNumAttrGuid,
        PumpMode = EntityPumModes.ExistAttr,
        AttributeBelong = EntitySetting.AttributeBelongs.ToLink,
        Properties = {
          FieldType = FieldTypes.ftString
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity5.Code] = entity5;
    Entity entity6 = new Entity("Тшт", string.Empty)
    {
      Settings = {
        PumpTo = (object) new Guid("CAD005E5-306C-11D8-B4E9-00304F19F545"),
        PumpMode = EntityPumModes.ExistAttr,
        ObjectType = TechCardConsts.ObjectTypes.OperationNormGUID,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftMeasured
        },
        MeasProdSettings = {
          PhysicalValueId = 2374
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity6.Code] = entity6;
    Entity entity7 = new Entity("КТшт", string.Empty)
    {
      Settings = {
        PumpTo = (object) new Guid("CAD01469-306C-11D8-B4E9-00304F19F545"),
        PumpMode = EntityPumModes.ExistAttr,
        ObjectType = TechCardConsts.ObjectTypes.OperationNormGUID,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftDouble
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity7.Code] = entity7;
    Entity entity8 = new Entity("Тпз", string.Empty)
    {
      Settings = {
        PumpTo = (object) new Guid("CAD005DA-306C-11D8-B4E9-00304F19F545"),
        PumpMode = EntityPumModes.ExistAttr,
        ObjectType = TechCardConsts.ObjectTypes.OperationNormGUID,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftMeasured
        },
        MeasProdSettings = {
          PhysicalValueId = 2374
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity8.Code] = entity8;
    Entity entity9 = new Entity("То", string.Empty)
    {
      Settings = {
        PumpTo = (object) new Guid("cad005e4-306c-11d8-b4e9-00304f19f545"),
        PumpMode = EntityPumModes.ExistAttr,
        ObjectType = TechCardConsts.ObjectTypes.OperationNormGUID,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftMeasured
        },
        MeasProdSettings = {
          PhysicalValueId = 2374
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity9.Code] = entity9;
    Entity entity10 = new Entity("Тв", string.Empty)
    {
      Settings = {
        PumpTo = (object) new Guid("cad005e0-306c-11d8-b4e9-00304f19f545"),
        PumpMode = EntityPumModes.ExistAttr,
        ObjectType = TechCardConsts.ObjectTypes.OperationNormGUID,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftMeasured
        },
        MeasProdSettings = {
          PhysicalValueId = 2374
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity10.Code] = entity10;
    Entity entity11 = new Entity("То_п", string.Empty)
    {
      Settings = {
        PumpTo = (object) new Guid("cad005e4-306c-11d8-b4e9-00304f19f545"),
        PumpMode = EntityPumModes.ExistAttr,
        ObjectType = TechCardConsts.ObjectTypes.PerehodNormGUID,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftMeasured
        },
        MeasProdSettings = {
          PhysicalValueId = 2374
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity11.Code] = entity11;
    Entity entity12 = new Entity("Тв_п", string.Empty)
    {
      Settings = {
        PumpTo = (object) new Guid("cad005e0-306c-11d8-b4e9-00304f19f545"),
        PumpMode = EntityPumModes.ExistAttr,
        ObjectType = TechCardConsts.ObjectTypes.PerehodNormGUID,
        AttributeBelong = EntitySetting.AttributeBelongs.ToObject,
        Properties = {
          FieldType = FieldTypes.ftMeasured
        },
        MeasProdSettings = {
          PhysicalValueId = 2374
        }
      },
      IsPermisibleAttr2TypeObj = true
    };
    EntitySettingsDefault._defaultSettings[entity12.Code] = entity12;
  }

  private static void SetDefaultSettings(Entity entity)
  {
    Entity entity1;
    if (!EntitySettingsDefault._defaultSettings.TryGetValue(entity.Code, out entity1))
      return;
    entity.Settings.Properties.Status = EntityPumpStatus.Commited;
    entity.Settings.PumpTo = entity1.Settings.PumpTo;
    entity.Settings.PumpMode = entity1.Settings.PumpMode;
    entity.Settings.ObjectType = entity1.Settings.ObjectType;
    entity.Settings.AttributeBelong = entity1.Settings.AttributeBelong;
    entity.Settings.Properties.FieldType = entity1.Settings.Properties.FieldType;
    entity.Settings.MeasProdSettings.PhysicalValueId = entity1.Settings.MeasProdSettings.PhysicalValueId;
    entity.Settings.MeasProdSettings.EntityWithMeasure = entity1.Settings.MeasProdSettings.EntityWithMeasure;
    entity.IsPermisibleAttr2TypeObj = entity1.IsPermisibleAttr2TypeObj;
  }

  private static void SetObjectLinkSettings(Entity entity)
  {
    if (!entity.IsMasterAttr || entity.EntityReference == null)
      return;
    entity.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
  }

  private void SetUniqueRecordSettings(Entity entity)
  {
    switch ((TechcardConsts.TpRecordType) entity.RecordID)
    {
      case TechcardConsts.TpRecordType.Oborud:
      case TechcardConsts.TpRecordType.Personal:
      case TechcardConsts.TpRecordType.MaterialAdd:
      case TechcardConsts.TpRecordType.Tool:
        if (entity.EntityReference != null && entity.EntityReference.Reference != 0)
          break;
        entity.Settings.AttributeBelong = EntitySetting.AttributeBelongs.ToLink;
        break;
    }
  }

  private void SetNotPumpSettings(Entity entity)
  {
    if (entity.Name.Contains("для ведомости"))
    {
      entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
    }
    else
    {
      if (entity.RecordID == 0 && entity.Code.StartsWith("R"))
      {
        string upper = entity.Code.ToUpper();
        if (string.CompareOrdinal(upper, "R2AA") > 0 && string.CompareOrdinal(upper, "RZZZ") < 0)
        {
          entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
          return;
        }
      }
      if (entity.RecordID == 25)
        entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
      else if (entity.RecordID == 6)
        entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
      else if (entity.Code == "ККТП" || entity.Code == "кктп")
        entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
      else if (entity.RecordID == 17 && string.CompareOrdinal(entity.Code, "Р00") > 0 && string.CompareOrdinal(entity.Code, "Р99") < 0)
        entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
      else if (entity.Code.StartsWith("#ch"))
        entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
      else if (entity.Code.StartsWith("%J"))
        entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
      else if (entity.Code.StartsWith("#R") && entity.Name.StartsWith("Дата"))
      {
        entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
      }
      else
      {
        if (!entity.Code.StartsWith("###"))
          return;
        entity.Settings.Properties.Status = EntityPumpStatus.NotPump;
      }
    }
  }

  public void Setup(IEnumerable<Entity> entities)
  {
    if (entities == null)
      return;
    List<Entity> list = entities.ToList<Entity>();
    EntitySettingsDefault.ReadEntitySettings((IReadOnlyList<Entity>) list);
    foreach (Entity entity in list)
    {
      if (!entity.LockedSettings && entity.Settings.Properties.Status == EntityPumpStatus.None)
      {
        EntitySettingsDefault.SetDefaultSettings(entity);
        EntitySettingsDefault.SetObjectLinkSettings(entity);
        this.SetUniqueRecordSettings(entity);
        this.SetNotPumpSettings(entity);
      }
    }
  }

  private static void SetDefaultConfigToNormScenEntities(IReadOnlyList<Entity> entities)
  {
    EntitySettingsDefault.SetDefaultConfigToTPNormScenEntities(entities);
    EntitySettingsDefault.SetDefaultConfigToOperNormScenEntities(entities);
    EntitySettingsDefault.SetDefaultConfigToStepNormScenEntities(entities);
    EntitySettingsDefault.SetDefaultConfigToAddStepNormScenEntities(entities);
  }

  private static Guid ScenTypeToIpsObjType(int scenType)
  {
    switch (scenType)
    {
      case 4:
        return TechCardConsts.ObjectTypes.OperationNormGUID;
      case 6:
        return TechCardConsts.ObjectTypes.PerehodNormGUID;
      case 14:
        return TechCardConsts.ObjectTypes.TechProcNormGUID;
      case 39:
        return TechCardConsts.ObjectTypes.DopPriemNormGUID;
      default:
        return Guid.Empty;
    }
  }

  private static TechcardConsts.TpRecordType ScenTypeToRecordTypeId(int scenType)
  {
    switch (scenType)
    {
      case 4:
        return TechcardConsts.TpRecordType.Oper;
      case 6:
        return TechcardConsts.TpRecordType.Perehod;
      case 14:
        return TechcardConsts.TpRecordType.GenaralInfo;
      case 39:
        return TechcardConsts.TpRecordType.DopPriem;
      default:
        return TechcardConsts.TpRecordType.Unknown;
    }
  }

  private static void SetDefaultConfigToScenEntities(
    int scenType,
    IReadOnlyList<Entity> entitiesToConfig)
  {
    string str1 = "pScenType";
    string str2 = TechcardConsts.Plugin.idb.DataBaseType == "IntermechConnection.MsSQL" ? "@" + str1 : ":" + str1;
    string str3 = "pRecordTypeId";
    string str4 = $"select distinct scen.F_CODE, ENT.F_NAME from TC_SCCELLS scen, TC_ENTITY ENT where scen.F_CODE = ENT.F_CODE and ENT.F_RECORDID = {(TechcardConsts.Plugin.idb.DataBaseType == "IntermechConnection.MsSQL" ? "@" + str3 : ":" + str3)} and scen.F_SCEN in(select F_KEY from TC_SCRIPTS where F_KIND = {str2}) order by scen.F_CODE";
    IDbCommand command = TechcardConsts.Plugin.idb.CreateCommand();
    command.CommandText = str4;
    IDbDataParameter parameter1 = command.CreateParameter();
    parameter1.ParameterName = str1;
    parameter1.Direction = ParameterDirection.Input;
    parameter1.Value = (object) scenType;
    command.Parameters.Add((object) parameter1);
    IDbDataParameter parameter2 = command.CreateParameter();
    parameter2.ParameterName = str3;
    parameter2.Direction = ParameterDirection.Input;
    parameter2.Value = (object) (int) EntitySettingsDefault.ScenTypeToRecordTypeId(scenType);
    command.Parameters.Add((object) parameter2);
    using (IDataReader idr = command.ExecuteReader())
    {
      int fldIdxF_CODE = idr.GetOrdinal("F_CODE");
      while (idr.Read())
      {
        Entity entity = entitiesToConfig.FirstOrDefault<Entity>((System.Func<Entity, bool>) (candidate => candidate.Code == idr.GetString(fldIdxF_CODE) && candidate.Settings.Properties.Status != EntityPumpStatus.Commited));
        if (entity != null)
        {
          entity.Settings.ObjectType = EntitySettingsDefault.ScenTypeToIpsObjType(scenType);
          entity.Settings.AttributeBelong = EntitySetting.AttributeBelongs.ToObject;
        }
      }
    }
  }

  private static void SetDefaultConfigToTPNormScenEntities(IReadOnlyList<Entity> entitiesToConfig)
  {
    EntitySettingsDefault.SetDefaultConfigToScenEntities(14, entitiesToConfig);
  }

  private static void SetDefaultConfigToOperNormScenEntities(IReadOnlyList<Entity> entitiesToConfig)
  {
    EntitySettingsDefault.SetDefaultConfigToScenEntities(4, entitiesToConfig);
  }

  private static void SetDefaultConfigToStepNormScenEntities(IReadOnlyList<Entity> entitiesToConfig)
  {
    EntitySettingsDefault.SetDefaultConfigToScenEntities(6, entitiesToConfig);
  }

  private static void SetDefaultConfigToAddStepNormScenEntities(
    IReadOnlyList<Entity> entitiesToConfig)
  {
    EntitySettingsDefault.SetDefaultConfigToScenEntities(39, entitiesToConfig);
  }
}
