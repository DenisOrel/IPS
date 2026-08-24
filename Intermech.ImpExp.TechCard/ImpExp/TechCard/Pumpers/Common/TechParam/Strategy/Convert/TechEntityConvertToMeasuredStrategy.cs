// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert.TechEntityConvertToMeasuredStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert;

[AttributeFieldType(new FieldTypes[] {FieldTypes.ftMeasured})]
internal class TechEntityConvertToMeasuredStrategy : TechEntityConvertStrategy
{
  private long GetMeasureId(
    TechObjectRecordBase record,
    TechParamList recordParamList,
    Entity entitySettings)
  {
    string entityWithMeasure = entitySettings.Settings.MeasProdSettings.EntityWithMeasure;
    if (!string.IsNullOrEmpty(entityWithMeasure))
    {
      string measureName = System.Convert.ToString(recordParamList.GetEntityValue(entityWithMeasure));
      if (!string.IsNullOrEmpty(measureName))
        return (((IEnumerable<MeasureDescriptor>) MeasureHelper.Measures).FirstOrDefault<MeasureDescriptor>((Func<MeasureDescriptor, bool>) (item => string.Compare(item.ShortName, measureName, StringComparison.InvariantCultureIgnoreCase) == 0)) ?? throw new Exception($"Неизвестная единица измерений: {measureName}")).MeasureID;
    }
    int productionId = this.GetProductionId(entitySettings, record);
    return entitySettings.Settings.MeasProdSettings[productionId];
  }

  private int GetProductionId(Entity entitySettings, TechObjectRecordBase record)
  {
    switch (entitySettings.RecordID)
    {
      case 1:
      case 8:
      case 15:
      case 21:
      case 23:
        try
        {
          return System.Convert.ToInt32(record.GetFieldValue("F_PRODUCTION"));
        }
        catch (Exception ex)
        {
          if (!(ex is OutOfMemoryException))
            return -1;
          throw;
        }
      default:
        return -1;
    }
  }

  private MeasuredValue GetBaseMeasureValue(double value, long measureId)
  {
    if (MeasureHelper.Measures == null)
      return (MeasuredValue) null;
    try
    {
      MeasuredValue measuredValue = new MeasuredValue(value, measureId);
      MeasuredValue baseMeasure = MeasureHelper.ConvertToBaseMeasure(measuredValue);
      baseMeasure.Caption = measuredValue.Caption;
      return baseMeasure;
    }
    catch (Exception ex)
    {
      throw new InvalidCastException($"Невозможно преобразовать значение {value} в единице измерения ID={measureId} в базовую единицу измерения по причине: {ex.Message}", ex);
    }
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
    if (techEntity == null)
      return false;
    long measureId = this.GetMeasureId(record, recordParamList, entitySettings);
    if (measureId == -1L || DataConvertor.IsEmptyValue(techEntity.Value))
      return false;
    double dblValue;
    if (DataConvertor.ConvertObjToDouble(techEntity.Value, out dblValue))
    {
      MeasuredValue baseMeasureValue = this.GetBaseMeasureValue(dblValue, measureId);
      if (baseMeasureValue == null)
        return false;
      techAttribute = (ITechParamAttribute) new TechParamAttribute(entitySettings.PumpToAttrType, (object) baseMeasureValue, entitySettings.Settings != null ? entitySettings.Settings.AttributeBelong : EntitySetting.AttributeBelongs.ToLinkAndObject);
      return true;
    }
    errorMessage = $"Ошибка преобразования \"{((ITechParamBase) techEntity).ToString()}\" в значение с плавающей точкой";
    return false;
  }
}
