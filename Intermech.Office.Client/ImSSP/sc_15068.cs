// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_15068
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_15068
{
  private static byte[] sspq = new byte[37]
  {
    (byte) 140,
    (byte) 79,
    (byte) 93,
    (byte) 149,
    (byte) 11,
    (byte) 213,
    (byte) 174,
    (byte) 150,
    (byte) 219,
    (byte) 110,
    (byte) 98,
    (byte) 138,
    (byte) 20,
    (byte) 24,
    (byte) 178,
    (byte) 115,
    (byte) 3,
    (byte) 183,
    (byte) 239,
    (byte) 210,
    (byte) 22,
    (byte) 233,
    (byte) 12,
    (byte) 183,
    (byte) 4,
    (byte) 108,
    (byte) 254,
    (byte) 141,
    (byte) 23,
    (byte) 117,
    (byte) 19,
    (byte) 234,
    (byte) 16 /*0x10*/,
    (byte) 111,
    (byte) 237,
    (byte) 238,
    (byte) 115
  };
  private static byte[] sspr = new byte[37]
  {
    (byte) 109,
    (byte) 23,
    (byte) 48 /*0x30*/,
    (byte) 126,
    (byte) 79,
    (byte) 67,
    (byte) 253,
    (byte) 160 /*0xA0*/,
    (byte) 202,
    (byte) 178,
    (byte) 134,
    (byte) 120,
    (byte) 147,
    (byte) 64 /*0x40*/,
    (byte) 30,
    (byte) 51,
    (byte) 223,
    (byte) 182,
    (byte) 131,
    (byte) 173,
    (byte) 231,
    (byte) 176 /*0xB0*/,
    (byte) 183,
    (byte) 237,
    (byte) 227,
    (byte) 81,
    (byte) 3,
    (byte) 228,
    (byte) 11,
    (byte) 181,
    (byte) 184,
    (byte) 186,
    (byte) 73,
    (byte) 208 /*0xD0*/,
    (byte) 128 /*0x80*/,
    (byte) 54,
    (byte) 121
  };

  internal static string ssp_office_15069()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[90];
      byte[] numArray2 = new byte[55]
      {
        (byte) 54,
        (byte) 16 /*0x10*/,
        (byte) 155,
        (byte) 4,
        (byte) 47,
        (byte) 211,
        (byte) 135,
        (byte) 49,
        (byte) 51,
        (byte) 58,
        (byte) 185,
        (byte) 200,
        (byte) 202,
        (byte) 187,
        (byte) 225,
        (byte) 241,
        (byte) 150,
        (byte) 229,
        (byte) 245,
        (byte) 158,
        (byte) 111,
        (byte) 144 /*0x90*/,
        (byte) 142,
        (byte) 218,
        (byte) 138,
        (byte) 24,
        (byte) 26,
        (byte) 47,
        (byte) 164,
        (byte) 86,
        (byte) 189,
        (byte) 223,
        (byte) 86,
        (byte) 110,
        (byte) 95,
        (byte) 6,
        (byte) 211,
        (byte) 94,
        (byte) 86,
        (byte) 199,
        (byte) 29,
        (byte) 5,
        (byte) 174,
        (byte) 217,
        (byte) 64 /*0x40*/,
        (byte) 234,
        (byte) 144 /*0x90*/,
        (byte) 170,
        (byte) 200,
        (byte) 4,
        (byte) 252,
        (byte) 63 /*0x3F*/,
        (byte) 214,
        (byte) 79,
        (byte) 114
      };
      byte[] numArray3 = new byte[55];
      numArray3[37] = (byte) 18;
      numArray3[52] = (byte) 235;
      numArray3[36] = (byte) 197;
      numArray3[3] = (byte) 56;
      numArray3[4] = (byte) 70;
      numArray3[5] = (byte) 21;
      numArray3[12] = (byte) 120;
      numArray3[27] = (byte) 213;
      numArray3[8] = (byte) 246;
      numArray3[26] = (byte) 2;
      numArray3[48 /*0x30*/] = (byte) 37;
      numArray3[34] = (byte) 62;
      numArray3[10] = (byte) 125;
      numArray3[13] = (byte) 6;
      numArray3[14] = (byte) 37;
      numArray3[15] = (byte) 138;
      numArray3[17] = (byte) 98;
      numArray3[0] = (byte) 252;
      numArray3[53] = (byte) 1;
      numArray3[19] = (byte) 163;
      numArray3[6] = (byte) 81;
      numArray3[21] = (byte) 107;
      numArray3[22] = (byte) 182;
      numArray3[23] = (byte) 82;
      numArray3[25] = (byte) 228;
      numArray3[29] = (byte) 148;
      numArray3[32 /*0x20*/] = (byte) 28;
      numArray3[35] = (byte) 233;
      numArray3[28] = (byte) 22;
      numArray3[33] = (byte) 89;
      numArray3[11] = (byte) 88;
      numArray3[18] = (byte) 163;
      numArray3[24] = (byte) 80 /*0x50*/;
      numArray3[54] = (byte) 38;
      numArray3[40] = (byte) 43;
      numArray3[38] = (byte) 131;
      numArray3[41] = (byte) 241;
      numArray3[31 /*0x1F*/] = (byte) 222;
      numArray3[2] = (byte) 13;
      numArray3[39] = (byte) 229;
      numArray3[49] = (byte) 158;
      numArray3[16 /*0x10*/] = (byte) 74;
      numArray3[42] = (byte) 139;
      numArray3[45] = (byte) 234;
      numArray3[50] = (byte) 18;
      numArray3[44] = (byte) 227;
      numArray3[46] = (byte) 228;
      numArray3[47] = (byte) 143;
      numArray3[20] = (byte) 98;
      numArray3[1] = (byte) 44;
      numArray3[30] = (byte) 168;
      numArray3[51] = (byte) 84;
      numArray3[43] = (byte) 231;
      numArray3[9] = (byte) 185;
      numArray3[7] = (byte) 56;
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 55);
      for (int index = 0; index < 55; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[35]
      {
        (byte) 38,
        (byte) 217,
        (byte) 142,
        (byte) 72,
        (byte) 240 /*0xF0*/,
        (byte) 236,
        (byte) 130,
        byte.MaxValue,
        (byte) 209,
        (byte) 108,
        (byte) 96 /*0x60*/,
        (byte) 160 /*0xA0*/,
        (byte) 86,
        (byte) 10,
        (byte) 84,
        (byte) 78,
        (byte) 30,
        (byte) 186,
        (byte) 240 /*0xF0*/,
        (byte) 16 /*0x10*/,
        (byte) 89,
        (byte) 51,
        (byte) 40,
        (byte) 82,
        (byte) 33,
        (byte) 121,
        (byte) 185,
        (byte) 150,
        (byte) 204,
        (byte) 95,
        (byte) 55,
        (byte) 242,
        (byte) 247,
        (byte) 183,
        (byte) 236
      };
      byte[] numArray5 = new byte[35]
      {
        (byte) 242,
        (byte) 3,
        (byte) 69,
        (byte) 243,
        (byte) 250,
        (byte) 129,
        (byte) 150,
        (byte) 239,
        (byte) 172,
        (byte) 46,
        (byte) 159,
        (byte) 183,
        (byte) 60,
        (byte) 8,
        (byte) 178,
        (byte) 63 /*0x3F*/,
        (byte) 212,
        (byte) 151,
        (byte) 40,
        (byte) 215,
        (byte) 237,
        (byte) 152,
        (byte) 242,
        (byte) 165,
        (byte) 253,
        (byte) 200,
        (byte) 103,
        (byte) 59,
        (byte) 90,
        (byte) 2,
        (byte) 202,
        (byte) 102,
        (byte) 83,
        (byte) 68,
        (byte) 131
      };
      key.Query(true, 349, numArray4, numArray4);
      Array.Copy((Array) numArray4, 0, (Array) numArray1, 55, 35);
      for (int index = 0; index < 35; ++index)
        numArray1[index + 55] ^= numArray5[index];
      byte[] numArray6 = new byte[16 /*0x10*/];
      byte[] response = new byte[16 /*0x10*/];
      Array.Copy((Array) sc_15068.sspq, 0, (Array) numArray6, 0, 16 /*0x10*/);
      key.Query(true, 349, numArray6, response);
      Array.Copy((Array) sc_15068.sspr, 0, (Array) numArray6, 0, 16 /*0x10*/);
      for (int index = 0; index < numArray6.Length; ++index)
      {
        if ((int) numArray6[index] != (int) response[index])
        {
          key.TagValue = (int) response[index];
          break;
        }
      }
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray7 = new byte[90];
    byte[] numArray8 = new byte[55]
    {
      (byte) 243,
      (byte) 176 /*0xB0*/,
      (byte) 59,
      (byte) 38,
      (byte) 118,
      (byte) 51,
      (byte) 235,
      (byte) 50,
      (byte) 114,
      (byte) 52,
      (byte) 109,
      (byte) 169,
      (byte) 158,
      (byte) 30,
      (byte) 74,
      (byte) 157,
      (byte) 88,
      (byte) 229,
      (byte) 100,
      (byte) 193,
      (byte) 252,
      (byte) 158,
      (byte) 29,
      (byte) 125,
      (byte) 115,
      (byte) 227,
      (byte) 221,
      (byte) 39,
      (byte) 106,
      (byte) 212,
      (byte) 34,
      (byte) 217,
      (byte) 69,
      (byte) 68,
      (byte) 248,
      (byte) 89,
      (byte) 117,
      (byte) 160 /*0xA0*/,
      (byte) 94,
      (byte) 89,
      (byte) 100,
      (byte) 42,
      (byte) 34,
      (byte) 214,
      (byte) 103,
      (byte) 4,
      (byte) 248,
      (byte) 189,
      (byte) 36,
      (byte) 196,
      (byte) 50,
      (byte) 46,
      (byte) 180,
      (byte) 241,
      (byte) 119
    };
    byte[] numArray9 = new byte[55]
    {
      (byte) 34,
      (byte) 27,
      (byte) 215,
      (byte) 64 /*0x40*/,
      (byte) 60,
      (byte) 115,
      (byte) 159,
      (byte) 90,
      (byte) 174,
      (byte) 220,
      (byte) 68,
      (byte) 232,
      (byte) 109,
      (byte) 120,
      (byte) 17,
      (byte) 134,
      (byte) 240 /*0xF0*/,
      (byte) 84,
      (byte) 95,
      (byte) 54,
      (byte) 149,
      (byte) 8,
      (byte) 23,
      (byte) 60,
      (byte) 241,
      (byte) 12,
      (byte) 33,
      (byte) 155,
      (byte) 122,
      (byte) 11,
      (byte) 75,
      (byte) 183,
      (byte) 25,
      (byte) 19,
      (byte) 104,
      (byte) 39,
      (byte) 211,
      (byte) 252,
      (byte) 54,
      (byte) 88,
      (byte) 220,
      (byte) 185,
      (byte) 183,
      (byte) 27,
      (byte) 167,
      (byte) 91,
      (byte) 99,
      (byte) 82,
      (byte) 88,
      (byte) 70,
      (byte) 147,
      (byte) 199,
      (byte) 243,
      (byte) 59,
      (byte) 213
    };
    key.Query(true, 349, numArray8, numArray8);
    Array.Copy((Array) numArray8, 0, (Array) numArray7, 0, 55);
    for (int index = 0; index < 55; ++index)
      numArray7[index] ^= numArray9[index];
    byte[] numArray10 = new byte[35]
    {
      (byte) 190,
      (byte) 179,
      (byte) 147,
      (byte) 85,
      (byte) 195,
      (byte) 200,
      (byte) 157,
      (byte) 17,
      (byte) 147,
      (byte) 100,
      (byte) 89,
      (byte) 37,
      (byte) 241,
      (byte) 40,
      (byte) 183,
      (byte) 135,
      (byte) 10,
      (byte) 93,
      (byte) 209,
      (byte) 123,
      (byte) 70,
      (byte) 22,
      (byte) 110,
      (byte) 235,
      (byte) 32 /*0x20*/,
      (byte) 3,
      (byte) 165,
      (byte) 162,
      (byte) 244,
      (byte) 37,
      (byte) 83,
      (byte) 205,
      (byte) 163,
      (byte) 197,
      (byte) 183
    };
    byte[] numArray11 = new byte[35]
    {
      (byte) 58,
      (byte) 97,
      (byte) 132,
      (byte) 103,
      (byte) 112 /*0x70*/,
      (byte) 94,
      (byte) 118,
      (byte) 3,
      (byte) 158,
      (byte) 166,
      (byte) 216,
      (byte) 48 /*0x30*/,
      (byte) 18,
      (byte) 35,
      (byte) 196,
      (byte) 108,
      (byte) 226,
      (byte) 97,
      (byte) 50,
      (byte) 121,
      (byte) 55,
      (byte) 249,
      (byte) 59,
      (byte) 62,
      (byte) 241,
      (byte) 67,
      (byte) 25,
      (byte) 247,
      (byte) 34,
      (byte) 185,
      (byte) 69,
      (byte) 142,
      (byte) 24,
      (byte) 202,
      (byte) 68
    };
    key.Query(true, 349, numArray10, numArray10);
    Array.Copy((Array) numArray10, 0, (Array) numArray7, 55, 35);
    for (int index = 0; index < 35; ++index)
      numArray7[index + 55] ^= numArray11[index];
    return Encoding.UTF8.GetString(numArray7);
  }

  internal static string ssp_office_15070()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 9)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/];
      numArray2[10] = (byte) 167;
      numArray2[12] = (byte) 147;
      numArray2[0] = (byte) 97;
      numArray2[2] = (byte) 196;
      numArray2[3] = (byte) 211;
      numArray2[5] = (byte) 158;
      numArray2[11] = (byte) 169;
      numArray2[7] = (byte) 50;
      numArray2[4] = (byte) 160 /*0xA0*/;
      numArray2[9] = (byte) 122;
      numArray2[1] = (byte) 132;
      numArray2[8] = (byte) 156;
      numArray2[6] = (byte) 151;
      numArray2[13] = (byte) 165;
      numArray2[14] = (byte) 217;
      numArray2[15] = (byte) 76;
      byte[] numArray3 = new byte[16 /*0x10*/];
      numArray3[13] = (byte) 221;
      numArray3[1] = (byte) 14;
      numArray3[0] = (byte) 22;
      numArray3[10] = (byte) 84;
      numArray3[3] = (byte) 148;
      numArray3[12] = (byte) 30;
      numArray3[6] = (byte) 82;
      numArray3[7] = (byte) 46;
      numArray3[8] = (byte) 73;
      numArray3[9] = (byte) 102;
      numArray3[15] = (byte) 35;
      numArray3[11] = (byte) 76;
      numArray3[4] = (byte) 138;
      numArray3[2] = (byte) 74;
      numArray3[14] = (byte) 18;
      numArray3[5] = (byte) 109;
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/]
    {
      (byte) 168,
      (byte) 34,
      (byte) 206,
      (byte) 224 /*0xE0*/,
      (byte) 62,
      (byte) 137,
      (byte) 167,
      (byte) 100,
      (byte) 143,
      (byte) 17,
      (byte) 20,
      (byte) 7,
      (byte) 43,
      (byte) 175,
      (byte) 235,
      (byte) 162
    };
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 32 /*0x20*/,
      (byte) 243,
      (byte) 44,
      (byte) 176 /*0xB0*/,
      (byte) 82,
      (byte) 150,
      (byte) 45,
      (byte) 92,
      (byte) 126,
      (byte) 54,
      (byte) 131,
      (byte) 225,
      (byte) 203,
      (byte) 77,
      (byte) 81,
      (byte) 151
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_office_15071()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 4)
    {
      byte[] numArray1 = new byte[15];
      byte[] numArray2 = new byte[15]
      {
        (byte) 101,
        (byte) 118,
        (byte) 91,
        (byte) 171,
        (byte) 238,
        (byte) 26,
        (byte) 211,
        (byte) 208 /*0xD0*/,
        (byte) 243,
        (byte) 80 /*0x50*/,
        (byte) 100,
        (byte) 184,
        (byte) 178,
        (byte) 97,
        (byte) 33
      };
      byte[] numArray3 = new byte[15];
      numArray3[8] = (byte) 72;
      numArray3[9] = (byte) 217;
      numArray3[12] = (byte) 15;
      numArray3[3] = (byte) 106;
      numArray3[7] = (byte) 147;
      numArray3[4] = (byte) 178;
      numArray3[6] = (byte) 233;
      numArray3[11] = (byte) 79;
      numArray3[1] = (byte) 200;
      numArray3[0] = (byte) 50;
      numArray3[10] = (byte) 142;
      numArray3[14] = (byte) 217;
      numArray3[5] = (byte) 88;
      numArray3[13] = (byte) 8;
      numArray3[2] = (byte) 133;
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 15);
      for (int index = 0; index < 15; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[15];
    byte[] numArray5 = new byte[15];
    numArray5[8] = (byte) 214;
    numArray5[7] = (byte) 182;
    numArray5[4] = (byte) 34;
    numArray5[2] = (byte) 50;
    numArray5[1] = (byte) 169;
    numArray5[13] = (byte) 84;
    numArray5[6] = (byte) 36;
    numArray5[0] = (byte) 174;
    numArray5[11] = (byte) 173;
    numArray5[10] = (byte) 63 /*0x3F*/;
    numArray5[5] = (byte) 242;
    numArray5[3] = (byte) 199;
    numArray5[12] = (byte) 92;
    numArray5[9] = (byte) 113;
    numArray5[14] = (byte) 201;
    byte[] numArray6 = new byte[15]
    {
      (byte) 151,
      (byte) 39,
      (byte) 48 /*0x30*/,
      (byte) 110,
      (byte) 136,
      (byte) 16 /*0x10*/,
      (byte) 119,
      (byte) 156,
      (byte) 210,
      (byte) 187,
      (byte) 9,
      (byte) 129,
      (byte) 233,
      (byte) 90,
      (byte) 164
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 15);
    for (int index = 0; index < 15; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[21];
    byte[] response = new byte[21];
    Array.Copy((Array) sc_15068.sspq, 16 /*0x10*/, (Array) numArray7, 0, 21);
    key.Query(true, 349, numArray7, response);
    Array.Copy((Array) sc_15068.sspr, 16 /*0x10*/, (Array) numArray7, 0, 21);
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

  internal static string ssp_office_15072()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 34,
        (byte) 250,
        (byte) 166,
        (byte) 234,
        (byte) 205,
        (byte) 231,
        (byte) 91,
        (byte) 104,
        (byte) 88,
        (byte) 216,
        (byte) 59,
        (byte) 140,
        (byte) 80 /*0x50*/,
        (byte) 17,
        (byte) 158,
        (byte) 127 /*0x7F*/
      };
      byte[] numArray3 = new byte[16 /*0x10*/];
      numArray3[6] = (byte) 52;
      numArray3[1] = (byte) 44;
      numArray3[2] = (byte) 9;
      numArray3[3] = (byte) 104;
      numArray3[4] = (byte) 233;
      numArray3[11] = (byte) 162;
      numArray3[8] = (byte) 220;
      numArray3[9] = (byte) 44;
      numArray3[5] = (byte) 125;
      numArray3[15] = (byte) 54;
      numArray3[10] = (byte) 157;
      numArray3[12] = (byte) 139;
      numArray3[7] = (byte) 226;
      numArray3[13] = (byte) 164;
      numArray3[14] = (byte) 21;
      numArray3[0] = (byte) 178;
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/];
    numArray5[14] = (byte) 145;
    numArray5[2] = (byte) 43;
    numArray5[0] = (byte) 244;
    numArray5[13] = (byte) 75;
    numArray5[4] = (byte) 36;
    numArray5[9] = (byte) 82;
    numArray5[6] = (byte) 156;
    numArray5[12] = (byte) 2;
    numArray5[5] = (byte) 92;
    numArray5[11] = (byte) 37;
    numArray5[10] = (byte) 139;
    numArray5[3] = (byte) 28;
    numArray5[7] = (byte) 20;
    numArray5[1] = (byte) 44;
    numArray5[8] = (byte) 37;
    numArray5[15] = (byte) 28;
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 100,
      (byte) 29,
      (byte) 4,
      (byte) 202,
      (byte) 12,
      (byte) 191,
      (byte) 176 /*0xB0*/,
      (byte) 183,
      (byte) 71,
      (byte) 74,
      (byte) 250,
      (byte) 222,
      (byte) 65,
      (byte) 113,
      (byte) 136,
      (byte) 195
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
