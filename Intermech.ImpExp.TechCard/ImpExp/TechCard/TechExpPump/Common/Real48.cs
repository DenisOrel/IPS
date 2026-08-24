// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.Common.Real48
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.Common;

public static class Real48
{
  private static int _real48_Size = 6;

  public static double Real48ToDouble(BinaryReader reader)
  {
    return reader != null ? Real48.Real48ToDouble(reader.ReadBytes(Real48._real48_Size)) : throw new FormatException("Real value's buffer is empty");
  }

  private static double Real48ToDouble(byte[] realValue)
  {
    if (realValue.Length != Real48._real48_Size)
      throw new FormatException("Invalid real value's buffer");
    if (realValue[0] == (byte) 0)
      return 0.0;
    double y = (double) realValue[0] - 129.0;
    double num1 = 0.0;
    for (int index = 1; index <= 4; ++index)
      num1 = (num1 + (double) realValue[index]) * (1.0 / 256.0);
    double num2 = (num1 + (double) ((int) realValue[5] & (int) sbyte.MaxValue)) * (1.0 / 128.0) + 1.0;
    if (((int) realValue[5] & 128 /*0x80*/) != 0)
      num2 = -num2;
    return num2 * Math.Pow(2.0, y);
  }
}
