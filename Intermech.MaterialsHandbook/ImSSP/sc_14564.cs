// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14564
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_14564
{
  internal static string ssp_imbase_14565()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 9)
    {
      byte[] numArray1 = new byte[9];
      byte[] numArray2 = new byte[9]
      {
        (byte) 61,
        (byte) 69,
        (byte) 20,
        (byte) 12,
        (byte) 20,
        (byte) 224 /*0xE0*/,
        (byte) 120,
        (byte) 23,
        (byte) 13
      };
      byte[] numArray3 = new byte[9]
      {
        (byte) 243,
        (byte) 247,
        (byte) 101,
        (byte) 226,
        (byte) 73,
        (byte) 113,
        (byte) 173,
        (byte) 95,
        (byte) 227
      };
      key.Query(true, 343, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 9);
      for (int index = 0; index < 9; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[9];
    byte[] numArray5 = new byte[9]
    {
      (byte) 222,
      (byte) 113,
      (byte) 124,
      (byte) 99,
      (byte) 154,
      (byte) 192 /*0xC0*/,
      (byte) 84,
      (byte) 72,
      (byte) 95
    };
    byte[] numArray6 = new byte[9];
    numArray6[7] = (byte) 54;
    numArray6[1] = (byte) 151;
    numArray6[2] = (byte) 42;
    numArray6[5] = (byte) 61;
    numArray6[3] = (byte) 189;
    numArray6[0] = (byte) 249;
    numArray6[6] = (byte) 142;
    numArray6[4] = (byte) 55;
    numArray6[8] = (byte) 206;
    key.Query(true, 343, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 9);
    for (int index = 0; index < 9; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
