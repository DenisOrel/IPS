// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.ObjectHolderListEnumerator
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class ObjectHolderListEnumerator
{
  private readonly bool _isFixupEnumerator;
  private readonly ObjectHolderList _list;
  private readonly int _startingVersion;
  private int _currPos;

  internal ObjectHolderListEnumerator(ObjectHolderList list, bool isFixupEnumerator)
  {
    this._list = list;
    this._startingVersion = this._list.Version;
    this._currPos = -1;
    this._isFixupEnumerator = isFixupEnumerator;
  }

  internal bool MoveNext()
  {
    if (this._isFixupEnumerator)
    {
      do
        ;
      while (++this._currPos < this._list.Count && this._list._values[this._currPos].CompletelyFixed);
      return this._currPos != this._list.Count;
    }
    ++this._currPos;
    return this._currPos != this._list.Count;
  }

  internal ObjectHolder Current => this._list._values[this._currPos];
}
