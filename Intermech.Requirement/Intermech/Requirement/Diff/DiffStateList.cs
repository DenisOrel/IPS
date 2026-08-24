// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Diff.DiffStateList
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

#nullable disable
namespace Intermech.Requirement.Diff;

internal class DiffStateList
{
  private DiffState[] _array;

  public DiffStateList(int destCount) => this._array = new DiffState[destCount];

  public DiffState GetByIndex(int index)
  {
    DiffState byIndex = this._array[index];
    if (byIndex == null)
    {
      byIndex = new DiffState();
      this._array[index] = byIndex;
    }
    return byIndex;
  }
}
