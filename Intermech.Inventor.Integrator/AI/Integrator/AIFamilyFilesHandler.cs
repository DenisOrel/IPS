// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.AIFamilyFilesHandler
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Files;
using Intermech.IO;
using System;
using System.IO;

#nullable disable
namespace Intermech.AI.Integrator;

internal sealed class AIFamilyFilesHandler : ServiceExtender
{
  private PathCollection modelFileExtensions;
  private IReadOnlyLocalFilesManager readOnlyLocalFiles;
  private static readonly object handlerContextKey = new object();

  public AIFamilyFilesHandler(IReadOnlyLocalFilesManager readOnlyLocalFiles)
  {
    if (readOnlyLocalFiles == null)
      throw new ArgumentNullException(nameof (readOnlyLocalFiles));
    this.modelFileExtensions = new PathCollection();
    this.modelFileExtensions.Add(AIConsts.AssemblyFileExtension);
    this.modelFileExtensions.Add(AIConsts.PartFileExtension);
    this.readOnlyLocalFiles = readOnlyLocalFiles;
  }

  protected override void DoEnable()
  {
    base.DoEnable();
    this.readOnlyLocalFiles.CanControlAttributeEvent += new EventHandler<CanControlFileAttributeEventArgs>(this.CanControlReadOnlyFileAttribute);
  }

  protected override void DoDisable()
  {
    base.DoDisable();
    this.readOnlyLocalFiles.CanControlAttributeEvent -= new EventHandler<CanControlFileAttributeEventArgs>(this.CanControlReadOnlyFileAttribute);
  }

  private void CanControlReadOnlyFileAttribute(object sender, CanControlFileAttributeEventArgs e)
  {
    AIFamilyFilesHandler.HandlerContext handlerContext = this.GetOrCreateHandlerContext(e);
    if (!handlerContext.IsModel || !e.CanControl || PathUtils.IsSamePath(e.RelativeFilePath, handlerContext.MasterFileName))
      return;
    string directoryName = Path.GetDirectoryName(e.RelativeFilePath);
    if (string.IsNullOrEmpty(directoryName) || !PathUtils.IsSamePath(directoryName, handlerContext.FamilyFilesDirectoryPath))
      return;
    e.CanControl = false;
  }

  private AIFamilyFilesHandler.HandlerContext GetOrCreateHandlerContext(
    CanControlFileAttributeEventArgs e)
  {
    object handlerContext1;
    if (e.DBObjectContext.TryGetValue(AIFamilyFilesHandler.handlerContextKey, out handlerContext1))
      return (AIFamilyFilesHandler.HandlerContext) handlerContext1;
    AIFamilyFilesHandler.HandlerContext handlerContext2 = this.CreateHandlerContext(e.RelativeFilePath);
    e.DBObjectContext.Add(AIFamilyFilesHandler.handlerContextKey, (object) handlerContext2);
    return handlerContext2;
  }

  private AIFamilyFilesHandler.HandlerContext CreateHandlerContext(string relativeFilePath)
  {
    AIFamilyFilesHandler.HandlerContext handlerContext = new AIFamilyFilesHandler.HandlerContext();
    handlerContext.IsModel = this.IsModelFile(relativeFilePath);
    if (handlerContext.IsModel)
    {
      handlerContext.MasterFileName = relativeFilePath;
      handlerContext.FamilyFilesDirectoryPath = Path.ChangeExtension(relativeFilePath, (string) null);
    }
    return handlerContext;
  }

  private bool IsModelFile(string filePath)
  {
    string str = Path.GetExtension(filePath);
    return !string.IsNullOrEmpty(str) && this.modelFileExtensions.Contains(str);
  }

  private sealed class HandlerContext
  {
    public bool IsModel { get; set; }

    public string MasterFileName { get; set; }

    public string FamilyFilesDirectoryPath { get; set; }
  }
}
