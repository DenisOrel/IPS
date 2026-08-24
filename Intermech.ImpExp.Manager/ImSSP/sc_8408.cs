// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_8408
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_8408
{
  private static byte[] sspq = new byte[32 /*0x20*/]
  {
    (byte) 134,
    (byte) 135,
    (byte) 142,
    (byte) 59,
    (byte) 3,
    (byte) 5,
    (byte) 165,
    (byte) 62,
    (byte) 239,
    (byte) 211,
    (byte) 253,
    (byte) 219,
    (byte) 113,
    (byte) 233,
    (byte) 17,
    (byte) 241,
    (byte) 196,
    (byte) 109,
    (byte) 136,
    (byte) 195,
    (byte) 94,
    (byte) 175,
    (byte) 171,
    (byte) 194,
    (byte) 181,
    (byte) 132,
    (byte) 235,
    (byte) 52,
    (byte) 233,
    (byte) 51,
    (byte) 39,
    (byte) 141
  };
  private static byte[] sspr = new byte[32 /*0x20*/]
  {
    (byte) 136,
    (byte) 84,
    (byte) 201,
    (byte) 172,
    (byte) 211,
    (byte) 141,
    (byte) 41,
    (byte) 43,
    (byte) 41,
    (byte) 175,
    (byte) 37,
    (byte) 239,
    (byte) 252,
    (byte) 200,
    (byte) 58,
    (byte) 131,
    (byte) 171,
    (byte) 61,
    (byte) 162,
    (byte) 27,
    (byte) 106,
    (byte) 148,
    (byte) 186,
    (byte) 180,
    (byte) 52,
    (byte) 146,
    (byte) 21,
    (byte) 104,
    (byte) 171,
    (byte) 34,
    (byte) 172,
    (byte) 87
  };

  internal static string ssp_imclient_8409()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 11)
    {
      byte[] numArray1 = new byte[27];
      byte[] numArray2 = new byte[27]
      {
        (byte) 214,
        (byte) 143,
        (byte) 120,
        (byte) 36,
        (byte) 169,
        (byte) 238,
        (byte) 65,
        (byte) 222,
        (byte) 182,
        (byte) 183,
        (byte) 49,
        (byte) 237,
        (byte) 101,
        (byte) 141,
        (byte) 42,
        (byte) 138,
        (byte) 85,
        (byte) 220,
        (byte) 133,
        (byte) 251,
        (byte) 250,
        (byte) 154,
        (byte) 140,
        (byte) 39,
        (byte) 4,
        (byte) 124,
        (byte) 34
      };
      byte[] numArray3 = new byte[27]
      {
        (byte) 90,
        (byte) 244,
        (byte) 34,
        (byte) 0,
        (byte) 12,
        (byte) 52,
        (byte) 71,
        (byte) 194,
        (byte) 84,
        (byte) 208 /*0xD0*/,
        (byte) 22,
        (byte) 67,
        (byte) 141,
        (byte) 140,
        (byte) 160 /*0xA0*/,
        (byte) 28,
        (byte) 170,
        (byte) 155,
        (byte) 34,
        (byte) 47,
        (byte) 78,
        (byte) 52,
        (byte) 70,
        (byte) 122,
        (byte) 156,
        (byte) 184,
        (byte) 41
      };
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 27);
      for (int index = 0; index < 27; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[27];
    byte[] numArray5 = new byte[27];
    numArray5[8] = (byte) 4;
    numArray5[25] = (byte) 59;
    numArray5[5] = (byte) 211;
    numArray5[3] = (byte) 226;
    numArray5[23] = (byte) 8;
    numArray5[19] = (byte) 155;
    numArray5[21] = (byte) 129;
    numArray5[26] = (byte) 3;
    numArray5[6] = (byte) 97;
    numArray5[9] = (byte) 103;
    numArray5[0] = (byte) 238;
    numArray5[4] = (byte) 253;
    numArray5[12] = (byte) 58;
    numArray5[13] = (byte) 82;
    numArray5[10] = (byte) 65;
    numArray5[15] = (byte) 181;
    numArray5[14] = (byte) 182;
    numArray5[1] = (byte) 151;
    numArray5[17] = (byte) 45;
    numArray5[22] = (byte) 218;
    numArray5[20] = (byte) 252;
    numArray5[2] = (byte) 153;
    numArray5[18] = (byte) 90;
    numArray5[11] = (byte) 231;
    numArray5[24] = (byte) 202;
    numArray5[7] = (byte) 185;
    numArray5[16 /*0x10*/] = (byte) 33;
    byte[] numArray6 = new byte[27];
    numArray6[6] = (byte) 180;
    numArray6[22] = (byte) 237;
    numArray6[2] = (byte) 105;
    numArray6[25] = (byte) 87;
    numArray6[17] = (byte) 122;
    numArray6[3] = (byte) 16 /*0x10*/;
    numArray6[4] = (byte) 87;
    numArray6[23] = (byte) 172;
    numArray6[7] = (byte) 19;
    numArray6[9] = (byte) 27;
    numArray6[1] = (byte) 12;
    numArray6[10] = (byte) 10;
    numArray6[12] = (byte) 124;
    numArray6[8] = (byte) 28;
    numArray6[14] = (byte) 111;
    numArray6[0] = (byte) 248;
    numArray6[16 /*0x10*/] = (byte) 171;
    numArray6[15] = (byte) 73;
    numArray6[18] = (byte) 196;
    numArray6[19] = (byte) 150;
    numArray6[20] = (byte) 126;
    numArray6[21] = (byte) 34;
    numArray6[13] = (byte) 41;
    numArray6[5] = (byte) 41;
    numArray6[24] = (byte) 157;
    numArray6[11] = (byte) 190;
    numArray6[26] = (byte) 247;
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 27);
    for (int index = 0; index < 27; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_8410()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 12)
    {
      byte[] numArray1 = new byte[41];
      byte[] numArray2 = new byte[41]
      {
        (byte) 216,
        (byte) 145,
        (byte) 107,
        (byte) 67,
        (byte) 92,
        (byte) 222,
        (byte) 195,
        (byte) 106,
        (byte) 206,
        (byte) 134,
        (byte) 23,
        (byte) 2,
        byte.MaxValue,
        (byte) 131,
        (byte) 180,
        (byte) 130,
        (byte) 165,
        (byte) 176 /*0xB0*/,
        (byte) 86,
        (byte) 237,
        (byte) 188,
        (byte) 86,
        (byte) 166,
        (byte) 119,
        (byte) 218,
        (byte) 195,
        (byte) 0,
        (byte) 67,
        (byte) 97,
        (byte) 230,
        (byte) 5,
        (byte) 49,
        (byte) 200,
        (byte) 6,
        (byte) 193,
        (byte) 115,
        (byte) 36,
        (byte) 119,
        (byte) 44,
        (byte) 60,
        (byte) 158
      };
      byte[] numArray3 = new byte[41]
      {
        (byte) 177,
        (byte) 55,
        (byte) 151,
        (byte) 135,
        (byte) 171,
        (byte) 75,
        (byte) 156,
        (byte) 220,
        (byte) 116,
        (byte) 103,
        (byte) 114,
        (byte) 36,
        (byte) 172,
        (byte) 253,
        (byte) 33,
        (byte) 26,
        (byte) 105,
        (byte) 65,
        (byte) 228,
        (byte) 114,
        (byte) 97,
        (byte) 173,
        (byte) 166,
        (byte) 164,
        (byte) 21,
        (byte) 96 /*0x60*/,
        (byte) 120,
        (byte) 246,
        (byte) 172,
        (byte) 57,
        (byte) 28,
        (byte) 138,
        (byte) 248,
        (byte) 103,
        (byte) 127 /*0x7F*/,
        (byte) 70,
        (byte) 218,
        (byte) 195,
        (byte) 151,
        (byte) 142,
        (byte) 108
      };
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 41);
      for (int index = 0; index < 41; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[41];
    byte[] numArray5 = new byte[41];
    numArray5[40] = (byte) 176 /*0xB0*/;
    numArray5[17] = (byte) 128 /*0x80*/;
    numArray5[26] = (byte) 93;
    numArray5[2] = (byte) 169;
    numArray5[3] = (byte) 56;
    numArray5[5] = (byte) 52;
    numArray5[6] = (byte) 53;
    numArray5[7] = (byte) 24;
    numArray5[21] = (byte) 229;
    numArray5[34] = (byte) 28;
    numArray5[10] = (byte) 21;
    numArray5[11] = (byte) 130;
    numArray5[37] = (byte) 200;
    numArray5[13] = (byte) 244;
    numArray5[1] = (byte) 94;
    numArray5[9] = (byte) 227;
    numArray5[16 /*0x10*/] = (byte) 221;
    numArray5[25] = (byte) 64 /*0x40*/;
    numArray5[18] = (byte) 9;
    numArray5[19] = (byte) 232;
    numArray5[12] = (byte) 234;
    numArray5[4] = (byte) 153;
    numArray5[22] = (byte) 57;
    numArray5[23] = (byte) 249;
    numArray5[24] = (byte) 194;
    numArray5[0] = (byte) 91;
    numArray5[30] = (byte) 79;
    numArray5[27] = (byte) 46;
    numArray5[28] = (byte) 132;
    numArray5[29] = (byte) 25;
    numArray5[38] = (byte) 169;
    numArray5[31 /*0x1F*/] = (byte) 136;
    numArray5[20] = (byte) 128 /*0x80*/;
    numArray5[33] = (byte) 41;
    numArray5[32 /*0x20*/] = (byte) 135;
    numArray5[35] = (byte) 124;
    numArray5[36] = (byte) 26;
    numArray5[15] = (byte) 152;
    numArray5[14] = (byte) 104;
    numArray5[39] = (byte) 44;
    numArray5[8] = (byte) 136;
    byte[] numArray6 = new byte[41];
    numArray6[36] = (byte) 207;
    numArray6[1] = (byte) 52;
    numArray6[2] = (byte) 104;
    numArray6[17] = (byte) 179;
    numArray6[4] = (byte) 133;
    numArray6[16 /*0x10*/] = (byte) 34;
    numArray6[30] = (byte) 189;
    numArray6[31 /*0x1F*/] = (byte) 119;
    numArray6[8] = (byte) 117;
    numArray6[29] = (byte) 111;
    numArray6[10] = (byte) 17;
    numArray6[11] = (byte) 118;
    numArray6[18] = (byte) 81;
    numArray6[34] = (byte) 51;
    numArray6[14] = (byte) 140;
    numArray6[12] = (byte) 197;
    numArray6[13] = (byte) 103;
    numArray6[23] = (byte) 135;
    numArray6[5] = (byte) 113;
    numArray6[9] = (byte) 97;
    numArray6[26] = (byte) 164;
    numArray6[32 /*0x20*/] = (byte) 14;
    numArray6[22] = (byte) 174;
    numArray6[24] = (byte) 206;
    numArray6[35] = (byte) 46;
    numArray6[21] = (byte) 244;
    numArray6[40] = (byte) 86;
    numArray6[27] = (byte) 249;
    numArray6[25] = (byte) 92;
    numArray6[3] = (byte) 32 /*0x20*/;
    numArray6[0] = (byte) 110;
    numArray6[28] = (byte) 129;
    numArray6[20] = (byte) 50;
    numArray6[33] = (byte) 106;
    numArray6[19] = (byte) 97;
    numArray6[7] = (byte) 200;
    numArray6[37] = (byte) 70;
    numArray6[6] = (byte) 7;
    numArray6[38] = (byte) 5;
    numArray6[39] = (byte) 156;
    numArray6[15] = (byte) 234;
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 41);
    for (int index = 0; index < 41; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[32 /*0x20*/];
    byte[] response = new byte[32 /*0x20*/];
    Array.Copy((Array) sc_8408.sspq, 0, (Array) numArray7, 0, 32 /*0x20*/);
    key.Query(true, 348, numArray7, response);
    Array.Copy((Array) sc_8408.sspr, 0, (Array) numArray7, 0, 32 /*0x20*/);
    for (int index = 0; index < numArray7.Length; ++index)
    {
      if ((int) numArray7[index] != (int) response[index])
      {
        key.TagValue = (int) response[index];
        break;
      }
    }
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_8411()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 5)
    {
      byte[] numArray1 = new byte[33];
      byte[] numArray2 = new byte[33]
      {
        (byte) 133,
        (byte) 193,
        (byte) 197,
        (byte) 32 /*0x20*/,
        (byte) 45,
        (byte) 54,
        (byte) 202,
        (byte) 0,
        (byte) 181,
        (byte) 177,
        (byte) 214,
        (byte) 233,
        (byte) 199,
        (byte) 67,
        (byte) 122,
        (byte) 28,
        (byte) 105,
        (byte) 208 /*0xD0*/,
        (byte) 196,
        (byte) 11,
        (byte) 80 /*0x50*/,
        (byte) 100,
        (byte) 215,
        (byte) 219,
        (byte) 203,
        (byte) 134,
        (byte) 126,
        (byte) 25,
        (byte) 113,
        (byte) 84,
        (byte) 188,
        (byte) 172,
        (byte) 112 /*0x70*/
      };
      byte[] numArray3 = new byte[33]
      {
        (byte) 107,
        (byte) 45,
        (byte) 229,
        (byte) 12,
        (byte) 150,
        (byte) 59,
        (byte) 166,
        (byte) 143,
        (byte) 70,
        (byte) 110,
        (byte) 87,
        (byte) 188,
        (byte) 30,
        (byte) 155,
        (byte) 36,
        (byte) 231,
        (byte) 169,
        (byte) 210,
        (byte) 8,
        (byte) 86,
        (byte) 122,
        (byte) 22,
        (byte) 18,
        (byte) 90,
        (byte) 139,
        (byte) 55,
        (byte) 200,
        (byte) 18,
        (byte) 53,
        (byte) 221,
        (byte) 16 /*0x10*/,
        (byte) 40,
        (byte) 52
      };
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 33);
      for (int index = 0; index < 33; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[33];
    byte[] numArray5 = new byte[33];
    numArray5[21] = (byte) 234;
    numArray5[17] = (byte) 130;
    numArray5[1] = (byte) 208 /*0xD0*/;
    numArray5[3] = (byte) 55;
    numArray5[11] = (byte) 64 /*0x40*/;
    numArray5[29] = (byte) 41;
    numArray5[6] = (byte) 169;
    numArray5[7] = (byte) 253;
    numArray5[8] = (byte) 213;
    numArray5[9] = (byte) 217;
    numArray5[4] = (byte) 190;
    numArray5[12] = (byte) 113;
    numArray5[0] = (byte) 75;
    numArray5[13] = (byte) 23;
    numArray5[5] = (byte) 194;
    numArray5[15] = (byte) 99;
    numArray5[31 /*0x1F*/] = (byte) 237;
    numArray5[22] = (byte) 85;
    numArray5[18] = (byte) 177;
    numArray5[19] = (byte) 33;
    numArray5[25] = (byte) 176 /*0xB0*/;
    numArray5[27] = (byte) 96 /*0x60*/;
    numArray5[24] = (byte) 183;
    numArray5[20] = (byte) 234;
    numArray5[14] = (byte) 128 /*0x80*/;
    numArray5[2] = (byte) 150;
    numArray5[26] = (byte) 143;
    numArray5[10] = (byte) 237;
    numArray5[28] = (byte) 245;
    numArray5[23] = (byte) 167;
    numArray5[30] = (byte) 121;
    numArray5[32 /*0x20*/] = (byte) 88;
    numArray5[16 /*0x10*/] = (byte) 98;
    byte[] numArray6 = new byte[33];
    numArray6[11] = (byte) 134;
    numArray6[8] = (byte) 183;
    numArray6[12] = (byte) 146;
    numArray6[3] = (byte) 248;
    numArray6[2] = (byte) 52;
    numArray6[14] = (byte) 54;
    numArray6[6] = (byte) 9;
    numArray6[28] = (byte) 62;
    numArray6[20] = (byte) 143;
    numArray6[23] = (byte) 77;
    numArray6[5] = (byte) 243;
    numArray6[4] = (byte) 250;
    numArray6[10] = (byte) 113;
    numArray6[30] = (byte) 186;
    numArray6[26] = (byte) 160 /*0xA0*/;
    numArray6[0] = (byte) 2;
    numArray6[16 /*0x10*/] = (byte) 88;
    numArray6[17] = (byte) 115;
    numArray6[9] = (byte) 201;
    numArray6[19] = (byte) 133;
    numArray6[31 /*0x1F*/] = (byte) 252;
    numArray6[7] = (byte) 90;
    numArray6[22] = (byte) 104;
    numArray6[18] = byte.MaxValue;
    numArray6[24] = (byte) 24;
    numArray6[25] = (byte) 250;
    numArray6[27] = (byte) 10;
    numArray6[29] = (byte) 156;
    numArray6[1] = (byte) 19;
    numArray6[21] = (byte) 144 /*0x90*/;
    numArray6[13] = (byte) 72;
    numArray6[15] = (byte) 9;
    numArray6[32 /*0x20*/] = (byte) 126;
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 33);
    for (int index = 0; index < 33; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
