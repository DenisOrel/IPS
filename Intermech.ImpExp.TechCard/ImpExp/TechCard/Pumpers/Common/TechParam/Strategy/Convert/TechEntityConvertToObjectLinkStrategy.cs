// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert.TechEntityConvertToObjectLinkStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert;

[AttributeFieldType(new FieldTypes[] {FieldTypes.ftObjectLink})]
internal class TechEntityConvertToObjectLinkStrategy : TechEntityConvertStrategy
{
  private readonly IDictionary<string, string> _code2CodeCaption = (IDictionary<string, string>) new Dictionary<string, string>();

  private DictionaryValue ConvertImbaseCode2ObjectLink(
    TechPumpBase techPumper,
    TechParamList recordParamList,
    Entity entity,
    string value)
  {
    if (entity == null || entity.EntityReference == null || entity.EntityReference.Field != -2)
      return (DictionaryValue) null;
    string empty = string.Empty;
    if (recordParamList != null)
    {
      string code = string.Empty;
      switch (entity.Code)
      {
        case "%МТР":
          code = "Ммтр";
          break;
        case "%ZAG":
          code = "SORT";
          break;
      }
      if (!string.IsNullOrEmpty(code))
      {
        ITechParamBase entity1 = (ITechParamBase) recordParamList.GetEntity(code);
        if (entity1 != null)
          empty = System.Convert.ToString(entity1.Value);
      }
    }
    return ImbaseKeyConvertor.Instance.ConvertValue(entity, value, empty, techPumper._import_data_main);
  }

  private DictionaryValue ConvertImbaseRef2ObjectLink(
    TechPumpBase techPumper,
    TechParamList recordParamList,
    Entity entity,
    int recordKey)
  {
    DictionaryValue dictionaryValue = ImbaseLinkConvertor.Instance.ConvertValue(entity, recordKey, techPumper._import_data_imbase);
    if (dictionaryValue == null)
      return (DictionaryValue) null;
    if (dictionaryValue.Caption == string.Empty && recordParamList != null && recordParamList.Count != 0)
    {
      string caption4EntityCode = this.GetEntityCaption4EntityCode(entity.Code);
      if (caption4EntityCode != string.Empty)
      {
        object entityValue = recordParamList.GetEntityValue(caption4EntityCode);
        if (entityValue != null)
          dictionaryValue.Caption = entityValue.ToString();
      }
    }
    return dictionaryValue;
  }

  private string GetEntityCaption4EntityCode(string entCode)
  {
    if (entCode == string.Empty)
      return entCode;
    string code;
    if (!this._code2CodeCaption.TryGetValue(entCode, out code))
    {
      List<Entity> entityList = new List<Entity>();
      foreach (Entity entity in TechPumpData.Entities.EntitiesList.Values)
      {
        if (!(entity.Code == entCode) && entity.EntityReference != null && !(entity.EntityReference.MasterCode != entCode))
          entityList.Add(entity);
      }
      if (entityList.Count > 0)
      {
        if (entityList.Count == 1)
        {
          code = entityList[0].Code;
        }
        else
        {
          foreach (Entity entity in entityList)
          {
            if (entity.EntityReference != null && entity.EntityReference.Field == -1)
            {
              code = entity.Code;
              break;
            }
          }
          int num = code == string.Empty ? 1 : 0;
        }
      }
      this._code2CodeCaption[entCode] = code;
    }
    return code;
  }

  public override bool Convert(
    PumpClass pumper,
    TechObjectRecordBase record,
    TechParamList recordParamList,
    ITechParamEntity techEntity,
    Entity entitySettings,
    out ITechParamAttribute techAttribute,
    out string errorMessage)
  {
    if (entitySettings == null)
      throw new ArgumentNullException(nameof (entitySettings));
    errorMessage = (string) null;
    techAttribute = (ITechParamAttribute) null;
    if (techEntity == null || !(pumper is TechPumpBase techPumper))
      return false;
    bool flag = true;
    DictionaryValue dictionaryValue = (DictionaryValue) null;
    if (DataConvertor.IsEmptyValue(techEntity.Value))
      return false;
    long intValue;
    if (DataConvertor.ConvertObjToInt(techEntity.Value, out intValue))
    {
      flag = false;
      if (techEntity.IsFixed)
      {
        techAttribute = (ITechParamAttribute) new TechParamAttributeCaption(entitySettings.PumpToAttrType, (object) intValue, ((TechParamEntityFixed) techEntity).Caption, entitySettings.Settings != null ? entitySettings.Settings.AttributeBelong : EntitySetting.AttributeBelongs.ToLinkAndObject);
        return true;
      }
      dictionaryValue = this.ConvertImbaseRef2ObjectLink(techPumper, recordParamList, entitySettings, (int) intValue);
    }
    if (dictionaryValue == null)
      dictionaryValue = this.ConvertImbaseCode2ObjectLink(techPumper, recordParamList, entitySettings, System.Convert.ToString(techEntity.Value));
    if (dictionaryValue != null)
    {
      techAttribute = (ITechParamAttribute) new TechParamAttributeCaption(entitySettings.PumpToAttrType, (object) dictionaryValue.NewObjectID, dictionaryValue.Caption, entitySettings.Settings != null ? entitySettings.Settings.AttributeBelong : EntitySetting.AttributeBelongs.ToLinkAndObject);
      return true;
    }
    if (flag)
      errorMessage = $"Ошибка преобразования \"{((ITechParamBase) techEntity).ToString()}\" в ссылку на объект";
    return false;
  }
}
