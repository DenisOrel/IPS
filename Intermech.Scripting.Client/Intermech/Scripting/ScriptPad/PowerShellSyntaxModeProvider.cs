// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.PowerShellSyntaxModeProvider
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System.Collections.Generic;
using System.Reflection;
using System.Xml;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal sealed class PowerShellSyntaxModeProvider : ISyntaxModeFileProvider
{
  private readonly SyntaxMode mainMode;
  private readonly IList<SyntaxMode> allModes;

  public PowerShellSyntaxModeProvider()
  {
    this.mainMode = new SyntaxMode("PowerShell.xshd", "PowerShell", new string[2]
    {
      ".ps1",
      ".psm1"
    });
    this.allModes = (IList<SyntaxMode>) new List<SyntaxMode>();
    this.allModes = (IList<SyntaxMode>) new SyntaxMode[1]
    {
      this.mainMode
    };
  }

  public XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode)
  {
    string name = $"{this.GetType().Namespace}.{syntaxMode.FileName}";
    return new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(name));
  }

  public ICollection<SyntaxMode> SyntaxModes => (ICollection<SyntaxMode>) this.allModes;
}
