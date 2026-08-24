// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.DBEventList
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
public class DBEventList
{
  private List<DBEvent> events = new List<DBEvent>();

  public DateTime GetMaxEventTime()
  {
    return this.events.Count > 0 ? this.events[this.events.Count - 1].Time : DateTime.MinValue;
  }

  public DateTime GetMinEventTime()
  {
    return this.events.Count > 0 ? this.events[0].Time : DateTime.MinValue;
  }

  public void AddRelationCountChanged(long relationId, double count)
  {
    this.events.Add(new DBEvent(relationId, count, DBEvent.DBEventType.RelationChanged));
  }

  public void AddRelationCreatedEvent(long relationId, long projId)
  {
    this.events.Add(new DBEvent(relationId, projId, DBEvent.DBEventType.CreateRelation));
  }

  public void AddRelationRemoveEvent(long relationId, long projId)
  {
    this.events.Add(new DBEvent(relationId, projId, DBEvent.DBEventType.RemoveRelation));
  }

  public void AddObjectRemoveEvent(long projId)
  {
    this.events.Add(new DBEvent(projId, DBEvent.DBEventType.RemoveObject));
  }

  public void AddObjectInChangedEvent(long objVerId, string caption)
  {
    DBEvent dbEvent = new DBEvent(objVerId, DBEvent.DBEventType.ObjVerIdChanged);
    if (caption != null)
      dbEvent.Tag = (object) caption;
    this.events.Add(dbEvent);
  }

  public bool ifContainsRelationRemoved(long relationId, DateTime dateOfLastCheck)
  {
    for (int index = this.events.Count - 1; index >= 0; --index)
    {
      DBEvent dbEvent = this.events[index];
      if (dbEvent.Time < dateOfLastCheck)
        return false;
      if (dbEvent.EventType == DBEvent.DBEventType.RemoveRelation && dbEvent.RelationId == relationId)
        return true;
    }
    return false;
  }

  public bool ifContainsRelationCreated(DateTime dateOfLastCheck)
  {
    for (int index = this.events.Count - 1; index >= 0; --index)
    {
      DBEvent dbEvent = this.events[index];
      if (dbEvent.Time < dateOfLastCheck)
        return false;
      if (dbEvent.EventType == DBEvent.DBEventType.CreateRelation)
        return true;
    }
    return false;
  }

  public bool ifContainsObjectVerIdChanged(
    long objectId,
    DateTime dateOfLastCheck,
    out string caption)
  {
    caption = (string) null;
    for (int index = this.events.Count - 1; index >= 0; --index)
    {
      DBEvent dbEvent = this.events[index];
      if (dbEvent.Time < dateOfLastCheck)
        return false;
      if (dbEvent.EventType == DBEvent.DBEventType.ObjVerIdChanged && dbEvent.ProjId == objectId)
      {
        if (dbEvent.Tag != null)
          caption = (string) dbEvent.Tag;
        return true;
      }
    }
    return false;
  }

  public bool ifContainsObjectRemoved(long objectId, DateTime dateOfLastCheck)
  {
    for (int index = this.events.Count - 1; index >= 0; --index)
    {
      DBEvent dbEvent = this.events[index];
      if (dbEvent.Time < dateOfLastCheck)
        return false;
      if (dbEvent.EventType == DBEvent.DBEventType.RemoveObject && dbEvent.ProjId == objectId)
        return true;
    }
    return false;
  }

  public bool ifContainsRelationCountChanged(
    long relId,
    DateTime dateOfLastCheck,
    out double count)
  {
    count = 0.0;
    for (int index = this.events.Count - 1; index >= 0; --index)
    {
      DBEvent dbEvent = this.events[index];
      if (dbEvent.Time < dateOfLastCheck)
        return false;
      if (dbEvent.EventType == DBEvent.DBEventType.RelationChanged && dbEvent.RelationId == relId)
      {
        count = Convert.ToDouble(dbEvent.Tag);
        return true;
      }
    }
    return false;
  }
}
