// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.ObjectHolderList
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class ObjectHolderList
{
  internal const int DefaultInitialSize = 8;
  internal ObjectHolder[] _values;
  internal int _count;

  internal ObjectHolderList()
    : this(8)
  {
  }

  internal ObjectHolderList(int startingSize)
  {
    this._count = 0;
    this._values = new ObjectHolder[startingSize];
  }

  internal void Add(ObjectHolder value)
  {
    if (this._count == this._values.Length)
      this.EnlargeArray();
    this._values[this._count++] = value;
  }

  internal ObjectHolderListEnumerator GetFixupEnumerator()
  {
    return new ObjectHolderListEnumerator(this, true);
  }

  private void EnlargeArray()
  {
    int length = this._values.Length * 2;
    if (length < 0)
      length = int.MaxValue;
    ObjectHolder[] destinationArray = new ObjectHolder[length];
    Array.Copy((Array) this._values, (Array) destinationArray, this._count);
    this._values = destinationArray;
  }

  internal int Version => this._count;

  internal int Count => this._count;
}
