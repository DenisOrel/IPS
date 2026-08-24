// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14745
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_14745
{
  internal static string ssp_mentor_14746()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 9)
    {
      byte[] numArray1 = new byte[98];
      byte[] numArray2 = new byte[55];
      numArray2[44] = (byte) 111;
      numArray2[51] = (byte) 70;
      numArray2[38] = (byte) 198;
      numArray2[3] = (byte) 167;
      numArray2[7] = (byte) 48 /*0x30*/;
      numArray2[49] = (byte) 119;
      numArray2[54] = (byte) 200;
      numArray2[37] = (byte) 121;
      numArray2[8] = (byte) 125;
      numArray2[13] = (byte) 172;
      numArray2[10] = (byte) 137;
      numArray2[26] = (byte) 233;
      numArray2[30] = (byte) 107;
      numArray2[0] = (byte) 198;
      numArray2[24] = (byte) 84;
      numArray2[15] = (byte) 18;
      numArray2[16 /*0x10*/] = (byte) 248;
      numArray2[17] = (byte) 110;
      numArray2[18] = (byte) 35;
      numArray2[22] = (byte) 132;
      numArray2[20] = (byte) 0;
      numArray2[21] = (byte) 226;
      numArray2[43] = (byte) 139;
      numArray2[23] = (byte) 4;
      numArray2[11] = (byte) 159;
      numArray2[25] = (byte) 143;
      numArray2[2] = (byte) 37;
      numArray2[9] = (byte) 50;
      numArray2[28] = (byte) 14;
      numArray2[29] = (byte) 130;
      numArray2[1] = (byte) 97;
      numArray2[35] = (byte) 142;
      numArray2[6] = (byte) 113;
      numArray2[12] = (byte) 167;
      numArray2[34] = (byte) 205;
      numArray2[41] = (byte) 14;
      numArray2[36] = (byte) 5;
      numArray2[31 /*0x1F*/] = (byte) 99;
      numArray2[33] = (byte) 105;
      numArray2[39] = (byte) 50;
      numArray2[40] = (byte) 225;
      numArray2[27] = (byte) 181;
      numArray2[5] = (byte) 141;
      numArray2[47] = (byte) 149;
      numArray2[14] = (byte) 226;
      numArray2[45] = (byte) 2;
      numArray2[46] = (byte) 92;
      numArray2[42] = (byte) 53;
      numArray2[53] = (byte) 146;
      numArray2[32 /*0x20*/] = (byte) 32 /*0x20*/;
      numArray2[50] = (byte) 245;
      numArray2[48 /*0x30*/] = (byte) 52;
      numArray2[52] = (byte) 136;
      numArray2[4] = (byte) 187;
      numArray2[19] = (byte) 77;
      byte[] numArray3 = new byte[55]
      {
        (byte) 42,
        (byte) 12,
        (byte) 215,
        (byte) 114,
        (byte) 234,
        (byte) 105,
        (byte) 159,
        (byte) 70,
        (byte) 167,
        (byte) 56,
        (byte) 101,
        (byte) 201,
        (byte) 254,
        (byte) 168,
        (byte) 95,
        (byte) 226,
        (byte) 223,
        (byte) 221,
        (byte) 155,
        (byte) 233,
        (byte) 32 /*0x20*/,
        (byte) 244,
        (byte) 151,
        (byte) 112 /*0x70*/,
        (byte) 9,
        (byte) 251,
        (byte) 52,
        (byte) 210,
        (byte) 220,
        (byte) 110,
        (byte) 88,
        (byte) 250,
        (byte) 221,
        (byte) 143,
        (byte) 119,
        (byte) 118,
        (byte) 211,
        (byte) 49,
        (byte) 51,
        (byte) 146,
        (byte) 240 /*0xF0*/,
        (byte) 32 /*0x20*/,
        (byte) 53,
        (byte) 136,
        (byte) 49,
        (byte) 88,
        (byte) 90,
        (byte) 68,
        (byte) 226,
        (byte) 115,
        (byte) 72,
        (byte) 92,
        (byte) 170,
        (byte) 64 /*0x40*/,
        (byte) 173
      };
      key.Query(true, 346, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 55);
      for (int index = 0; index < 55; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[43]
      {
        (byte) 139,
        (byte) 0,
        (byte) 249,
        (byte) 80 /*0x50*/,
        (byte) 58,
        (byte) 139,
        (byte) 109,
        (byte) 162,
        (byte) 75,
        (byte) 240 /*0xF0*/,
        (byte) 157,
        (byte) 244,
        (byte) 163,
        (byte) 40,
        (byte) 88,
        (byte) 12,
        (byte) 97,
        (byte) 70,
        (byte) 5,
        (byte) 114,
        (byte) 51,
        (byte) 76,
        (byte) 206,
        (byte) 21,
        (byte) 157,
        (byte) 160 /*0xA0*/,
        (byte) 44,
        (byte) 22,
        (byte) 134,
        (byte) 66,
        (byte) 104,
        (byte) 201,
        (byte) 179,
        (byte) 203,
        (byte) 88,
        (byte) 37,
        (byte) 161,
        (byte) 62,
        (byte) 235,
        (byte) 96 /*0x60*/,
        (byte) 204,
        (byte) 34,
        (byte) 253
      };
      byte[] numArray5 = new byte[43];
      numArray5[37] = (byte) 130;
      numArray5[14] = (byte) 141;
      numArray5[40] = (byte) 152;
      numArray5[5] = (byte) 65;
      numArray5[0] = (byte) 213;
      numArray5[2] = (byte) 17;
      numArray5[27] = (byte) 252;
      numArray5[35] = (byte) 242;
      numArray5[39] = (byte) 174;
      numArray5[22] = (byte) 156;
      numArray5[10] = (byte) 154;
      numArray5[11] = (byte) 252;
      numArray5[12] = (byte) 136;
      numArray5[28] = (byte) 23;
      numArray5[13] = (byte) 134;
      numArray5[15] = (byte) 148;
      numArray5[16 /*0x10*/] = (byte) 168;
      numArray5[17] = (byte) 41;
      numArray5[18] = (byte) 237;
      numArray5[19] = (byte) 17;
      numArray5[20] = (byte) 242;
      numArray5[23] = (byte) 39;
      numArray5[38] = (byte) 239;
      numArray5[3] = (byte) 2;
      numArray5[32 /*0x20*/] = (byte) 231;
      numArray5[25] = (byte) 142;
      numArray5[26] = (byte) 204;
      numArray5[4] = (byte) 150;
      numArray5[24] = (byte) 130;
      numArray5[29] = (byte) 16 /*0x10*/;
      numArray5[41] = (byte) 104;
      numArray5[42] = (byte) 2;
      numArray5[1] = (byte) 208 /*0xD0*/;
      numArray5[33] = (byte) 122;
      numArray5[34] = (byte) 193;
      numArray5[31 /*0x1F*/] = (byte) 2;
      numArray5[36] = (byte) 1;
      numArray5[21] = (byte) 75;
      numArray5[30] = (byte) 210;
      numArray5[8] = (byte) 103;
      numArray5[9] = (byte) 11;
      numArray5[6] = (byte) 239;
      numArray5[7] = (byte) 159;
      key.Query(true, 346, numArray4, numArray4);
      Array.Copy((Array) numArray4, 0, (Array) numArray1, 55, 43);
      for (int index = 0; index < 43; ++index)
        numArray1[index + 55] ^= numArray5[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray6 = new byte[98];
    byte[] numArray7 = new byte[55]
    {
      (byte) 24,
      (byte) 242,
      (byte) 189,
      (byte) 139,
      (byte) 81,
      (byte) 0,
      (byte) 117,
      (byte) 6,
      (byte) 244,
      (byte) 9,
      (byte) 171,
      (byte) 128 /*0x80*/,
      (byte) 100,
      (byte) 112 /*0x70*/,
      (byte) 234,
      (byte) 38,
      (byte) 18,
      (byte) 133,
      (byte) 44,
      (byte) 30,
      (byte) 161,
      (byte) 240 /*0xF0*/,
      (byte) 246,
      (byte) 78,
      (byte) 216,
      (byte) 53,
      (byte) 117,
      (byte) 94,
      (byte) 40,
      (byte) 253,
      (byte) 0,
      (byte) 221,
      (byte) 82,
      (byte) 190,
      (byte) 184,
      (byte) 64 /*0x40*/,
      (byte) 173,
      (byte) 152,
      (byte) 9,
      (byte) 164,
      (byte) 48 /*0x30*/,
      (byte) 9,
      (byte) 90,
      (byte) 50,
      (byte) 234,
      (byte) 90,
      byte.MaxValue,
      (byte) 121,
      (byte) 13,
      (byte) 204,
      (byte) 249,
      (byte) 2,
      (byte) 202,
      (byte) 209,
      (byte) 120
    };
    byte[] numArray8 = new byte[55];
    numArray8[48 /*0x30*/] = (byte) 179;
    numArray8[1] = (byte) 187;
    numArray8[32 /*0x20*/] = (byte) 44;
    numArray8[39] = (byte) 57;
    numArray8[18] = (byte) 34;
    numArray8[5] = (byte) 247;
    numArray8[6] = (byte) 0;
    numArray8[7] = (byte) 214;
    numArray8[15] = (byte) 247;
    numArray8[49] = (byte) 11;
    numArray8[46] = (byte) 44;
    numArray8[11] = (byte) 183;
    numArray8[30] = (byte) 164;
    numArray8[13] = (byte) 15;
    numArray8[47] = (byte) 62;
    numArray8[24] = (byte) 184;
    numArray8[16 /*0x10*/] = (byte) 70;
    numArray8[17] = (byte) 159;
    numArray8[12] = (byte) 43;
    numArray8[19] = (byte) 126;
    numArray8[20] = (byte) 167;
    numArray8[45] = (byte) 129;
    numArray8[38] = (byte) 244;
    numArray8[23] = (byte) 171;
    numArray8[8] = (byte) 6;
    numArray8[3] = (byte) 52;
    numArray8[26] = (byte) 120;
    numArray8[54] = (byte) 20;
    numArray8[31 /*0x1F*/] = (byte) 165;
    numArray8[25] = (byte) 126;
    numArray8[22] = (byte) 186;
    numArray8[44] = (byte) 193;
    numArray8[27] = (byte) 125;
    numArray8[33] = (byte) 171;
    numArray8[4] = (byte) 176 /*0xB0*/;
    numArray8[29] = (byte) 65;
    numArray8[36] = (byte) 106;
    numArray8[28] = (byte) 122;
    numArray8[40] = (byte) 85;
    numArray8[37] = (byte) 241;
    numArray8[0] = (byte) 7;
    numArray8[41] = (byte) 217;
    numArray8[42] = (byte) 62;
    numArray8[43] = (byte) 88;
    numArray8[21] = (byte) 215;
    numArray8[35] = (byte) 222;
    numArray8[50] = (byte) 194;
    numArray8[14] = (byte) 233;
    numArray8[10] = (byte) 151;
    numArray8[34] = (byte) 145;
    numArray8[53] = (byte) 97;
    numArray8[51] = (byte) 117;
    numArray8[52] = (byte) 90;
    numArray8[2] = (byte) 43;
    numArray8[9] = (byte) 91;
    key.Query(true, 346, numArray7, numArray7);
    Array.Copy((Array) numArray7, 0, (Array) numArray6, 0, 55);
    for (int index = 0; index < 55; ++index)
      numArray6[index] ^= numArray8[index];
    byte[] numArray9 = new byte[43];
    numArray9[39] = (byte) 166;
    numArray9[17] = (byte) 211;
    numArray9[0] = (byte) 11;
    numArray9[15] = (byte) 142;
    numArray9[4] = (byte) 197;
    numArray9[25] = (byte) 237;
    numArray9[6] = (byte) 243;
    numArray9[24] = (byte) 58;
    numArray9[26] = (byte) 7;
    numArray9[9] = (byte) 173;
    numArray9[7] = (byte) 10;
    numArray9[11] = (byte) 39;
    numArray9[19] = (byte) 50;
    numArray9[40] = (byte) 111;
    numArray9[14] = (byte) 149;
    numArray9[8] = (byte) 188;
    numArray9[16 /*0x10*/] = (byte) 109;
    numArray9[2] = (byte) 229;
    numArray9[1] = (byte) 53;
    numArray9[21] = (byte) 72;
    numArray9[5] = (byte) 131;
    numArray9[28] = (byte) 119;
    numArray9[34] = (byte) 50;
    numArray9[23] = (byte) 221;
    numArray9[13] = (byte) 60;
    numArray9[18] = (byte) 109;
    numArray9[20] = (byte) 154;
    numArray9[27] = (byte) 132;
    numArray9[30] = (byte) 157;
    numArray9[29] = (byte) 206;
    numArray9[3] = (byte) 143;
    numArray9[31 /*0x1F*/] = (byte) 102;
    numArray9[32 /*0x20*/] = byte.MaxValue;
    numArray9[12] = (byte) 103;
    numArray9[22] = (byte) 82;
    numArray9[35] = (byte) 175;
    numArray9[36] = (byte) 8;
    numArray9[37] = (byte) 138;
    numArray9[38] = (byte) 84;
    numArray9[33] = (byte) 10;
    numArray9[10] = (byte) 96 /*0x60*/;
    numArray9[41] = (byte) 30;
    numArray9[42] = (byte) 95;
    byte[] numArray10 = new byte[43]
    {
      (byte) 41,
      (byte) 247,
      (byte) 70,
      (byte) 136,
      (byte) 46,
      (byte) 55,
      (byte) 166,
      (byte) 150,
      (byte) 163,
      (byte) 124,
      (byte) 41,
      (byte) 183,
      (byte) 207,
      (byte) 223,
      (byte) 149,
      (byte) 244,
      (byte) 56,
      (byte) 104,
      (byte) 2,
      (byte) 209,
      (byte) 235,
      (byte) 237,
      (byte) 227,
      (byte) 131,
      (byte) 70,
      (byte) 204,
      (byte) 155,
      (byte) 179,
      (byte) 33,
      (byte) 61,
      (byte) 18,
      (byte) 192 /*0xC0*/,
      (byte) 9,
      (byte) 63 /*0x3F*/,
      (byte) 233,
      byte.MaxValue,
      (byte) 252,
      (byte) 220,
      (byte) 112 /*0x70*/,
      (byte) 39,
      (byte) 56,
      (byte) 117,
      (byte) 29
    };
    key.Query(true, 346, numArray9, numArray9);
    Array.Copy((Array) numArray9, 0, (Array) numArray6, 55, 43);
    for (int index = 0; index < 43; ++index)
      numArray6[index + 55] ^= numArray10[index];
    return Encoding.UTF8.GetString(numArray6);
  }
}
