// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.EvCode
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Flags]
public enum EvCode
{
  NoEvent = 0,
  Object = 1,
  Relation = 16, // 0x00000010
  Created = 4096, // 0x00001000
  Modified = 65536, // 0x00010000
  Deleted = 1048576, // 0x00100000
  IdChanged = 16777216, // 0x01000000
  AllEvents = IdChanged | Deleted | Modified | Created | Relation | Object, // 0x01111011
  ObjectCreated = Created | Object, // 0x00001001
  ObjectModified = Modified | Object, // 0x00010001
  ObjectDeleted = Deleted | Object, // 0x00100001
  RelationCreated = Created | Relation, // 0x00001010
  RelationModified = Modified | Relation, // 0x00010010
  RelationDeleted = Deleted | Relation, // 0x00100010
  ObjectIdChanged = IdChanged | Object, // 0x01000001
}
