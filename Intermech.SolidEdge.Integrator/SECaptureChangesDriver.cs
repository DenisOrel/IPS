// Decompiled with JetBrains decompiler
// Type: Intermech.SolidEdge.Integrator.SECaptureChangesDriver
// Assembly: Intermech.SolidEdge.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 213B90F8-0434-43B8-B8F6-9AF19E139193
// Assembly location: D:\IPS\Client\Intermech.SolidEdge.Integrator.dll

using Intermech.CADInterface.Proxies;
using Intermech.Data.SectionEntities;
using Intermech.Tools.DataExchange;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System.Collections.Generic;

#nullable disable
namespace Intermech.SolidEdge.Integrator;

internal sealed class SECaptureChangesDriver(IIntegrator integrator) : CICaptureChangesDriver(integrator)
{
  protected override void BeginAnalyzeDocuments(IEnumerable<SectionEntity> rootDocuments)
  {
    foreach (SectionEntity rootDocument in rootDocuments)
    {
      CADDocumentProxy openDocument = this.CADSystem.FindOpenDocument(FilesSection.GetMasterFile(rootDocument));
      if (openDocument != null && openDocument.Modified && !openDocument.ReadOnly)
        openDocument.Save();
    }
    base.BeginAnalyzeDocuments(rootDocuments);
  }
}
