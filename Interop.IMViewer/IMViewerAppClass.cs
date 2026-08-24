// Decompiled with JetBrains decompiler
// Type: Interop.IMViewer.IMViewerAppClass
// Assembly: Interop.IMViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: 1C0DF326-5EF4-4829-91C2-C30ABF7D8DD6
// Assembly location: D:\IPS\Client\Interop.IMViewer.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMViewer;

[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[ClassInterface(ClassInterfaceType.None)]
[Guid("572CD378-F45D-422F-80D1-9F3C1799BC9A")]
[ComImport]
public class IMViewerAppClass : IIMViewerApp, IMViewerApp
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public extern IMViewerAppClass();

  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void OpenDocument([MarshalAs(UnmanagedType.BStr), In] string ViewerFilePath, [MarshalAs(UnmanagedType.BStr), In] string ConfigName);

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void OpenModelMatch(
    [MarshalAs(UnmanagedType.BStr), In] string ViewerFilePath1,
    [MarshalAs(UnmanagedType.BStr), In] string ConfigName1,
    [MarshalAs(UnmanagedType.BStr), In] string ViewerFilePath2,
    [MarshalAs(UnmanagedType.BStr), In] string ConfigName2);

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void OpenAsmAnimation([MarshalAs(UnmanagedType.BStr), In] string ViewerFilePath, [MarshalAs(UnmanagedType.BStr), In] string ConfigName);

  [DispId(4)]
  public virtual extern IIMViewerView ActiveView { [DispId(4), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

  [DispId(5)]
  public virtual extern tagIMViewerViewType ActiveViewType { [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

  [DispId(6)]
  public virtual extern bool EnableMultiTabs { [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

  [DispId(7)]
  public virtual extern bool EnableOpenFile { [DispId(7), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(7), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }
}
