// Decompiled with JetBrains decompiler
// Type: InventorApprentice.SurfaceGeometryFormEnum
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("5DF86065-6B16-11D3-B794-0060B0F159EF")]
public enum SurfaceGeometryFormEnum
{
  SurfaceGeometryForm_ClosedUVLoops = 1,
  SurfaceGeometryForm_Not_ClosedUVLoops = 2,
  SurfaceGeometryForm_NURBS = 4,
  SurfaceGeometryForm_Not_NURBS = 8,
  SurfaceGeometryForm_ProceduralToNURBS = 16, // 0x00000010
}
