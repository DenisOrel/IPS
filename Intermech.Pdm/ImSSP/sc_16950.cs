// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_16950
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_16950
{
  private static byte[] sspq = new byte[31 /*0x1F*/]
  {
    (byte) 72,
    (byte) 191,
    (byte) 225,
    (byte) 249,
    (byte) 167,
    (byte) 54,
    (byte) 70,
    (byte) 253,
    (byte) 217,
    (byte) 159,
    (byte) 90,
    (byte) 114,
    (byte) 228,
    (byte) 129,
    (byte) 80 /*0x50*/,
    (byte) 223,
    (byte) 47,
    (byte) 38,
    (byte) 231,
    (byte) 80 /*0x50*/,
    (byte) 103,
    (byte) 219,
    (byte) 207,
    (byte) 233,
    (byte) 113,
    (byte) 40,
    (byte) 64 /*0x40*/,
    (byte) 214,
    (byte) 203,
    (byte) 199,
    (byte) 2
  };
  private static byte[] sspr = new byte[31 /*0x1F*/]
  {
    (byte) 219,
    (byte) 189,
    (byte) 117,
    (byte) 102,
    (byte) 21,
    (byte) 215,
    (byte) 103,
    (byte) 76,
    (byte) 51,
    (byte) 33,
    (byte) 14,
    (byte) 56,
    (byte) 81,
    (byte) 127 /*0x7F*/,
    (byte) 63 /*0x3F*/,
    (byte) 48 /*0x30*/,
    (byte) 176 /*0xB0*/,
    (byte) 62,
    (byte) 163,
    (byte) 165,
    (byte) 245,
    (byte) 130,
    (byte) 67,
    (byte) 204,
    (byte) 48 /*0x30*/,
    (byte) 85,
    (byte) 124,
    (byte) 56,
    (byte) 186,
    (byte) 132,
    (byte) 225
  };

  internal static string ssp_pdm_16951()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 7)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7]
      {
        (byte) 87,
        (byte) 134,
        (byte) 206,
        (byte) 236,
        (byte) 172,
        (byte) 242,
        (byte) 137
      };
      byte[] numArray3 = new byte[7]
      {
        (byte) 25,
        (byte) 1,
        (byte) 203,
        (byte) 80 /*0x50*/,
        (byte) 108,
        (byte) 115,
        (byte) 138
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7];
    numArray5[3] = (byte) 86;
    numArray5[1] = (byte) 152;
    numArray5[2] = (byte) 3;
    numArray5[5] = (byte) 207;
    numArray5[6] = (byte) 190;
    numArray5[4] = (byte) 56;
    numArray5[0] = (byte) 10;
    byte[] numArray6 = new byte[7]
    {
      (byte) 224 /*0xE0*/,
      (byte) 69,
      (byte) 171,
      (byte) 21,
      (byte) 29,
      (byte) 44,
      (byte) 213
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_pdm_16952()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7]
      {
        (byte) 1,
        (byte) 42,
        (byte) 3,
        (byte) 186,
        (byte) 48 /*0x30*/,
        (byte) 40,
        (byte) 134
      };
      byte[] numArray3 = new byte[7];
      numArray3[2] = (byte) 172;
      numArray3[1] = (byte) 219;
      numArray3[0] = (byte) 40;
      numArray3[5] = (byte) 194;
      numArray3[4] = (byte) 111;
      numArray3[3] = (byte) 52;
      numArray3[6] = (byte) 88;
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[31 /*0x1F*/];
      byte[] response = new byte[31 /*0x1F*/];
      Array.Copy((Array) sc_16950.sspq, 0, (Array) numArray4, 0, 31 /*0x1F*/);
      key.Query(true, 351, numArray4, response);
      Array.Copy((Array) sc_16950.sspr, 0, (Array) numArray4, 0, 31 /*0x1F*/);
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
    byte[] numArray6 = new byte[7];
    numArray6[5] = (byte) 200;
    numArray6[1] = (byte) 202;
    numArray6[0] = (byte) 36;
    numArray6[3] = (byte) 226;
    numArray6[4] = (byte) 227;
    numArray6[6] = (byte) 8;
    numArray6[2] = (byte) 220;
    byte[] numArray7 = new byte[7]
    {
      (byte) 17,
      (byte) 21,
      (byte) 202,
      (byte) 112 /*0x70*/,
      (byte) 188,
      (byte) 74,
      (byte) 80 /*0x50*/
    };
    key.Query(true, 351, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }

  internal static string ssp_pdm_16953()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 7)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7]
      {
        (byte) 172,
        (byte) 204,
        (byte) 143,
        (byte) 31 /*0x1F*/,
        (byte) 64 /*0x40*/,
        (byte) 197,
        (byte) 108
      };
      byte[] numArray3 = new byte[7]
      {
        (byte) 105,
        (byte) 181,
        (byte) 40,
        (byte) 52,
        (byte) 88,
        (byte) 59,
        (byte) 104
      };
      key.Query(true, 351, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[7];
    byte[] numArray5 = new byte[7];
    numArray5[2] = (byte) 112 /*0x70*/;
    numArray5[1] = (byte) 245;
    numArray5[5] = (byte) 157;
    numArray5[3] = (byte) 49;
    numArray5[0] = (byte) 127 /*0x7F*/;
    numArray5[4] = (byte) 17;
    numArray5[6] = (byte) 54;
    byte[] numArray6 = new byte[7]
    {
      (byte) 75,
      (byte) 45,
      (byte) 130,
      (byte) 167,
      (byte) 170,
      (byte) 217,
      (byte) 87
    };
    key.Query(true, 351, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
