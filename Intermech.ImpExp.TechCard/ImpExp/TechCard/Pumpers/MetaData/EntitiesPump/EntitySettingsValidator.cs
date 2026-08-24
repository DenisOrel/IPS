// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntitySettingsValidator
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

internal class EntitySettingsValidator
{
  private readonly bool _allowModification;

  private void CheckExistEntity(Entity entity, IList<EntityErrorRecord> errorList)
  {
    this.CheckSettingsPumpTo(entity, errorList);
    if (entity.Settings.Properties.Status != EntityPumpStatus.None && entity.Settings.Properties.Status != EntityPumpStatus.NotPump && entity.RecordID != 0 && entity.IsMasterAttr && entity.Settings.PumpTo != null && entity.Settings.PumpTo is Guid)
    {
      Guid pumpTo = (Guid) entity.Settings.PumpTo;
      if (TechcardConsts.Plugin != null && TechcardConsts.Plugin.Imdi != null)
      {
        IAttributeTypeItem byGuid = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(pumpTo);
        if (byGuid == null)
        {
          string message = $"Атрибут с Guid = '{pumpTo}' не найден";
          errorList.Add(new EntityErrorRecord(entity, message));
        }
        else if (byGuid.AttrValueType != 8)
        {
          string message = "Мастер атрибут должен быть типа 'Ссылка на версию объекта'";
          errorList.Add(new EntityErrorRecord(entity, message));
        }
      }
    }
    this.CheckSettingsMeasure(entity, errorList);
    this.CheckSettingsObjectType(entity, errorList);
    this.CheckSettingsAttributeType(entity, errorList);
    this.CheckSettingsRefEntity(entity, errorList);
  }

  private void CheckNewEntity(Entity entity, IList<EntityErrorRecord> errorList)
  {
    EntitySetting settings = entity.Settings;
    EntityProperties entityProperties = settings.Properties;
    IMSAttributeType imsAttributeType = (IMSAttributeType) null;
    IAttributeTypeItem attributeTypeItem = (IAttributeTypeItem) null;
    if (string.IsNullOrEmpty(entityProperties.Alias))
    {
      if (settings.PumpMode == EntityPumModes.NewAttr)
      {
        string message = "Значение псевдонима атрибута не задано";
        errorList.Add(new EntityErrorRecord(entity, message));
      }
    }
    else
    {
      if (TechcardConsts.Plugin != null)
        attributeTypeItem = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByAlias(entityProperties.Alias);
      else
        imsAttributeType = MetaDataHelper.GetAttributeTypesList().FirstOrDefault<IMSAttributeType>((Func<IMSAttributeType, bool>) (item => item.Alias == entityProperties.Alias));
      if (attributeTypeItem != null || imsAttributeType != null)
        errorList.Add(new EntityErrorRecord(entity, "Атрибут c таким псевдонимом уже существует"));
    }
    if (string.IsNullOrEmpty(entityProperties.Name))
    {
      string message = "Наименование атрибута не может быть пустым.";
      errorList.Add(new EntityErrorRecord(entity, message));
    }
    else
    {
      if (TechcardConsts.Plugin != null)
        attributeTypeItem = attributeTypeItem ?? TechcardConsts.Plugin.Imdi.AttributeTypes.GetByName(entityProperties.Name);
      else
        imsAttributeType = imsAttributeType ?? MetaDataHelper.GetAttributeTypesList().FirstOrDefault<IMSAttributeType>((Func<IMSAttributeType, bool>) (item => item.Name == entityProperties.Name));
      if (attributeTypeItem != null || imsAttributeType != null)
        errorList.Add(new EntityErrorRecord(entity, "Атрибут c таким наименованием уже существует"));
    }
    if (settings.PumpMode == EntityPumModes.NewAttr && entity.IsMasterAttr && entityProperties.FieldType != FieldTypes.ftObjectLink)
    {
      string message = "Тип поля должен быть \"Ссылка на объект\"";
      errorList.Add(new EntityErrorRecord(entity, message));
    }
    this.CheckSettingsMeasure(entity, errorList);
    this.CheckSettingsObjectType(entity, errorList);
  }

  private void CheckNoneEntity(Entity entity, IList<EntityErrorRecord> errorList)
  {
    if (entity.IsMasterAttr)
      errorList.Add(new EntityErrorRecord(entity, "Не настроено понятие - ссылка на справочник"));
    this.CheckSettingsObjectType(entity, errorList);
  }

  private void CheckSettingsPumpTo(Entity entity, IList<EntityErrorRecord> errorList)
  {
    if (entity.Settings.PumpTo != null)
      return;
    string message;
    switch (entity.Settings.PumpMode)
    {
      case EntityPumModes.ExistAttr:
        message = "Не указано поле \"атрибут\".";
        break;
      case EntityPumModes.ExistEntity:
        message = "Не указано поле \"понятие\".";
        break;
      default:
        message = "У понятия отсутствуют настройки";
        break;
    }
    if (string.IsNullOrEmpty(message))
      return;
    errorList.Add(new EntityErrorRecord(entity, message));
  }

  private void CheckSettingsMeasure(Entity entity, IList<EntityErrorRecord> errorList)
  {
    if (!entity.IsMeasureAtribute() || entity.Settings.MeasProdSettings == null || entity.Settings.MeasProdSettings.isMeasureSet())
      return;
    string message = "Не настроена единица измерения";
    errorList.Add(new EntityErrorRecord(entity, message));
  }

  private void CheckSettingsObjectType(Entity entity, IList<EntityErrorRecord> errorList)
  {
    if (entity.Settings.ObjectType == Guid.Empty || (TechcardConsts.Plugin == null ? MetaDataHelper.GetObjectType(entity.Settings.ObjectType) != null : TechcardConsts.Plugin.Imdi.ObjectTypes.ExistsByGuid(entity.Settings.ObjectType)))
      return;
    if (this._allowModification)
      entity.Settings.ObjectType = Guid.Empty;
    string message = "Тип объекта отсутствует в базе IPS";
    errorList.Add(new EntityErrorRecord(entity, message));
  }

  private void CheckSettingsAttributeType(Entity entity, IList<EntityErrorRecord> errorList)
  {
    if (!(entity.Settings.PumpTo is Guid) || (Guid) entity.Settings.PumpTo == Guid.Empty)
      return;
    Guid attributeGuid;
    if (!EntityHelper.GetAttributeGuid(entity, out attributeGuid) || attributeGuid.Equals(Guid.Empty))
    {
      if (TechcardConsts.Plugin == null)
        return;
      string Message = $"Понятие \"{entity}\" пропущено из за ошибки";
      TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
    }
    else
    {
      if ((TechcardConsts.Plugin != null ? (TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(attributeGuid) != null ? 1 : 0) : (MetaDataHelper.GetAttributeType(attributeGuid) != null ? 1 : 0)) != 0)
        return;
      if (this._allowModification)
        entity.Settings.Properties.Status = EntityPumpStatus.None;
      string message = "Тип атрибута отсутствует в базе IPS";
      errorList.Add(new EntityErrorRecord(entity, message));
    }
  }

  private void CheckSettingsRefEntity(Entity entity, IList<EntityErrorRecord> errorList)
  {
    if (!(entity.Settings.PumpTo is Entity pumpTo) || TechPumpData.Entities.EntitiesList.ContainsKey(pumpTo.Code))
      return;
    if (this._allowModification)
      entity.Settings.PumpTo = (object) null;
    string message = "Понятие из настроек отсутствует в исходной базе";
    errorList.Add(new EntityErrorRecord(entity, message));
  }

  public EntitySettingsValidator(bool allowModification = false)
  {
    this._allowModification = allowModification;
  }

  public void Execute(IEnumerable<Entity> entities, out IEnumerable<EntityErrorRecord> errors)
  {
    if (entities == null)
      throw new ArgumentNullException(nameof (entities));
    List<EntityErrorRecord> errorList = new List<EntityErrorRecord>();
    errors = (IEnumerable<EntityErrorRecord>) errorList;
    foreach (Entity entity in entities)
    {
      if (entity.Settings == null || entity.Settings.Properties == null)
      {
        string message = "У понятия отсутствуют настройки";
        errorList.Add(new EntityErrorRecord(entity, message));
      }
      else
      {
        switch (entity.Settings.Properties.Status)
        {
          case EntityPumpStatus.None:
            this.CheckNoneEntity(entity, (IList<EntityErrorRecord>) errorList);
            continue;
          case EntityPumpStatus.Exists:
            this.CheckExistEntity(entity, (IList<EntityErrorRecord>) errorList);
            continue;
          case EntityPumpStatus.New:
            this.CheckNewEntity(entity, (IList<EntityErrorRecord>) errorList);
            continue;
          case EntityPumpStatus.Commited:
            if (entity.Settings.PumpTo == null)
            {
              this.CheckNewEntity(entity, (IList<EntityErrorRecord>) errorList);
              continue;
            }
            this.CheckExistEntity(entity, (IList<EntityErrorRecord>) errorList);
            continue;
          default:
            continue;
        }
      }
    }
  }
}
