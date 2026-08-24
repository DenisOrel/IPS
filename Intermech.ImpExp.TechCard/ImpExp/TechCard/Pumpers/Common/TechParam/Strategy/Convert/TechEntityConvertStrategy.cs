// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert.TechEntityConvertStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert;

public abstract class TechEntityConvertStrategy
{
  public abstract bool Convert(
    PumpClass pumper,
    TechObjectRecordBase record,
    TechParamList recordParamList,
    ITechParamEntity techEntity,
    Entity entitySettings,
    out ITechParamAttribute techAttribute,
    out string errorMessage);
}
