// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14675
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_14675
{
  internal static string ssp_mentor_14676()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[78];
      byte[] numArray2 = new byte[55]
      {
        (byte) 68,
        (byte) 194,
        (byte) 122,
        (byte) 187,
        (byte) 195,
        (byte) 246,
        (byte) 16 /*0x10*/,
        (byte) 212,
        (byte) 55,
        (byte) 215,
        (byte) 113,
        (byte) 25,
        (byte) 251,
        (byte) 213,
        (byte) 125,
        (byte) 99,
        (byte) 252,
        (byte) 162,
        (byte) 54,
        byte.MaxValue,
        (byte) 153,
        (byte) 31 /*0x1F*/,
        (byte) 212,
        (byte) 187,
        (byte) 59,
        (byte) 128 /*0x80*/,
        (byte) 70,
        (byte) 53,
        (byte) 217,
        (byte) 176 /*0xB0*/,
        (byte) 161,
        (byte) 13,
        (byte) 26,
        (byte) 51,
        (byte) 216,
        (byte) 103,
        (byte) 187,
        (byte) 33,
        (byte) 53,
        (byte) 113,
        (byte) 188,
        (byte) 206,
        (byte) 1,
        (byte) 165,
        (byte) 253,
        (byte) 186,
        (byte) 252,
        (byte) 153,
        (byte) 218,
        (byte) 194,
        (byte) 252,
        (byte) 205,
        (byte) 190,
        (byte) 149,
        (byte) 78
      };
      byte[] numArray3 = new byte[55];
      numArray3[52] = (byte) 254;
      numArray3[1] = (byte) 0;
      numArray3[38] = (byte) 174;
      numArray3[33] = (byte) 88;
      numArray3[8] = (byte) 184;
      numArray3[5] = (byte) 49;
      numArray3[6] = (byte) 82;
      numArray3[7] = (byte) 196;
      numArray3[20] = (byte) 29;
      numArray3[10] = (byte) 223;
      numArray3[18] = (byte) 71;
      numArray3[42] = (byte) 106;
      numArray3[21] = (byte) 201;
      numArray3[13] = (byte) 66;
      numArray3[14] = (byte) 65;
      numArray3[15] = (byte) 182;
      numArray3[16 /*0x10*/] = (byte) 17;
      numArray3[37] = (byte) 150;
      numArray3[44] = (byte) 180;
      numArray3[11] = (byte) 55;
      numArray3[43] = (byte) 25;
      numArray3[32 /*0x20*/] = (byte) 106;
      numArray3[4] = (byte) 190;
      numArray3[40] = (byte) 48 /*0x30*/;
      numArray3[24] = (byte) 144 /*0x90*/;
      numArray3[25] = (byte) 141;
      numArray3[41] = (byte) 120;
      numArray3[27] = (byte) 85;
      numArray3[45] = (byte) 166;
      numArray3[54] = (byte) 227;
      numArray3[30] = (byte) 31 /*0x1F*/;
      numArray3[31 /*0x1F*/] = (byte) 126;
      numArray3[26] = (byte) 17;
      numArray3[36] = (byte) 139;
      numArray3[34] = (byte) 240 /*0xF0*/;
      numArray3[35] = (byte) 217;
      numArray3[19] = (byte) 0;
      numArray3[53] = (byte) 14;
      numArray3[17] = (byte) 35;
      numArray3[23] = (byte) 172;
      numArray3[47] = (byte) 5;
      numArray3[51] = (byte) 69;
      numArray3[39] = (byte) 121;
      numArray3[50] = (byte) 36;
      numArray3[29] = (byte) 147;
      numArray3[46] = (byte) 122;
      numArray3[3] = (byte) 226;
      numArray3[9] = (byte) 159;
      numArray3[48 /*0x30*/] = byte.MaxValue;
      numArray3[49] = (byte) 250;
      numArray3[22] = (byte) 29;
      numArray3[2] = (byte) 167;
      numArray3[0] = (byte) 41;
      numArray3[28] = (byte) 247;
      numArray3[12] = (byte) 106;
      key.Query(true, 346, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 55);
      for (int index = 0; index < 55; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[23]
      {
        byte.MaxValue,
        (byte) 254,
        (byte) 191,
        (byte) 4,
        (byte) 128 /*0x80*/,
        (byte) 227,
        (byte) 210,
        (byte) 97,
        (byte) 21,
        (byte) 121,
        (byte) 226,
        (byte) 72,
        (byte) 156,
        (byte) 170,
        (byte) 58,
        (byte) 123,
        (byte) 3,
        (byte) 97,
        (byte) 246,
        (byte) 162,
        (byte) 190,
        (byte) 37,
        (byte) 197
      };
      byte[] numArray5 = new byte[23];
      numArray5[17] = (byte) 6;
      numArray5[12] = (byte) 242;
      numArray5[6] = (byte) 88;
      numArray5[3] = (byte) 248;
      numArray5[10] = (byte) 167;
      numArray5[1] = (byte) 173;
      numArray5[22] = (byte) 181;
      numArray5[7] = (byte) 91;
      numArray5[8] = (byte) 51;
      numArray5[9] = (byte) 27;
      numArray5[4] = (byte) 13;
      numArray5[11] = (byte) 207;
      numArray5[16 /*0x10*/] = (byte) 235;
      numArray5[13] = (byte) 157;
      numArray5[2] = (byte) 162;
      numArray5[19] = (byte) 247;
      numArray5[15] = (byte) 36;
      numArray5[5] = (byte) 212;
      numArray5[18] = (byte) 111;
      numArray5[21] = (byte) 200;
      numArray5[20] = (byte) 132;
      numArray5[0] = (byte) 3;
      numArray5[14] = (byte) 244;
      key.Query(true, 346, numArray4, numArray4);
      Array.Copy((Array) numArray4, 0, (Array) numArray1, 55, 23);
      for (int index = 0; index < 23; ++index)
        numArray1[index + 55] ^= numArray5[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray6 = new byte[78];
    byte[] numArray7 = new byte[55]
    {
      (byte) 229,
      (byte) 245,
      (byte) 241,
      (byte) 140,
      (byte) 100,
      (byte) 1,
      (byte) 218,
      (byte) 57,
      (byte) 112 /*0x70*/,
      (byte) 244,
      (byte) 235,
      (byte) 230,
      (byte) 64 /*0x40*/,
      (byte) 238,
      (byte) 191,
      (byte) 70,
      (byte) 128 /*0x80*/,
      (byte) 156,
      (byte) 227,
      (byte) 173,
      (byte) 152,
      (byte) 73,
      (byte) 71,
      (byte) 0,
      (byte) 6,
      (byte) 250,
      (byte) 176 /*0xB0*/,
      (byte) 161,
      (byte) 130,
      (byte) 253,
      (byte) 187,
      (byte) 109,
      (byte) 141,
      (byte) 172,
      (byte) 141,
      (byte) 96 /*0x60*/,
      (byte) 64 /*0x40*/,
      (byte) 212,
      (byte) 71,
      (byte) 183,
      (byte) 35,
      (byte) 104,
      (byte) 193,
      (byte) 200,
      (byte) 117,
      (byte) 9,
      (byte) 104,
      (byte) 32 /*0x20*/,
      (byte) 196,
      (byte) 251,
      (byte) 209,
      (byte) 183,
      (byte) 68,
      (byte) 194,
      (byte) 78
    };
    byte[] numArray8 = new byte[55]
    {
      (byte) 155,
      (byte) 35,
      (byte) 199,
      (byte) 81,
      (byte) 33,
      (byte) 226,
      (byte) 52,
      (byte) 194,
      (byte) 90,
      (byte) 186,
      (byte) 14,
      (byte) 222,
      (byte) 22,
      (byte) 232,
      (byte) 211,
      (byte) 167,
      (byte) 69,
      (byte) 234,
      (byte) 135,
      (byte) 4,
      (byte) 94,
      (byte) 22,
      (byte) 228,
      (byte) 228,
      byte.MaxValue,
      (byte) 244,
      (byte) 168,
      (byte) 6,
      (byte) 61,
      (byte) 231,
      (byte) 186,
      (byte) 79,
      (byte) 62,
      (byte) 59,
      (byte) 101,
      (byte) 125,
      (byte) 191,
      (byte) 145,
      (byte) 34,
      (byte) 13,
      (byte) 118,
      (byte) 190,
      (byte) 203,
      (byte) 142,
      (byte) 230,
      (byte) 109,
      (byte) 223,
      (byte) 165,
      (byte) 241,
      (byte) 18,
      (byte) 200,
      (byte) 203,
      (byte) 193,
      (byte) 116,
      (byte) 116
    };
    key.Query(true, 346, numArray7, numArray7);
    Array.Copy((Array) numArray7, 0, (Array) numArray6, 0, 55);
    for (int index = 0; index < 55; ++index)
      numArray6[index] ^= numArray8[index];
    byte[] numArray9 = new byte[23]
    {
      (byte) 241,
      (byte) 134,
      (byte) 106,
      (byte) 81,
      (byte) 56,
      (byte) 183,
      (byte) 114,
      (byte) 56,
      (byte) 125,
      (byte) 89,
      (byte) 148,
      (byte) 97,
      (byte) 95,
      (byte) 6,
      (byte) 171,
      (byte) 84,
      (byte) 44,
      (byte) 138,
      (byte) 48 /*0x30*/,
      (byte) 136,
      (byte) 126,
      (byte) 139,
      (byte) 253
    };
    byte[] numArray10 = new byte[23];
    numArray10[12] = (byte) 213;
    numArray10[1] = (byte) 162;
    numArray10[2] = (byte) 34;
    numArray10[3] = (byte) 241;
    numArray10[4] = (byte) 138;
    numArray10[0] = (byte) 227;
    numArray10[6] = (byte) 86;
    numArray10[7] = (byte) 81;
    numArray10[8] = (byte) 215;
    numArray10[19] = (byte) 204;
    numArray10[10] = (byte) 130;
    numArray10[20] = (byte) 158;
    numArray10[11] = (byte) 20;
    numArray10[9] = (byte) 113;
    numArray10[21] = (byte) 40;
    numArray10[5] = (byte) 225;
    numArray10[14] = (byte) 224 /*0xE0*/;
    numArray10[15] = (byte) 40;
    numArray10[18] = (byte) 78;
    numArray10[13] = (byte) 203;
    numArray10[16 /*0x10*/] = (byte) 69;
    numArray10[17] = (byte) 218;
    numArray10[22] = (byte) 85;
    key.Query(true, 346, numArray9, numArray9);
    Array.Copy((Array) numArray9, 0, (Array) numArray6, 55, 23);
    for (int index = 0; index < 23; ++index)
      numArray6[index + 55] ^= numArray10[index];
    return Encoding.UTF8.GetString(numArray6);
  }
}
