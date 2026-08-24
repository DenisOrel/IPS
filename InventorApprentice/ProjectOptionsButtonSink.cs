// Decompiled with JetBrains decompiler
// Type: InventorApprentice.ProjectOptionsButtonSink
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[TypeLibType(4096 /*0x1000*/)]
[Guid("0C946530-B275-481A-9573-6CA7D4F93611")]
[InterfaceType(2)]
[ComImport]
public interface ProjectOptionsButtonSink
{
  [DispId(50445185)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnClick([MarshalAs(UnmanagedType.Interface), In] NameValueMap Context);
}
