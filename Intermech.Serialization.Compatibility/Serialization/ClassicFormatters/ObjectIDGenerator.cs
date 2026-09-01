// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.ObjectIDGenerator
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal class ObjectIDGenerator
{
  private const int NumBins = 4;
  internal int _currentCount;
  private int _currentSize;
  private long[] _ids;
  private object[] _objs;

  public ObjectIDGenerator()
  {
    this._currentCount = 1;
    this._currentSize = 3;
    this._ids = new long[this._currentSize * 4];
    this._objs = new object[this._currentSize * 4];
  }

  private int FindElement(object obj, out bool found)
  {
    int hashCode = RuntimeHelpers.GetHashCode(obj);
    int num1 = 1 + (hashCode & int.MaxValue) % (this._currentSize - 2);
    while (true)
    {
      int num2 = (hashCode & int.MaxValue) % this._currentSize * 4;
      for (int element = num2; element < num2 + 4; ++element)
      {
        if (this._objs[element] == null)
        {
          found = false;
          return element;
        }
        if (this._objs[element] == obj)
        {
          found = true;
          return element;
        }
      }
      hashCode += num1;
    }
  }

  public virtual long GetId(object obj, out bool firstTime)
  {
    bool found;
    int index = obj != null ? this.FindElement(obj, out found) : throw new ArgumentNullException(nameof (obj));
    long id;
    if (!found)
    {
      this._objs[index] = obj;
      this._ids[index] = (long) this._currentCount++;
      id = this._ids[index];
      if (this._currentCount > this._currentSize * 4 / 2)
        this.Rehash();
    }
    else
      id = this._ids[index];
    firstTime = !found;
    return id;
  }

  public virtual long HasId(object obj, out bool firstTime)
  {
    bool found;
    int index = obj != null ? this.FindElement(obj, out found) : throw new ArgumentNullException(nameof (obj));
    if (found)
    {
      firstTime = false;
      return this._ids[index];
    }
    firstTime = true;
    return 0;
  }

  private void Rehash()
  {
    int currentSize = this._currentSize;
    int num = HashHelpers.ExpandPrime(currentSize);
    this._currentSize = num != currentSize ? num : throw new SerializationException(SR2.Serialization_TooManyElements);
    long[] numArray = new long[this._currentSize * 4];
    object[] objArray = new object[this._currentSize * 4];
    long[] ids = this._ids;
    object[] objs = this._objs;
    this._ids = numArray;
    this._objs = objArray;
    for (int index = 0; index < objs.Length; ++index)
    {
      if (objs[index] != null)
      {
        int element = this.FindElement(objs[index], out bool _);
        this._objs[element] = objs[index];
        this._ids[element] = ids[index];
      }
    }
  }
}
