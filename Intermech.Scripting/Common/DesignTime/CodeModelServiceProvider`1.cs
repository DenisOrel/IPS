// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.CodeModelServiceProvider`1
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public class CodeModelServiceProvider<T> : ICodeModelServiceProvider where T : class
{
  private ICodeModel codeModel;
  private CodeModelErrorController errors;
  private bool? isSupportedCache;
  private Action<Exception> codeModelRecoveryAction;
  private static readonly Action<Exception> EmptyCodeModelRecoveryAction = new Action<Exception>(CodeModelServiceProvider<T>.EmptyCodeModelRecoveryHandler);

  public CodeModelServiceProvider(ICodeModel codeModel)
  {
    this.codeModel = codeModel != null ? codeModel : throw new ArgumentNullException(nameof (codeModel));
    this.errors = new CodeModelErrorController();
    this.codeModelRecoveryAction = CodeModelServiceProvider<T>.EmptyCodeModelRecoveryAction;
  }

  public ICodeModel CodeModel
  {
    [DebuggerStepThrough] get => this.codeModel;
  }

  public CodeModelErrorController Errors
  {
    [DebuggerStepThrough] get => this.errors;
  }

  public bool IsSupported
  {
    [DebuggerStepThrough] get
    {
      if (!this.isSupportedCache.HasValue)
        this.isSupportedCache = new bool?(this.codeModel is T);
      return this.isSupportedCache.Value;
    }
  }

  public bool IsSupportedAndAllowed
  {
    [DebuggerStepThrough] get => this.IsSupported && this.Errors.IsCapabilityAllowed;
  }

  public Action<Exception> CodeModelRecoveryAction
  {
    [DebuggerStepThrough] get
    {
      return !(this.codeModelRecoveryAction != CodeModelServiceProvider<T>.EmptyCodeModelRecoveryAction) ? (Action<Exception>) null : this.codeModelRecoveryAction;
    }
    [DebuggerStepThrough] set
    {
      this.codeModelRecoveryAction = value != null ? value : CodeModelServiceProvider<T>.EmptyCodeModelRecoveryAction;
    }
  }

  protected T TryGetService() => this.codeModel as T;

  private static void EmptyCodeModelRecoveryHandler(Exception exception)
  {
  }
}
