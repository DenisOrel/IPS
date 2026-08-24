// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert.TechEntityConvertDefaultStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert;

[AttributeFieldType(new FieldTypes[] {FieldTypes.ftUnknown})]
internal class TechEntityConvertDefaultStrategy : TechEntityConvertStrategy
{
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
    techAttribute = (ITechParamAttribute) new TechParamAttribute(entitySettings.PumpToAttrType, techEntity.Value, entitySettings.Settings != null ? entitySettings.Settings.AttributeBelong : EntitySetting.AttributeBelongs.ToLinkAndObject);
    return true;
  }
}
