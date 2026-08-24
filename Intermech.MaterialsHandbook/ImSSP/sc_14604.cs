// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14604
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14604
{
  internal static int ssp_imbase_14605(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/]
    {
      (byte) 207,
      (byte) 54,
      (byte) 91,
      (byte) 159,
      (byte) 5,
      (byte) 245,
      (byte) 17,
      (byte) 23,
      (byte) 77,
      (byte) 229,
      (byte) 8,
      (byte) 249,
      (byte) 138,
      (byte) 72,
      (byte) 203,
      (byte) 30,
      (byte) 46,
      (byte) 248,
      (byte) 137,
      (byte) 143,
      (byte) 75,
      (byte) 167,
      (byte) 141,
      (byte) 170,
      (byte) 61,
      (byte) 9,
      (byte) 89,
      (byte) 8,
      (byte) 116,
      (byte) 147,
      (byte) 118,
      (byte) 113,
      (byte) 174,
      (byte) 24,
      (byte) 156,
      (byte) 95,
      (byte) 23,
      (byte) 235,
      (byte) 225,
      (byte) 75,
      (byte) 249,
      (byte) 64 /*0x40*/,
      (byte) 71,
      (byte) 0,
      (byte) 183,
      (byte) 130,
      (byte) 83,
      (byte) 231
    };
    byte[] sourceArray2 = new byte[48 /*0x30*/]
    {
      (byte) 175,
      (byte) 39,
      (byte) 229,
      (byte) 33,
      (byte) 62,
      (byte) 71,
      (byte) 155,
      (byte) 75,
      (byte) 129,
      (byte) 23,
      (byte) 117,
      (byte) 167,
      (byte) 22,
      (byte) 224 /*0xE0*/,
      (byte) 108,
      (byte) 88,
      (byte) 155,
      (byte) 43,
      (byte) 240 /*0xF0*/,
      (byte) 10,
      (byte) 223,
      (byte) 201,
      (byte) 189,
      (byte) 250,
      (byte) 171,
      (byte) 56,
      (byte) 28,
      (byte) 177,
      (byte) 79,
      (byte) 191,
      (byte) 133,
      (byte) 153,
      (byte) 13,
      (byte) 0,
      (byte) 37,
      (byte) 87,
      (byte) 100,
      (byte) 84,
      (byte) 162,
      (byte) 180,
      (byte) 30,
      (byte) 94,
      (byte) 160 /*0xA0*/,
      (byte) 106,
      (byte) 229,
      (byte) 149,
      (byte) 237,
      (byte) 126
    };
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 343, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}
