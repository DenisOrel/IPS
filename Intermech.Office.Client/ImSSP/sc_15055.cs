// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_15055
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_15055
{
  private static byte[] sspq = new byte[87]
  {
    (byte) 159,
    (byte) 207,
    (byte) 220,
    (byte) 222,
    (byte) 42,
    (byte) 187,
    (byte) 49,
    (byte) 177,
    (byte) 206,
    (byte) 133,
    (byte) 86,
    (byte) 193,
    (byte) 239,
    (byte) 169,
    (byte) 88,
    (byte) 12,
    (byte) 112 /*0x70*/,
    (byte) 172,
    (byte) 188,
    (byte) 68,
    (byte) 154,
    (byte) 190,
    (byte) 124,
    (byte) 138,
    (byte) 211,
    (byte) 76,
    (byte) 94,
    (byte) 31 /*0x1F*/,
    (byte) 197,
    (byte) 236,
    (byte) 18,
    (byte) 73,
    (byte) 242,
    (byte) 24,
    (byte) 123,
    (byte) 18,
    (byte) 99,
    (byte) 52,
    (byte) 96 /*0x60*/,
    (byte) 179,
    (byte) 87,
    (byte) 222,
    (byte) 76,
    (byte) 12,
    (byte) 186,
    (byte) 141,
    (byte) 182,
    (byte) 126,
    (byte) 109,
    (byte) 27,
    (byte) 99,
    (byte) 147,
    (byte) 134,
    (byte) 152,
    (byte) 17,
    (byte) 202,
    (byte) 237,
    (byte) 27,
    (byte) 179,
    (byte) 107,
    (byte) 52,
    (byte) 120,
    (byte) 84,
    (byte) 171,
    (byte) 113,
    (byte) 214,
    (byte) 159,
    (byte) 167,
    (byte) 35,
    (byte) 28,
    (byte) 156,
    (byte) 127 /*0x7F*/,
    (byte) 221,
    (byte) 11,
    (byte) 185,
    (byte) 75,
    (byte) 53,
    (byte) 43,
    (byte) 80 /*0x50*/,
    (byte) 112 /*0x70*/,
    (byte) 2,
    (byte) 236,
    (byte) 20,
    (byte) 189,
    (byte) 129,
    (byte) 75,
    (byte) 250
  };
  private static byte[] sspr = new byte[87]
  {
    (byte) 214,
    (byte) 123,
    (byte) 191,
    (byte) 120,
    (byte) 92,
    (byte) 176 /*0xB0*/,
    (byte) 72,
    (byte) 216,
    (byte) 215,
    (byte) 55,
    (byte) 61,
    (byte) 49,
    (byte) 137,
    (byte) 75,
    (byte) 195,
    (byte) 209,
    (byte) 40,
    (byte) 121,
    (byte) 81,
    (byte) 91,
    (byte) 69,
    (byte) 244,
    (byte) 215,
    (byte) 131,
    (byte) 160 /*0xA0*/,
    (byte) 3,
    (byte) 132,
    (byte) 23,
    (byte) 65,
    (byte) 253,
    (byte) 32 /*0x20*/,
    (byte) 203,
    (byte) 169,
    (byte) 164,
    (byte) 2,
    (byte) 178,
    (byte) 81,
    (byte) 40,
    (byte) 49,
    (byte) 30,
    (byte) 198,
    (byte) 0,
    (byte) 167,
    (byte) 44,
    (byte) 164,
    (byte) 181,
    (byte) 153,
    (byte) 5,
    (byte) 115,
    (byte) 94,
    (byte) 108,
    (byte) 27,
    (byte) 237,
    (byte) 187,
    (byte) 72,
    (byte) 10,
    (byte) 139,
    (byte) 212,
    (byte) 128 /*0x80*/,
    (byte) 238,
    (byte) 81,
    (byte) 244,
    (byte) 63 /*0x3F*/,
    (byte) 250,
    (byte) 117,
    (byte) 53,
    (byte) 250,
    (byte) 94,
    (byte) 95,
    (byte) 75,
    (byte) 212,
    (byte) 208 /*0xD0*/,
    (byte) 30,
    (byte) 170,
    (byte) 117,
    (byte) 40,
    (byte) 115,
    (byte) 203,
    (byte) 228,
    (byte) 226,
    (byte) 31 /*0x1F*/,
    (byte) 225,
    (byte) 107,
    (byte) 254,
    (byte) 91,
    (byte) 97,
    (byte) 144 /*0x90*/
  };

  internal static string ssp_office_15056()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 4)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 85,
        (byte) 185,
        (byte) 126,
        (byte) 167,
        (byte) 93,
        (byte) 220,
        (byte) 237,
        (byte) 144 /*0x90*/,
        (byte) 155,
        (byte) 251,
        (byte) 209,
        (byte) 221,
        (byte) 36,
        (byte) 140,
        (byte) 204,
        (byte) 226
      };
      byte[] numArray3 = new byte[16 /*0x10*/];
      numArray3[10] = (byte) 28;
      numArray3[2] = (byte) 125;
      numArray3[0] = (byte) 109;
      numArray3[3] = (byte) 124;
      numArray3[7] = (byte) 45;
      numArray3[14] = (byte) 51;
      numArray3[4] = (byte) 117;
      numArray3[6] = (byte) 154;
      numArray3[8] = (byte) 26;
      numArray3[9] = (byte) 198;
      numArray3[12] = (byte) 37;
      numArray3[5] = (byte) 15;
      numArray3[13] = (byte) 219;
      numArray3[11] = (byte) 97;
      numArray3[1] = (byte) 213;
      numArray3[15] = (byte) 220;
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/];
    numArray5[12] = (byte) 54;
    numArray5[1] = (byte) 199;
    numArray5[0] = (byte) 116;
    numArray5[2] = (byte) 176 /*0xB0*/;
    numArray5[4] = (byte) 27;
    numArray5[5] = (byte) 35;
    numArray5[6] = (byte) 208 /*0xD0*/;
    numArray5[7] = (byte) 113;
    numArray5[3] = (byte) 77;
    numArray5[9] = (byte) 235;
    numArray5[11] = (byte) 6;
    numArray5[8] = (byte) 235;
    numArray5[10] = (byte) 98;
    numArray5[15] = (byte) 59;
    numArray5[14] = (byte) 196;
    numArray5[13] = (byte) 72;
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 251,
      (byte) 224 /*0xE0*/,
      (byte) 190,
      (byte) 8,
      (byte) 153,
      (byte) 25,
      (byte) 120,
      (byte) 120,
      (byte) 116,
      (byte) 243,
      (byte) 211,
      (byte) 57,
      (byte) 18,
      (byte) 110,
      (byte) 118,
      (byte) 27
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_office_15057()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 6)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 226,
        (byte) 64 /*0x40*/,
        (byte) 19,
        (byte) 138,
        (byte) 81,
        (byte) 79,
        (byte) 154,
        (byte) 105,
        (byte) 100,
        (byte) 230,
        (byte) 104,
        (byte) 171,
        (byte) 183,
        (byte) 32 /*0x20*/,
        (byte) 116,
        (byte) 48 /*0x30*/
      };
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 117,
        (byte) 29,
        (byte) 43,
        (byte) 1,
        (byte) 171,
        (byte) 22,
        (byte) 148,
        (byte) 41,
        (byte) 237,
        (byte) 152,
        (byte) 175,
        (byte) 159,
        (byte) 155,
        (byte) 240 /*0xF0*/,
        (byte) 212,
        (byte) 252
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[39];
      byte[] response = new byte[39];
      Array.Copy((Array) sc_15055.sspq, 0, (Array) numArray4, 0, 39);
      key.Query(true, 349, numArray4, response);
      Array.Copy((Array) sc_15055.sspr, 0, (Array) numArray4, 0, 39);
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
      (byte) 150,
      (byte) 29,
      (byte) 233,
      (byte) 155,
      (byte) 222,
      (byte) 143,
      (byte) 167,
      (byte) 188,
      (byte) 13,
      (byte) 199,
      (byte) 103,
      (byte) 192 /*0xC0*/,
      (byte) 29,
      (byte) 169,
      (byte) 62,
      (byte) 165
    };
    byte[] numArray7 = new byte[16 /*0x10*/];
    numArray7[8] = (byte) 158;
    numArray7[1] = (byte) 233;
    numArray7[2] = (byte) 75;
    numArray7[10] = (byte) 53;
    numArray7[4] = (byte) 230;
    numArray7[6] = (byte) 192 /*0xC0*/;
    numArray7[13] = (byte) 46;
    numArray7[7] = (byte) 232;
    numArray7[3] = (byte) 242;
    numArray7[0] = (byte) 91;
    numArray7[5] = (byte) 119;
    numArray7[11] = (byte) 115;
    numArray7[12] = (byte) 193;
    numArray7[9] = (byte) 165;
    numArray7[14] = (byte) 74;
    numArray7[15] = (byte) 232;
    key.Query(true, 349, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }

  internal static string ssp_office_15058()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 10)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/];
      numArray2[13] = (byte) 115;
      numArray2[11] = (byte) 159;
      numArray2[8] = (byte) 94;
      numArray2[3] = (byte) 250;
      numArray2[4] = (byte) 44;
      numArray2[12] = (byte) 180;
      numArray2[1] = (byte) 35;
      numArray2[7] = (byte) 87;
      numArray2[0] = (byte) 177;
      numArray2[5] = (byte) 144 /*0x90*/;
      numArray2[10] = (byte) 244;
      numArray2[9] = (byte) 44;
      numArray2[2] = (byte) 158;
      numArray2[15] = (byte) 191;
      numArray2[14] = (byte) 154;
      numArray2[6] = (byte) 80 /*0x50*/;
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 114,
        (byte) 163,
        (byte) 214,
        (byte) 77,
        (byte) 198,
        (byte) 102,
        (byte) 61,
        (byte) 249,
        (byte) 235,
        (byte) 116,
        (byte) 210,
        (byte) 124,
        (byte) 6,
        (byte) 0,
        (byte) 64 /*0x40*/,
        (byte) 167
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[48 /*0x30*/];
      byte[] response = new byte[48 /*0x30*/];
      Array.Copy((Array) sc_15055.sspq, 39, (Array) numArray4, 0, 48 /*0x30*/);
      key.Query(true, 349, numArray4, response);
      Array.Copy((Array) sc_15055.sspr, 39, (Array) numArray4, 0, 48 /*0x30*/);
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
      (byte) 38,
      (byte) 114,
      (byte) 7,
      (byte) 149,
      (byte) 252,
      (byte) 64 /*0x40*/,
      (byte) 81,
      (byte) 127 /*0x7F*/,
      (byte) 222,
      (byte) 138,
      (byte) 161,
      (byte) 154,
      (byte) 228,
      (byte) 237,
      (byte) 14,
      (byte) 91
    };
    byte[] numArray7 = new byte[16 /*0x10*/]
    {
      (byte) 47,
      (byte) 178,
      (byte) 227,
      (byte) 16 /*0x10*/,
      (byte) 148,
      (byte) 116,
      (byte) 108,
      (byte) 99,
      (byte) 172,
      (byte) 183,
      (byte) 173,
      (byte) 210,
      (byte) 190,
      (byte) 186,
      (byte) 254,
      (byte) 242
    };
    key.Query(true, 349, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }
}
