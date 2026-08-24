// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntitySettingsFixed
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal class EntitySettingsFixed
{
  private static readonly IDictionary<string, Entity> FixedSettings = EntitySettingsFixed.GetEntitySettings();

  private static IDictionary<string, Entity> GetEntitySettings()
  {
    Dictionary<string, Entity> entitySettings = new Dictionary<string, Entity>();
    Entity entity1 = new Entity("Квсм", string.Empty);
    entity1.Settings.PumpTo = (object) TechcardConsts.TypeConsts.atImbaseObjectAttrGuid;
    entity1.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
    entity1.IsPermisibleAttr2TypeObj = true;
    entitySettings.Add(entity1.Code, entity1);
    Entity entity2 = new Entity("A138", string.Empty);
    entity2.Settings.PumpTo = (object) TechcardConsts.TypeConsts.atImbaseObjectAttrGuid;
    entity2.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
    entity2.IsPermisibleAttr2TypeObj = true;
    entitySettings.Add(entity2.Code, entity2);
    Entity entity3 = new Entity("К_ОП", string.Empty);
    entity3.Settings.PumpTo = (object) TechcardConsts.TypeConsts.atImbaseObjectAttrGuid;
    entity3.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
    entity3.IsPermisibleAttr2TypeObj = true;
    entitySettings.Add(entity3.Code, entity3);
    Entity entity4 = new Entity("КПЕР", string.Empty);
    entity4.Settings.PumpTo = (object) TechcardConsts.TypeConsts.atImbaseObjectAttrGuid;
    entity4.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
    entitySettings.Add(entity4.Code, entity4);
    Entity entity5 = new Entity("Кдпр", string.Empty);
    entity5.Settings.PumpTo = (object) TechcardConsts.TypeConsts.atImbaseObjectAttrGuid;
    entity5.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
    entitySettings.Add(entity5.Code, entity5);
    Entity entity6 = new Entity("kisp", string.Empty);
    entity6.Settings.PumpTo = (object) TechcardConsts.TypeConsts.atImbaseObjectAttrGuid;
    entity6.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
    entity6.IsPermisibleAttr2TypeObj = true;
    entitySettings.Add(entity6.Code, entity6);
    Entity entity7 = new Entity("ВИД", string.Empty);
    entity7.Settings.PumpTo = (object) TechcardConsts.TypeConsts.atPartObjectAttrGuid;
    entity7.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
    entity7.IsPermisibleAttr2TypeObj = true;
    entitySettings.Add(entity7.Code, entity7);
    Entity entity8 = new Entity("ВЗГ", string.Empty);
    entity8.Settings.PumpTo = (object) TechcardConsts.TypeConsts.atVidZagAttrTypeGuid;
    entity8.Settings.Properties.FieldType = FieldTypes.ftObjectLink;
    entity8.IsPermisibleAttr2TypeObj = true;
    entitySettings.Add(entity8.Code, entity8);
    Entity entity9 = new Entity("%ZDS", string.Empty);
    entity9.Settings.PumpTo = (object) new Guid("cad0001f-306c-11d8-b4e9-00304f19f545");
    entity9.Settings.Properties.FieldType = FieldTypes.ftString;
    entity9.IsPermisibleAttr2TypeObj = true;
    entitySettings.Add(entity9.Code, entity9);
    return (IDictionary<string, Entity>) entitySettings;
  }

  public void Setup(IEnumerable<Entity> entities)
  {
    if (entities == null)
      return;
    foreach (Entity entity in entities)
    {
      if (entity != null)
      {
        EntitySettingsFixed.SetFixedSettings(entity);
        this.SetUniqueRecordSettings(entity);
        EntitySettingsFixed.SetMeasuredSettings(entity);
        EntitySettingsFixed.SetOleDraftSettings(entity);
      }
    }
  }

  private static void SetMeasuredSettings(Entity entity)
  {
    if (!entity.IsMeasureAtribute())
      return;
    entity.Settings.Properties.FieldType = FieldTypes.ftMeasured;
  }

  private static void SetFixedSettings(Entity entity)
  {
    Entity entity1;
    if (!EntitySettingsFixed.FixedSettings.TryGetValue(entity.Code, out entity1))
      return;
    entity.Settings.PumpTo = entity1.Settings.PumpTo;
    entity.Settings.PumpMode = EntityPumModes.ExistEntity;
    entity.Settings.Properties.Status = EntityPumpStatus.Commited;
    entity.Settings.Properties.FieldType = entity1.Settings.Properties.FieldType;
    entity.Settings.PumpMode = EntityPumModes.ExistAttr;
    entity.LockedSettings = true;
    entity.IsPermisibleAttr2TypeObj = entity1.IsPermisibleAttr2TypeObj;
  }

  private static void SetOleDraftSettings(Entity entity)
  {
    if (!entity.Type.Equals("E"))
      return;
    entity.LockedSettings = true;
    entity.Settings.PumpTo = (object) TechcardConsts.TypeConsts.atDraftOLEObjectGuid;
    entity.Settings.PumpMode = EntityPumModes.ExistAttr;
    entity.Settings.Properties.FieldType = FieldTypes.ftBlob;
    entity.Settings.Properties.Status = EntityPumpStatus.Commited;
  }

  private void SetUniqueRecordSettings(Entity entity)
  {
    switch ((TechcardConsts.TpRecordType) entity.RecordID)
    {
      case TechcardConsts.TpRecordType.Oborud:
      case TechcardConsts.TpRecordType.Personal:
      case TechcardConsts.TpRecordType.MaterialAdd:
      case TechcardConsts.TpRecordType.Tool:
        if (!entity.Type.Equals("К"))
          break;
        entity.Settings.AttributeBelong = EntitySetting.AttributeBelongs.ToLink;
        break;
    }
  }
}
