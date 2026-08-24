// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ComponentOccurrences
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("5DF86024-6B16-11D3-B794-0060B0F159EF")]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface ComponentOccurrences : IEnumerable
{
  [DispId(0)]
  ComponentOccurrence this[[In] int index] { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(2130706435 /*0x7F000003*/)]
  ObjectTypeEnum Type { [DispId(2130706435 /*0x7F000003*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706438 /*0x7F000006*/)]
  int Count { [DispId(2130706438 /*0x7F000006*/), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [TypeLibFunc(1)]
  [DispId(-4)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof (EnumeratorToEnumVariantMarshaler))]
  new IEnumerator GetEnumerator();

  [DispId(2130706471)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrence get_ItemByName([MarshalAs(UnmanagedType.BStr), In] string Name);

  [DispId(67114545)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrence Add([MarshalAs(UnmanagedType.BStr), In] string FullDocumentName, [MarshalAs(UnmanagedType.Interface), In] Matrix Position);

  [DispId(67114546)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrence AddByComponentDefinition([MarshalAs(UnmanagedType.Interface), In] ComponentDefinition CompDef, [MarshalAs(UnmanagedType.Interface), In] Matrix Position);

  [DispId(67114547)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrence AddiPartMember([MarshalAs(UnmanagedType.BStr), In] string FactoryFileName, [MarshalAs(UnmanagedType.Interface), In] Matrix Position, [MarshalAs(UnmanagedType.Struct), In, Optional] object Row);

  [DispId(67114548)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrence AddCustomiPartMember(
    [MarshalAs(UnmanagedType.BStr), In] string FactoryFileName,
    [MarshalAs(UnmanagedType.Interface), In] Matrix Position,
    [MarshalAs(UnmanagedType.BStr), In] string FullFileName,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Row,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object CustomInput);

  [TypeLibFunc(64 /*0x40*/)]
  [DispId(67114549)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrence _AddUsingiMates([MarshalAs(UnmanagedType.BStr), In] string FullDocumentName, [MarshalAs(UnmanagedType.Interface), In] Matrix Position);

  [DispId(67114555)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrencesEnumerator AddUsingiMates(
    [MarshalAs(UnmanagedType.BStr), In] string FullDocumentName,
    [In] bool PlaceAllMatching = false,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Options);

  [DispId(67114552)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrence AddVirtual([MarshalAs(UnmanagedType.BStr), In] string Name, [MarshalAs(UnmanagedType.Interface), In] Matrix Position);

  [DispId(67114550)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrencesEnumerator get_AllLeafOccurrences([MarshalAs(UnmanagedType.Struct), In, Optional] object LeafDefinition);

  [DispId(67114551)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrencesEnumerator get_AllReferencedOccurrences([MarshalAs(UnmanagedType.IDispatch), In] object Object);

  [DispId(67114553)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrence AddWithOptions(
    [MarshalAs(UnmanagedType.BStr), In] string FullDocumentName,
    [MarshalAs(UnmanagedType.Interface), In] Matrix Position,
    [MarshalAs(UnmanagedType.Interface), In] NameValueMap Options);

  [DispId(67114554)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  ComponentOccurrence AddiAssemblyMember(
    [MarshalAs(UnmanagedType.BStr), In] string FactoryDocumentName,
    [MarshalAs(UnmanagedType.Interface), In] Matrix Position,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Row,
    [MarshalAs(UnmanagedType.Struct), In, Optional] object Options);
}
