// Decompiled with JetBrains decompiler
// Type: ImSSP.sc_15086
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Protection;
using System;
using System.Text;

#nullable disable
namespace ImSSP;

internal static class sc_15086
{
  internal static string ssp_office_15087()
  {
    IProtectionKey key = ProtectionService.Key;
    if (DateTime.Now.Month == 8)
    {
      byte[] numArray1 = new byte[16 /*0x10*/];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 238,
        (byte) 142,
        (byte) 254,
        (byte) 103,
        (byte) 201,
        (byte) 56,
        (byte) 206,
        (byte) 43,
        (byte) 22,
        (byte) 77,
        (byte) 145,
        (byte) 63 /*0x3F*/,
        (byte) 16 /*0x10*/,
        (byte) 192 /*0xC0*/,
        (byte) 175,
        (byte) 219
      };
      byte[] numArray3 = new byte[16 /*0x10*/]
      {
        (byte) 85,
        (byte) 214,
        (byte) 155,
        (byte) 166,
        (byte) 111,
        (byte) 159,
        (byte) 99,
        (byte) 193,
        (byte) 5,
        (byte) 61,
        (byte) 113,
        (byte) 24,
        (byte) 130,
        (byte) 100,
        (byte) 57,
        (byte) 95
      };
      key.Query(true, 349, numArray2, numArray2);
      Array.Copy((Array) numArray2, 0, (Array) numArray1, 0, 16 /*0x10*/);
      for (int index = 0; index < 16 /*0x10*/; ++index)
        numArray1[index] ^= numArray3[index];
      return Encoding.UTF8.GetString(numArray1);
    }
    byte[] numArray4 = new byte[16 /*0x10*/];
    byte[] numArray5 = new byte[16 /*0x10*/];
    numArray5[12] = (byte) 200;
    numArray5[13] = (byte) 48 /*0x30*/;
    numArray5[2] = (byte) 151;
    numArray5[9] = (byte) 8;
    numArray5[4] = (byte) 150;
    numArray5[5] = (byte) 228;
    numArray5[6] = (byte) 251;
    numArray5[1] = (byte) 194;
    numArray5[14] = (byte) 186;
    numArray5[7] = (byte) 212;
    numArray5[10] = (byte) 251;
    numArray5[11] = (byte) 235;
    numArray5[8] = (byte) 218;
    numArray5[3] = (byte) 210;
    numArray5[0] = (byte) 131;
    numArray5[15] = (byte) 188;
    byte[] numArray6 = new byte[16 /*0x10*/]
    {
      (byte) 92,
      (byte) 159,
      (byte) 207,
      (byte) 117,
      (byte) 129,
      (byte) 93,
      (byte) 245,
      (byte) 40,
      (byte) 193,
      (byte) 230,
      (byte) 150,
      (byte) 81,
      (byte) 135,
      (byte) 71,
      (byte) 18,
      (byte) 100
    };
    key.Query(true, 349, numArray5, numArray5);
    Array.Copy((Array) numArray5, 0, (Array) numArray4, 0, 16 /*0x10*/);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      numArray4[index] ^= numArray6[index];
    return Encoding.UTF8.GetString(numArray4);
  }
}
