// Decompiled with JetBrains decompiler
// Type: InventorApprentice._DocPerformanceMonitorObject
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("AAF23B5F-E2FE-471C-85AA-E37FCE6DE651")]
[InterfaceType(2)]
[TypeLibType(4112)]
[ComImport]
public interface _DocPerformanceMonitorObject
{
  [DispId(50377217)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetGraphicsLOD(out int NumberOfLODs, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] LODTolerances);

  [DispId(50377218)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetGraphicsLOD([In] int NumberOfLODs);

  [DispId(50377219)]
  int SegmentCount { [DispId(50377219), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377220)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  _SegPerformanceMonitor get_SegmentItem([MarshalAs(UnmanagedType.Struct), In] object index);

  [DispId(50377221)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetMemoryInUse(out int Committed, out int Reserved, out int InActiveUse);

  [DispId(50377222)]
  int NodeCount { [DispId(50377222), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377223)]
  int ASMHistoryMemory { [DispId(50377223), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377224)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetDetectLeaks([In] bool detect);

  [DispId(50377225)]
  bool LeakDetected { [DispId(50377225), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377226)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void DontSaveIdentifiedMTLeaks([In] bool dontSave);
}
