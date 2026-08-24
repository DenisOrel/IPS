// Decompiled with JetBrains decompiler
// Type: InventorApprentice._AppPerformanceMonitor
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4112)]
[Guid("DDC2F383-3824-49E3-837C-7387D4775893")]
[InterfaceType(2)]
[ComImport]
public interface _AppPerformanceMonitor
{
  [DispId(50376961)]
  int AllMemoryInUse { [DispId(50376961), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50376962)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetMemoryInUse(out int Committed, out int Reserved);

  [DispId(50376963)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetGraphicsLOD([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] LODTolerances);

  [DispId(50376964)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetASMMemoryTotals(
    out int Allocations,
    out int DeAllocations,
    out int BytesAllocated,
    out int HighWaterMark);

  [DispId(50376965)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetASMFreeListMemory(
    out int Blocks,
    out int EmptyBlocks,
    out int TotalBytes,
    out int AllocatedBytes);

  [DispId(50376966)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetASMMemoryUtilizationRatios(out double Gross, out double Capacity, out double Theoretical);

  [DispId(50376967)]
  int ASMActiveEntityMemory { [DispId(50376967), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50376968)]
  int ASMHistoryMemory { [DispId(50376968), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50376972)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void InitStats();

  [DispId(50376969)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void StartTimer([MarshalAs(UnmanagedType.BStr), In] string TimerName);

  [DispId(50376970)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void LogElapsedTime([MarshalAs(UnmanagedType.BStr), In] string TimerName);

  [DispId(50376971)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void LogMemoryStatistics([MarshalAs(UnmanagedType.BStr), In] string MemStatName);

  [DispId(50376973)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string OutputMetrics();

  [DispId(50376974)]
  bool StatsActive { [DispId(50376974), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50376974), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50376975)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OutputElapsedTime([MarshalAs(UnmanagedType.BStr), In] string TimerName, out int ElapsedTime);

  [DispId(50376976)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OutputMemoryStatistics(
    [MarshalAs(UnmanagedType.BStr), In] string EntryName,
    [MarshalAs(UnmanagedType.BStr)] out string CommittedMem,
    [MarshalAs(UnmanagedType.BStr)] out string ReservedMem,
    [MarshalAs(UnmanagedType.BStr)] out string WastedMem);
}
