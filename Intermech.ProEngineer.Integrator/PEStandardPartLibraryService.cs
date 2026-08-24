// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.PEStandardPartLibraryService
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.Interfaces;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class PEStandardPartLibraryService(IIntegrator owner) : CADStandardPartLibraryService(owner, StandardLibraryMode.SeparateStandardSizes, PEConsts.StandardLibrary)
{
  protected override void DoPrepareToImportCadmechLibrary(string directoryPath)
  {
    base.DoPrepareToImportCadmechLibrary(directoryPath);
    ServiceUtils.GetService<IPackAndGoService>((object) this.Integrator, true).AdaptDocumentCopy(directoryPath, false);
  }
}
