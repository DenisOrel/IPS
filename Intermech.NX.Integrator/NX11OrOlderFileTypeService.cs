// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NX11OrOlderFileTypeService
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NX11OrOlderFileTypeService(IIntegrator owner) : StgFileTypesService(owner)
{
  protected override PathCollection GetFileExtensions()
  {
    PathCollection fileExtensions = base.GetFileExtensions();
    fileExtensions.Add(NXConsts.AnyFileExtension);
    return fileExtensions;
  }

  protected override ICollection<Guid> GetFileContentGuids()
  {
    ICollection<Guid> fileContentGuids = base.GetFileContentGuids();
    fileContentGuids.Add(new Guid("{B3C91E61-60FA-11D1-8AD9-0800362FB302}"));
    fileContentGuids.Add(new Guid("{203FAC50-0003-0000-5039-352002000000}"));
    fileContentGuids.Add(new Guid("{203FAC50-0000-0000-6A37-352003000000}"));
    fileContentGuids.Add(new Guid("{2048C478-0003-0000-DCE2-3D2002000000}"));
    fileContentGuids.Add(new Guid("{2048C478-0000-0000-EAE0-3D2003000000}"));
    fileContentGuids.Add(new Guid("{06E5AC50-0000-0000-6A37-DB0603000000}"));
    fileContentGuids.Add(new Guid("{070CAC50-0000-0000-6A37-020703000000}"));
    fileContentGuids.Add(new Guid("{B3C91E61-60FA-11D1-8AD9-0800362FB302}"));
    return fileContentGuids;
  }
}
