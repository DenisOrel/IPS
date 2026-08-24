// Decompiled with JetBrains decompiler
// Type: InventorApprentice.GraphicsNormalSet
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("D8297E9E-DD8B-4C8D-9271-186CAE8E00C1")]
[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[ComImport]
public interface GraphicsNormalSet
{
  [DispId(2130706435 /*0x7F000003*/)]
  ObjectTypeEnum Type { [DispId(2130706435 /*0x7F000003*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706438 /*0x7F000006*/)]
  int Count { [DispId(2130706438 /*0x7F000006*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50360065)]
  int Id { [DispId(50360065), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(50360066)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Remove([In] int index);

  [DispId(50360067)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Delete();

  [DispId(0)]
  [IndexerName("Normal")]
  UnitVector this[[In] int index] { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.Interface), In] set; }

  [DispId(50360577)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Add([In] int index, [MarshalAs(UnmanagedType.Interface), In] UnitVector Normal);

  [DispId(50360578)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetNormals([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Normals);

  [DispId(50360579)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PutNormals([MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_R8), In, Out] ref double[] Normals);
}
