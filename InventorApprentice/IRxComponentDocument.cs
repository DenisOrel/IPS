// Decompiled with JetBrains decompiler
// Type: InventorApprentice.IRxComponentDocument
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(16 /*0x10*/)]
[Guid("5DF8600C-6B16-11D3-B794-0060B0F159EF")]
[InterfaceType(1)]
[ComImport]
public interface IRxComponentDocument
{
  [DispId(67112961 /*0x04001001*/)]
  IRxEnumComponentDefinitions Definitions { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
