// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.WorkerDomainPool`1
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Runtime;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading;

#nullable disable
namespace Intermech.Scripting.CSharp;

internal sealed class WorkerDomainPool<TAgent>
{
  private string poolName;
  private int maxCapacity;
  private int appDomainUseCountLimit;
  private Func<AppDomain, TAgent> agentFactory;
  private AppDomain currentAppDomain;
  private List<WorkerDomainData<TAgent>> workers;
  private int workerIdGenerator;
  private object syncRoot;

  public WorkerDomainPool(
    string poolName,
    int maxCapacity,
    int appDomainUseCountLimit,
    Func<AppDomain, TAgent> agentFactory)
  {
    if (poolName == null)
      throw new ArgumentNullException(nameof (poolName));
    if (maxCapacity <= 0)
      throw new ArgumentOutOfRangeException(nameof (maxCapacity));
    if (appDomainUseCountLimit <= 0)
      throw new ArgumentOutOfRangeException(nameof (appDomainUseCountLimit));
    if (agentFactory == null)
      throw new ArgumentNullException(nameof (agentFactory));
    this.poolName = poolName;
    this.maxCapacity = maxCapacity;
    this.appDomainUseCountLimit = appDomainUseCountLimit;
    this.agentFactory = agentFactory;
    this.currentAppDomain = AppDomain.CurrentDomain;
    this.workers = new List<WorkerDomainData<TAgent>>(maxCapacity);
    this.syncRoot = new object();
  }

  public void Clear()
  {
    lock (this.syncRoot)
    {
      while (this.workers.Count != 0)
        this.ReleaseWorkerDomain(this.Allocate());
    }
  }

  public WorkerDomainData<TAgent> Allocate()
  {
    lock (this.syncRoot)
    {
      if (this.workers.Count == 0)
        return this.CreateWorkerDomain();
      int index = this.workers.Count - 1;
      WorkerDomainData<TAgent> worker = this.workers[index];
      this.workers.RemoveAt(index);
      return worker;
    }
  }

  public WorkerDomainData<TAgent> AllocateForSingleUse()
  {
    lock (this.syncRoot)
    {
      WorkerDomainData<TAgent> workerDomain = this.CreateWorkerDomain();
      workerDomain.Domain.SetData("IsSingleUseDomain", (object) true);
      workerDomain.SingleUseMode = true;
      return workerDomain;
    }
  }

  public void Release(WorkerDomainData<TAgent> data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    lock (this.syncRoot)
    {
      if (data.SingleUseMode)
        this.ReleaseWorkerDomain(data);
      else if (data.UseCount >= this.appDomainUseCountLimit)
        this.ReleaseWorkerDomain(data);
      else if (this.workers.Count < this.maxCapacity)
      {
        this.workers.Add(data);
      }
      else
      {
        this.ReleaseMostUsedWorkerDomain();
        this.workers.Add(data);
      }
    }
  }

  private void ReleaseMostUsedWorkerDomain()
  {
    int num = 0;
    int index1 = -1;
    for (int index2 = 0; index2 < this.workers.Count; ++index2)
    {
      WorkerDomainData<TAgent> worker = this.workers[index2];
      if (worker.UseCount > num)
      {
        num = worker.UseCount;
        index1 = index2;
      }
    }
    if (index1 < 0)
      return;
    WorkerDomainData<TAgent> worker1 = this.workers[index1];
    this.workers.RemoveAt(index1);
    this.ReleaseWorkerDomain(worker1);
  }

  private WorkerDomainData<TAgent> CreateWorkerDomain()
  {
    AppDomainSetup setupInformation = this.currentAppDomain.SetupInformation;
    setupInformation.LoaderOptimization = LoaderOptimization.MultiDomain;
    int key = Interlocked.Increment(ref this.workerIdGenerator);
    AppDomain domain = AppDomain.CreateDomain($"{this.poolName} {(object) key}", (Evidence) null, setupInformation);
    try
    {
      TAgent agent = this.agentFactory(domain);
      return new WorkerDomainData<TAgent>(key, domain, agent);
    }
    catch
    {
      this.ReleaseWorkerDomain(domain);
      throw;
    }
  }

  private void ReleaseWorkerDomain(WorkerDomainData<TAgent> data)
  {
    this.ReleaseWorkerDomain(data.Domain);
  }

  private void ReleaseWorkerDomain(AppDomain domain)
  {
    SilentActionInvoker.Default.Invoke((Action) (() => AppDomain.Unload(domain)));
  }
}
