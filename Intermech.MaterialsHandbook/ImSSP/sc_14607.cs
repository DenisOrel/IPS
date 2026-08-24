// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14607
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14607
{
  internal static int ssp_imbase_14608(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[1] = (byte) 155;
    sourceArray1[4] = (byte) 176 /*0xB0*/;
    sourceArray1[10] = (byte) 185;
    sourceArray1[8] = (byte) 9;
    sourceArray1[2] = (byte) 40;
    sourceArray1[9] = (byte) 163;
    sourceArray1[29] = (byte) 70;
    sourceArray1[21] = (byte) 150;
    sourceArray1[37] = (byte) 181;
    sourceArray1[12] = (byte) 150;
    sourceArray1[25] = (byte) 243;
    sourceArray1[11] = (byte) 111;
    sourceArray1[35] = (byte) 125;
    sourceArray1[13] = (byte) 75;
    sourceArray1[14] = (byte) 113;
    sourceArray1[15] = (byte) 172;
    sourceArray1[16 /*0x10*/] = (byte) 108;
    sourceArray1[6] = (byte) 252;
    sourceArray1[7] = (byte) 155;
    sourceArray1[19] = (byte) 22;
    sourceArray1[20] = (byte) 62;
    sourceArray1[33] = (byte) 229;
    sourceArray1[18] = (byte) 67;
    sourceArray1[0] = (byte) 37;
    sourceArray1[24] = (byte) 140;
    sourceArray1[3] = (byte) 169;
    sourceArray1[26] = (byte) 187;
    sourceArray1[27] = (byte) 181;
    sourceArray1[5] = (byte) 97;
    sourceArray1[45] = (byte) 145;
    sourceArray1[30] = (byte) 7;
    sourceArray1[31 /*0x1F*/] = (byte) 15;
    sourceArray1[32 /*0x20*/] = (byte) 97;
    sourceArray1[17] = (byte) 6;
    sourceArray1[34] = (byte) 136;
    sourceArray1[40] = (byte) 228;
    sourceArray1[38] = (byte) 103;
    sourceArray1[22] = (byte) 10;
    sourceArray1[39] = (byte) 138;
    sourceArray1[28] = (byte) 114;
    sourceArray1[23] = (byte) 95;
    sourceArray1[41] = (byte) 175;
    sourceArray1[42] = (byte) 240 /*0xF0*/;
    sourceArray1[47] = (byte) 55;
    sourceArray1[44] = (byte) 142;
    sourceArray1[43] = (byte) 157;
    sourceArray1[46] = (byte) 32 /*0x20*/;
    sourceArray1[36] = (byte) 115;
    byte[] sourceArray2 = new byte[48 /*0x30*/]
    {
      (byte) 82,
      (byte) 141,
      (byte) 150,
      byte.MaxValue,
      (byte) 71,
      (byte) 80 /*0x50*/,
      (byte) 146,
      (byte) 35,
      (byte) 58,
      (byte) 165,
      (byte) 38,
      (byte) 185,
      (byte) 93,
      (byte) 165,
      (byte) 42,
      (byte) 179,
      (byte) 43,
      (byte) 44,
      (byte) 250,
      (byte) 180,
      (byte) 89,
      (byte) 207,
      (byte) 100,
      (byte) 25,
      (byte) 193,
      (byte) 231,
      (byte) 179,
      (byte) 17,
      (byte) 160 /*0xA0*/,
      (byte) 43,
      (byte) 187,
      (byte) 192 /*0xC0*/,
      (byte) 45,
      (byte) 201,
      (byte) 30,
      (byte) 10,
      (byte) 182,
      (byte) 125,
      (byte) 195,
      (byte) 210,
      (byte) 79,
      (byte) 233,
      (byte) 25,
      (byte) 1,
      (byte) 232,
      (byte) 88,
      (byte) 226,
      (byte) 245
    };
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 343, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}
