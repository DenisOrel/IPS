// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_14467
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_14467
{
  private static byte[] sspq = new byte[42]
  {
    (byte) 178,
    (byte) 247,
    (byte) 103,
    (byte) 102,
    (byte) 202,
    (byte) 135,
    (byte) 230,
    (byte) 101,
    (byte) 109,
    (byte) 54,
    (byte) 61,
    (byte) 46,
    (byte) 69,
    (byte) 106,
    (byte) 52,
    (byte) 123,
    (byte) 230,
    (byte) 46,
    (byte) 47,
    (byte) 127 /*0x7F*/,
    (byte) 240 /*0xF0*/,
    (byte) 216,
    (byte) 175,
    (byte) 62,
    (byte) 148,
    (byte) 22,
    (byte) 215,
    (byte) 252,
    (byte) 141,
    (byte) 253,
    (byte) 96 /*0x60*/,
    (byte) 224 /*0xE0*/,
    (byte) 252,
    (byte) 142,
    (byte) 182,
    (byte) 125,
    (byte) 241,
    (byte) 151,
    (byte) 111,
    (byte) 196,
    (byte) 54,
    (byte) 218
  };
  private static byte[] sspr = new byte[42]
  {
    (byte) 216,
    (byte) 200,
    (byte) 222,
    (byte) 166,
    (byte) 21,
    (byte) 182,
    (byte) 228,
    (byte) 61,
    (byte) 215,
    (byte) 84,
    (byte) 99,
    (byte) 125,
    (byte) 20,
    (byte) 120,
    (byte) 15,
    (byte) 48 /*0x30*/,
    (byte) 66,
    (byte) 47,
    (byte) 238,
    (byte) 36,
    (byte) 170,
    (byte) 191,
    (byte) 14,
    (byte) 203,
    (byte) 123,
    (byte) 121,
    (byte) 232,
    (byte) 83,
    (byte) 99,
    (byte) 148,
    (byte) 219,
    (byte) 171,
    (byte) 176 /*0xB0*/,
    (byte) 246,
    (byte) 107,
    (byte) 120,
    (byte) 223,
    (byte) 90,
    (byte) 236,
    (byte) 237,
    (byte) 155,
    (byte) 249
  };

  internal static string ssp_imbase_14468()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 5)
    {
      byte[] numArray1 = new byte[10];
      byte[] numArray2 = new byte[10]
      {
        (byte) 190,
        (byte) 65,
        (byte) 190,
        (byte) 162,
        (byte) 46,
        (byte) 181,
        (byte) 6,
        (byte) 220,
        (byte) 225,
        (byte) 39
      };
      byte[] numArray3 = new byte[10];
      numArray3[3] = (byte) 178;
      numArray3[5] = (byte) 140;
      numArray3[2] = (byte) 24;
      numArray3[0] = (byte) 17;
      numArray3[4] = (byte) 41;
      numArray3[6] = (byte) 182;
      numArray3[1] = (byte) 109;
      numArray3[7] = (byte) 232;
      numArray3[8] = (byte) 26;
      numArray3[9] = (byte) 232;
      key.Query(true, 343, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 10);
      for (int index = 0; index < 10; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[10];
    byte[] numArray5 = new byte[10];
    numArray5[9] = (byte) 140;
    numArray5[1] = (byte) 116;
    numArray5[2] = (byte) 233;
    numArray5[6] = (byte) 250;
    numArray5[5] = (byte) 97;
    numArray5[8] = (byte) 92;
    numArray5[3] = (byte) 74;
    numArray5[7] = (byte) 249;
    numArray5[0] = (byte) 128 /*0x80*/;
    numArray5[4] = (byte) 210;
    byte[] numArray6 = new byte[10]
    {
      (byte) 13,
      (byte) 118,
      (byte) 26,
      (byte) 78,
      (byte) 28,
      (byte) 170,
      (byte) 89,
      (byte) 102,
      (byte) 214,
      (byte) 41
    };
    key.Query(true, 343, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 10);
    for (int index = 0; index < 10; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[21];
    byte[] response = new byte[21];
    Array.Copy((Array) sc_14467.sspq, 0, (Array) numArray7, 0, 21);
    key.Query(true, 343, numArray7, response);
    Array.Copy((Array) sc_14467.sspr, 0, (Array) numArray7, 0, 21);
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

  internal static string ssp_imbase_14469()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 5)
    {
      byte[] numArray1 = new byte[9];
      byte[] numArray2 = new byte[9]
      {
        (byte) 150,
        (byte) 134,
        (byte) 106,
        (byte) 60,
        (byte) 209,
        (byte) 104,
        (byte) 115,
        (byte) 213,
        (byte) 198
      };
      byte[] numArray3 = new byte[9]
      {
        (byte) 82,
        (byte) 52,
        (byte) 178,
        (byte) 32 /*0x20*/,
        (byte) 116,
        (byte) 189,
        (byte) 178,
        (byte) 41,
        (byte) 219
      };
      key.Query(true, 343, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 9);
      for (int index = 0; index < 9; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[9];
    byte[] numArray5 = new byte[9]
    {
      (byte) 152,
      (byte) 219,
      (byte) 250,
      (byte) 148,
      (byte) 46,
      (byte) 212,
      (byte) 165,
      (byte) 114,
      (byte) 180
    };
    byte[] numArray6 = new byte[9]
    {
      (byte) 33,
      (byte) 141,
      (byte) 164,
      (byte) 24,
      (byte) 248,
      (byte) 150,
      (byte) 109,
      (byte) 6,
      (byte) 250
    };
    key.Query(true, 343, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 9);
    for (int index = 0; index < 9; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[21];
    byte[] response = new byte[21];
    Array.Copy((Array) sc_14467.sspq, 21, (Array) numArray7, 0, 21);
    key.Query(true, 343, numArray7, response);
    Array.Copy((Array) sc_14467.sspr, 21, (Array) numArray7, 0, 21);
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
}
