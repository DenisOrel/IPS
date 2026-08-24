// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.DiskScriptSaveAsParameters
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal sealed class DiskScriptSaveAsParameters : ScriptSaveAsParameters
{
  private string path;

  public DiskScriptSaveAsParameters(string path)
  {
    this.path = path != null ? path : throw new ArgumentNullException(nameof (path));
  }

  public string Path => this.path;
}
