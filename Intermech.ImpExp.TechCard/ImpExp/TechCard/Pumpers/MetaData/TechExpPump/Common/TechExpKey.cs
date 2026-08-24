// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common.TechExpKey
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common;

public struct TechExpKey(long value) : IComparable, IComparable<TechExpKey>, IEquatable<TechExpKey>
{
  private long _value = value;

  public long Value
  {
    get => this._value;
    private set => this._value = value;
  }

  public int CompareTo(object obj) => !(obj is TechExpKey other) ? -1 : this.CompareTo(other);

  public int CompareTo(TechExpKey other) => this.Value.CompareTo(other.Value);

  public bool Equals(TechExpKey other) => this.CompareTo(other) == 0;
}
