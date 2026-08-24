// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.MetaDataPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;
using Intermech.ImpExp.TechCard.TechProcPump;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[TaskDescription("Инициализация данных для перекачки - закачка метаданных TechCard", "Перекачка метаданных TechCard")]
[TaskType(PumperType.MetaData)]
internal class MetaDataPump : PumpClass
{
  private ImportingCategory _imbaseAttributes;
  private readonly Guid _guid = new Guid("{6251F47B-225D-4353-8772-C5429540E4CA}");
  private readonly List<KeyValuePair<int, Entity>> _entities2TypeCreateList = new List<KeyValuePair<int, Entity>>();

  private void InitializeData()
  {
    this._imbaseAttributes = ImportingCategory.ImbaseGroupsAttributes;
  }

  private void PumpMetaData()
  {
    if (this.plugin?.Imdi == null)
      return;
    if (this.plugin.Imdi.UserSession == null)
    {
      this.plugin.appManager.AddWarningMessage("Невозможно получить пользовательскую сессию. Закачка метаданных TechCard будет прекращена.");
    }
    else
    {
      this.PumpMetaObjTypeData_Prepare();
      this.PumpMetaObjTypeData();
      this.PumpMetaApplicabilityData();
      this.PumpMetaAttrTypeData_Prepare();
      this.PumpMetaAttrTypeData();
      this.PumpMetaAttrTypeData_Complete();
    }
  }

  private void PumpMetaAttrTypeData()
  {
    if (this.plugin?.Imdi == null)
      return;
    try
    {
      IUserSession userSession = this.plugin.Imdi.UserSession;
      if (userSession == null)
        return;
      using (StepControlProgress stepControlProgress = new StepControlProgress())
      {
        IDBAttributeTypeCollection attributes = userSession.GetAttributesGroup(TechcardConsts.TypeConsts.gaImportedTechAtributeGroupGuid).Attributes;
        stepControlProgress.Text = "Закачка метаданных TechCard";
        stepControlProgress.SetCenterParentLocation(TechcardConsts.Plugin.appManager as Control);
        stepControlProgress.Visible = true;
        stepControlProgress.DrawProgressInCaption = true;
        stepControlProgress.SetProgress("Импорт типов атрибутов", 0);
        int num1 = 0;
        int count1 = this._entities2TypeCreateList.Count;
        foreach (KeyValuePair<int, Entity> entities2TypeCreate in this._entities2TypeCreateList)
        {
          try
          {
            Entity entity = entities2TypeCreate.Value;
            string code = entity.Code;
            EntitySetting settings = entities2TypeCreate.Value.Settings;
            EntityPumpStatus entityPumpStatus = settings.Properties.Status;
            switch (entityPumpStatus)
            {
              case EntityPumpStatus.None:
              case EntityPumpStatus.NotPump:
                continue;
              default:
                int num2 = -1;
                if (entityPumpStatus == EntityPumpStatus.New)
                {
                  AttributeTypeProperties typePropsByEntity = this.GetAttrTypePropsByEntity(settings.Properties, attributes);
                  bool flag1 = !string.IsNullOrEmpty(typePropsByEntity.Alias) && this.plugin.Imdi.AttributeTypes.ExistsByAlias(typePropsByEntity.Alias);
                  bool flag2 = this.plugin.Imdi.AttributeTypes.ExistsByName(typePropsByEntity.Name);
                  if (flag2 | flag1)
                  {
                    IAttributeTypeItem attributeTypeItem;
                    string str1;
                    if (flag2)
                    {
                      attributeTypeItem = this.plugin.Imdi.AttributeTypes.GetByName(typePropsByEntity.Name);
                      str1 = "именем";
                    }
                    else
                    {
                      attributeTypeItem = this.plugin.Imdi.AttributeTypes.GetByAlias(typePropsByEntity.Alias);
                      str1 = "алиасом";
                    }
                    if (attributeTypeItem != null)
                    {
                      if ((FieldTypes) attributeTypeItem.AttrValueType == settings.Properties.FieldType || settings.Properties.FieldType == FieldTypes.ftDouble && attributeTypeItem.AttrValueType == 13)
                      {
                        settings.PumpTo = (object) attributeTypeItem.GUID;
                        settings.PumpMode = EntityPumModes.ExistAttr;
                        settings.Properties.Status = EntityPumpStatus.Exists;
                        entityPumpStatus = EntityPumpStatus.Exists;
                        num2 = attributeTypeItem.ID;
                        this.plugin.appManager.AddWarningMessage($"Создание типа атрибута \"{typePropsByEntity.Name}\" для понятия \"{code}\": атрибут с таким {str1} уже существует, понятие будет перенастроено в данный атрибут ");
                        flag2 = false;
                        flag1 = false;
                      }
                      else if (flag2)
                      {
                        int num3 = 1;
                        string str2 = "Тех";
                        while (this.plugin.Imdi.AttributeTypes.ExistsByName($"{typePropsByEntity.Name} ({str2})"))
                        {
                          str2 = "Tex" + (object) num3;
                          ++num3;
                        }
                        settings.Properties.Name = $"{typePropsByEntity.Name} ({str2})";
                        this.plugin.appManager.AddWarningMessage($"Создание типа атрибута \"{typePropsByEntity.Name}\" для понятия \"{code}\": Атрибут с таким именем уже существует, имя атрибута будет заменено на \"{settings.Properties.Name}\"");
                        typePropsByEntity.Name = settings.Properties.Name;
                        flag2 = false;
                      }
                    }
                  }
                  if (flag1 | flag2)
                  {
                    string str = ((flag1 ? ", алиасом " : string.Empty) + (flag2 ? ", именем " : string.Empty)).Remove(0, 1);
                    this.plugin.appManager.AddWarningMessage($"Ошибки создания типа атрибута \"{typePropsByEntity.Name}\" для понятия \"{code}\": Атрибут с таким {str} уже существует ");
                    continue;
                  }
                  if (entityPumpStatus == EntityPumpStatus.New)
                  {
                    try
                    {
                      num2 = attributes.Create(typePropsByEntity);
                    }
                    catch (Exception ex)
                    {
                      this.plugin.appManager.AddWarningMessage($"Ошибки создания типа атрибута \"{typePropsByEntity.Name}\" для понятия \"{code}\": {ex.Message}");
                      if (ex is OutOfMemoryException)
                        throw;
                      continue;
                    }
                    if (num2 == -1)
                    {
                      this.plugin.appManager.AddWarningMessage($"Ошибка создания типа атрибута \"{typePropsByEntity.Name}\" для понятия \"{code}\": Идентификатор атрибута не задан");
                      continue;
                    }
                    if (!TechcardConsts.Plugin.Imdi.AttributeTypes.ExistsByGuid(typePropsByEntity.AttributeGuid))
                      TechcardConsts.Plugin.Imdi.AttributeTypes.Add(typePropsByEntity.Name, typePropsByEntity.ShortName, typePropsByEntity.Alias, typePropsByEntity.FieldType, typePropsByEntity.MultiValueMode, typePropsByEntity.SizeType, typePropsByEntity.AttributeGuid).ID = num2;
                    entity.Settings.PumpTo = (object) typePropsByEntity.AttributeGuid;
                    entity.Settings.PumpMode = EntityPumModes.ExistAttr;
                    entity.Settings.Properties.Status = EntityPumpStatus.Exists;
                    stepControlProgress.SetProgress($"Импорт типов атрибутов {num1} из {count1}", 100 * num1++ / count1);
                  }
                  if (TechcardConsts.TechcardCommon.Code2AttributeGuid.ContainsKey(code))
                    TechcardConsts.TechcardCommon.Code2AttributeGuid[code] = typePropsByEntity.AttributeGuid;
                  else
                    TechcardConsts.TechcardCommon.Code2AttributeGuid.Add(code, typePropsByEntity.AttributeGuid);
                }
                int key = entities2TypeCreate.Key;
                if (key != -1)
                {
                  if (num2 == -1)
                  {
                    if (entity.Settings.PumpTo != null)
                    {
                      if (entity.Settings.PumpTo is Guid)
                      {
                        IAttributeTypeItem byGuid = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid((Guid) entity.Settings.PumpTo);
                        if (byGuid != null)
                          num2 = byGuid.ID;
                      }
                      else
                        continue;
                    }
                    else
                      continue;
                  }
                  if (num2 != 0)
                  {
                    IDBObjectType objectType = userSession.GetObjectType(key);
                    if (!objectType.AnyAttributes)
                    {
                      if (objectType.Attributes.GetAttributeByID(num2) == null)
                      {
                        Attribute4ObjectTypeProperties properties4Attribute = this.GetProperties4Attribute(num2, userSession, objectType.ObjectType);
                        try
                        {
                          (objectType.Attributes as IDBAttribute4ObjectTypeCollection).Create(properties4Attribute);
                          continue;
                        }
                        catch (Exception ex)
                        {
                          TechcardConsts.Plugin.appManager.AddWarningMessage(ex.Message);
                          if (ex is OutOfMemoryException)
                            throw;
                          continue;
                        }
                      }
                      else
                        continue;
                    }
                    else
                      continue;
                  }
                  else
                    continue;
                }
                else
                  continue;
            }
          }
          catch (Exception ex)
          {
            this.plugin.appManager.AddWarningMessage($"Невозможно создать атрибут по понятию {entities2TypeCreate.Value}: {ex.Message}");
            if (ex is OutOfMemoryException)
              throw;
          }
        }
        IImportingData importingData;
        if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
          importingData = (IImportingData) null;
        else
          importingData = service.GetCache(this._imbaseAttributes, ImportingCategory.ImbaseCatalogs);
        IImportingData masterImport = importingData;
        try
        {
          int num4 = 0;
          int count2 = TechPumpData.EntTypeList.Count;
          stepControlProgress.SetProgress("Импорт мастер атрибутов", 0);
          foreach (KeyValuePair<int, EntityTypeRec> entType in (Dictionary<int, EntityTypeRec>) TechPumpData.EntTypeList)
          {
            TechTypeInfo techTypeInfo;
            if (TechPumpData.TechType.TechTypeList.TryGetValue(entType.Key, out techTypeInfo) && techTypeInfo != null)
            {
              if (techTypeInfo.TypeSett == null)
              {
                this.plugin.appManager.AddWarningMessage($"Настройки типа записи '{techTypeInfo.Name}' ( RecordId = {techTypeInfo.RecordID} ) не найдены");
              }
              else
              {
                Guid objType = techTypeInfo.TypeSett.ObjType;
                if (!(objType == Guid.Empty))
                {
                  IDBObjectType objectType = userSession.GetObjectType(objType);
                  if (objectType != null)
                  {
                    foreach (Entity entity1 in entType.Value.CodeList.Values)
                    {
                      Entity entity2;
                      if (entity1.EntityReference != null && !(entity1.EntityReference.MasterCode == entity1.Code) && entity1.Settings != null && entity1.Settings.PumpTo != null && entity1.Settings.PumpTo is Guid && entType.Value.CodeList.TryGetValue(entity1.EntityReference.MasterCode, out entity2) && entity2.Settings.PumpTo is Guid)
                      {
                        IDBAttributeType attributeType1 = userSession.GetAttributeType((Guid) entity1.Settings.PumpTo, false);
                        int attributeID1 = attributeType1 != null ? attributeType1.AttributeID : -1;
                        IDBAttributeType attributeType2 = userSession.GetAttributeType((Guid) entity2.Settings.PumpTo, false);
                        int attributeID2 = attributeType2 != null ? attributeType2.AttributeID : -1;
                        if (attributeID1 != -1 && attributeID2 != -1)
                        {
                          IDBAttributeType4 attributeById1 = objectType.Attributes.GetAttributeByID(attributeID1);
                          if (attributeById1 != null)
                          {
                            if ((attributeById1 as IDBAttributeType4Object).InheritMode == InheritModes.Inherited)
                            {
                              this.plugin.appManager.AddWarningMessage($"Невозможно изменить атрибут {attributeById1.Name} в контексте типа {objectType.ObjectTypeName} для понятия \"{entity1.Code}\" : Нет доступа на редактирование этого атрибута");
                            }
                            else
                            {
                              IDBAttributeType4 attributeById2 = objectType.Attributes.GetAttributeByID(attributeID2);
                              if (attributeById2 != null)
                              {
                                if (attributeById2.AttributeType != FieldTypes.ftObjectLink)
                                {
                                  this.plugin.appManager.AddWarningMessage($"Невозможно задать мастер атрибут (\"{attributeById2.Name}\") для атрибута (\"{attributeById1.Name}\") понятия \"{entity1.Code}\". Мастер атрибут должен быть типа 'Ссылка на версию объекта' ");
                                }
                                else
                                {
                                  try
                                  {
                                    attributeById1.MasterAttributeID = attributeID2;
                                    Guid guid = MetaDataPump.GetImbaseFieldAttributeGuid(masterImport, entity1.EntityReference.Reference, entity1.EntityReference.Field);
                                    if (guid == Guid.Empty && entity1.EntityReference.Field == -1)
                                      guid = new Guid("cad00020-306c-11d8-b4e9-00304f19f545");
                                    if (guid != Guid.Empty)
                                    {
                                      if (!this.plugin.Imdi.AttributeTypes.ExistsByGuid(guid))
                                      {
                                        IDBAttributeType attributeType3 = userSession.GetAttributeType(guid);
                                        if (attributeType3 == null)
                                          this.plugin.appManager.AddWarningMessage($"Ошибка назначения атрибута-источника для атрибута {attributeById1.Name} в контексте типа {objectType.ObjectTypeName} для понятия \"{entity1.Code}\":атрибут-источник с GUID = {guid} не найден");
                                        else
                                          attributeById1.SourceAttributeID = attributeType3.AttributeID;
                                      }
                                      else
                                      {
                                        IAttributeTypeItem byGuid = this.plugin.Imdi.AttributeTypes.GetByGuid(guid);
                                        int id = byGuid.ID;
                                        int attrValueType = byGuid.AttrValueType;
                                        int attributeType4 = (int) attributeById1.AttributeType;
                                        attributeById1.SourceAttributeID = id;
                                      }
                                    }
                                    else
                                      this.plugin.appManager.AddWarningMessage($"Ошибка назначения атрибута-источника для атрибута {attributeById1.Name} в контексте типа {objectType.ObjectTypeName} для понятия \"{entity1.Code}\" :атрибут-источник для поля ID = {entity1.EntityReference.Field} не найден");
                                  }
                                  catch (Exception ex)
                                  {
                                    this.plugin.appManager.AddWarningMessage(ex.Message);
                                    if (ex is OutOfMemoryException)
                                      throw;
                                  }
                                  stepControlProgress.SetProgress($"Импорт мастер атрибутов {num4} из {count2}", 100 * num4++ / count2);
                                }
                              }
                            }
                          }
                        }
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
          service?.ReleaseCache(this._imbaseAttributes, ImportingCategory.ImbaseCatalogs);
        }
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка закачки технологических метаданных: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
    finally
    {
      foreach (KeyValuePair<int, Entity> entities2TypeCreate in this._entities2TypeCreateList)
      {
        EntitySetting settings = entities2TypeCreate.Value.Settings;
        switch (settings.Properties.Status)
        {
          case EntityPumpStatus.None:
          case EntityPumpStatus.New:
          case EntityPumpStatus.NotPump:
            continue;
          default:
            if (settings.PumpTo is Entity)
            {
              Entity entity = entities2TypeCreate.Value;
              Guid attributeGuid;
              if (EntityHelper.GetAttributeGuid(entity, out attributeGuid) && !(attributeGuid == Guid.Empty))
              {
                if (TechPumpData.Entities.Code2AttributeGuid.ContainsKey(entity.Code))
                {
                  TechPumpData.Entities.Code2AttributeGuid[entity.Code] = attributeGuid;
                  continue;
                }
                TechPumpData.Entities.Code2AttributeGuid.Add(entity.Code, attributeGuid);
                continue;
              }
              continue;
            }
            continue;
        }
      }
      foreach (TechTypeInfo techTypeInfo in TechPumpData.TechType.TechTypeList.Values)
      {
        EntityTypeRec entityTypeRec;
        if (TechPumpData.EntTypeList.TryGetValue(techTypeInfo.RecordID, out entityTypeRec) && entityTypeRec != null)
        {
          foreach (KeyValuePair<string, Entity> code in entityTypeRec.CodeList)
          {
            if (TechPumpData.Entities.EntitiesList.ContainsKey(code.Key))
              entityTypeRec.CodeList[code.Key].Settings.CopyData(TechPumpData.Entities.EntitiesList[code.Key].Settings);
          }
        }
      }
      TechCache.WriteOneList(TechCache.CategoryList.Code2AttributeGuid, (object) TechcardConsts.TechcardCommon.Code2AttributeGuid);
      TechCache.WriteOneList(TechCache.CategoryList.EntitiesList, (object) TechPumpData.Entities.EntitiesList);
      TechCache.WriteOneList(TechCache.CategoryList.EntTypeList, (object) TechPumpData.EntTypeList);
    }
  }

  private void PumpMetaAttrTypeData_Prepare()
  {
    if (TechPumpData.Entities.EntitiesList == null || TechPumpData.Entities.EntitiesList.Count == 0)
      return;
    foreach (Entity entity in TechPumpData.Entities.EntitiesList.Values)
    {
      if (entity.Settings?.Properties != null && entity.Settings.Properties.Status != EntityPumpStatus.None && entity.Settings.Properties.Status != EntityPumpStatus.NotPump)
      {
        if (!entity.Settings.ObjectType.Equals(Guid.Empty))
        {
          IObjectTypeItem byGuid = TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(entity.Settings.ObjectType);
          KeyValuePair<int, Entity> keyValuePair = new KeyValuePair<int, Entity>(byGuid != null ? byGuid.ID : -1, entity);
          if (!this._entities2TypeCreateList.Contains(keyValuePair))
            this._entities2TypeCreateList.Add(keyValuePair);
        }
        else if (entity.IsPermisibleAttr2TypeObj || entity.Settings.Properties.Status == EntityPumpStatus.New)
        {
          bool flag = false;
          if (entity.RecordID == 4 || entity.RecordID == 5 || entity.RecordID == 8 || entity.RecordID == 15 || entity.RecordID == 17)
            flag = true;
          foreach (TechTypeInfo techTypeInfo in TechPumpData.TechType.TechTypeList.Values)
          {
            if (TechPumpData.EntTypeList.ContainsKey(techTypeInfo.RecordID) && techTypeInfo.TypeSett != null && !(techTypeInfo.TypeSett.ObjType == Guid.Empty))
            {
              int key = -1;
              if (!flag)
              {
                IObjectTypeItem byGuid = TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(techTypeInfo.TypeSett.ObjType);
                if (byGuid != null)
                  key = byGuid.ID;
              }
              if (flag || TechPumpData.EntTypeList[techTypeInfo.RecordID].CodeList.ContainsKey(entity.Code))
              {
                KeyValuePair<int, Entity> keyValuePair = new KeyValuePair<int, Entity>(key, entity);
                if (!this._entities2TypeCreateList.Contains(keyValuePair))
                  this._entities2TypeCreateList.Add(keyValuePair);
                if (flag)
                  break;
              }
            }
          }
          if (entity.RecordID == 0)
          {
            KeyValuePair<int, Entity> keyValuePair = new KeyValuePair<int, Entity>(-1, entity);
            if (!this._entities2TypeCreateList.Contains(keyValuePair))
              this._entities2TypeCreateList.Add(keyValuePair);
          }
        }
      }
    }
  }

  private void PumpMetaAttrTypeData_Complete()
  {
    if (!(this.plugin.Imdi.UserSession is IUserSessionCacheDataSet userSession))
      return;
    MetaDataHelper.Locked = false;
    MetaDataHelper.SyncMetadata(userSession.CacheDataSet, true);
    Dictionary<Guid, List<int>> dictionary1 = new Dictionary<Guid, List<int>>();
    List<int> intList;
    foreach (TechTypeInfo techTypeInfo in TechPumpData.TechType.TechTypeList.Values)
    {
      Guid key1;
      switch (techTypeInfo.RecordID)
      {
        case 8:
        case 15:
          key1 = TechcardConsts.TypeConsts.otTechTPOneObjTypeGuid;
          break;
        default:
          key1 = techTypeInfo.TypeSett != null ? techTypeInfo.TypeSett.ObjType : Guid.Empty;
          break;
      }
      EntityTypeRec entityTypeRec;
      if (!(key1 == Guid.Empty) && TechPumpData.EntTypeList.TryGetValue(techTypeInfo.RecordID, out entityTypeRec) && entityTypeRec != null)
      {
        if (!dictionary1.TryGetValue(key1, out intList))
        {
          intList = new List<int>();
          dictionary1.Add(key1, intList);
          intList.Add(MetaDataHelper.GetAttributeID((object) TechcardConsts.TypeConsts.atLastLevelSeek));
          intList.Add(MetaDataHelper.GetAttributeID((object) TechcardConsts.TypeConsts.atTechTypeKeyAttrGuid));
          intList.Add(MetaDataHelper.GetAttributeID((object) TechcardConsts.TypeConsts.atTechArtAtrGuid));
          intList.Add(MetaDataHelper.GetAttributeID((object) TechcardConsts.TypeConsts.atImbaseKeyAttrGuid));
          intList.Add(MetaDataHelper.GetAttributeID((object) TechcardConsts.TypeConsts.atTechTypeKeyAttrGuid));
        }
        foreach (string key2 in entityTypeRec.CodeList.Keys)
        {
          Guid attributeID;
          if (TechPumpData.Entities.Code2AttributeGuid.TryGetValue(key2, out attributeID))
            intList.Add(MetaDataHelper.GetAttributeID((object) attributeID));
        }
      }
    }
    if (dictionary1.TryGetValue(TechcardConsts.TypeConsts.otTechTPOneObjTypeGuid, out intList))
    {
      intList.Add(MetaDataHelper.GetAttributeID((object) TechcardConsts.TypeConsts.atProductionAttrTypeGuid));
      dictionary1[TechcardConsts.TypeConsts.otTechTPGroupObjTypeGuid] = intList;
      dictionary1[TechcardConsts.TypeConsts.otTechTPBaseObjTypeGuid] = intList;
      dictionary1[TechcardConsts.TypeConsts.otTechTPTypeObjTypeGuid] = intList;
    }
    Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
    foreach (KeyValuePair<Guid, List<int>> keyValuePair in dictionary1)
    {
      GenericListHelper.MakeUnique<int>(keyValuePair.Value);
      dictionary2[MetaDataHelper.GetObjectTypeID(keyValuePair.Key)] = keyValuePair.Value;
    }
    TechCache.WriteOneList(TechCache.CategoryList.TechAttributes2Exclude, (object) dictionary2);
  }

  private AttributeTypeProperties GetAttrTypePropsByEntity(
    EntityProperties entProp,
    IDBAttributeTypeCollection attrCol)
  {
    AttributeTypeProperties typePropsByEntity = new AttributeTypeProperties();
    typePropsByEntity.AttributeGuid = this.plugin.Imdi.NewPumpGuid();
    if (entProp == null || attrCol == null)
      return typePropsByEntity;
    AttributeTypePropertiesValidator validator = attrCol.GetValidator(entProp.FieldType);
    if (validator.SizeType != null && validator.SizeType.Length > 1)
      typePropsByEntity.SizeType = validator.SizeType[1];
    typePropsByEntity.FieldType = entProp.FieldType;
    typePropsByEntity.Name = entProp.Name;
    typePropsByEntity.LanguageID = validator.LanguageID;
    typePropsByEntity.AreaID = validator.AreaID;
    typePropsByEntity.ShortName = entProp.ShortName;
    typePropsByEntity.Alias = entProp.Alias;
    typePropsByEntity.MultiValueMode = entProp.MultipleValued;
    typePropsByEntity.Options = entProp.Options;
    typePropsByEntity.Mask = entProp.Mask;
    typePropsByEntity.Note = entProp.Note;
    typePropsByEntity.DefaultValue = entProp.DefaultValue;
    typePropsByEntity.Computed = entProp.Computed;
    typePropsByEntity.Formula = entProp.Formula;
    typePropsByEntity.Unique = entProp.UniqueMode;
    typePropsByEntity.IsContent = entProp.IsContent;
    return typePropsByEntity;
  }

  public Attribute4ObjectTypeProperties GetProperties4Attribute(
    int attributeId,
    IUserSession userSession,
    int objectType)
  {
    AttributeTypePropertiesValidator validatorForObjectType = userSession.GetAttributeTypeCollection(-1).GetValidatorForObjectType(attributeId);
    return new Attribute4ObjectTypeProperties()
    {
      AttributeID = attributeId,
      ComputeValueMode = validatorForObjectType.Computed == null ? ComputeValueModes.StoredValue : validatorForObjectType.Computed[0],
      DefaultValue = validatorForObjectType.DefaultValue,
      FieldType = validatorForObjectType.FieldType,
      Formula = validatorForObjectType.Formula != null ? validatorForObjectType.Formula.ToString() : string.Empty,
      InheritMode = InheritModes.Private,
      IsContent = validatorForObjectType.IsContent,
      LevelID = validatorForObjectType.LevelID,
      Mask = validatorForObjectType.Mask,
      MasterAttributeID = validatorForObjectType.MasterAttributeID,
      ObjectType = objectType,
      OptimizationMode = validatorForObjectType.OptimizationMode != null ? validatorForObjectType.OptimizationMode[0] : OptimizationModes.Write,
      Options = validatorForObjectType.Options,
      RequiredMode = RequiredModes.Manual,
      SourceAttributeID = validatorForObjectType.SourceAttributeID,
      UniqueValueMode = validatorForObjectType.Unique != null ? validatorForObjectType.Unique[0] : UniqueValueModes.NotUnique,
      ValidationRule = string.Empty
    };
  }

  private void PumpMetaObjTypeData()
  {
    if (this.plugin == null)
      return;
    if (this.plugin.Imdi == null)
      return;
    try
    {
      IUserSession userSession = this.plugin.Imdi.UserSession;
      if (userSession == null)
        return;
      using (StepControlProgress stepControlProgress = new StepControlProgress())
      {
        stepControlProgress.Text = "Закачка типов объектов TechCard";
        stepControlProgress.SetCenterParentLocation(TechcardConsts.Plugin.appManager as Control);
        stepControlProgress.Visible = true;
        stepControlProgress.DrawProgressInCaption = true;
        stepControlProgress.SetProgress("Импорт типов объектов", 0);
        int num1 = 0;
        int count = TechPumpData.TechType.TechTypeList.Values.Count;
        foreach (TechTypeInfo typeInfo in TechPumpData.TechType.TechTypeList.Values)
        {
          try
          {
            TechTypeSett typeSett = typeInfo.TypeSett;
            if (typeSett != null)
            {
              switch (typeSett.mode)
              {
                case TechTypePumpMode.NewObjType:
                  int num2 = -1;
                  Guid guid = TechcardConsts.TypeConsts.otTechobjectObjTypeGuid;
                  ObjectTypeProperties propertiesByTpType = this.GetObjectTypePropertiesByTPType(typeInfo, typeSett);
                  if (!typeSett.OwnerType.Equals(Guid.Empty))
                  {
                    propertiesByTpType.PublicLCSchema = InheritModes.Inherited;
                    guid = typeSett.OwnerType;
                  }
                  typeSett.ObjType = Guid.Empty;
                  try
                  {
                    int id = TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(guid).ID;
                    IDBObjectTypeCollection objectTypeCollection = userSession.GetObjectTypeCollection(id);
                    if (objectTypeCollection == null)
                    {
                      this.plugin.appManager.AddWarningMessage($"Ошибка создания типа объекта {propertiesByTpType.ObjectTypeName} :Не удалось получить коллекцию для типа объектов ID = {id}");
                      continue;
                    }
                    num2 = objectTypeCollection.Create(propertiesByTpType);
                  }
                  catch (Exception ex)
                  {
                    this.plugin.appManager.AddWarningMessage(ex.Message);
                    if (ex is OutOfMemoryException)
                      throw;
                  }
                  finally
                  {
                    stepControlProgress.SetProgress($"Импорт типов объектов {num1} из {count}", 100 * num1++ / count);
                  }
                  if (num2 != -1)
                  {
                    typeSett.ObjType = propertiesByTpType.ObjectTypeGuid;
                    continue;
                  }
                  continue;
                default:
                  continue;
              }
            }
          }
          catch (Exception ex)
          {
            this.plugin.appManager.AddWarningMessage($"Невозможно создать тип {typeInfo}: {ex.Message}");
            if (ex is OutOfMemoryException)
              throw;
          }
        }
      }
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка закачки типов объектов: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return;
      throw;
    }
    finally
    {
      TechCache.WriteOneList(TechCache.CategoryList.TechTypeList, (object) TechPumpData.TechType.TechTypeList);
    }
  }

  private void PumpMetaObjTypeData_Prepare()
  {
    if (TechPumpData.TechType.TechTypeList == null)
      return;
    int count = TechPumpData.TechType.TechTypeList.Count;
  }

  private ObjectTypeProperties GetObjectTypePropertiesByTPType(
    TechTypeInfo typeInfo,
    TechTypeSett typeSett)
  {
    int id = TechcardConsts.Plugin.Imdi.RelationTypes.GetByGuid(typeSett.RelType).ID;
    return new ObjectTypeProperties()
    {
      AnyAttributes = typeSett.AnyAttributes,
      AreaID = typeSett.Area,
      ObjectTypeGuid = TechcardConsts.Plugin.Imdi.NewPumpGuid(),
      ObjectTypeName = typeInfo.Name,
      Versionable = typeSett.Versionable,
      ObjectInstanceName = typeInfo.Name,
      DefaultRelation = id,
      Options = ObjectTypeOptions.None,
      ObjectTypeShortName = string.Empty,
      Note = string.Empty,
      SchemaID = 1
    };
  }

  private void PumpMetaApplicabilityData()
  {
    IUserSession session = this.plugin.Imdi.UserSession;
    if (session == null)
      return;
    Action<int, int, int> action = (Action<int, int, int>) ((projTypeId, partTypeId, relTypeId) =>
    {
      IDBRelationsApplicabilityCollection applicabilityCollection = session.GetRelationsApplicabilityCollection();
      if (MetaDataHelper.HasApplicability(projTypeId, partTypeId, relTypeId))
        return;
      RelationsApplicabilityProperties applicabilityProperties = new RelationsApplicabilityProperties()
      {
        InObjectType = projTypeId,
        ObjectType = partTypeId,
        RelationType = relTypeId,
        IsContent = true,
        CheckoutFiles = false,
        ApplicabilityMode = ApplicabilityModes.Enabled,
        CloneChildRelations = true,
        RelationConstraintMode = RelationConstraintModes.None
      };
      try
      {
        applicabilityCollection.Create(applicabilityProperties);
      }
      catch (Exception ex)
      {
        this.plugin.appManager.AddWarningMessage("Ошибка при добавлении применяемости : " + ex.Message);
        if (!(ex is OutOfMemoryException))
          return;
        throw;
      }
    });
    int objectTypeId = MetaDataHelper.GetObjectTypeID(TechcardConsts.TypeConsts.otToolRequestGuid);
    int relationTypeId = MetaDataHelper.GetRelationTypeID(TechcardConsts.TypeConsts.rtTechRelationGuid);
    action(MetaDataHelper.GetObjectTypeID(TechcardConsts.TypeConsts.otTechTPBaseObjTypeGuid), objectTypeId, relationTypeId);
    action(MetaDataHelper.GetObjectTypeID(TechcardConsts.TypeConsts.otOperationObjTypeGuid), objectTypeId, relationTypeId);
    action(MetaDataHelper.GetObjectTypeID(TechcardConsts.TypeConsts.otTechPerehodObjTypeGuid), objectTypeId, relationTypeId);
    action(MetaDataHelper.GetObjectTypeID(TechcardConsts.TypeConsts.otTechInstrumentObjTypeGuid), objectTypeId, relationTypeId);
    action(MetaDataHelper.GetObjectTypeID(TechcardConsts.TypeConsts.otInstrumentalPositionObjTypeGuid), objectTypeId, relationTypeId);
  }

  private void DoEndSaveMetadata(object sender, EventArgs e) => this.PumpMetaData();

  public MetaDataPump(PluginClass plugin)
    : base(plugin)
  {
    this.InitializeData();
    this.taskExam.Repumpble = true;
    this.taskPump.Repumpble = true;
  }

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
    this.plugin.appManager.AddEventOnSaveMetadata(new EventHandler(this.DoEndSaveMetadata));
    this.ExamCheckPoint("Подготовка данных успешно завершена", 100);
  }

  public override void Pump()
  {
  }

  public static Guid GetImbaseFieldAttributeGuid(
    IImportingData masterImport,
    int reference,
    int field)
  {
    if ((masterImport.GetTag(ImportingCategory.ImbaseGroupsAttributes, (object) reference) is ImbaseGroupAttributes tag ? tag.Attributes : (List<GroupAttribute>) null) != null)
    {
      foreach (GroupAttribute attribute in tag.Attributes)
      {
        if (attribute.Key == field)
          return attribute.AttrGuid;
      }
    }
    return Guid.Empty;
  }
}
