// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.PEFileImportService
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.Interfaces;
using Intermech.Tools.DataExchange;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System.IO;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class PEFileImportService(IIntegrator owner, CADCaptureChangesFactory factory) : 
  CADFileImportSupportService(owner, factory)
{
  protected override bool DoCheckCanImportFile(FileInfo fileInfo, Stream fileContent)
  {
    return !StringsHelper.IsNumericFileExtension(fileInfo.FullName) ? base.DoCheckCanImportFile(fileInfo, fileContent) : throw new FaultException(string.Format(Localization.rm.GetString("ProEngineer.Integrator_1"), (object) fileInfo.Name));
  }

  protected override void OnBeforeImportFile(string fullPath)
  {
    base.OnBeforeImportFile(fullPath);
    new CheckNoVersionFilesTask(fullPath).Perform();
  }

  protected override void OnAfterImportFile(CaptureChangesResult result)
  {
    base.OnAfterImportFile(result);
    new CheckNoVersionFilesTask(result.FullPath).Perform();
  }
}
