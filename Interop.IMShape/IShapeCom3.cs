// Decompiled with JetBrains decompiler
// Type: Interop.IMShape.IShapeCom3
// Assembly: Interop.IMShape, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: D89360AE-CA24-4DA7-8C37-DC22263AF86B
// Assembly location: D:\IPS\Client\Interop.IMShape.dll

using Interop.CADInterface;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMShape;

[TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
[Guid("0E33A242-BFB1-4DD4-95DA-81748739E1AA")]
[ComImport]
public interface IShapeCom3 : IShapeCom2
{
  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void Init();

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void AddModel([MarshalAs(UnmanagedType.Interface), In] Tesselation pTess, [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo);

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void AddDocument([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] bool bCheckPdm);

  [DispId(4)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void AddConfiguration([MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf, [In] bool bCheckPdm);

  [DispId(5)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunSearchDialogByModel([MarshalAs(UnmanagedType.Interface), In] Tesselation pTess, [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo, [In] int lParentHWND);

  [DispId(6)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunSearchDialogByConfiguration([MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf, [In] int lParentHWND);

  [DispId(7)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunSearchDialogByDoc([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] int lParentHWND);

  [DispId(8)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunSearchDialogFromCad([MarshalAs(UnmanagedType.Interface), In] ICADSystem2 pCadSystem, [In] int lParentHWND);

  [DispId(9)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunSearchDialogFromPdm([MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID, [In] int lParentHWND, out bool pbIsRegistered);

  [DispId(10)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddDialogByModel([MarshalAs(UnmanagedType.Interface), In] Tesselation pTess, [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo, [In] int lParentHWND);

  [DispId(11)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddDialogByConfiguration([MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf, [In] int lParentHWND);

  [DispId(12)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddDialogFromPdm([MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID, [In] int lParentHWND);

  [DispId(13)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddDialogByDoc([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] int lParentHWND);

  [DispId(14)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddDialogFromCad([MarshalAs(UnmanagedType.Interface), In] ICADSystem2 pCadSystem, [In] int lParentHWND);

  [DispId(15)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddFolderDialog([ComAliasName("Interop.IMShape.EComCadType"), In] EComCadType eType, [In] int lParentHWND);

  [DispId(16 /*0x10*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void DeleteModelByPdmArtID([MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID);

  [DispId(17)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void DeleteModelByPath([MarshalAs(UnmanagedType.BStr), In] string bstrPath);

  [DispId(18)]
  new string LastError { [DispId(18), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(19)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAboutDialog([In] int lParentHWND);

  [DispId(20)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddPdmModelsDialog([MarshalAs(UnmanagedType.Interface), In] PdmModelsList pList, [In] int lParentHWND);

  [DispId(21)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void GetThumbnailAndPreviewByPdmID(
    [MarshalAs(UnmanagedType.BStr), In] string bstrPdmID,
    [MarshalAs(UnmanagedType.BStr), In] string bstrThumbnailPath,
    [MarshalAs(UnmanagedType.BStr), In] string bstrPreviewPath);

  [DispId(22)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void GetThumbnailAndPreviewByPath(
    [MarshalAs(UnmanagedType.BStr), In] string bstrPath,
    [MarshalAs(UnmanagedType.BStr), In] string bstrConfig,
    [MarshalAs(UnmanagedType.BStr), In] string bstrThumbnailPath,
    [MarshalAs(UnmanagedType.BStr), In] string bstrPreviewPath);

  [DispId(23)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunSearchDialogByModel2([MarshalAs(UnmanagedType.Interface), In] Tesselation pTess, [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo, [In] long lParentHWND);

  [DispId(24)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunSearchDialogByConfiguration2([MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf, [In] long lParentHWND);

  [DispId(25)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunSearchDialogByDoc2([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] long lParentHWND);

  [DispId(26)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunSearchDialogFromCad2([MarshalAs(UnmanagedType.Interface), In] ICADSystem2 pCadSystem, [In] long lParentHWND);

  [DispId(27)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunSearchDialogFromPdm2([MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID, [In] long lParentHWND, out bool pbIsRegistered);

  [DispId(28)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddDialogByModel2([MarshalAs(UnmanagedType.Interface), In] Tesselation pTess, [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo, [In] long lParentHWND);

  [DispId(29)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddDialogByConfiguration2([MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf, [In] long lParentHWND);

  [DispId(30)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddDialogFromPdm2([MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID, [In] long lParentHWND);

  [DispId(31 /*0x1F*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddDialogByDoc2([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] long lParentHWND);

  [DispId(32 /*0x20*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddDialogFromCad2([MarshalAs(UnmanagedType.Interface), In] ICADSystem2 pCadSystem, [In] long lParentHWND);

  [DispId(33)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddFolderDialog2([ComAliasName("Interop.IMShape.EComCadType"), In] EComCadType eType, [In] long lParentHWND);

  [DispId(34)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAboutDialog2([In] long lParentHWND);

  [DispId(35)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  new void RunAddPdmModelsDialog2([MarshalAs(UnmanagedType.Interface), In] PdmModelsList pList, [In] long lParentHWND);

  [DispId(36)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  void Init2(int lCreatePDMSystemContext);
}
