// Decompiled with JetBrains decompiler
// Type: NormaCSAPI.IApplication
// Assembly: Intermech.NormaCSIntegrator.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: BC215C8E-677A-43E5-99F7-5ED2ECAA0726
// Assembly location: D:\IPS\Client\Intermech.NormaCSIntegrator.Client.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace NormaCSAPI;

[CompilerGenerated]
[Guid("16B9830C-F121-4548-8D94-EA6CCCF3CE38")]
[TypeIdentifier]
[ComImport]
public interface IApplication
{
  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void StartDocumentSearchByNumber([MarshalAs(UnmanagedType.BStr), In] string DocumentNumberPattern);

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void StartDocumentSearchByTitle([MarshalAs(UnmanagedType.BStr), In] string DocumentTitlePattern);

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void StartDocumentSearchByText([MarshalAs(UnmanagedType.BStr), In] string DocumentTextPattern);

  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap1_3();

  [DispId(5)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Launch();

  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap2_15();

  [DispId(16 /*0x10*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Connect();

  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap3_9();

  [DispId(28)]
  bool IsConnected { [DispId(28), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
}
