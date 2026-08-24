// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWSimpleFileTypeService
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SWSimpleFileTypeService(IIntegrator owner) : NameBasedFileTypesService(owner)
{
  protected override PathCollection GetFileExtensions()
  {
    PathCollection fileExtensions = base.GetFileExtensions();
    fileExtensions.Add(SWConsts.AssemblyFileExtension);
    fileExtensions.Add(SWConsts.PartFileExtension);
    fileExtensions.Add(SWConsts.DrawingFileExtension);
    return fileExtensions;
  }
}
