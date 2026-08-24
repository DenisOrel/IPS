// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.UpdateIpsSettingsPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData;

[TaskDescription("Инициализация настроек типов TechCard в IPS", "Анализ / корректировка настроек типов TechCard в IPS")]
[TaskType(PumperType.MetaData)]
internal class UpdateIpsSettingsPump(PluginClass plugin) : PumpClass(plugin)
{
  private readonly Guid _guid = new Guid("{B71E2C1C-1413-4687-BADA-CE529FD087FF}");

  private void UpdateObjectTypeSettings()
  {
    IDBObjectType objectType = this.plugin.Idw.GetUserSession().GetObjectType(TechCardConsts.ObjectTypes.TechBaseObjectGUID);
    if (objectType == null)
      return;
    int attributeTypeId = MetaDataHelper.GetAttributeTypeID(Intermech.Imbase.Consts.CreateNewObjectAttGUID);
    if (!(objectType.GetAttributeType(attributeTypeId) is IDBAttributeType4 attributeType) || attributeType.Required != RequiredModes.Manual)
      return;
    attributeType.Required = RequiredModes.Auto;
  }

  private void UpdateApplicabilitySettings()
  {
    ICache cacheService = TechCache.GetCacheService();
    IImportingData cache = cacheService.GetCache(ImportingCategory.DocTypes);
    try
    {
      IRelationTypeItem byGuid = this.plugin.Imdi.RelationTypes.GetByGuid(new Guid("cad0036b-306c-11d8-b4e9-00304f19f545"));
      if (byGuid == null)
        throw new Exception("Тип связи \"Изменяется по извещению\" не найдена");
      string customConfigById = TechPumpData.Configs.Cache.GetCustomConfigById(124, 0, out bool _);
      if (string.IsNullOrEmpty(customConfigById))
        return;
      string str1 = customConfigById;
      string[] separator = new string[1]
      {
        Environment.NewLine
      };
      foreach (string str2 in str1.Split(separator, StringSplitOptions.None))
      {
        string[] strArray = str2.Split(new char[1]{ '=' }, StringSplitOptions.None);
        if (strArray.Length != 2)
        {
          this.plugin.appManager.AddWarningMessage($"Входная строка имеет неверный формат : \"{str2}\"");
        }
        else
        {
          int result1;
          int result2;
          if (!int.TryParse(strArray[0], out result1) || !int.TryParse(strArray[1], out result2))
          {
            this.plugin.appManager.AddWarningMessage($"Входная строка имеет неверные типы объектов : \"{str2}\"");
          }
          else
          {
            TechTypeInfo typeRecByRecordId = TechPumpData.TechType.TechTypeList.GetTypeRecByRecordId(result1);
            if (typeRecByRecordId == null)
              this.plugin.appManager.AddWarningMessage($"Тип записи TechCard не найден. RecordId = \"{result1}\"");
            else if (typeRecByRecordId.TypeSett == null)
              this.plugin.appManager.AddWarningMessage($"Тип записи RecordId = \"{result1}\" ({typeRecByRecordId.Name}) не заданы настройки миграции. Привязка к ИИ не была обработана \"{str2}\"");
            else if (typeRecByRecordId.TypeSett.Mode == TechTypePumpMode.NotPumpType)
            {
              this.plugin.appManager.AddWarningMessage($"Тип записи RecordId = \"{result1}\" ({typeRecByRecordId.Name}) не настроен для миграции. Привязка к ИИ не была обработана \"{str2}\"");
            }
            else
            {
              int objectType = this.plugin.Imdi.UserSession.GetObjectType(typeRecByRecordId.TypeSett.ObjType).ObjectType;
              int newKey = (int) cache.GetNewKey(ImportingCategory.DocTypes, (object) result2);
              if (newKey == 0)
                this.plugin.appManager.AddWarningMessage($"Тип документа Search с docTypeId = \"{result2}\" не найден в IPS. Привязка к ИИ не была обработана \"{str2}\"");
              else if (this.plugin.Imdi.UserSession.GetRelationsApplicabilityCollection().GetApplicability(byGuid.ID, objectType, newKey) == null)
              {
                this.plugin.appManager.AddInfoMessage($"Добавление допустимой связи \"Изменяется по ИИ\" для объектов типа ({objectType}, {newKey})");
                RelationsApplicabilityProperties applicabilityProperties = new RelationsApplicabilityProperties(0, objectType, newKey, byGuid.ID, false, int.MaxValue, ApplicabilityModes.Enabled, RelationConstraintModes.ChildConstrained, false, true, ApplicabilityOptions.None);
                this.plugin.Imdi.UserSession.GetRelationsApplicabilityCollection().Create(applicabilityProperties);
              }
            }
          }
        }
      }
    }
    finally
    {
      cacheService.ReleaseCache(ImportingCategory.DocTypes);
    }
  }

  public override void Exam()
  {
    this.ExamCheckPoint("Инициализация настроек типов TechCard в IPS", 0);
    this.ExamCheckPoint("Инициализация настроек типов TechCard в IPS успешно завершена", 100);
  }

  public override void Pump()
  {
    this.PumpCheckPoint("Анализ / корректировка настроек типов объектов в IPS", 0);
    try
    {
      this.UpdateObjectTypeSettings();
      this.PumpCheckPoint("Анализ / корректировка настроек допустимых связей в IPS", 50);
      this.UpdateApplicabilitySettings();
    }
    finally
    {
      this.PumpCheckPoint("Анализ / корректировка настроек типов TechCard в IPS успешно завершены", 100);
    }
  }

  protected override Guid GUID => this._guid;
}
