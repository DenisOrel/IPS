// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.ISyntaxModeFileProvider
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal interface ISyntaxModeFileProvider
{
  XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode);

  ICollection<SyntaxMode> SyntaxModes { get; }
}
