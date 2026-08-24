// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxFacetsOld
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("CB69F159-558E-11D3-B793-0060B0F159EF")]
[InterfaceType(1)]
[ComConversionLoss]
[TypeLibType(16 /*0x10*/)]
[ComImport]
public interface IRxFacetsOld
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetFacets(
    [In] double ChordalHeightTol,
    [In, Out] ref uint pdwNumVertices,
    [Out] IntPtr ppVertices,
    [Out] IntPtr ppNormals,
    [In, Out] ref uint pdwNumFacets,
    [Out] IntPtr ppVertexIndices);
}
