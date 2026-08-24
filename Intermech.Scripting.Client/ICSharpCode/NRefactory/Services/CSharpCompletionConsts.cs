// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.CSharpCompletionConsts
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Diagnostics;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal static class CSharpCompletionConsts
{
  private static readonly Lazy<string> emptyStringProvider = new Lazy<string>((Func<string>) (() => string.Empty));

  public static Lazy<string> EmptyStringProvider
  {
    [DebuggerStepThrough] get => CSharpCompletionConsts.emptyStringProvider;
  }
}
