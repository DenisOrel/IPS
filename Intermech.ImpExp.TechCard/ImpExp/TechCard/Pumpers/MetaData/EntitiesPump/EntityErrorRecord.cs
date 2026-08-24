// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntityErrorRecord
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

public class EntityErrorRecord
{
  internal EntityErrorRecord(Entity entity, string message)
  {
    this.Entity = entity.Clone();
    this.Message = message;
  }

  public string Message { get; private set; }

  internal Entity Entity { get; private set; }
}
