// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16489
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16489
{
  private static byte[] sspq = new byte[47]
  {
    (byte) 132,
    (byte) 86,
    (byte) 20,
    (byte) 215,
    (byte) 173,
    (byte) 57,
    (byte) 229,
    (byte) 216,
    (byte) 37,
    (byte) 52,
    (byte) 250,
    (byte) 90,
    (byte) 201,
    (byte) 102,
    (byte) 129,
    (byte) 97,
    (byte) 246,
    (byte) 129,
    (byte) 180,
    (byte) 243,
    (byte) 189,
    (byte) 19,
    (byte) 139,
    (byte) 15,
    (byte) 172,
    (byte) 150,
    (byte) 161,
    (byte) 152,
    (byte) 180,
    (byte) 119,
    (byte) 27,
    (byte) 52,
    (byte) 171,
    (byte) 54,
    (byte) 215,
    (byte) 64 /*0x40*/,
    (byte) 146,
    (byte) 219,
    (byte) 209,
    (byte) 9,
    (byte) 155,
    (byte) 99,
    (byte) 204,
    (byte) 166,
    (byte) 30,
    (byte) 228,
    (byte) 55
  };
  private static byte[] sspr = new byte[47]
  {
    (byte) 163,
    (byte) 64 /*0x40*/,
    (byte) 253,
    (byte) 161,
    (byte) 153,
    (byte) 46,
    byte.MaxValue,
    (byte) 236,
    (byte) 245,
    (byte) 66,
    (byte) 55,
    (byte) 137,
    (byte) 158,
    (byte) 20,
    (byte) 233,
    (byte) 139,
    (byte) 209,
    (byte) 10,
    (byte) 128 /*0x80*/,
    (byte) 85,
    (byte) 95,
    (byte) 78,
    (byte) 89,
    (byte) 23,
    (byte) 160 /*0xA0*/,
    (byte) 13,
    (byte) 66,
    (byte) 106,
    (byte) 118,
    (byte) 81,
    (byte) 32 /*0x20*/,
    (byte) 231,
    (byte) 114,
    (byte) 171,
    (byte) 90,
    (byte) 77,
    (byte) 126,
    (byte) 235,
    (byte) 26,
    (byte) 108,
    (byte) 125,
    (byte) 98,
    (byte) 23,
    (byte) 207,
    (byte) 30,
    (byte) 61,
    (byte) 50
  };

  internal static string ssp_pdm_16490()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 6)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[1] = (byte) 137;
      numArray2[4] = (byte) 196;
      numArray2[0] = (byte) 239;
      numArray2[3] = (byte) 60;
      numArray2[5] = (byte) 70;
      numArray2[6] = (byte) 142;
      numArray2[2] = (byte) 189;
      byte[] numArray3 = new byte[7]
      {
        (byte) 196,
        (byte) 102,
        (byte) 46,
        (byte) 89,
        (byte) 25,
        (byte) 208 /*0xD0*/,
        (byte) 143
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
      (byte) 170,
      (byte) 0,
      (byte) 246,
      (byte) 115,
      (byte) 249,
      (byte) 5,
      (byte) 102
    };
    byte[] numArray6 = new byte[7]
    {
      (byte) 14,
      (byte) 91,
      (byte) 199,
      (byte) 69,
      (byte) 110,
      (byte) 119,
      (byte) 111
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_pdm_16491()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 6)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7]
      {
        (byte) 174,
        (byte) 112 /*0x70*/,
        (byte) 110,
        (byte) 143,
        (byte) 236,
        (byte) 116,
        (byte) 44
      };
      byte[] numArray3 = new byte[7]
      {
        (byte) 219,
        (byte) 237,
        (byte) 209,
        (byte) 98,
        (byte) 195,
        (byte) 214,
        (byte) 66
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
      (byte) 179,
      (byte) 127 /*0x7F*/,
      (byte) 0,
      (byte) 0,
      (byte) 59,
      (byte) 0,
      (byte) 0
    };
    numArray5[3] = (byte) 128 /*0x80*/;
    numArray5[2] = (byte) 66;
    numArray5[5] = (byte) 241;
    numArray5[6] = (byte) 219;
    byte[] numArray6 = new byte[7]
    {
      (byte) 108,
      (byte) 35,
      (byte) 27,
      (byte) 151,
      (byte) 43,
      (byte) 159,
      (byte) 149
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[47];
    byte[] response = new byte[47];
    Array.Copy((Array) sc_16489.sspq, 0, (Array) numArray7, 0, 47);
    key.Query(true, 351, numArray7, response);
    Array.Copy((Array) sc_16489.sspr, 0, (Array) numArray7, 0, 47);
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

  internal static string ssp_pdm_16492()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 6)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7]
      {
        (byte) 147,
        (byte) 90,
        (byte) 0,
        (byte) 243,
        (byte) 3,
        (byte) 213,
        (byte) 55
      };
      byte[] numArray3 = new byte[7]
      {
        (byte) 234,
        (byte) 15,
        (byte) 154,
        (byte) 95,
        (byte) 88,
        (byte) 81,
        (byte) 197
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
      (byte) 43,
      (byte) 22,
      (byte) 151,
      (byte) 76,
      (byte) 35,
      (byte) 174,
      (byte) 25
    };
    byte[] numArray6 = new byte[7]
    {
      (byte) 174,
      (byte) 94,
      (byte) 225,
      (byte) 5,
      (byte) 109,
      (byte) 47,
      (byte) 157
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
