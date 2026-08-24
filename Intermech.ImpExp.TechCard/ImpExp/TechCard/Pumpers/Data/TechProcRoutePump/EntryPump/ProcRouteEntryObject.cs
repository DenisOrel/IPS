// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump.ProcRouteEntryObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.Interfaces;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.TechProcRoutePump.EntryPump;

internal class ProcRouteEntryObject : TechObjectRecordDynamic
{
  public ProcRouteEntryObject()
    : base()
  {
    this.TableName = "TC_OBJ2LINK";
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    int int32_1 = BasePumpHelper.ToInt32(this.Fields["F_ART_TCKEY"]);
    int int32_2 = BasePumpHelper.ToInt32(this.Fields["F_PROJ_TCKEY"]);
    int int32_3 = BasePumpHelper.ToInt32(this.Fields["F_ZAK_TCKEY"]);
    int int32Value1 = DataSetProcessor.GetInt32Value(this.Fields["F_ART_ID"], 0);
    int int32Value2 = DataSetProcessor.GetInt32Value(this.Fields["F_PROJ_ID"], 0);
    int int32Value3 = DataSetProcessor.GetInt32Value(this.Fields["F_ZAK_ID"], 0);
    this.EntryInfo = new EntryInfo(int32_1, int32Value1, int32_2, int32Value2, int32_3, int32Value3);
    this.LinkedObj = new LinkedObjDescr(BasePumpHelper.ToInt32(this.Fields["F_OBJ_KEY"]), (LinkedObjectType) BasePumpHelper.ToInt32(this.Fields["F_OBJ_TYPE"]));
  }

  public EntryInfo EntryInfo { get; private set; }

  public LinkedObjDescr LinkedObj { get; private set; }

  public override void Clear()
  {
    base.Clear();
    this.EntryInfo = new EntryInfo(0, 0, 0, 0, 0, 0);
  }

  public override void Assign(object source)
  {
    base.Assign(source);
    if (!(source is ProcRouteEntryObject routeEntryObject))
      return;
    int artTcKey = routeEntryObject.EntryInfo.ArtTcKey;
    EntryInfo entryInfo = routeEntryObject.EntryInfo;
    int artId = entryInfo.ArtId;
    entryInfo = routeEntryObject.EntryInfo;
    int sbArtTcKey = entryInfo.SbArtTcKey;
    entryInfo = routeEntryObject.EntryInfo;
    int sbArtId = entryInfo.SbArtId;
    entryInfo = routeEntryObject.EntryInfo;
    int zakArtTcKey = entryInfo.ZakArtTcKey;
    entryInfo = routeEntryObject.EntryInfo;
    int zakArtId = entryInfo.ZakArtId;
    this.EntryInfo = new EntryInfo(artTcKey, artId, sbArtTcKey, sbArtId, zakArtTcKey, zakArtId);
  }

  public void AssignTo(ProcRouteEntryObject source, EntryInfo entryInfo)
  {
    this.Assign((object) source);
    this.EntryInfo = entryInfo;
  }
}
