// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.EntityTypeList
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump;

[Serializable]
internal class EntityTypeList : Dictionary<int, EntityTypeRec>
{
  public EntityTypeList()
  {
  }

  public EntityTypeList(int capacity)
    : base(capacity)
  {
  }

  protected EntityTypeList(SerializationInfo serializationInfo, StreamingContext streamingContext)
    : base(serializationInfo, streamingContext)
  {
  }

  public virtual EntityTypeRec GetRecByType(int recTypeId)
  {
    EntityTypeRec recByType;
    if (!this.TryGetValue(recTypeId, out recByType))
    {
      recByType = new EntityTypeRec();
      this.Add(recTypeId, recByType);
    }
    return recByType;
  }

  public virtual void AddEntity(Entity entity)
  {
    if (entity == null || entity.RecordID == 0)
      return;
    this.GetRecByType(entity.RecordID).AddEntity(entity);
  }
}
