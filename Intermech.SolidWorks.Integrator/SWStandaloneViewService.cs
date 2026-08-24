// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWStandaloneViewService
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.CADInterface.Proxies;
using Intermech.IO;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using Intermech.Tools.Integrators.StandaloneView;
using System;
using System.IO;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SWStandaloneViewService(IIntegrator owner) : StandaloneViewService(owner)
{
  protected override void DoInjectViewDataIntoAlreadyOpenFile(
    StandaloneViewDataInjectionOperation operation)
  {
    CADDocumentProxy document = ((CADOpenDocumentAdapter) ((StandaloneViewService.OpenDocumentData) operation.CustomData).Document).Document;
    int num = document.ReadOnly ? 1 : 0;
    if (num != 0)
    {
      FileUtils.SetReadOnlyAttribute(operation.Parameters.FilePath, false);
      document.ReadOnly = false;
    }
    DateTime? nullable = new DateTime?();
    this.DoWriteViewDataIntoOpenFile(operation);
    if (document.Modified)
    {
      nullable = new DateTime?(File.GetLastWriteTimeUtc(operation.Parameters.FilePath));
      document.Save();
    }
    if (num != 0)
    {
      document.ReadOnly = true;
      if (nullable.HasValue)
        File.SetLastWriteTimeUtc(operation.Parameters.FilePath, nullable.Value);
      FileUtils.SetReadOnlyAttribute(operation.Parameters.FilePath, true);
    }
    else
    {
      if (!nullable.HasValue)
        return;
      File.SetLastWriteTimeUtc(operation.Parameters.FilePath, nullable.Value);
    }
  }
}
