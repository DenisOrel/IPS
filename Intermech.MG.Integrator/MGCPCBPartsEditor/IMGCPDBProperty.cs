// Decompiled with JetBrains decompiler
// Type: MGCPCBPartsEditor.IMGCPDBProperty
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace MGCPCBPartsEditor;

[CompilerGenerated]
[Guid("4F971C0E-7CC2-4AD9-AE9D-73D79FA95C5E")]
[TypeIdentifier]
[ComImport]
public interface IMGCPDBProperty
{
  [DispId(1)]
  string Name { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap1_1();

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Struct)]
  object get_Value([In] EPDBUnit eUnit = EPDBUnit.epdbUnitCurrent);

  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap2_3();

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void set_Value([In] EPDBUnit eUnit = EPDBUnit.epdbUnitCurrent, [MarshalAs(UnmanagedType.Struct), In] object pValue);
}
