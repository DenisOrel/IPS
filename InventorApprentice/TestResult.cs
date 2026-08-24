// Decompiled with JetBrains decompiler
// Type: InventorApprentice.TestResult
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4112)]
[InterfaceType(2)]
[Guid("D88ABC2A-BA2E-4E03-AABE-E052F004A177")]
[ComImport]
public interface TestResult
{
  [DispId(2130706434 /*0x7F000002*/)]
  TestCase Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(50380801 /*0x0300C001*/)]
  string ValidationText { [DispId(50380801 /*0x0300C001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50380802 /*0x0300C002*/)]
  bool Succeeded { [DispId(50380802 /*0x0300C002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50380803 /*0x0300C003*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool get_Compare([MarshalAs(UnmanagedType.Interface), In] TestResult pBaselineResult);
}
