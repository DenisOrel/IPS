// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NX12OrNewerFileTypeService
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NX12OrNewerFileTypeService(IIntegrator owner) : ContentBasedFileTypesService(owner)
{
  private static readonly byte[] nx12StartPattern = new byte[8]
  {
    (byte) 83,
    (byte) 80 /*0x50*/,
    (byte) 76,
    (byte) 77,
    (byte) 83,
    (byte) 83,
    (byte) 84,
    (byte) 82
  };

  protected override PathCollection GetFileExtensions()
  {
    PathCollection fileExtensions = base.GetFileExtensions();
    fileExtensions.Add(NXConsts.AnyFileExtension);
    return fileExtensions;
  }

  protected override bool VerifyFileContent(FileInfo fileInfo, Stream fileContent)
  {
    byte[] numArray = new byte[8];
    return fileContent.Read(numArray, 0, numArray.Length) == numArray.Length && ((IEnumerable<byte>) numArray).SequenceEqual<byte>((IEnumerable<byte>) NX12OrNewerFileTypeService.nx12StartPattern);
  }
}
