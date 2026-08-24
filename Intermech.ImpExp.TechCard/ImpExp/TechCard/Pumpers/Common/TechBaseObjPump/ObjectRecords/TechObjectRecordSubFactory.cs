// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords.TechObjectRecordSubFactory
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.ImbaseObjectRecord;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;

internal class TechObjectRecordSubFactory
{
  public static TechObjectRecordSub Create(string dopType, bool newStructure = false)
  {
    TechObjectRecordSub techObjectRecordSub = (TechObjectRecordSub) null;
    switch (dopType)
    {
      case "D":
        techObjectRecordSub = newStructure ? (TechObjectRecordSub) new TechObjectRecordSub_D2() : (TechObjectRecordSub) new TechObjectRecordSub_D();
        break;
      case "F":
        techObjectRecordSub = (TechObjectRecordSub) new TechObjectRecordSub_F();
        break;
      case "I":
        techObjectRecordSub = (TechObjectRecordSub) new TechObjectRecordSub_I();
        break;
      case "REC":
        techObjectRecordSub = (TechObjectRecordSub) new ImbaseObjectRecordSub_Rec();
        break;
      case "S":
        techObjectRecordSub = (TechObjectRecordSub) new TechObjectRecordSub_S();
        break;
      case "S_D":
        techObjectRecordSub = (TechObjectRecordSub) new TechObjectRecordSub_D();
        break;
      case "S_F":
        techObjectRecordSub = (TechObjectRecordSub) new TechObjectRecordSub_F();
        break;
      case "S_I":
        techObjectRecordSub = (TechObjectRecordSub) new TechObjectRecordSub_I();
        break;
      case "S_S":
        techObjectRecordSub = (TechObjectRecordSub) new TechObjectRecordSub_S();
        break;
    }
    return techObjectRecordSub;
  }
}
