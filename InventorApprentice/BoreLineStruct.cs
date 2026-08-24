// Decompiled with JetBrains decompiler
// Type: InventorApprentice.BoreLineStruct
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BoreLineStruct
{
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = (UnmanagedType) 0)]
  public double[] m_point;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = (UnmanagedType) 0)]
  public double[] m_direction;
  public double m_front;
  public double m_back;
  public double m_radius;
}
