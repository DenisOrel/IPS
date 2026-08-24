// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16458
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16458
{
  private static byte[] sspq = new byte[139]
  {
    (byte) 104,
    (byte) 43,
    (byte) 129,
    (byte) 129,
    (byte) 248,
    (byte) 119,
    (byte) 54,
    (byte) 34,
    (byte) 158,
    (byte) 68,
    (byte) 136,
    (byte) 185,
    (byte) 30,
    (byte) 141,
    (byte) 84,
    (byte) 188,
    (byte) 22,
    (byte) 169,
    (byte) 192 /*0xC0*/,
    (byte) 30,
    (byte) 83,
    (byte) 86,
    (byte) 210,
    (byte) 88,
    (byte) 198,
    (byte) 68,
    (byte) 79,
    (byte) 60,
    (byte) 181,
    (byte) 168,
    (byte) 52,
    (byte) 189,
    (byte) 111,
    (byte) 90,
    (byte) 106,
    (byte) 60,
    (byte) 234,
    (byte) 197,
    (byte) 245,
    (byte) 210,
    (byte) 151,
    (byte) 49,
    (byte) 233,
    (byte) 236,
    (byte) 71,
    (byte) 142,
    (byte) 55,
    (byte) 231,
    (byte) 102,
    (byte) 41,
    (byte) 208 /*0xD0*/,
    (byte) 195,
    (byte) 92,
    (byte) 237,
    (byte) 135,
    (byte) 33,
    (byte) 109,
    (byte) 230,
    (byte) 17,
    (byte) 56,
    (byte) 13,
    (byte) 103,
    (byte) 213,
    (byte) 23,
    (byte) 130,
    (byte) 165,
    (byte) 192 /*0xC0*/,
    (byte) 36,
    (byte) 226,
    (byte) 30,
    (byte) 157,
    (byte) 105,
    (byte) 207,
    (byte) 233,
    (byte) 202,
    (byte) 52,
    (byte) 15,
    (byte) 71,
    (byte) 114,
    (byte) 67,
    (byte) 233,
    (byte) 93,
    (byte) 121,
    (byte) 9,
    (byte) 149,
    (byte) 244,
    (byte) 155,
    (byte) 162,
    (byte) 135,
    (byte) 51,
    (byte) 89,
    (byte) 98,
    (byte) 242,
    (byte) 11,
    (byte) 125,
    (byte) 181,
    (byte) 112 /*0x70*/,
    (byte) 170,
    (byte) 136,
    (byte) 155,
    (byte) 233,
    (byte) 121,
    (byte) 226,
    (byte) 213,
    (byte) 63 /*0x3F*/,
    (byte) 18,
    (byte) 116,
    (byte) 136,
    (byte) 250,
    (byte) 25,
    (byte) 72,
    (byte) 54,
    (byte) 201,
    (byte) 130,
    (byte) 127 /*0x7F*/,
    (byte) 18,
    (byte) 168,
    (byte) 110,
    (byte) 235,
    (byte) 34,
    (byte) 197,
    (byte) 199,
    (byte) 135,
    (byte) 125,
    (byte) 139,
    (byte) 6,
    (byte) 40,
    (byte) 250,
    (byte) 144 /*0x90*/,
    (byte) 203,
    (byte) 220,
    (byte) 51,
    (byte) 16 /*0x10*/,
    (byte) 4,
    (byte) 155,
    (byte) 176 /*0xB0*/,
    (byte) 27,
    (byte) 229,
    (byte) 85
  };
  private static byte[] sspr = new byte[139]
  {
    (byte) 59,
    (byte) 226,
    (byte) 234,
    (byte) 53,
    (byte) 119,
    (byte) 93,
    (byte) 248,
    (byte) 189,
    (byte) 42,
    (byte) 206,
    (byte) 202,
    (byte) 80 /*0x50*/,
    (byte) 178,
    (byte) 236,
    (byte) 103,
    (byte) 162,
    (byte) 225,
    (byte) 105,
    (byte) 184,
    (byte) 140,
    (byte) 241,
    (byte) 111,
    (byte) 161,
    (byte) 166,
    (byte) 65,
    (byte) 228,
    (byte) 96 /*0x60*/,
    (byte) 194,
    (byte) 241,
    (byte) 119,
    (byte) 191,
    (byte) 104,
    (byte) 6,
    (byte) 244,
    (byte) 137,
    (byte) 92,
    (byte) 187,
    (byte) 48 /*0x30*/,
    (byte) 27,
    (byte) 193,
    (byte) 154,
    (byte) 189,
    (byte) 180,
    (byte) 230,
    (byte) 37,
    (byte) 104,
    (byte) 34,
    (byte) 200,
    (byte) 157,
    (byte) 221,
    (byte) 74,
    (byte) 106,
    (byte) 177,
    (byte) 25,
    (byte) 9,
    (byte) 37,
    (byte) 182,
    (byte) 64 /*0x40*/,
    (byte) 197,
    (byte) 30,
    (byte) 30,
    (byte) 247,
    (byte) 151,
    (byte) 210,
    (byte) 132,
    (byte) 62,
    (byte) 247,
    (byte) 99,
    (byte) 68,
    (byte) 146,
    (byte) 203,
    (byte) 69,
    (byte) 200,
    (byte) 120,
    (byte) 60,
    (byte) 136,
    (byte) 184,
    (byte) 224 /*0xE0*/,
    (byte) 107,
    (byte) 242,
    (byte) 21,
    (byte) 48 /*0x30*/,
    (byte) 96 /*0x60*/,
    (byte) 109,
    (byte) 195,
    (byte) 102,
    (byte) 165,
    (byte) 12,
    (byte) 254,
    (byte) 76,
    (byte) 33,
    (byte) 186,
    (byte) 96 /*0x60*/,
    (byte) 234,
    (byte) 180,
    (byte) 77,
    (byte) 128 /*0x80*/,
    (byte) 224 /*0xE0*/,
    (byte) 249,
    (byte) 148,
    (byte) 183,
    (byte) 104,
    (byte) 14,
    (byte) 55,
    (byte) 13,
    (byte) 12,
    (byte) 98,
    (byte) 123,
    (byte) 173,
    (byte) 110,
    (byte) 89,
    (byte) 187,
    (byte) 180,
    (byte) 166,
    (byte) 230,
    (byte) 183,
    (byte) 245,
    (byte) 15,
    (byte) 26,
    (byte) 211,
    (byte) 42,
    (byte) 192 /*0xC0*/,
    (byte) 173,
    (byte) 217,
    (byte) 130,
    (byte) 94,
    (byte) 166,
    (byte) 149,
    (byte) 174,
    (byte) 206,
    (byte) 149,
    (byte) 96 /*0x60*/,
    (byte) 180,
    (byte) 104,
    (byte) 154,
    (byte) 229,
    (byte) 195,
    (byte) 123,
    (byte) 25
  };

  internal static string ssp_pdm_16459()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 9)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[2] = (byte) 84;
      numArray2[1] = (byte) 120;
      numArray2[3] = (byte) 62;
      numArray2[0] = (byte) 243;
      numArray2[4] = (byte) 50;
      numArray2[5] = (byte) 22;
      numArray2[6] = (byte) 142;
      byte[] numArray3 = new byte[7]
      {
        (byte) 162,
        (byte) 169,
        (byte) 49,
        (byte) 85,
        (byte) 45,
        (byte) 13,
        (byte) 101
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7];
    numArray5[3] = (byte) 131;
    numArray5[1] = (byte) 163;
    numArray5[2] = byte.MaxValue;
    numArray5[6] = (byte) 151;
    numArray5[4] = (byte) 168;
    numArray5[5] = (byte) 25;
    numArray5[0] = (byte) 211;
    byte[] numArray6 = new byte[7]
    {
      (byte) 154,
      (byte) 220,
      (byte) 113,
      (byte) 111,
      (byte) 95,
      (byte) 4,
      (byte) 202
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_pdm_16460()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 6)
    {
      byte[] numArray1 = new byte[14];
      byte[] numArray2 = new byte[14];
      numArray2[13] = (byte) 59;
      numArray2[12] = (byte) 146;
      numArray2[6] = (byte) 0;
      numArray2[3] = (byte) 68;
      numArray2[4] = (byte) 2;
      numArray2[5] = (byte) 37;
      numArray2[2] = (byte) 13;
      numArray2[7] = (byte) 122;
      numArray2[8] = (byte) 71;
      numArray2[0] = (byte) 212;
      numArray2[10] = (byte) 170;
      numArray2[11] = (byte) 142;
      numArray2[9] = (byte) 46;
      numArray2[1] = (byte) 121;
      byte[] numArray3 = new byte[14];
      numArray3[7] = (byte) 227;
      numArray3[3] = (byte) 145;
      numArray3[6] = (byte) 156;
      numArray3[2] = (byte) 7;
      numArray3[1] = (byte) 55;
      numArray3[5] = (byte) 134;
      numArray3[11] = (byte) 132;
      numArray3[12] = (byte) 111;
      numArray3[8] = (byte) 52;
      numArray3[9] = (byte) 124;
      numArray3[10] = (byte) 240 /*0xF0*/;
      numArray3[4] = (byte) 49;
      numArray3[0] = (byte) 137;
      numArray3[13] = (byte) 38;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 14);
      for (int index = 0; index < 14; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[14];
    byte[] numArray5 = new byte[14]
    {
      (byte) 70,
      (byte) 237,
      (byte) 73,
      (byte) 206,
      (byte) 84,
      (byte) 135,
      (byte) 48 /*0x30*/,
      (byte) 208 /*0xD0*/,
      (byte) 71,
      (byte) 85,
      (byte) 117,
      (byte) 187,
      (byte) 229,
      (byte) 231
    };
    byte[] numArray6 = new byte[14]
    {
      (byte) 173,
      (byte) 193,
      (byte) 226,
      (byte) 124,
      (byte) 120,
      (byte) 222,
      (byte) 200,
      (byte) 160 /*0xA0*/,
      (byte) 122,
      (byte) 250,
      (byte) 61,
      (byte) 78,
      (byte) 140,
      (byte) 48 /*0x30*/
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 14);
    for (int index = 0; index < 14; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[53];
    byte[] response = new byte[53];
    Array.Copy((Array) sc_16458.sspq, 0, (Array) numArray7, 0, 53);
    key.Query(true, 351, numArray7, response);
    Array.Copy((Array) sc_16458.sspr, 0, (Array) numArray7, 0, 53);
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

  internal static string ssp_pdm_16461()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[6];
      byte[] numArray2 = new byte[6]
      {
        (byte) 127 /*0x7F*/,
        (byte) 17,
        (byte) 89,
        (byte) 40,
        (byte) 186,
        (byte) 179
      };
      byte[] numArray3 = new byte[6]
      {
        (byte) 247,
        (byte) 27,
        (byte) 48 /*0x30*/,
        (byte) 13,
        (byte) 93,
        (byte) 49
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 6);
      for (int index = 0; index < 6; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[6];
    byte[] numArray5 = new byte[6]
    {
      (byte) 246,
      (byte) 15,
      (byte) 203,
      (byte) 73,
      (byte) 196,
      (byte) 35
    };
    byte[] numArray6 = new byte[6]
    {
      (byte) 181,
      (byte) 157,
      (byte) 241,
      (byte) 26,
      (byte) 81,
      (byte) 239
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 6);
    for (int index = 0; index < 6; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[10];
    byte[] response = new byte[10];
    Array.Copy((Array) sc_16458.sspq, 53, (Array) numArray7, 0, 10);
    key.Query(true, 351, numArray7, response);
    Array.Copy((Array) sc_16458.sspr, 53, (Array) numArray7, 0, 10);
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

  internal static string ssp_pdm_16462()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[4] = (byte) 218;
      numArray2[0] = (byte) 215;
      numArray2[1] = (byte) 113;
      numArray2[3] = (byte) 121;
      numArray2[2] = (byte) 97;
      numArray2[5] = (byte) 89;
      numArray2[6] = (byte) 227;
      byte[] numArray3 = new byte[7]
      {
        (byte) 237,
        (byte) 59,
        (byte) 6,
        (byte) 28,
        (byte) 197,
        (byte) 91,
        (byte) 190
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
      (byte) 114,
      (byte) 196,
      (byte) 78,
      (byte) 88,
      (byte) 126,
      (byte) 15,
      (byte) 110
    };
    byte[] numArray6 = new byte[7]
    {
      (byte) 69,
      (byte) 245,
      (byte) 57,
      (byte) 24,
      (byte) 150,
      (byte) 6,
      (byte) 89
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[49];
    byte[] response = new byte[49];
    Array.Copy((Array) sc_16458.sspq, 63 /*0x3F*/, (Array) numArray7, 0, 49);
    key.Query(true, 351, numArray7, response);
    Array.Copy((Array) sc_16458.sspr, 63 /*0x3F*/, (Array) numArray7, 0, 49);
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

  internal static string ssp_pdm_16463()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[6] = (byte) 141;
      numArray2[1] = (byte) 132;
      numArray2[2] = byte.MaxValue;
      numArray2[3] = (byte) 42;
      numArray2[4] = (byte) 105;
      numArray2[0] = (byte) 43;
      numArray2[5] = (byte) 130;
      byte[] numArray3 = new byte[7];
      numArray3[2] = (byte) 124;
      numArray3[1] = (byte) 100;
      numArray3[6] = (byte) 188;
      numArray3[5] = (byte) 144 /*0x90*/;
      numArray3[4] = (byte) 241;
      numArray3[0] = (byte) 106;
      numArray3[3] = (byte) 66;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[27];
      byte[] response = new byte[27];
      Array.Copy((Array) sc_16458.sspq, 112 /*0x70*/, (Array) numArray4, 0, 27);
      key.Query(true, 351, numArray4, response);
      Array.Copy((Array) sc_16458.sspr, 112 /*0x70*/, (Array) numArray4, 0, 27);
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
      (byte) 227,
      (byte) 120,
      (byte) 197,
      (byte) 208 /*0xD0*/,
      (byte) 19,
      (byte) 131,
      (byte) 133
    };
    byte[] numArray7 = new byte[7]
    {
      (byte) 98,
      (byte) 0,
      (byte) 141,
      (byte) 20,
      (byte) 0,
      (byte) 0,
      (byte) 193
    };
    numArray7[4] = (byte) 76;
    numArray7[5] = (byte) 4;
    numArray7[1] = (byte) 77;
    key.Query(true, 351, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }
}
