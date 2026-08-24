// Decompiled with JetBrains decompiler
// Type: Interop.IMShape.IShapeCom
// Assembly: Interop.IMShape, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: D89360AE-CA24-4DA7-8C37-DC22263AF86B
// Assembly location: D:\IPS\Client\Interop.IMShape.dll

using Interop.CADInterface;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMShape;

[Guid("C6CCF5F4-510F-4F57-83B2-82C18B9EA831")]
[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
[ComImport]
public interface IShapeCom
{
  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Init();

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddModel([MarshalAs(UnmanagedType.Interface), In] Tesselation pTess, [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo);

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddDocument([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] bool bCheckPdm);

  [DispId(4)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void AddConfiguration([MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf, [In] bool bCheckPdm);

  [DispId(5)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunSearchDialogByModel([MarshalAs(UnmanagedType.Interface), In] Tesselation pTess, [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo, [In] int lParentHWND);

  [DispId(6)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunSearchDialogByConfiguration([MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf, [In] int lParentHWND);

  [DispId(7)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunSearchDialogByDoc([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] int lParentHWND);

  [DispId(8)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunSearchDialogFromCad([MarshalAs(UnmanagedType.Interface), In] ICADSystem2 pCadSystem, [In] int lParentHWND);

  [DispId(9)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunSearchDialogFromPdm([MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID, [In] int lParentHWND, out bool pbIsRegistered);

  [DispId(10)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunAddDialogByModel([MarshalAs(UnmanagedType.Interface), In] Tesselation pTess, [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo, [In] int lParentHWND);

  [DispId(11)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunAddDialogByConfiguration([MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf, [In] int lParentHWND);

  [DispId(12)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunAddDialogFromPdm([MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID, [In] int lParentHWND);

  [DispId(13)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunAddDialogByDoc([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] int lParentHWND);

  [DispId(14)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunAddDialogFromCad([MarshalAs(UnmanagedType.Interface), In] ICADSystem2 pCadSystem, [In] int lParentHWND);

  [DispId(15)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunAddFolderDialog([ComAliasName("Interop.IMShape.EComCadType"), In] EComCadType eType, [In] int lParentHWND);

  [DispId(16 /*0x10*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void DeleteModelByPdmArtID([MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID);

  [DispId(17)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void DeleteModelByPath([MarshalAs(UnmanagedType.BStr), In] string bstrPath);

  [DispId(18)]
  string LastError { [DispId(18), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(19)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunAboutDialog([In] int lParentHWND);

  [DispId(20)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void RunAddPdmModelsDialog([MarshalAs(UnmanagedType.Interface), In] PdmModelsList pList, [In] int lParentHWND);

  [DispId(21)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetThumbnailAndPreviewByPdmID(
    [MarshalAs(UnmanagedType.BStr), In] string bstrPdmID,
    [MarshalAs(UnmanagedType.BStr), In] string bstrThumbnailPath,
    [MarshalAs(UnmanagedType.BStr), In] string bstrPreviewPath);

  [DispId(22)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void GetThumbnailAndPreviewByPath(
    [MarshalAs(UnmanagedType.BStr), In] string bstrPath,
    [MarshalAs(UnmanagedType.BStr), In] string bstrConfig,
    [MarshalAs(UnmanagedType.BStr), In] string bstrThumbnailPath,
    [MarshalAs(UnmanagedType.BStr), In] string bstrPreviewPath);
}
