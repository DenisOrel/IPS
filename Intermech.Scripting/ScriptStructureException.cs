// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptStructureException
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Scripting;

[Serializable]
public class ScriptStructureException : ScriptingException
{
  public ScriptStructureException(string message)
    : base(message)
  {
  }

  protected ScriptStructureException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
