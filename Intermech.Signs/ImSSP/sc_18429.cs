// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18429
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18429
{
  private static byte[] sspq = new byte[28]
  {
    (byte) 242,
    (byte) 44,
    (byte) 139,
    (byte) 145,
    (byte) 132,
    (byte) 9,
    (byte) 174,
    (byte) 144 /*0x90*/,
    (byte) 118,
    (byte) 143,
    (byte) 234,
    (byte) 206,
    (byte) 133,
    (byte) 181,
    (byte) 115,
    (byte) 12,
    (byte) 93,
    (byte) 246,
    (byte) 43,
    (byte) 247,
    (byte) 145,
    (byte) 67,
    (byte) 171,
    (byte) 93,
    (byte) 60,
    (byte) 90,
    (byte) 231,
    (byte) 167
  };
  private static byte[] sspr = new byte[28]
  {
    (byte) 85,
    (byte) 92,
    (byte) 153,
    (byte) 92,
    (byte) 147,
    (byte) 127 /*0x7F*/,
    (byte) 233,
    (byte) 151,
    (byte) 115,
    (byte) 163,
    (byte) 138,
    (byte) 131,
    (byte) 6,
    (byte) 56,
    (byte) 217,
    (byte) 15,
    (byte) 200,
    (byte) 68,
    (byte) 20,
    (byte) 45,
    (byte) 7,
    (byte) 160 /*0xA0*/,
    (byte) 35,
    (byte) 45,
    (byte) 170,
    (byte) 147,
    (byte) 40,
    (byte) 101
  };

  internal static string ssp_signs_18430()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 9)
    {
      byte[] numArray1 = new byte[8];
      byte[] numArray2 = new byte[8]
      {
        (byte) 14,
        (byte) 15,
        (byte) 200,
        (byte) 203,
        (byte) 225,
        (byte) 206,
        byte.MaxValue,
        (byte) 210
      };
      byte[] numArray3 = new byte[8]
      {
        (byte) 116,
        (byte) 153,
        (byte) 190,
        (byte) 84,
        (byte) 78,
        (byte) 92,
        (byte) 254,
        (byte) 236
      };
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 8);
      for (int index = 0; index < 8; ++index)
        numArray1[index] ^= numArray3[index];
      byte[] numArray4 = new byte[28];
      byte[] response = new byte[28];
      Array.Copy((Array) sc_18429.sspq, 0, (Array) numArray4, 0, 28);
      key.Query(true, 353, numArray4, response);
      Array.Copy((Array) sc_18429.sspr, 0, (Array) numArray4, 0, 28);
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
    byte[] numArray6 = new byte[8]
    {
      (byte) 147,
      (byte) 215,
      (byte) 229,
      (byte) 240 /*0xF0*/,
      (byte) 42,
      (byte) 241,
      (byte) 215,
      (byte) 35
    };
    byte[] numArray7 = new byte[8]
    {
      (byte) 21,
      (byte) 88,
      (byte) 232,
      (byte) 2,
      (byte) 25,
      (byte) 196,
      (byte) 203,
      (byte) 110
    };
    key.Query(true, 353, numArray6, numArray6);
    Array.Copy((Array) numArray6, 0, (Array) numArray5, 0, 8);
    for (int index = 0; index < 8; ++index)
      numArray5[index] ^= numArray7[index];
    return Encoding.UTF8.GetString(numArray5);
  }
}
