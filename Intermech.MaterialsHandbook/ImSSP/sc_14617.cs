// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14617
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14617
{
  internal static int ssp_imbase_14618(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[4] = (byte) 230;
    sourceArray1[30] = (byte) 106;
    sourceArray1[2] = (byte) 86;
    sourceArray1[5] = (byte) 117;
    sourceArray1[45] = (byte) 156;
    sourceArray1[29] = (byte) 20;
    sourceArray1[6] = (byte) 128 /*0x80*/;
    sourceArray1[27] = (byte) 167;
    sourceArray1[8] = (byte) 178;
    sourceArray1[9] = (byte) 54;
    sourceArray1[26] = (byte) 219;
    sourceArray1[11] = (byte) 114;
    sourceArray1[32 /*0x20*/] = (byte) 25;
    sourceArray1[24] = (byte) 186;
    sourceArray1[1] = (byte) 218;
    sourceArray1[10] = (byte) 180;
    sourceArray1[36] = (byte) 13;
    sourceArray1[17] = (byte) 141;
    sourceArray1[18] = (byte) 190;
    sourceArray1[22] = (byte) 17;
    sourceArray1[20] = (byte) 29;
    sourceArray1[21] = (byte) 149;
    sourceArray1[39] = (byte) 191;
    sourceArray1[23] = (byte) 63 /*0x3F*/;
    sourceArray1[0] = (byte) 9;
    sourceArray1[7] = (byte) 208 /*0xD0*/;
    sourceArray1[38] = (byte) 0;
    sourceArray1[16 /*0x10*/] = (byte) 21;
    sourceArray1[28] = (byte) 52;
    sourceArray1[14] = (byte) 171;
    sourceArray1[12] = (byte) 182;
    sourceArray1[46] = (byte) 19;
    sourceArray1[13] = (byte) 13;
    sourceArray1[33] = (byte) 101;
    sourceArray1[34] = (byte) 226;
    sourceArray1[35] = (byte) 156;
    sourceArray1[3] = (byte) 172;
    sourceArray1[37] = (byte) 96 /*0x60*/;
    sourceArray1[19] = (byte) 124;
    sourceArray1[31 /*0x1F*/] = (byte) 119;
    sourceArray1[40] = (byte) 210;
    sourceArray1[41] = (byte) 11;
    sourceArray1[42] = (byte) 147;
    sourceArray1[43] = (byte) 97;
    sourceArray1[44] = (byte) 36;
    sourceArray1[15] = (byte) 133;
    sourceArray1[25] = (byte) 62;
    sourceArray1[47] = (byte) 165;
    byte[] sourceArray2 = new byte[48 /*0x30*/]
    {
      (byte) 206,
      (byte) 30,
      (byte) 91,
      byte.MaxValue,
      (byte) 89,
      (byte) 139,
      (byte) 86,
      byte.MaxValue,
      (byte) 161,
      (byte) 11,
      (byte) 123,
      (byte) 28,
      (byte) 21,
      (byte) 98,
      (byte) 103,
      (byte) 251,
      (byte) 65,
      (byte) 45,
      (byte) 99,
      (byte) 43,
      (byte) 240 /*0xF0*/,
      (byte) 60,
      (byte) 59,
      (byte) 106,
      (byte) 172,
      (byte) 5,
      (byte) 36,
      (byte) 41,
      (byte) 36,
      (byte) 14,
      (byte) 217,
      (byte) 129,
      (byte) 18,
      (byte) 41,
      (byte) 12,
      (byte) 151,
      (byte) 57,
      (byte) 42,
      (byte) 23,
      (byte) 23,
      (byte) 216,
      (byte) 99,
      (byte) 230,
      (byte) 215,
      byte.MaxValue,
      (byte) 8,
      (byte) 147,
      (byte) 170
    };
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 343, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}
