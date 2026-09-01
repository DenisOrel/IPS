// Decompiled with JetBrains decompiler
// Type: Intermech.ApplicationModel.AppPluginContext
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System.Diagnostics;

#nullable disable
namespace Intermech.ApplicationModel;

public abstract class AppPluginContext
{
  private readonly string _id;
  private readonly string _pluginFile;
  private readonly string _pluginDirectory;

  public AppPluginContext(string pluginFile, string pluginDirectory)
  {
    this._id = pluginFile.ToLowerInvariant();
    this._pluginFile = pluginFile;
    this._pluginDirectory = pluginDirectory;
  }

  public string Id
  {
    [DebuggerStepThrough] get => this._id;
  }

  public string PluginFile
  {
    [DebuggerStepThrough] get => this._pluginFile;
  }

  public string PluginDirectory
  {
    [DebuggerStepThrough] get => this._pluginDirectory;
  }
}
