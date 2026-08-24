// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18676
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18676
{
  internal static string ssp_webportal_18677()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 4)
    {
      byte[] numArray1 = new byte[14];
      byte[] numArray2 = new byte[14];
      numArray2[4] = (byte) 69;
      numArray2[1] = (byte) 134;
      numArray2[0] = (byte) 53;
      numArray2[3] = (byte) 28;
      numArray2[2] = (byte) 111;
      numArray2[10] = (byte) 95;
      numArray2[12] = (byte) 175;
      numArray2[11] = (byte) 251;
      numArray2[8] = (byte) 72;
      numArray2[9] = (byte) 34;
      numArray2[7] = (byte) 91;
      numArray2[6] = (byte) 87;
      numArray2[5] = (byte) 117;
      numArray2[13] = (byte) 62;
      byte[] numArray3 = new byte[14];
      numArray3[11] = (byte) 217;
      numArray3[10] = (byte) 170;
      numArray3[2] = (byte) 58;
      numArray3[3] = (byte) 155;
      numArray3[1] = (byte) 102;
      numArray3[5] = (byte) 152;
      numArray3[6] = (byte) 214;
      numArray3[4] = (byte) 230;
      numArray3[0] = (byte) 82;
      numArray3[9] = (byte) 191;
      numArray3[7] = (byte) 43;
      numArray3[8] = (byte) 123;
      numArray3[12] = (byte) 138;
      numArray3[13] = (byte) 224 /*0xE0*/;
      key.Query(true, 363, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 14);
      for (int index = 0; index < 14; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[14];
    byte[] numArray5 = new byte[14]
    {
      (byte) 221,
      (byte) 205,
      (byte) 100,
      (byte) 99,
      (byte) 58,
      (byte) 131,
      (byte) 185,
      (byte) 54,
      (byte) 141,
      (byte) 32 /*0x20*/,
      (byte) 50,
      (byte) 109,
      (byte) 57,
      (byte) 197
    };
    byte[] numArray6 = new byte[14]
    {
      (byte) 52,
      (byte) 227,
      (byte) 163,
      (byte) 18,
      (byte) 15,
      (byte) 16 /*0x10*/,
      (byte) 154,
      (byte) 171,
      (byte) 183,
      (byte) 145,
      (byte) 228,
      (byte) 106,
      (byte) 112 /*0x70*/,
      (byte) 14
    };
    key.Query(true, 363, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 14);
    for (int index = 0; index < 14; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
