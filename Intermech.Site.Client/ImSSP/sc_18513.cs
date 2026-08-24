// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18513
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18513
{
  internal static string ssp_webportal_18514()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 2)
    {
      byte[] numArray1 = new byte[14];
      byte[] numArray2 = new byte[14];
      numArray2[1] = (byte) 150;
      numArray2[4] = (byte) 134;
      numArray2[2] = (byte) 233;
      numArray2[5] = (byte) 17;
      numArray2[7] = (byte) 139;
      numArray2[12] = (byte) 137;
      numArray2[6] = (byte) 188;
      numArray2[10] = (byte) 68;
      numArray2[0] = (byte) 49;
      numArray2[9] = (byte) 162;
      numArray2[8] = (byte) 49;
      numArray2[11] = (byte) 222;
      numArray2[3] = (byte) 76;
      numArray2[13] = (byte) 240 /*0xF0*/;
      byte[] numArray3 = new byte[14];
      numArray3[13] = (byte) 194;
      numArray3[1] = (byte) 247;
      numArray3[2] = (byte) 132;
      numArray3[4] = (byte) 185;
      numArray3[9] = (byte) 168;
      numArray3[3] = (byte) 176 /*0xB0*/;
      numArray3[6] = (byte) 140;
      numArray3[5] = (byte) 218;
      numArray3[8] = (byte) 51;
      numArray3[10] = (byte) 140;
      numArray3[0] = (byte) 221;
      numArray3[11] = (byte) 201;
      numArray3[12] = (byte) 171;
      numArray3[7] = (byte) 204;
      key.Query(true, 363, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 14);
      for (int index = 0; index < 14; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[14];
    byte[] numArray5 = new byte[14]
    {
      (byte) 22,
      (byte) 199,
      (byte) 119,
      (byte) 182,
      (byte) 149,
      (byte) 155,
      (byte) 47,
      (byte) 168,
      (byte) 25,
      (byte) 215,
      (byte) 204,
      (byte) 179,
      (byte) 98,
      (byte) 145
    };
    byte[] numArray6 = new byte[14]
    {
      (byte) 204,
      (byte) 70,
      (byte) 204,
      (byte) 170,
      (byte) 137,
      (byte) 206,
      (byte) 127 /*0x7F*/,
      (byte) 44,
      (byte) 130,
      (byte) 209,
      (byte) 129,
      (byte) 247,
      (byte) 172,
      (byte) 86
    };
    key.Query(true, 363, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 14);
    for (int index = 0; index < 14; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
