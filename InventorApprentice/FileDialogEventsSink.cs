// Decompiled with JetBrains decompiler
// Type: InventorApprentice.FileDialogEventsSink
// Assembly: InventorApprentice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D0FA8D60-444C-4AF2-8B56-3FFB3EB81E4B
// Assembly location: D:\IPS\Client\InventorApprentice.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace InventorApprentice;

[InterfaceType(2)]
[TypeLibType(4096 /*0x1000*/)]
[Guid("BF078925-9AC1-485E-9638-4DE87CABBCB7")]
[ComImport]
public interface FileDialogEventsSink
{
  [DispId(50414769)]
  [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void OnOptions([MarshalAs(UnmanagedType.Interface), In] NameValueMap Context, out HandlingCodeEnum HandlingCode);
}
