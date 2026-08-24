// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_15080
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_15080
{
  private static byte[] sspq = new byte[27]
  {
    (byte) 12,
    (byte) 190,
    (byte) 178,
    (byte) 213,
    (byte) 37,
    (byte) 228,
    (byte) 92,
    (byte) 75,
    (byte) 5,
    (byte) 153,
    (byte) 251,
    (byte) 147,
    (byte) 242,
    (byte) 215,
    (byte) 178,
    (byte) 69,
    (byte) 180,
    (byte) 62,
    (byte) 75,
    (byte) 115,
    (byte) 42,
    (byte) 199,
    (byte) 71,
    (byte) 197,
    (byte) 215,
    (byte) 24,
    (byte) 206
  };
  private static byte[] sspr = new byte[27]
  {
    (byte) 45,
    (byte) 193,
    (byte) 203,
    (byte) 187,
    (byte) 115,
    (byte) 90,
    (byte) 65,
    (byte) 23,
    (byte) 80 /*0x50*/,
    (byte) 213,
    (byte) 148,
    (byte) 222,
    (byte) 170,
    (byte) 253,
    (byte) 133,
    (byte) 48 /*0x30*/,
    (byte) 150,
    (byte) 149,
    (byte) 33,
    (byte) 229,
    (byte) 93,
    (byte) 154,
    (byte) 28,
    (byte) 216,
    (byte) 188,
    (byte) 178,
    (byte) 214
  };

  internal static string ssp_office_15081()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 39,
        (byte) 245,
        (byte) 210,
        (byte) 96 /*0x60*/,
        (byte) 78,
        (byte) 6,
        (byte) 19,
        (byte) 64 /*0x40*/,
        (byte) 148,
        (byte) 138,
        (byte) 38,
        (byte) 36,
        (byte) 204,
        (byte) 141,
        (byte) 157,
        (byte) 140
      };
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 201,
        (byte) 251,
        (byte) 63 /*0x3F*/,
        (byte) 183,
        (byte) 107,
        (byte) 207,
        (byte) 45,
        (byte) 223,
        (byte) 119,
        (byte) 51,
        (byte) 53,
        (byte) 89,
        (byte) 222,
        (byte) 176 /*0xB0*/,
        (byte) 13,
        (byte) 69
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
      (byte) 51,
      (byte) 24,
      (byte) 96 /*0x60*/,
      (byte) 254,
      (byte) 101,
      (byte) 113,
      (byte) 145,
      (byte) 101,
      (byte) 198,
      (byte) 153,
      (byte) 91,
      (byte) 25,
      (byte) 133,
      (byte) 161,
      (byte) 62,
      (byte) 245
    };
    byte[] numArray6 = new byte[16 /*0x10*/];
    numArray6[14] = (byte) 108;
    numArray6[15] = (byte) 12;
    numArray6[9] = (byte) 60;
    numArray6[3] = (byte) 222;
    numArray6[0] = (byte) 34;
    numArray6[4] = (byte) 49;
    numArray6[6] = (byte) 97;
    numArray6[7] = (byte) 241;
    numArray6[8] = (byte) 103;
    numArray6[12] = (byte) 107;
    numArray6[10] = (byte) 49;
    numArray6[11] = (byte) 197;
    numArray6[2] = (byte) 111;
    numArray6[13] = (byte) 175;
    numArray6[5] = (byte) 163;
    numArray6[1] = (byte) 93;
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_office_15082()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 7)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 199,
        (byte) 246,
        (byte) 108,
        (byte) 238,
        (byte) 189,
        (byte) 161,
        (byte) 77,
        (byte) 167,
        (byte) 57,
        (byte) 113,
        (byte) 160 /*0xA0*/,
        (byte) 130,
        (byte) 141,
        (byte) 151,
        (byte) 183,
        (byte) 103
      };
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 10,
        (byte) 58,
        (byte) 100,
        (byte) 56,
        (byte) 76,
        (byte) 192 /*0xC0*/,
        (byte) 100,
        (byte) 111,
        (byte) 34,
        (byte) 83,
        (byte) 247,
        (byte) 10,
        (byte) 81,
        (byte) 226,
        (byte) 41,
        (byte) 53
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/];
    numArray5[7] = (byte) 68;
    numArray5[1] = (byte) 69;
    numArray5[0] = (byte) 133;
    numArray5[3] = (byte) 107;
    numArray5[13] = (byte) 42;
    numArray5[2] = (byte) 42;
    numArray5[6] = (byte) 83;
    numArray5[9] = (byte) 109;
    numArray5[4] = (byte) 204;
    numArray5[5] = (byte) 171;
    numArray5[10] = (byte) 209;
    numArray5[11] = (byte) 124;
    numArray5[8] = (byte) 4;
    numArray5[12] = (byte) 150;
    numArray5[14] = (byte) 4;
    numArray5[15] = (byte) 46;
    byte[] numArray6 = new byte[16 /*0x10*/];
    numArray6[11] = (byte) 122;
    numArray6[1] = (byte) 53;
    numArray6[3] = (byte) 100;
    numArray6[7] = (byte) 135;
    numArray6[9] = (byte) 107;
    numArray6[5] = (byte) 235;
    numArray6[6] = (byte) 22;
    numArray6[13] = (byte) 242;
    numArray6[15] = (byte) 95;
    numArray6[2] = (byte) 6;
    numArray6[10] = (byte) 14;
    numArray6[14] = (byte) 55;
    numArray6[12] = (byte) 249;
    numArray6[0] = (byte) 200;
    numArray6[8] = (byte) 147;
    numArray6[4] = (byte) 233;
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_office_15083()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/];
      numArray2[11] = (byte) 249;
      numArray2[1] = (byte) 32 /*0x20*/;
      numArray2[10] = (byte) 187;
      numArray2[3] = (byte) 37;
      numArray2[6] = (byte) 73;
      numArray2[12] = (byte) 169;
      numArray2[0] = (byte) 229;
      numArray2[7] = (byte) 212;
      numArray2[8] = (byte) 104;
      numArray2[9] = (byte) 162;
      numArray2[4] = (byte) 26;
      numArray2[14] = (byte) 202;
      numArray2[15] = (byte) 52;
      numArray2[13] = (byte) 149;
      numArray2[2] = (byte) 44;
      numArray2[5] = (byte) 45;
      byte[] numArray3 = new byte[16 /*0x10*/];
      numArray3[13] = (byte) 82;
      numArray3[1] = (byte) 100;
      numArray3[12] = (byte) 103;
      numArray3[15] = (byte) 183;
      numArray3[0] = (byte) 189;
      numArray3[4] = (byte) 50;
      numArray3[2] = (byte) 137;
      numArray3[9] = (byte) 195;
      numArray3[8] = (byte) 196;
      numArray3[3] = (byte) 32 /*0x20*/;
      numArray3[10] = (byte) 27;
      numArray3[14] = (byte) 223;
      numArray3[11] = (byte) 170;
      numArray3[5] = (byte) 249;
      numArray3[7] = (byte) 218;
      numArray3[6] = (byte) 203;
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/]
    {
      (byte) 122,
      (byte) 216,
      (byte) 98,
      (byte) 151,
      (byte) 191,
      (byte) 193,
      (byte) 41,
      (byte) 14,
      (byte) 40,
      (byte) 153,
      (byte) 27,
      (byte) 230,
      (byte) 125,
      (byte) 123,
      (byte) 155,
      (byte) 182
    };
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 191,
      (byte) 146,
      (byte) 92,
      (byte) 28,
      (byte) 118,
      (byte) 23,
      (byte) 154,
      (byte) 174,
      (byte) 41,
      (byte) 42,
      (byte) 113,
      (byte) 178,
      (byte) 37,
      (byte) 89,
      (byte) 46,
      (byte) 0
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_office_15084()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 108,
        (byte) 53,
        (byte) 35,
        (byte) 36,
        (byte) 76,
        (byte) 76,
        (byte) 151,
        (byte) 97,
        (byte) 198,
        (byte) 98,
        (byte) 193,
        (byte) 3,
        (byte) 24,
        (byte) 204,
        (byte) 172,
        (byte) 71
      };
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 217,
        (byte) 213,
        (byte) 246,
        (byte) 67,
        (byte) 156,
        (byte) 7,
        (byte) 11,
        (byte) 85,
        (byte) 31 /*0x1F*/,
        (byte) 5,
        (byte) 193,
        (byte) 155,
        (byte) 52,
        (byte) 17,
        (byte) 245,
        (byte) 227
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[27];
      byte[] response = new byte[27];
      Array.Copy((Array) sc_15080.sspq, 0, (Array) numArray4, 0, 27);
      key.Query(true, 349, numArray4, response);
      Array.Copy((Array) sc_15080.sspr, 0, (Array) numArray4, 0, 27);
      for (int index = 0; index < numArray4.Length; ++index)
      {
        if ((int) numArray4[index] != (int) response[index])
        {
          key.TagValue = (int) response[index];
          break;
        }
      }
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray5 = new byte[16 /*0x10*/];
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 177,
      (byte) 207,
      (byte) 84,
      (byte) 214,
      (byte) 23,
      (byte) 168,
      (byte) 103,
      (byte) 241,
      (byte) 173,
      (byte) 131,
      (byte) 78,
      (byte) 254,
      (byte) 103,
      (byte) 239,
      (byte) 75,
      (byte) 92
    };
    byte[] numArray7 = new byte[16 /*0x10*/]
    {
      (byte) 241,
      (byte) 53,
      (byte) 52,
      (byte) 40,
      (byte) 254,
      (byte) 42,
      (byte) 25,
      (byte) 183,
      (byte) 215,
      (byte) 84,
      (byte) 67,
      (byte) 189,
      (byte) 51,
      (byte) 6,
      (byte) 243,
      (byte) 85
    };
    key.Query(true, 349, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }

  internal static string ssp_office_15085()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 4)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/];
      numArray2[6] = (byte) 35;
      numArray2[1] = (byte) 25;
      numArray2[9] = (byte) 118;
      numArray2[3] = (byte) 146;
      numArray2[4] = (byte) 161;
      numArray2[5] = (byte) 50;
      numArray2[11] = (byte) 254;
      numArray2[7] = (byte) 165;
      numArray2[8] = (byte) 68;
      numArray2[0] = (byte) 64 /*0x40*/;
      numArray2[10] = (byte) 236;
      numArray2[2] = (byte) 103;
      numArray2[12] = (byte) 13;
      numArray2[13] = (byte) 22;
      numArray2[14] = (byte) 171;
      numArray2[15] = (byte) 124;
      byte[] numArray3 = new byte[16 /*0x10*/];
      numArray3[6] = (byte) 142;
      numArray3[4] = (byte) 225;
      numArray3[2] = (byte) 97;
      numArray3[3] = (byte) 166;
      numArray3[7] = (byte) 206;
      numArray3[5] = (byte) 8;
      numArray3[8] = (byte) 151;
      numArray3[10] = (byte) 87;
      numArray3[0] = (byte) 10;
      numArray3[9] = (byte) 177;
      numArray3[13] = (byte) 194;
      numArray3[11] = (byte) 105;
      numArray3[1] = (byte) 98;
      numArray3[14] = (byte) 218;
      numArray3[12] = (byte) 204;
      numArray3[15] = (byte) 11;
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/]
    {
      (byte) 106,
      (byte) 131,
      (byte) 66,
      (byte) 134,
      (byte) 91,
      (byte) 20,
      (byte) 165,
      (byte) 43,
      (byte) 98,
      (byte) 79,
      (byte) 159,
      (byte) 197,
      (byte) 216,
      (byte) 92,
      (byte) 141,
      (byte) 84
    };
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 100,
      (byte) 143,
      (byte) 150,
      (byte) 136,
      (byte) 125,
      (byte) 23,
      (byte) 90,
      (byte) 238,
      (byte) 149,
      (byte) 2,
      (byte) 164,
      (byte) 69,
      (byte) 192 /*0xC0*/,
      (byte) 149,
      (byte) 230,
      (byte) 42
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
