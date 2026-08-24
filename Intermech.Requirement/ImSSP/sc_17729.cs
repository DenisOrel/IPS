// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_17729
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_17729
{
  private static byte[] sspq = new byte[12]
  {
    (byte) 157,
    (byte) 175,
    (byte) 103,
    (byte) 88,
    (byte) 186,
    (byte) 141,
    (byte) 11,
    (byte) 252,
    (byte) 115,
    (byte) 229,
    (byte) 10,
    (byte) 143
  };
  private static byte[] sspr = new byte[12]
  {
    (byte) 249,
    (byte) 214,
    (byte) 167,
    (byte) 110,
    (byte) 246,
    (byte) 196,
    (byte) 119,
    (byte) 140,
    (byte) 233,
    (byte) 19,
    (byte) 174,
    (byte) 243
  };

  internal static string ssp_improject_17730()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 10)
    {
      byte[] numArray1 = new byte[80 /*0x50*/];
      byte[] numArray2 = new byte[55];
      numArray2[15] = (byte) 74;
      numArray2[13] = (byte) 45;
      numArray2[2] = (byte) 231;
      numArray2[3] = (byte) 174;
      numArray2[50] = (byte) 164;
      numArray2[5] = (byte) 6;
      numArray2[6] = (byte) 128 /*0x80*/;
      numArray2[28] = (byte) 167;
      numArray2[0] = (byte) 156;
      numArray2[43] = (byte) 35;
      numArray2[53] = (byte) 198;
      numArray2[25] = (byte) 107;
      numArray2[45] = (byte) 155;
      numArray2[47] = (byte) 214;
      numArray2[14] = (byte) 148;
      numArray2[11] = (byte) 205;
      numArray2[16 /*0x10*/] = (byte) 123;
      numArray2[17] = (byte) 222;
      numArray2[31 /*0x1F*/] = (byte) 226;
      numArray2[19] = (byte) 253;
      numArray2[7] = (byte) 41;
      numArray2[21] = (byte) 214;
      numArray2[44] = (byte) 66;
      numArray2[23] = (byte) 185;
      numArray2[10] = (byte) 6;
      numArray2[35] = (byte) 174;
      numArray2[34] = (byte) 49;
      numArray2[27] = (byte) 51;
      numArray2[4] = (byte) 139;
      numArray2[37] = (byte) 71;
      numArray2[30] = (byte) 193;
      numArray2[20] = (byte) 144 /*0x90*/;
      numArray2[1] = (byte) 92;
      numArray2[33] = (byte) 64 /*0x40*/;
      numArray2[8] = (byte) 62;
      numArray2[24] = (byte) 71;
      numArray2[36] = (byte) 120;
      numArray2[12] = (byte) 10;
      numArray2[38] = (byte) 253;
      numArray2[22] = (byte) 221;
      numArray2[40] = (byte) 73;
      numArray2[41] = (byte) 103;
      numArray2[42] = (byte) 212;
      numArray2[29] = (byte) 126;
      numArray2[48 /*0x30*/] = (byte) 179;
      numArray2[52] = (byte) 11;
      numArray2[46] = (byte) 63 /*0x3F*/;
      numArray2[26] = (byte) 51;
      numArray2[18] = (byte) 217;
      numArray2[49] = (byte) 78;
      numArray2[32 /*0x20*/] = (byte) 35;
      numArray2[39] = (byte) 36;
      numArray2[51] = (byte) 82;
      numArray2[9] = (byte) 125;
      numArray2[54] = (byte) 193;
      byte[] numArray3 = new byte[55];
      numArray3[54] = (byte) 4;
      numArray3[47] = (byte) 6;
      numArray3[2] = (byte) 93;
      numArray3[26] = (byte) 74;
      numArray3[32 /*0x20*/] = (byte) 5;
      numArray3[24] = (byte) 95;
      numArray3[0] = (byte) 41;
      numArray3[18] = (byte) 47;
      numArray3[8] = (byte) 142;
      numArray3[9] = (byte) 96 /*0x60*/;
      numArray3[10] = (byte) 115;
      numArray3[49] = (byte) 187;
      numArray3[11] = byte.MaxValue;
      numArray3[13] = (byte) 61;
      numArray3[16 /*0x10*/] = (byte) 44;
      numArray3[28] = (byte) 70;
      numArray3[4] = (byte) 37;
      numArray3[19] = (byte) 95;
      numArray3[7] = (byte) 126;
      numArray3[27] = (byte) 240 /*0xF0*/;
      numArray3[20] = (byte) 171;
      numArray3[21] = (byte) 228;
      numArray3[35] = (byte) 237;
      numArray3[3] = (byte) 55;
      numArray3[37] = (byte) 16 /*0x10*/;
      numArray3[25] = (byte) 50;
      numArray3[52] = (byte) 121;
      numArray3[41] = (byte) 35;
      numArray3[22] = (byte) 159;
      numArray3[29] = (byte) 18;
      numArray3[30] = (byte) 191;
      numArray3[31 /*0x1F*/] = (byte) 62;
      numArray3[23] = (byte) 221;
      numArray3[5] = (byte) 244;
      numArray3[34] = (byte) 224 /*0xE0*/;
      numArray3[36] = (byte) 0;
      numArray3[33] = (byte) 208 /*0xD0*/;
      numArray3[38] = (byte) 182;
      numArray3[17] = (byte) 140;
      numArray3[39] = (byte) 238;
      numArray3[40] = (byte) 58;
      numArray3[12] = (byte) 38;
      numArray3[42] = (byte) 118;
      numArray3[43] = (byte) 88;
      numArray3[15] = (byte) 236;
      numArray3[45] = (byte) 217;
      numArray3[46] = (byte) 208 /*0xD0*/;
      numArray3[44] = (byte) 246;
      numArray3[48 /*0x30*/] = (byte) 37;
      numArray3[53] = (byte) 156;
      numArray3[50] = (byte) 196;
      numArray3[51] = (byte) 70;
      numArray3[1] = (byte) 5;
      numArray3[14] = (byte) 43;
      numArray3[6] = (byte) 147;
      key.Query(true, 344, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 55);
      for (int index = 0; index < 55; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[25];
      numArray4[5] = (byte) 77;
      numArray4[6] = (byte) 122;
      numArray4[14] = (byte) 45;
      numArray4[3] = (byte) 242;
      numArray4[15] = (byte) 23;
      numArray4[13] = (byte) 244;
      numArray4[22] = (byte) 20;
      numArray4[0] = (byte) 95;
      numArray4[7] = (byte) 78;
      numArray4[9] = (byte) 0;
      numArray4[2] = (byte) 62;
      numArray4[4] = (byte) 26;
      numArray4[12] = (byte) 78;
      numArray4[10] = (byte) 98;
      numArray4[23] = (byte) 137;
      numArray4[8] = (byte) 202;
      numArray4[16 /*0x10*/] = (byte) 133;
      numArray4[17] = (byte) 135;
      numArray4[1] = (byte) 0;
      numArray4[19] = (byte) 190;
      numArray4[11] = (byte) 232;
      numArray4[24] = (byte) 57;
      numArray4[20] = (byte) 53;
      numArray4[18] = (byte) 135;
      numArray4[21] = (byte) 133;
      byte[] numArray5 = new byte[25]
      {
        byte.MaxValue,
        (byte) 128 /*0x80*/,
        (byte) 152,
        (byte) 110,
        (byte) 88,
        (byte) 90,
        (byte) 123,
        (byte) 249,
        (byte) 190,
        byte.MaxValue,
        (byte) 143,
        (byte) 219,
        (byte) 226,
        (byte) 139,
        (byte) 64 /*0x40*/,
        (byte) 239,
        (byte) 114,
        (byte) 147,
        (byte) 244,
        (byte) 6,
        (byte) 6,
        (byte) 103,
        (byte) 41,
        (byte) 58,
        (byte) 21
      };
      key.Query(true, 344, numArray4, numArray4);
      Array.Copy((Array) numArray4, 0, (Array) numArray1, 55, 25);
      for (int index = 0; index < 25; ++index)
        numArray1[index + 55] ^= numArray5[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray6 = new byte[80 /*0x50*/];
    byte[] numArray7 = new byte[55];
    numArray7[26] = (byte) 216;
    numArray7[42] = (byte) 134;
    numArray7[28] = (byte) 229;
    numArray7[52] = (byte) 210;
    numArray7[24] = (byte) 111;
    numArray7[4] = (byte) 180;
    numArray7[6] = (byte) 123;
    numArray7[45] = (byte) 148;
    numArray7[14] = (byte) 90;
    numArray7[9] = (byte) 192 /*0xC0*/;
    numArray7[30] = (byte) 53;
    numArray7[11] = (byte) 16 /*0x10*/;
    numArray7[27] = (byte) 217;
    numArray7[13] = (byte) 21;
    numArray7[46] = (byte) 249;
    numArray7[41] = (byte) 8;
    numArray7[16 /*0x10*/] = (byte) 9;
    numArray7[2] = (byte) 217;
    numArray7[8] = (byte) 46;
    numArray7[10] = (byte) 59;
    numArray7[20] = (byte) 24;
    numArray7[21] = (byte) 9;
    numArray7[12] = (byte) 207;
    numArray7[43] = (byte) 11;
    numArray7[15] = (byte) 193;
    numArray7[53] = (byte) 97;
    numArray7[31 /*0x1F*/] = (byte) 33;
    numArray7[7] = (byte) 102;
    numArray7[1] = (byte) 62;
    numArray7[23] = (byte) 124;
    numArray7[29] = (byte) 92;
    numArray7[3] = (byte) 236;
    numArray7[32 /*0x20*/] = (byte) 27;
    numArray7[34] = (byte) 188;
    numArray7[51] = (byte) 236;
    numArray7[35] = (byte) 182;
    numArray7[36] = (byte) 89;
    numArray7[47] = (byte) 75;
    numArray7[38] = (byte) 248;
    numArray7[39] = (byte) 50;
    numArray7[19] = (byte) 114;
    numArray7[0] = (byte) 22;
    numArray7[22] = (byte) 104;
    numArray7[17] = (byte) 107;
    numArray7[49] = (byte) 63 /*0x3F*/;
    numArray7[44] = (byte) 204;
    numArray7[18] = (byte) 154;
    numArray7[33] = (byte) 197;
    numArray7[48 /*0x30*/] = (byte) 107;
    numArray7[40] = (byte) 154;
    numArray7[50] = (byte) 230;
    numArray7[25] = (byte) 191;
    numArray7[37] = (byte) 56;
    numArray7[5] = (byte) 184;
    numArray7[54] = (byte) 181;
    byte[] numArray8 = new byte[55]
    {
      (byte) 39,
      (byte) 134,
      (byte) 89,
      (byte) 70,
      (byte) 169,
      (byte) 21,
      (byte) 58,
      (byte) 56,
      (byte) 98,
      (byte) 248,
      (byte) 65,
      (byte) 215,
      (byte) 89,
      (byte) 125,
      (byte) 252,
      (byte) 203,
      (byte) 200,
      (byte) 137,
      (byte) 221,
      (byte) 7,
      (byte) 117,
      (byte) 239,
      (byte) 181,
      (byte) 227,
      (byte) 82,
      (byte) 212,
      (byte) 141,
      (byte) 118,
      (byte) 119,
      (byte) 144 /*0x90*/,
      (byte) 48 /*0x30*/,
      (byte) 216,
      (byte) 151,
      (byte) 116,
      (byte) 60,
      (byte) 13,
      (byte) 242,
      (byte) 145,
      (byte) 168,
      (byte) 174,
      (byte) 237,
      (byte) 28,
      (byte) 16 /*0x10*/,
      (byte) 205,
      (byte) 79,
      (byte) 31 /*0x1F*/,
      (byte) 3,
      (byte) 239,
      (byte) 9,
      (byte) 131,
      (byte) 188,
      (byte) 8,
      (byte) 52,
      (byte) 119,
      (byte) 149
    };
    key.Query(true, 344, numArray7, numArray7);
    Array.Copy((Array) numArray7, 0, (Array) numArray6, 0, 55);
    for (int index = 0; index < 55; ++index)
      numArray6[index] ^= numArray8[index];
    byte[] numArray9 = new byte[25]
    {
      (byte) 172,
      (byte) 162,
      (byte) 46,
      (byte) 84,
      (byte) 51,
      (byte) 218,
      (byte) 91,
      (byte) 66,
      (byte) 203,
      (byte) 58,
      (byte) 250,
      (byte) 169,
      (byte) 195,
      (byte) 107,
      (byte) 22,
      (byte) 111,
      (byte) 23,
      (byte) 223,
      (byte) 137,
      (byte) 206,
      (byte) 219,
      (byte) 253,
      (byte) 58,
      (byte) 214,
      (byte) 153
    };
    byte[] numArray10 = new byte[25];
    numArray10[3] = (byte) 217;
    numArray10[1] = (byte) 159;
    numArray10[8] = (byte) 90;
    numArray10[19] = (byte) 84;
    numArray10[22] = (byte) 44;
    numArray10[23] = (byte) 119;
    numArray10[6] = (byte) 192 /*0xC0*/;
    numArray10[7] = (byte) 3;
    numArray10[4] = (byte) 249;
    numArray10[9] = (byte) 197;
    numArray10[10] = (byte) 151;
    numArray10[2] = (byte) 36;
    numArray10[18] = (byte) 218;
    numArray10[13] = (byte) 135;
    numArray10[12] = (byte) 132;
    numArray10[11] = (byte) 91;
    numArray10[5] = (byte) 41;
    numArray10[17] = (byte) 177;
    numArray10[15] = (byte) 136;
    numArray10[0] = (byte) 198;
    numArray10[20] = (byte) 82;
    numArray10[21] = (byte) 159;
    numArray10[16 /*0x10*/] = (byte) 55;
    numArray10[14] = (byte) 235;
    numArray10[24] = (byte) 57;
    key.Query(true, 344, numArray9, numArray9);
    Array.Copy((Array) numArray9, 0, (Array) numArray6, 55, 25);
    for (int index = 0; index < 25; ++index)
      numArray6[index + 55] ^= numArray10[index];
    byte[] numArray11 = new byte[12];
    byte[] response = new byte[12];
    Array.Copy((Array) sc_17729.sspq, 0, (Array) numArray11, 0, 12);
    key.Query(true, 344, numArray11, response);
    Array.Copy((Array) sc_17729.sspr, 0, (Array) numArray11, 0, 12);
    for (int index = 0; index < numArray11.Length; ++index)
    {
      if ((int) numArray11[index] != (int) response[index])
      {
        key.TagValue = (int) response[index];
        break;
      }
    }
    return Encoding.UTF8.GetString(numArray6);
  }
}
