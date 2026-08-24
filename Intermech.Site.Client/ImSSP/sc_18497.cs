// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18497
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18497
{
  internal static string ssp_webportal_18498()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 1)
    {
      byte[] numArray1 = new byte[14];
      byte[] numArray2 = new byte[14];
      numArray2[10] = (byte) 98;
      numArray2[8] = (byte) 14;
      numArray2[0] = (byte) 100;
      numArray2[3] = (byte) 242;
      numArray2[4] = (byte) 37;
      numArray2[5] = (byte) 214;
      numArray2[9] = (byte) 235;
      numArray2[1] = (byte) 163;
      numArray2[2] = (byte) 145;
      numArray2[6] = (byte) 96 /*0x60*/;
      numArray2[13] = (byte) 78;
      numArray2[11] = (byte) 109;
      numArray2[12] = (byte) 66;
      numArray2[7] = (byte) 87;
      byte[] numArray3 = new byte[14]
      {
        (byte) 53,
        (byte) 151,
        (byte) 155,
        (byte) 194,
        (byte) 14,
        (byte) 101,
        (byte) 126,
        (byte) 108,
        (byte) 49,
        (byte) 36,
        (byte) 73,
        (byte) 112 /*0x70*/,
        (byte) 234,
        (byte) 82
      };
      key.Query(true, 363, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 14);
      for (int index = 0; index < 14; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[14];
    byte[] numArray5 = new byte[14]
    {
      (byte) 57,
      (byte) 152,
      (byte) 92,
      (byte) 237,
      (byte) 122,
      (byte) 13,
      (byte) 29,
      (byte) 17,
      (byte) 201,
      (byte) 202,
      (byte) 253,
      (byte) 231,
      (byte) 38,
      (byte) 178
    };
    byte[] numArray6 = new byte[14]
    {
      (byte) 118,
      (byte) 87,
      (byte) 69,
      (byte) 84,
      (byte) 110,
      (byte) 236,
      (byte) 97,
      (byte) 214,
      (byte) 81,
      (byte) 16 /*0x10*/,
      (byte) 120,
      (byte) 82,
      (byte) 13,
      (byte) 168
    };
    key.Query(true, 363, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 14);
    for (int index = 0; index < 14; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_imclient_18499()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 7)
    {
      byte[] numArray1 = new byte[12];
      byte[] numArray2 = new byte[12];
      numArray2[0] = (byte) 27;
      numArray2[11] = (byte) 212;
      numArray2[5] = (byte) 96 /*0x60*/;
      numArray2[8] = (byte) 136;
      numArray2[1] = (byte) 72;
      numArray2[9] = (byte) 78;
      numArray2[3] = (byte) 253;
      numArray2[7] = (byte) 4;
      numArray2[2] = (byte) 9;
      numArray2[4] = (byte) 184;
      numArray2[10] = (byte) 121;
      numArray2[6] = (byte) 29;
      byte[] numArray3 = new byte[12]
      {
        (byte) 107,
        (byte) 192 /*0xC0*/,
        (byte) 65,
        (byte) 235,
        (byte) 86,
        (byte) 186,
        (byte) 130,
        (byte) 123,
        (byte) 82,
        (byte) 240 /*0xF0*/,
        (byte) 240 /*0xF0*/,
        (byte) 43
      };
      key.Query(true, 348, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 12);
      for (int index = 0; index < 12; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[12];
    byte[] numArray5 = new byte[12];
    numArray5[9] = (byte) 16 /*0x10*/;
    numArray5[0] = (byte) 96 /*0x60*/;
    numArray5[2] = (byte) 86;
    numArray5[3] = (byte) 171;
    numArray5[4] = (byte) 52;
    numArray5[5] = (byte) 245;
    numArray5[1] = (byte) 120;
    numArray5[11] = (byte) 223;
    numArray5[8] = (byte) 200;
    numArray5[6] = (byte) 240 /*0xF0*/;
    numArray5[7] = (byte) 165;
    numArray5[10] = (byte) 59;
    byte[] numArray6 = new byte[12]
    {
      (byte) 96 /*0x60*/,
      (byte) 98,
      (byte) 34,
      (byte) 231,
      (byte) 33,
      (byte) 91,
      (byte) 43,
      (byte) 158,
      (byte) 64 /*0x40*/,
      (byte) 222,
      (byte) 101,
      (byte) 157
    };
    key.Query(true, 348, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 12);
    for (int index = 0; index < 12; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
