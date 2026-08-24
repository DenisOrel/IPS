// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.SchemaList
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

[Serializable]
internal class SchemaList : Dictionary<SchemaInfo, MapDocument>
{
  public bool TryGetSchema(SchemaInfo sh, out MapDocument outSchema)
  {
    return this.TryGetValue(sh, out outSchema);
  }
}
