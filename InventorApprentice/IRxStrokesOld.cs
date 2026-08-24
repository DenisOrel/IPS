// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxStrokesOld
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[ComConversionLoss]
[Guid("CB69F15A-558E-11D3-B793-0060B0F159EF")]
[TypeLibType(16 /*0x10*/)]
[InterfaceType(1)]
[ComImport]
public interface IRxStrokesOld
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetStrokes(
    [In] double ChordalHeightTol,
    [In, Out] ref uint pdwNumVertices,
    [Out] IntPtr ppVertices,
    [In, Out] ref uint pdwNumSegments,
    [Out] IntPtr ppVertexIndices);
}
