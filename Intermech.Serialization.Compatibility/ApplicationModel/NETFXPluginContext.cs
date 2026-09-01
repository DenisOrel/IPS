// Decompiled with JetBrains decompiler
// Type: Intermech.ApplicationModel.NETFXPluginContext
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System.IO;
using System.Reflection;

#nullable disable
namespace Intermech.ApplicationModel;

internal sealed class NETFXPluginContext(string pluginFile, string pluginDirectory) : 
  AppPluginContext(pluginFile, pluginDirectory)
{
  public string TryResolve(AssemblyName assemblyName)
  {
    string path = Path.Combine(this.PluginDirectory, assemblyName.Name + ".dll");
    return !File.Exists(path) ? (string) null : path;
  }
}
