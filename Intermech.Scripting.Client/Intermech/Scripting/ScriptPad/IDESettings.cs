// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IDESettings
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class IDESettings : ICloneable
{
  private List<string> xmlDocPathList;

  public IDESettings()
  {
    this.FontFamily = "Consolas";
    this.FontSize = 14;
    this.EnableCodeCompletion = true;
    this.xmlDocPathList = new List<string>();
  }

  public string FontFamily { get; set; }

  public int FontSize { get; set; }

  public bool EnableCodeCompletion { get; set; }

  public List<string> XmlDocPathList
  {
    [DebuggerStepThrough] get => this.xmlDocPathList;
    [DebuggerStepThrough] set
    {
      this.xmlDocPathList = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public IDESettings Clone()
  {
    IDESettings ideSettings = new IDESettings();
    ideSettings.FontFamily = this.FontFamily;
    ideSettings.FontSize = this.FontSize;
    ideSettings.EnableCodeCompletion = this.EnableCodeCompletion;
    ideSettings.XmlDocPathList.AddRange((IEnumerable<string>) this.XmlDocPathList);
    return ideSettings;
  }

  object ICloneable.Clone() => (object) this.Clone();
}
