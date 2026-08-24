// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.AIFamilyFilesModule
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using Intermech.ApplicationModel;
using Intermech.Files;
using Intermech.Interfaces.Client;
using System;

#nullable disable
namespace Intermech.AI.Integrator;

internal sealed class AIFamilyFilesModule : InitializerModule
{
  private AIFamilyFilesHandler familyFilesHandler;
  private IFileVault fileVaultService;

  protected override void DoInitialize()
  {
    base.DoInitialize();
    this.fileVaultService = ClientContext.FileVault;
    if (string.Equals(Environment.GetEnvironmentVariable("IPS_PROTECT_AI_FILES"), "1"))
      return;
    this.familyFilesHandler = new AIFamilyFilesHandler(this.fileVaultService.ReadOnlyLocalFiles);
    this.familyFilesHandler.Enabled = true;
  }

  protected override void DoShutdown()
  {
    base.DoShutdown();
    if (this.familyFilesHandler != null)
    {
      this.familyFilesHandler.Enabled = false;
      this.familyFilesHandler = (AIFamilyFilesHandler) null;
    }
    if (this.fileVaultService == null)
      return;
    this.fileVaultService = (IFileVault) null;
  }
}
