// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.IVisObject
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public interface IVisObject
{
  int Level { get; set; }

  Point Org { get; set; }

  Size Size { get; set; }

  Rectangle Rect { get; }
}
