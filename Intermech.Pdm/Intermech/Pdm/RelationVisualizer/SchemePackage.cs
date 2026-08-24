// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.SchemePackage
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public struct SchemePackage(VisScheme sch, MapDocument doc)
{
  public VisScheme scheme = sch;
  public MapDocument document = doc;
}
