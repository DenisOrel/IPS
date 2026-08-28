// Decompiled with JetBrains decompiler
// Type: Intermech.Server.Service.OutputViewService
// Assembly: Intermech.Server.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E91FE21E-230A-49EC-A627-5E0B3AE2517E
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Server.Service.exe

using Intermech.ApplicationModel;
using Intermech.Diagnostics;
using Intermech.Kernel.Services;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Server.Service;

internal sealed class OutputViewService : OutputViewServiceBase, IDisposable
{
  private EventLogWriterSyncWrapper fileEventLogWriter;
  private bool isDisposed;

  public OutputViewService()
  {
    this.fileEventLogWriter = EventLogWriters.Synchronized((IEventLogWriter) ApplicationEventLogWriters.CreateTextFileWriter("imserverOutput.log"));
  }

  public void Dispose()
  {
    if (this.fileEventLogWriter == null)
      return;
    DisposeUtils.SafelyDispose(this.fileEventLogWriter.Unwrap() as IDisposable);
    this.fileEventLogWriter = (EventLogWriterSyncWrapper) null;
    this.isDisposed = true;
  }

  private bool IsDisposed
  {
    [DebuggerStepThrough] get => this.isDisposed;
  }

  protected override void OnAfterWriteString(string category, string text)
  {
    if (this.IsDisposed)
      return;
    base.OnAfterWriteString(category, text);
    this.fileEventLogWriter.Write(this.CombineCategoryWithText(category, text), EventLogItemType.Information);
  }
}
