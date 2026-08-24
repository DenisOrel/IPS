// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.ModelJTFilesProvider
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Data.SectionEntities;
using Intermech.IO;
using Intermech.Tools.DataExchange;
using Intermech.Tools.Integrators;
using System.IO;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class ModelJTFilesProvider : AncillaryFilesProvider
{
  protected override void DoCollectFiles(SectionEntity documentEntity, PathCollection result)
  {
    string masterFile = FilesSection.GetMasterFile(documentEntity);
    string directoryName = Path.GetDirectoryName(masterFile);
    string withoutExtension = Path.GetFileNameWithoutExtension(masterFile);
    string path1 = Path.Combine(directoryName, withoutExtension + "__model.jt");
    if (File.Exists(path1))
    {
      result.Add(path1);
    }
    else
    {
      string path2 = Path.Combine(directoryName, withoutExtension.Replace('.', '_') + "__model.jt");
      if (!File.Exists(path2))
        return;
      result.Add(path2);
    }
  }
}
