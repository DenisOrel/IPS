// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18452
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18452
{
  private static byte[] sspq = new byte[45]
  {
    (byte) 33,
    (byte) 6,
    (byte) 252,
    (byte) 158,
    (byte) 170,
    (byte) 250,
    (byte) 164,
    (byte) 147,
    (byte) 222,
    (byte) 80 /*0x50*/,
    (byte) 161,
    (byte) 86,
    (byte) 174,
    (byte) 254,
    (byte) 138,
    (byte) 53,
    (byte) 131,
    (byte) 43,
    (byte) 56,
    (byte) 66,
    (byte) 20,
    (byte) 187,
    (byte) 76,
    (byte) 160 /*0xA0*/,
    (byte) 99,
    (byte) 151,
    (byte) 225,
    (byte) 104,
    (byte) 27,
    (byte) 40,
    (byte) 162,
    (byte) 60,
    (byte) 178,
    (byte) 29,
    (byte) 11,
    (byte) 101,
    (byte) 94,
    (byte) 34,
    (byte) 212,
    (byte) 42,
    (byte) 86,
    (byte) 178,
    (byte) 78,
    (byte) 10,
    (byte) 81
  };
  private static byte[] sspr = new byte[45]
  {
    (byte) 51,
    (byte) 96 /*0x60*/,
    (byte) 233,
    (byte) 157,
    (byte) 13,
    (byte) 90,
    (byte) 252,
    (byte) 31 /*0x1F*/,
    (byte) 209,
    (byte) 17,
    (byte) 66,
    (byte) 203,
    (byte) 126,
    (byte) 18,
    (byte) 176 /*0xB0*/,
    (byte) 18,
    (byte) 161,
    (byte) 222,
    (byte) 67,
    (byte) 149,
    (byte) 140,
    (byte) 2,
    (byte) 145,
    (byte) 192 /*0xC0*/,
    (byte) 178,
    (byte) 16 /*0x10*/,
    (byte) 143,
    (byte) 66,
    (byte) 12,
    (byte) 134,
    (byte) 19,
    (byte) 149,
    (byte) 19,
    (byte) 141,
    (byte) 249,
    (byte) 101,
    (byte) 48 /*0x30*/,
    (byte) 248,
    (byte) 145,
    (byte) 232,
    (byte) 234,
    (byte) 243,
    (byte) 84,
    (byte) 118,
    (byte) 153
  };

  internal static string ssp_signs_18453()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 5)
    {
      byte[] numArray1 = new byte[8];
      byte[] numArray2 = new byte[8];
      numArray2[6] = (byte) 70;
      numArray2[1] = (byte) 91;
      numArray2[2] = (byte) 192 /*0xC0*/;
      numArray2[3] = (byte) 127 /*0x7F*/;
      numArray2[4] = (byte) 36;
      numArray2[0] = (byte) 108;
      numArray2[5] = (byte) 250;
      numArray2[7] = (byte) 44;
      byte[] numArray3 = new byte[8]
      {
        (byte) 28,
        (byte) 203,
        (byte) 66,
        (byte) 14,
        (byte) 58,
        (byte) 238,
        (byte) 244,
        (byte) 134
      };
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 8);
      for (int index = 0; index < 8; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[45];
      byte[] response = new byte[45];
      Array.Copy((Array) sc_18452.sspq, 0, (Array) numArray4, 0, 45);
      key.Query(true, 353, numArray4, response);
      Array.Copy((Array) sc_18452.sspr, 0, (Array) numArray4, 0, 45);
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
    byte[] numArray5 = new byte[8];
    byte[] numArray6 = new byte[8];
    numArray6[2] = (byte) 16 /*0x10*/;
    numArray6[4] = (byte) 160 /*0xA0*/;
    numArray6[3] = (byte) 54;
    numArray6[1] = (byte) 158;
    numArray6[5] = (byte) 43;
    numArray6[7] = (byte) 245;
    numArray6[6] = (byte) 85;
    numArray6[0] = (byte) 30;
    byte[] numArray7 = new byte[8]
    {
      (byte) 65,
      (byte) 135,
      (byte) 163,
      (byte) 107,
      (byte) 152,
      (byte) 78,
      (byte) 157,
      (byte) 203
    };
    key.Query(true, 353, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 8);
    for (int index = 0; index < 8; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }
}
