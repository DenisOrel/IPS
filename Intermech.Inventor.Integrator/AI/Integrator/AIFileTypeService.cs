// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.AIFileTypeService
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.AI.Integrator;

internal sealed class AIFileTypeService(IIntegrator owner) : StgFileTypesService(owner)
{
  protected override PathCollection GetFileExtensions()
  {
    PathCollection fileExtensions = base.GetFileExtensions();
    fileExtensions.Add(AIConsts.AssemblyFileExtension);
    fileExtensions.Add(AIConsts.PartFileExtension);
    fileExtensions.Add(AIConsts.DrawingFileExtension);
    return fileExtensions;
  }

  protected override ICollection<Guid> GetFileContentGuids()
  {
    ICollection<Guid> fileContentGuids = base.GetFileContentGuids();
    fileContentGuids.Add(new Guid("E60F81E1-49B3-11D0-93C3-7E0706000000"));
    fileContentGuids.Add(new Guid("4D29B490-49B2-11D0-93C3-7E0706000000"));
    fileContentGuids.Add(new Guid("BBF9FDF1-52DC-11D0-8C04-0800090BE8EC"));
    return fileContentGuids;
  }
}
