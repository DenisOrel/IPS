// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisOperations
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public static class VisOperations
{
  public static void Shift(this Point a, Size shift)
  {
    a.X += shift.Width;
    a.Y += shift.Height;
  }

  public static PointF ShiftF(this PointF a, SizeF shift)
  {
    return new PointF(a.X + shift.Width, a.Y + shift.Height);
  }
}
