// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.SyntaxMode
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class SyntaxMode
{
  public SyntaxMode(string fileName, string name, params string[] extensions)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (extensions == null)
      throw new ArgumentNullException(nameof (extensions));
    this.FileName = fileName;
    this.Name = name;
    this.Extensions = extensions;
  }

  public string FileName { get; private set; }

  public string Name { get; private set; }

  public string[] Extensions { get; private set; }
}
