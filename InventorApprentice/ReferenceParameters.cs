// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ReferenceParameters
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4096 /*0x1000*/)]
[Guid("1304BB1D-95AE-4738-80F8-CCCA1ABCFF6B")]
[InterfaceType(2)]
[ComImport]
public interface ReferenceParameters : IEnumerable
{
  [DispId(0)]
  ReferenceParameter this[[MarshalAs(UnmanagedType.Struct), In] object index] { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706435 /*0x7F000003*/)]
  ObjectTypeEnum Type { [DispId(2130706435 /*0x7F000003*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706438 /*0x7F000006*/)]
  int Count { [DispId(2130706438 /*0x7F000006*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [TypeLibFunc(1)]
  [DispId(-4)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler))]
  new IEnumerator GetEnumerator();

  [DispId(50347521)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ReferenceParameter AddByExpression([MarshalAs(UnmanagedType.BStr), In] string Expression, [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier, [MarshalAs(UnmanagedType.BStr), In] string Name = "");

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50347522)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ReferenceParameter _AddByValue([In] double Value, [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier, [MarshalAs(UnmanagedType.BStr), In] string Name = "");

  [DispId(50347523)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ReferenceParameter AddByValue([MarshalAs(UnmanagedType.Struct), In] object Value, [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier, [MarshalAs(UnmanagedType.BStr), In] string Name = "");
}
