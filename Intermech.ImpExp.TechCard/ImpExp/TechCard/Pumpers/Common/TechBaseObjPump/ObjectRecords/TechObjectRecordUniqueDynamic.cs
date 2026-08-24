// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords.TechObjectRecordUniqueDynamic
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;

internal class TechObjectRecordUniqueDynamic(string tableName = "", TechRecordParser parser = null) : 
  TechObjectRecordDynamic(tableName, parser)
{
  public string UniqueRecordHash;

  public override void Clear()
  {
    base.Clear();
    this.UniqueRecordHash = string.Empty;
  }

  public override void Assign(object source) => base.Assign(source);
}
