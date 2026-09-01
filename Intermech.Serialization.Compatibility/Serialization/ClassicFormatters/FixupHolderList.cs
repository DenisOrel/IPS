// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.FixupHolderList
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class FixupHolderList
{
  internal const int InitialSize = 2;
  internal FixupHolder[] _values;
  internal int _count;

  internal FixupHolderList()
    : this(2)
  {
  }

  internal FixupHolderList(int startingSize)
  {
    this._count = 0;
    this._values = new FixupHolder[startingSize];
  }

  internal void Add(FixupHolder fixup)
  {
    if (this._count == this._values.Length)
      this.EnlargeArray();
    this._values[this._count++] = fixup;
  }

  private void EnlargeArray()
  {
    int length = this._values.Length * 2;
    if (length < 0)
      length = int.MaxValue;
    FixupHolder[] destinationArray = new FixupHolder[length];
    Array.Copy((Array) this._values, (Array) destinationArray, this._count);
    this._values = destinationArray;
  }
}
