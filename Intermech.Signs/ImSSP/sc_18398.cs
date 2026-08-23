// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18398
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18398
{
  private static byte[] sspq = new byte[27]
  {
    (byte) 180,
    (byte) 82,
    (byte) 93,
    (byte) 170,
    (byte) 1,
    (byte) 45,
    (byte) 45,
    (byte) 132,
    (byte) 24,
    (byte) 252,
    (byte) 222,
    (byte) 241,
    (byte) 143,
    (byte) 61,
    (byte) 88,
    (byte) 243,
    (byte) 138,
    (byte) 47,
    (byte) 242,
    (byte) 198,
    (byte) 116,
    (byte) 47,
    (byte) 23,
    (byte) 0,
    (byte) 151,
    (byte) 66,
    (byte) 87
  };
  private static byte[] sspr = new byte[27]
  {
    (byte) 59,
    (byte) 14,
    (byte) 54,
    (byte) 168,
    (byte) 244,
    (byte) 28,
    (byte) 21,
    (byte) 41,
    (byte) 208 /*0xD0*/,
    (byte) 156,
    (byte) 120,
    (byte) 111,
    (byte) 196,
    (byte) 232,
    (byte) 122,
    (byte) 213,
    (byte) 73,
    (byte) 119,
    (byte) 20,
    (byte) 240 /*0xF0*/,
    (byte) 214,
    (byte) 57,
    (byte) 223,
    (byte) 133,
    (byte) 250,
    (byte) 121,
    (byte) 230
  };

  internal static string ssp_signs_18399()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 6)
    {
      byte[] numArray1 = new byte[8];
      byte[] numArray2 = new byte[8]
      {
        (byte) 91,
        (byte) 28,
        (byte) 253,
        (byte) 154,
        (byte) 26,
        (byte) 27,
        (byte) 221,
        (byte) 117
      };
      byte[] numArray3 = new byte[8]
      {
        (byte) 26,
        (byte) 79,
        (byte) 93,
        (byte) 188,
        (byte) 73,
        (byte) 181,
        (byte) 145,
        (byte) 210
      };
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 8);
      for (int index = 0; index < 8; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[8];
    byte[] numArray5 = new byte[8];
    numArray5[4] = (byte) 96 /*0x60*/;
    numArray5[1] = (byte) 80 /*0x50*/;
    numArray5[3] = (byte) 101;
    numArray5[2] = (byte) 162;
    numArray5[0] = (byte) 224 /*0xE0*/;
    numArray5[5] = (byte) 196;
    numArray5[6] = (byte) 148;
    numArray5[7] = (byte) 135;
    byte[] numArray6 = new byte[8];
    numArray6[4] = (byte) 50;
    numArray6[1] = (byte) 216;
    numArray6[6] = (byte) 101;
    numArray6[2] = (byte) 253;
    numArray6[0] = (byte) 72;
    numArray6[5] = (byte) 90;
    numArray6[3] = (byte) 99;
    numArray6[7] = (byte) 252;
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 8);
    for (int index = 0; index < 8; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_signs_18400()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 7)
    {
      byte[] numArray1 = new byte[9];
      byte[] numArray2 = new byte[9]
      {
        (byte) 142,
        (byte) 62,
        (byte) 69,
        byte.MaxValue,
        (byte) 153,
        (byte) 146,
        (byte) 119,
        (byte) 105,
        (byte) 235
      };
      byte[] numArray3 = new byte[9];
      numArray3[3] = (byte) 189;
      numArray3[0] = (byte) 218;
      numArray3[8] = (byte) 170;
      numArray3[1] = (byte) 198;
      numArray3[2] = (byte) 32 /*0x20*/;
      numArray3[5] = (byte) 241;
      numArray3[6] = (byte) 151;
      numArray3[7] = (byte) 1;
      numArray3[4] = (byte) 88;
      key.Query(true, 353, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 9);
      for (int index = 0; index < 9; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[9];
    byte[] numArray5 = new byte[9];
    numArray5[8] = (byte) 249;
    numArray5[4] = (byte) 20;
    numArray5[2] = (byte) 24;
    numArray5[3] = (byte) 209;
    numArray5[5] = (byte) 122;
    numArray5[1] = (byte) 60;
    numArray5[0] = (byte) 227;
    numArray5[7] = (byte) 139;
    numArray5[6] = (byte) 122;
    byte[] numArray6 = new byte[9];
    numArray6[1] = (byte) 103;
    numArray6[4] = (byte) 160 /*0xA0*/;
    numArray6[2] = (byte) 57;
    numArray6[5] = (byte) 88;
    numArray6[3] = (byte) 229;
    numArray6[0] = (byte) 101;
    numArray6[6] = (byte) 78;
    numArray6[7] = (byte) 233;
    numArray6[8] = (byte) 63 /*0x3F*/;
    key.Query(true, 353, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 9);
    for (int index = 0; index < 9; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[27];
    byte[] response = new byte[27];
    Array.Copy((Array) sc_18398.sspq, 0, (Array) numArray7, 0, 27);
    key.Query(true, 353, numArray7, response);
    Array.Copy((Array) sc_18398.sspr, 0, (Array) numArray7, 0, 27);
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
