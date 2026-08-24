// Decompiled with JetBrains decompiler
// Type: InventorApprentice.SurfaceBody
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[DefaultMember("Type")]
[Guid("5DF86089-6B16-11D3-B794-0060B0F159EF")]
[InterfaceType(2)]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface SurfaceBody
{
  [DispId(0)]
  ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706433 /*0x7F000001*/)]
  object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  object Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(67118897)]
  FaceShells FaceShells { [DispId(67118897), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67118898)]
  Faces Faces { [DispId(67118898), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67118899)]
  Edges Edges { [DispId(67118899), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67118915)]
  Vertices Vertices { [DispId(67118915), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67118919)]
  Wires Wires { [DispId(67118919), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67118900)]
  ComponentDefinition ComponentDefinition { [DispId(67118900), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67118901)]
  Box RangeBox { [DispId(67118901), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67118902)]
  bool IsSolid { [DispId(67118902), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67118903)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double get_Volume([In] double PrecisionPercent);

  [DispId(67118904)]
  int GeometryForm { [DispId(67118904), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67118905)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  SurfaceBody get_AlternateBody([In] int AlternateForm);

  [DispId(2130706452)]
  AttributeSets AttributeSets { [DispId(2130706452), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67118909)]
  EdgeCollection ConcaveEdges { [DispId(67118909), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67118910)]
  EdgeCollection ConvexEdges { [DispId(67118910), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706454)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetReferenceKey([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] ReferenceKey, [In] int KeyContext = 0);

  [DispId(2130706464 /*0x7F000020*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CalculateFacets(
    [In] double Tolerance,
    out int VertexCount,
    out int FacetCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] NormalVectors,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] VertexIndices);

  [DispId(2130706465)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingFacets(
    [In] double ToleranceIndex,
    out int VertexCount,
    out int FacetCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] NormalVectors,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] VertexIndices);

  [DispId(2130706466)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingFacetTolerances(out int ToleranceCount, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] ExistingTolerances);

  [DispId(2130706467)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CalculateStrokes(
    [In] double Tolerance,
    out int VertexCount,
    out int SegmentCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] VertexIndices);

  [DispId(2130706469)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingStrokes(
    [In] double ToleranceIndex,
    out int VertexCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    out int PolylineCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] PolylineLengths);

  [DispId(2130706468)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingStrokeTolerances(out int ToleranceCount, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] ExistingTolerances);

  [DispId(67118911)]
  bool Visible { [DispId(67118911), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(67118911), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67118912)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.IDispatch)]
  object LocateUsingPoint(
    [In] ObjectTypeEnum ObjectType,
    [MarshalAs(UnmanagedType.Interface), In] Point PointOnObject,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object ProximityTolerance);

  [DispId(67118913)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void FindUsingRay(
    [MarshalAs(UnmanagedType.Interface), In] Point RayStartPoint,
    [MarshalAs(UnmanagedType.Interface), In] UnitVector RayDirection,
    [In] double Radius,
    [MarshalAs(UnmanagedType.Interface)] out ObjectsEnumerator FoundEntities,
    [MarshalAs(UnmanagedType.Interface)] out ObjectsEnumerator LocationPoints,
    [In] bool FindFirstOnly = false);

  [DispId(67118914)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  ContainmentEnum get_IsPointInside([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Point, [In] bool UseBox = true);

  [DispId(67118916)]
  DataIO DataIO { [DispId(67118916), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67118917)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.IDispatch)]
  object BindTransientKeyToObject([In] int TransientKey);

  [DispId(2130706475)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CalculateFacetsAndTextureMap(
    [In] double Tolerance,
    out int VertexCount,
    out int FacetCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] NormalVectors,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] VertexIndices,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] TextureCoordinates);

  [DispId(2130706476)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingFacetsAndTextureMap(
    [In] double ToleranceIndex,
    out int VertexCount,
    out int FacetCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] NormalVectors,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] VertexIndices,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] TextureCoordinates);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(2130706479)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CalculateFacetsWithOptions(
    [In] double ChordalTolerance,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Options,
    out int VertexCount,
    out int FacetCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] NormalVectors,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] VertexIndices,
    [MarshalAs(UnmanagedType.Struct), Optional] out object TextureCoordinates);

  [DispId(2130706480 /*0x7F000030*/)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingFacetsAndTextureMap2(
    [In] double ChordalTolerance,
    out int VertexCount,
    out int FacetCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] NormalVectors,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] VertexIndices,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] TextureCoordinates,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] IndexCountPerFace);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(2130706481)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CalculateStrokesWithOptions(
    [In] double ChordalTolerance,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Options,
    out int VertexCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8)] out double[] VertexCoordinates,
    out int PolylineCount,
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] out int[] PolylineLengths);

  [DispId(67118918)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Delete();

  [DispId(67118920)]
  bool IsTransient { [DispId(67118920), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(67118921)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool IsEntityValid([MarshalAs(UnmanagedType.Struct), In, Optional] object EntityToCheck, [In] int CheckLevel = 3, [MarshalAs(UnmanagedType.Struct), In, Out, Optional] ref object ProblemEntities);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(67118922)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  RenderStyle GetRenderStyle(out StyleSourceTypeEnum StyleSourceType);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(67118923)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetRenderStyle([In] StyleSourceTypeEnum StyleSourceType, [MarshalAs(UnmanagedType.Struct), In, Optional] object RenderStyle, [In] bool StripOverrides = false);

  [DispId(67118924)]
  PartFeature CreatedByFeature { [DispId(67118924), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(4194932)]
  PartFeaturesEnumerator AffectedByFeatures { [DispId(4194932), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(4194933)]
  string Name { [DispId(4194933), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(4194933), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

  [DispId(4194934)]
  bool Exported { [DispId(4194934), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(4194934), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(4194935)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  NameValueMap GetASMAttributes([MarshalAs(UnmanagedType.Struct), In, Optional] object Entity);

  [DispId(67118925)]
  Asset Appearance { [DispId(67118925), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [DispId(67118925), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(67118926)]
  AppearanceSourceTypeEnum AppearanceSourceType { [DispId(67118926), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(67118926), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(67118927)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void ClearAppearanceOverrides();

  [DispId(67118928)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetAppearanceTextureMappingData(
    out TextureMappingTypeEnum TextureMappingType,
    [MarshalAs(UnmanagedType.Struct), Optional] out object TextureMappingAlignment);

  [DispId(67118929)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetAppearanceTextureMappingData(
    [In] TextureMappingTypeEnum TextureMappingType,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object TextureMappingAlignment);
}
