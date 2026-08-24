// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18518
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18518
{
  private static byte[] sspq = new byte[50]
  {
    (byte) 190,
    (byte) 141,
    (byte) 50,
    (byte) 86,
    (byte) 250,
    (byte) 47,
    (byte) 236,
    (byte) 168,
    (byte) 127 /*0x7F*/,
    (byte) 196,
    (byte) 93,
    (byte) 180,
    (byte) 191,
    (byte) 203,
    (byte) 129,
    (byte) 241,
    (byte) 16 /*0x10*/,
    (byte) 178,
    (byte) 129,
    (byte) 1,
    (byte) 251,
    (byte) 181,
    (byte) 251,
    (byte) 157,
    (byte) 148,
    (byte) 168,
    (byte) 155,
    (byte) 109,
    (byte) 189,
    (byte) 254,
    (byte) 47,
    (byte) 236,
    (byte) 137,
    (byte) 186,
    (byte) 136,
    (byte) 218,
    (byte) 48 /*0x30*/,
    (byte) 197,
    (byte) 167,
    (byte) 102,
    (byte) 216,
    (byte) 211,
    (byte) 130,
    (byte) 212,
    (byte) 160 /*0xA0*/,
    (byte) 230,
    (byte) 63 /*0x3F*/,
    (byte) 178,
    (byte) 232,
    (byte) 30
  };
  private static byte[] sspr = new byte[50]
  {
    (byte) 1,
    (byte) 30,
    (byte) 57,
    (byte) 46,
    (byte) 132,
    (byte) 203,
    (byte) 4,
    (byte) 159,
    (byte) 8,
    (byte) 206,
    (byte) 213,
    (byte) 239,
    (byte) 203,
    (byte) 231,
    (byte) 187,
    (byte) 164,
    (byte) 202,
    (byte) 134,
    (byte) 233,
    (byte) 218,
    (byte) 26,
    (byte) 214,
    (byte) 97,
    (byte) 160 /*0xA0*/,
    (byte) 168,
    (byte) 193,
    (byte) 178,
    (byte) 18,
    (byte) 118,
    (byte) 3,
    (byte) 175,
    (byte) 104,
    (byte) 39,
    (byte) 154,
    (byte) 166,
    (byte) 205,
    (byte) 191,
    (byte) 73,
    (byte) 27,
    (byte) 60,
    (byte) 3,
    (byte) 82,
    (byte) 16 /*0x10*/,
    (byte) 17,
    (byte) 202,
    (byte) 176 /*0xB0*/,
    (byte) 179,
    (byte) 33,
    (byte) 127 /*0x7F*/,
    (byte) 43
  };

  internal static string ssp_webportal_18519()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 7)
    {
      byte[] numArray1 = new byte[15];
      byte[] numArray2 = new byte[15]
      {
        (byte) 99,
        (byte) 28,
        (byte) 161,
        (byte) 149,
        (byte) 197,
        (byte) 123,
        (byte) 1,
        (byte) 153,
        (byte) 254,
        (byte) 155,
        byte.MaxValue,
        (byte) 126,
        (byte) 105,
        (byte) 15,
        (byte) 181
      };
      byte[] numArray3 = new byte[15]
      {
        (byte) 200,
        (byte) 116,
        (byte) 168,
        (byte) 174,
        (byte) 166,
        (byte) 195,
        (byte) 234,
        (byte) 56,
        (byte) 147,
        (byte) 149,
        (byte) 69,
        (byte) 33,
        (byte) 121,
        (byte) 200,
        (byte) 86
      };
      key.Query(true, 363, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 15);
      for (int index = 0; index < 15; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[50];
      byte[] response = new byte[50];
      Array.Copy((Array) sc_18518.sspq, 0, (Array) numArray4, 0, 50);
      key.Query(true, 363, numArray4, response);
      Array.Copy((Array) sc_18518.sspr, 0, (Array) numArray4, 0, 50);
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
    byte[] numArray5 = new byte[15];
    byte[] numArray6 = new byte[15]
    {
      (byte) 164,
      (byte) 128 /*0x80*/,
      (byte) 34,
      (byte) 25,
      (byte) 115,
      (byte) 9,
      (byte) 146,
      (byte) 149,
      (byte) 194,
      (byte) 81,
      (byte) 147,
      (byte) 126,
      (byte) 234,
      (byte) 128 /*0x80*/,
      (byte) 149
    };
    byte[] numArray7 = new byte[15]
    {
      (byte) 35,
      (byte) 36,
      (byte) 168,
      (byte) 60,
      (byte) 7,
      (byte) 231,
      (byte) 84,
      (byte) 15,
      (byte) 48 /*0x30*/,
      (byte) 142,
      (byte) 74,
      (byte) 61,
      (byte) 247,
      (byte) 195,
      (byte) 86
    };
    key.Query(true, 363, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 15);
    for (int index = 0; index < 15; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }
}
