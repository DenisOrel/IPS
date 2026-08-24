// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18669
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18669
{
  internal static string ssp_webportal_18670()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 10)
    {
      byte[] numArray1 = new byte[14];
      byte[] numArray2 = new byte[14]
      {
        (byte) 47,
        (byte) 242,
        (byte) 228,
        (byte) 150,
        (byte) 104,
        (byte) 126,
        (byte) 41,
        (byte) 112 /*0x70*/,
        (byte) 225,
        (byte) 66,
        (byte) 18,
        (byte) 196,
        (byte) 199,
        (byte) 193
      };
      byte[] numArray3 = new byte[14]
      {
        (byte) 46,
        (byte) 92,
        (byte) 63 /*0x3F*/,
        (byte) 168,
        (byte) 18,
        (byte) 30,
        (byte) 179,
        (byte) 154,
        (byte) 128 /*0x80*/,
        (byte) 141,
        (byte) 42,
        (byte) 65,
        (byte) 69,
        (byte) 32 /*0x20*/
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
      (byte) 189,
      (byte) 47,
      (byte) 20,
      (byte) 79,
      (byte) 215,
      (byte) 151,
      (byte) 155,
      (byte) 115,
      (byte) 8,
      (byte) 131,
      (byte) 121,
      (byte) 49,
      (byte) 70,
      (byte) 188
    };
    byte[] numArray6 = new byte[14];
    numArray6[9] = (byte) 152;
    numArray6[8] = (byte) 17;
    numArray6[4] = (byte) 155;
    numArray6[5] = (byte) 175;
    numArray6[2] = (byte) 90;
    numArray6[10] = (byte) 114;
    numArray6[6] = (byte) 215;
    numArray6[7] = (byte) 155;
    numArray6[1] = (byte) 40;
    numArray6[0] = (byte) 208 /*0xD0*/;
    numArray6[3] = (byte) 151;
    numArray6[11] = (byte) 217;
    numArray6[12] = (byte) 50;
    numArray6[13] = (byte) 43;
    key.Query(true, 363, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 14);
    for (int index = 0; index < 14; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
