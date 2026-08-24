// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14480
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_14480
{
  private static byte[] sspq = new byte[54]
  {
    (byte) 253,
    (byte) 55,
    (byte) 222,
    (byte) 104,
    (byte) 186,
    (byte) 96 /*0x60*/,
    (byte) 60,
    (byte) 178,
    (byte) 253,
    (byte) 228,
    (byte) 110,
    (byte) 109,
    (byte) 83,
    (byte) 118,
    (byte) 242,
    (byte) 98,
    (byte) 120,
    (byte) 253,
    (byte) 153,
    (byte) 36,
    (byte) 218,
    (byte) 205,
    (byte) 158,
    (byte) 229,
    (byte) 79,
    (byte) 95,
    (byte) 124,
    (byte) 96 /*0x60*/,
    (byte) 99,
    (byte) 197,
    (byte) 197,
    (byte) 156,
    (byte) 135,
    (byte) 54,
    (byte) 247,
    (byte) 206,
    (byte) 120,
    (byte) 84,
    (byte) 136,
    (byte) 181,
    (byte) 64 /*0x40*/,
    (byte) 111,
    (byte) 18,
    (byte) 24,
    (byte) 39,
    (byte) 91,
    (byte) 229,
    (byte) 172,
    (byte) 204,
    (byte) 97,
    (byte) 67,
    (byte) 89,
    (byte) 38,
    (byte) 254
  };
  private static byte[] sspr = new byte[54]
  {
    (byte) 20,
    (byte) 114,
    (byte) 221,
    (byte) 236,
    (byte) 216,
    (byte) 203,
    (byte) 160 /*0xA0*/,
    (byte) 161,
    (byte) 242,
    (byte) 183,
    (byte) 107,
    (byte) 27,
    (byte) 240 /*0xF0*/,
    (byte) 92,
    (byte) 80 /*0x50*/,
    (byte) 201,
    (byte) 119,
    (byte) 126,
    (byte) 141,
    (byte) 195,
    (byte) 153,
    (byte) 30,
    (byte) 108,
    (byte) 209,
    (byte) 46,
    (byte) 76,
    (byte) 154,
    (byte) 202,
    (byte) 252,
    (byte) 23,
    (byte) 144 /*0x90*/,
    (byte) 33,
    (byte) 64 /*0x40*/,
    (byte) 196,
    (byte) 3,
    (byte) 236,
    (byte) 17,
    (byte) 140,
    (byte) 206,
    (byte) 225,
    (byte) 64 /*0x40*/,
    (byte) 49,
    (byte) 251,
    (byte) 38,
    (byte) 83,
    (byte) 248,
    (byte) 207,
    (byte) 118,
    (byte) 185,
    (byte) 129,
    (byte) 172,
    (byte) 91,
    (byte) 62,
    (byte) 239
  };

  internal static int ssp_imbase_14481(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray1 = new byte[4];
    byte[] response1 = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/]
    {
      (byte) 129,
      (byte) 82,
      (byte) 0,
      (byte) 90,
      (byte) 218,
      (byte) 126,
      (byte) 0,
      (byte) 202,
      (byte) 23,
      (byte) 195,
      (byte) 107,
      (byte) 248,
      (byte) 185,
      (byte) 53,
      (byte) 64 /*0x40*/,
      (byte) 175,
      (byte) 161,
      (byte) 252,
      (byte) 221,
      (byte) 154,
      (byte) 122,
      (byte) 25,
      (byte) 152,
      (byte) 159,
      (byte) 30,
      (byte) 39,
      (byte) 128 /*0x80*/,
      (byte) 28,
      (byte) 92,
      (byte) 170,
      (byte) 112 /*0x70*/,
      (byte) 245,
      (byte) 6,
      (byte) 86,
      (byte) 188,
      (byte) 66,
      (byte) 22,
      (byte) 125,
      (byte) 33,
      (byte) 15,
      (byte) 252,
      (byte) 149,
      (byte) 201,
      (byte) 56,
      (byte) 34,
      (byte) 190,
      (byte) 196,
      (byte) 187
    };
    byte[] sourceArray2 = new byte[48 /*0x30*/]
    {
      (byte) 227,
      (byte) 49,
      (byte) 130,
      (byte) 75,
      (byte) 4,
      (byte) 51,
      (byte) 141,
      (byte) 245,
      (byte) 150,
      (byte) 25,
      (byte) 252,
      (byte) 85,
      (byte) 201,
      (byte) 48 /*0x30*/,
      (byte) 101,
      (byte) 180,
      (byte) 218,
      (byte) 229,
      (byte) 72,
      (byte) 212,
      (byte) 163,
      (byte) 8,
      (byte) 24,
      (byte) 46,
      (byte) 176 /*0xB0*/,
      (byte) 140,
      (byte) 236,
      (byte) 40,
      (byte) 219,
      (byte) 115,
      (byte) 52,
      (byte) 129,
      (byte) 66,
      (byte) 197,
      (byte) 111,
      (byte) 50,
      (byte) 87,
      (byte) 252,
      (byte) 149,
      (byte) 74,
      (byte) 97,
      (byte) 138,
      (byte) 114,
      (byte) 84,
      (byte) 247,
      (byte) 126,
      (byte) 228,
      (byte) 52
    };
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray1, 0, 4);
    key.Query(true, 343, numArray1, response1);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray1, 0, 4);
    byte[] numArray2 = new byte[54];
    byte[] response2 = new byte[54];
    Array.Copy((Array) sc_14480.sspq, 0, (Array) numArray2, 0, 54);
    key.Query(true, 343, numArray2, response2);
    Array.Copy((Array) sc_14480.sspr, 0, (Array) numArray2, 0, 54);
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

  internal static int ssp_imbase_14482(int k)
  {
    IProtectionKey key = ProtectionService.Key;
    byte[] numArray = new byte[4];
    byte[] response = new byte[4];
    byte[] sourceArray1 = new byte[48 /*0x30*/];
    sourceArray1[12] = (byte) 126;
    sourceArray1[17] = (byte) 227;
    sourceArray1[2] = (byte) 24;
    sourceArray1[46] = (byte) 45;
    sourceArray1[22] = (byte) 189;
    sourceArray1[5] = (byte) 220;
    sourceArray1[6] = (byte) 78;
    sourceArray1[32 /*0x20*/] = (byte) 215;
    sourceArray1[8] = (byte) 30;
    sourceArray1[23] = (byte) 244;
    sourceArray1[10] = (byte) 230;
    sourceArray1[4] = (byte) 227;
    sourceArray1[0] = (byte) 153;
    sourceArray1[13] = (byte) 35;
    sourceArray1[18] = (byte) 227;
    sourceArray1[47] = (byte) 168;
    sourceArray1[16 /*0x10*/] = (byte) 117;
    sourceArray1[11] = (byte) 28;
    sourceArray1[15] = (byte) 35;
    sourceArray1[3] = (byte) 90;
    sourceArray1[20] = (byte) 187;
    sourceArray1[21] = (byte) 133;
    sourceArray1[42] = (byte) 249;
    sourceArray1[24] = (byte) 121;
    sourceArray1[38] = (byte) 210;
    sourceArray1[14] = (byte) 234;
    sourceArray1[26] = (byte) 147;
    sourceArray1[36] = (byte) 60;
    sourceArray1[9] = (byte) 209;
    sourceArray1[29] = (byte) 107;
    sourceArray1[30] = (byte) 117;
    sourceArray1[31 /*0x1F*/] = (byte) 136;
    sourceArray1[44] = (byte) 117;
    sourceArray1[33] = (byte) 199;
    sourceArray1[39] = (byte) 100;
    sourceArray1[43] = (byte) 85;
    sourceArray1[34] = (byte) 53;
    sourceArray1[37] = (byte) 45;
    sourceArray1[25] = (byte) 208 /*0xD0*/;
    sourceArray1[19] = (byte) 107;
    sourceArray1[40] = (byte) 32 /*0x20*/;
    sourceArray1[41] = (byte) 217;
    sourceArray1[35] = (byte) 127 /*0x7F*/;
    sourceArray1[28] = (byte) 129;
    sourceArray1[27] = (byte) 2;
    sourceArray1[45] = (byte) 201;
    sourceArray1[1] = (byte) 248;
    sourceArray1[7] = (byte) 56;
    byte[] sourceArray2 = new byte[48 /*0x30*/];
    sourceArray2[0] = (byte) 221;
    sourceArray2[14] = byte.MaxValue;
    sourceArray2[2] = (byte) 129;
    sourceArray2[42] = (byte) 252;
    sourceArray2[15] = (byte) 218;
    sourceArray2[39] = (byte) 153;
    sourceArray2[46] = (byte) 231;
    sourceArray2[7] = (byte) 93;
    sourceArray2[4] = (byte) 16 /*0x10*/;
    sourceArray2[9] = (byte) 207;
    sourceArray2[10] = (byte) 120;
    sourceArray2[20] = (byte) 23;
    sourceArray2[12] = (byte) 139;
    sourceArray2[13] = (byte) 173;
    sourceArray2[18] = (byte) 240 /*0xF0*/;
    sourceArray2[37] = (byte) 69;
    sourceArray2[16 /*0x10*/] = (byte) 235;
    sourceArray2[17] = (byte) 247;
    sourceArray2[28] = (byte) 44;
    sourceArray2[19] = (byte) 228;
    sourceArray2[36] = (byte) 141;
    sourceArray2[45] = (byte) 106;
    sourceArray2[22] = (byte) 47;
    sourceArray2[44] = (byte) 133;
    sourceArray2[24] = (byte) 150;
    sourceArray2[25] = (byte) 14;
    sourceArray2[26] = (byte) 2;
    sourceArray2[33] = (byte) 57;
    sourceArray2[6] = (byte) 129;
    sourceArray2[11] = (byte) 23;
    sourceArray2[30] = (byte) 123;
    sourceArray2[41] = (byte) 146;
    sourceArray2[5] = (byte) 1;
    sourceArray2[3] = (byte) 0;
    sourceArray2[34] = (byte) 107;
    sourceArray2[8] = (byte) 105;
    sourceArray2[32 /*0x20*/] = (byte) 188;
    sourceArray2[23] = (byte) 51;
    sourceArray2[38] = (byte) 125;
    sourceArray2[1] = (byte) 184;
    sourceArray2[31 /*0x1F*/] = (byte) 66;
    sourceArray2[27] = (byte) 230;
    sourceArray2[29] = (byte) 212;
    sourceArray2[43] = (byte) 228;
    sourceArray2[35] = (byte) 162;
    sourceArray2[21] = (byte) 181;
    sourceArray2[47] = (byte) 234;
    sourceArray2[40] = (byte) 119;
    Array.Copy((Array) sourceArray1, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    key.Query(true, 343, numArray, response);
    Array.Copy((Array) sourceArray2, (DateTime.Now.Month - 1) * 4, (Array) numArray, 0, 4);
    return BitConverter.ToInt32(response, 0) ^ BitConverter.ToInt32(numArray, 0) ^ k;
  }

  internal static string ssp_imbase_14483()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[34];
      byte[] numArray2 = new byte[34];
      numArray2[3] = (byte) 206;
      numArray2[0] = (byte) 78;
      numArray2[1] = (byte) 191;
      numArray2[14] = (byte) 58;
      numArray2[4] = (byte) 26;
      numArray2[15] = (byte) 53;
      numArray2[6] = (byte) 39;
      numArray2[13] = (byte) 223;
      numArray2[19] = (byte) 62;
      numArray2[5] = (byte) 207;
      numArray2[10] = (byte) 229;
      numArray2[11] = (byte) 105;
      numArray2[17] = (byte) 64 /*0x40*/;
      numArray2[21] = (byte) 68;
      numArray2[9] = (byte) 236;
      numArray2[12] = (byte) 158;
      numArray2[16 /*0x10*/] = (byte) 211;
      numArray2[2] = (byte) 21;
      numArray2[18] = (byte) 100;
      numArray2[22] = (byte) 43;
      numArray2[26] = (byte) 41;
      numArray2[8] = (byte) 20;
      numArray2[20] = (byte) 3;
      numArray2[23] = (byte) 63 /*0x3F*/;
      numArray2[24] = (byte) 42;
      numArray2[25] = (byte) 62;
      numArray2[7] = (byte) 33;
      numArray2[27] = (byte) 32 /*0x20*/;
      numArray2[28] = (byte) 41;
      numArray2[29] = (byte) 161;
      numArray2[30] = (byte) 50;
      numArray2[31 /*0x1F*/] = (byte) 181;
      numArray2[32 /*0x20*/] = (byte) 92;
      numArray2[33] = (byte) 225;
      byte[] numArray3 = new byte[34];
      numArray3[11] = (byte) 15;
      numArray3[1] = (byte) 61;
      numArray3[2] = (byte) 222;
      numArray3[3] = (byte) 88;
      numArray3[0] = (byte) 116;
      numArray3[23] = (byte) 213;
      numArray3[20] = (byte) 136;
      numArray3[14] = (byte) 76;
      numArray3[8] = (byte) 140;
      numArray3[17] = (byte) 187;
      numArray3[6] = (byte) 245;
      numArray3[10] = (byte) 252;
      numArray3[28] = (byte) 84;
      numArray3[13] = (byte) 104;
      numArray3[29] = (byte) 183;
      numArray3[15] = (byte) 118;
      numArray3[9] = (byte) 220;
      numArray3[4] = (byte) 229;
      numArray3[18] = (byte) 222;
      numArray3[5] = (byte) 66;
      numArray3[24] = (byte) 227;
      numArray3[21] = (byte) 199;
      numArray3[22] = (byte) 61;
      numArray3[30] = (byte) 235;
      numArray3[16 /*0x10*/] = (byte) 121;
      numArray3[25] = (byte) 114;
      numArray3[26] = (byte) 136;
      numArray3[12] = (byte) 201;
      numArray3[7] = (byte) 207;
      numArray3[19] = (byte) 144 /*0x90*/;
      numArray3[33] = (byte) 182;
      numArray3[27] = (byte) 145;
      numArray3[32 /*0x20*/] = (byte) 106;
      numArray3[31 /*0x1F*/] = (byte) 69;
      key.Query(true, 343, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 34);
      for (int index = 0; index < 34; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[34];
    byte[] numArray5 = new byte[34];
    numArray5[16 /*0x10*/] = (byte) 228;
    numArray5[1] = (byte) 70;
    numArray5[33] = (byte) 248;
    numArray5[3] = (byte) 51;
    numArray5[13] = (byte) 192 /*0xC0*/;
    numArray5[30] = (byte) 93;
    numArray5[9] = (byte) 236;
    numArray5[28] = (byte) 57;
    numArray5[8] = (byte) 57;
    numArray5[2] = (byte) 71;
    numArray5[10] = (byte) 158;
    numArray5[19] = (byte) 136;
    numArray5[12] = (byte) 208 /*0xD0*/;
    numArray5[6] = (byte) 172;
    numArray5[14] = (byte) 226;
    numArray5[5] = (byte) 221;
    numArray5[23] = (byte) 203;
    numArray5[20] = (byte) 74;
    numArray5[18] = (byte) 226;
    numArray5[7] = (byte) 48 /*0x30*/;
    numArray5[17] = (byte) 243;
    numArray5[25] = (byte) 227;
    numArray5[22] = (byte) 195;
    numArray5[29] = (byte) 183;
    numArray5[24] = (byte) 199;
    numArray5[4] = (byte) 32 /*0x20*/;
    numArray5[26] = (byte) 136;
    numArray5[0] = (byte) 10;
    numArray5[32 /*0x20*/] = (byte) 141;
    numArray5[11] = (byte) 123;
    numArray5[27] = (byte) 92;
    numArray5[31 /*0x1F*/] = (byte) 193;
    numArray5[21] = (byte) 151;
    numArray5[15] = (byte) 134;
    byte[] numArray6 = new byte[34]
    {
      (byte) 84,
      (byte) 101,
      (byte) 216,
      (byte) 126,
      (byte) 248,
      (byte) 1,
      (byte) 155,
      (byte) 89,
      (byte) 239,
      (byte) 207,
      (byte) 155,
      (byte) 61,
      (byte) 226,
      (byte) 158,
      (byte) 154,
      (byte) 95,
      (byte) 117,
      (byte) 206,
      (byte) 131,
      (byte) 82,
      (byte) 254,
      (byte) 68,
      (byte) 198,
      (byte) 53,
      (byte) 126,
      (byte) 194,
      (byte) 45,
      (byte) 98,
      (byte) 180,
      (byte) 75,
      (byte) 181,
      (byte) 188,
      (byte) 210,
      (byte) 244
    };
    key.Query(true, 343, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 34);
    for (int index = 0; index < 34; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
