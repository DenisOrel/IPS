// Decompiled with JetBrains decompiler
// Type: InventorApprentice.AttributeManager
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4096 /*0x1000*/)]
[InterfaceType(2)]
[DefaultMember("Type")]
[Guid("46D51BD4-B58D-4C94-BA7A-124B184AC687")]
[ComImport]
public interface AttributeManager
{
  [DispId(0)]
  ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706434 /*0x7F000002*/)]
  object Parent { [DispId(2130706434 /*0x7F000002*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IDispatch)] get; }

  [DispId(50351873)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  AttributesEnumerator FindAttributes(
    [MarshalAs(UnmanagedType.BStr), In] string AttributeSetName = "*",
    [MarshalAs(UnmanagedType.BStr), In] string AttributeName = "*",
    [MarshalAs(UnmanagedType.Struct), In, Optional] object AttributeValue);

  [DispId(50351874)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  AttributeSetsEnumerator FindAttributeSets(
    [MarshalAs(UnmanagedType.BStr), In] string AttributeSetName = "*",
    [MarshalAs(UnmanagedType.BStr), In] string AttributeName = "*",
    [MarshalAs(UnmanagedType.Struct), In, Optional] object AttributeValue);

  [DispId(50351875)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ObjectCollection FindObjects(
    [MarshalAs(UnmanagedType.BStr), In] string AttributeSetName = "*",
    [MarshalAs(UnmanagedType.BStr), In] string AttributeName = "*",
    [MarshalAs(UnmanagedType.Struct), In, Optional] object AttributeValue);

  [DispId(50351876)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ObjectCollection FindObjectsByPattern([MarshalAs(UnmanagedType.BStr), In] string XPath);

  [DispId(50351877)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void PurgeAttributeSets([MarshalAs(UnmanagedType.BStr), In] string AttributeSetName = "*", [In] bool Preview = false, [MarshalAs(UnmanagedType.Struct), Optional] out object PreviewResult);

  [DispId(50351881)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  AttributeSetsEnumerator OpenAttributeSets([MarshalAs(UnmanagedType.Interface), In] ObjectCollection Objects, [MarshalAs(UnmanagedType.BStr), In] string AttributeSetName);

  [DispId(50351882)]
  string RevisionId { [DispId(50351882), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }
}
