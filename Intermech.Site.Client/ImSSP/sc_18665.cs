// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_18665
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_18665
{
  internal static string ssp_webportal_18666()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 5)
    {
      byte[] numArray1 = new byte[14];
      byte[] numArray2 = new byte[14]
      {
        (byte) 98,
        (byte) 9,
        (byte) 238,
        (byte) 117,
        (byte) 3,
        (byte) 95,
        (byte) 239,
        (byte) 6,
        (byte) 89,
        (byte) 58,
        (byte) 203,
        (byte) 180,
        (byte) 246,
        (byte) 172
      };
      byte[] numArray3 = new byte[14]
      {
        (byte) 235,
        (byte) 80 /*0x50*/,
        (byte) 128 /*0x80*/,
        (byte) 74,
        (byte) 209,
        (byte) 118,
        (byte) 20,
        (byte) 75,
        (byte) 136,
        (byte) 1,
        (byte) 242,
        (byte) 0,
        (byte) 150,
        (byte) 250
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
      (byte) 163,
      (byte) 197,
      (byte) 185,
      (byte) 143,
      (byte) 97,
      (byte) 23,
      (byte) 184,
      (byte) 125,
      (byte) 157,
      (byte) 178,
      (byte) 104,
      (byte) 75,
      (byte) 246,
      (byte) 131
    };
    byte[] numArray6 = new byte[14]
    {
      (byte) 95,
      (byte) 72,
      (byte) 155,
      (byte) 148,
      (byte) 16 /*0x10*/,
      (byte) 68,
      (byte) 142,
      (byte) 173,
      (byte) 228,
      (byte) 23,
      (byte) 92,
      (byte) 56,
      (byte) 136,
      (byte) 238
    };
    key.Query(true, 363, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 14);
    for (int index = 0; index < 14; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }

  internal static string ssp_webportal_18667()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 3)
    {
      byte[] numArray1 = new byte[14];
      byte[] numArray2 = new byte[14];
      numArray2[3] = (byte) 225;
      numArray2[9] = (byte) 141;
      numArray2[2] = (byte) 56;
      numArray2[10] = (byte) 144 /*0x90*/;
      numArray2[4] = (byte) 219;
      numArray2[13] = (byte) 16 /*0x10*/;
      numArray2[8] = (byte) 230;
      numArray2[1] = (byte) 56;
      numArray2[5] = (byte) 211;
      numArray2[7] = (byte) 39;
      numArray2[6] = (byte) 20;
      numArray2[11] = (byte) 43;
      numArray2[12] = (byte) 194;
      numArray2[0] = (byte) 0;
      byte[] numArray3 = new byte[14]
      {
        (byte) 20,
        (byte) 67,
        (byte) 109,
        (byte) 137,
        (byte) 117,
        (byte) 10,
        (byte) 2,
        (byte) 119,
        (byte) 253,
        (byte) 239,
        (byte) 211,
        (byte) 129,
        (byte) 138,
        (byte) 46
      };
      key.Query(true, 363, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 14);
      for (int index = 0; index < 14; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[14];
    byte[] numArray5 = new byte[14];
    numArray5[12] = (byte) 42;
    numArray5[1] = (byte) 149;
    numArray5[11] = (byte) 48 /*0x30*/;
    numArray5[3] = (byte) 145;
    numArray5[4] = (byte) 123;
    numArray5[0] = (byte) 171;
    numArray5[2] = (byte) 90;
    numArray5[10] = (byte) 57;
    numArray5[5] = (byte) 114;
    numArray5[9] = (byte) 122;
    numArray5[7] = (byte) 234;
    numArray5[13] = (byte) 236;
    numArray5[8] = (byte) 229;
    numArray5[6] = (byte) 193;
    byte[] numArray6 = new byte[14]
    {
      (byte) 189,
      (byte) 231,
      (byte) 96 /*0x60*/,
      (byte) 152,
      (byte) 154,
      (byte) 132,
      (byte) 224 /*0xE0*/,
      (byte) 237,
      (byte) 160 /*0xA0*/,
      (byte) 183,
      (byte) 79,
      (byte) 172,
      (byte) 149,
      (byte) 229
    };
    key.Query(true, 363, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 14);
    for (int index = 0; index < 14; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
