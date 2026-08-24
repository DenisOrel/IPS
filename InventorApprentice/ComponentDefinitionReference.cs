// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ComponentDefinitionReference
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("5DF8601F-6B16-11D3-B794-0060B0F159EF")]
[TypeLibType(4112)]
[ComImport]
public interface ComponentDefinitionReference
{
  [DispId(67113521)]
  ComponentOccurrencesEnumerator ImmediateOccurrences { [DispId(67113521), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67113522)]
  ComponentDefinition ReferencedDefinition { [DispId(67113522), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(67113523)]
  ReferencedFileDescriptor ReferencedFileDescriptor { [DispId(67113523), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
