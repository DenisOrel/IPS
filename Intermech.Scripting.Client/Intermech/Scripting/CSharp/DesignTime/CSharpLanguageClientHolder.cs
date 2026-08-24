// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpLanguageClientHolder
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using Intermech.Scripting.CSharp.ServiceProcess;
using Intermech.Scripting.Utils;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal class CSharpLanguageClientHolder
{
  private LanguageServerClient languageServerClient;
  private WeakEventSource<EventArgs> connectionLost;

  public CSharpLanguageClientHolder() => this.connectionLost = new WeakEventSource<EventArgs>();

  public LanguageServerClient LanguageServerClient
  {
    [DebuggerStepThrough] get
    {
      if (this.languageServerClient == null)
        this.languageServerClient = new LanguageServerClient();
      return this.languageServerClient;
    }
  }

  public event EventHandler<EventArgs> ConnectionLost
  {
    add => this.connectionLost.Subscribe(value);
    remove => this.connectionLost.Unsubscribe(value);
  }

  public void PerformConnectionLost(ICodeModel codeModel)
  {
    this.connectionLost.Raise((object) codeModel, EventArgs.Empty);
  }
}
