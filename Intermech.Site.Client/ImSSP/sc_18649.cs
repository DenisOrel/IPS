// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18649
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18649
{
  internal static string ssp_webportal_18650()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 5)
    {
      byte[] numArray1 = new byte[14];
      byte[] numArray2 = new byte[14]
      {
        (byte) 170,
        (byte) 226,
        (byte) 56,
        (byte) 17,
        (byte) 194,
        (byte) 190,
        (byte) 32 /*0x20*/,
        (byte) 11,
        (byte) 163,
        (byte) 132,
        (byte) 201,
        (byte) 34,
        (byte) 186,
        (byte) 48 /*0x30*/
      };
      byte[] numArray3 = new byte[14]
      {
        (byte) 82,
        (byte) 222,
        (byte) 100,
        (byte) 86,
        (byte) 76,
        (byte) 109,
        (byte) 155,
        (byte) 23,
        (byte) 183,
        (byte) 48 /*0x30*/,
        (byte) 81,
        (byte) 233,
        (byte) 155,
        (byte) 209
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
      (byte) 174,
      (byte) 228,
      (byte) 58,
      (byte) 195,
      (byte) 26,
      (byte) 129,
      (byte) 33,
      (byte) 89,
      (byte) 69,
      (byte) 33,
      (byte) 110,
      (byte) 163,
      (byte) 88,
      (byte) 141
    };
    byte[] numArray6 = new byte[14]
    {
      (byte) 110,
      (byte) 58,
      (byte) 17,
      (byte) 93,
      (byte) 198,
      (byte) 41,
      (byte) 29,
      (byte) 37,
      (byte) 173,
      (byte) 175,
      (byte) 67,
      (byte) 203,
      (byte) 15,
      (byte) 15
    };
    key.Query(true, 363, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 14);
    for (int index = 0; index < 14; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
