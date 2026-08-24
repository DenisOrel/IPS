// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SW2015FileTypeService
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SW2015FileTypeService(IIntegrator owner) : ContentBasedFileTypesService(owner)
{
  private ICollection<Guid> fileTypeGuids;

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this.fileTypeGuids = (ICollection<Guid>) new List<Guid>();
    this.fileTypeGuids.Add(new Guid("00000022-F634-66E6-9676-D203D2645616"));
    this.fileTypeGuids.Add(new Guid("0000000D-F634-47E6-56E6-4737F234D476"));
    this.fileTypeGuids.Add(new Guid("00000013-F634-47E6-56E6-4737F2445666"));
  }

  protected override PathCollection GetFileExtensions()
  {
    PathCollection fileExtensions = base.GetFileExtensions();
    fileExtensions.Add(SWConsts.AssemblyFileExtension);
    fileExtensions.Add(SWConsts.PartFileExtension);
    fileExtensions.Add(SWConsts.DrawingFileExtension);
    return fileExtensions;
  }

  protected override bool VerifyFileContent(FileInfo fileInfo, Stream fileContent)
  {
    if (fileInfo == null)
      throw new ArgumentNullException(nameof (fileInfo));
    byte[] b = fileContent != null ? this.TryReadSignature(fileContent) : throw new ArgumentNullException(nameof (fileContent));
    return b != null && this.fileTypeGuids.Contains(new Guid(b));
  }

  private byte[] TryReadSignature(Stream fileContent)
  {
    byte[] buffer = new byte[16 /*0x10*/];
    fileContent.Seek(34L, SeekOrigin.Begin);
    return fileContent.Read(buffer, 0, buffer.Length) == buffer.Length ? buffer : (byte[]) null;
  }
}
