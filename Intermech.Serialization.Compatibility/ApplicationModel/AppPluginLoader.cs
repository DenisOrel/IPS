// Decompiled with JetBrains decompiler
// Type: Intermech.ApplicationModel.AppPluginLoader
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

#nullable disable
namespace Intermech.ApplicationModel;

public class AppPluginLoader
{
  private readonly string baseDirectory;
  private static readonly string DirectorySeparatorChar = new string(Path.DirectorySeparatorChar, 1);
  private static AppPluginLoader s_instance = new AppPluginLoader();

  public AppPluginLoader() => this.baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

  public string BaseDirectory
  {
    [DebuggerStepThrough] get => this.baseDirectory;
  }

  public Assembly LoadPlugin(string pluginFile)
  {
    if (pluginFile == null)
      throw new ArgumentNullException(nameof (pluginFile));
    if (pluginFile == string.Empty)
      throw new ArgumentException("The plugin file name must not be empty.", nameof (pluginFile));
    if (!Path.IsPathRooted(pluginFile))
      pluginFile = Path.GetFullPath(Path.Combine(this.BaseDirectory, pluginFile));
    return this.DoLoadPlugin(pluginFile);
  }

  protected virtual Assembly DoLoadPlugin(string pluginFile)
  {
    return !string.Equals(Path.GetDirectoryName(pluginFile), this.BaseDirectory, StringComparison.CurrentCultureIgnoreCase) ? Assembly.LoadFrom(pluginFile) : Assembly.Load(Path.GetFileNameWithoutExtension(pluginFile));
  }

  protected bool IsPathEquals(string path1, string path2)
  {
    return string.Equals(path1, path2, StringComparison.InvariantCultureIgnoreCase);
  }

  protected string GetPluginDirectory(string pluginFile)
  {
    string directoryName = Path.GetDirectoryName(pluginFile);
    if (!string.IsNullOrEmpty(directoryName) && !directoryName.EndsWith(AppPluginLoader.DirectorySeparatorChar))
      directoryName += AppPluginLoader.DirectorySeparatorChar;
    return directoryName;
  }

  public static AppPluginLoader Instance
  {
    [DebuggerStepThrough] get => AppPluginLoader.s_instance;
    [DebuggerStepThrough] set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      Interlocked.Exchange<AppPluginLoader>(ref AppPluginLoader.s_instance, value);
    }
  }
}
