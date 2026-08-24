// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_7268
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_7268
{
  private static byte[] sspq = new byte[55]
  {
    (byte) 68,
    (byte) 41,
    (byte) 165,
    (byte) 210,
    (byte) 47,
    (byte) 237,
    (byte) 114,
    (byte) 42,
    (byte) 11,
    (byte) 241,
    (byte) 108,
    (byte) 91,
    (byte) 194,
    (byte) 52,
    (byte) 53,
    (byte) 62,
    (byte) 152,
    (byte) 136,
    (byte) 253,
    (byte) 78,
    (byte) 82,
    (byte) 243,
    (byte) 126,
    (byte) 66,
    (byte) 115,
    (byte) 174,
    (byte) 27,
    (byte) 157,
    (byte) 13,
    (byte) 209,
    (byte) 236,
    (byte) 59,
    (byte) 11,
    (byte) 12,
    (byte) 179,
    (byte) 81,
    (byte) 186,
    (byte) 209,
    (byte) 166,
    (byte) 41,
    (byte) 114,
    (byte) 138,
    (byte) 11,
    (byte) 153,
    (byte) 96 /*0x60*/,
    (byte) 186,
    (byte) 148,
    (byte) 91,
    (byte) 18,
    (byte) 122,
    (byte) 122,
    (byte) 161,
    (byte) 13,
    (byte) 162,
    (byte) 25
  };
  private static byte[] sspr = new byte[55]
  {
    (byte) 203,
    (byte) 145,
    (byte) 51,
    (byte) 131,
    (byte) 29,
    (byte) 49,
    (byte) 243,
    (byte) 56,
    (byte) 170,
    (byte) 174,
    (byte) 230,
    (byte) 61,
    (byte) 14,
    (byte) 1,
    (byte) 152,
    (byte) 130,
    (byte) 57,
    (byte) 153,
    (byte) 134,
    (byte) 34,
    (byte) 95,
    (byte) 175,
    (byte) 183,
    (byte) 53,
    (byte) 164,
    (byte) 240 /*0xF0*/,
    (byte) 188,
    (byte) 155,
    (byte) 121,
    (byte) 100,
    byte.MaxValue,
    (byte) 35,
    (byte) 50,
    (byte) 96 /*0x60*/,
    (byte) 34,
    (byte) 70,
    (byte) 32 /*0x20*/,
    (byte) 25,
    (byte) 213,
    (byte) 133,
    (byte) 236,
    (byte) 84,
    (byte) 7,
    (byte) 136,
    (byte) 155,
    (byte) 1,
    (byte) 156,
    (byte) 41,
    (byte) 46,
    (byte) 245,
    (byte) 155,
    (byte) 148,
    (byte) 112 /*0x70*/,
    (byte) 246,
    (byte) 61
  };

  internal static string ssp_imbase_7269()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 11)
    {
      byte[] numArray1 = new byte[7];
      byte[] numArray2 = new byte[7];
      numArray2[1] = (byte) 160 /*0xA0*/;
      numArray2[5] = (byte) 156;
      numArray2[2] = (byte) 33;
      numArray2[0] = (byte) 23;
      numArray2[6] = (byte) 84;
      numArray2[4] = (byte) 135;
      numArray2[3] = (byte) 209;
      byte[] numArray3 = new byte[7];
      numArray3[4] = (byte) 69;
      numArray3[1] = (byte) 27;
      numArray3[2] = (byte) 1;
      numArray3[5] = (byte) 91;
      numArray3[6] = (byte) 168;
      numArray3[0] = (byte) 158;
      numArray3[3] = (byte) 176 /*0xB0*/;
      key.Query(true, 343, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 7);
      for (int index = 0; index < 7; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[17];
      byte[] response = new byte[17];
      Array.Copy((Array) sc_7268.sspq, 0, (Array) numArray4, 0, 17);
      key.Query(true, 343, numArray4, response);
      Array.Copy((Array) sc_7268.sspr, 0, (Array) numArray4, 0, 17);
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
      (byte) 86,
      (byte) 195,
      (byte) 34,
      (byte) 185,
      (byte) 227,
      (byte) 110,
      (byte) 17
    };
    byte[] numArray7 = new byte[7]
    {
      (byte) 166,
      (byte) 248,
      (byte) 199,
      (byte) 93,
      byte.MaxValue,
      (byte) 201,
      (byte) 3
    };
    key.Query(true, 343, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 7);
    for (int index = 0; index < 7; ++index)
      numArray5[index] ^= numArray7[index];
    byte[] numArray8 = new byte[38];
    byte[] response1 = new byte[38];
    Array.Copy((Array) sc_7268.sspq, 17, (Array) numArray8, 0, 38);
    key.Query(true, 343, numArray8, response1);
    Array.Copy((Array) sc_7268.sspr, 17, (Array) numArray8, 0, 38);
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
}
