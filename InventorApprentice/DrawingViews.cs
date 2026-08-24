// Decompiled with JetBrains decompiler
// Type: InventorApprentice.DrawingViews
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;

#nullable disable
namespace InventorApprentice;

[Guid("206B59B2-22A6-11D4-B7A8-0060B0F159EF")]
[InterfaceType(2)]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface DrawingViews : IEnumerable
{
  [DispId(0)]
  DrawingView this[[In] int index] { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706435 /*0x7F000003*/)]
  ObjectTypeEnum Type { [DispId(2130706435 /*0x7F000003*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706438 /*0x7F000006*/)]
  int Count { [DispId(2130706438 /*0x7F000006*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(-4)]
  [TypeLibFunc(1)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler))]
  new IEnumerator GetEnumerator();

  [DispId(117442311 /*0x07000707*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DrawingView AddBaseView(
    [MarshalAs(UnmanagedType.Interface), In] Document Model,
    [MarshalAs(UnmanagedType.Interface), In] Point2d Position,
    [In] double Scale,
    [In] ViewOrientationTypeEnum ViewOrientation,
    [In] DrawingViewStyleEnum ViewStyle,
    [MarshalAs(UnmanagedType.BStr), In] string ModelViewName = "",
    [MarshalAs(UnmanagedType.Struct), In, Optional] object ArbitraryCamera,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object AdditionalOptions);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(117442306 /*0x07000702*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DrawingView _AddProjectedView(
    [MarshalAs(UnmanagedType.Interface), In] DrawingView ParentView,
    [MarshalAs(UnmanagedType.Interface), In] Point2d Position,
    [In] ViewOrientationTypeEnum ViewOrientation,
    [In] DrawingViewStyleEnum ViewStyle,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Scale);

  [DispId(117442310 /*0x07000706*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DrawingView AddProjectedView(
    [MarshalAs(UnmanagedType.Interface), In] DrawingView ParentView,
    [MarshalAs(UnmanagedType.Interface), In] Point2d Position,
    [In] DrawingViewStyleEnum ViewStyle,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Scale);

  [DispId(117442307 /*0x07000703*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DrawingView AddAssociativeDraftView([MarshalAs(UnmanagedType.Interface), In] Document Model, [MarshalAs(UnmanagedType.Interface), In] Point2d Position, [In] double Scale = 1.0, [MarshalAs(UnmanagedType.BStr), In] string Name = "");

  [DispId(117442308 /*0x07000704*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DrawingView AddDraftView([In] double Scale = 1.0, [MarshalAs(UnmanagedType.BStr), In] string Name = "");

  [DispId(117442309 /*0x07000705*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SectionDrawingView AddSectionView(
    [MarshalAs(UnmanagedType.Interface), In] DrawingView ParentView,
    [MarshalAs(UnmanagedType.Interface), In] DrawingSketch SectionLineSketch,
    [MarshalAs(UnmanagedType.Interface), In] Point2d Position,
    [In] DrawingViewStyleEnum ViewStyle,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Scale,
    [In] bool ShowLabel = true,
    [MarshalAs(UnmanagedType.BStr), In] string Name = "",
    [In] bool Reserved = true,
    [In] bool FullDepth = true,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object SectionDepth);

  [DispId(117442312 /*0x07000708*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DetailDrawingView AddDetailView(
    [MarshalAs(UnmanagedType.Interface), In] DrawingView ParentView,
    [MarshalAs(UnmanagedType.Interface), In] Point2d Position,
    [In] DrawingViewStyleEnum ViewStyle,
    [In] bool CircularFence,
    [MarshalAs(UnmanagedType.Interface), In] Point2d FenceCenterOrCornerOne,
    [MarshalAs(UnmanagedType.Struct), In] object FenceRadiusOrCornerTwo,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object AttachPoint,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Scale,
    [In] bool ShowLabel = true,
    [MarshalAs(UnmanagedType.BStr), In] string Name = "",
    [In] bool Reserved = true);

  [DispId(117442313 /*0x07000709*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DrawingView AddAuxiliaryView(
    [MarshalAs(UnmanagedType.Interface), In] DrawingView ParentView,
    [MarshalAs(UnmanagedType.Interface), In] DrawingCurve OrientationEdge,
    [MarshalAs(UnmanagedType.Interface), In] Point2d Position,
    [In] DrawingViewStyleEnum ViewStyle,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Scale,
    [In] bool ShowLabel = true,
    [MarshalAs(UnmanagedType.BStr), In] string Name = "");

  [DispId(117442314 /*0x0700070A*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DrawingView AddOverlayView(
    [MarshalAs(UnmanagedType.Interface), In] DrawingView ParentView,
    [MarshalAs(UnmanagedType.BStr), In] string PositionalRepresentation,
    [MarshalAs(UnmanagedType.BStr), In] string DesignViewRepresentation,
    [In] bool DesignViewAssociative,
    [In] DrawingViewStyleEnum ViewStyle,
    [In] bool ShowLabel = true,
    [MarshalAs(UnmanagedType.BStr), In] string Name = "");

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(117442305 /*0x07000701*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DrawingView _AddBaseView(
    [MarshalAs(UnmanagedType.Interface), In] Document Model,
    [MarshalAs(UnmanagedType.Interface), In] Point2d Position,
    [In] double Scale,
    [In] ViewOrientationTypeEnum ViewOrientation,
    [In] DrawingViewStyleEnum ViewStyle,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object ViewFileName,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object ViewName,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object ArbitraryCamera,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object AdditionalOptions);
}
