// Decompiled with JetBrains decompiler
// Type: MGCPCBPartsEditor.IMGCPDBPart
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace MGCPCBPartsEditor;

[CompilerGenerated]
[Guid("54A956B5-330B-42C1-B040-195A694159D4")]
[TypeIdentifier]
[ComImport]
public interface IMGCPDBPart
{
  [SpecialName]
  [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
  sealed extern void _VtblGap1_1();

  [DispId(2)]
  Properties Properties { [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
