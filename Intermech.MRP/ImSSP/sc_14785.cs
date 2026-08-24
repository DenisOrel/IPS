// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14785
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14785
{
  internal static int ssp_mrp_14786(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[2] = (byte) 244;
    sourceArray1[1] = (byte) 20;
    sourceArray1[0] = (byte) 212;
    sourceArray1[41] = (byte) 114;
    sourceArray1[6] = (byte) 240 /*0xF0*/;
    sourceArray1[44] = (byte) 66;
    sourceArray1[8] = (byte) 23;
    sourceArray1[3] = (byte) 130;
    sourceArray1[16 /*0x10*/] = (byte) 103;
    sourceArray1[9] = (byte) 132;
    sourceArray1[10] = (byte) 175;
    sourceArray1[11] = (byte) 2;
    sourceArray1[34] = (byte) 44;
    sourceArray1[13] = (byte) 193;
    sourceArray1[18] = (byte) 47;
    sourceArray1[32 /*0x20*/] = (byte) 150;
    sourceArray1[39] = (byte) 235;
    sourceArray1[15] = (byte) 119;
    sourceArray1[28] = (byte) 149;
    sourceArray1[19] = (byte) 81;
    sourceArray1[45] = (byte) 142;
    sourceArray1[21] = (byte) 149;
    sourceArray1[22] = (byte) 12;
    sourceArray1[23] = (byte) 7;
    sourceArray1[24] = (byte) 252;
    sourceArray1[36] = (byte) 125;
    sourceArray1[26] = (byte) 5;
    sourceArray1[17] = (byte) 231;
    sourceArray1[38] = (byte) 247;
    sourceArray1[29] = (byte) 191;
    sourceArray1[30] = (byte) 223;
    sourceArray1[35] = (byte) 118;
    sourceArray1[7] = (byte) 155;
    sourceArray1[5] = (byte) 134;
    sourceArray1[42] = (byte) 236;
    sourceArray1[27] = (byte) 72;
    sourceArray1[33] = (byte) 159;
    sourceArray1[25] = (byte) 73;
    sourceArray1[31 /*0x1F*/] = (byte) 128 /*0x80*/;
    sourceArray1[14] = (byte) 240 /*0xF0*/;
    sourceArray1[40] = (byte) 175;
    sourceArray1[4] = (byte) 6;
    sourceArray1[43] = (byte) 237;
    sourceArray1[20] = (byte) 227;
    sourceArray1[37] = (byte) 234;
    sourceArray1[12] = (byte) 156;
    sourceArray1[46] = (byte) 14;
    sourceArray1[47] = (byte) 217;
    byte[] sourceArray2 = new byte[48 /*0x30*/]
    {
      (byte) 83,
      (byte) 185,
      (byte) 44,
      (byte) 248,
      (byte) 125,
      (byte) 124,
      (byte) 205,
      (byte) 89,
      (byte) 139,
      (byte) 39,
      (byte) 9,
      (byte) 145,
      (byte) 155,
      (byte) 135,
      (byte) 138,
      (byte) 19,
      (byte) 243,
      (byte) 6,
      (byte) 232,
      (byte) 234,
      (byte) 216,
      (byte) 42,
      (byte) 75,
      (byte) 21,
      (byte) 132,
      (byte) 84,
      (byte) 214,
      (byte) 206,
      (byte) 120,
      (byte) 214,
      (byte) 174,
      (byte) 136,
      (byte) 68,
      (byte) 187,
      (byte) 108,
      (byte) 209,
      (byte) 104,
      (byte) 138,
      (byte) 111,
      (byte) 114,
      (byte) 223,
      (byte) 175,
      (byte) 191,
      (byte) 86,
      (byte) 59,
      (byte) 88,
      (byte) 251,
      (byte) 109
    };
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 347, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}
