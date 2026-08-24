// Decompiled with JetBrains decompiler
// Type: InventorApprentice.Tolerance
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("77B88412-A66B-43BE-BEE2-06CFE38B0C70")]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface Tolerance
{
  [DispId(2130706435 /*0x7F000003*/)]
  ObjectTypeEnum Type { [DispId(2130706435 /*0x7F000003*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  object Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(50377985)]
  ToleranceTypeEnum ToleranceType { [DispId(50377985), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377986)]
  double Upper { [DispId(50377986), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377987)]
  double Lower { [DispId(50377987), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50377988)]
  string HoleTolerance { [DispId(50377988), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50377989)]
  string ShaftTolerance { [DispId(50377989), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(50377990)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToDefault();

  [DispId(50377991)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToDeviation([MarshalAs(UnmanagedType.Struct), In] object UpperTolerance, [MarshalAs(UnmanagedType.Struct), In] object LowerTolerance);

  [DispId(50377992)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToFits([In] ToleranceTypeEnum FitsToleranceType, [MarshalAs(UnmanagedType.BStr), In] string HoleTolerance, [MarshalAs(UnmanagedType.BStr), In] string ShaftTolerance);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50377993)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void _SetToLimits([MarshalAs(UnmanagedType.Struct), In] object UpperTolerance, [MarshalAs(UnmanagedType.Struct), In] object LowerTolerance);

  [DispId(50377997)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToLimits(
    [In] ToleranceTypeEnum LimitsToleranceType,
    [MarshalAs(UnmanagedType.Struct), In] object UpperTolerance,
    [MarshalAs(UnmanagedType.Struct), In] object LowerTolerance);

  [DispId(50377994)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToMinMax([In] ToleranceTypeEnum MinMaxToleranceType, [MarshalAs(UnmanagedType.Struct), In, Optional] object DeviationValue);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(50377995)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToShowFits([In] ToleranceTypeEnum FitsToleranceType, [MarshalAs(UnmanagedType.BStr), In] string HoleOrShaftTolerance);

  [DispId(50377996)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToSymmetric([MarshalAs(UnmanagedType.Struct), In] object Tolerance);

  [DispId(50377998)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToMin();

  [DispId(50377999)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToMax();

  [DispId(50378000)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToBasic();

  [DispId(50378001)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void SetToReference();
}
