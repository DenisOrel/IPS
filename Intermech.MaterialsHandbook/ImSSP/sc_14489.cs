// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14489
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14489
{
  internal static int ssp_imbase_14490(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[7] = (byte) 237;
    sourceArray1[1] = (byte) 58;
    sourceArray1[2] = (byte) 166;
    sourceArray1[27] = (byte) 107;
    sourceArray1[32 /*0x20*/] = (byte) 252;
    sourceArray1[5] = (byte) 33;
    sourceArray1[23] = (byte) 143;
    sourceArray1[0] = (byte) 167;
    sourceArray1[3] = (byte) 172;
    sourceArray1[9] = (byte) 85;
    sourceArray1[4] = (byte) 94;
    sourceArray1[46] = (byte) 99;
    sourceArray1[42] = (byte) 122;
    sourceArray1[13] = (byte) 253;
    sourceArray1[41] = (byte) 31 /*0x1F*/;
    sourceArray1[35] = (byte) 252;
    sourceArray1[15] = (byte) 141;
    sourceArray1[12] = (byte) 142;
    sourceArray1[29] = (byte) 169;
    sourceArray1[18] = (byte) 39;
    sourceArray1[16 /*0x10*/] = (byte) 110;
    sourceArray1[21] = (byte) 151;
    sourceArray1[22] = (byte) 37;
    sourceArray1[6] = (byte) 153;
    sourceArray1[24] = (byte) 83;
    sourceArray1[30] = (byte) 111;
    sourceArray1[26] = (byte) 179;
    sourceArray1[19] = (byte) 86;
    sourceArray1[25] = (byte) 74;
    sourceArray1[10] = (byte) 139;
    sourceArray1[11] = (byte) 165;
    sourceArray1[31 /*0x1F*/] = (byte) 28;
    sourceArray1[28] = (byte) 42;
    sourceArray1[33] = (byte) 185;
    sourceArray1[34] = (byte) 112 /*0x70*/;
    sourceArray1[14] = (byte) 93;
    sourceArray1[36] = (byte) 192 /*0xC0*/;
    sourceArray1[44] = (byte) 77;
    sourceArray1[38] = (byte) 68;
    sourceArray1[37] = (byte) 7;
    sourceArray1[20] = (byte) 71;
    sourceArray1[39] = (byte) 115;
    sourceArray1[47] = (byte) 253;
    sourceArray1[43] = (byte) 137;
    sourceArray1[45] = (byte) 67;
    sourceArray1[8] = (byte) 173;
    sourceArray1[17] = (byte) 234;
    sourceArray1[40] = (byte) 94;
    byte[] sourceArray2 = new byte[48 /*0x30*/];
    sourceArray2[20] = (byte) 197;
    sourceArray2[1] = (byte) 214;
    sourceArray2[13] = (byte) 198;
    sourceArray2[46] = (byte) 242;
    sourceArray2[4] = (byte) 231;
    sourceArray2[5] = (byte) 173;
    sourceArray2[38] = (byte) 149;
    sourceArray2[7] = (byte) 16 /*0x10*/;
    sourceArray2[8] = (byte) 29;
    sourceArray2[21] = (byte) 116;
    sourceArray2[19] = (byte) 32 /*0x20*/;
    sourceArray2[33] = (byte) 49;
    sourceArray2[12] = (byte) 231;
    sourceArray2[2] = (byte) 74;
    sourceArray2[42] = (byte) 218;
    sourceArray2[25] = (byte) 191;
    sourceArray2[28] = (byte) 205;
    sourceArray2[17] = (byte) 103;
    sourceArray2[18] = (byte) 170;
    sourceArray2[36] = (byte) 69;
    sourceArray2[27] = (byte) 6;
    sourceArray2[6] = (byte) 233;
    sourceArray2[37] = (byte) 42;
    sourceArray2[23] = (byte) 43;
    sourceArray2[24] = (byte) 146;
    sourceArray2[9] = (byte) 89;
    sourceArray2[26] = (byte) 115;
    sourceArray2[45] = (byte) 87;
    sourceArray2[31 /*0x1F*/] = (byte) 80 /*0x50*/;
    sourceArray2[14] = (byte) 116;
    sourceArray2[30] = (byte) 249;
    sourceArray2[3] = (byte) 201;
    sourceArray2[32 /*0x20*/] = (byte) 119;
    sourceArray2[16 /*0x10*/] = (byte) 228;
    sourceArray2[34] = (byte) 35;
    sourceArray2[22] = (byte) 120;
    sourceArray2[10] = (byte) 35;
    sourceArray2[35] = (byte) 157;
    sourceArray2[15] = (byte) 235;
    sourceArray2[39] = (byte) 55;
    sourceArray2[40] = (byte) 187;
    sourceArray2[41] = (byte) 167;
    sourceArray2[0] = (byte) 46;
    sourceArray2[43] = (byte) 79;
    sourceArray2[44] = (byte) 245;
    sourceArray2[11] = (byte) 92;
    sourceArray2[29] = (byte) 213;
    sourceArray2[47] = (byte) 194;
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 343, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}
