// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.LongList
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class LongList
{
  private const int InitialSize = 2;
  private long[] _values;
  private int _count;
  private int _totalItems;
  private int _currentItem;

  internal LongList()
    : this(2)
  {
  }

  internal LongList(int startingSize)
  {
    this._count = 0;
    this._totalItems = 0;
    this._values = new long[startingSize];
  }

  internal void Add(long value)
  {
    if (this._totalItems == this._values.Length)
      this.EnlargeArray();
    this._values[this._totalItems++] = value;
    ++this._count;
  }

  internal int Count => this._count;

  internal void StartEnumeration() => this._currentItem = -1;

  internal bool MoveNext()
  {
    do
      ;
    while (++this._currentItem < this._totalItems && this._values[this._currentItem] == -1L);
    return this._currentItem != this._totalItems;
  }

  internal long Current => this._values[this._currentItem];

  internal bool RemoveElement(long value)
  {
    int index = 0;
    while (index < this._totalItems && this._values[index] != value)
      ++index;
    if (index == this._totalItems)
      return false;
    this._values[index] = -1L;
    return true;
  }

  private void EnlargeArray()
  {
    int length = this._values.Length * 2;
    if (length < 0)
      length = int.MaxValue;
    long[] destinationArray = new long[length];
    Array.Copy((Array) this._values, (Array) destinationArray, this._count);
    this._values = destinationArray;
  }
}
