// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ScriptCodeKey
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common;

[Serializable]
public sealed class ScriptCodeKey : IEquatable<ScriptCodeKey>
{
  private static readonly ByteArrayComparer scriptCodeHashComparer = new ByteArrayComparer();

  public ScriptCodeKey(byte[] scriptCodeHash, bool hasDebugInfo)
  {
    this.ScriptCodeHash = scriptCodeHash != null ? scriptCodeHash : throw new ArgumentNullException(nameof (scriptCodeHash));
    this.HasDebugInfo = hasDebugInfo;
  }

  public byte[] ScriptCodeHash { get; private set; }

  public bool HasDebugInfo { get; private set; }

  public bool Equals(ScriptCodeKey other)
  {
    return other != null && ScriptCodeKey.scriptCodeHashComparer.Equals(this.ScriptCodeHash, other.ScriptCodeHash) && this.HasDebugInfo == other.HasDebugInfo;
  }

  public override bool Equals(object obj)
  {
    return !(obj is ScriptCodeKey other) ? base.Equals(obj) : this.Equals(other);
  }

  public override int GetHashCode()
  {
    return ScriptCodeKey.scriptCodeHashComparer.GetHashCode(this.ScriptCodeHash) ^ (this.HasDebugInfo ? 32768 /*0x8000*/ : 0);
  }
}
