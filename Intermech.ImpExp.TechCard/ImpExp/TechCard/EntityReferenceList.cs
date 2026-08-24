// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.EntityReferenceList
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[Serializable]
public class EntityReferenceList : Dictionary<string, EntityReference>
{
  public EntityReferenceList()
  {
  }

  protected EntityReferenceList(
    SerializationInfo serializationInfo,
    StreamingContext streamingContext)
    : base(serializationInfo, streamingContext)
  {
  }

  public void Add(EntityReference entityReference)
  {
    if (entityReference == null)
      return;
    this[entityReference.Code] = entityReference;
  }
}
