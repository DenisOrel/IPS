// Decompiled with JetBrains decompiler
// Type: InventorApprentice.MeasureTools
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("78B3596A-176A-43F5-A65C-4BDFFC042236")]
[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[ComImport]
public interface MeasureTools
{
  [DispId(2130706433 /*0x7F000001*/)]
  object Application { [DispId(2130706433 /*0x7F000001*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(2130706435 /*0x7F000003*/)]
  ObjectTypeEnum Type { [DispId(2130706435 /*0x7F000003*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  object Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(50413569)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double GetMinimumDistance(
    [MarshalAs(UnmanagedType.IDispatch), In] object EntityOne,
    [MarshalAs(UnmanagedType.IDispatch), In] object EntityTwo,
    [In] InferredTypeEnum EntityOneInferredType = InferredTypeEnum.kNoInference,
    [In] InferredTypeEnum EntityTwoInferredType = InferredTypeEnum.kNoInference,
    [MarshalAs(UnmanagedType.Struct), In, Out, Optional] ref object Context);

  [DispId(50413570)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double GetAngle([MarshalAs(UnmanagedType.IDispatch), In] object EntityOne, [MarshalAs(UnmanagedType.IDispatch), In] object EntityTwo, [MarshalAs(UnmanagedType.Struct), In, Optional] object EntityThree);

  [DispId(50413571)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double GetLoopLength([MarshalAs(UnmanagedType.IDispatch), In] object Curves);
}
