// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxReferencedOLEFileDescriptor
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("9CAF9896-33EA-11D3-B78F-0060B0F159EF")]
[InterfaceType(1)]
[ComImport]
public interface IRxReferencedOLEFileDescriptor
{
  [DispId(50342407)]
  OLEDocumentTypeEnum OLEDocumentType { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50342408)]
  string LogicalName { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50342409)]
  string DisplayName { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50342410)]
  string FullFileName { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Activate([In] OLEVerbEnum Verb, [MarshalAs(UnmanagedType.IUnknown)] out object ppOLEDocumentObject);
}
