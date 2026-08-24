// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWOldFileTypeService
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SWOldFileTypeService(IIntegrator owner) : StgFileTypesService(owner)
{
  protected override PathCollection GetFileExtensions()
  {
    PathCollection fileExtensions = base.GetFileExtensions();
    fileExtensions.Add(SWConsts.AssemblyFileExtension);
    fileExtensions.Add(SWConsts.PartFileExtension);
    fileExtensions.Add(SWConsts.DrawingFileExtension);
    return fileExtensions;
  }

  protected override ICollection<Guid> GetFileContentGuids()
  {
    ICollection<Guid> fileContentGuids = base.GetFileContentGuids();
    fileContentGuids.Add(new Guid("{83A33D36-27C5-11CE-BFD4-00400513BB57}"));
    fileContentGuids.Add(new Guid("{83A33D30-27C5-11CE-BFD4-00400513BB57}"));
    fileContentGuids.Add(new Guid("{83A33D34-27C5-11CE-BFD4-00400513BB57}"));
    return fileContentGuids;
  }
}
