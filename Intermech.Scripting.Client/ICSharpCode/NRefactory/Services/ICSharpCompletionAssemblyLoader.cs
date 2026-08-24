// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.ICSharpCompletionAssemblyLoader
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.TypeSystem;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal interface ICSharpCompletionAssemblyLoader
{
  IUnresolvedAssembly TryLoadAssembly(string assemblyFilePath);
}
