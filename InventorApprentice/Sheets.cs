// Decompiled with JetBrains decompiler
// Type: InventorApprentice.Sheets
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("206B59AF-22A6-11D4-B7A8-0060B0F159EF")]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface Sheets : IEnumerable
{
  [DispId(0)]
  Sheet this[[MarshalAs(UnmanagedType.Struct), In] object index] { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706435 /*0x7F000003*/)]
  ObjectTypeEnum Type { [DispId(2130706435 /*0x7F000003*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706438 /*0x7F000006*/)]
  int Count { [DispId(2130706438 /*0x7F000006*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [TypeLibFunc(1)]
  [DispId(-4)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler))]
  new IEnumerator GetEnumerator();

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(117442561 /*0x07000801*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Sheet _Add([MarshalAs(UnmanagedType.Interface), In] Sheet SheetToCopy);

  [DispId(2130706433 /*0x7F000001*/)]
  object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(117442562 /*0x07000802*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Sheet Add(
    [In] DrawingSheetSizeEnum Size = DrawingSheetSizeEnum.kCDrawingSheetSize,
    [In] PageOrientationTypeEnum Orientation = PageOrientationTypeEnum.kLandscapePageOrientation,
    [MarshalAs(UnmanagedType.BStr), In] string SheetName = "",
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Width,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Height);

  [DispId(117442563 /*0x07000803*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Sheet AddUsingSheetFormat(
    [MarshalAs(UnmanagedType.Interface), In] SheetFormat SheetFormat,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Model,
    [MarshalAs(UnmanagedType.BStr), In] string SheetName = "",
    [MarshalAs(UnmanagedType.Struct), In, Optional] object AdditionalOptions,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object TitleBlockPromptStrings,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object BorderPromptStrings);
}
