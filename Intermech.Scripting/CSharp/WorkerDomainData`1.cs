// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.WorkerDomainData`1
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.CSharp;

internal sealed class WorkerDomainData<TAgent>
{
  public WorkerDomainData(int key, AppDomain domain, TAgent agent)
  {
    this.Key = key;
    this.Domain = domain;
    this.Agent = agent;
  }

  /// <summary>
  /// Уникальный идентификатор изолированного AppDomain.
  /// Внимание! Этот идентификатор - это не AppDomain.Id
  /// </summary>
  public int Key { get; private set; }

  public AppDomain Domain { get; private set; }

  public TAgent Agent { get; private set; }

  public int UseCount { get; set; }

  /// <summary>
  /// Возвращает или задает признак одноразового AppDomain.
  /// Такой AppDomain создается для выполнения только одного задания и
  /// освобожается сразу после этого.
  /// </summary>
  public bool SingleUseMode { get; set; }
}
