// Decompiled with JetBrains decompiler
// Type: InventorApprentice.TransientObjects
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[Guid("6BA435D7-BBD6-11D4-8DE6-0010B541CAA8")]
[ComImport]
public interface TransientObjects
{
  [DispId(50350337)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  TranslationContext CreateTranslationContext();

  [DispId(50350338)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  DataMedium CreateDataMedium();

  [DispId(50350339)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  NameValueMap CreateNameValueMap();

  [DispId(50350340)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ObjectCollection CreateObjectCollection([MarshalAs(UnmanagedType.Struct), In, Optional] object ObjectsEnumerator);

  [DispId(50350341)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ObjectCollectionByVariant CreateObjectCollectionByVariant([MarshalAs(UnmanagedType.Struct), In, Optional] object ObjectsEnumeratorByVariant);

  [DispId(50350342)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  EdgeCollection CreateEdgeCollection([MarshalAs(UnmanagedType.Struct), In, Optional] object ObjectsEnumerator);

  [DispId(50350344)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  FaceCollection CreateFaceCollection([MarshalAs(UnmanagedType.Struct), In, Optional] object ObjectsEnumerator);

  [DispId(50350343)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Color CreateColor([In] byte Red, [In] byte Green, [In] byte Blue, [In] double Opacity = 1.0);

  [DispId(50350345)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string CreateSignatureString([MarshalAs(UnmanagedType.BStr), In] string StringToSign);

  [DispId(50350346)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  FileMetadata CreateFileMetadata([MarshalAs(UnmanagedType.Struct), In, Optional] object FullFileName);

  [DispId(50350347)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Camera CreateCamera();
}
