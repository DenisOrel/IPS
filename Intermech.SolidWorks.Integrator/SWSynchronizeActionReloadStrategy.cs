// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWSynchronizeActionReloadStrategy
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using Intermech.Files;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal sealed class SWSynchronizeActionReloadStrategy : ISynchronizeActionReloadStrategy
{
  private readonly IIntegrator integrator;
  private bool isInitialized;
  private OpenFilesReloadService openFilesReloadService;
  private OpenFilesUnloadResult unloadResult;
  private static readonly OpenFilesUnloadResult noFilesToUnload = new OpenFilesUnloadResult(true, (object) null);

  public SWSynchronizeActionReloadStrategy(IIntegrator integrator)
  {
    this.integrator = integrator != null ? integrator : throw new ArgumentNullException(nameof (integrator));
  }

  private void InitializeLazily()
  {
    if (this.isInitialized)
      return;
    this.InitializeCore();
    this.isInitialized = true;
  }

  private void InitializeCore()
  {
    try
    {
      this.openFilesReloadService = new OpenFilesReloadService(this.integrator, false);
    }
    catch
    {
      this.openFilesReloadService = (OpenFilesReloadService) null;
      throw;
    }
  }

  public void BeginOperation(List<DBObjectState> dbObjects)
  {
    if (dbObjects == null)
      throw new ArgumentNullException(nameof (dbObjects));
    this.InitializeLazily();
    if (dbObjects.Count != 0)
      this.unloadResult = this.openFilesReloadService.UnloadAll();
    else
      this.unloadResult = SWSynchronizeActionReloadStrategy.noFilesToUnload;
  }

  private bool IsInOperation
  {
    [DebuggerStepThrough] get => this.unloadResult != null;
  }

  private void CheckIfInOperation()
  {
    if (!this.IsInOperation)
      throw new InvalidOperationException($"Call the method '{"BeginOperation"}' first.");
  }

  public bool TryUnlockFiles()
  {
    this.InitializeLazily();
    this.CheckIfInOperation();
    return this.unloadResult.IsSucceessful;
  }

  public void EndOperation()
  {
    this.InitializeLazily();
    this.CheckIfInOperation();
    try
    {
      if (this.unloadResult.ReloadState == null)
        return;
      this.openFilesReloadService.Reload(this.unloadResult.ReloadState);
    }
    finally
    {
      this.unloadResult = (OpenFilesUnloadResult) null;
    }
  }
}
