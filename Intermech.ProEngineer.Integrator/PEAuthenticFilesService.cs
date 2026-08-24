// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.PEAuthenticFilesService
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System;
using System.IO;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal sealed class PEAuthenticFilesService(IIntegrator owner) : CADAuthenticFilesService(owner)
{
  public override string MakeFilePath(string documentFilePath, string authenticFileType)
  {
    documentFilePath = documentFilePath != null ? this.ToSafeCreoFileName(documentFilePath) : throw new ArgumentNullException(nameof (documentFilePath));
    return base.MakeFilePath(documentFilePath, authenticFileType);
  }

  private string ToSafeCreoFileName(string name)
  {
    string directoryName = Path.GetDirectoryName(name);
    string path2 = Path.GetFileName(name).Replace('.', '_');
    if (!string.IsNullOrEmpty(directoryName))
      path2 = Path.Combine(directoryName, path2);
    return path2;
  }

  protected override bool RequireVisibleDocument() => true;
}
