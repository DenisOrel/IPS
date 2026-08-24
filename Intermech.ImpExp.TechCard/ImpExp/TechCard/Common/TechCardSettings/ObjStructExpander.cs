// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechCardSettings.ObjStructExpander
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.TechCardSettings;

internal abstract class ObjStructExpander
{
  public abstract List<ArtInfoLight> CollectObjsFromObjsStructures(List<ArtInfoLight> headObjsInfo);
}
