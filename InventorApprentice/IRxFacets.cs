// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxFacets
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComConversionLoss]
[Guid("2894395B-1E28-4516-8308-6AD0911B83D5")]
[TypeLibType(16 /*0x10*/)]
[InterfaceType(1)]
[ComImport]
public interface IRxFacets : IRxFacetsOld
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void GetFacets(
    [In] double ChordalHeightTol,
    [In, Out] ref uint pdwNumVertices,
    [Out] IntPtr ppVertices,
    [Out] IntPtr ppNormals,
    [In, Out] ref uint pdwNumFacets,
    [Out] IntPtr ppVertexIndices);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingTolerances([In, Out] ref uint pdwNumTols, [Out] IntPtr ppTols);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingFacets(
    [In] double ChordalHeightTol,
    [In, Out] ref uint pdwNumVertices,
    [Out] IntPtr ppVertices,
    [Out] IntPtr ppNormals,
    [In, Out] ref uint pdwNumFacets,
    [Out] IntPtr ppVertexIndices);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingFacetsAndTextureMap(
    [In] double ChordalHeightTol,
    [In, Out] ref uint pdwNumVertices,
    [Out] IntPtr ppVertices,
    [Out] IntPtr ppNormals,
    [In, Out] ref uint pdwNumFacets,
    [Out] IntPtr ppVertexIndices,
    [Out] IntPtr ppTextureCoordinates);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CalculateFacetsAndTextureMap(
    [In] double ChordalHeightTol,
    [In, Out] ref uint pdwNumVertices,
    [Out] IntPtr ppVertices,
    [Out] IntPtr ppNormals,
    [In, Out] ref uint pdwNumFacets,
    [Out] IntPtr ppVertexIndices,
    [Out] IntPtr ppTextureCoordinates);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CalculateFacetsWithOptions(
    [In] double ChordalTolerance,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Options,
    [In, Out] ref uint VertexCount,
    [In, Out] ref uint FacetCount,
    [Out] IntPtr VertexCoordinates,
    [Out] IntPtr NormalVectors,
    [Out] IntPtr VertexIndices,
    [Out] IntPtr TextureCoordinates);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingFacetsAndTextureMap2(
    [In] double ChordalTolerance,
    [In, Out] ref uint VertexCount,
    [In, Out] ref uint FacetCount,
    [In, Out] ref uint FaceCount,
    [Out] IntPtr VertexCoordinates,
    [Out] IntPtr NormalVectors,
    [Out] IntPtr VertexIndices,
    [Out] IntPtr TextureCoordinates,
    [Out] IntPtr IndexCountPerFace);
}
