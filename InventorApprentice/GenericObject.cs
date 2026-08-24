// Decompiled with JetBrains decompiler
// Type: InventorApprentice.GenericObject
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[Guid("32E4A31B-C5E8-11D2-B77F-0060B0F159EF")]
[TypeLibType(4096 /*0x1000*/)]
[DefaultMember("Type")]
[InterfaceType(2)]
[ComImport]
public interface GenericObject
{
  [DispId(0)]
  ObjectTypeEnum Type { [DispId(0), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(2130706482)]
  ObjectTypeEnum ObjectType { [DispId(2130706482), MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
}
