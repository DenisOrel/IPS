// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16722
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16722
{
  private static byte[] sspq = new byte[156]
  {
    (byte) 108,
    (byte) 240 /*0xF0*/,
    (byte) 71,
    (byte) 86,
    (byte) 78,
    (byte) 118,
    (byte) 32 /*0x20*/,
    (byte) 184,
    (byte) 68,
    (byte) 85,
    (byte) 113,
    (byte) 246,
    (byte) 181,
    (byte) 164,
    (byte) 138,
    (byte) 124,
    (byte) 40,
    (byte) 101,
    (byte) 247,
    (byte) 39,
    (byte) 189,
    (byte) 220,
    (byte) 105,
    (byte) 60,
    (byte) 237,
    (byte) 32 /*0x20*/,
    (byte) 180,
    (byte) 90,
    (byte) 115,
    (byte) 198,
    (byte) 219,
    (byte) 219,
    (byte) 142,
    (byte) 160 /*0xA0*/,
    (byte) 217,
    (byte) 92,
    (byte) 151,
    (byte) 114,
    (byte) 52,
    (byte) 177,
    (byte) 248,
    (byte) 188,
    (byte) 52,
    (byte) 56,
    (byte) 236,
    (byte) 185,
    (byte) 127 /*0x7F*/,
    (byte) 225,
    (byte) 71,
    (byte) 197,
    (byte) 235,
    (byte) 97,
    (byte) 177,
    (byte) 31 /*0x1F*/,
    (byte) 173,
    (byte) 50,
    (byte) 17,
    (byte) 127 /*0x7F*/,
    (byte) 79,
    (byte) 61,
    (byte) 208 /*0xD0*/,
    (byte) 213,
    (byte) 44,
    (byte) 118,
    (byte) 186,
    (byte) 8,
    (byte) 165,
    (byte) 211,
    (byte) 3,
    (byte) 135,
    (byte) 51,
    (byte) 41,
    (byte) 117,
    (byte) 23,
    (byte) 35,
    (byte) 247,
    (byte) 225,
    (byte) 35,
    (byte) 143,
    (byte) 189,
    (byte) 126,
    (byte) 228,
    (byte) 19,
    (byte) 171,
    (byte) 221,
    (byte) 192 /*0xC0*/,
    (byte) 145,
    (byte) 194,
    (byte) 236,
    (byte) 171,
    (byte) 148,
    (byte) 51,
    (byte) 40,
    (byte) 222,
    (byte) 114,
    (byte) 253,
    (byte) 157,
    (byte) 253,
    (byte) 194,
    (byte) 126,
    (byte) 155,
    (byte) 64 /*0x40*/,
    (byte) 222,
    (byte) 53,
    (byte) 224 /*0xE0*/,
    (byte) 92,
    (byte) 60,
    (byte) 27,
    (byte) 6,
    (byte) 125,
    (byte) 36,
    (byte) 103,
    (byte) 233,
    (byte) 76,
    (byte) 180,
    (byte) 41,
    (byte) 62,
    (byte) 244,
    (byte) 245,
    (byte) 82,
    (byte) 28,
    (byte) 24,
    (byte) 140,
    (byte) 126,
    (byte) 219,
    (byte) 191,
    (byte) 126,
    (byte) 180,
    (byte) 95,
    (byte) 89,
    (byte) 33,
    (byte) 222,
    (byte) 207,
    (byte) 117,
    (byte) 224 /*0xE0*/,
    (byte) 35,
    (byte) 64 /*0x40*/,
    (byte) 6,
    (byte) 216,
    (byte) 41,
    (byte) 118,
    (byte) 35,
    (byte) 96 /*0x60*/,
    (byte) 41,
    (byte) 116,
    (byte) 205,
    (byte) 166,
    (byte) 92,
    (byte) 29,
    (byte) 246,
    (byte) 174,
    (byte) 116,
    (byte) 161,
    (byte) 66,
    (byte) 92,
    (byte) 103
  };
  private static byte[] sspr = new byte[156]
  {
    (byte) 139,
    (byte) 94,
    (byte) 241,
    (byte) 84,
    (byte) 216,
    (byte) 141,
    (byte) 210,
    (byte) 16 /*0x10*/,
    (byte) 230,
    (byte) 209,
    (byte) 11,
    (byte) 193,
    (byte) 17,
    (byte) 120,
    (byte) 110,
    (byte) 76,
    (byte) 247,
    (byte) 165,
    (byte) 177,
    (byte) 10,
    (byte) 121,
    (byte) 253,
    (byte) 244,
    (byte) 231,
    (byte) 83,
    (byte) 237,
    (byte) 180,
    (byte) 79,
    (byte) 68,
    (byte) 3,
    (byte) 241,
    (byte) 162,
    (byte) 38,
    (byte) 211,
    (byte) 224 /*0xE0*/,
    (byte) 84,
    (byte) 49,
    (byte) 90,
    (byte) 1,
    (byte) 42,
    (byte) 193,
    (byte) 79,
    (byte) 223,
    (byte) 167,
    (byte) 177,
    (byte) 136,
    (byte) 242,
    (byte) 61,
    (byte) 242,
    (byte) 109,
    (byte) 122,
    (byte) 144 /*0x90*/,
    (byte) 245,
    (byte) 196,
    (byte) 172,
    (byte) 23,
    (byte) 105,
    (byte) 240 /*0xF0*/,
    (byte) 148,
    (byte) 160 /*0xA0*/,
    (byte) 138,
    (byte) 242,
    (byte) 79,
    (byte) 101,
    (byte) 215,
    (byte) 11,
    (byte) 219,
    (byte) 223,
    (byte) 90,
    (byte) 49,
    (byte) 90,
    (byte) 201,
    (byte) 227,
    (byte) 12,
    (byte) 176 /*0xB0*/,
    (byte) 241,
    (byte) 240 /*0xF0*/,
    (byte) 61,
    (byte) 9,
    (byte) 144 /*0x90*/,
    (byte) 139,
    (byte) 201,
    (byte) 118,
    (byte) 39,
    (byte) 155,
    (byte) 45,
    (byte) 47,
    (byte) 240 /*0xF0*/,
    (byte) 116,
    (byte) 29,
    (byte) 92,
    (byte) 104,
    (byte) 83,
    (byte) 60,
    (byte) 157,
    (byte) 159,
    (byte) 74,
    (byte) 52,
    (byte) 127 /*0x7F*/,
    (byte) 69,
    (byte) 31 /*0x1F*/,
    (byte) 195,
    (byte) 91,
    (byte) 143,
    (byte) 89,
    (byte) 165,
    (byte) 19,
    (byte) 238,
    (byte) 132,
    (byte) 181,
    (byte) 169,
    (byte) 224 /*0xE0*/,
    (byte) 97,
    (byte) 237,
    (byte) 107,
    (byte) 248,
    (byte) 149,
    (byte) 138,
    (byte) 154,
    (byte) 204,
    (byte) 26,
    (byte) 131,
    (byte) 195,
    (byte) 112 /*0x70*/,
    (byte) 178,
    (byte) 104,
    (byte) 95,
    (byte) 174,
    (byte) 175,
    (byte) 103,
    (byte) 77,
    (byte) 237,
    (byte) 192 /*0xC0*/,
    (byte) 237,
    (byte) 90,
    (byte) 113,
    (byte) 16 /*0x10*/,
    (byte) 245,
    (byte) 198,
    (byte) 22,
    (byte) 8,
    (byte) 250,
    (byte) 35,
    (byte) 126,
    (byte) 151,
    (byte) 159,
    (byte) 175,
    (byte) 215,
    (byte) 5,
    (byte) 239,
    (byte) 117,
    (byte) 90,
    (byte) 185,
    (byte) 87,
    (byte) 226,
    (byte) 72
  };

  internal static string ssp_pdm_16723()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 12)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7]
      {
        (byte) 20,
        (byte) 199,
        (byte) 171,
        (byte) 195,
        (byte) 230,
        (byte) 241,
        (byte) 146
      };
      byte[] numArray3 = new byte[7];
      numArray3[6] = (byte) 44;
      numArray3[2] = (byte) 114;
      numArray3[4] = (byte) 55;
      numArray3[1] = (byte) 148;
      numArray3[0] = (byte) 96 /*0x60*/;
      numArray3[5] = (byte) 138;
      numArray3[3] = (byte) 128 /*0x80*/;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7];
    numArray5[6] = (byte) 46;
    numArray5[0] = (byte) 77;
    numArray5[2] = (byte) 88;
    numArray5[1] = byte.MaxValue;
    numArray5[4] = (byte) 112 /*0x70*/;
    numArray5[5] = (byte) 61;
    numArray5[3] = (byte) 111;
    byte[] numArray6 = new byte[7]
    {
      (byte) 158,
      (byte) 64 /*0x40*/,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 66
    };
    numArray6[3] = (byte) 192 /*0xC0*/;
    numArray6[2] = (byte) 166;
    numArray6[4] = (byte) 243;
    numArray6[5] = (byte) 41;
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[25];
    byte[] response = new byte[25];
    Array.Copy((Array) sc_16722.sspq, 0, (Array) numArray7, 0, 25);
    key.Query(true, 351, numArray7, response);
    Array.Copy((Array) sc_16722.sspr, 0, (Array) numArray7, 0, 25);
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

  internal static string ssp_pdm_16724()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[2] = (byte) 58;
      numArray2[5] = (byte) 19;
      numArray2[3] = (byte) 202;
      numArray2[6] = (byte) 114;
      numArray2[4] = (byte) 249;
      numArray2[0] = (byte) 147;
      numArray2[1] = (byte) 175;
      byte[] numArray3 = new byte[7]
      {
        (byte) 129,
        (byte) 159,
        (byte) 82,
        (byte) 12,
        (byte) 125,
        (byte) 201,
        (byte) 32 /*0x20*/
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7]
    {
      (byte) 205,
      (byte) 146,
      (byte) 53,
      (byte) 40,
      (byte) 93,
      (byte) 113,
      (byte) 206
    };
    byte[] numArray6 = new byte[7];
    numArray6[2] = (byte) 169;
    numArray6[1] = (byte) 49;
    numArray6[3] = (byte) 86;
    numArray6[5] = (byte) 243;
    numArray6[6] = (byte) 141;
    numArray6[0] = (byte) 183;
    numArray6[4] = (byte) 249;
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_pdm_16725()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 5)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[4] = (byte) 93;
      numArray2[1] = (byte) 125;
      numArray2[2] = (byte) 121;
      numArray2[6] = (byte) 47;
      numArray2[3] = (byte) 91;
      numArray2[0] = (byte) 112 /*0x70*/;
      numArray2[5] = (byte) 132;
      byte[] numArray3 = new byte[7];
      numArray3[5] = (byte) 201;
      numArray3[0] = (byte) 99;
      numArray3[1] = (byte) 161;
      numArray3[3] = (byte) 219;
      numArray3[2] = (byte) 176 /*0xB0*/;
      numArray3[4] = (byte) 149;
      numArray3[6] = (byte) 191;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[48 /*0x30*/];
      byte[] response = new byte[48 /*0x30*/];
      Array.Copy((Array) sc_16722.sspq, 25, (Array) numArray4, 0, 48 /*0x30*/);
      key.Query(true, 351, numArray4, response);
      Array.Copy((Array) sc_16722.sspr, 25, (Array) numArray4, 0, 48 /*0x30*/);
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
    byte[] numArray5 = new byte[7];
    byte[] numArray6 = new byte[7]
    {
      (byte) 5,
      (byte) 138,
      (byte) 121,
      (byte) 9,
      (byte) 51,
      (byte) 13,
      (byte) 82
    };
    byte[] numArray7 = new byte[7];
    numArray7[2] = (byte) 52;
    numArray7[1] = (byte) 172;
    numArray7[3] = (byte) 0;
    numArray7[5] = (byte) 3;
    numArray7[4] = (byte) 40;
    numArray7[0] = (byte) 120;
    numArray7[6] = (byte) 20;
    key.Query(true, 351, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray5[index] ^= numArray7[index];
    byte[] numArray8 = new byte[54];
    byte[] response1 = new byte[54];
    Array.Copy((Array) sc_16722.sspq, 73, (Array) numArray8, 0, 54);
    key.Query(true, 351, numArray8, response1);
    Array.Copy((Array) sc_16722.sspr, 73, (Array) numArray8, 0, 54);
    for (int index = 0; index < numArray8.Length; ++index)
    {
      if ((int) numArray8[index] != (int) response1[index])
      {
        key.TagValue = (int) response1[index];
        break;
      }
    }
    return Encoding.UTF8.GetString(numArray5);
  }

  internal static string ssp_pdm_16726()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[6] = (byte) 34;
      numArray2[1] = (byte) 37;
      numArray2[2] = (byte) 90;
      numArray2[3] = (byte) 227;
      numArray2[4] = (byte) 97;
      numArray2[5] = (byte) 17;
      numArray2[0] = (byte) 165;
      byte[] numArray3 = new byte[7]
      {
        (byte) 240 /*0xF0*/,
        (byte) 86,
        (byte) 165,
        (byte) 181,
        (byte) 167,
        (byte) 176 /*0xB0*/,
        (byte) 126
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7]
    {
      (byte) 44,
      (byte) 74,
      (byte) 220,
      (byte) 41,
      (byte) 72,
      (byte) 190,
      (byte) 168
    };
    byte[] numArray6 = new byte[7]
    {
      (byte) 160 /*0xA0*/,
      (byte) 102,
      (byte) 230,
      (byte) 235,
      (byte) 246,
      (byte) 177,
      (byte) 83
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[29];
    byte[] response = new byte[29];
    Array.Copy((Array) sc_16722.sspq, (int) sbyte.MaxValue, (Array) numArray7, 0, 29);
    key.Query(true, 351, numArray7, response);
    Array.Copy((Array) sc_16722.sspr, (int) sbyte.MaxValue, (Array) numArray7, 0, 29);
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

  internal static string ssp_pdm_16727()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 10)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7]
      {
        (byte) 219,
        (byte) 241,
        (byte) 175,
        (byte) 160 /*0xA0*/,
        (byte) 137,
        (byte) 165,
        (byte) 132
      };
      byte[] numArray3 = new byte[7]
      {
        (byte) 240 /*0xF0*/,
        (byte) 116,
        (byte) 254,
        (byte) 137,
        (byte) 162,
        (byte) 159,
        (byte) 65
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7];
    numArray5[3] = (byte) 71;
    numArray5[4] = (byte) 38;
    numArray5[2] = (byte) 75;
    numArray5[0] = (byte) 0;
    numArray5[5] = (byte) 114;
    numArray5[1] = (byte) 219;
    numArray5[6] = (byte) 179;
    byte[] numArray6 = new byte[7]
    {
      (byte) 26,
      (byte) 38,
      (byte) 249,
      (byte) 230,
      (byte) 252,
      byte.MaxValue,
      (byte) 226
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
