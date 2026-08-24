// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14485
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;

#nullable disable
namespace ImSSP;

internal static class sc_14485
{
  private static byte[] sspq = new byte[12]
  {
    (byte) 13,
    (byte) 40,
    (byte) 223,
    (byte) 216,
    (byte) 232,
    (byte) 241,
    (byte) 250,
    (byte) 251,
    (byte) 113,
    (byte) 154,
    (byte) 126,
    (byte) 230
  };
  private static byte[] sspr = new byte[12]
  {
    (byte) 14,
    (byte) 101,
    (byte) 10,
    (byte) 106,
    (byte) 165,
    (byte) 33,
    (byte) 38,
    (byte) 149,
    (byte) 172,
    (byte) 55,
    (byte) 105,
    (byte) 204
  };

  internal static int ssp_imbase_14486(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray1 = new byte[4];
    byte[] response1 = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[15] = (byte) 20;
    sourceArray1[1] = (byte) 37;
    sourceArray1[2] = (byte) 240 /*0xF0*/;
    sourceArray1[34] = (byte) 157;
    sourceArray1[26] = (byte) 126;
    sourceArray1[29] = (byte) 203;
    sourceArray1[6] = (byte) 11;
    sourceArray1[35] = (byte) 79;
    sourceArray1[5] = (byte) 112 /*0x70*/;
    sourceArray1[9] = (byte) 181;
    sourceArray1[10] = (byte) 83;
    sourceArray1[11] = (byte) 232;
    sourceArray1[32 /*0x20*/] = (byte) 222;
    sourceArray1[13] = (byte) 94;
    sourceArray1[40] = (byte) 96 /*0x60*/;
    sourceArray1[33] = (byte) 83;
    sourceArray1[12] = (byte) 190;
    sourceArray1[8] = (byte) 163;
    sourceArray1[25] = (byte) 211;
    sourceArray1[0] = (byte) 198;
    sourceArray1[43] = (byte) 154;
    sourceArray1[47] = (byte) 126;
    sourceArray1[22] = (byte) 19;
    sourceArray1[20] = (byte) 108;
    sourceArray1[17] = (byte) 14;
    sourceArray1[28] = (byte) 106;
    sourceArray1[4] = (byte) 96 /*0x60*/;
    sourceArray1[44] = (byte) 163;
    sourceArray1[45] = (byte) 153;
    sourceArray1[7] = (byte) 159;
    sourceArray1[30] = (byte) 86;
    sourceArray1[31 /*0x1F*/] = (byte) 191;
    sourceArray1[27] = (byte) 206;
    sourceArray1[36] = (byte) 243;
    sourceArray1[24] = (byte) 143;
    sourceArray1[21] = (byte) 198;
    sourceArray1[41] = (byte) 156;
    sourceArray1[39] = (byte) 11;
    sourceArray1[18] = (byte) 228;
    sourceArray1[3] = (byte) 133;
    sourceArray1[38] = (byte) 34;
    sourceArray1[19] = (byte) 136;
    sourceArray1[42] = (byte) 177;
    sourceArray1[16 /*0x10*/] = (byte) 52;
    sourceArray1[23] = (byte) 98;
    sourceArray1[37] = (byte) 237;
    sourceArray1[46] = (byte) 209;
    sourceArray1[14] = (byte) 24;
    byte[] sourceArray2 = new byte[48 /*0x30*/];
    sourceArray2[29] = (byte) 46;
    sourceArray2[5] = (byte) 203;
    sourceArray2[2] = (byte) 181;
    sourceArray2[36] = (byte) 231;
    sourceArray2[15] = (byte) 107;
    sourceArray2[26] = (byte) 167;
    sourceArray2[24] = (byte) 76;
    sourceArray2[7] = (byte) 62;
    sourceArray2[8] = (byte) 179;
    sourceArray2[34] = (byte) 117;
    sourceArray2[40] = (byte) 228;
    sourceArray2[16 /*0x10*/] = (byte) 190;
    sourceArray2[9] = (byte) 246;
    sourceArray2[13] = (byte) 206;
    sourceArray2[14] = (byte) 110;
    sourceArray2[37] = (byte) 139;
    sourceArray2[0] = (byte) 197;
    sourceArray2[25] = (byte) 77;
    sourceArray2[18] = (byte) 135;
    sourceArray2[19] = (byte) 183;
    sourceArray2[1] = (byte) 175;
    sourceArray2[21] = (byte) 240 /*0xF0*/;
    sourceArray2[22] = (byte) 88;
    sourceArray2[10] = (byte) 38;
    sourceArray2[12] = (byte) 244;
    sourceArray2[42] = (byte) 100;
    sourceArray2[6] = (byte) 60;
    sourceArray2[27] = (byte) 227;
    sourceArray2[28] = (byte) 116;
    sourceArray2[20] = (byte) 88;
    sourceArray2[11] = (byte) 21;
    sourceArray2[23] = (byte) 19;
    sourceArray2[32 /*0x20*/] = (byte) 13;
    sourceArray2[44] = (byte) 147;
    sourceArray2[30] = (byte) 48 /*0x30*/;
    sourceArray2[47] = (byte) 254;
    sourceArray2[33] = (byte) 223;
    sourceArray2[35] = (byte) 186;
    sourceArray2[38] = (byte) 0;
    sourceArray2[3] = (byte) 139;
    sourceArray2[39] = (byte) 243;
    sourceArray2[17] = (byte) 116;
    sourceArray2[31 /*0x1F*/] = (byte) 81;
    sourceArray2[4] = (byte) 173;
    sourceArray2[41] = (byte) 23;
    sourceArray2[45] = (byte) 96 /*0x60*/;
    sourceArray2[46] = (byte) 42;
    sourceArray2[43] = (byte) 51;
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray1, 0, 4);
    key.Query(true, 343, numArray1, response1);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray1, 0, 4);
    byte[] numArray2 = new byte[12];
    byte[] response2 = new byte[12];
    Array.Copy((Array) sc_14485.sspq, 0, (Array) numArray2, 0, 12);
    key.Query(true, 343, numArray2, response2);
    Array.Copy((Array) sc_14485.sspr, 0, (Array) numArray2, 0, 12);
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
