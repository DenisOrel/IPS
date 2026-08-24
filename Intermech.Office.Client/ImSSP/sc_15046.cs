// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_15046
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_15046
{
  internal static string ssp_office_15047()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 12)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 32 /*0x20*/,
        (byte) 119,
        (byte) 242,
        (byte) 162,
        (byte) 84,
        (byte) 98,
        (byte) 111,
        (byte) 195,
        (byte) 167,
        (byte) 146,
        (byte) 24,
        (byte) 182,
        (byte) 124,
        (byte) 197,
        (byte) 110,
        (byte) 81
      };
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 115,
        (byte) 209,
        (byte) 50,
        (byte) 130,
        (byte) 136,
        (byte) 46,
        (byte) 194,
        (byte) 156,
        (byte) 147,
        byte.MaxValue,
        (byte) 197,
        (byte) 71,
        (byte) 111,
        (byte) 71,
        (byte) 104,
        (byte) 15
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/]
    {
      (byte) 207,
      (byte) 173,
      (byte) 112 /*0x70*/,
      (byte) 54,
      (byte) 105,
      (byte) 73,
      (byte) 196,
      (byte) 14,
      (byte) 152,
      (byte) 187,
      (byte) 133,
      (byte) 92,
      (byte) 16 /*0x10*/,
      (byte) 163,
      (byte) 169,
      (byte) 71
    };
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 219,
      (byte) 107,
      (byte) 6,
      (byte) 103,
      (byte) 48 /*0x30*/,
      (byte) 34,
      (byte) 82,
      (byte) 85,
      (byte) 76,
      (byte) 9,
      (byte) 188,
      (byte) 121,
      (byte) 236,
      (byte) 156,
      (byte) 235,
      (byte) 247
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
