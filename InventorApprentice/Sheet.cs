// Decompiled with JetBrains decompiler
// Type: InventorApprentice.Sheet
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4096 /*0x1000*/)]
[DefaultMember("Type")]
[Guid("206B59AE-22A6-11D4-B7A8-0060B0F159EF")]
[InterfaceType(2)]
[ComImport]
public interface Sheet
{
  [DispId(0)]
  ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706433 /*0x7F000001*/)]
  object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  object Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(117441290 /*0x0700030A*/)]
  string Name { [DispId(117441290 /*0x0700030A*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(117441290 /*0x0700030A*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  [DispId(117441282 /*0x07000302*/)]
  string InternalName { [DispId(117441282 /*0x07000302*/), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(117441283 /*0x07000303*/)]
  DrawingViews DrawingViews { [DispId(117441283 /*0x07000303*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441284 /*0x07000304*/)]
  DataIO DataIO { [DispId(117441284 /*0x07000304*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441285 /*0x07000305*/)]
  DrawingSheetSizeEnum Size { [DispId(117441285 /*0x07000305*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(117441285 /*0x07000305*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(117441286 /*0x07000306*/)]
  PageOrientationTypeEnum Orientation { [DispId(117441286 /*0x07000306*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(117441286 /*0x07000306*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(117441288 /*0x07000308*/)]
  double Height { [DispId(117441288 /*0x07000308*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(117441288 /*0x07000308*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(117441287 /*0x07000307*/)]
  double Width { [DispId(117441287 /*0x07000307*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(117441287 /*0x07000307*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(117441291 /*0x0700030B*/)]
  DrawingSheetStatusBits Status { [DispId(117441291 /*0x0700030B*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(117441292 /*0x0700030C*/)]
  DrawingSketches Sketches { [DispId(117441292 /*0x0700030C*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441293 /*0x0700030D*/)]
  PartsLists PartsLists { [DispId(117441293 /*0x0700030D*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441316)]
  Balloons Balloons { [DispId(117441316), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441294 /*0x0700030E*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Activate();

  [DispId(117441295 /*0x0700030F*/)]
  bool ExcludeFromCount { [DispId(117441295 /*0x0700030F*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(117441295 /*0x0700030F*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(117441296 /*0x07000310*/)]
  bool ExcludeFromPrinting { [DispId(117441296 /*0x07000310*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(117441296 /*0x07000310*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(117441297)]
  TitleBlock TitleBlock { [DispId(117441297), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441300)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  TitleBlock AddTitleBlock(
    [MarshalAs(UnmanagedType.Struct), In] object TitleBlockDefinition,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object TitleBlockLocation,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object PromptStrings);

  [DispId(117441298)]
  Border Border { [DispId(117441298), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441315)]
  CustomTables CustomTables { [DispId(117441315), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441317)]
  RevisionTables RevisionTables { [DispId(117441317), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441318)]
  HoleTables HoleTables { [DispId(117441318), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441319)]
  DrawingNotes DrawingNotes { [DispId(117441319), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441301)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Border AddBorder([MarshalAs(UnmanagedType.Struct), In] object BorderDefinition, [MarshalAs(UnmanagedType.Struct), In, Optional] object PromptStrings);

  [DispId(117441302)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DefaultBorder AddDefaultBorder(
    [MarshalAs(UnmanagedType.Struct), In, Optional] object HorizontalZoneCount,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object HorizontalZoneLabelMode,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object VerticalZoneCount,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object VerticalZoneLabelMode,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object LabelFromBottomRight,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object DelimitByLines,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object CenterMarks,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object TopMargin,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object BottomMargin,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object LeftMargin,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object RightMargin,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object TextStyle,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object TextLayer,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object LineLayer);

  [DispId(117441299)]
  SketchedSymbols SketchedSymbols { [DispId(117441299), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441331)]
  AutoCADBlocks AutoCADBlocks { [DispId(117441331), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441303)]
  ClientViews ClientViews { [DispId(117441303), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441305)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Sheet CopyTo([MarshalAs(UnmanagedType.Interface), In] DrawingDocument TargetDocument);

  [DispId(117441313)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Delete([In] bool RetainDependentViews = false);

  [DispId(117441321)]
  ClientGraphicsCollection ClientGraphicsCollection { [DispId(117441321), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441322)]
  GraphicsDataSetsCollection GraphicsDataSetsCollection { [DispId(117441322), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441326)]
  bool IsModelSpaceSheet { [DispId(117441326), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(117441327)]
  FeatureControlFrames FeatureControlFrames { [DispId(117441327), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441328 /*0x07000330*/)]
  SurfaceTextureSymbols SurfaceTextureSymbols { [DispId(117441328 /*0x07000330*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706452)]
  AttributeSets AttributeSets { [DispId(2130706452), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706454)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetReferenceKey([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] ReferenceKey, [In] int KeyContext = 0);

  [DispId(117441314)]
  DrawingDimensions DrawingDimensions { [DispId(117441314), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441320)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  GeometryIntent CreateGeometryIntent([MarshalAs(UnmanagedType.IDispatch), In] object Geometry, [MarshalAs(UnmanagedType.Struct), In, Optional] object Intent);

  [DispId(117441323)]
  CenterMarks CenterMarks { [DispId(117441323), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441324)]
  Centerlines Centerlines { [DispId(117441324), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(117441325)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ObjectsEnumerator FindUsingPoint([MarshalAs(UnmanagedType.Interface), In] Point2d PointOnSheet, [MarshalAs(UnmanagedType.Struct), In, Optional] object ProximityTolerance);

  [DispId(117441329)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Update();

  [DispId(117441330)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ChangeLayer([MarshalAs(UnmanagedType.Interface), In] ObjectCollection Objects, [MarshalAs(UnmanagedType.Interface), In] Layer Layer);

  [DispId(117441304)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void _SelectByONK([MarshalAs(UnmanagedType.BStr), In] string ObjectNameKey);

  [DispId(117441281 /*0x07000301*/)]
  string _DisplayName { [DispId(117441281 /*0x07000301*/), TypeLibFunc(64 /*0x40*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }
}
