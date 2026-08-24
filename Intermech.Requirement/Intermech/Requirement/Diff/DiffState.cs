// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Diff.DiffState
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

#nullable disable
namespace Intermech.Requirement.Diff;

internal class DiffState
{
  private const int BAD_INDEX = -1;
  private int _startIndex;
  private int _length;

  public int StartIndex => this._startIndex;

  public int EndIndex => this._startIndex + this._length - 1;

  public int Length => this._length <= 0 ? (this._length != 0 ? 0 : 1) : this._length;

  public DiffStatus Status
  {
    get
    {
      return this._length <= 0 ? (this._length != -1 ? DiffStatus.Unknown : DiffStatus.NoMatch) : DiffStatus.Matched;
    }
  }

  public DiffState() => this.SetToUnkown();

  protected void SetToUnkown()
  {
    this._startIndex = -1;
    this._length = -2;
  }

  public void SetMatch(int start, int length)
  {
    this._startIndex = start;
    this._length = length;
  }

  public void SetNoMatch()
  {
    this._startIndex = -1;
    this._length = -1;
  }

  public bool HasValidLength(int newStart, int newEnd, int maxPossibleDestLength)
  {
    if (this._length > 0 && (maxPossibleDestLength < this._length || this._startIndex < newStart || this.EndIndex > newEnd))
      this.SetToUnkown();
    return this._length != -2;
  }
}
