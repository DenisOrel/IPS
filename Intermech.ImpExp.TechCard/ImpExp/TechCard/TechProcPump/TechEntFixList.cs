// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.TechEntFixList
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump;

[Serializable]
internal class TechEntFixList : Dictionary<string, Dictionary<string, TechEntityFix>>
{
  protected TechEntFixList(SerializationInfo serializationInfo, StreamingContext streamingContext)
    : base(serializationInfo, streamingContext)
  {
  }

  public TechEntFixList()
  {
  }

  public Dictionary<string, TechEntityFix> GetEntities(string tableName)
  {
    Dictionary<string, TechEntityFix> entities;
    if (!this.TryGetValue(tableName, out entities))
    {
      entities = new Dictionary<string, TechEntityFix>();
      this.Add(tableName, entities);
    }
    return entities;
  }

  public string GetEntity(string tableName, string fieldName)
  {
    string entity = string.Empty;
    TechEntityFix techEntityFix;
    if (this.GetEntities(tableName).TryGetValue(fieldName, out techEntityFix))
      entity = techEntityFix.EntCode;
    return entity;
  }

  public void AddEntity(TechEntityFix techEntFix)
  {
    if (techEntFix == null)
      return;
    Dictionary<string, TechEntityFix> entities = this.GetEntities(techEntFix.TableName);
    if (entities.ContainsKey(techEntFix.FieldName))
      return;
    entities.Add(techEntFix.FieldName, techEntFix);
  }
}
