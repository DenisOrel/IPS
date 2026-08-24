// Decompiled with JetBrains decompiler
// Type: MGCPCBAutomationLicensing.IApplication
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace MGCPCBAutomationLicensing;

[CompilerGenerated]
[Guid("11A7542F-3B50-45C9-B40A-BB9DFFD701F5")]
[TypeIdentifier]
[ComImport]
public interface IApplication
{
  [DispId(1)]
  [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
  int GetToken([In] int nKey);
}
