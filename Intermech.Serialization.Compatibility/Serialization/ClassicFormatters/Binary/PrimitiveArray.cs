// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.PrimitiveArray
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Globalization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class PrimitiveArray
{
  private readonly InternalPrimitiveTypeE _code;
  private readonly bool[] _booleanA;
  private readonly char[] _charA;
  private readonly double[] _doubleA;
  private readonly short[] _int16A;
  private readonly int[] _int32A;
  private readonly long[] _int64A;
  private readonly sbyte[] _sbyteA;
  private readonly float[] _singleA;
  private readonly ushort[] _uint16A;
  private readonly uint[] _uint32A;
  private readonly ulong[] _uint64A;

  internal PrimitiveArray(InternalPrimitiveTypeE code, Array array)
  {
    this._code = code;
    switch (code)
    {
      case InternalPrimitiveTypeE.Boolean:
        this._booleanA = (bool[]) array;
        break;
      case InternalPrimitiveTypeE.Char:
        this._charA = (char[]) array;
        break;
      case InternalPrimitiveTypeE.Double:
        this._doubleA = (double[]) array;
        break;
      case InternalPrimitiveTypeE.Int16:
        this._int16A = (short[]) array;
        break;
      case InternalPrimitiveTypeE.Int32:
        this._int32A = (int[]) array;
        break;
      case InternalPrimitiveTypeE.Int64:
        this._int64A = (long[]) array;
        break;
      case InternalPrimitiveTypeE.SByte:
        this._sbyteA = (sbyte[]) array;
        break;
      case InternalPrimitiveTypeE.Single:
        this._singleA = (float[]) array;
        break;
      case InternalPrimitiveTypeE.UInt16:
        this._uint16A = (ushort[]) array;
        break;
      case InternalPrimitiveTypeE.UInt32:
        this._uint32A = (uint[]) array;
        break;
      case InternalPrimitiveTypeE.UInt64:
        this._uint64A = (ulong[]) array;
        break;
    }
  }

  internal void SetValue(string value, int index)
  {
    switch (this._code)
    {
      case InternalPrimitiveTypeE.Boolean:
        this._booleanA[index] = bool.Parse(value);
        break;
      case InternalPrimitiveTypeE.Char:
        if (value[0] == '_' && value.Equals("_0x00_"))
        {
          this._charA[index] = char.MinValue;
          break;
        }
        this._charA[index] = char.Parse(value);
        break;
      case InternalPrimitiveTypeE.Double:
        this._doubleA[index] = double.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case InternalPrimitiveTypeE.Int16:
        this._int16A[index] = short.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case InternalPrimitiveTypeE.Int32:
        this._int32A[index] = int.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case InternalPrimitiveTypeE.Int64:
        this._int64A[index] = long.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case InternalPrimitiveTypeE.SByte:
        this._sbyteA[index] = sbyte.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case InternalPrimitiveTypeE.Single:
        this._singleA[index] = float.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case InternalPrimitiveTypeE.UInt16:
        this._uint16A[index] = ushort.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case InternalPrimitiveTypeE.UInt32:
        this._uint32A[index] = uint.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      case InternalPrimitiveTypeE.UInt64:
        this._uint64A[index] = ulong.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
    }
  }
}
