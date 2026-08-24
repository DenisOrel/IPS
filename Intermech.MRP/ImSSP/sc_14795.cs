// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14795
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14795
{
  internal static int ssp_mrp_14796(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/]
    {
      (byte) 197,
      (byte) 131,
      (byte) 167,
      (byte) 63 /*0x3F*/,
      (byte) 61,
      (byte) 33,
      (byte) 134,
      (byte) 58,
      (byte) 106,
      (byte) 222,
      (byte) 38,
      (byte) 157,
      (byte) 179,
      (byte) 122,
      (byte) 17,
      (byte) 18,
      (byte) 84,
      (byte) 161,
      (byte) 29,
      (byte) 226,
      (byte) 217,
      (byte) 177,
      (byte) 237,
      (byte) 105,
      (byte) 45,
      (byte) 82,
      (byte) 0,
      (byte) 131,
      (byte) 209,
      (byte) 161,
      (byte) 50,
      (byte) 176 /*0xB0*/,
      (byte) 136,
      (byte) 201,
      (byte) 15,
      (byte) 239,
      (byte) 72,
      (byte) 58,
      (byte) 31 /*0x1F*/,
      (byte) 134,
      (byte) 186,
      (byte) 86,
      (byte) 160 /*0xA0*/,
      (byte) 194,
      (byte) 64 /*0x40*/,
      (byte) 196,
      (byte) 21,
      (byte) 208 /*0xD0*/
    };
    byte[] sourceArray2 = new byte[48 /*0x30*/];
    sourceArray2[12] = (byte) 210;
    sourceArray2[17] = (byte) 80 /*0x50*/;
    sourceArray2[9] = (byte) 4;
    sourceArray2[39] = (byte) 146;
    sourceArray2[4] = (byte) 151;
    sourceArray2[5] = (byte) 239;
    sourceArray2[6] = (byte) 172;
    sourceArray2[1] = (byte) 221;
    sourceArray2[2] = (byte) 233;
    sourceArray2[37] = (byte) 77;
    sourceArray2[10] = (byte) 157;
    sourceArray2[13] = (byte) 197;
    sourceArray2[33] = (byte) 162;
    sourceArray2[15] = (byte) 218;
    sourceArray2[14] = (byte) 228;
    sourceArray2[11] = (byte) 168;
    sourceArray2[16 /*0x10*/] = (byte) 43;
    sourceArray2[41] = (byte) 46;
    sourceArray2[18] = (byte) 49;
    sourceArray2[31 /*0x1F*/] = (byte) 152;
    sourceArray2[43] = (byte) 251;
    sourceArray2[34] = (byte) 166;
    sourceArray2[26] = (byte) 215;
    sourceArray2[44] = (byte) 80 /*0x50*/;
    sourceArray2[0] = (byte) 247;
    sourceArray2[7] = (byte) 13;
    sourceArray2[23] = (byte) 129;
    sourceArray2[27] = (byte) 120;
    sourceArray2[3] = (byte) 38;
    sourceArray2[29] = (byte) 24;
    sourceArray2[30] = (byte) 51;
    sourceArray2[24] = (byte) 32 /*0x20*/;
    sourceArray2[32 /*0x20*/] = (byte) 104;
    sourceArray2[20] = (byte) 242;
    sourceArray2[21] = (byte) 71;
    sourceArray2[35] = (byte) 53;
    sourceArray2[36] = (byte) 240 /*0xF0*/;
    sourceArray2[42] = (byte) 1;
    sourceArray2[38] = (byte) 181;
    sourceArray2[8] = (byte) 37;
    sourceArray2[40] = (byte) 215;
    sourceArray2[19] = (byte) 168;
    sourceArray2[28] = (byte) 212;
    sourceArray2[46] = (byte) 116;
    sourceArray2[22] = (byte) 142;
    sourceArray2[45] = (byte) 0;
    sourceArray2[25] = (byte) 202;
    sourceArray2[47] = (byte) 128 /*0x80*/;
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 347, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }
}
