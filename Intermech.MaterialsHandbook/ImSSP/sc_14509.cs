// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14509
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14509
{
  private static byte[] sspq = new byte[40]
  {
    (byte) 51,
    (byte) 57,
    (byte) 216,
    (byte) 249,
    (byte) 236,
    (byte) 152,
    (byte) 103,
    (byte) 145,
    (byte) 66,
    (byte) 138,
    (byte) 250,
    (byte) 42,
    (byte) 181,
    (byte) 65,
    (byte) 97,
    (byte) 47,
    (byte) 113,
    (byte) 186,
    (byte) 65,
    (byte) 94,
    (byte) 196,
    (byte) 183,
    (byte) 214,
    (byte) 62,
    (byte) 57,
    (byte) 151,
    (byte) 79,
    (byte) 8,
    (byte) 14,
    (byte) 208 /*0xD0*/,
    (byte) 133,
    (byte) 151,
    (byte) 240 /*0xF0*/,
    (byte) 121,
    (byte) 58,
    (byte) 116,
    (byte) 105,
    (byte) 15,
    (byte) 124,
    (byte) 47
  };
  private static byte[] sspr = new byte[40]
  {
    (byte) 152,
    (byte) 228,
    (byte) 211,
    (byte) 157,
    (byte) 195,
    (byte) 37,
    (byte) 189,
    (byte) 248,
    (byte) 91,
    (byte) 114,
    (byte) 49,
    (byte) 28,
    (byte) 218,
    (byte) 9,
    (byte) 93,
    (byte) 206,
    (byte) 210,
    (byte) 40,
    (byte) 196,
    (byte) 168,
    (byte) 200,
    (byte) 118,
    (byte) 89,
    (byte) 14,
    (byte) 131,
    (byte) 221,
    (byte) 114,
    (byte) 148,
    (byte) 193,
    (byte) 111,
    (byte) 149,
    (byte) 49,
    (byte) 14,
    (byte) 164,
    (byte) 77,
    (byte) 47,
    (byte) 24,
    (byte) 252,
    (byte) 147,
    (byte) 68
  };

  internal static int ssp_imbase_14510(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray1 = new byte[4];
    byte[] response1 = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/]
    {
      (byte) 81,
      (byte) 153,
      (byte) 219,
      (byte) 142,
      (byte) 107,
      (byte) 210,
      (byte) 246,
      (byte) 166,
      (byte) 115,
      (byte) 53,
      (byte) 62,
      (byte) 153,
      (byte) 152,
      (byte) 63 /*0x3F*/,
      (byte) 14,
      (byte) 40,
      (byte) 115,
      (byte) 10,
      (byte) 251,
      (byte) 203,
      (byte) 145,
      (byte) 95,
      (byte) 65,
      (byte) 119,
      (byte) 223,
      (byte) 92,
      (byte) 96 /*0x60*/,
      (byte) 123,
      (byte) 131,
      (byte) 106,
      (byte) 95,
      (byte) 172,
      (byte) 90,
      (byte) 35,
      (byte) 249,
      (byte) 150,
      (byte) 250,
      (byte) 105,
      (byte) 136,
      (byte) 175,
      (byte) 58,
      (byte) 151,
      (byte) 104,
      (byte) 19,
      (byte) 200,
      (byte) 113,
      (byte) 174,
      (byte) 30
    };
    byte[] sourceArray2 = new byte[48 /*0x30*/];
    sourceArray2[29] = (byte) 33;
    sourceArray2[1] = (byte) 114;
    sourceArray2[16 /*0x10*/] = (byte) 92;
    sourceArray2[39] = (byte) 253;
    sourceArray2[4] = (byte) 66;
    sourceArray2[26] = (byte) 203;
    sourceArray2[27] = (byte) 133;
    sourceArray2[6] = (byte) 31 /*0x1F*/;
    sourceArray2[8] = (byte) 119;
    sourceArray2[35] = (byte) 213;
    sourceArray2[10] = (byte) 237;
    sourceArray2[47] = (byte) 28;
    sourceArray2[19] = (byte) 197;
    sourceArray2[13] = (byte) 43;
    sourceArray2[44] = (byte) 10;
    sourceArray2[15] = (byte) 139;
    sourceArray2[21] = (byte) 165;
    sourceArray2[17] = (byte) 248;
    sourceArray2[18] = (byte) 93;
    sourceArray2[0] = (byte) 19;
    sourceArray2[5] = (byte) 227;
    sourceArray2[45] = (byte) 51;
    sourceArray2[22] = (byte) 129;
    sourceArray2[42] = (byte) 57;
    sourceArray2[24] = (byte) 155;
    sourceArray2[25] = (byte) 103;
    sourceArray2[32 /*0x20*/] = (byte) 169;
    sourceArray2[31 /*0x1F*/] = (byte) 97;
    sourceArray2[28] = (byte) 192 /*0xC0*/;
    sourceArray2[14] = (byte) 223;
    sourceArray2[7] = (byte) 239;
    sourceArray2[20] = (byte) 25;
    sourceArray2[9] = (byte) 20;
    sourceArray2[33] = (byte) 174;
    sourceArray2[12] = (byte) 213;
    sourceArray2[30] = (byte) 179;
    sourceArray2[36] = (byte) 166;
    sourceArray2[37] = (byte) 32 /*0x20*/;
    sourceArray2[38] = (byte) 44;
    sourceArray2[2] = (byte) 108;
    sourceArray2[40] = (byte) 115;
    sourceArray2[41] = (byte) 250;
    sourceArray2[34] = (byte) 250;
    sourceArray2[43] = (byte) 143;
    sourceArray2[23] = (byte) 52;
    sourceArray2[3] = (byte) 166;
    sourceArray2[46] = (byte) 234;
    sourceArray2[11] = (byte) 31 /*0x1F*/;
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray1, 0, 4);
    key.Query(true, 343, numArray1, response1);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray1, 0, 4);
    byte[] numArray2 = new byte[40];
    byte[] response2 = new byte[40];
    Array.Copy((Array) sc_14509.sspq, 0, (Array) numArray2, 0, 40);
    key.Query(true, 343, numArray2, response2);
    Array.Copy((Array) sc_14509.sspr, 0, (Array) numArray2, 0, 40);
    for (int index = 0; index < numArray2.Length; ++index)
    {
      if ((int) numArray2[index] != (int) response2[index])
      {
        key.TagValue = (int) response2[index];
        break;
      }
    }
    return BitConverter.ToInt32(response1, 0) ^ BitConverter.ToInt32(numArray1, 0) ^ k;
  }
}
