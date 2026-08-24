// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert.TechEntityConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert;

internal class TechEntityConverter
{
  private readonly PumpClass _pumper;
  private readonly ObjectForAttributeFieldTypeService<TechEntityConvertStrategy> _convertStrategyCache;

  private void AddWarningMessage(string message)
  {
    if (this._pumper == null || this._pumper.Plugin == null || this._pumper.Plugin.appManager == null || string.IsNullOrEmpty(message))
      return;
    this._pumper.Plugin.appManager.AddNewWarningMessage(message);
  }

  public TechEntityConverter(
    PumpClass pumper,
    ObjectForAttributeFieldTypeService<TechEntityConvertStrategy> convertStrategyCache)
  {
    if (convertStrategyCache == null)
      throw new ArgumentNullException(nameof (convertStrategyCache));
    this._pumper = pumper;
    this._convertStrategyCache = convertStrategyCache;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ITechParamAttribute Convert(
    TechObjectRecordBase record,
    TechParamList recordParamList,
    ITechParamEntity techEntity,
    Entity entitySettings)
  {
    if (techEntity == null || entitySettings == null || entitySettings.PumpToAttrType == null)
      return (ITechParamAttribute) null;
    TechEntityConvertStrategy entityConvertStrategy = this._convertStrategyCache.GetObject((FieldTypes) entitySettings.PumpToAttrType.AttrValueType, true);
    if (entityConvertStrategy == null)
      return (ITechParamAttribute) null;
    try
    {
      ITechParamAttribute techAttribute;
      string errorMessage;
      if (!entityConvertStrategy.Convert(this._pumper, record, recordParamList, techEntity, entitySettings, out techAttribute, out errorMessage))
        this.AddWarningMessage(errorMessage);
      return techAttribute;
    }
    catch (InvalidCastException ex)
    {
      this.AddWarningMessage($"Невозможно конвертировать {((ITechParamBase) techEntity).ToString()} по в атрибут типа {System.Convert.ToString((object) (FieldTypes) entitySettings.PumpToAttrType.AttrValueType)} по причине {ex.Message}");
    }
    return (ITechParamAttribute) null;
  }
}
