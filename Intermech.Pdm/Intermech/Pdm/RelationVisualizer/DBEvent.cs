// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.DBEvent
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
public class DBEvent
{
  private DBEvent.DBEventType eventType;
  private long relationId;
  private long projId;
  private object tag;
  private DateTime time;

  public DBEvent(long relID, long projId, DBEvent.DBEventType evType)
    : this(evType)
  {
    this.relationId = relID;
    this.projId = projId;
  }

  public DBEvent(long relID, double count, DBEvent.DBEventType evType)
    : this(evType)
  {
    this.relationId = relID;
    this.tag = (object) count;
  }

  public DBEvent(long projId, DBEvent.DBEventType evType)
    : this(evType)
  {
    this.projId = projId;
  }

  private DBEvent(DBEvent.DBEventType evType)
  {
    this.eventType = evType;
    this.time = DateTime.Now;
  }

  public object Tag
  {
    get => this.tag;
    set => this.tag = value;
  }

  public DateTime Time => this.time;

  public DBEvent.DBEventType EventType => this.eventType;

  public long ProjId => this.projId;

  public long RelationId => this.relationId;

  [Serializable]
  public enum DBEventType
  {
    CreateRelation,
    RemoveRelation,
    RemoveObject,
    RelationChanged,
    ObjVerIdChanged,
    All,
  }
}
