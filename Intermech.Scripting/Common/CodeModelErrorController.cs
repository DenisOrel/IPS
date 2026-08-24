// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.CodeModelErrorController
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Реализует подсчет количества ошибок подряд в работе какой-либо возможности <see cref="!:ICodeModel" />.
/// После превышения допустимого порога ошибок предлагается отключение этой возможности.
/// </summary>
public class CodeModelErrorController
{
  private readonly int errorLimit;
  private int errorCount;
  private bool isAllowed;
  private bool isBanned;

  public CodeModelErrorController(int errorLimit = 3)
  {
    this.errorLimit = errorLimit > 0 ? errorLimit : throw new ArgumentOutOfRangeException(nameof (errorLimit));
    this.isAllowed = true;
  }

  /// <summary>
  /// Возвращает максимальное количество ошибок подряд, при превышении которого
  /// предлагается отключение соответствующей возможности <see cref="!:ICodeModel" />.
  /// </summary>
  public int ErrorLimit
  {
    [DebuggerStepThrough] get => this.errorLimit;
  }

  /// <summary>Текущее количество ошибок подряд.</summary>
  public int ErrorCount
  {
    [DebuggerStepThrough] get => this.errorCount;
    private set
    {
      if (this.errorCount == value)
        return;
      this.errorCount = value;
      this.OnErrorCountChanged();
    }
  }

  /// <summary>
  /// Возвращает признак, что соответствующая возможность <see cref="!:ICodeModel" /> может использоваться.
  /// </summary>
  public bool IsCapabilityAllowed
  {
    [DebuggerStepThrough] get => this.isAllowed;
  }

  /// <summary>
  /// Возвращает признак, что соответствующая возможность <see cref="!:ICodeModel" /> больше не должна использоваться.
  /// </summary>
  public bool IsCapabilityBanned
  {
    [DebuggerStepThrough] get => this.isBanned;
  }

  private void OnErrorCountChanged()
  {
    this.isAllowed = this.errorCount <= this.errorLimit;
    this.isBanned = !this.isAllowed;
  }

  /// <summary>Увеличивает счетчик ошибок</summary>
  public void RegisterError() => ++this.ErrorCount;

  /// <summary>Сбрасывае счетчик ошибок.</summary>
  public void Reset() => this.ErrorCount = 0;
}
