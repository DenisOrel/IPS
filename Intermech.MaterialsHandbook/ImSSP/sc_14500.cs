// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14500
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14500
{
  internal static int ssp_imbase_14501(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[28] = (byte) 109;
    sourceArray1[21] = (byte) 114;
    sourceArray1[2] = (byte) 224 /*0xE0*/;
    sourceArray1[40] = (byte) 126;
    sourceArray1[25] = (byte) 30;
    sourceArray1[27] = (byte) 157;
    sourceArray1[43] = (byte) 203;
    sourceArray1[45] = (byte) 171;
    sourceArray1[31 /*0x1F*/] = (byte) 77;
    sourceArray1[9] = (byte) 196;
    sourceArray1[10] = (byte) 245;
    sourceArray1[11] = (byte) 12;
    sourceArray1[12] = (byte) 121;
    sourceArray1[13] = (byte) 94;
    sourceArray1[14] = (byte) 57;
    sourceArray1[44] = (byte) 57;
    sourceArray1[15] = (byte) 200;
    sourceArray1[16 /*0x10*/] = (byte) 75;
    sourceArray1[18] = (byte) 113;
    sourceArray1[19] = (byte) 210;
    sourceArray1[7] = (byte) 182;
    sourceArray1[38] = (byte) 96 /*0x60*/;
    sourceArray1[6] = (byte) 247;
    sourceArray1[23] = (byte) 77;
    sourceArray1[24] = (byte) 206;
    sourceArray1[17] = (byte) 127 /*0x7F*/;
    sourceArray1[1] = (byte) 209;
    sourceArray1[26] = (byte) 193;
    sourceArray1[5] = (byte) 203;
    sourceArray1[29] = (byte) 190;
    sourceArray1[30] = (byte) 8;
    sourceArray1[0] = (byte) 79;
    sourceArray1[39] = (byte) 152;
    sourceArray1[33] = (byte) 113;
    sourceArray1[34] = (byte) 94;
    sourceArray1[41] = (byte) 163;
    sourceArray1[37] = (byte) 185;
    sourceArray1[36] = (byte) 234;
    sourceArray1[20] = (byte) 211;
    sourceArray1[32 /*0x20*/] = (byte) 82;
    sourceArray1[35] = (byte) 244;
    sourceArray1[46] = (byte) 115;
    sourceArray1[42] = (byte) 0;
    sourceArray1[4] = (byte) 152;
    sourceArray1[3] = (byte) 238;
    sourceArray1[8] = (byte) 68;
    sourceArray1[22] = (byte) 207;
    sourceArray1[47] = (byte) 242;
    byte[] sourceArray2 = new byte[48 /*0x30*/];
    sourceArray2[0] = (byte) 141;
    sourceArray2[43] = (byte) 142;
    sourceArray2[2] = (byte) 140;
    sourceArray2[3] = (byte) 120;
    sourceArray2[12] = (byte) 198;
    sourceArray2[45] = (byte) 71;
    sourceArray2[6] = (byte) 46;
    sourceArray2[7] = (byte) 142;
    sourceArray2[8] = (byte) 177;
    sourceArray2[9] = (byte) 10;
    sourceArray2[10] = (byte) 17;
    sourceArray2[34] = (byte) 4;
    sourceArray2[30] = (byte) 152;
    sourceArray2[15] = (byte) 235;
    sourceArray2[14] = (byte) 217;
    sourceArray2[40] = (byte) 0;
    sourceArray2[16 /*0x10*/] = (byte) 210;
    sourceArray2[17] = (byte) 79;
    sourceArray2[18] = (byte) 3;
    sourceArray2[19] = (byte) 4;
    sourceArray2[26] = (byte) 228;
    sourceArray2[27] = (byte) 150;
    sourceArray2[13] = (byte) 76;
    sourceArray2[23] = (byte) 177;
    sourceArray2[33] = (byte) 232;
    sourceArray2[31 /*0x1F*/] = (byte) 144 /*0x90*/;
    sourceArray2[11] = (byte) 86;
    sourceArray2[20] = (byte) 63 /*0x3F*/;
    sourceArray2[28] = (byte) 236;
    sourceArray2[29] = (byte) 115;
    sourceArray2[22] = (byte) 171;
    sourceArray2[1] = (byte) 50;
    sourceArray2[44] = (byte) 92;
    sourceArray2[21] = (byte) 121;
    sourceArray2[4] = (byte) 209;
    sourceArray2[35] = (byte) 245;
    sourceArray2[24] = (byte) 67;
    sourceArray2[37] = (byte) 36;
    sourceArray2[38] = (byte) 28;
    sourceArray2[39] = (byte) 30;
    sourceArray2[41] = (byte) 247;
    sourceArray2[32 /*0x20*/] = (byte) 52;
    sourceArray2[42] = (byte) 71;
    sourceArray2[5] = (byte) 136;
    sourceArray2[25] = (byte) 4;
    sourceArray2[36] = (byte) 107;
    sourceArray2[46] = (byte) 216;
    sourceArray2[47] = (byte) 233;
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 343, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}
