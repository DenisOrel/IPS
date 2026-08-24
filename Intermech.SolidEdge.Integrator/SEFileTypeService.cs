// Decompiled with JetBrains decompiler
// Type: Intermech.SolidEdge.Integrator.SEFileTypeService
// Assembly: Intermech.SolidEdge.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 213B90F8-0434-43B8-B8F6-9AF19E139193
// Assembly location: D:\IPS\Client\Intermech.SolidEdge.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.SolidEdge.Integrator;

internal sealed class SEFileTypeService(IIntegrator owner) : StgFileTypesService(owner)
{
  protected override PathCollection GetFileExtensions()
  {
    PathCollection fileExtensions = base.GetFileExtensions();
    fileExtensions.Add(SEConsts.AssemblyFileExtension);
    fileExtensions.Add(SEConsts.PartFileExtension);
    fileExtensions.Add(SEConsts.SheetMetalFileExtension);
    fileExtensions.Add(SEConsts.WeldmentFileExtension);
    fileExtensions.Add(SEConsts.DrawingFileExtension);
    return fileExtensions;
  }

  protected override ICollection<Guid> GetFileContentGuids()
  {
    ICollection<Guid> fileContentGuids = base.GetFileContentGuids();
    fileContentGuids.Add(new Guid("{00C6BF00-483B-11CE-951A-08003601BE52}"));
    fileContentGuids.Add(new Guid("{23C52E80-4698-11CE-B307-0800363A1E02}"));
    fileContentGuids.Add(new Guid("{DD8522E0-2375-11D0-AC05-080036FD1802}"));
    fileContentGuids.Add(new Guid("{98CCDF9C-213B-11D4-B64C-00C04F79B2BF}"));
    fileContentGuids.Add(new Guid("{016B11FB-CDC0-11CE-A035-08003601E53B}"));
    fileContentGuids.Add(new Guid("{04D613A0-A322-40B5-A2A4-36CA0DE6F5D9}"));
    return fileContentGuids;
  }
}
