// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisObjData
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisObjData : IVisObjectData
{
  public long ObjVerId { get; set; }

  public int ObjTypeId { get; set; }

  public int LCLevelId { get; set; }

  public string Caption { get; set; }

  public List<VisStatus> StatusList { get; set; }

  public long ID { get; set; }

  public VisObjData(long ovId, int otId, int lcId)
  {
    this.ObjVerId = ovId;
    this.ObjTypeId = otId;
    this.LCLevelId = lcId;
  }

  public VisObjData(long ovId, int otId, int lcId, List<VisStatus> sList)
    : this(ovId, otId, lcId)
  {
    this.StatusList = sList;
  }

  public VisObjData(long ovId, int otId, int lcId, List<VisStatus> sList, string capt)
    : this(ovId, otId, lcId)
  {
    this.StatusList = sList;
    this.Caption = capt;
  }

  public VisObjData(long ovId)
  {
    this.ObjVerId = ovId;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.Init(sessionKeeper.Session);
  }

  public VisObjData(long ovId, IUserSession ius)
  {
    this.ObjVerId = ovId;
    this.Init(ius);
  }

  public void Init(IUserSession ius)
  {
    IDBObject dbObject = ius.GetObject(this.ObjVerId, false);
    if (dbObject == null)
      return;
    this.ObjTypeId = dbObject.ObjectType;
    this.LCLevelId = ius.GetLifecycleStep(dbObject.LCStep).LevelID;
    this.Caption = dbObject.Caption;
    this.ID = dbObject.ID;
  }
}
