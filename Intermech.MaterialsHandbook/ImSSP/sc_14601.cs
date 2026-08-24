// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14601
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14601
{
  internal static int ssp_imbase_14602(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[16 /*0x10*/] = (byte) 79;
    sourceArray1[44] = (byte) 234;
    sourceArray1[40] = (byte) 111;
    sourceArray1[12] = (byte) 7;
    sourceArray1[7] = (byte) 1;
    sourceArray1[3] = (byte) 37;
    sourceArray1[6] = (byte) 240 /*0xF0*/;
    sourceArray1[0] = (byte) 113;
    sourceArray1[8] = (byte) 130;
    sourceArray1[9] = (byte) 203;
    sourceArray1[10] = (byte) 121;
    sourceArray1[11] = (byte) 17;
    sourceArray1[22] = (byte) 14;
    sourceArray1[13] = (byte) 190;
    sourceArray1[14] = (byte) 17;
    sourceArray1[33] = (byte) 234;
    sourceArray1[17] = (byte) 40;
    sourceArray1[2] = (byte) 49;
    sourceArray1[4] = (byte) 182;
    sourceArray1[19] = (byte) 175;
    sourceArray1[35] = (byte) 160 /*0xA0*/;
    sourceArray1[20] = (byte) 186;
    sourceArray1[15] = (byte) 207;
    sourceArray1[27] = (byte) 43;
    sourceArray1[39] = (byte) 112 /*0x70*/;
    sourceArray1[25] = (byte) 164;
    sourceArray1[5] = (byte) 215;
    sourceArray1[1] = (byte) 194;
    sourceArray1[28] = (byte) 70;
    sourceArray1[45] = (byte) 40;
    sourceArray1[30] = (byte) 124;
    sourceArray1[31 /*0x1F*/] = (byte) 13;
    sourceArray1[38] = (byte) 157;
    sourceArray1[34] = (byte) 10;
    sourceArray1[43] = (byte) 58;
    sourceArray1[23] = (byte) 169;
    sourceArray1[36] = (byte) 104;
    sourceArray1[29] = (byte) 236;
    sourceArray1[18] = (byte) 231;
    sourceArray1[21] = (byte) 211;
    sourceArray1[26] = (byte) 113;
    sourceArray1[41] = (byte) 128 /*0x80*/;
    sourceArray1[42] = (byte) 239;
    sourceArray1[32 /*0x20*/] = (byte) 142;
    sourceArray1[24] = (byte) 217;
    sourceArray1[37] = (byte) 155;
    sourceArray1[46] = (byte) 52;
    sourceArray1[47] = (byte) 72;
    byte[] sourceArray2 = new byte[48 /*0x30*/];
    sourceArray2[27] = (byte) 241;
    sourceArray2[7] = (byte) 172;
    sourceArray2[2] = (byte) 63 /*0x3F*/;
    sourceArray2[3] = (byte) 229;
    sourceArray2[4] = (byte) 31 /*0x1F*/;
    sourceArray2[28] = (byte) 200;
    sourceArray2[6] = (byte) 242;
    sourceArray2[38] = (byte) 155;
    sourceArray2[11] = (byte) 244;
    sourceArray2[12] = (byte) 115;
    sourceArray2[16 /*0x10*/] = (byte) 240 /*0xF0*/;
    sourceArray2[33] = (byte) 27;
    sourceArray2[15] = (byte) 132;
    sourceArray2[1] = (byte) 20;
    sourceArray2[14] = (byte) 154;
    sourceArray2[18] = (byte) 221;
    sourceArray2[43] = (byte) 21;
    sourceArray2[44] = (byte) 220;
    sourceArray2[13] = (byte) 126;
    sourceArray2[19] = (byte) 191;
    sourceArray2[20] = (byte) 138;
    sourceArray2[31 /*0x1F*/] = (byte) 1;
    sourceArray2[47] = (byte) 247;
    sourceArray2[35] = (byte) 214;
    sourceArray2[24] = (byte) 253;
    sourceArray2[25] = (byte) 2;
    sourceArray2[8] = (byte) 71;
    sourceArray2[9] = (byte) 182;
    sourceArray2[21] = (byte) 46;
    sourceArray2[29] = (byte) 187;
    sourceArray2[30] = (byte) 203;
    sourceArray2[5] = (byte) 58;
    sourceArray2[32 /*0x20*/] = (byte) 92;
    sourceArray2[26] = (byte) 90;
    sourceArray2[46] = (byte) 19;
    sourceArray2[34] = (byte) 124;
    sourceArray2[36] = (byte) 95;
    sourceArray2[37] = (byte) 226;
    sourceArray2[17] = (byte) 70;
    sourceArray2[10] = (byte) 38;
    sourceArray2[40] = (byte) 197;
    sourceArray2[41] = (byte) 85;
    sourceArray2[0] = (byte) 142;
    sourceArray2[39] = (byte) 195;
    sourceArray2[42] = (byte) 114;
    sourceArray2[45] = (byte) 7;
    sourceArray2[23] = (byte) 60;
    sourceArray2[22] = (byte) 95;
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 343, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}
