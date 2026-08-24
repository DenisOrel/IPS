// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Diff.DiffResultSpan
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System;

#nullable disable
namespace Intermech.Requirement.Diff;

public class DiffResultSpan : IComparable
{
  private const int BAD_INDEX = -1;
  private int _destIndex;
  private int _sourceIndex;
  private int _length;
  private DiffResultSpanStatus _status;

  public int DestIndex => this._destIndex;

  public int SourceIndex => this._sourceIndex;

  public int Length => this._length;

  public DiffResultSpanStatus Status => this._status;

  protected DiffResultSpan(
    DiffResultSpanStatus status,
    int destIndex,
    int sourceIndex,
    int length)
  {
    this._status = status;
    this._destIndex = destIndex;
    this._sourceIndex = sourceIndex;
    this._length = length;
  }

  public static DiffResultSpan CreateNoChange(int destIndex, int sourceIndex, int length)
  {
    return new DiffResultSpan(DiffResultSpanStatus.NoChange, destIndex, sourceIndex, length);
  }

  public static DiffResultSpan CreateReplace(int destIndex, int sourceIndex, int length)
  {
    return new DiffResultSpan(DiffResultSpanStatus.Replace, destIndex, sourceIndex, length);
  }

  public static DiffResultSpan CreateDeleteSource(int sourceIndex, int length)
  {
    return new DiffResultSpan(DiffResultSpanStatus.DeleteSource, -1, sourceIndex, length);
  }

  public static DiffResultSpan CreateAddDestination(int destIndex, int length)
  {
    return new DiffResultSpan(DiffResultSpanStatus.AddDestination, destIndex, -1, length);
  }

  public void AddLength(int i) => this._length += i;

  public override string ToString()
  {
    return $"{this._status.ToString()} (Dest: {this._destIndex.ToString()},Source: {this._sourceIndex.ToString()}) {this._length.ToString()}";
  }

  public int CompareTo(object obj) => this._destIndex.CompareTo(((DiffResultSpan) obj)._destIndex);
}
