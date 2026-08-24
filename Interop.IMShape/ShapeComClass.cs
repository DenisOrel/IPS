// Decompiled with JetBrains decompiler
// Type: Interop.IMShape.ShapeComClass
// Assembly: Interop.IMShape, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8d2bc20ab69e4bb4
// MVID: D89360AE-CA24-4DA7-8C37-DC22263AF86B
// Assembly location: D:\IPS\Client\Interop.IMShape.dll

using Interop.CADInterface;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.IMShape;

[TypeLibType(TypeLibTypeFlags.FCanCreate)]
[Guid("2A0D3A11-0DF8-455F-B3DA-7226EC0D45B6")]
[ClassInterface(ClassInterfaceType.None)]
[ComImport]
public class ShapeComClass : IShapeCom, ShapeCom
{
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public extern ShapeComClass();

  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void Init();

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void AddModel([MarshalAs(UnmanagedType.Interface), In] Tesselation pTess, [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo);

  [DispId(3)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void AddDocument([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] bool bCheckPdm);

  [DispId(4)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void AddConfiguration([MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf, [In] bool bCheckPdm);

  [DispId(5)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunSearchDialogByModel(
    [MarshalAs(UnmanagedType.Interface), In] Tesselation pTess,
    [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo,
    [In] int lParentHWND);

  [DispId(6)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunSearchDialogByConfiguration(
    [MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf,
    [In] int lParentHWND);

  [DispId(7)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunSearchDialogByDoc([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] int lParentHWND);

  [DispId(8)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunSearchDialogFromCad([MarshalAs(UnmanagedType.Interface), In] ICADSystem2 pCadSystem, [In] int lParentHWND);

  [DispId(9)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunSearchDialogFromPdm(
    [MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID,
    [In] int lParentHWND,
    out bool pbIsRegistered);

  [DispId(10)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunAddDialogByModel(
    [MarshalAs(UnmanagedType.Interface), In] Tesselation pTess,
    [MarshalAs(UnmanagedType.Interface), In] ShapeItemInfo pInfo,
    [In] int lParentHWND);

  [DispId(11)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunAddDialogByConfiguration(
    [MarshalAs(UnmanagedType.Interface), In] IModelConfiguration2 pConf,
    [In] int lParentHWND);

  [DispId(12)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunAddDialogFromPdm([MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID, [In] int lParentHWND);

  [DispId(13)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunAddDialogByDoc([MarshalAs(UnmanagedType.Interface), In] ICADDocument pDoc, [In] int lParentHWND);

  [DispId(14)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunAddDialogFromCad([MarshalAs(UnmanagedType.Interface), In] ICADSystem2 pCadSystem, [In] int lParentHWND);

  [DispId(15)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunAddFolderDialog([ComAliasName("Interop.IMShape.EComCadType"), In] EComCadType eType, [In] int lParentHWND);

  [DispId(16 /*0x10*/)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void DeleteModelByPdmArtID([MarshalAs(UnmanagedType.BStr), In] string bstrPdmArtID);

  [DispId(17)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void DeleteModelByPath([MarshalAs(UnmanagedType.BStr), In] string bstrPath);

  [DispId(18)]
  public virtual extern string LastError { [DispId(18), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; }

  [DispId(19)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunAboutDialog([In] int lParentHWND);

  [DispId(20)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void RunAddPdmModelsDialog([MarshalAs(UnmanagedType.Interface), In] PdmModelsList pList, [In] int lParentHWND);

  [DispId(21)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void GetThumbnailAndPreviewByPdmID(
    [MarshalAs(UnmanagedType.BStr), In] string bstrPdmID,
    [MarshalAs(UnmanagedType.BStr), In] string bstrThumbnailPath,
    [MarshalAs(UnmanagedType.BStr), In] string bstrPreviewPath);

  [DispId(22)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  public virtual extern void GetThumbnailAndPreviewByPath(
    [MarshalAs(UnmanagedType.BStr), In] string bstrPath,
    [MarshalAs(UnmanagedType.BStr), In] string bstrConfig,
    [MarshalAs(UnmanagedType.BStr), In] string bstrThumbnailPath,
    [MarshalAs(UnmanagedType.BStr), In] string bstrPreviewPath);
}
