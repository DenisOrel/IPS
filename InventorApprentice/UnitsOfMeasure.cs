// Decompiled with JetBrains decompiler
// Type: InventorApprentice.UnitsOfMeasure
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[DefaultMember("Type")]
[Guid("D007B6F9-71BB-48FF-B14C-EE5D633CB0C3")]
[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[ComImport]
public interface UnitsOfMeasure
{
  [DispId(0)]
  ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  object Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(50346241)]
  UnitsTypeEnum LengthUnits { [DispId(50346241), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50346241), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50346242)]
  UnitsTypeEnum AngleUnits { [DispId(50346242), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50346242), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50346243)]
  UnitsTypeEnum MassUnits { [DispId(50346243), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50346243), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50346244)]
  UnitsTypeEnum TimeUnits { [DispId(50346244), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50346244), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50346247)]
  int LengthDisplayPrecision { [DispId(50346247), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50346247), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50346246)]
  int AngleDisplayPrecision { [DispId(50346246), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50346246), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(50346248)]
  [TypeLibFunc(64 /*0x40*/)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double _GetValueFromExpression([MarshalAs(UnmanagedType.BStr), In] string Expression, [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier);

  [DispId(50346259)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Struct)]
  object GetValueFromExpression([MarshalAs(UnmanagedType.BStr), In] string Expression, [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier);

  [DispId(50346249)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetStringFromValue([In] double Value, [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier);

  [DispId(50346250)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  UnitsTypeEnum GetTypeFromString([MarshalAs(UnmanagedType.BStr), In] string UnitsString);

  [DispId(50346251)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetStringFromType([In] UnitsTypeEnum UnitsType);

  [DispId(50346252)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool CompatibleUnits(
    [MarshalAs(UnmanagedType.BStr), In] string Expression1,
    [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier1,
    [MarshalAs(UnmanagedType.BStr), In] string Expression2,
    [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier2);

  [DispId(50346253)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  double ConvertUnits([In] double Value, [MarshalAs(UnmanagedType.Struct), In] object InputUnitsSpecifier, [MarshalAs(UnmanagedType.Struct), In] object OutputUnitsSpecifier);

  [DispId(50346254)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetLocaleCorrectedExpression([MarshalAs(UnmanagedType.BStr), In] string Expression, [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier);

  [DispId(50346255)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ParametersEnumerator GetDrivingParameters([MarshalAs(UnmanagedType.BStr), In] string Expression);

  [DispId(50346256)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetDatabaseUnitsFromExpression([MarshalAs(UnmanagedType.BStr), In] string Expression, [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier);

  [DispId(50346257)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetPreciseStringFromValue([In] double Value, [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier);

  [DispId(50346258)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  bool IsExpressionValid([MarshalAs(UnmanagedType.BStr), In] string Expression, [MarshalAs(UnmanagedType.Struct), In] object UnitsSpecifier);
}
