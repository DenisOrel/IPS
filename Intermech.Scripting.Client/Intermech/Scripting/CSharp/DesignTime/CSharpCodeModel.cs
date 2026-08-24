// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpCodeModel
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common;
using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.CSharp.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal class CSharpCodeModel : 
  ICodeModel,
  IHoverInfoService,
  ICodeNavigationService,
  ICodeFoldingService
{
  private Uri scriptId;
  private CSharpLanguageClientHolder languageClientHolder;
  private ScriptParseOptions internalParseOptions;
  private long synchronizationLastCheckTicks;
  private CodeModelSynchronizationStatus synchronizationStatus;
  private static readonly long SynchronizationStatusRecheckInterval = TimeSpan.FromSeconds(30.0).Ticks;

  public CSharpCodeModel(Uri scriptId, CSharpLanguageClientHolder languageClientHolder)
  {
    if (scriptId == (Uri) null)
      throw new ArgumentNullException(nameof (scriptId));
    if (languageClientHolder == null)
      throw new ArgumentNullException(nameof (languageClientHolder));
    this.scriptId = scriptId;
    this.languageClientHolder = languageClientHolder;
    this.internalParseOptions = new ScriptParseOptions();
    this.synchronizationLastCheckTicks = 0L;
    this.synchronizationStatus = CodeModelSynchronizationStatus.NonSynchronized;
    this.languageClientHolder.ConnectionLost += new EventHandler<EventArgs>(this.OnOutsideConnectionLost);
  }

  public Action<string> Log { get; set; }

  public Dictionary<string, string> ParseOptions
  {
    [DebuggerStepThrough] get => ScriptParseOptions.ToDictionary(this.internalParseOptions);
    set
    {
      ScriptParseOptions internalParseOptions = this.internalParseOptions;
      try
      {
        this.internalParseOptions = ScriptParseOptions.FromDictionary(value);
      }
      catch (KeyNotFoundException ex)
      {
        this.internalParseOptions = internalParseOptions;
        throw new KeyNotFoundException(ex.Message + " Параметры разбора текста сценария не были изменены.", (Exception) ex);
      }
      if (this.CheckSynchronizationStatus() != CodeModelSynchronizationStatus.Synchronized)
        return;
      try
      {
        this.languageClientHolder.LanguageServerClient.GetScriptLanguageServer().ChangeParseOptions(this.scriptId, this.internalParseOptions);
      }
      catch (Exception ex)
      {
        this.SetSynchronizationLostStatus();
        throw;
      }
    }
  }

  public CodeModelSynchronizationStatus CheckSynchronizationStatus()
  {
    if (this.synchronizationStatus == CodeModelSynchronizationStatus.Synchronized)
    {
      long ticks = DateTime.UtcNow.Ticks;
      if (ticks - this.synchronizationLastCheckTicks >= CSharpCodeModel.SynchronizationStatusRecheckInterval)
      {
        if (!this.TestLanguageServerConnection())
          this.SetSynchronizationLostStatus();
        this.synchronizationLastCheckTicks = ticks;
      }
    }
    return this.synchronizationStatus;
  }

  public void OpenText(string text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (this.synchronizationStatus == CodeModelSynchronizationStatus.SynchronizationLost)
    {
      if (!this.TestLanguageServerConnection())
        this.SetSynchronizationLostStatus();
      else
        this.synchronizationStatus = CodeModelSynchronizationStatus.Synchronized;
    }
    try
    {
      this.languageClientHolder.LanguageServerClient.GetScriptLanguageServer().OpenDocument(this.scriptId, this.internalParseOptions, text);
      this.synchronizationStatus = CodeModelSynchronizationStatus.Synchronized;
    }
    catch (Exception ex)
    {
      this.SetSynchronizationLostStatus();
      throw;
    }
  }

  public void ChangeText(List<ScriptTextChange> changes)
  {
    if (changes == null)
      throw new ArgumentNullException(nameof (changes));
    this.CheckIfSynchronized();
    try
    {
      this.languageClientHolder.LanguageServerClient.GetScriptLanguageServer().ChangeDocument(this.scriptId, changes);
    }
    catch (Exception ex)
    {
      this.SetSynchronizationLostStatus();
      throw;
    }
  }

  public void CloseText(bool throwIfError)
  {
    if (this.synchronizationStatus == CodeModelSynchronizationStatus.NonSynchronized)
      return;
    try
    {
      this.languageClientHolder.LanguageServerClient.GetScriptLanguageServer().CloseDocument(this.scriptId);
    }
    catch
    {
      if (!throwIfError)
        return;
      throw;
    }
    finally
    {
      this.synchronizationStatus = CodeModelSynchronizationStatus.NonSynchronized;
      this.synchronizationLastCheckTicks = 0L;
    }
  }

  private void CheckIfSynchronized()
  {
    if (this.CheckSynchronizationStatus() != CodeModelSynchronizationStatus.Synchronized)
      throw new InvalidOperationException($"Предварительно требуется вызвать метод {"CheckSynchronizationStatus"}");
  }

  public HoverInfo GetHoverInfo(int offset)
  {
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset));
    this.CheckIfSynchronized();
    try
    {
      return new HoverInfo(this.languageClientHolder.LanguageServerClient.GetParserService().GetDocumentation(this.scriptId, offset));
    }
    catch (Exception ex)
    {
      this.ResolveProcessDrop(ex);
      throw;
    }
  }

  public IList<NavigationItem> GetNavigationItems()
  {
    this.CheckIfSynchronized();
    try
    {
      return this.languageClientHolder.LanguageServerClient.GetParserService().GetNavigationItems(this.scriptId);
    }
    catch (Exception ex)
    {
      this.ResolveProcessDrop(ex);
      throw;
    }
  }

  public IList<FoldingRegionItem> GetFoldingRegions()
  {
    this.CheckIfSynchronized();
    try
    {
      return this.languageClientHolder.LanguageServerClient.GetParserService().GetFoldingRegions(this.scriptId);
    }
    catch (Exception ex)
    {
      this.ResolveProcessDrop(ex);
      throw;
    }
  }

  private void ResolveProcessDrop(Exception x)
  {
    if (!this.IsRemotingException(x) || this.TestLanguageServerConnection())
      return;
    this.LogMessage("[Code Model]: соединение с языковым сервером утеряно.");
    this.SetSynchronizationLostStatus();
  }

  private bool IsRemotingException(Exception exception)
  {
    switch (exception)
    {
      case WebException _:
      case SocketException _:
      case RemotingException _:
        return true;
      default:
        return false;
    }
  }

  private bool TestLanguageServerConnection()
  {
    return this.languageClientHolder.LanguageServerClient.IsConnected;
  }

  private void SetSynchronizationLostStatus(bool raiseEvent = true)
  {
    this.synchronizationStatus = CodeModelSynchronizationStatus.SynchronizationLost;
    if (!raiseEvent)
      return;
    this.languageClientHolder.PerformConnectionLost((ICodeModel) this);
  }

  private void OnOutsideConnectionLost(object sender, EventArgs e)
  {
    if (sender == this || this.synchronizationStatus == CodeModelSynchronizationStatus.NonSynchronized)
      return;
    this.SetSynchronizationLostStatus(false);
  }

  private void LogMessage(string message)
  {
    if (this.Log == null)
      return;
    this.Log(message);
  }
}
