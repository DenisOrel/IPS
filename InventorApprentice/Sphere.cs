// Decompiled with JetBrains decompiler
// Type: InventorApprentice.Sphere
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4096 /*0x1000*/)]
[Guid("5DF860A5-6B16-11D3-B794-0060B0F159EF")]
[InterfaceType(2)]
[ComImport]
public interface Sphere
{
  [DispId(67123761)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetSphereData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] CenterPoint, out double Radius);

  [DispId(67123762)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutSphereData([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] CenterPoint, [In] double Radius);

  [DispId(67123763)]
  Point CenterPoint { [DispId(67123763), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [DispId(67123763), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67123764)]
  double Radius { [DispId(67123764), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(67123764), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67123768)]
  SurfaceEvaluator Evaluator { [DispId(67123768), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67123769)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Sphere Copy();
}
