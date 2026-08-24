// Decompiled with JetBrains decompiler
// Type: MGCPCBPartsEditor.IMGCPDBPartsDB
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace MGCPCBPartsEditor;

[CompilerGenerated]
[Guid("74DB39D9-41B3-4B08-B891-E58525744087")]
[TypeIdentifier]
[ComImport]
public interface IMGCPDBPartsDB
{
  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap1_1();

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.Interface)]
  Partitions get_Partitions([MarshalAs(UnmanagedType.BStr), In] string sPartitionName = "*");
}
