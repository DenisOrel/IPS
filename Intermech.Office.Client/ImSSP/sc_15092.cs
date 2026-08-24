// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_15092
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_15092
{
  private static byte[] sspq = new byte[47]
  {
    (byte) 108,
    (byte) 139,
    (byte) 83,
    (byte) 199,
    (byte) 35,
    (byte) 203,
    (byte) 73,
    (byte) 44,
    (byte) 2,
    (byte) 205,
    (byte) 110,
    (byte) 236,
    (byte) 135,
    (byte) 67,
    (byte) 111,
    (byte) 3,
    (byte) 12,
    (byte) 32 /*0x20*/,
    (byte) 110,
    (byte) 180,
    (byte) 9,
    (byte) 89,
    (byte) 208 /*0xD0*/,
    (byte) 34,
    (byte) 121,
    (byte) 19,
    (byte) 250,
    (byte) 253,
    (byte) 49,
    (byte) 20,
    (byte) 77,
    (byte) 50,
    (byte) 31 /*0x1F*/,
    (byte) 196,
    (byte) 76,
    (byte) 226,
    (byte) 56,
    (byte) 197,
    (byte) 132,
    (byte) 100,
    (byte) 98,
    (byte) 231,
    (byte) 218,
    (byte) 213,
    (byte) 82,
    (byte) 34,
    (byte) 4
  };
  private static byte[] sspr = new byte[47]
  {
    (byte) 48 /*0x30*/,
    (byte) 93,
    (byte) 96 /*0x60*/,
    (byte) 83,
    (byte) 241,
    (byte) 155,
    (byte) 62,
    (byte) 191,
    (byte) 53,
    (byte) 74,
    (byte) 78,
    (byte) 117,
    (byte) 184,
    (byte) 202,
    (byte) 216,
    (byte) 208 /*0xD0*/,
    (byte) 217,
    (byte) 250,
    (byte) 248,
    (byte) 193,
    (byte) 56,
    (byte) 4,
    (byte) 151,
    (byte) 8,
    (byte) 161,
    (byte) 51,
    (byte) 63 /*0x3F*/,
    (byte) 166,
    (byte) 128 /*0x80*/,
    (byte) 210,
    (byte) 168,
    (byte) 119,
    (byte) 132,
    (byte) 83,
    (byte) 35,
    (byte) 13,
    (byte) 99,
    (byte) 49,
    (byte) 69,
    (byte) 119,
    (byte) 12,
    (byte) 44,
    (byte) 153,
    (byte) 19,
    (byte) 199,
    (byte) 184,
    (byte) 130
  };

  internal static string ssp_office_15093()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 5)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 86,
        (byte) 188,
        (byte) 104,
        (byte) 130,
        (byte) 205,
        (byte) 99,
        (byte) 243,
        (byte) 34,
        (byte) 27,
        (byte) 98,
        (byte) 201,
        (byte) 15,
        (byte) 86,
        (byte) 3,
        (byte) 211,
        (byte) 86
      };
      byte[] numArray3 = new byte[16 /*0x10*/];
      numArray3[15] = (byte) 124;
      numArray3[1] = (byte) 81;
      numArray3[2] = (byte) 144 /*0x90*/;
      numArray3[3] = (byte) 165;
      numArray3[4] = (byte) 2;
      numArray3[14] = (byte) 164;
      numArray3[6] = (byte) 202;
      numArray3[0] = (byte) 52;
      numArray3[11] = (byte) 142;
      numArray3[7] = (byte) 9;
      numArray3[8] = (byte) 246;
      numArray3[5] = (byte) 149;
      numArray3[12] = (byte) 174;
      numArray3[13] = (byte) 187;
      numArray3[10] = (byte) 139;
      numArray3[9] = (byte) 238;
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/];
    numArray5[12] = (byte) 19;
    numArray5[2] = (byte) 88;
    numArray5[5] = (byte) 82;
    numArray5[3] = (byte) 239;
    numArray5[4] = (byte) 191;
    numArray5[11] = (byte) 94;
    numArray5[6] = (byte) 88;
    numArray5[7] = (byte) 248;
    numArray5[8] = (byte) 177;
    numArray5[14] = (byte) 92;
    numArray5[13] = (byte) 217;
    numArray5[0] = (byte) 31 /*0x1F*/;
    numArray5[15] = (byte) 99;
    numArray5[1] = (byte) 28;
    numArray5[9] = (byte) 243;
    numArray5[10] = (byte) 80 /*0x50*/;
    byte[] numArray6 = new byte[16 /*0x10*/];
    numArray6[10] = (byte) 144 /*0x90*/;
    numArray6[15] = (byte) 32 /*0x20*/;
    numArray6[9] = (byte) 181;
    numArray6[2] = (byte) 164;
    numArray6[11] = (byte) 128 /*0x80*/;
    numArray6[5] = (byte) 186;
    numArray6[0] = (byte) 161;
    numArray6[7] = (byte) 103;
    numArray6[8] = (byte) 212;
    numArray6[4] = (byte) 161;
    numArray6[6] = (byte) 233;
    numArray6[3] = (byte) 116;
    numArray6[12] = (byte) 210;
    numArray6[1] = (byte) 33;
    numArray6[13] = (byte) 84;
    numArray6[14] = (byte) 253;
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_office_15094()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/];
      numArray2[6] = (byte) 104;
      numArray2[1] = (byte) 92;
      numArray2[14] = (byte) 111;
      numArray2[8] = (byte) 139;
      numArray2[4] = (byte) 140;
      numArray2[3] = (byte) 33;
      numArray2[0] = (byte) 79;
      numArray2[7] = (byte) 24;
      numArray2[10] = (byte) 233;
      numArray2[9] = (byte) 140;
      numArray2[5] = (byte) 90;
      numArray2[11] = (byte) 180;
      numArray2[2] = (byte) 146;
      numArray2[13] = (byte) 225;
      numArray2[12] = (byte) 155;
      numArray2[15] = (byte) 118;
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 60,
        (byte) 25,
        (byte) 98,
        (byte) 118,
        (byte) 131,
        (byte) 0,
        (byte) 115,
        (byte) 183,
        (byte) 4,
        (byte) 154,
        (byte) 194,
        (byte) 127 /*0x7F*/,
        (byte) 122,
        (byte) 109,
        (byte) 64 /*0x40*/,
        (byte) 57
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/]
    {
      (byte) 145,
      (byte) 11,
      (byte) 146,
      (byte) 172,
      (byte) 215,
      (byte) 108,
      (byte) 55,
      (byte) 95,
      (byte) 98,
      (byte) 160 /*0xA0*/,
      (byte) 88,
      (byte) 73,
      (byte) 82,
      (byte) 120,
      (byte) 50,
      (byte) 47
    };
    byte[] numArray6 = new byte[16 /*0x10*/];
    numArray6[4] = (byte) 240 /*0xF0*/;
    numArray6[1] = (byte) 37;
    numArray6[6] = (byte) 228;
    numArray6[3] = (byte) 42;
    numArray6[5] = (byte) 137;
    numArray6[8] = (byte) 15;
    numArray6[12] = (byte) 244;
    numArray6[7] = (byte) 87;
    numArray6[14] = (byte) 188;
    numArray6[9] = (byte) 145;
    numArray6[10] = (byte) 54;
    numArray6[2] = (byte) 76;
    numArray6[0] = (byte) 68;
    numArray6[11] = (byte) 166;
    numArray6[13] = (byte) 226;
    numArray6[15] = (byte) 30;
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_office_15095()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 7)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/];
      numArray2[15] = (byte) 238;
      numArray2[8] = (byte) 153;
      numArray2[6] = (byte) 184;
      numArray2[3] = (byte) 128 /*0x80*/;
      numArray2[4] = (byte) 191;
      numArray2[9] = (byte) 97;
      numArray2[5] = (byte) 6;
      numArray2[1] = (byte) 118;
      numArray2[11] = (byte) 39;
      numArray2[0] = (byte) 145;
      numArray2[10] = (byte) 119;
      numArray2[2] = (byte) 90;
      numArray2[7] = (byte) 168;
      numArray2[12] = (byte) 202;
      numArray2[13] = (byte) 66;
      numArray2[14] = (byte) 252;
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 201,
        (byte) 64 /*0x40*/,
        (byte) 49,
        (byte) 249,
        (byte) 251,
        (byte) 136,
        (byte) 137,
        (byte) 141,
        (byte) 221,
        (byte) 140,
        (byte) 176 /*0xB0*/,
        (byte) 1,
        (byte) 91,
        (byte) 216,
        (byte) 29,
        (byte) 182
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/]
    {
      (byte) 95,
      (byte) 29,
      (byte) 54,
      (byte) 114,
      (byte) 226,
      (byte) 136,
      (byte) 159,
      (byte) 151,
      (byte) 129,
      (byte) 25,
      (byte) 161,
      (byte) 79,
      (byte) 215,
      (byte) 188,
      (byte) 149,
      (byte) 184
    };
    byte[] numArray6 = new byte[16 /*0x10*/];
    numArray6[13] = (byte) 2;
    numArray6[6] = (byte) 101;
    numArray6[4] = (byte) 97;
    numArray6[3] = byte.MaxValue;
    numArray6[10] = (byte) 1;
    numArray6[0] = (byte) 122;
    numArray6[11] = (byte) 198;
    numArray6[7] = (byte) 172;
    numArray6[1] = (byte) 125;
    numArray6[9] = (byte) 40;
    numArray6[5] = (byte) 43;
    numArray6[12] = (byte) 150;
    numArray6[2] = (byte) 246;
    numArray6[8] = (byte) 115;
    numArray6[14] = (byte) 252;
    numArray6[15] = (byte) 138;
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[47];
    byte[] response = new byte[47];
    Array.Copy((Array) sc_15092.sspq, 0, (Array) numArray7, 0, 47);
    key.Query(true, 349, numArray7, response);
    Array.Copy((Array) sc_15092.sspr, 0, (Array) numArray7, 0, 47);
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

  internal static string ssp_office_15096()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 2)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 19,
        (byte) 227,
        (byte) 128 /*0x80*/,
        (byte) 225,
        (byte) 54,
        (byte) 232,
        (byte) 199,
        (byte) 19,
        (byte) 187,
        (byte) 49,
        (byte) 78,
        (byte) 15,
        (byte) 208 /*0xD0*/,
        (byte) 75,
        (byte) 251,
        (byte) 237
      };
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 33,
        (byte) 30,
        (byte) 138,
        (byte) 101,
        (byte) 200,
        (byte) 153,
        (byte) 145,
        (byte) 244,
        (byte) 159,
        (byte) 91,
        (byte) 10,
        (byte) 239,
        (byte) 169,
        (byte) 29,
        (byte) 94,
        (byte) 82
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/];
    numArray5[1] = (byte) 247;
    numArray5[0] = (byte) 194;
    numArray5[12] = (byte) 86;
    numArray5[9] = (byte) 126;
    numArray5[4] = (byte) 70;
    numArray5[3] = (byte) 203;
    numArray5[14] = (byte) 161;
    numArray5[5] = (byte) 155;
    numArray5[8] = (byte) 33;
    numArray5[7] = (byte) 81;
    numArray5[10] = (byte) 251;
    numArray5[6] = (byte) 10;
    numArray5[11] = (byte) 207;
    numArray5[13] = (byte) 14;
    numArray5[2] = (byte) 140;
    numArray5[15] = (byte) 234;
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 56,
      (byte) 135,
      (byte) 244,
      (byte) 181,
      (byte) 126,
      (byte) 64 /*0x40*/,
      (byte) 121,
      (byte) 240 /*0xF0*/,
      (byte) 185,
      (byte) 144 /*0x90*/,
      (byte) 92,
      (byte) 54,
      (byte) 222,
      (byte) 167,
      (byte) 109,
      (byte) 48 /*0x30*/
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
