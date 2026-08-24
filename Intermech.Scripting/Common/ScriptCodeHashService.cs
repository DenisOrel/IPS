// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ScriptCodeHashService
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Pools;
using System;

#nullable disable
namespace Intermech.Scripting.Common;

/// <summary>
/// Сервис для хэширования текстов сценариев.
/// Реализация класса является thread safe и может вызываться из изолированных AppDomain.
/// </summary>
public class ScriptCodeHashService : ScriptExecutorServiceBase
{
  private ConcurrentBagPool<ITextHasher> scriptCodeHasherPool;

  /// <summary>Создает объект.</summary>
  public ScriptCodeHashService()
  {
    this.scriptCodeHasherPool = new ConcurrentBagPool<ITextHasher>(1, (Func<ITextHasher>) (() => (ITextHasher) new LRUTextHasher((ITextHasher) new SHA1TextHasher(8192 /*0x2000*/), 1048576 /*0x100000*/)));
  }

  public byte[] ComputeHash(string scriptCode)
  {
    if (scriptCode == null)
      throw new ArgumentNullException(nameof (scriptCode));
    using (ObjectPoolScope<ITextHasher> objectPoolScope = this.scriptCodeHasherPool.AllocateInScope<ITextHasher>())
      return objectPoolScope.Object.ComputeHash(scriptCode);
  }
}
