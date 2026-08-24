// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.AIModelDrawingsService
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using Intermech.IO;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface.ModelDrawings;
using System.IO;

#nullable disable
namespace Intermech.AI.Integrator;

internal sealed class AIModelDrawingsService(
  IIntegrator owner,
  string drawingExtension,
  params string[] modelExtensions) : NormalModelDrawingsService(owner, drawingExtension, modelExtensions)
{
  protected override string DoTranslateModelFileName(
    string fileName,
    ModelDrawingsFindContext findContext)
  {
    return PathUtils.IsSamePath(Path.GetDirectoryName(fileName) + Path.GetExtension(fileName), findContext.ModelMasterFileName) ? Path.Combine(findContext.ModelDirectory, Path.GetFileName(fileName)) : base.DoTranslateModelFileName(fileName, findContext);
  }
}
