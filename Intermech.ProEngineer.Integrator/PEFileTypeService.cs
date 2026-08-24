// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.PEFileTypeService
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class PEFileTypeService(IIntegrator owner) : ContentBasedFileTypesService(owner)
{
  private static readonly List<ProEngineerContentPattern> patterns = new List<ProEngineerContentPattern>((IEnumerable<ProEngineerContentPattern>) new ProEngineerContentPattern[7]
  {
    new ProEngineerContentPattern(0L, "23554743", ProEngineerFilePart.Beginning),
    new ProEngineerContentPattern(-12L, "23454E445F4F465F5547430A", ProEngineerFilePart.End),
    new ProEngineerContentPattern(7L, "4D46475F415353454D", ProEngineerFilePart.Mfg),
    new ProEngineerContentPattern(7L, "415353454D424C59", ProEngineerFilePart.Assembly),
    new ProEngineerContentPattern(7L, "44524157494E47", ProEngineerFilePart.Drawing),
    new ProEngineerContentPattern(7L, "4C41594F5554", ProEngineerFilePart.Lay),
    new ProEngineerContentPattern(7L, "50415254", ProEngineerFilePart.Part)
  });

  private static bool ValidProEngineerFileBorders(Stream stream)
  {
    return SignatureSearch.ContainsSignature(PEFileTypeService.patterns[0], stream) && SignatureSearch.ContainsSignature(PEFileTypeService.patterns[1], stream);
  }

  private static bool ValidProEngineerFile(Stream stream)
  {
    return PEFileTypeService.GetProEngineerFileType(stream) != 0;
  }

  private static ProEngineerFilePart GetProEngineerFileType(Stream stream)
  {
    if (stream == null || !PEFileTypeService.ValidProEngineerFileBorders(stream))
      return ProEngineerFilePart.Unknown;
    for (int index = 2; index < PEFileTypeService.patterns.Count; ++index)
    {
      if (SignatureSearch.ContainsSignature(PEFileTypeService.patterns[index], stream))
        return PEFileTypeService.patterns[index].Type;
    }
    return ProEngineerFilePart.Unknown;
  }

  protected override PathCollection GetFileExtensions()
  {
    PathCollection fileExtensions = base.GetFileExtensions();
    fileExtensions.Add(PEConsts.AssemblyFileExtension);
    fileExtensions.Add(PEConsts.ManufacturingFileExtension);
    fileExtensions.Add(PEConsts.PartFileExtension);
    fileExtensions.Add(PEConsts.DrawingFileExtension);
    fileExtensions.Add(PEConsts.LayoutFileExtension);
    fileExtensions.Add(PEConsts.SectionFileExtension);
    return fileExtensions;
  }

  protected override bool VerifyFileContent(FileInfo fileInfo, Stream fileContent)
  {
    if (fileInfo == null)
      throw new ArgumentNullException(nameof (fileInfo));
    return fileContent != null ? PEFileTypeService.ValidProEngineerFile(fileContent) : throw new ArgumentNullException(nameof (fileContent));
  }
}
