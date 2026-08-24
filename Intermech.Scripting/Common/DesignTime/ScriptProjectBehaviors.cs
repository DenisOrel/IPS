// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ScriptProjectBehaviors
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.ComponentModel.Design;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public class ScriptProjectBehaviors
{
  private IScriptProjectRepository repository;
  private IScriptDisplayBehavior displayBehavior;
  private IScriptSaveChangesBehavior saveChangesBehavior;
  private IScriptReplacementBehavior replacementBehavior;
  private IScriptTextEditorBehavior textEditorBehavior;
  private IScriptDebugBehavior debugBehavior;
  private IScriptProjectOptionsBehavior projectOptionsBehavior;
  private ServiceContainer customBehaviors;

  public ScriptProjectBehaviors() => this.customBehaviors = new ServiceContainer();

  public void AddRepository(IScriptProjectRepository repository)
  {
    this.repository = repository != null ? repository : throw new ArgumentNullException(nameof (repository));
  }

  public IScriptProjectRepository GetRepository(bool throwIfNotFound = true)
  {
    if (this.repository != null)
      return this.repository;
    if (!throwIfNotFound)
      return (IScriptProjectRepository) null;
    throw this.BehaviorNotAvailableException(typeof (IScriptProjectRepository));
  }

  public void AddDisplayBehavior(IScriptDisplayBehavior behavior)
  {
    this.displayBehavior = behavior != null ? behavior : throw new ArgumentNullException(nameof (behavior));
  }

  public IScriptDisplayBehavior GetDisplayBehavior(bool throwIfNotFound = true)
  {
    if (this.displayBehavior != null)
      return this.displayBehavior;
    if (!throwIfNotFound)
      return (IScriptDisplayBehavior) null;
    throw this.BehaviorNotAvailableException(typeof (IScriptDisplayBehavior));
  }

  public void AddSaveChangesBehavior(IScriptSaveChangesBehavior behavior)
  {
    this.saveChangesBehavior = behavior != null ? behavior : throw new ArgumentNullException(nameof (behavior));
  }

  public IScriptSaveChangesBehavior GetSaveChangesBehavior(bool throwIfNotFound = true)
  {
    if (this.saveChangesBehavior != null)
      return this.saveChangesBehavior;
    if (!throwIfNotFound)
      return (IScriptSaveChangesBehavior) null;
    throw this.BehaviorNotAvailableException(typeof (IScriptSaveChangesBehavior));
  }

  public void AddReplacementBehavior(IScriptReplacementBehavior behavior)
  {
    this.replacementBehavior = behavior != null ? behavior : throw new ArgumentNullException(nameof (behavior));
  }

  public IScriptReplacementBehavior GetReplacementBehavior(bool throwIfNotFound = true)
  {
    if (this.replacementBehavior != null)
      return this.replacementBehavior;
    if (!throwIfNotFound)
      return (IScriptReplacementBehavior) null;
    throw this.BehaviorNotAvailableException(typeof (IScriptReplacementBehavior));
  }

  public void AddTextEditorBehavior(IScriptTextEditorBehavior behavior)
  {
    this.textEditorBehavior = behavior != null ? behavior : throw new ArgumentNullException(nameof (behavior));
  }

  public IScriptTextEditorBehavior GetTextEditorBehavior(bool throwIfNotFound = true)
  {
    if (this.textEditorBehavior != null)
      return this.textEditorBehavior;
    if (!throwIfNotFound)
      return (IScriptTextEditorBehavior) null;
    throw this.BehaviorNotAvailableException(typeof (IScriptTextEditorBehavior));
  }

  public void AddDebugBehavior(IScriptDebugBehavior behavior)
  {
    this.debugBehavior = behavior != null ? behavior : throw new ArgumentNullException(nameof (behavior));
  }

  public IScriptDebugBehavior GetDebugBehavior(bool throwIfNotFound = true)
  {
    if (this.debugBehavior != null)
      return this.debugBehavior;
    if (!throwIfNotFound)
      return (IScriptDebugBehavior) null;
    throw this.BehaviorNotAvailableException(typeof (IScriptDebugBehavior));
  }

  public void AddProjectOptionsBehavior(IScriptProjectOptionsBehavior behavior)
  {
    this.projectOptionsBehavior = behavior != null ? behavior : throw new ArgumentNullException(nameof (behavior));
  }

  public IScriptProjectOptionsBehavior GetProjectOptionsBehavior(bool throwIfNotFound = true)
  {
    if (this.projectOptionsBehavior != null)
      return this.projectOptionsBehavior;
    if (!throwIfNotFound)
      return (IScriptProjectOptionsBehavior) null;
    throw this.BehaviorNotAvailableException(typeof (IScriptProjectOptionsBehavior));
  }

  public void AddCustomBehavior(Type behaviorType, object behaviorInstance)
  {
    if (behaviorType == (Type) null)
      throw new ArgumentNullException(nameof (behaviorType));
    if (behaviorInstance == null)
      throw new ArgumentNullException(nameof (behaviorInstance));
    this.customBehaviors.AddService(behaviorType, behaviorInstance);
  }

  public object GetCustomBehavior(Type behaviorType, bool throwIfNotFound)
  {
    object customBehavior = !(behaviorType == (Type) null) ? this.customBehaviors.GetService(behaviorType) : throw new ArgumentNullException(nameof (behaviorType));
    if (customBehavior != null)
      return customBehavior;
    if (!throwIfNotFound)
      return (object) null;
    throw this.BehaviorNotAvailableException(behaviorType);
  }

  private Exception BehaviorNotAvailableException(Type behaviorType)
  {
    return (Exception) new ScriptDesignTimeException($"Сервис '{behaviorType}' не был предоставлен для проекта сценария.");
  }
}
