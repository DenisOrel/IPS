// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.PECaptureChangesDriver
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.Data.SectionEntities;
using Intermech.Interfaces.Client;
using Intermech.IO;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System.IO;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class PECaptureChangesDriver(IIntegrator integrator) : CICaptureChangesDriver(integrator)
{
  protected override bool GetDocumentOpenVisibleMode(SectionEntity documentItem, string fullPath)
  {
    return this.IsDrawingFile(fullPath) && this.SaveChangesMode == SaveChangesMode.Checkin || base.GetDocumentOpenVisibleMode(documentItem, fullPath);
  }

  private bool IsDrawingFile(string fullPath)
  {
    return PathUtils.IsSamePath(Path.GetExtension(fullPath), PEConsts.DrawingFileExtension);
  }
}
