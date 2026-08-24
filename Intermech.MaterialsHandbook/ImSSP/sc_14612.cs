// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14612
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14612
{
  internal static int ssp_imbase_14613(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/]
    {
      (byte) 111,
      (byte) 117,
      (byte) 237,
      (byte) 2,
      (byte) 158,
      (byte) 197,
      (byte) 135,
      (byte) 203,
      (byte) 190,
      (byte) 233,
      (byte) 34,
      (byte) 248,
      (byte) 34,
      (byte) 233,
      (byte) 35,
      (byte) 115,
      (byte) 242,
      (byte) 208 /*0xD0*/,
      (byte) 249,
      (byte) 219,
      (byte) 169,
      (byte) 25,
      (byte) 201,
      (byte) 7,
      (byte) 141,
      (byte) 29,
      (byte) 245,
      (byte) 213,
      (byte) 148,
      (byte) 9,
      (byte) 119,
      (byte) 108,
      (byte) 235,
      (byte) 118,
      (byte) 100,
      (byte) 13,
      (byte) 230,
      (byte) 52,
      (byte) 83,
      (byte) 220,
      (byte) 70,
      (byte) 152,
      (byte) 217,
      (byte) 15,
      (byte) 201,
      (byte) 178,
      (byte) 118,
      (byte) 192 /*0xC0*/
    };
    byte[] sourceArray2 = new byte[48 /*0x30*/]
    {
      (byte) 33,
      (byte) 26,
      (byte) 95,
      (byte) 42,
      (byte) 147,
      (byte) 35,
      (byte) 209,
      (byte) 110,
      (byte) 114,
      (byte) 88,
      (byte) 167,
      (byte) 184,
      (byte) 96 /*0x60*/,
      (byte) 177,
      (byte) 13,
      (byte) 174,
      (byte) 121,
      (byte) 189,
      (byte) 208 /*0xD0*/,
      (byte) 131,
      (byte) 51,
      (byte) 213,
      (byte) 22,
      (byte) 139,
      (byte) 95,
      (byte) 59,
      (byte) 253,
      (byte) 59,
      (byte) 84,
      (byte) 227,
      (byte) 124,
      (byte) 235,
      (byte) 189,
      (byte) 67,
      (byte) 108,
      (byte) 108,
      (byte) 61,
      (byte) 129,
      (byte) 158,
      (byte) 38,
      (byte) 201,
      (byte) 92,
      (byte) 159,
      (byte) 50,
      (byte) 212,
      (byte) 75,
      (byte) 213,
      (byte) 78
    };
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 343, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}
