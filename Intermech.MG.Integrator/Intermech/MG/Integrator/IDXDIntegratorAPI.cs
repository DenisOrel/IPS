// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.IDXDIntegratorAPI
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Interop.Viewdraw;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

[ComVisible(true)]
[Guid("B92B3F18-87C1-4F18-A9ED-18558FAA5EE6")]
[InterfaceType(ComInterfaceType.InterfaceIsDual)]
public interface IDXDIntegratorAPI
{
  void CreateElementList(IVdApp application);

  void CreateSpecification(IVdApp application);

  void ImportProject(IVdApp application);

  void SaveChanges(IVdApp application);

  void ImbaseBinding(IVdApp application);

  void ViewDocumentProperties(IVdApp application);

  int ErrorCode { get; }

  string ErrorMessage { get; }
}
