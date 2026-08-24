// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisStatusKey
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public struct VisStatusKey(Guid g, int i) : IEquatable<VisStatusKey>
{
  public Guid PluginGuid { get; private set; } = g;

  public int StatusSet { get; private set; } = i;

  public override bool Equals(object obj) => obj is VisStatusKey other && this._Equals(other);

  public override int GetHashCode() => this.PluginGuid.GetHashCode() ^ this.StatusSet;

  public override string ToString()
  {
    return $"{this.PluginGuid.ToString()} [{this.StatusSet.ToString()}]";
  }

  internal bool _Equals(VisStatusKey other)
  {
    return this.PluginGuid.Equals(other.PluginGuid) && this.StatusSet == other.StatusSet;
  }

  public bool Equals(VisStatusKey other) => this._Equals(other);
}
