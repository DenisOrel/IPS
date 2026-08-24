// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ReferenceKeyEventsSink
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("4DA70A52-6AE0-4674-95A6-6D7E563CD589")]
[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[ComImport]
public interface ReferenceKeyEventsSink
{
  [DispId(50443137)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnBindKeyToObject(
    [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1), In, Out] ref byte[] ReferenceKey,
    [MarshalAs(UnmanagedType.IDispatch), In] object Document,
    [MarshalAs(UnmanagedType.IDispatch), In, Out] ref object Object,
    out SolutionNatureEnum MatchType,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context,
    [In, Out] ref HandlingCodeEnum HandlingCode);
}
