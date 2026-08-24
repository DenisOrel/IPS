// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.IExPCBIntegratorAPI
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using MGCPCB;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.MG.Integrator;

[ComVisible(true)]
[Guid("74CD9ADD-9E7D-40FB-A240-250E1DCB6B2A")]
[InterfaceType(ComInterfaceType.InterfaceIsDual)]
public interface IExPCBIntegratorAPI
{
  void CreateElementList(Application application);

  void CreateSpecification(Application application);

  void ImportProject(Application application);

  void SaveChanges(Application application);

  int ImbaseBinding(Application application);

  void ViewDocumentProperties(Application application);

  int ErrorCode { get; }

  string ErrorMessage { get; }
}
