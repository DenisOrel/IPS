// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Projects.DiskScripts.DiskScriptKey
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.IO;
using System;

#nullable disable
namespace Intermech.Scripting.Projects.DiskScripts;

internal sealed class DiskScriptKey : IEquatable<DiskScriptKey>
{
  private readonly string path;

  public DiskScriptKey(string path)
  {
    this.path = path != null ? path : throw new ArgumentNullException(nameof (path));
  }

  public string Path => this.path;

  public bool Equals(DiskScriptKey other)
  {
    return other != null && PathUtils.IsSamePath(other.Path, this.Path);
  }

  public override bool Equals(object obj)
  {
    return !(obj is DiskScriptKey other) ? base.Equals(obj) : this.Equals(other);
  }

  public override int GetHashCode() => this.path.GetHashCode();

  internal static DiskScriptKey CastFrom(object key)
  {
    return key != null ? (DiskScriptKey) key : throw new ArgumentNullException(nameof (key));
  }
}
