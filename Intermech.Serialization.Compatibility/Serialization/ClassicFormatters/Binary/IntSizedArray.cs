// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.IntSizedArray
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class IntSizedArray : ICloneable
{
  internal int[] _objects = new int[16 /*0x10*/];
  internal int[] _negObjects = new int[4];

  public IntSizedArray()
  {
  }

  private IntSizedArray(IntSizedArray sizedArray)
  {
    this._objects = new int[sizedArray._objects.Length];
    sizedArray._objects.CopyTo((Array) this._objects, 0);
    this._negObjects = new int[sizedArray._negObjects.Length];
    sizedArray._negObjects.CopyTo((Array) this._negObjects, 0);
  }

  public object Clone() => (object) new IntSizedArray(this);

  internal int this[int index]
  {
    get
    {
      return index < 0 ? (-index <= this._negObjects.Length - 1 ? this._negObjects[-index] : 0) : (index <= this._objects.Length - 1 ? this._objects[index] : 0);
    }
    set
    {
      if (index < 0)
      {
        if (-index > this._negObjects.Length - 1)
          this.IncreaseCapacity(index);
        this._negObjects[-index] = value;
      }
      else
      {
        if (index > this._objects.Length - 1)
          this.IncreaseCapacity(index);
        this._objects[index] = value;
      }
    }
  }

  internal void IncreaseCapacity(int index)
  {
    try
    {
      if (index < 0)
      {
        int[] destinationArray = new int[Math.Max(this._negObjects.Length * 2, -index + 1)];
        Array.Copy((Array) this._negObjects, (Array) destinationArray, this._negObjects.Length);
        this._negObjects = destinationArray;
      }
      else
      {
        int[] destinationArray = new int[Math.Max(this._objects.Length * 2, index + 1)];
        Array.Copy((Array) this._objects, (Array) destinationArray, this._objects.Length);
        this._objects = destinationArray;
      }
    }
    catch (Exception ex)
    {
      throw new SerializationException(SR2.Serialization_CorruptedStream);
    }
  }
}
