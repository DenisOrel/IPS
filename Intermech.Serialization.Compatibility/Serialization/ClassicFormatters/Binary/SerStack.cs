// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.SerStack
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class SerStack
{
  internal object[] _objects = new object[5];
  internal string _stackId;
  internal int _top = -1;

  internal SerStack(string stackId) => this._stackId = stackId;

  internal void Push(object obj)
  {
    if (this._top == this._objects.Length - 1)
      this.IncreaseCapacity();
    this._objects[++this._top] = obj;
  }

  internal object Pop()
  {
    if (this._top < 0)
      return (object) null;
    object obj = this._objects[this._top];
    this._objects[this._top--] = (object) null;
    return obj;
  }

  internal void IncreaseCapacity()
  {
    object[] destinationArray = new object[this._objects.Length * 2];
    Array.Copy((Array) this._objects, (Array) destinationArray, this._objects.Length);
    this._objects = destinationArray;
  }

  internal object Peek() => this._top >= 0 ? this._objects[this._top] : (object) null;

  internal object PeekPeek() => this._top >= 1 ? this._objects[this._top - 1] : (object) null;

  internal bool IsEmpty() => this._top <= 0;
}
