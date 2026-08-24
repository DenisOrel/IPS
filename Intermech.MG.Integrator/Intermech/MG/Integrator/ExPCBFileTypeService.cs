// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBFileTypeService
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using System;
using System.IO;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBFileTypeService(IIntegrator owner) : ContentBasedFileTypesService(owner)
{
  protected override PathCollection GetFileExtensions()
  {
    PathCollection fileExtensions = base.GetFileExtensions();
    fileExtensions.Add(MGConsts.ProjectFileExtension);
    return fileExtensions;
  }

  protected override bool VerifyFileContent(FileInfo fileInfo, Stream fileContent)
  {
    if (fileInfo == null)
      throw new ArgumentNullException(nameof (fileInfo));
    if (fileContent == null)
      throw new ArgumentNullException(nameof (fileContent));
    return MGProjectHelper.DefineProjectType(fileContent, out string _) == MGProjectType.Foreign;
  }
}
