// Decompiled with JetBrains decompiler
// Type: InventorApprentice.HelpEventsSink
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("39E63B3F-3A40-4735-9C8F-012AFB75F087")]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface HelpEventsSink
{
  [DispId(50442753)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnApplicationHelp([MarshalAs(UnmanagedType.Interface), In] NameValueMap Context, out HandlingCodeEnum HandlingCode);
}
