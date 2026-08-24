// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18643
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18643
{
  private static byte[] sspq = new byte[24]
  {
    (byte) 231,
    (byte) 206,
    (byte) 242,
    (byte) 46,
    (byte) 122,
    (byte) 68,
    (byte) 65,
    (byte) 0,
    (byte) 56,
    (byte) 194,
    (byte) 128 /*0x80*/,
    (byte) 251,
    (byte) 169,
    (byte) 221,
    (byte) 3,
    (byte) 238,
    (byte) 110,
    (byte) 39,
    (byte) 96 /*0x60*/,
    (byte) 156,
    (byte) 218,
    (byte) 89,
    (byte) 135,
    (byte) 0
  };
  private static byte[] sspr = new byte[24]
  {
    (byte) 182,
    (byte) 17,
    (byte) 186,
    (byte) 19,
    (byte) 98,
    (byte) 251,
    (byte) 228,
    (byte) 167,
    (byte) 146,
    (byte) 216,
    (byte) 169,
    (byte) 140,
    (byte) 199,
    (byte) 10,
    (byte) 187,
    (byte) 248,
    (byte) 201,
    (byte) 82,
    (byte) 136,
    (byte) 134,
    (byte) 31 /*0x1F*/,
    (byte) 247,
    (byte) 229,
    (byte) 146
  };

  internal static string ssp_webportal_18644()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 7)
    {
      byte[] numArray1 = new byte[14];
      byte[] numArray2 = new byte[14]
      {
        (byte) 167,
        (byte) 192 /*0xC0*/,
        (byte) 75,
        (byte) 10,
        (byte) 27,
        (byte) 41,
        (byte) 40,
        (byte) 51,
        (byte) 38,
        (byte) 253,
        (byte) 173,
        (byte) 104,
        (byte) 233,
        (byte) 133
      };
      byte[] numArray3 = new byte[14];
      numArray3[11] = (byte) 151;
      numArray3[1] = (byte) 175;
      numArray3[2] = (byte) 5;
      numArray3[10] = (byte) 151;
      numArray3[9] = (byte) 36;
      numArray3[13] = (byte) 38;
      numArray3[6] = (byte) 254;
      numArray3[7] = (byte) 246;
      numArray3[12] = (byte) 7;
      numArray3[5] = (byte) 210;
      numArray3[4] = (byte) 251;
      numArray3[0] = (byte) 24;
      numArray3[3] = (byte) 201;
      numArray3[8] = (byte) 217;
      key.Query(true, 363, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 14);
      for (int index = 0; index < 14; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[14];
    byte[] numArray5 = new byte[14]
    {
      (byte) 187,
      (byte) 58,
      (byte) 110,
      (byte) 64 /*0x40*/,
      (byte) 93,
      (byte) 89,
      (byte) 118,
      (byte) 7,
      (byte) 239,
      (byte) 75,
      (byte) 213,
      (byte) 107,
      (byte) 151,
      (byte) 21
    };
    byte[] numArray6 = new byte[14];
    numArray6[3] = (byte) 200;
    numArray6[7] = (byte) 101;
    numArray6[11] = (byte) 53;
    numArray6[9] = (byte) 229;
    numArray6[6] = (byte) 123;
    numArray6[5] = (byte) 131;
    numArray6[0] = (byte) 87;
    numArray6[4] = (byte) 127 /*0x7F*/;
    numArray6[8] = (byte) 116;
    numArray6[10] = (byte) 155;
    numArray6[1] = (byte) 62;
    numArray6[2] = (byte) 160 /*0xA0*/;
    numArray6[12] = (byte) 172;
    numArray6[13] = (byte) 55;
    key.Query(true, 363, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 14);
    for (int index = 0; index < 14; ++index)
      numArray4[index] ^= numArray6[index];
    byte[] numArray7 = new byte[24];
    byte[] response = new byte[24];
    Array.Copy((Array) sc_18643.sspq, 0, (Array) numArray7, 0, 24);
    key.Query(true, 363, numArray7, response);
    Array.Copy((Array) sc_18643.sspr, 0, (Array) numArray7, 0, 24);
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
