// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.NRefactoryCodeCompletionServiceProvider
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.Services;
using Intermech.Interfaces;
using Intermech.Scripting.Common.Debugging;
using Intermech.Scripting.CSharp.Debugging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal sealed class NRefactoryCodeCompletionServiceProvider
{
  private ICollection<string> xmlDocPathList;
  private SharedAssemblyLoader sharedAssemblyLoader;
  private Task<CSharpCompletionService> clientCompletionServiceFactory;
  private Task<CSharpCompletionService> serverCompletionServiceFactory;

  public NRefactoryCodeCompletionServiceProvider(ICollection<string> xmlDocPathList)
  {
    this.xmlDocPathList = xmlDocPathList != null ? xmlDocPathList : throw new ArgumentNullException(nameof (xmlDocPathList));
    this.sharedAssemblyLoader = new SharedAssemblyLoader(this.xmlDocPathList);
    this.clientCompletionServiceFactory = Task.Factory.StartNew<CSharpCompletionService>(new Func<CSharpCompletionService>(this.CreateClientCompletionService));
    this.serverCompletionServiceFactory = Task.Factory.StartNew<CSharpCompletionService>(new Func<CSharpCompletionService>(this.CreateServerCompletionService));
  }

  public ICollection<string> XmlDocPathList
  {
    [DebuggerStepThrough] get => this.xmlDocPathList;
  }

  public CSharpCompletionService TryGetCodeCompletionService(bool runAtClientSide)
  {
    if (runAtClientSide)
    {
      if (this.clientCompletionServiceFactory.IsCompleted && !this.clientCompletionServiceFactory.IsFaulted)
        return this.clientCompletionServiceFactory.Result;
    }
    else if (this.serverCompletionServiceFactory.IsCompleted && !this.serverCompletionServiceFactory.IsFaulted)
      return this.serverCompletionServiceFactory.Result;
    return (CSharpCompletionService) null;
  }

  private CSharpCompletionService CreateClientCompletionService()
  {
    return this.CreateCompletionService(this.GetClientAssemblies());
  }

  private ICollection<string> GetClientAssemblies()
  {
    return new CSharpDebugOperations().GetAssembliesForAutocompletion();
  }

  private CSharpCompletionService CreateServerCompletionService()
  {
    return this.CreateCompletionService(this.GetServerAssemblies());
  }

  private ICollection<string> GetServerAssemblies()
  {
    int clientToken = ClientTokenProvider.Default.GetClientToken();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ICSharpDebugExecutor customService = (ICSharpDebugExecutor) sessionKeeper.Session.GetCustomService(typeof (ICSharpScriptExecutor));
      if (customService.CanDebug(clientToken))
        return customService.GetAssembliesForAutocompletion(clientToken);
    }
    return (ICollection<string>) new string[0];
  }

  private CSharpCompletionService CreateCompletionService(
    ICollection<string> assembliesForAutocompletion)
  {
    CSharpCompletionService completionService = new CSharpCompletionService((ICSharpCompletionAssemblyLoader) this.sharedAssemblyLoader);
    foreach (string str in (IEnumerable<string>) assembliesForAutocompletion)
    {
      if (File.Exists(str))
        completionService.AddAssembly(str);
    }
    return completionService;
  }

  public string InitializationPendingMessage
  {
    [DebuggerStepThrough] get => "Автодополнение еще не готово...";
  }

  public Lazy<string> InitializationPendingDescription
  {
    [DebuggerStepThrough] get => CSharpCompletionConsts.EmptyStringProvider;
  }
}
