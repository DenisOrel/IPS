// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.AddinReferenceResolver
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using System;
using System.IO;
using System.Reflection;

#nullable disable
namespace CSharpPlugin;

public sealed class AddinReferenceResolver : IDisposable
{
  private readonly string[] searchDirs;
  private bool disposed;

  public AddinReferenceResolver()
  {
    this.searchDirs = AddinReferenceResolver.CreateSearchDirs(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location));
    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.ResolveHandler);
  }

  public void Dispose()
  {
    if (this.disposed)
      return;
    this.disposed = true;
    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(this.ResolveHandler);
  }

  private void CheckAlive()
  {
    if (this.disposed)
      throw new ObjectDisposedException(this.GetType().FullName);
  }

  private static string[] CreateSearchDirs(string addinDir)
  {
    return !string.IsNullOrEmpty(addinDir) ? new string[2]
    {
      Path.GetDirectoryName(addinDir),
      addinDir
    } : throw new ArgumentNullException(nameof (addinDir));
  }

  private Assembly ResolveHandler(object sender, ResolveEventArgs args)
  {
    this.CheckAlive();
    AssemblyName assemblyName = new AssemblyName(args.Name);
    string path2 = string.Compare(Path.GetExtension(assemblyName.Name), ".dll", true) == 0 ? assemblyName.Name : assemblyName.Name + ".dll";
    foreach (string searchDir in this.searchDirs)
    {
      string path = Path.Combine(searchDir, path2);
      if (File.Exists(path))
      {
        try
        {
          return Assembly.LoadFile(path);
        }
        catch (Exception ex)
        {
        }
      }
    }
    return (Assembly) null;
  }
}
