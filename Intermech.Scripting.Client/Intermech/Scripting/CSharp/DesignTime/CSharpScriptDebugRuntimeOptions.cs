// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.DesignTime.CSharpScriptDebugRuntimeOptions
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Intermech.Scripting.CSharp.DesignTime;

internal sealed class CSharpScriptDebugRuntimeOptions
{
  private ICollection<string> autoReferencedAssemblies;
  private ICollection<string> searchPathList;
  private const string AutoReferencedAssembliesOption = "AutoReferencedAssemblies";
  private const string SearchPathListOption = "SearchPathList";

  public CSharpScriptDebugRuntimeOptions()
  {
    this.autoReferencedAssemblies = (ICollection<string>) new string[0];
    this.searchPathList = (ICollection<string>) new string[0];
  }

  public ICollection<string> AutoReferencedAssemblies
  {
    [DebuggerStepThrough] get => this.autoReferencedAssemblies;
    set
    {
      this.autoReferencedAssemblies = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public ICollection<string> SearchPathList
  {
    [DebuggerStepThrough] get => this.searchPathList;
    set
    {
      this.searchPathList = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public static Dictionary<string, string> ToDictionary(CSharpScriptDebugRuntimeOptions options)
  {
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    return new Dictionary<string, string>()
    {
      {
        "AutoReferencedAssemblies",
        string.Join(";", (IEnumerable<string>) options.AutoReferencedAssemblies)
      },
      {
        "SearchPathList",
        string.Join(";", (IEnumerable<string>) options.SearchPathList)
      }
    };
  }

  public static CSharpScriptDebugRuntimeOptions FromDictionary(Dictionary<string, string> options)
  {
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    CSharpScriptDebugRuntimeOptions debugRuntimeOptions = new CSharpScriptDebugRuntimeOptions();
    string str1;
    if (options.TryGetValue("AutoReferencedAssemblies", out str1))
      debugRuntimeOptions.AutoReferencedAssemblies = (ICollection<string>) ((IEnumerable<string>) str1.Split(';')).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))).ToArray<string>();
    string str2;
    if (options.TryGetValue("SearchPathList", out str2))
      debugRuntimeOptions.SearchPathList = (ICollection<string>) ((IEnumerable<string>) str2.Split(';')).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))).ToArray<string>();
    return debugRuntimeOptions;
  }
}
