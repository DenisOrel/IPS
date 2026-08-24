// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxStrokes
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("DAEA25A5-513E-41CA-BB8F-8E88B507C52E")]
[TypeLibType(16 /*0x10*/)]
[InterfaceType(1)]
[ComConversionLoss]
[ComImport]
public interface IRxStrokes : IRxStrokesOld
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void GetStrokes(
    [In] double ChordalHeightTol,
    [In, Out] ref uint pdwNumVertices,
    [Out] IntPtr ppVertices,
    [In, Out] ref uint pdwNumSegments,
    [Out] IntPtr ppVertexIndices);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingTolerances([In, Out] ref uint pdwNumTols, [Out] IntPtr ppTols);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetExistingStrokes(
    [In] double ChordalHeightTol,
    [In, Out] ref uint pdwNumVertices,
    [Out] IntPtr ppVertices,
    [In, Out] ref uint pdwNumPolyLines,
    [Out] IntPtr ppPolyLineLengths);

  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void CalculateStrokesWithOptions(
    [In] double ChordalTolerance,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Options,
    [In, Out] ref uint VertexCount,
    [Out] IntPtr VertexCoordinates,
    [In, Out] ref uint PolylineCount,
    [Out] IntPtr PolylineLengths);
}
