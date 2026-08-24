// Decompiled with JetBrains decompiler
// Type: ImPictEditSrv.IIMPictEdit
// Assembly: Interop.imPictEditSrv, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: 052EF0E2-3D00-4569-98E4-E9080F388C0C
// Assembly location: D:\IPS\Client\Interop.imPictEditSrv.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ImPictEditSrv;

[Guid("5391B521-A8E7-4BE1-A293-6612D099813C")]
[TypeLibType(4160)]
[ComImport]
public interface IIMPictEdit
{
  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int OpenPictureEditor([MarshalAs(UnmanagedType.BStr), In] string exFile, [MarshalAs(UnmanagedType.BStr), In] string parameters, [MarshalAs(UnmanagedType.BStr), In] string directory);

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int ClosePictureEditor();

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int OpenPicture([MarshalAs(UnmanagedType.BStr), In] string fileName, [In] int readOnly);

  [DispId(4)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int CreatePicture([MarshalAs(UnmanagedType.BStr), In] string fileName, [MarshalAs(UnmanagedType.BStr), In] string prototypeName);

  [DispId(5)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int Test();

  [DispId(6)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int ShowOper([MarshalAs(UnmanagedType.BStr), In] string dwgName, [MarshalAs(UnmanagedType.BStr), In] string layerCode, [MarshalAs(UnmanagedType.BStr), In] string nameOper);

  [DispId(7)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int CopyOper([MarshalAs(UnmanagedType.BStr), In] string layerCodeFrom, [MarshalAs(UnmanagedType.BStr), In] string nameOperFrom, [MarshalAs(UnmanagedType.BStr), In] string layerCodeTo, [MarshalAs(UnmanagedType.BStr), In] string nameOper);

  [DispId(8)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int GetDimText([MarshalAs(UnmanagedType.BStr), In, Out] ref string dimText);

  [DispId(9)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int GetText([MarshalAs(UnmanagedType.BStr), In, Out] ref string text);

  [DispId(10)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int SetDraftName([MarshalAs(UnmanagedType.BStr), In] string text);

  [DispId(11)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int GetDraftName([MarshalAs(UnmanagedType.BStr), In, Out] ref string text);

  [DispId(12)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int SetDrawingName([MarshalAs(UnmanagedType.BStr), In] string text);

  [DispId(13)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int GetAcadHWND();

  [DispId(14)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int RestoreACadHWND();

  [DispId(15)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int SavePicture([MarshalAs(UnmanagedType.BStr), In] string name);

  [DispId(16 /*0x10*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int SavePictureAs([MarshalAs(UnmanagedType.BStr), In] string name);

  [DispId(17)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int GetTechCustomer([MarshalAs(UnmanagedType.BStr), In] string fileName, [MarshalAs(UnmanagedType.BStr), In, Out] ref string ttText);

  [DispId(18)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int SetInterfaceObject([MarshalAs(UnmanagedType.IDispatch), In] object io);

  [DispId(19)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int SelectStdElem([MarshalAs(UnmanagedType.BStr), In, Out] ref string imbaseCode);

  [DispId(20)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int ClosePicture([MarshalAs(UnmanagedType.BStr), In] string name);

  [DispId(21)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int GetServerStatus();

  [DispId(201)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int CopyOperFrom([MarshalAs(UnmanagedType.BStr), In] string dwgFrom, [MarshalAs(UnmanagedType.BStr), In] string dwgTo, [MarshalAs(UnmanagedType.Struct), In] object codeList);

  [DispId(202)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int MoveOperFrom([MarshalAs(UnmanagedType.BStr), In] string dwgFrom, [MarshalAs(UnmanagedType.BStr), In] string dwgTo, [MarshalAs(UnmanagedType.Struct), In] object codeList);

  [DispId(203)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int DeleteOper([MarshalAs(UnmanagedType.BStr), In] string dwgName, [MarshalAs(UnmanagedType.Struct), In] object codeList);

  [DispId(204)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int ReplaceDimText([MarshalAs(UnmanagedType.Struct), In] object textList);
}
