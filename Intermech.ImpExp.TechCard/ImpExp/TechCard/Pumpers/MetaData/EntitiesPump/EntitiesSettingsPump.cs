// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntitiesSettingsPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Imbase;
using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using System;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

[TaskDescription("Настройка атрибутов для понятий Techcard", "Настройка атрибутов Techcard")]
internal class EntitiesSettingsPump : PumpClass
{
  private readonly Guid _guid = new Guid("{981B7A60-068E-4B1C-AB05-89821F96453A}");

  private void UpdateImbaseSelectMode()
  {
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      return;
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseCatalogsCreated);
    if (cache == null)
      return;
    try
    {
      foreach (Entity entity in TechPumpData.Entities.EntitiesList.Values.Where<Entity>((Func<Entity, bool>) (entity => entity.EntityReference != null && entity.IsMasterAttr && entity.Settings.AttributeBelong != EntitySetting.AttributeBelongs.ToLink)))
      {
        int pumpToAttrTypeId = entity.PumpToAttrTypeID;
        IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(pumpToAttrTypeId);
        if (attributeType != null && attributeType.FieldType == FieldTypes.ftObjectLink)
        {
          int objTypeID = entity.Settings.ObjectType != Guid.Empty ? entity.Settings.ObjectTypeID : TechPumpData.TechType.TechTypeList.GetObjTypeId(entity.RecordID);
          if (objTypeID != -1)
          {
            ImbaseExtendedItem imbaseExtendedItem = (ImbaseExtendedItem) null;
            ExtendedServiceHelper.ObjTypeInfo objTypeData = ExtendedServiceHelper.GetObjTypeData(objTypeID, this.plugin.Imdi.UserSession);
            if (objTypeData != null)
              imbaseExtendedItem = objTypeData.GetValue(pumpToAttrTypeId, this.plugin.Imdi.UserSession);
            if (imbaseExtendedItem != null && (imbaseExtendedItem.SelectMode == ImbaseCatalogSelectMode.imcmNone || imbaseExtendedItem.CatalogIDs.Count <= 0))
            {
              DictionaryValue dictionaryValue = cache.GetValue(ImportingCategory.ImbaseCatalogsCreated, (object) entity.EntityReference.Reference);
              if (dictionaryValue != null && dictionaryValue.NewObjectID != 0L)
              {
                imbaseExtendedItem.SelectMode = ImbaseCatalogSelectMode.imcmSelectFolder;
                imbaseExtendedItem.CatalogIDs.Clear();
                imbaseExtendedItem.CatalogIDs.Add(dictionaryValue.NewObjectID);
                objTypeData.SetValue(pumpToAttrTypeId, imbaseExtendedItem);
                objTypeData.SaveData(this.plugin.Imdi.UserSession);
              }
            }
          }
        }
      }
    }
    finally
    {
      service.ReleaseCache(ImportingCategory.ImbaseCatalogsCreated);
    }
  }

  private void UpdateMasterAttributeMode()
  {
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      return;
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseGroupsAttributes);
    if (cache == null)
      return;
    try
    {
      foreach (Entity entity1 in TechPumpData.Entities.EntitiesList.Values.Where<Entity>((Func<Entity, bool>) (entity => entity.EntityReference != null && !entity.IsMasterAttr && entity.EntityReference.Reference != 0 && entity.Settings.AttributeBelong != EntitySetting.AttributeBelongs.ToLink)))
      {
        int pumpToAttrTypeId = entity1.PumpToAttrTypeID;
        if (MetaDataHelper.GetAttributeType(pumpToAttrTypeId) != null)
        {
          int anObjectType = entity1.Settings.ObjectType != Guid.Empty ? entity1.Settings.ObjectTypeID : TechPumpData.TechType.TechTypeList.GetObjTypeId(entity1.RecordID);
          if (anObjectType != -1)
          {
            IDBObjectType objectType = this.plugin.Imdi.UserSession.GetObjectType(anObjectType, false);
            if (objectType != null)
            {
              IDBAttributeType4 attributeById1 = objectType.Attributes.GetAttributeByID(pumpToAttrTypeId, false);
              Entity entity2;
              if (attributeById1 != null && attributeById1.MasterAttributeID == 0 && TechPumpData.Entities.EntitiesList.TryGetValue(entity1.EntityReference.MasterCode, out entity2) && entity2 != null && entity2.PumpToAttrTypeID != 0)
              {
                IDBAttributeType4 attributeById2 = objectType.Attributes.GetAttributeByID(entity2.PumpToAttrTypeID, false);
                if (attributeById2 != null && attributeById2.AttributeType == FieldTypes.ftObjectLink)
                {
                  attributeById1.MasterAttributeID = entity2.PumpToAttrTypeID;
                  int attributeId = MetaDataHelper.GetAttributeID((object) MetaDataPump.GetImbaseFieldAttributeGuid(cache, entity1.EntityReference.Reference, entity1.EntityReference.Field));
                  switch (attributeId)
                  {
                    case -10000:
                    case 0:
                      continue;
                    default:
                      if (attributeId != pumpToAttrTypeId)
                      {
                        attributeById1.SourceAttributeID = attributeId;
                        continue;
                      }
                      continue;
                  }
                }
              }
            }
          }
        }
      }
    }
    finally
    {
      service.ReleaseCache(ImportingCategory.ImbaseGroupsAttributes);
    }
  }

  public EntitiesSettingsPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  public override void Exam()
  {
  }

  public override void Pump()
  {
    try
    {
      if (this.plugin.Imdi.UserSession is IUserSessionCacheDataSet userSession)
        MetaDataHelper.SyncMetadata(userSession.CacheDataSet, true);
      this.UpdateImbaseSelectMode();
      this.UpdateMasterAttributeMode();
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка при настройке атрибутов: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
  }

  protected override Guid GUID => this._guid;
}
