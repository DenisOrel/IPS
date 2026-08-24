// Decompiled with JetBrains decompiler
// Type: InventorApprentice.TestProgram
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("52D6C08A-B387-4F4C-A2E3-4F3CFFF276CE")]
[TypeLibType(4112)]
[InterfaceType(2)]
[ComImport]
public interface TestProgram
{
  [DispId(50380290)]
  string Name { [DispId(50380290), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50380291)]
  string Description { [DispId(50380291), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50380292)]
  string Path { [DispId(50380292), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50380289)]
  string Type { [DispId(50380289), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50380294)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Run([MarshalAs(UnmanagedType.IDispatch), In] object TestObject, [MarshalAs(UnmanagedType.Interface), In] TestInputOutput TestIO, [In] bool bDebug);

  [DispId(50380293)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ActivatePane([In] bool bDebug);
}
