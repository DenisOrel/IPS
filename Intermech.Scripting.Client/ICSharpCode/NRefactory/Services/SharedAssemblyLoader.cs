// Decompiled with JetBrains decompiler
// Type: ICSharpCode.NRefactory.Services.SharedAssemblyLoader
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.TypeSystem;
using Intermech.Diagnostics;
using Intermech.IO;
using Intermech.Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace ICSharpCode.NRefactory.Services;

internal sealed class SharedAssemblyLoader : ICSharpCompletionAssemblyLoader
{
  private PathComparer pathComparer;
  private ICollection<string> xmlDocPathList;
  private ConcurrentDictionary<string, XmlDocumentationProvider> xmlDocCache;
  private string referenceAssembliesPath;
  private ConcurrentDictionary<string, IUnresolvedAssembly> assemblyCache;
  private ICollection<string> assemblyBlackList;

  public SharedAssemblyLoader(ICollection<string> xmlDocPathList)
  {
    if (xmlDocPathList == null)
      throw new ArgumentNullException(nameof (xmlDocPathList));
    this.pathComparer = new PathComparer();
    this.xmlDocPathList = xmlDocPathList;
    this.xmlDocCache = new ConcurrentDictionary<string, XmlDocumentationProvider>((IEqualityComparer<string>) this.pathComparer);
    this.referenceAssembliesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.7.2");
    this.assemblyCache = new ConcurrentDictionary<string, IUnresolvedAssembly>((IEqualityComparer<string>) this.pathComparer);
    this.assemblyBlackList = (ICollection<string>) new HashSet<string>((IEqualityComparer<string>) this.pathComparer);
    this.assemblyBlackList.Add("Microsoft.mshtml.dll");
  }

  public IUnresolvedAssembly TryLoadAssembly(string assemblyFilePath)
  {
    string key1 = assemblyFilePath != null ? Path.GetFileName(assemblyFilePath) : throw new ArgumentNullException(nameof (assemblyFilePath));
    if (this.assemblyBlackList.Contains(key1))
      return (IUnresolvedAssembly) null;
    try
    {
      return this.assemblyCache.GetOrAdd(key1, (Func<string, IUnresolvedAssembly>) (key => this.LoadAssemblySlow(assemblyFilePath)));
    }
    catch (Exception ex)
    {
      string currentMethodName = this.GetCurrentMethodName(nameof (TryLoadAssembly));
      SuppressedExceptions.TraceException(ex, currentMethodName);
      return (IUnresolvedAssembly) null;
    }
  }

  private IUnresolvedAssembly LoadAssemblySlow(string assemblyFilePath)
  {
    CecilLoader cecilLoader = new CecilLoader();
    cecilLoader.DocumentationProvider = (IDocumentationProvider) this.TryGetXmlDocProvider(assemblyFilePath);
    return cecilLoader.LoadAssemblyFile(assemblyFilePath);
  }

  private XmlDocumentationProvider TryGetXmlDocProvider(string assemblyFilePath)
  {
    return this.xmlDocCache.GetOrAdd(assemblyFilePath, new Func<string, XmlDocumentationProvider>(this.TryGetXmlDocProviderSlow));
  }

  private XmlDocumentationProvider TryGetXmlDocProviderSlow(string assemblyFilePath)
  {
    string xmlDocPathSlow = this.FindXmlDocPathSlow(assemblyFilePath);
    return xmlDocPathSlow != null ? this.TryCreateXmlDocProvider(xmlDocPathSlow) : (XmlDocumentationProvider) null;
  }

  private string FindXmlDocPathSlow(string assemblyFilePath)
  {
    string path2 = Path.GetFileNameWithoutExtension(assemblyFilePath) + ".xml";
    string path1 = Path.Combine(Path.GetDirectoryName(assemblyFilePath), path2);
    if (File.Exists(path1))
      return path1;
    string path3 = Path.Combine(this.referenceAssembliesPath, path2);
    if (File.Exists(path3))
      return path3;
    foreach (string xmlDocPath in (IEnumerable<string>) this.xmlDocPathList)
    {
      string path4 = Path.Combine(xmlDocPath, path2);
      if (File.Exists(path4))
        return path4;
    }
    return (string) null;
  }

  private XmlDocumentationProvider TryCreateXmlDocProvider(string xmlFilePath)
  {
    try
    {
      return new XmlDocumentationProvider(xmlFilePath);
    }
    catch (Exception ex)
    {
      string currentMethodName = this.GetCurrentMethodName(nameof (TryCreateXmlDocProvider));
      SuppressedExceptions.TraceException(ex, currentMethodName);
      return (XmlDocumentationProvider) null;
    }
  }
}
