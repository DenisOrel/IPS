// Decompiled with JetBrains decompiler
// Type: InventorApprentice.CameraEventsSink
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[Guid("839AA92C-F073-4BB6-9657-51061150E17C")]
[TypeLibType(4096 /*0x1000*/)]
[ComImport]
public interface CameraEventsSink
{
  [DispId(50435457)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnCameraChange([MarshalAs(UnmanagedType.Interface), In] View View, [In] EventTimingEnum BeforeOrAfter, [MarshalAs(UnmanagedType.Interface), In] NameValueMap Context);
}
