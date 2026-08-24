// Decompiled with JetBrains decompiler
// Type: Interop.Viewdraw.IStringList
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Interop.Viewdraw;

[CompilerGenerated]
[Guid("3566B3FB-97C7-4F45-B1A6-E5469271F9AC")]
[TypeIdentifier]
[ComImport]
public interface IStringList
{
  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int GetCount();

  [DispId(2)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  [return: MarshalAs(UnmanagedType.BStr)]
  string GetItem([In] int Index);
}
