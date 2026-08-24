// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisDBEvent
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class VisDBEvent
{
  public EvCode EventCode { get; private set; }

  public long ObjId { get; private set; }

  public long RelId { get; private set; }

  public MeasuredValue Quantity { get; private set; }

  public VisDBEvent(EvCode code, long Id)
  {
    this.EventCode = code;
    if (VisDBEvent.IsObject(code))
    {
      this.ObjId = Id;
      this.RelId = 0L;
    }
    else
    {
      this.ObjId = 0L;
      this.RelId = Id;
    }
  }

  public VisDBEvent(EvCode code, long objId, long relId)
  {
    this.EventCode = code;
    this.ObjId = objId;
    this.RelId = relId;
  }

  public VisDBEvent(long relId, MeasuredValue quan)
  {
    this.EventCode = EvCode.RelationModified;
    this.ObjId = 0L;
    this.RelId = relId;
    this.Quantity = quan;
  }

  public static bool IsObject(EvCode code) => (code & EvCode.Object) != 0;

  public static bool IsRelation(EvCode code) => (code & EvCode.Relation) != 0;
}
